using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace Spotify
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Spotify spotify = new Spotify();
            bool exit = false;
            var isSucceed = true;
            do
            {
                Console.WriteLine("---Spotify---");
                Console.WriteLine("Press key:");
                Console.WriteLine("A         Add Playlist");
                Console.WriteLine("S         Add Song to Playlist");
                Console.WriteLine("V         View Songs in Playlist");
                Console.WriteLine("ESC       Exit");
                var Key = Console.ReadKey(true).Key;
                switch (Key)
                {
                    case ConsoleKey.A:
                        Console.WriteLine("Enter playlist name");
                        RepeatPlaylistname: string playlistname = Console.ReadLine();
                        if(string.IsNullOrWhiteSpace(playlistname))
                        {                            
                            Console.WriteLine("!Enter name!");
                            goto RepeatPlaylistname;
                        }
                        else 
                        {
                            spotify.AddPlaylist(new Playlist(playlistname));
                            Console.WriteLine($"{playlistname} created");
                        }
                        break;
                    case ConsoleKey.S:
                        foreach(var playlist in spotify.Playlists)
                        {
                            playlist.GetPlaylistDetails();
                        }
                        RepeatPlaylistId: Console.WriteLine("Enter id of playlist");
                        string input = Console.ReadLine();
                        isSucceed = int.TryParse(input, out int result);
                        if (isSucceed)
                        {
                            var Exist = spotify.Playlists.FirstOrDefault(x => x.Id == result);
                            if (Exist != null)
                            {
                                RepeatSongName: Console.WriteLine("Enter something to add song to your playlist");
                                string query = Console.ReadLine().Trim().ToLower();
                                if (!string.IsNullOrWhiteSpace(query))
                                {
                                    List<Song> foundSongs = spotify.Songs.Where(song => song.Artistname.ToLower().Contains(query) || song.Songname.ToLower().Contains(query) || song.Genre.ToLower().Contains(query)).ToList();

                                    if (foundSongs.Any())
                                    {
                                        Console.WriteLine("Found songs:");
                                        for (int i = 0; i < foundSongs.Count; i++)
                                        {
                                            Console.WriteLine($"{i + 1}. {foundSongs[i].GetSongDetails()}");
                                        }

                                        RepeatNumber: Console.WriteLine("Enter the number of the song you want to pick (or type 'exit' to quit):");
                                        input = Console.ReadLine().Trim();

                                        if (input.ToLower() == "exit")
                                        {
                                            exit = true;
                                        }
                                        else if (int.TryParse(input, out int selectedIndex) && selectedIndex >= 1 && selectedIndex <= foundSongs.Count)
                                        {
                                            Exist.Songs.Add(foundSongs[selectedIndex - 1]);
                                            Console.WriteLine($"You picked: {foundSongs[selectedIndex - 1].GetSongDetails()}");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid input. Please try again.");
                                            goto RepeatNumber;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("No songs found.");
                                        goto RepeatSongName;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(ErrorMessages.FormatError);
                                    goto RepeatSongName;
                                }
                            }
                            else
                            {
                                Console.WriteLine(ErrorMessages.PlaylistNotFound);
                                goto RepeatPlaylistId;
                            }
                        }
                        else
                        {
                            Console.WriteLine(ErrorMessages.FormatError);
                            goto RepeatPlaylistId;
                        }
                        break;
                    case ConsoleKey.V:
                        foreach (var playlist in spotify.Playlists)
                        {
                            playlist.GetPlaylistDetails();
                        }
                        Console.WriteLine("Enter id of playlist");
                        RepeatPLaylistId: input = Console.ReadLine();
                        isSucceed = int.TryParse(input, out result);
                        if (isSucceed)
                        {
                            var Exist = spotify.Playlists.FirstOrDefault(x => x.Id == result);
                            if (Exist != null)
                            {
                                Exist.GetSongsinPlaylist();
                            }
                            else
                            {
                                Console.WriteLine(ErrorMessages.PlaylistNotFound);
                                goto RepeatPLaylistId;
                            }
                        }
                        else
                        {
                            Console.WriteLine(ErrorMessages.FormatError);
                            goto RepeatPlaylistId;
                        }
                        break;
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option try again");
                        break;
                }
            }while (!exit);
        }
    }
}

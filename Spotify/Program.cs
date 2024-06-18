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
            Initialize:
            do
            {
                Console.WriteLine("---Spotify---");
                Console.WriteLine("Press key:");
                Console.WriteLine("A           Add Playlist");
                Console.WriteLine("S           Add Song to Playlist");
                Console.WriteLine("V           Listen song in Playlist");
                Console.WriteLine("R           Rename playlist");
                Console.WriteLine("Del         Delete playlist");
                Console.WriteLine("Backspace   Delete song in playlist");
                Console.WriteLine("L           Listen to song");
                Console.WriteLine("ESC         Exit");
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
                                RepeatSongName1: Console.WriteLine("Enter something to add song to your playlist");
                                string Query = Console.ReadLine().Trim().ToLower();
                                if (!string.IsNullOrWhiteSpace(Query))
                                {
                                    List<Song> foundSongs = spotify.Songs.Where(song => song.Artistname.ToLower().Contains(Query) || song.Songname.ToLower().Contains(Query) || song.Genre.ToLower().Contains(Query)).ToList();

                                    if (foundSongs.Any())
                                    {
                                        RepeatFoundSongs: Console.WriteLine("Found songs:");
                                        for(int i = 0; i < foundSongs.Count; i++)
                                        {
                                            Console.WriteLine(foundSongs[i].GetSongDetails());
                                        }

                                        RepeatNumber: Console.WriteLine("Enter the number of the song you want to pick (or type 'exit' to quit):");
                                        input = Console.ReadLine();

                                        if (input.ToLower() == "exit")
                                        {
                                            goto Initialize;
                                        }
                                        else if (int.TryParse(input, out int selectedIndex))
                                        {

                                            var existsong = foundSongs.FirstOrDefault(foundSongs => foundSongs.Id == selectedIndex);
                                            if (existsong is not null)
                                            {
                                                Exist.Songs.Add(existsong);
                                                Console.WriteLine($"You picked: {existsong.GetSongDetails()}");
                                            }
                                            else
                                            {
                                                Console.WriteLine(ErrorMessages.SongNotFound);
                                                goto RepeatNumber;
                                            }
                                            
                                        }
                                        else
                                        {
                                            Console.WriteLine(ErrorMessages.FormatError);
                                            goto RepeatNumber;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine(ErrorMessages.SongNotFound);
                                        goto RepeatSongName1;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(ErrorMessages.FormatError);
                                    goto RepeatSongName1;
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
                                RepeatSongId: Console.WriteLine("Select id to listen");
                                input = Console.ReadLine();
                                isSucceed = int.TryParse(input, out result);
                                if (isSucceed)
                                {
                                    var Existsong = Exist.Songs.FirstOrDefault(x => x.Id == result);
                                    if(Existsong != null)
                                    {
                                        Console.WriteLine("You are listening...");
                                        Console.WriteLine(Existsong.Artistname);
                                        Console.WriteLine(Existsong.Songname);
                                        Console.WriteLine(Existsong.Genre);
                                        Console.WriteLine("Listened");
                                    }
                                    else
                                    {
                                        Console.WriteLine(ErrorMessages.SongNotFound);
                                        goto RepeatSongId;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(ErrorMessages.FormatError);
                                    goto RepeatSongId;
                                }

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
                    case ConsoleKey.L:
                        RepeatSongName: 
                        Console.WriteLine("Enter something to listen song");
                        string query = Console.ReadLine().Trim().ToLower();
                        if (!string.IsNullOrWhiteSpace(query))
                        {
                            List<Song> foundSongs = spotify.Songs.Where(song => song.Artistname.ToLower().Contains(query) || song.Songname.ToLower().Contains(query) || song.Genre.ToLower().Contains(query)).ToList();

                            if (foundSongs.Any())
                            {
                                Console.WriteLine("Found songs:");
                                for (int i = 0; i < foundSongs.Count; i++)
                                {
                                    Console.WriteLine(foundSongs[i].GetSongDetails());
                                }

                            RepeatNumber: Console.WriteLine("Enter the number of the song you want to pick (or type 'exit' to quit):");
                                input = Console.ReadLine().Trim();
                                if (int.TryParse(input, out int selectedIndex))
                                {
                                    var exist = foundSongs.FirstOrDefault(song => song.Id == selectedIndex);
                                    if(exist is not null)
                                    {

                                    
                                    Console.WriteLine("You listening...");
                                    Console.WriteLine($"You picked: {exist.GetSongDetails()}");
                                    Console.WriteLine("Listened");
                                    RepeatChoice: Console.WriteLine("Do you want to add this song to playlist (Press y/n)");
                                    Key = Console.ReadKey(true).Key;
                                        if (Key == ConsoleKey.Y)
                                        {
                                            Console.WriteLine("Select playlist or create to add");
                                        RepeatPlaylistsId: foreach (var pleylist in spotify.Playlists)
                                            {
                                                pleylist.GetPlaylistDetails();
                                            }
                                            Console.WriteLine("Enter playlist id or type '0' to create new playlist");
                                            string choice = Console.ReadLine();
                                            isSucceed = int.TryParse(choice, out selectedIndex);
                                            if (isSucceed)
                                            {
                                                if (choice == "0")
                                                {
                                                    Console.WriteLine("Enter playlist name");
                                                RepeatPlaylistname2: playlistname = Console.ReadLine();
                                                    if (string.IsNullOrWhiteSpace(playlistname))
                                                    {
                                                        Console.WriteLine("!Enter name!");
                                                        goto RepeatPlaylistname2;
                                                    }
                                                    else
                                                    {
                                                        spotify.Playlists.Add(new Playlist(playlistname));
                                                        var newplaylist = spotify.Playlists.FirstOrDefault(x => x.Playlistname == playlistname);
                                                        newplaylist.Songs.Add(exist);
                                                        Console.WriteLine($"{playlistname} created and {exist.Songname} vas added");
                                                    }
                                                }
                                                else
                                                {
                                                    var existsong = spotify.Playlists.FirstOrDefault(x => x.Id == selectedIndex);
                                                    if (exist is not null)
                                                    {
                                                        existsong.Songs.Add(foundSongs[selectedIndex - 1]);
                                                        Console.WriteLine($"{foundSongs[selectedIndex - 1].GetSongDetails()} add to {existsong.Playlistname}");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine(ErrorMessages.PlaylistNotFound);
                                                        goto RepeatPlaylistsId;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine(ErrorMessages.FormatError);
                                                goto RepeatPlaylistsId;
                                            }
                                        }
                                        else if (Key == ConsoleKey.N)
                                        {
                                            goto Initialize;
                                        }
                                        else
                                        {
                                            Console.WriteLine(ErrorMessages.FormatError);
                                            goto RepeatChoice;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine(ErrorMessages.SongNotFound);
                                        goto RepeatNumber;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(ErrorMessages.FormatError);
                                    goto RepeatNumber;
                                }
                            }
                            else
                            {
                                Console.WriteLine(ErrorMessages.SongNotFound);
                                goto RepeatSongName;
                            }
                        }
                        else
                        {
                            Console.WriteLine(ErrorMessages.FormatError);
                            goto RepeatSongName;
                        }
                        break;
                    case ConsoleKey.R:
                        RepeatPlaylistId1: foreach(var pleylist in spotify.Playlists)
                        {
                            pleylist.GetPlaylistDetails();
                        }
                        Console.WriteLine("enter id of playlist to rename");
                        input = Console.ReadLine();
                        isSucceed = int.TryParse(input, out int id);
                        if (isSucceed)
                        {
                            var exist = spotify.Playlists.FirstOrDefault(x => x.Id == id);
                            if(exist != null)
                            {
                                RepeatNewName: Console.WriteLine("Enter new name");
                                playlistname = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(playlistname))
                                {
                                    exist.Playlistname = playlistname;
                                }
                                else
                                {
                                    Console.WriteLine(ErrorMessages.FormatError);
                                    goto RepeatNewName;
                                }
                            }
                            else
                            {
                                Console.WriteLine(ErrorMessages.PlaylistNotFound);
                                goto RepeatPlaylistId1;
                            }
                        }
                        else
                        {
                            Console.WriteLine(ErrorMessages.FormatError);
                            goto RepeatPlaylistId1;
                        }
                        break;
                    case ConsoleKey.Delete:
                    RepeatPlaylistId2: foreach (var pleylist in spotify.Playlists)
                        {
                            pleylist.GetPlaylistDetails();
                        }
                        Console.WriteLine("enter id of playlist to delete");
                        input = Console.ReadLine();
                        isSucceed = int.TryParse(input, out id);
                        if (isSucceed)
                        {
                            var exist = spotify.Playlists.FirstOrDefault(x => x.Id == id);
                            if (exist != null)
                            {
                                spotify.Playlists.Remove(exist);
                            }
                            else
                            {
                                Console.WriteLine(ErrorMessages.PlaylistNotFound);
                                goto RepeatPlaylistId2;
                            }
                        }
                        else
                        {
                            Console.WriteLine(ErrorMessages.FormatError);
                            goto RepeatPlaylistId2;
                        }
                        break;
                    case ConsoleKey.Backspace:
                    RepeatPlaylistId3: foreach (var pleylist in spotify.Playlists)
                        {
                            pleylist.GetPlaylistDetails();
                        }
                         Console.WriteLine("enter id of playlist to delete");
                        input = Console.ReadLine();
                        isSucceed = int.TryParse(input, out id);
                        if (isSucceed)
                        {
                            var exist = spotify.Playlists.FirstOrDefault(x => x.Id == id);
                            if (exist != null)
                            {
                            RepeatSongId: foreach (var song in exist.Songs)
                                {
                                    song.GetSongDetails();
                                }
                                Console.WriteLine("Enter song id to delete");
                                input = Console.ReadLine();
                                isSucceed = int.TryParse(input, out id);
                                if (isSucceed)
                                {
                                    var existsong = exist.Songs.FirstOrDefault(x => x.Id == id);
                                    if(existsong != null)
                                    {
                                        exist.Songs.Remove(existsong);
                                    }
                                    else
                                    {
                                        Console.WriteLine(ErrorMessages.SongNotFound);
                                        goto RepeatSongId;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(ErrorMessages.FormatError);
                                    goto RepeatSongId;
                                }

                            }
                            else
                            {
                                Console.WriteLine(ErrorMessages.PlaylistNotFound);
                                goto RepeatPlaylistId3;
                            }
                        }
                        else
                        {
                            Console.WriteLine(ErrorMessages.FormatError);
                            goto RepeatPlaylistId3;
                        }
                        break;
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine(ErrorMessages.FormatError);
                        break;
                }
            }while (!exit);
        }
    }
}

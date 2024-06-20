using Spotify;
using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace Spotify
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Spotify spotify = new Spotify();
            string input = null;
            int id = 0;
            int result = 0;
            bool exit = false;
            var isSucceed = true;
        Intialize1:
            do
            {
                Console.WriteLine("---Sign in---");
                Console.WriteLine("U    Sign up");
                Console.WriteLine("I    Sign in");
                Console.WriteLine("Q    Quit");

                var Key = Console.ReadKey(true).Key;
                switch (Key)
                {
                    case ConsoleKey.U:
                    RepeatSurname: Console.WriteLine("Enter your surname");
                        string surname = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(surname) && !surname.Contains(" "))
                        {
                        RepeatName: Console.WriteLine("Enter your name");
                            string name = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(name) && !name.Contains(" "))
                            {
                            RepeatEmail: Console.WriteLine("Enter email");
                                string email = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(email) && email.Contains("@") && !email.Contains(" "))
                                {
                                    if (spotify.Account.FirstOrDefault(x => x.Email == email) == null)
                                    {
                                    RepeatPassword: Console.WriteLine("Enter password");
                                        string password = Console.ReadLine();
                                        if (!string.IsNullOrWhiteSpace(password) && !password.Contains(" "))
                                        {
                                        RepeatRepeatPassword: Console.WriteLine("Repeat password");
                                            string repeatedpassword = Console.ReadLine();
                                            if (repeatedpassword == password)
                                            {
                                                Accounts account = new Accounts(name, surname, email, password);
                                                spotify.Account.Add(account);

                                                Console.WriteLine("your account created");
                                                goto Intialize1;
                                            }
                                            else
                                            {
                                                Console.WriteLine(ErrorMessages.PasswordError);
                                                goto RepeatRepeatPassword;
                                            }   
                                        }
                                        else
                                        {
                                            Console.WriteLine(ErrorMessages.FormatError);
                                            goto RepeatPassword;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("This email is used");
                                        goto RepeatEmail;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(ErrorMessages.FormatError);
                                    goto RepeatEmail;
                                }
                            }
                            else
                            {
                                Console.WriteLine(ErrorMessages.FormatError);
                                goto RepeatName;
                            }
                        }
                        else
                        {
                            Console.WriteLine(ErrorMessages.FormatError);
                            goto RepeatSurname;
                        }

                    case ConsoleKey.I:
                        SignIn(spotify);
                        break;
                    case ConsoleKey.Q:
                        goto exit;


                    default:
                        Console.WriteLine(ErrorMessages.FormatError);
                        goto Intialize1;

                }
            Initialize2:
                if (spotify.CurrentUser is not null)
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
                    Console.WriteLine("ESC           Sign out");
                    Console.WriteLine("Q        Exit");
                    Key = Console.ReadKey(true).Key;
                    switch (Key)
                    {
                        case ConsoleKey.A:
                            RepeatPlaylistname: Console.WriteLine("Enter playlist name");
                            string playlistname = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(playlistname) && playlistname.Contains(" "))
                            {
                                Console.WriteLine("!Enter name!");
                                goto RepeatPlaylistname;
                            }
                            else
                            {
                                if(spotify.CurrentUser.Playlists.FirstOrDefault(x=>x.Playlistname == playlistname) is null)
                                {
                                    spotify.CurrentUser.AddPlaylist(new Playlist(playlistname));
                                    Console.WriteLine($"{playlistname} created");
                                    goto Initialize2;
                                }
                                else
                                {
                                    Console.WriteLine(ErrorMessages.ExistPlaylist);
                                    goto RepeatPlaylistname;
                                }
                            }

                        case ConsoleKey.S:
                            if (spotify.CurrentUser.Playlists.Count != 0)
                            {
                                foreach (var playlist in spotify.CurrentUser.Playlists)
                                {
                                    playlist.GetPlaylistDetails();
                                }
                                RepeatPlaylistId: Console.WriteLine("Enter id of playlist");
                                input = Console.ReadLine();
                                isSucceed = int.TryParse(input, out result);
                                if (isSucceed)
                                {
                                    var Exist = spotify.CurrentUser.Playlists.FirstOrDefault(x => x.Id == result);
                                    if (Exist != null)
                                    {
                                    RepeatSongName1: Console.WriteLine("Enter something to add song to your playlist");
                                        string Query = Console.ReadLine().Trim().ToLower();
                                        if (!string.IsNullOrWhiteSpace(Query))
                                        {
                                            List<Song> foundSongs = spotify.Songs.Where(song => song.Artistname.ToLower().Contains(Query) || song.Songname.ToLower().Contains(Query) || song.Genre.ToLower().Contains(Query)).ToList();

                                            if (foundSongs.Any())
                                            {
                                                Console.WriteLine("Found songs:");
                                                for (int i = 0; i < foundSongs.Count; i++)
                                                {
                                                    Console.WriteLine(foundSongs[i].GetSongDetails());
                                                }

                                            RepeatNumber: Console.WriteLine("Enter the number of the song you want to pick (or type 'exit' to quit):");
                                                input = Console.ReadLine();

                                                if (input.ToLower() == "exit")
                                                {
                                                    goto Initialize2;
                                                }
                                                else if (int.TryParse(input, out int selectedIndex))
                                                {

                                                    var existsong = foundSongs.FirstOrDefault(foundSongs => foundSongs.Id == selectedIndex);
                                                    if (existsong is not null)
                                                    {
                                                        Exist.Songs.Add(existsong);
                                                        Console.WriteLine($"You picked: {existsong.GetSongDetails()}");
                                                        goto Initialize2;
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
                            }
                            else
                            {
                                Console.WriteLine(ErrorMessages.PlaylistNotFound);
                                
                            }
                            goto Initialize2;

                        case ConsoleKey.V:
                            RepeatPLaylistId:
                            if (spotify.CurrentUser.Playlists.Count != 0)
                            {
                                
                                foreach (var playlist in spotify.CurrentUser.Playlists)
                                {
                                    playlist.GetPlaylistDetails();
                                }
                                     Console.WriteLine("Enter id of playlist");
                                     input = Console.ReadLine();
                                     isSucceed = int.TryParse(input, out result);
                                if (isSucceed)
                                {
                                    var Exist = spotify.CurrentUser.Playlists.FirstOrDefault(x => x.Id == result);
                                    if (Exist != null)
                                    {
                                        Exist.GetSongsinPlaylist();
                                    RepeatSongId: Console.WriteLine("Select id to listen");
                                        input = Console.ReadLine();
                                        isSucceed = int.TryParse(input, out result);
                                        if (isSucceed)
                                        {
                                            var Existsong = Exist.Songs.FirstOrDefault(x => x.Id == result);
                                            if (Existsong != null)
                                            {
                                                Console.WriteLine("You are listening...");
                                                Console.WriteLine(Existsong.Artistname);
                                                Console.WriteLine(Existsong.Songname);
                                                Console.WriteLine(Existsong.Genre);
                                                Console.WriteLine("Listened");
                                                goto Initialize2;
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
                                    goto RepeatPLaylistId;
                                }
                            }
                            else
                            {
                                Console.WriteLine(ErrorMessages.PlaylistNotFound);
                                goto Initialize2;
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

                                RepeatNumber: Console.WriteLine("Enter the number of the song you want to pick (or type '0' to quit):");
                                    input = Console.ReadLine().Trim();
                                    if (int.TryParse(input, out int selectedIndex))
                                    {
                                        var exist1 = foundSongs.FirstOrDefault(song => song.Id == selectedIndex);
                                        if (exist1 is not null)
                                        {


                                            Console.WriteLine("You listening...");
                                            Console.WriteLine($"You picked: {exist1.GetSongDetails()}");
                                            Console.WriteLine("Listened");
                                        RepeatChoice: Console.WriteLine("Do you want to add this song to playlist (Press y/n)");
                                            Key = Console.ReadKey(true).Key;
                                            if (Key == ConsoleKey.Y)
                                            {
                                                Console.WriteLine("Select playlist or create to add");
                                            RepeatPlaylistsId: foreach (var pleylist in spotify.CurrentUser.Playlists)
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
                                                        if (string.IsNullOrWhiteSpace(playlistname) && playlistname.Contains(" "))
                                                        {
                                                            Console.WriteLine("!Enter name!");
                                                            goto RepeatPlaylistname2;
                                                        }
                                                        else
                                                        {
                                                            spotify.CurrentUser.Playlists.Add(new Playlist(playlistname));
                                                            var newplaylist = spotify.CurrentUser.Playlists.FirstOrDefault(x => x.Playlistname == playlistname);
                                                            newplaylist.Songs.Add(exist1);
                                                            Console.WriteLine($"{playlistname} created and {exist1.Songname} vas added");
                                                            goto Initialize2;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var existsong = spotify.CurrentUser.Playlists.FirstOrDefault(x => x.Id == selectedIndex);
                                                        if (existsong is not null)
                                                        {
                                                            existsong.Songs.Add(foundSongs[selectedIndex - 1]);
                                                            Console.WriteLine($"{foundSongs[selectedIndex - 1].GetSongDetails()} add to {existsong.Playlistname}");
                                                            goto Initialize2;
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
                                                goto Initialize2;
                                            }
                                            else
                                            {
                                                Console.WriteLine(ErrorMessages.FormatError);
                                                goto RepeatChoice;
                                            }
                                        }
                                        else if(selectedIndex == 0)
                                        {
                                            goto Initialize2;
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

                        case ConsoleKey.R:
                        RepeatPlaylistId1:
                            if (spotify.CurrentUser.Playlists.Count != 0)
                            {
                                foreach (var pleylist in spotify.CurrentUser.Playlists)
                                {
                                    pleylist.GetPlaylistDetails();
                                }
                                Console.WriteLine("enter id of playlist to rename");
                                input = Console.ReadLine();
                                isSucceed = int.TryParse(input, out id);
                                if (isSucceed)
                                {
                                    var exist1 = spotify.CurrentUser.Playlists.FirstOrDefault(x => x.Id == id);
                                    if (exist1 != null)
                                    {
                                    RepeatNewName: Console.WriteLine("Enter new name");
                                        playlistname = Console.ReadLine();
                                        if (!string.IsNullOrWhiteSpace(playlistname) && !playlistname.Contains(" "))
                                        {
                                            exist1.Playlistname = playlistname;
                                            goto Initialize2;
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
                            }
                            else
                            {
                                Console.WriteLine(ErrorMessages.PlaylistNotFound);
                                goto Initialize2;
                            }

                        case ConsoleKey.Delete:
                        RepeatPlaylistId2:
                            if (spotify.CurrentUser.Playlists.Count != 0)
                            {
                                foreach (var pleylist in spotify.CurrentUser.Playlists)
                                {
                                    pleylist.GetPlaylistDetails();
                                }
                                Console.WriteLine("enter id of playlist to delete");
                                input = Console.ReadLine();
                                isSucceed = int.TryParse(input, out id);
                                if (isSucceed)
                                {
                                    var exist1 = spotify.CurrentUser.Playlists.FirstOrDefault(x => x.Id == id);
                                    if (exist1 != null)
                                    {
                                        spotify.CurrentUser.Playlists.Remove(exist1);
                                        goto Initialize2;
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
                            }
                            else
                            {
                                Console.WriteLine(ErrorMessages.PlaylistNotFound);
                                goto Initialize2;
                            }

                        case ConsoleKey.Backspace:
                        RepeatPlaylistId3:
                            if (spotify.CurrentUser.Playlists.Count != 0)
                            {
                                foreach (var pleylist in spotify.CurrentUser.Playlists)
                                {
                                    pleylist.GetPlaylistDetails();
                                }
                                Console.WriteLine("enter id of playlist to delete");
                                input = Console.ReadLine();
                                isSucceed = int.TryParse(input, out id);
                                if (isSucceed)
                                {
                                    var exist1 = spotify.CurrentUser.Playlists.FirstOrDefault(x => x.Id == id);
                                    if (exist1 != null)
                                    {
                                    RepeatSongId: foreach (var song in exist1.Songs)
                                        {
                                            song.GetSongDetails();
                                        }
                                        Console.WriteLine("Enter song id to delete");
                                        input = Console.ReadLine();
                                        isSucceed = int.TryParse(input, out id);
                                        if (isSucceed)
                                        {
                                            var existsong = exist1.Songs.FirstOrDefault(x => x.Id == id);
                                            if (existsong != null)
                                            {
                                                exist1.Songs.Remove(existsong);
                                                goto Initialize2;
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
                            }
                            else
                            {
                                Console.WriteLine(ErrorMessages.PlaylistNotFound);
                                goto Initialize2;
                            }
                        case ConsoleKey.Escape:
                            goto Intialize1;
                        case ConsoleKey.Q:
                            goto exit;

                        default:
                            Console.WriteLine(ErrorMessages.FormatError);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Error");
                }
            } while (!exit);
        exit: Console.WriteLine("exited");
        }
        static void SignIn(Spotify spotify)
        {
            Console.WriteLine("Enter email:");
            string email = Console.ReadLine();
            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();
            var user = spotify.SignIn(email, password);
            if (user != null)
            {
                Console.WriteLine($"Welcome, {user.Name}");
                spotify.CurrentUser = user;
            }
            else
            {
                Console.WriteLine("Invalid credentials. Please try again.");
            }
        }
    }
    
}

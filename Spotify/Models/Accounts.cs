using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify
{
    internal class Accounts
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Surname { get; set; }
        public List<Playlist> Playlists { get; set; }

        public Accounts(string name, string surname, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
            Surname = surname;
            Playlists = new List<Playlist>();
        }
        public void AddPlaylist(Playlist playlist)
        {
            Playlists.Add(playlist);
        }
        public void RemovePlaylist(Playlist playlist)
        {
            Playlists.Remove(playlist);
        }
    }
}

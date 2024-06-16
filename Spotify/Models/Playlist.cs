using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify
{
    internal class Playlist
    {
        public List<Song> Songs;
        private static int id { get; set; } = 1;
        public int Id { get; set; }
        public string Playlistname { get; set; }
        public Playlist(string name) 
        {
            Songs = new List<Song>();
            Id = id++;
            Playlistname = name;
        }
        public void GetPlaylistDetails()
        {
            Console.WriteLine($"Id: {Id} PLaylist: {Playlistname}");
        }
        public void GetSongsinPlaylist()
        {
            foreach(var song in Songs)
            {
                Console.WriteLine(song.GetSongDetails());
            }
        }
    }
}

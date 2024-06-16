using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify
{
    internal class Song
    {
        public string Songname {  get; set; }
        public string Artistname { get; set; }
        public string Genre { get; set; }
        public Song(string name, string artist, string genre)
        {
            Songname = name;
            Artistname = artist;
            Genre = genre;
        }
        public string GetSongDetails()
        {
            return $"Artist: {Artistname} - Song: {Songname} - Genre: {Genre}";
        }
    }
}

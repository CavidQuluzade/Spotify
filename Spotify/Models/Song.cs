using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify
{
    internal class Song
    {
        private static int id { get; set; } = 1;
        public int Id { get; set; }
        public string Songname {  get; set; }
        public string Artistname { get; set; }
        public string Genre { get; set; }
        public Song(string artist, string name, string genre)
        {
            Id = id ++;
            Songname = name;
            Artistname = artist;
            Genre = genre;
        }
        public string GetSongDetails()
        {
            return $"Id: {Id} - Artist: {Artistname} - Song: {Songname} - Genre: {Genre}";
        }
    }
}

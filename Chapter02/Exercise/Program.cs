using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise {
    class Program {
        static void Main(string[] args) {
            //2.1.3
            var songs = new Song[] {
                new Song("Let it be", "The Beatles", 234),
                new Song("Let it be", "The Beatles", 243),
                new Song("Let it be", "The Beatles", 252),
                new Song("Let it be", "The Beatles", 261),
                new Song("Let it be", "The Beatles", 270),
            };

            PrintSong(songs);
        }

        //2.1.4
        private static void PrintSong(Song[] songs) {
            foreach (var song in songs) {
                Console.WriteLine("{0}, {1}, {2:m\\:ss}",song.Title, song.ArtistName, TimeSpan.FromSeconds(song.Length));
            }
        }
    }
}

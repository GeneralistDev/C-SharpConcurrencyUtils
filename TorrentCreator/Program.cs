using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TorrentCreator
{
    class Program
    {
        public const int PIECE_SIZE = 32768;
        static void Main(string[] args)
        {
            String path = null;
            Console.WriteLine("File to create torrent for <absolute path>: ");
            path = Console.ReadLine();
            while (!(File.Exists(path)))
            {
                Console.WriteLine("File does not exist, try again.");
                Console.WriteLine("File to create torrent for <absolute path>: ");
                path = Console.ReadLine();
            }
            Sha1Hasher sha1Hasher = new Sha1Hasher();
            FileReader fileReader = new FileReader(path, sha1Hasher.inputChannel, PIECE_SIZE);

            path += ".torrent";
            FileStream torrentFile = new FileStream(path, FileMode.Create);

            TorrentFileWriter torrentFileWriter = new TorrentFileWriter(torrentFile, PIECE_SIZE);
            
        }
    }
}

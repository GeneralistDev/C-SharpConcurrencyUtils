using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace TorrentCreator
{
    class Program
    {
        public const int PIECE_SIZE = 32768;
        static void Main(string[] args)
        {
            String path;
            Console.Write("File to create torrent for <absolute path>: ");
            path = Console.ReadLine();
            while (!(File.Exists(path)))
            {
                Console.WriteLine("File does not exist, try again.");
                Console.Write("File to create torrent for <absolute path>: ");
                path = Console.ReadLine();
            }
            Sha1Hasher sha1Hasher = new Sha1Hasher();
            FileReader fileReader = new FileReader(path, sha1Hasher.inputChannel, PIECE_SIZE);

			string torrentPath = path + ".torrent";
			FileStream torrentFile = new FileStream(torrentPath, FileMode.Create);
			int pieces = (int)Math.Ceiling((double)fileReader.fileBytes.Length / (double)PIECE_SIZE);
			TorrentFileWriter torrentFileWriter = new TorrentFileWriter(torrentFile, pieces, sha1Hasher.outputChannel);

			torrentFileWriter.Start();

			StringBuilder torrentHeader = new StringBuilder();
			torrentHeader.Append("d8:announce44:udp://tracker.openbittorrent.com:80/announce");
			torrentHeader.Append("8:encoding5:UTF-8");
			torrentHeader.Append("4:info");
			torrentHeader.Append("d6:lengthi" + fileReader.fileBytes.Length + "e");
			string filename = path.Substring(path.LastIndexOf('/') + 1);
			torrentHeader.Append("4:name" + filename.Length + ":" + filename);
			torrentHeader.Append("12:piece lengthi" + PIECE_SIZE + "e");
			torrentHeader.Append("6:pieces" + pieces * 20 + ":");
			torrentFileWriter.inputChannel.Put(Encoding.UTF8.GetBytes(torrentHeader.ToString()));
				
			sha1Hasher.Start();

			fileReader.Start();

			while (!torrentFileWriter.WritingComplete()){}

			string end = "ee";
			torrentFileWriter.inputChannel.Put(Encoding.UTF8.GetBytes(end));

			while (!torrentFileWriter.WritingComplete()){}

			torrentFileWriter.Stop();
			//torrentFileWriter.Stop();
			torrentFile.Close();

			Console.WriteLine("Torrent file created: \"" + torrentPath + "\"");
        }
    }
}

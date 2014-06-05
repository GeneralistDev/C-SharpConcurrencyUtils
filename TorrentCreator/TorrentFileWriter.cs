using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcurrencyUtils;
using System.IO;

namespace TorrentCreator
{
    class TorrentFileWriter: InputChannelActiveObject<byte[]>
    {
        private FileStream torrentFile;
        private int numberOfPieces;
        private int piecesWritten = 0;

        public TorrentFileWriter(FileStream torrentFile, int numberOfPieces): base()
        {
            this.torrentFile = torrentFile;
            this.numberOfPieces = numberOfPieces;
        }

        protected override void Process(byte[] data)
        {
            torrentFile.Write(data, 0, data.Length);
            piecesWritten++;
            if (numberOfPieces == piecesWritten)
            {
                torrentFile.Close();
                Stop();
            }
        }
    }
}

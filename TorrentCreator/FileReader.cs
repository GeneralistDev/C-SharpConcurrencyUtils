using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcurrencyUtils;
using System.IO;

namespace TorrentCreator
{
    class FileReader: ActiveObject
    {
		public byte[] fileBytes;
        private readonly Channel<byte[]> hashChannel;
        private int pieceSize;
        public int numberOfPieces;

        public FileReader(String fileName, Channel<byte[]> hashChannel, int pieceSize): base()
        {
            fileBytes = File.ReadAllBytes(fileName);
            this.hashChannel = hashChannel;
            this.pieceSize = pieceSize;
            this.numberOfPieces = (fileBytes.Length + pieceSize - 1) / pieceSize;
        }

        protected override void Execute()
        {
            for (int pieceStart = 0; pieceStart < fileBytes.Length; pieceStart += pieceSize)
            {
                if (!(pieceStart > fileBytes.Length))
                {
                    byte[] piece = new byte[pieceSize];
                    if ((fileBytes.Length - pieceStart) < pieceSize)
                    {
                        for (int i = 0; i < (fileBytes.Length - pieceStart); i++)
                        {
                            piece[i] = fileBytes[pieceStart + i];
                        }
                    }
                    else
                    {
                        for (int i = 0; i < pieceSize; i++)
                        {
                            piece[i] = fileBytes[pieceStart + i];
                        }
                    }
                    hashChannel.Put(piece);
                }
            }
			byte[] endPiece = new byte[0];
			hashChannel.Put(endPiece);
        }
    }
}

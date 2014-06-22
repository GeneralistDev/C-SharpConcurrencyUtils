using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcurrencyUtils;
using System.IO;

namespace TorrentCreator
{
	/// <summary>
	/// File reader class, reads a single file in as bytes and processes
	/// them into piecesso they can be hashed.
	/// </summary>
    class FileReader: ActiveObject
    {
		public byte[] fileBytes;
        private readonly Channel<byte[]> hashChannel;
        private int pieceSize;
        public int numberOfPieces;

		/// <summary>
		/// Initializes a new instance of the FileReader class.
		/// </summary>
		/// <param name="fileName">File to create torrent for.</param>
		/// <param name="hashChannel">Hash channel.</param>
		/// <param name="pieceSize">Piece size.</param>
        public FileReader(String fileName, Channel<byte[]> hashChannel, int pieceSize): base()
        {
            fileBytes = File.ReadAllBytes(fileName);
            this.hashChannel = hashChannel;
            this.pieceSize = pieceSize;
            this.numberOfPieces = (fileBytes.Length + pieceSize - 1) / pieceSize;
        }

		/// <summary>
		/// Separate the read bytes into pieces and put them on the hashChannel
		/// </summary>
        protected override void Execute()
        {
            for (int pieceStart = 0; pieceStart < fileBytes.Length; pieceStart += pieceSize)
            {
                if (!(pieceStart > fileBytes.Length))
                {
					byte[] piece;
                    if ((fileBytes.Length - pieceStart) < pieceSize)
                    {
						piece = new byte[fileBytes.Length - pieceStart];
                        for (int i = 0; i < (fileBytes.Length - pieceStart); i++)
                        {
                            piece[i] = fileBytes[pieceStart + i];
                        }
                    }
                    else
                    {
						piece = new byte[pieceSize];
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

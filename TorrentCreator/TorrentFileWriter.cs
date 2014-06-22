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
	/// Writes to a torrent file.
	/// </summary>
    class TorrentFileWriter: InputChannelActiveObject<byte[]>
    {
        private FileStream torrentFile;
        private int numberOfPieces;
		private int piecesWritten = 0;
		private Boolean finished = false;

		/// <summary>
		/// Initializes a new instance of the TorrentFileWriter class.
		/// </summary>
		/// <param name="torrentFile">Torrent file to write to.</param>
		/// <param name="numberOfPieces">Number of pieces (hashes) to write.</param>
		/// <param name="hashOutputChannel">Hash output channel.</param>
		public TorrentFileWriter(FileStream torrentFile, int numberOfPieces, Channel<byte[]> hashOutputChannel): base()
        {
            this.torrentFile = torrentFile;
            this.numberOfPieces = numberOfPieces;
			this.inputChannel = hashOutputChannel;
        }

		/// <summary>
		/// Write the given byte[] to file. Sets a flag if all the expected
		/// pieces have been written.
		/// </summary>
		/// <param name="data">The data unit to process.</param>
        protected override void Process(byte[] data)
		{
			if (data.Length != 0)
			{
				torrentFile.Write(data, 0, data.Length);
				piecesWritten++;
				if (piecesWritten >= numberOfPieces + 1)
				{
					lock(this)
					{
						finished = true;
					}
				}
			}
        }

		/// <summary>
		/// Check if writing has completed. Resets the flag after checking
		/// </summary>
		/// <returns>true if writing completed, false otherwise.</returns>
		public Boolean WritingComplete() 
		{
			lock(this)
			{
				Boolean isFinished = finished;
				finished = false;
				return isFinished;
			}
		}
    }
}

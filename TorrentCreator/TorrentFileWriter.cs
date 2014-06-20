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
		private Boolean finished = false;

		public TorrentFileWriter(FileStream torrentFile, int numberOfPieces, Channel<byte[]> hashOutputChannel): base()
        {
            this.torrentFile = torrentFile;
            this.numberOfPieces = numberOfPieces;
			this.inputChannel = hashOutputChannel;
        }

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

		public Boolean writingComplete() 
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

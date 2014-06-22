using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConcurrencyUtils;
using System.Security.Cryptography;

namespace TorrentCreator
{
	/// <summary>
	/// Class that takes data off it's input channel, hashes it using the SHA-1
	/// hashing algorithm and puts the resulting hash on it's output channel.
	/// </summary>
    class Sha1Hasher: InputOutputChannelActiveObject<byte[], byte[]>
    {
        private SHA1 hasher = SHA1.Create();

		/// <summary>
		/// Hash a byte[] and put it on the output channel.
		/// </summary>
		/// <param name="data">The byte[] to hash.</param>
		/// <returns>The SHA-1 hash of the given data.</returns>
        protected override byte[] Process(byte[] data)
        {
			try {
				if (data.Length == 0)
				{
					Stop();
					return data;
				} else
				{
					return hasher.ComputeHash(data);
				}
			}
			catch (ThreadInterruptedException)
			{
				return data;
			}
        }
    }
}

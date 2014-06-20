using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcurrencyUtils;
using System.Security.Cryptography;

namespace TorrentCreator
{
    class Sha1Hasher: InputOutputChannelActiveObject<byte[], byte[]>
    {
        private SHA1 hasher = SHA1.Create();

        protected override byte[] Process(byte[] data)
        {
			if (data.Length == 0)
			{
				Stop();
				return data;
			} else
			{
				return hasher.ComputeHash(data);
			}
        }
    }
}

using System;
using ConcurrencyUtils;
using System.IO;

namespace ActiveObjectVowelCount
{
	public class FileWriter: InputChannelActiveObject<char>
	{
		FileStream outFile;
		public FileWriter(string path):base()
		{
			outFile = new FileStream(path, FileMode.Create);
		}

		protected override void Process(char data)
		{
			outFile.WriteByte((byte)data);
		}
	}
}


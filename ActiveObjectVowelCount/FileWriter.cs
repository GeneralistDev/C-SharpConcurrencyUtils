using System;
using ConcurrencyUtils;
using System.IO;

namespace ActiveObjectVowelCount
{
	/// <summary>
	/// Vowel File writer, writes whatever it gets on it's input channel to a file.
	/// </summary>
	public class FileWriter: InputChannelActiveObject<char>
	{
		FileStream outFile;

		/// <summary>
		/// Initializes a new instance of the FileWriter class.
		/// </summary>
		/// <param name="path">Path to create file.</param>
		public FileWriter(string path):base()
		{
			outFile = new FileStream(path, FileMode.Create);
		}

		/// <summary>
		/// Process a given unit of data.
		/// </summary>
		/// <param name="data">The data unit to process.</param>
		protected override void Process(char data)
		{
			outFile.WriteByte((byte)data);
		}
	}
}


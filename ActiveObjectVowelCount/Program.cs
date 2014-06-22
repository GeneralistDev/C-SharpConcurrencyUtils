using System;
using System.IO;
using ConcurrencyUtils;

namespace ActiveObjectVowelCount
{
	/// <summary>
	/// Main Program class.
	/// </summary>
	public class Program
	{
		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public static void Main(String[] args)
		{
			Console.Write("Where would you like the output file?: ");
			String path = Console.ReadLine();

			Semaphore askWait = new Semaphore(1);

			FileWriter fileWriter = new FileWriter(path);
			VowelCount vowelCount = new VowelCount();

			// Plug all the correct channels to each active object
			vowelCount.outputChannel = fileWriter.inputChannel;
			VowelFilter vowelFilter = new VowelFilter(vowelCount.inputChannel);
			SentenceReader sentenceReader = new SentenceReader(askWait);
			sentenceReader.outputChannel = vowelFilter.inputChannel;

			// Start all the active objects
			fileWriter.Start();
			vowelFilter.Start();
			vowelCount.Start();
			sentenceReader.Start();

			while (true)
			{
				vowelCount.PrintCount();
				askWait.Release();
			}
		}
	}
}

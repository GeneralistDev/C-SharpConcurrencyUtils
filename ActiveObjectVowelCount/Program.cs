using System;
using System.IO;
using ConcurrencyUtils;

namespace ActiveObjectVowelCount
{
	public class Program
	{
		public static void Main(String[] args)
		{
			Console.Write("Where would you like the output file?: ");
			String path = Console.ReadLine();

			Semaphore askWait = new Semaphore(1);

			FileWriter fileWriter = new FileWriter(path);
			VowelCount vowelCount = new VowelCount();

			vowelCount.outputChannel = fileWriter.inputChannel;
			VowelFilter vowelFilter = new VowelFilter(vowelCount.inputChannel);
			SentenceReader sentenceReader = new SentenceReader(askWait);
			sentenceReader.outputChannel = vowelFilter.inputChannel;

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

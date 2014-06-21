using System;
using System.IO;

namespace ActiveObjectVowelCount
{
	public class Program
	{
		public static void Main(String[] args)
		{
			Console.Write("Where would you like the output file?: ");
			String path = Console.ReadLine();

			FileWriter fileWriter = new FileWriter(path);
			VowelCount vowelCount = new VowelCount();

			vowelCount.outputChannel = fileWriter.inputChannel;
			VowelFilter vowelFilter = new VowelFilter(vowelCount.inputChannel);

			fileWriter.Start();
			vowelFilter.Start();
			vowelCount.Start();

			while (true)
			{
				Console.Write("Give me a sentence to find the vowels in:\n> ");
				String sentence = Console.ReadLine();
				vowelFilter.inputChannel.Put(sentence);
				vowelCount.printCount();
			}
		}
	}
}

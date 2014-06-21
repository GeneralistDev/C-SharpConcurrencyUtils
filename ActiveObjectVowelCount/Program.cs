using System;

namespace ActiveObjectVowelCount
{
	public class Program
	{
		public static void Main(String[] args)
		{
			VowelCount vowelCount = new VowelCount();
			VowelFilter vowelFilter = new VowelFilter(vowelCount.inputChannel);
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

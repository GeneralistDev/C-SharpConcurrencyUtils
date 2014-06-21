using System;
using ConcurrencyUtils;

namespace ActiveObjectVowelCount
{
	public class SentenceReader: OutputChannelActiveObject<string>
	{
		Semaphore askWait;
		public SentenceReader(Semaphore askWait):base()
		{
			this.askWait = askWait;
		}

		protected override string Process()
		{
			askWait.Acquire();
			Console.Write("Give me a sentence to find the vowels in:\n> ");
			String sentence = Console.ReadLine();
			return sentence;
		}
	}
}


using System;
using ConcurrencyUtils;

namespace ActiveObjectVowelCount
{
	/// <summary>
	/// 	Reads sentences from console and puts them on the output channel.
	/// </summary>
	public class SentenceReader: OutputChannelActiveObject<string>
	{
		Semaphore askWait;
		/// <summary>
		/// Initializes a new instance of the SentenceReader class.
		/// </summary>
		/// <param name="askWait">Ask wait.</param>
		public SentenceReader(Semaphore askWait):base()
		{
			this.askWait = askWait;
		}

		/// <summary>
		/// Reads lines from console, but only after acquiring the askWait semaphore.
		/// </summary>
		/// <returns></returns>
		protected override string Process()
		{
			askWait.Acquire();
			Console.Write("Give me a sentence to find the vowels in:\n> ");
			String sentence = Console.ReadLine();
			return sentence;
		}
	}
}


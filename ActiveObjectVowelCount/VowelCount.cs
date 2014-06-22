using System;
using System.Collections.Generic;
using ConcurrencyUtils;

namespace ActiveObjectVowelCount
{
	/// <summary>
	/// Counts the occurance of each vowel and keeps a total.
	/// </summary>
	public class VowelCount: InputOutputChannelActiveObject<char, char>
	{
		private Semaphore finishedSemaphore = new Semaphore(0);
		Dictionary<char, int> counts = new Dictionary<char, int>();

		/// <summary>
		/// Initializes a new instance of the VowelCount class.
		/// </summary>
		public VowelCount() : base()
		{
			counts.Add('a', 0);
			counts.Add('e', 0);
			counts.Add('i', 0);
			counts.Add('o', 0);
			counts.Add('u', 0);
		}

		/// <summary>
		/// Process the input character
		/// </summary>
		/// <param name="data">The character to process.</param>
		/// <returns>The same input character</returns>
		protected override char Process(char data)
		{
			if (data != (char)0)
			{
				counts[data] = ++counts[data];
				return data;
			} else
			{
				finishedSemaphore.Release();
				return '\n';
			}
		}

		/// <summary>
		/// Reset this counts for each character.
		/// </summary>
		public void Reset()
		{
			counts['a'] = 0;
			counts['e'] = 0;
			counts['i'] = 0;
			counts['o'] = 0;
			counts['u'] = 0;
		}

		/// <summary>
		/// Prints the counts for each character.
		/// </summary>
		public void PrintCount()
		{
			finishedSemaphore.Acquire();
			foreach (char c in counts.Keys)
			{
				Console.WriteLine("\'" + c + "\' occurred " + counts[c] + " times");
			}
			Reset();
		}
	}
}


using System;
using ConcurrencyUtils;

namespace ActiveObjectVowelCount
{
	/// <summary>
	/// Put all the vowels in a given string onto the counterChannel
	/// </summary>
	public class VowelFilter: InputChannelActiveObject<String>
	{
		private Channel<char> counterChannel;

		/// <summary>
		/// Initializes a new instance of the VowelFilter class.
		/// </summary>
		/// <param name="counterChannel">Counter channel.</param>
		public VowelFilter(Channel<char> counterChannel): base()
		{
			this.counterChannel = counterChannel;
		}

		/// <summary>
		/// Select the vowels in the given string and put them onto the counterChannel.
		/// </summary>
		/// <param name="data">The data unit to process.</param>
		protected override void Process(string data)
		{
			foreach (char c in data)
			{
				if (c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u')
				{
					counterChannel.Put(c);
				}
			}
			counterChannel.Put((char)0);
		}
	}
}


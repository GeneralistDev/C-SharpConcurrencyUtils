using System;
using ConcurrencyUtils;

namespace ActiveObjectVowelCount
{
	public class VowelFilter: InputChannelActiveObject<String>
	{
		private Channel<char> counterChannel;
		public VowelFilter(Channel<char> counterChannel): base()
		{
			this.counterChannel = counterChannel;
		}

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


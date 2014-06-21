using System;

namespace RandomTutorial
{
	public class Program
	{
		public static void Main(String[] args)
		{
			// Create the generator using default time seed
			Random randomGenerator = new Random();

			// Get the next random integer
			int randInt = randomGenerator.Next();

			// Get a random number no larger than 100
			int randMax = randomGenerator.Next(100);

			// Get a random number between -100 and 100
			int randRange = randomGenerator.Next(-100, 100);

			// Get one hundred random numbers into a byte array;
			byte[] randHundred = new byte[100];
			randomGenerator.NextBytes(randHundred);

			// Get a random floating point number between 0.0 and 1.0
			double randDouble = randomGenerator.NextDouble();

			Console.WriteLine("randInt: " + randInt);
			Console.WriteLine("randMax: " + randMax);
			Console.WriteLine("randRange: " + randRange);
			Console.WriteLine("randHundred: ");
			for (int i = 0; i < randHundred.Length; i++)
			{
				Console.WriteLine(randHundred[i]);
			}
			Console.WriteLine("randDouble: " + randDouble);
		}
	}
}


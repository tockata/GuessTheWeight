namespace GuessTheWeight
{
    using System;
    using System.Threading;
    using Contracts;

    public class CustomRandom : IRandomGenerator
    {
        private static int seed = Environment.TickCount;

        private readonly ThreadLocal<Random> random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

        public int Rand(int minValue, int maxValue)
        {
            return this.random.Value.Next(minValue, maxValue);
        }
    }
}
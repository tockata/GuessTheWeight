namespace GuessTheWeight
{
    using System;
    using System.Threading;

    public static class StaticRandom
    {
        private static int seed = Environment.TickCount;

        private static readonly ThreadLocal<Random> Random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

        public static int Rand(int minValue, int maxValue)
        {
            return Random.Value.Next(minValue, maxValue);
        }
    }
}
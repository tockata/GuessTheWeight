namespace GuessTheWeight.Models
{
    using Enumerations;
    using GuessTheWeight.Contracts;

    public class MemoryPlayer : Player, IHonestPlayer, IRandomPlayer
    {
        private const PlayerType DefaultPlayerType = PlayerType.Memory;

        public MemoryPlayer(string name, IRandomGenerator randomGenerator)
            : base(name)
        {
            this.Type = DefaultPlayerType;
            this.RandomGenerator = randomGenerator;
        }

        public IRandomGenerator RandomGenerator { get; set; }

        public int MakeGuess()
        {
            bool isAllowedGuess = false;
            int guess = int.MinValue;
            while (!isAllowedGuess)
            {
                guess = this.RandomGenerator.Rand(MinWeight, MaxWeight);
                isAllowedGuess = !this.Guesses.Contains(guess);
            }

            this.Guesses.Add(guess);
            return guess;
        }
    }
}
namespace GuessTheWeight.Models
{
    using GuessTheWeight.Contracts;
    using GuessTheWeight.Enumerations;

    public class RandomPlayer : Player, IHonestPlayer, IRandomPlayer
    {
        private const PlayerType DefaultPlayerType = PlayerType.Random;

        public RandomPlayer(string name, IRandomGenerator randomGenerator)
            : base(name)
        {
            this.Type = DefaultPlayerType;
            this.RandomGenerator = randomGenerator;
        }

        public IRandomGenerator RandomGenerator { get; set; }

        public int MakeGuess()
        {
            int guess = this.RandomGenerator.Rand(MinWeight, MaxWeight);
            this.Guesses.Add(guess);

            return guess;
        }
    }
}
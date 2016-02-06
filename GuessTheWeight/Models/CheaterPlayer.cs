namespace GuessTheWeight.Models
{
    using System.Collections;
    using System.Linq;
    using Enumerations;
    using GuessTheWeight.Contracts;

    public class CheaterPlayer : Player, IRandomPlayer, ICheaterPlayer
    {
        private const PlayerType DefaultPlayerType = PlayerType.Cheater;
        private const int GuessOffset = 40;

        public CheaterPlayer(string name, IRandomGenerator randomGenerator)
            : base(name)
        {
            this.Type = DefaultPlayerType;
            this.RandomGenerator = randomGenerator;
        }

        public IRandomGenerator RandomGenerator { get; set; }

        public int MakeGuess(IEnumerable allGuesses)
        {
            bool[] allGuessesArray = allGuesses.Cast<bool>().ToArray();
            bool isAllowedGuess = false;
            int guess = int.MinValue;
            while (!isAllowedGuess)
            {
                guess = this.RandomGenerator.Rand(MinWeight, MaxWeight);
                isAllowedGuess = !allGuessesArray[guess - GuessOffset];
            }

            this.Guesses.Add(guess);

            return guess;
        }
    }
}
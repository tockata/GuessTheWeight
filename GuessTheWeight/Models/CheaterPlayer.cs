namespace GuessTheWeight.Models
{
    using System.Collections;
    using System.Linq;
    using Enumerations;
    using GuessTheWeight.Contracts;

    public class CheaterPlayer : RandomPlayer, ICheaterPlayer
    {
        private const PlayerType DefaultPlayerType = PlayerType.Cheater;
        private const int GuessOffset = 40;

        public CheaterPlayer(string name, IRandomGenerator randomGenerator)
            : base(name, randomGenerator)
        {
            this.Type = DefaultPlayerType;
        }

        public int MakeCheatGuess(IEnumerable allGuesses)
        {
            bool[] allGuessesArray = allGuesses.Cast<bool>().ToArray();
            bool isAllowedGuess = false;
            int guess = int.MinValue;

            while (!isAllowedGuess)
            {
                guess = base.MakeGuess();
                isAllowedGuess = !allGuessesArray[guess - GuessOffset];
            }

            return guess;
        }
    }
}
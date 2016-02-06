namespace GuessTheWeight.Models
{
    using System;
    using System.Collections;
    using System.Linq;
    using GuessTheWeight.Contracts;
    using GuessTheWeight.Enumerations;

    public class ThoroughCheaterPlayer : Player, ICheaterPlayer
    {
        private const PlayerType DefaultPlayerType = PlayerType.ThoroughCheater;
        private const int GuessOffset = 40;

        public ThoroughCheaterPlayer(string name)
            : base(name)
        {
            this.Type = DefaultPlayerType;
        }

        public int MakeGuess(IEnumerable allGuesses)
        {
            bool[] allGuessesArray = allGuesses.Cast<bool>().ToArray();
            int firstAllowedGuessIndex = Array.IndexOf(allGuessesArray, false);
            int guess = firstAllowedGuessIndex + GuessOffset;

            this.Guesses.Add(guess);
            return guess;
        }
    }
}
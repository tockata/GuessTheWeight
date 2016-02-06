﻿namespace GuessTheWeight.Models
{
    using System;
    using System.Collections;
    using System.Linq;
    using GuessTheWeight.Contracts;
    using GuessTheWeight.Enumerations;

    public class ThoroughCheaterPlayer : ThoroughPlayer, ICheaterPlayer
    {
        private const PlayerType DefaultPlayerType = PlayerType.ThoroughCheater;

        public ThoroughCheaterPlayer(string name)
            : base(name)
        {
            this.Type = DefaultPlayerType;
        }

        public int MakeCheatGuess(IEnumerable allGuesses)
        {
            bool[] allGuessesArray = allGuesses.Cast<bool>().ToArray();
            bool isAllowedGuess = false;
            int guess = base.MakeGuess();

            while (!isAllowedGuess)
            {
                isAllowedGuess = !allGuessesArray[guess - GuessOffset];
                if (!isAllowedGuess)
                {
                    guess++;
                }
            }

            return guess;
        }
    }
}
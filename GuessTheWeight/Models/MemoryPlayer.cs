namespace GuessTheWeight.Models
{
    using Enumerations;
    using GuessTheWeight.Contracts;

    public class MemoryPlayer : RandomPlayer
    {
        private const PlayerType DefaultPlayerType = PlayerType.Memory;

        public MemoryPlayer(string name, IRandomGenerator randomGenerator)
            : base(name, randomGenerator)
        {
            this.Type = DefaultPlayerType;
        }

        public override int MakeGuess()
        {
            bool isAllowedGuess = false;
            int guess = int.MinValue;
            while (!isAllowedGuess)
            {
                guess = base.MakeGuess();
                isAllowedGuess = !this.Guesses.Contains(guess);
            }

            return guess;
        }
    }
}
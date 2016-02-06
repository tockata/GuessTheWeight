namespace GuessTheWeight.Models
{
    using GuessTheWeight.Enumerations;

    public class ThoroughPlayer : Player
    {
        private const PlayerType DefaultPlayerType = PlayerType.Thorough;
        protected const int GuessOffset = 40;

        public ThoroughPlayer(string name)
            : base(name)
        {
            this.Type = DefaultPlayerType;
        }

        public override int MakeGuess()
        {
            int guessesCount = this.Guesses.Count;
            int guess = guessesCount + GuessOffset;

            return guess;
        }
    }
}
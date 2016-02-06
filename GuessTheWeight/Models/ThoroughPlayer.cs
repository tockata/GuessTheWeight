namespace GuessTheWeight.Models
{
    using Contracts;
    using GuessTheWeight.Enumerations;

    public class ThoroughPlayer : Player, IHonestPlayer
    {
        private const PlayerType DefaultPlayerType = PlayerType.Thorough;
        private const int GuessOffset = 40;

        public ThoroughPlayer(string name)
            : base(name)
        {
            this.Type = DefaultPlayerType;
        }

        public int MakeGuess()
        {
            int guessesCount = this.Guesses.Count;
            int guess = guessesCount + GuessOffset;

            this.Guesses.Add(guess);
            return guess;
        }
    }
}
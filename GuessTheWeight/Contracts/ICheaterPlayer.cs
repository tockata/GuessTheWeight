namespace GuessTheWeight.Contracts
{
    using System.Collections;

    public interface ICheaterPlayer : IPlayer
    {
        int MakeGuess(IEnumerable allGuesses);
    }
}
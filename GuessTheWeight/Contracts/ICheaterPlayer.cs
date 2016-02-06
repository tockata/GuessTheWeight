namespace GuessTheWeight.Contracts
{
    using System.Collections;

    public interface ICheaterPlayer : IPlayer
    {
        int MakeCheatGuess(IEnumerable allGuesses);
    }
}
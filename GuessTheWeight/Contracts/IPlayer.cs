namespace GuessTheWeight.Contracts
{
    using System.Collections.Generic;
    using GuessTheWeight.Enumerations;

    public interface IPlayer
    {
        string Name { get; set; }

        HashSet<int> Guesses { get; }

        PlayerType Type { get; set; }
    }
}
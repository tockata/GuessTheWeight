namespace GuessTheWeight.Contracts
{
    public interface IRandomPlayer : IPlayer
    {
        IRandomGenerator RandomGenerator { get; set; }
    }
}
namespace GuessTheWeight
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    public class GameEngine
    {
        private const int GuessOffset = 40;
        private const int MinWeight = 40;
        private const int MaxWeight = 141;
        private const int MaxGuessPosibilities = 101;

        private object thisLock = new object();

        private List<Player> players;

        private int attemptsCount = 0;
        private bool[] allGuesses;

        private long elapsedMilliseconds = 0;
        private bool isWeightFound = false;
        private int realWeight;
        private Player winner;
        private int closestWeight = -1;

        public GameEngine()
        {
            this.players = new List<Player>();
            this.allGuesses = new bool[MaxGuessPosibilities];
        }

        public void Start()
        {
            this.GetPlayers();
            this.realWeight = StaticRandom.Rand(MinWeight, MaxWeight);
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("Fruit basket real weight is: {0}", this.realWeight);
            Console.WriteLine();
            Stopwatch stopwatch = new Stopwatch();
            this.StartPlayersThreads();

            stopwatch.Start();
            while (!this.isWeightFound && this.attemptsCount < 100 && this.elapsedMilliseconds < 1500)
            {
                this.elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            }

            stopwatch.Stop();

            if (this.isWeightFound)
            {
                Console.WriteLine("Winner is: {0}. Total attempts in game: {1}", this.winner.Name, this.attemptsCount);
            }
            else
            {
                Console.WriteLine("Winner with closest guess is: {0}. Guess: {1}", this.winner.Name, this.closestWeight);
            }
        }

        private void GetPlayers()
        {
            bool isCorrectNumberOfPlayers = false;
            int numberOfPlayers = int.MinValue;
            while (!isCorrectNumberOfPlayers)
            {
                try
                {
                    Console.Write("Enter number of players [2 to 8]: ");
                    numberOfPlayers = int.Parse(Console.ReadLine());

                    if (numberOfPlayers < 2 || 8 < numberOfPlayers)
                    {
                        throw new FormatException("Number of players must be between 2 and 8!");
                    }

                    isCorrectNumberOfPlayers = true;
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            this.SetPlayersNamesAndTypes(numberOfPlayers);
        }

        private void SetPlayersNamesAndTypes(int numberOfPlayers)
        {
            Console.WriteLine("Enter players \"Name\" and choose players types");
            Console.WriteLine("Player types are:");
            Console.WriteLine("1. Random player");
            Console.WriteLine("2. Memory player");
            Console.WriteLine("3. Thorough player");
            Console.WriteLine("4. Cheater player");
            Console.WriteLine("5. Thorough Cheater player");
            for (int i = 0; i < numberOfPlayers; i++)
            {
                bool isCorrectPlayerInput = false;
                while (!isCorrectPlayerInput)
                {
                    try
                    {
                        Console.Write("Enter Player {0} name: ", i + 1);
                        string name = Console.ReadLine();

                        Console.Write("Choose type for player {0} [1 to 5]: ", i + 1);
                        int playerTypeInput = int.Parse(Console.ReadLine());
                        if (playerTypeInput < 1 || 5 < playerTypeInput)
                        {
                            throw new FormatException("Player type must be between 1 and 5!");
                        }

                        PlayerType playerType = PlayerType.Random;
                        switch (playerTypeInput)
                        {
                            case 2:
                                playerType = PlayerType.Memory;
                                break;

                            case 3:
                                playerType = PlayerType.Thorough;
                                break;

                            case 4:
                                playerType = PlayerType.Cheater;
                                break;

                            case 5:
                                playerType = PlayerType.ThoroughCheater;
                                break;

                            default:
                                break;
                        }

                        Player newPlayer = new Player(name, playerType);
                        this.players.Add(newPlayer);

                        isCorrectPlayerInput = true;
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (ArgumentNullException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private void StartPlayersThreads()
        {
            for (int i = 0; i < this.players.Count; i++)
            {
                Thread newThread = new Thread(this.Play);
                newThread.Start(this.players[i]);
            }
        }

        private void Play(object param)
        {
            Player player = param as Player;
            int guess;
            while (!this.isWeightFound && this.attemptsCount < 100 && this.elapsedMilliseconds < 1500)
            {
                lock (this.thisLock)
                {
                    this.attemptsCount++;
                }

                switch (player.Type)
                {
                    case PlayerType.Random:
                        guess = this.RandomPlayerGuess(player);
                        this.ProcessGuess(guess, player);
                        break;

                    case PlayerType.Memory:
                        guess = this.MemoryPlayerGuess(player);
                        this.ProcessGuess(guess, player);
                        break;

                    case PlayerType.Thorough:
                        guess = this.ThoroughPlayerGuess(player);
                        this.ProcessGuess(guess, player);
                        break;

                    case PlayerType.Cheater:
                        guess = this.CheaterPlayerGuess(player);
                        this.ProcessGuess(guess, player);
                        break;

                    case PlayerType.ThoroughCheater:
                        guess = this.ThoroughCheaterPlayerGuess(player);
                        this.ProcessGuess(guess, player);
                        break;

                    default:
                        break;
                }
            }
        }

        private int RandomPlayerGuess(Player player)
        {
            int guess = StaticRandom.Rand(MinWeight, MaxWeight);
            player.Guesses.Add(guess);
            lock (this.thisLock)
            {
                this.allGuesses[guess - GuessOffset] = true;
            }

            return guess;
        }

        private int MemoryPlayerGuess(Player player)
        {
            bool isAllowedGuess = false;
            int guess = int.MinValue;
            while (!isAllowedGuess)
            {
                guess = StaticRandom.Rand(MinWeight, MaxWeight);
                isAllowedGuess = !player.Guesses.Contains(guess);
            }

            player.Guesses.Add(guess);
            lock (this.thisLock)
            {
                this.allGuesses[guess - GuessOffset] = true;
            }

            return guess;
        }

        private int ThoroughPlayerGuess(Player player)
        {
            int guessesCount = player.Guesses.Count;
            int guess = guessesCount + GuessOffset;

            player.Guesses.Add(guess);
            lock (this.thisLock)
            {
                this.allGuesses[guess - GuessOffset] = true;
            }

            return guess;
        }

        private int CheaterPlayerGuess(Player player)
        {
            bool isAllowedGuess = false;
            int guess = int.MinValue;
            lock (this.thisLock)
            {
                while (!isAllowedGuess)
                {
                    guess = StaticRandom.Rand(MinWeight, MaxWeight);
                    isAllowedGuess = !this.allGuesses[guess - GuessOffset];
                }

                player.Guesses.Add(guess);
                this.allGuesses[guess - GuessOffset] = true;
            }

            return guess;
        }

        private int ThoroughCheaterPlayerGuess(Player player)
        {
            lock (this.thisLock)
            {
                int firstAllowedGuessIndex = Array.IndexOf(this.allGuesses, false);
                int guess = firstAllowedGuessIndex + GuessOffset;

                player.Guesses.Add(guess);
                this.allGuesses[guess - GuessOffset] = true;
                return guess;
            }
        }

        private void ProcessGuess(int guess, Player player)
        {
            if (guess == this.realWeight)
            {
                lock (this.thisLock)
                {
                    this.isWeightFound = true;
                    this.winner = player;
                }
            }
            else
            {
                int currentGuessDelta = Math.Abs(this.realWeight - guess);
                lock (this.thisLock)
                {
                    int closestGuessDelta = Math.Abs(this.realWeight - this.closestWeight);
                    if (currentGuessDelta < closestGuessDelta)
                    {
                        this.closestWeight = guess;
                        this.winner = player;
                    }
                }

                Thread.Sleep(currentGuessDelta);
            }
        }
    }
}
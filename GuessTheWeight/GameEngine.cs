namespace GuessTheWeight
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using Contracts;
    using Models;

    public class GameEngine
    {
        private const int MinNumberOfPlayers = 2;
        private const int MaxNumberOfPlayers = 8;
        private const int GuessOffset = 40;
        private const int MinWeight = 40;
        private const int MaxWeight = 141;
        private const int MaxGuessPosibilities = 101;
        private const int MaxAttemptsCount = 100;
        private const int MaxElapsedMilliseconds = 1500;

        private object thisLock = new object();

        private List<IPlayer> players;

        private int attemptsCount = 0;
        private bool[] allGuesses;

        private long elapsedMilliseconds = 0;
        private bool isWeightFound = false;
        private int realWeight;
        private IPlayer winner;
        private int closestWeight = -1;

        public GameEngine()
        {
            this.players = new List<IPlayer>();
            this.allGuesses = new bool[MaxGuessPosibilities];
        }

        public void Start()
        {
            this.GetPlayers();
            this.realWeight = new CustomRandom().Rand(MinWeight, MaxWeight);
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("Fruit basket real weight is: {0}", this.realWeight);
            Console.WriteLine();
            Stopwatch stopwatch = new Stopwatch();
            this.StartPlayersThreads();

            stopwatch.Start();
            while (!this.isWeightFound &&
                this.attemptsCount < MaxAttemptsCount &&
                this.elapsedMilliseconds < MaxElapsedMilliseconds)
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

                    if (numberOfPlayers < MinNumberOfPlayers || MaxNumberOfPlayers < numberOfPlayers)
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

                        switch (playerTypeInput)
                        {
                            case 1:
                                {
                                    RandomPlayer newPlayer = new RandomPlayer(name, new CustomRandom());
                                    this.players.Add(newPlayer);
                                    break;
                                }

                            case 2:
                                {
                                    MemoryPlayer newPlayer = new MemoryPlayer(name, new CustomRandom());
                                    this.players.Add(newPlayer);
                                    break;
                                }

                            case 3:
                                {
                                    ThoroughPlayer newPlayer = new ThoroughPlayer(name);
                                    this.players.Add(newPlayer);
                                    break;
                                }

                            case 4:
                                {
                                    CheaterPlayer newPlayer = new CheaterPlayer(name, new CustomRandom());
                                    this.players.Add(newPlayer);
                                    break;
                                }

                            case 5:
                                {
                                    ThoroughCheaterPlayer newPlayer = new ThoroughCheaterPlayer(name);
                                    this.players.Add(newPlayer);
                                    break;
                                }

                            default:
                                break;
                        }

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
            while (!this.isWeightFound &&
                this.attemptsCount < MaxAttemptsCount &&
                this.elapsedMilliseconds < MaxElapsedMilliseconds)
            {
                lock (this.thisLock)
                {
                    this.attemptsCount++;
                }

                if (player is ICheaterPlayer)
                {
                    lock (this.thisLock)
                    {
                        guess = (player as ICheaterPlayer).MakeCheatGuess(this.allGuesses);
                        player.Guesses.Add(guess);
                        this.allGuesses[guess - GuessOffset] = true;
                    }

                    this.ProcessGuess(guess, player);
                }
                else
                {
                    guess = player.MakeGuess();
                    player.Guesses.Add(guess);
                    this.allGuesses[guess - GuessOffset] = true;

                    this.ProcessGuess(guess, player);
                }
            }
        }

        private void ProcessGuess(int guess, Player player)
        {
            if (guess == this.realWeight)
            {
                lock (this.thisLock)
                {
                    if (this.isWeightFound == false)
                    {
                        this.isWeightFound = true;
                        this.winner = player;
                    }
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
namespace GuessTheWeight
{
    using System;
    using System.Collections.Generic;

    public class Player
    {
        private string name;
        private HashSet<int> guesses;

        public Player(string name, PlayerType type)
        {
            this.Name = name;
            this.Type = type;
            this.guesses = new HashSet<int>();
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Name", "Player name can not be empty!");
                }

                this.name = value;
            }
        }

        public HashSet<int> Guesses
        {
            get
            {
                return this.guesses;
            }

            private set
            {
            }
        }

        public PlayerType Type { get; set; }
    }
}
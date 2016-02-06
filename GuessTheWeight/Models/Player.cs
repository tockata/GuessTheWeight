namespace GuessTheWeight.Models
{
    using System;
    using System.Collections.Generic;
    using Contracts;
    using Enumerations;

    public abstract class Player : IPlayer
    {
        protected const int MinWeight = 40;
        protected const int MaxWeight = 141;

        private string name;
        private HashSet<int> guesses;

        public Player(string name)
        {
            this.Name = name;
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
            get { return this.guesses; }

            private set { }
        }

        public PlayerType Type { get; set; }
    }
}
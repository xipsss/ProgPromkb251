using System;

namespace CatAndMouseGame
{
    public class Player
    {
        public string Name { get; private set; }
        public int Position { get; private set; }
        public int TotalDistance { get; private set; }
        public PlayerState State { get; private set; }
        private int boardSize;

        public Player(string name, int boardSize)
        {
            Name = name;
            this.boardSize = boardSize;
            Position = 0;
            TotalDistance = 0;
            State = PlayerState.NotInGame;
        }

        public bool IsInGame => State != PlayerState.NotInGame;

        public void SetPosition(int pos)
        {
            if (pos >= 1 && pos <= boardSize)
            {
                Position = pos;
                State = PlayerState.Playing;
            }
        }

        public void Move(int steps)
        {
            if (State == PlayerState.Winner || State == PlayerState.Loser) return;

            int currentZeroBased = Position - 1;
            int newZeroBased = currentZeroBased + steps;
            newZeroBased = ((newZeroBased % boardSize) + boardSize) % boardSize;
            Position = newZeroBased + 1;
            TotalDistance += Math.Abs(steps);
        }

        public void SetWinner() => State = PlayerState.Winner;
        public void SetLoser() => State = PlayerState.Loser;
    }
}
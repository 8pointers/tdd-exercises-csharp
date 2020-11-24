using System;

namespace roulette
{
    public class Bet
    {
        private Player Player { get; }

        public int Value { get; }

        public Bet(Player player, int value)
        {
            Player = player;
            Value = value;
        }

        private bool Equals(Bet other)
        {
            return Player.Equals(other.Player) && Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Bet) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Player, Value);
        }
    }
}

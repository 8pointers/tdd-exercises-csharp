using System;

namespace roulette
{
    public class Bet
    {
        private Player Player { get; }
        public int Value { get; }
        private BetType BetType { get; }

        public Bet(Player player, int value, BetType betType = BetType.SINGLE)
        {
            Player = player;
            Value = value;
            BetType = betType;
        }

        private bool Equals(Bet other)
        {
            return Equals(Player, other.Player) && Value == other.Value && BetType == other.BetType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Bet) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Player, Value, (int) BetType);
        }
    }
}

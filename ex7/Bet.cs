using System;

namespace roulette
{
    public class Bet
    {
        private Player p;
        private int betValue;
        private BetType betType;

        public Bet(Player p, int value)
            : this(p, value, BetType.SINGLE)
        {
        }

        public Bet(Player p, int value, BetType betType)
        {
            this.p = p;
            this.betValue = value;
            this.betType = betType;
        }

        public int Value
        {
            get { return betValue; }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Bet)) return false;
            return Equals((Bet) obj);
        }

        public bool Equals(Bet obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj.p, p) && Equals(obj.betValue, betValue) && obj.betType == this.betType;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((p != null ? p.GetHashCode() : 0) * 397) + (betValue.GetHashCode()) + (betType.GetHashCode());
            }
        }

        public Player Player
        {
            get { return p; }
        }

        public int NumberOfFields
        {
            get
            {
                switch (betType)
                {
                    case BetType.SINGLE: return 1;
                    case BetType.SPLIT: return 2;
                }

                throw new ArgumentException("Cannot find number of fields for " + betType);
            }
        }
    }
}
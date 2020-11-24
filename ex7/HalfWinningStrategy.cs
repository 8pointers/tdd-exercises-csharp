namespace roulette
{
    public class HalfWinningStrategy : WinningStrategy
    {
        private int half;

        public HalfWinningStrategy(int half)
        {
            this.half = half;
        }

        public bool WinsOn(int wheelPosition)
        {
            if (wheelPosition == 0) return false;
            return ((wheelPosition - 1) / 18) + 1 == half;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(HalfWinningStrategy)) return false;
            return Equals((HalfWinningStrategy) obj);
        }

        public bool Equals(HalfWinningStrategy obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.half == this.half;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (half.GetHashCode());
            }
        }
    }
}
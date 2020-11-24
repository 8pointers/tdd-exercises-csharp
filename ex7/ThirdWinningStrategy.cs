namespace roulette
{
    public class ThirdWinningStrategy : WinningStrategy
    {
        private int third;

        public ThirdWinningStrategy(int third)
        {
            this.third = third;
        }

        public bool WinsOn(int wheelPosition)
        {
            if (wheelPosition == 0) return false;
            return ((wheelPosition - 1) / 12) + 1 == third;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ThirdWinningStrategy)) return false;
            return Equals((ThirdWinningStrategy) obj);
        }

        public bool Equals(ThirdWinningStrategy obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.third == this.third;
        }

        public override int GetHashCode()
        {
            return (third.GetHashCode());
        }
    }
}
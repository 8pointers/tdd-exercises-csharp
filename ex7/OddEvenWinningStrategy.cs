namespace roulette
{
    public class OddEvenWinningStrategy : WinningStrategy
    {
        private bool isOdd;

        public OddEvenWinningStrategy(bool isOdd)
        {
            this.isOdd = isOdd;
        }

        public bool WinsOn(int wheelPosition)
        {
            if (wheelPosition == 0) return false;
            if (wheelPosition % 2 == 1 && isOdd) return true;
            if (wheelPosition % 2 == 0 && (!isOdd)) return true;
            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(OddEvenWinningStrategy)) return false;
            return Equals((OddEvenWinningStrategy) obj);
        }

        public bool Equals(OddEvenWinningStrategy obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.isOdd == this.isOdd;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (isOdd.GetHashCode());
            }
        }
    }
}
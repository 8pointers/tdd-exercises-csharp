namespace roulette
{
    public class OddEvenWinningStrategy : WinningStrategy
    {
        private readonly bool _isOdd;

        public OddEvenWinningStrategy(bool isOdd)
        {
            _isOdd = isOdd;
        }

        public bool WinsOn(int wheelPosition)
        {
            if (wheelPosition == 0) return false;
            if (wheelPosition % 2 == 1 && _isOdd) return true;
            return wheelPosition % 2 == 0 && (!_isOdd);
        }

        private bool Equals(OddEvenWinningStrategy other)
        {
            return _isOdd == other._isOdd;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((OddEvenWinningStrategy) obj);
        }

        public override int GetHashCode()
        {
            return _isOdd.GetHashCode();
        }
    }
}

namespace roulette
{
    public class SingleNumberWinningStrategy : WinningStrategy
    {
        private readonly int _wheelPosition;

        public SingleNumberWinningStrategy(int wheelPosition)
        {
            _wheelPosition = wheelPosition;
        }

        public bool WinsOn(int wheelPosition)
        {
            return wheelPosition == _wheelPosition;
        }

        private bool Equals(SingleNumberWinningStrategy other)
        {
            return _wheelPosition == other._wheelPosition;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((SingleNumberWinningStrategy) obj);
        }

        public override int GetHashCode()
        {
            return _wheelPosition;
        }
    }
}

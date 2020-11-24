namespace roulette
{
    public class SingleNumberWinningStrategy : WinningStrategy
    {
        private int wheelPosition;

        public SingleNumberWinningStrategy(int wheelPosition)
        {
            this.wheelPosition = wheelPosition;
        }

        public bool WinsOn(int wheelPosition)
        {
            return wheelPosition == this.wheelPosition;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(SingleNumberWinningStrategy)) return false;
            return Equals((SingleNumberWinningStrategy) obj);
        }

        public bool Equals(SingleNumberWinningStrategy obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.wheelPosition == this.wheelPosition;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (wheelPosition.GetHashCode());
            }
        }
    }
}
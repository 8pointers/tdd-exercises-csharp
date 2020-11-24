using System.Linq;

namespace roulette
{
    class RedBlackWinningStrategy : WinningStrategy
    {
        private static readonly int[] redNumbers = new int[]
            {1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36};

        private bool isRed;

        public RedBlackWinningStrategy(bool isRed)
        {
            this.isRed = isRed;
        }

        public bool WinsOn(int wheelPosition)
        {
            if (wheelPosition == 0) return false;
            if (isRed && redNumbers.Contains(wheelPosition)) return true;
            if (!isRed && !redNumbers.Contains(wheelPosition)) return true;
            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(RedBlackWinningStrategy)) return false;
            return Equals((RedBlackWinningStrategy) obj);
        }

        public bool Equals(RedBlackWinningStrategy obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.isRed == this.isRed;
        }

        public override int GetHashCode()
        {
            return (isRed.GetHashCode());
        }
    }
}
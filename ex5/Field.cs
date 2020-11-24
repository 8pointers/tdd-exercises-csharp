using System;

namespace roulette
{
    public class Field
    {
        public static Field ODD = new Field {FieldName = "ODD", WinningStrategy = new OddEvenWinningStrategy(true)};
        public static Field EVEN = new Field {FieldName = "EVEN", WinningStrategy = new OddEvenWinningStrategy(false)};

        public string FieldName { get; internal set; }
        public WinningStrategy WinningStrategy { get; internal set; }

        internal Field()
        {
        }

        public static Field ForNumber(int number)
        {
            return new Field {FieldName = number.ToString(), WinningStrategy = new SingleNumberWinningStrategy(number)};
        }

        private bool Equals(Field other)
        {
            return FieldName == other.FieldName && Equals(WinningStrategy, other.WinningStrategy);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Field) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FieldName, WinningStrategy);
        }
    }
}

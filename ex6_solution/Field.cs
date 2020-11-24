using System;

namespace roulette
{
    public class Field
    {
        public String FieldName { get; internal set; }
        public WinningStrategy WinningStrategy { get; internal set; }

        internal Field()
        {
        }

        public static Field ForNumber(int number)
        {
            return new Field {FieldName = number.ToString(), WinningStrategy = new SingleNumberWinningStrategy(number)};
        }

        public static Field ODD = new Field {FieldName = "ODD", WinningStrategy = new OddEvenWinningStrategy(true)};
        public static Field EVEN = new Field {FieldName = "EVEN", WinningStrategy = new OddEvenWinningStrategy(false)};

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Field)) return false;
            return Equals((Field) obj);
        }

        public bool Equals(Field obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.FieldName == this.FieldName && Equals(this.WinningStrategy, obj.WinningStrategy);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((FieldName != null ? FieldName.GetHashCode() : 0)) +
                       ((WinningStrategy != null ? WinningStrategy.GetHashCode() : 0));
            }
        }
    }
}
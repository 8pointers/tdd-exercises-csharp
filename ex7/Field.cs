using System;

namespace roulette
{
    public class Field
    {
        public String FieldName { get; internal set; }
        public WinningStrategy WinningStrategy { get; internal set; }
        public int PayoutCoefficient { get; internal set; }

        internal Field()
        {
        }

        public static Field ForNumber(int number)
        {
            return new Field
            {
                FieldName = number.ToString(), WinningStrategy = new SingleNumberWinningStrategy(number),
                PayoutCoefficient = 36
            };
        }

        public static Field ODD = new Field
            {FieldName = "ODD", WinningStrategy = new OddEvenWinningStrategy(true), PayoutCoefficient = 2};

        public static Field EVEN = new Field
            {FieldName = "EVEN", WinningStrategy = new OddEvenWinningStrategy(false), PayoutCoefficient = 2};

        public static Field RED = new Field
            {FieldName = "RED", WinningStrategy = new RedBlackWinningStrategy(true), PayoutCoefficient = 2};

        public static Field BLACK = new Field
            {FieldName = "BLACK", WinningStrategy = new RedBlackWinningStrategy(false), PayoutCoefficient = 2};

        public static Field ONE_EIGHTEEN = new Field
            {FieldName = "1-18", WinningStrategy = new HalfWinningStrategy(1), PayoutCoefficient = 2};

        public static Field NINETEEN_THIRTYSIX = new Field
            {FieldName = "19-36", WinningStrategy = new HalfWinningStrategy(2), PayoutCoefficient = 2};

        public static Field ONE_TWELVE = new Field
            {FieldName = "1-12", WinningStrategy = new ThirdWinningStrategy(1), PayoutCoefficient = 3};

        public static Field THIRTEEN_TWENTYFOUR = new Field
            {FieldName = "13-24", WinningStrategy = new ThirdWinningStrategy(2), PayoutCoefficient = 3};

        public static Field TWENTYFIVE_THIRTYSIX = new Field
            {FieldName = "25-36", WinningStrategy = new ThirdWinningStrategy(3), PayoutCoefficient = 3};

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
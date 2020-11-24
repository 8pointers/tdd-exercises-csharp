using System;

namespace roulette
{
    public class Field
    {
        public String FieldName { get; private set; }

        public static Field ForNumber(int number)
        {
            return new Field {FieldName = number.ToString()};
        }

        public static readonly Field ODD = new Field {FieldName = "ODD"};

        private bool Equals(Field other)
        {
            return FieldName == other.FieldName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Field) obj);
        }

        public override int GetHashCode()
        {
            return FieldName != null ? FieldName.GetHashCode() : 0;
        }
    }
}

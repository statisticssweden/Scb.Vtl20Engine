using System;
using System.Globalization;

namespace VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes
{
    public class NumberType : BasicScalarType<decimal?>
    {
        public NumberType(decimal? value) => Value = value;
        public static implicit operator decimal? (NumberType b) => b.Value;
        public static implicit operator NumberType(decimal? b) => new NumberType(b);
        public static NumberType operator +(NumberType a, NumberType b) => new NumberType(a.Value + b.Value);
        public static NumberType operator -(NumberType a, NumberType b) => new NumberType(a.Value - b.Value);
        public static NumberType operator *(NumberType a, NumberType b) => new NumberType(a.Value * b.Value);
        public static NumberType operator /(NumberType a, NumberType b) => new NumberType(a.Value / b.Value);
        public override ScalarType Clone()
        {
            return new NumberType(Value);
        }

        public override int CompareTo(object obj)
        {
            var number = ((NumberType)obj).Value;
            if (number == null && Value == null)
            {
                return 0;
            }
            if (number == null)
            {
                return 1;
            }
            if (Value == null)
            {
                return -1;
            }
            return decimal.Compare(Value.Value, number.Value);
        }

        public override string ToString()
        {
            if(Value.HasValue)
                return Value.Value.ToString("0.##############################", new CultureInfo("en-US"));
            return "Null";
        }
    }
}

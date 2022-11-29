using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;

namespace VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes
{
    public class IntegerType : NumberType
    {
        public IntegerType(long? value) : base(value) { }
        public static implicit operator long? (IntegerType b) => (long?)b.Value;
        public static implicit operator IntegerType(long? value) => new IntegerType(value);
        public static IntegerType operator +(IntegerType a, IntegerType b) => new IntegerType((long?)a + (long?)b);
        public static IntegerType operator -(IntegerType a, IntegerType b) => new IntegerType((long?)a - (long?)b);
        public static IntegerType operator *(IntegerType a, IntegerType b) => new IntegerType((long?)a * (long?)b);

        public override ScalarType Clone()
        {
            return new IntegerType((long?)Value);
        }

        public override string ToString()
        {
            if (Value.HasValue)
                return Value.Value.ToString();
            return "Null";
        }
    }
}

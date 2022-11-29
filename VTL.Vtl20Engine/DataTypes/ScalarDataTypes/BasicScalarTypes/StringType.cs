using System;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;

namespace VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes
{
    public class StringType : BasicScalarType<string>
    {
        public StringType(string value) => Value = value;
        public static implicit operator string(StringType b) => b.Value;
        public static implicit operator StringType(string b) => new StringType(b);

        public override ScalarType Clone()
        {
            return new StringType(Value.Clone() as string);
        }

        public override int CompareTo(object obj)
        {
            return StringComparer.Ordinal.Compare(Value, (StringType)obj);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
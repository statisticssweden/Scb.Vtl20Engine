using System.Collections.Generic;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes;

namespace VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes
{
    public abstract class BasicScalarType<T> : ScalarType
    {
        protected T Value;

        public override string ToString()
        {
            if (Value == null) return "null";
            return Value.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is BasicScalarType<T> scalar)
            {
                if (scalar.Value == null) return this.Value == null;
                return scalar.Value.Equals(Value);
            }
            return false;
        }

        protected bool Equals(BasicScalarType<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Value);
        }

        public override bool HasValue()
        {
            return Value != null;
        }

    }
}

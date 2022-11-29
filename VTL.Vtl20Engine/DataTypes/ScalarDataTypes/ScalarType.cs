using System;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;

namespace VTL.Vtl20Engine.DataTypes.ScalarDataTypes
{
    public abstract class ScalarType : DataType, IComparable
    {
        public abstract ScalarType Clone();

        public abstract bool HasValue();

        public abstract int CompareTo(object obj);

        public static bool operator >(ScalarType s1, ScalarType s2)
        {
            return s1.CompareTo(s2) > 0;
        }

        public static bool operator <(ScalarType s1, ScalarType s2)
        {
            return s1.CompareTo(s2) < 0;
        }

        public static bool operator >=(ScalarType s1, ScalarType s2)
        {
            return s1.CompareTo(s2) >= 0;
        }

        public static bool operator <=(ScalarType s1, ScalarType s2)
        {
            return s1.CompareTo(s2) <= 0;
        }

    }
}

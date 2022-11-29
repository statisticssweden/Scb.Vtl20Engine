using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.ScalarDataTypes.SetScalarTypes
{
    public class SetScalarType<T> : ScalarType where T : ScalarType
    {
        public T[] Values { get; set; }

        public SetScalarType(T[] values)
        {
            Values = values;
        }

        public override ScalarType Clone()
        {
            var clone = new T[Values.Length];
            for (int i = 0; i < Values.Length; i++)
            {
                clone[i] = (T)Values[i].Clone();
            }
            return new SetScalarType<T>(clone);
        }

        public override bool HasValue()
        {
            return Values != null && Values.Any();
        }

        public override int CompareTo(object obj)
        {
            // om this är större än obj -> 1

            // om obj är större än this -> -1

            throw new NotImplementedException();
        }

        internal BooleanType Contains(T val)
        {
            return Values.Contains(val);
        }
    }
}

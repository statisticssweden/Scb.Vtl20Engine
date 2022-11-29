using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes
{
    public class DataPointType : CompoundType, IEnumerable<ScalarType>, ICloneable
    {
        public DataPointType()
        { }

        public DataPointType(ScalarType[] value)
        {
            _value = value;
        }

        public DataPointType(int size)
        {
            _value = new ScalarType[size];
        }

        public DataPointType(IEnumerable<ScalarType> datum)
        {
            _value = datum.ToArray();
        }

        private readonly ScalarType[] _value;

        public ScalarType this[int i]
        {
            get => _value[i];
            set => _value[i] = value;
        }

        public IEnumerator<ScalarType> GetEnumerator()
        {
            foreach (var scalarType in _value)
            {
                yield return scalarType;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public DataPointType Transform(ComponentType[] fromStructure, ComponentType[] toStructure)
        {
            var newValue = new ScalarType[toStructure.Length];
            for (int i = 0; i < toStructure.Length; i++)
            {
                var fromComp = fromStructure.FirstOrDefault(c => c.Name.Equals(toStructure[i].Name));
                if (fromComp != null)
                {
                    newValue[i] = _value[Array.IndexOf(fromStructure, fromComp)];
                }
                else
                {
                    if(toStructure[i].DataType == typeof(IntegerType))
                            newValue[i] = new IntegerType(null);
                    else if (toStructure[i].DataType == typeof(NumberType))
                            newValue[i] = new NumberType(null);
                    else if(toStructure[i].DataType == typeof(StringType))
                            newValue[i] = new StringType(null);
                    else if(toStructure[i].DataType == typeof(BooleanType))
                            newValue[i] = new BooleanType(null);
                }
            }

            return new DataPointType(newValue);
        }

        public object Clone()
        {
            var clone = new DataPointType(_value.Length);
            for(int i = 0; i < _value.Length; i++)
            {
                clone[i] = _value[i].Clone();
            }
            return clone;
        }
    }
}

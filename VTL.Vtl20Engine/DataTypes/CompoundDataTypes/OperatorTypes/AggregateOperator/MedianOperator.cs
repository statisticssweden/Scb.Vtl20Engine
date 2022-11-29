using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.AggregateOperator
{
    public class MedianOperator : AggregateOperator
    {
        public MedianOperator(Operand opernad)
        {
            Operand = opernad;
        }

        internal override ScalarType PerformCalculation(List<ScalarType> scalars)
        {
            var numbers = scalars.OfType<NumberType>();
            var notNull = numbers.Where(n => n.HasValue()).ToList();
            if(!notNull.Any()) return new NumberType(null);
            notNull.Sort();
            if (notNull.Count % 2 == 1)
            {
                return notNull[notNull.Count / 2];
            }
            else
            {
                return (notNull[notNull.Count / 2 - 1] + notNull[notNull.Count / 2]) /2;
            }
        }

        protected override bool CompatibleDataType(Type dataType)
        {
            return dataType == typeof(IntegerType) || dataType == typeof(NumberType);
        }

        protected override Type GetResultMeasureDatatype(ComponentType component)
        {
            if (component.DataType == typeof(IntegerType))
            {
                return typeof(NumberType);
            }

            return component.DataType;
        }
    }
}

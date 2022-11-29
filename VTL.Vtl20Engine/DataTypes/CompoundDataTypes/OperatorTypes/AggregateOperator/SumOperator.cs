using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.AggregateOperator
{
    public class SumOperator : AggregateOperator
    {
        public SumOperator(Operand opernad)
        {
            Operand = opernad;
        }

        internal override ScalarType PerformCalculation(List<ScalarType> scalars)
        {
            var numbers = scalars.OfType<NumberType>().ToList();
            if (numbers.TrueForAll(n => n is IntegerType))
            {
                var sum = new IntegerType(0);
                foreach (var integer in numbers.OfType<IntegerType>())
                {
                    if (integer.HasValue())
                    {
                        sum += integer;
                    }
                }
                return sum;
            }
            else
            {
                var sum = new NumberType(0m);
                foreach (var number in numbers)
                {
                    if (number.HasValue())
                    {
                        sum += number;
                    }
                }
                return sum;
            }
        }

        protected override bool CompatibleDataType(Type dataType)
        {
            return dataType == typeof(IntegerType) || dataType == typeof(NumberType);
        }
    }
}

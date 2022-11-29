using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.AggregateOperator
{
    public class AverageOperator : AggregateOperator
    {
        public AverageOperator(Operand operand)
        {
            Operand = operand;
        }

        internal override ScalarType PerformCalculation(List<ScalarType> scalars)
        {
            var numbers = scalars.OfType<NumberType>();
            var sum = new NumberType(0m);
            int i = 0; 
            foreach (var number in numbers)
            {
                if (number.HasValue())
                {
                    sum += number;
                    i += 1;
                }
            }

            if (i == 0) return new NumberType(null);
            return sum/i;
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

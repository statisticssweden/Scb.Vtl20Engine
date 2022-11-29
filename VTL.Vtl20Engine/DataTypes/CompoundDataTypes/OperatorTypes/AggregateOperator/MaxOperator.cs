using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.AggregateOperator
{
    public class MaxOperator : AggregateOperator
    {
        public MaxOperator(Operand operand)
        {
            Operand = operand;
        }

        internal override ScalarType PerformCalculation(List<ScalarType> scalars)
        {
            var numbers = scalars.OfType<NumberType>();
            if (numbers.Any())
            {
                var max = new NumberType(null);
                foreach (var number in numbers)
                {
                    if (number.HasValue())
                    {
                        if (!max.HasValue() || number > max)
                        {
                            max = number;
                        }
                    }
                }
                return max;
            }

            var timePeriods = scalars.OfType<TimePeriodType>();
            if (timePeriods.Any())
            {
                var max = new TimePeriodType(1, Duration.Annual, 0);
                foreach (var timePeriod in timePeriods)
                {
                    if (timePeriod.HasValue())
                    {
                        if (!max.HasValue() || timePeriod > max)
                        {
                            max = timePeriod;
                        }
                    }
                }
                return max;
            }

            throw new NotImplementedException();
        }
    }
}
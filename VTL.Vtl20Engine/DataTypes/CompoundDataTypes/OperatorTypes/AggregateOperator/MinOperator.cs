using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.AggregateOperator
{
    public class MinOperator : AggregateOperator
    {
        public MinOperator(Operand operand)
        {
            Operand = operand;
        }

        internal override ScalarType PerformCalculation(List<ScalarType> scalars)
        {
            var numbers = scalars.OfType<NumberType>();
            if (numbers.Any())
            {
                var min = new NumberType(null);
                foreach (var number in numbers)
                {
                    if (number.HasValue())
                    {
                        if (!min.HasValue() || number < min)
                        {
                            min = number;
                        }
                    }
                }

                return min;
            }

            var timePeriods = scalars.OfType<TimePeriodType>();
            if (timePeriods.Any())
            {
                var min = new TimePeriodType(9999, Duration.Annual, 0);
                foreach (var timePeriod in timePeriods)
                {
                    if (timePeriod.HasValue())
                    {
                        if (!min.HasValue() || timePeriod < min)
                        {
                            min = timePeriod;
                        }
                    }
                }
                return min;
            }

            throw new NotImplementedException();
        }

    }
}
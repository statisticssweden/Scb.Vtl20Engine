using System.Collections;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ComparisonOperator
{
    public class LessThanOrEqualToOperator: BinaryComparisonOperator
    {
        public LessThanOrEqualToOperator(Operand operand1, Operand operand2)
        {
            Operand1 = operand1;
            Operand2 = operand2;
        }

        public override BooleanType PerformCalculation(NumberType number1, NumberType number2)
        {
            return number1 <= number2;
        }

        public override BooleanType PerformCalculation(StringType string1, StringType string2)
        {
            var compareTo = string1.CompareTo(string2);
            return compareTo <= 0;
        }

        public override BooleanType PerformCalculation(TimePeriodType timePeriod1, TimePeriodType timePeriod2)
        {
            var compareTo = timePeriod1.CompareTo(timePeriod2);
            return compareTo <= 0;
        }

        public override BooleanType PerformCalculation(BooleanType boolean1, BooleanType boolean2)
        {
            return boolean1 <= boolean2;
        }
    }
}

using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ComparisonOperator
{
    public class EqualToOperator : BinaryComparisonOperator
    {
        public EqualToOperator(Operand operand1, Operand operand2)
        {
            Operand1 = operand1;
            Operand2 = operand2;
        }

        public override BooleanType PerformCalculation(NumberType number1, NumberType number2)
        {
            return number1.Equals(number2);
        }

        public override BooleanType PerformCalculation(StringType string1, StringType string2)
        {
            return string1.Equals(string2);
        }

        public override BooleanType PerformCalculation(TimePeriodType timePeriod1, TimePeriodType timePeriod2)
        {
            return timePeriod1.Equals(timePeriod2);
        }

        public override BooleanType PerformCalculation(BooleanType boolean1, BooleanType boolean2)
        {
            return boolean1.Equals(boolean2);
        }

        public override BooleanType PerformNullCalculation(ScalarType scalar)
        {
            return !scalar.HasValue();
        }
    }
}
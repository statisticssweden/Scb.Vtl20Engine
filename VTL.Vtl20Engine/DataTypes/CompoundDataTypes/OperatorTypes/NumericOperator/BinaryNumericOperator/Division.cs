using System;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.NumericOperator.BinaryNumericOperator
{
    public class Division : BinaryNumericOperator
    {
        public Division(Operand operand1, Operand operand2)
        {
            Operand1 = operand1;
            Operand2 = operand2;
        }

        public override NumberType PerformCalculation(IntegerType integer1, IntegerType integer2)
        {
            if (integer2.Equals(new IntegerType(0))) throw new DivideByZeroException("Division med 0 är inte tillåtet!");
            return integer1 / integer2;
        }

        public override NumberType PerformCalculation(NumberType number1, NumberType number2)
        {
            if (number2.Equals(new NumberType(0))) throw new DivideByZeroException("Division med 0 är inte tillåtet!");
            return number1 / number2;
        }

        protected override Type GetResultType(Type component1DataType, Type component2DataType)
        {
            return typeof(NumberType);
        }
    }
}

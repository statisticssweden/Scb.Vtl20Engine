using System;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.NumericOperator.BinaryNumericOperator
{
    public class Addition : BinaryNumericOperator
    {

        public Addition(Operand operand1, Operand operand2)
        {
            Operand1 = operand1;
            Operand2 = operand2;
        }

        public override NumberType PerformCalculation(IntegerType integer1, IntegerType integer2)
        {
            return integer1 + integer2;
        }

        public override NumberType PerformCalculation(NumberType number1, NumberType number2)
        {
            return number1 + number2;
        }
    }
}

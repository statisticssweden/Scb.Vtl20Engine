using System;
using System.Collections.Generic;
using System.Text;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.NumericOperator.UnaryNumericOperator
{
    public class Abs : UnaryNumericOperator
    {
        public Abs(Operand op)
        {
            Operand = op;
        }

        public override NumberType PerformCalculation(IntegerType integer)
        {   
            if (integer < 0) { return new IntegerType(-integer); }
            return new IntegerType(integer);
        }

        public override NumberType PerformCalculation(NumberType number)
        {
            if (number < 0) { return new NumberType(-number); }
            return new NumberType(number);
        }
    }

}

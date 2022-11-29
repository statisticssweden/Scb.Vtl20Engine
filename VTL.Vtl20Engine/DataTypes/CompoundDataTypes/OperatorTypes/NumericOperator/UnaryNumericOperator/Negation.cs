using System;
using System.Collections.Generic;
using System.Text;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.NumericOperator.UnaryNumericOperator
{
    public class Negation : UnaryNumericOperator
    {
        public Negation(Operand op)
        {
            Operand = op;
        }

        public override NumberType PerformCalculation(IntegerType integer)
        {
            return new IntegerType(-integer);
        }

        public override NumberType PerformCalculation(NumberType number)
        {
            return new NumberType(-number);
        }
    }
}

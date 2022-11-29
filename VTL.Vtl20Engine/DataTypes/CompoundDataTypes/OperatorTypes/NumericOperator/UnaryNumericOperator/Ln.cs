using System;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.NumericOperator.UnaryNumericOperator
{
    public class Ln : UnaryNumericOperator
    {
        public Ln(Operand op)
        {
            Operand = op;
        }

        public override NumberType PerformCalculation(IntegerType integer)
        {
            if (!integer.HasValue())
            {
                return new NumberType(null);
            }

            var doubleArgument = Convert.ToDouble((int)integer);
            var doubleResult = Math.Log(doubleArgument);
            return new NumberType(Convert.ToDecimal(doubleResult));
        }

        public override NumberType PerformCalculation(NumberType number)
        {
            if(!number.HasValue())
            {
                return new NumberType(null);
            }

            var doubleArgument = Convert.ToDouble((decimal)number);
            var doubleResult = Math.Log(doubleArgument);
            return new NumberType(Convert.ToDecimal(doubleResult));
        }
    }
}

using System;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.NumericOperator.UnaryNumericOperator
{
    public class Sqrt : UnaryNumericOperator
    {
        public Sqrt(Operand op)
        {
            Operand = op;
        }

        public override NumberType PerformCalculation(IntegerType integer)
        {

            if (!integer.HasValue()) return new NumberType(null);
            if (integer < 0) throw new Exception("Man kan inte dra kvadratroten ur ett negativt tal");
            var result = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble((int)integer)));
            return new NumberType(result);
        }

        public override NumberType PerformCalculation(NumberType number)
        {
            if (!number.HasValue()) return new NumberType(null);
            if (number < 0) throw new Exception("Man kan inte dra kvadratroten ur ett negativt tal");
            var result = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble((decimal)number)));
            return new NumberType(result);
        }
    }

}

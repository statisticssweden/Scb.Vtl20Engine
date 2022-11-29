using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.BooleanOperator.BinaryBooleanOperator
{
    public class Xor : BinaryBooleanOperator
    {
        public Xor(Operand operand1, Operand operand2)
        {
            Operand1 = operand1;
            Operand2 = operand2;
        }

        public override BooleanType PerformCalculation(BooleanType boolean1, BooleanType boolean2)
        {
            if (!boolean1.HasValue() || !boolean2.HasValue())
            {
                return new BooleanType(null);
            }

            return new BooleanType((bool)boolean1 ^ (bool)boolean2);
        }
    }
}

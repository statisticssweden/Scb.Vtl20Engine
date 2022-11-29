using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.BooleanOperator.BinaryBooleanOperator
{
    public class Or : BinaryBooleanOperator
    {
        public Or(Operand operand1, Operand operand2)
        {
            Operand1 = operand1;
            Operand2 = operand2;
        }

        public override BooleanType PerformCalculation(BooleanType boolean1, BooleanType boolean2)
        {
            if (boolean1.HasValue() && (bool)boolean1) return true;
            if (boolean2.HasValue() && (bool)boolean2) return true;
 
            if (boolean1.HasValue() && boolean2.HasValue())
            {
                return new BooleanType(false);
            }
            return new BooleanType(null);
        }
    }
}

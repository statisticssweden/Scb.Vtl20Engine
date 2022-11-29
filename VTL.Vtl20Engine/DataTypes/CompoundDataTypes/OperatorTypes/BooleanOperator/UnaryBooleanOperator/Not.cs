using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.BooleanOperator.UnaryBooleanOperator
{
    public class Not : UnaryBooleanOperator
    {
        public Not(Operand operand)
        {
            Operand = operand;
        }

        public override BooleanType PerformCalculation(BooleanType boolean)
        {
            return !boolean;
        }
    }
}

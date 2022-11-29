using System.Collections.Generic;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.SetOperator
{
    public class SetDiff : SetOperator
    {
        public SetDiff(List<Operand> operands)
        {
            Operands = operands;
        }

        internal override DataPointType PerformCalculation(DataPointType dp1, DataPointType dp2, DataPointComparer dataPointComparer)
        {
            if (dp1 == null)
            {
                return null;
            }
            if (dp2 == null)
            {
                return dp1;
            }
            var c = dataPointComparer.Compare(dp1, dp2);
            return c < 0 ? dp1 : null;
        }
    }
}

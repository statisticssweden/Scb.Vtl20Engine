using System.Collections.Generic;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.SetOperator
{
    public class SymDiff : SetOperator
    {
        public SymDiff(List<Operand> operands)
        {
            Operands = operands;
        }

        internal override DataPointType PerformCalculation(DataPointType dp1, DataPointType dp2, DataPointComparer dataPointComparer)
        {
            if (dp1 == null)
            {
                return dp2;
            }
            if (dp2 == null)
            {
                return dp1;
            }
            var c = dataPointComparer.Compare(dp1, dp2);
            if (c < 0)
            {
                return dp1;
            }

            if (c > 0)
            {
                return dp2;
            }

            return null;
        }
    }
}

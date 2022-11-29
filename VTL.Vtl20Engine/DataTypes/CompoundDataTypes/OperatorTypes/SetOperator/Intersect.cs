using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.SetOperator
{
    public class Intersect: SetOperator
    {
        public Intersect(List<Operand> operands)
        {
            Operands = operands;
        }

        internal override DataPointType PerformCalculation(DataPointType dp1, DataPointType dp2, DataPointComparer dataPointComparer)
        {

            if (dp1 == null || dp2 == null) return null;
            var c = dataPointComparer.Compare(dp1, dp2);
            if (c == 0)
            {
                return dp1;
            }
            return null;
        } 
    }
}
 
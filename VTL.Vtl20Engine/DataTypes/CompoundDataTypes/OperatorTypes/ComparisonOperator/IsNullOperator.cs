using System;
using System.Collections.Generic;
using System.Text;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ComparisonOperator
{
    class IsNullOperator : UnaryComparisonOperator
    {

        public IsNullOperator(Operand operand)
        {
            _operand =  operand;
            if (operand.GetMeasureNames().Length > 1 )
            {
                throw new Exception("Operatorn isnull kan inte användas på datasetnivå om datasetet har mer än en measurekomponent.");
            }
        }

        protected override BooleanType PerformCalculation(ScalarType scalar)
        {
            return !scalar.HasValue();
        }
    }
}

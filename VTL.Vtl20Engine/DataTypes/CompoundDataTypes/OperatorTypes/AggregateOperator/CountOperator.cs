using System;
using System.Collections.Generic;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.AggregateOperator
{
    public class CountOperator : AggregateOperator
    {
        public CountOperator(Operand operand)
        {
            Operand = operand;
        }

        internal override ScalarType PerformCalculation(List<ScalarType> scalars)
        {
            return new IntegerType(scalars.Count);
        }

        protected override Type GetResultMeasureDatatype(ComponentType component)
        {
            return typeof(IntegerType);
        }
    }
}
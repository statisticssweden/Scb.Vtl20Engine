using System;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.BooleanOperator.BinaryBooleanOperator
{
    public abstract class BinaryBooleanOperator : BinaryOperator
    {
        public abstract BooleanType PerformCalculation(BooleanType boolean1, BooleanType boolean2);

        public override ScalarType PerformCalculation(ScalarType scalar1, ScalarType scalar2)
        {
            if (scalar1 is BooleanType boolean1 && scalar2 is BooleanType boolean2)
            {
                return PerformCalculation(boolean1, boolean2);
            }
            throw new NotImplementedException();
        }

        internal override bool CompatibleDataType(Type dataType)
        {
            return dataType == typeof(BooleanType);
        }
    }
}

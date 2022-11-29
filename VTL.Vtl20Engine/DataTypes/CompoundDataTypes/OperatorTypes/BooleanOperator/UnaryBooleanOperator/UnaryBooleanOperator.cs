using System;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.BooleanOperator.UnaryBooleanOperator
{
    public abstract class UnaryBooleanOperator : UnaryOperator
    {
        public abstract BooleanType PerformCalculation(BooleanType boolean);

        public override ScalarType PerformCalculation(ScalarType scalar)
        {
            if (scalar == null)
            {
                return new BooleanType(null);
            }
            if (scalar is BooleanType boolean)
            {
                return PerformCalculation(boolean);
            }
            throw new NotImplementedException();
        }

        internal override bool CompatibleDataType(Type dataType)
        {
            return dataType == typeof(BooleanType);
        }
    }
}

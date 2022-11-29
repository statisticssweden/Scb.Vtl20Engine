using System;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.NumericOperator.UnaryNumericOperator
{
    public abstract class UnaryNumericOperator : UnaryOperator
    {
        public abstract NumberType PerformCalculation(IntegerType integer);

        public abstract NumberType PerformCalculation(NumberType number);

        public override ScalarType PerformCalculation(ScalarType scalar)
        {
            if (scalar == null)
            {
                return new IntegerType(null);
            }
            if (scalar is IntegerType inte)
            {
                return PerformCalculation(inte);
            }
            if (scalar is NumberType num)
            {
                return PerformCalculation(num);
            }
            throw new NotImplementedException();
        }

        internal override bool CompatibleDataType(Type dataType)
        {
            return dataType == typeof(IntegerType) ||
                   dataType == typeof(NumberType);
        }
    }
}

using System;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.NumericOperator.BinaryNumericOperator
{
    public abstract class BinaryNumericOperator : BinaryOperator
    {

        public abstract NumberType PerformCalculation(IntegerType integer1, IntegerType integer2);
        public abstract NumberType PerformCalculation(NumberType number1, NumberType number2);

        public override ScalarType PerformCalculation(ScalarType scalar1, ScalarType scalar2)
        {
            if (scalar1 == null || scalar1 is NullType || scalar2 == null || scalar2 is NullType)
            {
                return new IntegerType(null);
            }
            if (scalar1 is IntegerType int1 && scalar2 is IntegerType int2)
            {
                return PerformCalculation(int1, int2);
            }
            if (scalar1 is NumberType num1 && scalar2 is NumberType num2)
            {
                return PerformCalculation(num1, num2);
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

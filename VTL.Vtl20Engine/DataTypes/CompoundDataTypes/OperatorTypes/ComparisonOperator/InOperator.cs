using System;
using System.Collections.Generic;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.SetScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ComparisonOperator
{
    public class InOperator : NaryComparisonOperator
    {
        private readonly Operand _setOperand;

        public InOperator(Operand operand, Operand setOperand)
        {
            _operand = operand;
            _setOperand = setOperand;
        }
        
        protected override BooleanType PerformCalculation(ScalarType scalarType)
        {
            var set = _setOperand.GetValue();
            if (set is SetScalarType<IntegerType> integerSet && scalarType is IntegerType integerScalar)
            {
                return integerSet.Contains(integerScalar);
            }

            if (set is SetScalarType<IntegerType> intSet && scalarType is NumberType numericScalar)
            {
                var intSetValues = new List<NumberType>();
                foreach (var intSetValue in intSet.Values)
                {
                    intSetValues.Add(intSetValue);
                }
                return intSetValues.Contains(numericScalar);
            }

            if (set is SetScalarType<NumberType> numberSet && scalarType is NumberType numberScalar)
            {
                return numberSet.Contains(numberScalar);
            }

            if (set is SetScalarType<StringType> stringSet && scalarType is StringType stringScalar)
            {
                return stringSet.Contains(stringScalar);
            }

            throw new Exception();
        }
        
    }
}

using System;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.GroupingOperator
{
    public class GroupByNoneOperator : GroupingOperator
    {
        private readonly Operand _operand;

        public GroupByNoneOperator(Operand operand)
        {
            _operand = operand;

        }

        internal override DataType PerformCalculation()
        {
            if (_operand.GetValue() is DataSetType dataSet)
            {
                return PerformGrouping(dataSet, new string[0]);
            }
            else
            {
                throw new Exception("Group by kan bara utföras på dataset.");
            }
        }

        internal override string[] GetComponentNames()
        {
            return _operand.GetMeasureNames().ToArray();
        }

        internal override string[] GetIdentifierNames()
        {
            return new string[0];
        }

        internal override string[] GetMeasureNames()
        {
            return _operand.GetMeasureNames().ToArray();
        }
    }
}
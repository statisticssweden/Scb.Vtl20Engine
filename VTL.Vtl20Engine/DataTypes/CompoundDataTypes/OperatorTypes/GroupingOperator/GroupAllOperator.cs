using System;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.Exceptions;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.GroupingOperator
{
    public class GroupAllOperator : Operator
    {
        private readonly Operand _operand;
        private readonly string[] _groupExceptComponentNames;

        public GroupAllOperator(Operand operand)
        {
            _operand = operand;
        }

        internal override DataType PerformCalculation()
        {
            if (_operand.GetValue() is DataSetType dataSet)
            {
                return dataSet;
            }
            else
            {
                throw new Exception("Group All kan bara utföras på dataset.");
            }
        }

        internal override string[] GetComponentNames()
        {
            return _operand.GetComponentNames();
        }

        internal override string[] GetIdentifierNames()
        {
            return _operand.GetIdentifierNames();
        }

        internal override string[] GetMeasureNames()
        {
            return _operand.GetMeasureNames();
        }
    }
}
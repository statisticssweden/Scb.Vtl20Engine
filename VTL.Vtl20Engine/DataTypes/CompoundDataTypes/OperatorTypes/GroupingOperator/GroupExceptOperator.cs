using System;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.Exceptions;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.GroupingOperator
{
    public class GroupExceptOperator : GroupingOperator
    {
        private readonly Operand _operand;
        private readonly string[] _groupExceptComponentNames;

        public GroupExceptOperator(Operand operand, string[] groupExceptComponentNames)
        {
            _operand = operand;
            _groupExceptComponentNames = groupExceptComponentNames;

            foreach (var groupExceptComponentName in groupExceptComponentNames)
            {
                try
                {
                    if (!operand.GetIdentifierNames().Contains(groupExceptComponentName))
                    {
                        throw new Exception($"Dataset kan inte skapas då identifierkomponent med namn {groupExceptComponentName} saknas.");
                    }
                }
                catch (OverrideValidationException)
                {
                    //Ignore exception for eval operator.getIndentifiers
                }

            }
        }

        internal override DataType PerformCalculation()
        {
            if (_operand.GetValue() is DataSetType dataSet)
            {
                var keptComponentNames = dataSet.DataSetComponents
                    .Where(c => c.Role == ComponentType.ComponentRole.Identifier &&
                                !_groupExceptComponentNames.Contains(c.Name)).Select(c => c.Name).ToArray();
                return PerformGrouping(dataSet, keptComponentNames);
            }
            else
            {
                throw new Exception("Group Except kan bara utföras på dataset.");
            }
        }

        internal override string[] GetComponentNames()
        {
            return _operand.GetComponentNames().Except(_groupExceptComponentNames).ToArray();
        }

        internal override string[] GetIdentifierNames()
        {
            return _operand.GetIdentifierNames().Except(_groupExceptComponentNames).ToArray();
        }

        internal override string[] GetMeasureNames()
        {
            return _operand.GetMeasureNames().ToArray();
        }
    }
}
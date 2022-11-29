using System;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.Exceptions;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.GroupingOperator
{
    public class GroupByOperator : GroupingOperator
    {
        private readonly Operand _operand;
        private readonly string[] _groupByComponentNames;

        public GroupByOperator(Operand operand, string[] groupByComponentNames)
        {
            _operand = operand;
            _groupByComponentNames = groupByComponentNames;

            foreach (var groupExceptComponentName in groupByComponentNames)
            {
                try
                {
                    if (!operand.GetIdentifierNames().Contains(groupExceptComponentName))
                    {
                        throw new Exception(
                            $"Dataset kan inte skapas då identifierkomponent med namn {groupExceptComponentName} saknas.");
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
                return PerformGrouping(dataSet, _groupByComponentNames);
            }
            else
            {
                throw new Exception("Group by kan bara utföras på dataset.");
            }
        }

        internal override string[] GetComponentNames()
        {
            var measures = _operand.GetMeasureNames().ToArray();
            var identifiers = new string[_groupByComponentNames.Length];
            Array.Copy(_groupByComponentNames, identifiers, _groupByComponentNames.Length);
            return measures.Concat(identifiers).ToArray();
        }

        internal override string[] GetIdentifierNames()
        {
            var identifiers = new string[_groupByComponentNames.Length];
            Array.Copy(_groupByComponentNames, identifiers, _groupByComponentNames.Length);
            return identifiers;
        }

        internal override string[] GetMeasureNames()
        {
            return _operand.GetMeasureNames().ToArray();
        }
    }
}
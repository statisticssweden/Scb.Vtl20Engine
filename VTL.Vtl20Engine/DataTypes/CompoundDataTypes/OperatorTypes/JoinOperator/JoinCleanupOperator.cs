using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.JoinOperator
{
    public class JoinCleanupOperator : Operator
    {
        private readonly Operand _operand;

        public JoinCleanupOperator(Operand operand)
        {
            _operand = operand;
        }

        internal override DataType PerformCalculation()
        {
            var result = _operand.GetValue();
            if (result is DataSetType dataSet)
            {
                var duplicates = dataSet.DataSetComponents.Select(c => c.Name.Contains('#') ? c.Name.Substring(c.Name.IndexOf("#") + 1) : c.Name).
                    GroupBy(c => c, StringComparer.OrdinalIgnoreCase).Where(c => c.Count() > 1).Select(n => n.Key).ToList();
                if (duplicates.Any())
                {
                    throw new Exception($"Resultatet av join innehåller flera komponenter med namn {string.Join(", ", duplicates)}");
                }

                foreach (var dataSetComponent in dataSet.DataSetComponents)
                {
                    if (dataSetComponent.Name.Contains('#'))
                    {
                        var newName = dataSetComponent.Name.Substring(dataSetComponent.Name.IndexOf("#") + 1);
                        if (dataSet.ComponentSortOrder.Contains(newName, StringComparer.OrdinalIgnoreCase))
                        {
                            throw new Exception($"Kan inte slutföra joinkommandot eftersom det skulle resultera i två komponenter med namnet {newName}.");
                        }
                        dataSet.RenameComponent(dataSetComponent.Name, newName);
                    }
                }
            }
            return result;
        }

        internal override string[] GetComponentNames()
        {
            return _operand.GetComponentNames().Select(c => c.Substring(c.IndexOf("#") + 1)).ToArray();
        }

        internal override string[] GetIdentifierNames()
        {
            return _operand.GetIdentifierNames().Select(c => c.Substring(c.IndexOf("#") + 1)).ToArray();
        }

        internal override string[] GetMeasureNames()
        {
            return _operand.GetMeasureNames().Select(c => c.Substring(c.IndexOf("#") + 1)).ToArray();
        }
    }
}

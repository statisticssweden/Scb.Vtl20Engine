using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes
{
    public class GetComponentOperator : Operator
    {
        private readonly Operand _operand;
        private readonly string _componentName;

        public GetComponentOperator(Operand operand, string componentName)
        {
            _operand = operand;
            _componentName = componentName;
        }


        internal override DataType PerformCalculation()
        {
            if (_operand.GetValue() is DataSetType dataSet)
            {
                dataSet.SortComponents();
                dataSet.SortDataPoints();
                return dataSet.DataSetComponents.FirstOrDefault(c => c.Name.Equals(_componentName)) ??
                       dataSet.DataSetComponents.FirstOrDefault(c => c.Name.Substring(c.Name.IndexOf("#") + 1).Equals(_componentName));
            }

            if (_operand.GetValue() is ComponentType component)
            {
                return component;
            }

            return null;
        }

        internal override string[] GetComponentNames()
        {
            return new[] {_componentName};
        }

        internal override string[] GetIdentifierNames()
        {
            var identifiers = _operand.GetIdentifierNames().Where(i => i.Equals(_componentName)).ToArray();
            return identifiers.Any() ? identifiers.ToArray() : new string[0];
        }

        internal override string[] GetMeasureNames()
        {
            var measures = _operand.GetMeasureNames().Where(i => i.Equals(_componentName)).ToArray();
            return measures.Any() ? measures.ToArray() : new string[0];
        }
    }
}

using System;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.GeneralPurposeOperator
{
    internal class MembershipOperator : Operator
    {
        private readonly Operand _operand;
        private readonly string _componentName;

        public MembershipOperator(Operand operand, string componentName)
        {
            _operand = operand;
            _componentName = componentName;
        }

        internal override DataType PerformCalculation()
        {
            var value = _operand.GetValue();
            if (value == null) throw new Exception($"{_operand.Alias} kändes inte igen.");
            var dataSet = value as DataSetType;
            if (dataSet == null) throw new Exception("Operatorn # kan endast utföras på dataset.");
            var original = dataSet.DataSetComponents.FirstOrDefault(c => c.Name.Equals(_componentName)) ??
                           dataSet.DataSetComponents.FirstOrDefault(c => c.Name.Substring(c.Name.IndexOf("#") + 1).Equals(_componentName));
            if(original == null) throw new Exception($"Komponenten {_componentName} hittades inte i datasetet {_operand.Alias}.");
            var component = new ComponentType(original);
            var componentList = dataSet.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier).ToList();

            if (original.Role == ComponentType.ComponentRole.Identifier)
            {
                if (component.DataType == typeof(StringType))
                {
                    component.Name = "string_var";
                }
                if (component.DataType == typeof(NumberType))
                {
                    component.Name = "num_var";
                }
                if (component.DataType == typeof(IntegerType))
                {
                    component.Name = "int_var";
                }
                if (component.DataType == typeof(DateType))
                {
                    component.Name = "date_var";
                }
                if (component.DataType == typeof(BooleanType))
                {
                    component.Name = "bool_var";
                }
            }
            if (component.DataType == typeof(BooleanType))
            {
                component.Name = "condition";
            }
            component.Role = ComponentType.ComponentRole.Measure;
            componentList.Add(component);
            
            var result = new DataSetType(componentList.ToArray());
            var componentCount = componentList.Count;
            foreach (var dataPoint in dataSet.DataPoints)
            {

                var newDataPoint = new DataPointType(componentCount);
                for (var i = 0; i < componentCount; i++)
                {
                    newDataPoint[result.OriginalIndexOfComponent(componentList[i].Name)] =
                        i == componentCount - 1 ?
                        dataPoint[dataSet.OriginalIndexOfComponent(_componentName)] :
                        dataPoint[dataSet.OriginalIndexOfComponent(componentList[i].Name)];
                }
                result.Add(newDataPoint);
            }

            return result;
        }

        internal override string[] GetComponentNames()
        {
            // Denna går tyvärr inte att köra korrekt utan att först utföra beräkningen
            // eftersom den är beroende av resultatets datatyp
            var componentNames = _operand.GetIdentifierNames();
            if(_operand.GetIdentifierNames().Contains(_componentName))
                return componentNames.Append("string_var").ToArray();
            return componentNames.Append(_componentName).ToArray();
        }

        internal override string[] GetIdentifierNames()
        {
            return _operand.GetIdentifierNames();
        }

        internal override string[] GetMeasureNames()
        {
            // Denna går tyvärr inte att köra korrekt utan att först utföra beräkningen
            // eftersom den är beroende av resultatets datatyp
            if (_operand.GetIdentifierNames().Contains(_componentName))
                return new []{"string_var"};
            return new []{_componentName};
        }
    }
}
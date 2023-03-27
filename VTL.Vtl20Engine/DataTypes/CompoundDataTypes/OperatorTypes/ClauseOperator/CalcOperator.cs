using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ClauseOperator
{
    public class CalcOperator : Operator
    {
        protected Operand InOperand;
        protected Operand[] ComponentOperands;

        public CalcOperator(Operand inOperand, Operand[] componentOperands)
        {
            InOperand = inOperand;
            ComponentOperands = componentOperands;

            foreach (var componentOperand in componentOperands)
            {
                if (inOperand.GetIdentifierNames().Contains(componentOperand.Alias))
                {
                    throw new Exception($"Identifier {componentOperand.Alias} får ej uppdateras.");
                }
                if (componentOperand.Alias.Length > Constants.MAX_COMPONENT_NAME_LENGTH)
                {
                    throw new Exception($"{componentOperand.Alias} är för långt. Komponentnamn får inte vara längre än {Constants.MAX_COMPONENT_NAME_LENGTH} tecken.");
                }

            }
        }

        internal override DataType PerformCalculation()
        {

            var inOperands = new List<Operand> { InOperand };
            foreach(var c in ComponentOperands)
            {
                inOperands.Add(c);
            }

            var inValues = inOperands.AsParallel().Select(x => x.GetValue()).ToArray();

            var dataSet = inValues[0] as DataSetType;

            if (dataSet == null)
            {
                throw new Exception("Calc kan bara utföras på dataset.");
            }
            for (var i = 0; i < ComponentOperands.Length; i++)
            {
                ComponentType component = null;
                var alias = ComponentOperands[i].Alias;
                var value = inValues[i + 1];

                if (value is ScalarType scalartType)
                {
                    component = new ComponentType(value.GetType(), new ConstantComponentContainer(scalartType));
                }

                if (value is ComponentType componentType)
                {
                    component = componentType;
                }

                if (value is DataSetType dataSetType)
                {
                    if (dataSetType.DataSetComponents.Count(c => c.Role == ComponentType.ComponentRole.Measure) != 1)
                    {
                        var valueComponents = new List<ComponentType>();
                        foreach (var componentName in dataSet.DataSetComponents.Select(c => c.Name))
                        {
                            var valueComponent =
                                dataSetType.DataSetComponents.FirstOrDefault(c => c.Name.Equals(componentName));
                            if (valueComponent != null)
                            {
                                valueComponents.Add(valueComponent);
                            }
                        }

                        dataSetType.DataSetComponents = valueComponents.ToArray();
                    }

                    dataSetType.SortDataPoints();

                    if (dataSetType.DataSetComponents.Any(c => c.Name.Equals(alias)))
                    {
                        component = dataSetType.DataSetComponents.FirstOrDefault(c =>
                            c.Name.Equals(alias));
                    }
                    else if (dataSetType.DataSetComponents.Any(c =>
                            c.Name.Substring(c.Name.IndexOf("#") + 1).Equals(alias)))
                    {
                        component = dataSetType.DataSetComponents.FirstOrDefault(c =>
                            c.Name.Substring(c.Name.IndexOf("#") + 1).Equals(alias));
                    }
                    else if (dataSetType.DataSetComponents.Count(c =>
                            c.Role == ComponentType.ComponentRole.Measure) == 1)
                    {
                        component = dataSetType.DataSetComponents.Single(c =>
                            c.Role == ComponentType.ComponentRole.Measure);
                    }
                    else
                    {
                        throw new Exception($"Operationen kan inte utföras då {InOperand.Alias} innehåller flera värdekomponenter. Specificera vilken komponent som avses genom {InOperand.Alias}#komponent.");
                    }
                }

                if (component == null)
                {
                    throw new Exception($"Kunde inte hitta komponenten {alias}");
                }

                component.Role = ComponentOperands[i].Role;

                if (component.ComponentDataHandler.Length == 1)
                {
                    var enumerator = component.ComponentDataHandler.GetEnumerator();
                    enumerator.MoveNext();
                    component.ComponentDataHandler = new ConstantComponentContainer(enumerator.Current);
                }

                if (component.Role == null) component.Role = ComponentType.ComponentRole.Measure;

                component.Name = alias;
                dataSet.SetDataSetComponent(component);
            }

            return dataSet;
        }

        internal override string[] GetComponentNames()
        {
            var components = GetIdentifierNames().ToList();
            components.AddRange(GetMeasureNames());
            return components.ToArray();
        }

        internal override string[] GetIdentifierNames()
        {
            List<string> identifierNames = InOperand.GetIdentifierNames().ToList();
            foreach (var identifierName in ComponentOperands.
                Where(co => co.Role == ComponentType.ComponentRole.Identifier).Select(c => c.Alias))
            {
                if (!identifierNames.Contains(identifierName))
                {
                    identifierNames.Add(identifierName);
                }
            }

            return identifierNames.ToArray();
        }

        internal override string[] GetMeasureNames()
        {
            List<string> measureNames = InOperand.GetMeasureNames().ToList();
            foreach (var measureName in ComponentOperands.
                Where(co => co.Role == ComponentType.ComponentRole.Measure).Select(c => c.Alias))
            {
                var simlilarMeasures = measureNames.Where(m => (m.Contains("#") && m.Substring(m.IndexOf("#") + 1) == measureName)
                                        || m == measureName).ToArray();
                if (simlilarMeasures.Any())
                {
                    foreach (var similarMeasure in simlilarMeasures)
                    {
                        measureNames.Remove(similarMeasure);
                    }
                }
                measureNames.Add(measureName);
            }

            return measureNames.ToArray();
        }
    }
}
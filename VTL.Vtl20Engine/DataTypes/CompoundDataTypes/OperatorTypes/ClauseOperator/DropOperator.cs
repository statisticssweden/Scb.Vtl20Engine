using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ClauseOperator
{
    public class DropOperator : Operator
    {
        protected Operand InOperand;
        protected Operand[] ComponentOperands;

        public DropOperator(Operand inOperand, Operand[] componentOperands)
        {
            InOperand = inOperand;
            foreach(var componentOperand in componentOperands)
            {
                if (inOperand.GetIdentifierNames().Contains(componentOperand.Alias))
                {
                    throw new Exception($"Det ät otillåtet att utföra drop med komponenten {componentOperand.Alias} eftersom den är en identifierare.");
                }
                if (componentOperand.Alias.Contains("#"))
                {
                    if (!inOperand.GetComponentNames().Contains(componentOperand.Alias))
                    {
                        throw new Exception($"{componentOperand.Alias} saknas i {InOperand.Alias}.");
                    }
                }
                else
                {
                    var inOperandComponents = inOperand.GetComponentNames().Select(c => c.Contains("#") ? c.Substring(c.IndexOf("#") + 1) : c);
                    if (!inOperandComponents.Contains(componentOperand.Alias))
                    {
                        throw new Exception($"{componentOperand.Alias} saknas i {InOperand.Alias}.");
                    }
                    if (inOperandComponents.Count(c => c == componentOperand.Alias) > 1)
                    {
                        throw new Exception($"{InOperand.Alias} innehåller flera komponenter med namnet {componentOperand.Alias}.");
                    }
                }
            }
            ComponentOperands = componentOperands;
        }

        internal override DataType PerformCalculation()
        {
            var dataSet = InOperand.GetValue() as DataSetType;

            if(dataSet == null)
            {
                throw new Exception("Drop kan bara utföras på dataset.");
            }
            var componentList = dataSet.DataSetComponents.ToList();
            foreach (var componentOperand in ComponentOperands)
            {
                ComponentType component = null;

                var value = componentOperand.GetValue();

                if (value is ComponentType componentType)
                {
                    component = componentType;
                }
                else if (value is DataSetType dataSetType)
                {
                    component = dataSetType.DataSetComponents.FirstOrDefault(c => c.Name.Equals(componentOperand.Alias));
                }
                else if (!string.IsNullOrEmpty(componentOperand.Alias))
                {
                    component = dataSet.GetDataSetComponent(componentOperand.Alias);
                }

                if (component == null)
                {
                    throw new Exception($"Kunde inte hitta komponenten {componentOperand.Alias}");
                }
                if (component.Role == ComponentType.ComponentRole.Identifier)
                {
                    throw new Exception($"Det ät otillåtet att utföra drop med komponenten {componentOperand.Alias} eftersom den är en identifierare.");
                }
                componentList.Remove(component);
            }

            var result = new DataSetType(componentList.ToArray());

            var componentCount = componentList.Count;

            var componentEnumerators = componentList.Select(c => c.GetEnumerator()).ToArray();

            var dataPoint = new DataPointType(componentCount);
            var keepGoing = componentEnumerators.All(e => e.MoveNext());
            while (keepGoing)
            {
                dataPoint = new DataPointType(componentCount);
                for (int i = 0; i < componentCount; i++)
                {
                    dataPoint[i] = componentEnumerators[i].Current;
                }
                result.Add(dataPoint);
                keepGoing = componentEnumerators.All(e => e.MoveNext());
            }
            return result;
        }

        internal override string[] GetComponentNames()
        {
            var componentNames = InOperand.GetComponentNames().ToList();
            var dropComponentNames = new List<string>();
            foreach (var componentOperand in ComponentOperands)
            {
                componentNames.Where(cn => componentOperand.NameEquals(cn)).ToList().ForEach(c => dropComponentNames.Add(c));
            }

            return componentNames.Except(dropComponentNames).ToArray();
        }

        internal override string[] GetIdentifierNames()
        {
            return InOperand.GetIdentifierNames();
        }

        internal override string[] GetMeasureNames()
        {
            var measureNames = InOperand.GetMeasureNames().ToList();
            foreach (var componentOperand in ComponentOperands)
            {
                if (measureNames.Contains(componentOperand.Alias))
                {
                    measureNames.Remove(componentOperand.Alias);
                }
            }

            return measureNames.ToArray();
        }
    }
}
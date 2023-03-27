using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ClauseOperator
{
    public class KeepOperator : Operator
    {
        protected Operand InOperand;
        protected Operand[] ComponentOperands;

        public KeepOperator(Operand inOperand, Operand[] componentOperands)
        {
            InOperand = inOperand;
            ComponentOperands = componentOperands;
        }

        internal override DataType PerformCalculation()
        {
            var inOperands = new List<Operand> { InOperand };
            foreach (var c in ComponentOperands)
            {
                inOperands.Add(c);
            }

            var inValues = inOperands.AsParallel().Select(x => x.GetValue()).ToArray();

            var dataSet = inValues[0] as DataSetType;

            if (dataSet == null)
            {
                throw new Exception("Keep kan bara utföras på dataset.");
            }
            var componentList = dataSet.DataSetComponents
                .Where(c => c.Role == ComponentType.ComponentRole.Identifier).ToList();
            for (var i = 0; i < ComponentOperands.Length; i++)
            {
                ComponentType component = null;
                var alias = ComponentOperands[i].Alias;
                var value = inValues[i + 1];

                if (value is ComponentType componentType)
                {
                    component = componentType;
                }

                if (value is DataSetType dataSetType)
                {
                    component = 
                        dataSetType.DataSetComponents.FirstOrDefault(c => c.Name.Equals(alias)) ??
                        dataSetType.DataSetComponents.FirstOrDefault(c => c.Name.Substring(c.Name.IndexOf("#") + 1).Equals(alias));
                }
                    
                if (component == null)
                {
                    throw new Exception($"Kunde inte hitta komponenten {alias}");
                }

                if (component.Role == ComponentType.ComponentRole.Identifier)
                {
                    throw new Exception($"Det ät otillåtet att utföra keep med komponenten {alias} eftersom den är en identifierare.");
                }

                componentList.Add(component);
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
            var componentNames = InOperand.GetIdentifierNames().ToList();
            foreach (var componentOperand in ComponentOperands)
            {
                componentNames.Add(componentOperand.Alias);
            }

            return componentNames.ToArray();
        }

        internal override string[] GetIdentifierNames()
        {
            return InOperand.GetIdentifierNames();
        }

        internal override string[] GetMeasureNames()
        {
            var measureNames = new List<string>();
            foreach (var componentOperand in ComponentOperands)
            {
                measureNames.Add(componentOperand.Alias);
            }

            return measureNames.ToArray();
        }
    }
}

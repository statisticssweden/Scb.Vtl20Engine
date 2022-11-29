using System;
using System.Linq;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes
{
    public abstract class UnaryOperator : Operator
    {
        protected Operand Operand;

        public abstract ScalarType PerformCalculation(ScalarType scalar);

        public DataSetType PerformCalculation(DataSetType dataSet)
        {
            foreach (var dsMeasure in dataSet.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Measure))
            {
                if (!CompatibleDataType(dsMeasure.DataType))
                {
                    throw new Exception($"Värdekomponenten {dsMeasure.Name} har datatypen {dsMeasure.DataType.Name} vilken inte är tillåten för operatorn {GetType().Name}.");
                }

                dsMeasure.DataType = SetDataTypeForMeasureComponent(dsMeasure);

            }

            var result = new DataSetType(dataSet.DataSetComponents);

            foreach (var resultDataSetComponent in result.DataSetComponents.
                Where(c => c.Role == ComponentType.ComponentRole.Measure))
            {
                resultDataSetComponent.DataType = GetResultType(resultDataSetComponent.DataType);
            }

            foreach (var operandDataPoint in dataSet.DataPoints)
            {
                var resultDataPoint = new DataPointType(dataSet.DataSetComponents.Length);
                for (int j = 0; j < dataSet.DataSetComponents.Length; j++)
                {
                    if (dataSet.DataSetComponents[j].Role == ComponentType.ComponentRole.Identifier)
                        resultDataPoint[j] = operandDataPoint[j];
                    if (dataSet.DataSetComponents[j].Role == ComponentType.ComponentRole.Measure)
                        resultDataPoint[j] = PerformCalculation(operandDataPoint[j]);
                }

                result.Add(resultDataPoint);
            }

            return result;
        }

        protected virtual Type GetResultType(Type dataType)
        {
            return dataType;
        }

        internal virtual Type SetDataTypeForMeasureComponent(ComponentType componentType)
        {
            return componentType.DataType;
        }

        internal abstract bool CompatibleDataType(Type dataType);

        public ComponentType PerformCalculation(ComponentType component)
        {
            Type type = SetDataTypeForMeasureComponent(component);
            var resultComponent = new ComponentType(type, VtlEngine.DataContainerFactory.CreateComponentContainer(component.Length))
            {
                Role = component.Role
            };
            foreach (var scalar in component)
            {
                resultComponent.Add(PerformCalculation(scalar));
            }

            return resultComponent;
        }
        
        internal override DataType PerformCalculation()
        {
            var operand = Operand.GetValue();

            if (operand is DataSetType ds) return PerformCalculation(ds);
            if (operand is ComponentType comp) return PerformCalculation(comp);
            if (operand is ScalarType scalar) return PerformCalculation(scalar);

            throw new Exception($"{Operand.Alias} kändes inte igen.");
        }

        internal override string[] GetComponentNames()
        {
            return Operand.GetComponentNames();
        }

        internal override string[] GetIdentifierNames()
        {
            return Operand.GetIdentifierNames();
        }

        internal override string[] GetMeasureNames()
        {
            return Operand.GetMeasureNames();
        }
    }
}

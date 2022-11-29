using System;
using System.Linq;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ComparisonOperator
{
    public abstract class NaryComparisonOperator : Operator
    {
        protected Operand _operand;

        protected abstract BooleanType PerformCalculation(ScalarType scalar);

        internal override DataType PerformCalculation()
        {
            var data = _operand.GetValue();
            if (data is DataSetType dataSet)
            {
                return PerformCalculation(dataSet);
            }

            if (data is ComponentType component)
            {
                return PerformCalculation(component);
            }

            throw new Exception($"{_operand.Alias} kändes inte igen.");
        }

        private DataSetType PerformCalculation(DataSetType dataSet)
        {
            if (dataSet.DataSetComponents.Count(c => c.Role == ComponentType.ComponentRole.Measure) != 1)
            {
                throw new Exception(
                    "Dataset som är argument till between måste ha exakt bara ha en measure-komponent.");
            }

            var container = VtlEngine.DataContainerFactory.CreateDataPointContainer(
                dataSet.DataPointCount * dataSet.DataSetComponents.Length);
            container.OriginalSortOrder = dataSet.DataPoints.SortOrder ?? dataSet.DataPoints.OriginalSortOrder;
            var result = new DataSetType(dataSet.DataSetComponents, container);

            foreach (var dataPoint in dataSet.DataPoints)
            {
                var resultDataPoint = new DataPointType(dataSet.DataSetComponents.Length);
                for (int i = 0; i < dataSet.DataSetComponents.Length; i++)
                {
                    if (dataSet.DataSetComponents[i].Role == ComponentType.ComponentRole.Identifier)
                        resultDataPoint[i] = dataPoint[i];
                    if (dataSet.DataSetComponents[i].Role == ComponentType.ComponentRole.Measure)
                        resultDataPoint[i] = PerformCalculation(dataPoint[i]);
                }

                result.Add(resultDataPoint);
            }

            var measure = result.DataSetComponents.First(c => c.Role == ComponentType.ComponentRole.Measure);
            result.RenameComponent(measure.Name, "bool_var");
            var componentNames = result.DataSetComponents.Select(c => c.Name).ToArray();
            measure.DataType = typeof(BooleanType);
            result.SortComponents(componentNames);

            return result;
        }

        private ComponentType PerformCalculation(ComponentType component)
        {
            var resultComponent = new ComponentType(typeof(BooleanType), VtlEngine.DataContainerFactory.CreateComponentContainer(component.Length));
            resultComponent.Name = "bool_var";

            foreach (var scalar in component)
            {
                resultComponent.Add(PerformCalculation(scalar));
            }

            return resultComponent;
        }
        
        internal override string[] GetComponentNames()
        {
            var components = _operand.GetIdentifierNames().ToList();
            components.Add("bool_var");
            return components.ToArray();
        }

        internal override string[] GetIdentifierNames()
        {
            return _operand.GetIdentifierNames();
        }

        internal override string[] GetMeasureNames()
        {
            return new[] {"bool_var"};
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Extensions;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ComparisonOperator
{
    public abstract class BinaryComparisonOperator : Operator
    {
        protected Operand Operand1;
        protected Operand Operand2;
        private int _ds1MeasureIndex, _ds2MeasureIndex;

        public abstract BooleanType PerformCalculation(NumberType number1, NumberType number2);
        public abstract BooleanType PerformCalculation(StringType string1, StringType string2);
        public abstract BooleanType PerformCalculation(TimePeriodType timePeriod1, TimePeriodType timePeriod2);
        public abstract BooleanType PerformCalculation(BooleanType boolean1, BooleanType boolean2);

        public virtual BooleanType PerformNullCalculation(ScalarType scalar)
        {
            return null;
        }

        public DataPointType PerformCalculation(DataPointType dataPoint1, DataPointType dataPoint2)
        {
            if (dataPoint1.Count() >= dataPoint2.Count())
            {
                var newDataPoint = new DataPointType(dataPoint1);
                newDataPoint[_ds1MeasureIndex] =
                    PerformCalculation(dataPoint1[_ds1MeasureIndex], dataPoint2[_ds2MeasureIndex]);
                return newDataPoint;
            }
            else
            {
                var newDataPoint = new DataPointType(dataPoint2);
                newDataPoint[_ds2MeasureIndex] =
                    PerformCalculation(dataPoint1[_ds1MeasureIndex], dataPoint2[_ds2MeasureIndex]);
                return newDataPoint;
            }
        }

        public ScalarType PerformCalculation(ScalarType scalar1, ScalarType scalar2)
        {
            if (scalar1 is NullType)
            {
                return PerformNullCalculation(scalar2);
            }
            if (scalar2 is NullType)
            {
                return PerformNullCalculation(scalar1);
            }

            if (!scalar1.HasValue() || !scalar2.HasValue())
            {
                return new BooleanType(null);
            }

            if (scalar1 is NumberType num1 && scalar2 is NumberType num2)
            {
                return PerformCalculation(num1, num2);
            }

            if (scalar1 is StringType str1 && scalar2 is StringType str2)
            {
                return PerformCalculation(str1, str2);
            }

            if (scalar1 is TimePeriodType tp1 && scalar2 is TimePeriodType tp2)
            {
                return PerformCalculation(tp1, tp2);
            }

            if (scalar1 is BooleanType b1 && scalar2 is BooleanType b2)
            {
                return PerformCalculation(b1, b2);
            }
            return new BooleanType(false);
        }

        public DataSetType PerformCalculation(DataSetType dataSet1, DataSetType dataSet2)
        {
            var ds1Identifiers = dataSet1.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier)
                .ToArray();
            var ds2Identifiers = dataSet2.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier)
                .ToArray();
            var ds1Measures = dataSet1.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Measure)
                .ToArray();
            var ds2Measures = dataSet2.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Measure)
                .ToArray();

            if (!(ds1Measures.Length == 1 && ds2Measures.Length == 1))
            {
                throw new Exception(
                    $"Strukturen för två ingående dataset var inte kompatibla. \r\n" +
                    $"De båda dataseten måste innehålla en och samma värde-komponent.");
            }

            ComponentType[] subsetIdentifiers;
            if (ds2Identifiers.All(c => ds1Identifiers.Contains(c)))
            {
                subsetIdentifiers = ds2Identifiers;
            }
            else if (ds1Identifiers.All(c => ds2Identifiers.Contains(c)))
            {
                subsetIdentifiers = ds1Identifiers;
            }
            else
            {
                throw new Exception(
                    $"Strukturen för två ingående dataset var inte kompatibla. \r\n" +
                    $"Det ena datasetet måste innehålla samtliga identifier-komponenter som finns i det andra.");
            }

            dataSet1.SortDataPoints(subsetIdentifiers.Select(c => c.Name).Concat(ds1Measures.Select(c => c.Name)).ToArray());
            dataSet2.SortDataPoints(subsetIdentifiers.Select(c => c.Name).Concat(ds2Measures.Select(c => c.Name)).ToArray());

            _ds1MeasureIndex = dataSet1.IndexOfComponent(ds1Measures.First());
            _ds2MeasureIndex = dataSet2.IndexOfComponent(ds2Measures.First());

            var result = dataSet1.PerformJoinCalculation(dataSet2, PerformCalculation);

            var measure = result.DataSetComponents.FirstOrDefault(c => c.Role == ComponentType.ComponentRole.Measure);
            result.RenameComponent(measure.Name, "bool_var");
            measure.DataType = typeof(BooleanType);
            result.SortDataPoints(result.DataSetComponents.Select(c => new OrderByName() { ComponentName = c.Name }).ToArray());

            return result;
        }

        public ComponentType PerformCalculation(ComponentType component1, ComponentType component2)
        {
            var resultComponent = new ComponentType(typeof(BooleanType),
                VtlEngine.DataContainerFactory.CreateComponentContainer(Math.Min(component1.Length, component2.Length)))
            {
                Name = "bool_var",
                Role = component1.Role
            };
            using (var enumerator1 = component1.GetEnumerator())
            using (var enumerator2 = component2.GetEnumerator())
            {
                while (enumerator1.MoveNext() && enumerator2.MoveNext())
                {
                    resultComponent.Add(PerformCalculation(enumerator1.Current, enumerator2.Current));
                }
            }

            return resultComponent;
        }

        public ComponentType PerformCalculation(ComponentType component, ScalarType scalar)
        {
            var resultComponent = new ComponentType(typeof(BooleanType), VtlEngine.DataContainerFactory.CreateComponentContainer(component.Length))
            {
                Name = "bool_var",
                Role = component.Role
            };
            foreach (var c in component)
            {
                resultComponent.Add(PerformCalculation(c, scalar));
            }

            return resultComponent;
        }

        public DataSetType PerformCalculation(DataSetType dataSet, ScalarType scalar)
        {
            var result = new DataSetType(dataSet.DataSetComponents);

            if (dataSet.DataSetComponents.Count(c => c.Role == ComponentType.ComponentRole.Measure) != 1)
            {
                throw new Exception("Dataset som används som argument till en jämförelse måste ha exakt ett mätvärde.");
            }

            foreach (var dataPoint in dataSet.DataPoints)
            {
                var resultDataPoint = new DataPointType(dataSet.DataSetComponents.Length);
                for (int j = 0; j < dataSet.DataSetComponents.Length; j++)
                {
                    if (dataSet.DataSetComponents[j].Role == ComponentType.ComponentRole.Identifier)
                        resultDataPoint[j] = dataPoint[j];
                    if (dataSet.DataSetComponents[j].Role == ComponentType.ComponentRole.Measure)
                        resultDataPoint[j] = PerformCalculation(dataPoint[j], scalar);
                }

                result.Add(resultDataPoint);
            }

            var measure = result.DataSetComponents.FirstOrDefault(c => c.Role == ComponentType.ComponentRole.Measure);
            result.RenameComponent(measure.Name, "bool_var");
            var componentNames = result.DataSetComponents.Select(c => c.Name).ToArray();
            measure.DataType = typeof(BooleanType);
            result.SortComponents(componentNames);

            return result;
        }

        internal override DataType PerformCalculation()
        {
            var operand1 = Operand1.GetValue();
            var operand2 = Operand2.GetValue();

            // Both operands are datasets
            var dataSet1 = operand1 as DataSetType;
            var dataSet2 = operand2 as DataSetType;
            if (dataSet1 != null && dataSet2 != null)
            {
                return PerformCalculation(dataSet1, dataSet2);
            }

            // One operand is dataset the other scalar
            var dataSet = dataSet1 ?? dataSet2;
            var scalar = operand1 is ScalarType type ? type : operand2 as ScalarType;
            if (dataSet != null && scalar != null)
            {
                return PerformCalculation(dataSet, scalar);
            }

            // Both operands are components
            var component1 = operand1 as ComponentType;
            var component2 = operand2 as ComponentType;
            if (component1 != null && component2 != null)
            {
                return PerformCalculation(component1, component2);
            }

            // One operand is dataset the other scalar
            var component = component1 ?? component2;
            if (component != null && scalar != null)
            {
                return PerformCalculation(component, scalar);
            }

            // Both operands are scalar
            var scalar1 = operand1 as ScalarType;
            var scalar2 = operand2 as ScalarType;
            if (scalar1 != null && scalar2 != null)
            {
                return PerformCalculation(scalar1, scalar2);
            }

            if (operand1 == null)
            {
                throw new Exception($"{Operand1.Alias} kändes inte igen.");
            }

            throw new Exception($"{Operand2.Alias} kändes inte igen.");
        }

        internal override string[] GetComponentNames()
        {
            var components1 = Operand1.GetIdentifierNames().ToList();
            var components2 = Operand2.GetIdentifierNames().ToList();
            var superSet = components1.Count >= components2.Count ? components1 : components2;
            superSet.Add("bool_var");
            return superSet.ToArray();
        }

        internal override string[] GetIdentifierNames()
        {
            var identifiers1 = Operand1.GetIdentifierNames();
            var identifiers2 = Operand2.GetIdentifierNames();
            return identifiers1.Length >= identifiers2.Length ? identifiers1 : identifiers2;
        }

        internal override string[] GetMeasureNames()
        {
            return new[] { "bool_var" };
        }
    }
}
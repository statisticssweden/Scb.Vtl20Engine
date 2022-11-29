using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Extensions;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes
{
    public abstract class BinaryOperator : Operator
    {
        protected Operand Operand1;
        protected Operand Operand2;
        private List<Tuple<int, int>> _measureMap;

        public abstract ScalarType PerformCalculation(ScalarType scalar1, ScalarType scalar2);

        public DataPointType PerformCalculation(DataPointType dataPoint1, DataPointType dataPoint2)
        {
            bool datapoint1IsSuperpoint = dataPoint1.Count() > dataPoint2.Count();
            
            var newDataPoint = new DataPointType(datapoint1IsSuperpoint ? dataPoint1 : dataPoint2);
            var gotValue = false;
            foreach (var map in _measureMap)
            {
                var val = PerformCalculation(dataPoint1[map.Item1], dataPoint2[map.Item2]);
                if (val != null) gotValue = true;
                newDataPoint[datapoint1IsSuperpoint ? map.Item1 : map.Item2] = val;
            }
            if (gotValue)
            {
                return newDataPoint;
            }

            return null;
        }

        public DataSetType PerformCalculation(DataSetType dataSet1, DataSetType dataSet2)
        {
            var ds1Identifiers = dataSet1.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier).ToArray();
            var ds2Identifiers = dataSet2.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier).ToArray();
            var ds1Measures = dataSet1.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Measure).ToArray();
            var ds2Measures = dataSet2.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Measure).ToArray();

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

            if (dataSet1.DataPointCount > 0 && dataSet2.DataPointCount > 0)
            {
                foreach (var dsMeasure in ds1Measures.Union(ds2Measures))
                {
                    ValidateCompatibleDataType(dsMeasure);
                }
            }

            _measureMap = new List<Tuple<int, int>>();
            // Handle cases like calc c := a#Value1 + b#value2
            if (ds1Measures.Length == 1 && 
                ds2Measures.Length == 1 && 
                ds1Measures.FirstOrDefault().Name.Contains("#") &&
                ds2Measures.FirstOrDefault().Name.Contains("#"))
            {
                _measureMap.Add(
                    new Tuple<int, int>(Array.IndexOf(dataSet1.ComponentSortOrder, dataSet1.DataSetComponents.
                                            First(c => c.Role == ComponentType.ComponentRole.Measure).Name),
                                        Array.IndexOf(dataSet2.ComponentSortOrder, dataSet2.DataSetComponents.
                                            First(c => c.Role == ComponentType.ComponentRole.Measure).Name)));
            }
            else
            {
                if (!(ds1Measures.Length == ds2Measures.Length &&
                      ds2Measures.All(c => ds1Measures.Contains(c)) &&
                      ds1Measures.All(c => ds2Measures.Contains(c))))
                {
                    throw new Exception(
                        $"Strukturen för två ingående dataset var inte kompatibla. \r\n" +
                        $"De båda dataseten måste innehålla samma värde-komponenter.");
                }

                foreach (var ds1Measure in ds1Measures)
                {
                    _measureMap.Add(
                        new Tuple<int, int>(Array.IndexOf(dataSet1.ComponentSortOrder, ds1Measure.Name),
                            Array.IndexOf(dataSet2.ComponentSortOrder, ds1Measure.Name)));
                }
            }

            var result = dataSet1.PerformJoinCalculation(dataSet2, PerformCalculation);

            int i = 0;
            foreach (var resultDataSetComponent in result.DataSetComponents.
                Where(c => c.Role == ComponentType.ComponentRole.Measure))
            {
                resultDataSetComponent.DataType = GetResultType(
                    dataSet1.DataSetComponents[_measureMap[i].Item1].DataType,
                    dataSet2.DataSetComponents[_measureMap[i].Item2].DataType);
                i++;
            }
            return result;
        }



        internal abstract bool CompatibleDataType(Type dataType);

        public ComponentType PerformCalculation(ComponentType component1, ComponentType component2)
        {
            var resultComponent = new ComponentType(
                GetResultType(component1.DataType, component2.DataType),
                VtlEngine.DataContainerFactory.CreateComponentContainer(Math.Min(component1.Length, component1.Length)))
            {
                Role = component1.Role
            };
            using (var component1Enumerator = component1.GetEnumerator())
            using (var component2Enumerator = component2.GetEnumerator())
            {
                while (component1Enumerator.MoveNext() && component2Enumerator.MoveNext())
                {
                    resultComponent.Add(PerformCalculation(component1Enumerator.Current, component2Enumerator.Current));
                }
            }
            
            return resultComponent;
        }

        protected virtual Type GetResultType(Type component1DataType, Type component2DataType)
        {
            if (component1DataType == typeof(NumberType) || component2DataType == typeof(NumberType))
            {
                return typeof(NumberType);
            }

            return component1DataType;
        }

        public ComponentType PerformCalculation(ComponentType component, ScalarType scalar)
        {
            var resultComponent = new ComponentType(
                GetResultType(component.DataType, scalar.GetType()),
                new SimpleComponentContainer())
            {
                Role = component.Role
            };

            foreach (var c in component)
            {
                resultComponent.Add(PerformCalculation(c, scalar));
            }

            return resultComponent;
        }

        public ComponentType PerformCalculation(ScalarType scalar, ComponentType component)
        {
            var resultComponent = new ComponentType(
                GetResultType(component.DataType, scalar.GetType()),
                new SimpleComponentContainer())
            {
                Role = component.Role
            };

            foreach (var c in component)
            {
                resultComponent.Add(PerformCalculation(scalar, c));
            }

            return resultComponent;
        }

        public DataSetType PerformCalculation(DataSetType dataSet, ScalarType scalar)
        {

            var measures = dataSet.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Measure);
            foreach (var measure in measures)
            {
                ValidateCompatibleDataType(measure);
                measure.DataType = GetResultType(measure.DataType, scalar.GetType());
            }

            var resultDataSet = new DataSetType(dataSet.DataSetComponents);
            resultDataSet.DataPoints.OriginalSortOrder = dataSet.DataPoints.SortOrder ?? dataSet.DataPoints.OriginalSortOrder;

            var operandDataPointEnumerator = dataSet.GetDataPointEnumerator();

            while (operandDataPointEnumerator.MoveNext())
            {
                var operandDataPoint = operandDataPointEnumerator.Current;
                var resultDataPoint = new DataPointType(dataSet.DataSetComponents.Length);
                for (int j = 0; j < dataSet.DataSetComponents.Length; j++)
                {
                    if (dataSet.DataSetComponents[j].Role == ComponentType.ComponentRole.Identifier ||
                        dataSet.DataSetComponents[j].Role == ComponentType.ComponentRole.Attribure)
                        resultDataPoint[j] = operandDataPoint[j];
                    if (dataSet.DataSetComponents[j].Role == ComponentType.ComponentRole.Measure)
                    {
                        resultDataPoint[j] = PerformCalculation(operandDataPoint[j], scalar);
                    }
                }
                resultDataSet.Add(resultDataPoint);
            }

            return resultDataSet;
        }

        public DataSetType PerformCalculation(ScalarType scalar, DataSetType dataSet)
        {
            var measures = dataSet.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Measure);
            foreach (var measure in measures)
            {
                measure.DataType = GetResultType(measure.DataType, scalar.GetType());
            }

            var resultDataSet = new DataSetType(dataSet.DataSetComponents);
            resultDataSet.DataPoints.OriginalSortOrder = dataSet.DataPoints.SortOrder ?? dataSet.DataPoints.OriginalSortOrder;

            var operandDataPointEnumerator = dataSet.GetDataPointEnumerator();

            while (operandDataPointEnumerator.MoveNext())
            {
                var operandDataPoint = operandDataPointEnumerator.Current;
                var resultDataPoint = new DataPointType(dataSet.DataSetComponents.Length);
                for (int j = 0; j < dataSet.DataSetComponents.Length; j++)
                {
                    if (dataSet.DataSetComponents[j].Role == ComponentType.ComponentRole.Identifier ||
                        dataSet.DataSetComponents[j].Role == ComponentType.ComponentRole.Attribure)
                        resultDataPoint[j] = operandDataPoint[j];
                    if (dataSet.DataSetComponents[j].Role == ComponentType.ComponentRole.Measure)
                        resultDataPoint[j] = PerformCalculation(scalar, operandDataPoint[j]);
                }

                resultDataSet.Add(resultDataPoint);
            }

            return resultDataSet;
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
            var scalar1 = operand1 as ScalarType;
            var scalar2 = operand2 as ScalarType;
            if (dataSet1 != null && scalar2 != null)
            {
                return PerformCalculation(dataSet1, scalar2);
            }
            if (scalar1 != null && dataSet2 != null)
            {
                return PerformCalculation(scalar1, dataSet2);
            }


            // Both operands are components
            var component1 = operand1 as ComponentType;
            var component2 = operand2 as ComponentType;
            if (component1 != null && component2 != null)
            {
                return PerformCalculation(component1, component2);
            }

            // One operand is dataset the other scalar
            if (component1 != null && scalar2 != null)
            {
                return PerformCalculation(component1, scalar2);
            }

            // One operand is dataset the other scalar
            if (scalar1 != null && component2 != null)
            {
                return PerformCalculation(scalar1, component2);
            }

            // Both operands are scalar
            if (scalar1 != null && scalar2 != null)
            {
                ValidateCompatibleDataType(scalar1);
                ValidateCompatibleDataType(scalar2);
                return PerformCalculation(scalar1, scalar2);
            }

            if (operand1 == null)
            {
                throw new Exception($"{Operand1.Alias} kändes inte igen.");
            }

            throw new Exception($"{Operand2.Alias} kändes inte igen.");
        }

        private void ValidateCompatibleDataType(ScalarType scalar)
        {
            if (!CompatibleDataType(scalar.GetType()))
            {
                throw new Exception($"Värdet {scalar} har datatypen {scalar.GetType().Name} vilken inte är tillåten för operatorn { GetType().Name}.");
            }
        }
        private void ValidateCompatibleDataType(ComponentType dsMeasure)
        {
            if (!CompatibleDataType(dsMeasure.DataType))
            {
                throw new Exception(
                    $"Värdekomponenten {dsMeasure.Name} har datatypen {dsMeasure.DataType.Name} vilken inte är tillåten för operatorn { GetType().Name}.");
            }
        }
        internal override string[] GetComponentNames()
        {
            var components1 = Operand1.GetComponentNames();
            var components2 = Operand2.GetComponentNames();
            return components1.Length >= components2.Length ? components1 : components2;
        }

        internal override string[] GetIdentifierNames()
        {
            var identifiers1 = Operand1.GetIdentifierNames();
            var identifiers2 = Operand2.GetIdentifierNames();
            return identifiers1.Length >= identifiers2.Length ? identifiers1 : identifiers2;
        }

        internal override string[] GetMeasureNames()
        {
            var measures1 = Operand1.GetMeasureNames();
            var measures2 = Operand2.GetMeasureNames();
            return measures1.Length >= measures2.Length ? measures1 : measures2;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.AnalyticOperator
{
    public abstract class AnalyticOperator : Operator
    {
        public Operand _operand;
        public IEnumerable<string> _partitionBy;
        public IEnumerable<OrderByName> _orderBy;

        internal override DataType PerformCalculation()
        {
            var value = _operand.GetValue();

            if (value is DataSetType dataSet)
            {

                var missingPartition = _partitionBy.Where(p => !dataSet.DataSetComponents.Select(c => c.Name).Contains(p)).ToArray();
                if (missingPartition.Any())
                {
                    throw new Exception($"{GetType().Name} kunde inte utföras eftersom {string.Join(", ", missingPartition)} saknas i {_operand.Alias}.");
                }

                var missingOrderBy = _orderBy?.Where(o => !dataSet.DataSetComponents.Select(c => c.Name).Contains(o.ComponentName)).Select(o => o.ComponentName).ToArray();
                if (missingOrderBy != null && missingOrderBy.Any())
                {
                    throw new Exception($"{GetType().Name} Rank kunde inte utföras eftersom {string.Join(", ", missingOrderBy)} saknas i {_operand.Alias}.");
                }

                // Sort data by partition
                var components = new List<string>();
                foreach (var componentName in _partitionBy)
                {
                    components.Add(componentName);
                }
                foreach (var componentName in dataSet.DataSetComponents.Select(c => c.Name).Except(_partitionBy))
                {
                    components.Add(componentName);
                }

                dataSet.SortComponents(components.ToArray());
                dataSet.SortDataPoints(components.Select(c => new OrderByName() { ComponentName = c }).ToArray());

                // Execute operator per partition
                var partition = new List<DataPointType>();
                var dataPointContainer = VtlEngine.DataContainerFactory.CreateDataPointContainer(
                    dataSet.DataPointCount * dataSet.DataSetComponents.Length);

                dataPointContainer.OriginalComponentOrder = dataSet.ComponentSortOrder;
                dataPointContainer.OriginalSortOrder = dataSet.DataPoints.SortOrder ?? dataSet.DataPoints.OriginalSortOrder;

                var componentContainer = new SimpleComponentContainer();
                var currentSelection = new ScalarType[_partitionBy.Count()];
                var measureIndexes = dataSet.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Measure).Select(i => dataSet.IndexOfComponent(i));

                var orderByIndexes = CalculateOrderByIndexes(dataSet.DataSetComponents);

                using (var dataPointEnumerator = dataSet.DataPoints.GetEnumerator())
                {
                    while (dataPointEnumerator.MoveNext())
                    {
                        bool same = true;
                        for (int i = 0; i < currentSelection.Count(); i++)
                        {
                            same &= currentSelection[i] != null && currentSelection[i].Equals(dataPointEnumerator.Current[i]);
                        }

                        if (same)
                        {
                            partition.Add(dataPointEnumerator.Current);
                        }
                        else
                        {
                            // Make calculation
                            if (partition.Any())
                            {
                                var partitionResult = PerformCalculation(partition, measureIndexes.ToList(), orderByIndexes);
                                foreach (var part in partitionResult)
                                {
                                    if (part is DataPointType dp)
                                    {
                                        dataPointContainer.Add(dp);
                                    }
                                    if (part is ScalarType s)
                                    {
                                        componentContainer.Add(s);
                                    }
                                }
                            }

                            // Make new selection
                            for (int i = 0; i < currentSelection.Count(); i++)
                            {
                                currentSelection[i] = dataPointEnumerator.Current[i];
                            }
                            partition.Clear();
                            partition.Add(dataPointEnumerator.Current);
                        }
                    }

                    // Make calculation with final partition
                    var partitionResult2 = PerformCalculation(partition, measureIndexes.ToList(), orderByIndexes);
                    foreach (var part in partitionResult2)
                    {
                        if (part is DataPointType dp)
                        {
                            dataPointContainer.Add(dp);
                        }
                        if (part is ScalarType s)
                        {
                            componentContainer.Add(s);
                        }
                    }

                }

                var resultComponents = dataSet.DataSetComponents;
                foreach (ComponentType component in resultComponents.Where(c => c.Role == ComponentType.ComponentRole.Measure))
                {
                  component.DataType = GetResultMeasureDatatype(component);
                }
                var result = new DataSetType(resultComponents.ToArray(), dataPointContainer);
                result.SortComponents();
                result.SortDataPoints(result.ComponentSortOrder);
                return result;
            }

            throw new NotImplementedException();
        }

        protected virtual Type GetResultMeasureDatatype(ComponentType component)
        {
            return component.DataType;
        }

        private List<Tuple<int, bool>> CalculateOrderByIndexes(ComponentType[] components)
        {
            var orderByIndexes = new List<Tuple<int, bool>>();
            if (_orderBy == null) return orderByIndexes;
            foreach (var orderBy in _orderBy)
            {
                orderByIndexes.Add(new Tuple<int, bool>(
                    Array.IndexOf(components, components.FirstOrDefault(c => c.Name.Equals(orderBy.ComponentName))),
                    orderBy.Descending));
            }
            return orderByIndexes;
        }
        
        internal abstract IEnumerable<DataType> PerformCalculation(List<DataPointType> partition, List<int> measureIndexes, List<Tuple<int, bool>> orderByIndexes);

        internal override string[] GetComponentNames()
        {
            return _operand.GetComponentNames();
        }

        internal override string[] GetIdentifierNames()
        {
            return _operand.GetIdentifierNames();
        }

        internal override string[] GetMeasureNames()
        {
            return _operand.GetMeasureNames();
        }
    }
}

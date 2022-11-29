using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.AnalyticOperator
{
    public abstract class AnalyticComponentOperator : Operator
    {
        public Operand _operand;
        public IEnumerable<string> _partitionBy;
        public IEnumerable<OrderByName> _orderBy;

        internal override DataType PerformCalculation()
        {
            var value = _operand.GetValue();

            if (value is DataSetType dataSet)
            {
                // Sort data by partition
                var components = new List<ComponentType>();

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

                dataSet.SortComponents(_partitionBy.ToArray());
                dataSet.SortDataPoints(_partitionBy.Select(p => new OrderByName() { ComponentName = p }).ToArray());


                // Execute operator per partition
                var partition = new DataSetType(dataSet.DataSetComponents);
                var dataPointContainer = VtlEngine.DataContainerFactory.CreateDataPointContainer(
                    dataSet.DataPointCount * dataSet.DataSetComponents.Length);

                dataPointContainer.OriginalComponentOrder = dataSet.ComponentSortOrder;
                dataPointContainer.OriginalSortOrder = dataSet.DataPoints.SortOrder ?? dataSet.DataPoints.OriginalSortOrder;

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
                            if (partition.DataPointCount > 0)
                            {
                                var partitionResult = PerformCalculation(partition, orderByIndexes);
                                foreach (var part in partitionResult)
                                {
                                    if (part is DataPointType dp)
                                    {
                                        dataPointContainer.Add(dp);
                                    }
                                }
                            }

                            // Make new selection
                            for (int i = 0; i < currentSelection.Count(); i++)
                            {
                                currentSelection[i] = dataPointEnumerator.Current[i];
                            }
                            partition = new DataSetType(dataSet.DataSetComponents);
                            partition.Add(dataPointEnumerator.Current);
                        }
                    }

                    // Make calculation with final partition
                    var partitionResult2 = PerformCalculation(partition, orderByIndexes);
                    foreach (var part in partitionResult2)
                    {
                        if (part is DataPointType dp)
                        {
                            dataPointContainer.Add(dp);
                        }
                    }

                }

                var resultStructure = dataSet.DataSetComponents.Where(c => c.Role != ComponentType.ComponentRole.Measure).ToList();
                resultStructure.Add(
                    new ComponentType(typeof(IntegerType),
                    new DataSetComponentContainer(dataPointContainer, "int_var"))
                    { Name = "int_var", Role = ComponentType.ComponentRole.Measure });
                return new DataSetType(resultStructure.ToArray(), dataPointContainer);

            }

            if (value is ComponentType component)
            {

            }
            throw new NotImplementedException();
        }
        
        private List<OrderByIndex> CalculateOrderByIndexes(ComponentType[] components)
        {
            var orderByIndexes = new List<OrderByIndex>();
            if (_orderBy == null) return orderByIndexes;
            foreach (var orderBy in _orderBy)
            {
                orderByIndexes.Add(new OrderByIndex(
                    Array.IndexOf(components, components.FirstOrDefault(c => c.Name.Equals(orderBy.ComponentName))),
                    orderBy.Descending));
            }
            return orderByIndexes;
        }

        protected bool AddOriginalOrder { get; set; }

        internal abstract IEnumerable<DataPointType> PerformCalculation
            (DataSetType dataSetPartition, List<OrderByIndex> orderByIndexes);

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

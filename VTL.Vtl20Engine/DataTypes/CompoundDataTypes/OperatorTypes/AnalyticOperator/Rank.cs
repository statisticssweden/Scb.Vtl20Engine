using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.AnalyticOperator
{
    public class Rank : AnalyticComponentOperator
    {
        public Rank(Operand operand, IEnumerable<string> partitionBy, IEnumerable<OrderByName> orderBy)
        {
            _operand = operand;
            _partitionBy = partitionBy;
            _orderBy = orderBy;
            AddOriginalOrder = true;
        }

        internal override IEnumerable<DataPointType> PerformCalculation(
            DataSetType partition, List<OrderByIndex> orderByIndexes)
        {
            var rankComparer = new DataPointComparer(orderByIndexes);
            partition.DataPoints.SortOrder = _orderBy.ToArray();
            var identifiers = partition.DataSetComponents.Where(c => c.Role != ComponentType.ComponentRole.Measure);
            var identifierIndexes = identifiers.Select(i => partition.IndexOfComponent(i)).ToArray();
            var result = new List<DataPointType>();

            int counter = 0;
            DataPointType lastDataPoint = null;
            int lastCount = 0;
            foreach(var dataPoint in partition.DataPoints)
            {
                counter++;
                var resultDataPoint = new DataPointType(identifiers.Count() + 1);

                for(var i = 0; i < identifierIndexes.Length; i++)
                {
                    resultDataPoint[i] = dataPoint[identifierIndexes[i]];
                }
                if (orderByIndexes.Any(index => !dataPoint[index.ComponentIndex].HasValue()))
                {
                    resultDataPoint[identifierIndexes.Length] = new IntegerType(null);
                }
                else if (lastDataPoint != null && orderByIndexes.All(index => dataPoint[index.ComponentIndex].Equals(lastDataPoint[index.ComponentIndex])))
                {
                    resultDataPoint[identifierIndexes.Length] = new IntegerType(lastCount);
                }
                else
                {
                    resultDataPoint[identifierIndexes.Length] = new IntegerType(counter);
                    lastCount = counter;
                }

                lastDataPoint = dataPoint;
                result.Add(resultDataPoint);
            }

            return result;
        }
    }
}

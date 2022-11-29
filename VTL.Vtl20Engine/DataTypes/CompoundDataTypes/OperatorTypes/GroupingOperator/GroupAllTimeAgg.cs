using System;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.TimeOperator
{
    internal class GroupAllTimeAgg : TimeAggBase
    {
        private Operand _comp;

        public GroupAllTimeAgg(Duration? periodIndTo, Duration? periodIndFrom, Operand op, Operand comp, bool last)
        {
            _periodIndTo = periodIndTo;
            _periodIndFrom = periodIndFrom;
            _op = op;
            _comp = comp;
            _last = last;
        }

        internal override DataType PerformCalculation()
        {
            var input = _op.GetValue();
            if (input is DataSetType dataset)
            {
                return PerformCalculation(dataset);
            }
            if (input is ComponentType component)
            {
                return PerformCalculation(component);
            }
            if(input is ScalarType scalar)
            {
                return PerformCalculation(scalar);
            }

            throw new NotImplementedException();
        }

        private DataType PerformCalculation(ComponentType component)
        {
            throw new NotImplementedException();
        }

        private DataType PerformCalculation(DataSetType dataSet)
        {
            var timeComps = dataSet.DataSetComponents.Where(c => c.DataType == typeof(DateType) ||
                                                          c.DataType == typeof(TimeType) ||
                                                          c.DataType == typeof(TimePeriodType));
            if (!timeComps.Any())
            {
                throw new Exception("Datasetet innehåller ingen komponent av datatypen time, date eller time_period.");
            }

            if (timeComps.Count() > 1)
            {
                throw new Exception("Datasetet innehåller fler än en komponent av datatypen time, date eller time_period.");
            }
            var timeComp = timeComps.First();

            dataSet.SortDataPoints(dataSet.DataSetComponents.Except(new[] { timeComp })
                .Where(c => c.Role == ComponentType.ComponentRole.Identifier)
                .Select(c => c.Name).ToArray());
            var timeCompIndex = dataSet.IndexOfComponent(timeComp);
            ScalarType lastTimeValue = null, timeValue = null;

            ComponentType[] measures;
            if (_comp != null && _comp.GetValue() is ComponentType comp)
            {
                measures = new[] { comp };
            }
            else
            {
                measures = dataSet.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Measure).ToArray();
            }
            var measureIndexes = measures.Select(m => dataSet.IndexOfComponent(m)).ToArray();
            var identifierIndexes = dataSet.DataSetComponents.Except(new[] { timeComp })
                .Where(c => c.Role == ComponentType.ComponentRole.Identifier)
                .Select(c => dataSet.IndexOfComponent(c)).ToArray();
            var result = new DataSetType(dataSet.DataSetComponents);

            using (var datasetEnumerator = dataSet.GetDataPointEnumerator())
            {
                DataPointType aggregatedDataPoint = null;
                DataPointType dataPoint;
                do
                {
                    dataPoint = datasetEnumerator.MoveNext() ? datasetEnumerator.Current as DataPointType : null;
                    bool sameIdentifiers = aggregatedDataPoint != null && dataPoint != null &&
                                            identifierIndexes.All(index =>
                                                dataPoint[index].Equals(aggregatedDataPoint[index]));
                    timeValue = dataPoint != null ? PerformCalculation(dataPoint[timeCompIndex]) : null;
                    if (aggregatedDataPoint != null)
                    {
                        result.Add(aggregatedDataPoint);
                    }
                    if (dataPoint != null)
                    {
                        aggregatedDataPoint = dataPoint;
                        aggregatedDataPoint[timeCompIndex] = timeValue;
                        lastTimeValue = timeValue;
                    }
                } while (dataPoint != null);
            }
            return result;

        }
    }
}
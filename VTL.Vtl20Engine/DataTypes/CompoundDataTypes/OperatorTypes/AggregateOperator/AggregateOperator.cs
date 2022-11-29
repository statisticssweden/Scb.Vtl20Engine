using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.AggregateOperator
{
    public abstract class AggregateOperator : Operator
    {
        protected Operand Operand;

        internal override DataType PerformCalculation()
        {
            var dataSet = Operand.GetValue() as DataSetType;

            if(dataSet == null)
            {
                throw new Exception("Aggregate kan bara utföras på dataset.");
            }
            if (dataSet.DataPointCount == 0) return dataSet;
            var measures = dataSet.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Measure);
            foreach (var measure in measures)
            {
                if (!measure.Any())
                {
                    throw new Exception("Delresultatet saknar värden. Operationen kan därför inte utföras.");
                }

                if (!CompatibleDataType(measure.DataType))
                {
                    throw new Exception(
                        $"Värdekomponenten {measure.Name} är inte numerisk. Operationen kan därför inte utföras.");
                }
            }

            var result = new DataSetType(dataSet.DataSetComponents);;
            var identifierIndexes = dataSet.DataSetComponents
                .Where(c => c.Role == ComponentType.ComponentRole.Identifier)
                .Select(c => dataSet.IndexOfComponent(c)).ToArray();
            var measureIndexes = measures.Select(c => dataSet.IndexOfComponent(c)).ToArray();
            var measureNames = measures.Select(c => c.Name).ToArray();
            DataPointType lastDataPoint = null;
            var measuresToAggregate = new List<ScalarType>[measureIndexes.Length];
            dataSet.SortDataPoints();
            var enumerator = dataSet.DataPoints.GetEnumerator();
            DataPointType dataPoint;

            do
            {
                dataPoint = enumerator.MoveNext() ? enumerator.Current as DataPointType : null;
                bool sameIdentifiers = lastDataPoint != null && dataPoint != null &&
                                        identifierIndexes.All(index =>
                                            dataPoint[index].Equals(lastDataPoint[index]));

                if (lastDataPoint != null)
                {
                    for (int i = 0; i < measureIndexes.Length; i++)
                    {
                        measuresToAggregate[i].Add(lastDataPoint[measureIndexes[i]]);
                    }

                    if (!sameIdentifiers)
                    {
                        for (int i = 0; i < measureIndexes.Length; i++)
                        {
                            lastDataPoint[measureIndexes[i]] = PerformCalculation(measuresToAggregate[i]);
                        }
                        result.Add(lastDataPoint);
                    }
                }

                if (!sameIdentifiers)
                {
                    for (int i = 0; i < measureIndexes.Length; i++)
                    {
                        measuresToAggregate[i] = new List<ScalarType>();
                    }
                }

                lastDataPoint = dataPoint;
            } while (dataPoint != null);

            foreach (var component in result.DataSetComponents)
            {
                if (component.Role == ComponentType.ComponentRole.Measure)
                {
                    component.DataType = GetResultMeasureDatatype(component);
                }
            }

            return result;
        }

        protected virtual bool CompatibleDataType(Type dataType)
        {
            return true;
        }

        internal abstract ScalarType PerformCalculation(List<ScalarType> scalars);

        protected virtual Type GetResultMeasureDatatype(ComponentType component)
        {
            return component.DataType;
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
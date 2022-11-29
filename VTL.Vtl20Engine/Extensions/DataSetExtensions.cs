using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;

namespace VTL.Vtl20Engine.Extensions
{
    public static class DataSetExtensions
    {
        public static DataSetType PerformJoinCalculation(this DataSetType dataSet1, DataSetType dataSet2,
            Func<DataPointType, DataPointType, DataPointType> performCalculation)
        {
            DataSetType superset;

            var ds1Identifiers = dataSet1.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier).ToArray();
            var ds2Identifiers = dataSet2.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier).ToArray();
            
            if (ds2Identifiers.All(c => ds1Identifiers.Contains(c)))
            {
                superset = dataSet1;
            }
            else if (ds1Identifiers.All(c => ds2Identifiers.Contains(c)))
            {
                superset = dataSet2;
            }
            else
            {
                throw new Exception(
                    $"Strukturen för två ingående dataset var inte kompatibla. \r\n" +
                    $"Det ena datasetet måste innehålla samtliga identifier-komponenter som finns i det andra.");
            }

            dataSet1.SortDataPoints();
            dataSet2.SortDataPoints();
            var identifierComparer = 
                new IdentifierComparer(dataSet1, dataSet2);

            var result = new DataSetType(superset.DataSetComponents);
            result.DataPoints.OriginalSortOrder = superset.DataPoints.SortOrder ?? superset.DataPoints.OriginalSortOrder;

            using (var dataset1Enumerator = dataSet1.DataPoints.GetEnumerator())
            using (var dataset2Enumerator = dataSet2.DataPoints.GetEnumerator())
            {
                bool dataset1EnumeratorHasValue = dataset1Enumerator.MoveNext();
                bool dataset2EnumeratorHasValue = dataset2Enumerator.MoveNext();
                while (dataset1EnumeratorHasValue && dataset2EnumeratorHasValue)
                {
                    var dataPoint1 = dataset1Enumerator.Current as DataPointType;
                    var dataPoint2 = dataset2Enumerator.Current as DataPointType;

                    var c = identifierComparer.Compare(dataPoint1, dataPoint2);
                    if (c < 0)
                    {
                        dataset1EnumeratorHasValue = dataset1Enumerator.MoveNext();
                    }

                    if (c > 0)
                    {
                        dataset2EnumeratorHasValue = dataset2Enumerator.MoveNext();
                    }

                    if (c == 0)
                    {
                        var resultDataPoint = performCalculation(dataPoint1, dataPoint2);
                        if (resultDataPoint != null)
                        {
                            result.Add(resultDataPoint);
                        }

                        if (dataPoint1.Count() > dataPoint2.Count())
                        {
                            dataset1EnumeratorHasValue = dataset1Enumerator.MoveNext();
                        }
                        else
                        {
                            dataset2EnumeratorHasValue = dataset2Enumerator.MoveNext();
                        }
                    }
                }
            }

            return result;
        }

        public static DataSetType Union(this DataSetType thisOne, DataSetType other)
        {
            // TODO Union tar inte hänsyn till dubletter
            // Fixa det vid implementation av VTL-kommandot union

            var resultDataSet = new DataSetType(thisOne.DataSetComponents, thisOne.DataPointCount + other.DataPointCount);

            foreach (var dataPoint in thisOne.DataPoints)
            {
                resultDataSet.Add(dataPoint);
            }

            foreach (var dataPoint in other.DataPoints)
            {
                resultDataSet.Add(dataPoint);
            }

            return resultDataSet;
        }

    }
}

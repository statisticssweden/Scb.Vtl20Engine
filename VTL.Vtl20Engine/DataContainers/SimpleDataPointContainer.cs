using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;

namespace VTL.Vtl20Engine.DataContainers
{
    public class SimpleDataPointContainer : IDataPointContainer
    {
        private HashSet<DataPointType> _dataPoints;

        public SimpleDataPointContainer()
        {
            _dataPoints = new HashSet<DataPointType>();
        }

        public SimpleDataPointContainer(HashSet<DataPointType> dataPoints)
        {
            _dataPoints = dataPoints;
        }

        public string[] ComponentSortOrder { get; set; }

        public string[] OriginalComponentOrder { get; set; }

        public OrderByName[] OriginalSortOrder { get; set; }

        public OrderByName[] SortOrder { get; set; }

        public int Length => _dataPoints.Count;

        public IEnumerator<DataPointType> GetEnumerator()
        {
            if(!_dataPoints.Any())
            {
                yield break;
            }

            if (Sorted())
            {
                if (ComponentsSorted())
                {
                    foreach (var dataPoint in _dataPoints)
                    {
                        yield return dataPoint;
                    }
                }
                else
                {
                    var componentCount = OriginalComponentOrder.Length;
                    var componentSortIndexes = ComponentSortOrder.Select(c => Array.IndexOf(OriginalComponentOrder, c)).ToArray();
                    foreach (var dataPoint in _dataPoints)
                    {
                        var newDataPoint = new DataPointType(componentCount);
                        for (int i = 0; i < componentCount; i++)
                        {
                            newDataPoint[i] = dataPoint[componentSortIndexes[i]];
                        }

                        yield return newDataPoint;
                    }
                }
            }
            else
            {
                if(SortOrder == null)
                {
                    SortOrder = OriginalComponentOrder.
                        Select(x => new OrderByName() { ComponentName = x }).ToArray();
                }

                var componentCount = OriginalComponentOrder.Length;
                var componentSortIndexes = ComponentSortOrder != null ?
                    ComponentSortOrder.Select(c => Array.IndexOf(OriginalComponentOrder, c)).ToArray() :
                    Enumerable.Range(0, OriginalComponentOrder.Length).ToArray();

                foreach (var dataPoint in _dataPoints.OrderBy(x => x, GetSortComparer()))
                {
                    var newDataPoint = new DataPointType(componentCount);
                    for (int i = 0; i < componentCount; i++)
                    {
                        newDataPoint[i] = dataPoint[componentSortIndexes[i]];
                    }

                    yield return newDataPoint;
                }
            }
        }

        private bool Sorted()
        {
            if(OriginalSortOrder == null)
            {
                return false;
            }

            if(SortOrder == null)
            {
                return true;
            }

            if(SortOrder.Length != OriginalSortOrder.Length)
            {
                return false;
            }

            for (int i = 0; i < SortOrder.Length; i++)
            {
                if (!SortOrder[i].Equals(OriginalSortOrder[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool ComponentsSorted()
        {
            if (ComponentSortOrder != null)
            {
                for (int i = 0; i < ComponentSortOrder.Length; i++)
                {
                    if (!ComponentSortOrder[i].Equals(OriginalComponentOrder[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public IEnumerator<ScalarType> GetEnumerator(string componentname)
        {
            if (Sorted())
            {
                foreach (var dataPoint in _dataPoints)
                {
                    var componentIndex = Array.IndexOf(OriginalComponentOrder, componentname);
                    yield return dataPoint[componentIndex];
                }
            }
            else
            {
                if (SortOrder == null)
                {
                    SortOrder = OriginalComponentOrder.
                        Select(x => new OrderByName() { ComponentName = x }).ToArray();
                }

                var componentCount = OriginalComponentOrder.Length;
                var componentIndex = Array.IndexOf(OriginalComponentOrder, componentname);
                
                foreach (var dataPoint in _dataPoints.OrderBy(x => x, GetSortComparer()))
                {
                    yield return dataPoint[componentIndex];
                }
            }
        }

        private DataPointComparer GetSortComparer()
        {
            var dataSortIndexes = new OrderByIndex[SortOrder.Length];
            for (int i = 0; i < SortOrder.Length; i++)
            {
                int index = Array.IndexOf(OriginalComponentOrder, SortOrder[i].ComponentName);
                if (index == -1 && SortOrder[i].ComponentName.Contains("#"))
                {
                    var compName = SortOrder[i].ComponentName.Substring(SortOrder[i].ComponentName.IndexOf("#") + 1);
                    index = Array.IndexOf(OriginalComponentOrder, compName);
                }
                dataSortIndexes[i] = new OrderByIndex(index, SortOrder[i].Descending, SortOrder[i].NullValuesLast);
            }

            return new DataPointComparer(dataSortIndexes);
        }

        public void Add(DataPointType dataPoint)
        {
            _dataPoints.Add(dataPoint);
        }

        public void Dispose()
        {
        }

        public void RenameComponent(string oldName, string newName)
        {
            if (OriginalComponentOrder != null)
            {
                var indexOriginalComponentOrder = Array.IndexOf(OriginalComponentOrder, oldName);
                if (indexOriginalComponentOrder != -1)
                {
                    OriginalComponentOrder[indexOriginalComponentOrder] = newName;
                }
            }

            if (ComponentSortOrder != null)
            {
                var indexComponentSortOrder = Array.IndexOf(ComponentSortOrder, oldName);
                if (indexComponentSortOrder != -1)
                {
                    ComponentSortOrder[indexComponentSortOrder] = newName;
                }
            }

            if (OriginalSortOrder != null)
            {
                var obj = OriginalSortOrder.FirstOrDefault(x => x.ComponentName.Equals(oldName));
                obj.ComponentName = newName;
            }

            if (SortOrder != null)
            {
                var obj = SortOrder.FirstOrDefault(x => x.ComponentName.Equals(oldName));
                obj.ComponentName = newName;
            }
        }

        public void Flush()
        {
        }
    }

}

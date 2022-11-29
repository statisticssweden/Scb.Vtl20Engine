using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;

namespace VTL.Vtl20Engine.DataContainers
{
    public class DataSetComponentContainer : IComponentContainer
    {
        private readonly IDataPointContainer _dataPointContainer;
        private string _componentName;
        public DataSetComponentContainer(IDataPointContainer dataPointContainer, string componentName)
        {
            _dataPointContainer = dataPointContainer;
            _componentName = componentName;
        }
        public int Length => _dataPointContainer.Length;

        public IEnumerator<ScalarType> GetEnumerator()
        {
            return _dataPointContainer.GetEnumerator(_componentName);
        }

        public void Add(ScalarType scalar)
        {
            throw new NotImplementedException("Kan inte stoppa in data i dataset via komponent.");
        }

        public void Rename(string newName)
        {
            var oldName = _componentName;
            _componentName = newName;

            if (_dataPointContainer.OriginalComponentOrder != null)
            {
                var indexOriginalComponentOrder = Array.IndexOf(_dataPointContainer.OriginalComponentOrder, oldName);
                if (indexOriginalComponentOrder != -1)
                {
                    _dataPointContainer.OriginalComponentOrder[indexOriginalComponentOrder] = newName;
                }
            }

            if (_dataPointContainer.ComponentSortOrder != null)
            {
                var indexComponentSortOrder = Array.IndexOf(_dataPointContainer.ComponentSortOrder, oldName);
                if (indexComponentSortOrder != -1)
                {
                    _dataPointContainer.ComponentSortOrder[indexComponentSortOrder] = newName;
                }
            }

            if (_dataPointContainer.OriginalSortOrder != null)
            {
                var indexOriginalSortOrder = Array.IndexOf(_dataPointContainer.OriginalSortOrder, oldName);
                if (indexOriginalSortOrder != -1)
                {
                    _dataPointContainer.OriginalSortOrder[indexOriginalSortOrder].ComponentName = newName;
                }
            }

            if (_dataPointContainer.SortOrder != null)
            {
                var indexSortOrder = Array.IndexOf(_dataPointContainer.SortOrder, oldName);
                if (indexSortOrder != -1)
                {
                    _dataPointContainer.SortOrder[indexSortOrder].ComponentName = newName;
                }
            }
        }

        public void Dispose()
        {
            if(_dataPointContainer != null)
            {
                _dataPointContainer.Dispose();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataContainers;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes
{
    public class DataSetType : CompoundType
    {
        private ComponentType[] _dataSetComponents;
        public int DataPointCount => DataPoints.Length;

        public IDataPointContainer DataPoints { get; set; }

        public DataSetType(ComponentType[] components, IDataPointContainer dataPointContainer)
        {
            _dataSetComponents = new ComponentType[components.Length];
            for (int i = 0; i < _dataSetComponents.Length; i++)
            {
                _dataSetComponents[i] = new ComponentType(components[i].DataType, 
                    new DataSetComponentContainer(dataPointContainer, components[i].Name))
                {
                    Name = components[i].Name,
                    Role = components[i].Role
                };
            }

            DataPoints = dataPointContainer;
            ComponentSortOrder = _dataSetComponents.Select(c => c.Name).ToArray();
            DataPoints.OriginalComponentOrder = ComponentSortOrder;
        }

        public DataSetType(ComponentType[] components, int size)
        {

            _dataSetComponents = new ComponentType[components.Length];
            DataPoints = VtlEngine.DataContainerFactory.CreateDataPointContainer(size);

            for (int i = 0; i < _dataSetComponents.Length; i++)
            {
                _dataSetComponents[i] = new ComponentType(components[i].DataType, 
                    new DataSetComponentContainer(DataPoints, components[i].Name));
                _dataSetComponents[i].Name = components[i].Name;
                _dataSetComponents[i].Role = components[i].Role;
            }

            ComponentSortOrder = _dataSetComponents.Select(c => c.Name).ToArray();
            DataPoints.OriginalComponentOrder = ComponentSortOrder;

        }

        public DataSetType(ComponentType[] components) :
            this(components, components[0].Length * components.Length)
        {
        }

        internal void RenameComponent(string oldName, string newName)
        {
            var index = Array.IndexOf(ComponentSortOrder, oldName);
            ComponentSortOrder[index] = newName;
            var component = _dataSetComponents.FirstOrDefault(c => c.Name.Equals(oldName));
            component.Rename(newName);

            DataPoints.RenameComponent(oldName, newName);
        }

        private string[] _componentSortOrder;

        public void SortComponents(string[] componentSortOrder)
        {
            var otherComponentNames = _dataSetComponents.Select(c => c.Name).Except(componentSortOrder).ToList();
            var sortedComponentNames = componentSortOrder.ToList();
            sortedComponentNames.AddRange(otherComponentNames);
            ComponentSortOrder = sortedComponentNames.ToArray();
            DataPoints.ComponentSortOrder = ComponentSortOrder;
        }

        public void SortComponents()
        {
            SortComponents(_dataSetComponents.OrderBy(x => x.Role).ThenBy(x => x.Name).Select(x => x.Name).ToArray());
        }

        public DataSetType(DataSetType ds)
        {
            DataPoints = VtlEngine.DataContainerFactory.CreateDataPointContainer(
                ds.DataPointCount * ds.DataSetComponents.Length);
            ComponentSortOrder = ds.DataSetComponents.Select(c => c.Name).ToArray();
            DataPoints.OriginalComponentOrder = ComponentSortOrder;
            DataPoints.OriginalSortOrder = ds.DataPoints.SortOrder;
            foreach (var dataPoint in ds.DataPoints)
            {
                DataPoints.Add(dataPoint);
            }

            _dataSetComponents = new ComponentType[ds.DataSetComponents.Length];
            for (int i = 0; i < _dataSetComponents.Length; i++)
            {
                _dataSetComponents[i] = 
                    new ComponentType(ds.DataSetComponents[i].DataType, 
                    new DataSetComponentContainer(DataPoints, ds.DataSetComponents[i].Name));
                _dataSetComponents[i].Name = ds.DataSetComponents[i].Name;
                _dataSetComponents[i].Role = ds.DataSetComponents[i].Role;
            }
            
        }

        private ComponentType[] _sorted;

        public ComponentType[] DataSetComponents
        {
            get
            {
                if (ComponentSortOrder != null && _sorted == null)
                {
                    _sorted = new ComponentType[_dataSetComponents.Length];
                    var all = Enumerable.Range(0, _dataSetComponents.Length).ToList();
                    for (int i = 0; i < ComponentSortOrder.Length; i++)
                    {
                        var index = OriginalIndexOfComponent(ComponentSortOrder[i]);
                        _sorted[i] = _dataSetComponents[index];
                        all.Remove(index);
                    }

                    for (int i = ComponentSortOrder.Length; i < _dataSetComponents.Length; i++)
                    {
                        var index = all.FirstOrDefault();
                        _sorted[i] = _dataSetComponents[index];
                        all.Remove(index);
                    }
                    return _sorted;
                }

                return _sorted;
            }
            set => _dataSetComponents = value;
        }

        public IEnumerator<DataPointType> GetDataPointEnumerator()
        {
            return DataPoints.GetEnumerator();
        }

        public string[] ComponentSortOrder 
        {
            get => _componentSortOrder;
            set
            {
                _sorted = null;
                _componentSortOrder = value;
            }
        }

        internal void SortDataPoints()
        {
            if (DataPoints.ComponentSortOrder != null)
            {
                DataPoints.SortOrder = DataPoints.ComponentSortOrder.
                    Select(x => new OrderByName { ComponentName = x }).ToArray();
            }
            else
            {
                DataPoints.SortOrder = DataPoints.OriginalComponentOrder.
                    Select(x => new OrderByName { ComponentName = x }).ToArray();
            }
        }

        public void SortDataPoints(string[] sortOrder)
        {
            SortDataPoints(sortOrder.
                Select(x => new OrderByName { ComponentName = x }).ToArray());
        }

        public void SortDataPoints(OrderByName[] sortOrder)
        {
            ComponentSortOrder = sortOrder.Select(c => c.ComponentName).ToArray();
            SortComponents(ComponentSortOrder);
            DataPoints.SortOrder = sortOrder;
        }

        public void SetDataSetComponent(ComponentType component)
        {
            int index;

            if (_dataSetComponents.Any(c => c.Name.EndsWith("#" + component.Name)))
            {
                var componentsToRemove = _dataSetComponents.Where(c => c.Name.EndsWith("#" + component.Name));
                _dataSetComponents = _dataSetComponents.Except(componentsToRemove).ToArray();
                ComponentSortOrder = ComponentSortOrder.Except(componentsToRemove.Select(c => c.Name)).ToArray();
                DataPoints.OriginalComponentOrder = ComponentSortOrder;
                SortDataPoints(ComponentSortOrder);
            }

            if (_dataSetComponents.Any(c => c.Name.Equals(component.Name)))
            {
                index = OriginalIndexOfComponent(component);
                _dataSetComponents[index] = component;
            }
            else
            {
                index = _dataSetComponents.Length;
                _dataSetComponents = _dataSetComponents.Append(component).ToArray();

            }

            using (var componentEnumerator = component._ComponentDataHandler.GetEnumerator())
            using (var dataSetEnumerator = DataPoints.GetEnumerator())
            {
                var newDataPoints = VtlEngine.DataContainerFactory.CreateDataPointContainer(
                    DataPoints.Length * DataSetComponents.Length);
                var componentSortOrder = DataPoints.ComponentSortOrder ?? DataPoints.OriginalComponentOrder;
                var componentCount = componentSortOrder.Length;
                
                if (!componentSortOrder.Contains(component.Name))
                {
                    newDataPoints.OriginalComponentOrder = new string[componentCount + 1];
                    for (int i = 0; i < componentCount; i++)
                    {
                        newDataPoints.OriginalComponentOrder[i] = componentSortOrder[i];
                    }
                    newDataPoints.OriginalComponentOrder[componentCount] = component.Name;
                }
                else
                {
                    newDataPoints.OriginalComponentOrder = new string[componentCount];
                    for (int i = 0; i < componentCount; i++)
                    {
                        newDataPoints.OriginalComponentOrder[i] = componentSortOrder[i];
                    }
                }
                newDataPoints.OriginalSortOrder = DataPoints.SortOrder ?? DataPoints.OriginalSortOrder;

                while (componentEnumerator.MoveNext() && dataSetEnumerator.MoveNext())
                {
                    var newDataPoint = new DataPointType(_dataSetComponents.Length);
                    for (int i = 0; i < _dataSetComponents.Length; i++)
                    {
                        if (i == index)
                        {
                            newDataPoint[i] = componentEnumerator.Current;
                        }
                        else
                        {
                            newDataPoint[i] = dataSetEnumerator.Current[i];
                        }
                    }
                    newDataPoints.Add(newDataPoint);
                }

                DataPoints = newDataPoints;
            }
        }

        public ComponentType GetDataSetComponent(string name)
        {
            var component = _dataSetComponents.FirstOrDefault(c => c.Name.Equals(name));
            if (component == null)
            {
                throw new Exception($"Hittade ingen komponent med namnet {name}");
            }
            component._ComponentDataHandler = new DataSetComponentContainer(DataPoints, name);
            return component;
        }

        public int IndexOfComponent(ComponentType component)
        {
            var components = DataSetComponents.Select(c => c.Name).ToList();
            return components.IndexOf(component.Name);
        }

        private int OriginalIndexOfComponent(ComponentType component)
        {
            var components = _dataSetComponents.Select(c => c.Name).ToList();
            return components.IndexOf(component.Name);
        }

        public int OriginalIndexOfComponent(string componentName)
        {
            var components = _dataSetComponents.Select(c => c.Name).ToList();
            return components.IndexOf(componentName);
        }

        public int IndexOfComponent(string componentName)
        {
            return Array.IndexOf(ComponentSortOrder, componentName);
        }

        internal void Add(DataPointType dataPoint)
        {
            DataPoints.Add(dataPoint);
            for (int i = 0; i < _dataSetComponents.Length; i++)
            {
                _dataSetComponents[i]._ComponentDataHandler = new DataSetComponentContainer(DataPoints, _dataSetComponents[i].Name);
            }
        }

        public override void Dispose()
        {
            if (DataPoints != null)
            {
                DataPoints.Dispose();
            }
        }
    }
}
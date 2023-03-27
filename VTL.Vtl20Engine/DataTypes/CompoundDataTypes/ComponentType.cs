using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes
{
    public class ComponentType : CompoundType, IEnumerable<ScalarType>
    {
        public IComponentContainer ComponentDataHandler;

        public ComponentType(Type t, IComponentContainer componentDataHandler)
        {
            if (!typeof(ScalarType).IsAssignableFrom(t))
            {
                throw new Exception("Component type must be a scalar type!");
            }

            DataType = t;

            ComponentDataHandler = componentDataHandler;
        }

        public ComponentType(ComponentType component)
        {
            Name = component.Name;
            Role = component.Role;
            DataType = component.DataType;
            ComponentDataHandler = VtlEngine.DataContainerFactory.CreateComponentContainer(component.Length);
            foreach(var datum in component)
            {
                ComponentDataHandler.Add(datum);
            }
        }

        public string Name { get; set; }

        public ComponentRole? Role { get; set; }

        public Type DataType { get; set; }

        internal void Rename(string newName)
        {
            Name = newName;
            ComponentDataHandler.Rename(newName);
        }

        public int Length => ComponentDataHandler.Length;

        public override bool Equals(object obj)
        {
            if (obj is ComponentType other)
            {
                return Name.Equals(other.Name) && 
                       Role.Equals(other.Role) &&
                       (
                           DataType.IsAssignableFrom(other.DataType) || 
                           other.DataType.IsAssignableFrom(DataType)
                        );
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) Role;
                return hashCode;
            }
        }
        
        public enum ComponentRole
        {
            Identifier = 1,
            Measure = 2,
            Attribure = 3
        }

        public bool IsAssignableFrom(Type dataType)
        {
            if (DataType == typeof(NumberType))
                return dataType == typeof(NumberType) || dataType == typeof(IntegerType);
            return DataType == dataType;
        }

        public IEnumerator<ScalarType> GetEnumerator()
        {
            return ComponentDataHandler.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ScalarType scalar)
        {
            ComponentDataHandler.Add(scalar);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;

namespace VTL.Vtl20Engine.Comparers
{
    public class ComponentComparer : IComparer<ComponentType>
    {
        private readonly string[] _prioritizedComponents;

        public ComponentComparer()
        {
            _prioritizedComponents = new string[0];
        }

        public ComponentComparer(string[] prioritizedComponents)
        {
            _prioritizedComponents = prioritizedComponents;
        }

        public int Compare(ComponentType x, ComponentType y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;

            if (x.Role != y.Role)
            {
                if (x.Role == null) return 1;
                if (y.Role == null) return -1;
                return (int)x.Role - (int)y.Role;
            }

            if (_prioritizedComponents.Contains(x.Name) && !_prioritizedComponents.Contains(y.Name))
            {
                return -1;
            }
            if (!_prioritizedComponents.Contains(x.Name) && _prioritizedComponents.Contains(y.Name))
            {
                return 1;
            }

            if (x.Name != y.Name)
            {
                return String.Compare(x.Name, y.Name, StringComparison.Ordinal);
            }

            return 0;
        }
    }
}

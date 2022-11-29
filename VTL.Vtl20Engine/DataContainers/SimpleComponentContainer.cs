using System;
using System.Collections.Generic;
using System.Text;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;

namespace VTL.Vtl20Engine.DataContainers
{
    public class SimpleComponentContainer : IComponentContainer
    {
        private List<ScalarType> _data;
        public SimpleComponentContainer()
        {
            _data = new List<ScalarType>();
        }
        public int Length => _data.Count;

        public IEnumerator<ScalarType> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public void Add(ScalarType scalar)
        {
            _data.Add(scalar);
        }

        public void Rename(string newName)
        {
        }

        public void Dispose()
        {
        }
    }
}

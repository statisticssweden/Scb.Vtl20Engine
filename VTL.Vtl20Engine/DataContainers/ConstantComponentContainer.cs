using System;
using System.Collections.Generic;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;

namespace VTL.Vtl20Engine.DataContainers
{
    public class ConstantComponentContainer : IComponentContainer
    {
        private readonly ScalarType _scalartType;

        public ConstantComponentContainer(ScalarType scalartType)
        {
            _scalartType = scalartType;
        }

        public int Length { get; }

        public IEnumerator<ScalarType> GetEnumerator()
        {
            while (true)
            {
                yield return _scalartType;
            }
        }

        public void Add(ScalarType scalar)
        {
            throw new NotImplementedException();
        }

        public void Rename(string newName)
        {
        }

        public void Dispose()
        {
        }

        public void CompleteWrite()
        {
        }
    }
}

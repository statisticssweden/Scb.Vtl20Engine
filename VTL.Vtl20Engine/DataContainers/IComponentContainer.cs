using System;
using System.Collections.Generic;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;

namespace VTL.Vtl20Engine.DataContainers
{
    public interface IComponentContainer : IDisposable
    {
        int Length { get; }

        IEnumerator<ScalarType> GetEnumerator();

        void Add(ScalarType scalar);

        void Rename(string newName);
    }
}

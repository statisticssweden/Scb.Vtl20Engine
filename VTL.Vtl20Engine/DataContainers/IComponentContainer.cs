using System;
using System.Collections.Generic;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;

namespace VTL.Vtl20Engine.DataContainers
{
    public interface IComponentContainer : IDisposable
    {
        /// <summary>
        /// Number of scalar values in the data
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Returns an enumerator that iterates through all scalar values
        /// </summary>
        /// <returns>The enumerator object</returns>
        IEnumerator<ScalarType> GetEnumerator();

        /// <summary>
        /// Adds a scalar value to the container
        /// </summary>
        /// <param name="scalar"></param>
        void Add(ScalarType scalar);

        /// <summary>
        /// Renames the component
        /// </summary>
        /// <param name="newName">New component name</param>
        void Rename(string newName);

        /// <summary>
        /// Flushes any buffer in the data container at the end of any writing operation
        /// </summary>
        void CompleteWrite();
    }
}

using System;
using System.Collections.Generic;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;

namespace VTL.Vtl20Engine.DataContainers
{
    public interface IDataPointContainer : IDisposable
    {
        /// <summary>
        /// Component names in requested sort order
        /// </summary>
        string[] ComponentSortOrder { get; set; }

        /// <summary>
        /// Names of the components ordered as stored in the underlying data structure
        /// </summary>
        string[] OriginalComponentOrder { get; set; }

        /// <summary>
        /// Order of the data when read from the data handler
        /// </summary>
        OrderByName[] OriginalSortOrder { get; set; }

        /// <summary>
        /// Requested sort order of data points
        /// </summary>
        OrderByName[] SortOrder { get; set; }

        /// <summary>
        /// Flushes any buffer in the data container at the end of any writing operation
        /// </summary>
        void Flush();

        /// <summary>
        /// Number of data points in the data
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Returns an enumerator that iterates through all data points
        /// </summary>
        /// <returns>The enumerator object</returns>
        IEnumerator<DataPointType> GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through all values of the specified component
        /// </summary>
        /// <param name="componentName">The name of the component to iterate through</param>
        /// <returns>The enumerator object</returns>
        IEnumerator<ScalarType> GetEnumerator(string componentName);

        /// <summary>
        /// Adds a data point to the container
        /// </summary>
        /// <param name="dataPoint"></param>
        void Add(DataPointType dataPoint);

        /// <summary>
        /// Renames the specified component
        /// </summary>
        /// <param name="oldName">Old component name</param>
        /// <param name="newName">New component name</param>
        void RenameComponent(string oldName, string newName);
    }
}
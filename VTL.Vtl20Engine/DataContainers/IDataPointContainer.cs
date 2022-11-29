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
        /// Order of the data when read from the datahandler
        /// </summary>
        OrderByName[] OriginalSortOrder { get; set; }

        /// <summary>
        /// Requested sort order of data points
        /// </summary>
        OrderByName[] SortOrder { get; set; }

        void Flush();

        /// <summary>
        /// Number of datapoints in the data
        /// </summary>
        int Length { get; }

        IEnumerator<DataPointType> GetEnumerator();

        IEnumerator<ScalarType> GetEnumerator(string componentname);

        void Add(DataPointType dataPoint);

        void RenameComponent(string oldName, string newName);
    }
}
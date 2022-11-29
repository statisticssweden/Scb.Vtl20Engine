using System;

namespace VTL.Vtl20Engine.DataContainers
{
    public interface IDataContainerFactory : IDisposable
    {
        /// <summary>
        /// Creates a suitable DataPointContainer
        /// </summary>
        /// <param name="size">Expected number of datum (cells) in the container to be created</param>
        /// <returns>New DataPointContainer</returns>
        IDataPointContainer CreateDataPointContainer(int size);
        /// <summary>
        /// Creates a suitable ComponentContainer
        /// </summary>
        /// <param name="size">Expected number of datum (cells) in the container to be created</param>
        /// <returns>New ComponentContainer</returns>
        IComponentContainer CreateComponentContainer(int size);
    }
}

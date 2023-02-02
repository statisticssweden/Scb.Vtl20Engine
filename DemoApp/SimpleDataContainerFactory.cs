using VTL.Vtl20Engine.DataContainers;

namespace DemoApp
{
    internal class SimpleDataContainerFactory : IDataContainerFactory
    {
        public IComponentContainer CreateComponentContainer(int size)
        {
            return new SimpleComponentContainer();
        }

        public IDataPointContainer CreateDataPointContainer(int size)
        {
            return new SimpleDataPointContainer();
        }

        public void Dispose()
        {
        }
    }
}

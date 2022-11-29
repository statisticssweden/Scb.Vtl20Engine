using VTL.Vtl20Engine.DataContainers;

namespace VTL.Vtl20Engine.Test.Mocks
{
    public class DataContainerFactory : IDataContainerFactory
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

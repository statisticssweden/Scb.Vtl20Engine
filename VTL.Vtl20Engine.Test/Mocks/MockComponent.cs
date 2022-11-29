using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;

namespace VTL.Vtl20Engine.Test.Mocks
{
    public class MockComponent
    {
        public Type Type;

        public MockComponent(Type type)
        {
            Type = type;
        }

        public string Name { get; set; }
        public ComponentType.ComponentRole Role { get; set; }

        public static DataSetType MakeDataSet(List<MockComponent> mockComponents, SimpleDataPointContainer simpleDataPointContainer)
        {
            var components = new List<ComponentType>();
            for (int i = 0; i < mockComponents.Count; i++)
            {
                components.Add(new ComponentType(mockComponents[i].Type, new DataSetComponentContainer(simpleDataPointContainer, mockComponents[i].Name))
                {
                    Name = mockComponents[i].Name,
                    Role = mockComponents[i].Role
                });
            }
            simpleDataPointContainer.OriginalComponentOrder = mockComponents.Select(c => c.Name).ToArray();
            return new DataSetType(components.ToArray(), simpleDataPointContainer);
        }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;

namespace VTL.Vtl20Engine.Comparers
{
    public class IdentifierComparer : IComparer<DataPointType>
    {
        private List<Tuple<int, int>> _identifierMap;

        public IdentifierComparer(DataSetType dataset1, DataSetType dataset2)
        {
            _identifierMap = new List<Tuple<int, int>>();
            var supersetComponents = dataset1.DataSetComponents.Length > dataset2.DataSetComponents.Length
                ? dataset1.DataSetComponents : dataset2.DataSetComponents;
            for (int i = 0; i < supersetComponents.Length; i++)
            {
                var matching1 = dataset1.DataSetComponents.FirstOrDefault(c => supersetComponents[i].Name.Equals(c.Name));
                var matching2 = dataset2.DataSetComponents.FirstOrDefault(c => supersetComponents[i].Name.Equals(c.Name));
                if (supersetComponents[i].Role == ComponentType.ComponentRole.Identifier && matching1 != null && matching2 != null)
                {
                    _identifierMap.Add(new Tuple<int, int>(
                        Array.IndexOf(dataset1.DataSetComponents, matching1),
                        Array.IndexOf(dataset2.DataSetComponents, matching2)));
                }
            }
        }

        public int Compare(DataPointType dataPoint1, DataPointType dataPoint2)
        {
            foreach (var map in _identifierMap)
            {
                var c = dataPoint1[map.Item1].CompareTo(dataPoint2[map.Item2]);
                if (c != 0)
                {
                    return c;
                }
            }

            return 0;
        }
    }
}

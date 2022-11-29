using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;

namespace VTL.Vtl20Engine.Comparers
{
    public class DataPointComparer : IComparer<DataPointType>
    {

        private readonly IEnumerable<OrderByIndex> _orderByIndexes;

        public DataPointComparer(IEnumerable<int> orderByIndexes)
        {
            _orderByIndexes = orderByIndexes.Select(x => new OrderByIndex(x, false));
        }

        public DataPointComparer(IEnumerable<OrderByIndex> orderByIndexes)
        {
            _orderByIndexes = orderByIndexes;
        }

        public int Compare(DataPointType dpX, DataPointType dpY)
        {
            foreach (var orderByIndex in _orderByIndexes)
            {
                var x = dpX[orderByIndex.ComponentIndex];
                var y = dpY[orderByIndex.ComponentIndex];
                var xNull = !x.HasValue();
                var yNull = !y.HasValue();
                if (xNull && yNull) continue;
                if (xNull)
                {
                    if (orderByIndex.NullValueLast) return 1;
                    else return -1;
                }
                if (yNull)
                {
                    if (orderByIndex.NullValueLast) return -1;
                    else return 1;
                }
                var compare = x.CompareTo(y);
                if (compare != 0) return orderByIndex.Descending ? -compare : compare;
            }

            return 0;
        }
    }

}

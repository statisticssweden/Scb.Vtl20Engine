namespace VTL.Vtl20Engine.Comparers
{
    public class OrderByIndex
    {
        public int ComponentIndex { get; set; }
        public bool Descending { get; set; }
        public bool NullValueLast { get; set; }

        public OrderByIndex(int index, bool descending, bool nullValueLast)
        {
            ComponentIndex = index;
            Descending = descending;
            NullValueLast = nullValueLast;
        }

        public OrderByIndex(int index, bool descending)
            : this(index, false, false)
        {
        }

        public OrderByIndex(int index)
            : this(index, false)
        {
        }

        public override bool Equals(object obj)
        {
            var item = obj as OrderByIndex;

            if (item == null)
            {
                return false;
            }

            return ComponentIndex.Equals(item.ComponentIndex) &&
                Descending.Equals(item.Descending);
        }

        public override int GetHashCode()
        {
            return ComponentIndex.GetHashCode() ^ Descending.GetHashCode();
        }
    }
}
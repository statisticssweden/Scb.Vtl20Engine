namespace VTL.Vtl20Engine.Comparers
{
    public class OrderByName
    {
        public string ComponentName { get; set; }
        public bool Descending { get; set; }
        public bool NullValuesLast { get; set; }
        public override bool Equals(object obj)
        {
            var item = obj as OrderByName;

            if (item == null)
            {
                return false;
            }

            return ComponentName.Equals(item.ComponentName) &&
                Descending.Equals(item.Descending) &&
                NullValuesLast.Equals(item.NullValuesLast);
        }

        public override int GetHashCode()
        {
            return ComponentName.GetHashCode() ^ 
                Descending.GetHashCode() ^
                NullValuesLast.GetHashCode();
        }
    }
}
namespace VTL.Vtl20Engine.DataTypes.ScalarDataTypes
{
    public class NullType : ScalarType
    {
        public override ScalarType Clone()
        {
            return this;
        }

        public override bool HasValue()
        {
            return false;
        }

        public override int CompareTo(object obj)
        {
            return 0;
        }

    }
}

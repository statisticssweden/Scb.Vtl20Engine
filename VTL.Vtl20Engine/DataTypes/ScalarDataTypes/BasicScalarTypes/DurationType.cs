namespace VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes
{
    public class DurationType : BasicScalarType<Duration>
    {
        public DurationType(Duration duration)
        {
            Value = duration;
        }

        public override ScalarType Clone()
        {
            return new DurationType(Value);
        }

        public override int CompareTo(object obj)
        {
            return ((int)Value).CompareTo((int)obj);
        }
    }
}

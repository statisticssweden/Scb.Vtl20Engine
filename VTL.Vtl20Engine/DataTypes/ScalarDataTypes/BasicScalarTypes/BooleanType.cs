namespace VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes
{
    public class BooleanType : BasicScalarType<bool?>
    {
        public BooleanType(bool? value) => Value = value;
        public static implicit operator bool?(BooleanType b) => b.Value;
        public static implicit operator BooleanType(bool? b) => new BooleanType(b);
        public static bool? operator !(BooleanType b) => new BooleanType(!b.Value);
        public override ScalarType Clone()
        {
            return new BooleanType(Value);
        }
        public override int CompareTo(object obj)
        {
            var boolObj = obj as BooleanType;
            if (boolObj?.Value == null && Value == null)
            {
                return 0;
            }
            if (boolObj?.Value == null)
            {
                return 1;
            }
            if (Value == null)
            {
                return -1;
            }
            return Value.Value.CompareTo(boolObj.Value);
        }

        public override string ToString()
        {
            if(Value.HasValue)
            {
                if(Value.Value)
                {
                    return "True";
                }
                else
                {
                    return "False";
                }
            }
            return "Null";
        }
    }
}

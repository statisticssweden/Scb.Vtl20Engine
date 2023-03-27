using System;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.TimeOperator
{
    public class CurrentDate : Operator
    {
        internal override DataType PerformCalculation()
        {
            return new DateType(DateTime.Now);

        }

        internal override string[] GetComponentNames()
        {
            return new string[0];
        }

        internal override string[] GetIdentifierNames()
        {
            return new string[0];
        }

        internal override string[] GetMeasureNames()
        {
            return new string[0];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using static VTL.Vtl20Engine.DataTypes.CompoundDataTypes.ComponentType;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.StringOperator
{
    public class Concat : BinaryOperator
    {
        public Concat(Operand operand1, Operand operand2)
        {
            Operand1 = operand1;
            Operand2 = operand2;
        }

        public override ScalarType PerformCalculation(ScalarType scalar1, ScalarType scalar2)
        {
            if (scalar1 is StringType string1 && scalar2 is StringType string2)
            {
                return new StringType(string1 + string2);
            }
            throw new Exception("Operatorn || kan bara utföras på strängar.");
        }

        internal override bool CompatibleDataType(Type dataType)
        {
            return dataType == typeof(StringType);
        }
    }
}

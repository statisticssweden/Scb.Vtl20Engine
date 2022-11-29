using System;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes
{
    public abstract class Operator : CompoundType
    {
        internal DataType PerformCalculationWithLog()
        {
            var result = PerformCalculation();
            return result;
        }

        internal abstract DataType PerformCalculation();

        internal abstract string[] GetComponentNames();

        internal abstract string[] GetIdentifierNames();

        internal abstract string[] GetMeasureNames();
    }
}

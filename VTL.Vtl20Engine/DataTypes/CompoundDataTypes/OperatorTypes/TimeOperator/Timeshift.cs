using System;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.TimeOperator
{
    public class Timeshift : Operator
    {
        private Operand Operand;
        private IntegerType _shiftNumber;

        public Timeshift(Operand operand, IntegerType shiftNumber)
        {
            Operand = operand;
            _shiftNumber = shiftNumber;
        }

        internal override DataType PerformCalculation()
        {
            var operand = Operand.GetValue();

            if (operand is DataSetType dataSet)
            {
                var result = new DataSetType(dataSet.DataSetComponents);
                var timePeriods = dataSet.DataSetComponents.Where(c => c.DataType == typeof(TimePeriodType));
                if(timePeriods.Count() == 0)
                {
                    throw new Exception($"Dataset som timeshift ska utföras på måste ha en komponent av datatypen time_period.");
                }
                if (timePeriods.Count() > 1)
                {
                    throw new Exception($"Dataset som timeshift ska utföras på får inte ha mer än en komponent av datatypen time_period.");
                }
                var timePeriodIndex = dataSet.IndexOfComponent(timePeriods.Single());
                using (var datasetEnumerator = dataSet.GetDataPointEnumerator())
                {
                    while(datasetEnumerator.MoveNext())
                    {
                        var dataPoint = new DataPointType(datasetEnumerator.Current);
                        var timePeriod = dataPoint[timePeriodIndex] as TimePeriodType;
                        dataPoint[timePeriodIndex] = timePeriod + _shiftNumber;
                        result.Add(dataPoint);
                    }
                }
                return result;
            }
            else
            {
                throw new Exception($"Timeshift kan endast utföras på dataset.");
            }

            throw new Exception($"{Operand.Alias} kändes inte igen.");
        }

        internal override string[] GetComponentNames()
        {
            return Operand.GetComponentNames();
        }

        internal override string[] GetIdentifierNames()
        {
            return Operand.GetIdentifierNames();
        }

        internal override string[] GetMeasureNames()
        {
            return Operand.GetMeasureNames();
        }
    }
}

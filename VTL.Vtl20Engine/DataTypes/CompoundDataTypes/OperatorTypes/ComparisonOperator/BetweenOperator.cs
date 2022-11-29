using System;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ComparisonOperator
{
    internal class BetweenOperator : NaryComparisonOperator
    {
        private readonly Operand _from;
        private readonly Operand _to;

        public BetweenOperator(Operand operand, Operand from, Operand to)
        {
            _operand = operand;
            _from = from;
            _to = to;
        }

        protected override BooleanType PerformCalculation(ScalarType scalar)
        {
            var fromValue = _from.GetValue();
            var toValue = _to.GetValue();

            if (scalar is NumberType number)
            {
                if (fromValue is NumberType fromNumber && toValue is NumberType toNumber)
                {
                    if (!number.HasValue()) return new BooleanType(null);
                    return number >= fromNumber && number <= toNumber;
                }

                throw new Exception("Alla argument till between måste ha samma datatyp.");
            }

            if (scalar is TimePeriodType timePeriod)
            {
                if (fromValue is TimePeriodType fromTimePeriod && toValue is TimePeriodType toTimePeriod)
                {
                    if (!timePeriod.HasValue()) return new BooleanType(null);
                    return timePeriod >= fromTimePeriod && timePeriod <= toTimePeriod;
                }

                throw new Exception("Alla argument till between måste ha samma datatyp.");
            }

            if (scalar is StringType text)
            {
                if (fromValue is StringType fromString && toValue is StringType toString)
                {
                    if (!text.HasValue()) return new BooleanType(null);
                    return text >= fromString && text <= toString;
                }

                throw new Exception("Alla argument till between måste ha samma datatyp.");
            }

            throw new NotImplementedException();
        }
    }
}
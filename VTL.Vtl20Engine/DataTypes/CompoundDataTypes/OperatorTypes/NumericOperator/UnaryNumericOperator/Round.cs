using System;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.NumericOperator.UnaryNumericOperator
{
    internal class Round : UnaryNumericOperator
    {
        private IntegerType _decimals;
        private int _multiplier;
        private readonly Operand _numDigit;

        public Round(Operand operand, Operand numDigit)
        {
            Operand = operand;
            _numDigit = numDigit;
        }

        public override NumberType PerformCalculation(IntegerType integer)
        {
            return integer;
        }

        public override NumberType PerformCalculation(NumberType number)
        {
            if (_numDigit == null)
            {
                if (!number.HasValue()) return new IntegerType(null);
                return new IntegerType((long)Math.Round((decimal)number, MidpointRounding.AwayFromZero));
            }
            if (!number.HasValue()) return number;
            if(_decimals == null)
            {
                _decimals = _numDigit.GetValue() as IntegerType;
                _multiplier = (int)Math.Pow(10, (long)_decimals * -1);
            }

            if (_decimals < 0)
            {
                return _multiplier * Math.Round((decimal) number/_multiplier, MidpointRounding.AwayFromZero);
            }
            else
            {
                return Math.Round((decimal) number, (int)_decimals, MidpointRounding.AwayFromZero);
            }
        }

        protected override Type GetResultType(Type dataType)
        {
            if (_numDigit == null)
            {
                return typeof(IntegerType);
            }
            if (dataType == typeof(IntegerType))
            {
                return typeof(NumberType);
            }
            return dataType;
        }
    }
}
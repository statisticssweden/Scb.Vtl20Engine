using System;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.NumericOperator.UnaryNumericOperator
{
    internal class Trunc : UnaryNumericOperator
    {
        private IntegerType _decimals;
        private int _multiplier;
        private readonly Operand _numDigit;

        public Trunc(Operand operand, Operand numDigit)
        {
            Operand = operand;
            _numDigit = numDigit;
        }

        public override NumberType PerformCalculation(IntegerType integer)
        {
            if (!integer.HasValue()) return new NumberType(null);
            if (_numDigit == null)
            {
                return integer;
            }
            if (_decimals == null)
            {
                _decimals = _numDigit != null ? _numDigit.GetValue() as IntegerType : new IntegerType(0);
                _decimals = _numDigit.GetValue() as IntegerType;
                if (_decimals < 0)
                {
                    _multiplier = (int)Math.Pow(10, (long)_decimals * -1);
                }
                else
                {
                    _multiplier = (int)Math.Pow(10, (long)_decimals);
                }
            }
            if (_decimals < 0)
            {
                return new NumberType(_multiplier * Math.Floor((decimal)integer / _multiplier));
            }
            else
            {
                return new NumberType((long)integer);
            }
        }

        public override NumberType PerformCalculation(NumberType number)
        {
            if(_numDigit == null)
            {
                if (!number.HasValue()) return new IntegerType(null);
                return new IntegerType((long)Math.Floor((decimal)number));
            }
            if (!number.HasValue()) return number;
            if (_decimals == null)
            {
                _decimals = _numDigit.GetValue() as IntegerType;
                if (_decimals < 0)
                {
                    _multiplier = (int)Math.Pow(10, (long)_decimals * -1);
                }
                else
                {
                    _multiplier = (int)Math.Pow(10, (long)_decimals);
                }
            }
            if (_decimals == 0)
            {
                return new NumberType(Math.Floor((decimal)number));
            }
            else if (_decimals < 0)
            {
                return new NumberType(_multiplier * Math.Floor((decimal) number/_multiplier));
            }
            else
            {
                return new NumberType(Math.Floor((decimal)number * _multiplier) / _multiplier);
            }
        }

        protected override Type GetResultType(Type dataType)
        {
            if(_numDigit == null)
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
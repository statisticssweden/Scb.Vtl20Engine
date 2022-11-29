using System;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.NumericOperator.UnaryNumericOperator
{
    public class Power : UnaryNumericOperator
    {
        private readonly Operand _exponent;
        private double? _exponentValue;
        public Power(Operand operand, Operand exponent)
        {
            Operand = operand;
            _exponent = exponent;
        }
        public double? PowerExp
        {
            get
            {
                if (!_exponentValue.HasValue)
                {
                    var temp = _exponent.GetValue() as NumberType;
                    if (!temp.HasValue()) return null;
                    _exponentValue = Convert.ToDouble((decimal)temp);
                }
                return _exponentValue.Value;
            }

        }

        private bool ExponentIsDecimal
        {
            get
            {
                return _exponentValue != null && _exponentValue % 1 != 0;
            }
        }


        public override NumberType PerformCalculation(IntegerType integer)
        {
            if(PowerExp==null) return new NumberType(null);
            if(!integer.HasValue()) return new NumberType(null);
            if (integer == 0 && PowerExp.Value < 0) throw new Exception("Beräkningen resulterar i division med 0");
            if(integer<0 && ExponentIsDecimal) throw new Exception("Negativa tal får endast upphöjas till heltal.");

            var result = Convert.ToDecimal(Math.Pow(Convert.ToDouble((long)integer), PowerExp.Value));
            return new NumberType(result);

        }

        public override NumberType PerformCalculation(NumberType number)
        {
            if (PowerExp == null) return new NumberType(null);
            if (!number.HasValue()) return new NumberType(null);
            if (number == 0 && PowerExp.Value < 0) throw new Exception("Beräkningen resulterar i division med 0");
            if (number < 0 && ExponentIsDecimal) throw new Exception("Negativa tal får endast upphöjas till heltal.");

            var result = Convert.ToDecimal(Math.Pow(Convert.ToDouble((decimal)number), PowerExp.Value));
            return new NumberType(result);
        }

        protected override Type GetResultType(Type component1DataType)
        {
            return typeof(NumberType);
        }
    }
}

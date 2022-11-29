using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using System.Linq;
namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.GeneralPurposeOperator
{
    public class CastOperator : UnaryOperator
    {
        private Type _type;
        private string _mask = null;

        public CastOperator(Operand operand, Type type, String mask)
        {
            Operand = operand;
            _type = type;
            _mask = mask;
        }

        public CastOperator(Operand operand, Type type)
        {
            Operand = operand;
            _type = type;
        }

        public override ScalarType PerformCalculation(ScalarType scalar)
        {

            if (_type == typeof(IntegerType))
            {
                if (scalar is NumberType number)
                {
                    if(!number.HasValue())
                    {
                        return number;
                    }
                    else if (number is IntegerType)
                    {
                        return number as IntegerType;
                    }
                    else if (number % 1 == 0)
                    {
                        return new IntegerType(Convert.ToInt64((decimal)number));
                    }
                    throw new Exception("Indatavärdet till cast hade ett felaktigt format.");
                }

                if (scalar is BooleanType boolean)
                {
                    return new IntegerType(boolean.Equals(new BooleanType(true))? 1 : 0);
                }

                if (scalar is StringType indataString)
                {
                    if (int.TryParse(indataString, out int integer))
                    {
                        return new IntegerType(integer);
                    }
                    throw new Exception("Indatasträngen till cast hade ett felaktigt format.");
                }

            }

            if (_type == typeof(NumberType))
            {
                if (scalar is IntegerType integer)
                {
                    return integer as NumberType;
                }
                if (scalar is BooleanType boolean)
                {
                    return new NumberType(boolean.Equals(new BooleanType(true)) ? 1.0m : 0.0m);
                }

                if (scalar is StringType)
                {
                    string trimmedAndValidatedMask = TrimAndValidateNumberMask(_mask);
                    var resultNumber = Convert.ToDecimal(scalar.ToString(), new CultureInfo("en-US"));

                    if (trimmedAndValidatedMask.Contains('.'))
                    {
                        var decimalString = trimmedAndValidatedMask.Split('.').Last();
                        if (decimalString.Contains('*')|| decimalString.Contains('+'))
                        {
                            return new NumberType(resultNumber);
                        }
                        return new NumberType(Math.Round(resultNumber, decimalString.Length));
                    }
                    return new NumberType(Math.Round(resultNumber, 0));
                }
            }

            if (_type == typeof(StringType))
            {
                if(!string.IsNullOrEmpty(_mask))
                {
                    if(scalar is TimePeriodType timePeriodType)
                    {
                        return new StringType(timePeriodType.ToString(_mask.Trim('"')));
                    }
                }
                return new StringType(scalar.ToString());
            }

            if (_type == typeof(BooleanType))
            {
                if (scalar is NumberType number)
                {
                    return new BooleanType(number.Equals(new NumberType(0.0m))? false : true);
                }

                if (scalar is StringType indataString)
                {
                    throw new Exception("String kan inte castas till boolean.");
                }
            }

            if (_type == typeof(TimePeriodType))
            {
                if (scalar is StringType stringType)
                {
                    return CastStringToTimePeriod(stringType);
                }
                if (scalar is DateType dateType)
                {
                    return new TimePeriodType(dateType);
                }
            }

            if(_type == typeof(DateType))
            {
                if (scalar is StringType stringType)
                {
                    return CastStringToDate(stringType);
                }
                if (scalar is TimePeriodType timePeriodType)
                {
                    return new DateType(timePeriodType.ToDateTime());
                }
            }

            throw new Exception("Casting misslyckades");
        }

        private DateType CastStringToDate(StringType stringType)
        {
            int year = 0, month = 0, day = 0;
            var inputString = stringType.ToString();
            var stringIndex = 0;
            var maskIndex = 0;
            string mask = _mask.Trim('"');

            while (stringIndex < inputString.Length && maskIndex < mask.Length)
            {
                switch (mask[maskIndex])
                {
                    case '\\':
                        stringIndex++;
                        maskIndex++;
                        maskIndex++;
                        break;

                    case 'Y':
                        var yearString = new StringBuilder();
                        yearString.Append(inputString[stringIndex]);
                        while (maskIndex + 1 < mask.Length && mask[maskIndex + 1] == 'Y' && stringIndex + 1 < inputString.Length)
                        {
                            stringIndex++;
                            maskIndex++;
                            yearString.Append(inputString[stringIndex]);
                        }
                        stringIndex++;
                        maskIndex++;
                        year += int.Parse(yearString.ToString());
                        if (year < 100)
                        {
                            year += 2000;
                        }
                        break;
                    case 'S':
                        var semesterString = new StringBuilder();
                        semesterString.Append(inputString[stringIndex]);
                        while (maskIndex + 1 < mask.Length && mask[maskIndex + 1] == 'S' && stringIndex + 1 < inputString.Length)
                        {
                            stringIndex++;
                            maskIndex++;
                            semesterString.Append(inputString[stringIndex]);
                        }
                        stringIndex++;
                        maskIndex++;
                        month += int.Parse(semesterString.ToString()) * 6;
                        break;
                    case 'Q':
                        var quarterString = new StringBuilder();
                        quarterString.Append(inputString[stringIndex]);
                        while (maskIndex + 1 < mask.Length && mask[maskIndex + 1] == 'Q' && stringIndex + 1 < inputString.Length)
                        {
                            stringIndex++;
                            maskIndex++;
                            quarterString.Append(inputString[stringIndex]);
                        }
                        stringIndex++;
                        maskIndex++;
                        month += int.Parse(quarterString.ToString()) * 3 - 2;
                        break;
                    case 'M':
                        var monthString = new StringBuilder();
                        monthString.Append(inputString[stringIndex]);
                        while (maskIndex + 1 < mask.Length && mask[maskIndex + 1] == 'M' && stringIndex + 1 < inputString.Length)
                        {
                            stringIndex++;
                            maskIndex++;
                            monthString.Append(inputString[stringIndex]);
                        }
                        stringIndex++;
                        maskIndex++;
                        month += int.Parse(monthString.ToString());
                        break;
                    case 'W':
                        var weekString = new StringBuilder();
                        weekString.Append(inputString[stringIndex]);
                        while (maskIndex + 1 < mask.Length && mask[maskIndex + 1] == 'W' && stringIndex + 1 < inputString.Length)
                        {
                            stringIndex++;
                            maskIndex++;
                            weekString.Append(inputString[stringIndex]);
                        }
                        stringIndex++;
                        maskIndex++;
                        day += int.Parse(weekString.ToString()) * 7;
                        break;
                    case 'D':
                        var dayString = new StringBuilder();
                        dayString.Append(inputString[stringIndex]);
                        while (maskIndex + 1 < mask.Length && mask[maskIndex + 1] == 'D')
                        {
                            stringIndex++;
                            maskIndex++;
                            dayString.Append(inputString[stringIndex]);
                        }
                        stringIndex++;
                        maskIndex++;
                        day += int.Parse(dayString.ToString());
                        break;

                    default:
                        stringIndex++;
                        maskIndex++;
                        break;
                }
            }
            if (month == 0) month = 1;
            if (day == 0) day = 1;
            return new DateType(new DateTime(year, month, day));
        }

        private string TrimAndValidateNumberMask(string mask)
        {
            char[] charsToTrim = { '"', ' ', '\'' };
            string trimmedMask = mask.Trim(charsToTrim);
            ValidateNumberMask(trimmedMask);
            return trimmedMask;
        }

        internal override Type SetDataTypeForMeasureComponent(ComponentType componentType)
        {
            return _type;
        }

        private void ValidateNumberMask(string trimmedMask)
        {
            string pattern = @"^(^[D]([\*\+]|[D]*)([\.][D]([\*\+]|[D]*))?)$";
            Regex rg = new Regex(pattern);

            if (!rg.IsMatch(trimmedMask))
            {
                throw new Exception("Masken är inkorrekt.");
            }
        }

        internal override bool CompatibleDataType(Type dataType)
        {

            if (_type == typeof(IntegerType) &&
                (dataType == typeof(NumberType) || dataType == typeof(BooleanType) || dataType == typeof(StringType))) return true;
            
            if (_type == typeof(NumberType) &&
                (dataType == typeof(IntegerType)|| dataType == typeof(BooleanType) || dataType == typeof(StringType))) return true;

            if (_type == typeof(BooleanType) && 
                (dataType == typeof(NumberType) || dataType == typeof(IntegerType))) return true;

            if (_type == typeof(DateType) &&
                (dataType == typeof(StringType)) || dataType == typeof(TimePeriodType)) return true;

            if (_type == typeof(TimeType) &&
                (dataType == typeof(StringType)) || dataType == typeof(TimePeriodType) || dataType == typeof(DateType)) return true;

            if (_type == typeof(TimePeriodType) &&
                (dataType == typeof(StringType)) || dataType == typeof(DateType)) return true;

            if (_type == typeof(StringType)) return true;
            return false;
        }

        private ScalarType CastStringToTimePeriod(StringType stringType)
        {
            int year = 0, month = 0, day = 0;
            Duration duration = Duration.Annual;
            var inputString = stringType.ToString();
            var stringIndex = 0;
            var maskIndex = 0;
            string mask = _mask.Trim('"');

            while (stringIndex < inputString.Length && maskIndex < mask.Length)
            {
                switch (mask[maskIndex])
                {
                    case '\\':
                        stringIndex++;
                        maskIndex++;
                        maskIndex++;
                        break;

                    case 'Y':
                        var yearString = new StringBuilder();
                        yearString.Append(inputString[stringIndex]);
                        while (maskIndex + 1 < mask.Length && mask[maskIndex + 1] == 'Y' && stringIndex + 1 < inputString.Length)
                        {
                            stringIndex++;
                            maskIndex++;
                            yearString.Append(inputString[stringIndex]);
                        }
                        stringIndex++;
                        maskIndex++;
                        year += int.Parse(yearString.ToString());
                        if (year < 100)
                        {
                            year += 2000;
                        }
                        break;
                    case 'S':
                        var semesterString = new StringBuilder();
                        semesterString.Append(inputString[stringIndex]);
                        while (maskIndex + 1 < mask.Length && mask[maskIndex + 1] == 'S' && stringIndex + 1 < inputString.Length)
                        {
                            stringIndex++;
                            maskIndex++;
                            semesterString.Append(inputString[stringIndex]);
                        }
                        stringIndex++;
                        maskIndex++;
                        month += int.Parse(semesterString.ToString()) * 6;
                        if (duration > Duration.Semester) duration = Duration.Semester;
                        break;
                    case 'Q':
                        var quarterString = new StringBuilder();
                        quarterString.Append(inputString[stringIndex]);
                        while (maskIndex + 1 < mask.Length && mask[maskIndex + 1] == 'Q' && stringIndex + 1 < inputString.Length)
                        {
                            stringIndex++;
                            maskIndex++;
                            quarterString.Append(inputString[stringIndex]);
                        }
                        stringIndex++;
                        maskIndex++;
                        month += int.Parse(quarterString.ToString()) * 3;
                        if (duration > Duration.Quarter) duration = Duration.Quarter;
                        break;
                    case 'M':
                        var monthString = new StringBuilder();
                        monthString.Append(inputString[stringIndex]);
                        while (maskIndex + 1 < mask.Length && mask[maskIndex + 1] == 'M' && stringIndex + 1 < inputString.Length)
                        {
                            stringIndex++;
                            maskIndex++;
                            monthString.Append(inputString[stringIndex]);
                        }
                        stringIndex++;
                        maskIndex++;
                        if (monthString.Length<2) throw new VtlException("Masken stämmer inte för värdet som castas.", new Exception(), null);
                        month += int.Parse(monthString.ToString());
                        if (duration > Duration.Month) duration = Duration.Month;
                        break;
                    case 'W':
                        var weekString = new StringBuilder();
                        weekString.Append(inputString[stringIndex]);
                        while (maskIndex + 1 < mask.Length && mask[maskIndex + 1] == 'W' && stringIndex + 1 < inputString.Length)
                        {
                            stringIndex++;
                            maskIndex++;
                            weekString.Append(inputString[stringIndex]);
                        }
                        stringIndex++;
                        maskIndex++;
                        day += int.Parse(weekString.ToString()) * 7;
                        if (duration > Duration.Week) duration = Duration.Week;
                        break;
                    case 'D':
                        var dayString = new StringBuilder();
                        dayString.Append(inputString[stringIndex]);
                        while (maskIndex + 1 < mask.Length && mask[maskIndex + 1] == 'D' )
                        {
                            stringIndex++;
                            maskIndex++;
                            dayString.Append(inputString[stringIndex]);
                        }
                        stringIndex++;
                        maskIndex++;
                        day += int.Parse(dayString.ToString());
                        if (duration > Duration.Day) duration = Duration.Day;
                        break;

                    default:
                        stringIndex++;
                        maskIndex++;
                        break;
                }
            }
            if (month == 0) month = 1;
            if (day == 0) day = 1;
            return new TimePeriodType(new DateTime(year, month, day), duration);
        }

    }
}

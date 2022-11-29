using System;
using System.Text;

namespace VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes
{
    public class TimePeriodType : BasicScalarType<Tuple<int, Duration, int>>
    {
        public TimePeriodType(int year, Duration duration, int number)
        {
            if (duration == Duration.Annual)
            {
                Value = new Tuple<int, Duration, int>(year, duration, 0);
            }
            else
            {
                Value = new Tuple<int, Duration, int>(year, duration, number);
            }
        }

        public int Year { get { return Value.Item1; } }

        public Duration Duration { get { return Value.Item2; } }

        public int Number { get { return Value.Item3; } }

        public TimePeriodType(string timePeriodString)
        {
            if(!int.TryParse(timePeriodString.Substring(0, 4), out var year))
            {
                throw new Exception($"Time period string {timePeriodString} must start with a four number year.");
            }
            if(timePeriodString.Length < 5)
            {
                throw new Exception($"Time period string {timePeriodString} must have a period specifier.");
            }
            int offset = 4;
            if(timePeriodString[offset] == '-')
            {
                offset++;
            }
            var period = timePeriodString.Substring(offset, 1).ToUpper();
            if(period == "A")
            {
                Value = new Tuple<int, Duration, int>(year, Duration.Annual, 0);
                return;
            }
            if (!int.TryParse(timePeriodString.Substring(offset + 1), out var number))
            {
                throw new Exception($"Time period string {timePeriodString} must end with a number.");
            }

            switch(period)
            {
                case "D":
                    var daysOfYear = DateTime.IsLeapYear(year) ? 366 : 365;
                    while(number > daysOfYear)
                    {
                        number -= daysOfYear;
                        year++;
                        daysOfYear = DateTime.IsLeapYear(year) ? 366 : 365;
                    }                    
                    Value = new Tuple<int, Duration, int>(year, Duration.Day, number);
                    return;
                case "W":
                    Value = new Tuple<int, Duration, int>(year + (number - 1) / 52, Duration.Week, (number - 1) % 52 + 1);
                    return;
                case "M":
                    Value = new Tuple<int, Duration, int>(year + (number - 1) / 12, Duration.Month, (number - 1) % 12 + 1);
                    return;
                case "Q":
                    Value = new Tuple<int, Duration, int>(year + (number - 1) / 4, Duration.Quarter, (number - 1) % 4 + 1);
                    return;
                case "S":
                    Value = new Tuple<int, Duration, int>(year + (number - 1) / 2, Duration.Semester, (number - 1) % 2 + 1);
                    return;
                default:
                    throw new Exception($"{period} är inte en giltig periodspecifierare för time_period.");
            }

        }

        internal DateTime ToDateTime()
        {
            switch (Duration)
            {
                case Duration.Day:
                    return new DateTime(Year, 0, Number);
                case Duration.Week:
                    return new DateTime(Year, 0, Number * 7);
                case Duration.Month:
                    return new DateTime(Year, Number, 1);
                case Duration.Quarter:
                    return new DateTime(Year, Number * 3, 1);
                case Duration.Semester:
                    return new DateTime(Year, Number * 6, 1);
                case Duration.Annual:
                    return new DateTime(Year, 1, 1);
                default:
                    throw new Exception($"{Duration} är inte en giltig periodspecifierare för time_period.");
            }
        }

        internal TimeType ToTimeType()
        {
            switch (Duration)
            {
                case Duration.Day:
                    return new TimeType(new DateTime(Year, 0, Number), new DateTime(Year, 0, Number));
                case Duration.Week:
                    return new TimeType(new DateTime(Year, 0, Number * 7), new DateTime(Year, 0, Number * 7 + 6));
                case Duration.Month:
                    return new TimeType(new DateTime(Year, Number, 1), new DateTime(Year, Number, DateTime.DaysInMonth(Year, Number)));
                case Duration.Quarter:
                    return new TimeType(new DateTime(Year, Number * 3 - 2, 1), new DateTime(Year, Number * 3, DateTime.DaysInMonth(Year, Number * 3)));
                case Duration.Semester:
                    return new TimeType(new DateTime(Year, Number * 6 - 5, 1), new DateTime(Year, Number * 6, DateTime.DaysInMonth(Year, Number * 6)));
                case Duration.Annual:
                    return new TimeType(new DateTime(Year, 1, 1), new DateTime(Year, 12, 31));
                default:
                    throw new Exception($"{Duration} är inte en giltig periodspecifierare för time_period.");
            }
        }

        public TimePeriodType(DateTime date, Duration duration)
        {
            switch (duration)
            {
                case Duration.Day:
                    Value = new Tuple<int, Duration, int>(date.Year, duration, date.DayOfYear);
                    break;
                case Duration.Week:
                    Value = new Tuple<int, Duration, int>(date.Year, duration, (int)Math.Ceiling(date.DayOfYear/7.0));
                    break;
                case Duration.Month:
                    Value = new Tuple<int, Duration, int>(date.Year, duration, date.Month);
                    break;
                case Duration.Quarter:
                    Value = new Tuple<int, Duration, int>(date.Year, duration, (int)Math.Ceiling(date.Month/3.0));
                    break;
                case Duration.Semester:
                    Value = new Tuple<int, Duration, int>(date.Year, duration, (int)Math.Ceiling(date.Month/6.0));
                    break;
                case Duration.Annual:
                    Value = new Tuple<int, Duration, int>(date.Year, duration, 0);
                    break;
            }
        }

        public TimePeriodType(DateType dateType)
        {
            var dateTime = (DateTime)dateType;
            Value = new Tuple<int, Duration, int>(dateTime.Year, Duration.Day, dateTime.DayOfYear);
        }

        public static TimePeriodType operator +(TimePeriodType a, IntegerType b)
        {
            var year = a.Value.Item1;
            var period = a.Value.Item2;
            var number = a.Value.Item3 + (int)b;

            switch (period)
            {
                case Duration.Day:
                    if (b >= 0)
                    {
                        var daysOfYear = DateTime.IsLeapYear(year) ? 366 : 365;
                        while (number > daysOfYear)
                        {
                            number -= daysOfYear;
                            year++;
                            daysOfYear = DateTime.IsLeapYear(year) ? 366 : 365;
                        }
                        return new TimePeriodType(year, Duration.Day, number);
                    }
                    else
                    {
                        var daysOfYear = DateTime.IsLeapYear(year-1) ? 366 : 365;
                        while (number <= 0)
                        {
                            number += daysOfYear;
                            year--;
                            daysOfYear = DateTime.IsLeapYear(year-1) ? 366 : 365;
                        }
                        return new TimePeriodType(year, Duration.Day, number);
                    }
                case Duration.Week:
                    return new TimePeriodType(year + div(number - 1, 52), Duration.Week, mod(number - 1, 52) + 1);
                case Duration.Month:
                    return new TimePeriodType(year + div(number - 1, 12), Duration.Month, mod(number - 1, 12) + 1);
                case Duration.Quarter:
                    return new TimePeriodType(year + div(number - 1, 4), Duration.Quarter, mod(number - 1, 4) + 1);
                case Duration.Semester:
                    return new TimePeriodType(year + div(number - 1, 2), Duration.Semester, mod(number - 1, 2) + 1);
                case Duration.Annual:
                    return new TimePeriodType(year + number, Duration.Annual, 0);
                default:
                    throw new Exception($"{period} är inte en giltig periodspecifierare för time_period.");
            }
        }

        private static int mod(int x, int m)
        {
            return (x % m + m) % m;
        }

        private static int div(int a, int b)
        {
            int res = a / b;
            return (a < 0 && a != b * res) ? res - 1 : res;
        }

        public override ScalarType Clone()
        {
            return new TimePeriodType(Value.Item1, Value.Item2, Value.Item3);
        }

        public override int CompareTo(object obj)
        {
            var timePeriodObj = obj as TimePeriodType;
            return ToDateTime().CompareTo(timePeriodObj?.ToDateTime());
        }

        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }

            if (obj is TimePeriodType timePeriod)
            {
                if (Year != timePeriod.Year)
                {
                    return false;
                }
                if (Duration != timePeriod.Duration)
                {
                    return false;
                }
                if (Number != timePeriod.Number)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public static bool operator ==(TimePeriodType obj1, TimePeriodType obj2)
        {
            if (ReferenceEquals(null, obj1))
            {
                return ReferenceEquals(null, obj2);
            }
            if (ReferenceEquals(null, obj2))
            {
                return ReferenceEquals(null, obj1);
            }
            return obj1.Equals(obj2);
        }

        public static bool operator !=(TimePeriodType obj1, TimePeriodType obj2)
        {
            return !(obj1 == obj2);
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.Append(Value.Item1);
            result.Append("-");
            switch(Value.Item2)
            {
                case Duration.Day:
                    result.Append("D");
                    result.Append(Value.Item3.ToString("000"));
                    break;
                case Duration.Week:
                    result.Append("W");
                    result.Append(Value.Item3.ToString("00"));
                    break;
                case Duration.Month:
                    result.Append("M");
                    result.Append(Value.Item3.ToString("00"));
                    break;
                case Duration.Quarter:
                    result.Append("Q");
                    result.Append(Value.Item3.ToString("0"));
                    break;
                case Duration.Semester:
                    result.Append("S");
                    result.Append(Value.Item3.ToString("0"));
                    break;
                case Duration.Annual:
                    result.Append("A1");
                    break;
            }
            return result.ToString();
        }

        public string ToString(string mask)
        {

            var resultString = new StringBuilder();

            var maskIndex = 0;
            bool optional = false;

            while (maskIndex < mask.Length)
            {
                switch (mask[maskIndex])
                {
                    case 'Y':
                        int yCount = 1;
                        while (maskIndex + 1 < mask.Length && mask[maskIndex + 1] == 'Y')
                        {
                            yCount++;
                            maskIndex++;
                        }
                        var yearString = Value.Item1.ToString();
                        if(yearString.Length < yCount)
                        {
                            yCount = yearString.Length;
                        }
                        resultString.Append(yearString.Substring(yearString.Length - yCount));
                        break;
                    case 'P':
                        switch (Value.Item2)
                        {
                            case Duration.Day:
                                resultString.Append("D");
                                break;
                            case Duration.Week:
                                resultString.Append("W");
                                break;
                            case Duration.Month:
                                resultString.Append("M");
                                break;
                            case Duration.Quarter:
                                resultString.Append("Q");
                                break;
                            case Duration.Semester:
                                resultString.Append("S");
                                break;
                            case Duration.Annual:
                                resultString.Append("A");
                                break;
                        }
                        break;
                    case '{':
                        optional = true;
                        break;

                    case 'p':
                        resultString.Append(makeNumberString('p', mask, ref maskIndex, ref optional));
                        break;
                    case 'D':
                        if (Value.Item2 != Duration.Day)
                        {
                            throw new Exception($"D kan inte användas för att konvertera en time_period med perioden {Value.Item2}");
                        }
                        resultString.Append(makeNumberString('D', mask, ref maskIndex, ref optional));
                        break;
                    case 'W':
                        if (Value.Item2 != Duration.Week)
                        {
                            throw new Exception($"W kan inte användas för att konvertera en time_period med perioden {Value.Item2}");
                        }
                        resultString.Append(makeNumberString('W', mask, ref maskIndex, ref optional));
                        break;
                    case 'M':
                        if (Value.Item2 != Duration.Month)
                        {
                            throw new Exception($"M kan inte användas för att konvertera en time_period med perioden {Value.Item2}");
                        }
                        var optionalcheck = optional;
                        var monthstring = makeNumberString('M', mask, ref maskIndex, ref optional);
                        if (monthstring.Length < 2 && !optionalcheck) throw new VtlException("Masken stämmer inte för värdet som castas.", new Exception(), "");
                        resultString.Append(monthstring);
                        break;
                    case 'Q':
                        if (Value.Item2 != Duration.Quarter)
                        {
                            throw new Exception($"Q kan inte användas för att konvertera en time_period med perioden {Value.Item2}");
                        }
                        resultString.Append(makeNumberString('Q', mask, ref maskIndex, ref optional));
                        break;
                    case '\\':
                        maskIndex++;
                        resultString.Append(mask[maskIndex]);
                        break;
                    default:
                        resultString.Append(mask[maskIndex]);
                        break;
                }
                maskIndex++;
            }

            return resultString.ToString();
        }

        private string makeNumberString(char letter, string mask, ref int maskIndex, ref bool optional)
        {

            int optionalPCount = 0;
            int pCount = 0;
            do
            {
                if (optional)
                    optionalPCount++;
                else
                    pCount++;
                maskIndex++;
                if (maskIndex + 1 < mask.Length && mask[maskIndex] == '}')
                {
                    optional = false;
                    maskIndex++;
                }
            }
            while (maskIndex < mask.Length && mask[maskIndex] == letter);

            var numberString = Value.Item3.ToString();
            if (numberString.Length > pCount)
            {
                return numberString.Substring(0, Math.Min(pCount + optionalPCount, numberString.Length));
            }
            else
            {
                return Value.Item3.ToString($"D{pCount}");
            }

        }

        public override int GetHashCode()
        {
            int hashCode = 641310951;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Year.GetHashCode();
            hashCode = hashCode * -1521134295 + Duration.GetHashCode();
            hashCode = hashCode * -1521134295 + Number.GetHashCode();
            return hashCode;
        }
    }
}

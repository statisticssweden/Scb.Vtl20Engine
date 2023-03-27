using System;
using System.Text;

namespace VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes
{
    public class DateType : BasicScalarType<DateTime?>
    {

        public DateType(DateTime? value) => Value = value;
        public static implicit operator DateTime?(DateType b) => b.Value;
        public static implicit operator DateType(DateTime? b) => new DateType(b);

        public override ScalarType Clone()
        {
            return new DateType(Value);
        }

        public override int CompareTo(object obj)
        {
            var dateObj = obj as DateType;
            if (dateObj.HasValue() && Value.HasValue)
            {
                return Value.Value.CompareTo(dateObj.Value);
            }
            return HasValue() ? 1 : dateObj.HasValue() ? -1 : 0;
        }

        public override string ToString()
        {
            if (Value.HasValue)
            {
                return Value.Value.ToString("yyyyMMdd-hh:mm");
            }
            return "Null";
        }
        public string ToString(string mask)
        {
            if (Value.HasValue)
            {
                var resultString = new StringBuilder();
                var maskIndex = 0;
                
                while (maskIndex < mask.Length)
                {
                    switch (mask[maskIndex])
                    {
                        case'Y':
                            resultString.Append('y');
                            break;
                        case 'D':
                            resultString.Append('d');
                            break;
                        case 'h':
                            resultString.Append('H');
                            break;
                        default:
                            resultString.Append(mask[maskIndex]);
                            break;
                    }
                    maskIndex++;
                }
                return Value.Value.ToString(resultString.ToString());
            }
            return "Null";
        }

        public static TimeSpan operator -(DateType a, DateType b)
        {
            return a.Value.Value - b.Value.Value;
        }
        public static DateType operator +(DateType a, TimeSpan b)
        {
            return a.Value.Value + b;
        }

        public void AddDay()
        {
            Value = Value.Value.AddDays(1);
        }

        public void AddWeek()
        {
            Value = Value.Value.AddDays(7);
        }

        public void AddMonth()
        {
            Value = Value.Value.AddMonths(1);
        }

        public void AddQuarter()
        {
            Value = Value.Value.AddMonths(3);
        }

        public void AddSemester()
        {
            Value = Value.Value.AddMonths(6);
        }

        public void AddYear()
        {
            Value = Value.Value.AddYears(1);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is DateType date)
            {
                return Value == date.Value;
            }

            return false;
        }

        public static bool operator ==(DateType obj1, DateType obj2)
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

        public static bool operator !=(DateType obj1, DateType obj2)
        {
            return !(obj1 == obj2);
        }


    }
}

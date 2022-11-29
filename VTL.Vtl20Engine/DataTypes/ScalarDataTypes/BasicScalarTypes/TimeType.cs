using System;

namespace VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes
{
    public class TimeType : BasicScalarType<Tuple<DateTime, DateTime>>
    {
        public TimeType(Tuple<DateTime, DateTime> value) => Value = value;

        public TimeType(DateTime start, DateTime end) => Value = new Tuple<DateTime, DateTime>(start, end);

        public DateTime Start { get { return Value.Item1; } }

        public DateTime End { get { return Value.Item2; } }

        public TimeSpan Duration { get { return End - Start; } }

        public TimeType(string timeString)
        {
            var errorMessage = "Time string must be expressed YYYY-MM-DD/YYYY-MM-DD.";
            if (timeString[4] != '-' ||
                timeString[4] != '-' ||
                timeString[7] != '-' ||
                timeString[10] != '/' ||
                timeString[15] != '-' ||
                timeString[18] != '-' ||
               !int.TryParse(timeString.Substring(0, 4), out var fromYear) ||
               !int.TryParse(timeString.Substring(5, 2), out var fromMonth) ||
               !int.TryParse(timeString.Substring(8, 2), out var fromDay) ||
               !int.TryParse(timeString.Substring(11, 4), out var untilYear) ||
               !int.TryParse(timeString.Substring(16, 2), out var untilMonth) ||
               !int.TryParse(timeString.Substring(19, 2), out var untilDay))
            {
                throw new Exception(errorMessage);
            }
            Value = new Tuple<DateTime, DateTime>
                (new DateTime(fromYear, fromMonth, fromDay),
                new DateTime(untilYear, untilMonth, untilDay));
        }

        public override ScalarType Clone()
        {
            return new TimeType(new Tuple<DateTime, DateTime>(Value.Item1, Value.Item2));
        }

        public override int CompareTo(object obj)
        {
            var time = obj as TimeType;
            if (time == null)
            {
                throw new Exception($"TimeType can not be compared to {obj.GetType().Name}");
            }
            var compare1 = Value.Item1.CompareTo(time.Value.Item1);
            if (compare1 != 0)
            {
                return compare1;
            }

            return Value.Item2.CompareTo(time.Value.Item2);
        }

        public override string ToString()
        {
            return $"{Value.Item1:yyyy-MM-dd}/{Value.Item2:yyyy-MM-dd}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.TimeOperator
{
    public class FillTimeSeries : Operator
    {
        private Operand _op { get; }

        private bool _single { get; }

        public FillTimeSeries(Operand op, bool single)
        {
            _op = op;
            _single = single;
        }

        internal override DataType PerformCalculation()
        {
            var dataset = _op.GetValue() as DataSetType;
            if (dataset == null)
            {
                throw new Exception("fill_time_series kräver ett dataset som inparameter.");
            }

            ComponentType timeIdentifier = null;

            try
            {
                timeIdentifier = dataset.DataSetComponents.Single(c => c.Role == ComponentType.ComponentRole.Identifier &&
                    (c.DataType == typeof(TimeType) || c.DataType == typeof(TimePeriodType) || c.DataType == typeof(DateType)));
            }
            catch
            {
                throw new Exception("Datasetet som skickas med till fill_time_series måste ha EN identifier med datatypen Time, TimePeriod eller Date.");
            }

            if (timeIdentifier.DataType == typeof(TimePeriodType))
            {
                return PerformCalculationWithTimePeriod(dataset, timeIdentifier);
            }
            else if (timeIdentifier.DataType == typeof(TimeType))
            {
                return PerformCalculationWithTime(dataset, timeIdentifier);
            }
            else if (timeIdentifier.DataType == typeof(DateType))
            {
                return PerformCalculationWithDate(dataset, timeIdentifier);
            }
            throw new NotImplementedException();
        }

        private DataType PerformCalculationWithTimePeriod(DataSetType dataset, ComponentType timeIdentifier)
        {
            // Gå igenom ds och plocka ut start/stop för varje tidsserie
            TimePeriodType startLimit = null;
            TimePeriodType endLimit = null;
            var durations = new List<Duration>();
            var timeComponentIndex = dataset.IndexOfComponent(timeIdentifier);
            var measureIndexes = dataset.DataSetComponents.
                Where(c => c.Role == ComponentType.ComponentRole.Measure).Select(m => dataset.IndexOfComponent(m));

            using (var timeSeriesEnumerator = dataset.GetDataPointEnumerator())
            {
                while (timeSeriesEnumerator.MoveNext())
                {
                    var currentTimeSample = timeSeriesEnumerator.Current[timeComponentIndex] as TimePeriodType;
                    if (!durations.Contains(currentTimeSample.Duration))
                    {
                        durations.Add(currentTimeSample.Duration);
                    }
                    if (startLimit == null)
                    {
                        startLimit = currentTimeSample;
                        endLimit = currentTimeSample;
                    }
                    else if (currentTimeSample > endLimit)
                    {
                        endLimit = currentTimeSample;
                    }
                    else if (currentTimeSample < startLimit)
                    {
                        startLimit = currentTimeSample;
                    }
                }
            }

            // sortera på alla id utom tidsserie
            var sort = dataset.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier &&
                c != timeIdentifier).Select(c => c.Name).ToList();
            sort.Add(timeIdentifier.Name);
            dataset.SortDataPoints(sort.ToArray());
            timeComponentIndex = dataset.IndexOfComponent(timeIdentifier);

            // gå igenom varje tidpunkt för varje unik (utom tidsserie) datapunkt

            var result = new DataSetType(dataset.DataSetComponents);
            var dataPointComparer = new DataPointComparer(Enumerable.Range(0, result.DataSetComponents.
                Count(c => c.Role == ComponentType.ComponentRole.Identifier) - 1).ToArray());

            TimePeriodType expectedTime = null;
            DataPointType lastDatapoint = null;

            foreach (var duration in durations)
            {
                expectedTime = null;
                lastDatapoint = null;
                using (var dataPointEnumerator = dataset.GetDataPointEnumerator())
                {
                    while (dataPointEnumerator.MoveNext())
                    {
                        var currentTimePeriod = dataPointEnumerator.Current[timeComponentIndex] as TimePeriodType;
                        if (currentTimePeriod.Duration == duration)
                        {
                            if (lastDatapoint == null)
                            {
                                expectedTime = null;
                            }
                            else if (dataPointComparer.Compare(dataPointEnumerator.Current, lastDatapoint) != 0)
                            {
                                if (!_single)
                                {
                                    // skriv datapunkter kvarvarande till endLimit till resultatet
                                    while (expectedTime.ToTimeType().End <= endLimit.ToTimeType().End)
                                    {
                                        result.Add(MakeNullDataPoint(lastDatapoint, measureIndexes, timeComponentIndex, expectedTime));
                                        expectedTime = expectedTime + 1;
                                    }
                                }
                                expectedTime = null;
                            }

                            if (expectedTime == null)
                            {
                                var currentTime = dataPointEnumerator.Current[timeComponentIndex] as TimePeriodType;
                                expectedTime = _single ? currentTime : new TimePeriodType(startLimit.ToDateTime(), currentTime.Duration);
                            }
                            while (currentTimePeriod > expectedTime)
                            {
                                result.Add(MakeNullDataPoint(dataPointEnumerator.Current, measureIndexes, timeComponentIndex, expectedTime));
                                expectedTime = expectedTime + 1;
                            }
                            if (currentTimePeriod.Duration == expectedTime.Duration && currentTimePeriod.Year == expectedTime.Year &&
                                (currentTimePeriod.Duration == Duration.Annual || currentTimePeriod.Number == expectedTime.Number))
                            {
                                result.Add(dataPointEnumerator.Current);
                            }
                            lastDatapoint = dataPointEnumerator.Current;
                            expectedTime = expectedTime + 1;
                        }
                    }
                }
            }

            if (!_single)
            {
                // skriv datapunkter kvarvarande till endLimit till resultatet
                while (expectedTime.ToTimeType().End <= endLimit.ToTimeType().End)
                {
                    result.Add(MakeNullDataPoint(lastDatapoint, measureIndexes, timeComponentIndex, expectedTime));
                    expectedTime = expectedTime + 1;
                }
            }
            return result;
        }

        private DataType PerformCalculationWithTime(DataSetType dataset, ComponentType timeIdentifier)
        {
            // Gå igenom ds och plocka ut start/stop för varje tidsserie
            DateTime? startLimit = null;
            DateTime? endLimit = null;
            var durations = new List<Duration>();
            var timeComponentIndex = dataset.IndexOfComponent(timeIdentifier);
            var measureIndexes = dataset.DataSetComponents.
                Where(c => c.Role == ComponentType.ComponentRole.Measure).Select(m => dataset.IndexOfComponent(m));

            using (var timeSeriesEnumerator = dataset.GetDataPointEnumerator())
            {
                while (timeSeriesEnumerator.MoveNext())
                {
                    var currentTimeSample = timeSeriesEnumerator.Current[timeComponentIndex] as TimeType;
                    var duration = GetDuration(currentTimeSample);
                    if (!durations.Contains(duration))
                    {
                        durations.Add(duration);
                    }
                    if (startLimit == null)
                    {
                        startLimit = currentTimeSample.Start;
                        endLimit = currentTimeSample.End;
                    }
                    else if (currentTimeSample.End > endLimit)
                    {
                        endLimit = currentTimeSample.End;
                    }
                    else if (currentTimeSample.Start < startLimit)
                    {
                        startLimit = currentTimeSample.Start;
                    }
                }
            }

            // sortera på alla id utom tidsserie
            var sort = dataset.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier &&
                c != timeIdentifier).Select(c => c.Name).ToList();
            sort.Add(timeIdentifier.Name);
            dataset.SortDataPoints(sort.ToArray());
            timeComponentIndex = dataset.IndexOfComponent(timeIdentifier);

            // gå igenom varje tidpunkt för varje unik (utom tidsserie) datapunkt

            var result = new DataSetType(dataset.DataSetComponents);
            var dataPointComparer = new DataPointComparer(Enumerable.Range(0, result.DataSetComponents.
                Count(c => c.Role == ComponentType.ComponentRole.Identifier) - 1).ToArray());

            DateTime? expectedTime = null;
            DataPointType lastDatapoint = null;

            foreach (var duration in durations)
            {
                expectedTime = null;
                lastDatapoint = null;
                using (var dataPointEnumerator = dataset.GetDataPointEnumerator())
                {
                    while (dataPointEnumerator.MoveNext())
                    {
                        var currentTimePeriod = dataPointEnumerator.Current[timeComponentIndex] as TimeType;
                        if (GetDuration(currentTimePeriod) == duration)
                        {
                            if (lastDatapoint == null)
                            {
                                expectedTime = null;
                            }
                            else if (dataPointComparer.Compare(dataPointEnumerator.Current, lastDatapoint) != 0)
                            {
                                if (!_single)
                                {
                                    // skriv datapunkter kvarvarande till endLimit till resultatet
                                    while (expectedTime <= endLimit)
                                    {
                                        var nextTime = AddDuration(expectedTime, duration);
                                        result.Add(MakeNullDataPoint(lastDatapoint, measureIndexes, timeComponentIndex, 
                                            new TimeType(expectedTime.Value, nextTime.AddDays(-1))));
                                        expectedTime = nextTime;
                                    }
                                }
                                expectedTime = null;
                            }

                            if (expectedTime == null)
                            {
                                var currentTime = dataPointEnumerator.Current[timeComponentIndex] as TimeType;
                                expectedTime = _single ? currentTime.Start : startLimit;
                            }
                            while (currentTimePeriod.Start > expectedTime)
                            {
                                var nextTime = AddDuration(expectedTime, duration);
                                result.Add(MakeNullDataPoint(dataPointEnumerator.Current, measureIndexes, timeComponentIndex,
                                            new TimeType(expectedTime.Value, nextTime.AddDays(-1))));
                                expectedTime = nextTime;
                            }
                            if (expectedTime >= currentTimePeriod.Start && expectedTime <= currentTimePeriod.End)
                            {
                                result.Add(dataPointEnumerator.Current);
                            }
                            lastDatapoint = dataPointEnumerator.Current;
                            expectedTime = AddDuration(expectedTime, duration);
                        }
                    }
                }
            }

            if (!_single)
            {
                // skriv datapunkter kvarvarande till endLimit till resultatet
                while (expectedTime <= endLimit)
                {
                    var nextTime = AddDuration(expectedTime, durations.Last());
                    result.Add(MakeNullDataPoint(lastDatapoint, measureIndexes, timeComponentIndex,
                        new TimeType(expectedTime.Value, nextTime.AddDays(-1))));
                    expectedTime = nextTime;
                }
            }
            return result;
        }

        private DateTime AddDuration(DateTime? date, Duration duration)
        {
            switch(duration)
            {
                case Duration.Day:
                    return date.Value.AddDays(1);
                case Duration.Week:
                    return date.Value.AddDays(7);
                case Duration.Month:
                    return date.Value.AddMonths(1);
                case Duration.Quarter:
                    return date.Value.AddMonths(3);
                case Duration.Semester:
                    return date.Value.AddMonths(6);
                default:
                    return date.Value.AddYears(1);
            }
        }

        private Duration GetDuration(TimeType time)
        {
            if(time.Duration <= new TimeSpan(1, 0, 0, 0))
            {
                return Duration.Day;
            }
            else if (time.Duration <= new TimeSpan(7, 0, 0, 0))
            {
                return Duration.Week;
            }
            else if (time.Duration <= new TimeSpan(31, 0, 0, 0))
            {
                return Duration.Month;
            }
            else if (time.Duration <= new TimeSpan(3*31, 0, 0, 0))
            {
                return Duration.Quarter;
            }
            else if (time.Duration <= new TimeSpan(6*31, 0, 0, 0))
            {
                return Duration.Semester;
            }
            else
            {
                return Duration.Annual;
            }
        }

        private DataType PerformCalculationWithDate(DataSetType dataset, ComponentType timeIdentifier)
        {
            // Sortera dataset på tidsvariabel
            dataset.SortDataPoints(new[] { timeIdentifier.Name });

            // Hämta minsta tidsintervall i serien samt plocka ut start/stop
            DateType startLimit = null;
            DateType endLimit = null;
            var minDuration = new TimeSpan(1000, 0, 0, 0, 0);
            var timeComponentIndex = dataset.IndexOfComponent(timeIdentifier);
            var measureIndexes = dataset.DataSetComponents.
                Where(c => c.Role == ComponentType.ComponentRole.Measure).Select(m => dataset.IndexOfComponent(m));
            DateType lastTimeSample = null;

            using (var timeSeriesEnumerator = dataset.GetDataPointEnumerator())
            {
                while (timeSeriesEnumerator.MoveNext())
                {
                    var currentTimeSample = timeSeriesEnumerator.Current[timeComponentIndex] as DateType;
                    if (lastTimeSample != null)
                    {
                        var duration = currentTimeSample - lastTimeSample;
                        if(duration < new TimeSpan(1, 0, 0))
                        {
                            continue;
                        }
                        if (duration < minDuration)
                        {
                            minDuration = duration;
                        }
                    }
                    else
                    {
                        startLimit = currentTimeSample;
                    }
                    lastTimeSample = currentTimeSample;
                }
                endLimit = lastTimeSample;
            }

            Action<DateType> advance;
            if (minDuration <= new TimeSpan(1, 0, 0, 0))
            {
                advance = (DateType currentTime) => currentTime.AddDay();
            }
            else if (minDuration <= new TimeSpan(7, 0, 0, 0))
            {
                advance = (DateType currentTime) => currentTime.AddWeek();
            }
            else if (minDuration <= new TimeSpan(31, 0, 0, 0))
            {
                advance = (DateType currentTime) => currentTime.AddMonth();
            }
            else if (minDuration <= new TimeSpan(31 * 3, 0, 0, 0))
            {
                advance = (DateType currentTime) => currentTime.AddQuarter();
            }
            else if (minDuration <= new TimeSpan(31 * 6, 0, 0, 0))
            {
                advance = (DateType currentTime) => currentTime.AddSemester();
            }
            else
            {
                advance = (DateType currentTime) => currentTime.AddYear();
            }

            // sortera på alla id utom tidsserie
            var sort = dataset.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier &&
                c != timeIdentifier).Select(c => c.Name).ToList();
            sort.Add(timeIdentifier.Name);
            dataset.SortDataPoints(sort.ToArray());
            timeComponentIndex = dataset.IndexOfComponent(timeIdentifier);

            // gå igenom varje tidpunkt för varje unik (utom tidsserie) datapunkt
            var result = new DataSetType(dataset.DataSetComponents);
            var dataPointComparer = new DataPointComparer(Enumerable.Range(0, result.DataSetComponents.
                Count(c => c.Role == ComponentType.ComponentRole.Identifier) - 1).ToArray());

            DateType expectedTime = null;
            DataPointType lastDatapoint = null;

            expectedTime = null;
            lastDatapoint = null;
            using (var dataPointEnumerator = dataset.GetDataPointEnumerator())
            {
                while (dataPointEnumerator.MoveNext())
                {
                    var currentTimePeriod = dataPointEnumerator.Current[timeComponentIndex] as DateType;

                    if (lastDatapoint != null && dataPointComparer.Compare(dataPointEnumerator.Current, lastDatapoint) != 0)
                    {
                        if (!_single)
                        {
                            // skriv datapunkter kvarvarande till endLimit till resultatet
                            while (expectedTime <= endLimit)
                            {
                                result.Add(MakeNullDataPoint(lastDatapoint, measureIndexes, timeComponentIndex, expectedTime.Clone() as DateType));
                                advance(expectedTime);
                            }
                        }
                        expectedTime = null;
                    }

                    if (expectedTime == null)
                    {
                        var currentTime = dataPointEnumerator.Current[timeComponentIndex] as DateType;
                        expectedTime = _single ? currentTime.Clone() as DateType : startLimit.Clone() as DateType;
                    }
                    while (currentTimePeriod > expectedTime)
                    {
                        result.Add(MakeNullDataPoint(dataPointEnumerator.Current, measureIndexes, timeComponentIndex, expectedTime.Clone() as DateType));
                        advance(expectedTime);
                    }
                    if (currentTimePeriod == expectedTime)
                    {
                        result.Add(dataPointEnumerator.Current);
                    }
                    lastDatapoint = dataPointEnumerator.Current;
                    advance(expectedTime);
                }
            }

            if (!_single)
            {
                // skriv datapunkter kvarvarande till endLimit till resultatet
                while (expectedTime <= endLimit)
                {
                    result.Add(MakeNullDataPoint(lastDatapoint, measureIndexes, timeComponentIndex, expectedTime.Clone() as DateType));
                    advance(expectedTime);
                }
            }
            return result;
        }


        private static DataPointType MakeNullDataPoint(DataPointType templateDatapoint, IEnumerable<int> measureIndexes, int timeComponentIndex, ScalarType timeValue)
        {
            var nullDp = templateDatapoint.Clone() as DataPointType;
            nullDp[timeComponentIndex] = timeValue;

            foreach (var i in measureIndexes)
            {
                nullDp[i] = new NullType();
            }

            return nullDp;
        }

        internal override string[] GetComponentNames()
        {
            return _op.GetComponentNames();
        }

        internal override string[] GetIdentifierNames()
        {
            return _op.GetIdentifierNames();
        }

        internal override string[] GetMeasureNames()
        {
            return _op.GetMeasureNames();
        }

    }
}

using System;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.TimeOperator
{
    internal abstract class TimeAggBase : Operator
    {
        protected Duration? _periodIndTo, _periodIndFrom;
        protected Operand _op;
        protected bool _last;

        protected ScalarType PerformCalculation(ScalarType scalar)
        {
            if(scalar is TimePeriodType timePeriod)
            {
                if (timePeriod.Duration >= _periodIndTo)
                {
                    throw new Exception("Aggregering kan bara göras till en grövre tidsnivå.");
                }
                var d = timePeriod.ToDateTime();

                return new TimePeriodType(d, _periodIndTo.Value);
            }
            if(scalar is DateType date)
            {
                var dateTime = (DateTime)date;
                switch (_periodIndTo)
                {
                    case Duration.Annual:
                        return _last ?
                            new DateType(new DateTime(dateTime.Year, 12, 31)) :
                            new DateType(new DateTime(dateTime.Year, 1, 1));
                    case Duration.Semester:
                        var s = (int)Math.Ceiling(dateTime.Month / 6.0);
                        var sDate = new DateTime(dateTime.Year, (s - 1) * 6 + s, 1);
                        return _last ?
                            new DateType(sDate.AddMonths(6).AddDays(-1)) :
                            new DateType(sDate);
                    case Duration.Quarter:
                        var q = (int)Math.Ceiling(dateTime.Month / 3.0);
                        var qDate = new DateTime(dateTime.Year, (q - 1) * 3 + q, 1);
                        return _last ?
                            new DateType(qDate.AddMonths(3).AddDays(-1)) :
                            new DateType(qDate);
                    case Duration.Month:
                        var mDate = new DateTime(dateTime.Year, dateTime.Month, 1);
                        return _last ?
                            new DateType(mDate.AddMonths(1).AddDays(-1)) :
                            new DateType(mDate);
                    case Duration.Week:
                        var day = dateTime.DayOfWeek;
                        return _last ?
                            new DateType(dateTime.AddDays(6 - (int)day)) :
                            new DateType(dateTime.AddDays(-(int)day));
                    case Duration.Day:
                        return date;
                }
            }
            if(scalar is TimeType time)
            {
                switch (_periodIndTo)
                {
                    case Duration.Annual:
                        return new TimeType(new Tuple<DateTime, DateTime>(
                            new DateTime(time.Start.Year, 1, 1),
                            new DateTime(time.End.Year, 12, 31)));
                    case Duration.Semester:
                        var sStart = (int)Math.Ceiling(time.Start.Month / 6.0);
                        var sStartDate = new DateTime(time.Start.Year, (sStart - 1) * 6 + sStart, 1);
                        var sEnd = (int)Math.Ceiling(time.End.Month / 6.0);
                        var sEndDate = new DateTime(time.End.Year, (sEnd - 1) * 6 + sEnd, 1)
                            .AddMonths(6).AddDays(-1);
                        return new TimeType(new Tuple<DateTime, DateTime>(sStartDate, sEndDate));
                    case Duration.Quarter:
                        var qStart = (int)Math.Ceiling(time.Start.Month / 3.0);
                        var qStartDate = new DateTime(time.Start.Year, (qStart - 1) * 3 + 1, 1);
                        var qEnd = (int)Math.Ceiling(time.End.Month / 3.0);
                        var qEndDate = new DateTime(time.End.Year, (qEnd - 1) * 3 + 1, 1)
                            .AddMonths(3).AddDays(-1);
                        return new TimeType(new Tuple<DateTime, DateTime>(qStartDate, qEndDate));
                    case Duration.Month:
                        var mStartDate = new DateTime(time.Start.Year, time.Start.Month, 1);
                        var mEndDate = new DateTime(time.End.Year, time.End.Month, 1).AddMonths(1).AddDays(-1);
                        return new TimeType(new Tuple<DateTime, DateTime>(mStartDate, mEndDate));
                    case Duration.Week:
                        var startDay = time.Start.AddDays(-(int)time.Start.DayOfWeek);
                        var endDay = time.End.AddDays(6 - (int)time.End.DayOfWeek);
                        return new TimeType(new Tuple<DateTime, DateTime>(startDay, endDay));
                    case Duration.Day:
                        return time;
                }
            }
            throw new Exception($"TimeAgg kan bara utföras på datatyperna time, date och time_period. Bifogad datatyp är {scalar.GetType().Name}.");
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
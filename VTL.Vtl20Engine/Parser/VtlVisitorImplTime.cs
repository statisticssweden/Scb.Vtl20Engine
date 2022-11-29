using Antlr4.Runtime.Misc;
using System;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.TimeOperator;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.Parser
{
    public partial class VtlVisitorImpl
    {
        public override Operand VisitTimeShiftAtom([NotNull] VtlParser.TimeShiftAtomContext context)
        {
            var operand = Visit(context.expr());
            if (!int.TryParse(context.signedInteger().GetText(), out var integer))
            {
                throw new Exception("Det andra argumentet till timeshift måste vara ett heltal!");
            }
            var shiftNumber = new IntegerType(integer);

            return new Operand() { Data = new Timeshift(operand, shiftNumber) };
        }

        public override Operand VisitTimeAggAtom([NotNull] VtlParser.TimeAggAtomContext context)
        {
            var string_constants = context.STRING_CONSTANT().Select(sc => sc.GetText()).ToArray();
            Duration? periodIndTo = string_constants.Length >= 1 ? GetPeriodIndicator(string_constants[0].Trim('"')) : null;
            Duration? periodIndFrom = string_constants.Length >= 2 ? GetPeriodIndicator(string_constants[1].Trim('"')) : null;
            var op = Visit(context.op);
            var last = context.LAST() != null;
            if (_stack.Any())
            {
                return new Operand() { Data = new GroupAllTimeAgg(periodIndTo, periodIndFrom, _stack.Peek(), op, last) };
            }
            else
            {
                return new Operand() { Data = new TimeAgg(periodIndTo, periodIndFrom, op, last) };
            }
        }

        public override Operand VisitTimeAggAtomComponent([NotNull] VtlParser.TimeAggAtomComponentContext context)
        {
            var string_constants = context.STRING_CONSTANT().Select(sc => sc.GetText()).ToArray();
            Duration? periodIndTo = string_constants.Length >= 1 ? GetPeriodIndicator(string_constants[0].Trim('"')) : null;
            Duration? periodIndFrom = string_constants.Length >= 2 ? GetPeriodIndicator(string_constants[1].Trim('"')) : null;
            var op = Visit(context.op);
            var last = context.LAST() != null;
            if (_stack.Any())
            {
                return new Operand() { Data = new GroupAllTimeAgg(periodIndTo, periodIndFrom, _stack.Peek(), op, last) };
            }
            else
            {
                return new Operand() { Data = new TimeAgg(periodIndTo, periodIndFrom, op, last) };
            }
        }

        public override Operand VisitFillTimeAtom([NotNull] VtlParser.FillTimeAtomContext context)
        {
            var op = Visit(context.expr());
            bool single = context.SINGLE() != null;
            return new Operand() { Data = new FillTimeSeries(op, single) };
        }

        private Duration? GetPeriodIndicator(string s)
        {
            switch (s.ToUpper())
            {
                case ("D"):
                    return Duration.Day;
                case ("W"):
                    return Duration.Week;
                case ("M"):
                    return Duration.Month;
                case ("Q"):
                    return Duration.Quarter;
                case ("S"):
                    return Duration.Semester;
                case ("A"):
                    return Duration.Annual;
                case ("_"):
                    return null;
                default:
                    throw new VtlException($"{s} är inte en korrekt tidsperiod.", null, _alias);
            }
        }
    }
}
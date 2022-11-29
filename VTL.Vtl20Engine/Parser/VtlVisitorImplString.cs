using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.StringOperator;

namespace VTL.Vtl20Engine.Parser
{
    public partial class VtlVisitorImpl
    {
        public override Operand VisitSubstrAtom(VtlParser.SubstrAtomContext context)
        {
            var operand = Visit(context.expr());
            var arguments = new List<Operand>();
            if(context.COMMA().Length > 2)
            {
                throw new Exception("Du får inte ha mer än tre parametrar till funktionen substring.");
            }
            foreach (var optionalExpr in context.optionalExpr())
            {
                arguments.Add(Visit(optionalExpr));
            }

            return new Operand() { Data = new Substr(operand, arguments) };
        }

        public override Operand VisitSubstrAtomComponent([NotNull] VtlParser.SubstrAtomComponentContext context)
        {
            var operand = Visit(context.exprComponent());
            var arguments = new List<Operand>();
            foreach (var optionalExpr in context.optionalExprComponent())
            {
                arguments.Add(Visit(optionalExpr));
            }

            return new Operand() { Data = new Substr(operand, arguments) };
        }

        public override Operand VisitUnaryStringFunction([NotNull] VtlParser.UnaryStringFunctionContext context)
        {
            if (context.LEN() != null)
            {
                var operand = Visit(context.expr());
                return new Operand() { Data = new Length(operand) };
            }
            return base.VisitUnaryStringFunction(context);
        }

        public override Operand VisitUnaryStringFunctionComponent([NotNull] VtlParser.UnaryStringFunctionComponentContext context)
        {
            if (context.LEN() != null)
            {
                var operand = Visit(context.exprComponent());
                return new Operand() { Data = new Length(operand) };
            }
            return base.VisitUnaryStringFunctionComponent(context);
        }

        public override Operand VisitInstrAtom(VtlParser.InstrAtomContext context)
        {
            var operand = Visit(context.expr(0));
            var arguments = new List<Operand>();
            arguments.Add(Visit(context.expr(1)));
            foreach (var optionalExpr in context.optionalExpr())
            {
                arguments.Add(Visit(optionalExpr));
            }

            return new Operand() { Data = new Instr(operand, arguments) };
        }

        public override Operand VisitInstrAtomComponent([NotNull] VtlParser.InstrAtomComponentContext context)
        {
            var operand = Visit(context.exprComponent(0));
            var arguments = new List<Operand>();
            arguments.Add(Visit(context.exprComponent(1)));
            foreach (var optionalExpr in context.optionalExprComponent())
            {
                arguments.Add(Visit(optionalExpr));
            }

            return new Operand() { Data = new Instr(operand, arguments) };
        }
    }
}


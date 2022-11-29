using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.GeneralPurposeOperator;

namespace VTL.Vtl20Engine.Parser
{
    public partial class VtlVisitorImpl
    {
        public override Operand VisitEvalAtom([NotNull] VtlParser.EvalAtomContext context)
        {
            var operands = new List<Operand>();

            using (var contextEnumerator = context.children.GetEnumerator())
            {
                var lp = 0;
                while (lp < 2)
                {
                    contextEnumerator.MoveNext();
                    if (contextEnumerator.Current != null &&
                        contextEnumerator.Current.GetText().Equals("("))
                    {
                        lp++;
                    }
                }

                while (contextEnumerator.MoveNext() &&
                       contextEnumerator.Current != null &&
                       !contextEnumerator.Current.GetText().Equals(")"))
                {
                    if (contextEnumerator.Current.GetText() != ",")
                    {
                        var op = _heap.FirstOrDefault(h => h.Alias.Equals(contextEnumerator.Current.GetText())) ??
                                 Visit(contextEnumerator.Current);
                        if (op != null)
                        {
                            operands.Add(op);
                        }
                    }
                }
            }

            var routineName = context.routineName().GetText();

            return new Operand { Data = new EvalOperator(operands, routineName, ExternalFunctionExecutor, _validation) };
        }

        public override Operand VisitEvalAtomComponent([NotNull] VtlParser.EvalAtomComponentContext context)
        {
            var operands = new List<Operand>();

            using (var contextEnumerator = context.children.GetEnumerator())
            {
                var lp = 0;
                while (lp < 2)
                {
                    contextEnumerator.MoveNext();
                    if (contextEnumerator.Current != null &&
                        contextEnumerator.Current.GetText().Equals("("))
                    {
                        lp++;
                    }
                }

                while (contextEnumerator.MoveNext() &&
                       contextEnumerator.Current != null &&
                       !contextEnumerator.Current.GetText().Equals(")"))
                {
                    if (contextEnumerator.Current.GetText() != ",")
                    {
                        var op = _heap.FirstOrDefault(h => h.Alias.Equals(contextEnumerator.Current.GetText())) ??
                                 Visit(contextEnumerator.Current);
                        if (op != null)
                        {
                            operands.Add(op);
                        }
                    }
                }
            }

            var routineName = context.routineName().GetText();

            return new Operand { Data = new EvalOperator(operands, routineName, ExternalFunctionExecutor, _validation) };
        }

    }
}
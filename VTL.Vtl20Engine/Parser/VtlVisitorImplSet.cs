using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.SetOperator;

namespace VTL.Vtl20Engine.Parser
{
    public partial class VtlVisitorImpl
    {
        public override Operand VisitUnionAtom([NotNull] VtlParser.UnionAtomContext context)
        {
            var operand = new List<Operand>();
            foreach (var child in context.children)
            {
                var visitedChild = Visit(child);
                if (visitedChild != null) operand.Add(visitedChild);
            }
            return new Operand() { Data = new Union(operand) };
        }

        public override Operand VisitIntersectAtom([NotNull] VtlParser.IntersectAtomContext context)
        {
            var operand = new List<Operand>();
            foreach (var child in context.children)
            {
                var visitedChild = Visit(child);
                if (visitedChild != null) operand.Add(visitedChild);
            }
            return new Operand() { Data = new Intersect(operand) };
        }

        public override Operand VisitSetOrSYmDiffAtom([NotNull] VtlParser.SetOrSYmDiffAtomContext context)
        {
            var operand = new List<Operand>();
            foreach (var child in context.children)
            {
                var visitedChild = Visit(child);
                if (visitedChild != null) operand.Add(visitedChild);
            }

            if (context.SETDIFF() != null)
            {
                return new Operand() { Data = new SetDiff(operand) };
            }

            if (context.SYMDIFF() != null)
            {
                if (operand.Count() != 2)
                    throw new Exception("Setdiff måste ha två dataset som argument.");
                return new Operand() { Data = new SymDiff(operand) };
            }

            return base.VisitSetOrSYmDiffAtom(context);
        }

    }
}
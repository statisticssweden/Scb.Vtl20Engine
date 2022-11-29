using Antlr4.Runtime.Misc;
using System;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.AggregateOperator;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.GroupingOperator;

namespace VTL.Vtl20Engine.Parser
{
    public partial class VtlVisitorImpl
    {
        public override Operand VisitAggrDataset([NotNull] VtlParser.AggrDatasetContext context)
        {
            var onHeap = Visit(context.expr());

            _stack.Push(onHeap);

            var goupingOperand = context.groupingClause() != null
                ? Visit(context.groupingClause())
                : new Operand() { Data = new GroupByNoneOperator(_stack.FirstOrDefault()) };

            var havingClause = context.havingClause() != null
                ? Visit(context.havingClause())
                : null;

            try
            {
                if (context.COUNT() != null)
                {
                    return new Operand() { Data = new CountOperator(goupingOperand) };
                }

                if (context.MIN() != null)
                {
                    return new Operand() { Data = new MinOperator(goupingOperand) };
                }

                if (context.MAX() != null)
                {
                    return new Operand() { Data = new MaxOperator(goupingOperand) };
                }

                if (context.MEDIAN() != null)
                {
                    return new Operand() { Data = new MedianOperator(goupingOperand) };
                }

                if (context.SUM() != null)
                {
                    return new Operand() { Data = new SumOperator(goupingOperand) };
                }

                if (context.AVG() != null)
                {
                    return new Operand() { Data = new AverageOperator(goupingOperand) };
                }
            }
            finally
            {
                _stack.Pop();
            }

            return new Operand();
        }

        public override Operand VisitGroupByOrExcept([NotNull] VtlParser.GroupByOrExceptContext context)
        {
            var identifiers = context.componentID().Select(i => i.GetText()).ToArray();
            if (context.BY() != null)
            {
                return new Operand() { Data = new GroupByOperator(_stack.FirstOrDefault(), identifiers) };
            }

            if (context.EXCEPT() != null)
            {
                return new Operand() { Data = new GroupExceptOperator(_stack.FirstOrDefault(), identifiers) };
            }

            return base.VisitGroupByOrExcept(context);
        }


        public override Operand VisitGroupAll([NotNull] VtlParser.GroupAllContext context)
        {
            if (context.ALL() != null)
            {
                return new Operand() { Data = new GroupAllOperator(Visit(context.exprComponent())) };
            }

            return base.VisitGroupAll(context);
        }

        public override Operand VisitHavingClause(VtlParser.HavingClauseContext context)
        {
            throw new NotImplementedException("Having är inte implementerad än.");
        }
    }
}
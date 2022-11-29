using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.AnalyticOperator;

namespace VTL.Vtl20Engine.Parser
{
    public partial class VtlVisitorImpl
    {
        public override Operand VisitRankAnComponent([NotNull] VtlParser.RankAnComponentContext context)
        {
            var operand = _stack.Peek(); // or calc
            IEnumerable<string> partitionBy = null;
            List<OrderByName> orderByClause = null;
            if (context.OVER() != null)
            {
                if (context.partitionByClause() != null && context.partitionByClause().PARTITION() != null)
                {
                    partitionBy = context.partitionByClause().componentID().Select(i => i.GetText());
                }

                if (context.orderByClause() != null && context.orderByClause().ORDER() != null && context.orderByClause().BY() != null)
                {
                    orderByClause = new List<OrderByName>();

                    using (var orderByEnumerator =
                        context.orderByClause().children.GetEnumerator())
                    {
                        var orderByParam = new OrderByName { NullValuesLast = true };
                        while (orderByEnumerator.MoveNext())
                        {
                            switch (orderByEnumerator.Current.GetText())
                            {
                                case "order":
                                case "by":
                                    break;
                                case ",":
                                    orderByClause.Add(orderByParam);
                                    orderByParam = new OrderByName { NullValuesLast = true };
                                    break;
                                case "asc":
                                    orderByParam.Descending = false;
                                    break;
                                case "desc":
                                    orderByParam.Descending = true;
                                    break;
                                default:
                                    orderByParam.ComponentName = orderByEnumerator.Current.GetChild(0).GetText();
                                    if(orderByEnumerator.Current.ChildCount > 1)
                                    {
                                        var desc = orderByEnumerator.Current.GetChild(1).GetText();
                                        if (!string.IsNullOrEmpty(desc) && desc.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            orderByParam.Descending = true;
                                        }
                                    }
                                    break;
                            }
                        }

                        if (!string.IsNullOrEmpty(orderByParam.ComponentName))
                        {
                            orderByClause.Add(orderByParam);
                        }
                    }
                }
            }

            return new Operand() { Data = new Rank(operand, partitionBy, orderByClause) };
        }

        public override Operand VisitRatioToReportAn([NotNull] VtlParser.RatioToReportAnContext context)
        {
            var operand = context.expr() != null ?
                Visit(context.expr()) : // ordinary expression
                _stack.Peek(); // or calc
            IEnumerable<string> partitionBy = null;
            if (context.OVER() != null)
            {
                if (context.partitionByClause() != null && context.partitionByClause().PARTITION() != null)
                {
                    partitionBy = context.partitionByClause().componentID().Select(i => i.GetText());
                }
            }

            return new Operand() { Data = new RatioToReport(operand, partitionBy) };
        }

        public override Operand VisitRatioToReportAnComponent([NotNull] VtlParser.RatioToReportAnComponentContext context)
        {
            var operand = context.exprComponent() != null ?
                Visit(context.exprComponent()) : // ordinary expression
                _stack.Peek(); // or calc
            IEnumerable<string> partitionBy = null;
            if (context.OVER() != null)
            {
                if (context.partitionByClause() != null && context.partitionByClause().PARTITION() != null)
                {
                    partitionBy = context.partitionByClause().componentID().Select(i => i.GetText());
                }
            }

            return new Operand() { Data = new RatioToReport(operand, partitionBy) };
        }

    }
}

using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ClauseOperator;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.JoinOperator;

namespace VTL.Vtl20Engine.Parser
{
    public partial class VtlVisitorImpl
    {
        List<Operand> _operands;

        public override Operand VisitJoinExpr([NotNull] VtlParser.JoinExprContext context)
        {
            _operands = new List<Operand>();
            IEnumerable<string> usings = null;

            var joinClauseItems = context.joinClause()?.joinClauseItem() ?? context.joinClauseWithoutUsing().joinClauseItem();

            foreach (var joinClauseItemContext in joinClauseItems)
            {

                var joinClauseItem = Visit(joinClauseItemContext);

                //apply dataset renaming done with as keyword
                if (joinClauseItemContext.AS() != null)
                {
                    var temporaryAlias = joinClauseItemContext.alias().GetText();

                    if (_heap.Any(h => h.Alias == temporaryAlias))
                    {
                        throw new Exception($"Variabelnamnet {temporaryAlias} är upptaget.");
                    }
                    var newOperand = new Operand
                    {
                        Alias = temporaryAlias,
                        Data = joinClauseItem.Data
                    };
                    _heap.Add(newOperand);
                    _operands.Add(newOperand);
                }
                else
                {
                    _operands.Add(new Operand
                    {
                        Alias = joinClauseItem.Alias,
                        Data = joinClauseItem.Data
                    });
                }

            }

            if (context.joinClause()?.USING() != null)
            {
                usings = context.joinClause()?.componentID()?.Select(u => u.GetText());
            }

            Operand vds1 = null;

            if (context.joinKeyword.Text.Equals("CROSS_JOIN", StringComparison.InvariantCultureIgnoreCase))
            {
                if (usings != null) throw new Exception("CROSS_JOIN: Cross_join tillåter inte tillvalet using");
                vds1 = new Operand { Data = new CrossJoin(_operands) };
            }
            if (context.joinKeyword.Text.Equals("INNER_JOIN", StringComparison.InvariantCultureIgnoreCase))
            {
                vds1 = new Operand { Data = new InnerJoin(_operands, usings?.ToArray()) };
            }
            if (context.joinKeyword.Text.Equals("LEFT_JOIN", StringComparison.InvariantCultureIgnoreCase))
            {
                vds1 = new Operand { Data = new LeftJoin(_operands, usings?.ToArray()) };
            }
            if (context.joinKeyword.Text.Equals("FULL_JOIN", StringComparison.InvariantCultureIgnoreCase))
            {
                if (usings != null) throw new Exception("FULL_JOIN: Full_join tillåter inte tillvalet using");
                vds1 = new Operand { Data = new FullJoin(_operands) };
            }

            if (vds1 != null)
            {
                //filter
                Operand vds2;
                if (context.joinBody().filterClause() != null)
                {
                    _stack.Push(vds1);
                    vds2 = Visit(context.joinBody().filterClause());
                    _stack.Pop();
                }
                else
                {
                    vds2 = vds1;
                }

                //apply, calc, aggr
                Operand vds3;
                if (context.joinBody().joinApplyClause() != null)
                {
                    IEnumerable<string> commonMeasures = _operands.First().GetMeasureNames();
                    foreach (var operand in _operands)
                    {
                        commonMeasures = commonMeasures.Intersect(operand.GetMeasureNames());
                    }

                    var lastApply = vds2;
                    foreach (var measure in commonMeasures)
                    {
                        foreach (var operand in _operands)
                        {
                            _stack.Push(new Operand()
                            {
                                Alias = operand.Alias,
                                Data = new GetComponentOperator(vds2, operand.Alias + "#" + measure)
                            });
                        }

                        var apply = Visit(context.joinBody().joinApplyClause());
                        apply.Alias = measure;

                        lastApply = new Operand() { Data = new ApplyOperator(lastApply, apply) };

                        for (int i = 0; i < _operands.Count; i++)
                        {
                            _stack.Pop();
                        }
                    }
                    vds3 = lastApply;
                }
                else if (context.joinBody().calcClause() != null)
                {
                    _stack.Push(vds2);
                    vds3 = Visit(context.joinBody().calcClause());
                    _stack.Pop();
                }
                else if (context.joinBody().aggrClause() != null)
                {
                    _stack.Push(vds2);
                    vds3 = Visit(context.joinBody().aggrClause());
                    _stack.Pop();
                }
                else
                {
                    vds3 = vds2;
                }

                //keep, drop
                Operand vds4;
                if (context.joinBody().keepOrDropClause() != null)
                {
                    _stack.Push(vds3);
                    vds4 = Visit(context.joinBody().keepOrDropClause());
                    _stack.Pop();
                }
                else
                {
                    vds4 = vds3;
                }

                //rename
                Operand vds5;
                if (context.joinBody().renameClause() != null)
                {
                    _stack.Push(vds4);
                    vds5 = Visit(context.joinBody().renameClause());
                    _stack.Pop();
                }
                else
                {
                    vds5 = vds4;
                }

                //var vds5 = vds1;
                //reverse dataset renaming done with as keyword
                foreach(var operand in _operands)
                {
                    if(_heap.Contains(operand))
                    {
                        _heap.Remove(operand);
                    }
                }
                return new Operand()
                {
                    Alias = vds5.Alias,
                    Data = new JoinCleanupOperator(vds5)
                };

            }
            return base.VisitJoinExpr(context);
        }

        public override Operand VisitJoinClauseItem([NotNull] VtlParser.JoinClauseItemContext context)
        {
            var result = Visit(context.expr());
            return result;
        }
    }
}
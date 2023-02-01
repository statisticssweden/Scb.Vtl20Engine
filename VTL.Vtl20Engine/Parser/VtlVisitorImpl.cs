using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VTL.Vtl20Engine.Contracts;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.BooleanOperator.BinaryBooleanOperator;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.BooleanOperator.UnaryBooleanOperator;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ComparisonOperator;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ConditionalOperator;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.GeneralPurposeOperator;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.NumericOperator.BinaryNumericOperator;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.NumericOperator.UnaryNumericOperator;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.StringOperator;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Exceptions;

namespace VTL.Vtl20Engine.Parser
{
    public partial class VtlVisitorImpl : VtlBaseVisitor<Operand>
    {
        private readonly List<Operand> _heap;
        private readonly Stack<Operand> _stack;
        private string _alias;
        private readonly bool _validation;
        private int _overflowIndicator;

        public IExternalFunctionExecutor ExternalFunctionExecutor { get; set; }

        public VtlVisitorImpl(List<Operand> heap, bool validation)
        {
            _heap = heap;
            _stack = new Stack<Operand>();
            _validation = validation;
            _overflowIndicator = 0;
        }

        public override Operand VisitStart(VtlParser.StartContext context)
        {
            Operand output = null;
            
            foreach (var statementContext in context.statement())
            {
                _alias = GetAlias(statementContext);

                if (!string.IsNullOrEmpty(_alias))
                {
                    if (_heap.Any(h => h.Alias.Equals(_alias)))
                    {
                        throw new VtlException(
                            $"{_alias} tilldelades flera gånger i VTL-koden. Varje variabelnamn får bara tilldelas en gång.", null, _alias);
                    }

                    _heap.Add(new Operand()
                    {
                        Alias = _alias,
                        Source = statementContext,
                        Persistant = statementContext.children[1].GetText() == "<-"
                    });
                }
            }

            foreach (var statementContext in context.statement())
            {
                var alias = GetAlias(statementContext);
                _alias = alias;

                try
                {
                    output = Visit(statementContext);
                }
                catch (OverrideValidationException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new VtlException(e.Message, e, _alias);
                }
                if (!string.IsNullOrEmpty(alias))
                {
                    var op = _heap.First(o => o.Alias.Equals(alias));
                    if (op.Data == null)
                    {
                        op.Data = output.Data;
                    }
                }
            }

            foreach (var operand in _heap)
            {
                if (operand.Data == null)
                {
                    throw new VtlException($"{operand.Alias} tilldelades aldrig något värde.", null, operand.Alias);
                }
            }

            return output;
        }

        private string GetAlias(VtlParser.StatementContext statementContext)
        {
            if (statementContext.ChildCount > 1 && 
                (statementContext.children[1].GetText() == ":=" || statementContext.children[1].GetText() == "<-"))
            {
                return statementContext.children[0].GetText();
            }
            return null;
        }

        public override Operand VisitTemporaryAssignment([NotNull] VtlParser.TemporaryAssignmentContext context)
        {
            if (_overflowIndicator++ > 1000) throw new Exception($"Koden innehåller cirkelreferenser gällande operand {_alias}, eller innehåller för många tilldelningar.");
            _alias = context.varID().GetText();
            var expr = Visit(context.expr());
            return new Operand
            {
                Persistant = false,
                Alias = _alias,
                Source = context,
                Data = expr.Data
            };
        }

        public override Operand VisitPersistAssignment([NotNull] VtlParser.PersistAssignmentContext context)
        {
            if (_overflowIndicator++ > 1000) throw new Exception($"Koden innehåller cirkelreferenser gällande operand {_alias}, eller innehåller för många tilldelningar.");
            var alias = context.varID().GetText();
            var expr = Visit(context.expr());
            return new Operand
            {
                Persistant = true,
                Alias = alias,
                Source = context,
                Data = expr?.Data
            };
        }

        public override Operand VisitParenthesisExpr([NotNull] VtlParser.ParenthesisExprContext context)
        {
            return Visit(context.expr());
        }

        public override Operand VisitParenthesisExprComp([NotNull] VtlParser.ParenthesisExprCompContext context)
        {
            return Visit(context.exprComponent());
        }

        public override Operand VisitVarIdExpr([NotNull] VtlParser.VarIdExprContext context)
        {
            var dataSetOperand = Visit(context.children[0]);
            if (dataSetOperand.Data == null)
                throw new Exception($"Beräkningen kan inte exekveras eftersom dataset eller komponent med namn { context.children[0].GetText()} inte kan hittas.");
            _stack.Push(dataSetOperand);
            var datasetOperand = Visit(context.varID());
            _stack.Pop();
            return datasetOperand;
        }

        public override Operand VisitIfExpr([NotNull] VtlParser.IfExprContext context)
        {
            var cond = Visit(context.conditionalExpr);
            var ifOp = Visit(context.thenExpr);
            var elseOp = Visit(context.elseExpr);
            return new Operand() { Data = new IfOperator(cond, ifOp, elseOp) };
        }

        public override Operand VisitIfExprComp([NotNull] VtlParser.IfExprCompContext context)
        {
            var cond = Visit(context.conditionalExpr);
            var ifOp = Visit(context.thenExpr);
            var elseOp = Visit(context.elseExpr);
            return new Operand() { Data = new IfOperator(cond, ifOp, elseOp) };
        }

        public override Operand VisitInNotInExpr([NotNull] VtlParser.InNotInExprContext context)
        {
            var operand = Visit(context.expr());
            var lists = Visit(context.lists());
            return context.IN() != null ?
                new Operand() { Data = new InOperator(operand, lists) } :
                new Operand() { Data = new NotInOperator(operand, lists) };
        }

        public override Operand VisitInNotInExprComp([NotNull] VtlParser.InNotInExprCompContext context)
        {
            var operand = Visit(context.exprComponent());
            var lists = Visit(context.lists());
            return context.IN() != null ?
                new Operand() { Data = new InOperator(operand, lists) } :
                new Operand() { Data = new NotInOperator(operand, lists) };
        }

        public override Operand VisitExistInAtom([NotNull] VtlParser.ExistInAtomContext context)
        {
            var operand1 = Visit(context.left);
            var operand2 = Visit(context.right);
            var retainType = context.retainType()?.BOOLEAN_CONSTANT()?.GetText();
            if (retainType == "true")
            {
                return new Operand() { Data = new ExistsInOperator(operand1, operand2, ExistsInRetainType.True) };
            }
            if (retainType == "false")
            {
                return new Operand() { Data = new ExistsInOperator(operand1, operand2, ExistsInRetainType.False) };
            }
            return new Operand() { Data = new ExistsInOperator(operand1, operand2, ExistsInRetainType.All) };
        }

        public override Operand VisitBooleanExpr([NotNull] VtlParser.BooleanExprContext context)
        {
            if (context.AND() != null)
            {
                var operand1 = Visit(context.children[0]);
                var operand2 = Visit(context.children[2]);
                return new Operand() { Data = new And(operand1, operand2) };
            }
            else if (context.OR() != null)
            {
                var operand1 = Visit(context.children[0]);
                var operand2 = Visit(context.children[2]);
                return new Operand() { Data = new Or(operand1, operand2) };
            }
            else if (context.XOR() != null)
            {
                var operand1 = Visit(context.children[0]);
                var operand2 = Visit(context.children[2]);
                return new Operand() { Data = new Xor(operand1, operand2) };
            }
            return base.VisitBooleanExpr(context);
        }

        public override Operand VisitBooleanExprComp([NotNull] VtlParser.BooleanExprCompContext context)
        {
            if (context.AND() != null)
            {
                var operand1 = Visit(context.children[0]);
                var operand2 = Visit(context.children[2]);
                return new Operand() { Data = new And(operand1, operand2) };
            }
            else if (context.OR() != null)
            {
                var operand1 = Visit(context.children[0]);
                var operand2 = Visit(context.children[2]);
                return new Operand() { Data = new Or(operand1, operand2) };
            }
            else if (context.XOR() != null)
            {
                var operand1 = Visit(context.children[0]);
                var operand2 = Visit(context.children[2]);
                return new Operand() { Data = new Xor(operand1, operand2) };
            }
            return base.VisitBooleanExprComp(context);
        }

        public override Operand VisitUnaryExprComp([NotNull] VtlParser.UnaryExprCompContext context)
        {
            var operand = Visit(context.right);
            if (context.PLUS() != null)
            {
                return operand;
            }
            if (context.MINUS() != null)
            {
                return new Operand() { Data = new Negation(operand) };
            }
            if (context.NOT() != null)
            {
                return new Operand() { Data = new Not(operand) };
            }
            return base.VisitUnaryExprComp(context);
        }

        public override Operand VisitUnaryExpr([NotNull] VtlParser.UnaryExprContext context)
        {
            var operand = Visit(context.right);
            if (context.PLUS() != null)
            {
                return operand;
            }
            if(context.MINUS() != null)
            {
                return new Operand() { Data = new Negation(operand) };
            }
            if (context.NOT() != null)
            {
                return new Operand() { Data = new Not(operand) };
            }
            return base.VisitUnaryExpr(context);
        }

        public override Operand VisitMembershipExpr([NotNull] VtlParser.MembershipExprContext context)
        {
            var operand1 = Visit(context.expr());
            var operand2 = Visit(context.simpleComponentId());

            var operand = _stack.FirstOrDefault(o => o.GetComponentNames().Contains(context.GetText()));
            if (operand != null)
            {
                operand1 = operand;
                operand2.Alias = context.GetText();
            }

            if (operand1 == null)
            {
                throw new Exception($"{context.children[0].GetText()}#{context.children[2].GetText()} kändes inte igen.");
            }
            if (operand1.Data == null)
            {
                throw new Exception($"{operand1.Alias} kändes inte igen.");
            }
            if (operand1.GetComponentNames().Contains(operand2.Alias))
            {
                return new Operand()
                {
                    Alias = $"{operand1.Alias}#{operand2.Alias}",
                    Data = new MembershipOperator(operand1, operand2.Alias)
                };
            }
            else
            {
                throw new Exception($"{operand1.Alias} innehåller ingen komponent som heter {operand2.Alias}");
            }
        }

        public override Operand VisitSimpleComponentId([NotNull] VtlParser.SimpleComponentIdContext context)
        {
            return new Operand() { Alias = context.IDENTIFIER().GetText() };
        }

        public override Operand VisitArithmeticExpr([NotNull] VtlParser.ArithmeticExprContext context)
        {
            var operand1 = Visit(context.left);
            var operand2 = Visit(context.right);

            if (context.MUL() != null)
            {
                return new Operand() { Data = new Multiplication(operand1, operand2) };
            }
            if(context.DIV() != null)
            {
                return new Operand() { Data = new Division(operand1, operand2) };
            }
            return base.VisitArithmeticExpr(context);
        }

        public override Operand VisitFunctionsExpression([NotNull] VtlParser.FunctionsExpressionContext context)
        {
            var result = Visit(context.functions());
            if(result == null)
            {
                throw new Exception($"Kunde inte tolka vtl-koden");
            }
            return result;
        }

        public override Operand VisitArithmeticExprComp([NotNull] VtlParser.ArithmeticExprCompContext context)
        {
            var operand1 = Visit(context.left);
            var operand2 = Visit(context.right);

            if (context.MUL() != null)
            {
                return new Operand() { Data = new Multiplication(operand1, operand2) };
            }
            if (context.DIV() != null)
            {
                return new Operand() { Data = new Division(operand1, operand2) };
            }
            return base.VisitArithmeticExprComp(context);
        }

        public override Operand VisitArithmeticExprOrConcat([NotNull] VtlParser.ArithmeticExprOrConcatContext context)
        {
            var operand1 = Visit(context.left);
            var operand2 = Visit(context.right);

            if (context.PLUS() != null)
            {
                return new Operand() { Data = new Addition(operand1, operand2) };
            }
            if(context.MINUS() != null)
            {
                return new Operand() { Data = new Minus(operand1, operand2) };
            }
            if(context.CONCAT() != null)
            {
                return new Operand() { Data = new Concat(operand1, operand2) };
            }
            return base.VisitArithmeticExprOrConcat(context);
        }

        public override Operand VisitArithmeticExprOrConcatComp([NotNull] VtlParser.ArithmeticExprOrConcatCompContext context)
        {
            var operand1 = Visit(context.left);
            var operand2 = Visit(context.right);

            if (context.PLUS() != null)
            {
                return new Operand() { Data = new Addition(operand1, operand2) };
            }
            if (context.MINUS() != null)
            {
                return new Operand() { Data = new Minus(operand1, operand2) };
            }
            if (context.CONCAT() != null)
            {
                return new Operand() { Data = new Concat(operand1, operand2) };
            }
            return base.VisitArithmeticExprOrConcatComp(context);
        }

        public override Operand VisitComparisonExpr([NotNull] VtlParser.ComparisonExprContext context)
        {
            var operand1 = Visit(context.left);
            var operand2 = Visit(context.right);

            if (context.comparisonOperand().EQ() != null)
            {
                return new Operand() { Data = new EqualToOperator(operand1, operand2) };
            }
            if(context.comparisonOperand().NEQ() != null)
            {
                return new Operand() { Data = new NotEqualToOperator(operand1, operand2) };
            }
            if (context.comparisonOperand().MT() != null)
            {
                return new Operand() { Data = new GreaterThanOperator(operand1, operand2) };
            }
            if (context.comparisonOperand().ME() != null)
            {
                return new Operand() { Data = new GreaterOrEqualToOperator(operand1, operand2) };
            }
            if (context.comparisonOperand().LT() != null)
            {
                return new Operand() { Data = new LessThanOperator(operand1, operand2) };
            }
            if (context.comparisonOperand().LE() != null)
            {
                return new Operand() { Data = new LessThanOrEqualToOperator(operand1, operand2) };
            }
            return base.VisitComparisonExpr(context);
        }

        public override Operand VisitComparisonExprComp([NotNull] VtlParser.ComparisonExprCompContext context)
        {
            var operand1 = Visit(context.left);
            var operand2 = Visit(context.right);

            if (context.comparisonOperand().EQ() != null)
            {
                return new Operand() { Data = new EqualToOperator(operand1, operand2) };
            }
            if (context.comparisonOperand().NEQ() != null)
            {
                return new Operand() { Data = new NotEqualToOperator(operand1, operand2) };
            }
            if (context.comparisonOperand().MT() != null)
            {
                return new Operand() { Data = new GreaterThanOperator(operand1, operand2) };
            }
            if (context.comparisonOperand().ME() != null)
            {
                return new Operand() { Data = new GreaterOrEqualToOperator(operand1, operand2) };
            }
            if (context.comparisonOperand().LT() != null)
            {
                return new Operand() { Data = new LessThanOperator(operand1, operand2) };
            }
            if (context.comparisonOperand().LE() != null)
            {
                return new Operand() { Data = new LessThanOrEqualToOperator(operand1, operand2) };
            }
            return base.VisitComparisonExprComp(context);
        }

        public override Operand VisitIsNullAtom(VtlParser.IsNullAtomContext context)
        {
            var operand = Visit(context.expr());
            return new Operand() { Data = new IsNullOperator(operand) };
        }

        public override Operand VisitIsNullAtomComponent([NotNull] VtlParser.IsNullAtomComponentContext context)
        {
            var operand = Visit(context.exprComponent());
            return new Operand() { Data = new IsNullOperator(operand) };
        }

        public override Operand VisitCastExprDataset([NotNull] VtlParser.CastExprDatasetContext context)
        {
            Type type = null;

            if (context.basicScalarType() == null) throw new Exception("Datatyp okänd");

            if (context.basicScalarType().BOOLEAN() != null) type = typeof(BooleanType);

            if (context.basicScalarType().STRING() != null) type = typeof(StringType);

            if (context.basicScalarType().INTEGER() != null) type = typeof(IntegerType);

            if (context.basicScalarType().NUMBER() != null) type = typeof(NumberType);

            if (context.basicScalarType().DATE() != null) type = typeof(DateType);

            if (context.basicScalarType().TIME_PERIOD() != null) type = typeof(TimePeriodType);

            if (context.basicScalarType().TIME() != null) type = typeof(TimeType);

            if (context.basicScalarType().DURATION() != null) type = typeof(DurationType);

            if (context.STRING_CONSTANT() == null)
            {
                return new Operand
                {
                    Data = new CastOperator(Visit(context.expr()), type)
                };
            }

            return new Operand
            {
                Data = new CastOperator(Visit(context.expr()), type, context.STRING_CONSTANT().GetText())
            };
        }

        public override Operand VisitCastExprComponent([NotNull] VtlParser.CastExprComponentContext context)
        {
            Type type = null;

            if (context.basicScalarType() == null) throw new Exception("Datatyp okänd");

            if (context.basicScalarType().BOOLEAN() != null) type = typeof(BooleanType);

            if (context.basicScalarType().STRING() != null) type = typeof(StringType);

            if (context.basicScalarType().INTEGER() != null) type = typeof(IntegerType);

            if (context.basicScalarType().NUMBER() != null) type = typeof(NumberType);

            if (context.basicScalarType().DATE() != null) type = typeof(DateType);

            if (context.basicScalarType().TIME_PERIOD() != null) type = typeof(TimePeriodType);

            if (context.basicScalarType().TIME() != null) type = typeof(TimeType);

            if (context.basicScalarType().DURATION() != null) type = typeof(DurationType);

            if (context.STRING_CONSTANT() == null)
            {
                return new Operand
                {
                    Data = new CastOperator(Visit(context.exprComponent()), type)
                };
            }

            return new Operand
            {
                Data = new CastOperator(Visit(context.exprComponent()), type, context.STRING_CONSTANT().GetText())
            };
        }

        public override Operand VisitVarID(VtlParser.VarIDContext context)
        {
            string text = context.GetText();

            if (_stack.Count > 0)
            {
                var onStack = _stack.FirstOrDefault(o => o.GetComponentNames().Contains(text)) ??
                    _stack.FirstOrDefault(o => o.GetComponentNames().Where(c => c.Contains("#")).Select(n => n.Substring(n.IndexOf("#") + 1)).Contains(text)) ??
                    _stack.FirstOrDefault(o => o.GetComponentNames().Where(c => c.Contains("#")).Select(n => n.Substring(0, n.IndexOf("#"))).Distinct().Contains(text));
                if (onStack != null)
                {
                    return new Operand() { Alias = text, Data = new GetComponentOperator(onStack, text) };
                }

                if (_stack.Any(s => s.Alias == text))
                    return _stack.FirstOrDefault(s => s.Alias == text);


                if (_heap.Any(s => s.Alias == text && s.Data is ScalarType))
                    return _heap.FirstOrDefault(s => s.Alias == text);
                else
                    throw new Exception($"{text} kunde inte hittas.");
            }
            else
            {
                var onHeap = _heap.FirstOrDefault(h => h.Alias.Equals(text));
                if (onHeap == null)
                {
                    throw new Exception($"Beräkningen kan inte exekveras eftersom dataset eller komponent med namn {text} inte kan hittas.");
                }

                if (onHeap.Data == null)
                {
                    if (onHeap.Source == null)
                    {
                        throw new Exception($"{text} tilldelades aldrig något värde.");
                    }
                    var op = Visit(onHeap.Source);
                    if (onHeap.Data == null)
                    {
                        onHeap.Data = op.Data;
                    }
                }

                return onHeap;
            }
        }

        public override Operand VisitComponentID(VtlParser.ComponentIDContext context)
        {
            var text = context.GetText();
            string dsName = "";
            string compName = "";
            if (text.Contains("#"))
            {
                dsName = text.Substring(0, text.IndexOf("#"));
                compName = text.Substring(text.IndexOf("#") + 1);
            }
            else
            {
                compName = text;
            }


            if (_stack.Count > 0)
            {
                if (dsName != "")
                {
                    foreach (var op in _stack)
                    {
                        if (!string.IsNullOrEmpty(op.Alias) && op.Alias == dsName)
                        {
                            var componentNames = op.GetComponentNames();
                            foreach (var componentName in componentNames)
                            {
                                if (componentName.Contains("#"))
                                {
                                    if (componentName == op.Alias + "#" + compName)
                                    {
                                        return new Operand() { Alias = text, Data = new MembershipOperator(op, text) };
                                    }
                                }
                                else
                                {
                                    if(componentName == compName)
                                    {
                                        return new Operand() { Alias = text, Data = new MembershipOperator(op, compName) };
                                    }
                                }
                            }
                        }
                    }
                }


                var onStack = _stack.SingleOrDefault(o => o.GetComponentNames().Contains(text)) ??
                              _stack.SingleOrDefault(o => o.GetComponentNames().Select(s => s.Substring(s.IndexOf("#") + 1)).Count(s => s == text) == 1);
                if (onStack != null)
                {
                    return new Operand() { Alias = text, Data = new GetComponentOperator(onStack, text) };
                }
            }

            var onHeap = _heap.SingleOrDefault(h => h.Alias == text);
            if(onHeap != null)
            {
                return onHeap;
            }
            throw new Exception($"{text} kunde inte hittas.");
        }

        public override Operand VisitConstant(VtlParser.ConstantContext context)
        {
            string text = context.GetText();
            if (text.Equals("null"))
                return new Operand() { Data = new NullType() };
            if (long.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out var integer) && !text.Contains("."))
                return new Operand() { Data = new IntegerType(integer) };
            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
                return new Operand() { Data = new NumberType(number) };
            if (bool.TryParse(text, out var boolean))
                return new Operand() { Data = new BooleanType(boolean) };
            if (text.StartsWith("\"") && text.EndsWith("\""))
            {
                return new Operand() { Data = new StringType(text.Substring(1, text.Length - 2)) };
            }

            return VisitChildren(context);
        }

    }
}
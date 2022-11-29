using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ComparisonOperator;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.NumericOperator.UnaryNumericOperator;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.SetScalarTypes;

namespace VTL.Vtl20Engine.Parser
{
    public partial class VtlVisitorImpl
    {
        
        public override Operand VisitUnaryNumeric([NotNull] VtlParser.UnaryNumericContext context)
        {
            if (context.CEIL() != null)
            {
                throw new NotImplementedException();
            }
            if (context.FLOOR() != null)
            {
                throw new NotImplementedException();
            }
            if (context.ABS() != null)
            {
                var operand = Visit(context.children[2]);
                return new Operand() { Data = new Abs(operand) };
            }
            if (context.EXP() != null)
            {
                var operand = Visit(context.expr());
                return new Operand() { Data = new Exp(operand) };
            }
            if (context.LN() != null)
            {
                var operand = Visit(context.expr());
                return new Operand() { Data = new Ln(operand) };
            }
            if (context.SQRT() != null)
            {
                var operand = Visit(context.children[2]);
                return new Operand() { Data = new Sqrt(operand) };
            }
            return base.VisitUnaryNumeric(context);
        }

        public override Operand VisitUnaryNumericComponent([NotNull] VtlParser.UnaryNumericComponentContext context)
        {
            if (context.CEIL() != null)
            {
                throw new NotImplementedException();
            }
            if (context.FLOOR() != null)
            {
                throw new NotImplementedException();
            }
            if (context.ABS() != null)
            {
                var operand = Visit(context.children[2]);
                return new Operand() { Data = new Abs(operand) };
            }
            if (context.EXP() != null)
            {
                var operand = Visit(context.exprComponent());
                return new Operand() { Data = new Exp(operand) };
            }
            if (context.LN() != null)
            {
                var operand = Visit(context.exprComponent());
                return new Operand() { Data = new Ln(operand) };
            }
            if (context.SQRT() != null)
            {
                var operand = Visit(context.children[2]);
                return new Operand() { Data = new Sqrt(operand) };
            }
            return base.VisitUnaryNumericComponent(context);
        }

        public override Operand VisitUnaryWithOptionalNumeric([NotNull] VtlParser.UnaryWithOptionalNumericContext context)
        {
            if (context.ROUND() != null)
            {
                var operand = Visit(context.children[2]);
                var numDigit = context.children.Count > 4 ? Visit(context.children[4]) : null;
                return new Operand() { Data = new Round(operand, numDigit) };
            }
            if (context.TRUNC() != null)
            {
                var operand = Visit(context.children[2]);
                var numDigit = context.children.Count > 4 ? Visit(context.children[4]) : null;
                return new Operand() { Data = new Trunc(operand, numDigit) };
            }
            return base.VisitUnaryWithOptionalNumeric(context);
        }

        public override Operand VisitUnaryWithOptionalNumericComponent([NotNull] VtlParser.UnaryWithOptionalNumericComponentContext context)
        {
            if (context.ROUND() != null)
            {
                var operand = Visit(context.children[2]);
                var numDigit = context.children.Count > 4 ? Visit(context.children[4]) : null;
                return new Operand() { Data = new Round(operand, numDigit) };
            }
            if (context.TRUNC() != null)
            {
                var operand = Visit(context.children[2]);
                var numDigit = context.children.Count > 4 ? Visit(context.children[4]) : null;
                return new Operand() { Data = new Trunc(operand, numDigit) };
            }
            return base.VisitUnaryWithOptionalNumericComponent(context);
        }

        public override Operand VisitBinaryNumeric([NotNull] VtlParser.BinaryNumericContext context)
        {
            if (context.MOD() != null)
            {
                throw new NotImplementedException();
            }
            if (context.POWER() != null)
            {
                if (context.expr().Length < 1) throw new Exception("Power err");
                var baseOperand = Visit(context.expr()[0]);
                var exponent = Visit(context.expr()[1]);

                return new Operand() { Data = new Power(baseOperand, exponent) };
            }
            if (context.LOG() != null)
            {
                throw new NotImplementedException();
            }
            return base.VisitBinaryNumeric(context);
        }

        public override Operand VisitBinaryNumericComponent([NotNull] VtlParser.BinaryNumericComponentContext context)
        {
            if (context.MOD() != null)
            {
                throw new NotImplementedException();
            }
            if (context.POWER() != null)
            {
                if (context.exprComponent().Length < 1) throw new Exception("Power err");
                var baseOperand = Visit(context.exprComponent()[0]);
                var exponent = Visit(context.exprComponent()[1]);

                return new Operand() { Data = new Power(baseOperand, exponent) };
            }
            if (context.LOG() != null)
            {
                throw new NotImplementedException();
            }
            return base.VisitBinaryNumericComponent(context);
        }

        public override Operand VisitBetweenAtom(VtlParser.BetweenAtomContext context)
        {
            var operand = Visit(context.op);
            var from = Visit(context.from_);
            var to = Visit(context.to_);
            return new Operand() { Data = new BetweenOperator(operand, from, to) };
        }

        public override Operand VisitBetweenAtomComponent([NotNull] VtlParser.BetweenAtomComponentContext context)
        {
            var operand = Visit(context.op);
            var from = Visit(context.from_);
            var to = Visit(context.to_);
            return new Operand() { Data = new BetweenOperator(operand, from, to) };
        }

        public override Operand VisitLists([NotNull] VtlParser.ListsContext context)
        {
            var constantList = new List<DataType>();
            foreach (var scalarItem in context.scalarItem())
            {
                constantList.Add(Visit(scalarItem).Data);
            }

            if (constantList[0] is IntegerType)
            {
                return new Operand()
                {
                    Data = new SetScalarType<IntegerType>(constantList.OfType<IntegerType>().ToArray())
                };
            }
            if (constantList[0] is NumberType)
            {
                return new Operand()
                {
                    Data = new SetScalarType<NumberType>(constantList.OfType<NumberType>().ToArray())
                };
            }
            if (constantList[0] is StringType)
            {
                return new Operand()
                {
                    Data = new SetScalarType<StringType>(constantList.OfType<StringType>().ToArray())
                };
            }
            if (constantList[0] is BooleanType)
            {
                return new Operand()
                {
                    Data = new SetScalarType<BooleanType>(constantList.OfType<BooleanType>().ToArray())
                };
            }

            throw new NotImplementedException();
        }


    }
}
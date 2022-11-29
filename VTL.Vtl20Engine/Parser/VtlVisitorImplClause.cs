using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ClauseOperator;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.Parser
{
    public partial class VtlVisitorImpl
    {
        public override Operand VisitClauseExpr([NotNull] VtlParser.ClauseExprContext context)
        {
            if(context.ChildCount > 4)
            {
                throw new Exception("Anropet har för många argument.");
            }
            _stack.Push(Visit(context.expr()));
            var op = Visit(context.datasetClause());
            _stack.Pop();
            return op;
        }

        public override Operand VisitDatasetClause(VtlParser.DatasetClauseContext context)
        {
            if (context.filterClause() != null)
            {
                return Visit(context.filterClause());
            }

            if (context.calcClause() != null)
            {
                return Visit(context.calcClause());
            }

            if (context.keepOrDropClause() != null)
            {
                return Visit(context.keepOrDropClause());
            }

            if (context.renameClause() != null)
            {
                return Visit(context.renameClause());
            }

            if (context.pivotOrUnpivotClause() != null)
            {
                return Visit(context.pivotOrUnpivotClause());
            }

            if (context.subspaceClause() != null)
            {
                return Visit(context.subspaceClause());
            }
            return base.VisitDatasetClause(context);
        }

        public override Operand VisitFilterClause(VtlParser.FilterClauseContext context)
        {
            var filterCondition = Visit(context.exprComponent());
            return new Operand() { Data = new FilterOperator(_stack.Peek(), filterCondition) };
        }

        public override Operand VisitCalcClause(VtlParser.CalcClauseContext context)
        {
            var components = new List<Operand>();
            foreach (var calcClauseItemContext in context.calcClauseItem())
            {
                components.Add(Visit(calcClauseItemContext));
            }

            return new Operand() { Data = new CalcOperator(_stack.Peek(), components.ToArray()) };
        }


        public override Operand VisitCalcClauseItem(VtlParser.CalcClauseItemContext context)
        {
            var id = new Operand() { Alias = context.componentID().GetText() };
            var res = Visit(context.exprComponent());

            if (context.componentRole() != null)
            {
                if (context.componentRole().DIMENSION() != null)
                {
                    id.Role = ComponentType.ComponentRole.Identifier;
                }
                else if (context.componentRole().MEASURE() != null)
                {
                    id.Role = ComponentType.ComponentRole.Measure;
                }
                else if (context.componentRole().ATTRIBUTE() != null)
                {
                    id.Role = ComponentType.ComponentRole.Attribure;
                }
            }
            else
            {
                id.Role = ComponentType.ComponentRole.Measure;
            }

            if (res.Data is ScalarType scalar)
            {
                var component = new ComponentType(scalar.GetType(), VtlEngine.DataContainerFactory.CreateComponentContainer(1))
                {
                    scalar
                };

                id.Data = component;
            }
            else
            {
                id.Data = res.Data;
            }

            return id;
        }

        public override Operand VisitKeepOrDropClause([NotNull] VtlParser.KeepOrDropClauseContext context)
        {
            if (context.KEEP() != null)
            {
                var components = new List<Operand>();
                foreach (var componentId in context.componentID())
                {
                    components.Add(Visit(componentId));
                }

                return new Operand() { Data = new KeepOperator(_stack.Peek(), components.ToArray()) };
            }
            else if (context.DROP() != null)
            {
                var components = new List<Operand>();
                foreach (var componentId in context.componentID())
                {
                    components.Add(Visit(componentId));
                }

                return new Operand() { Data = new DropOperator(_stack.Peek(), components.ToArray()) };
            }
            return base.VisitKeepOrDropClause(context);
        }

        public override Operand VisitRenameClause(VtlParser.RenameClauseContext context)
        {
            var fromTo = new List<Tuple<string, string>>();

            foreach(var renameClauseItem in context.renameClauseItem())
            {
                fromTo.Add(new Tuple<string, string>(renameClauseItem.fromName.GetText(), renameClauseItem.toName.GetText()));
            }

            return new Operand() { Data = new RenameOperator(_stack.Peek(), fromTo) };
        }

        public override Operand VisitPivotOrUnpivotClause([NotNull] VtlParser.PivotOrUnpivotClauseContext context)
        {
            if (context.PIVOT() != null)
            {
                var arguments = context.componentID();
                if (arguments.Length != 2)
                {
                    throw new Exception("Pivot kräver två parametrar.");
                }

                var identifier = Visit(context.id_);
                var measure = Visit(context.mea);

                return new Operand() { Data = new PivotOperator(_stack.Peek(), identifier.Alias, measure.Alias, _validation) };
            }
            else if(context.UNPIVOT() != null)
            {
                throw new NotImplementedException("Kommandot unpivot är inte implementerat.");
            }
            return base.VisitPivotOrUnpivotClause(context);
        }

        public override Operand VisitSubspaceClause([NotNull] VtlParser.SubspaceClauseContext context)
        {
            var varIdContexts = context.subspaceClauseItem();
            var constants = new Operand[varIdContexts.Length];
            for (var i = 0; i < varIdContexts.Length; i++)
            {
                var identifier = Visit(varIdContexts[i].componentID());
                var constant = Visit(varIdContexts[i].scalarItem());

                constants[i] = new Operand() { Alias = identifier.Alias, Data = constant.Data, Role = identifier.Role };

            }

            return new Operand() { Data = new SubspaceOperator(_stack.Peek(), constants) };

        }

    }
}
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.HierarchicalAggregation;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.RulesetType;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.Parser
{
    public partial class VtlVisitorImpl
    {
        public Dictionary<string, HierarchicalRuleset> HierarchicalRulesets;

        public override Operand VisitDefHierarchical(VtlParser.DefHierarchicalContext context)
        {
            if (HierarchicalRulesets == null)
            {
                HierarchicalRulesets = new Dictionary<string, HierarchicalRuleset>();
            }

            HierarchicalRuleset ruleset;
            if (context.hierRuleSignature().VARIABLE() != null)
            {
                ruleset = new HierarchicalValueDomainRuleset();
            }
            else
            {
                ruleset = new HierarchicalVariableRuleset();
            }

            ruleset.Name = context.rulesetID().GetText();
            ruleset.Signature = context.hierRuleSignature().IDENTIFIER().GetText();
            ruleset.Rules = new Collection<HierarchicalRule>();

            foreach (var ruleItemHierarchicalContext in context.ruleClauseHierarchical().ruleItemHierarchical())
            {
                var resOperand = Visit(ruleItemHierarchicalContext);
                if (resOperand.Data is HierarchicalRule hierarchicalRule)
                {
                    ruleset.Rules.Add(hierarchicalRule);
                }
            }

            if (HierarchicalRulesets.ContainsKey(ruleset.Name))
            {
                throw new Exception($"Hierarchical ruleset with name {ruleset.Name} already defined.");
            }
            HierarchicalRulesets.Add(ruleset.Name, ruleset);

            return new Operand { Data = new NumberType(0) };
        }

        public override Operand VisitRuleItemHierarchical(VtlParser.RuleItemHierarchicalContext context)
        {
            if(context.codeItemRelation()?.comparisonOperand() == null)
            {
                throw new Exception($"Felaktigt formaterat regelnamn: {context.GetText()}. Tillåtna tecken är a-z, 0-9, _ och .");
            }

            var hierarchicalRule = new HierarchicalRule
            {
                RuleName = context.IDENTIFIER()?.GetText()
            };

            var leftOperand = Visit(context.codeItemRelation());
            if (leftOperand.Data is CodeItemRelation leftRelation)
            {
                hierarchicalRule.LeftCodeItem = leftRelation;
            }

            if (context.codeItemRelation().WHEN() != null)
            {
                hierarchicalRule.WhenCondition = context.codeItemRelation().exprComponent().GetText();
            }

            if (context.erCode() != null)
            {
                hierarchicalRule.ErrorCode = context.erCode().constant().GetText();
            }

            if (context.erLevel() != null)
            {
                hierarchicalRule.ErrorLevel = int.Parse(context.erLevel().constant().GetText());
            }

            var rightRelations = new Collection<CodeItemRelation>();
            foreach (var codeItemRelationClauseContext in context.codeItemRelation().codeItemRelationClause())
            {
                var rightOperand = Visit(codeItemRelationClauseContext);
                if (rightOperand.Data is CodeItemRelation rightCodeItemRelation)
                {
                    rightRelations.Add(rightCodeItemRelation);
                }
            }

            hierarchicalRule.RightCodeItems = rightRelations;

            return new Operand
            {
                Data = hierarchicalRule
            };
        }

        public override Operand VisitCodeItemRelation([NotNull] VtlParser.CodeItemRelationContext context)
        {
            var codeItemRelation = new CodeItemRelation
            {
                CodeItemName = context.valueDomainValue().GetText()
            };
            switch (context.comparisonOperand().GetText())
            {
                case "=":
                    codeItemRelation.Relation = CodeItemRelationType.Coincides;
                    break;
                case "<":
                    codeItemRelation.Relation = CodeItemRelationType.Implies;
                    break;
                case "<=":
                    codeItemRelation.Relation = CodeItemRelationType.ImpliesOrCoincides;
                    break;
                case ">":
                    codeItemRelation.Relation = CodeItemRelationType.IsImpliedBy;
                    break;
                case ">=":
                    codeItemRelation.Relation = CodeItemRelationType.IsImpliedByOrCoincides;
                    break;
            }

            return new Operand { Data = codeItemRelation };
        }

        public override Operand VisitCodeItemRelationClause([NotNull] VtlParser.CodeItemRelationClauseContext context)
        {
            var codeItemRelation = new CodeItemRelation
            {
                CodeItemName = context.valueDomainValue().GetText()
            };

            if (context.opAdd != null)
            {
                switch (context.opAdd.Text)
                {
                    case "+":
                        codeItemRelation.Operator = CodeItemOperator.With;
                        break;
                    case "-":
                        codeItemRelation.Operator = CodeItemOperator.Without;
                        break;
                }
            }

            return new Operand { Data = codeItemRelation };
        }

        public override Operand VisitHierarchyOperators([NotNull] VtlParser.HierarchyOperatorsContext context)
        {
            var op = Visit(context.expr());
            using (var enumerator = context.children.GetEnumerator())
            {
                // Get ruleset
                enumerator.MoveNext();
                while (enumerator.Current.GetText() != ",")
                {
                    enumerator.MoveNext();
                }
                enumerator.MoveNext();
                var rulesetName = enumerator.Current.GetText();
                if (!HierarchicalRulesets.ContainsKey(rulesetName))
                {
                    throw new Exception($"Ruleset med namn {rulesetName} hittades inte.");
                }
                var ruleset = HierarchicalRulesets[rulesetName];

                string componentName = "";
                while (enumerator.MoveNext())
                {
                    // Get the name for the component to apply the rule to
                    if (enumerator.Current.GetText().Equals("rule") && string.IsNullOrEmpty(componentName))
                    {
                        enumerator.MoveNext();
                        componentName = enumerator.Current.GetText();
                    }
                }

                InputModeHierarchy input = InputModeHierarchy.Rule;
                if (context.inputModeHierarchy()?.DATASET() != null)
                {
                    input = InputModeHierarchy.DataSet;
                }
                else if (context.inputModeHierarchy()?.RULE_PRIORITY() != null)
                {
                    input = InputModeHierarchy.RulePriority;
                }

                ValidationMode mode = ValidationMode.NonNull;
                if (context.validationMode()?.NON_NULL() != null)
                {
                    mode = ValidationMode.NonNull;
                }
                else if (context.validationMode()?.NON_ZERO() != null)
                {
                    mode = ValidationMode.NonZero;
                }
                else if (context.validationMode()?.PARTIAL_NULL() != null)
                {
                    mode = ValidationMode.PartialNull;
                }
                else if (context.validationMode()?.PARTIAL_ZERO() != null)
                {
                    mode = ValidationMode.PartialZero;
                }
                else if (context.validationMode()?.ALWAYS_NULL() != null)
                {
                    mode = ValidationMode.AlwaysNull;
                }
                else if (context.validationMode()?.ALWAYS_ZERO() != null)
                {
                    mode = ValidationMode.AlwaysZero;
                }

                OutputModeHierarchy output = OutputModeHierarchy.Computed;
                if (context.outputModeHierarchy()?.COMPUTED() != null)
                {
                    output = OutputModeHierarchy.Computed;
                }
                else if (context.outputModeHierarchy()?.ALL() != null)
                {
                    output = OutputModeHierarchy.All;
                }

                return new Operand() { Data = new Hierarchy(op, ruleset, componentName, input, mode, output) };
            }

        }
    }
}
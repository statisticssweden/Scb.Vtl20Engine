using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Text;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.DataValidationOperator;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.HierarchicalAggregation;

namespace VTL.Vtl20Engine.Parser
{
    public partial class VtlVisitorImpl
    {

        public override Operand VisitValidationSimple([NotNull] VtlParser.ValidationSimpleContext context)
        {
            Operand op = Visit(context.op);
            Operand imbalance = context.imbalanceExpr() != null ? Visit(context.imbalanceExpr()) : null;
            Operand errorcode = context.erCode() != null ? Visit(context.erCode()) : null;
            Operand errorlevel = context.erLevel() != null ? Visit(context.erLevel()) : null;
            var invalid = context.INVALID() != null;

            return new Operand { Data = new CheckOperator(op, errorcode, errorlevel, imbalance, invalid) };
        }

        public override Operand VisitValidateHRruleset([NotNull] VtlParser.ValidateHRrulesetContext context)
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

                InputMode input = InputMode.DataSet;
                if (context.inputMode()?.DATASET_PRIORITY() != null)
                {
                    input = InputMode.DataSetPriority;
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

                ValidationOutput output = ValidationOutput.Invalid;
                if (context.validationOutput()?.ALL() != null)
                {
                    output = ValidationOutput.All;
                }
                else if (context.validationOutput()?.ALL_MEASURES() != null)
                {
                    output = ValidationOutput.All_measures;
                }

                return new Operand() { Data = new CheckHierarchy(op, ruleset, componentName, input, mode, output) };
            }
        }

    }
}

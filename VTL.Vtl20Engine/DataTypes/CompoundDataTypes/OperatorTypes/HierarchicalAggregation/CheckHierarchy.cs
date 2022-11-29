using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.RulesetType;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.HierarchicalAggregation
{
    public enum InputMode
    {
        DataSet,
        DataSetPriority
    }

    public enum ValidationOutput
    {
        Invalid,
        All,
        All_measures
    }

    public class CheckHierarchy : Operator
    {
        protected Operand InOperand;
        private HierarchicalRuleset _ruleset;
        private string _componentName;
        private InputMode _input;
        private ValidationMode _mode;
        private ValidationOutput _output;

        public CheckHierarchy(
            Operand inOperand,
            HierarchicalRuleset ruleset,
            string componentName,
            InputMode input,
            ValidationMode mode,
            ValidationOutput output)
        {
            InOperand = inOperand;
            _ruleset = ruleset;
            _componentName = componentName;
            _input = input;
            _mode = mode;
            _output = output;
        }

        public IEnumerable<CodeItemRelation> GetNestedRule(string rule, bool parentHaveWithoutOperator = false)
        {
            var nested = _ruleset.Rules.FirstOrDefault(r => r.LeftCodeItem.CodeItemName.Equals(rule));
            if (nested == null) return null;
            var result = new List<CodeItemRelation>();

            foreach (var codeItem in nested.RightCodeItems)
            {

                if (codeItem.CodeItemName.Equals(rule))
                {
                    var newCodeItem = CopyCodeItem(codeItem);
                    newCodeItem.Operator = parentHaveWithoutOperator ? CodeItemOperator.Without : CodeItemOperator.With;
                    result.Add(newCodeItem);

                }
                else
                {
                    var withoutOperator = codeItem.Operator == CodeItemOperator.Without ^ parentHaveWithoutOperator;

                    var nestedCodeItems = GetNestedRule(codeItem.CodeItemName, withoutOperator);
                    if (nestedCodeItems != null && nestedCodeItems.Any())
                    {
                        result.AddRange(nestedCodeItems);
                    }
                    else
                    {
                        var newCodeItem = CopyCodeItem(codeItem);

                        newCodeItem.Operator = withoutOperator ? CodeItemOperator.Without : CodeItemOperator.With;
                        result.Add(newCodeItem);
                    }
                }
            }

            return result;
        }

        private CodeItemRelation CopyCodeItem(CodeItemRelation codeItem)
        {
            var newCodeItem = new CodeItemRelation
            {
                CodeItemName = codeItem.CodeItemName,
                Condition = codeItem.Condition,
                Operator = codeItem.Operator,
                Relation = codeItem.Relation
            };
            return newCodeItem;
        }

        internal override DataType PerformCalculation()
        {
            var dataSet = InOperand.GetValue() as DataSetType;

            if (dataSet == null)
            {
                throw new Exception($"Inparametern {InOperand.Alias} till hierarchy saknade värde.");
            }

            // reorder identifiers to get rule component last
            var sortOrder = dataSet.DataSetComponents
                .Where(c => c.Role == ComponentType.ComponentRole.Identifier)
                .Select(c => c.Name)
                .Except(new string[] { _componentName }).ToArray();
            dataSet.SortComponents(sortOrder);
            dataSet.SortDataPoints(sortOrder);
            var ruleComponentIndex = dataSet.IndexOfComponent(_componentName);

            var measure = dataSet.DataSetComponents.FirstOrDefault(m => m.Role == ComponentType.ComponentRole.Measure);
            var valueComponentIndex = dataSet.IndexOfComponent(measure.Name);

            var componentCount = _output == ValidationOutput.All_measures ?
                dataSet.DataSetComponents.Length + 5 : dataSet.DataSetComponents.Length + 4;

            var resultDataPoints = VtlEngine.DataContainerFactory.CreateDataPointContainer(
                dataSet.DataPointCount * componentCount);

            var resultStructureList = _output != ValidationOutput.All ? dataSet.DataSetComponents.ToList() :
                dataSet.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier).ToList();
            resultStructureList.Add(new ComponentType(typeof(StringType), new DataSetComponentContainer(resultDataPoints, "ruleid"))
            { Role = ComponentType.ComponentRole.Identifier, Name = "ruleid" });
            if (_output != ValidationOutput.Invalid)
            {
                resultStructureList.Add(new ComponentType(typeof(BooleanType), new DataSetComponentContainer(resultDataPoints, "bool_var"))
                { Role = ComponentType.ComponentRole.Measure, Name = "bool_var" });
            }
            resultStructureList.Add(new ComponentType(typeof(NumberType), new DataSetComponentContainer(resultDataPoints, "imbalance"))
            { Role = ComponentType.ComponentRole.Measure, Name = "imbalance" });
            resultStructureList.Add(new ComponentType(typeof(StringType), new DataSetComponentContainer(resultDataPoints, "errorcode"))
            { Role = ComponentType.ComponentRole.Measure, Name = "errorcode" });
            resultStructureList.Add(new ComponentType(typeof(IntegerType), new DataSetComponentContainer(resultDataPoints, "errorlevel"))
            { Role = ComponentType.ComponentRole.Measure, Name = "errorlevel" });
            var resultStructure = resultStructureList.ToArray();

            var inIdentifiers = dataSet.DataSetComponents
                .Where(c => c.Role == ComponentType.ComponentRole.Identifier)
                .Select(c => c.Name);
            resultDataPoints.OriginalComponentOrder = resultStructure.Select(c => c.Name).ToArray();
            resultDataPoints.OriginalSortOrder =
                resultStructure.Select(x => new OrderByName() { ComponentName = x.Name }).ToArray();

            var dataPointEnumerator = dataSet.DataPoints.GetEnumerator();
            DataPointType lastDataPoint = null;
            var partition = new List<DataPointType>();

            while (dataPointEnumerator.MoveNext())
            {
                var currentDataPoint = dataPointEnumerator.Current as DataPointType;
                if (currentDataPoint == null) continue;

                // Är vi fortfarande i samma partition d.v.s.
                // har den nya datapunkten andra identifiers än den förra?
                if (lastDataPoint != null && !SameIdentifiers(currentDataPoint, lastDataPoint, ruleComponentIndex))
                {
                    foreach (var dp in PerformCalculation(partition, ruleComponentIndex, valueComponentIndex, resultStructure))
                    {
                        // depending on mode
                        resultDataPoints.Add(dp);
                    }

                    partition = new List<DataPointType>();
                }

                lastDataPoint = currentDataPoint;
                partition.Add(currentDataPoint);
            }

            foreach (var dp in PerformCalculation(partition, ruleComponentIndex, valueComponentIndex, resultStructure))
            {
                resultDataPoints.Add(dp);
            }

            return new DataSetType(resultStructure, resultDataPoints);
        }

        private IEnumerable<DataPointType> PerformCalculation(IEnumerable<DataPointType> partition,
            int ruleComponentIndex, int valueComponentIndex, ComponentType[] resultStructure)
        {
            var imbalanceComponentIndex = IndexOf(resultStructure, "imbalance");

            var result = new List<DataPointType>();
            for (int i = 0; i < _ruleset.Rules.Count(); i++)
            {
                var rule = _ruleset.Rules.ElementAt(i);
                var leftDp = partition.FirstOrDefault(dp => (StringType)dp[ruleComponentIndex] == rule.LeftCodeItem.CodeItemName);
                var dataPoint = new DataPointType(resultStructure.Count());

                // Get partition identifier values
                for (int j = 0; j < partition.FirstOrDefault().Count(); j++)
                {
                    dataPoint[j] = partition.FirstOrDefault()[j];
                }

                // Get partition measure value
                if (_output != ValidationOutput.All)
                {
                    if (leftDp != null)
                    {
                        dataPoint[valueComponentIndex] = leftDp[valueComponentIndex];
                    }
                    else
                    {
                        if (_mode == ValidationMode.NonNull || _mode == ValidationMode.PartialNull || _mode == ValidationMode.AlwaysNull)
                        {
                            dataPoint[valueComponentIndex] = new NullType();
                        }
                        else
                        {
                            dataPoint[valueComponentIndex] = new IntegerType(0);
                        }
                    }
                }

                var valueComponentType = partition.FirstOrDefault()[valueComponentIndex].GetType();

                // Get aditional partition values
                dataPoint[ruleComponentIndex] = new StringType(rule.LeftCodeItem.CodeItemName);
                dataPoint[IndexOf(resultStructure, "ruleid")] = rule.RuleName != null ?
                    new StringType(rule.RuleName) : new StringType((i + 1).ToString());
                if (_output != ValidationOutput.Invalid)
                {
                    dataPoint[IndexOf(resultStructure, "bool_var")] = new BooleanType(null);
                }
                dataPoint[imbalanceComponentIndex] = null;
                dataPoint[IndexOf(resultStructure, "errorcode")] = new StringType(null);
                dataPoint[IndexOf(resultStructure, "errorlevel")] = new IntegerType(null);

                // Gå igenom alla code items i relationerna regelns högra sida
                var codeItems = _input == InputMode.DataSet ||
                    _input == InputMode.DataSetPriority && !dataPoint[valueComponentIndex].HasValue() ?
                    rule.RightCodeItems : GetNestedRule(rule.LeftCodeItem.CodeItemName);
                foreach (var codeItem in codeItems)
                {
                    var foundDp = partition.FirstOrDefault(dp =>
                        dp[ruleComponentIndex].ToString().Equals(codeItem.CodeItemName));
                    if (foundDp == null)
                    {
                        if (_mode == ValidationMode.NonNull || _mode == ValidationMode.PartialNull || _mode == ValidationMode.AlwaysNull)
                        {
                            dataPoint[imbalanceComponentIndex] = new IntegerType(null);
                        }
                        else
                        {
                            if (dataPoint[imbalanceComponentIndex] == null)
                            {
                                dataPoint[imbalanceComponentIndex] = new IntegerType(0);
                            }
                        }
                    }
                    else
                    {
                        if (dataPoint[imbalanceComponentIndex] == null)
                        {
                            dataPoint[imbalanceComponentIndex] = foundDp[valueComponentIndex];
                        }
                        else if (!dataPoint[imbalanceComponentIndex].HasValue() ||
                           !foundDp[valueComponentIndex].HasValue())
                        {
                            dataPoint[imbalanceComponentIndex] = new IntegerType(null);
                        }
                        else if (codeItem.Operator == CodeItemOperator.Without)
                        {
                            dataPoint[imbalanceComponentIndex] =
                                valueComponentType == typeof(IntegerType) ?
                                    (IntegerType)dataPoint[imbalanceComponentIndex] -
                                    (IntegerType)foundDp[valueComponentIndex] :
                                    (NumberType)dataPoint[imbalanceComponentIndex] -
                                    (NumberType)foundDp[valueComponentIndex];
                        }
                        else
                        {
                            dataPoint[imbalanceComponentIndex] =
                                valueComponentType == typeof(IntegerType) ?
                                    (IntegerType)dataPoint[imbalanceComponentIndex] +
                                    (IntegerType)foundDp[valueComponentIndex] :
                                    (NumberType)dataPoint[imbalanceComponentIndex] +
                                    (NumberType)foundDp[valueComponentIndex];
                        }
                    }
                }

                // Uppdatera imbalance med värdet från regelns vänstra sida
                if (leftDp == null)
                {
                    if (_mode == ValidationMode.NonNull || _mode == ValidationMode.PartialNull || _mode == ValidationMode.AlwaysNull)
                    {
                        dataPoint[imbalanceComponentIndex] = new IntegerType(null);
                    }
                    else
                    {
                        if (dataPoint[imbalanceComponentIndex] == null)
                        {
                            dataPoint[imbalanceComponentIndex] = new IntegerType(0);
                        }
                    }
                }
                else if (leftDp[valueComponentIndex] is NullType)
                {
                    dataPoint[imbalanceComponentIndex] = new IntegerType(null);
                }
                else
                {
                    if (dataPoint[imbalanceComponentIndex] is NullType)
                    {
                        dataPoint[imbalanceComponentIndex] = new IntegerType(0);
                    }
                    dataPoint[imbalanceComponentIndex] =
                        valueComponentType == typeof(IntegerType) ?
                            (IntegerType)leftDp[valueComponentIndex] -
                            (IntegerType)dataPoint[imbalanceComponentIndex] :
                            (NumberType)leftDp[valueComponentIndex] -
                            (NumberType)dataPoint[imbalanceComponentIndex];
                }

                // Beräkna om regelns villkor är uppfyllt eller inte
                var imbalance = dataPoint[imbalanceComponentIndex] as NumberType;
                bool? bool_var = null;
                if (imbalance == null || !imbalance.HasValue())
                {
                    dataPoint[IndexOf(resultStructure, "imbalance")] = new NumberType(null);
                    if (_output != ValidationOutput.Invalid)
                    {
                        dataPoint[IndexOf(resultStructure, "bool_var")] = new BooleanType(null);
                    }
                }
                else
                {
                    switch (rule.LeftCodeItem.Relation)
                    {
                        // =
                        case CodeItemRelationType.Coincides:
                            bool_var = imbalance == 0;
                            break;
                        // <
                        case CodeItemRelationType.Implies:
                            bool_var = imbalance < 0;
                            break;
                        // <=
                        case CodeItemRelationType.ImpliesOrCoincides:
                            bool_var = imbalance <= 0;
                            break;
                        // >
                        case CodeItemRelationType.IsImpliedBy:
                            bool_var = imbalance > 0;
                            break;
                        // >=
                        case CodeItemRelationType.IsImpliedByOrCoincides:
                            bool_var = imbalance >= 0;
                            break;
                    }
                    if (bool_var == false)
                    {
                        dataPoint[IndexOf(resultStructure, "errorcode")] = new StringType(rule.ErrorCode);
                        dataPoint[IndexOf(resultStructure, "errorlevel")] = new IntegerType(rule.ErrorLevel);
                    }
                    if (_output != ValidationOutput.Invalid)
                    {
                        dataPoint[IndexOf(resultStructure, "bool_var")] = new BooleanType(bool_var);
                    }
                }

                if (_output != ValidationOutput.Invalid || bool_var == false)
                {
                    result.Add(dataPoint);
                }
            }

            return result;
        }

        private int IndexOf(ComponentType[] resultStructure, string componentName)
        {
            return Array.IndexOf(resultStructure, resultStructure.FirstOrDefault(c => c.Name == componentName));
        }

        private static bool SameIdentifiers(DataPointType currentDataPoint, DataPointType lastDataPoint,
            int componentIndex)
        {
            if (lastDataPoint == null || currentDataPoint == null)
                return false;

            for (int i = 0; i < componentIndex; i++)
            {
                if (!currentDataPoint[i].Equals(lastDataPoint[i]))
                    return false;
            }

            return true;
        }

        public CodeItemRelation IncludedInRule(string item, HierarchicalRule rule)
        {
            var codeItems = new List<CodeItemRelation>();
            foreach (var codeItem in rule.RightCodeItems)
            {
                if (codeItem.CodeItemName.Equals(rule.LeftCodeItem.CodeItemName))
                {
                    codeItems.Add(codeItem);
                }
                else
                {
                    var nestedRules = _ruleset.Rules.Where(r =>
                        r.LeftCodeItem.CodeItemName.Equals(codeItem.CodeItemName));
                    if (nestedRules.Any())
                    {
                        codeItems.AddRange(nestedRules.Select(r => IncludedInRule(item, r)));
                    }
                    else
                    {
                        codeItems.Add(codeItem);
                    }
                }
            }

            return codeItems.FirstOrDefault(i => i != null && i.CodeItemName.Equals(item));
        }


        internal override string[] GetComponentNames()
        {
            var resultStructureList = _output != ValidationOutput.All ?
                InOperand.GetComponentNames().ToList() :
                InOperand.GetIdentifierNames().ToList();
            resultStructureList.Add("ruleid");
            if (_output != ValidationOutput.Invalid)
            {
                resultStructureList.Add("bool_var");
            }
            resultStructureList.Add("imbalance");
            resultStructureList.Add("errorcode");
            resultStructureList.Add("errorlevel");
            return resultStructureList.ToArray();
        }

        internal override string[] GetIdentifierNames()
        {
            var resultStructureList = InOperand.GetIdentifierNames().ToList();
            resultStructureList.Add("ruleid");
            return resultStructureList.ToArray();
        }

        internal override string[] GetMeasureNames()
        {
            var resultStructureList = new List<string>();
            if (_output != ValidationOutput.All)
            {
                resultStructureList.AddRange(InOperand.GetMeasureNames().ToList());
            }
            if (_output != ValidationOutput.Invalid)
            {
                resultStructureList.Add("bool_var");
            }
            resultStructureList.Add("imbalance");
            resultStructureList.Add("errorcode");
            resultStructureList.Add("errorlevel");
            return resultStructureList.ToArray();
        }

    }
}

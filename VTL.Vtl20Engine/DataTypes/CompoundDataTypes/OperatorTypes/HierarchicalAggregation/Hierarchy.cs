using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.RulesetType;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.HierarchicalAggregation
{
    public enum InputModeHierarchy
    {
        DataSet,
        Rule,
        RulePriority
    }

    public enum ValidationMode
    {
        NonNull,
        NonZero,
        PartialNull,
        PartialZero,
        AlwaysNull,
        AlwaysZero
    }

    public enum OutputModeHierarchy
    {
        Computed,
        All
    }

    public class Hierarchy : Operator
    {
        protected Operand InOperand;
        private readonly HierarchicalRuleset _ruleset;
        private readonly string _componentName;
        private readonly InputModeHierarchy _input;
        private readonly ValidationMode _mode;
        private readonly OutputModeHierarchy _output;

        public Hierarchy(Operand inOperand,
            HierarchicalRuleset ruleset,
            string componentName,
            InputModeHierarchy input,
            ValidationMode mode,
            OutputModeHierarchy output)
        {
            InOperand = inOperand;
            _ruleset = ruleset;
            _componentName = componentName;
            _input = input;
            _mode = mode;
            _output = output;
            if (_mode != ValidationMode.AlwaysZero)
            {
                throw new NotImplementedException("Endast mode always_zero är implementerat i hierarchy.");
            }
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
            var valueComponentDataType = dataSet.DataSetComponents[valueComponentIndex].DataType;

            var resultDataPoints = VtlEngine.DataContainerFactory.CreateDataPointContainer(dataSet.DataPointCount * dataSet.DataSetComponents.Length);
            resultDataPoints.OriginalComponentOrder = dataSet.ComponentSortOrder;
            resultDataPoints.OriginalSortOrder = dataSet.DataPoints.SortOrder ?? dataSet.DataPoints.OriginalSortOrder;

            if (dataSet.DataPointCount == 0 && _mode == ValidationMode.AlwaysZero)
            {
                return new DataSetType(dataSet.DataSetComponents, resultDataPoints);
            }

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
                    foreach (var dp in PerformCalculation(partition, ruleComponentIndex, valueComponentIndex, valueComponentDataType))
                    {
                        // depending on mode
                        resultDataPoints.Add(dp);
                    }

                    partition = new List<DataPointType>();
                }

                lastDataPoint = currentDataPoint;
                partition.Add(currentDataPoint);
            }

            foreach (var dp in PerformCalculation(partition, ruleComponentIndex, valueComponentIndex, valueComponentDataType))
            {
                resultDataPoints.Add(dp);
            }
            resultDataPoints.CompleteWrite();

            if (_output == OutputModeHierarchy.All)
            {
                var allResult = VtlEngine.DataContainerFactory.CreateDataPointContainer(
                    dataSet.DataPointCount * dataSet.DataSetComponents.Length);
                allResult.OriginalComponentOrder = dataSet.ComponentSortOrder;
                var inEnumerator = dataSet.DataPoints.GetEnumerator();
                var outEnumerator = resultDataPoints.GetEnumerator();
                var inOk = inEnumerator.MoveNext();
                var outOk = outEnumerator.MoveNext();
                var dataPointCompprer = new DataPointComparer(Enumerable.Range(0, ruleComponentIndex + 1).ToArray());

                while (inOk || outOk)
                {
                    if (inOk && outOk)
                    {
                        var inDataPoint = inEnumerator.Current as DataPointType;
                        var outDataPoint = outEnumerator.Current;

                        var compare = dataPointCompprer.Compare(inDataPoint, outDataPoint);
                        if (compare < 0)
                        {
                            allResult.Add(inDataPoint);
                            inOk = inEnumerator.MoveNext();
                        }
                        else if (compare > 0)
                        {
                            allResult.Add(outDataPoint);
                            outOk = outEnumerator.MoveNext();
                        }
                        else
                        {
                            allResult.Add(outDataPoint);
                            inOk = inEnumerator.MoveNext();
                            outOk = outEnumerator.MoveNext();
                        }
                    }
                    else if (outOk)
                    {
                        allResult.Add(outEnumerator.Current);
                        outOk = outEnumerator.MoveNext();
                    }
                    else if (inOk)
                    {
                        allResult.Add(inEnumerator.Current as DataPointType);
                        inOk = inEnumerator.MoveNext();
                    }
                }
                resultDataPoints = allResult;
            }

            return new DataSetType(dataSet.DataSetComponents, resultDataPoints);
        }

        private IEnumerable<DataPointType> PerformCalculation(IEnumerable<DataPointType> partition,
            int ruleComponentIndex, int valueComponentIndex, Type valueComponentType)
        {
            var result = new List<DataPointType>();
            foreach (var rule in _ruleset.Rules)
            {
                var dataPoint = new DataPointType(partition.FirstOrDefault());
                dataPoint[ruleComponentIndex] = new StringType(rule.LeftCodeItem.CodeItemName);
                dataPoint[valueComponentIndex] = new IntegerType(null);
                bool found1 = false, found2 = false;


                if (_input != InputModeHierarchy.DataSet)
                {
                    var nestedRule = GetNestedRule(rule.LeftCodeItem.CodeItemName);
                    foreach (var codeItem in nestedRule)
                    {
                        var foundDp = partition.FirstOrDefault(dp =>
                            dp[ruleComponentIndex].ToString().Equals(codeItem.CodeItemName));
                        if (foundDp != null)
                        {
                            if (!found1)
                            {
                                found1 = true;
                                dataPoint[valueComponentIndex] = new IntegerType(0);
                            }

                            if (codeItem.Operator == CodeItemOperator.Without)
                            {
                                dataPoint[valueComponentIndex] =
                                    valueComponentType == typeof(IntegerType) ?
                                        (IntegerType)dataPoint[valueComponentIndex] -
                                        (IntegerType)foundDp[valueComponentIndex] :
                                        (NumberType)dataPoint[valueComponentIndex] -
                                        (NumberType)foundDp[valueComponentIndex];
                            }
                            else
                            {
                                dataPoint[valueComponentIndex] =
                                    valueComponentType == typeof(IntegerType) ?
                                        (IntegerType)dataPoint[valueComponentIndex] +
                                        (IntegerType)foundDp[valueComponentIndex] :
                                        (NumberType)dataPoint[valueComponentIndex] +
                                        (NumberType)foundDp[valueComponentIndex];
                            }
                        }
                    }
                }


                if (_input == InputModeHierarchy.DataSet ||
                    _input == InputModeHierarchy.RulePriority && !dataPoint[valueComponentIndex].HasValue())
                {
                    foreach (var codeItem in rule.RightCodeItems)
                    {
                        var foundDp = partition.FirstOrDefault(dp =>
                            dp[ruleComponentIndex].ToString().Equals(codeItem.CodeItemName));
                        if (foundDp != null)
                        {
                            if (!found2)
                            {
                                found2 = true;
                                dataPoint[valueComponentIndex] = new IntegerType(0);
                            }

                            if (codeItem.Operator == CodeItemOperator.Without)
                            {
                                dataPoint[valueComponentIndex] =
                                    valueComponentType == typeof(IntegerType) ?
                                        (IntegerType)dataPoint[valueComponentIndex] -
                                        (IntegerType)foundDp[valueComponentIndex] :
                                        (NumberType)dataPoint[valueComponentIndex] -
                                        (NumberType)foundDp[valueComponentIndex];
                            }
                            else
                            {
                                dataPoint[valueComponentIndex] =
                                    valueComponentType == typeof(IntegerType) ?
                                        (IntegerType)dataPoint[valueComponentIndex] +
                                        (IntegerType)foundDp[valueComponentIndex] :
                                        (NumberType)dataPoint[valueComponentIndex] +
                                        (NumberType)foundDp[valueComponentIndex];
                            }
                        }
                    }
                }

                if (found1 || found2)
                {
                    result.Add(dataPoint);
                }
                else
                {
                    if (_mode == ValidationMode.AlwaysZero)
                    {
                        dataPoint[valueComponentIndex] = new IntegerType(0);
                        result.Add(dataPoint);
                    }
                }
            }

            return result;
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
            return InOperand.GetComponentNames();
        }

        internal override string[] GetIdentifierNames()
        {
            return InOperand.GetIdentifierNames();
        }

        internal override string[] GetMeasureNames()
        {
            return InOperand.GetMeasureNames();
        }
    }
}
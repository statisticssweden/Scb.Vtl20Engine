using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.JoinOperator
{
    public class LeftJoin : Operator
    {
        private readonly List<Operand> _operands;
        private readonly string[] _usings;

        public LeftJoin(List<Operand> operands, string[] usings)
        {
            _operands = operands;
            _usings = usings;

            var identifierNames = _operands.Select(o => o.GetIdentifierNames());

            var identifiers = identifierNames.ToArray();

            if (_usings == null)
            {
                for (var i = 1; i < identifierNames.Count(); i++)
                {
                    if (!identifiers[i].All(x => identifiers[i - 1].Any(y => y == x)) ||
                        !identifiers[i - 1].All(x => identifiers[i].Any(y => y == x)))
                    {
                        throw new Exception("I left_join måste alla dataset innehålla samma identifierare om inte using används.");
                    }
                }
            }
            else
            {
                for (var i = 0; i < _usings.Count(); i++)
                {
                    for (var j = 0; j < identifierNames.Count(); j++)
                    {
                        if (!identifiers[j].Any(x => x == _usings[i]))
                        {
                            throw new Exception($"Alla komponeter som omnämns i using måste förekomma i alla dataset. {_usings[i]} förekommer inte i {_operands[j].Alias}.");
                        }
                    }
                }

                for (var i = 2; i < identifierNames.Count(); i++)
                {
                    if (!identifiers[i].All(x => identifiers[i - 1].Any(y => y == x)) ||
                        !identifiers[i - 1].All(x => identifiers[i].Any(y => y == x)))
                    {
                        throw new Exception("Dataset till höger om referensdatasetet måste innehålla samma identifierare i left_join");
                    }
                }
            }
        }

        internal override DataType PerformCalculation()
        {
            var dsOperands = _operands.Select(o => o.GetValue()).OfType<DataSetType>().ToArray();
            if (dsOperands.Length != _operands.Count)
            {
                throw new Exception("left_join kan endast hantera dataset.");
            }

            for (int i = 0; i < dsOperands.Length; i++)
            {
                foreach (var dataSetComponent in dsOperands[i].DataSetComponents)
                {
                    dsOperands[i].RenameComponent(dataSetComponent.Name, _operands[i].Alias + "#" + dataSetComponent.Name);
                }
            }

            var intermediateResult = dsOperands[0];
            for (var i = 1; i < dsOperands.Length; i++)
            {
                var datset2 = dsOperands[i];
                intermediateResult = PerformCalculation(intermediateResult, datset2);
            }

            return intermediateResult;
        }

        private DataSetType PerformCalculation(DataSetType ds1, DataSetType ds2)
        {
            var leftDataset = ds1;
            var rightDataset = ds2;

            var supersetIdentifiers = leftDataset.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier)
                .Select(c => c.Name.Contains("#") ? c.Name.Substring(c.Name.IndexOf('#') + 1) : c.Name).ToArray();
            var subsetIdentifiers = rightDataset.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier)
                .Select(c => c.Name.Contains("#") ? c.Name.Substring(c.Name.IndexOf('#') + 1) : c.Name).ToArray();

            var joinKeys = _usings != null ? _usings : subsetIdentifiers;
            var resultComponents = leftDataset.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier).ToList();
            resultComponents.AddRange(rightDataset.DataSetComponents
                .Where(c => c.Role == ComponentType.ComponentRole.Identifier && !joinKeys.Contains(c.Name.Substring(c.Name.IndexOf('#') + 1))));
            resultComponents.AddRange(leftDataset.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Measure).ToList());
            resultComponents.AddRange(rightDataset.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Measure).ToList());

            var result = new DataSetType(resultComponents.ToArray());
            if (leftDataset.DataSetComponents.FirstOrDefault().Name.Contains("#"))
            {
                var dsName = leftDataset.DataSetComponents.FirstOrDefault().Name.Substring(0, leftDataset.DataSetComponents.FirstOrDefault().Name.IndexOf('#'));
                leftDataset.SortDataPoints(joinKeys.Select(i => dsName + "#" + i).ToArray());
            }
            else
            {
                leftDataset.SortDataPoints(joinKeys);
            }
            if (rightDataset.DataSetComponents.FirstOrDefault().Name.Contains("#"))
            {
                var dsName = rightDataset.DataSetComponents.FirstOrDefault().Name.Substring(0, rightDataset.DataSetComponents.FirstOrDefault().Name.IndexOf('#'));
                rightDataset.SortDataPoints(joinKeys.Select(i => dsName + "#" + i).ToArray());
            }
            else
            {
                rightDataset.SortDataPoints(joinKeys);
            }

            var numJoinKeys = joinKeys.Length;
            var dataPointComparer = new DataPointComparer(Enumerable.Range(0, numJoinKeys).ToArray());

            if (_usings == null) //Idn i ds1 och ds2 måste vara samma
            {
                using (var leftDatasetEnumerator = leftDataset.DataPoints.GetEnumerator())
                using (var rightDatasetEnumerator = rightDataset.DataPoints.GetEnumerator())
                {
                    var leftDatasetEnumeratorHasValue = leftDatasetEnumerator.MoveNext();
                    var rightDatasetEnumeratorHasValue = rightDatasetEnumerator.MoveNext();
                    while (leftDatasetEnumeratorHasValue)
                    {
                        var leftDatapoint = leftDatasetEnumerator.Current;
                        var rightDatapoint = rightDatasetEnumerator.Current;

                        var resultDataPoint = new DataPointType(resultComponents.Count);
                        var c = dataPointComparer.Compare(leftDatapoint, rightDatapoint);
                        if (c == 0)
                        {


                            foreach (var identifier in leftDataset.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Identifier))
                            {
                                resultDataPoint[resultComponents.IndexOf(identifier)] = leftDatapoint[leftDataset.IndexOfComponent(identifier)];
                            }

                            foreach (var supersetMeasure in leftDataset.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Measure))
                            {
                                resultDataPoint[resultComponents.IndexOf(supersetMeasure)] = leftDatapoint[leftDataset.IndexOfComponent(supersetMeasure)];
                            }

                            foreach (var subsetMeasure in rightDataset.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Measure))
                            {
                                resultDataPoint[resultComponents.IndexOf(subsetMeasure)] = rightDatapoint[rightDataset.IndexOfComponent(subsetMeasure)];
                            }

                            result.DataPoints.Add(resultDataPoint);

                            leftDatasetEnumeratorHasValue = leftDatasetEnumerator.MoveNext();
                            rightDatasetEnumeratorHasValue = rightDatasetEnumerator.MoveNext();
                        }
                        else if (c < 0 || !rightDatasetEnumeratorHasValue) //LeftDataset har lägre sortering
                        {
                            foreach (var identifier in leftDataset.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Identifier))
                            {
                                resultDataPoint[resultComponents.IndexOf(identifier)] = leftDatapoint[leftDataset.IndexOfComponent(identifier)];
                            }

                            foreach (var supersetMeasure in leftDataset.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Measure))
                            {
                                resultDataPoint[resultComponents.IndexOf(supersetMeasure)] = leftDatapoint[leftDataset.IndexOfComponent(supersetMeasure)];
                            }
                            foreach (var subsetMeasure in rightDataset.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Measure))
                            {
                                resultDataPoint[resultComponents.IndexOf(subsetMeasure)] = new NullType();
                            }

                            result.DataPoints.Add(resultDataPoint);
                            leftDatasetEnumeratorHasValue = leftDatasetEnumerator.MoveNext();
                        }
                        else if (c > 0)
                        {
                            rightDatasetEnumeratorHasValue = rightDatasetEnumerator.MoveNext();

                        }
                    }
                }
            }
            else
            {
                leftDataset.SortDataPoints(_usings.SelectMany(u => leftDataset.DataSetComponents.Select(c => c.Name).Where(c => c.EndsWith($"#{u}"))).ToArray());
                rightDataset.SortDataPoints(_usings.SelectMany(u => rightDataset.DataSetComponents.Select(c => c.Name).Where(c => c.EndsWith($"#{u}"))).ToArray());

                using (var leftDatasetEnumerator = leftDataset.DataPoints.GetEnumerator())
                using (var rightDatasetEnumerator = rightDataset.DataPoints.GetEnumerator())
                {
                    var leftDatasetEnumeratorHasValue = leftDatasetEnumerator.MoveNext();
                    var rightDatasetEnumeratorHasValue = rightDatasetEnumerator.MoveNext();
                    DataPointType rightDatapoint = null;

                    while (leftDatasetEnumeratorHasValue && rightDatasetEnumeratorHasValue)
                    {
                        // Make partition
                        var partition = new List<DataPointType>();
                        var partitionKey = leftDatasetEnumerator.Current;
                        var currentDataPoint = partitionKey;
                        while (dataPointComparer.Compare(currentDataPoint, partitionKey) == 0 && leftDatasetEnumeratorHasValue)
                        {
                            partition.Add(currentDataPoint);
                            leftDatasetEnumeratorHasValue = leftDatasetEnumerator.MoveNext();
                            currentDataPoint = leftDatasetEnumerator.Current;
                        }

                        rightDatapoint = rightDatasetEnumerator.Current;
                        var partitionAdded = false;
                        for (var c = dataPointComparer.Compare(partitionKey, rightDatapoint);
                            rightDatasetEnumeratorHasValue && c >= 0;
                            c = dataPointComparer.Compare(partitionKey, rightDatapoint))
                        {
                            if (c == 0)
                            {
                                foreach (var leftDatapoint in partition)
                                {
                                    var resultDataPoint = new DataPointType(resultComponents.Count);

                                    foreach (var identifier in leftDataset.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Identifier))
                                    {
                                        resultDataPoint[resultComponents.IndexOf(identifier)] = leftDatapoint[leftDataset.IndexOfComponent(identifier)];
                                    }

                                    foreach (var subsetIdentifier in rightDataset.DataSetComponents.
                                        Where(id => id.Role == ComponentType.ComponentRole.Identifier && !joinKeys.Contains(id.Name.Substring(id.Name.IndexOf('#') + 1))))
                                    {
                                        resultDataPoint[resultComponents.IndexOf(subsetIdentifier)] = rightDatapoint[rightDataset.IndexOfComponent(subsetIdentifier)];
                                    }

                                    foreach (var supersetMeasure in leftDataset.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Measure))
                                    {
                                        resultDataPoint[resultComponents.IndexOf(supersetMeasure)] = leftDatapoint[leftDataset.IndexOfComponent(supersetMeasure)];
                                    }

                                    foreach (var subsetMeasure in rightDataset.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Measure))
                                    {
                                        resultDataPoint[resultComponents.IndexOf(subsetMeasure)] = rightDatapoint[rightDataset.IndexOfComponent(subsetMeasure)];
                                    }

                                    result.DataPoints.Add(resultDataPoint);
                                    partitionAdded = true;
                                }
                            }
                            rightDatasetEnumeratorHasValue = rightDatasetEnumerator.MoveNext();
                            rightDatapoint = rightDatasetEnumerator.Current;
                        }
                        if (!partitionAdded)
                        {
                            foreach (var leftDatapoint in partition)
                            {
                                var resultDataPoint = new DataPointType(resultComponents.Count);

                                foreach (var identifier in leftDataset.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Identifier))
                                {
                                    resultDataPoint[resultComponents.IndexOf(identifier)] = leftDatapoint[leftDataset.IndexOfComponent(identifier)];
                                }

                                foreach (var rightIdentifier in rightDataset.DataSetComponents.
                                    Where(id => id.Role == ComponentType.ComponentRole.Identifier && !joinKeys.Contains(id.Name.Substring(id.Name.IndexOf('#') + 1))))
                                {
                                    resultDataPoint[resultComponents.IndexOf(rightIdentifier)] = new NullType();
                                }

                                foreach (var leftMeasure in leftDataset.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Measure))
                                {
                                    resultDataPoint[resultComponents.IndexOf(leftMeasure)] = leftDatapoint[leftDataset.IndexOfComponent(leftMeasure)];
                                }

                                foreach (var rightMeasure in rightDataset.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Measure))
                                {
                                    resultDataPoint[resultComponents.IndexOf(rightMeasure)] = new NullType();
                                }

                                result.DataPoints.Add(resultDataPoint);
                            }
                        }
                    }
                }
            }
            return result;
        }

        internal override string[] GetComponentNames()
        {
            var componentNames = GetIdentifierNames().ToList();
            componentNames.AddRange(GetMeasureNames().ToList());
            return componentNames.ToArray();
        }

        internal override string[] GetIdentifierNames()
        {
            var result = new List<string>();
            if (_usings != null)
            {
                var usings = _usings ?? new string[0];

                result.AddRange(usings);
                foreach (var operand in _operands)
                {
                    result.AddRange(operand.GetIdentifierNames().Except(usings).Select(i => operand.Alias + "#" + i));
                }
                return result.ToArray();
            }
            else
            {
                var added = new List<string>();
                foreach (var operand in _operands)
                {
                    var opNames = operand.GetIdentifierNames();
                    result.AddRange(opNames.Except(added).Select(i => operand.Alias + "#" + i));
                    added.AddRange(opNames);
                }
                return result.ToArray();
            }
        }

        internal override string[] GetMeasureNames()
        {
            return _operands.SelectMany(o => o.GetMeasureNames().Select(n => o.Alias + "#" + n)).ToArray();
        }

    }
}





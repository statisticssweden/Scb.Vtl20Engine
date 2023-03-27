using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.JoinOperator
{
    public class InnerJoin : Operator
    {
        private readonly List<Operand> _operands;
        private readonly string[] _usings;

        public InnerJoin(List<Operand> operands, string[] usings)
        {
            _operands = operands;
            _usings = usings;

            var identifierNames = _operands.Select(o => o.GetIdentifierNames()).ToArray();
            var numIdentifiers = identifierNames.Select(i => i.Length).Max();
            var resultIdentifierNames = identifierNames.First(i => i.Length == numIdentifiers);

            if (_usings == null)
            {
                if (identifierNames.Any(i => !i.All(id => resultIdentifierNames.Contains(id))))
                {
                    throw new Exception("Fler än ett dataset innehåller unika identifierare, varför en inner_join inte var möjlig att utföra.");
                }
            }
            else
            {
                for (var i = 0; i < _usings.Count(); i++)
                {
                    for (var j = 0; j < identifierNames.Count(); j++)
                    {
                        if (!identifierNames[j].Any(x => x == _usings[i]))
                        {
                            throw new Exception($"Alla komponeter som omnämns i using måste förekomma i alla dataset. {_usings[i]} förekommer inte i {_operands[j].Alias}.");
                        }
                    }
                }

                if (identifierNames.Length > 2)
                {

                    var uniqueIdentifiers = identifierNames.Where(i1 => identifierNames.Count(i2 => i2.All(i => i1.Contains(i))) == 1);
                    if (uniqueIdentifiers.Count() > 1)
                    {
                        throw new Exception("Bara ett av dataseten får innehålla unika identifierare i inner_join.");
                    }
                }
            }
        }

        internal override DataType PerformCalculation()
        {
            var dsOperands = _operands.AsParallel().Select(o => o.GetValue()).OfType<DataSetType>().ToArray();
            if (dsOperands.Length != _operands.Count)
            {
                throw new Exception("inner_join kan endast hantera dataset.");
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
            var superset = ds1.DataSetComponents.Length >= ds2.DataSetComponents.Length ? ds1 : ds2;
            var subset = ds1.DataSetComponents.Length >= ds2.DataSetComponents.Length ? ds2 : ds1;

            var supersetIdentifiers = superset.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier)
                .Select(c => c.Name.Contains("#") ? c.Name.Substring(c.Name.IndexOf('#') + 1) : c.Name).ToArray();
            var subsetIdentifiers = subset.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier)
                .Select(c => c.Name.Contains("#") ? c.Name.Substring(c.Name.IndexOf('#') + 1) : c.Name).ToArray();

            var joinKeys = _usings != null ? _usings : subsetIdentifiers;
            var resultComponents = superset.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier).ToList();
            resultComponents.AddRange(subset.DataSetComponents
                .Where(c => c.Role == ComponentType.ComponentRole.Identifier && !joinKeys.Contains(c.Name.Substring(c.Name.IndexOf('#') + 1))));
            resultComponents.AddRange(superset.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Measure).ToList());
            resultComponents.AddRange(subset.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Measure).ToList());

            var result = new DataSetType(resultComponents.ToArray());
            if (superset.DataSetComponents.FirstOrDefault().Name.Contains("#"))
            {
                var dsName = superset.DataSetComponents.FirstOrDefault().Name.Substring(0, superset.DataSetComponents.FirstOrDefault().Name.IndexOf('#'));
                superset.SortDataPoints(joinKeys.Select(i => dsName + "#" + i).ToArray());
            }
            else
            {
                superset.SortDataPoints(joinKeys);
            }
            if (subset.DataSetComponents.FirstOrDefault().Name.Contains("#"))
            {
                var dsName = subset.DataSetComponents.FirstOrDefault().Name.Substring(0, subset.DataSetComponents.FirstOrDefault().Name.IndexOf('#'));
                subset.SortDataPoints(joinKeys.Select(i => dsName + "#" + i).ToArray());
            }
            else
            {
                subset.SortDataPoints(joinKeys);
            }

            var numJoinKeys = joinKeys.Length;
            var dataPointComparer = new DataPointComparer(Enumerable.Range(0, numJoinKeys).ToArray());

            if (_usings == null)
            {
                using (var supersetEnumerator = superset.DataPoints.GetEnumerator())
                using (var subsetEnumerator = subset.DataPoints.GetEnumerator())
                {
                    var supersetEnumeratorHasValue = supersetEnumerator.MoveNext();
                    var subsetEnumeratorHasValue = subsetEnumerator.MoveNext();
                    while (supersetEnumeratorHasValue && subsetEnumeratorHasValue)
                    {
                        var supersetDatapoint = supersetEnumerator.Current;
                        var subsetDatapoint = subsetEnumerator.Current;

                        var c = dataPointComparer.Compare(supersetDatapoint, subsetDatapoint);
                        if (c == 0)
                        {
                            var resultDataPoint = new DataPointType(resultComponents.Count);

                            foreach (var identifier in superset.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Identifier))
                            {
                                resultDataPoint[resultComponents.IndexOf(identifier)] = supersetDatapoint[superset.IndexOfComponent(identifier)];
                            }

                            foreach (var supersetMeasure in superset.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Measure))
                            {
                                resultDataPoint[resultComponents.IndexOf(supersetMeasure)] = supersetDatapoint[superset.IndexOfComponent(supersetMeasure)];
                            }

                            foreach (var subsetMeasure in subset.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Measure))
                            {
                                resultDataPoint[resultComponents.IndexOf(subsetMeasure)] = subsetDatapoint[subset.IndexOfComponent(subsetMeasure)];
                            }

                            result.DataPoints.Add(resultDataPoint);

                            supersetEnumeratorHasValue = supersetEnumerator.MoveNext();
                        }
                        else if (c < 0)
                        {
                            supersetEnumeratorHasValue = supersetEnumerator.MoveNext();
                        }
                        else if (c > 0)
                        {
                            subsetEnumeratorHasValue = subsetEnumerator.MoveNext();
                        }
                    }
                }
            }
            else
            {
                superset.SortDataPoints(_usings.SelectMany(u => superset.DataSetComponents.Select(c => c.Name).Where(c => c.EndsWith($"#{u}"))).ToArray());
                subset.SortDataPoints(_usings.SelectMany(u => subset.DataSetComponents.Select(c => c.Name).Where(c => c.EndsWith($"#{u}"))).ToArray());

                using (var supersetEnumerator = superset.DataPoints.GetEnumerator())
                using (var subsetEnumerator = subset.DataPoints.GetEnumerator())
                {
                    var supersetEnumeratorHasValue = supersetEnumerator.MoveNext();
                    var subsetEnumeratorHasValue = subsetEnumerator.MoveNext();
                    DataPointType subsetDatapoint = null;

                    while (supersetEnumeratorHasValue && subsetEnumeratorHasValue)
                    {
                        // Make partition
                        var partition = new List<DataPointType>();
                        var partitionKey = supersetEnumerator.Current;
                        var currentDataPoint = partitionKey;
                        while (dataPointComparer.Compare(currentDataPoint, partitionKey) == 0 && supersetEnumeratorHasValue)
                        {
                            partition.Add(currentDataPoint);
                            supersetEnumeratorHasValue = supersetEnumerator.MoveNext();
                            currentDataPoint = supersetEnumerator.Current;
                        }

                        subsetDatapoint = subsetEnumerator.Current;
                        for(var c = dataPointComparer.Compare(partitionKey, subsetDatapoint); 
                            subsetEnumeratorHasValue && c >= 0; 
                            c = dataPointComparer.Compare(partitionKey, subsetDatapoint))
                        {
                            if (c == 0)
                            {
                                foreach (var supersetDatapoint in partition)
                                {
                                    var resultDataPoint = new DataPointType(resultComponents.Count);

                                    foreach (var identifier in superset.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Identifier))
                                    {
                                        resultDataPoint[resultComponents.IndexOf(identifier)] = supersetDatapoint[superset.IndexOfComponent(identifier)];
                                    }

                                    foreach (var subsetIdentifier in subset.DataSetComponents.
                                        Where(id => id.Role == ComponentType.ComponentRole.Identifier && !joinKeys.Contains(id.Name.Substring(id.Name.IndexOf('#') + 1))))
                                    {
                                        resultDataPoint[resultComponents.IndexOf(subsetIdentifier)] = subsetDatapoint[subset.IndexOfComponent(subsetIdentifier)];
                                    }

                                    foreach (var supersetMeasure in superset.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Measure))
                                    {
                                        resultDataPoint[resultComponents.IndexOf(supersetMeasure)] = supersetDatapoint[superset.IndexOfComponent(supersetMeasure)];
                                    }

                                    foreach (var subsetMeasure in subset.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Measure))
                                    {
                                        resultDataPoint[resultComponents.IndexOf(subsetMeasure)] = subsetDatapoint[subset.IndexOfComponent(subsetMeasure)];
                                    }

                                    result.DataPoints.Add(resultDataPoint);
                                }
                            }
                            subsetEnumeratorHasValue = subsetEnumerator.MoveNext();
                            subsetDatapoint = subsetEnumerator.Current;
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

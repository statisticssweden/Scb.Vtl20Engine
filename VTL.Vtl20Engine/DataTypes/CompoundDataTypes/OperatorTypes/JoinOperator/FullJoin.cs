using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.JoinOperator
{
    public class FullJoin : Operator
    {
        private readonly List<Operand> _operands;

        public FullJoin(List<Operand> operands)
        {
            _operands = operands;

            var identifiers = _operands.Select(o => o.GetIdentifierNames()).ToArray();

            for (var i = 1; i < identifiers.Count(); i++)
            {
                if (!identifiers[i].All(x => identifiers[i - 1].Any(y => y == x)))
                {
                    throw new Exception("Alla dataset i full_join måste innehålla samma identifierare");
                }
                if (!identifiers[i - 1].All(x => identifiers[i].Any(y => y == x)))
                {
                    throw new Exception("Alla dataset i full_join måste innehålla samma identifierare");
                }
            }
        }

        internal override DataType PerformCalculation()
        {
            var dsOperands = _operands.AsParallel().Select(o => o.GetValue()).OfType<DataSetType>().ToArray();
            if (dsOperands.Length != _operands.Count)
            {
                throw new Exception("full_join kan endast hantera dataset.");
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
            var joinKeys = ds1.DataSetComponents
                .Where(c => c.Role == ComponentType.ComponentRole.Identifier)
                .Select(c => c.Name.Contains("#") 
                ? c.Name.Substring(c.Name.IndexOf('#') + 1) 
                : c.Name).ToArray();
            

            var resultComponents = ds1.DataSetComponents
                .Where(c => c.Role == ComponentType.ComponentRole.Identifier).ToList();

            resultComponents.AddRange(ds2.DataSetComponents
                .Where(c => c.Role == ComponentType.ComponentRole.Identifier && !joinKeys.Contains(c.Name.Substring(c.Name.IndexOf('#') + 1))));

            resultComponents.AddRange(ds1.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Measure).ToList());
            resultComponents.AddRange(ds2.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Measure).ToList());

            var result = new DataSetType(resultComponents.ToArray());
            if (ds1.DataSetComponents.FirstOrDefault().Name.Contains("#"))
            {
                var dsName = ds1.DataSetComponents.FirstOrDefault().Name.Substring(0, ds1.DataSetComponents.FirstOrDefault().Name.IndexOf('#'));
                ds1.SortDataPoints(joinKeys.Select(i => dsName + "#" + i).ToArray());
            }
            else
            {
                ds1.SortDataPoints(joinKeys);
            }
            if (ds2.DataSetComponents.FirstOrDefault().Name.Contains("#"))
            {
                var dsName = ds2.DataSetComponents.FirstOrDefault().Name.Substring(0, ds2.DataSetComponents.FirstOrDefault().Name.IndexOf('#'));
                ds2.SortDataPoints(joinKeys.Select(i => dsName + "#" + i).ToArray());
            }
            else
            {
                ds2.SortDataPoints(joinKeys);
            }

            var numJoinKeys = joinKeys.Length;
            var dataPointComparer = new DataPointComparer(Enumerable.Range(0, numJoinKeys).ToArray());

            using (var ds1Enumerator = ds1.DataPoints.GetEnumerator())
            using (var ds2Enumerator = ds2.DataPoints.GetEnumerator())
            {
                var ds1EnumeratorHasValue = ds1Enumerator.MoveNext();
                var ds2EnumeratorHasValue = ds2Enumerator.MoveNext();
                while (ds1EnumeratorHasValue || ds2EnumeratorHasValue)
                {
                    var ds1Datapoint = ds1Enumerator.Current;
                    var ds2Datapoint = ds2Enumerator.Current;

                    var resultDataPoint = new DataPointType(resultComponents.Count);
                    var c = dataPointComparer.Compare(ds1Datapoint, ds2Datapoint);
                    if (c == 0)
                    {

                        foreach (var identifier in ds1.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Identifier))
                        {
                            resultDataPoint[resultComponents.IndexOf(identifier)] = ds1Datapoint[ds1.IndexOfComponent(identifier)];
                        }

                        foreach (var d1Measure in ds1.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Measure))
                        {
                            resultDataPoint[resultComponents.IndexOf(d1Measure)] = ds1Datapoint[ds1.IndexOfComponent(d1Measure)];
                        }

                        foreach (var d2Measure in ds2.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Measure))
                        {
                            resultDataPoint[resultComponents.IndexOf(d2Measure)] = ds2Datapoint[ds2.IndexOfComponent(d2Measure)];
                        }

                        result.DataPoints.Add(resultDataPoint);

                        ds1EnumeratorHasValue = ds1Enumerator.MoveNext();
                        ds2EnumeratorHasValue = ds2Enumerator.MoveNext();
                    }
                    else if ((c < 0 && ds1EnumeratorHasValue) || !ds2EnumeratorHasValue) //SuperSet har lägre sortering
                    {
                        foreach (var identifier in ds1.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Identifier))
                        {
                            resultDataPoint[resultComponents.IndexOf(identifier)] = ds1Datapoint[ds1.IndexOfComponent(identifier)];
                        }

                        foreach (var d1Measure in ds1.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Measure))
                        {
                            resultDataPoint[resultComponents.IndexOf(d1Measure)] = ds1Datapoint[ds1.IndexOfComponent(d1Measure)];
                        }
                        foreach (var d2Measure in ds2.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Measure))
                        {
                            resultDataPoint[resultComponents.IndexOf(d2Measure)] = new NullType();
                        }

                        result.DataPoints.Add(resultDataPoint);
                        ds1EnumeratorHasValue = ds1Enumerator.MoveNext();
                    }
                    else if ((c > 0 && ds2EnumeratorHasValue) || !ds1EnumeratorHasValue)
                    {
                        foreach (var identifier in ds1.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Identifier))
                        {
                            resultDataPoint[resultComponents.IndexOf(identifier)] = ds2Datapoint[ds1.IndexOfComponent(identifier)];
                        }

                        foreach (var d1Measure in ds1.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Measure))
                        {
                            resultDataPoint[resultComponents.IndexOf(d1Measure)] = new NullType();
                        }
                        foreach (var d2Measure in ds2.DataSetComponents.Where(id => id.Role == ComponentType.ComponentRole.Measure))
                        {
                            resultDataPoint[resultComponents.IndexOf(d2Measure)] = ds2Datapoint[ds2.IndexOfComponent(d2Measure)];
                        }

                        result.DataPoints.Add(resultDataPoint);
                        ds2EnumeratorHasValue = ds2Enumerator.MoveNext();
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
            var added = new List<string>();
            foreach (var operand in _operands)
            {
                var opNames = operand.GetIdentifierNames();
                result.AddRange(opNames.Except(added).Select(i => operand.Alias + "#" + i));
                added.AddRange(opNames);
            }
            return result.ToArray();
        }

        internal override string[] GetMeasureNames()
        {
            return _operands.SelectMany(o => o.GetMeasureNames().Select(n => o.Alias + "#" + n)).ToArray();
        }

    }
}





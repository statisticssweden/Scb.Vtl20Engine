using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ComparisonOperator
{
    public enum ExistsInRetainType
    {
        All,
        True,
        False
    }

    public class ExistsInOperator : Operator
    {
        private readonly Operand _operand1, _operand2;
        private readonly ExistsInRetainType _retainType;

        public ExistsInOperator(Operand operand1, Operand operand2, ExistsInRetainType retainType)
        {
            _operand1 = operand1;
            _operand2 = operand2;
            _retainType = retainType;

            if (!_operand1.GetIdentifierNames().All(c => _operand2.GetIdentifierNames().Contains(c)))
            {
                throw new Exception("I exists_in måste alla identifiers i det första datasetet återfinnas i det andra datasetet.");
            }
        }

        internal override DataType PerformCalculation()
        {
            var ds1 = _operand1.GetValue() as DataSetType;
            var ds2 = _operand2.GetValue() as DataSetType;
            var ds1Identifiers = ds1.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier);
            if (ds1 == null || ds2 == null)
            {
                throw new Exception("Exists_in kan bara utföras på dataset.");
            }
            ds1.SortDataPoints();
            ds2.SortDataPoints(ds1Identifiers.Select(i => i.Name).ToArray());

            var resultComponents = ds1Identifiers.ToList();
            var resultMeasureComponent = new ComponentType(typeof(BooleanType),
                VtlEngine.DataContainerFactory.CreateComponentContainer(ds1.DataPointCount));
            resultMeasureComponent.Name = "bool_var";
            resultMeasureComponent.Role = ComponentType.ComponentRole.Measure;
            resultComponents.Add(resultMeasureComponent);
            var result = new DataSetType(resultComponents.ToArray());

            var comparer = new IdentifierComparer(ds1, ds2);
            var nIdentifiers = ds1Identifiers.Count();

            using (var ds1Enumerator = ds1.DataPoints.GetEnumerator())
            using (var ds2Enumerator = ds2.DataPoints.GetEnumerator())
            {
                var ds1EnumeratorHasValue = ds1Enumerator.MoveNext();
                var ds2EnumeratorHasValue = ds2Enumerator.MoveNext();

                while (ds1EnumeratorHasValue)
                {
                    var ds1datapoint =
                        ds1EnumeratorHasValue ? ds1Enumerator.Current : null;
                    var ds2datapoint =
                        ds2EnumeratorHasValue ? ds2Enumerator.Current : null;

                    if (!ds2EnumeratorHasValue)
                    {
                        if (_retainType == ExistsInRetainType.All || _retainType == ExistsInRetainType.False)
                        {
                            result.Add(MakeDataPoint(ds1datapoint, nIdentifiers, false));
                        }

                        ds1EnumeratorHasValue = ds1Enumerator.MoveNext();
                    }
                    else
                    {
                        var c = comparer.Compare(ds1Enumerator.Current, ds2Enumerator.Current);
                        if (c == 0)
                        {
                            if (_retainType == ExistsInRetainType.All || _retainType == ExistsInRetainType.True)
                            {
                                result.Add(MakeDataPoint(ds1datapoint, nIdentifiers, true));
                            }

                            ds1EnumeratorHasValue = ds1Enumerator.MoveNext();
                            ds2EnumeratorHasValue = ds2Enumerator.MoveNext();
                        }
                        else if (c < 0)
                        {
                            if (_retainType == ExistsInRetainType.All || _retainType == ExistsInRetainType.False)
                            {
                                result.Add(MakeDataPoint(ds1datapoint, nIdentifiers, false));
                            }

                            ds1EnumeratorHasValue = ds1Enumerator.MoveNext();
                        }
                        else if (c > 0)
                        {
                            ds2EnumeratorHasValue = ds2Enumerator.MoveNext();
                        }
                    }
                }
            }
            return result;
        }

        private DataPointType MakeDataPoint(DataPointType original, int nIdentifiers, bool val)
        {
            var dataPoint = new DataPointType(nIdentifiers + 1);
            for (int i = 0; i < nIdentifiers; i++)
            {
                dataPoint[i] = original[i];
            }
            dataPoint[nIdentifiers] = new BooleanType(val);
            return dataPoint;
        }

        internal override string[] GetComponentNames()
        {
            var componentNames = GetIdentifierNames().ToList();
            componentNames.Add("bool_var");
            return componentNames.ToArray();
        }

        internal override string[] GetIdentifierNames()
        {
            return _operand1.GetIdentifierNames();
        }

        internal override string[] GetMeasureNames()
        {
            return new[] { "bool_var" };
        }

    }
}

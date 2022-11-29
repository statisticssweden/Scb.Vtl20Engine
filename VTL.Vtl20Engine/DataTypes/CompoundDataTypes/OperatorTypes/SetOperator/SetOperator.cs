using System;
using System.Collections.Generic;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using System.Linq;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.SetOperator
{
    public abstract class SetOperator : Operator
    {
        private List<Operand> _operands;
        public List<Operand> Operands
        {
            get
            {
                return _operands;
            }
            set
            {
                _operands = value;

                if (Operands.Any(o => o.GetComponentNames().Length <2))
                {
                  throw new Exception($"Alla argument till {GetType().Name} måste vara datset.");
                }

                var ds1Names = Operands.FirstOrDefault()?.GetComponentNames();
                for (int i = 1; i < Operands.Count(); i++)
                {
                    var d2Names = Operands[i].GetComponentNames();
                    if (ds1Names.Length != d2Names.Length || !ds1Names.All(c => d2Names.Contains(c)))
                    {
                        throw new Exception( $"Alla dataset som ingår i {GetType().Name} måste ha samma datsetkomponenter.");
                    }
                }
            }
        }


        internal override DataType PerformCalculation()
        {
            if (Operands.FirstOrDefault()?.GetValue() is DataSetType ds1)
            {
                var numIdentifiers = ds1.DataSetComponents.Count(c => c.Role == ComponentType.ComponentRole.Identifier);
                var dataPointComparer = new DataPointComparer(Enumerable.Range(0, numIdentifiers).ToArray());

                for (int i = 1; i < Operands.Count(); i++)
                {
                    var preResult = new DataSetType(ds1.DataSetComponents);
                    if (Operands[i].GetValue() is DataSetType ds2)
                    {
                        if (ds1.DataSetComponents.Length != ds2.DataSetComponents.Length ||
                            !ds1.DataSetComponents.All(c => ds2.DataSetComponents.Contains(c)))
                        {
                            throw new Exception(
                                $"Alla dataset som ingår i {GetType().Name} måste ha samma datsetkomponenter.");
                        }

                        for (int c = 0; c < ds1.DataSetComponents.Length; c++)
                        {
                            if(ds1.DataSetComponents[c].DataType == typeof(IntegerType) &&
                                ds2.DataSetComponents[c].DataType == typeof(NumberType))
                            {
                                preResult.DataSetComponents[c].DataType = typeof(NumberType);
                            }
                        }

                        ds2.SortDataPoints(ds1.ComponentSortOrder.
                        Select(x => new OrderByName() { ComponentName = x }).ToArray());

                        using (var ds1Enumerator = ds1.DataPoints.GetEnumerator())
                        using (var ds2Enumerator = ds2.DataPoints.GetEnumerator())
                        {

                            var ds1EnumeratorHasValue = ds1Enumerator.MoveNext();
                            var ds2EnumeratorHasValue = ds2Enumerator.MoveNext();
                            while (ds1EnumeratorHasValue || ds2EnumeratorHasValue)
                            {
                                var ds1datapoint =
                                    ds1EnumeratorHasValue ? ds1Enumerator.Current as DataPointType : null;
                                var ds2datapoint =
                                    ds2EnumeratorHasValue ? ds2Enumerator.Current as DataPointType : null;
                                var obj = PerformCalculation(ds1datapoint, ds2datapoint, dataPointComparer);
                                if (obj != null)
                                {
                                    preResult.Add(obj);
                                }

                                //stega upp enumerators
                                if (!ds1EnumeratorHasValue)
                                {
                                    ds2EnumeratorHasValue = ds2Enumerator.MoveNext();
                                }
                                else if (!ds2EnumeratorHasValue)
                                {
                                    ds1EnumeratorHasValue = ds1Enumerator.MoveNext();
                                }
                                else
                                {
                                    var c = dataPointComparer.Compare(ds1datapoint, ds2datapoint);
                                    if (c == 0)
                                    {
                                        ds1EnumeratorHasValue = ds1Enumerator.MoveNext();
                                        ds2EnumeratorHasValue = ds2Enumerator.MoveNext();
                                    }
                                    else if (c < 0)
                                    {
                                        ds1EnumeratorHasValue = ds1Enumerator.MoveNext();
                                    }
                                    else if (c > 0)
                                    {
                                        ds2EnumeratorHasValue = ds2Enumerator.MoveNext();
                                    }
                                }
                            }
                            ds1 = preResult;
                        }
                    }
                    else
                    {
                        throw new Exception($"Alla argument till {GetType().Name} måste vara datset.");
                    }
                }
                return ds1;
            }
            else
            {
                throw new Exception($"Alla argument till {GetType().Name} måste vara datset.");
            }
        }

        internal abstract DataPointType PerformCalculation(DataPointType dp1, DataPointType dp2, DataPointComparer dataPointComparer);

        internal override string[] GetIdentifierNames()
        {
            return Operands.FirstOrDefault().GetIdentifierNames();
        }

        internal override string[] GetMeasureNames()
        {
            return Operands.FirstOrDefault().GetMeasureNames();
        }

        internal override string[] GetComponentNames()
        {
            return Operands.FirstOrDefault().GetComponentNames();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.DataValidationOperator
{
    public class CheckOperator : Operator
    {
        private Operand _op;
        private Operand _errorcode;
        private Operand _errorlevel;
        private Operand _imbalance;
        private bool _invalid;

        public CheckOperator(Operand op, Operand errorcode, Operand errorlevel, Operand imbalance, bool invalid)
        {
            _op = op;
            _errorcode = errorcode;
            _errorlevel = errorlevel;
            _imbalance = imbalance;
            _invalid = invalid;

            if(_op.GetMeasureNames().Length != 1)
            {
                throw new Exception("Operanden måste ha exakt en measurekomponent.");
            }

            if(_imbalance != null)
            {
                if (_imbalance.GetMeasureNames().Length != 1)
                {
                    throw new Exception("Imbalance måste ha exakt en measurekomponent.");
                }

                var opIdentifiers = _op.GetIdentifierNames();
                var imbalanceIdentifiers = _imbalance.GetIdentifierNames();
                if(opIdentifiers.Any(o => !imbalanceIdentifiers.Contains(o) || 
                imbalanceIdentifiers.Any(i => !opIdentifiers.Contains(o))))
                {
                    throw new Exception("Operand och imbalance måste ha samma identifiers.");
                }
            }
        }

        internal override DataType PerformCalculation()
        {
            var opDs = _op.GetValue() as DataSetType;
            var imbalanceDs = _imbalance?.GetValue() as DataSetType;
            var errorCode = _errorcode?.GetValue() as ScalarType;
            var errorLevel = _errorlevel?.GetValue() as ScalarType;

            var resultComponents = opDs.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier).ToList();
            resultComponents.AddRange(new List<ComponentType>{
                new ComponentType(typeof(BooleanType), new SimpleComponentContainer())
                {
                    Name = "bool_var", Role = ComponentType.ComponentRole.Measure
                },
                new ComponentType(typeof(NumberType), new SimpleComponentContainer())
                {
                    Name = "imbalance", Role = ComponentType.ComponentRole.Measure
                },
                new ComponentType(errorCode?.GetType() == typeof(IntegerType) ? typeof(IntegerType) : typeof(StringType), new SimpleComponentContainer())
                {
                    Name = "errorcode", Role = ComponentType.ComponentRole.Measure
                },
                new ComponentType(errorLevel?.GetType() == typeof(IntegerType) ? typeof(IntegerType) : typeof(StringType), new SimpleComponentContainer())
                {
                    Name = "errorlevel", Role = ComponentType.ComponentRole.Measure
                }
            });
            var result = new DataSetType(resultComponents.ToArray());
            var numComponents = result.DataSetComponents.Length;
            var dataPointComparer = new DataPointComparer(Enumerable.Range(0, result.DataSetComponents.
                Count(c => c.Role == ComponentType.ComponentRole.Identifier)).ToArray());
            var measureIndex = opDs.IndexOfComponent(opDs.DataSetComponents.Single(c => c.Role == ComponentType.ComponentRole.Measure));

            using (var opEnumerator = opDs.DataPoints.GetEnumerator())
            using (var imbalanceEnumerator = imbalanceDs?.DataPoints.GetEnumerator())
            {
                opEnumerator.MoveNext();
                imbalanceEnumerator?.MoveNext();
                do
                {
                    if(imbalanceEnumerator != null && dataPointComparer.Compare(opEnumerator.Current, imbalanceEnumerator.Current) != 0)
                    {
                        throw new Exception("Bara matchande datapunkter för operand och imbalance får förekomma i check.");
                    }
                    var condition = opEnumerator.Current[measureIndex].HasValue() && (bool)(opEnumerator.Current[measureIndex] as BooleanType);
                    if (_invalid && condition)
                    {
                        continue;
                    }
                    var resultDatapoint = new DataPointType(numComponents);
                    int i;
                    for (i = 0; i < opEnumerator.Current.Count(); i++)
                    {
                        resultDatapoint[i] = opEnumerator.Current[i];
                    }

                    if (imbalanceEnumerator != null)
                    {
                        resultDatapoint[i] = imbalanceEnumerator.Current.Last();
                    }
                    else
                    {
                        resultDatapoint[i] = new NumberType(null);
                    }

                    i++;
                    if (errorCode != null && !condition)
                    {
                        resultDatapoint[i] = errorCode;
                    }
                    else
                    {
                        resultDatapoint[i] = new StringType(null);
                    }

                    i++;
                    if (errorLevel != null && !condition)
                    {
                        resultDatapoint[i] = errorLevel;
                    }
                    else
                    {
                        resultDatapoint[i] = new StringType(null);
                    }

                    result.DataPoints.Add(resultDatapoint);

                } while (opEnumerator.MoveNext() && (imbalanceEnumerator == null || imbalanceEnumerator.MoveNext()));
            }
            return result;
        }

        internal override string[] GetComponentNames()
        {
            var id = GetIdentifierNames();
            var me = GetMeasureNames();
            var r = new string[id.Length + me.Length];
            id.CopyTo(r, 0);
            me.CopyTo(r, id.Length);
            return r;
        }

        internal override string[] GetIdentifierNames()
        {
            return _op.GetIdentifierNames();
        }

        internal override string[] GetMeasureNames()
        {
            return new string[] { "bool_var", "imbalance", "errorcode", "errorlevel" };
        }
    }
}

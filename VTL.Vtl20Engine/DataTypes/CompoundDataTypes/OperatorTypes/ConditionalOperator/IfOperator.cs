using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Extensions;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ConditionalOperator
{
    public class IfOperator : Operator
    {
        private readonly Operand _condition, _thenOperand, _elseOperand;

        public IfOperator(Operand condition, Operand thenOperand, Operand elseOperand)
        {
            _condition = condition;
            _thenOperand = thenOperand;
            _elseOperand = elseOperand;
        }

        internal override DataType PerformCalculation()
        {
            DataType conditionValue = null, thenValue = null, elseValue = null;

            Parallel.Invoke(
                () => conditionValue = _condition.GetValue(),
                () => thenValue = _thenOperand.GetValue(),
                () => elseValue = _elseOperand.GetValue());

            if (conditionValue is BooleanType booleanCondition)
            {
                return PerformCalculation(booleanCondition, thenValue, elseValue);
            }

            if (conditionValue is ComponentType componentCondition)
            {
                return PerformCalculation(componentCondition, thenValue, elseValue);
            }

            if (conditionValue is DataSetType dataSetCondition)
            {
                return PerformCalculation(dataSetCondition, thenValue, elseValue);
            }

            throw new NotImplementedException();
        }

        internal DataType PerformCalculation(BooleanType booleanCondition, DataType thenValue, DataType elseValue)
        {
            if (booleanCondition == true)
            {
                return thenValue;
            }
            else
            {
                return elseValue;
            }
        }

        private DataType PerformCalculation(ComponentType conditionComponent, DataType thenValue, DataType elseValue)
        {
            var componentType = thenValue is ComponentType ? thenValue as ComponentType : elseValue as ComponentType;
            if (componentType == null)
            {
                throw new Exception("Ett if-uttryck med komponentvillkor måste ha minst en komponent som argument.");
            }
            
            var result = new ComponentType(componentType);
            result.ComponentDataHandler = VtlEngine.DataContainerFactory.CreateComponentContainer(conditionComponent.Length);

            var thenEnumerator = thenValue is ComponentType thenComponent ? thenComponent.GetEnumerator() : null;
            var thenScalar = thenValue as ScalarType;
            var elseEnumerator = elseValue is ComponentType elseComponent ? elseComponent.GetEnumerator() : null;
            var elseScalar = elseValue as ScalarType;

            using (var conditionEnumerator = conditionComponent.GetEnumerator())
            {
                while (conditionEnumerator.MoveNext())
                {
                    thenEnumerator?.MoveNext();
                    elseEnumerator?.MoveNext();

                    if (((BooleanType) conditionEnumerator.Current).Equals(new BooleanType(true)))
                    {
                        if (thenEnumerator != null)
                        {
                            result.Add(thenEnumerator.Current);
                        }
                        else if (thenScalar != null)
                        {
                            result.Add(thenScalar);
                        }
                    }
                    else
                    {
                        if (elseEnumerator != null)
                        {
                            result.Add(elseEnumerator.Current);
                        }
                        else if (elseValue != null)
                        {
                            result.Add(elseScalar);
                        }
                    }
                }
            }

            return result;
        }

        private ComponentType[] _resultComponents;
        private int _condIndex;

        private DataType PerformCalculation(DataSetType conditionDataSet, DataType thenValue, DataType elseValue)
        {
            var thenDataSet = thenValue as DataSetType;
            var elseDataSet = elseValue as DataSetType;
            var thenScalar = thenValue as ScalarType;
            var elseScalar = elseValue as ScalarType;

            if (conditionDataSet.DataSetComponents.Count(
                    c => c.Role == ComponentType.ComponentRole.Measure) != 1 ||
                conditionDataSet.DataSetComponents.First(
                    c => c.Role == ComponentType.ComponentRole.Measure).DataType != typeof(BooleanType))
            {
                throw new Exception(
                    "Vilkorsdatasetet måste innehålla endast en measurekomponent och den ska vara av datatypen boolean.");
            }

            if (thenDataSet == null && elseDataSet == null)
            {
                throw new Exception("Ett if-uttryck med datasetvillkor måste ha minst ett dataset som argument.");
            }

            if (thenDataSet != null)
            {
                var conditionIdentifiers = conditionDataSet.DataSetComponents
                    .Where(c => c.Role == ComponentType.ComponentRole.Identifier).ToArray();
                var thenIdentifiers = thenDataSet.DataSetComponents
                    .Where(c => c.Role == ComponentType.ComponentRole.Identifier).ToArray();
                if (conditionIdentifiers.Length != thenIdentifiers.Length ||
                    !conditionIdentifiers.All(c => thenIdentifiers.Contains(c)))
                {
                    throw new Exception(
                        $"Strukturen för två ingående dataset var inte kompatibla. \r\n" +
                        $"Vilkorsdatasetet och then-datasetet måste innehålla samma identifier-komponenter.");
                }

            }

            if (elseDataSet != null)
            {
                var conditionIdentifiers = conditionDataSet.DataSetComponents
                    .Where(c => c.Role == ComponentType.ComponentRole.Identifier).ToArray();
                var elseIdentifiers = elseDataSet.DataSetComponents
                    .Where(c => c.Role == ComponentType.ComponentRole.Identifier).ToArray();
                if (conditionIdentifiers.Length != elseIdentifiers.Length ||
                    !conditionIdentifiers.All(c => elseIdentifiers.Contains(c)))
                {
                    throw new Exception(
                        $"Strukturen för två ingående dataset var inte kompatibla. \r\n" +
                        $"Vilkorsdatasetet och else-datasetet måste innehålla samma identifier-komponenter.");
                }

            }

            if (thenDataSet != null && elseDataSet != null)
            {
                if (thenDataSet.DataSetComponents.Length != elseDataSet.DataSetComponents.Length ||
                    !thenDataSet.DataSetComponents.All(c => elseDataSet.DataSetComponents.Contains(c)))
                {
                    throw new Exception(
                        $"Strukturen för två ingående dataset var inte kompatibla. \r\n" +
                        $"Then-datasetet och else-datasetet måste sa samma struktur.");
                }
            }

            if (thenDataSet != null && elseScalar != null &&
                thenDataSet.DataSetComponents.Any(c => c.Role == ComponentType.ComponentRole.Measure && 
                                                       !c.IsAssignableFrom(elseScalar.GetType())))
            {
                throw new Exception(
                    "Alla measure-komponenter i then-datasetet måste ha samma datatyp som det konstanta else-värdet.");
            }

            if (elseDataSet != null && thenScalar != null &&
                elseDataSet.DataSetComponents.Any(c => c.Role == ComponentType.ComponentRole.Measure &&
                                                       !c.IsAssignableFrom(thenScalar.GetType())))
            {
                throw new Exception(
                    "Alla measure-komponenter i else-datasetet måste ha samma datatyp som det konstanta then-värdet.");
            }
            
            _resultComponents = thenDataSet != null ? thenDataSet.DataSetComponents : elseDataSet.DataSetComponents;
            _condIndex = conditionDataSet.IndexOfComponent(
                conditionDataSet.DataSetComponents.FirstOrDefault(c => c.Role == ComponentType.ComponentRole.Measure));

            DataSetType thenResult = null;
            DataSetType elseResult = null;

            if (thenDataSet != null)
            {
                thenResult = thenDataSet.PerformJoinCalculation(conditionDataSet, ThenCalculation);
            }

            if (thenScalar != null)
            {
                thenResult = new DataSetType(elseDataSet.DataSetComponents);
                foreach (var dataPoint in DataPoints(conditionDataSet, thenScalar, true))
                {
                    thenResult.Add(dataPoint);
                }
            }

            if (elseDataSet != null)
            {
                elseResult = elseDataSet.PerformJoinCalculation(conditionDataSet, ElseCalculation);
            }

            if (elseScalar != null)
            {
                elseResult = new DataSetType(thenDataSet.DataSetComponents);
                foreach (var dataPoint in DataPoints(conditionDataSet, elseScalar, false))
                {
                    elseResult.Add(dataPoint);
                }
            }

            var result = thenResult.Union(elseResult);
            return result;
        }

        private IEnumerable<DataPointType> DataPoints(DataSetType conditionDataSet, ScalarType scalar, bool then)
        {
            foreach (var conditionDataPoint in conditionDataSet.DataPoints)
            {
                if (conditionDataPoint[_condIndex] is BooleanType cond)
                {
                    if (cond == then)
                    {
                        var dataPoint = new DataPointType(_resultComponents.Length);
                        dataPoint = dataPoint.Transform(conditionDataSet.DataSetComponents, _resultComponents);
                        for (int j = 0; j < _resultComponents.Length; j++)
                        {
                            if (_resultComponents[j].Role == ComponentType.ComponentRole.Measure)
                            {
                                dataPoint[j] = scalar;
                            }

                            if (_resultComponents[j].Role == ComponentType.ComponentRole.Identifier)
                            {
                                dataPoint[j] = conditionDataPoint[j];
                            }
                        }

                        yield return dataPoint;
                    }
                }
            }
        }

        internal DataPointType ThenCalculation(DataPointType val, DataPointType cond)
        {
            if (cond[_condIndex] is BooleanType c && c == true)
            {
                return new DataPointType(val);
            }
            else
            {
                return null;
            }
        }

        internal DataPointType ElseCalculation(DataPointType val, DataPointType cond)
        {
            if (cond[_condIndex] is BooleanType c && c == false)
            {
                return new DataPointType(val);
            }
            else
            {
                return null;
            }
        }

        internal override string[] GetComponentNames()
        {
            if (_thenOperand.GetComponentNames().Length > 1)
            {
                return _thenOperand.GetComponentNames();
            }

            return _elseOperand.GetComponentNames();
        }

        internal override string[] GetIdentifierNames()
        {
            if (_thenOperand.GetIdentifierNames().Length > 1)
            {
                return _thenOperand.GetIdentifierNames();
            }

            return _elseOperand.GetIdentifierNames();
        }

        internal override string[] GetMeasureNames()
        {
            if (_thenOperand.GetMeasureNames().Length > 1)
            {
                return _thenOperand.GetMeasureNames();
            }

            return _elseOperand.GetMeasureNames();
        }
    }
}
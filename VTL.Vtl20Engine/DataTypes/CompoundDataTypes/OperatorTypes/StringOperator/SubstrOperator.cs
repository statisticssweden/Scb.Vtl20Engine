using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataContainers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using static VTL.Vtl20Engine.DataTypes.CompoundDataTypes.ComponentType;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.StringOperator
{
    public class Substr : Operator
    {

        private readonly List<Operand> _arguments;

        private Operand _operand;

        public Substr(Operand operand, IEnumerable<Operand> arguments)
        {
            _operand = operand;
            _arguments = arguments.ToList();

        }

        internal override DataType PerformCalculation()
        {
            ValidateArguments(_arguments);
            var operandData = _operand.GetValue();
            var startpos = _arguments[0]?.GetValue();
            var stringLength = _arguments.Count > 1 ? _arguments[1]?.GetValue() : null;

            if (operandData is DataSetType dataSet)
            {
                return PerformCalculation(dataSet, startpos, stringLength);
            }

            if (operandData is ComponentType component)
            {
                return PerformCalculation(component, startpos, stringLength);
            }

            if (operandData is ScalarType scalar)
            {
                var startInt = startpos as IntegerType;
                var lengthInt = stringLength as IntegerType;
                return PerformCalculation(scalar, startInt, lengthInt);
            }

            throw new Exception("Substr är inte implemenenterad för denna datatyp");

        }

        private void ValidateArguments(List<Operand> substrArguments)
        {
            if (substrArguments.Count <= 0) throw new Exception("Du måste ha minst två parametrar till funktionen substring.");
            if (substrArguments.Count > 2) throw new Exception("Du får inte ha mer än tre parametrar till funktionen substring.");

            var startArg = substrArguments[0]?.GetValue();
            if (startArg is DataSetType dsStart)
            {
                try
                {
                    var components = 
                    dsStart.DataSetComponents.SingleOrDefault(ds => ds.Role == ComponentRole.Measure && ds.DataType == typeof(IntegerType));

                    if (components == null) throw new Exception();
                    foreach (var componentValue in components)
                    {
                        var componentValueAsInt = componentValue as IntegerType;
                        if (componentValueAsInt < 0)
                        {
                            throw new Exception();
                        }
                    }
                }
                catch
                {
                    throw new Exception("Startpositionen måste vara ett heltal större än 0.");
                }
            }
            else if(startArg is IntegerType startindex)
            {
                if (startindex == null || startindex < 1)
                {
                    throw new Exception("Startpositionen måste vara ett heltal större än 0.");
                }
            }

            if (substrArguments.Count == 2)
            {
                var lengthArg = substrArguments[1]?.GetValue();
                if (lengthArg is DataSetType dsLength)
                {
                    var component = dsLength.DataSetComponents.FirstOrDefault(ds =>
                        ds.Role == ComponentRole.Measure && ds.DataType == typeof(IntegerType));
                    if (component == null)
                    {
                        throw new Exception("Om du anger längd måste den vara ett heltal större än 0.");
                    }
                }
                else if(lengthArg is IntegerType lenght)
                {
                    if (lenght == null || lenght < 1)
                    {
                        throw new Exception("Om du anger längd måste den vara ett heltal större än 0.");
                    }
                }
            }
        }

        public DataSetType PerformCalculation(DataSetType dataSet, DataType startIndex, DataType length)
        {
            var result = new DataSetType(dataSet.DataSetComponents);

            var startIndexDataSet = startIndex as DataSetType;
            var startIndexEnumerator = startIndexDataSet?.GetDataPointEnumerator();
            var startIndexComponent = startIndexDataSet?.DataSetComponents.FirstOrDefault(ds =>
                ds.Role == ComponentRole.Measure && ds.DataType == typeof(IntegerType));
            var startIndexComponentIndex = startIndexDataSet?.IndexOfComponent(startIndexComponent);

            var lengthDataSet = length as DataSetType;
            var lengthEnumerator = lengthDataSet?.GetDataPointEnumerator();
            var lengthComppnent = lengthDataSet?.DataSetComponents.FirstOrDefault(ds =>
                ds.Role == ComponentRole.Measure && ds.DataType == typeof(IntegerType));
            var lengthComponentIndex = lengthDataSet?.IndexOfComponent(lengthComppnent);

            foreach (var operandDataPoint in dataSet.DataPoints)
            {
                startIndexEnumerator?.MoveNext();
                lengthEnumerator?.MoveNext();
                var resultDataPoint = new DataPointType(dataSet.DataSetComponents.Length);
                for (int j = 0; j < dataSet.DataSetComponents.Length; j++)
                {
                    if (dataSet.DataSetComponents[j].Role == ComponentType.ComponentRole.Identifier ||
                        dataSet.DataSetComponents[j].Role == ComponentType.ComponentRole.Attribure)
                        resultDataPoint[j] = operandDataPoint[j];
                    if (dataSet.DataSetComponents[j].Role == ComponentType.ComponentRole.Measure)
                    {
                        var scalarStart = startIndexEnumerator != null ?
                            startIndexEnumerator.Current[startIndexComponentIndex.Value] :
                            startIndex as ScalarType;
                        
                        var scalarLength = lengthEnumerator != null ?
                            lengthEnumerator.Current[lengthComponentIndex.Value] :
                            length as ScalarType;

                        resultDataPoint[j] = PerformCalculation(operandDataPoint[j],
                            scalarStart as IntegerType, scalarLength as IntegerType);
                    }
                }

                result.Add(resultDataPoint);
            }

            return result;
        }

        public ComponentType PerformCalculation(ComponentType component, DataType startIndex, DataType length)
        {
            var resultComponent = new ComponentType(typeof(StringType), VtlEngine.DataContainerFactory.CreateComponentContainer(component.Length))
            {
                Role = component.Role
            };

            var startIndexEnumerator = startIndex is ComponentType compStart ? compStart.GetEnumerator() : null;
            var lengthEnumerator = length is ComponentType compLength ? compLength.GetEnumerator() : null;

            foreach (var scalar in component)
            {
                startIndexEnumerator?.MoveNext();
                var scalarStart = startIndexEnumerator?.Current ?? startIndex as ScalarType;
                lengthEnumerator?.MoveNext();
                var scalarLength = lengthEnumerator?.Current ?? length as ScalarType;

                resultComponent.Add(PerformCalculation(scalar,
                    scalarStart as IntegerType, scalarLength as IntegerType));
            }

            return resultComponent;
        }

        public ScalarType PerformCalculation(ScalarType scalar, IntegerType startIndex, IntegerType length)
        {
            if (!scalar.HasValue()) return scalar;
            if(startIndex == null) startIndex = new IntegerType(1);
            if (scalar is StringType stringScalar)
            {
                if (length != null)
                {
                    int corrLength = Math.Min(stringScalar.ToString().Length - (int)startIndex + 1, (int)length);
                    if(corrLength<1) return new StringType("");
                    return new StringType(stringScalar.ToString().Substring((int)startIndex - 1, corrLength));
                }
                if (startIndex > stringScalar.ToString().Length) return new StringType("");
                return new StringType(stringScalar.ToString().Substring((int)startIndex - 1));
            }
            throw new Exception("Substr kan endast användas på värden av typen sträng.");
        }


        internal override string[] GetIdentifierNames()
        {
            return _operand.GetIdentifierNames();
        }

        internal override string[] GetMeasureNames()
        {
            return _operand.GetMeasureNames();
        }

        internal override string[] GetComponentNames()
        {
            return _operand.GetComponentNames();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using static VTL.Vtl20Engine.DataTypes.CompoundDataTypes.ComponentType;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.StringOperator
{
    public class Instr : Operator
    {
        private readonly List<Operand> _arguments;

        private Operand _operand;

        public Instr(Operand operand, IEnumerable<Operand> arguments)
        {
            _operand = operand;
            if (operand.GetMeasureNames().Length > 1)
                throw new Exception("Operatorn instr kan inte användas på datasetnivå om datasetet har mer än en measurekomponent.");
            _arguments = arguments.ToList();
        }

        internal override DataType PerformCalculation()
        {
            ValidateArguments(_arguments);
            var operandData = _operand.GetValue();
            var pattern = _arguments[0]?.GetValue();

            var startpos = _arguments.Count > 1 ? _arguments[1]?.GetValue() : null;
            var occurences = _arguments.Count > 2 ? _arguments[2]?.GetValue() : null;
            if (operandData is DataSetType dataSet)
            {
                return PerformCalculation(dataSet, pattern, startpos, occurences);
            }

            if (operandData is ComponentType component)
            {
                return PerformCalculation(component, pattern, startpos, occurences);
            }

            if (operandData is StringType scalar)
            {
                var scalarString = operandData as StringType;
                var patternString = pattern as StringType;
                var startInt = startpos as IntegerType;
                var occurencesint = occurences as IntegerType;

                return PerformCalculation(scalarString, patternString, startInt, occurencesint);
            }
            throw new Exception("Instr är inte implemenenterad för denna datatyp.");
        }

        private void ValidateArguments(List<Operand> instrArguments)
        {
            if (instrArguments.Count <= 0) throw new Exception("Du måste ha minst två parametrar till funktionen instr.");
            if (instrArguments.Count > 3) throw new Exception("Du får inte ha mer än fyra parametrar till funktionen instr.");

            var patternArg = instrArguments[0]?.GetValue();
            if (patternArg is ComponentType patterncomp)
            {
                if (!(patterncomp.DataType == typeof(StringType)))
                { 
                    throw new Exception("Andra parametern i instr måste vara ett strängvärde.");
                }
            }
            else if (!(patternArg is StringType))
            {
                throw new Exception("Andra parametern i instr måste vara ett strängvärde.");
            }

            if (instrArguments.Count >1)
            {
                var startarg = instrArguments[1]?.GetValue();
                if (startarg is ComponentType startcomp)
                {
                    if (!(startcomp.DataType == typeof(IntegerType)))
                    {
                        throw new Exception("Tredje parametern i instr måste vara ett positivt heltal.");
                    }
                }
                else if (startarg is IntegerType startValue)
                {
                    if (startValue == null || startValue < 1)
                    {
                        throw new Exception("Tredje parametern i instr måste vara ett positivt heltal.");
                    }
                }
                else
                {
                    throw new Exception("Tredje parametern i instr måste vara ett positivt heltal.");
                }
            }

            if (instrArguments.Count == 3)
            {
                var occurencearg = instrArguments[2]?.GetValue();
                if (occurencearg is ComponentType occurencecomp)
                {
                    if (!(occurencecomp.DataType == typeof(IntegerType)))
                    {
                        throw new Exception("Fjärde parametern i instr måste vara ett positivt heltal.");
                    }
                }
                else if (occurencearg is IntegerType occurenceValue)
                {
                    if (occurenceValue == null || occurenceValue < 1)
                    {
                        throw new Exception("Fjärde parametern i instr måste vara ett positivt heltal.");
                    }
                }
                else
                {
                    throw new Exception("Fjärde parametern i instr måste vara ett positivt heltal.");
                }
            }
        }

        public DataSetType PerformCalculation(DataSetType dataSet, DataType pattern, DataType startpos, DataType occurences)
        {
            var dsMeasure = dataSet.DataSetComponents.FirstOrDefault(c => c.Role == ComponentType.ComponentRole.Measure);
            if (dsMeasure.DataType != typeof(StringType))
            {
                throw new Exception($"Värdekomponenten {dsMeasure.Name} har datatypen {dsMeasure.DataType.Name} vilken inte är tillåten för operatorn {GetType().Name}.");
            }
            var newcomp = new ComponentType(dsMeasure)
            {
                Name = "int_var",
                DataType = typeof(IntegerType),
                Role = ComponentType.ComponentRole.Measure
            };
            var resultcomponents = dataSet.DataSetComponents.Where(c => c.Role != ComponentType.ComponentRole.Measure).ToList();
            resultcomponents.Add(newcomp);
            var result = new DataSetType(resultcomponents.ToArray());

            var patternDataSet = pattern as DataSetType;
            var patternEnumerator = patternDataSet?.GetDataPointEnumerator();
            var patternComponent = patternDataSet?.DataSetComponents.FirstOrDefault(ds =>
                ds.Role == ComponentRole.Measure && ds.DataType == typeof(IntegerType));
            var patternComponentIndex = patternDataSet?.IndexOfComponent(patternComponent);

            var startDataSet = startpos as DataSetType;
            var startEnumerator = startDataSet?.GetDataPointEnumerator();
            var startComponent = startDataSet?.DataSetComponents.FirstOrDefault(ds =>
                ds.Role == ComponentRole.Measure && ds.DataType == typeof(IntegerType));
            var startComponentIndex = startDataSet?.IndexOfComponent(startComponent);

            var occurenceDataSet = occurences as DataSetType;
            var occurenceEnumerator = occurenceDataSet?.GetDataPointEnumerator();
            var occurenceComponent = occurenceDataSet?.DataSetComponents.FirstOrDefault(ds =>
                ds.Role == ComponentRole.Measure && ds.DataType == typeof(IntegerType));
            var occurenceComponentIndex = occurenceDataSet?.IndexOfComponent(occurenceComponent);

            foreach (var operandDataPoint in dataSet.DataPoints)
            {
                patternEnumerator?.MoveNext();
                startEnumerator?.MoveNext();
                var resultDataPoint = new DataPointType(dataSet.DataSetComponents.Length);
                for (int j = 0; j < dataSet.DataSetComponents.Length; j++)
                {
                    if (dataSet.DataSetComponents[j].Role == ComponentType.ComponentRole.Identifier ||
                        dataSet.DataSetComponents[j].Role == ComponentType.ComponentRole.Attribure)
                        resultDataPoint[j] = operandDataPoint[j];
                    if (dataSet.DataSetComponents[j].Role == ComponentType.ComponentRole.Measure)
                    {
                        var scalarPattern = patternEnumerator != null ?
                            patternEnumerator.Current[patternComponentIndex.Value] :
                            pattern as ScalarType;

                        var scalarStart = startEnumerator != null ?
                            startEnumerator.Current[startComponentIndex.Value] :
                            startpos as ScalarType;

                        var scalarOccurences = occurenceEnumerator != null ?
                            occurenceEnumerator.Current[occurenceComponentIndex.Value] :
                            occurences as ScalarType;

                        resultDataPoint[j] = PerformCalculation(operandDataPoint[j] as StringType,
                           pattern as StringType,  scalarStart as IntegerType, scalarOccurences as IntegerType);
                    }
                }

                result.Add(resultDataPoint);
            }

            return result;
        }

        public ComponentType PerformCalculation(ComponentType component, DataType pattern, DataType startpos, DataType occurences)
        {
            var resultComponent = new ComponentType(typeof(IntegerType), VtlEngine.DataContainerFactory.CreateComponentContainer(component.Length))
            {
                Role = component.Role
            };

            var startIndexEnumerator = startpos is ComponentType compStart ? compStart.GetEnumerator() : null;
            var occurencesEnumerator = occurences is ComponentType compOccurences ? compOccurences.GetEnumerator() : null;
            var patternEnumerator = pattern is ComponentType compPattern ? compPattern.GetEnumerator() : null;

            foreach (var scalar in component)
            {
                startIndexEnumerator?.MoveNext();
                var scalarStart = startIndexEnumerator?.Current ?? startpos as ScalarType;
                occurencesEnumerator?.MoveNext();
                var scalarOccurences = occurencesEnumerator?.Current ?? occurences as ScalarType;
                patternEnumerator?.MoveNext();
                var scalarPattern = patternEnumerator?.Current ?? pattern as ScalarType;

                resultComponent.Add(PerformCalculation(scalar as StringType, scalarPattern as StringType,
                    scalarStart as IntegerType, scalarOccurences as IntegerType));
            }

            return resultComponent;
        }

        public ScalarType PerformCalculation(StringType scalar, StringType pattern, IntegerType startpos, IntegerType occurences)
        {
            if (!scalar.HasValue()) return scalar;
            if (!(scalar is StringType stringScalar)) throw new Exception("Första parametern i instr måste vara ett strängvärde.");
            if (!(pattern is StringType stringpattern)) throw new Exception("Andra parametern i instr måste vara ett strängvärde.");

            int startPosInt;
            if (startpos is IntegerType) startPosInt =(int)(startpos as IntegerType);
            else if (startpos == null) startPosInt = 1;
            else throw new Exception("Tredje parametern i instr måste vara ett positivt heltal.");
            if (startPosInt < 1) throw new Exception("Tredje parametern i instr måste vara ett positivt heltal.");

            int occurencesInt;
            if (occurences is IntegerType) occurencesInt = (int)(occurences as IntegerType);
            else if (occurences == null) occurencesInt = 1;
            else throw new Exception("Tredje parametern i instr måste vara ett positivt heltal.");

            int indexOfNth = IndexOfNth(stringScalar.ToString(), stringpattern.ToString(), startPosInt - 1, occurencesInt);
            return new IntegerType(indexOfNth == -1 ? 0: indexOfNth - (startPosInt - 2)) ;
        }

        private int IndexOfNth( string input,string pattern, int startIndex, int nth)
        {
            if (nth < 1)
                throw new NotSupportedException("Fjärde parametern i instr måste vara ett positivt heltal."); 

            if (nth == 1)
                return input.IndexOf(pattern, startIndex);
            var idx = input.IndexOf(pattern, startIndex);
            if (idx == -1)
                return -1;
            nth--;
            return IndexOfNth(input,pattern, idx + 1, nth);
        }

        internal override string[] GetIdentifierNames()
        {
            return GetIdentifierNames().Concat(new string[] { "int_var" }).ToArray();
        }

        internal override string[] GetMeasureNames()
        {
            return new string[] { "int_var" };
        }

        internal override string[] GetComponentNames()
        {
            return _operand.GetComponentNames();
        }
    }
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VTL.Vtl20Engine.Comparers;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Exceptions;
using static VTL.Vtl20Engine.DataTypes.CompoundDataTypes.ComponentType;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ClauseOperator
{
    public class PivotOperator : Operator
    {
        protected Operand InOperand;
        private string _identifierParameter;
        private string _measureParameter;
        private DataSetType _result;
        private bool _validation;

        public PivotOperator(Operand inOperand, string identifier, string measure, bool validation)
        {
            InOperand = inOperand;
            _identifierParameter = identifier;
            _measureParameter = measure;
            _validation = validation;
        }

        internal override DataType PerformCalculation()
        {
            if (_result == null)
            {
                DataSetType inDataSet = InOperand.GetValue() as DataSetType;

                if (inDataSet == null)
                {
                    throw new Exception("Pivot kan bara utföras på dataset.");
                }

                ComponentType pivotMeasureComponent = GetPivotMeasureComponent(inDataSet);
                ComponentType pivotIdentifiersComponent = GetPivotIdentifierComponent(inDataSet);
                var preservedIdentifiers = inDataSet.DataSetComponents.Where(c => c.Role == ComponentRole.Identifier && c != pivotIdentifiersComponent);

                var resultComponents = new List<ComponentType>();

                foreach (var preservedIdentifier in preservedIdentifiers)
                {
                    ComponentType comp = CreateNewComponent(preservedIdentifier, preservedIdentifier.Name);
                    resultComponents.Add(comp);
                }

                foreach (var datum in pivotIdentifiersComponent)
                {
                    if (preservedIdentifiers.Any(c => c.Name == datum.ToString()))
                    {
                        throw new Exception($"En komponent med namnet \"{datum}\" kan inte skapas då den redan finns i datasetet.");
                    }

                    if (resultComponents.Any(c => c.Name == datum.ToString())) continue;
                    ComponentType comp = CreateNewComponent(pivotMeasureComponent, datum.ToString());
                    resultComponents.Add(comp);
                }

                _result = new DataSetType(resultComponents.ToArray());

                inDataSet.SortDataPoints(preservedIdentifiers.Select(p => p.Name).ToArray());
                int indexPivotIdentifier = Array.IndexOf(inDataSet.ComponentSortOrder, pivotIdentifiersComponent.Name);
                int indexPivotMeasure = Array.IndexOf(inDataSet.ComponentSortOrder, pivotMeasureComponent.Name);
                var valuePositions = new List<KeyValuePair<int, ScalarType>>();
                DataPointType dataPoint = new DataPointType(resultComponents.Count);
                using (var dataPointEnumerator = inDataSet.DataPoints.GetEnumerator())
                {
                    while (dataPointEnumerator.MoveNext())
                    {
                        if (valuePositions.Any(vp => !dataPointEnumerator.Current[vp.Key].Equals(vp.Value)) || valuePositions.Count == 0)
                        {
                            if (valuePositions.Count > 0)
                            {
                                FixMissingValues(dataPoint, _result.DataSetComponents);
                                _result.Add(dataPoint);
                                dataPoint = new DataPointType(resultComponents.Count);
                            }
                            valuePositions.Clear();
                            foreach (var identifier in preservedIdentifiers)
                            {
                                var index = Array.IndexOf(inDataSet.ComponentSortOrder, identifier.Name);
                                var identifierValue = dataPointEnumerator.Current[index];
                                valuePositions.Add(
                                    new KeyValuePair<int, ScalarType>(index, identifierValue));
                                dataPoint[index] = identifierValue;
                            }
                        }

                        bool isIncluded = true;
                        foreach (var valuePosition in valuePositions)
                        {
                            isIncluded = (!dataPointEnumerator.Current[valuePosition.Key].Equals(valuePosition.Value));
                            if (!isIncluded) break;
                        }
                        var pivotValue = dataPointEnumerator.Current[indexPivotIdentifier];
                        int pivotIndex = Array.IndexOf(_result.ComponentSortOrder, pivotValue.ToString());
                        dataPoint[pivotIndex] = dataPointEnumerator.Current[indexPivotMeasure];
                    }
                    FixMissingValues(dataPoint, _result.DataSetComponents);
                    _result.Add(dataPoint);
                }
            }
            return _result;
        }

        private ComponentType GetPivotIdentifierComponent(DataSetType inDataSet)
        {
            var identifer = inDataSet.DataSetComponents.FirstOrDefault(c => c.Name.Equals(_identifierParameter));
            if(identifer == null)
            {
                throw new Exception($"{_identifierParameter} kunde inte hittas.");
            }
            if(identifer.Role != ComponentRole.Identifier)
            {
                throw new Exception($"Komponenten {_identifierParameter} är en värdekomponent, funktionen förväntar sig en identifierare.");
            }
            return identifer;
        }

        private ComponentType GetPivotMeasureComponent(DataSetType inDataSet)
        {
            var measure = inDataSet.DataSetComponents.FirstOrDefault(c => c.Name.Equals(_measureParameter));
            if (measure == null)
            {
                throw new Exception($"{_measureParameter} kunde inte hittas.");
            }
            if (measure.Role != ComponentRole.Measure)
            {
                throw new Exception($"Komponenten {_measureParameter} är en identifierare, funktionen förväntar sig en värdekomponent.");
            }
            return measure;
        }

        private void FixMissingValues(DataPointType datapoint, ComponentType[] resComponents)
        {
            for(int i = 0; i < datapoint.Count(); i++)
            {
                if (datapoint[i] == null)
                {
                    if (resComponents[i].DataType == typeof(BooleanType))
                    {
                        datapoint[i] = new BooleanType(null);
                    }
                    if (resComponents[i].DataType == typeof(DateType))
                    {
                        datapoint[i] = new DateType(null);

                    }
                    if (resComponents[i].DataType == typeof(IntegerType))
                    {
                        datapoint[i] = new IntegerType(null);
                    }
                    if (resComponents[i].DataType == typeof(NumberType))
                    {
                        datapoint[i] = new NumberType(null);
                    }

                    if (resComponents[i].DataType == typeof(StringType))
                    {
                        datapoint[i] = new StringType(null);
                    }

                    if (resComponents[i].DataType == typeof(TimePeriodType))
                    {
                        throw new Exception("TimePeriod kan ännu inte pivoteras om det innehåller nullvärden.");
                    }
                    if (resComponents[i].DataType == typeof(TimeType))
                    {
                        throw new Exception("Time kan ännu inte pivoteras om det innehåller nullvärden.");
                    }

                }
            }
        }

        private static ComponentType CreateNewComponent(ComponentType modelComponent, string name)
        {
            ComponentType comp = new ComponentType(modelComponent.DataType,
                VtlEngine.DataContainerFactory.CreateComponentContainer(modelComponent.Length));
            comp.Name = name;
            comp.Role = modelComponent.Role;
            return comp;
        }



        internal override string[] GetComponentNames()
        {
            if (_validation)
            {
                throw new OverrideValidationException("Pivot");
            }
            else
            {
                var result = PerformCalculation() as DataSetType;
                return result.ComponentSortOrder.Clone() as string[];
            }
        }

        internal override string[] GetIdentifierNames()
        {
            if (_validation)
            {
                throw new OverrideValidationException("Pivot");
            }
            else
            {
                DataSetType inDataSet = InOperand.GetValue() as DataSetType;

                if (inDataSet == null)
                {
                    throw new Exception("Pivot kan bara utföras på dataset.");
                }

                ComponentType pivotIdentifiersComponent = GetPivotIdentifierComponent(inDataSet);
                return inDataSet.DataSetComponents.Where(c => c.Role == ComponentRole.Identifier && c != pivotIdentifiersComponent).
                    Select(c => c.Name).ToArray().Clone() as string[];
            }
        }

        internal override string[] GetMeasureNames()
        {
            if (_validation)
            {
                throw new OverrideValidationException("Pivot");
            }
            else
            {
                var result = PerformCalculation() as DataSetType;
                return result.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Measure).
                    Select(c => c.Name).ToArray().Clone() as string[];
            }
        }
    }
}

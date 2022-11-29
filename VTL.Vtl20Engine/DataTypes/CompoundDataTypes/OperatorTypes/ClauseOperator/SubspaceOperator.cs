using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ClauseOperator
{
    public class SubspaceOperator : Operator
    {
        protected Operand InOperand;
        protected Operand[] ComponentOperands;

        public SubspaceOperator(Operand inOperand, Operand[] componentOperands)
        {
            InOperand = inOperand;
            ComponentOperands = componentOperands;
        }

        internal class OperandIndex
        {
            public string Name { get; set; }
            public int DataPointIndex { get; set; }
            public ScalarType SearchedScalarType { get; set; }
        }

        internal override DataType PerformCalculation()
        {

            var dataSet = InOperand.GetValue() as DataSetType;
            if (dataSet == null)
            {
                throw new Exception("Subspace kan bara utföras på dataset.");
            }

            if (ComponentOperands.Select(x => x.Alias).Distinct().Count() != ComponentOperands.Length)
            {
                throw new Exception("Sökt komponent får bara förekomma en gång");
            }

            var operandsIndex = new List<OperandIndex>();

            for (var i = 0; i < dataSet.DataSetComponents.Length; i++)
            {
                foreach (var componentOperand in ComponentOperands)
                    if (dataSet.DataSetComponents[i].Name.Equals(componentOperand.Alias))
                    {
                        ValidateDataType(dataSet.DataSetComponents[i], componentOperand);
                        operandsIndex.Add(new OperandIndex()
                        {
                            Name = dataSet.DataSetComponents[i].Name,
                            DataPointIndex = i,
                            SearchedScalarType = componentOperand.GetValue() as ScalarType
                        });
                    }
            }

            var newDataPointIndexLength = dataSet.DataSetComponents.Length - ComponentOperands.Length;
            var newComponents =
                dataSet.DataSetComponents.Where(x => !ComponentOperands.Select(n => n.Alias).Contains(x.Name));

            var newDataSet = new DataSetType(newComponents.ToArray());

            foreach (var dataSetPoint in dataSet.DataPoints)
            {
                var exists = true;
                foreach (var operandIndex in operandsIndex)
                {
                    var originalValueDataSetPoint = dataSetPoint[operandIndex.DataPointIndex];
                    var includeValue = operandIndex.SearchedScalarType;

                    if (!originalValueDataSetPoint.Equals(includeValue)) exists = false;
                }

                if (exists)
                {
                    newDataSet.DataPoints.Add(CreateDataPoint(dataSetPoint, operandsIndex, newDataPointIndexLength));
                }

            }

            return newDataSet;
        }

        private DataPointType CreateDataPoint(DataPointType originalDataPointType, List<OperandIndex> operandIndex, int arrayLength)
        {
            var result = new ScalarType[arrayLength];

            using (var originalDataPoint = originalDataPointType.GetEnumerator())
            {
                var excludedIndexRows = operandIndex.Select(x => x.DataPointIndex).ToArray();
                var orgIndex = 0;
                var newIndex = 0;
                while (originalDataPoint.MoveNext())
                {
                    if (!excludedIndexRows.Contains(orgIndex))
                    {
                        result[newIndex] = originalDataPoint.Current;
                        newIndex++;
                    }

                    orgIndex++;
                }
            }

            return new DataPointType(result);
        }
        private void ValidateDataType(ComponentType inComponent, Operand componentOperand)
        {
            var identifierType = inComponent.DataType;
            var componentType = componentOperand.GetValue().GetType();

            if (inComponent.Role != ComponentType.ComponentRole.Identifier)
                throw new Exception($"Sökt komponent måste ha rollen IDENTIFIER.");

            if (identifierType == typeof(IntegerType) || identifierType == typeof(NumberType))
            {
                if (componentType == typeof(IntegerType) || componentType == typeof(NumberType)) return;
            }
            if (componentType == identifierType) return;

            throw new Exception($"Identifier {inComponent.Name} har inte samma datatyp som sökt data");

        }

        internal override string[] GetComponentNames()
        {
            var result = InOperand.GetComponentNames().Except(ComponentOperands.Select(x => x.Alias));
            return result.ToArray();

        }

        internal override string[] GetIdentifierNames()
        {
            var result = InOperand.GetIdentifierNames().Except(ComponentOperands.Select(x => x.Alias));
            return result.ToArray();
        }

        internal override string[] GetMeasureNames()
        {
            return InOperand.GetMeasureNames();
        }
    }
}
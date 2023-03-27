using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.JoinOperator
{
    public class CrossJoin : Operator
    {
        private readonly List<Operand> _operands;

        public CrossJoin(List<Operand> operands)
        {
            _operands = operands;
        }

        internal override DataType PerformCalculation()
        {
            var dsOperands = _operands.AsParallel().Select(o => o.GetValue()).OfType<DataSetType>().ToArray();
            if (dsOperands.Length != _operands.Count)
            {
                throw new Exception("cross_join kan endast hantera dataset.");
            }

            if (dsOperands.Length < 2) throw new Exception("cross_join måste utföras på minst två dataset.");

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

        private DataSetType PerformCalculation(DataSetType dataSet1, DataSetType dataSet2)
        {
            var result = new DataSetType(dataSet1.DataSetComponents.Concat(dataSet2.DataSetComponents).ToArray());
            foreach (var dp1 in dataSet1.DataPoints)
            {
                foreach (var dp2 in dataSet2.DataPoints)
                {
                    var newDataPoint = new DataPointType(dp1.Count() + dp2.Count());
                    for (var k = 0; k < dataSet1.DataSetComponents.Length; k++)
                    {
                        newDataPoint[k] = dp1[k];
                    }

                    for (var k = 0; k < dataSet2.DataSetComponents.Length; k++)
                    {
                        newDataPoint[k + dataSet1.DataSetComponents.Length] = dp2[k];
                    }

                    result.Add(newDataPoint);
                }
            }


            return result;
        }

        internal override string[] GetComponentNames()
        {
            return _operands.SelectMany(o => o.GetComponentNames().Select(n => o.Alias + "#" + n)).ToArray();
        }

        internal override string[] GetIdentifierNames()
        {
            return _operands.SelectMany(o => o.GetIdentifierNames().Select(n => o.Alias + "#" + n)).ToArray();
        }

        internal override string[] GetMeasureNames()
        {
            return _operands.SelectMany(o => o.GetMeasureNames().Select(n => o.Alias + "#" + n)).ToArray();
        }
    }
}

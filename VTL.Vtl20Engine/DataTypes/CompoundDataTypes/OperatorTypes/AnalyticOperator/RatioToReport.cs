using System;
using System.Collections.Generic;
using System.Linq;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.AnalyticOperator
{
    public class RatioToReport : AnalyticOperator
    {
        public RatioToReport(Operand operand, IEnumerable<string> partitionBy)
        {
            _operand = operand;
            _partitionBy = partitionBy;
        }

        internal override IEnumerable<DataType> PerformCalculation(List<DataPointType> partition, List<int> measureIndexes, List<Tuple<int, bool>> orderByIndexes)
        {
            for (int i = 0; i < measureIndexes.Count(); i++)
            {
                if (partition.Any() && partition[0][measureIndexes[i]] is NumberType)
                {
                    var sum = new NumberType(0m);
                    foreach (var dataPoint in partition)
                    {
                        if (dataPoint[measureIndexes[i]] is NumberType number && number.HasValue())
                            sum += number;
                    }

                    foreach (var dataPoint in partition)
                    {
                        if (dataPoint[measureIndexes[i]] is NumberType number)
                            dataPoint[measureIndexes[i]] = sum != 0m ? number / sum : new NumberType(null);
                    }
                }
            }

            return partition;
        }

        protected override Type GetResultMeasureDatatype(ComponentType component)
        {
            if (component.DataType == typeof(IntegerType))
            {
                return typeof(NumberType);
            }

            return component.DataType;
        }
    }
}

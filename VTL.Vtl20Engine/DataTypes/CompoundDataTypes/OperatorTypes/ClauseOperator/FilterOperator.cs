using System;
using System.Linq;
using System.Threading.Tasks;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ClauseOperator
{
    public class FilterOperator : Operator
    {
        protected Operand InOperand;
        protected Operand ConditionOperand;

        public FilterOperator(Operand inOperand, Operand conditionOperand)
        {
            InOperand = inOperand;
            ConditionOperand = conditionOperand;
        }

        internal override DataType PerformCalculation()
        {
            DataSetType dataSet = null;
            DataType condition = null;
            Parallel.Invoke(
                () => dataSet = InOperand.GetValue() as DataSetType,
                () => condition = ConditionOperand.GetValue());

            if (dataSet == null)
            {
                throw new Exception($"{InOperand.Alias} kändes inte igen.");
            }

            if (condition is BooleanType booleanScalar)
            {
                if (booleanScalar == true)
                {
                    return new DataSetType(dataSet);
                }
                else
                {
                    return new DataSetType(dataSet.DataSetComponents);
                }
            }

            var result = new DataSetType(dataSet.DataSetComponents);
            using (var dataSetEnumerator = dataSet.DataPoints.GetEnumerator())
            {

                if (condition is ComponentType componentCondition)
                {
                    using (var conditionEnumerator = componentCondition.GetEnumerator())
                    {
                        while (dataSetEnumerator.MoveNext() && conditionEnumerator.MoveNext())
                        {
                            if (conditionEnumerator.Current is BooleanType boolean && boolean == true)
                            {
                                result.Add(dataSetEnumerator.Current);
                            }
                        }
                    }
                }
                else if (condition is DataSetType dataSetCondition)
                {
                    componentCondition =
                        dataSetCondition.DataSetComponents.FirstOrDefault(c => c.DataType == typeof(BooleanType));
                    using (var conditionEnumerator = componentCondition.GetEnumerator())
                    {
                        while (dataSetEnumerator.MoveNext() && conditionEnumerator.MoveNext())
                        {
                            if (conditionEnumerator.Current is BooleanType boolean && boolean == true)
                            {
                                result.Add(dataSetEnumerator.Current);
                            }
                        }
                    }
                }
            }

            return result;
        }

        internal override string[] GetComponentNames()
        {
            return InOperand.GetComponentNames();
        }

        internal override string[] GetIdentifierNames()
        {
            return InOperand.GetIdentifierNames();
        }

        internal override string[] GetMeasureNames()
        {
            return InOperand.GetMeasureNames();
        }
    }
}

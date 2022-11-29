using System;
using System.Linq;
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
            var dataSet = InOperand.GetValue() as DataSetType;

            if (dataSet == null)
            {
                throw new Exception($"{InOperand.Alias} kändes inte igen.");
            }
            var result = new DataSetType(dataSet.DataSetComponents);
            var condition = ConditionOperand.GetValue();
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

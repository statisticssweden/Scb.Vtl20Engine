using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.StringOperator
{
    public class Length : Operator
    {
        private Operand _operand;
        public Length(Operand operand)
        {
            _operand = operand;
            if (operand.GetMeasureNames().Length > 1)
                throw new Exception("Operatorn length kan inte användas på datasetnivå om datasetet har mer än en measurekomponent.");
        }

        internal override DataType PerformCalculation()
        {
            var operandData = _operand.GetValue();
            if (operandData is DataSetType dataSet)
            {
                return PerformCalculation(dataSet);
            }

            if (operandData is ComponentType component)
            {
                return PerformCalculation(component);
            }

            if (operandData is ScalarType scalar)
            {
                return PerformCalculation(scalar);
            }

            throw new Exception("Substr är inte implemenenterad för denna datatyp");

        }

        public  ScalarType PerformCalculation(ScalarType scalar)
        {
            if (scalar is StringType lengthstring)
            {
                return scalar.HasValue() ?
                    new IntegerType(lengthstring.ToString().Length) : new IntegerType(null);
            }
            throw new Exception("Operatorn length kan bara utföras på strängar.");
        }

        public ComponentType PerformCalculation(ComponentType component)
        {
            var resultComponent = new ComponentType(typeof(IntegerType), VtlEngine.DataContainerFactory.CreateComponentContainer(component.Length))
            {
                Role = component.Role
            };
            foreach (var scalar in component)
            {
                resultComponent.Add(PerformCalculation(scalar));
            }

            return resultComponent;
        }

        public DataSetType PerformCalculation(DataSetType dataSet)
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

            foreach (var operandDataPoint in dataSet.DataPoints)
            {
                var resultDataPoint = new DataPointType(dataSet.DataSetComponents.Length);
                for (int j = 0; j < dataSet.DataSetComponents.Length; j++)
                {
                    if (dataSet.DataSetComponents[j].Role == ComponentType.ComponentRole.Identifier)
                        resultDataPoint[j] = operandDataPoint[j];
                    if (dataSet.DataSetComponents[j].Role == ComponentType.ComponentRole.Measure)
                        resultDataPoint[j] = PerformCalculation(operandDataPoint[j]);
                }

                result.Add(resultDataPoint);
            }

            return result;
        }

        internal override string[] GetComponentNames()
        {
            return GetIdentifierNames().Concat(new string[] { "int_var" }).ToArray();
        }

        internal override string[] GetIdentifierNames()
        {
            return _operand.GetIdentifierNames();
        }

        internal override string[] GetMeasureNames()
        {
            return  new string[] { "int_var" };
        }

    }
}

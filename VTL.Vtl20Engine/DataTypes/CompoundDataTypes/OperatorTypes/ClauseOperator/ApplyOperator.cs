using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.ClauseOperator
{
    public class ApplyOperator : Operator
    {
        private readonly Operand _operand;
        private readonly Operand _apply;

        public ApplyOperator(Operand operand, Operand apply)
        {
            _operand = operand;
            _apply = apply;
        }

        internal override DataType PerformCalculation()
        {
            var components = new List<ComponentType>();

            DataType operand = null, apply = null;
            Parallel.Invoke(
                () => operand = _operand.GetValue(),
                () => apply = _apply.GetValue());

            if (operand is DataSetType ds && apply is ComponentType cp)
            {
                foreach (var dsc in ds.DataSetComponents)
                {
                    if (!dsc.Name.EndsWith(_apply.Alias))
                    {
                        components.Add(dsc);
                    }
                }
                ComponentType applyComponent = _apply.GetValue() as ComponentType;
                applyComponent.Name = _apply.Alias;
                components.Add(applyComponent);

                var result = new DataSetType(components.ToArray());

                using (var dsEnumerator = ds.GetDataPointEnumerator())
                using (var cpEnumerator = cp.GetEnumerator())
                {
                    while (dsEnumerator.MoveNext() && cpEnumerator.MoveNext())
                    {
                        var newDataPoint = new DataPointType(components.Count);
                        int oldIndex = 0, newIndex = 0;
                        foreach (var dsc in ds.DataSetComponents)
                        {
                            if (!dsc.Name.EndsWith(_apply.Alias))
                            {
                                newDataPoint[newIndex++] = dsEnumerator.Current[oldIndex];
                            }
                            oldIndex++;
                        }
                        newDataPoint[newIndex] = cpEnumerator.Current;
                        result.Add(newDataPoint);
                    }
                }
                return result;
            }
            return null;
        }

        internal override string[] GetComponentNames()
        {
            var result = new List<string>(_operand.GetComponentNames());
            result.RemoveAll(s => s.EndsWith(_apply.Alias));
            result.Add(_apply.Alias);
            return result.ToArray();
        }

        internal override string[] GetIdentifierNames()
        {
            return _operand.GetIdentifierNames();
        }

        internal override string[] GetMeasureNames()
        { 
            var result = new List<string>(_operand.GetMeasureNames());
            result.RemoveAll(s => s.EndsWith(_apply.Alias));
            result.Add(_apply.Alias);
            return result.ToArray();
        }
 
    }
}

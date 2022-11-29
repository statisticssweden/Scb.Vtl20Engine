using System;
using System.Linq;
using Antlr4.Runtime;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes
{
    public class Operand : IDisposable
    {
        public string Alias { get; set; }

        public ParserRuleContext Source { get; set; }

        private DataType _result;

        public DataType Data { get; set; }

        public ComponentType.ComponentRole? Role;

        public bool Persistant { get; set; }

        public DataType GetValue()
        {
            try
            {
                if (_result == null)
                {
                    if(Data is Operator op)
                    {
                        _result = op.PerformCalculationWithLog();
                        if(string.IsNullOrEmpty(Alias))
                        {
                            Data.Dispose();
                        }
                    }
                    else
                    {
                        _result = Data;
                    }
                    if (_result is ComponentType comp && Role != null)
                    {
                        comp.Role = Role;
                    }

                    if (_result is DataSetType ds)
                    {
                        ds.SortComponents();
                        ds.SortDataPoints();
                    }
                }

                if (_result is DataSetType dataSet)
                {
                    return new DataSetType(dataSet);
                }

                if (_result is ComponentType component)
                {
                    return new ComponentType(component);
                }

                return _result;
            }
            catch (VtlException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new VtlException(e.Message, e, Alias);
            }
        }

        public string[] GetComponentNames()
        {
            if (Data is Operator op)
                return op.GetComponentNames();
            if (Data is DataSetType dataSet)
                return dataSet.DataSetComponents.Select(c => c.Name).ToArray();
            if (Data is ComponentType component)
                return new []{component.Name};
            return new string[0];
        }

        public string[] GetIdentifierNames()
        {
            if (Data is Operator op)
                return op.GetIdentifierNames();
            if (Data is DataSetType dataSet)
                return dataSet.DataSetComponents
                    .Where(c => c.Role == ComponentType.ComponentRole.Identifier)
                    .Select(c => c.Name).ToArray();
            if (Data is ComponentType component && component.Role == ComponentType.ComponentRole.Identifier)
                return new[] { component.Name };
            return new string[0];
        }

        public string[] GetMeasureNames()
        {
            if (Data is Operator op)
                return op.GetMeasureNames();
            if (Data is DataSetType dataSet)
                return dataSet.DataSetComponents
                    .Where(c => c.Role == ComponentType.ComponentRole.Measure)
                    .Select(c => c.Name).ToArray();
            if (Data is ComponentType component && component.Role == ComponentType.ComponentRole.Measure)
                return new[] { component.Name };
            return new string[0];
        }

        public bool NameEquals(string compareName)
        {
            if (Alias.Contains("#"))
            {
                return compareName == Alias;
            }
            return compareName.Substring(compareName.LastIndexOf("#") + 1) == Alias;
        }

        public void Dispose()
        {
            if (_result != null)
            {
                _result.Dispose();
            }
        }
    }
}

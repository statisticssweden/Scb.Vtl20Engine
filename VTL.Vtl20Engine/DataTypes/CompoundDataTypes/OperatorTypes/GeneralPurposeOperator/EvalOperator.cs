using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VTL.Vtl20Engine.Contracts;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes;
using VTL.Vtl20Engine.DataTypes.ScalarDataTypes.BasicScalarTypes;
using VTL.Vtl20Engine.Exceptions;

namespace VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperatorTypes.GeneralPurposeOperator
{

    internal class EvalOperator : Operator
    {

        private readonly IEnumerable<Operand> _operand;
        private readonly IExternalFunctionExecutor _exteranlFunctionExecutor;
        private readonly string _externaRoutineName;
        private DataType _result;
        private bool _validation;
        
        public EvalOperator(IEnumerable<Operand> operand, string externaRoutineName, IExternalFunctionExecutor exteranlFunctionExecutor, bool validation)
        {
            _operand = operand;
            _externaRoutineName = externaRoutineName;
            _exteranlFunctionExecutor = exteranlFunctionExecutor;
            _validation = validation;
        }

        internal override DataType PerformCalculation()
        {
            if (_result == null)
            {
                var operands = new List<DataType>();
                foreach (var operand in _operand)
                {
                    var op = operand.GetValue();
                    if (op == null)
                    {
                        throw new Exception($"{operand.Alias} är en okänd variabel.");
                    }
                    operands.Add(op);
                }
                _result = _exteranlFunctionExecutor.Execute(_externaRoutineName, operands, "");
            }
            return _result;
        }

        internal override string[] GetComponentNames()
        {
            if (_validation)
            {
                throw new OverrideValidationException("Eval");
            }
            else
            {
                var result = PerformCalculation() as DataSetType;
                if (result == null)
                {
                    return new string[0];
                }
                return result.ComponentSortOrder.Clone() as string[];
            }
        }

        internal override string[] GetIdentifierNames()
        {
            if (_validation)
            {
                throw new OverrideValidationException("Eval");
            }
            else
            {
                var result = PerformCalculation() as DataSetType;
                if(result == null)
                {
                    return new string[0];
                }
                return result.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Identifier).
                    Select(c => c.Name).ToArray().Clone() as string[];
            }
        }

        internal override string[] GetMeasureNames()
        {
            if (_validation)
            {
                throw new OverrideValidationException("Eval");
            }
            else
            {
                var result = PerformCalculation() as DataSetType;
                if (result == null)
                {
                    return new string[0];
                }
                return result.DataSetComponents.Where(c => c.Role == ComponentType.ComponentRole.Measure).
                    Select(c => c.Name).ToArray().Clone() as string[];
            }
        }

    }

    //internal class EvalOperator : Operator
    //{

    //    private readonly OutputParameterType _operand;

    //    public EvalOperator(OutputParameterType operand, string externaRoutineName, string language)
    //    {
    //        _operand = operand;
    //        ExternaRoutineName = externaRoutineName;

    //        switch (language.ToLower().Replace("\"", ""))
    //        {
    //            case "sql":
    //                Language = LanguageTypeEnum.SQL;
    //                break;
    //            case "sas":
    //                Language = LanguageTypeEnum.Unknown;
    //                break;
    //            default:
    //                Language = LanguageTypeEnum.Unknown;
    //                break;
    //        }

    //    }

    //    internal override string[] GetComponentNames()
    //    {
    //        if (_operand is BasicScalarType)
    //            return new string[0];
    //        else if (_operand is DatasetType dst)
    //        {
    //            var components = dst.DataSet.CompConstraints.Select(x=>x.ComponentId.Identifier).ToArray();
    //            return components;
    //        }
    //        throw new NotImplementedException();
    //    }

    //    internal override string[] GetIdentifierNames()
    //    {
    //        if (_operand is BasicScalarType)
    //            return new string[0];
    //        else if (_operand is DatasetType dst)
    //        {
    //            var identifiers = dst.DataSet.CompConstraints
    //                .Where(x=>x.ComponentType.ComponentRole.ComponentRoleEnum==ComponentRoleEnum.TempIdentifier)
    //                .Select(x => x.ComponentId.Identifier).ToArray();
    //            return identifiers;
    //        }
    //        throw new NotImplementedException();
    //    }

    //    internal override string[] GetMeasureNames()
    //    {
    //        if (_operand is BasicScalarType)
    //            return new string[0];
    //        else if (_operand is DatasetType dst)
    //        {
    //            var measures = dst.DataSet.CompConstraints
    //                .Where(x => x.ComponentType.ComponentRole.ComponentRoleEnum == ComponentRoleEnum.MEASURE)
    //                .Select(x => x.ComponentId.Identifier).ToArray();
    //            return measures;
    //        }
    //        throw new NotImplementedException();
    //    }

    //    internal override DataType PerformCalculation()
    //    {

    //        return new IntegerType(42);

    //    }
    //    internal string ExternaRoutineName { get; }
    //    internal LanguageTypeEnum Language { get; }
    //}


        /*

    internal abstract class OutputParameterType
    {

    }

    #region scalarType
    internal class ScalarType : OutputParameterType
    {

    }

    internal class BasicScalarType : ScalarType
    {
        public BasicScalarType(string type)
        {
            switch (type)
            {
                case "integer":
                    BasicScalarTypeEnum = BasicScalarTypeEnum.INTEGER;
                    break;
                case "string":
                    BasicScalarTypeEnum = BasicScalarTypeEnum.STRING;
                    break;
            }
        }
        public BasicScalarTypeEnum BasicScalarTypeEnum { get; }
    }

    internal class ValueDomainName : ScalarType
    {
        public ValueDomainName(string identifier)
        {
            Identifier = identifier;
        }
        public string Identifier { get; }
    }

    internal class SetName : ScalarType
    {
        public SetName(string identifier)
        {
            Identifier = identifier;
        }
        public string Identifier { get; }
    }


    public enum BasicScalarTypeEnum
    {
        STRING,
        INTEGER,
        NUMBER,
        BOOLEAN,
        DATE,
        TIME_PERIOD,
        DURATION,
        SCALAR,
        TIME
    }

    #endregion

    #region datasetType

    internal class DatasetType : OutputParameterType
    {
        public DatasetType(DataSet dataSet)
        {
            DataSet = dataSet;
        }
        public DataSet DataSet { get; }
    }
    internal class DataSet
    {
        public DataSet(CompConstraint[] compConstraints)
        {
            CompConstraints = compConstraints;
        }
        public CompConstraint[] CompConstraints { get; }
    }

    internal class CompConstraint
    {
        public CompConstraint(ComponentType_ componentType, string componentId = null, string multModifier = null)
        {
            ComponentType = componentType;
            if (!string.IsNullOrWhiteSpace(componentId))
            {
                ComponentId = new ComponentId(componentId);
            }
            else
            {
                MultModifier = new MultModifier(multModifier);
            }

        }
        public ComponentType_ ComponentType { get; }
        public ComponentId ComponentId { get; }

        public MultModifier MultModifier { get; }
    }
    internal class ComponentType_
    {
        public ComponentType_(ComponentRole componentRole, ScalarType scalarType = null)
        {
            ComponentRole = componentRole;
            ScalarType = scalarType;
        }
        public ComponentRole ComponentRole { get; }
        public ScalarType ScalarType { get; }
    }

    internal class ComponentRole
    {
        public ComponentRole(string componentRoleString)
        {
            switch (componentRoleString)
            {
                case "measure":
                    ComponentRoleEnum = ComponentRoleEnum.MEASURE;
                    break;
                case "component":
                    ComponentRoleEnum = ComponentRoleEnum.COMPONENT;
                    break;
                case "identifier":
                    ComponentRoleEnum = ComponentRoleEnum.TempIdentifier;
                    break;
                default:
                    ComponentRoleEnum = ComponentRoleEnum.Unknown;
                    break;
            }
        }
        public ComponentRoleEnum ComponentRoleEnum { get; }

    }

    internal class ComponentId
    {
        public ComponentId(string identifier)
        {
            Identifier = identifier;
        }
        public string Identifier { get; }
    }

    internal class MultModifier
    {
        public MultModifier(string modifier)
        {
            Modifier = modifier;
        }
        public string Modifier { get; }
    }
    public enum ComponentRoleEnum
    {
        MEASURE,
        COMPONENT,
        DIMENSION,
        ATTRIBUTE,
        viralAttribute,
        TempIdentifier,
        Unknown
    }

    #endregion
    */
}

using System.Collections.Generic;
using VTL.Vtl20Engine.DataTypes;

namespace VTL.Vtl20Engine.Contracts
{
    public interface IExternalFunctionExecutor
    {
        DataType Execute(string name, IEnumerable<DataType> argument, string returnType);
        string[] GetOutputComponentNames(string name);
        string[] GetOutputIdentifierNames(string name);
        string[] GetOutputMeasureNames(string name);
    }
}

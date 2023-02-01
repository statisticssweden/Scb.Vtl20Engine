using System.Collections.Generic;
using VTL.Vtl20Engine.DataTypes;

namespace VTL.Vtl20Engine.Contracts
{
    public interface IExternalFunctionExecutor
    {
        /// <summary>
        /// Executes the external function with provided arguments
        /// </summary>
        /// <param name="name">Name of the external function</param>
        /// <param name="argument">Arguments of any VTL type</param>
        /// <param name="returnType">For future use</param>
        /// <returns>Result of external function data processing</returns>
        DataType Execute(string name, IEnumerable<DataType> argument, string returnType);
    }
}

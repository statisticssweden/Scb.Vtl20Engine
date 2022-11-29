using System;
using System.Collections.Generic;
using VTL.Vtl20Engine.DataTypes;
using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;

namespace VTL.Vtl20Engine
{
    public interface IVtlEngine
    {
        /// <summary>
        /// Executes the VTL code in the provided VTL transformation scheme with
        /// the input operands specified.
        /// </summary>
        /// <param name="vtlTransformationScheme">Posible multiline VTL code.</param>
        /// <param name="inputOperands">Input operands containing vtl type data
        /// and alias as refered by the VTL code.</param>
        /// <returns>All persistant calculation results allocated with the := operator
        /// containing their data and alias.</returns>
        Operand[] Execute(string vtlTransformationScheme, Operand[] inputOperands);
        Operand[] Validate(string vtlTransformationScheme, Operand[] inputOperands);
        void ValidateSyntax(string vtlTransformationScheme);
    }
}

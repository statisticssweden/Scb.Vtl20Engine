using VTL.Vtl20Engine.DataTypes.CompoundDataTypes.OperandTypes;

namespace VTL.Vtl20Engine
{
    public interface IVtlEngine
    {
        /// <summary>
        /// Executes the provided the VTL code with the input operands specified.
        /// </summary>
        /// <param name="vtlTransformationScheme">Possible multiline VTL code.</param>
        /// <param name="inputOperands">Input operands containing vtl type data
        /// and alias as referred by the VTL code.</param>
        /// <returns>All persistent calculation results allocated with the <- operator
        /// containing their data and alias.</returns>
        Operand[] Execute(string vtlTransformationScheme, Operand[] inputOperands);

        /// <summary>
        /// Dry execution of the provided of the VTL code.
        /// Validates VTL code and alias matching of input operands.
        /// The provided input operands may be empty data sets.
        ///
        /// The commands Pivot and Eval has output structure dependent on input data.
        /// Thus, the data structure cannot be validated for these commands and
        /// will always validate true.
        /// </summary>
        /// <param name="vtlTransformationScheme">Possible multiline VTL code.</param>
        /// <param name="inputOperands">Input operands that may contain empty data sets.
        /// Alias as referred by the VTL code.</param>
        /// <returns>All persistent calculation results allocated with the <- operator
        /// for validation purpuse only.</returns>
        Operand[] Validate(string vtlTransformationScheme, Operand[] inputOperands);

        /// <summary>
        /// Syntax validation of provided VTL code.
        /// </summary>
        /// <param name="vtlTransformationScheme">Possible multiline VTL code.</param>
        void ValidateSyntax(string vtlTransformationScheme);
    }
}

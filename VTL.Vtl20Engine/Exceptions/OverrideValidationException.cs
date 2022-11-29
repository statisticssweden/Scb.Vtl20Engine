using System;

namespace VTL.Vtl20Engine.Exceptions
{
    public class OverrideValidationException : Exception
    {
        public OverrideValidationException(string message)
            : base(message)
            { }
    }
}

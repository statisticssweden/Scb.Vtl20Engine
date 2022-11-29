using System;
using System.Collections.Generic;
using System.Text;

namespace VTL.Vtl20Engine.Exceptions
{
    public class VTLParserException : Exception
    {
        public VTLParserException(string message, Exception innerException)
        : base(message, innerException)
        {
  
        }
    }
}

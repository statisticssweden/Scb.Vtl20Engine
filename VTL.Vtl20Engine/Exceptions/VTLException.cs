using System;

namespace VTL.Vtl20Engine
{
    public class VtlException : Exception
    {
        public string Alias { get; set; }

        public VtlException(string message, Exception innerException, string alias)
        :base(message, innerException)
        {
            Alias = alias;
        }
    }
}

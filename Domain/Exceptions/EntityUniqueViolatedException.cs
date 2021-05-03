using System;
using System.Collections.Generic;
using System.Text;

namespace Matrix.QC.Domain.Exceptions
{
    /// <summary>
    /// Exception que representa chave duplicada
    /// </summary>
    public class EntityUniqueViolatedException : Exception
    {
        public EntityUniqueViolatedException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }
    }
}

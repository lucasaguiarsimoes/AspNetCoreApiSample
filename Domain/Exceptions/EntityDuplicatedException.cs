using System;
using System.Collections.Generic;
using System.Text;

namespace Matrix.QC.Domain.Exceptions
{
    /// <summary>
    /// Exception que representa uma falha por uma entidade em duplicidade
    /// </summary>
    public class EntityDuplicatedException : Exception
    {
        public EntityDuplicatedException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }
    }
}

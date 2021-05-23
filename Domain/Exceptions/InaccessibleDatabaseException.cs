using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreApiSample.Domain.Exceptions
{
    /// <summary>
    /// Exception que representa uma falha por problemas de acesso ao banco de dados
    /// </summary>
    public class InaccessibleDatabaseException : Exception
    {
        public InaccessibleDatabaseException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }
    }
}

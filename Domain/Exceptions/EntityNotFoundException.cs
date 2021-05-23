using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreApiSample.Domain.Exceptions
{
    /// <summary>
    /// Exception que representa a entidade esperada não encontrada
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }
    }
}

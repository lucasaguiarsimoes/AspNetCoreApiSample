using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Domain
{
    public static class Constants
    {
        /// <summary>
        /// Texto reservado utilizado pela autenticação JWT para representar o nome do claim utilizado para representar os 'Roles'
        /// </summary>
        public const string ROLE_CLAIM_TYPE = "Role";
    }
}

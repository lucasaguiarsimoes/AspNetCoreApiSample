using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Domain.Transfer
{
    /// <summary>
    /// Parâmetros para configuração da autenticação via Bearer Token
    /// </summary>
    public class TokenBearerParameters
    {
        /// <summary>
        /// Chave de segurança da assinatura do jwt
        /// </summary>
        public SecurityKey SigningKey { get; set; } = null!;

        /// <summary>
        /// Audiência do jwt
        /// </summary>
        public string Audience { get; set; } = null!;

        /// <summary>
        /// Issuer do jwt
        /// </summary>
        public string Issuer { get; set; } = null!;
    }
}

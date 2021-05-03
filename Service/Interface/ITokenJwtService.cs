using AspNetCoreApiSample.Domain.Transfer;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace AspNetCoreApiSample.Service.Interface
{
    public interface ITokenJwtService
    {
        /// <summary>
        /// Gera um novo token para autenticação com o sistema a partir de claims já conhecidos
        /// </summary>
        string CreateToken(IEnumerable<Claim> claimsIdentity);

        /// <summary>
        /// Obtém os parâmetros para configuração da autenticação via Bearer Token
        /// </summary>
        TokenBearerParameters GetTokenBearerParameters();
    }
}

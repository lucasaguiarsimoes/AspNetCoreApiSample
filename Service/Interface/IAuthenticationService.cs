using AspNetCoreApiSample.Domain.Commands;
using AspNetCoreApiSample.Domain.Transfer;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Service.Interface
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Realiza o login recebendo as credenciais e retornando o token
        /// </summary>
        Task<string> LoginAsync(LoginCommand loginCommand, CancellationToken cancellationToken);
    }
}

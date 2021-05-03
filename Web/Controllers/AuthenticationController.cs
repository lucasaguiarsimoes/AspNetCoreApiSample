using AspNetCoreApiSample.Domain.Commands;
using AspNetCoreApiSample.Domain.Enums;
using AspNetCoreApiSample.Domain.Model;
using AspNetCoreApiSample.Domain.Queries;
using AspNetCoreApiSample.Domain.QueryResponses;
using AspNetCoreApiSample.Service.Interface;
using AspNetCoreApiSample.Web.Attributes;
using AspNetCoreApiSample.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IMapper mapper, IAuthenticationService authenticationService)
        {
            this._mapper = mapper;
            this._authenticationService = authenticationService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<TokenViewModel>> Login([FromBody] LoginViewModel loginViewModel, CancellationToken cancellationToken)
        {
            // Aciona o login para obter o token
            TokenViewModel token = await RequestToken(loginViewModel, cancellationToken);

            // Aplica o Action para o retorno
            return Ok(token);
        }

        /// <summary>
        /// Faz a requisição do token através do comando de login
        /// </summary>
        private async Task<TokenViewModel> RequestToken(LoginViewModel login, CancellationToken cancellationToken)
        {
            // Monta o Command a partir do ViewModel
            LoginCommand loginCommand = this._mapper.Map<LoginCommand>(login);

            // Aciona o Command pelo service
            return new TokenViewModel()
            {
                Token = await this._authenticationService.LoginAsync(loginCommand, cancellationToken)
            };
        }
    }
}

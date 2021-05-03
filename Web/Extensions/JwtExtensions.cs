using AspNetCoreApiSample.Domain;
using AspNetCoreApiSample.Domain.Transfer;
using AspNetCoreApiSample.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Web.Extensions
{
    /// <summary>
    /// Classe de extensão para configuração do token JWT
    /// </summary>
    public static class JwtExtensions
    {
        /// <summary>
        /// Configura a autorização para o token jwt
        /// </summary>
        public static IServiceCollection AddJwtAuthorization(this IServiceCollection services)
        {
            // Carrega o serviço de token para obter os dados necessários para a configuração do JwtBearer
            IServiceProvider provider = services.BuildServiceProvider();
            ITokenJwtService tokenService = provider.GetRequiredService<ITokenJwtService>();
            TokenBearerParameters bearerParameters = tokenService.GetTokenBearerParameters();

            // Configura a autenticação via Bearer/JWT
            services
                .AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                // Adiciona configurações de comportamento de autenticação via Bearer/JWT
                .AddJwtBearer(bearerOptions =>
                {
                    // Fornece a chave privada para geração de tokens JWT e os nomes de quem solicita e de quem valida o token, respectivamente
                    TokenValidationParameters paramsValidation = bearerOptions.TokenValidationParameters;
                    paramsValidation.IssuerSigningKey = bearerParameters.SigningKey;
                    paramsValidation.ValidAudience = bearerParameters.Audience;
                    paramsValidation.ValidIssuer = bearerParameters.Issuer;

                    // Valida quem é responsável por gerar o token (Isto é, esta aplicação/API)
                    paramsValidation.ValidateIssuer = true;

                    // Valida a assinatura de um token recebido
                    paramsValidation.ValidateIssuerSigningKey = true;

                    // Valida quem solicita o token
                    paramsValidation.ValidateAudience = true;

                    // Valida se um token recebido ainda é válido, isto é, valida a expiração do token
                    paramsValidation.ValidateLifetime = true;

                    // Tempo de tolerância após a expiração de um token
                    paramsValidation.ClockSkew = TimeSpan.Zero;

                    // Ao utilizar claimTypes customizadas é necessário informar ao ClaimsPrincipal 
                    // para que o IsInRole utilize o type customizado na validação.
                    paramsValidation.RoleClaimType = Constants.ROLE_CLAIM_TYPE;
                });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(
                    "Bearer",
                    new AuthorizationPolicyBuilder()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build()
                );
            });

            return services;
        }
    }
}

using AspNetCoreApiSample.Domain.Transfer;
using AspNetCoreApiSample.Service.Interface;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AspNetCoreApiSample.Service.Services
{
    public class TokenJwtService : ITokenJwtService
    {
        /// <summary>
        /// Audiência do JWT: É quem solicita e utiliza o token
        /// </summary>
        public const string Audience = "aspnetcoreapi-sample-client";

        /// <summary>
        /// Issuer do JWT: É quem gera e valida o token
        /// </summary>
        public const string Issuer = "aspnetcoreapi-sample-api";

        public string CreateToken(IEnumerable<Claim> claimsIdentity)
        {
            // Define a data de validade do token
            DateTime dataValidade = DateTime.Now.AddHours(4);

            // Remove "aud" Audience caso já esteja na lista (renovação de token)
            // Motivo: Na renovação de token a partir de um token já existente, um novo valor para 'aud' é colocado na lista de Claims e após diversas renovações o payload fica gigantesco, causando:
            // BadRequest (400 Request Header Or Cookie Too Large)
            IEnumerable<Claim> claimsList = claimsIdentity.Where(c => c.Type != JwtRegisteredClaimNames.Aud).ToList();

            // Cria payload com dados do JWT
            JwtPayload payload = new JwtPayload(Issuer, Audience, claimsList, null, dataValidade);

            // Chave de segurança da assinatura do jwt
            SecurityKey SigningKey = GetSecurityKey();

            // Gera credenciais para o token
            SigningCredentials signingCredentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            // Cria header do JWT
            JwtHeader header = new JwtHeader(signingCredentials);

            // Monta token JWT em string
            JwtSecurityToken secToken = new JwtSecurityToken(header, payload);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string tokenString = handler.WriteToken(secToken);

            return tokenString;
        }

        public TokenBearerParameters GetTokenBearerParameters()
        {
            return new TokenBearerParameters()
            {
                SigningKey = GetSecurityKey(),
                Audience = Audience,
                Issuer = Issuer
            };
        }

        /// <summary>
        /// Gera uma chave de segurança a partir da chave privada recebida
        /// </summary>
        private static SecurityKey GetSecurityKey()
        {
            // Pega a chave privada que deve ficar somente no servidor e bem protegida
            string privateKey = GetPrivateKey();

            // Gera a chave de segurança a partir da chave privada obtida
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));
        }

        /// <summary>
        /// Retorna uma chave privada para geração de token JWT
        /// </summary>
        private static string GetPrivateKey()
        {
            // O uso da mesma chave sempre permitirá que, mesmo reiniciando a API, tokens gerados anteriormente ainda sejam válidos, a não ser que sejam expirados
            return "2xE6XF154uViVL9fTd5AKt3zlas4DSrX0ta2HmycQW1oZTq7wFj279moYvU70kjCNlj3LYU0Q5q99VZ2m28gp5R6/+nRuD1yKH3qHbAmRQquxb50WoITe0OpyKjRkPwycwrcmAFzCOfTGSu2qwVuF9RbaLeuE8rkImF7xJcSNTM=";

            // Abaixo é uma alternativa em que uma nova chave privada é auto-gerada a cada vez que o sistema é executado e sobe a API, por exemplo (Ou alguma outra condição)
            // Para que um mesmo token JWT não possa ser utilizado em diferentes instalações do sistema, pode ser importante que cada instalação auto-gere sua própria chave privada
            // Essa chave pode ser gerada também em outros momentos que não sejam no início da aplicação para que os usuários que já possuíam tokens não precisem re-autenticar a cada vez que a aplicação é reiniciada
            //return GenerateNewPrivateKey();
        }

        /// <summary>
        /// Gera a nova chave privada gerada
        /// </summary>
        private static string GenerateNewPrivateKey()
        {
            // Cria uma nova chave RSA de 2048 bits
            using var cryptoProvider = new RSACryptoServiceProvider(2048);

            // Exporta os parâmetros da chave criada
            RSAParameters parametersKey = cryptoProvider.ExportParameters(true);

            // Utiliza um dos parâmetros como chave privada
            string privateKey = parametersKey.P != null ? Convert.ToBase64String(parametersKey.P) : string.Empty;

            // Retorna a chave privada gerada
            return privateKey;
        }
    }
}

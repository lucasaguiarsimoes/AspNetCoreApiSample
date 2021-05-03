using AspNetCoreApiSample.Domain;
using AspNetCoreApiSample.Domain.Commands;
using AspNetCoreApiSample.Domain.Enums;
using AspNetCoreApiSample.Domain.Model;
using AspNetCoreApiSample.Domain.Transfer;
using AspNetCoreApiSample.Repository.Interface;
using AspNetCoreApiSample.Service.Interface;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenJwtService _tokenJwtService;

        public AuthenticationService(IUsuarioRepository usuarioRepository, ITokenJwtService tokenJwtService)
        {
            this._usuarioRepository = usuarioRepository;
            this._tokenJwtService = tokenJwtService;
        }

        public async Task<string> LoginAsync(LoginCommand loginCommand, CancellationToken cancellationToken)
        {
            // Não é uma boa prática, mas garante que sempre existirá o usuário padrão
            // Isso seria uma falha de segurança se o banco de dados não fosse InMemory e se fosse um sistema real
            if (!await this._usuarioRepository.AnyAsync(cancellationToken))
            {
                await AddFirstUsuario(cancellationToken);
            }

            // Tenta encontrar o usuário digitado como código
            Usuario? usuario = await this._usuarioRepository.GetByCodigoAsync(loginCommand.Usuario, cancellationToken);

            // Se não encontrou, verifica se o usuário foi digitado como email
            if (usuario == null)
            {
                usuario = await this._usuarioRepository.GetByEmailAsync(loginCommand.Usuario, cancellationToken);
            }

            // Se mesmo assim não encontrou o usuário, não há o que fazer
            if (usuario == null)
            {
                throw new UnauthorizedAccessException("Usuário ou senha inválidos.");
            }

            // Verifica se a senha digitada bate com o do usuário encontrado
            if (usuario.Senha != this.EncryptPassword(loginCommand.Senha))
            {
                throw new UnauthorizedAccessException("Usuário ou senha inválidos.");
            }

            // Se chegou aqui, a autenticação ocorreu com sucesso. Portanto, pode gerar o token de autenticação
            // Cria uma lista de claims para associar à identidade do usuário autenticado no token
            IEnumerable<Claim> claimsUsuario = GetAuthenticatedUserClaims(usuario);

            // Gera e retorna o token JWT
            return this._tokenJwtService.CreateToken(claimsUsuario);
        }

        /// <summary>
        /// Monta todas as claims do token do usuário autenticado
        /// </summary>
        private IEnumerable<Claim> GetAuthenticatedUserClaims(Usuario usuario)
        {
            // Inclui o claim que representa o dono do token
            yield return new Claim(JwtRegisteredClaimNames.Sub, usuario.Nome);

            // Inclui um guid para representar o identificador único deste token
            yield return new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString().ToUpper());

            // Varre todas as permissões do usuário em questão
            foreach (UsuarioPermissao usuarioPermissao in usuario.Permissoes)
            {
                // Inclui cada permissão do usuário no token
                yield return new Claim(Constants.ROLE_CLAIM_TYPE, ((int)usuarioPermissao.Permissao).ToString());
            }
        }

        public string EncryptPassword(string password)
        {
            // Define uma chave privada fixa para criptografar a senha (Para fortalecer a segurança, cada usuário pode ter seu próprio hash auto-gerado também)
            string hash = "66B67248-9225-4325-B425-99A4EFEFE129";

            // Concatena a senha com mais algumas informações
            string passwordPlus = password + hash + password.Length;

            // Criptografa o texto definido
            string passwordSHA256 = CryptographySHA256(passwordPlus);

            // Segunda iteração para aumentar a complexidade do hash da senha
            passwordSHA256 = CryptographySHA256(passwordSHA256 + hash + passwordSHA256.Length);

            // Retorna a senha calculadas
            return passwordSHA256;
        }

        /// <summary>
        /// Aplica a criptografia SHA256 utilizando uma string
        /// </summary>
        private string CryptographySHA256(string texto)
        {
            StringBuilder builder = new StringBuilder();

            // Instancia o objeto para criptografar o valor recebido
            using (SHA256 objSHA = SHA256.Create())
            {
                // Utiliza o encoding UTF8 para obter byte a byte do valor recebido
                Encoding objEncoding = Encoding.UTF8;
                byte[] hash = objSHA.ComputeHash(objEncoding.GetBytes(texto));

                // Converte cada byte obtido para hexadecimal (ToString("x2") = 2 caracteres Hexadecimais uppercase para cada byte)
                foreach (byte b in hash)
                {
                    builder.Append(b.ToString("x2"));
                }
            }

            return builder.ToString();
        }

        private async Task AddFirstUsuario(CancellationToken cancellationToken)
        {
            // Cria um usuário inicial do sistema
            Usuario firstUsuario = new Usuario()
            {
                Codigo = "admin",
                Nome = "Administrador",
                Email = "admin@sample.com",
                Permissoes = Enum.GetValues(typeof(PermissaoSistemaEnum)).Cast<PermissaoSistemaEnum>().Select(p => new UsuarioPermissao() { Permissao = p }).ToList(),
                Senha = this.EncryptPassword("admin")
            };

            // Persiste o primeiro usuário do sistema
            await this._usuarioRepository.AddAsync(firstUsuario, cancellationToken);
        }
    }
}

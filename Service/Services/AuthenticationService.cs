using AspNetCoreApiSample.Domain;
using AspNetCoreApiSample.Domain.Commands;
using AspNetCoreApiSample.Domain.Enums;
using AspNetCoreApiSample.Domain.Model;
using AspNetCoreApiSample.Domain.Transfer;
using AspNetCoreApiSample.Repository;
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
        private readonly IUnitOfWork _uow;
        private readonly IUsuarioService _usuarioService;
        private readonly ITokenJwtService _tokenJwtService;
        private readonly ICryptographyService _cryptographyService;

        public AuthenticationService(IUnitOfWork uow, IUsuarioService usuarioService, ITokenJwtService tokenJwtService, ICryptographyService cryptographyService)
        {
            this._uow = uow;
            this._usuarioService = usuarioService;
            this._tokenJwtService = tokenJwtService;
            this._cryptographyService = cryptographyService;
        }

        public async Task<string> LoginAsync(LoginCommand loginCommand, CancellationToken cancellationToken)
        {
            // Não é uma boa prática, mas garante que sempre existirá o usuário padrão
            // Isso seria uma potencial falha de segurança em um sistema real
            if (!await this._uow.UsuarioRepository.AnyAsync(cancellationToken))
            {
                await AddFirstUsuario(cancellationToken);
            }

            // Tenta encontrar o usuário digitado como código
            Usuario? usuario = await this._uow.UsuarioRepository.GetByCodigoAsync(loginCommand.Usuario, cancellationToken);

            // Se não encontrou, verifica se o usuário foi digitado como email
            if (usuario == null)
            {
                usuario = await this._uow.UsuarioRepository.GetByEmailAsync(loginCommand.Usuario, cancellationToken);
            }

            // Se mesmo assim não encontrou o usuário, não há o que fazer
            if (usuario == null)
            {
                throw new UnauthorizedAccessException("Usuário ou senha inválidos.");
            }

            // Verifica se a senha digitada bate com o do usuário encontrado
            if (usuario.Senha != this._cryptographyService.Encrypt(loginCommand.Senha))
            {
                throw new UnauthorizedAccessException("Usuário ou senha inválidos.");
            }

            // Verifica se a senha do usuário está expirada (Caso a expiração esteja habilitada e caso o usuário fique mais de 3 meses sem trocar sua senha)
            if (usuario.ExpiracaoSenhaAtivada && usuario.DataHoraUltimaAlteracaoSenha.AddMonths(3) < DateTime.Now)
            {
                throw new UnauthorizedAccessException("Senha expirada. Peça ao administrador do sistema para redefinir sua senha.");
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

        private async Task AddFirstUsuario(CancellationToken cancellationToken)
        {
            // Cria um comando para a criação automática do primeiro usuário do sistema
            UsuarioCommandAdd command = new UsuarioCommandAdd()
            {
                Codigo = "admin",
                Nome = "Administrador",
                Email = "admin@sample.com",
                Permissoes = Enum.GetValues(typeof(PermissaoSistemaEnum)).Cast<PermissaoSistemaEnum>().ToList(),
                Senha = "admin"
            };

            // Inclui e persiste o primeiro usuário do sistema
            await this._usuarioService.AddAsync(command, cancellationToken);
        }
    }
}

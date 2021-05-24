using AspNetCoreApiSample.Domain.Commands;
using AspNetCoreApiSample.Domain.Enums;
using AspNetCoreApiSample.Domain.Model;
using AspNetCoreApiSample.Repository;
using AspNetCoreApiSample.Repository.Interface;
using AspNetCoreApiSample.Service.Interface;
using AspNetCoreApiSample.Service.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Service.Tests
{
    public class AuthenticationServiceTests
    {
        public class LoginAsync
        {
            // Usuário montado para utilização nos testes
            private readonly Usuario _usuarioDefault = new Usuario()
            {
                ID = 1,
                Codigo = "admin",
                Nome = "Administrador",
                Email = "admin@sample.com",
                Senha = "admin",
                ExpiracaoSenhaAtivada = true,
                DataHoraUltimaAlteracaoSenha = DateTime.Now,
                Permissoes = new List<UsuarioPermissao>()
                {
                    new UsuarioPermissao() { ID = 1, UsuarioID = 1, Permissao = PermissaoSistemaEnum.UsuarioConsulta },
                    new UsuarioPermissao() { ID = 2, UsuarioID = 1, Permissao = PermissaoSistemaEnum.UsuarioCria },
                    new UsuarioPermissao() { ID = 3, UsuarioID = 1, Permissao = PermissaoSistemaEnum.UsuarioEdita },
                    new UsuarioPermissao() { ID = 4, UsuarioID = 1, Permissao = PermissaoSistemaEnum.UsuarioRemove },
                },
            };

            [Fact]
            public async Task ExpiracaoSenhaAtiva_1MonthLastPasswordChange_Success()
            {
                // Arrange: Prepara os inputs do método a ser testado
                CancellationToken cancellationToken = new CancellationToken();
                LoginCommand loginCommand = new LoginCommand()
                {
                    Usuario = "admin",
                    Senha = "admin",
                };

                // Arrange: Prepara o Seed do usuário válido
                Usuario usuarioValido = this._usuarioDefault;
                usuarioValido.DataHoraUltimaAlteracaoSenha = DateTime.Now.AddMonths(-1);
                string tokenMock = "tokenMock";

                // Arrange: Cria os Substitutes das dependências 
                ICryptographyService cryptographyService = Substitute.For<ICryptographyService>();
                ITokenJwtService tokenJwtService = Substitute.For<ITokenJwtService>();
                IUsuarioService usuarioService = Substitute.For<IUsuarioService>();
                IUnitOfWork uow = Substitute.For<IUnitOfWork>();
                IUsuarioRepository usuarioRepository = Substitute.For<IUsuarioRepository>();

                // Arrange: Prepara os retornos para os acessos às dependências para provocar o cenário desejado
                usuarioRepository.AnyAsync(cancellationToken).Returns(true);
                usuarioRepository.GetByCodigoAsync(usuarioValido.Codigo, cancellationToken).Returns(usuarioValido);
                cryptographyService.Encrypt("admin").Returns("admin");
                tokenJwtService.CreateToken(null).ReturnsForAnyArgs(tokenMock);
                uow.UsuarioRepository.Returns(usuarioRepository);

                // Arrange: Prepara a classe a ser testada
                AuthenticationService service = new AuthenticationService(uow, usuarioService, tokenJwtService, cryptographyService);

                // Act
                string tokenResponse = await service.LoginAsync(loginCommand, cancellationToken);

                // Assert
                Assert.Equal(tokenMock, tokenResponse);
            }

            [Fact]
            public async Task ExpiracaoSenhaAtiva_3Month1DayLastPasswordChange_Unauthorized()
            {
                // Arrange: Prepara os inputs do método a ser testado
                CancellationToken cancellationToken = new CancellationToken();
                LoginCommand loginCommand = new LoginCommand()
                {
                    Usuario = "admin",
                    Senha = "admin",
                };

                // Arrange: Prepara o Seed do usuário válido
                Usuario usuarioValido = this._usuarioDefault;
                usuarioValido.DataHoraUltimaAlteracaoSenha = DateTime.Now.AddMonths(-3).AddDays(-1);
                string tokenMock = "tokenMock";

                // Arrange: Cria os Substitutes das dependências 
                ICryptographyService cryptographyService = Substitute.For<ICryptographyService>();
                ITokenJwtService tokenJwtService = Substitute.For<ITokenJwtService>();
                IUsuarioService usuarioService = Substitute.For<IUsuarioService>();
                IUnitOfWork uow = Substitute.For<IUnitOfWork>();
                IUsuarioRepository usuarioRepository = Substitute.For<IUsuarioRepository>();

                // Arrange: Prepara os retornos para os acessos às dependências para provocar o cenário desejado
                usuarioRepository.AnyAsync(cancellationToken).Returns(true);
                usuarioRepository.GetByCodigoAsync(usuarioValido.Codigo, cancellationToken).Returns(usuarioValido);
                cryptographyService.Encrypt("admin").Returns("admin");
                tokenJwtService.CreateToken(null).ReturnsForAnyArgs(tokenMock);
                uow.UsuarioRepository.Returns(usuarioRepository);

                // Arrange: Prepara a classe a ser testada
                AuthenticationService service = new AuthenticationService(uow, usuarioService, tokenJwtService, cryptographyService);

                // Act -> Assert
                await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.LoginAsync(loginCommand, cancellationToken));
            }
        }
    }
}

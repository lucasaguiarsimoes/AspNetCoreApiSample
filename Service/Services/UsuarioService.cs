using AspNetCoreApiSample.Domain.Commands;
using AspNetCoreApiSample.Domain.Enums;
using AspNetCoreApiSample.Domain.Model;
using AspNetCoreApiSample.Domain.Queries;
using AspNetCoreApiSample.Domain.QueryResponses;
using AspNetCoreApiSample.Repository.Interface;
using AspNetCoreApiSample.Service.Interface;
using Matrix.QC.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Service.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IAuthenticationService authenticationService, IUsuarioRepository usuarioRepository)
        {
            this._authenticationService = authenticationService;
            this._usuarioRepository = usuarioRepository;
        }

        public async Task<long> AddAsync(UsuarioCommandAdd command, CancellationToken cancellationToken)
        {
            Usuario usuario = new Usuario()
            {
                Codigo = command.Codigo,
                Nome = command.Nome,
                Email = command.Email,
                Senha = this._authenticationService.EncryptPassword(command.Senha),
                Permissoes = command.Permissoes.Select(p => new UsuarioPermissao() { Permissao = p }),
            };

            await this._usuarioRepository.AddAsync(usuario, cancellationToken);

            return usuario.ID;
        }

        public async Task EditAsync(UsuarioCommandEdit command, CancellationToken cancellationToken)
        {
            Usuario? usuario = await this._usuarioRepository.GetByIdAsync(command.ID, cancellationToken);

            if (usuario == null)
            {
                throw new EntityNotFoundException("Usuário não encontrado.");
            }

            usuario.Codigo = command.Codigo;
            usuario.Nome = command.Nome;
            usuario.Email = command.Email;
            usuario.Permissoes = command.Permissoes.Select(p => new UsuarioPermissao() { Permissao = p });

            // Se foi solicitada a troca da senha, criptografa a nova senha
            if (!string.IsNullOrWhiteSpace(command.Senha))
            {
                usuario.Senha = this._authenticationService.EncryptPassword(command.Senha);
            }

            await this._usuarioRepository.UpdateAsync(usuario, cancellationToken);
        }

        public async Task RemoveAsync(UsuarioCommandRemove command, CancellationToken cancellationToken)
        {
            Usuario? usuario = await this._usuarioRepository.GetByIdAsync(command.ID, cancellationToken);

            if (usuario == null)
            {
                throw new EntityNotFoundException("Usuário não encontrado.");
            }

            await this._usuarioRepository.RemoveAsync(usuario, cancellationToken);
        }

        public async Task<Usuario?> GetByIdAsync(UsuarioQueryGetById usuarioQueryGetById, CancellationToken cancellationToken)
        {
            return await this._usuarioRepository.GetByIdAsync(usuarioQueryGetById.ID, cancellationToken);
        }

        public async Task<IEnumerable<UsuarioQueryResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await this._usuarioRepository.GetAllAsync(cancellationToken);
        }
    }
}

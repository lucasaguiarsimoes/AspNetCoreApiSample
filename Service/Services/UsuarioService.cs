using AspNetCoreApiSample.Domain.Commands;
using AspNetCoreApiSample.Domain.Enums;
using AspNetCoreApiSample.Domain.Exceptions;
using AspNetCoreApiSample.Domain.Model;
using AspNetCoreApiSample.Domain.Queries;
using AspNetCoreApiSample.Domain.QueryResponses;
using AspNetCoreApiSample.Repository;
using AspNetCoreApiSample.Repository.Interface;
using AspNetCoreApiSample.Service.Interface;
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
        private readonly IUnitOfWork _uow;
        private readonly ICryptographyService _cryptographyService;

        public UsuarioService(IUnitOfWork uow, ICryptographyService cryptographyService)
        {
            this._uow = uow;
            this._cryptographyService = cryptographyService;
        }

        public async Task<long> AddAsync(UsuarioCommandAdd command, CancellationToken cancellationToken)
        {
            // Tenta carregar o usuário solicitado do banco de dados para verificar se ele já existe, já que o código de usuário deve ser único no sistema
            Usuario? usuarioExistente = await this._uow.UsuarioRepository.GetByCodigoAsync(command.Codigo, cancellationToken);

            // Dispara falha caso já exista um usuário com o código fornecido
            if (usuarioExistente != null)
            {
                throw new EntityDuplicatedException("Já existe um usuário com o código informado.");
            }

            // Cria o novo usuário com os dados fornecidos
            Usuario usuario = new Usuario()
            {
                Codigo = command.Codigo,
                Nome = command.Nome,
                Email = command.Email,
                Senha = this._cryptographyService.Encrypt(command.Senha),
                Permissoes = command.Permissoes.Select(p => new UsuarioPermissao() { Permissao = p }).ToList(),
            };

            // Aciona a inclusão do usuário no banco de dados
            await this._uow.UsuarioRepository.AddAsync(usuario, cancellationToken);

            // Aplica as alterações no banco de dados
            await this._uow.ApplyChangesAsync(cancellationToken);

            // Retorna o ID do novo usuário criado
            return usuario.ID;
        }

        public async Task EditAsync(UsuarioCommandEdit command, CancellationToken cancellationToken)
        {
            // Carrega o usuário solicitado do banco de dados
            Usuario? usuario = await this._uow.UsuarioRepository.GetByIdAsync(command.ID, cancellationToken);

            // Dispara falha caso o usuário não tenha sido encontrado
            if (usuario == null)
            {
                throw new EntityNotFoundException("Usuário não encontrado.");
            }

            // Atualiza os dados do usuário a partir dos dados recebidos
            usuario.Codigo = command.Codigo;
            usuario.Nome = command.Nome;
            usuario.Email = command.Email;
            usuario.Permissoes = command.Permissoes.Select(p => new UsuarioPermissao() { Permissao = p }).ToList();

            // Se foi solicitada a troca da senha, criptografa a nova senha
            if (!string.IsNullOrWhiteSpace(command.Senha))
            {
                usuario.Senha = this._cryptographyService.Encrypt(command.Senha);
            }

            // Aciona a atualização dos dados do usuário
            this._uow.UsuarioRepository.Update(usuario);

            // Aplica as alterações no banco de dados
            await this._uow.ApplyChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(UsuarioCommandRemove command, CancellationToken cancellationToken)
        {
            // Carrega o usuário solicitado do banco de dados
            Usuario? usuario = await this._uow.UsuarioRepository.GetByIdAsync(command.ID, cancellationToken);

            // Dispara falha caso o usuário não tenha sido encontrado
            if (usuario == null)
            {
                throw new EntityNotFoundException("Usuário não encontrado.");
            }
            
            // Aciona a exclusão do usuário encontrado
            this._uow.UsuarioRepository.Remove(usuario);

            // Aplica as alterações no banco de dados
            await this._uow.ApplyChangesAsync(cancellationToken);
        }

        public async Task<Usuario?> GetByIdAsync(UsuarioQueryGetById usuarioQueryGetById, CancellationToken cancellationToken)
        {
            return await this._uow.UsuarioRepository.GetByIdAsync(usuarioQueryGetById.ID, cancellationToken);
        }

        public async Task<IEnumerable<UsuarioQueryResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await this._uow.UsuarioRepository.GetAllAsync(cancellationToken);
        }
    }
}

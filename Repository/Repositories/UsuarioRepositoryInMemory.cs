using AspNetCoreApiSample.Domain.Enums;
using AspNetCoreApiSample.Domain.Model;
using AspNetCoreApiSample.Domain.QueryResponses;
using AspNetCoreApiSample.Repository.Interface;
using Matrix.QC.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Repository.Repositories
{
    public class UsuarioRepositoryInMemory : IUsuarioRepository
    {
        /// <summary>
        /// Simulação de uma tabela do banco de dados
        /// </summary>
        private readonly List<Usuario> _tabelaUsuariosInMemory = new List<Usuario>();

        public async Task<long> AddAsync(Usuario usuario, CancellationToken cancellationToken)
        {
            if (this._tabelaUsuariosInMemory.Contains(usuario))
            {
                throw new EntityDuplicatedException("O usuário informado já existe.");
            }

            if (this._tabelaUsuariosInMemory.Any(u => u.Codigo == usuario.Codigo))
            {
                throw new EntityUniqueViolatedException("Já existe um outro usuário com o código informado.");
            }

            if (this._tabelaUsuariosInMemory.Any(u => u.Email == usuario.Email))
            {
                throw new EntityUniqueViolatedException("Já existe um outro usuário com o e-mail informado.");
            }
            
            // Define o próximo ID para o usuário
            lock (this._tabelaUsuariosInMemory)
            {
                usuario.ID = !this._tabelaUsuariosInMemory.Any()
                    ? 1
                    : this._tabelaUsuariosInMemory.Max(u => u.ID) + 1;
            }

            this._tabelaUsuariosInMemory.Add(usuario);

            return await Task.FromResult(usuario.ID);
        }

        public async Task UpdateAsync(Usuario usuario, CancellationToken cancellationToken)
        {
            Usuario? usuarioBanco = this._tabelaUsuariosInMemory.FirstOrDefault(u => u.ID == usuario.ID);

            if (usuarioBanco == null)
            {
                throw new EntityNotFoundException("O usuário informado não existe.");
            }

            if (!this._tabelaUsuariosInMemory.Contains(usuario))
            {
                throw new EntityNotFoundException("Não foi utilizada a referência de objeto interna da entidade para edição, por isso a edição não foi realizada.");
            }

            // Por ser um banco 'InMemory', o usuário informado já vai ter sido atualizado
            await Task.CompletedTask;
        }

        public async Task RemoveAsync(Usuario usuario, CancellationToken cancellationToken)
        {
            Usuario? usuarioBanco = this._tabelaUsuariosInMemory.FirstOrDefault(u => u.ID == usuario.ID);

            if (usuarioBanco == null)
            {
                throw new EntityDuplicatedException("O usuário informado não existe.");
            }

            this._tabelaUsuariosInMemory.Remove(usuarioBanco);

            await Task.CompletedTask;
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken)
        {
            return await Task.FromResult(this._tabelaUsuariosInMemory.Any());
        }

        public async Task<Usuario?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await Task.FromResult(this._tabelaUsuariosInMemory.FirstOrDefault(u => u.ID == id));
        }

        public async Task<Usuario?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken)
        {
            return await Task.FromResult(this._tabelaUsuariosInMemory.FirstOrDefault(u => u.Codigo.ToLower() == codigo.ToLower()));
        }

        public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await Task.FromResult(this._tabelaUsuariosInMemory.FirstOrDefault(u => u.Email.ToLower() == email.ToLower()));
        }

        public async Task<IEnumerable<UsuarioQueryResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await Task.FromResult(this._tabelaUsuariosInMemory.Select(u => new UsuarioQueryResponse()
            {
                ID = u.ID,
                Codigo = u.Codigo,
                Nome = u.Nome,
                Email = u.Email,
                Permissoes = u.Permissoes.Select(p => p.Permissao)
            }));
        }
    }
}

using AspNetCoreApiSample.Domain.Model;
using AspNetCoreApiSample.Domain.QueryResponses;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Repository.Interface
{
    public interface IUsuarioRepository
    {
        /// <summary>
        /// Inclui um novo usuário no sistema e retorna o ID gerado
        /// </summary>
        Task<long> AddAsync(Usuario usuario, CancellationToken cancellationToken);

        /// <summary>
        /// Atualiza um usuário existente no sistema
        /// </summary>
        Task UpdateAsync(Usuario usuario, CancellationToken cancellationToken);

        /// <summary>
        /// Remove um usuário existente do sistema
        /// </summary>
        Task RemoveAsync(Usuario usuario, CancellationToken cancellationToken);

        /// <summary>
        /// Verifica se existe algum usuário persistido no sistema
        /// </summary>
        Task<bool> AnyAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Carrega um usuário através do ID
        /// </summary>
        Task<Usuario?> GetByIdAsync(long id, CancellationToken cancellationToken);

        /// <summary>
        /// Carrega um usuário através do Código
        /// </summary>
        Task<Usuario?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken);

        /// <summary>
        /// Carrega um usuário através do Email
        /// </summary>
        Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken);

        /// <summary>
        /// Retorna todos os usuários existentes no sistema
        /// </summary>
        Task<IEnumerable<UsuarioQueryResponse>> GetAllAsync(CancellationToken cancellationToken);
    }
}

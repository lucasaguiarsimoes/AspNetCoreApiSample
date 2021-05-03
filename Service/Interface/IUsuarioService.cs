using AspNetCoreApiSample.Domain.Commands;
using AspNetCoreApiSample.Domain.Model;
using AspNetCoreApiSample.Domain.Queries;
using AspNetCoreApiSample.Domain.QueryResponses;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Service.Interface
{
    public interface IUsuarioService
    {
        /// <summary>
        /// Inclui um usuário no sistema e retorna o ID gerado
        /// </summary>
        Task<long> AddAsync(UsuarioCommandAdd command, CancellationToken cancellationToken);

        /// <summary>
        /// Edita um usuário do sistema
        /// </summary>
        Task EditAsync(UsuarioCommandEdit command, CancellationToken cancellationToken);

        /// <summary>
        /// Remove um usuário do sistema
        /// </summary>
        Task RemoveAsync(UsuarioCommandRemove command, CancellationToken cancellationToken);

        /// <summary>
        /// Carrega um usuário específico através do seu ID
        /// </summary>
        Task<Usuario?> GetByIdAsync(UsuarioQueryGetById usuarioQueryGetById, CancellationToken cancellationToken);

        /// <summary>
        /// Carrega todos os usuários do sistema
        /// </summary>
        Task<IEnumerable<UsuarioQueryResponse>> GetAllAsync(CancellationToken cancellationToken);
    }
}

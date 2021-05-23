using AspNetCoreApiSample.Domain.Model;
using AspNetCoreApiSample.Domain.QueryResponses;
using AspNetCoreApiSample.Repository.Interface.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Repository.Interface
{
    public interface IUsuarioRepository :
        ICommandRepository<Usuario>,
        IQueryRepository<Usuario>
    {
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

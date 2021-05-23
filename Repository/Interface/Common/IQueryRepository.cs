using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Repository.Interface.Common
{
    /// <summary>
    /// Interface de repository dedicada a consultas ao banco de dados
    /// </summary>
    public interface IQueryRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Verifica se existe algum item da entidade persistido no sistema
        /// </summary>
        Task<bool> AnyAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Carrega uma entidade através do seu ID
        /// </summary>
        Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Repository.Interface.Common
{
    /// <summary>
    /// Interface de repository dedicada a comandos ao banco de dados
    /// </summary>
    public interface ICommandRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Inclui uma nova entidade no sistema
        /// </summary>
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Atualiza uma entidade existente no sistema
        /// </summary>
        void Update(TEntity entity);

        /// <summary>
        /// Remove uma entidade existente do sistema
        /// </summary>
        void Remove(TEntity entity);
    }
}

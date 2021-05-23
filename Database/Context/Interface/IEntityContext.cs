using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Database.Context.Interface
{
    /// <summary>
    /// Interface que representa um contexto de entidades com acesso à base de dados
    /// </summary>
    public interface IEntityContext
    {
        /// <summary>
        /// Abre e usa conexão com a base de dados neste contexto de entidades
        /// </summary>
        DbConnection UseConnection();

        /// <summary>
        /// Aciona a execução de todas as migrations pendentes para o contexto em questão
        /// </summary>
        Task MigrateAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Aplica as mudanças registradas nas entidades para o banco de dados
        /// </summary>
        Task<int> ApplyChangesAsync(CancellationToken cancellationToken);
    }
}

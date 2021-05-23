using AspNetCoreApiSample.Repository.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Repository
{
    /// <summary>
    /// Interface para representar a unidade de trabalho para centralizar o acesso à base de dados e os repositórios das entidades do sistema
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Aciona migrations para a base de dados em questão
        /// </summary>
        Task MigrateAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Aplica as mudanças realizadas nos objetos para o banco de dados
        /// </summary>
        Task<int> ApplyChangesAsync(CancellationToken cancellationToken);

        // Repositórios do sistema
        IUsuarioRepository UsuarioRepository { get; }
        IUsuarioPermissaoRepository UsuarioPermissaoRepository { get; }
    }
}

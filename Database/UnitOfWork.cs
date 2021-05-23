using AspNetCoreApiSample.Database.Context.Interface;
using AspNetCoreApiSample.Repository;
using AspNetCoreApiSample.Repository.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Provider de serviços do sistema para solicitar o repository específico solicitado
        /// </summary>
        private readonly IServiceProvider _serviceProvider;
        private readonly IEntityContext _entityContext;

        // Repositórios do sistema
        public IUsuarioRepository UsuarioRepository => this._serviceProvider.GetRequiredService<IUsuarioRepository>();
        public IUsuarioPermissaoRepository UsuarioPermissaoRepository => this._serviceProvider.GetRequiredService<IUsuarioPermissaoRepository>();

        public UnitOfWork(IServiceProvider serviceProvider, IEntityContext entityContext)
        {
            this._serviceProvider = serviceProvider;
            this._entityContext = entityContext;
        }

        public Task<int> ApplyChangesAsync(CancellationToken cancellationToken)
        {
            return this._entityContext.ApplyChangesAsync(cancellationToken);
        }

        public async Task MigrateAsync(CancellationToken cancellationToken)
        {
            await this._entityContext.MigrateAsync(cancellationToken);
        }
    }
}

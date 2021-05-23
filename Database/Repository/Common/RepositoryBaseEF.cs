using AspNetCoreApiSample.Database.Context.Interface;
using AspNetCoreApiSample.Repository.Interface.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Database.Repository.Common
{
    public class RepositoryBaseEF<TEntity> :
        ICommandRepository<TEntity>,
        IQueryRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Objeto de contexto para alterações ou buscas
        /// </summary>
        protected DbContext DbContextEF => (DbContext)this._entityContext;
        private readonly IEntityContext _entityContext;

        /// <summary>
        /// Objeto para aplicação de alterações e buscas
        /// </summary>
        protected DbSet<TEntity> DbSet => GetDbSet();
        private DbSet<TEntity>? _DbSet = null;

        public RepositoryBaseEF(IEntityContext contexto)
        {
            this._entityContext = contexto;
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await this.DbSet.AddAsync(entity, cancellationToken);
        }

        public void Update(TEntity entity)
        {
            this.DbSet.Update(entity);
        }

        public void Remove(TEntity entity)
        {
            this.DbSet.Remove(entity);
        }

        public async Task<bool> AnyAsync(CancellationToken cancellationToken)
        {
            return await this.DbSet.AnyAsync(cancellationToken);
        }

        public virtual async Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await this.DbSet.FindAsync(new object[] { id! }, cancellationToken);
        }

        /// <summary>
        /// Prepara o DbSet para alterações e buscas através do contexto
        /// </summary>
        private DbSet<TEntity> GetDbSet()
        {
            if (this._DbSet == null)
            {
                // Prepara o uso da conexão para que ela se mantenha aberta durante o escopo de uso
                this._entityContext.UseConnection();

                // Prepara o objeto para acesso ao context
                this._DbSet = this.DbContextEF.Set<TEntity>();
            }

            return this._DbSet;
        }
    }
}

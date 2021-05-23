using AspNetCoreApiSample.Database.Connection.Interface;
using AspNetCoreApiSample.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Database.Connection
{
    /// <summary>
    /// Classe que gerencia e representa uma conexão baseada em um DbConnection
    /// </summary>
    public abstract class ConnectionBase : IConnection
    {
        /// <summary>
        /// Aplica a conexão no DbContext do EntityFramework
        /// </summary>
        public abstract void UseDatabaseEF(DbContextOptionsBuilder optionsBuilder);

        /// <summary>
        /// Objeto de conexão com o banco de dados (SQLServer, PostgreSQL, Odbc, etc)
        /// </summary>
        public abstract DbConnection DbConnection { get; }

        /// <summary>
        /// Definição da string de conexão para o banco de dados
        /// </summary>
        protected string ConnectionString { get; }

        public ConnectionBase(string? connectionString)
        {
            this.ConnectionString = GetConnectionString(connectionString);
        }

        /// <summary>
        /// Obtém a string de conexão com o banco de dados em uso
        /// </summary>
        protected virtual string GetConnectionString(string? connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InaccessibleDatabaseException("Nenhuma connection string informada para acesso ao banco de dados.");
            }

            return connectionString;
        }

        #region IDbQCConnection
        public virtual bool IsConnectionOpen()
        {
            return this.DbConnection.State != ConnectionState.Closed;
        }

        public virtual bool Open()
        {
            this.DbConnection.Open();
            return this.DbConnection.State == ConnectionState.Open;
        }

        public virtual bool Close()
        {
            this.DbConnection.Close();
            return this.DbConnection.State == ConnectionState.Closed;
        }

        public virtual void Dispose()
        {
            this.DbConnection?.Dispose();
        }
        #endregion
    }
}

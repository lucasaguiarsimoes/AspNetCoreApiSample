using AspNetCoreApiSample.Database.Connection.Interface;
using Microsoft.Data.SqlClient;
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
    /// Definição de uma conexão padrão para o banco SQLServer
    /// </summary>
    public class SQLServerConnection : ConnectionBase
    {
        /// <summary>
        /// Objeto de conexão para acesso ao banco
        /// </summary>
        public override DbConnection DbConnection => this._Connection;
        private readonly SqlConnection _Connection;

        public SQLServerConnection(string? connectionString)
            : base(connectionString)
        {
            this._Connection = new SqlConnection(this.ConnectionString);
        }

        public override void UseDatabaseEF(DbContextOptionsBuilder optionsBuilder)
        {
            // Aplica o acesso do EF Core ao SQLServer
            optionsBuilder.UseSqlServer(this._Connection);
        }
    }
}

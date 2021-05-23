using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Database.Connection.Interface
{
    /// <summary>
    /// Interface para o gerenciamento da conexão com o banco de dados
    /// </summary>
    public interface IConnection : IDisposable
    {
        /// <summary>
        /// Aplica a conexão no DbContext do EntityFramework
        /// </summary>
        void UseDatabaseEF(DbContextOptionsBuilder optionsBuilder);

        /// <summary>
        /// Objeto de conexão com o banco de dados (SQLServer, PostgreSQL, Odbc, etc)
        /// </summary>
        DbConnection DbConnection { get; }

        /// <summary>
        /// Verifica se a conexão com o banco está com status aberto
        /// </summary>
        bool IsConnectionOpen();

        /// <summary>
        /// Solicita a abertura da conexão com o banco e retorna se houve sucesso
        /// </summary>
        bool Open();

        /// <summary>
        /// Solicita o encerramento da conexão com o banco e retorna se houve sucesso
        /// </summary>
        bool Close();
    }
}

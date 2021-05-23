using AspNetCoreApiSample.Database.Configuration;
using AspNetCoreApiSample.Database.Connection.Interface;
using AspNetCoreApiSample.Database.Context.Interface;
using AspNetCoreApiSample.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Database.Context
{
    /// <summary>
    /// Contexto de conexões do sistema com logger para produção
    /// </summary>
    public class DefaultDbContext : DbContext, IEntityContext
    {
        /// <summary>
        /// Conexão com o banco de dados
        /// </summary>
        private readonly IConnection? _connection;

        /// <summary>
        /// Factory do logger do sistema
        /// </summary>
        private readonly ILoggerFactory? _logger;

        /// <summary>
        /// Controle para verificar conexão aberta
        /// </summary>
        private bool _isConnectionOpen = false;

        /// <summary>
        /// O construtor vazio é necessário para realizar o Command add-migration em tempo de compilação
        /// </summary>
        public DefaultDbContext()
        {
        }

        public DefaultDbContext(IConnection Conexao, ILoggerFactory logger)
        {
            this._connection = Conexao;
            this._logger = logger;
        }

        #region Overrides EntityFrameworkCore
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Se não tiver conexão, o acionamento foi feito para a criação de migrations em tempo de desenvolvimento
            if (this._connection == null)
            {
                // O objetivo é acionar o provider do EF Core com o banco de dados para gerar os migrations para o banco em questão, no caso o SQLServer
                optionsBuilder.UseSqlServer("INVALID_STRING_FOR_MIGRATIONS_BUILDING");
            }
            else
            {
                // Aplica o uso do DbConnection para o banco de dados especificado
                this._connection.UseDatabaseEF(optionsBuilder);

                // Aplica o uso do logger recebido para o EF Core
                optionsBuilder.UseLoggerFactory(this._logger);
            }

            // Ao descomentar os comandos abaixo, o EF Core prepara um log mais detalhado de Erros
            //optionsBuilder.EnableDetailedErrors();
            //optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Pega o namespace de um dos configurations existentes para servir como exemplo para os demais
            string? configNamespace = typeof(UsuarioConfig).Namespace;

            // Cria e aplica dinamicamente todos os configurations dos models implementados
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), t => t.Namespace == configNamespace);
        }
        #endregion

        #region Implementação IEntityContext
        public DbConnection UseConnection()
        {
            // Não há como utilizar uma conexão que não foi previamente definida
            if (_connection == null)
            {
                throw new Exception("A conexão com o banco de dados foi solicitada, porém não foi previamente definida.");
            }

            // Tenta abrir a conexão apenas se ela ainda não estiver aberta. Isto é, se ainda estiver no primeiro uso
            if (!_isConnectionOpen)
            {
                try
                {
                    // Verifica se algum erro ocorre na tentativa de abrir a conexão com o banco de dados
                    _isConnectionOpen = _connection.Open();
                }
                catch (Exception exc)
                {
                    throw new InaccessibleDatabaseException("Falha na tentativa de abrir a conexão com o banco de dados.", exc);
                }

                // Valida se a conexão foi realmente aberta
                if (!_isConnectionOpen)
                {
                    throw new InaccessibleDatabaseException("Houve uma tentativa de abertura de uma conexão com o banco de dados, porém sem sucesso.");
                }
            }

            // Retorna a conexão em aberto
            return _connection.DbConnection;
        }

        public async Task MigrateAsync(CancellationToken cancellationToken)
        {
            // Atualiza a modelagem do banco de dados aplica os migrations pendentes
            await Database.MigrateAsync(cancellationToken);
        }

        public async Task<int> ApplyChangesAsync(CancellationToken cancellationToken)
        {
            // Realiza o salvamento de todas as mudanças notificadas ao EF Core através de uma transação já gerenciada pelo próprio EF Core
            return await SaveChangesAsync(cancellationToken);
        }
        #endregion
    }
}

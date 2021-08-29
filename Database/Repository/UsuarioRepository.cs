using AspNetCoreApiSample.Database.Context.Interface;
using AspNetCoreApiSample.Database.Repository.Common;
using AspNetCoreApiSample.Domain.Model;
using AspNetCoreApiSample.Domain.Queries;
using AspNetCoreApiSample.Domain.QueryResponses;
using AspNetCoreApiSample.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Database.Repository
{
    public class UsuarioRepository : RepositoryBaseEF<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(IEntityContext contexto) : base(contexto)
        {
        }

        public override async Task<Usuario?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            // O override foi feito para carregar as permissões junto com o usuário
            return await this.DbSet
                .Include(u => u.Permissoes)
                .FirstOrDefaultAsync(u => u.ID == id);
        }

        public async Task<Usuario?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken)
        {
            return await this.DbSet
                .Include(u => u.Permissoes)
                .FirstOrDefaultAsync(u => u.Codigo == codigo);
        }

        public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await this.DbSet
                .Include(u => u.Permissoes)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<UsuarioQueryResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await this.DbSet
                .Select(u => new UsuarioQueryResponse()
                {
                    ID = u.ID,
                    Codigo = u.Codigo,
                    Nome = u.Nome,
                    Email = u.Email,
                    Permissoes = u.Permissoes.Select(p => p.Permissao)
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<UsuarioQueryResponseGetFilteredList>> GetFilteredListAsync(UsuarioQueryGetFilteredList query, CancellationToken cancellationToken)
        {
            return await this.DbSet
                .Where(u =>
                    (string.IsNullOrEmpty(query.Codigo) || u.Codigo.Contains(query.Codigo)) &&
                    (string.IsNullOrEmpty(query.Nome) || u.Nome.Contains(query.Nome)) &&
                    (string.IsNullOrEmpty(query.Email) || u.Email.Contains(query.Email)))
                .Select(u => new UsuarioQueryResponseGetFilteredList()
                {
                    ID = u.ID,
                    Codigo = u.Codigo,
                    Nome = u.Nome,
                    Email = u.Email,
                    ExpiracaoSenhaAtivada = u.ExpiracaoSenhaAtivada,
                })
                .ToListAsync(cancellationToken);
        }
    }
}

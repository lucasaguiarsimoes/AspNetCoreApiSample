using AspNetCoreApiSample.Database.Context.Interface;
using AspNetCoreApiSample.Database.Repository.Common;
using AspNetCoreApiSample.Domain.Model;
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
    public class UsuarioPermissaoRepository : RepositoryBaseEF<UsuarioPermissao>, IUsuarioPermissaoRepository
    {
        public UsuarioPermissaoRepository(IEntityContext contexto) : base(contexto)
        {
        }
    }
}

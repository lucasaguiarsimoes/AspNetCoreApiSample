using AspNetCoreApiSample.Domain.Model;
using AspNetCoreApiSample.Domain.QueryResponses;
using AspNetCoreApiSample.Repository.Interface.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Repository.Interface
{
    public interface IUsuarioPermissaoRepository :
        IQueryRepository<UsuarioPermissao>
    {
    }
}

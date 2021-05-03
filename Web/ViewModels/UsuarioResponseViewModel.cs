using AspNetCoreApiSample.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Web.ViewModels
{
    public class UsuarioResponseViewModel
    {
        public long ID { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;

        public IEnumerable<PermissaoSistemaEnum> Permissoes { get; set; } = null!;
    }
}

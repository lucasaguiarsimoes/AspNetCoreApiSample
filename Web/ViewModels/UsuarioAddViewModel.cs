using AspNetCoreApiSample.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Web.ViewModels
{
    public class UsuarioAddViewModel
    {
        public string Codigo { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Senha { get; set; } = null!;
        public bool ExpiracaoSenhaAtivada { get; set; }

        public List<PermissaoSistemaEnum> Permissoes { get; set; } = null!;
    }
}

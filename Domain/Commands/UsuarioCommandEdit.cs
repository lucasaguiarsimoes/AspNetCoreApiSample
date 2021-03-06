using AspNetCoreApiSample.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Domain.Commands
{
    public class UsuarioCommandEdit
    {
        public long ID { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Senha { get; set; }
        public bool ExpiracaoSenhaAtivada { get; set; }

        public List<PermissaoSistemaEnum> Permissoes { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;

namespace AspNetCoreApiSample.Domain.Model
{
    public class Usuario
    {
        public long ID { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Senha { get; set; } = null!;
        public DateTime DataHoraUltimaAlteracaoSenha { get; set; }
        public bool ExpiracaoSenhaAtivada { get; set; }

        public List<UsuarioPermissao> Permissoes { get; set; } = null!;
    }
}

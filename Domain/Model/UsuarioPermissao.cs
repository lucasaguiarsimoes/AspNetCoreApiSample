using AspNetCoreApiSample.Domain.Enums;
using System;

namespace AspNetCoreApiSample.Domain.Model
{
    public class UsuarioPermissao
    {
        public long ID { get; set; }
        public long UsuarioID { get; set; }
        public PermissaoSistemaEnum Permissao { get; set; }

        public Usuario Usuario { get; set; } = null!;
    }
}

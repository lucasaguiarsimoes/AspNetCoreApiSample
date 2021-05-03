using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Domain.Enums
{
    /// <summary>
    /// Enum de permissões do sistema
    /// </summary>
    public enum PermissaoSistemaEnum
    {
        UsuarioConsulta = 1,
        UsuarioCria = 2,
        UsuarioEdita = 3,
        UsuarioRemove = 4,
    }
}

using AspNetCoreApiSample.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Web.Attributes
{
    /// <summary>
    /// Atributo para facilitar a validação da autenticação junto com roles de permissões específicas
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class AuthorizeWithRoleAttribute : AuthorizeAttribute
    {
        public AuthorizeWithRoleAttribute(params PermissaoSistemaEnum[] permissoes) : base("Bearer")
        {
            // Concatena as permissões exigidas para o acesso
            if (permissoes.Any())
            {
                this.Roles = FormataRoles(permissoes);
            }
        }

        /// <summary>
        /// A partir de um array de PermissaoSistemaEnum retorna valores em formato aceito para utilização na propriedade Role 
        /// </summary>
        private string FormataRoles(PermissaoSistemaEnum[] permissoes)
        {
            // Se o token utilizado possuir pelo menos um dos roles separados por vírgula já é suficiente para permitir o acesso
            return string.Join(",", permissoes.Select(p => (int)p));
        }
    }
}

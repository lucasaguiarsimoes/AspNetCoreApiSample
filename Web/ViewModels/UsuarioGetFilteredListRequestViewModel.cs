using AspNetCoreApiSample.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Web.ViewModels
{
    public class UsuarioGetFilteredListRequestViewModel
    {
        public string? Codigo { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
    }
}

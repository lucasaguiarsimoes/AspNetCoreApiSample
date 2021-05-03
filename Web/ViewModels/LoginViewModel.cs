using AspNetCoreApiSample.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Web.ViewModels
{
    public class LoginViewModel
    {
        public string Usuario { get; set; } = null!;
        public string Senha { get; set; } = null!;
    }
}

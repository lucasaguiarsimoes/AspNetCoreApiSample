using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Domain.Commands
{
    public class LoginCommand
    {
        public string Usuario { get; set; } = null!;
        public string Senha { get; set; } = null!;
    }
}

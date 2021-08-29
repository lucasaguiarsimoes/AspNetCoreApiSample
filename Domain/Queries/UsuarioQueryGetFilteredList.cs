using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreApiSample.Domain.Queries
{
    public class UsuarioQueryGetFilteredList
    {
        public string? Codigo { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
    }
}

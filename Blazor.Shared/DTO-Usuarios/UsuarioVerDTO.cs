using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Shared.DTO_Usuarios
{
    public class UsuarioVerDTO
    {
        public long Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Estado { get; set; }
        public List<string> roles { get; set; }
    }
}

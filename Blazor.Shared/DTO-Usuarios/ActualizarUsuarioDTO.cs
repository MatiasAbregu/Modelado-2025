using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Shared.DTO_Usuarios
{
    public class ActualizarUsuarioDTO
    {
        [Required(ErrorMessage = "Es necesario un ID para actualizar el registro.")]
        public long Id { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Contrasena { get; set; }
        public List<string>? Roles { get; set; }
    }
}

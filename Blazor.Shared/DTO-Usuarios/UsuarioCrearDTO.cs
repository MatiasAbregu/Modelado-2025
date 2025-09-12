using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Shared.DTO_Usuarios
{
    public class UsuarioCrearDTO
    {
        [Required(ErrorMessage = "El usuario debe contar con un nombre de usuario obligatoriamente.")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El campo de la contraseña es obligatorio.")]
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "Al menos debe tener un rol el usuario.")]
        public List<string> Roles { get; set; }
    }
}
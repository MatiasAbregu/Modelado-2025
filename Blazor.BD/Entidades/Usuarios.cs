using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.BD.Entidades
{
    [Index(nameof(NombreUsuario), IsUnique = true)]
    public class Usuarios
    {
        [Key]
        public long Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(5,ErrorMessage = "La contraseña debe poseer mínimo 5 caracteres.")]
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "El estado del usuario es obligatorio. 1 = Activo / 0 = Desactivado")]
        public bool Estado { get; set; } = true;

        public Tokens Token { get; set; }
        public List<UsuariosRoles> Roles { get; set; }
    }
}
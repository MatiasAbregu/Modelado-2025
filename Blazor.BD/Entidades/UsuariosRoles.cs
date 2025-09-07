using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.BD.Entidades
{
    public class UsuariosRoles : IEntityBase<long>
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long UsuarioId { get; set; }
        public Usuarios Usuario { get; set; }

        [Required]
        public Guid RolId { get; set; }
        public Roles Rol { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.BD.Entidades
{
    public class TipoProducto
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(70)")]
        [Required]
        public string Categoria { get; set; }
    }
}

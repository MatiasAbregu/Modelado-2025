using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.BD.Entidades
{
    public class UnidadMedida
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(15)")]
        [Required(ErrorMessage = "Unidad de medida obligatoria.")]
        public string Medida { get; set; }
    }
}
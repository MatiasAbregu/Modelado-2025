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
    [Index(nameof(CodigoBarra), IsUnique = true)]
    public class Productos
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El código de barra es obligatorio.")]
        public string CodigoBarra { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Required(ErrorMessage = "El nombre de producto es obligatorio.")]
        public string NombreProducto { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        public double PrecioUnitario { get; set; }

        [Required(ErrorMessage = "Debe ingresar una cantidad obligatoriamente. Debe ser 0 o más.")]
        public int Cantidad { get; set; }

        public string? Descripcion { get; set; } = null;

        [Required(ErrorMessage = "Debe tener asociado un material.")]
        public int TipoMaterialId { get; set; }
        public TipoMaterial TipoMaterial { get; set; }

        [Required(ErrorMessage = "Debe tener asociado un tipo producto.")]
        public int TipoProductoId { get; set; }
        public TipoProducto TipoProducto { get; set; }

        public List<PropiedadesProducto> Propiedades { get; set; }
    }
}

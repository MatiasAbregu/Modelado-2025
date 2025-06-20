using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Acá se podrá ingresar 50 + el ID de unidad medida KG para mostrar 50KG o 50 + M para hacer referencia a 50 metros
// El usuario podra quitar o agregar todas las propiedades que quiera del producto
namespace Blazor.BD.Entidades
{
    public class PropiedadesProducto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El producto asociado a esta unidad de medida es obligatoria.")]
        public int ProductoId { get; set; }
        public Productos Productos { get; set; }

        [Required(ErrorMessage = "Es obligatoria especificar de qué estamos hablando, ej. color, peso, alto, ancho")]
        public string Caracteristica { get; set; }

        public string? DescripcionCaracteristica { get; set; }

        [Required(ErrorMessage = "Es obligatorio especificar de cuánto estamos hablando, ej. 50kg.")]
        public double? ValorCaracteristica { get; set; }

        [Required(ErrorMessage = "Debe tener asociada una unidad de medida.")]
        public int? UnidadMedidaId { get; set; }
        public UnidadMedida? UnidadMedida { get; set; }

    }
}

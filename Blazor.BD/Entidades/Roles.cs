using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.BD.Entidades
{
    [Index(nameof(Nombre), IsUnique = true)]
    public class Roles : IEntityBase<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        private string _nombre;

        [Required]
        public string Nombre
        {
            get => _nombre; 
            set => _nombre = value?.ToUpperInvariant();
        }

    }
}
using Blazor.BD.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.BD
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        DbSet<Usuario> Usuarios { get; set; }
        DbSet<TipoProducto> TipoProductos { get; set; }
        DbSet<TipoMaterial> TipoMateriales { get; set; }
        DbSet<UnidadMedida> UnidadMedida { get; set; }
        DbSet<Productos> Productos { get; set; }
        DbSet<PropiedadesProducto> PropiedadesProductos { get;set; }
    }
}

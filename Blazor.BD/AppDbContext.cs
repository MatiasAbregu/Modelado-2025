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

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Tokens> Tokens { get; set; }  
        public DbSet<UsuariosRoles> UsuariosRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Roles>()
                .Property(rol => rol.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Usuarios>()
                .HasOne(u => u.Token)
                .WithOne(t => t.Usuario)
                .HasForeignKey<Tokens>(t => t.UserId);

            modelBuilder.Entity<Roles>().HasData(
                new Roles() { Id = new Guid("b74ddd14-6340-4840-95c2-db12554843e5"), Nombre = "Administrador" },
                new Roles() { Id = new Guid("fab4fac1-c546-41de-aebc-a14da6895711"), Nombre = "Usuario" });
        }
    }
}

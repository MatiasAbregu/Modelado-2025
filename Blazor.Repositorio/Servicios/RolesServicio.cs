using Blazor.BD;
using Blazor.BD.Entidades;
using Blazor.Repositorios.Implementaciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Repositorios.Servicios
{
    public class RolesServicio : BaseServicio<Roles, Guid>, IRolesServicio
    {
        private readonly AppDbContext baseDeDatos;
        public RolesServicio(AppDbContext baseDeDatos) : base(baseDeDatos)
        {
            this.baseDeDatos = baseDeDatos;
        }

        public async Task<Roles> BuscarRolPorNombre(string rol)
        {
            rol = rol.ToUpper();
            return await baseDeDatos.Roles.FirstOrDefaultAsync(r => r.Nombre == rol);
        }
    }
}

using Blazor.BD;
using Blazor.BD.Entidades;
using Blazor.Repositorios.Implementaciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Repositorios.Servicios
{
    public class RolesServicio : IRolesServicio
    {
        private readonly AppDbContext baseDeDatos;
        public RolesServicio(AppDbContext baseDeDatos)
        {
            this.baseDeDatos = baseDeDatos;
        }

        public async Task<bool> RolesExisten(List<string> roles)
        {
            roles = roles.Select(r => r.ToUpper().Trim()).ToList();
            var rolesBBDD = await baseDeDatos.Roles.CountAsync(rol => roles.Contains(rol.Nombre));

            return rolesBBDD == roles.Count;
        }

        public async Task<Roles> BuscarRolPorNombre(string rol)
        {
            rol = rol.ToUpper().Trim();
            return await baseDeDatos.Roles.FirstOrDefaultAsync(r => r.Nombre == rol);
        }


        public async Task<List<Roles>> BuscarRolesPorNombres(List<string> roles)
        {
            roles = roles.Select(r => r.ToUpper().Trim()).ToList();
            return await baseDeDatos.Roles.Where(rol => roles.Contains(rol.Nombre)).ToListAsync();
        }
    }
}

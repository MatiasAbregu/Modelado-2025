using Blazor.BD.Entidades;
using Blazor.Repositorios.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Repositorios.Implementaciones
{
    public interface IRolesServicio
    {
        public Task<Roles> BuscarRolPorNombre(string rol);
        public Task<bool> RolesExisten(List<string> roles);
        public Task<List<Roles>> BuscarRolesPorNombres(List<string> roles);
    }
}

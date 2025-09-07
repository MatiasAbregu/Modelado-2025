using Blazor.BD;
using Blazor.BD.Entidades;
using Blazor.Repositorios.Implementaciones;
using Blazor.Shared.DTO_Token;
using Blazor.Shared.DTO_Usuarios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Repositorios.Servicios
{
    public class UsuarioServicio : BaseServicio<Usuarios, long>, IUsuarioServicio
    {
        private readonly AppDbContext baseDeDatos;
        private readonly IRolesServicio rolesServicio;

        public UsuarioServicio(AppDbContext baseDeDatos, IRolesServicio rolesServicio)
            : base(baseDeDatos)
        {
            this.baseDeDatos = baseDeDatos;
            this.rolesServicio = rolesServicio;
        }

        public async Task<Usuarios?> BuscarUsuarioPorNombre(string nombre)
        {
            return await baseDeDatos.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == nombre);
        }

        public async Task<ValueTuple<bool, string, List<UsuarioVerDTO>>> BuscarUsuarios()
        {
            List<UsuarioVerDTO> usuarios = await baseDeDatos
                .Usuarios.Include(u => u.Roles).ThenInclude(ur => ur.Rol)
                .Select(u => new UsuarioVerDTO()
                {
                    Id = u.Id,
                    NombreUsuario = u.NombreUsuario,
                    Estado = u.Estado ? "Activo" : "Desactivado",
                    roles = u.Roles.Select(r => r.Rol.Nombre).ToList()
                }).ToListAsync();

            if (usuarios.Count == 0)
                return (true, "La lista no tiene datos actualmente.", usuarios);
            else if (usuarios.Count > 0)
                return (true, "", usuarios);
            return (false, "Hubo un error al cargar los datos", usuarios);
        }

        public async Task<ValueTuple<bool, string, Usuarios?>> CrearNuevoUsuario(Usuarios usuario, List<string> roles)
        {
            try
            {
                usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena);
                baseDeDatos.Usuarios.Add(usuario);
                foreach (var rol in roles)
                {
                    Roles rolEncontrado = await rolesServicio.BuscarRolPorNombre(rol);
                    if (rolEncontrado == null)
                    {
                        return (false, "Uno de los roles es inválido.", null);
                    }
                    else
                    {
                        baseDeDatos.UsuariosRoles.Add(new UsuariosRoles() { Usuario = usuario, Rol = rolEncontrado });
                    }
                }
                await baseDeDatos.SaveChangesAsync();
                return (true, "Usuario creado con éxito", usuario);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ocurrió un error {ex}");

                if (ex.InnerException != null &&
                    ex.InnerException.Message.Contains("unique index 'IX_Usuarios_NombreUsuario'"))
                {
                    return (false, $"El nombre de usuario ya está en uso", null);
                }
                return (false, $"Ocurrió un error: {ex.Message}", null);
            }
        }

    }
}
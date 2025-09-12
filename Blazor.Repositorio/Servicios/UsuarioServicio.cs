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
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly AppDbContext baseDeDatos;
        private readonly IRolesServicio rolesServicio;

        public UsuarioServicio(AppDbContext baseDeDatos, IRolesServicio rolesServicio)
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

        public async Task<ValueTuple<bool, string, Usuarios?>> CrearNuevoUsuario(UsuarioCrearDTO usuarioDTO)
        {
            try
            {
                Usuarios usuario = new Usuarios()
                {
                    NombreUsuario = usuarioDTO.NombreUsuario,
                    Contrasena = BCrypt.Net.BCrypt.HashPassword(usuarioDTO.Contrasena),
                    Estado = true,
                };

                baseDeDatos.Usuarios.Add(usuario);

                if (usuarioDTO.Roles.Count != usuarioDTO.Roles.Distinct(StringComparer.OrdinalIgnoreCase).Count())
                    return (false, "Hay varios roles que se repiten, no puede haber duplicados.", null);

                if (!await rolesServicio.RolesExisten(usuarioDTO.Roles))
                    return (false, "Uno de los roles no existe.", null);

                var rolesBBDD = await rolesServicio.BuscarRolesPorNombres(usuarioDTO.Roles);

                foreach (var rol in rolesBBDD)
                {
                    baseDeDatos.UsuariosRoles.Add(
                        new UsuariosRoles() { Usuario = usuario, RolId = rol.Id });
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

        public async Task<ValueTuple<bool, string, Usuarios?>> ActualizarUsuario(ActualizarUsuarioDTO usuarioDTO)
        {
            Usuarios? usuario = await baseDeDatos.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioDTO.Id);

            if (usuario == null) return (false, "Usuario no encontrado.", null);

            if (!string.IsNullOrEmpty(usuarioDTO.NombreUsuario))
                usuario.NombreUsuario = usuarioDTO.NombreUsuario;

            if (!string.IsNullOrEmpty(usuarioDTO.Contrasena))
                usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(usuarioDTO.Contrasena);

            if (usuarioDTO.Roles != null)
            {
                // Validar existencia de roles y no duplicados
                if (usuarioDTO.Roles.Count != usuarioDTO.Roles.Distinct(StringComparer.OrdinalIgnoreCase).Count())
                    return (false, "Hay varios roles que se repiten, no puede haber duplicados.", null);

                if (!await rolesServicio.RolesExisten(usuarioDTO.Roles))
                    return (false, "Uno de los roles no existe.", null);

                // Paso prueba, entonces comienza a ver que roles se quedan, se añaden y se quitan
                var rolesEnBBDD = await baseDeDatos.UsuariosRoles
                    .Where(u => u.UsuarioId == usuarioDTO.Id)
                    .Select(u => u.Rol.Nombre).ToListAsync();
                usuarioDTO.Roles = usuarioDTO.Roles.Select(r => r.ToUpper().Trim()).ToList();

                var nombresRolesAEliminar = rolesEnBBDD.Except(usuarioDTO.Roles).ToList();
                var rolesAEliminar = await rolesServicio.BuscarRolesPorNombres(nombresRolesAEliminar);

                if (rolesAEliminar.Count > 0)
                {
                    var registrosAEliminar = await baseDeDatos.UsuariosRoles
                        .Where(u => u.UsuarioId == usuarioDTO.Id &&
                                rolesAEliminar.Select(r => r.Id).Contains(u.RolId))
                        .ToListAsync();

                    baseDeDatos.UsuariosRoles.RemoveRange(registrosAEliminar);
                }

                var nombresRolesAAnadir = usuarioDTO.Roles.Except(rolesEnBBDD).ToList();
                var rolesAAnadir = await rolesServicio.BuscarRolesPorNombres(nombresRolesAAnadir);

                if (rolesAAnadir.Count > 0)
                {
                    foreach (var rol in rolesAAnadir)
                    {
                        baseDeDatos.UsuariosRoles
                            .Add(new UsuariosRoles() { UsuarioId = usuario.Id, RolId = rol.Id });
                    }
                }

            }

            baseDeDatos.Usuarios.Update(usuario);
            await baseDeDatos.SaveChangesAsync();

            return (true, "Usuario actualizado con éxito.", usuario);
        }

        public async Task<ValueTuple<bool, string>> EliminarUsuario(long id)
        {
            Usuarios? usuarios = await baseDeDatos.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

            if (usuarios == null) return (false, "El usuario no existe");

            usuarios.Estado = false;

            baseDeDatos.Usuarios.Update(usuarios);
            await baseDeDatos.SaveChangesAsync();
            return (true, "Usuario eliminado con éxito");
        }
    }
}
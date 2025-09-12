using Blazor.BD.Entidades;
using Blazor.Repositorios.Implementaciones;
using Blazor.Shared.DTO_Token;
using Blazor.Shared.DTO_Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Repositorios.Servicios
{
    public class AuthServicio : IAuthServicio
    {
        private readonly IUsuarioServicio usuarioServicio;
        private readonly ITokenServicio tokenServicio;

        public AuthServicio(IUsuarioServicio usuarioServicio, ITokenServicio tokenServicio)
        {
            this.usuarioServicio = usuarioServicio;
            this.tokenServicio = tokenServicio;
        }

        public async Task<(bool, string, Token?)> IniciarSesion(UsuarioAutenticacion usuario)
        {
            Usuarios usuarioEncontrado = await usuarioServicio.BuscarUsuarioPorNombre(usuario.NombreUsuario);

            if (usuarioEncontrado == null) return (false, "Usuario no encontrado.", null);

            if (BCrypt.Net.BCrypt.Verify(usuario.Contrasena, usuarioEncontrado.Contrasena))
            {
                Token token = await tokenServicio.GenerarToken(usuarioEncontrado, usuario.MantenerSesion);
                return (true, "Inicio de sesión éxitoso", token);
            }
            else return (true, "Nombre de usuario o contraseña inválido.", null);
        }

        public Task<(bool, string)> CerrarSesion()
        {
            throw new NotImplementedException();
        }
    }
}

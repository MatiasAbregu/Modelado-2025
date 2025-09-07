using Blazor.Shared.DTO_Token;
using Blazor.Shared.DTO_Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Repositorios.Implementaciones
{
    public interface IAuthServicio
    {
        public Task<ValueTuple<bool, string, Token?>> IniciarSesion(UsuarioAutenticacion usuario);
        public Task<ValueTuple<bool, string>> CerrarSesion();
    }
}

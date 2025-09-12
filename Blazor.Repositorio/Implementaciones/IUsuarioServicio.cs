using Blazor.BD.Entidades;
using Blazor.Shared.DTO_Usuarios;
using Blazor.Shared.DTO_Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Repositorios.Implementaciones
{
    public interface IUsuarioServicio
    {
        public Task<ValueTuple<bool, string, List<UsuarioVerDTO>>> BuscarUsuarios();
        public Task<Usuarios?> BuscarUsuarioPorNombre(string nombre);
        public Task<ValueTuple<bool, string, Usuarios?>> CrearNuevoUsuario(UsuarioCrearDTO usuarioDTO);
        public Task<ValueTuple<bool, string, Usuarios?>> ActualizarUsuario(ActualizarUsuarioDTO usuarioDTO);
        public Task<ValueTuple<bool, string>> EliminarUsuario(long id);
        
    }
}
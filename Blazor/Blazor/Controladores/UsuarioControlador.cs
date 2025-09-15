using Blazor.Repositorios.Implementaciones;
using Blazor.Repositorios.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Blazor.Shared.DTO_Usuarios;
using Blazor.BD.Entidades;
using Microsoft.AspNetCore.Authorization;

namespace Blazor.Server.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioControlador : ControllerBase
    {

        private readonly IUsuarioServicio usuarioServicio;

        public UsuarioControlador(IUsuarioServicio usuarioServicio)
        {
            this.usuarioServicio = usuarioServicio;
        }

        [HttpGet]
        public async Task<ActionResult> VisualizarUsuarios()
        {
            ValueTuple<bool, string, List<UsuarioVerDTO>> res 
                = await usuarioServicio.BuscarUsuarios();

            if (res.Item1 == true && res.Item2 == "") return StatusCode(200, res.Item3);
            else if (res.Item1 == true) return StatusCode(200, res.Item2);
            else return StatusCode(500, res.Item2);
        }

        [HttpPost]
        public async Task<ActionResult> CrearNuevoUsuario(UsuarioCrearDTO usuarioDTO)
        {
            if (usuarioDTO == null) return StatusCode(400, "Hubo un error en el servidor, intentelo más tarde.");

            ValueTuple<bool, string, Usuarios> res = await usuarioServicio.CrearNuevoUsuario(usuarioDTO);

            if (res.Item1 == true) return StatusCode(200, res.Item2);
            return StatusCode(400, res.Item2);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> ActualizarUsuario(ActualizarUsuarioDTO usuarioDTO, long id)
        {
            if (usuarioDTO.Id != id) return StatusCode(409, "Hubo un error en el servidor, intentelo más tarde.");

            ValueTuple<bool, string, Usuarios> res = await usuarioServicio.ActualizarUsuario(usuarioDTO);

            if (res.Item1 == true) return StatusCode(200, res.Item2); 
            return StatusCode(409, res.Item2);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> EliminarUsuario(long id)
        {
            ValueTuple<bool, string> res = await usuarioServicio.EliminarUsuario(id);

            if (res.Item1 == true) return StatusCode(200, res.Item2);
            return StatusCode(400, res.Item2);
        }
    }
}
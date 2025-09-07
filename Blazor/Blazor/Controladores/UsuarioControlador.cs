using Blazor.Repositorios.Implementaciones;
using Blazor.Repositorios.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Blazor.Shared.DTO_Usuarios;
using Blazor.BD.Entidades;

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
        public async Task<ActionResult<List<Usuarios>>> VisualizarUsuarios()
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

            ValueTuple<bool, string, Usuarios> res = await usuarioServicio.CrearNuevoUsuario(new Usuarios()
            {
                NombreUsuario = usuarioDTO.NombreUsuario,
                Contrasena = usuarioDTO.Contrasena,
                Estado = true
            },
            usuarioDTO.Roles);

            if (res.Item1 == true) return StatusCode(200, res.Item2);
            return StatusCode(400, res.Item2);
        }
    }
}
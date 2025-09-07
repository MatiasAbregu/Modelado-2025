using Blazor.BD.Entidades;
using Blazor.Shared.DTO_Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Repositorios.Implementaciones
{
    public interface ITokenServicio
    {
        Task<Token> GenerarToken(Usuarios usuario, bool MantenerSesion);
        Task<ValueTuple<bool, string, Token>> SolicitarNuevoJWTToken(Token token, long usuarioId);
        Task<ValueTuple<bool, string>> RevocarToken(Token token, long usuarioId);
    }
}

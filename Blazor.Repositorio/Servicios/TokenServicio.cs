using BCrypt.Net;
using Blazor.BD;
using Blazor.BD.Entidades;
using Blazor.Repositorios.Implementaciones;
using Blazor.Shared.DTO_Token;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blazor.Repositorios.Servicios
{
    public class TokenServicio : ITokenServicio
    {
        private readonly AppDbContext baseDeDatos;
        private readonly IConfiguration _configuracion;
        private readonly SymmetricSecurityKey llave;

        public TokenServicio(AppDbContext baseDeDatos, IConfiguration configuracion)
        {
            this.baseDeDatos = baseDeDatos;
            _configuracion = configuracion;
            llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuracion["JWT:Clave"]));
        }

        public async Task<Token> GenerarToken(Usuarios usuario, bool mantenerSesion)
        {
            // Añadir roles a los claims en el JWT como el ID y el nombre de usuario
            List<UsuariosRoles> roles
                = await baseDeDatos.UsuariosRoles.Where(u => u.UsuarioId == usuario.Id)
                                                 .Include(u => u.Rol).ToListAsync();

            List<Claim> claims = [new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                                  new Claim(ClaimTypes.GivenName, usuario.NombreUsuario)];

            foreach (var rol in roles) claims.Add(new Claim(ClaimTypes.Role, rol.Rol.Nombre));

            // FIRMAR EL TOKEN Y ASIGNAR EL TIEMPO QUE SEA
            SigningCredentials credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha512);
            JwtSecurityToken JWTToken = new(expires: mantenerSesion
                                        ? DateTime.UtcNow.AddMinutes(15) : DateTime.UtcNow.AddDays(1),
                                        claims: claims,
                                        signingCredentials: credenciales,
                                        issuer: _configuracion["JWT:FirmaDelBackend"]);

            // GENERAR REFRESH Y LIMPIAR DE LA BASE DE DATOS
            string refreshToken = "";
            if (mantenerSesion)
            {
                refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

                var tokenViejo = await baseDeDatos.Tokens.FirstOrDefaultAsync(t => t.UserId == usuario.Id);
                if (tokenViejo != null) baseDeDatos.Tokens.Remove(tokenViejo);

                baseDeDatos.Tokens.Add(new
                    Tokens(usuario.Id, BCrypt.Net.BCrypt.HashPassword(refreshToken), DateTime.UtcNow, DateTime.UtcNow.AddDays(14)));

                await baseDeDatos.SaveChangesAsync();
            }

            Token newToken = new Token()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(JWTToken),
                RefreshToken = refreshToken,
            };

            return newToken;
        }

        public async Task<ValueTuple<bool, string, Token>> SolicitarNuevoJWTToken(Token token, long usuarioId)
        {
            Tokens tokenGuardado = await baseDeDatos.Tokens.Include(t => t.Usuario)
                .FirstOrDefaultAsync(t => t.ExpiresAt > DateTime.UtcNow && t.UserId == usuarioId);

            if (tokenGuardado == null) return (false, "Refresh token inválido o expirado.", null);

            bool valido = BCrypt.Net.BCrypt.Verify(token.RefreshToken, tokenGuardado.RefreshToken);
            if (!valido) return (false, "Refresh token no coincide.", null);

            Token nuevoToken = await GenerarToken(tokenGuardado.Usuario, true);

            return (true, "", nuevoToken);
        }

        public Task<(bool, string)> RevocarToken(Token token, long usuarioId)
        {
            throw new NotImplementedException();
        }

    }
}

using mvcLoginForm.Models;
using System.Security.Claims;

namespace mvcLoginForm.Utils
{
    public class ClaimsUtils
    {
        public List<Claim> generarClaim(Usuario usuario)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.id.ToString()),
                new Claim("Usuario", usuario.usuario!),
                new Claim(ClaimTypes.Email, usuario.email!)
            };
            return claims;
        }
        public List<Claim> generarClaimMVC(Usuario usuario)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.id.ToString()),
                new Claim(ClaimTypes.Name, usuario.nombre!),
                new Claim("Apellido", usuario.apellido!),
                new Claim("Usuario", usuario.usuario!),
                new Claim(ClaimTypes.Email, usuario.email!),
                new Claim("Contraseña", usuario.contrasena!)
            };
            return claims;
        }
    }
}

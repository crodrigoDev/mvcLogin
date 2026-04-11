using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvcLoginForm.DAO;
using mvcLoginForm.Models;
using mvcLoginForm.Models.dto;
using mvcLoginForm.Utils;
using System.Net;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace mvcLoginForm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly usuarioDAO _dao;
        private readonly TokenJwt _tokenJwt;
        public AuthController(usuarioDAO dao, TokenJwt tokenJwt)
        {
            _dao = dao;
            _tokenJwt = tokenJwt;
        }
        
        // POST api/<ValuesController>
        [HttpPost("login")]
        public IActionResult login([FromBody] Login login)
        {
            Usuario usuario = _dao.getLogin(login.login!, login.contrasena!);
            if(usuario == null){
                return Unauthorized(new {
                    success = false,
                    data = (string)null,
                    message = "No se encontró un usuario con las credenciales proporcionadas."
                });
            }
            List<Claim> c = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, usuario.nombre!),
                    new Claim("Apellido", usuario.apellido!),
                    new Claim("Usuario", usuario.usuario!),
                    new Claim(ClaimTypes.Email, usuario.email!),
                    new Claim("Contraseña", usuario.contrasena!),
                    new Claim(ClaimTypes.NameIdentifier, usuario.id.ToString())
                };

            var tokenjwt = _tokenJwt.JwtToken(c);
            return Ok(new
            {
                success = true,
                token = tokenjwt,
                data = new
                {
                    usuario.id,
                    usuario.usuario,
                    usuario.nombre,
                    usuario.apellido,
                    usuario.email,
                    usuario.contrasena
                },
                message = "Usuario loggeado exitosamente"
            });
        }

        [HttpPut("actualizar")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult actualizar([FromBody] Crear user)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            bool vUsuario = _dao.validarUsuario(userId, user.usuario!);
            if (vUsuario)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "El usuario ya existe, elija otro"
                });
            }
            bool vEmail = _dao.validarEmail(userId, user.email!);
            if (vEmail)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "El correo ya esta en uso"
                });
            }
            if (user.contrasena != user.r_contrasena)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Contraseñas diferentes"
                });
            }
            bool vPass = _dao.validarPass(userId,user.contrasena!);
            if (!vPass)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "La contraseña debe ser diferente a la actual"
                });
            }
            _dao.actualizarUsuario(userId, user.usuario!, user.nombre!, user.apellido!, user.email!, user.contrasena!);
            return Ok(new
            {
                success = true,
                message = "Usuario actualizado"
            });
        }
    }
}

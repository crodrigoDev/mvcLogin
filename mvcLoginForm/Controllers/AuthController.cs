using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvcLoginForm.DAO;
using mvcLoginForm.Models;
using mvcLoginForm.Models.dto;
using mvcLoginForm.Utils;
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
        private readonly ClaimsUtils _claim;
        public AuthController(usuarioDAO dao, TokenJwt tokenJwt, ClaimsUtils claim)
        {
            _dao = dao;
            _tokenJwt = tokenJwt;
            _claim = claim;
        }

        // POST api/<ValuesController>
        [HttpPost("login")]
        public IActionResult login([FromBody] Login login)
        {
            Usuario usuario = _dao.getLogin(login.login!, login.contrasena!);
            if (usuario == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    data = (string)null!,
                    message = "No se encontró un usuario con las credenciales proporcionadas."
                });
            }
            List<Claim> c = _claim.generarClaim(usuario);

            string tokenjwt = _tokenJwt.JwtToken(c);
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
            if (vUsuario) return BadRequest(new { success = false, message = "El usuario ya existe, elija otro" });

            bool vEmail = _dao.validarEmail(userId, user.email!);
            if (vEmail) return BadRequest(new { success = false, message = "El correo ya esta en uso" });

            if (user.contrasena != user.r_contrasena) return BadRequest(new { success = false, message = "Contraseñas diferentes" });

            bool vPass = _dao.validarPass(userId, "", user.contrasena!);
            if (vPass) return BadRequest(new { success = false, message = "La contraseña debe ser diferente a la actual" });

            _dao.actualizarUsuario(userId, user.usuario!, user.nombre!, user.apellido!, user.email!, user.contrasena!);
            List<Claim> c = _claim.generarClaim(new Usuario
            {
                id = userId,
                usuario = user.usuario,
                email = user.email,
            });
            string newToken = _tokenJwt.JwtToken(c);
            return Ok(new
            {
                success = true,
                token = newToken,
                data = new
                {
                    userId,
                    user.usuario,
                    user.nombre,
                    user.apellido,
                    user.email,
                    user.contrasena
                },
                message = "Usuario actualizado"
            });
        }

        [HttpPatch("recuperar")]
        public IActionResult recuperar([FromBody] Recuperar recuperar)
        {
            bool vUsuario = _dao.validarUsuario(null, recuperar.usuario!);
            if(!vUsuario) return BadRequest(new { success = false, message = "El usuario no existe" });

            if(recuperar.contrasena != recuperar.r_contrasena) return BadRequest(new { success = false, message = "Contraseñas diferentes" });

            bool vPass = _dao.validarPass(0, recuperar.usuario!, recuperar.contrasena!);
            if (vPass) return BadRequest(new { success = false, message = "La contraseña debe ser diferente a la actual" });

            _dao.putPass(recuperar.contrasena!, recuperar.usuario!);
            return Ok(new
            {
                success = true,
                message = "Contraseña actualizada"
            });
        }

        [HttpPost("registrar")]
        public IActionResult registrar([FromBody] Crear crear)
        {   
            bool vUsuario = _dao.validarUsuario(null, crear.usuario!);
            if(vUsuario) return BadRequest(new { success = false, message = "El usuario ya existe, elija otro" });

            bool vEmail = _dao.validarEmail(0, crear.email!);
            if(vEmail) return BadRequest( new { success = false, message = "El correo ya esta en uso" });

            if(crear.contrasena != crear.r_contrasena) return BadRequest(new { success = false, message = "Contraseñas diferentes" });

            _dao.crearUsuario(crear.usuario!, crear.nombre!, crear.apellido!, crear.email!, crear.contrasena!);
            return Ok(new
            {
                success = true,
                message = "Usuario registrado exitosamente"
            });
        } 
    }
}

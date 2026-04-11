using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvcLoginForm.DAO;
using mvcLoginForm.Models;
using mvcLoginForm.Models.dto;
using mvcLoginForm.Utils;
using System.Security.Claims;

namespace mvcLoginForm.Controllers
{
    public class LoginController : Controller
    {
        private readonly usuarioDAO _dao;
        private readonly ClaimsUtils _claims;

        public LoginController(usuarioDAO dao, ClaimsUtils claims)
        {
            _dao = dao;
            _claims = claims;
        }
        public IActionResult Index()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                    return RedirectToAction("LoginPass", "Login");
            }
            return View();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult LoginPass()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Login log)
        {
            Usuario usuario = _dao.getLogin(log.login!, log.contrasena!);
            if (usuario == null)
            {
                TempData["MensajeError"] = "Usuario no encontrado";
                return RedirectToAction("Index", "Login");
            }
            List<Claim> c = _claims.generarClaimMVC(usuario);
            ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties ap = new();
            ap.AllowRefresh = true;
            ap.IsPersistent = true;
            ap.ExpiresUtc = DateTime.UtcNow.AddMinutes(30);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), ap);

            return RedirectToAction("LoginPass", "Login");

        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }

        public IActionResult Recuperar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CambiarPass(Recuperar recuperar)
        {
            bool vUsuario = _dao.validarUsuario(null,recuperar.usuario!);
            if (!vUsuario)
            {
                ViewBag.Mensaje = "Usuario no encontrado";
                return View("~/Views/Login/Recuperar.cshtml");
            }
            if (recuperar.contrasena != recuperar.r_contrasena)
            {
                ViewBag.Mensaje = "Las dos contraseñas deben ser iguales";
                return View("~/Views/Login/Recuperar.cshtml");
            }
            bool vPass = _dao.validarPass(null, recuperar.usuario!, recuperar.contrasena!);
            if (vPass)
            {
                ViewBag.Mensaje = "Ingrese una contraseña diferente a la actual";
                return View("~/Views/Login/Recuperar.cshtml");
            }
            _dao.putPass(recuperar.contrasena!, recuperar.usuario!);
            TempData["PassExitoso"] = "Contraseña cambiada correctamente";
            return View("~/Views/Login/Index.cshtml");
        }

        public IActionResult Registrar()
        {
            return View();
        }

        public IActionResult CrearUsuario(Crear crear)
        {
            bool vUsuario = _dao.validarUsuario(null,crear.usuario!);
            if (vUsuario)
            {
                ViewBag.Mensaje = "El usuario ya existe, elija otro";
                return View("~/Views/Login/Registrar.cshtml");
            }
            bool vEmail = _dao.validarEmail(null, crear.email!);
            if (vEmail)
            {
                ViewBag.Mensaje = "El correo ya esta en uso";
                return View("~/Views/Login/Registrar.cshtml");
            }
            if (crear.contrasena != crear.r_contrasena)
            {
                ViewBag.Mensaje = "Las dos contraseñas deben ser iguales";
                return View("~/Views/Login/Registrar.cshtml");
            }
            _dao.crearUsuario(crear.usuario!, crear.nombre!, crear.apellido!, crear.email!, crear.contrasena!);
            TempData["RegistroExitoso"] = "Usuario registrado correctamente";
            return RedirectToAction("Index", "Login");
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Actualizar()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ActualizarUsuario(Crear crear)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            bool vUsuario = _dao.validarUsuario(userId,crear.usuario!);
            if (vUsuario)
            {
                ViewBag.Mensaje = "El usuario ya existe, elija otro";
                return View("~/Views/Login/Actualizar.cshtml");
            }
            bool vEmail = _dao.validarEmail(userId, crear.email!);
            if (vEmail)
            {
                ViewBag.Mensaje = "El correo ya esta en uso";
                return View("~/Views/Login/Actualizar.cshtml");
            }
            if(crear.contrasena != crear.r_contrasena)
            {
                ViewBag.Mensaje = "Las dos contraseñas deben ser iguales";
                return View("~/Views/Login/Actualizar.cshtml");
            }
            _dao.actualizarUsuario(userId, crear.usuario!, crear.nombre!, crear.apellido!, crear.email!, crear.contrasena!);
            List<Claim> c = _claims.generarClaimMVC(new Usuario
            {
                id = userId,
                nombre = crear.nombre!,
                apellido = crear.apellido!,
                usuario = crear.usuario!,
                email = crear.email!,
                contrasena = crear.contrasena!
            });
            ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci));
            TempData["ActualizacionExitosa"] = "Usuario actualizado exitosamente";
            return RedirectToAction("LoginPass", "Login");
        }
    }
}

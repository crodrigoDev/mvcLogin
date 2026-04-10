using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvcLoginForm.DAO;
using mvcLoginForm.Models.dto;
using System.Security.Claims;

namespace mvcLoginForm.Controllers
{
    public class LoginController : Controller
    {
        private readonly usuarioDAO _dao;

        public LoginController(usuarioDAO dao) => _dao = dao;
        public IActionResult Index()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                    return RedirectToAction("LoginPass", "Login", new { id = int.Parse(c.Identity.Name) });
            }
            return View();
        }

        [Authorize]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult LoginPass(int id)
        {
            return View(_dao.getUsuario(id));
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Login log)
        {
            int access = _dao.getLogin(log.login, log.contrasena);
            if (access != 0)
            {
                List<Claim> c = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, access.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, access.ToString())
                };
                ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties ap = new();
                ap.AllowRefresh = true;
                ap.IsPersistent = true;
                ap.ExpiresUtc = DateTime.UtcNow.AddMinutes(30);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), ap);

                return RedirectToAction("LoginPass", "Login", new { id = access });
            }
            TempData["MensajeError"] = "Usuario no encontrado";
            return RedirectToAction("Index", "Login");
        }

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
            bool vUsuario = _dao.validarUsuario(recuperar.usuario);
            if (vUsuario)
            {
                if (recuperar.contrasena != recuperar.r_contrasena)
                {
                    ViewBag.Mensaje = "Las dos contraseñas deben ser iguales";
                    return View("~/Views/Login/Recuperar.cshtml");
                }
                bool vPass = _dao.validarPass(recuperar.contrasena);
                if (vPass)
                {
                    _dao.putPass(recuperar.contrasena, recuperar.usuario, recuperar.id);
                    TempData["PassExitoso"] = "Contraseña cambiada correctamente";
                    return View("~/Views/Login/Index.cshtml");
                }
                ViewBag.Mensaje = "Ingrese una contraseña diferente a la actual";
                return View("~/Views/Login/Recuperar.cshtml");
            }
            ViewBag.Mensaje = "Usuario no encontrado";
            return View("~/Views/Login/Recuperar.cshtml");
        }

        public IActionResult Registrar()
        {
            return View();
        }

        public IActionResult CrearUsuario(Crear crear)
        {
            bool vUsuario = _dao.validarUsuario(crear.usuario);
            if (!vUsuario)
            {
                bool vEmail = _dao.validarEmail(crear.email);
                if (!vEmail)
                {
                    if (crear.contrasena == crear.r_contrasena)
                    {
                        _dao.crearUsuario(crear.usuario, crear.nombre, crear.apellido, crear.email, crear.contrasena);
                        TempData["RegistroExitoso"] = "Usuario registrado correctamente";
                        return RedirectToAction("Index", "Login");
                    }
                    ViewBag.Mensaje = "Las dos contraseñas deben ser iguales";
                    return View("~/Views/Login/Registrar.cshtml");
                }
                ViewBag.Mensaje = "El correo ya esta en uso";
                return View("~/Views/Login/Registrar.cshtml");

            }
            ViewBag.Mensaje = "El usuario ya existe, elija otro";
            return View("~/Views/Login/Registrar.cshtml");
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvcLoginForm.DAO;
using mvcLoginForm.Models;
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
            if (usuario != null)
            {
                List<Claim> c = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, usuario.nombre!),
                    new Claim("Apellido", usuario.apellido!),
                    new Claim("Usuario", usuario.usuario!),
                    new Claim(ClaimTypes.Email, usuario.email!),
                    new Claim("Contraseña", usuario.contrasena!),
                    new Claim(ClaimTypes.NameIdentifier, usuario.id.ToString())
                };
                ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties ap = new();
                ap.AllowRefresh = true;
                ap.IsPersistent = true;
                ap.ExpiresUtc = DateTime.UtcNow.AddMinutes(30);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), ap);

                return RedirectToAction("LoginPass", "Login");
            }
            TempData["MensajeError"] = "Usuario no encontrado";
            return RedirectToAction("Index", "Login");
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
            bool vUsuario = _dao.validarUsuario(null,recuperar.usuario);
            if (vUsuario)
            {
                if (recuperar.contrasena != recuperar.r_contrasena)
                {
                    ViewBag.Mensaje = "Las dos contraseñas deben ser iguales";
                    return View("~/Views/Login/Recuperar.cshtml");
                }
                bool vPass = _dao.validarPass(null,recuperar.contrasena);
                if (!vPass)
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
            bool vUsuario = _dao.validarUsuario(null,crear.usuario);
            if (!vUsuario)
            {
                bool vEmail = _dao.validarEmail(null, crear.email);
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

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Actualizar()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ActualizarUsuario(Crear crear)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            bool vUsuario = _dao.validarUsuario(userId,crear.usuario);
            if (!vUsuario)
            {
                bool vEmail = _dao.validarEmail(userId,crear.email);
                if (!vEmail)
                {
                    if (crear.contrasena == crear.r_contrasena)
                    {
                        _dao.actualizarUsuario(userId, crear.usuario, crear.nombre, crear.apellido, crear.email, crear.contrasena);
                        List<Claim> c = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, crear.nombre!),
                            new Claim("Apellido", crear.apellido!),
                            new Claim("Usuario", crear.usuario!),
                            new Claim(ClaimTypes.Email, crear.email!),
                            new Claim("Contraseña", crear.contrasena!), 
                            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                        };

                        ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci));
                        TempData["ActualizacionExitosa"] = "Usuario actualizado exitosamente";
                        return RedirectToAction("LoginPass", "Login");
                    }
                    ViewBag.Mensaje = "Las dos contraseñas deben ser iguales";
                    return View("~/Views/Login/Actualizar.cshtml");
                }
                ViewBag.Mensaje = "El correo ya esta en uso";
                return View("~/Views/Login/Actualizar.cshtml");

            }
            ViewBag.Mensaje = "El usuario ya existe, elija otro";
            return View("~/Views/Login/Actualizar.cshtml");
        }
    }
}

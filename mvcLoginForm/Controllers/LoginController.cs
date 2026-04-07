using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using mvcLoginForm.DAO;
using mvcLoginForm.Models;

namespace mvcLoginForm.Controllers
{
    public class LoginController : Controller
    {
        private readonly usuarioDAO _dao;

        public LoginController(usuarioDAO dao) => _dao = dao;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Recuperar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IniciarSesion(string contrasena = "", string usuario = "", string email = "")
        {
            bool access = _dao.getLogin(contrasena, usuario, email);
            if (!access) return View("~/Views/Login/Index.cshtml", ViewBag.Mensaje = "Usuario no encontrado");
            return View("~/Views/Login/LoginPass.cshtml", _dao.getUsuario(usuario));
        }

        [HttpPost]
        public IActionResult CambiarPass(string contrasena, string r_contrasena,string usuario, int id)
        {
            if(contrasena != r_contrasena) return View("~/Views/Login/Recuperar.cshtml", ViewBag.Mensaje = "Las dos contraseñas deben ser iguales");
            _dao.putPass(contrasena, usuario, id);
            return View("~/Views/Login/Index.cshtml", ViewBag.Mensaje = "Contraseña cambiada correctamente");

        }

        public IActionResult CerrarSesion()
        {
            return View("~/Views/Login/Index.cshtml");
        }
    }
}

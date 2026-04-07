using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using mvcLoginForm.DAO;

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

        [HttpPost]
        public IActionResult IniciarSesion(string contrasena = "", string usuario = "", string email = "")
        {
            bool access = _dao.getLogin(contrasena, usuario, email);
            if (!access) return View("~/Views/Login/Index.cshtml");
            return View("~/Views/Login/LoginPass.cshtml", _dao.getUsuario(usuario));
        }
    }
}

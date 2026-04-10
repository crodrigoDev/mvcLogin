using mvcLoginForm.Config;
using mvcLoginForm.Mapper;
using mvcLoginForm.Models;
using System.Data;

namespace mvcLoginForm.DAO
{
    public class usuarioDAO
    {
        private readonly clsBD _bd;
        public usuarioDAO(clsBD bd) => _bd = bd;

        public int getLogin(string user, string pass)
        {
            _bd.Sentencia($"sp_validarLogin '{user}', '{pass}'");
            DataTable dt = _bd.getDataTable();
            if (dt == null || dt.Rows.Count == 0) return 0;
            int id = Convert.ToInt32(dt.Rows[0]["id"]);
            return id;
        }

        public void putPass(string pass, string usuario, int id = 0)
        {
            _bd.update($"sp_cambiarContrasena {id}, '{pass}', '{usuario}'");
        }

        public bool validarUsuario(string user)
        {
            _bd.Sentencia($"sp_validarUsuario '{user}'");
            DataTable dt = _bd.getDataTable();
            if (dt.Rows.Count == 0) return false;
            return true;
        }

        public bool validarPass(string pass)
        {
            _bd.Sentencia($"sp_validarContrasena '{pass}'");
            DataTable dt = _bd.getDataTable();
            if (dt.Rows.Count == 0) return false;
            return true;
        }
        public bool validarEmail(string email)
        {
            _bd.Sentencia($"sp_validarEmail '{email}'");
            DataTable dt = _bd.getDataTable();
            if (dt.Rows.Count == 0) return false;
            return true;
        }

        public Usuario getUsuario(int id)
        {
            _bd.Sentencia($"sp_verDatosUsuario {id}");
            DataTable dt = _bd.getDataTable();
            if (dt.Rows.Count == 0) return null;
            return LoginMapper.UsuarioMap(dt.Rows[0]);
        }

        public void crearUsuario(string usuario, string nombre, string apellido, string email, string contrasena)
        {
            _bd.update($"sp_crearUsuario '{usuario}', '{nombre}', '{apellido}', '{email}', '{contrasena}'");
        }
    }
}

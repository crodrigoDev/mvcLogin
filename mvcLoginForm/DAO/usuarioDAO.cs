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

        public Usuario getLogin(string user, string pass)
        {
            _bd.Sentencia($"sp_validarLogin '{user}', '{pass}'");
            DataTable dt = _bd.getDataTable();
            if (dt == null || dt.Rows.Count == 0) return null;
            return LoginMapper.UsuarioMap(dt.Rows[0]);
        }

        public void putPass(string pass, string usuario, int id = 0)
        {
            _bd.update($"sp_cambiarContrasena {id}, '{pass}', '{usuario}'");
        }

        public bool validarUsuario(int? id, string user)
        {
            _bd.Sentencia($"sp_validarUsuario {id}, '{user}'");
            DataTable dt = _bd.getDataTable();
            if (dt.Rows.Count == 0) return false;
            return true;
        }

        public bool validarPass(int? id, string pass)
        {
            _bd.Sentencia($"sp_validarContrasena {id}, '{pass}'");
            DataTable dt = _bd.getDataTable();
            if (dt.Rows.Count == 0) return false;
            return true;
        }
        public bool validarEmail(int? id,string email)
        {
            _bd.Sentencia($"sp_validarEmail {id}, '{email}'");
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

        public void actualizarUsuario(int id,string usuario, string nombre, string apellido, string email, string contrasena)
        {
            _bd.update($"sp_actualizarUsuario {id}, '{usuario}', '{nombre}', '{apellido}', '{email}', '{contrasena}'");
        }
    }
}

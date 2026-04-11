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

        public void putPass(string pass, string usuario)
        {
            _bd.update($"sp_cambiarContrasena '{pass}', '{usuario}'");
        }
        public void crearUsuario(string usuario, string nombre, string apellido, string email, string contrasena)
        {
            _bd.update($"sp_crearUsuario '{usuario}', '{nombre}', '{apellido}', '{email}', '{contrasena}'");
        }

        public void actualizarUsuario(int id, string usuario, string nombre, string apellido, string email, string contrasena)
        {
            _bd.update($"sp_actualizarUsuario {id}, '{usuario}', '{nombre}', '{apellido}', '{email}', '{contrasena}'");
        }


        // Validaciones
        public bool validarUsuario(int? id, string user)
        {
            string idValue = id.HasValue ? id.Value.ToString() : "NULL";
            _bd.Sentencia($"sp_validarUsuario {idValue}, '{user}'");
            DataTable dt = _bd.getDataTable();
            if (dt.Rows.Count == 0) return false;
            return true;
        }

        public bool validarPass(int? id, string user, string pass)
        {
            string idValue = id.HasValue ? id.Value.ToString() : "NULL";
            _bd.Sentencia($"sp_validarContrasena {idValue}, '{user}', '{pass}'");
            DataTable dt = _bd.getDataTable();
            if (dt.Rows.Count == 0) return false;
            return true;
        }
        public bool validarEmail(int? id,string email)
        {
            string idValue = id.HasValue ? id.Value.ToString() : "NULL";
            _bd.Sentencia($"sp_validarEmail {idValue}, '{email}'");
            DataTable dt = _bd.getDataTable();
            if (dt.Rows.Count == 0) return false;
            return true;
        }
        
    }
}

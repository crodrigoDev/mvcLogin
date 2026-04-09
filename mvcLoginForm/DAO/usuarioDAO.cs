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

        public bool getLogin(string user, string pass)
        {
            _bd.Sentencia($"sp_loginPorUsuario '{user}', '{pass}'");
            DataTable dt = _bd.getDataTable();
            if (dt == null || dt.Rows.Count == 0) return false;
            return true;
        }

        public void putPass(string pass, string usuario, int id=0)
        {
            _bd.update($"sp_cambiarContrasena {id}, '{pass}', '{usuario}'");
        }

        public Usuario getUsuario(string user)
        {
            _bd.Sentencia($"sp_verDatosUsuario '{user}'");
            DataTable dt = _bd.getDataTable();
            if (dt.Rows.Count == 0) return null;
            return LoginMapper.UsuarioMap(dt.Rows[0]);
        }
    }
}

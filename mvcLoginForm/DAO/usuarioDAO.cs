using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public bool getLogin(string pass = "", string usuario = "",string email = "")
        {
            bool access = false;
            if (usuario == "") _bd.Sentencia($"sp_loginPorEmail '{email}', '{pass}'");
            if (email == "") _bd.Sentencia($"sp_loginPorUsuario '{usuario}', '{pass}'");
            DataTable dt = _bd.getDataTable();
            if (dt.Rows.Count == 0) return access;
            Usuario user = LoginMapper.UsuarioMap(dt.Rows[0]);
            if (user == null) return access;
            return access = true;
        }

        public Usuario getUsuario(string usuario)
        {
            _bd.Sentencia($"sp_verDatosUsuario '{usuario}'");
            DataTable dt = _bd.getDataTable();
            if (dt.Rows.Count == 0) return null;
            return LoginMapper.UsuarioMap(dt.Rows[0]);
        }
    }
}

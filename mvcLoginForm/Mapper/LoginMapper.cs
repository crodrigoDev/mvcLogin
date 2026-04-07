using mvcLoginForm.Models;
using System.Data;

namespace mvcLoginForm.Mapper
{
    public static class LoginMapper
    {
        public static Usuario UsuarioMap(DataRow dr)
        {
            Usuario user = new Usuario { };
            if (dr.Table.Columns.Contains("usuario") && dr.Table.Columns.Count == 1) user.usuario = dr["usuario"].ToString().Trim();
            if (dr.Table.Columns.Contains("email") && dr.Table.Columns.Count == 1) user.email = dr["email"].ToString().Trim();
            if (dr.Table.Columns.Contains("nombre"))
            {
                user.id = (int)dr["id"];
                user.email = dr["email"].ToString().Trim();
                user.contrasena = dr["contrasena"].ToString().Trim();
                user.usuario = dr["usuario"].ToString().Trim();
                user.apellido = dr["apellido"].ToString().Trim();
                user.nombre = dr["nombre"].ToString().Trim();
            }
            return user;
        }
    }
}

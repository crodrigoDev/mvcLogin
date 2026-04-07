using mvcLoginForm.Models;
using System.Data;

namespace mvcLoginForm.Mapper
{
    public static class LoginMapper
    {
        public static Usuario UsuarioMap(DataRow dr)
        {
            Usuario user = new Usuario{ contrasena = dr["contrasena"].ToString().Trim() };
            if (dr.Table.Columns.Contains("usuario")) user.usuario = dr["usuario"].ToString().Trim();
            if (dr.Table.Columns.Contains("email")) user.email = dr["email"].ToString().Trim();
            if (dr.Table.Columns.Contains("nombre"))
            {
                user.email = dr["email"].ToString().Trim();
                user.usuario = dr["usuario"].ToString().Trim();
                user.apellido = dr["apellido"].ToString().Trim();
                user.nombre = dr["nombre"].ToString().Trim();
            }
            return user;
        }
    }
}

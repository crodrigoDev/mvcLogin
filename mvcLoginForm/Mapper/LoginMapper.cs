using mvcLoginForm.Models;
using System.Data;

namespace mvcLoginForm.Mapper
{
    public static class LoginMapper
    {
        public static Usuario UsuarioMap(DataRow dr)
        {
            return new Usuario {
                id = (int)dr["id"],
                email = dr["email"].ToString().Trim(),
                contrasena = dr["contrasena"].ToString().Trim(),
                usuario = dr["usuario"].ToString().Trim(),
                apellido = dr["apellido"].ToString().Trim(),
                nombre = dr["nombre"].ToString().Trim()
            };
        }
    }
}

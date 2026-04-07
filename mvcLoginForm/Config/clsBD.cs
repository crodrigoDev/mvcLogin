using Microsoft.Data.SqlClient;
using System.Data;

namespace mvcLoginForm.Config
{
    public class clsBD
    {
        private readonly string _cadena;

        SqlConnection cn = null;
        SqlCommand cmd = null;
        SqlDataAdapter da = null;
        public clsBD(IConfiguration config)
        {
            _cadena = config.GetConnectionString("DefaultConnection");
            cn = new SqlConnection(_cadena);
            cmd = new SqlCommand("", cn);
            da = new SqlDataAdapter(cmd);
        }

        public void Sentencia(string query)
        {
            cmd.CommandText = query;
            cmd.Parameters.Clear();
        }

        public void update(string query)
        {
            cmd.CommandText = query;
            cmd.Parameters.Clear();
            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        public DataTable getDataTable()
        {
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Resourses
{
    public class ClassResourses
    {
        public static void RegistrarAuditora(int idAccion, string mantenimiento, int idProceso,int? idUsuario = null, string descripcion = null, int? idGeneral = null)
        {
            try
            {
                string usuarioWindows = Environment.UserName;
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Registrar_Auditoria", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@mantenimiento", mantenimiento);

                if (!string.IsNullOrEmpty(descripcion)){cmd.Parameters.AddWithValue("@descripcion", descripcion);}
                else{cmd.Parameters.AddWithValue("@descripcion", DBNull.Value);}

                cmd.Parameters.AddWithValue("@idTipoAccion", idAccion);
                cmd.Parameters.AddWithValue("@nombreUsuarioSesion", usuarioWindows);

                if (idGeneral.HasValue){cmd.Parameters.AddWithValue("@idGeneral", idGeneral.Value);}
                else{cmd.Parameters.AddWithValue("@idGeneral", DBNull.Value);}

                cmd.Parameters.AddWithValue("@idProceso", idProceso);

                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

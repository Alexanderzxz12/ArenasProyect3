using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                cmd = new SqlCommand("Auditoria_Registro", con);
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

        //MÉTODO PARA ENVIAR CORREOS POR LA ANULACIÓN DE UN REQUERIMIENTO
        public static void Enviar(string para, string asunto, string mensaje)
        {
            var outlokkApp = new Microsoft.Office.Interop.Outlook.Application();
            var mailItem = (Microsoft.Office.Interop.Outlook.MailItem)outlokkApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            mailItem.To = para;
            mailItem.Subject = asunto;
            mailItem.Body = mensaje;

            mailItem.Send();
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(mailItem);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(outlokkApp);
        }

        //METODO PARA LIMPIAR EL TEXTO SI CONTIENE ESPACIOS EN BLANCO O SALTOS DE LINEA
        public static void LimpiarTexto_EspaciosEnBlanco(TextBox txt)
        {
            string limpiartexto = Regex.Replace(txt.Text.Trim(), @"\s+", " ");

            txt.Text = limpiartexto;
        }
    }
}

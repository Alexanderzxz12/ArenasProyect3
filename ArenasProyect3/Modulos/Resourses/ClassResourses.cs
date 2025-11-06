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

        //METODO QUE REFRESCA LOS REQUERIMIENTOS GUARDANDO EL SCROLL
        public static void RefrescarRequerimientos(string tipoOperacion,DataGridView DGV, Action metodoCarga)
        {
            //VARIABLES QUE ALMACENARAN EL SCROLL Y LA FILA SELECCIONADA POR EL USUARIO
            int indiceScroll = 0;
            int filaseleccionada = -1;

            //SI EXISTEN FILAS O SI LA FILA DE PUEDE LEER
            if (DGV.RowCount > 0 && DGV.DisplayedRowCount(true) > 0)
            {
                //ASIGNA EL NUMERO LA PRIMERA FILA QUE SE PUEDE VISUALIZAR EN EL CONTROL
                indiceScroll = DGV.FirstDisplayedScrollingRowIndex;

                //ASIGNAR LA FILA SELECCIONADA POR EL USUARIO
                filaseleccionada = DGV.CurrentRow?.Index ?? -1;  //SI NO SE SELECCIONO NINGUNA SE GUARDARA COMO -1
            }

            //METODO QUE CARGA LOS REGISTROS NUEVAMENTE DE LA BASE DE DATOS
            metodoCarga();

            //SI LA OPERACION QUE SE REALICE ES GUARDAR Y
            if (tipoOperacion == "guardar" && DGV.Rows.Count > 0)
            {
                //SE AGARRA LA ULTIMA FILA
                int ultimaFila = DGV.Rows.Count - 1;

                //SE DIRIGE A LA ULTIMA FILA
                DGV.FirstDisplayedScrollingRowIndex = ultimaFila;

                // Y SE SELECCIONA
                DGV.Rows[ultimaFila].Selected = true;
            }
            else
            {
                //SI ES UN TIPO DE OPERACION COMO APROBAR,ANULAR,LIBERAR,GENERAR,ETC

                //SE LE PASA EL SCROLL
                if (indiceScroll >= 0 && indiceScroll < DGV.Rows.Count)
                {
                    DGV.FirstDisplayedScrollingRowIndex = indiceScroll;
                }

                //Y SE SELECCIONA LA FILA SELECCIONADA
                if (filaseleccionada >= 0 && filaseleccionada < DGV.Rows.Count)
                {
                    DGV.ClearSelection(); 
                    DGV.Rows[filaseleccionada].Selected = true; 
                    DGV.CurrentCell = DGV.Rows[filaseleccionada].Cells[0]; 
                }
            }
        }
    }
}

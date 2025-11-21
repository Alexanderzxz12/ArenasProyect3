using ArenasProyect3.Modulos.Logistica.Almacen;
using CrystalDecisions.CrystalReports.Engine;
//USING PARA CONECTAR LOS REPORTES A LA BASE DE DATOS
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;
using System.Web.Configuration;
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


        //METODO PARA APLICAR LA CONEXION A LOS REPORTES 
        public static void AplicarConexionReportes(ReportDocument reporte)
        {
            //CONEXION A TRAVES DE LO DECLARADO EN EL APP CONFIG
            string cadena = WebConfigurationManager.ConnectionStrings["ConexionBD"].ConnectionString;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(cadena);

            //DATOS DE LA CONEXION QUE SE PUEDEN VISUALIZAR EN EL APP CONFIG
            ConnectionInfo conexionInfo = new ConnectionInfo
            {
                ServerName = builder.DataSource,
                DatabaseName = builder.InitialCatalog,
                UserID = builder.UserID,
                Password = builder.Password,
                IntegratedSecurity = builder.IntegratedSecurity
            };

            //PASAR LAS CONEXIONES A TODAS LAS TABLAS QUE ALMACENE EL REPORTE PRINCIPAL
            foreach(Table tabla in reporte.Database.Tables)
            {
                TableLogOnInfo logInfo = tabla.LogOnInfo;
                logInfo.ConnectionInfo = conexionInfo;
                tabla.ApplyLogOnInfo(logInfo);

                tabla.Location = $"{builder.InitialCatalog}.dbo.{tabla.Name}";
            }

            //PASAR LAS CONEXIONES A LAS SECCIONES DEL REPORTE
            foreach(Section seccion in reporte.ReportDefinition.Sections)
            {
                foreach(ReportObject objeto in seccion.ReportObjects)
                {
                    if(objeto.Kind == ReportObjectKind.SubreportObject)
                    {
                        SubreportObject sub = (SubreportObject)objeto;
                        ReportDocument subreporte = sub.OpenSubreport(sub.SubreportName);

                        foreach(Table tabla in subreporte.Database.Tables)
                        {
                            TableLogOnInfo logInfo = tabla.LogOnInfo;
                            logInfo.ConnectionInfo = conexionInfo;
                            tabla.ApplyLogOnInfo(logInfo);
                            tabla.Location = $"{builder.InitialCatalog}.dbo.{tabla.Name}";
                        }
                    }
                }
            }
        }

        //METODO PARA EXPORTAR LOS REPORTES DESDE EL VISUALIZADOR
        //CONTROL DE LISTADO - COLUMNA DEL ID - COLUMNAESTADO -DICCIONARIO (STRING 'CLAVE = ESTADO', FUNC 'INT = IDREGISTRO', VA A DEVOLVER FORM'INSTANCIA FORMULARIO') - Titulo del archivo
        public static void ExportarDesdeVisualizador(DataGridView dgv,int columnaId,int columnaEstado,Dictionary<string, Func<int, Form>> formulariosPorEstado,string tituloarchivo)
        {
            // VALIDACION SI SE SELECCIONO UNA FILA
            if (dgv.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un registro antes de exportar.", "Validación del Sistema");
                return;
            }

            // CAPTURAR ID
            int idregistro = Convert.ToInt32(dgv.SelectedCells[columnaId].Value);


            // MENSAJE DE DIALOGO PARA QUE EL USUARIO DECIDA LA UBICACION DONDE SE EXPORTARA EL ARCHIVO Y TAMBIEN EL NOMBRE DEL ARCHIVO QUE DEFINA EL USUARIO
            // CAPTURA DE LA RUTA SELECCIONADA POR EL USUARIO PARA LA EXPORTACION
            string rutaSalida = "";

            using (SaveFileDialog saveFile = new SaveFileDialog())
            {
                saveFile.Filter = "PDF files (*.pdf)|*.pdf";
                //saveFile.FileName = $"{tituloarchivo} {idregistro}.pdf";
                saveFile.FileName = $"{tituloarchivo} {idregistro}.pdf";
                if (saveFile.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                //GUARDADO DEL NOMBRE DEL ARCHIVO
                rutaSalida = saveFile.FileName;

                // SI EL USUARIO BORRA EL .pdf QUE SE VUELVA A PONER
                if (!rutaSalida.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    rutaSalida += ".pdf";
                }
            }

            try
            {
                // CAPTURAR ESTADO
                string estado = dgv.SelectedCells[columnaEstado].Value.ToString();

                // VERIFICACION SI EL ESTADO INGRESADO EN EL DICCIONARIO TIENE UN FORMULARIO
                if (!formulariosPorEstado.ContainsKey(estado))
                {
                    MessageBox.Show($"No existe formulario para el estado: {estado}", "Error");
                    return;
                }

                // DEPENDIENDO DEL ESTADO SE LE PASARA LA FUNCION DEL DICCIONARIO Y SE LE INVOCARA CON EL IDREGISTRO (EL IDREGISTRO ES DEBIDO AL CONSTRUCTOR)
                //DEL DICCIONARIO formulariosPorEstado, CAPTURA LA FUNCION ASOCIADA AL ESTADO, Y EJECUTALO PASANDOLE EL PARAMETRO idregistro.”
                Form frm = formulariosPorEstado[estado].Invoke(idregistro);


                //  BUSQUEDA DEL CONTROL CRYSTAL VIEWER SI EXISTE EN EL FORMULARIO
                var viewer = frm.Controls.OfType<CrystalDecisions.Windows.Forms.CrystalReportViewer>().FirstOrDefault();

                //SI NO SE ENCONTRO AL CONTROL VIEWER ENVIA MENSAJE Y PARA EL PROCESO
                if (viewer == null)
                {
                    MessageBox.Show("No se encontró el visor Crystal Reports.", "Error");
                    return;
                }

                // OBTENCION DEL REPORTE YA CARGADO EN EL CRYSTAL VIEWER
                var reporte = viewer.ReportSource as CrystalDecisions.CrystalReports.Engine.ReportDocument;

                // SI EL REPORTE NO ESTA CARGADO CON DATOS ENTONCES ENVIA MENSAJE Y PARA EL PROCESO
                if (reporte == null)
                {
                    MessageBox.Show("No se pudo obtener el reporte.", "Error");
                    return;
                }

                //SI TODO SALIO OK SE EXPORTA Y SE ENVIA MENSAJE DE EXITO
                reporte.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,rutaSalida);

                MessageBox.Show($"Reporte exportado correctamente:\n{rutaSalida}", "Éxito");

                // LIMPIEZA DEL REPORTE Y CIERRE DEL FORMULARIO
                reporte.Close();
                reporte.Dispose();
                frm.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //METODO PARA VISUALIZAR LOS REPORTES
        //CONTROL DE LISTAR - COLUMNA DEL IDREGISTRO - COLUMNAESTADO - DICCIONARIO (CLAVE = "STRING -> ESTADOS", VALOR = "FUNC< INT, FORM" ESTO SE LE VA TENER QUE ENVIAR UN ID Y VA A RETORNAR UN FORMULARIO)
        public static void VisualizarReporte(DataGridView dgv,int columnaId,int columnaEstado, Dictionary<string, Func<int, Form>> formulariosPorEstado)
        {
            try
            {
                //VERIFICACION SI SE SELECCIONO UNA FILA
                if (dgv.CurrentRow != null)
                {
                    //CAPTURA DEL ID
                    int idregistro = Convert.ToInt32(dgv.Rows[dgv.CurrentRow.Index].Cells[columnaId].Value.ToString());

                    //CAPTURA DEL ESTADO DEL REGISTRO
                    string estado = dgv.SelectedCells[columnaEstado].Value.ToString();

                    //VERIFICACION PARA SABER SI EXISTE UN FORMULARIO PARA EL ESTADO INGRESADO
                    if (!formulariosPorEstado.ContainsKey(estado))
                    {
                        MessageBox.Show($"No existe formulario para el estado: {estado}", "Error");
                        return;
                    }

                    //DEL DICCIONARIO formulariosPorEstado, CAPTURA LA FUNCION ASOCIADA AL ESTADO, Y EJECUTALO PASANDOLE EL PARAMETRO idregistro.”
                    Form frm = formulariosPorEstado[estado].Invoke(idregistro);

                    //ABRE EL FORMULARIO Y MUESTRALO
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }    
        }

        //METODO PARA CARGAR LOS REPORTES
        public static void CargarReportes(CrystalReportViewer viewer, ReportDocument reporte,string parametro, int idregistro)
        {
            //METODO PARA PASAR LA CONEXION AL REPORTE
            AplicarConexionReportes(reporte);

            //PARAMETRO NECESARIO PARA LA CARGA DEL REPORTE
            reporte.SetParameterValue(parametro, idregistro);

            //PASAR EL REPORTE AL VIEWER
            viewer.ReportSource = reporte;
        }

    }
}

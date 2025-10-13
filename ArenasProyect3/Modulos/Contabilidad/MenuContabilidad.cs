using ArenasProyect3.Modulos.Mantenimientos;
using ArenasProyect3.Modulos.Resourses;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Contabilidad
{
    public partial class MenuContabilidad : Form
    {
        //VARIABLES GENERALES
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        string ruta = ManGeneral.Manual.manualAreaComercial;
        string maquina = Environment.MachineName;

        //CONSTRUCTOR DEL MANTENIMIENTO - MENU COMERCIAL
        public MenuContabilidad()
        {
            InitializeComponent();
        }

        //CÓDIGO PARA PODER MOSTRAR LA HORA EN VIVO
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHoraVivo.Text = DateTime.Now.ToString("hh:mm:ss tt");
            lblFechaVivo.Text = DateTime.Now.ToLongDateString();
        }

        //Drag Form - LIBRERIA PARA PODER MOVER EL FORMULARIO PRINCIPAL
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int LParam);

        //EVENTO PARA TRAER LAS LIBRERIAS PARA PODER MOVER
        private void panelPrincipal_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        //MINIMIZAR EL MENÚ PRINCIPAL
        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        //CERRAR EL MENÚ PRINCIPAÑ
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        //EVENTO DE INICIO Y DE CARGA DEL MENÚ PRINCIPA
        private void MenuContabilidad_Load(object sender, EventArgs e)
        {
            DatosUsuario();

            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;

            //ReporteMenuProcesos(DesdeFecha.Value, HastaFecha.Value);
        }

        //EVENTOS DE ACCIONES  DEL MENÚ RPINCIPAL------------------------------------------------------------------
        //ABRIR EL PEQUEÑO PANEL DE CIERRE DE SESIÓN Y CONFIGURACIÓN
        private void imgUsuario_Click(object sender, EventArgs e)
        {
            if (panelConfiguracionUsuario.Visible == true)
            {
                panelConfiguracionUsuario.Visible = false;
            }
            else
            {
                panelConfiguracionUsuario.Visible = true;
            }
        }

        //ABRIR EL PANEL DE CONFIGURACIÓN DE LA APLICACIÓN
        private void btnConfiguracion_Click(object sender, EventArgs e)
        {
            panelConfiguracion.Visible = true;
            txtMaquinaHabilitada.Text = maquina;
            txtUsuarioActual.Text = datalistadoBusquedaUusario.SelectedCells[1].Value.ToString() + " " + datalistadoBusquedaUusario.SelectedCells[2].Value.ToString();
        }

        //ABRIR EL PANEL DE LOS DATOS DEL USUARIO ACTUAL
        private void btnDetalleUsuario_Click(object sender, EventArgs e)
        {
            panelDetallesUsuario.Visible = true;
            VisualizarDatosUsuario();
        }

        //ABRIR EL PANEL DE LOS DATOS DEL USUARIO ACTUAL
        private void btnInformacionUsuario_Click(object sender, EventArgs e)
        {
            panelDetallesUsuario.Visible = true;
            panelConfiguracionUsuario.Visible = false;
            VisualizarDatosUsuario();
        }

        //ABRIR EL PANEL DE PERSONALIZACION DE BARRA DE ACCESIBILIDAD
        private void btnPersonalizacionAccesibilidad_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //ABRIR EL PANEL DE GESTION DE GRAFICOS
        private void btnAreaGraficos_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //ABRIRI EL PANEL CON LOS DATOS DE LA TABLA AUDITORA
        private void btnAuditora_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //ABRIR EL PANEL PAR ADMINISTRAR LAS NOTIFICACIONES
        private void btnAdministrarNotificaciones_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //ABRIR EL PANLE PARA ADMINISTRAR USUARIOS
        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //ABRIR ULTIMOS CAMBIOS REALOZADPS
        private void btnUltimosCambios_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //ABRIR EL PANEL DE CONFIRMACIÓN DE VISUALIZACIÓN DE CONTRASEÑA
        private void btnVisualizarContrasena_Click(object sender, EventArgs e)
        {
            panelConfirmacioncontrasena.Visible = true;
        }

        //MOSTRA CONTRASEÑA EN LA ETAPA DE CONFIRMACIÓN
        private void btnConfirmarContrasena_Click(object sender, EventArgs e)
        {
            txtCOntrasenaUsuario.PasswordChar = '\0';
            panelConfirmacioncontrasena.Visible = false;
        }

        //CANCELAR MUESTRA DE LA CONTRASEÑA EN LA ETAPA DE CONFIRMACIÓN
        private void btnCancelarContrasena_Click(object sender, EventArgs e)
        {
            panelConfirmacioncontrasena.Visible = false;
        }

        //CERRAR VENTANA DE DETALLES DEL USUARIO QUE INICIO SESIÓN
        private void btnCerrarDetallesUsuario_Click(object sender, EventArgs e)
        {
            panelDetallesUsuario.Visible = false;
            txtCOntrasenaUsuario.PasswordChar = '*';
        }

        //CERRAR VENTANA DE DETALLES DEL USUARIO QUE INICIO SESIÓN
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            panelDetallesUsuario.Visible = false;
            txtCOntrasenaUsuario.PasswordChar = '*';
        }

        //CERRAR PANEL DE CONFIGURACIÓN DEL MENÚ DEL SISTEMA
        private void btnCerrarConfiguracion_Click(object sender, EventArgs e)
        {
            panelConfiguracion.Visible = false;
        }

        //CERRAR PANEL DE NOVEDADES EL MENÚ DEL SISTEMA
        private void btnCerrarPanelNovedades_Click(object sender, EventArgs e)
        {
            panelNovedades.Visible = false;
        }

        //LINK QUE ABRE LA PÁGINA DE LA EMPRESA HACIENDO CLICK EN EL LOGO
        private void btnPaginaArenas_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.arenassrl.com.pe/");
        }

        //REGRESAR AL INCIO DEL MENÚ Y CERRAR TODAS LOS SUBMENUS
        private void btnHome_Click(object sender, EventArgs e)
        {
            this.panelPrincipalContabilidad.Controls.Clear();
            panelDatos.Visible = true;
        }

        //CERRAR SESIÓN DEL SISTEMA
        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            this.Hide();
            Modulos.Login frm = new Modulos.Login();
            frm.Show();
        }

        //BUSQUEDA DE REPORTES SEGÚN LA FECH ASELECCIONADA - DESDE
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {

        }

        //BUSQUEDA DE REPORTES SEGÚN LA FECH ASELECCIONADA - HASTA
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {

        }

        //BLOQUEO DE ACCESOS------------------------------------------------------
        //BLOQUEO DE ACCESO A REPORTE GENERAL DEL ÁREA
        private void btnReportesContabilidad_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //SALIR DE LA VENTANA DE NOTIFICACIÓN DE PROHIBICIÓN DE ACCESO
        private void btnSalirNotificacionProhibicion_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = false;
        }

        //BLOEQUEO HACI EL REPORTE NÚMERO 1
        private void lblReporte1_Click(object sender, EventArgs e)
        {
            ExportarReporteOpFechas("", DesdeFecha.Value, HastaFecha.Value);
        }

        //BLOEQUEO HACI EL REPORTE NÚMERO 2
        private void lblReporte2_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //BLOEQUEO HACI EL REPORTE NÚMERO 3
        private void lblReporte3_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //BLOEQUEO HACI EL REPORTE NÚMERO 4
        private void lblReporte4_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //BLOEQUEO HACI EL REPORTE NÚMERO 5
        private void lblReporte5_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //BLOEQUEO HACI EL REPORTE NÚMERO 6
        private void lblGeneracionGraficos_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //ACCIONES DE ENTRADA Y SALIDA DE MENUS Y SUBMENUS DEL SISTEMA-------------------
        //ABRIR EL MENÚ Y MANTENIMEINTOS
        private void btnListadoProductos_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //ABRIR EL MENÚ Y MANTENIMIENTO DE ORDENES FENERALES
        private void btnOrdenesGenerales_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //ABRIR EL MENÚ Y MANTENIMIENTO DE REQUERIMIENTOS DE VIAJE
        private void btnAbrirRequerimientosViaje_Click(object sender, EventArgs e)
        {
            if (panelPrincipalContabilidad.Controls.Count == 1)
            {
                panelPrincipalContabilidad.Controls.Clear();
                AbrirFormularios(new RequerimientosVenta.MenuRequerimientoVenta());
            }
            else
            {
                panelPrincipalContabilidad.Controls.Clear();
                AbrirFormularios(new RequerimientosVenta.MenuRequerimientoVenta());
            }
            panelDatos.Visible = false;
        }

        //ABRIR EL MENÚ Y MANTENIMIENTO DE LIQUIDACIONES DE VIAJE
        private void btnAbrirLiquidacionesViaje_Click(object sender, EventArgs e)
        {
            if (panelPrincipalContabilidad.Controls.Count == 1)
            {
                panelPrincipalContabilidad.Controls.Clear();
                AbrirFormularios(new RequerimientosVenta.MenuLiquidacionVenta());
            }
            else
            {
                panelPrincipalContabilidad.Controls.Clear();
                AbrirFormularios(new RequerimientosVenta.MenuLiquidacionVenta());
            }
            panelDatos.Visible = false;
        }

        //ABRIR EL MENÚ Y MANTENIMIENTO DE COSTOS
        private void btnCostos_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }
        //ABRIR EL MENÚ Y MANTENIMIENTO DE REQUERIMEINTOS SIMPLES
        private void brnRequerimientos_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //ABRIR LA OPCION DE MAS DETALLES DE LA NOTIFICACIONES
        private void btnVerDetallesNotificacion_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }
        //--------------------------------------------------------------------------------------------------------

        //CARGA DE METODOS - GENERAL----------------------------------------------------------------------------------
        //CARGA DE DATOS DEL USUARIO QUE INICIO SESIÓN
        public void VisualizarDatosUsuario()
        {
            imgUsuario2.BackgroundImage = null;
            byte[] b = (Byte[])datalistadoBusquedaUusario.SelectedCells[5].Value;
            MemoryStream ms = new MemoryStream(b);
            imgUsuario2.Image = Image.FromStream(ms);

            txtNombreusuario.Text = datalistadoBusquedaUusario.SelectedCells[1].Value.ToString();
            txtApellidousuario.Text = datalistadoBusquedaUusario.SelectedCells[2].Value.ToString();
            txtCorreoUsuario.Text = datalistadoBusquedaUusario.SelectedCells[3].Value.ToString();
            txtCOntrasenaUsuario.Text = datalistadoBusquedaUusario.SelectedCells[4].Value.ToString();
            txtAreaUsuario.Text = datalistadoBusquedaUusario.SelectedCells[7].Value.ToString();
            txtRolusuario.Text = datalistadoBusquedaUusario.SelectedCells[9].Value.ToString();
        }
        //BUSQUEDA DE USUARIO
        public void DatosUsuario()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarUsuarioPorCodigo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idusuario", Program.IdUsuario);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoBusquedaUusario.DataSource = dt;
            con.Close();

            imgUsuario.BackgroundImage = null;
            byte[] b = (Byte[])datalistadoBusquedaUusario.SelectedCells[5].Value;
            MemoryStream ms = new MemoryStream(b);
            imgUsuario.Image = Image.FromStream(ms);

            lblusuarioActual.Text = datalistadoBusquedaUusario.SelectedCells[1].Value.ToString() + " " + datalistadoBusquedaUusario.SelectedCells[2].Value.ToString();
            Program.NombreUsuarioCompleto = datalistadoBusquedaUusario.SelectedCells[1].Value.ToString() + " " + datalistadoBusquedaUusario.SelectedCells[2].Value.ToString();
        }

        //REPORTE DE MENUS
        public void ReporteMenuComercial(DateTime desde, DateTime hasta)
        {
            SqlConnection conp = new SqlConnection();
            conp.ConnectionString = Conexion.ConexionMaestra.conexion;

            SqlCommand cmdp = new SqlCommand();
            cmdp = new SqlCommand("ReporteMenuComercial", conp);
            cmdp.CommandType = CommandType.StoredProcedure;
            cmdp.Parameters.AddWithValue("@desdeFecha", desde);
            cmdp.Parameters.AddWithValue("@hastaFecha", hasta);


            SqlParameter totalRequerimeintosPendientes = new SqlParameter("@totalRequerimeintosPendientes", 0); totalRequerimeintosPendientes.Direction = ParameterDirection.Output;
            SqlParameter totalLiquidacionesPendientes = new SqlParameter("@totalLiquidacionesPendientes", 0); totalLiquidacionesPendientes.Direction = ParameterDirection.Output;
            SqlParameter totalActasGenradas = new SqlParameter("@totalActasGeneradas", 0); totalActasGenradas.Direction = ParameterDirection.Output;
            SqlParameter totalRequerimeintosAtrasados = new SqlParameter("@totalRequerimeintosAtrasados", 0); totalRequerimeintosAtrasados.Direction = ParameterDirection.Output;

            cmdp.Parameters.Add(totalRequerimeintosPendientes);
            cmdp.Parameters.Add(totalLiquidacionesPendientes);
            cmdp.Parameters.Add(totalActasGenradas);
            cmdp.Parameters.Add(totalRequerimeintosAtrasados);

            conp.Open();
            cmdp.ExecuteNonQuery();

            lblReporteRequerimeintosPendietens.Text = cmdp.Parameters["@totalRequerimeintosPendientes"].Value.ToString();
            lblReporteLiquidacionesPendientes.Text = cmdp.Parameters["@totalLiquidacionesPendientes"].Value.ToString();
            lblReporteActasGeneradas.Text = cmdp.Parameters["@totalActasGeneradas"].Value.ToString();
            lblReporteRequerimeintosAtrasados.Text = cmdp.Parameters["@totalRequerimeintosAtrasados"].Value.ToString();

            conp.Close();
        }

        //CARGA DE LAS NOVEDADES Y VALIDACIÓN DEL TIEMPO DE APARICIÓN
        public void CargarNovedades()
        {
            //CARGA DE PROCEDIMIENTO ALMACENADO
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            da = new SqlDataAdapter("SELECT IdEstadoSistemaInicio, Descripcion, VersionSsitema, FechaInstalacionSsitema, NuevasFuncionesNovedades, FechaAparicion FROM EstadoSistemaInicio WHERE Estado = 1", con);
            da.Fill(dt);
            datalistadoNovedades.DataSource = dt;
            con.Close();
            //SI NO HAY FILAS O SI LA CONSULTA NO TIENE RESULTADO
            if (datalistadoNovedades.RowCount > 0)
            {
                //SE CAPTURA LA FECHA DE TÉRMINO DEL MENSAJE DE MI LISTADO YA CARGADO GRACIAS A LA CONSULTA
                DateTime fechaTerminoNotificacion = Convert.ToDateTime(datalistadoNovedades.SelectedCells[5].Value.ToString());
                //SI LA FECHA DE TÉRMINO ES MENOR A LA FECHA ACTUAL
                if (fechaTerminoNotificacion > DateTime.Now)
                {
                    //CAPTURA DE VARIABLES PARA LA MUESTRA DE ESTAS
                    string versionSistema = datalistadoNovedades.SelectedCells[2].Value.ToString();
                    string fechaInstalacion = datalistadoNovedades.SelectedCells[3].Value.ToString();
                    string mensajeNovedades = datalistadoNovedades.SelectedCells[4].Value.ToString();
                    //HABILITAR LA VISIBILIDAD DEL PANEL
                    panelNovedades.Visible = true;
                    //ASIGNAR LAS VARIABLES CAPTURADAS A LOS ELEMNETOS DEL PANEL DE NOVEDADES
                    lblVersionSistema.Text = versionSistema;
                    lblFechaInstalacion.Text = fechaInstalacion;
                    lblNovedadesNuevasFunciones.Text = mensajeNovedades;
                }
                else
                {
                    //OCULTAR EL PANEL DE NOVEDADES
                    panelNovedades.Visible = false;
                }
            }
            else
            {
                //OCULTAR EL PANEL DE NOVEDADES
                panelNovedades.Visible = false;
            }
        }

        //ACIONES DE ENTRAR Y SALIR DE LOS FORMULARIOS Y DE CIERRE DE SESION
        public void AbrirFormularios(object formFormulario)
        {
            Form frm = formFormulario as Form;
            frm.TopLevel = false;
            frm.Dock = DockStyle.Fill;
            this.panelPrincipalContabilidad.Controls.Add(frm);
            this.panelPrincipalContabilidad.Tag = frm;
            frm.Show();
        }

        //VIZUALIZAR DATOS EXCEL COMPLETO--------------------------------------------------------------------
        public void MostrarExcelCompleto()
        {
            datalistadoReporteOPFechasExcel.Rows.Clear();

            foreach (DataGridViewRow dgv in datalistadoReporteOPFechas.Rows)
            {
                string fechaOP = Convert.ToDateTime(dgv.Cells[0].Value).ToString("yyyy/MM/dd");
                string numeroOP = dgv.Cells[1].Value.ToString();
                string unidadOP = dgv.Cells[2].Value.ToString();
                string descripcionProducto = dgv.Cells[3].Value.ToString();

                string fechaCulminada = "";
                if (dgv.Cells[4].Value != null && DateTime.TryParse(dgv.Cells[4].Value.ToString(), out DateTime fecha))
                {
                    fechaCulminada = fecha.ToString("yyyy/MM/dd");
                }
                else
                {
                    fechaCulminada = "NULL"; // o cualquier valor por defecto que prefieras
                }

                string cantidadRealizada = dgv.Cells[5].Value.ToString();
                string estado = dgv.Cells[6].Value.ToString();

                datalistadoReporteOPFechasExcel.Rows.Add(new[] { fechaOP, numeroOP, unidadOP, descripcionProducto, fechaCulminada, cantidadRealizada, estado});
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------

        //FUNCION PAARA EXPORTAR A EXCEL MI LISTADO COMPLETO
        public void ExportarReporteOpFechas(string cliente, DateTime inicio, DateTime fin)
        {
            //try
            //{
            //    DataTable dt = new DataTable();
            //    SqlConnection con = new SqlConnection();
            //    con.ConnectionString = Conexion.ConexionMaestra.conexionSoft;
            //    con.Open();
            //    SqlCommand cmd = new SqlCommand();
            //    cmd = new SqlCommand("SP_A_REPORTE_OP_PAOLA", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@DESCLI", cliente);
            //    cmd.Parameters.AddWithValue("@DESDE", inicio);
            //    cmd.Parameters.AddWithValue("@HASTA", fin);
            //    SqlDataAdapter da = new SqlDataAdapter(cmd);
            //    da.Fill(dt);
            //    datalistadoReporteOPFechas.DataSource = dt;
            //    con.Close();

            //    MostrarExcelCompleto();

            //    SLDocument sl = new SLDocument();
            //    SLStyle style = new SLStyle();
            //    SLStyle styleC = new SLStyle();

            //    //COLUMNAS
            //    sl.SetColumnWidth(1, 15);
            //    sl.SetColumnWidth(2, 15);
            //    sl.SetColumnWidth(3, 15);
            //    sl.SetColumnWidth(4, 100);
            //    sl.SetColumnWidth(5, 15);
            //    sl.SetColumnWidth(6, 15);
            //    sl.SetColumnWidth(7, 15);

            //    //CABECERA
            //    style.Font.FontSize = 11;
            //    style.Font.Bold = true;
            //    style.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            //    style.Alignment.WrapText = true;
            //    style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Beige, System.Drawing.Color.Beige);
            //    style.Border.LeftBorder.BorderStyle = BorderStyleValues.Hair;
            //    style.Border.RightBorder.BorderStyle = BorderStyleValues.Hair;
            //    style.Border.BottomBorder.BorderStyle = BorderStyleValues.Hair;
            //    style.Border.TopBorder.BorderStyle = BorderStyleValues.Hair;

            //    //FILAS
            //    styleC.Font.FontSize = 10;
            //    styleC.Alignment.Horizontal = HorizontalAlignmentValues.Center;

            //    styleC.Border.LeftBorder.BorderStyle = BorderStyleValues.Hair;
            //    styleC.Border.RightBorder.BorderStyle = BorderStyleValues.Hair;
            //    styleC.Border.BottomBorder.BorderStyle = BorderStyleValues.Hair;
            //    styleC.Border.TopBorder.BorderStyle = BorderStyleValues.Hair;

            //    int ic = 1;
            //    foreach (DataGridViewColumn column in datalistadoReporteOPFechasExcel.Columns)
            //    {
            //        sl.SetCellValue(1, ic, column.HeaderText.ToString());
            //        sl.SetCellStyle(1, ic, style);
            //        ic++;
            //    }

            //    int ir = 2;
            //    foreach (DataGridViewRow row in datalistadoReporteOPFechasExcel.Rows)
            //    {
            //        sl.SetCellValue(ir, 1, row.Cells[0].Value.ToString());
            //        sl.SetCellValue(ir, 2, row.Cells[1].Value.ToString());
            //        sl.SetCellValue(ir, 3, row.Cells[2].Value.ToString());
            //        sl.SetCellValue(ir, 4, row.Cells[3].Value.ToString());
            //        sl.SetCellValue(ir, 5, row.Cells[4].Value.ToString());
            //        sl.SetCellValue(ir, 6, row.Cells[5].Value.ToString());
            //        sl.SetCellValue(ir, 7, row.Cells[6].Value.ToString());
            //        sl.SetCellStyle(ir, 1, styleC);
            //        sl.SetCellStyle(ir, 2, styleC);
            //        sl.SetCellStyle(ir, 3, styleC);
            //        sl.SetCellStyle(ir, 4, styleC);
            //        sl.SetCellStyle(ir, 5, styleC);
            //        sl.SetCellStyle(ir, 6, styleC);
            //        sl.SetCellStyle(ir, 7, styleC);
            //        ir++;
            //    }

            //    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //    sl.SaveAs(desktopPath + @"\Reporte de ordenes de producción.xlsx");
            //    MessageBox.Show("Se exportó los datos a un archivo de Microsoft Excel en la siguiente ubicación: " + desktopPath, "Validación del Sistema", MessageBoxButtons.OK);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        //MOSTRARA MI MANUAL DE USUARIO
        private void btnAbrirManual_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }
        //--------------------------------------------------------------------------------------------------------
    }
}

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

namespace ArenasProyect3.Modulos.Logistica
{
    public partial class MenuLogistcia : Form
    {
        //VARIABLES GENERALES
        string maquina = Environment.MachineName;
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        string ruta = ManGeneral.Manual.manualAreaLogistica;

        //CONSTRUCTOR DEL MANTENIMIENTO - MENU COMERCIAL
        public MenuLogistcia()
        {
            InitializeComponent();
        }

        //CÓDIGO PARA PODER MOSTRAR LA HORA EN VIVO
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHoraVivo.Text = DateTime.Now.ToString("H:mm:ss tt");
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

        //EVENTO DE INICIO Y DE CARGA DEL MENÚ PRINCIPAL
        private void MenuLogistcia_Load(object sender, EventArgs e)
        {
            //FUNCION PARA CARGAR DATOS DEL USUARIO
            DatosUsuario();
            CargarNovedades();
            CargarReportePedidos();
            CargarReporteOrdenProduccion();

            //AJUSTAR FECHAS DESDE EL PRIEMRO AL ULTIMO DEIA DEL MES
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;

            //FUNCION PARA COLOCAR DATOS RELEVANTES
            //ReporteMenuComercial(DesdeFecha.Value, HastaFecha.Value);
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
            //if (panelPrincipalComercial.Controls.Count == 1)
            //{
            //    panelPrincipalComercial.Controls.Clear();
            //    AbrirFormularios(new MenuReportes());
            //}
            //else
            //{
            //    panelPrincipalComercial.Controls.Clear();
            //    AbrirFormularios(new MenuReportes());
            //}
            //panelDatos.Visible = false;
        }

        //ABRIR EL PANEL PAR ADMINISTRAR LAS NOTIFICACIONES
        private void btnAdministrarNotificaciones_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //ABRIR EL PANEL PAR ADMINISTRAR LAS NOTIFICACIONES
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
        private void btnConfirmarContrasenaF_Click(object sender, EventArgs e)
        {
            txtCOntrasenaUsuario.PasswordChar = '\0';
            panelConfirmacioncontrasena.Visible = false;
        }

        //CANCELAR MUESTRA DE LA CONTRASEÑA EN LA ETAPA DE CONFIRMACIÓN
        private void btnCancelarContrasenaF_Click(object sender, EventArgs e)
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
        private void btnAceptarF_Click(object sender, EventArgs e)
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
            this.panelPrincipalLogistica.Controls.Clear();
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
            CargarReportePedidos();
        }

        //BUSQUEDA DE REPORTES SEGÚN LA FECH ASELECCIONADA - HASTA
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            CargarReportePedidos();
        }

        //BLOQUEO DE ACCESOS------------------------------------------------------
        //BLOQUEO DE ACCESO A REPORTE GENERAL DEL ÁREA
        //SALIR DE LA VENTANA DE NOTIFICACIÓN DE PROHIBICIÓN DE ACCESO
        private void btnSalirNotificacionProhibicionF_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = false;
        }

        //BLOEQUEO HACI EL REPORTE NÚMERO 1
        private void lblReportes1_Click(object sender, EventArgs e)
        {
            MostrarExcelPedido();

            SLDocument sl = new SLDocument();
            SLStyle style = new SLStyle();
            SLStyle styleC = new SLStyle();

            //COLUMNAS
            sl.SetColumnWidth(1, 15);
            sl.SetColumnWidth(2, 20);
            sl.SetColumnWidth(3, 20);
            sl.SetColumnWidth(4, 50);
            sl.SetColumnWidth(5, 18);
            sl.SetColumnWidth(6, 15);
            sl.SetColumnWidth(7, 15);
            sl.SetColumnWidth(8, 25);
            sl.SetColumnWidth(9, 25);
            sl.SetColumnWidth(10, 35);

            //CABECERA
            style.Font.FontSize = 11;
            style.Font.Bold = true;
            style.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Beige, System.Drawing.Color.Beige);
            style.Border.LeftBorder.BorderStyle = BorderStyleValues.Hair;
            style.Border.RightBorder.BorderStyle = BorderStyleValues.Hair;
            style.Border.BottomBorder.BorderStyle = BorderStyleValues.Hair;
            style.Border.TopBorder.BorderStyle = BorderStyleValues.Hair;

            //FILAS
            styleC.Font.FontSize = 10;
            styleC.Alignment.Horizontal = HorizontalAlignmentValues.Center;

            styleC.Border.LeftBorder.BorderStyle = BorderStyleValues.Hair;
            styleC.Border.RightBorder.BorderStyle = BorderStyleValues.Hair;
            styleC.Border.BottomBorder.BorderStyle = BorderStyleValues.Hair;
            styleC.Border.TopBorder.BorderStyle = BorderStyleValues.Hair;

            int ic = 1;
            foreach (DataGridViewColumn column in datalistadoExcelPedido.Columns)
            {
                sl.SetCellValue(1, ic, column.HeaderText.ToString());
                sl.SetCellStyle(1, ic, style);
                ic++;
            }

            int ir = 2;
            foreach (DataGridViewRow row in datalistadoExcelPedido.Rows)
            {
                sl.SetCellValue(ir, 1, row.Cells[0].Value.ToString());
                sl.SetCellValue(ir, 2, row.Cells[1].Value.ToString());
                sl.SetCellValue(ir, 3, row.Cells[2].Value.ToString());
                sl.SetCellValue(ir, 4, row.Cells[3].Value.ToString());
                sl.SetCellValue(ir, 5, row.Cells[4].Value.ToString());
                sl.SetCellValue(ir, 6, row.Cells[5].Value.ToString());
                sl.SetCellValue(ir, 7, row.Cells[6].Value.ToString());
                sl.SetCellValue(ir, 8, row.Cells[7].Value.ToString());
                sl.SetCellValue(ir, 9, row.Cells[8].Value.ToString());
                sl.SetCellValue(ir, 10, row.Cells[9].Value.ToString());
                sl.SetCellStyle(ir, 1, styleC);
                sl.SetCellStyle(ir, 2, styleC);
                sl.SetCellStyle(ir, 3, styleC);
                sl.SetCellStyle(ir, 4, styleC);
                sl.SetCellStyle(ir, 5, styleC);
                sl.SetCellStyle(ir, 6, styleC);
                sl.SetCellStyle(ir, 7, styleC);
                sl.SetCellStyle(ir, 8, styleC);
                sl.SetCellStyle(ir, 9, styleC);
                sl.SetCellStyle(ir, 10, styleC);
                ir++;
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            sl.SaveAs(desktopPath + @"\Reporte de pedidos.xlsx");
            MessageBox.Show("Se exportó los datos a un archivo de Microsoft Excel en la siguiente ubicación: " + desktopPath, "Validación del Sistema", MessageBoxButtons.OK);
        }

        //BLOEQUEO HACI EL REPORTE NÚMERO 2
        private void lblReportes2_Click(object sender, EventArgs e)
        {
            
        }

        //BLOEQUEO HACI EL REPORTE NÚMERO 3
        private void lblReportes3_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //BLOEQUEO HACI EL REPORTE NÚMERO 4
        private void lblReportes4_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //BLOEQUEO HACI EL REPORTE NÚMERO 5
        private void lblReportes5_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //BLOEQUEO HACI EL REPORTE NÚMERO 6
        private void lblGeneracionGraficos_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //ACCIONES DE ENTRADA Y SALIDA DE MENUS Y SUBMENUS DEL SISTEMA-------------------
        //ABRIR EL MENÚ Y MANTENIMEINTO
        private void btnOrdenesLogistica_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //ABRIR EL MENÚ Y MANTENIMENTO ALMACEN
        private void btnAlmacen_Click(object sender, EventArgs e)
        {
            if (panelPrincipalLogistica.Controls.Count == 1)
            {
                panelPrincipalLogistica.Controls.Clear();
                AbrirFormularios(new Logistica.Almacen.MenuAccionesAlmacen());
            }
            else
            {
                panelPrincipalLogistica.Controls.Clear();
                AbrirFormularios(new Logistica.Almacen.MenuAccionesAlmacen());
            }
            panelDatos.Visible = false;
        }

        //ABRIR EL MENÚ Y MANTENIMENTO COMPRAS
        private void btnCompras_Click(object sender, EventArgs e)
        {
            if (panelPrincipalLogistica.Controls.Count == 1)
            {
                panelPrincipalLogistica.Controls.Clear();
                AbrirFormularios(new Logistica.Compras.MenuAccionesCompra());
            }
            else
            {
                panelPrincipalLogistica.Controls.Clear();
                AbrirFormularios(new Logistica.Compras.MenuAccionesCompra());
            }
            panelDatos.Visible = false;
        }

        //ABRIR EL MENÚ Y MANTENIMENTO DESPACHOS
        private void btnDespacho_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //ABRIR EL MENÚ Y MANTENIMENTO LINEAS
        private void btnLineas_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //ABRIR EL MENÚ Y MANTENIMENTO REQUERIMIENTOS
        private void btnRequerimientos_Click(object sender, EventArgs e)
        {
            if (panelPrincipalLogistica.Controls.Count == 1)
            {
                panelPrincipalLogistica.Controls.Clear();
                AbrirFormularios(new Mantenimientos.MenuRequerimientoSimple());
            }
            else
            {
                panelPrincipalLogistica.Controls.Clear();
                AbrirFormularios(new Mantenimientos.MenuRequerimientoSimple());
            }
            panelDatos.Visible = false;
        }

        //ABRIR EL MENÚ Y MANTENIMENTO REPORTES
        private void btnReportes_Click(object sender, EventArgs e)
        {
            panelProhibicion.Visible = true;
        }

        //ABRIR LA OPCION DE MAS DETALLES DE LA NOTIFICACIONES
        private void btnVerDetallesNotificaciones_Click(object sender, EventArgs e)
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

        //BUSQUEDA DE PEDIDOS POR FECHA
        public void CargarReportePedidos()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexionSoft;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("SP_LISTADO_GENERA_PEDIDO_FECHAS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DESCLI", "");
            cmd.Parameters.AddWithValue("@DESDE", DesdeFecha.Value);
            cmd.Parameters.AddWithValue("@HASTA", HastaFecha.Value);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoBusquedaPedidoPorFecha_Externo.DataSource = dt;
            con.Close();
            lblReportePedidos.Text = Convert.ToString(datalistadoBusquedaPedidoPorFecha_Externo.RowCount);
        }

        //BUSQUEDA DE PEDIDOS POR FECHA
        public void CargarReporteOrdenProduccion()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexionSoft;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("SP_LISTADO_ORDEN_PRODUCCION_FECHAS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DESCLI", "");
            cmd.Parameters.AddWithValue("@DESDE", DesdeFecha.Value);
            cmd.Parameters.AddWithValue("@HASTA", HastaFecha.Value);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoBusquedaOrdenProduccionPorFecha_Externo.DataSource = dt;
            con.Close();
            lblReporteOrdenesProduccion.Text = Convert.ToString(datalistadoBusquedaOrdenProduccionPorFecha_Externo.RowCount);
        }

        //VIZUALIZAR DATOS EXCEL PEDIDO--------------------------------------------------------------------
        public void MostrarExcelPedido()
        {
            datalistadoExcelPedido.Rows.Clear();

            foreach (DataGridViewRow dgv in datalistadoBusquedaPedidoPorFecha_Externo.Rows)
            {
                string numeroPedido = dgv.Cells[0].Value.ToString();
                string fechaInicio = dgv.Cells[1].Value.ToString();
                string fechaVencimiento = dgv.Cells[2].Value.ToString();
                string cliente = dgv.Cells[3].Value.ToString();
                string total = dgv.Cells[4].Value.ToString();
                string numeroCotizacion = dgv.Cells[5].Value.ToString();
                string cantidadItems = dgv.Cells[6].Value.ToString();
                string unidad = dgv.Cells[7].Value.ToString();
                string ordenCOmpra = dgv.Cells[8].Value.ToString();
                string estado = dgv.Cells[16].Value.ToString();

                datalistadoExcelPedido.Rows.Add(new[] { numeroPedido, fechaInicio, fechaVencimiento, cliente, total, numeroCotizacion, cantidadItems, unidad, ordenCOmpra, estado });
            }
        }

        //VIZUALIZAR DATOS EXCEL PEDIDO--------------------------------------------------------------------
        public void MostrarExcelOrdenProduccion()
        {
            datalistadoExcelOrdenProduccion.Rows.Clear();

            foreach (DataGridViewRow dgv in datalistadoBusquedaOrdenProduccionPorFecha_Externo.Rows)
            {
                string numeroOrdenProduccion = dgv.Cells[0].Value.ToString();
                string fechaInicio = dgv.Cells[1].Value.ToString();
                string fechaEntrega = dgv.Cells[2].Value.ToString();
                string cliente = dgv.Cells[3].Value.ToString();
                string unidad = dgv.Cells[4].Value.ToString();
                string item = dgv.Cells[5].Value.ToString();
                string descripcionProducto = dgv.Cells[6].Value.ToString();
                string cantidad = dgv.Cells[7].Value.ToString();
                string color = dgv.Cells[8].Value.ToString();
                string numeroPedido = dgv.Cells[9].Value.ToString();
                string cantidadRealizada = dgv.Cells[10].Value.ToString();
                string estado = dgv.Cells[11].Value.ToString();

                datalistadoExcelOrdenProduccion.Rows.Add(new[] { numeroOrdenProduccion, fechaInicio, fechaEntrega, cliente, unidad, item, descripcionProducto, cantidad, color, numeroPedido, cantidadRealizada, estado });
            }
        }

        ////REPORTE DE MENUS
        //public void ReporteMenuComercial(DateTime desde, DateTime hasta)
        //{
        //    SqlConnection conp = new SqlConnection();
        //    conp.ConnectionString = Conexion.ConexionMaestra.conexion;

        //    SqlCommand cmdp = new SqlCommand();
        //    cmdp = new SqlCommand("ReporteMenuComercial", conp);
        //    cmdp.CommandType = CommandType.StoredProcedure;
        //    cmdp.Parameters.AddWithValue("@desdeFecha", desde);
        //    cmdp.Parameters.AddWithValue("@hastaFecha", hasta);


        //    SqlParameter totalRequerimeintosPendientes = new SqlParameter("@totalRequerimeintosPendientes", 0); totalRequerimeintosPendientes.Direction = ParameterDirection.Output;
        //    SqlParameter totalRequeSinLiqui = new SqlParameter("@totalRequeSinLiqui", 0); totalRequeSinLiqui.Direction = ParameterDirection.Output;
        //    SqlParameter totalActasGenradas = new SqlParameter("@totalActasGeneradas", 0); totalActasGenradas.Direction = ParameterDirection.Output;
        //    SqlParameter totalRequerimeintosAtrasados = new SqlParameter("@totalRequerimeintosAtrasados", 0); totalRequerimeintosAtrasados.Direction = ParameterDirection.Output;

        //    cmdp.Parameters.Add(totalRequerimeintosPendientes);
        //    cmdp.Parameters.Add(totalRequeSinLiqui);
        //    cmdp.Parameters.Add(totalActasGenradas);
        //    cmdp.Parameters.Add(totalRequerimeintosAtrasados);

        //    conp.Open();
        //    cmdp.ExecuteNonQuery();

        //    lblReporteRequerimeintosPendietens.Text = cmdp.Parameters["@totalRequerimeintosPendientes"].Value.ToString();
        //    lblReporteLiquidacionesPendientes.Text = cmdp.Parameters["@totalRequeSinLiqui"].Value.ToString();
        //    lblReporteActasGeneradas.Text = cmdp.Parameters["@totalActasGeneradas"].Value.ToString();
        //    lblReporteRequerimeintosAtrasados.Text = cmdp.Parameters["@totalRequerimeintosAtrasados"].Value.ToString();

        //    conp.Close();
        //}


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
            this.panelPrincipalLogistica.Controls.Add(frm);
            this.panelPrincipalLogistica.Tag = frm;
            frm.Show();
        }

        //BOTON DE ORDENES LOGISTICAS
        private void btnOrdenesLogistica_MouseHover(object sender, EventArgs e)
        {
            btnOrdenesLogistica.BackColor = System.Drawing.Color.FromArgb(243, 243, 243);
        }

        //LEVANTAR MOUSE
        private void btnOrdenesLogistica_MouseLeave(object sender, EventArgs e)
        {
            btnOrdenesLogistica.BackColor = System.Drawing.Color.White;
        }

        //BOTON DE ALMACENES
        private void btnAlmacen_MouseHover(object sender, EventArgs e)
        {
            btnAlmacen.BackColor = System.Drawing.Color.FromArgb(243, 243, 243);
        }

        //LEVANTAR MOUSE
        private void btnAlmacen_MouseLeave(object sender, EventArgs e)
        {
            btnAlmacen.BackColor = System.Drawing.Color.White;
        }

        //BOTON DE COMPRAS
        private void btnCompras_MouseHover(object sender, EventArgs e)
        {
            btnCompras.BackColor = System.Drawing.Color.FromArgb(243, 243, 243);
        }

        //LEVANTAR MOUSE
        private void btnCompras_MouseLeave(object sender, EventArgs e)
        {
            btnCompras.BackColor = System.Drawing.Color.White;
        }

        //BOTON DE DESPACHOS
        private void btnDespacho_MouseHover(object sender, EventArgs e)
        {
            btnDespacho.BackColor = System.Drawing.Color.FromArgb(243, 243, 243);
        }

        //LEVANTAR MOUSE
        private void btnDespacho_MouseLeave(object sender, EventArgs e)
        {
            btnDespacho.BackColor = System.Drawing.Color.White;
        }

        //BOTON DE LINEAS
        private void btnLineas_MouseHover(object sender, EventArgs e)
        {
            btnLineas.BackColor = System.Drawing.Color.FromArgb(243, 243, 243);
        }

        //LEVANTAR MOUSE
        private void btnLineas_MouseLeave(object sender, EventArgs e)
        {
            btnLineas.BackColor = System.Drawing.Color.White;
        }

        //BOTON DE REQUERIMIENTO
        private void btnRequerimientos_MouseHover(object sender, EventArgs e)
        {
            btnRequerimientos.BackColor = System.Drawing.Color.FromArgb(243, 243, 243);
        }

        //LEVANTAR MOUSE
        private void btnRequerimientos_MouseLeave(object sender, EventArgs e)
        {
            btnRequerimientos.BackColor = System.Drawing.Color.White;
        }

        //BOTON DE REPORTES
        private void btnReportes_MouseHover(object sender, EventArgs e)
        {
            btnReportes.BackColor = System.Drawing.Color.FromArgb(243, 243, 243);
        }

        //LEVANTAR MOUSE
        private void btnReportes_MouseLeave(object sender, EventArgs e)
        {
            btnReportes.BackColor = System.Drawing.Color.White;
        }

        private void btnAbrirManual_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }
    }
}

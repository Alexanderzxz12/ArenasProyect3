using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SpreadsheetLight;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Logistica.Compras
{
    public partial class ListadoOrdenesCompra : Form
    {
        //VARIABLES GLOBALES
        int valorEstadoBusqueda = 0;
        private Cursor curAnterior = null;
        string area = "";
        string cantidadOrdenesCompra = "";
        string cantidadOrdenesCompra2 = "";
        string codigoOrdenCOmpra = "";

        //CONSTRUCTOR DE MI FORMULARIO
        public ListadoOrdenesCompra()
        {
            InitializeComponent();
        }

        //VIZUALIZAR DATOS EXCEL--------------------------------------------------------------------
        public void MostrarExcel()
        {
            datalistadoExcel.Rows.Clear();

            foreach (DataGridViewRow dgv in datalistadoTodasOC.Rows)
            {
                string numeroOC = dgv.Cells[2].Value.ToString();
                string fechaIngreso = dgv.Cells[3].Value.ToString();
                string fechaEstimada = dgv.Cells[4].Value.ToString();
                string proveedor = dgv.Cells[5].Value.ToString();
                string formaPago = dgv.Cells[6].Value.ToString();
                string moneda = dgv.Cells[7].Value.ToString();
                string estadoItems = dgv.Cells[8].Value.ToString();
                string observaciones = dgv.Cells[9].Value.ToString();
                string total = dgv.Cells[10].Value.ToString();
                string estadoOC = dgv.Cells[11].Value.ToString();

                datalistadoExcel.Rows.Add(new[] { numeroOC, fechaIngreso, fechaEstimada, proveedor, formaPago, moneda, estadoItems, observaciones, total, estadoOC });
            }
        }

        //INICIO Y CARGA INICIAL DE LAS ORDENES DE COMPRA - CONSTRUCTOR--------------------------------------------------------------------------------------
        private void ListadoOrdenesCompra_Load(object sender, EventArgs e)
        {
            //AJUSTAR FECHAS AL INICIO DEL MES Y FINAL DEL MES
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);
            //ASIGNARLE LAS VARIABLES YA CARGADAS A MIS DateTimerPicker
            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            datalistadoTodasOC.DataSource = null;
        }

        //VER OC POR CODIGO
        public void BuscarOrdenCompra(int ordenCompra)
        {
            //PROCEDIMIENTO ALMACENADO PARA LISTAR TODOS LOS DATOS DE LA ORDEN DE COMPRA SELECIOANADA
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarOrdenesCompra", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ordenCompra", ordenCompra);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoOrdenCompra.DataSource = dt;
            con.Close();
        }

        //VER DETALLES DE LA OC POR CODIGO
        public void BuscarDetallesOrdenCompra(int ordenCompra)
        {
            //PROCEDIMIENTO ALMACENADO PARA LISTAR LOS DETALLES DE LA ORDEN DE COMPRA
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarDetallesOrdenesCompra", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ordenCompra", ordenCompra);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoOrdenCompra.DataSource = dt;
            con.Close();
        }

        //LISTADO DE REQUERIMIENTOS Y SELECCIÓN DE PDF Y ESTADO DE LIQUIDACIÓN---------------------------------------------------------------
        //MOSTRAR REQUERIMIENTOS AL INCIO Y POR FECHAS
        public void MostrarOrdenesCompra(DateTime fechaInicio, DateTime fechaTermino)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarOrdenesCompraPorFecha", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoTodasOC.DataSource = dt;
            con.Close();
            RedimensionarListado(datalistadoTodasOC);
        }

        //MOSTRAR REQUERIMIENTOS POR RESPONSABLE DE ACUERDO A LAS FECHAS SELECCIONADAS
        public void MostrarOrdenesCompraesponsable(string proveedor, DateTime fechaInicio, DateTime fechaTermino)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarOrdenesCompraPorProveedor", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@proveedor", proveedor);
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoTodasOC.DataSource = dt;
            con.Close();
            RedimensionarListado(datalistadoTodasOC);
        }

        //REDIMENSIONAR MIS LISTADOS DE LA OC
        public void RedimensionarListado(DataGridView DGV)
        {
            //SE REDIMENSIONA EL TAMAÑO DE CADA COLUMNA DE MI LISTADO DE ORDENES DE COMPRA
            DGV.Columns[2].Width = 85;
            DGV.Columns[3].Width = 85;
            DGV.Columns[4].Width = 85;
            DGV.Columns[5].Width = 300;
            DGV.Columns[6].Width = 150;
            DGV.Columns[7].Width = 145;
            DGV.Columns[8].Width = 90;
            DGV.Columns[9].Width = 150;
            DGV.Columns[10].Width = 65;
            DGV.Columns[11].Width = 120;
            //SE QUITA LAS COLUMNAS QUE NO SON RELEVANTES PARA EL USUARIO
            DGV.Columns[1].Visible = false;
            DGV.Columns[12].Visible = false;
            //CARGAR LOS COLORES DE ACUERDO A SU ESTADO
            ColoresListadoOrdenCompra();

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //COLOREAR REGISTROS
        public void ColoresListadoOrdenCompra()
        {
            try
            {
                for (var i = 0; i <= datalistadoTodasOC.RowCount - 1; i++)
                {
                    if (datalistadoTodasOC.Rows[i].Cells[11].Value.ToString() == "SIN CONFORMIDAD")
                    {
                        //POR ATENDER -> 0
                        datalistadoTodasOC.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else if (datalistadoTodasOC.Rows[i].Cells[11].Value.ToString() == "CONFORME")
                    {
                        //COMPLETADO -> 1
                        datalistadoTodasOC.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                    }
                    else
                    {
                        //SI NO HAY NINGUN CASO
                        datalistadoTodasOC.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //VER DETALLES(ITEMS) DE MI ORDEN DE COMPRA
        public void CargarDetallesItems(int idOrdenCompra)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("ListarOdenCompraItemsGeneralLogistica_SP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idOrdenCompra", idOrdenCompra);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoDetallesOrdenCompra.DataSource = dt;
                con.Close();
                //NO MOSTRAR LAS COLUMNAS QUE NO SEAN DE REELEVANCIA PARA EL USUARIO
                datalistadoDetallesOrdenCompra.Columns[1].Visible = false;
                datalistadoDetallesOrdenCompra.Columns[7].Visible = false;
                //REDIMENSIONAR LAS COLUMNAS SEGUN EL TEMAÑO REQUERIDO
                datalistadoDetallesOrdenCompra.Columns[0].Width = 50;
                datalistadoDetallesOrdenCompra.Columns[2].Width = 100;
                datalistadoDetallesOrdenCompra.Columns[3].Width = 350;
                datalistadoDetallesOrdenCompra.Columns[4].Width = 100;
                datalistadoDetallesOrdenCompra.Columns[5].Width = 90;
                datalistadoDetallesOrdenCompra.Columns[6].Width = 90;
                datalistadoDetallesOrdenCompra.Columns[8].Width = 120;
                //CARGAR METODO PARA COLOREAR
                ColoresListadoItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //COLOREAR REGISTROS (ITEMS)
        public void ColoresListadoItems()
        {
            try
            {
                for (var i = 0; i <= datalistadoDetallesOrdenCompra.RowCount - 1; i++)
                {
                    decimal cantidadTotal = 0;
                    cantidadTotal = Convert.ToDecimal(datalistadoDetallesOrdenCompra.Rows[i].Cells[5].Value.ToString());

                    if (cantidadTotal > Convert.ToDecimal(datalistadoDetallesOrdenCompra.Rows[i].Cells[6].Value.ToString()))
                    {
                        //PRODUCTOS SIN STOCK
                        datalistadoDetallesOrdenCompra.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                    }
                    if (cantidadTotal < Convert.ToDecimal(datalistadoDetallesOrdenCompra.Rows[i].Cells[6].Value.ToString()))
                    {
                        //PRODUCTOS CON STOCK
                        datalistadoDetallesOrdenCompra.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //BUSCAR OC GENERAL POR FECHAS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarOrdenesCompra(DesdeFecha.Value, HastaFecha.Value);
        }

        //BUSCAR OC POR FECHA - DESDE
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarOrdenesCompra(DesdeFecha.Value, HastaFecha.Value);
        }

        //BUSCAR OC POR FECHA - HASTA
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarOrdenesCompra(DesdeFecha.Value, HastaFecha.Value);
        }

        //BUSCAR OC POR RESPONSABLE DE ESTE
        private void txtBusquedaResponsable_TextChanged(object sender, EventArgs e)
        {
            MostrarOrdenesCompraesponsable(txtBusquedaResponsable.Text, DesdeFecha.Value, HastaFecha.Value);
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN DE GENERACIÓN DEL PDF
        private void datalistadoTodasOC_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoTodasOC.Columns[e.ColumnIndex].Name == "btnGenerarPdf")
            {
                this.datalistadoTodasOC.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoTodasOC.Cursor = curAnterior;
            }
        }

        //VER OC
        private void btnVerOC_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasOC.CurrentRow != null)
            {
                if (datalistadoTodasOC.SelectedCells[11].Value.ToString() == "ANULADO")
                {
                    MessageBox.Show("La orden de compra se encuentra anulada, no se puede visualizar la orden de compra.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    string codigoReporte = datalistadoTodasOC.Rows[datalistadoTodasOC.CurrentRow.Index].Cells[1].Value.ToString();
                    Visualizadores.VisualizarOrdenCompra frm = new Visualizadores.VisualizarOrdenCompra();
                    frm.lblCodigo.Text = codigoReporte;

                    frm.Show();
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una orden de compra para poder generar el PDF.", "Validación del Sistema");
            }
        }

        //SELECCION DEL PDF GENERADO CON SUS RESPECTIVAS FIRMAS, INCLUIDO LA JEFATURA
        private void datalistadoTodasOC_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoTodasOC.Columns[e.ColumnIndex];

            if (currentColumn.Name == "btnGenerarPdf")
            {
                if (datalistadoTodasOC.SelectedCells[11].Value.ToString() == "ANULADO")
                {
                    MessageBox.Show("La orden de compra se encuentra anulada, no se puede visualizar la orden de compra.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    string codigoReporte = datalistadoTodasOC.Rows[datalistadoTodasOC.CurrentRow.Index].Cells[1].Value.ToString();
                    Visualizadores.VisualizarOrdenCompra frm = new Visualizadores.VisualizarOrdenCompra();
                    frm.lblCodigo.Text = codigoReporte;

                    frm.Show();
                }
            }
        }

        //DOBLE CLICK PARA ENTRAR A LOS DETALLES
        private void datalistadoTodasOC_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoTodasOC.Columns[e.ColumnIndex];

            //SI NO HAY UN REGISTRO SELECCIONADO
            if (datalistadoTodasOC.CurrentRow != null)
            {                
                //CAPTURAR EL CÓDIFO DE MI ORDEN DE COMPRA 
                int idOrdenCompra = Convert.ToInt32(datalistadoTodasOC.SelectedCells[1].Value.ToString());
                //VER EL PANEL DE LOS DETALLES DE M ORDEN DE COMPRA
                panelDetallesOrdenCompra.Visible = true;
                txtCodigoOrdenCompra.Text = datalistadoTodasOC.SelectedCells[2].Value.ToString();
                //MOSTRAR LOS ITEMS DE MI ORDEN DE COMPRA
                CargarDetallesItems(idOrdenCompra);
                txtCantidadItems.Text = Convert.ToString(datalistadoDetallesOrdenCompra.RowCount);
            }

            datalistadoTodasOC.Enabled = false;
        }

        //OCULTAR EL PANEL DE LOS DETALLES DE ORDEN DE COMPRA
        private void btnSalirDetallesOrdenCompra_Click(object sender, EventArgs e)
        {
            //OCULTAR EL PANEL DE LOS DETALLES DE MI ORDEN DE COMPRA
            panelDetallesOrdenCompra.Visible = false;
            datalistadoTodasOC.Enabled = true;
        }

        //OCULTAR EL PANEL DE LOS DETALLES DE MI ORDEN DE COMPRA
        private void lblRetrocederDetalleOrdenCompra_Click(object sender, EventArgs e)
        {
            //OCULTAR EL PANEL DE LOS DETALLES DE ORDEN DE COMPRA
            panelDetallesOrdenCompra.Visible = false;
            datalistadoTodasOC.Enabled = true;
        }

        //VISUALIZAR COTIZACION DE MI OC
        private void btnVerCotizacion_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasOC.CurrentRow != null)
            {
                string ruta = datalistadoTodasOC.SelectedCells[12].Value.ToString();
                if (ruta == "")
                {
                    MessageBox.Show("No existe un documento asociado a esta orden de compra.", "Abrir Docuemtno");
                }
                else
                {
                    Process.Start(ruta);
                }
            }
            else
            {
                MessageBox.Show("Por fevor, seleccione un registro para poder abrir la cotización adjuntada.", "Abrir Docuemtno");
            }
        }

        //ANULAR MI ORDEN DE COMPRA
        private void btnAnularOC_Click(object sender, EventArgs e)
        {
            //SI NO HAY UN REQUERIMIENTO SELECCIOANOD
            if (datalistadoTodasOC.CurrentRow != null)
            {
                if (datalistadoTodasOC.SelectedCells[11].Value.ToString() == "ANULADO")
                {
                    MessageBox.Show("La orden de compra que intenta anular ya se encuentra anulada.", "Validación del Sistema");
                }
                else
                {
                    panleAnulacion.Visible = true;
                    datalistadoTodasOC.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un requerimiento para poder anularlo.", "Validación del Sistema");
            }
        }

        //PROCEDER A LA ANULACION
        private void btnProcederAnulacion_Click(object sender, EventArgs e)
        {
            //SI NO HAY UN orden de compra seleccionada
            if (txtJustificacionAnulacion.Text != "")
            {
                //MENSAJE DE CONFIRMACIÓN PARA LA ANLACIÓN DE UN REQUERIMIENTO
                DialogResult boton = MessageBox.Show("¿Realmente desea anular esta orde de compra?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    //RECOPILACIÓN DE VARIABLES
                    int idOrdenCompra = Convert.ToInt32(datalistadoTodasOC.SelectedCells[1].Value.ToString());
                    string estadoOC = datalistadoTodasOC.SelectedCells[11].Value.ToString();

                    //SI EL ESTADO DE OC SE ENCUENTRA EN UN ESTADO DIFERENTE
                    if (estadoOC == "CONFORME" || estadoOC == "ANULADO")
                    {
                        MessageBox.Show("Esta orden de compra ya está anulado o atendida totalmente.", "Validación del Sistema");
                        txtJustificacionAnulacion.Text = "";
                        panleAnulacion.Visible = false;
                    }
                    else
                    {
                        try
                        {
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("AnularoOrdenCompra", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idOrdenCompra", idOrdenCompra);
                            cmd.Parameters.AddWithValue("@mensajeAnulacion", txtJustificacionAnulacion.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Orden de compra anulada exitosamente.", "Validación del Sistema");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }

                }
            }
            else
            {
                MessageBox.Show("Debe ingresar una justificación para poder anular esta orden de compra.", "Validación del Sistema");
            }
        }

        //RETROCEDER EN LA ANULACION
        private void btnRetrocederAnulacion_Click(object sender, EventArgs e)
        {
            txtJustificacionAnulacion.Text = "";
            panleAnulacion.Visible = false;
            datalistadoTodasOC.Enabled = true;
        }

        //EDIDCION DE MI ORDEN DE COMPRA
        private void btnModificarOC_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Función no habilitada.", "Validación del Sistema", MessageBoxButtons.OK);
        }

        //FUNCIO PARA EXPORTAR TODOS LOS DATOS POR EXCEL
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            MostrarExcel();

            SLDocument sl = new SLDocument();
            SLStyle style = new SLStyle();
            SLStyle styleC = new SLStyle();

            //COLUMNAS
            sl.SetColumnWidth(1, 20);
            sl.SetColumnWidth(2, 20);
            sl.SetColumnWidth(3, 20);
            sl.SetColumnWidth(4, 45);
            sl.SetColumnWidth(5, 45);
            sl.SetColumnWidth(6, 30);
            sl.SetColumnWidth(7, 25);
            sl.SetColumnWidth(8, 45);
            sl.SetColumnWidth(9, 20);
            sl.SetColumnWidth(10, 25);

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
            foreach (DataGridViewColumn column in datalistadoExcel.Columns)
            {
                sl.SetCellValue(1, ic, column.HeaderText.ToString());
                sl.SetCellStyle(1, ic, style);
                ic++;
            }

            int ir = 2;
            foreach (DataGridViewRow row in datalistadoExcel.Rows)
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
            sl.SaveAs(desktopPath + @"\Reporte de Orden de Compra.xlsx");
            MessageBox.Show("Se exportó los datos a un archivo de Microsoft Excel en la siguiente ubicación: " + desktopPath, "Validación del Sistema", MessageBoxButtons.OK);
        }

        //EXPORTAR DOCUMENTO SELECCIOANDO
        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear una instancia del reporte
                ReportDocument crystalReport = new ReportDocument();

                // Ruta del reporte .rpt
                //string rutaBase = Application.StartupPath;
                string rutaBase = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Recursos y Programas\";
                string rutaReporte = "";
                rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeOrdenCompra.rpt");

                crystalReport.Load(rutaReporte);

                // Configurar la conexión a la base de datos
                ConnectionInfo connectionInfo = new ConnectionInfo
                {
                    ServerName = "192.168.1.154,1433", // Ejemplo: "localhost" o "192.168.1.100"
                    DatabaseName = "BD_VENTAS_2", // Nombre de la base de datos
                    UserID = "sa", // Usuario de la base de datos
                    Password = "Arenas.2020!" // Contraseña del usuario
                };

                // Aplicar la conexión a cada tabla del reporte
                foreach (CrystalDecisions.CrystalReports.Engine.Table table in crystalReport.Database.Tables)
                {
                    TableLogOnInfo logOnInfo = table.LogOnInfo;
                    logOnInfo.ConnectionInfo = connectionInfo;
                    table.ApplyLogOnInfo(logOnInfo);
                }

                // **Enviar parámetro al reporte**
                // Cambia "NombreParametro" por el nombre exacto del parámetro en tu reporte
                int idReque = Convert.ToInt32(datalistadoTodasOC.SelectedCells[1].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string codigoOC = Convert.ToString(datalistadoTodasOC.SelectedCells[2].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                crystalReport.SetParameterValue("@idOrdenCompra", idReque);

                // Ruta de salida en el escritorio
                string rutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string rutaSalida = System.IO.Path.Combine(rutaEscritorio, "Orden de compra número " + codigoOC + ".pdf");

                // Exportar a PDF
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaSalida);

                MessageBox.Show($"Reporte exportado correctamente a: {rutaSalida}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al exportar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using ArenasProyect3.Modulos.ManGeneral;
using SpreadsheetLight;
using DocumentFormat.OpenXml.Spreadsheet;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;

namespace ArenasProyect3.Modulos.Comercial.Ventas
{
    public partial class Pedido : Form
    {
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        string ruta = Manual.manualComercial;
        private Cursor curAnterior = null;

        //CONMSTRUCTOR DE MI FORMULARIO
        public Pedido()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI FORMULARIO
        private void Pedido_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            datalistadoTodasPedido.DataSource = null;
            cboBusqeuda.SelectedIndex = 0;

            //PREFILES Y PERSIMOS---------------------------------------------------------------
            if (Program.RangoEfecto != 1)
            {
                btnAnularPedido.Visible = false;
                lblAnularPedido.Visible = false;
            }
            //---------------------------------------------------------------------------------
        }

        //VIZUALIZAR DATOS EXCEL--------------------------------------------------------------------
        public void MostrarExcel()
        {
            datalistadoExcel.Rows.Clear();

            foreach (DataGridViewRow dgv in datalistadoTodasPedido.Rows)
            {
                string numeroPedido = dgv.Cells[2].Value.ToString();
                string fechaInicio = dgv.Cells[3].Value.ToString();
                string fechaVencimiento = dgv.Cells[4].Value.ToString();
                string cliente = dgv.Cells[5].Value.ToString();
                string tipoMoneda = dgv.Cells[6].Value.ToString();
                string total = dgv.Cells[7].Value.ToString();
                string numeroCotizacion = dgv.Cells[8].Value.ToString();
                string cantidadItems = dgv.Cells[9].Value.ToString();
                string unidad = dgv.Cells[10].Value.ToString();
                string ordenCOmpra = dgv.Cells[11].Value.ToString();
                string estado = dgv.Cells[12].Value.ToString();

                datalistadoExcel.Rows.Add(new[] { numeroPedido, fechaInicio, fechaVencimiento, cliente, tipoMoneda, total, numeroCotizacion, cantidadItems, unidad, ordenCOmpra, estado });
            }
        }

        //LISTADO DE PEDIDOS Y SELECCION DE PDF Y ESTADO DE PEDIDOS---------------------
        //MOSTRAR PEDIDOS AL INCIO 
        public void MostrarPedidoPorFecha(DateTime fechaInicio, DateTime fechaTermino)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarPedidoPorFecha_Jefatura", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoTodasPedido.DataSource = dt;
            con.Close();
            RedimensionarListadoGeneralPedido(datalistadoTodasPedido);

        }

        //MOSTRAR ACTAS POR CLIENTE
        public void MostrarPedidoCliente(string cliente, DateTime fechaInicio, DateTime fechaTermino)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarPedidoPorCliente_Jefatura", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@cliente", cliente);
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoTodasPedido.DataSource = dt;
            con.Close();
            RedimensionarListadoGeneralPedido(datalistadoTodasPedido);
        }

        //FUNCION PARA REDIMENSIONAR MIS LISTADOS
        public void RedimensionarListadoGeneralPedido(DataGridView DGV)
        {
            //REDIEMNSION DE PEDIDOS
            DGV.Columns[2].Width = 80;
            DGV.Columns[3].Width = 100;
            DGV.Columns[4].Width = 100;
            DGV.Columns[5].Width = 350;
            DGV.Columns[6].Width = 150;
            DGV.Columns[7].Width = 80;
            DGV.Columns[8].Width = 80;
            DGV.Columns[9].Width = 80;
            DGV.Columns[10].Width = 170;
            DGV.Columns[11].Width = 120;
            DGV.Columns[12].Width = 150;

            DGV.Columns[1].Visible = false;
            DGV.Columns[13].Visible = false;

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN
        private void datalistadoTodasPedido_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoTodasPedido.Columns[e.ColumnIndex].Name == "detalles")
            {
                this.datalistadoTodasPedido.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoTodasPedido.Cursor = curAnterior;
            }
        }

        //MOSTRAR PEDIDOS SEGUN LAS FECHAS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarPedidoPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR PEDIDOS SEGUN LAS FECHAS
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarPedidoPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR PEDIDOS SEGUN LAS FECHAS
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarPedidoPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR PEDIDOS SEGUN EL CLIENTE
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            MostrarPedidoCliente(txtBusqueda.Text, DesdeFecha.Value, HastaFecha.Value);
        }

        //GENERACION DE REPORTES
        private void btnGenerarPedidoPdf_Click(object sender, EventArgs e)
        {
            string ccodigoCotizacion = datalistadoTodasPedido.Rows[datalistadoTodasPedido.CurrentRow.Index].Cells[1].Value.ToString();
            Visualizadores.VisualizarPedidoVenta frm = new Visualizadores.VisualizarPedidoVenta();
            frm.lblCodigo.Text = ccodigoCotizacion;

            frm.Show();
        }

        //PRODEDIMEINTO PARA ANULAR MI PEDIDO
        private void btnAnularPedido_Click(object sender, EventArgs e)
        {
            panleAnulacion.Visible = true;
        }

        //FUNCION PARA PROCEDER A ANULAR MI PEDIDO, COTIZACION
        private void btnProcederAnulacion_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasPedido.CurrentRow != null)
            {
                int idPedido = Convert.ToInt32(datalistadoTodasPedido.SelectedCells[1].Value.ToString());
                string idCotizacion = datalistadoTodasPedido.SelectedCells[13].Value.ToString();

                DialogResult boton = MessageBox.Show("¿Realmente desea anular esta pedido?. Se anulará la cotización asociada ha este pedido.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        SqlCommand cmd = new SqlCommand();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        cmd = new SqlCommand("AnularPedido", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idPedido", idPedido);
                        cmd.Parameters.AddWithValue("@idCotizacion", idCotizacion);
                        cmd.Parameters.AddWithValue("@mensajeAnulado", txtJustificacionAnulacion.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Pedido y cotización asociado a esta, anuladas exitosamente.", "Validación del Sistema");
                        MostrarPedidoPorFecha(DesdeFecha.Value, HastaFecha.Value);

                        panleAnulacion.Visible = false;
                        txtJustificacionAnulacion.Text = "";

                        //Enviar("jhoalexxxcc@gmail.com.pe", "ANULACIÓN DEL PEDIDO N°. " + codigoPedido, "Correo de verificación de anulación de un pedido por parte del usuario '" + Program.UnoNombreUnoApellidoUsuario + "' el la fecha siguiente: " + DateTime.Now);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un pedido para poder anularlo.", "Validación del Sistema");
            }
        }

        //BOTON PARA RETROCEDER DE LA ANULACION
        private void btnRetrocederAnulacion_Click(object sender, EventArgs e)
        {
            panleAnulacion.Visible = false;
            txtJustificacionAnulacion.Text = "";
        }

        //FUNCION PARA EDITAR MI PEDIDO
        private void btnEditarPedido_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Función no habilitada", "Validación del Sistema", MessageBoxButtons.OK);
        }

        //BOTON PARA EXPORTAR MIS DATOS
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            MostrarExcel();

            SLDocument sl = new SLDocument();
            SLStyle style = new SLStyle();
            SLStyle styleC = new SLStyle();

            //COLUMNAS
            sl.SetColumnWidth(1, 15);
            sl.SetColumnWidth(2, 20);
            sl.SetColumnWidth(3, 20);
            sl.SetColumnWidth(4, 50);
            sl.SetColumnWidth(5, 35);
            sl.SetColumnWidth(6, 20);
            sl.SetColumnWidth(7, 20);
            sl.SetColumnWidth(8, 20);
            sl.SetColumnWidth(9, 35);
            sl.SetColumnWidth(10, 20);
            sl.SetColumnWidth(11, 35);

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
                sl.SetCellValue(ir, 11, row.Cells[10].Value.ToString());
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
                sl.SetCellStyle(ir, 11, styleC);
                ir++;
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            sl.SaveAs(desktopPath + @"\Reporte de pedidos.xlsx");
            MessageBox.Show("Se exportó los datos a un archivo de Microsoft Excel en la siguiente ubicación: " + desktopPath, "Validación del Sistema", MessageBoxButtons.OK);
        }

        ////MÉTODO PARA ENVIAR CORREOS POR LA ANULACIÓN DE UN REQUERIMIENTO
        //public void Enviar(string para, string asunto, string mensaje)
        //{
        //    var outlokkApp = new Microsoft.Office.Interop.Outlook.Application();
        //    var mailItem = (Microsoft.Office.Interop.Outlook.MailItem)outlokkApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
        //    mailItem.To = para;
        //    mailItem.Subject = asunto;
        //    mailItem.Body = mensaje;

        //    mailItem.Send();
        //    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(mailItem);
        //    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(outlokkApp);
        //}

        //FUNCION PARA ABRIR EL MANUAL DE USUARIO
        private void btnInfoPedido_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //FUNCION PARA ABRIR EL MANUAL DE USUARIO
        private void btnInfoDetalles_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
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

                rutaReporte = Path.Combine(rutaBase, "Reportes", "InformePedidoVenta.rpt");

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
                int idPedido = Convert.ToInt32(datalistadoTodasPedido.SelectedCells[1].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                int codigoPedido = Convert.ToInt32(datalistadoTodasPedido.SelectedCells[2].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string cliente = datalistadoTodasPedido.SelectedCells[5].Value.ToString(); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string unidad = datalistadoTodasPedido.SelectedCells[10].Value.ToString(); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                crystalReport.SetParameterValue("@idPedido", idPedido);

                // Ruta de salida en el escritorio
                string rutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string rutaSalida = System.IO.Path.Combine(rutaEscritorio, "Pedido número " + codigoPedido + " - " + cliente + " - " + unidad + ".pdf");

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

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SpreadsheetLight;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Logistica.Almacen
{
    public partial class ListadoNotaSalida : Form
    {
        //VARIABLES GLOBALES DE MI FORMS
        private Cursor curAnterior = null;

        //CONSTRUCTIR DE MI FORMULARIO
        public ListadoNotaSalida()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI OFRMULARIO
        private void ListadoNotaSalida_Load(object sender, EventArgs e)
        {
            //AJUSTAR FECHAS DESDE EL PRIMER DIA DEL MES HASTA EL ÚLTIMO DIA DEL MES
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            datalistadoNotasSalida.DataSource = null;

            //PREFILES Y PERSIMOS---------------------------------------------------------------
            if (Program.RangoEfecto == 1 || Program.RangoEfecto == 5 || Program.RangoEfecto == 8 || Program.RangoEfecto == 11 || Program.RangoEfecto == 14)
            {

            }
            else
            {

            }
            //---------------------------------------------------------------------------------
        }

        //VIZUALIZAR DATOS EXCEL--------------------------------------------------------------------
        public void MostrarExcel()
        {
            datalistadoExcel.Rows.Clear();

            foreach (DataGridViewRow dgv in datalistadoNotasSalida.Rows)
            {
                string numeroTipoMovimeinto = dgv.Cells[2].Value.ToString();
                string fechaSalida = dgv.Cells[3].Value.ToString();
                string numeroRequerimiento = dgv.Cells[4].Value.ToString();
                string solicitante = dgv.Cells[5].Value.ToString();
                string almacen = dgv.Cells[6].Value.ToString();
                string tipoMovimeinto = dgv.Cells[7].Value.ToString();
                string estado = dgv.Cells[8].Value.ToString();

                datalistadoExcel.Rows.Add(new[] { numeroTipoMovimeinto, fechaSalida, numeroRequerimiento, solicitante, almacen, tipoMovimeinto, estado });
            }
        }

        //VER DETALLES(ITEMS) DE MI NOTA DE SALIDA
        public void CargarDetallesItems(int idNotaSalida)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarDetallesNotaSalida", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idNotaSalida", idNotaSalida);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoDetallesNotaSalida.DataSource = dt;
                con.Close();
                //NO MOSTRAR LAS COLUMNAS QUE NO SEAN DE REELEVANCIA PARA EL USUARIO
                datalistadoDetallesNotaSalida.Columns[0].Visible = false;
                //REDIMENSIONAR LAS COLUMNAS SEGUN EL TEMAÑO REQUERIDO
                datalistadoDetallesNotaSalida.Columns[1].Width = 100;
                datalistadoDetallesNotaSalida.Columns[2].Width = 450;
                datalistadoDetallesNotaSalida.Columns[3].Width = 100;
                datalistadoDetallesNotaSalida.Columns[4].Width = 207;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }
        //------------------------------------------------------------------------------------------

        //LISTADO DE REQUERIMEINTOS SIMPLES---------------------
        //MOSTRAR REQUERIMIENTOS POR FECHA Y ESTADO
        public void MostrarNotasSalidaSolicitante(DateTime fechaInicio, DateTime fechaTermino, string solicitante)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarNotasSalidaPorSolicitante_Kardex", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            cmd.Parameters.AddWithValue("@solicitante", solicitante);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoNotasSalida.DataSource = dt;
            con.Close();
            ReordenarColumnas(datalistadoNotasSalida);
        }

        //MOSTRAR REQUERIMIENTOS POR FECHA
        public void MostrarNotasSalidaPorFecha(DateTime fechaInicio, DateTime fechaTermino)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarNotasSalidaPorFechas_Kardex", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoNotasSalida.DataSource = dt;
            con.Close();
            ReordenarColumnas(datalistadoNotasSalida);
        }

        //METODO PARA REORDENAR MIS COLUMNAS
        public void ReordenarColumnas(DataGridView DGV)
        {
            //NO MOSTRAR LAS COLUMNAS QUE NO SEAN DE REELEVANCIA PARA EL USUARIO
            DGV.Columns[1].Visible = false;
            //REDIMENSIONAR LAS COLUMNAS SEGUN EL TEMAÑO REQUERIDO
            DGV.Columns[2].Width = 95;
            DGV.Columns[3].Width = 95;
            DGV.Columns[4].Width = 115;
            DGV.Columns[5].Width = 260;
            DGV.Columns[6].Width = 225;
            DGV.Columns[7].Width = 225;
            //CARGAR EL MÉTODO QUE COLOREA LAS FILAS
            //ColoresListado();

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //HACER QUE RESALTE EL CURSOR AL MOMENTO DE PASAR SOBRE EL BOTÓN
        private void datalistadoNotasSalida_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoNotasSalida.Columns[e.ColumnIndex].Name == "btnDetallesNotaSalida")
            {
                this.datalistadoNotasSalida.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoNotasSalida.Cursor = curAnterior;
            }
        }

        //COLOREAR REGISTROS
        public void ColoresListado()
        {
            try
            {
                for (var i = 0; i <= datalistadoNotasSalida.RowCount - 1; i++)
                {
                    if (datalistadoNotasSalida.Rows[i].Cells[8].Value.ToString() == "PENDIENTE")
                    {
                        //PENDIENTE -> 1
                        datalistadoNotasSalida.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else if (datalistadoNotasSalida.Rows[i].Cells[8].Value.ToString() == "CERRADO")
                    {
                        //CERRAD -> 2
                        datalistadoNotasSalida.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        //SI NO HAY NINGUN CASO
                        datalistadoNotasSalida.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //CARGA DE METODOS - GENERAL----------------------------------------------------------------------------------
        //MOSTRAR TODOS LAS NOTAS DE SALIDA GENERAL
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarNotasSalidaPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR TODOS LAS NOTAS DE SALIDA SEGUN LA FECAH ESCOGIDA
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarNotasSalidaPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR TODOS LAS NOTAS DE SALIDA SEGUN LA FECAH ESCOGIDA
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarNotasSalidaPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR TODOS LAS NOTAS DE SALIDA SEGUN LEL SOLICITANT
        private void txtBusquedaJefatura_TextChanged(object sender, EventArgs e)
        {
            MostrarNotasSalidaSolicitante(DesdeFecha.Value, HastaFecha.Value, txtSoliciatante.Text);
        }

        //CARGA DE METODOS - GENERAL----------------------------------------------------------------------------------
        //MOSTRAR TODOS LAS NOTAS DE SALIDA GENERAL
        private void datalistadoNotasSalida_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoNotasSalida.Columns[e.ColumnIndex];

            //SI SE PRECIONA SOBRE LA COLUMNA CON EL NOMBRE SELECCIOANDO
            if (currentColumn.Name == "btnDetallesNotaSalida")
            {
                //SI NO HAY UN REGISTRO SELECCIONADO
                if (datalistadoNotasSalida.CurrentRow != null)
                {
                    //CAPTURAR EL CÓDIFO DE MI NOTA DE SALIDA
                    int idNotaSalida = Convert.ToInt32(datalistadoNotasSalida.SelectedCells[1].Value.ToString());
                    //VER EL PANEL DE LOS DETALLES DE MI NOTA DE SALIDA
                    panelDetallesNotaSalida.Visible = true;
                    txtCodigoRequerimiento.Text = datalistadoNotasSalida.SelectedCells[2].Value.ToString();
                    //MOSTRAR LOS ITEMS DE MI NOTA DE SALIDA
                    CargarDetallesItems(idNotaSalida);
                }
            }
        }

        //OCULTAR EL PANEL DE LOS DETALLES DE LA NOTA DE SALIDA
        private void btnSalirDetallesSalida_Click(object sender, EventArgs e)
        {
            //OCULTAR EL PANEL DE LOS DETALLES DE LA NOTA DE SALIDA
            panelDetallesNotaSalida.Visible = false;
        }

        //OCULTAR EL PANEL DE LOS DETALLES DE LA NOTA DE SALIDA
        private void lblRetrocederDetalleNotaSalida_Click(object sender, EventArgs e)
        {
            //OCULTAR EL PANEL DE LOS DETALLES DE LA NOTA DE SALIDA
            panelDetallesNotaSalida.Visible = false;
        }

        //VISUALIZAR Y GENERAR PDF DE MI NOTA DE SALIDA
        private void btnVerNotaSalida_Click(object sender, EventArgs e)
        {
            if (datalistadoNotasSalida.CurrentRow != null)
            {
                string codigoReporte = datalistadoNotasSalida.Rows[datalistadoNotasSalida.CurrentRow.Index].Cells[1].Value.ToString();
                Visualizadores.VisualizarNotaSalida frm = new Visualizadores.VisualizarNotaSalida();
                frm.lblCodigo.Text = codigoReporte;
                //CARGAR VENTANA
                frm.Show();
            }
            else
            {
                MessageBox.Show("Debe seleccionar una nota de salida para poder generar el PDF respectivo.", "Validación del Sistema");
            }
        }

        //FUNCION PARA EXPORTAR  EL PDF A MI ESCRITORIO
        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                //Crear una instancia del reporte
                ReportDocument crystalReport = new ReportDocument();

                // Ruta del reporte .rpt
                //string rutaBase = Application.StartupPath;
                string rutaBase = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Recursos y Programas\";
                string rutaReporte = "";
                rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeNotaSalida.rpt");


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
                int idNotaSalida = Convert.ToInt32(datalistadoNotasSalida.SelectedCells[1].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string codigoNotaSalida = Convert.ToString(datalistadoNotasSalida.SelectedCells[2].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                crystalReport.SetParameterValue("@idNotaSalida", idNotaSalida);

                // Ruta de salida en el escritorio
                string rutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string rutaSalida = System.IO.Path.Combine(rutaEscritorio, "NOTA DE SALIDA N " + codigoNotaSalida + ".pdf");

                // Exportar a PDF
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaSalida);

                MessageBox.Show($"Reporte exportado correctamente a: {rutaSalida}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al exportar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //BOTON PARA EXPORTAR
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            MostrarExcel();

            SLDocument sl = new SLDocument();
            SLStyle style = new SLStyle();
            SLStyle styleC = new SLStyle();

            //COLUMNAS
            sl.SetColumnWidth(1, 25);
            sl.SetColumnWidth(2, 20);
            sl.SetColumnWidth(3, 25);
            sl.SetColumnWidth(4, 40);
            sl.SetColumnWidth(5, 40);
            sl.SetColumnWidth(6, 40);
            sl.SetColumnWidth(7, 25);

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
                sl.SetCellStyle(ir, 1, styleC);
                sl.SetCellStyle(ir, 2, styleC);
                sl.SetCellStyle(ir, 3, styleC);
                sl.SetCellStyle(ir, 4, styleC);
                sl.SetCellStyle(ir, 5, styleC);
                sl.SetCellStyle(ir, 6, styleC);
                sl.SetCellStyle(ir, 7, styleC);
                ir++;
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            sl.SaveAs(desktopPath + @"\Reporte de Notas de Salida.xlsx");
            MessageBox.Show("Se exportó los datos a un archivo de Microsoft Excel en la ubicación siguiente: " + desktopPath, "Validación del Sistema", MessageBoxButtons.OK);
        }
    }
}

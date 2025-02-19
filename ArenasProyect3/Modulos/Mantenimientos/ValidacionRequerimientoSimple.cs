using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SpreadsheetLight;
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
using DocumentFormat.OpenXml.Spreadsheet;

namespace ArenasProyect3.Modulos.Mantenimientos
{
    public partial class ValidacionRequerimientoSimple : Form
    {
        //VARIABLES GLOBALES
        int idJefatura = 0;
        string alias = "";
        string area = "";
        int IdUsuario = 0;
        private Cursor curAnterior = null;

        public ValidacionRequerimientoSimple()
        {
            InitializeComponent();
        }

        //PRIMERA EJECUCIÓN DE MI FORMULARIO
        private void ValidacionRequerimientoSimple_Load(object sender, EventArgs e)
        {
            //CARGA DE COMBOS Y DATOS DEL USUARIO
            DatosUsuario();

            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            datalistadoRequerimiento.DataSource = null;

            //SELECCIÓN AUTOMÁTICA DE LA JEFATURA INMEDIATA
            if (area == "Comercial")
            {
                DatosJefaturas(1);
            }
            else if (area == "Procesos")
            {
                DatosJefaturas(5);
            }
            else if (area == "Contabilidad")
            {
                DatosJefaturas(8);
            }
            else if (area == "Logística")
            {
                DatosJefaturas(11);
            }
            else if (area == "Ingienería")
            {
                DatosJefaturas(14);
            }

            //PREFILES Y PERSIMOS---------------------------------------------------------------
            if (Program.RangoEfecto == 1 || Program.RangoEfecto == 5 || Program.RangoEfecto == 8 || Program.RangoEfecto == 11 || Program.RangoEfecto == 14)
            {
                btnAprobarRequerimiento.Visible = true;
                lblAprobacionRequerimiento.Visible = true;
            }
            else
            {
                btnAprobarRequerimiento.Visible = false;
                lblAprobacionRequerimiento.Visible = false;
            }
            //---------------------------------------------------------------------------------
        }

        //VIZUALIZAR DATOS EXCEL--------------------------------------------------------------------
        public void MostrarExcel()
        {
            datalistadoExcel.Rows.Clear();

            foreach (DataGridViewRow dgv in datalistadoRequerimiento.Rows)
            {
                string numeroReque = dgv.Cells[3].Value.ToString();
                string fechaRequerida = dgv.Cells[4].Value.ToString();
                string fechaSolicitada = dgv.Cells[5].Value.ToString();
                string jefatura = dgv.Cells[6].Value.ToString();
                string solicitante = dgv.Cells[8].Value.ToString();
                string centroCostos = dgv.Cells[10].Value.ToString();
                string area = dgv.Cells[12].Value.ToString();
                string estadoAtencion = dgv.Cells[13].Value.ToString();

                datalistadoExcel.Rows.Add(new[] { numeroReque, fechaRequerida, fechaSolicitada, jefatura, solicitante, centroCostos, area, estadoAtencion });
            }
        }

        //BUSCAR DETALLES DE MI REQUERIMIENTO PARA CARGARLO
        public void BuscarDetallesRequerimiento(int codigoRequerimientoSimple)
        {
            //PROCEDIMIENTO ALMACENADO PARA LISTAR LOS DETALLES DE MI REQUERIMEINTO
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarDetallesRequerimientoSimple", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigoRequerimientoSimple", codigoRequerimientoSimple);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datallistadoDetalles.DataSource = dt;
            con.Close();
        }

        //VER DETALLES(ITEMS) DE MI REQUERIMIENTO SIMPLE
        public void CargarDetallesItems(int idRequerimeinto)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("ListaRequerimientoItemsGeneralLogistica_SP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idRequerimeinto", idRequerimeinto);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoDetallesRequerimiento.DataSource = dt;
                con.Close();
                //NO MOSTRAR LAS COLUMNAS QUE NO SEAN DE REELEVANCIA PARA EL USUARIO
                datalistadoDetallesRequerimiento.Columns[1].Visible = false;
                datalistadoDetallesRequerimiento.Columns[8].Visible = false;
                //REDIMENSIONAR LAS COLUMNAS SEGUN EL TEMAÑO REQUERIDO
                datalistadoDetallesRequerimiento.Columns[0].Width = 70;
                datalistadoDetallesRequerimiento.Columns[2].Width = 100;
                datalistadoDetallesRequerimiento.Columns[3].Width = 250;
                datalistadoDetallesRequerimiento.Columns[4].Width = 100;
                datalistadoDetallesRequerimiento.Columns[5].Width = 90;
                datalistadoDetallesRequerimiento.Columns[6].Width = 90;
                datalistadoDetallesRequerimiento.Columns[7].Width = 90;
                datalistadoDetallesRequerimiento.Columns[9].Width = 110;
                //CARGAR METODO PARA COLOREAR
                ColoresListadoItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------

        //LISTADO DE REQUERIMEINTOS SIMPLES---------------------
        //MOSTRAR REQUERIMIENTOS POR FECHA 
        public void MostrarRequerimientoPorFecha(DateTime fechaInicio, DateTime fechaTermino, string jefatura)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarRequerimientoSimplePorJefatura1", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            cmd.Parameters.AddWithValue("@jefatura", jefatura);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoRequerimiento.DataSource = dt;
            con.Close();
            Redimencionar(datalistadoRequerimiento);
        }

        //REDIMENSION DE MIS COLUMNAS
        public void Redimencionar(DataGridView DGV)
        {
            //NO MOSTRAR LAS COLUMNAS QUE NO SEAN DE REELEVANCIA PARA EL USUARIO
            DGV.Columns[1].Visible = false;
            DGV.Columns[2].Visible = false;
            DGV.Columns[7].Visible = false;
            DGV.Columns[9].Visible = false;
            DGV.Columns[11].Visible = false;
            DGV.Columns[14].Visible = false;
            DGV.Columns[15].Visible = false;
            DGV.Columns[16].Visible = false;
            DGV.Columns[17].Visible = false;
            DGV.Columns[18].Visible = false;
            DGV.Columns[19].Visible = false;
            //REDIMENSIONAR LAS COLUMNAS SEGUN EL TEMAÑO REQUERIDO
            DGV.Columns[3].Width = 110;
            DGV.Columns[4].Width = 95;
            DGV.Columns[5].Width = 95;
            DGV.Columns[6].Width = 250;
            DGV.Columns[8].Width = 250;
            DGV.Columns[10].Width = 200;
            DGV.Columns[12].Width = 200;
            DGV.Columns[13].Width = 100;
            //CARGAR EL MÉTODO QUE COLOREA LAS FILAS
            ColoresListado();

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //COLOREAR REGISTROS
        public void ColoresListado()
        {
            try
            {
                for (var i = 0; i <= datalistadoRequerimiento.RowCount - 1; i++)
                {
                    if (datalistadoRequerimiento.Rows[i].Cells[13].Value.ToString() == "POR ATENDER")
                    {
                        //POR ATENDER -> 1
                        datalistadoRequerimiento.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else if (datalistadoRequerimiento.Rows[i].Cells[13].Value.ToString() == "EVALUADO")
                    {
                        //EVALUADO -> 2
                        datalistadoRequerimiento.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (datalistadoRequerimiento.Rows[i].Cells[13].Value.ToString() == "OC EN CURSO")
                    {
                        //OC EN CURSO -> 3
                        datalistadoRequerimiento.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Orange;
                    }
                    else if (datalistadoRequerimiento.Rows[i].Cells[13].Value.ToString() == "OC CULMINADA")
                    {
                        //OC TERMINADA -> 4
                        datalistadoRequerimiento.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Teal;
                    }
                    else if (datalistadoRequerimiento.Rows[i].Cells[13].Value.ToString() == "ATENCION PARCIAL")
                    {
                        //ATENDIDO TOTAL -> 5
                        datalistadoRequerimiento.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(192, 192, 0);
                    }
                    else if (datalistadoRequerimiento.Rows[i].Cells[13].Value.ToString() == "ATENCION TOTAL")
                    {
                        //ATENDIDO TOTAL -> 6
                        datalistadoRequerimiento.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                    }
                    else if (datalistadoRequerimiento.Rows[i].Cells[13].Value.ToString() == "ANULADO")
                    {
                        //ATENDIDO TOTAL -> 0
                        datalistadoRequerimiento.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        //SI NO HAY NINGUN CASO
                        datalistadoRequerimiento.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                }
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
                for (var i = 0; i <= datalistadoDetallesRequerimiento.RowCount - 1; i++)
                {
                    decimal cantidadTotal = 0;
                    cantidadTotal = Convert.ToDecimal(datalistadoDetallesRequerimiento.Rows[i].Cells[5].Value.ToString());
                    decimal cantidadRetirada = 0;
                    cantidadRetirada = Convert.ToDecimal(datalistadoDetallesRequerimiento.Rows[i].Cells[6].Value.ToString());
                    decimal resultadoRestante = 0;

                    resultadoRestante = cantidadTotal - cantidadRetirada;

                    if (resultadoRestante > Convert.ToDecimal(datalistadoDetallesRequerimiento.Rows[i].Cells[7].Value.ToString()))
                    {
                        //PRODUCTOS SIN STOCK
                        datalistadoDetallesRequerimiento.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                    }
                    if (resultadoRestante < Convert.ToDecimal(datalistadoDetallesRequerimiento.Rows[i].Cells[7].Value.ToString()))
                    {
                        //PRODUCTOS CON STOCK
                        datalistadoDetallesRequerimiento.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    if (datalistadoDetallesRequerimiento.Rows[i].Cells[9].Value.ToString() == "ENTREGADO")
                    {
                        //PRODUCTOS ENTREGADO
                        datalistadoDetallesRequerimiento.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //CARGA DE METODOS - GENERAL----------------------------------------------------------------------------------
        //CARGA DE DATOS DEL USUARIO QUE INICIO SESIÓN
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

            IdUsuario = Convert.ToInt32(datalistadoBusquedaUusario.SelectedCells[0].Value.ToString());
            area = datalistadoBusquedaUusario.SelectedCells[7].Value.ToString();
        }

        //BUSQUEDA DE JEFATURAS
        public void DatosJefaturas(int idusuario)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarJefaturas", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idRol", idusuario);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoBusquedaJefatura.DataSource = dt;
            con.Close();

            txtBusquedaJefatura.Text = datalistadoBusquedaJefatura.SelectedCells[1].Value.ToString() + " " + datalistadoBusquedaJefatura.SelectedCells[2].Value.ToString();
        }

        //VISULIZAR EL PDF DEL REQUERIMEINTO SIMPLE
        private void btnVerReque_Click(object sender, EventArgs e)
        {
            if (datalistadoRequerimiento.CurrentRow != null)
            {
                if (datalistadoRequerimiento.SelectedCells[13].Value.ToString() == "ANULADO")
                {
                    string codigoActaReporte = datalistadoRequerimiento.Rows[datalistadoRequerimiento.CurrentRow.Index].Cells[1].Value.ToString();
                }
                else
                {
                    string codigoReporte = datalistadoRequerimiento.Rows[datalistadoRequerimiento.CurrentRow.Index].Cells[1].Value.ToString();
                    Visualizadores.VisualizarRequerimientoSimple frm = new Visualizadores.VisualizarRequerimientoSimple();
                    frm.lblCodigo.Text = codigoReporte;

                    frm.Show();
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un requerimiento para poder generar el PDF.", "Validación del Sistema");
            }
        }

        //MOSTRAR TODOS LOS REQUERIMIENTOS SEGÚN LA FECHA
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarRequerimientoPorFecha(DesdeFecha.Value, HastaFecha.Value, txtBusquedaJefatura.Text);
        }

        //MOSTRAR TODOS LOS REQUERIMIENTOS SEGÚN LA FECHA
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarRequerimientoPorFecha(DesdeFecha.Value, HastaFecha.Value, txtBusquedaJefatura.Text);
        }

        //MOSTRAR TODOS LOS REQUERIMIENTOS SEGÚN LA FECHA
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarRequerimientoPorFecha(DesdeFecha.Value, HastaFecha.Value, txtBusquedaJefatura.Text);
        }

        //HACER QUE RESALTE EL CURSOR AL MOMENTO DE PASAR SOBRE EL BOTÓN
        private void datalistadoRequerimiento_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoRequerimiento.Columns[e.ColumnIndex].Name == "Seleccionar")
            {
                this.datalistadoRequerimiento.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoRequerimiento.Cursor = curAnterior;
            }
        }

        //SELECCIONAR LOS DETALLES DE MI REQUERIMIENT
        private void datalistadoRequerimiento_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoRequerimiento.Columns[e.ColumnIndex];

            //SI SE PRECIONA SOBRE LA COLUMNA CON EL NOMBRE SELECCIOANDO
            if (currentColumn.Name == "Seleccionar")
            {
                //SI NO HAY UN REGISTRO SELECCIONADO
                if (datalistadoRequerimiento.CurrentRow != null)
                {
                    //CAPTURAR EL CÓDIFO DE MI REQUERIMIENTO SIMPLE
                    int idRequerimiento = Convert.ToInt32(datalistadoRequerimiento.SelectedCells[1].Value.ToString());
                    //VER EL PANEL DE LOS DETALLES DEL REQUERIMIENTO
                    panelDetallesRequerimiento.Visible = true;
                    txtCodigoRequerimiento.Text = datalistadoRequerimiento.SelectedCells[3].Value.ToString();
                    txtCantidadItems.Text = datalistadoRequerimiento.SelectedCells[14].Value.ToString();
                    //MOSTRAR LOS ITEMS DEL REQUERIMIENTO SIMPLE
                    CargarDetallesItems(idRequerimiento);
                }
            }
        }

        //OCULTAR EL PANEL DE LOS DETALLES DEL REQUERIMIENTO
        private void btnSalirDetallesRequerimiento_Click(object sender, EventArgs e)
        {
            //OCULTAR EL PANEL DE LOS DETALLES DEL REQUERIMIENTO
            panelDetallesRequerimiento.Visible = false;
        }

        //ACCIÓN DE APROBAR REQUERIMIENTO SIMPLE
        private void btnAprobarRequerimiento_Click(object sender, EventArgs e)
        {
            if (datalistadoRequerimiento.SelectedCells[13].Value.ToString() == "EVALUADO" ||
               datalistadoRequerimiento.SelectedCells[13].Value.ToString() == "ATENDIDO PARCIAL" ||
               datalistadoRequerimiento.SelectedCells[13].Value.ToString() == "ATENDIDO TOTAL" ||
               datalistadoRequerimiento.SelectedCells[13].Value.ToString() == "ANULADO" ||
               datalistadoRequerimiento.SelectedCells[13].Value.ToString() == "OC EN CURSO")
            {
                MessageBox.Show("El requerimiento que intenta atender se encuentra en un estado diferente a 'POR ATENDER'.", "Validación del Sistema");
            }
            else
            {
                DialogResult boton = MessageBox.Show("¿Realmente desea atender este requerimiento?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("CambioEstadoRequerimientoSimple_Jefatura", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //APROBAR - REQUERIMIENTO SIMPLE
                    cmd.Parameters.AddWithValue("@idRequerimientoSimple", datalistadoRequerimiento.SelectedCells[1].Value.ToString());
                    cmd.Parameters.AddWithValue("@estado", 2);
                    cmd.Parameters.AddWithValue("@mensajeAnulacion", "");
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Se atendió el requerimiento correctamente.", "Validación del Sistema");

                    MostrarRequerimientoPorFecha(DesdeFecha.Value, HastaFecha.Value, txtBusquedaJefatura.Text);
                }
            }
        }

        //ANULAR REQUERIMIENTO SIMPLE
        private void btnDesaprobarReque_Click(object sender, EventArgs e)
        {
            //SI NO HAY UN REQUERIMIENTO SELECCIOANOD
            if (datalistadoRequerimiento.CurrentRow != null)
            {
                //CAPTURAR EL NOMBRE DE USUARIO
                string usuarioEncargado = datalistadoRequerimiento.SelectedCells[8].Value.ToString();

                //SI EL USUARIO LOGEADO ES IGUAL AL USUARIO ENCARGADO DE DEL REQUERIMINTO
                if (usuarioEncargado == Program.NombreUsuarioCompleto)
                {
                    panleAnulacion.Visible = true;
                }
                else
                {
                    MessageBox.Show("El usuario que desea anular este requerimiento debe ser el mismo que lo ha creado.", "Validación del Sistema");
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un requerimiento para poder anularlo.", "Validación del Sistema");
            }
        }

        //ACCCION PARA PROCEDER A ANULAR MI REQUERIMEINTO
        private void btnProcederAnulacion_Click(object sender, EventArgs e)
        {
            //SI NO HAY UN REQUERIMIENTO SELECCIOANOD
            if (txtJustificacionAnulacion.Text != "")
            {
                //MENSAJE DE CONFIRMACIÓN PARA LA ANLACIÓN DE UN REQUERIMIENTO
                DialogResult boton = MessageBox.Show("¿Realmente desea anular este requerimiento?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    //RECOPILACIÓN DE VARIABLES
                    int idRequerimiento = Convert.ToInt32(datalistadoRequerimiento.SelectedCells[1].Value.ToString());
                    string estadoRequerimiento = datalistadoRequerimiento.SelectedCells[13].Value.ToString();

                    //SI EL ESTADO DE MI REQUERIMIENTO ES 
                    if (estadoRequerimiento == "ANULADO" || estadoRequerimiento == "ATENDIDO PARCIAL" || estadoRequerimiento == "ATENDIDO TOTAL" || estadoRequerimiento == "OC EN CURSO")
                    {
                        MessageBox.Show("Este requerimiento ya está anulado, se ha generado una orden de compra, atendido parcialmente o atendido totalmente.", "Validación del Sistema");
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
                            cmd = new SqlCommand("CambioEstadoRequerimientoSimple_Jefatura", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idRequerimientoSimple", idRequerimiento);
                            cmd.Parameters.AddWithValue("@estado", 0);
                            cmd.Parameters.AddWithValue("@mensajeAnulacion", txtJustificacionAnulacion.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Requerimiento anulado exitosamente.", "Validación del Sistema");

                            MostrarRequerimientoPorFecha(DesdeFecha.Value, HastaFecha.Value, txtBusquedaJefatura.Text);
                            panleAnulacion.Visible = false;
                            txtJustificacionAnulacion.Text = "";
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
                MessageBox.Show("Debe ingresar una justificación para poder anular este requerimiento.", "Validación del Sistema");
            }
        }

        //MÉTODO PARA SALIR DE LA ANULACIÓN DE UN REQUERIMIENTO
        private void btnRetrocederAnulacion_Click(object sender, EventArgs e)
        {
            txtJustificacionAnulacion.Text = "";
            panleAnulacion.Visible = false;
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
            sl.SetColumnWidth(6, 45);
            sl.SetColumnWidth(7, 45);
            sl.SetColumnWidth(8, 45);

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
                sl.SetCellStyle(ir, 1, styleC);
                sl.SetCellStyle(ir, 2, styleC);
                sl.SetCellStyle(ir, 3, styleC);
                sl.SetCellStyle(ir, 4, styleC);
                sl.SetCellStyle(ir, 5, styleC);
                sl.SetCellStyle(ir, 6, styleC);
                sl.SetCellStyle(ir, 7, styleC);
                sl.SetCellStyle(ir, 8, styleC);
                ir++;
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            sl.SaveAs(desktopPath + @"\Reporte de Requerimento Simple.xlsx");
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
                rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeRequerimientoSimple.rpt");

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
                int idReque = Convert.ToInt32(datalistadoRequerimiento.SelectedCells[1].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string codigoReque = Convert.ToString(datalistadoRequerimiento.SelectedCells[3].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                crystalReport.SetParameterValue("@codigo", idReque);

                // Ruta de salida en el escritorio
                string rutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string rutaSalida = System.IO.Path.Combine(rutaEscritorio, "Requerimiento simple número " + codigoReque + ".pdf");

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

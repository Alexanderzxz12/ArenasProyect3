using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Produccion.ConsultasOT
{
    public partial class ListadoOT : Form
    {
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        private Cursor curAnterior = null;
        int totalCantidades = 0;

        //CONMSTRUCTOR DE MI FORMULARIO
        public ListadoOT()
        {
            InitializeComponent();
        }

        //VIZUALIZAR DATOS EXCEL--------------------------------------------------------------------
        public void MostrarExcel()
        {
            datalistadoExcel.Rows.Clear();

            foreach (DataGridViewRow dgv in datalistadoEnProc.Rows)
            {
                string numeroOT = dgv.Cells[2].Value.ToString();
                string fechaInicio = dgv.Cells[3].Value.ToString();
                string fechaFinal = dgv.Cells[4].Value.ToString();
                string cliente = dgv.Cells[5].Value.ToString();
                string descripcionDescripcion = dgv.Cells[6].Value.ToString();
                string cantidad = dgv.Cells[7].Value.ToString();
                string color = dgv.Cells[8].Value.ToString();
                string numeroOrdenProduccion = dgv.Cells[9].Value.ToString();
                string cantidadRealizada = dgv.Cells[10].Value.ToString();
                string estado = dgv.Cells[11].Value.ToString();

                datalistadoExcel.Rows.Add(new[] { numeroOT, fechaInicio, fechaFinal, cliente, descripcionDescripcion, cantidad, color, numeroOrdenProduccion, cantidadRealizada, estado });
            }
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO Y VER SI ESTAN VENCIDOS
        //public void CargarColoresListadoOPGeneral()
        //{
        //    try
        //    {
        //        //VARIABLE DE FECHA
        //        var DateAndTime = DateTime.Now;
        //        //RECORRER MI LISTADO PARA VALIDAR MIS OPs, SI ESTAN VENCIDAS O NO
        //        foreach (DataGridViewRow datorecuperado in datalistadoEnProc.Rows)
        //        {
        //            //RECUERAR LA FECHA Y EL CÓDIGO DE MI OP
        //            DateTime fechaEntrega = Convert.ToDateTime(datorecuperado.Cells["FECHA DE ENTREGA"].Value);
        //            int codigoOP = Convert.ToInt32(datorecuperado.Cells["ID"].Value);
        //            string estadoOP = Convert.ToString(datorecuperado.Cells["ESTADO"].Value);

        //            int cantidadEsperada = Convert.ToInt32(datorecuperado.Cells["CANTIDAD"].Value);
        //            int cantidadRealizada = Convert.ToInt32(datorecuperado.Cells["CANTIDAD REALIZADA"].Value);

        //            if (estadoOP != "ANULADO")
        //            {
        //                //SI LA FECHA DE VALIDEZ ES MAYOR A LA FECHA ACTUAL CONSULTADA
        //                if (fechaEntrega == DateAndTime.Date)
        //                {
        //                    //CAMBIAR EL ESTADO DE MI COTIZACIÓN
        //                    SqlConnection con = new SqlConnection();
        //                    SqlCommand cmd = new SqlCommand();
        //                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
        //                    con.Open();
        //                    cmd = new SqlCommand("CambiarEstadoOT", con);
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.AddWithValue("@idOT", codigoOP);
        //                    cmd.Parameters.AddWithValue("@estadoOT", 2);
        //                    cmd.ExecuteNonQuery();
        //                    con.Close();
        //                }
        //                else if (fechaEntrega < DateAndTime.Date)
        //                {
        //                    //CAMBIAR EL ESTADO DE MI COTIZACIÓN
        //                    SqlConnection con = new SqlConnection();
        //                    SqlCommand cmd = new SqlCommand();
        //                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
        //                    con.Open();
        //                    cmd = new SqlCommand("CambiarEstadoOT", con);
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.AddWithValue("@idOT", codigoOP);
        //                    cmd.Parameters.AddWithValue("@estadoOT", 3);
        //                    cmd.ExecuteNonQuery();
        //                    con.Close();
        //                }
        //                else if (fechaEntrega > DateAndTime)
        //                {
        //                    //CAMBIAR EL ESTADO DE MI COTIZACIÓN
        //                    SqlConnection con = new SqlConnection();
        //                    SqlCommand cmd = new SqlCommand();
        //                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
        //                    con.Open();
        //                    cmd = new SqlCommand("CambiarEstadoOT", con);
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.AddWithValue("@idOT", codigoOP);
        //                    cmd.Parameters.AddWithValue("@estadoOT", 1);
        //                    cmd.ExecuteNonQuery();
        //                    con.Close();
        //                }

        //                if (cantidadEsperada == cantidadRealizada)
        //                {
        //                    //CAMBIAR EL ESTADO DE MI OP
        //                    SqlConnection con = new SqlConnection();
        //                    SqlCommand cmd = new SqlCommand();
        //                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
        //                    con.Open();
        //                    cmd = new SqlCommand("CambiarEstadoOT", con);
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.AddWithValue("@idOT", codigoOP);
        //                    cmd.Parameters.AddWithValue("@estadoOT", 4);
        //                    cmd.ExecuteNonQuery();
        //                    con.Close();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error en la operación por: " + ex.Message);
        //    }
        //}

        //PRIMERA CARGA DE MI FORMULARIO
        private void ListadoOT_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            //datalistadoTodasOT.DataSource = null;
            cboBusqeuda.SelectedIndex = 0;
            // MostrarOrdenTrabajoPorFecha(oPrimerDiaDelMes, oUltimoDiaDelMes);
            //MostrarOrdenTrabajo(oPrimerDiaDelMes, oUltimoDiaDelMes);
            //PREFILES Y PERSIMOS---------------------------------------------------------------
            if (Program.RangoEfecto != 1)
            {
                //btnAnularPedido.Visible = false;
                //lblAnularPedido.Visible = false;
            }
            //---------------------------------------------------------------------------------
        }

        //FUNCION PARA VERIFICAR SI HAY UNA CANTIDAD 
        public void MostrarCantidadesSegunOT(int idOrdenServicio)
        {
            totalCantidades = 0;

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarCantidadesSegunOT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idOrdenServicio", idOrdenServicio);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoCantidades.DataSource = dt;
            con.Close();
            datalistadoCantidades.Columns[0].Width = 40;
            datalistadoCantidades.Columns[1].Width = 120;
            datalistadoCantidades.Columns[2].Width = 100;
            alternarColorFilas(datalistadoCantidades);

            //CONTAR CUANTAS CANTIDADES HAY
            foreach (DataGridViewRow row in datalistadoCantidades.Rows)
            {
                totalCantidades = totalCantidades + Convert.ToInt32(row.Cells[1].Value.ToString());
            }
        }

        public void MostrarCantidadesSegunOTCalidad(int idOrdenServicio)
        {
            using (SqlConnection con = new SqlConnection(Conexion.ConexionMaestra.conexion))
            using (SqlCommand cmd = new SqlCommand("OT_MostrarCantidadesCalidad", con)) // Nuevo SP para OT
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idOrdenServicio", idOrdenServicio); // Ajustamos el nombre del parámetro

                try
                {
                    con.Open();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoHistorial.DataSource = dt;

                    // OPTIMIZACIÓN DE REDIMENSIONAMIENTO POR NOMBRE Y AUTOAJUSTE
                    datalistadoHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                    // Ajustes manuales por NOMBRE de columna (más robusto)
                    if (datalistadoHistorial.Columns.Contains("FECHA"))
                        datalistadoHistorial.Columns["FECHA"].Width = 120;
                    if (datalistadoHistorial.Columns.Contains("OBSERVACIONES"))
                        datalistadoHistorial.Columns["OBSERVACIONES"].Width = 250;

                    // Ocultar columnas por NOMBRE
                    if (datalistadoHistorial.Columns.Contains("ID"))
                        datalistadoHistorial.Columns["ID"].Visible = false;
                    if (datalistadoHistorial.Columns.Contains("OBSERVACIONES"))
                        datalistadoHistorial.Columns["OBSERVACIONES"].Visible = false; // El índice 6 del original

                    //ColoresListadoCantidades();

                    // DESHABILITAR CLICK Y REORDENAMIENTO
                    foreach (DataGridViewColumn column in datalistadoHistorial.Columns)
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar detalle de calidad: " + ex.Message);
                }
            }
        }

        //COLOREAR MI LISTADO
        public void alternarColorFilas(DataGridView dgv)
        {
            try
            {
                {
                    var withBlock = dgv;
                    withBlock.RowsDefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
                    withBlock.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //deshabilitar el click y  reordenamiento por columnas
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //LISTADO DE OT Y SELECCION DE PDF Y ESTADO DE OT---------------------
        //MOSTRAR OT AL INCIO 

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //public void MostrarOrdenTrabajoPorFecha(DateTime fechaInicio, DateTime fechaTermino)
        //{
        //    DataTable dt = new DataTable();
        //    SqlConnection con = new SqlConnection();
        //    con.ConnectionString = Conexion.ConexionMaestra.conexion;
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand();
        //    cmd = new SqlCommand("MostrarOrdenServicionPorFecha", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
        //    cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);
        //    datalistadoTodasOT.DataSource = dt;
        //    con.Close();
        //    RedimensionarListadoGeneral(datalistadoTodasOT);
        //}

        ////MOSTRAR OT POR CLIENTE
        //public void MostrarOrdenTrabajoPorCliente(DateTime fechaInicio, DateTime fechaTermino, string cliente)
        //{
        //    DataTable dt = new DataTable();
        //    SqlConnection con = new SqlConnection();
        //    con.ConnectionString = Conexion.ConexionMaestra.conexion;
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand();
        //    cmd = new SqlCommand("MostrarOrdenServicionPorCliente", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
        //    cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
        //    cmd.Parameters.AddWithValue("@cliente", cliente);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);
        //    datalistadoTodasOT.DataSource = dt;
        //    con.Close();
        //    RedimensionarListadoGeneral(datalistadoTodasOT);
        //}

        ////MOSTRAR OP POR CODIGO OT
        //public void MostrarOrdenServicioPorCodigoOT(DateTime fechaInicio, DateTime fechaTermino, string codigoOT)
        //{
        //    DataTable dt = new DataTable();
        //    SqlConnection con = new SqlConnection();
        //    con.ConnectionString = Conexion.ConexionMaestra.conexion;
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand();
        //    cmd = new SqlCommand("MostrarOrdenServicionPorCodigoOT", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
        //    cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
        //    cmd.Parameters.AddWithValue("@codigoOT", codigoOT);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);
        //    datalistadoTodasOT.DataSource = dt;
        //    con.Close();
        //    RedimensionarListadoGeneral(datalistadoTodasOT);
        //}

        ////MOSTRAR OP POR CODIGO OT
        //public void MostrarOrdenServicioPorDescripcion(DateTime fechaInicio, DateTime fechaTermino, string descripcipon)
        //{
        //    DataTable dt = new DataTable();
        //    SqlConnection con = new SqlConnection();
        //    con.ConnectionString = Conexion.ConexionMaestra.conexion;
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand();
        //    cmd = new SqlCommand("MostrarOrdenServicionPorDescripcion", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
        //    cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
        //    cmd.Parameters.AddWithValue("@descripcion", descripcipon);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);
        //    datalistadoTodasOT.DataSource = dt;
        //    con.Close();
        //    RedimensionarListadoGeneral(datalistadoTodasOT);
        
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public void MostrarOrdenTrabajo(
            DateTime fechaInicio,
            DateTime fechaTermino,
            string cliente = null,
            string codigoOT = null,
            string descripcion = null)
        {
            // Usamos 'using' para asegurar que la conexión se cierre y se libere la memoria correctamente
            using (SqlConnection con = new SqlConnection(Conexion.ConexionMaestra.conexion))
            using (SqlCommand cmd = new SqlCommand("OT_MostrarOrdenServicio", con))
            {
                // Nuevo SP unificado
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                    cmd.Parameters.AddWithValue("@cliente", (object)cliente ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@codigoOT", (object)codigoOT ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@descripcion", (object)descripcion ?? DBNull.Value);

                    try
                    {
                        con.Open();
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                        datalistadoTodasOT.DataSource = dt;
                        DataRow[] rowsEnProceso = dt.Select("ESTADO = 'PENDIENTE'");
                        // Si hay filas, crea un nuevo DataTable, si no, usa una copia vacía del esquema.
                        DataTable dtEnProceso = rowsEnProceso.Any() ? rowsEnProceso.CopyToDataTable() : dt.Clone();
                        datalistadoEnProc.DataSource = dtEnProceso; // Asumiendo este es el nombre de tu DataGrid

                        // --- 2.3. OT Observadas (Asumimos que el estado "Observadas" es FUERA DE FECHA o LÍMITE) ---
                        DataRow[] rowsObservadas = dt.Select("[INDICADOR SNC] = 1");
                        DataTable dtObservadas = rowsObservadas.Any() ? rowsObservadas.CopyToDataTable() : dt.Clone();
                        datalistadoObs.DataSource = dtObservadas; // Asumiendo este es el nombre de tu DataGrid

                        RedimensionarListadoGeneral(datalistadoTodasOT);
                        RedimensionarListadoGeneral(datalistadoEnProc);
                        RedimensionarListadoGeneral(datalistadoObs);
                    }
                    catch (Exception ex)
                    {
                        // Manejar el error, por ejemplo, mostrando un mensaje
                        MessageBox.Show("Error al cargar las órdenes de trabajo: " + ex.Message);
                    }
                }
            }
        }

        //FUNCION PARA REDIMENSIONAR MIS LISTADOS
        public void RedimensionarListadoGeneral(DataGridView DGV)
        {
            DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            DGV.ReadOnly = true;
            int columna_Cliente = 5;
            int columna_Descripcion = 6;
            int columna_EstadoCalidad = 16;


            DGV.Columns[1].Visible = false;
            DGV.Columns[10].Visible = false;
            DGV.Columns[11].Visible = false;
            DGV.Columns[15].Visible = false;
            DGV.Columns[16].Visible = false;

            if (DGV.Columns.Count > columna_Cliente)
            {
                DGV.Columns[columna_Cliente].Width = 270;
            }
            if (DGV.Columns.Count > columna_Descripcion)
            {
                DGV.Columns[columna_Descripcion].Width = 350;
            }

            if (DGV.Name == "datalistadoObs")
            {
                if (DGV.Columns.Count > columna_EstadoCalidad)
                {
                    DGV.Columns[columna_EstadoCalidad].Visible = true;
                    // Opcional: Dale un ancho específico si es necesario
                    // DGV.Columns[columna_EstadoCalidad].Width = 100;
                }
            }

            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //CargarColoresListadoOPGeneral();
           // ColoresListado();
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO
        public void ColoresListado()
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= datalistadoEnProc.RowCount - 1; i++)
                {
                    if (datalistadoEnProc.Rows[i].Cells[11].Value.ToString() == "FUERA DE FECHA")
                    {
                        datalistadoEnProc.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    else if (datalistadoEnProc.Rows[i].Cells[11].Value.ToString() == "LÍMITE")
                    {
                        datalistadoEnProc.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Orange;
                    }
                    else if (datalistadoEnProc.Rows[i].Cells[11].Value.ToString() == "PENDIENTE")
                    {
                        datalistadoEnProc.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else if (datalistadoEnProc.Rows[i].Cells[11].Value.ToString() == "CULMINADO")
                    {
                        datalistadoEnProc.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.DarkGreen;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN
        private void datalistadoTodasOT_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoEnProc.Columns[e.ColumnIndex].Name == "detalles")
            {
                this.datalistadoEnProc.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoEnProc.Cursor = curAnterior;
            }
        }

        //MOSTRAR OT POR FECHA
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            //MostrarOrdenTrabajoPorFecha(DesdeFecha.Value, HastaFecha.Value);
            MostrarOrdenTrabajo(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OT POR FECHA
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            //MostrarOrdenTrabajoPorFecha(DesdeFecha.Value, HastaFecha.Value);
            MostrarOrdenTrabajo(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OT POR FECHA
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            //MostrarOrdenTrabajoPorFecha(DesdeFecha.Value, HastaFecha.Value);
            MostrarOrdenTrabajo(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OPRDENES TRABAJO DEPENDIENTO LA OPCIÓN ESCOGIDA
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
         
            string cliente = null;
            string codigoOT = null;
            string descripcion = null;
            string textoBusqueda = txtBusqueda.Text;

            // 2. Determinar qué filtro debe usarse (basado en el ComboBox)
            if (cboBusqeuda.Text == "CÓDIGO OT")
            {
                // Si el usuario selecciona "CÓDIGO OT", solo asignamos el texto a 'codigoOT'.
                codigoOT = textoBusqueda;
            }
            else if (cboBusqeuda.Text == "CLIENTE")
            {
                // Si el usuario selecciona "CLIENTE", solo asignamos el texto a 'cliente'.
                cliente = textoBusqueda;
            }
            else if (cboBusqeuda.Text == "DESCRIPCIÓN PRODUCTO")
            {
                // Si el usuario selecciona "DESCRIPCIÓN PRODUCTO", solo asignamos el texto a 'descripcion'.
                descripcion = textoBusqueda;
            }

            // 3. Llamar al método UNIFICADO 'MostrarOrdenTrabajo'.
            //    Solo el parámetro que se eligió tendrá el valor de búsqueda; los otros serán NULL.
            MostrarOrdenTrabajo(
                DesdeFecha.Value,
                HastaFecha.Value,
                cliente,
                codigoOT,
                descripcion
            );
        }

        //LIMPIAR EL CAMBIO DE BUSQUEDA
        private void cboBusqeuda_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";
        }

        //VISUALIZAR EL PLANO DEL PRODUCTO
        private void btnVisualizarPDFPorducto_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(datalistadoEnProc.SelectedCells[13].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Documento no encontrado, hubo un error al momento de cargar el archivo.", ex.Message);
            }
        }

        //VISUALIZAR EL PLANO DEL SEMI-PRODUCTO
        private void btnVisualizarPDFSemiProducido_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(datalistadoEnProc.SelectedCells[12].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Documento no encontrado, hubo un error al momento de cargar el archivo.", ex.Message);
            }
        }

        //EVENTO PARA ABRIR EL INGRESO DE CANTIDADES
        private void datalistadoTodasOT_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoEnProc.CurrentRow != null)
            {
                int count = 0;
                foreach (DataGridViewRow row in datalistadoEnProc.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value))
                    {
                        count++;
                    }
                }

                if (count == 0) { count = 1; }

                txtOtsSeleccionadas.Text = Convert.ToString(count);
                //CARGA DE DAOTS
                txtCodigoOT.Text = datalistadoEnProc.SelectedCells[2].Value.ToString();
                int IdOrdenServicio = Convert.ToInt32(datalistadoEnProc.SelectedCells[1].Value.ToString());
                txtDescripcionSub_Producto.Text = datalistadoEnProc.SelectedCells[6].Value.ToString();
                txtCantidadTotalOT.Text = datalistadoEnProc.SelectedCells[7].Value.ToString();
                txtCantidadRequerida.Text = datalistadoEnProc.SelectedCells[7].Value.ToString();
                dtpFechaRealizada.Value = DateTime.Now;
                txtCantidadRealizada.Text = "";
                txtCantidadRestante.Text = "";
                MostrarCantidadesSegunOT(IdOrdenServicio);
                lblCantidadTotalInghresada.Text = Convert.ToString(totalCantidades);
                txtCantidadRestante.Text = Convert.ToString(Convert.ToInt32(txtCantidadRequerida.Text) - Convert.ToInt32(lblCantidadTotalInghresada.Text));

                if (txtCantidadRestante.Text == "0")
                {
                    datalistadoEnProc.Enabled = true;
                    panelIngresoCantidades.Visible = false;
                    MessageBox.Show("Esta OT ya culminó satisfactoriamente.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    datalistadoEnProc.Enabled = false;
                    panelIngresoCantidades.Visible = true;

                    if (count != 1)
                    {
                        btnGenerarGuardarCantidades.Visible = true;
                        lblGenerarGuardarCantidades.Visible = true;
                        btnGuardarCantidad.Visible = false;
                        lblGuardarCantidad.Visible = false;
                        txtCantidadRealizada.ReadOnly = true;
                        txtCantidadRealizada.Text = "Gen. Automática";
                        lblIdOT.Text = "Varios";
                        txtCantidadRestante.Text = "0";
                    }
                    else
                    {
                        btnGuardarCantidad.Visible = true;
                        lblGuardarCantidad.Visible = true;
                        btnGenerarGuardarCantidades.Visible = false;
                        lblGenerarGuardarCantidades.Visible = false;
                        txtCantidadRealizada.ReadOnly = false;
                        lblIdOT.Text = datalistadoEnProc.SelectedCells[1].Value.ToString();
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una OT para poder continuar.", "Validación del Sistema");
            }
        }

        private void datalistadoObs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (datalistadoObs.RowCount != 0)
            //{
            //    btnVisualizarSNC.Visible = false;
            //    panelControlCalidad.Visible = true;

            //    lblIdOP.Text = datalistadoObs.SelectedCells[1].Value.ToString();
            //    txtCoidgoOPCalidad.Text = datalistadoObs.SelectedCells[2].Value.ToString();
            //    txtDescripcionProductoCalidad.Text = datalistadoObs.SelectedCells[8].Value.ToString();
            //    txtArea.Text = datalistadoObs.SelectedCells[26].Value.ToString();
            //    txtIdArea.Text = datalistadoObs.SelectedCells[28].Value.ToString();
            //    txtIdOP.Text = datalistadoObs.SelectedCells[1].Value.ToString();
            //    MostrarCantidadesSegunOPCalidad(Convert.ToInt32(lblIdOP.Text));
            //    btnGenerarCSM.Visible = false;
            //    lblGenerarCSM.Visible = false;
            //    datalistadoObs.Enabled = false;
            //}
            if (datalistadoObs.CurrentRow != null) // Usamos CurrentRow en lugar de RowCount != 0
            {
                btnVisualizarSNC.Visible = false;
                panelControlCalidad.Visible = true;

                DataGridViewRow selectedRow = datalistadoObs.CurrentRow;

                // --- LECTURA OPTIMIZADA POR NOMBRE DE COLUMNA ---

                // Asumiendo estos nombres de columna en el grid de OT (SP: OT_MostrarOrdenServicio)
                // Por favor, verifica los nombres exactos si son diferentes (ej. [ID] o [N°. OT])

                string idOT = selectedRow.Cells["ID"].Value.ToString();
                string codigoOT = selectedRow.Cells["N°. OT"].Value.ToString();
                string descripcion = selectedRow.Cells["DESCRIPCIÓN DEL SUB-PRODUCTO"].Value.ToString();

                // --- Nota: Para las columnas 26 y 28, necesitas conocer su Alias en el SP ---
                // Ejemplo asumiendo que son 'AREA' y 'ID AREA'
                string area = selectedRow.Cells["AREA"].Value.ToString();
                string idArea = selectedRow.Cells["IdA"].Value.ToString();

                // --- Carga de datos ---
                lblIdOT.Text = idOT;
                txtCoidgoOTCalidad.Text = codigoOT;
                txtDescripcionProductoCalidad.Text = descripcion;
                txtArea.Text = area;
                txtIdArea.Text = idArea;
                txtIdOP.Text = idOT; // Asumiendo que lblIdOP y txtIdOP deben ser el mismo valor (el ID)

                MostrarCantidadesSegunOTCalidad(Convert.ToInt32(lblIdOT.Text)); // Llamada al método optimizado

                btnGenerarCSM.Visible = false;
                lblGenerarCSM.Visible = false;
                datalistadoObs.Enabled = false;
            }
        }

        //GENERACION DE REPORTES
        private void btnVsualizarPDFPT_Click(object sender, EventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoEnProc.CurrentRow != null)
            {
                string codigoOrdenTrabajo = datalistadoEnProc.Rows[datalistadoEnProc.CurrentRow.Index].Cells[1].Value.ToString();
                Visualizadores.VisualizarOrdenTrabajo frm = new Visualizadores.VisualizarOrdenTrabajo();
                frm.lblCodigo.Text = codigoOrdenTrabajo;

                frm.Show();
            }
            else
            {
                MessageBox.Show("Debe seleccionar una OT para poder generar el PDF.", "Validación del Sistema");
            }
        }

        //EVENTO PARA GUARDAR MI S CANTIDADES INGRESADAS
        private void btnGuardarCantidad_Click(object sender, EventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoEnProc.CurrentRow != null)
            {
                if (txtCantidadRealizada.Text == "" || txtCantidadRealizada.Text == "0")
                {
                    MessageBox.Show("Debe ingresar una cantidad válida para poder registrar.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else if (txtCantidadRequerida.Text == lblCantidadTotalInghresada.Text)
                {
                    MessageBox.Show("La orden de producción ya culminó.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else if (Convert.ToInt32(txtCantidadRestante.Text) < Convert.ToInt32(txtCantidadRealizada.Text))
                {
                    MessageBox.Show("No se puede ingresar una cantidad mayor a la restante.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    DialogResult boton = MessageBox.Show("¿Realmente desea ingresar esta cantidad?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                    if (boton == DialogResult.OK)
                    {
                        try
                        {
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("IngresarRegistroCantidadOT", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idOrdenServicio", lblIdOT.Text);
                            cmd.Parameters.AddWithValue("@cantidad", txtCantidadRealizada.Text);
                            cmd.Parameters.AddWithValue("@fechaRegistro", Convert.ToDateTime(dtpFechaRealizada.Value));
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Cantidd ingresada correctamente.", "Validación del Sistema");
                            
                            MostrarOrdenTrabajo(DesdeFecha.Value, HastaFecha.Value);
                            LimpiarCantidades();
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
                MessageBox.Show("Debe seleccionar una OT para poder continuar.", "Validación del Sistema");
            }
        }

        //EVENTO PARA GUARDAR VARIAS CANTIDADES INGRESADAS
        private void btnGenerarGuardarCantidades_Click(object sender, EventArgs e)
        {
            List<int> idOTSeleccionada = new List<int>();
            List<int> CantidadTotalOTSeleccionada = new List<int>();

            foreach (DataGridViewRow row in datalistadoEnProc.Rows)
            {
                DataGridViewCheckBoxCell checkBox = row.Cells[0] as DataGridViewCheckBoxCell;

                if (checkBox != null && Convert.ToBoolean(checkBox.Value) == true)
                {
                    try
                    {
                        int idOt = Convert.ToInt32(row.Cells[1].Value.ToString());
                        int cantidadEsperada = Convert.ToInt32(row.Cells[7].Value.ToString());
                        int cantidadHecha = Convert.ToInt32(row.Cells[10].Value.ToString());
                        int TotalCantidad = cantidadEsperada - cantidadHecha;

                        if (TotalCantidad != 0)
                        {
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("IngresarRegistroCantidadOT", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idOrdenServicio", idOt);
                            cmd.Parameters.AddWithValue("@cantidad", TotalCantidad);
                            cmd.Parameters.AddWithValue("@fechaRegistro", Convert.ToDateTime(dtpFechaRealizada.Value));
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            MessageBox.Show("Operación terminada.", "Validación del Sistema");
            MostrarOrdenTrabajo(DesdeFecha.Value, HastaFecha.Value);
            //MostrarOrdenTrabajo(DesdeFecha.Value, HastaFecha.Value);
            LimpiarCantidades();
        }

        //EVENTO PARA RETROCEDER O SALIR DE MI VENTANA DE INGRESO DE CANTIDADES
        private void btnSalirCantidad_Click(object sender, EventArgs e)
        {
            LimpiarCantidades();
        }

        //EVENTO PARA RETROCEDER O SALIR DE MI VENTANA DE INGRESO DE CANTIDADES
        private void btnCerrarDetallesOPCantidades_Click(object sender, EventArgs e)
        {
            LimpiarCantidades();
        }

        //FUNCION PARA LIMPIAR LAS CANTIDADES
        public void LimpiarCantidades()
        {
            datalistadoEnProc.Enabled = true;
            panelIngresoCantidades.Visible = false;
            txtOtsSeleccionadas.Text = "";
            txtCantidadRealizada.Text = "";
            txtCantidadRestante.Text = "";
        }
        //-----------------------------------------------------------------------------------------------------------

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
            sl.SetColumnWidth(4, 40);
            sl.SetColumnWidth(5, 60);
            sl.SetColumnWidth(6, 15);
            sl.SetColumnWidth(7, 20);
            sl.SetColumnWidth(8, 20);
            sl.SetColumnWidth(9, 15);
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
            sl.SaveAs(desktopPath + @"\Reporte de ordenes de producción.xlsx");
            MessageBox.Show("Se exportó los datos a un archivo de Microsoft Excel en la siguiente ubicación: " + desktopPath, "Validación del Sistema", MessageBoxButtons.OK);
        }

        //FUNCION PARA GUARDAR 
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

                rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeOrdenTrabajo.rpt");

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
                int idOrdenServicio = Convert.ToInt32(datalistadoEnProc.SelectedCells[1].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string codigoOrdenServicio = datalistadoEnProc.SelectedCells[2].Value.ToString(); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string cliente = datalistadoEnProc.SelectedCells[6].Value.ToString(); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string unidad = datalistadoEnProc.SelectedCells[7].Value.ToString(); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                crystalReport.SetParameterValue("@idOrdenServicio", idOrdenServicio);

                // Ruta de salida en el escritorio
                string rutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string rutaSalida = System.IO.Path.Combine(rutaEscritorio, "OT número " + codigoOrdenServicio + " - " + cliente + " - " + unidad + ".pdf");

                // Exportar a PDF
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaSalida);

                MessageBox.Show($"Reporte exportado correctamente a: {rutaSalida}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al exportar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtDescripcionSub_Producto_TextChanged(object sender, EventArgs e)
        {

        }

        private void panelIngresoCantidades_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnRegresarControl_Click(object sender, EventArgs e)
        {
            panelControlCalidad.Visible = false;
            datalistadoObs.Enabled = true;
        }
    }
}

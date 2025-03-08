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

namespace ArenasProyect3.Modulos.Produccion.ConsultasOP
{
    public partial class ListadoOrdenProduccion : Form
    {
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        private Cursor curAnterior = null;
        int totalCantidades = 0;

        //CONMSTRUCTOR DE MI FORMULARIO
        public ListadoOrdenProduccion()
        {
            InitializeComponent();
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO Y VER SI ESTAN VENCIDOS
        public void CargarColoresListadoOPGeneral()
        {
            try
            {
                //VARIABLE DE FECHA
                var DateAndTime = DateTime.Now;
                //RECORRER MI LISTADO PARA VALIDAR MIS OPs, SI ESTAN VENCIDAS O NO
                foreach (DataGridViewRow datorecuperado in datalistadoTodasOP.Rows)
                {
                    //RECUERAR LA FECHA Y EL CÓDIGO DE MI OP
                    DateTime fechaEntrega = Convert.ToDateTime(datorecuperado.Cells["FECHA DE ENTREGA"].Value);
                    int codigoOP = Convert.ToInt32(datorecuperado.Cells["ID"].Value);
                    string estadoOP = Convert.ToString(datorecuperado.Cells["ESTADO"].Value);

                    int cantidadEsperada = Convert.ToInt32(datorecuperado.Cells["CANTIDAD"].Value);
                    int cantidadRealizada = Convert.ToInt32(datorecuperado.Cells["CANTIDAD REALIZADA"].Value);

                    if (estadoOP != "ANULADO")
                    {
                        //SI LA FECHA DE VALIDEZ ES MAYOR A LA FECHA ACTUAL CONSULTADA
                        if (fechaEntrega == DateAndTime.Date)
                        {
                            //CAMBIAR EL ESTADO DE MI COTIZACIÓN
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("CambiarEstadoOP", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idOP", codigoOP);
                            cmd.Parameters.AddWithValue("@estadoOP", 2);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        else if (fechaEntrega < DateAndTime.Date)
                        {
                            //CAMBIAR EL ESTADO DE MI COTIZACIÓN
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("CambiarEstadoOP", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idOP", codigoOP);
                            cmd.Parameters.AddWithValue("@estadoOP", 3);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        else if (fechaEntrega > DateAndTime)
                        {
                            //CAMBIAR EL ESTADO DE MI COTIZACIÓN
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("CambiarEstadoOP", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idOP", codigoOP);
                            cmd.Parameters.AddWithValue("@estadoOP", 1);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }

                        if (cantidadEsperada == cantidadRealizada)
                        {
                            //CAMBIAR EL ESTADO DE MI OP
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("CambiarEstadoOP", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idOP", codigoOP);
                            cmd.Parameters.AddWithValue("@estadoOP", 4);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO
        public void ColoresListado()
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= datalistadoTodasOP.RowCount - 1; i++)
                {
                    if (datalistadoTodasOP.Rows[i].Cells[13].Value.ToString() == "FUERA DE FECHA")
                    {
                        datalistadoTodasOP.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    else if (datalistadoTodasOP.Rows[i].Cells[13].Value.ToString() == "LÍMITE")
                    {
                        datalistadoTodasOP.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Orange;
                    }
                    else if (datalistadoTodasOP.Rows[i].Cells[13].Value.ToString() == "PENDIENTE")
                    {
                        datalistadoTodasOP.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else if (datalistadoTodasOP.Rows[i].Cells[13].Value.ToString() == "CULMINADO")
                    {
                        datalistadoTodasOP.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.DarkGreen;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //VIZUALIZAR DATOS EXCEL--------------------------------------------------------------------
        public void MostrarExcel()
        {
            datalistadoExcel.Rows.Clear();

            foreach (DataGridViewRow dgv in datalistadoTodasOP.Rows)
            {
                string numeroOP = dgv.Cells[2].Value.ToString();
                string fechaInicio = dgv.Cells[3].Value.ToString();
                string fechaFinal = dgv.Cells[4].Value.ToString();
                string cliente = dgv.Cells[5].Value.ToString();
                string unidad = dgv.Cells[6].Value.ToString();
                string item = dgv.Cells[7].Value.ToString();
                string descripcionDescripcion = dgv.Cells[8].Value.ToString();
                string cantidad = dgv.Cells[9].Value.ToString();
                string color = dgv.Cells[10].Value.ToString();
                string numeroPedido = dgv.Cells[11].Value.ToString();
                string cantidadRealizada = dgv.Cells[12].Value.ToString();
                string estado = dgv.Cells[12].Value.ToString();
                string estadoOC = dgv.Cells[14].Value.ToString();

                datalistadoExcel.Rows.Add(new[] { numeroOP, fechaInicio, fechaFinal, cliente, unidad, item, descripcionDescripcion, cantidad, color, numeroPedido, estado, cantidadRealizada, estado, estadoOC });
            }
        }

        //PRIMERA CARGA DE MI FORMULARIO
        private void ListadoOrdenProduccion_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            datalistadoTodasOP.DataSource = null;
            cboBusqeuda.SelectedIndex = 0;

            //PREFILES Y PERSIMOS---------------------------------------------------------------
            if (Program.RangoEfecto != 1)
            {
                //btnAnularPedido.Visible = false;
                //lblAnularPedido.Visible = false;
            }
            //---------------------------------------------------------------------------------
        }

        //FUNCION PARA VERIFICAR SI HAY UNA CANTIDAD 
        public void MostrarCantidadesSegunOP(int idOrdenProduccion)
        {
            totalCantidades = 0;

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarCantidadesSegunOP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idOrdenProduccion", idOrdenProduccion);
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

        //LISTADO DE OP Y SELECCION DE PDF Y ESTADO DE OP---------------------
        //MOSTRAR OP AL INCIO 
        public void MostrarOrdenProduccionPorFecha(DateTime fechaInicio, DateTime fechaTermino)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarOrdenProduccionPorFecha", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoTodasOP.DataSource = dt;
            con.Close();
            RedimensionarListadoGeneralPedido(datalistadoTodasOP);
        }

        //MOSTRAR OP POR CLIENTE
        public void MostrarOrdenProduccionPorCliente(DateTime fechaInicio, DateTime fechaTermino, string cliente)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarOrdenProduccionPorCliente", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            cmd.Parameters.AddWithValue("@cliente", cliente);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoTodasOP.DataSource = dt;
            con.Close();
            RedimensionarListadoGeneralPedido(datalistadoTodasOP);
        }

        //MOSTRAR OP POR CODIGO OP
        public void MostrarOrdenProduccionPorCodigoOP(DateTime fechaInicio, DateTime fechaTermino, string codigoOP)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarOrdenProduccionPorCodigoOP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            cmd.Parameters.AddWithValue("@codigoOP", codigoOP);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoTodasOP.DataSource = dt;
            con.Close();
            RedimensionarListadoGeneralPedido(datalistadoTodasOP);
        }

        //MOSTRAR OP POR CODIGO OP
        public void MostrarOrdenProduccionPorDescripcion(DateTime fechaInicio, DateTime fechaTermino, string descripcipon)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarOrdenProduccionPorDescripcion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            cmd.Parameters.AddWithValue("@descripcion", descripcipon);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoTodasOP.DataSource = dt;
            con.Close();
            RedimensionarListadoGeneralPedido(datalistadoTodasOP);
        }

        //FUNCION PARA REDIMENSIONAR MIS LISTADOS
        public void RedimensionarListadoGeneralPedido(DataGridView DGV)
        {
            //REDIEMNSION DE PEDIDOS
            DGV.Columns[2].Width = 80;
            DGV.Columns[3].Width = 80;
            DGV.Columns[4].Width = 80;
            DGV.Columns[5].Width = 300;
            DGV.Columns[6].Width = 130;
            DGV.Columns[7].Width = 35;
            DGV.Columns[8].Width = 300;
            DGV.Columns[9].Width = 55;
            DGV.Columns[10].Width = 75;
            DGV.Columns[11].Width = 75;
            DGV.Columns[12].Width = 60;
            DGV.Columns[13].Width = 110;
            DGV.Columns[14].Width = 65;
            //SE HACE NO VISIBLE LAS COLUMNAS QUE NO LES INTERESA AL USUARIO
            DGV.Columns[1].Visible = false;
            DGV.Columns[15].Visible = false;
            DGV.Columns[16].Visible = false;
            DGV.Columns[15].Visible = false;
            DGV.Columns[16].Visible = false;
            DGV.Columns[17].Visible = false;
            DGV.Columns[18].Visible = false;
            DGV.Columns[19].Visible = false;
            DGV.Columns[20].Visible = false;
            DGV.Columns[21].Visible = false;
            //SE BLOQUEA MI LISTADO
            DGV.Columns[2].ReadOnly = true;
            DGV.Columns[3].ReadOnly = true;
            DGV.Columns[4].ReadOnly = true;
            DGV.Columns[5].ReadOnly = true;
            DGV.Columns[6].ReadOnly = true;
            DGV.Columns[7].ReadOnly = true;
            DGV.Columns[8].ReadOnly = true;
            DGV.Columns[9].ReadOnly = true;
            DGV.Columns[10].ReadOnly = true;
            DGV.Columns[11].ReadOnly = true;
            DGV.Columns[12].ReadOnly = true;
            DGV.Columns[13].ReadOnly = true;
            DGV.Columns[14].ReadOnly = true;

            CargarColoresListadoOPGeneral();
            ColoresListado();

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN
        private void datalistadoTodasOP_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoTodasOP.Columns[e.ColumnIndex].Name == "detalles")
            {
                this.datalistadoTodasOP.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoTodasOP.Cursor = curAnterior;
            }
        }

        //EVENTO PARA ABRIR EL INGRESO DE CANTIDADES
        private void datalistadoTodasOP_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoTodasOP.CurrentRow != null)
            {
                int count = 0;
                foreach (DataGridViewRow row in datalistadoTodasOP.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value))
                    {
                        count++;
                    }
                }

                if (count == 0) { count = 1; }

                datalistadoTodasOP.Enabled = false;
                panelIngresoCantidades.Visible = true;
                txtOpsSeleccionadas.Text = Convert.ToString(count);
                //CARGA DE DAOTS
                txtCodigoOP.Text = datalistadoTodasOP.SelectedCells[2].Value.ToString();
                int IdOrdenProduccion = Convert.ToInt32(datalistadoTodasOP.SelectedCells[1].Value.ToString());
                txtDescripcionProducto.Text = datalistadoTodasOP.SelectedCells[5].Value.ToString();
                txtCantidadTotalOP.Text = datalistadoTodasOP.SelectedCells[9].Value.ToString();
                txtCantidadRequerida.Text = datalistadoTodasOP.SelectedCells[9].Value.ToString();
                dtpFechaRealizada.Value = DateTime.Now;
                txtCantidadRealizada.Text = "";
                txtCantidadRestante.Text = "";
                MostrarCantidadesSegunOP(IdOrdenProduccion);
                lblCantidadTotalInghresada.Text = Convert.ToString(totalCantidades);
                txtCantidadRestante.Text = Convert.ToString(Convert.ToInt32(txtCantidadRequerida.Text) - Convert.ToInt32(lblCantidadTotalInghresada.Text));

                if (count != 1)
                {
                    btnGenerarGuardarCantidades.Visible = true;
                    lblGenerarGuardarCantidades.Visible = true;
                    btnGuardarCantidad.Visible = false;
                    lblGuardarCantidad.Visible = false;
                    txtCantidadRealizada.ReadOnly = true;
                    txtCantidadRealizada.Text = "Gen. Automática";
                    lblIdOP.Text = "Varios";
                    txtCantidadRestante.Text = "0";
                }
                else
                {
                    btnGuardarCantidad.Visible = true;
                    lblGuardarCantidad.Visible = true;
                    btnGenerarGuardarCantidades.Visible = false;
                    lblGenerarGuardarCantidades.Visible = false;
                    txtCantidadRealizada.ReadOnly = false;
                    lblIdOP.Text = datalistadoTodasOP.SelectedCells[1].Value.ToString();
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una OP para poder continuar.", "Validación del Sistema");
            }
        }

        //MOSTRAR PEDIDOS SEGUN LAS FECHAS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
            MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OP SEGUN LAS FECHAS
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
            MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OP SEGUN LAS FECHAS
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
            MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OPRDENES PRODUCCION DEPENDIENTO LA OPCIÓN ESCOGIDA
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            if (cboBusqeuda.Text == "CÓDIGO OP")
            {
                MostrarOrdenProduccionPorCodigoOP(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
                MostrarOrdenProduccionPorCodigoOP(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
            }
            else if (cboBusqeuda.Text == "CLIENTE")
            {
                MostrarOrdenProduccionPorCliente(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
                MostrarOrdenProduccionPorCliente(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
            }
            else if (cboBusqeuda.Text == "DESCRIPCIÓN PRODUCTO")
            {
                MostrarOrdenProduccionPorDescripcion(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
                MostrarOrdenProduccionPorDescripcion(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
            }
        }

        //GENERACION DE REPORTES
        private void btnGenerarOrdenProduccionPDF_Click(object sender, EventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoTodasOP.CurrentRow != null)
            {
                string codigoOrdenProduccion = datalistadoTodasOP.Rows[datalistadoTodasOP.CurrentRow.Index].Cells[1].Value.ToString();
                Visualizadores.VisualizarOrdenProduccion frm = new Visualizadores.VisualizarOrdenProduccion();
                frm.lblCodigo.Text = codigoOrdenProduccion;

                frm.Show();
            }
            else
            {
                MessageBox.Show("Debe seleccionar una OP para poder generar el PDF.", "Validación del Sistema");
            }
        }

        //CARGAR MI PLANO DE PRODUCTO ASIGANDO A LA OP
        private void btnPlano_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(datalistadoTodasOP.SelectedCells[16].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Documento no encontrado, hubo un error al momento de cargar el archivo.", ex.Message);
            }
        }

        //CARGAR MI OC TRAIDO DESDE MI PEDIDO
        private void btnOC_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(datalistadoTodasOP.SelectedCells[15].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Documento no encontrado, hubo un error al momento de cargar el archivo.", ex.Message);
            }
        }

        //EVENTO PARA GUARDAR MI S CANTIDADES INGRESADAS
        private void btnGuardarCantidad_Click(object sender, EventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoTodasOP.CurrentRow != null)
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
                            cmd = new SqlCommand("IngresarRegistroCantidad", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idOrdenProduccion", lblIdOP.Text);
                            cmd.Parameters.AddWithValue("@cantidad", txtCantidadRealizada.Text);
                            cmd.Parameters.AddWithValue("@fechaRegistro", Convert.ToDateTime(dtpFechaRealizada.Value));
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Cantidd ingresada correctamente.", "Validación del Sistema");
                            MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
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
                MessageBox.Show("Debe seleccionar una OP para poder continuar.", "Validación del Sistema");
            }
        }

        //EVENTO PARA GUARDAR VARIAS CANTIDADES INGRESADAS
        private void btnGenerarGuardarCantidades_Click(object sender, EventArgs e)
        {
            List<int> idOPSeleccionada = new List<int>();
            List<int> CantidadTotalOPSeleccionada = new List<int>();

            foreach (DataGridViewRow row in datalistadoTodasOP.Rows)
            {
                DataGridViewCheckBoxCell checkBox = row.Cells[0] as DataGridViewCheckBoxCell;

                if (checkBox != null && Convert.ToBoolean(checkBox.Value) == true)
                {
                    try
                    {
                        int idOp = Convert.ToInt32(row.Cells[1].Value.ToString());
                        int cantidadEsperada = Convert.ToInt32(row.Cells[9].Value.ToString());
                        int cantidadHecha = Convert.ToInt32(row.Cells[12].Value.ToString());
                        int TotalCantidad = cantidadEsperada - cantidadHecha;

                        if (TotalCantidad != 0)
                        {
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("IngresarRegistroCantidad", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idOrdenProduccion", idOp);
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
            MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
            LimpiarCantidades();
        }

        //EVENTO PARA RETROCEDER O SALIR DE MI VENTANA DE INGRESO DE CANTIDADES
        private void btnSalirCantidad_Click(object sender, EventArgs e)
        {
            LimpiarCantidades();
        }

        //FUNCION PARA LIMPIAR LAS CANTIDADES
        public void LimpiarCantidades()
        {
            datalistadoTodasOP.Enabled = true;
            panelIngresoCantidades.Visible = false;
            txtOpsSeleccionadas.Text = "";
            txtCantidadRealizada.Text = "";
            txtCantidadRestante.Text = "";
        }

        //ANULACION DE MI OP - PEDIDO - COTIZACION
        private void btnAnularOP_Click(object sender, EventArgs e)
        {
            LimpiarAnulacionPedido();
            panleAnulacion.Visible = true;
            datalistadoTodasOP.Enabled = false;
        }

        //FUNCION PARA PROCEDER A ANULAR MI PEDIDO, COTIZACION Y PRODICCION
        private void btnProcederAnulacion_Click(object sender, EventArgs e)
        {
            //if (datalistadoTodasPedido.CurrentRow != null)
            //{
            //    int idOrdenProduccion = Convert.ToInt32(datalistadoTodasPedido.SelectedCells[1].Value.ToString());
            //    int idPedido = Convert.ToInt32(datalistadoTodasPedido.SelectedCells[1].Value.ToString());
            //    string idCotizacion = datalistadoTodasPedido.SelectedCells[13].Value.ToString();

            //    VerificarOPxPedidoAnulacion(idPedido);

            //    if (datalistadoBuscarOPxPedidoAnulacion.RowCount > 0)
            //    {
            //        ordenProduccion = datalistadoBuscarOPxPedidoAnulacion.RowCount;
            //    }

            //    DialogResult boton = MessageBox.Show("¿Realmente desea anular esta orden de producción?. Se anulará la cotización y pedido asociada ha esta orden de producción.", "Validación del Sistema", MessageBoxButtons.OKCancel);
            //    if (boton == DialogResult.OK)
            //    {
            //        if (ordenProduccion > 0)
            //        {
            //            MessageBox.Show("El pedido que desea anular ya tiene una orden de producción generada.", "Validación del Sistema", MessageBoxButtons.OK);
            //        }
            //        else
            //        {
            //            try
            //            {
            //                SqlConnection con = new SqlConnection();
            //                SqlCommand cmd = new SqlCommand();
            //                con.ConnectionString = Conexion.ConexionMaestra.conexion;
            //                con.Open();
            //                cmd = new SqlCommand("AnularPedido", con);
            //                cmd.CommandType = CommandType.StoredProcedure;
            //                cmd.Parameters.AddWithValue("@idOrdenProduccion", idPedido);
            //                cmd.Parameters.AddWithValue("@idPedido", idPedido);
            //                cmd.Parameters.AddWithValue("@idCotizacion", idCotizacion);
            //                cmd.Parameters.AddWithValue("@mensajeAnulado", txtJustificacionAnulacion.Text);
            //                cmd.ExecuteNonQuery();
            //                con.Close();

            //                MessageBox.Show("Cotización, pedido y orden de producción asociado a esta, anuladas exitosamente.", "Validación del Sistema");
            //                MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
            //                LimpiarAnulacionPedido();
            //                panleAnulacion.Visible = false;
            //                datalistadoTodasOP.Enabled = true;
            //            }
            //            catch (Exception ex)
            //            {
            //                MessageBox.Show(ex.Message);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Debe seleccionar una orden de producción para poder anularlo.", "Validación del Sistema");
            //}
        }

        //BOTON PARA RETROCEDER DE LA ANULACION
        private void btnRetrocederAnulacion_Click(object sender, EventArgs e)
        {
            LimpiarAnulacionPedido();
            panleAnulacion.Visible = false;
            datalistadoTodasOP.Enabled = true;
        }

        //FUNCION PARA LIMPIAR MIS CONTROLES ORIETADO A ANULACION DE PEDIDO
        public void LimpiarAnulacionPedido()
        {
            //datalistadoBuscarOPxPedidoAnulacion.Rows.Clear();
            //txtJustificacionAnulacion.Text = "";
        }
        //----------------------------------------------------------------------------------------------------------
        //MODIFICAR MI FECHA DE ENTREGA DE MI ORDEN DE PRODUCCION
        private void btnModificarFecha_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasOP.CurrentRow != null)
            {
                datalistadoTodasOP.Enabled = false;
                panelModiFechaEntrega.Visible = true;
                int IdOP = Convert.ToInt32(datalistadoTodasOP.SelectedCells[1].Value);
                txtModiCodigoOP.Text = datalistadoTodasOP.SelectedCells[2].Value.ToString();
                dtpModiFechaOP.Value = Convert.ToDateTime(datalistadoTodasOP.SelectedCells[3].Value);
                dtpModiFechaEntrega.Value = Convert.ToDateTime(datalistadoTodasOP.SelectedCells[4].Value);
                txtModiObservacionModiFecha.Text = "";
            }
        }

        //CONFIRMAR MI MODIFICACION DE FECHAS
        private void btnModiConfirmar_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasOP.CurrentRow != null)
            {
                int idOrdenProduccion = Convert.ToInt32(datalistadoTodasOP.SelectedCells[1].Value.ToString());

                DialogResult boton = MessageBox.Show("¿Realmente desea modificar esta fecha de orden de producción?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        SqlCommand cmd = new SqlCommand();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        cmd = new SqlCommand("ModificarFechaOrdenProduccion", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idOrdenProduccion", idOrdenProduccion);
                        cmd.Parameters.AddWithValue("@fechaEntrega", dtpModiFechaEntrega.Value);
                        cmd.Parameters.AddWithValue("@observacion", txtModiObservacionModiFecha.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Fecha de entrega de mi orden de producción modificada exitosamente.", "Validación del Sistema");
                        MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
                        MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
                        datalistadoTodasOP.Enabled = true;
                        panelModiFechaEntrega.Visible = false;
                        txtModiObservacionModiFecha.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una orden de producción para poder editarla.", "Validación del Sistema");
            }
        }

        //FUNCION PARA RETROCEDER MI MODIFICACION DE FECHA
        private void btnModiRetroceder_Click(object sender, EventArgs e)
        {
            txtModiObservacionModiFecha.Text = "";
            datalistadoTodasOP.Enabled = true;
            panelModiFechaEntrega.Visible = false;
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
            sl.SetColumnWidth(4, 50);
            sl.SetColumnWidth(5, 35);
            sl.SetColumnWidth(6, 10);
            sl.SetColumnWidth(7, 50);
            sl.SetColumnWidth(8, 15);
            sl.SetColumnWidth(9, 15);
            sl.SetColumnWidth(10, 15);
            sl.SetColumnWidth(11, 15);
            sl.SetColumnWidth(12, 20);
            sl.SetColumnWidth(13, 15);

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
                sl.SetCellValue(ir, 12, row.Cells[11].Value.ToString());
                sl.SetCellValue(ir, 13, row.Cells[12].Value.ToString());
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
                sl.SetCellStyle(ir, 12, styleC);
                sl.SetCellStyle(ir, 13, styleC);
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

                rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeOrdenProduccion.rpt");

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
                int idOrdenProduccion = Convert.ToInt32(datalistadoTodasOP.SelectedCells[1].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string codigoOrdenProduccion = datalistadoTodasOP.SelectedCells[2].Value.ToString(); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string cliente = datalistadoTodasOP.SelectedCells[5].Value.ToString(); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string unidad = datalistadoTodasOP.SelectedCells[6].Value.ToString(); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                crystalReport.SetParameterValue("@idOrdenProduccion", idOrdenProduccion);

                // Ruta de salida en el escritorio
                string rutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string rutaSalida = System.IO.Path.Combine(rutaEscritorio, "OP número " + codigoOrdenProduccion + " - " + cliente + " - " + unidad + ".pdf");

                // Exportar a PDF
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaSalida);

                MessageBox.Show($"Reporte exportado correctamente a: {rutaSalida}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al exportar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //EVENTO PARA VALIDAR EL INGRESO DE NUMEROS Y SIGNOS
        private void txtCantidadRealizada_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, puntos, comas y teclas de control (como retroceso)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }
        }
    }
}

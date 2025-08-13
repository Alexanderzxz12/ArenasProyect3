using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Calidad.Revision
{
    public partial class ListadoOrdenProduccion : Form
    {
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        private Cursor curAnterior = null;
        int totalCantidades = 0;

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
                            cmd = new SqlCommand("OP_CambiarEstado", con);
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
                            cmd = new SqlCommand("OP_CambiarEstado", con);
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
                            cmd = new SqlCommand("OP_CambiarEstado", con);
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
                            cmd = new SqlCommand("OP_CambiarEstado", con);
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
                    if (datalistadoTodasOP.Rows[i].Cells[14].Value.ToString() == "FUERA DE FECHA")
                    {
                        datalistadoTodasOP.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Fuchsia;
                    }
                    else if (datalistadoTodasOP.Rows[i].Cells[14].Value.ToString() == "LÍMITE")
                    {
                        datalistadoTodasOP.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Orange;
                    }
                    else if (datalistadoTodasOP.Rows[i].Cells[14].Value.ToString() == "PENDIENTE")
                    {
                        datalistadoTodasOP.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else if (datalistadoTodasOP.Rows[i].Cells[14].Value.ToString() == "CULMINADO")
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
            cmd = new SqlCommand("Calidad_MostrarCantidades", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idOrdenProduccion", idOrdenProduccion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoHistorial.DataSource = dt;
            con.Close();
            datalistadoHistorial.Columns[0].Width = 40;
            datalistadoHistorial.Columns[1].Width = 80;
            datalistadoHistorial.Columns[2].Width = 70;
            datalistadoHistorial.Columns[3].Width = 80;
            datalistadoHistorial.Columns[4].Width = 300;
            datalistadoHistorial.Columns[5].Width = 80;
            //alternarColorFilas(datalistadoHistorial);


            //CONTAR CUANTAS CANTIDADES HAY
            foreach (DataGridViewRow row in datalistadoHistorial.Rows)
            {
                totalCantidades = totalCantidades + Convert.ToInt32(row.Cells[1].Value.ToString());
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
            cmd = new SqlCommand("Calidad_MostrarPorFecha", con);
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
            cmd = new SqlCommand("Calidad_MostrarPorCliente", con);
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
            cmd = new SqlCommand("Calidad_MostrarPorCodigo", con);
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
            cmd = new SqlCommand("Calidad_MostrarPorDescripcion", con);
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
            DGV.Columns[7].Width = 40;
            DGV.Columns[8].Width = 300;
            DGV.Columns[9].Width = 60;
            DGV.Columns[10].Width = 85;
            DGV.Columns[11].Width = 75;
            DGV.Columns[12].Width = 75;
            DGV.Columns[13].Width = 75;
            DGV.Columns[14].Width = 110;
            DGV.Columns[15].Width = 65;
            //SE HACE NO VISIBLE LAS COLUMNAS QUE NO LES INTERESA AL USUARIO
            DGV.Columns[1].Visible = false;
            DGV.Columns[16].Visible = false;
            DGV.Columns[17].Visible = false;
            DGV.Columns[18].Visible = false;
            DGV.Columns[19].Visible = false;
            DGV.Columns[20].Visible = false;
            DGV.Columns[21].Visible = false;
            DGV.Columns[22].Visible = false;
            DGV.Columns[23].Visible = false;
            DGV.Columns[24].Visible = false;
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

        private void datalistadoTodasOP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoTodasOP.RowCount != 0)
            {
                DataGridViewColumn currentColumnT = datalistadoTodasOP.Columns[e.ColumnIndex];

                if (currentColumnT.Name == "detalles")
                {
                    panelControlCalidad.Visible = true;

                    lblIdOP.Text = datalistadoTodasOP.SelectedCells[1].Value.ToString();
                    txtCodigoOP.Text = datalistadoTodasOP.SelectedCells[2].Value.ToString();
                    txtDescripcionProducto.Text = datalistadoTodasOP.SelectedCells[8].Value.ToString();
                    txtCantidadTotalPedido.Text = datalistadoTodasOP.SelectedCells[9].Value.ToString();
                    int IdOrdenProduccion = Convert.ToInt32(datalistadoTodasOP.SelectedCells[1].Value.ToString());
                    MostrarCantidadesSegunOP(IdOrdenProduccion);
                    txtCantidadRestante.Text = Convert.ToString(Convert.ToInt32(txtCantidadTotalPedido.Text) - Convert.ToInt32(datalistadoTodasOP.SelectedCells[13].Value.ToString()));
                    txtPesoTeorico.Text = "0.00";
                    txtPesoReal.Text = "0.00";
                    txtObservaciones.Text = "";
                }
            }
        }

        //CERRAR MI PANEL DE CONTROL DE CALIDAD
        private void btnCerrarDetallesOPCantidades_Click(object sender, EventArgs e)
        {
            panelControlCalidad.Visible = false;
        }

        //MOSTRAR OP SEGUN LAS FECHAS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
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

        //MOSTRAR OP SEGUN LAS FECHAS
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
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
                Process.Start(datalistadoTodasOP.SelectedCells[17].Value.ToString());
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
                Process.Start(datalistadoTodasOP.SelectedCells[16].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Documento no encontrado, hubo un error al momento de cargar el archivo.", ex.Message);
            }
        }

        //APROBAR LAS CANTIDADES INGRESADAS
        private void btnAprobar_Click(object sender, EventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoTodasOP.CurrentRow != null)
            {
                if (txtCantidadInspeccionar.Text == "" || txtCantidadInspeccionar.Text == "0")
                {
                    MessageBox.Show("Debe ingresar una cantidad válida para poder aprobar o desaprobar.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else if (Convert.ToInt32(txtCantidadInspeccionar.Text) > Convert.ToInt32(txtCantidadRestante.Text))
                {
                    MessageBox.Show("No se puede revisar más de la cantidad restante", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    DialogResult boton = MessageBox.Show("¿Realmente desea aprobar esta cantidad?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                    if (boton == DialogResult.OK)
                    {
                        try
                        {
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("Calidad_IngresarRegistroCantidad", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idOrdenProduccion", Convert.ToInt32(lblIdOP.Text));
                            cmd.Parameters.AddWithValue("@cantidad", Convert.ToInt32(txtCantidadInspeccionar.Text));
                            cmd.Parameters.AddWithValue("@fechaRegistro", Convert.ToDateTime(dtpFechaRealizada.Value));
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Cantidd revisada correctamente.", "Validación del Sistema");
                            MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
                            MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
                            //LimpiarCantidades();
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

        //DESAPROBAR LAS CANTIDADES INGRESADAS
        private void btnDesaprobar_Click(object sender, EventArgs e)
        {

        }


        //VALIDAR QUE SOLO INGRESE NÚMEROS ENTEROS
        private void txtCantidadInspeccionar_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo dígitos y teclas de control como Backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Bloquea el carácter
            }
        }




    }
}

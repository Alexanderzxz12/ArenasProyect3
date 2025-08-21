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
            //REORDENAMIENTO DE COLUMNAS
            datalistadoHistorial.Columns[1].Width = 70;
            datalistadoHistorial.Columns[2].Width = 50;
            datalistadoHistorial.Columns[3].Width = 300;
            datalistadoHistorial.Columns[4].Width = 80;
            datalistadoHistorial.Columns[5].Width = 80;
            //COLUMNAS NO VISIBLES
            datalistadoHistorial.Columns[0].Visible = false;
            ColoresListadoCantidades();
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
                    txtCantidadTotalPedido.Text = datalistadoTodasOP.SelectedCells[12].Value.ToString();
                    MostrarCantidadesSegunOP(Convert.ToInt32(lblIdOP.Text));
                    lblCantidadRealizada.Text = datalistadoTodasOP.SelectedCells[13].Value.ToString();
                    txtCantidadRestante.Text = Convert.ToString(Convert.ToInt32(txtCantidadTotalPedido.Text) - Convert.ToInt32(lblCantidadRealizada.Text));
                    txtPesoTeorico.Text = "0.00";
                    txtPesoReal.Text = "0.00";
                    txtObservaciones.Text = "";
                    btnGenerarCSM.Visible = false;
                    lblGenerarCSM.Visible = false;
                }
            }
        }

        //CERRAR MI PANEL DE CONTROL DE CALIDAD
        private void btnCerrarDetallesOPCantidades_Click(object sender, EventArgs e)
        {
            panelControlCalidad.Visible = false;
            LimpiarCantidades();
            MostrarOrdenProduccionPorDescripcion(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
            MostrarOrdenProduccionPorDescripcion(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
        }

        //CERRAR MI PANEL DE CONTROL DE CALIDAD
        private void btnRegresarControl_Click(object sender, EventArgs e)
        {
            panelControlCalidad.Visible = false;
            LimpiarCantidades();
            MostrarOrdenProduccionPorDescripcion(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
            MostrarOrdenProduccionPorDescripcion(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
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
                if (txtCantidadInspeccionar.Text == "" || txtCantidadInspeccionar.Text == "0" || txtObservaciones.Text == "")
                {
                    MessageBox.Show("Debe ingresar una cantidad u obserbación válida para poder aprobar o desaprobar.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else if (Convert.ToInt32(txtCantidadInspeccionar.Text) > Convert.ToInt32(txtCantidadRestante.Text))
                {
                    MessageBox.Show("No se puede revisar más de la cantidad restante.", "Validación del Sistema", MessageBoxButtons.OK);
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
                            cmd.Parameters.AddWithValue("@pesoTeorico", Convert.ToDecimal(txtPesoTeorico.Text));
                            cmd.Parameters.AddWithValue("@pesoReal", Convert.ToDecimal(txtPesoReal.Text));
                            cmd.Parameters.AddWithValue("@observaciones", txtObservaciones.Text);
                            cmd.Parameters.AddWithValue("@estadoAD", 2);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Cantidad revisada correctamente.", "Validación del Sistema");
                            txtCantidadRestante.Text = Convert.ToString(Convert.ToInt16(txtCantidadRestante.Text) - Convert.ToInt16(txtCantidadInspeccionar.Text));
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

        //DESAPROBAR LAS CANTIDADES INGRESADAS
        private void btnDesaprobar_Click(object sender, EventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoTodasOP.CurrentRow != null)
            {
                if (txtCantidadInspeccionar.Text == "" || txtCantidadInspeccionar.Text == "0" || txtObservaciones.Text == "")
                {
                    MessageBox.Show("Debe ingresar una cantidad u obserbación válida para poder aprobar o desaprobar.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else if (Convert.ToInt32(txtCantidadInspeccionar.Text) > Convert.ToInt32(txtCantidadRestante.Text))
                {
                    MessageBox.Show("No se puede revisar más de la cantidad restante.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    DialogResult boton = MessageBox.Show("¿Realmente desea desaprobar esta cantidad?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
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
                            cmd.Parameters.AddWithValue("@pesoTeorico", Convert.ToDecimal(txtPesoTeorico.Text));
                            cmd.Parameters.AddWithValue("@pesoReal", Convert.ToDecimal(txtPesoReal.Text));
                            cmd.Parameters.AddWithValue("@observaciones", txtObservaciones.Text);
                            cmd.Parameters.AddWithValue("@estadoAD", 0);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Cantidad revisada correctamente.", "Validación del Sistema");
                            txtCantidadRestante.Text = Convert.ToString(Convert.ToInt16(txtCantidadRestante.Text) - Convert.ToInt16(txtCantidadInspeccionar.Text));
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

        //CARGAR PERSONA QUE AUTORIZA
        public void CargarResposnableAutoriza()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdUsuarios, Nombres + ' ' + Apellidos AS [NOMBRES] FROM Usuarios WHERE Estado = 'Activo' ORDER BY Nombres", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboAutorizaSNC.ValueMember = "IdUsuarios";
                cboAutorizaSNC.DisplayMember = "NOMBRES";
                cboAutorizaSNC.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //LIMPIAR MIS DATOS DE MI INGRESO DE CANTIDADES
        public void LimpiarCantidades()
        {
            txtCantidadInspeccionar.Text = "";
            txtPesoReal.Text = "0.00";
            txtObservaciones.Text = "";
            MostrarCantidadesSegunOP(Convert.ToInt16(lblIdOP.Text));
        }

        //GENERA UNA SALIDA NO CONFORME
        private void btnGenerarCSM_Click(object sender, EventArgs e)
        {
            panelSNC.Visible = true;
            txtReponsableRegistro.Text = Program.UnoNombreUnoApellidoUsuario;
            txtOrdenProduccionSNC.Text = txtCodigoOP.Text;
            CargarResposnableAutoriza();
            LimpiarSNC();
        }

        //FUNCION PARA LIMPIAR MI SNC
        public void LimpiarSNC()
        {
            txtDescripcionSNC.Text = "";
            txtAccionesTomadasSNC.Text = "";
            dtpInicioSNC.Value = DateTime.Now;
            dtpFinalSNC.Value = DateTime.Now;
            txtImagen1.Text = "";
            txtImagen2.Text = "";
            txtImagen3.Text = "";
            txtOtrosSNC.Text = "";
            ckLiberacion.Checked = false;
            ckRectificacion.Checked = false;
            ckCorrecion.Checked = false;
            ckRecuperacionComponetntes.Checked = false;
            ckReproceso.Checked = false;
            ckDestruccion.Checked = false;
            ckOtros.Checked = false;
        }

        //BOTON PARA GUARDAR MI SNC
        private void btnGuardarSNC_Click(object sender, EventArgs e)
        {
            //SI LOS CAMPOS ESTAN VACIOS
            if (txtReponsableRegistro.Text == "" || txtOrdenProduccionSNC.Text == "0" || txtDescripcionSNC.Text == "" || txtAccionesTomadasSNC.Text == "")
            {
                MessageBox.Show("Debe ingresar todos los campos para poder registrar la SNC.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                DialogResult boton = MessageBox.Show("¿Realmente desea generar esta SNC?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        SqlCommand cmd = new SqlCommand();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        cmd = new SqlCommand("Calidad_IngresarSNC", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idDetalleCantidadCalidad", Convert.ToInt32(datalistadoHistorial.SelectedCells[0].Value.ToString()));
                        cmd.Parameters.AddWithValue("@idUsuarioResponsable", Program.IdUsuario);
                        cmd.Parameters.AddWithValue("@fechaHallazgo", dtpFechaHallazgo.Value);
                        cmd.Parameters.AddWithValue("@IdOp", Convert.ToInt32(lblIdOP.Text));
                        cmd.Parameters.AddWithValue("@descripcionSNC", txtDescripcionSNC.Text);
                        cmd.Parameters.AddWithValue("@descripcionAcciones", txtAccionesTomadasSNC.Text);
                        cmd.Parameters.AddWithValue("@idUsuarioAutorizacion", cboAutorizaSNC.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@inicio", dtpInicioSNC.Value);
                        cmd.Parameters.AddWithValue("@finaliza", dtpFinalSNC.Value);
                        cmd.Parameters.AddWithValue("@imagen1", txtImagen1.Text);
                        cmd.Parameters.AddWithValue("@imagen2", txtImagen2.Text);
                        cmd.Parameters.AddWithValue("@imagen3", txtImagen3.Text);
                        cmd.Parameters.AddWithValue("@skLiberacion", ckLiberacion.Checked);
                        cmd.Parameters.AddWithValue("@skCorrecion", ckCorrecion.Checked);
                        cmd.Parameters.AddWithValue("@skReproceso", ckReproceso.Checked);
                        cmd.Parameters.AddWithValue("@skReclasificacion", ckRectificacion.Checked);
                        cmd.Parameters.AddWithValue("@skRecuperacion", ckRecuperacionComponetntes.Checked);
                        cmd.Parameters.AddWithValue("@skDestruccion", ckDestruccion.Checked);
                        cmd.Parameters.AddWithValue("@skOtros", ckOtros.Checked);
                        cmd.Parameters.AddWithValue("@descripcionOtros", txtOtrosSNC.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Salida No Conforme registrada correctamente.", "Validación del Sistema");
                        MostrarCantidadesSegunOP(Convert.ToInt16(lblIdOP.Text));
                        panelSNC.Visible = false;
                        LimpiarSNC();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //BOTON PARA SALIR DE MI SNC
        private void btnCerrarSNC_Click(object sender, EventArgs e)
        {
            panelSNC.Visible = false;
            LimpiarSNC();
        }

        //VISUALIZAR DOCUMETNOS DEL CONTROL DE CALIDAD
        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoHistorial.CurrentRow != null)
            {
                //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO DESAPROBADO
                string codigoDetalleCantidadCalidad = datalistadoHistorial.Rows[datalistadoHistorial.CurrentRow.Index].Cells[0].Value.ToString();
                Visualizadores.VisualizarSNC frm = new Visualizadores.VisualizarSNC();
                frm.lblCodigo.Text = codigoDetalleCantidadCalidad;
                //CARGAR VENTANA
                frm.Show();
            }
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO DE CANTIDADES
        public void ColoresListadoCantidades()
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= datalistadoHistorial.RowCount - 1; i++)
                {
                    if (datalistadoHistorial.Rows[i].Cells[5].Value.ToString() == "APROBADO")
                    {
                        datalistadoHistorial.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                    }
                    else if (datalistadoHistorial.Rows[i].Cells[5].Value.ToString() == "DESAPROBADO")
                    {
                        datalistadoHistorial.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    else if (datalistadoHistorial.Rows[i].Cells[5].Value.ToString() == "SNC GENERADA")
                    {
                        datalistadoHistorial.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.DarkOrange;
                    }
                    else
                    {
                        datalistadoHistorial.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //SELECCIONAR MI REGISTRO SI ESTA DESAPROBADO
        private void datalistadoHistorial_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoHistorial.SelectedCells[5].Value.ToString() == "DESAPROBADO")
            {
                btnGenerarCSM.Visible = true;
                lblGenerarCSM.Visible = true;
            }
            else
            {
                btnGenerarCSM.Visible = false;
                lblGenerarCSM.Visible = false;
            }

            if(datalistadoHistorial.SelectedCells[5].Value.ToString() == "SNC GENERADA")
            {
                btnVisualizar.Visible = true;
                lblLeyendaVisualizar.Visible = true;
            }
            else
            {
                btnVisualizar.Visible = false;
                lblLeyendaVisualizar.Visible = false;
            }
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

        //ACCIONAR EL CK OTROS PARA QUE PUEDA INGRESAR UN TEXTO
        private void ckOtros_CheckedChanged(object sender, EventArgs e)
        {
            if (ckOtros.Checked == true)
            {
                txtOtrosSNC.ReadOnly = false;
            }
            else
            {
                txtOtrosSNC.ReadOnly = true;
            }
        }
    }
}

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
using static System.Windows.Forms.MonthCalendar;

namespace ArenasProyect3.Modulos.Produccion.ConsultasOT
{
    public partial class ListadoPedidosOT : Form
    {
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        string ruta = ManGeneral.Manual.manualAreaProduccion;
        private Cursor curAnterior = null;
        string VisualizarOC = "";
        DataGridView dgvActivo = null;

        public ListadoPedidosOT()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI FORMULARIO
        private void ListadoPedidosOT_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            //VerificarDGVActivo();
        }

        //CÓDIGO PARA PODER MOSTRAR LA HORA EN VIVO
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHoraFecha.Text = DateTime.Now.ToString("H:mm:ss tt");
        }

        //BUSCAR DETALLES DE MI PEDIDO
        public void BuscarPedidoPorCodigo(int idPedido)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Pedido_BuscarPorCodigo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idPedido", idPedido);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoPedido.DataSource = dt;
            con.Close();
        }

        //BUSCAR DETALLES DE MI PEDIDO
        public void BuscarPedidoPorCodigoDetalle(int idPedido)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Pedido_BuscarPorCodigoDetalles", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idPedido", idPedido);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoDetallePedido.DataSource = dt;
            con.Close();
        }

        //COMBO DE DETALLES
        //CARGAR SEDE
        public void CargarSede()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdSede, Descripcion FROM Sede WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            System.Data.DataTable dt = new System.Data.DataTable();
            data.Fill(dt);
            cboSede.ValueMember = "IdSede";
            cboSede.DisplayMember = "Descripcion";
            cboSede.DataSource = dt;
        }

        //CARGAR PRIORIDAD
        public void CargarPrioridad()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdPrioridad, Descripcion FROM Prioridades WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            System.Data.DataTable dt = new System.Data.DataTable();
            data.Fill(dt);
            cboPrioridad.ValueMember = "IdPrioridad";
            cboPrioridad.DisplayMember = "Descripcion";
            cboPrioridad.DataSource = dt;
        }

        //CARGAR LOCAL
        public void CargarLocal()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdLocal, Descripcion FROM Local WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            System.Data.DataTable dt = new System.Data.DataTable();
            data.Fill(dt);
            cboLocal.ValueMember = "IdLocal";
            cboLocal.DisplayMember = "Descripcion";
            cboLocal.DataSource = dt;
        }

        //CARGAR TIPO OEPRACION
        public void CargarTipoOperacion()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand(" SELECT IdTipoOperacionPro, Nombre FROM TipoOperacionPro WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            System.Data.DataTable dt = new System.Data.DataTable();
            data.Fill(dt);
            cboOperacion.ValueMember = "IdTipoOperacionPro";
            cboOperacion.DisplayMember = "Nombre";
            cboOperacion.DataSource = dt;
        }

        //BUSCAR MI RELACION DEL PRODUCTO POR EL SEMIPORDUCIDO SI APLICA
        public void BuscarRelacionProductoSemi(string codigoFormulacion)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("OP_BuscarRelacionFormulacion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigoFormulacion", codigoFormulacion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoBuscarRelacionFormulacion.DataSource = dt;
            con.Close();
        }

        //BUSCAR DETALLES Y MATERIALES DE MI FORMULACION
        public void BuscarMaterialesFormulacion(string codigoFormulacion)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Pedido_BuscarMaterialesFormulacion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigoFormulacion", codigoFormulacion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoDetallesMaterialesFormulacion.DataSource = dt;
            con.Close();
        }

        //BUSCAR DETALLES Y MATERIALES DE MI FORMULACION
        public void BuscarMaterialesFormulacionSemi(string codigoFormulacion)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Pedido_BuscarMaterialesFormulacionSemi", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigoFormulacion", codigoFormulacion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoDetallesMaterialesFormulacionSemi.DataSource = dt;
            con.Close();
        }

        //BUSCAR LA LINEA DE MI FORMULACION
        public void BuscarLineaFormulacion(string codigoFormulacion)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Pedido_BuscarLineaFormulacion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigoFormulacion", codigoFormulacion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoLineaFormulacion.DataSource = dt;
            con.Close();
        }

        //BUSCAR MI SEMIPRODUCIDO DE MI FRMULACION
        public void BuscarSemiProducidoFormulacionOP(string codigoFormulacion)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Pedido_BuscarSemiProducidoFormulacionOP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigoFormulacion", codigoFormulacion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoSemiProducidoFormulacion.DataSource = dt;
            con.Close();
        }

        //BUSCAR EL ULTIMO COLOR DE MI PRODUCTO EN UNA OP
        public void BuscarUltimoColorProducto(int idProducto)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Pedido_BuscarUltimoColorProductoOP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idProducto", idProducto);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoBusquedaColorUltimoProducto.DataSource = dt;
            con.Close();
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
        }

        //VERIFICAR SI TODOS LOS ITEMS TIENNE OP
        public void ValidarOPparaPedidos(int IdPedido, int totalItems)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Pedido_BuscarOP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idPedido", IdPedido);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoBusquedaOPporPedido.DataSource = dt;
            con.Close();

            if (datalistadoBusquedaOPporPedido.RowCount == totalItems)
            {
                List<int> estados = new List<int>();

                foreach (DataGridViewRow dgv in datalistadoBusquedaOPporPedido.Rows)
                {
                    estados.Add(Convert.ToInt32(dgv.Cells[2].Value.ToString()));
                }

                if (estados.Contains(4) && estados.Contains(1) || estados.Contains(4) && estados.Contains(2) || estados.Contains(4) && estados.Contains(3))
                {
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    cmd = new SqlCommand("Pedido_CambioEstado", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idPedido", IdPedido);
                    cmd.Parameters.AddWithValue("@estadoPedido", 2);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                else if (estados.All(e => e == 4))
                {
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    cmd = new SqlCommand("Pedido_CambioEstado", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idPedido", IdPedido);
                    cmd.Parameters.AddWithValue("@estadoPedido", 3);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                else
                {
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    cmd = new SqlCommand("Pedido_CambioEstado", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idPedido", IdPedido);
                    cmd.Parameters.AddWithValue("@estadoPedido", 2);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            else
            {
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                cmd = new SqlCommand("Pedido_CambioEstado", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idPedido", IdPedido);
                cmd.Parameters.AddWithValue("@estadoPedido", 1);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        //LISTADO DE OP Y SELECCION DE PDF Y ESTADO DE OP---------------------
        //MOSTRAR OP AL INCIO 
        //FUNCION PARA VISUALIZAR MIS RESULTADOS
        public void MostrarPedidos(DateTime fechaInicio, DateTime fechaTermino, string cliente = null)
        {
            using (SqlConnection con = new SqlConnection(Conexion.ConexionMaestra.conexion))
            using (SqlCommand cmd = new SqlCommand("Pedido_MostrarOT", con))
            {
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                    cmd.Parameters.AddWithValue("@cliente", (object)cliente ?? DBNull.Value);
                    try
                    {
                        con.Open();
                        System.Data.DataTable dt = new System.Data.DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                        datalistadoTodasPedido.DataSource = dt;
                        DataRow[] rowsEnProceso = dt.Select("ESTADO IN ('PENDIENTE', 'FUERA DE FECHA', 'LÍMITE')");
                        // Si hay filas, crea un nuevo DataTable, si no, usa una copia vacía del esquema.
                        System.Data.DataTable dtEnProceso = rowsEnProceso.Any() ? rowsEnProceso.CopyToDataTable() : dt.Clone();
                        datalistadoPendientePedido.DataSource = dtEnProceso; // Asumiendo este es el nombre de tu DataGrid

                        // --- 2.3. OT Observadas (Asumimos que el estado "Observadas" es FUERA DE FECHA o LÍMITE) ---
                        DataRow[] rowIncompletos = dt.Select("ESTADO IN ('INCOMPLETA')");
                        System.Data.DataTable dtIncompletas = rowIncompletos.Any() ? rowIncompletos.CopyToDataTable() : dt.Clone();
                        datalistadoIncompletoPedido.DataSource = dtIncompletas; // Asumiendo este es el nombre de tu DataGrid

                        // --- 2.3. OT Observadas (Asumimos que el estado "Observadas" es FUERA DE FECHA o LÍMITE) ---
                        DataRow[] rowCulminada = dt.Select("ESTADO IN ('CULMINADA')");
                        System.Data.DataTable dtCulminada = rowCulminada.Any() ? rowCulminada.CopyToDataTable() : dt.Clone();
                        datalistadoCompletoPedido.DataSource = dtCulminada; // Asumiendo este es el nombre de tu DataGrid

                        // --- 2.3. OT Observadas (Asumimos que el estado "Observadas" es FUERA DE FECHA o LÍMITE) ---
                        DataRow[] rowDespachda = dt.Select("ESTADO IN ('DESPACHADO')");
                        System.Data.DataTable dtDespachada = rowDespachda.Any() ? rowDespachda.CopyToDataTable() : dt.Clone();
                        datalistadoDespahacoPedido.DataSource = dtDespachada; // Asumiendo este es el nombre de tu DataGrid

                        RedimensionarListadoGeneralPedido(datalistadoTodasPedido);
                        RedimensionarListadoGeneralPedido(datalistadoPendientePedido);
                        RedimensionarListadoGeneralPedido(datalistadoIncompletoPedido);
                        RedimensionarListadoGeneralPedido(datalistadoCompletoPedido);
                        RedimensionarListadoGeneralPedido(datalistadoDespahacoPedido);
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
            DGV.Columns[14].Visible = false;
            DGV.Columns[15].Visible = false;
            ColoresListadoPedidos(DGV);
            ColoresListadoPedidos(DGV);
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO PEDIDOS
        public void ColoresListadoPedidos(DataGridView DGV)
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= DGV.RowCount - 1; i++)
                {
                    ValidarOPparaPedidos(Convert.ToInt32(DGV.Rows[i].Cells[1].Value), Convert.ToInt32(DGV.Rows[i].Cells[9].Value));

                    string estadoPedido = Convert.ToString(DGV.Rows[i].Cells[12].Value);

                    if (estadoPedido == "PENDIENTE")
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else if (estadoPedido == "INCOMPLETA")
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(192, 192, 0);
                    }
                    else if (estadoPedido == "CULMINADA")
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                    }
                    else if (estadoPedido == "DESPACHADO")
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                    }
                    else
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //MOSTRAR PEDIDOS SEGUN LAS FECHAS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarPedidos(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR PEDIDOS SEGUN LAS FECHAS
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarPedidos(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR PEDIDOS SEGUN LAS FECHAS
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarPedidos(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR PEDIDOS SEGUN EL CLIENTE
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            string cliente = null;
            string textoBusqueda = txtBusqueda.Text;

            MostrarPedidos(
                DesdeFecha.Value,
                HastaFecha.Value,
                cliente
            );
        }
        //-------------------------------------------------------------------------------------------------------

        //CREAR ORDEN DE PRODUCCION 
        private void btnOrdenTrabajo_Click(object sender, EventArgs e)
        {
            VisualizarOC = "";

            if (datalistadoTodasPedido.CurrentRow != null)
            {
                DateTime fechaPedido = Convert.ToDateTime(datalistadoTodasPedido.SelectedCells[4].Value);
                string formatoFechaPedido = fechaPedido.ToString("yyyy-MM-dd");

                if (datalistadoTodasPedido.SelectedCells[12].Value.ToString() == "ANULADO")
                {
                    MessageBox.Show("El pedido se encuentra anulado.", "Validación del Sistema");
                }
                else if (Convert.ToDateTime(formatoFechaPedido) < DateTime.Now.Date)
                {
                    MessageBox.Show("Se ha pasado la fecha de vencimiento del pedido.", "Validación del Sistema");
                }
                else
                {
                    LimpiarCamposOrdenProduccion();
                    txtSolicitante.Text = Program.NombreUsuarioCompleto;
                    int idPedido = Convert.ToInt32(datalistadoTodasPedido.SelectedCells[1].Value.ToString());
                    BuscarPedidoPorCodigo(idPedido);
                    BuscarPedidoPorCodigoDetalle(idPedido);
                    CargarSede();
                    CargarPrioridad();
                    CargarLocal();
                    CargarTipoOperacion();
                    cboOperacion.SelectedIndex = 0;
                    dtFechaCreacionOT.Value = DateTime.Now;
                    dtFechaTerminoOT.Value = DateTime.Now;
                    dtpFechaGeneraPedido.Value = DateTime.Now;

                    panelGenerarOT.Visible = true;

                    lblIdCliente.Text = datalistadoPedido.SelectedCells[3].Value.ToString();
                    lblIdUnidad.Text = datalistadoPedido.SelectedCells[7].Value.ToString();
                    lblIdSolicitante.Text = datalistadoPedido.SelectedCells[9].Value.ToString();
                    lblLuharEntrega.Text = datalistadoPedido.SelectedCells[14].Value.ToString();

                    lblCodigoPedido.Text = datalistadoPedido.SelectedCells[1].Value.ToString();
                    lblIdPedido.Text = datalistadoPedido.SelectedCells[0].Value.ToString();
                    dtFechaTerminoOT.Value = Convert.ToDateTime(datalistadoPedido.SelectedCells[11].Value.ToString());

                    txtCliente.Text = datalistadoPedido.SelectedCells[4].Value.ToString();
                    txtUnidad.Text = datalistadoPedido.SelectedCells[8].Value.ToString();
                    txtResponsable.Text = datalistadoPedido.SelectedCells[10].Value.ToString();
                    VisualizarOC = datalistadoPedido.SelectedCells[13].Value.ToString();

                    datalistadoProductos.Rows.Clear();

                    foreach (DataGridViewRow dgv in datalistadoDetallePedido.Rows)
                    {
                        string idDetallePedido = dgv.Cells[0].Value.ToString();
                        string item = dgv.Cells[1].Value.ToString();
                        string descripcionProducto = dgv.Cells[2].Value.ToString();
                        string codigoPedido = dgv.Cells[3].Value.ToString();
                        string medidoProducto = dgv.Cells[4].Value.ToString();
                        string cantidadPedido = dgv.Cells[5].Value.ToString();
                        DateTime fechaEntrega = Convert.ToDateTime(dgv.Cells[6].Value.ToString());
                        string formatoFecha = fechaEntrega.ToString("yyyy-MM-dd");
                        string codigoProducto = dgv.Cells[7].Value.ToString();
                        string codigoBss = dgv.Cells[8].Value.ToString();
                        string codigoCliente = dgv.Cells[9].Value.ToString();
                        string stock = dgv.Cells[10].Value.ToString();
                        string codigoFormulacion = dgv.Cells[11].Value.ToString();
                        string idArt = dgv.Cells[12].Value.ToString();
                        string planoProducto = dgv.Cells[13].Value.ToString();
                        string planoSemiProducido = dgv.Cells[14].Value.ToString();
                        string idPedidoD = dgv.Cells[15].Value.ToString();
                        string totalItems = dgv.Cells[16].Value.ToString();
                        string numeroItem = dgv.Cells[17].Value.ToString();

                        datalistadoProductos.Rows.Add(new[] { null, null, item, descripcionProducto, codigoPedido, medidoProducto, cantidadPedido, cantidadPedido, null, null, stock, formatoFecha, codigoProducto, codigoBss, codigoCliente, codigoFormulacion, idArt, planoProducto, planoSemiProducido, idPedidoD, totalItems, numeroItem, idDetallePedido });
                    }

                    alternarColorFilas(datalistadoProductos);
                    lblCantidadItems.Text = Convert.ToString(datalistadoProductos.RowCount);
                    datalistadoProductos.Columns[2].ReadOnly = true;
                    datalistadoProductos.Columns[3].ReadOnly = true;
                    datalistadoProductos.Columns[4].ReadOnly = true;
                    datalistadoProductos.Columns[5].ReadOnly = true;
                    datalistadoProductos.Columns[5].ReadOnly = true;
                    datalistadoProductos.Columns[7].ReadOnly = true;
                    datalistadoProductos.Columns[8].ReadOnly = true;
                    datalistadoProductos.Columns[9].ReadOnly = true;
                    datalistadoProductos.Columns[10].ReadOnly = true;
                    datalistadoProductos.Columns[11].ReadOnly = true;
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un registro para poder generar una OP.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //FUNCION PARA LIMPIAR TODOS MI CAMPOS DE MI ORDEN DE PRODUCCION
        public void LimpiarCamposOrdenProduccion()
        {
            datalistadoActividades.Rows.Clear();
            datalistadoProductos.Rows.Clear();
            dtpFechaGeneraPedido.Value = DateTime.Now;
            dtFechaCreacionOT.Value = DateTime.Now;
            dtFechaTerminoOT.Value = DateTime.Now;
            txtCliente.Text = "";
            txtUnidad.Text = "";
            txtResponsable.Text = "";
            txtProducto.Text = "";
            txtCodigoBSS.Text = "";
            txtCodigoSistema.Text = "";
            txtCodigoCliente.Text = "";
            txtArea.Text = "";
            txtSolicitante.Text = "";
            txtCodigoFormulacion.Text = "";
            txtColorProducto.Text = "";
            txtObservacionesOT.Text = "";
            lblCantidadItemsMateriales.Text = "***";
            lblCantidadItems.Text = "***";
            cboSede.SelectedItem = 0;
            cboPrioridad.SelectedItem = 0;
            cboLocal.SelectedItem = 0;
            cboOperacion.SelectedItem = 0;
        }

        //LIMPIAR AMPOS DE LA ORDEN DE PRIDCCUIN (NO TODOS LOS DATOS)
        public void LimpiarCamposOrdenProduccionInconpleto()
        {
            datalistadoActividades.Rows.Clear();
            datalistadoProductos.Rows.Clear();
            dtpFechaGeneraPedido.Value = DateTime.Now;
            dtFechaCreacionOT.Value = DateTime.Now;
            dtFechaTerminoOT.Value = DateTime.Now;
            txtProducto.Text = "";
            txtCodigoBSS.Text = "";
            txtCodigoSistema.Text = "";
            txtCodigoCliente.Text = "";
            txtArea.Text = "";
            txtCodigoFormulacion.Text = "";
            txtColorProducto.Text = "";
            txtObservacionesOT.Text = "";
            lblCantidadItemsMateriales.Text = "***";
            lblCantidadItems.Text = "***";
            cboSede.SelectedIndex = 0;
            cboPrioridad.SelectedIndex = 0;
            cboLocal.SelectedIndex = 0;
            cboOperacion.SelectedIndex = 0;
        }

        //SELECCIONAR UN PRODUCTO DE MI LISTADO
        private void datalistadoProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoProductos.CurrentRow != null)
            {
                // Verifica que la celda seleccionada sea válida y que no sea el encabezado
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    // Obtener la celda en la columna 4 (índice 3, porque comienza en 0)
                    DataGridViewCell cell = datalistadoProductos.Rows[e.RowIndex].Cells[6];

                    // Validar si la celda está vacía o es cero
                    if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value.ToString()) || cell.Value.ToString() == "0")
                    {
                        cell.Value = 0; // Asigna el valor predeterminado, por ejemplo, 1
                    }
                }

                // Verifica que la celda seleccionada sea válida y que no sea el encabezado
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    // Obtener la celda en la columna 4 (índice 3, porque comienza en 0)
                    DataGridViewCell cell = datalistadoProductos.Rows[e.RowIndex].Cells[8];

                    // Validar si la celda está vacía o es cero
                    if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value.ToString()) || cell.Value.ToString() == "0")
                    {
                        cell.Value = 0; // Asigna el valor predeterminado, por ejemplo, 1
                    }
                }

                // Verifica que la celda seleccionada sea válida y que no sea el encabezado
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    // Obtener la celda en la columna 4 (índice 3, porque comienza en 0)
                    DataGridViewCell cell = datalistadoProductos.Rows[e.RowIndex].Cells[9];

                    // Validar si la celda está vacía o es cero
                    if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value.ToString()) || cell.Value.ToString() == "0")
                    {
                        cell.Value = 0; // Asigna el valor predeterminado, por ejemplo, 1
                    }
                }

                //ABIRIR PLANOS
                DataGridViewColumn currentColumn = datalistadoProductos.Columns[e.ColumnIndex];

                //SI SE PRECIONA SOBRE LA COLUMNA CON EL NOMBRE SELECCIOANDO
                if (currentColumn.Name == "pl1")
                {
                    //SI NO HAY UN REGISTRO SELECCIONADO
                    if (datalistadoProductos.CurrentRow != null)
                    {
                        //CAPTURAR EL PLANO DE MI PRODUCTO
                        string planoProducto = datalistadoProductos.SelectedCells[17].Value.ToString();
                        try
                        {
                            Process.Start(planoProducto);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("No se ha podido encontrar el archivo o plano, por favor cargar el plano o seleccionarlo al momento de crear la formulación." + ex.Message, "Validación del Sistema", MessageBoxButtons.OK);
                        }
                    }
                }

                //ABIRIR PLANOS
                DataGridViewColumn currentColumn2 = datalistadoProductos.Columns[e.ColumnIndex];

                //SI SE PRECIONA SOBRE LA COLUMNA CON EL NOMBRE SELECCIOANDO
                if (currentColumn2.Name == "pl2")
                {
                    //SI NO HAY UN REGISTRO SELECCIONADO
                    if (datalistadoProductos.CurrentRow != null)
                    {
                        //CAPTURAR EL PLANO DE MI PRODUCTO
                        string planoSemiProducido = datalistadoProductos.SelectedCells[18].Value.ToString();
                        try
                        {
                            Process.Start(planoSemiProducido);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("No se ha podido encontrar el archivo o plano, por favor cargar el plano o seleccionarlo al momento de crear la formulación." + ex.Message, "Validación del Sistema", MessageBoxButtons.OK);
                        }
                    }
                }

                txtProducto.Text = datalistadoProductos.SelectedCells[3].Value.ToString();
                lblIdProducto.Text = datalistadoProductos.SelectedCells[16].Value.ToString();
                txtCodigoBSS.Text = datalistadoProductos.SelectedCells[13].Value.ToString();
                txtCodigoSistema.Text = datalistadoProductos.SelectedCells[12].Value.ToString();
                txtCodigoCliente.Text = datalistadoProductos.SelectedCells[14].Value.ToString();
                string codigoFormulacion = datalistadoProductos.SelectedCells[15].Value.ToString();
                dtFechaTerminoOT.Value = Convert.ToDateTime(datalistadoProductos.SelectedCells[11].Value.ToString());
                txtCodigoFormulacion.Text = codigoFormulacion;
                int numeroProducir = Convert.ToInt32(datalistadoProductos.SelectedCells[6].Value.ToString());

                BuscarRelacionProductoSemi(codigoFormulacion);

                BuscarMaterialesFormulacion(codigoFormulacion);
                BuscarMaterialesFormulacionSemi(codigoFormulacion);
                BuscarLineaFormulacion(codigoFormulacion);

                BuscarSemiProducidoFormulacionOP(codigoFormulacion);

                txtColorProducto.Text = "";

                if (lblIdProducto.Text == "---")
                {
                    BuscarUltimoColorProducto(0);
                }
                else
                {
                    BuscarUltimoColorProducto(Convert.ToInt32(lblIdProducto.Text));

                    if (datalistadoBusquedaColorUltimoProducto.Rows.Count > 0)
                    {
                        txtColorProducto.Text = datalistadoBusquedaColorUltimoProducto.SelectedCells[0].Value.ToString();
                    }
                }

                txtIdArea.Text = datalistadoLineaFormulacion.SelectedCells[2].Value.ToString();
                txtArea.Text = datalistadoLineaFormulacion.SelectedCells[1].Value.ToString();
                datalistadoActividades.Rows.Clear();
                datalistadoActividadesSemi.Rows.Clear();

                //CARGAR MATERIALES DE MI PRODUCTO
                int contador = 1;
                foreach (DataGridViewRow dgv in datalistadoDetallesMaterialesFormulacion.Rows)
                {
                    string idMaterialDetalleActividad = dgv.Cells[0].Value.ToString();
                    string idProducto = dgv.Cells[1].Value.ToString();
                    string codigoBSS = dgv.Cells[2].Value.ToString();
                    string codigoSistema = dgv.Cells[3].Value.ToString();
                    string descripcionProducto = dgv.Cells[4].Value.ToString();
                    string cantidad = dgv.Cells[5].Value.ToString();
                    string medida = dgv.Cells[6].Value.ToString();
                    string idFormulacion = dgv.Cells[7].Value.ToString();
                    string stock = dgv.Cells[8].Value.ToString();

                    decimal totalProductas = Convert.ToDecimal(cantidad) * numeroProducir;

                    datalistadoActividades.Rows.Add(new[] { Convert.ToString(contador), idMaterialDetalleActividad, idProducto, codigoBSS, codigoSistema, descripcionProducto, cantidad, Convert.ToString(totalProductas), medida, stock });
                    contador = contador + 1;
                }

                //CARGAR MATERIALES DE MI SEMIPRODUCIDO
                int contador2 = 1;
                foreach (DataGridViewRow dgv in datalistadoDetallesMaterialesFormulacionSemi.Rows)
                {
                    string idMaterialDetalleActividad = dgv.Cells[0].Value.ToString();
                    string idProducto = dgv.Cells[1].Value.ToString();
                    string codigoBSS = dgv.Cells[2].Value.ToString();
                    string codigoSistema = dgv.Cells[3].Value.ToString();
                    string descripcionProducto = dgv.Cells[4].Value.ToString();
                    string cantidad = dgv.Cells[5].Value.ToString();
                    string medida = dgv.Cells[6].Value.ToString();
                    string idFormulacion = dgv.Cells[7].Value.ToString();
                    string stock = dgv.Cells[8].Value.ToString();
                    decimal totalProductas = 0;

                    int relacionFormulacion = Convert.ToInt16(datalistadoBuscarRelacionFormulacion.SelectedCells[0].Value);
                    totalProductas = Convert.ToDecimal(cantidad) * numeroProducir * relacionFormulacion;

                    datalistadoActividadesSemi.Rows.Add(new[] { Convert.ToString(contador), idMaterialDetalleActividad, idProducto, codigoBSS, codigoSistema, descripcionProducto, cantidad, Convert.ToString(totalProductas), medida, stock });
                    contador2 = contador2 + 1;
                }

                lblCantidadMaterialesItemsSemi.Text = Convert.ToString(datalistadoActividadesSemi.RowCount);
                lblCantidadItemsMateriales.Text = Convert.ToString(datalistadoActividades.RowCount);
                alternarColorFilas(datalistadoActividades);
            }
        }

        //BOTON PARA GUARDAR MI OP Y REQUERIMEINTOP
        private void btnGuardarOT_Click(object sender, EventArgs e)
        {
            //VALIDACIÓN DE CANTIDADES
            decimal? cantidadProduccion = Convert.ToDecimal(datalistadoProductos.SelectedCells[6].Value);
            decimal? cantidadPedido = Convert.ToDecimal(datalistadoProductos.SelectedCells[7].Value);

            if (cantidadProduccion > cantidadPedido)
            {
                MessageBox.Show("No se puede mandar a producir más de la cantidad pedida.", "Validación de Sistema");
                return;
            }
            else if (cantidadProduccion == 0 || cantidadProduccion == null)
            {
                MessageBox.Show("Debe ingresar una cantidad a producir.", "Validación de Sistema");
                return;
            }

            if (datalistadoProductos.RowCount == 0)
            {
                MessageBox.Show("No hay productos para fabribar, por favor validar esta parte.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                if (txtColorProducto.Text == "")
                {
                    MessageBox.Show("Debe ingresar un color para el producto a fabricar.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    DialogResult boton = MessageBox.Show("¿Realmente desea generar esta orden de trabajo?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                    if (boton == DialogResult.OK)
                    {
                        //INGRESAR MI ORDEN DE PRODCCUIN-------------------------------------------------
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        SqlCommand cmd = new SqlCommand();
                        //INGRESAR MI OT PARA MI ORDEN DE PRODUCCION-----------------------------
                        SqlConnection con3 = new SqlConnection();
                        con3.ConnectionString = Conexion.ConexionMaestra.conexion;
                        SqlCommand cmd3 = new SqlCommand();
                        con3.Open();
                        cmd3 = new SqlCommand("OT_InsertarOrdenTrabajo", con3);
                        cmd3.CommandType = CommandType.StoredProcedure;

                        //INGRESAR LOS DATOS GENERALES DE OT
                        cmd3.Parameters.AddWithValue("@fechaInicial", dtFechaCreacionOT.Value);
                        cmd3.Parameters.AddWithValue("@fechaEntrega", dtFechaTerminoOT.Value);
                        cmd3.Parameters.AddWithValue("@idArt", lblIdProducto.Text);
                        cmd3.Parameters.AddWithValue("@codigoProducto", txtCodigoSistema.Text);
                        cmd3.Parameters.AddWithValue("@descripcionProducto", txtProducto.Text);
                        cmd3.Parameters.AddWithValue("@planoProducto", datalistadoProductos.SelectedCells[17].Value.ToString());
                        cmd3.Parameters.AddWithValue("@color", txtColorProducto.Text);
                        cmd3.Parameters.AddWithValue("@codigoBSS", txtCodigoBSS.Text);
                        cmd3.Parameters.AddWithValue("@idGeneraUsuario", DBNull.Value);
                        cmd3.Parameters.AddWithValue("@usuarioGenera", txtSolicitante.Text);
                        cmd3.Parameters.AddWithValue("@idSede", cboSede.SelectedValue.ToString());
                        cmd3.Parameters.AddWithValue("@idPrioridad", cboPrioridad.SelectedValue.ToString());
                        cmd3.Parameters.AddWithValue("@idLocal", cboLocal.SelectedValue.ToString());
                        cmd3.Parameters.AddWithValue("@idOperacion", cboOperacion.SelectedValue.ToString());
                        cmd3.Parameters.AddWithValue("@observacion", txtObservacionesOT.Text);


                        cmd3.Parameters.AddWithValue("@cantidad", datalistadoProductos.SelectedCells[6].Value.ToString());
                        cmd3.Parameters.AddWithValue("@idCliente", Convert.ToInt32(lblIdCliente.Text));
                        cmd3.Parameters.AddWithValue("@codigoFormulacion", txtCodigoFormulacion.Text);
                        cmd3.Parameters.AddWithValue("@idDetallePedido", datalistadoProductos.SelectedCells[22].Value.ToString());
                        cmd3.ExecuteNonQuery();
                        con3.Close();

                        //INGRESAR MI REQUERIMIENTO PARA MI ORDEN DE PRODUCCION-----------------------------
                        SqlConnection con4 = new SqlConnection();
                        con4.ConnectionString = Conexion.ConexionMaestra.conexion;
                        SqlCommand cmd4 = new SqlCommand();
                        con4.Open();
                        cmd4 = new SqlCommand("OP_InsertarRequerimientoSimpleOT", con4);

                        cmd4.CommandType = CommandType.StoredProcedure;
                        //INGRESAR LOS DATOS GENERALES DE MI REQUERIMIENTO
                        cmd4.Parameters.AddWithValue("@fechaRequerida", DateTime.Now);
                        cmd4.Parameters.AddWithValue("@fechaSolicitada", DateTime.Now);
                        cmd4.Parameters.AddWithValue("@desJefatura", "LUIS CLEMENTE");
                        cmd4.Parameters.AddWithValue("@idSolicitante", 1052);
                        cmd4.Parameters.AddWithValue("@idCentroCostos", 8);
                        cmd4.Parameters.AddWithValue("@observaciones", "REQUERIMIENTO PARA ORDEN DE SERVICIO");
                        cmd4.Parameters.AddWithValue("@idSede", 1);
                        cmd4.Parameters.AddWithValue("@idLocal", 1);
                        cmd4.Parameters.AddWithValue("@idArea", 13);
                        cmd4.Parameters.AddWithValue("@idipo", 2);
                        cmd4.Parameters.AddWithValue("@estadoLogistica", 1);
                        cmd4.Parameters.AddWithValue("@mensajeAnulacion", "");
                        cmd4.Parameters.AddWithValue("@idJefatura", 1052);
                        cmd4.Parameters.AddWithValue("@aliasCargaJefatura", "Jefe de Ingeniería");
                        cmd4.Parameters.AddWithValue("@cantidadItems", Convert.ToInt32(lblCantidadItemsMateriales.Text));
                        cmd4.Parameters.AddWithValue("@idPrioridad", 1);
                        cmd4.ExecuteNonQuery();
                        con4.Close();

                        //VARIABLE PARA CONTAR LA CANTIDAD DE ITEMS QUE HAY
                        int contadorOT = 1;
                        //INGRESO DE LOS DETALLES DEL REQUERIMIENTO SIMPLE CON UN FOREACH
                        foreach (DataGridViewRow row in datalistadoActividades.Rows)
                        {
                            decimal cantidad = Convert.ToDecimal(row.Cells["cantidad"].Value);

                            //PROCEDIMIENTO ALMACENADO PARA GUARDAR LOS PRODUCTOS
                            con.Open();
                            cmd = new SqlCommand("OP_InsertarRequerimientoSimpleDetalleProductos", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@item", contadorOT);
                            cmd.Parameters.AddWithValue("@idArt", Convert.ToString(row.Cells[2].Value));
                            //SI NO HAN PUESTO UN VALOR AL PRODUCTO
                            if (cantidad == 0)
                            {
                                cmd.Parameters.AddWithValue("@cantidad", 1.000);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@cantidad", cantidad);
                            }

                            cmd.Parameters.AddWithValue("@stock", Convert.ToString(row.Cells[9].Value));
                            cmd.Parameters.AddWithValue("@cantidadTotal", Convert.ToString(row.Cells[7].Value));
                            cmd.ExecuteNonQuery();
                            con.Close();

                            //AUMENTAR
                            contadorOT++;
                            //}
                        }

                        //MENSAJE DE CONFORMIAD CON EL INGRESO DE LA ORDEN DE SERVICIO
                        MessageBox.Show("Se generó la Orden de servicio correctamente.", "Validación del Sistema");

                        LimpiarCamposOrdenProduccionInconpleto();

                        int idPedido = Convert.ToInt32(lblIdPedido.Text);
                        //RECARGA DE DATOS PARA TRAER LA NUEVA LISTA CON LOS NUEVOS DATOS
                        BuscarPedidoPorCodigo(idPedido);
                        BuscarPedidoPorCodigoDetalle(idPedido);
                        //RELLENAR MI LISTADO DE PRODUCTOS CON LA NUEVA LISTA
                        foreach (DataGridViewRow dgv in datalistadoDetallePedido.Rows)
                        {
                            string idDetallePedido = dgv.Cells[0].Value.ToString();
                            string item = dgv.Cells[1].Value.ToString();
                            string descripcionProducto = dgv.Cells[2].Value.ToString();
                            string codigoPedido = dgv.Cells[3].Value.ToString();
                            string medidoProducto = dgv.Cells[4].Value.ToString();
                            string cantidadPedidop = dgv.Cells[5].Value.ToString();
                            DateTime fechaEntrega = Convert.ToDateTime(dgv.Cells[6].Value.ToString());
                            string formatoFecha = fechaEntrega.ToString("yyyy-MM-dd");
                            string codigoProducto = dgv.Cells[7].Value.ToString();
                            string codigoBss = dgv.Cells[8].Value.ToString();
                            string codigoCliente = dgv.Cells[9].Value.ToString();
                            string stock = dgv.Cells[10].Value.ToString();
                            string codigoFormulacion = dgv.Cells[11].Value.ToString();
                            string idArt = dgv.Cells[12].Value.ToString();
                            string planoProducto = dgv.Cells[13].Value.ToString();
                            string planoSemiProducido = dgv.Cells[14].Value.ToString();
                            string idPedidoD = dgv.Cells[15].Value.ToString();
                            string totalItems = dgv.Cells[16].Value.ToString();
                            string numeroItem = dgv.Cells[17].Value.ToString();

                            datalistadoProductos.Rows.Add(new[] { null, null, item, descripcionProducto, codigoPedido, medidoProducto, null, cantidadPedidop, null, null, stock, formatoFecha, codigoProducto, codigoBss, codigoCliente, codigoFormulacion, idArt, planoProducto, planoSemiProducido, idPedidoD, totalItems, numeroItem, idDetallePedido });
                        }

                        alternarColorFilas(datalistadoProductos);
                        lblCantidadItems.Text = Convert.ToString(datalistadoProductos.RowCount);
                        datalistadoProductos.Columns[2].ReadOnly = true;
                        datalistadoProductos.Columns[3].ReadOnly = true;
                        datalistadoProductos.Columns[4].ReadOnly = true;
                        datalistadoProductos.Columns[5].ReadOnly = true;
                        datalistadoProductos.Columns[5].ReadOnly = true;
                        datalistadoProductos.Columns[7].ReadOnly = true;
                        datalistadoProductos.Columns[8].ReadOnly = true;
                        datalistadoProductos.Columns[9].ReadOnly = true;
                        datalistadoProductos.Columns[10].ReadOnly = true;
                        datalistadoProductos.Columns[11].ReadOnly = true;
                    }
                }
            }
        }

        //BOTON PARA SALÑIR DE MI CREACION DE OP
        private void btnSalirOT_Click(object sender, EventArgs e)
        {
            panelGenerarOT.Visible = false;
        }
    }
}

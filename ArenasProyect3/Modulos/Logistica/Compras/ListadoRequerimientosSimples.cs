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

namespace ArenasProyect3.Modulos.Logistica.Compras
{
    public partial class ListadoRequerimientosSimples : Form
    {
        //VARIABLES GENERALES
        private Cursor curAnterior = null;
        string area = "";
        string cantidadOrdenesCompra = "";
        string cantidadOrdenesCompra2 = "";
        string codigoOrdenCOmpra = "";

        //CONSTRUCTOR DE MI MANTENIMIENTO
        public ListadoRequerimientosSimples()
        {
            InitializeComponent();
        }

        //CARGA INICIAL DEL MANTENIMEINTO
        private void ListadoRequerimientosSimples_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            datalistadoRequerimiento.DataSource = null;

            //PREFILES Y PERSIMOS---------------------------------------------------------------
            if (Program.RangoEfecto != 1)
            {

            }
            //---------------------------------------------------------------------------------
        }

        //CARGA DE METODOS - GENERAL----------------------------------------------------------------------------------
        //CARGA DE DATOS DEL USUARIO QUE INICIO SESIÓN
        //BUSQUEDA DE USUARIO
        public void DatosUsuario(int idUsuario)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarUsuarioPorCodigo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idusuario", idUsuario);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoDatosUsuario.DataSource = dt;
            con.Close();

            area = datalistadoDatosUsuario.SelectedCells[7].Value.ToString();
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
            datalistadoDatosJefatura.DataSource = dt;
            con.Close();

            txtAutorizadoPor.Text = datalistadoDatosJefatura.SelectedCells[1].Value.ToString() + " " + datalistadoDatosJefatura.SelectedCells[2].Value.ToString();
        }

        //FUNCION PARA RECONOCER LA JEFATURA
        public void ReconocerAreaJefatura(string area)
        {
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
        }

        //VER DETALLES (ITEMS) DE MI REQUERIMIENTO SIMPLE
        public void BuscarDetallesRequerimiento(DataGridView DGV, int codigoRequerimientoSimple)
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
            DGV.DataSource = dt;
            con.Close();
        }

        //CARGA DEL TIPO DEORDEN DE COMPRA
        public void CargarTipoOrdenCompra()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdTipoOrdenCompra, Descripcion FROM TipoOrdenCompra WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboTipoOC.DisplayMember = "Descripcion";
            cboTipoOC.ValueMember = "IdTipoOrdenCompra";
            cboTipoOC.DataSource = dt;
        }

        //CARGA FORMA DE PAGO
        public void CargarTipoFormaPago()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdFormaPago, Descripcion FROM FormaPago WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboFormaPago.DisplayMember = "Descripcion";
            cboFormaPago.ValueMember = "IdFormaPago";
            cboFormaPago.DataSource = dt;
        }

        //CARGA CENTRO DE COSTOS
        public void CargarCentroCostos()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdCentroCostos, Descripcion FROM CentroCostos WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboCentreoCostos.DisplayMember = "Descripcion";
            cboCentreoCostos.ValueMember = "IdCentroCostos";
            cboCentreoCostos.DataSource = dt;
        }

        //CARGA TIPO DE BANCO
        public void CargarTiposBancos(int idProveedor)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT DAPCB.IdDatosAnexosProveedorCuentaBancaria,B.Descripcion, DAPCB.NumeroCUenta FROM DatosAnexosProveedor_CuentasBancarias DAPCB INNER JOIN Bancos B ON B.IdBanco = DAPCB.IdBanco WHERE DAPCB.Estado = 1 AND IdProveedor = @idProveedor", con);
            comando.Parameters.AddWithValue("@idProveedor", idProveedor);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboTipoVanco.DisplayMember = "Descripcion";
            cboTipoVanco.ValueMember = "IdDatosAnexosProveedorCuentaBancaria";
            DataRow row = dt.Rows[0];
            cboNumeroCuenta.Text = System.Convert.ToString(row["NumeroCUenta"]);
            cboTipoVanco.DataSource = dt;
        }


        //-----------------------------------------------------------------------------------------------

        //LISTADO DE REQUERIMEINTOS SIMPLES---------------------
        //MOSTRAR REQUERIMIENTOS POR FECHA 
        public void MostrarRequerimientoPorFecha(DateTime fechaInicio, DateTime fechaTermino)
        {
            if (lblCarga.Text == "0")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarRequerimientoSimplePorFecha2_Jefatura", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoRequerimiento.DataSource = dt;
                con.Close();
                ReimenisonarListado(datalistadoRequerimiento);
            }
            else
            {
                lblCarga.Text = "0";
            }

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in datalistadoRequerimiento.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //MOSTRAR REQUERIMIENTOS POR NUMERO
        public void MostrarRequerimientoPorNumero(DateTime fechaInicio, DateTime fechaTermino, string numeroRequerimiento)
        {
            if (lblCarga.Text == "0")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarRequerimientoSimplePorCodigo2_Jefatura", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                cmd.Parameters.AddWithValue("@codigo", numeroRequerimiento);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoRequerimiento.DataSource = dt;
                con.Close();
                ReimenisonarListado(datalistadoRequerimiento);
            }
            else
            {
                lblCarga.Text = "0";
            }

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in datalistadoRequerimiento.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //MOSTRAR REQUERIMIENTOS POR AREA
        public void MostrarRequerimientoPorArea(string area, DateTime fechaInicio, DateTime fechaTermino)
        {
            if (lblCarga.Text == "0")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarRequerimientoSimplePorArea2_Jefatura", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                cmd.Parameters.AddWithValue("@area", area);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoRequerimiento.DataSource = dt;
                con.Close();
                ReimenisonarListado(datalistadoRequerimiento);
            }
            else
            {
                lblCarga.Text = "0";
            }

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in datalistadoRequerimiento.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //MOSTRAR REQUERIMIENTOS POR SOLICITANTE
        public void MostrarRequerimientoPorSolicitante(string solicitante, DateTime fechaInicio, DateTime fechaTermino)
        {
            if (lblCarga.Text == "0")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarRequerimientoSimplePorSolicitante2_Jefatura", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                cmd.Parameters.AddWithValue("@solicitante", solicitante);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoRequerimiento.DataSource = dt;
                con.Close();
                ReimenisonarListado(datalistadoRequerimiento);
            }
            else
            {
                lblCarga.Text = "0";
            }

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in datalistadoRequerimiento.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //VER DETALLES(ITEMS) DE MI REQUERIMIENTO SIMPLE VALIDACION
        public void CargarDetallesVerificacion()
        {
            try
            {
                for (var i = 0; i <= datalistadoRequerimiento.RowCount - 1; i++)
                {
                    int idRequerimeinto = Convert.ToInt32(datalistadoRequerimiento.Rows[i].Cells[0].Value.ToString());

                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("ListaRequerimientoGeneralLogistica_SP", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idRequerimeinto", idRequerimeinto);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoDetallesRequerimiento.DataSource = dt;
                    con.Close();
                    //CARGAR METODO PARA COLOREAR
                    ColoresListado();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
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
                datalistadoDetallesRequerimientoD.DataSource = dt;
                con.Close();
                //NO MOSTRAR LAS COLUMNAS QUE NO SEAN DE REELEVANCIA PARA EL USUARIO
                datalistadoDetallesRequerimientoD.Columns[1].Visible = false;
                datalistadoDetallesRequerimientoD.Columns[8].Visible = false;
                //REDIMENSIONAR LAS COLUMNAS SEGUN EL TEMAÑO REQUERIDO
                datalistadoDetallesRequerimientoD.Columns[0].Width = 70;
                datalistadoDetallesRequerimientoD.Columns[2].Width = 100;
                datalistadoDetallesRequerimientoD.Columns[3].Width = 250;
                datalistadoDetallesRequerimientoD.Columns[4].Width = 100;
                datalistadoDetallesRequerimientoD.Columns[5].Width = 90;
                datalistadoDetallesRequerimientoD.Columns[6].Width = 90;
                datalistadoDetallesRequerimientoD.Columns[7].Width = 90;
                datalistadoDetallesRequerimientoD.Columns[9].Width = 110;
                //CARGAR METODO PARA COLOREAR
                ColoresListadoItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //COLOREAR REGISTROS
        public void ColoresListado()
        {
            try
            {
                for (var i = 0; i <= datalistadoRequerimiento.RowCount - 1; i++)
                {
                    //COLORES DE REQUERIMEINTOS
                    if (datalistadoRequerimiento.Rows[i].Cells[12].Value.ToString() == "POR ATENDER")
                    {
                        //POR ATENDER -> 1
                        datalistadoRequerimiento.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else if (datalistadoRequerimiento.Rows[i].Cells[12].Value.ToString() == "EVALUADO")
                    {
                        //EVALUADO -> 2
                        datalistadoRequerimiento.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                    }
                    else if (datalistadoRequerimiento.Rows[i].Cells[12].Value.ToString() == "OC EN CURSO")
                    {
                        //OC EN CURSO -> 3
                        datalistadoRequerimiento.Rows[i].DefaultCellStyle.ForeColor = Color.Orange;
                    }
                    else if (datalistadoRequerimiento.Rows[i].Cells[12].Value.ToString() == "OC CULMINADA")
                    {
                        //OC TERMINADA -> 4
                        datalistadoRequerimiento.Rows[i].DefaultCellStyle.ForeColor = Color.Teal;
                    }
                    else if (datalistadoRequerimiento.Rows[i].Cells[12].Value.ToString() == "ATENCION PARCIAL")
                    {
                        //ATENDIDO TOTAL -> 5
                        datalistadoRequerimiento.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(192, 192, 0);
                    }
                    else if (datalistadoRequerimiento.Rows[i].Cells[12].Value.ToString() == "ATENCION TOTAL")
                    {
                        //ATENDIDO TOTAL -> 6
                        datalistadoRequerimiento.Rows[i].DefaultCellStyle.ForeColor = Color.ForestGreen;
                    }
                    else if (datalistadoRequerimiento.Rows[i].Cells[12].Value.ToString() == "ANULADO")
                    {
                        //ATENDIDO TOTAL -> 0
                        datalistadoRequerimiento.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    }
                    else
                    {
                        //SI NO HAY NINGUN CASO
                        datalistadoRequerimiento.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
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
                for (var i = 0; i <= datalistadoDetallesRequerimientoD.RowCount - 1; i++)
                {
                    decimal cantidadTotal = 0;
                    cantidadTotal = Convert.ToDecimal(datalistadoDetallesRequerimientoD.Rows[i].Cells[5].Value.ToString());
                    decimal cantidadRetirada = 0;
                    cantidadRetirada = Convert.ToDecimal(datalistadoDetallesRequerimientoD.Rows[i].Cells[6].Value.ToString());
                    decimal resultadoRestante = 0;

                    resultadoRestante = cantidadTotal - cantidadRetirada;

                    if (resultadoRestante > Convert.ToDecimal(datalistadoDetallesRequerimientoD.Rows[i].Cells[7].Value.ToString()))
                    {
                        //PRODUCTOS SIN STOCK
                        datalistadoDetallesRequerimientoD.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                    }
                    if (resultadoRestante < Convert.ToDecimal(datalistadoDetallesRequerimientoD.Rows[i].Cells[7].Value.ToString()))
                    {
                        //PRODUCTOS CON STOCK
                        datalistadoDetallesRequerimientoD.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                    }
                    if (datalistadoDetallesRequerimientoD.Rows[i].Cells[9].Value.ToString() == "ENTREGADO")
                    {
                        //PRODUCTOS ENTREGADO
                        datalistadoDetallesRequerimientoD.Rows[i].DefaultCellStyle.ForeColor = Color.ForestGreen;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //FUNCION ARA AJUSTAR MIS COLUMNAS
        public void ReimenisonarListado(DataGridView DGV)
        {
            //NO MOSTRAR LAS COLUMNAS QUE NO SEAN DE REELEVANCIA PARA EL USUARIO
            DGV.Columns[1].Visible = false;
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
            DGV.Columns[2].Width = 35;
            DGV.Columns[3].Width = 110;
            DGV.Columns[4].Width = 100;
            DGV.Columns[5].Width = 100;
            DGV.Columns[6].Width = 220;
            DGV.Columns[8].Width = 220;
            DGV.Columns[10].Width = 130;
            DGV.Columns[12].Width = 110;
            DGV.Columns[13].Width = 100;
            //DEFINICIÓND DE SOLO LECTURA DE MI LISTADO DE PRODUCTOS
            DGV.Columns[3].ReadOnly = true;
            DGV.Columns[4].ReadOnly = true;
            DGV.Columns[5].ReadOnly = true;
            DGV.Columns[6].ReadOnly = true;
            DGV.Columns[8].ReadOnly = true;
            DGV.Columns[10].ReadOnly = true;
            DGV.Columns[12].ReadOnly = true;
            DGV.Columns[13].ReadOnly = true;
            //CARGAR METODO PARA VERIFICAR LOS DETALLES
            //CargarDetallesVerificacion();
        }

        //MOSTRAR TODOS LOS REQUERMIEBNTOS SEGÚN LA DFECHA
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarRequerimientoPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR TODOS LOS REQUERMIEBNTOS SEGÚN LA DFECHA
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarRequerimientoPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR TODOS LOS REQUERMIEBNTOS SEGÚN LA DFECHA
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarRequerimientoPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR TODOS LOS REQUERMIEBNTOS SEGÚN EL CODIGO DE REQUERIMIENTO
        private void txtBusquedaNumeroResquerimiento_TextChanged(object sender, EventArgs e)
        {
            MostrarRequerimientoPorNumero(DesdeFecha.Value, HastaFecha.Value, txtBusquedaNumeroResquerimiento.Text);
        }

        //MOSTRAR TODOS LOS REQUERMIEBNTOS SEGÚN EL ÁREA
        private void txtBusquedaArea_TextChanged(object sender, EventArgs e)
        {
            MostrarRequerimientoPorArea(txtBusquedaArea.Text, DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR TODOS LOS REQUERMIEBNTOS SEGÚN EL SOLICITANTE
        private void txtBusquedaSolicitante_TextChanged(object sender, EventArgs e)
        {
            MostrarRequerimientoPorSolicitante(txtBusquedaSolicitante.Text, DesdeFecha.Value, HastaFecha.Value);
        }

        //SELECCIONAR LOS DETALLES DE MI REQUERIMIENT
        private void datalistadoRequerimiento_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoRequerimiento.Columns[e.ColumnIndex];

            //SI NO HAY UN REGISTRO SELECCIONADO
            if (datalistadoRequerimiento.CurrentRow != null)
            {
                //CAPTURAR EL CÓDIFO DE MI REQUERIMIENTO SIMPLE
                int idRequerimeinto = Convert.ToInt32(datalistadoRequerimiento.SelectedCells[1].Value.ToString());
                //VER EL PANEL DE LOS DETALLES DEL REQUERIMIENTO
                panelDetallesRequerimiento.Visible = true;
                txtCodigoRequerimiento.Text = datalistadoRequerimiento.SelectedCells[3].Value.ToString();
                txtCantidadItems.Text = datalistadoRequerimiento.SelectedCells[14].Value.ToString();
                //MOSTRAR LOS ITEMS DEL REQUERIMIENTO SIMPLE
                CargarDetallesItems(idRequerimeinto);
            }

            datalistadoRequerimiento.Enabled = false;
        }

        //OCULTAR EL PANEL DE LOS DETALLES DEL REQUERIMIENTO
        private void btnSalirDetallesRequerimiento_Click(object sender, EventArgs e)
        {
            //OCULTAR EL PANEL DE LOS DETALLES DEL REQUERIMIENTO
            panelDetallesRequerimiento.Visible = false;
            datalistadoRequerimiento.Enabled = true;
        }

        //OCULTAR EL PANEL DE LOS DETALLES DEL REQUERIMIENTO
        private void lblRetrocederDetalleRequerimiento_Click(object sender, EventArgs e)
        {
            //OCULTAR EL PANEL DE LOS DETALLES DEL REQUERIMIENTO
            panelDetallesRequerimiento.Visible = false;
            datalistadoRequerimiento.Enabled = true;
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN DE GENERACIÓN DEL PDF
        private void datalistadoRequerimiento_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoRequerimiento.Columns[e.ColumnIndex].Name == "btnGenerarPdf")
            {
                this.datalistadoRequerimiento.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoRequerimiento.Cursor = curAnterior;
            }
        }
    }
}

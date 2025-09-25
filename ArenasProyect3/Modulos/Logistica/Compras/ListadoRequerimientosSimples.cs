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
using System.Drawing;
using System.IO;
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

        //METODO PARA PINTAR DE COLORES LAS FILAS DE MI LSITADO
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
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
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

        //CONTAR LA CANTIDAD DE REQUERIMIENTOS QUE HAY EN MI TABLA
        public void ConteoOrdenesCompra()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            da = new SqlDataAdapter("SELECT IdOrdenCompra FROM OrdenCompra WHERE IdOrdenCompra = (SELECT MAX(IdOrdenCompra) FROM OrdenCompra)", con);
            da.Fill(dt);
            datalistadoCargarCantidadOC.DataSource = dt;
            con.Close();

            if (datalistadoCargarCantidadOC.RowCount > 0)
            {
                cantidadOrdenesCompra = datalistadoCargarCantidadOC.SelectedCells[0].Value.ToString();

                if (cantidadOrdenesCompra.Length == 1)
                {
                    cantidadOrdenesCompra2 = "000000" + cantidadOrdenesCompra;
                }
                else if (cantidadOrdenesCompra.Length == 2)
                {
                    cantidadOrdenesCompra2 = "00000" + cantidadOrdenesCompra;
                }
                else if (cantidadOrdenesCompra.Length == 3)
                {
                    cantidadOrdenesCompra2 = "0000" + cantidadOrdenesCompra;
                }
                else if (cantidadOrdenesCompra.Length == 4)
                {
                    cantidadOrdenesCompra2 = "000" + cantidadOrdenesCompra;
                }
                else if (cantidadOrdenesCompra.Length == 5)
                {
                    cantidadOrdenesCompra2 = "00" + cantidadOrdenesCompra;
                }
                else if (cantidadOrdenesCompra.Length == 6)
                {
                    cantidadOrdenesCompra2 = "0" + cantidadOrdenesCompra;
                }
                else if (cantidadOrdenesCompra.Length == 7)
                {
                    cantidadOrdenesCompra2 = cantidadOrdenesCompra;
                }
            }
            else
            {
                cantidadOrdenesCompra2 = cantidadOrdenesCompra;
            }
        }

        //CARGAR Y GENERAR EL CÓDIGO DEL REQUERIMIENTO SIMPLE
        public void GenerarCodigoRequerimientoSimple()
        {
            ConteoOrdenesCompra();

            DateTime date = DateTime.Now;

            codigoOrdenCOmpra = Convert.ToString(date.Year) + cantidadOrdenesCompra2;
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

        //CARGA TIPO DE MONEDA
        public void CargarContactoProveedor(int idProveedor)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdDatosAnexosProveedorContacto, Nombre FROM DatosAnexosProveedor_Contacto WHERE Estado = 1 AND IdProveedor = @idProveedor", con);
            comando.Parameters.AddWithValue("@idProveedor", idProveedor);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboContacto.DisplayMember = "Nombre";
            cboContacto.ValueMember = "IdDatosAnexosProveedorContacto";
            cboContacto.DataSource = dt;
        }

        //CARGA TIPO DE MONEDA
        public void CargarTipoMoneda()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdTipoMonedas, Descripcion FROM TipoMonedas WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboMoneda.DisplayMember = "Descripcion";
            cboMoneda.ValueMember = "IdTipoMonedas";
            cboMoneda.DataSource = dt;
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
                datalistadoDetallesRequerimientoD.Columns[0].Width = 50;
                datalistadoDetallesRequerimientoD.Columns[2].Width = 100;
                datalistadoDetallesRequerimientoD.Columns[3].Width = 350;
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
                        datalistadoDetallesRequerimientoD.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                    }
                    if (resultadoRestante < Convert.ToDecimal(datalistadoDetallesRequerimientoD.Rows[i].Cells[7].Value.ToString()))
                    {
                        //PRODUCTOS CON STOCK
                        datalistadoDetallesRequerimientoD.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    if (datalistadoDetallesRequerimientoD.Rows[i].Cells[9].Value.ToString() == "ENTREGADO")
                    {
                        //PRODUCTOS ENTREGADO
                        datalistadoDetallesRequerimientoD.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
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
            DGV.Columns[4].Width = 95;
            DGV.Columns[5].Width = 95;
            DGV.Columns[6].Width = 250;
            DGV.Columns[8].Width = 250;
            DGV.Columns[10].Width = 200;
            DGV.Columns[12].Width = 200;
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
            ColoresListado();
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

        //VISUALIZAR MI REQUERMIENTO PDF
        private void btnVerRequerimiento_Click(object sender, EventArgs e)
        {
            if (datalistadoRequerimiento.CurrentRow != null)
            {
                if (datalistadoRequerimiento.SelectedCells[13].Value.ToString() == "ANULADO")
                {
                    MessageBox.Show("El requerimiento se encuentra anulado, no se puede visualizar el requerimiento.", "Validación del Sistema", MessageBoxButtons.OK);
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

        //SELECCION DEL PDF GENERADO CON SUS RESPECTIVAS FIRMAS, INCLUIDO LA JEFATURA
        private void datalistadoRequerimiento_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoRequerimiento.Columns[e.ColumnIndex];

            if (currentColumn.Name == "btnGenerarPdf")
            {
                if (datalistadoRequerimiento.SelectedCells[13].Value.ToString() == "ANULADO")
                {
                    MessageBox.Show("El requerimiento se encuentra anulado, no se puede visualizar el requerimiento.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    string codigoReporte = datalistadoRequerimiento.Rows[datalistadoRequerimiento.CurrentRow.Index].Cells[1].Value.ToString();
                    Visualizadores.VisualizarRequerimientoSimple frm = new Visualizadores.VisualizarRequerimientoSimple();
                    frm.lblCodigo.Text = codigoReporte;

                    frm.Show();
                }
            }
        }

        //MOSTRAR LA POSIBILIDAD DE ELEJIR LAS FECHAS SEGÚN EL CAMPO SEELCCIOANDO
        private void datalistadoDetalleOC_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 9)
            {
                panelFechaInicio.Visible = true;
            }
        }

        //CARGAR FECHA DE INICIO
        private void btnCargarFechaInicio_Click(object sender, EventArgs e)
        {
            datalistadoDetalleOC.CurrentRow.Cells[10].Value = dateTimeFechaInicio.Text;
            panelFechaInicio.Visible = false;
        }

        //SALIR DE LA FECHA DE TÉRMINO - CARGA
        private void btnSalirFechaInicio_Click(object sender, EventArgs e)
        {
            panelFechaInicio.Visible = false;
        }

        //GENERAR NUEVA ORDEN DE COMPRA
        private void btnNuevoOC_Click(object sender, EventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoRequerimiento.CurrentRow != null)
            {
                int count = 0;
                foreach (DataGridViewRow row in datalistadoRequerimiento.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[2].Value) && Convert.ToString(row.Cells[13].Value) == "EVALUADO")
                    {
                        count++;

                        if (count == 1)
                        {
                            txtRequerimiento1.Text = Convert.ToString(row.Cells[3].Value);
                            lblCodigoReuqe1.Text = Convert.ToString(row.Cells[1].Value);
                            txtAutorizadoPor.Text = "";
                        }

                        if (count == 2)
                        {
                            txtRequerimiento2.Text = Convert.ToString(row.Cells[3].Value);
                            lblCodigoReuqe2.Text = Convert.ToString(row.Cells[1].Value);
                            txtAutorizadoPor.Text = "DIVERSOS REQUERIMEINTOS";
                            txtGeneradoPor.Text = "DIVERSOS REQUERIMEINTOS";
                        }

                        if (count == 3)
                        {
                            txtRequerimiento3.Text = Convert.ToString(row.Cells[3].Value);
                            lblCodigoReuqe3.Text = Convert.ToString(row.Cells[1].Value);
                            txtAutorizadoPor.Text = "DIVERSOS REQUERIMEINTOS";
                            txtGeneradoPor.Text = "DIVERSOS REQUERIMEINTOS";
                        }
                    }
                }

                if (count == 0)
                {
                    MessageBox.Show("Debe seleccionar un requerimiento para poder generar una orden de compra.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    panelNuevaOC.Visible = true;

                    CargarTipoOrdenCompra();
                    cboTipoOC.SelectedIndex = 1;
                    CargarTipoFormaPago();
                    CargarCentroCostos();
                    cboCentreoCostos.SelectedValue = datalistadoRequerimiento.SelectedCells[9].Value.ToString();
                    CargarTipoMoneda();
                    BuscarDetallesRequerimiento(datallistadoDetalles1, Convert.ToInt32(lblCodigoReuqe1.Text));
                    BuscarDetallesRequerimiento(datallistadoDetalles2, Convert.ToInt32(lblCodigoReuqe2.Text));
                    BuscarDetallesRequerimiento(datallistadoDetalles3, Convert.ToInt32(lblCodigoReuqe3.Text));
                    dataTimeFechaRequerimiento.Value = Convert.ToDateTime(datalistadoRequerimiento.SelectedCells[4].Value.ToString());
                    dataTimeFechaEntrega.Value = Convert.ToDateTime(datalistadoRequerimiento.SelectedCells[5].Value.ToString());
                    //FECHA ESTIMADA DE LA OC + 5 DIAS
                    DateTime fechaEstimada = DateTime.Now;
                    fechaEstimada = fechaEstimada.AddDays(5);
                    dataTimeFechaEstimada.Value = fechaEstimada;

                    //RECUPERAR DATOS A MI LISTADO
                    //SE USA EL FOREACH PARA RECORRER TODAS LAS FILAS SELECCIOANDAS
                    //1
                    foreach (DataGridViewRow row in datallistadoDetalles1.Rows)
                    {
                        //SE CAPTURA LAS VARIABLES 
                        string idArt = Convert.ToString(row.Cells[3].Value);
                        string codigo = Convert.ToString(row.Cells[4].Value);
                        string producto = Convert.ToString(row.Cells[5].Value);
                        string idMedida = Convert.ToString(row.Cells[9].Value);
                        string tipoMedida = Convert.ToString(row.Cells[6].Value);
                        string cantidadTotal = Convert.ToString(row.Cells[7].Value);

                        //SE AGREGA A LA NUEVA LISTA
                        datalistadoDetalleOC.Rows.Add(new[] { idArt, codigo, producto, idMedida, tipoMedida, cantidadTotal, null, null, null, null, null });
                    }
                    //2
                    foreach (DataGridViewRow row in datallistadoDetalles2.Rows)
                    {
                        //SE CAPTURA LAS VARIABLES 
                        string idArt = Convert.ToString(row.Cells[3].Value);
                        string codigo = Convert.ToString(row.Cells[4].Value);
                        string producto = Convert.ToString(row.Cells[5].Value);
                        string idMedida = Convert.ToString(row.Cells[9].Value);
                        string tipoMedida = Convert.ToString(row.Cells[6].Value);
                        string cantidadTotal = Convert.ToString(row.Cells[7].Value);

                        //SE AGREGA A LA NUEVA LISTA
                        datalistadoDetalleOC.Rows.Add(new[] { idArt, codigo, producto, idMedida, tipoMedida, cantidadTotal, null, null, null, null, null });
                    }
                    //3
                    foreach (DataGridViewRow row in datallistadoDetalles3.Rows)
                    {
                        //SE CAPTURA LAS VARIABLES 
                        string idArt = Convert.ToString(row.Cells[3].Value);
                        string codigo = Convert.ToString(row.Cells[4].Value);
                        string producto = Convert.ToString(row.Cells[5].Value);
                        string idMedida = Convert.ToString(row.Cells[9].Value);
                        string tipoMedida = Convert.ToString(row.Cells[6].Value);
                        string cantidadTotal = Convert.ToString(row.Cells[7].Value);

                        //SE AGREGA A LA NUEVA LISTA
                        datalistadoDetalleOC.Rows.Add(new[] { idArt, codigo, producto, idMedida, tipoMedida, cantidadTotal, null, null, null, null, null });
                    }

                    //VALIDADOR DE REPETICIONES
                    int m = 1;
                    int n = datalistadoDetalleOC.Rows.Count - 1;
                    int k;
                    string estaFila, unaFila;
                    decimal estaCantidad, unaCantidad;

                    while (m <= n)
                    {
                        k = 1;
                        estaFila = String.Empty;
                        estaFila = datalistadoDetalleOC.Rows[m].Cells[1].Value.ToString();
                        estaCantidad = Convert.ToDecimal(datalistadoDetalleOC.Rows[m].Cells[5].Value.ToString());

                        while (k < n)
                        {
                            unaFila = String.Empty;
                            unaFila = datalistadoDetalleOC.Rows[k].Cells[1].Value.ToString();
                            unaCantidad = Convert.ToDecimal(datalistadoDetalleOC.Rows[k].Cells[5].Value.ToString());

                            if (String.Compare(estaFila, unaFila) == 0 && k != m)
                            {
                                datalistadoDetalleOC.Rows[m].Cells[5].Value = estaCantidad + unaCantidad;
                                datalistadoDetalleOC.Rows.RemoveAt(k);

                                n--;
                            }
                            k++;
                        }
                        m++;
                    }

                    //DEFINICIÓND DE SOLO LECTURA DE MI LISTADO DE PRODUCTOS
                    datalistadoDetalleOC.Columns[1].ReadOnly = true;
                    datalistadoDetalleOC.Columns[2].ReadOnly = true;
                    datalistadoDetalleOC.Columns[4].ReadOnly = true;
                    datalistadoDetalleOC.Columns[8].ReadOnly = true;
                    datalistadoDetalleOC.Columns[10].ReadOnly = true;
                    cboDireccionEntrega.SelectedIndex = 0;
                    alternarColorFilas(datalistadoDetalleOC);

                    if (txtRequerimiento1.Text != "" && txtRequerimiento2.Text == "")
                    {
                        lblUsuarioSolicitante.Text = datalistadoRequerimiento.SelectedCells[7].Value.ToString();
                        DatosUsuario(Convert.ToInt32(lblUsuarioSolicitante.Text));
                        ReconocerAreaJefatura(area);
                        txtGeneradoPor.Text = datalistadoRequerimiento.SelectedCells[8].Value.ToString();
                    }
                }
            }
        }

        //ACCION PARA BUSCAR A LOS PROVEEDORES GUARDADOS
        private void txtNombrewProveedor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                cboTipoBusquedaProveedor.SelectedIndex = 0;
                panelBusquedaProveedor.Visible = true;
                txtBusquedaProveedores.Text = txtNombrewProveedor.Text;
                txtBusquedaProveedores.Focus();
            }
        }

        //CUANDO SE CAMBIO DE CRITERIO DE BUSQUEDA, SE BORRA EL TEXTO DE LA BARRA DE BUSQUEDA
        private void cboTipoBusquedaProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusquedaProveedores.Text = "";
        }

        //CAJADE BÚSQUEDA DE PROVEEDORES SEGÚN CITERIO DE BÚSQUEDA
        private void txtBusquedaProveedores_TextChanged(object sender, EventArgs e)
        {
            if (cboTipoBusquedaProveedor.Text == "NOMBRES")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarProveedorPorNombre", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre", txtBusquedaProveedores.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoproveedor.DataSource = dt;
                con.Close();
            }
            else if (cboTipoBusquedaProveedor.Text == "DOCUMENTO")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarProveedorPorDocumento", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@numero", txtBusquedaProveedores.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoproveedor.DataSource = dt;
                con.Close();
            }
            RedimensionarProveedores(datalistadoproveedor);
        }

        //SELECIONAR PROVEEDOR
        private void datalistadoproveedor_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            lblIdProveedor.Text = datalistadoproveedor.SelectedCells[31].Value.ToString();
            txtCodigoProveedor.Text = datalistadoproveedor.SelectedCells[30].Value.ToString();
            txtRuc.Text = datalistadoproveedor.SelectedCells[1].Value.ToString();
            txtNombrewProveedor.Text = datalistadoproveedor.SelectedCells[2].Value.ToString();
            panelBusquedaProveedor.Visible = false;
            CargarContactoProveedor(Convert.ToInt32(lblIdProveedor.Text));
            CargarTiposBancos(Convert.ToInt32(lblIdProveedor.Text));
        }

        //REDIMENSION DE MIS COLUMNAS DE LISTADO DE PROVEEDORES
        public void RedimensionarProveedores(DataGridView DGV)
        {
            DGV.Columns[0].Width = 100;
            DGV.Columns[1].Width = 100;
            DGV.Columns[2].Width = 350;
            DGV.Columns[3].Width = 100;
            DGV.Columns[4].Width = 112;

            DGV.Columns[5].Visible = false;
            DGV.Columns[6].Visible = false;
            DGV.Columns[7].Visible = false;
            DGV.Columns[8].Visible = false;
            DGV.Columns[9].Visible = false;
            DGV.Columns[10].Visible = false;
            DGV.Columns[11].Visible = false;
            DGV.Columns[12].Visible = false;
            DGV.Columns[13].Visible = false;
            DGV.Columns[14].Visible = false;
            DGV.Columns[15].Visible = false;
            DGV.Columns[16].Visible = false;
            DGV.Columns[17].Visible = false;
            DGV.Columns[18].Visible = false;
            DGV.Columns[19].Visible = false;
            DGV.Columns[20].Visible = false;
            DGV.Columns[21].Visible = false;
            DGV.Columns[22].Visible = false;
            DGV.Columns[23].Visible = false;
            DGV.Columns[24].Visible = false;
            DGV.Columns[25].Visible = false;
            DGV.Columns[26].Visible = false;
            DGV.Columns[27].Visible = false;
            DGV.Columns[28].Visible = false;
            DGV.Columns[29].Visible = false;
            DGV.Columns[30].Visible = false;
            DGV.Columns[31].Visible = false;
        }

        //LISTADO DE PRODUCTOS, CALCULO DE COSTOS
        private void datalistadoDetalleOC_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //VARIABLES PARA ALMACENAR LOS DATOS
            decimal cantidad;
            decimal a;
            decimal b;
            string c;
            //ALMACENAMIENTO DE DATOS PARA LA VALIDACIÓN
            //RECORRIDO DE MI LISTADO PARA ALMACENAR LAS DIFERENTES COLUMNAS
            DataGridViewRow row = (DataGridViewRow)datalistadoDetalleOC.Rows[e.RowIndex];
            //ALMACENAMIENTOS DE COLUMNAS
            cantidad = Convert.ToDecimal(row.Cells[5].Value);

            a = Convert.ToDecimal(row.Cells[6].Value);

            //VALIDACIÓN DEL PRECIO
            if (row.Cells[6].Value == DBNull.Value)
            {
                //REINICIO DE CAMPO
                a = Convert.ToDecimal("0.000");
            }
            else
            {
                //CAPTURA DEL VALOR
                a = Convert.ToDecimal(row.Cells[6].Value);
            }

            b = Convert.ToDecimal(row.Cells[7].Value);

            //VALIDACIÓN DEL DESCUENTO
            if (row.Cells[7].Value == DBNull.Value)
            {
                //REINICIO DE CAMPO
                b = Convert.ToDecimal("0.000");
            }
            else
            {
                //CAPTURA DEL VALOR
                b = Convert.ToDecimal(row.Cells[7].Value);
            }

            c = Convert.ToString(row.Cells[7].Value);

            decimal total = cantidad * a - b;
            row.Cells[6].Value = String.Format("{0:#,0.000}", a);
            row.Cells[7].Value = String.Format("{0:#,0.000}", b);
            row.Cells[8].Value = String.Format("{0:#,0.000}", total);

            SubTotalOC(datalistadoDetalleOC);
            DescuentoTotalOC(datalistadoDetalleOC);

            double igv = Convert.ToDouble(txtSubTotalOC.Text) * 0.18;
            txtIGV.Text = String.Format("{0:#,0.00}", igv);

            decimal totalOC = Convert.ToDecimal(txtSubTotalOC.Text) + Convert.ToDecimal(txtFlete.Text) + Convert.ToDecimal(txtIGV.Text);
            txtTotalOC.Text = Convert.ToString(totalOC);
        }

        //FUNCIÓN PÁRA CALCULAR EL SUBTOTAL
        public void SubTotalOC(DataGridView dgv)
        {
            decimal subtotal = 0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells[8].Value == null)
                {
                    // Exit Sub
                    row.Cells[8].Value = "0.00";
                    subtotal += Convert.ToDecimal(row.Cells[8].Value);
                }
                else
                {
                    subtotal += Convert.ToDecimal(row.Cells[8].Value);
                }
            }

            txtSubTotalOC.Text = String.Format("{0:#,0.00}", subtotal);
        }

        //FUNCIÓN PÁRA CALCULAR EL DESCUENTO TOTAL
        public void DescuentoTotalOC(DataGridView dgv)
        {
            decimal descuentoTotal = 0;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells[7].Value == null)
                {
                    // Exit Sub
                    row.Cells[7].Value = "0.00";
                    descuentoTotal += Convert.ToDecimal(row.Cells[7].Value);
                }
                else
                {
                    descuentoTotal += Convert.ToDecimal(row.Cells[7].Value);
                }
            }

            txtDescuentoTotal.Text = String.Format("{0:#,0.00}", descuentoTotal);
        }

        //PROCEDER A GUARDAR MI ORDEN DE COM´RA
        private void btnGuardarOrdenCompra_Click(object sender, EventArgs e)
        {
            if (txtCodigoProveedor.Text == "" || txtRuc.Text == "")
            {
                MessageBox.Show("Debe ingresar todos los datos solicitados para poder registrar la orden de compra.", "Validación del Sistema");
            }
            else
            {
                if (cboNumeroCuenta.Text == "" || cboTipoVanco.SelectedValue == null || cboContacto.SelectedValue == null)
                {
                    MessageBox.Show("El proveedor seleccionado le faltan datos para poder generar una orden de compra, por favor, complete los datos y vuelva a generar la OC.", "Validación del Sistema");
                }
                else
                {
                    if (txtTotalOC.Text == "")
                    {
                        MessageBox.Show("Debe ingresar los montos de los productos para poder continuar.", "Validación del Sistema");
                    }
                    else
                    {
                        if (datalistadoDetalleOC.Rows.Count == 0)
                        {
                            MessageBox.Show("No se cargó el listado de productos para realizar la orden de compra, por favor vuelva a intentarlo.", "Validación del Sistema");
                        }
                        else
                        {
                            DialogResult boton = MessageBox.Show("¿Realmente desea guardar esta nueva orden de compra?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                            if (boton == DialogResult.OK)
                            {
                                bool sinFecha = false;

                                //VALIDAR SI SE INGRESARON FECHAS
                                foreach (DataGridViewRow row in datalistadoDetalleOC.Rows)
                                {
                                    DateTime fechaInicio = Convert.ToDateTime(row.Cells["columFechaEstimada"].Value);

                                    if (fechaInicio == null || fechaInicio == Convert.ToDateTime("1/01/0001 00:00:00"))
                                    {
                                        sinFecha = true;
                                        MessageBox.Show("Debe ingresar la fecha correspondiente a la entrega.", "Validación del Sistema");
                                        return;
                                    }
                                }

                                try
                                {
                                    SqlConnection con = new SqlConnection();
                                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                                    con.Open();
                                    SqlCommand cmd = new SqlCommand();
                                    cmd = new SqlCommand("InsertarOrdenCompra", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    //INGRESO - PARTE GENERAL DEL REQUERIMIENTO SIMPLE
                                    GenerarCodigoRequerimientoSimple();
                                    cmd.Parameters.AddWithValue("@codigoOrdenCompra", codigoOrdenCOmpra);
                                    cmd.Parameters.AddWithValue("@idRequerimiento1", Convert.ToInt32(lblCodigoReuqe1.Text));
                                    cmd.Parameters.AddWithValue("@idRequerimiento2", Convert.ToInt32(lblCodigoReuqe2.Text));
                                    cmd.Parameters.AddWithValue("@idRequerimiento3", Convert.ToInt32(lblCodigoReuqe3.Text));
                                    cmd.Parameters.AddWithValue("@idProveedor", Convert.ToInt32(lblIdProveedor.Text));
                                    cmd.Parameters.AddWithValue("@idContactoProveedor", cboContacto.SelectedValue.ToString());
                                    cmd.Parameters.AddWithValue("@idBancoProveedor", cboTipoVanco.SelectedValue.ToString());
                                    cmd.Parameters.AddWithValue("@idTipoOrdenCompra", cboTipoOC.SelectedValue.ToString());
                                    cmd.Parameters.AddWithValue("@idFormaPago", cboFormaPago.SelectedValue.ToString());
                                    cmd.Parameters.AddWithValue("@idCentroCostos", cboCentreoCostos.SelectedValue.ToString());
                                    cmd.Parameters.AddWithValue("@idTipoMoneda", cboMoneda.SelectedValue.ToString());
                                    cmd.Parameters.AddWithValue("@idLugarEntrega", 1);
                                    cmd.Parameters.AddWithValue("@fechaOrdenCompra", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@fechaEstimada", dataTimeFechaEstimada.Value);
                                    cmd.Parameters.AddWithValue("@fechaRequerimientoMasAntiguo", dataTimeFechaRequerimiento.Value);
                                    cmd.Parameters.AddWithValue("@fechaRequerimeintoEntregaMasProximo", dataTimeFechaEntrega.Value);
                                    cmd.Parameters.AddWithValue("@codigoCotizacion", txtCotizacionProveedor.Text);

                                    if (txtFileCotizacion.Text != "")
                                    {
                                        string rutaOld1 = txtFileCotizacion.Text;
                                        string name = System.IO.Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                                        string RutaNew1 = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Logística\Ordenes de Compra\" + name + ".pdf";
                                        File.Copy(rutaOld1, RutaNew1);
                                        cmd.Parameters.AddWithValue("@FileCotizacion", RutaNew1);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@FileCotizacion", "");
                                    }

                                    cmd.Parameters.AddWithValue("@autorizacion", txtAutorizadoPor.Text);
                                    cmd.Parameters.AddWithValue("@generacion", txtGeneradoPor.Text);
                                    cmd.Parameters.AddWithValue("@observaciones", txtObservacionesOC.Text);
                                    cmd.Parameters.AddWithValue("@subtotal", txtSubTotalOC.Text);
                                    cmd.Parameters.AddWithValue("@descuento", txtDescuentoTotal.Text);
                                    cmd.Parameters.AddWithValue("@flete", txtFlete.Text);
                                    cmd.Parameters.AddWithValue("@igv", txtIGV.Text);
                                    cmd.Parameters.AddWithValue("@total", txtTotalOC.Text);
                                    cmd.ExecuteNonQuery();
                                    con.Close();

                                    //VARIABLE PARA CONTAR LA CANTIDAD DE ITEMS QUE HAY
                                    int contador = 1;
                                    //INGRESO DE LOS DETALLES DE LA ORDEN DE COMPRA CON UN FOREACH
                                    foreach (DataGridViewRow row in datalistadoDetalleOC.Rows)
                                    {
                                        //PROCEDIMIENTO ALMACENADO PARA GUARDAR LOS PRODUCTOS
                                        con.Open();
                                        cmd = new SqlCommand("InsertarOrdenCompra_DetalleProductos", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@atendido", 0);
                                        cmd.Parameters.AddWithValue("@item", contador);
                                        cmd.Parameters.AddWithValue("@idArt", Convert.ToString(row.Cells[0].Value));
                                        cmd.Parameters.AddWithValue("@cantidad", Convert.ToString(row.Cells[5].Value));
                                        cmd.Parameters.AddWithValue("@precio", Convert.ToString(row.Cells[6].Value));
                                        cmd.Parameters.AddWithValue("@descuento", Convert.ToString(row.Cells[7].Value));
                                        cmd.Parameters.AddWithValue("@total", Convert.ToString(row.Cells[8].Value));
                                        cmd.Parameters.AddWithValue("@fechaEstimada", Convert.ToString(row.Cells[9].Value));
                                        cmd.Parameters.AddWithValue("@descripcionProveedor", Convert.ToString(row.Cells[10].Value));
                                        cmd.ExecuteNonQuery();
                                        con.Close();

                                        contador++;
                                    }

                                    MessageBox.Show("Se ingresó la orden de compra correctamente.", "Validación del Sistema");

                                    datalistadoDetalleOC.Rows.Clear();
                                    txtSubTotalOC.Text = "";
                                    txtObservacionesOC.Text = "";
                                    txtDescuentoTotal.Text = "";
                                    txtIGV.Text = "";
                                    txtTotalOC.Text = "";
                                    txtNombrewProveedor.Text = "";
                                    txtCodigoProveedor.Text = "";
                                    txtRuc.Text = "";
                                    cboNumeroCuenta.Text = "";
                                    txtFileCotizacion.Text = "";
                                    txtCotizacionProveedor.Text = "";
                                    cboTipoVanco.DataSource = null;
                                    cboContacto.DataSource = null;
                                    panelNuevaOC.Visible = false;

                                    //CAMBIO DE ESTADO DE DEL REQUEIRMIENTO SIMPLE - ESTADO OC Y ESTADO REQUE - 1
                                    SqlConnection con2 = new SqlConnection();
                                    SqlCommand cmd2 = new SqlCommand();
                                    con2.ConnectionString = Conexion.ConexionMaestra.conexion;
                                    con2.Open();
                                    cmd2 = new SqlCommand("CambioEstadoRequerimientoSimple_JefaturaOC", con2);
                                    cmd2.CommandType = CommandType.StoredProcedure;
                                    cmd2.Parameters.AddWithValue("@idRequerimientoSimple", lblCodigoReuqe1.Text);
                                    cmd2.ExecuteNonQuery();
                                    con2.Close();

                                    //CAMBIO DE ESTADO DE DEL REQUEIRMIENTO SIMPLE - ESTADO OC Y ESTADO REQUE - 2
                                    SqlConnection con3 = new SqlConnection();
                                    SqlCommand cmd3 = new SqlCommand();
                                    con3.ConnectionString = Conexion.ConexionMaestra.conexion;
                                    con3.Open();
                                    cmd3 = new SqlCommand("CambioEstadoRequerimientoSimple_JefaturaOC", con3);
                                    cmd3.CommandType = CommandType.StoredProcedure;
                                    cmd3.Parameters.AddWithValue("@idRequerimientoSimple", lblCodigoReuqe2.Text);
                                    cmd3.ExecuteNonQuery();
                                    con3.Close();

                                    //CAMBIO DE ESTADO DE DEL REQUEIRMIENTO SIMPLE - ESTADO OC Y ESTADO REQUE - 3
                                    SqlConnection con4 = new SqlConnection();
                                    SqlCommand cmd4 = new SqlCommand();
                                    con4.ConnectionString = Conexion.ConexionMaestra.conexion;
                                    con4.Open();
                                    cmd4 = new SqlCommand("CambioEstadoRequerimientoSimple_JefaturaOC", con4);
                                    cmd4.CommandType = CommandType.StoredProcedure;
                                    cmd4.Parameters.AddWithValue("@idRequerimientoSimple", lblCodigoReuqe3.Text);
                                    cmd4.ExecuteNonQuery();
                                    con4.Close();

                                    MostrarRequerimientoPorFecha(DesdeFecha.Value, HastaFecha.Value);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                        }
                    }
                }
            }
        }

        //BOTON PARA ABRIR LA VENTANA DE CARGA PARA EL DOCUMENTO 
        private void btnCargarCotizacion_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Todos los archivos (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFileCotizacion.Text = openFileDialog1.FileName;
            }
        }

        //SALIR Y REGRESAR DE ORDEN DE COMPRA
        private void btnRegresarOrdenCompra_Click(object sender, EventArgs e)
        {
            datalistadoDetalleOC.Rows.Clear();
            txtSubTotalOC.Text = "";
            txtObservacionesOC.Text = "";
            txtDescuentoTotal.Text = "";
            txtIGV.Text = "";
            txtTotalOC.Text = "";
            txtNombrewProveedor.Text = "";
            txtCodigoProveedor.Text = "";
            txtRuc.Text = "";
            cboNumeroCuenta.Text = "";
            txtFileCotizacion.Text = "";
            txtCotizacionProveedor.Text = "";
            cboTipoVanco.DataSource = null;
            cboContacto.DataSource = null;
            panelNuevaOC.Visible = false;
            lblCodigoReuqe1.Text = "0";
            lblCodigoReuqe2.Text = "0";
            lblCodigoReuqe3.Text = "0";
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

                MessageBox.Show("Reporte exportado correctamente a: {rutaSalida}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al exportar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            sl.SaveAs(desktopPath + @"\Reporte de Requerimiento Simple.xlsx");
            MessageBox.Show("Se exportó los datos a un archivo de Microsoft Excel en la siguiente ubicación: " + desktopPath, "Validación del Sistema", MessageBoxButtons.OK);
        }
    }
}

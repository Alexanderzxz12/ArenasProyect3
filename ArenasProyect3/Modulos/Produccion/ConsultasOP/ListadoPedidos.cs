﻿using ArenasProyect3.Modulos.ManGeneral;
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
    public partial class ListadoPedidos : Form
    {
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        string ruta = ManGeneral.Manual.manualRequerimientosSimples;
        private Cursor curAnterior = null;
        string VisualizarOC = "";

        string codigoOrdenProduccion = "";
        string cantidadOrdenProduccion = "0000000";
        string cantidadOrdenProduccion2 = "";

        string codigoOS = "";
        string cantidadOS = "0000000";
        string cantidadOS2 = "";

        string codigoRequerimientoSimple = "";
        string cantidadRequerimiento = "0000000";
        string cantidadRequerimiento2 = "";

        //CÓDIGO PARA PODER MOSTRAR LA HORA EN VIVO
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHoraFecha.Text = DateTime.Now.ToString("H:mm:ss tt");
        }

        //CONMSTRUCTOR DE MI FORMULARIO
        public ListadoPedidos()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI FORMULARIO
        private void ListadoPedidos_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            datalistadoTodasPedido.DataSource = null;

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

        //CONTAR LA CANTIDAD DE REQUERIMIENTOS QUE HAY EN MI TABLA me estoy moudie de s
        public void ConteoRequerimientosSimples()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            da = new SqlDataAdapter("SELECT IdRequerimientoSimple FROM RequerimientoSimple WHERE IdRequerimientoSimple = (SELECT MAX(IdRequerimientoSimple) FROM RequerimientoSimple)", con);
            da.Fill(dt);
            datalistadoCargarCantidadRequerimeintoSimple.DataSource = dt;
            con.Close();

            if (datalistadoCargarCantidadRequerimeintoSimple.RowCount > 0)
            {
                cantidadRequerimiento = datalistadoCargarCantidadRequerimeintoSimple.SelectedCells[0].Value.ToString();

                if (cantidadRequerimiento.Length == 1)
                {
                    cantidadRequerimiento2 = "000000" + cantidadRequerimiento;
                }
                else if (cantidadRequerimiento.Length == 2)
                {
                    cantidadRequerimiento2 = "00000" + cantidadRequerimiento;
                }
                else if (cantidadRequerimiento.Length == 3)
                {
                    cantidadRequerimiento2 = "0000" + cantidadRequerimiento;
                }
                else if (cantidadRequerimiento.Length == 4)
                {
                    cantidadRequerimiento2 = "000" + cantidadRequerimiento;
                }
                else if (cantidadRequerimiento.Length == 5)
                {
                    cantidadRequerimiento2 = "00" + cantidadRequerimiento;
                }
                else if (cantidadRequerimiento.Length == 6)
                {
                    cantidadRequerimiento2 = "0" + cantidadRequerimiento;
                }
                else if (cantidadRequerimiento.Length == 7)
                {
                    cantidadRequerimiento2 = cantidadRequerimiento;
                }
            }
            else
            {
                cantidadRequerimiento2 = cantidadRequerimiento;
            }
        }

        //CARGAR Y GENERAR EL CÓDIGO DEL REQUERIMIENTO SIMPLE
        public void GenerarCodigoRequerimientoSimple()
        {
            ConteoRequerimientosSimples();

            DateTime date = DateTime.Now;

            codigoRequerimientoSimple = Convert.ToString(date.Year) + cantidadRequerimiento2;
        }

        //CONTAR LA CANTIDAD DE ORDENES DE PRODUCCION
        public void ConteoOrdenProduccion()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            da = new SqlDataAdapter("SELECT IdOrdenProduccion FROM OrdenProduccion WHERE IdOrdenProduccion = (SELECT MAX(IdOrdenProduccion) FROM OrdenProduccion)", con);
            da.Fill(dt);
            datalistadoConteoOP.DataSource = dt;
            con.Close();

            if (datalistadoConteoOP.RowCount > 0)
            {
                cantidadOrdenProduccion = datalistadoConteoOP.SelectedCells[0].Value.ToString();

                if (cantidadOrdenProduccion.Length == 1)
                {
                    cantidadOrdenProduccion2 = "000000" + cantidadOrdenProduccion;
                }
                else if (cantidadOrdenProduccion.Length == 2)
                {
                    cantidadOrdenProduccion2 = "00000" + cantidadOrdenProduccion;
                }
                else if (cantidadOrdenProduccion2.Length == 3)
                {
                    cantidadOrdenProduccion = "0000" + cantidadOrdenProduccion;
                }
                else if (cantidadOrdenProduccion.Length == 4)
                {
                    cantidadOrdenProduccion2 = "000" + cantidadOrdenProduccion;
                }
                else if (cantidadOrdenProduccion.Length == 5)
                {
                    cantidadOrdenProduccion2 = "00" + cantidadOrdenProduccion;
                }
                else if (cantidadOrdenProduccion.Length == 6)
                {
                    cantidadOrdenProduccion2 = "0" + cantidadOrdenProduccion;
                }
                else if (cantidadOrdenProduccion.Length == 7)
                {
                    cantidadOrdenProduccion2 = cantidadOrdenProduccion;
                }
            }
            else
            {
                cantidadOrdenProduccion2 = cantidadOrdenProduccion;
            }
        }

        //CONTAR LA CANTIDAD DE OS
        public void ConteoOS()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            da = new SqlDataAdapter("SELECT IdOrdenServicio FROM OrdenServicio WHERE IdOrdenServicio = (SELECT MAX(IdOrdenServicio) FROM OrdenServicio)", con);
            da.Fill(dt);
            datalistadoConteoOS.DataSource = dt;
            con.Close();

            if (datalistadoConteoOS.RowCount > 0)
            {
                cantidadOS = datalistadoConteoOS.SelectedCells[0].Value.ToString();

                if (cantidadOS.Length == 1)
                {
                    cantidadOS2 = "000000" + cantidadOS;
                }
                else if (cantidadOS.Length == 2)
                {
                    cantidadOS2 = "00000" + cantidadOS;
                }
                else if (cantidadOS.Length == 3)
                {
                    cantidadOS2 = "0000" + cantidadOS;
                }
                else if (cantidadOS.Length == 4)
                {
                    cantidadOS2 = "000" + cantidadOS;
                }
                else if (cantidadOS.Length == 5)
                {
                    cantidadOS2 = "00" + cantidadOS;
                }
                else if (cantidadOS.Length == 6)
                {
                    cantidadOS2 = "0" + cantidadOS;
                }
                else if (cantidadOS.Length == 7)
                {
                    cantidadOS2 = cantidadOS;
                }
            }
            else
            {
                cantidadOS2 = cantidadOS;
            }
        }

        //TRAER EL ULTIMO REGISTRO PARA CREAR MI REQUERIMIENTO
        public void UltimaOP()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            da = new SqlDataAdapter("SELECT IdOrdenProduccion FROM OrdenProduccion WHERE IdOrdenProduccion = (SELECT MAX(IdOrdenProduccion) FROM OrdenProduccion)", con);
            da.Fill(dt);
            datalistadoUltimaOP.DataSource = dt;
            con.Close();
        }

        //CARGAR Y GENERAR EL CÓDIGO DE OP
        public void GenerarCodigoOrdenProduccion()
        {
            ConteoOrdenProduccion();

            DateTime date = DateTime.Now;

            codigoOrdenProduccion = Convert.ToString(date.Year) + cantidadOrdenProduccion2;
        }

        //VERIFICAR SI TODOS LOS ITEMS TIENNE OP
        public void ValidarOPparaPedidos(int IdPedido, int totalItems)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarOPporPedido", con);
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
                    cmd = new SqlCommand("CambioEstadoPedido", con);
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
                    cmd = new SqlCommand("CambioEstadoPedido", con);
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
                    cmd = new SqlCommand("CambioEstadoPedido", con);
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
                cmd = new SqlCommand("CambioEstadoPedido", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idPedido", IdPedido);
                cmd.Parameters.AddWithValue("@estadoPedido", 1);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        //CARGAR Y GENERAR EL CÓDIGO DE OS
        public void GenerarCodigoOS()
        {
            ConteoOS();

            DateTime date = DateTime.Now;

            codigoOS = Convert.ToString(date.Year) + cantidadOS2;
        }

        //BUSCAR DETALLES DE MI PEDIDO
        public void BuscarPedidoPorCodigo(int idPedido)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarPedidoPorCodigo", con);
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
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarPedidoPorCodigoDetalles", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idPedido", idPedido);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoDetallePedido.DataSource = dt;
            con.Close();
        }

        //BUSCAR DETALLES Y MATERIALES DE MI FORMULACION
        public void BuscarMaterialesFormulacion(string codigoFormulacion)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarMaterialesFormulacion", con);
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
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarMaterialesFormulacionSemi", con);
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
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarLineaFormulacion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigoFormulacion", codigoFormulacion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoLineaFormulacion.DataSource = dt;
            con.Close();
        }

        //BUSCAR EL ULTIMO COLOR DE MI PRODUCTO EN UNA OP
        public void BuscarUltimoColorProducto(int idProducto)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarUltimoColorProductoOP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idProducto", idProducto);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoBusquedaColorUltimoProducto.DataSource = dt;
            con.Close();
        }

        //BUSCAR MI SEMIPRODUCIDO DE MI FRMULACION
        public void BuscarSemiProducidoFormulacionOP(string codigoFormulacion)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarSemiProducidoFormulacionOP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigoFormulacion", codigoFormulacion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoSemiProducidoFormulacion.DataSource = dt;
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
            DataTable dt = new DataTable();
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
            DataTable dt = new DataTable();
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
            DataTable dt = new DataTable();
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
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboOperacion.ValueMember = "IdTipoOperacionPro";
            cboOperacion.DisplayMember = "Nombre";
            cboOperacion.DataSource = dt;
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
        public void MostrarPedidoPorCliente(string cliente, DateTime fechaInicio, DateTime fechaTermino)
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
            DGV.Columns[14].Visible = false;

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            ColoresListadoPedidos();
            ColoresListadoPedidos();
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO PEDIDOS
        public void ColoresListadoPedidos()
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= datalistadoTodasPedido.RowCount - 1; i++)
                {
                    ValidarOPparaPedidos(Convert.ToInt32(datalistadoTodasPedido.Rows[i].Cells[1].Value), Convert.ToInt32(datalistadoTodasPedido.Rows[i].Cells[9].Value));

                    if (datalistadoTodasPedido.Rows[i].Cells[12].Value.ToString() == "PENDIENTE")
                    {
                        datalistadoTodasPedido.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else if (datalistadoTodasPedido.Rows[i].Cells[12].Value.ToString() == "INCOMPLETA")
                    {
                        datalistadoTodasPedido.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(192, 192, 0);
                    }
                    else if (datalistadoTodasPedido.Rows[i].Cells[12].Value.ToString() == "CULMINADA")
                    {
                        datalistadoTodasPedido.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                    }
                    else if (datalistadoTodasPedido.Rows[i].Cells[12].Value.ToString() == "DESPACHADO")
                    {
                        datalistadoTodasPedido.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                    }
                    else
                    {
                        datalistadoTodasPedido.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
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

        //FUNCIOAN PARA CAMBIAR MI CURSOR 
        private void datalistadoProductos_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoProductos.Columns[e.ColumnIndex].Name == "pl1" || this.datalistadoProductos.Columns[e.ColumnIndex].Name == "pl2")
            {
                this.datalistadoProductos.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoProductos.Cursor = curAnterior;
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
            MostrarPedidoPorCliente(txtBusqueda.Text, DesdeFecha.Value, HastaFecha.Value);
        }

        //GENERACION DE REPORTES
        private void btnGenerarPedidoPdf_Click(object sender, EventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoTodasPedido.CurrentRow != null)
            {
                string ccodigoCotizacion = datalistadoTodasPedido.Rows[datalistadoTodasPedido.CurrentRow.Index].Cells[1].Value.ToString();
                Visualizadores.VisualizarPedidoVenta frm = new Visualizadores.VisualizarPedidoVenta();
                frm.lblCodigo.Text = ccodigoCotizacion;

                frm.Show();
            }
            else
            {
                MessageBox.Show("Debe seleccionar un pedido para poder generar el PDF.", "Validación del Sistema");
            }
        }

        //VISUALIZAR ORDEN DE COMPRA
        private void btnAbiriOrdenCompra_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(datalistadoTodasPedido.SelectedCells[14].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Documento no encontrado, hubo un error al momento de cargar el archivo.", ex.Message);
            }
        }

        //FUNCION PARA ANULAR UN PEDIDO/COTIZACION-------------------------------------------------------------
        //FUNCION PARA VERIFICAR SI HAY OP CREADA PARA PROCEDER A ANULAR PEDIDO
        public void VerificarOPxPedidoAnulacion(int idPedido)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarOPxPedidoAnulacion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idPedido", idPedido);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoBuscarOPxPedidoAnulacion.DataSource = dt;
            con.Close();
        }

        //PRODEDIMEINTO PARA ANULAR MI PEDIDO
        private void btnAnularPedido_Click(object sender, EventArgs e)
        {
            LimpiarAnulacionPedido();
            panleAnulacion.Visible = true;
            datalistadoTodasPedido.Enabled = false;
        }

        //FUNCION PARA PROCEDER A ANULAR MI PEDIDO, COTIZACION
        private void btnProcederAnulacion_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasPedido.CurrentRow != null)
            {
                int idPedido = Convert.ToInt32(datalistadoTodasPedido.SelectedCells[1].Value.ToString());
                string idCotizacion = datalistadoTodasPedido.SelectedCells[13].Value.ToString();
                int ordenProduccion = 0;

                VerificarOPxPedidoAnulacion(idPedido);

                if (datalistadoBuscarOPxPedidoAnulacion.RowCount > 0)
                {
                    ordenProduccion = datalistadoBuscarOPxPedidoAnulacion.RowCount;
                }

                DialogResult boton = MessageBox.Show("¿Realmente desea anular esta pedido?. Se anulará la cotización asociada ha este pedido.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    if (ordenProduccion > 0)
                    {
                        MessageBox.Show("El pedido que desea anular ya tiene una orden de producción generada.", "Validación del Sistema", MessageBoxButtons.OK);
                    }
                    else
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
                            LimpiarAnulacionPedido();
                            panleAnulacion.Visible = false;
                            datalistadoTodasPedido.Enabled = true;
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
                MessageBox.Show("Debe seleccionar un pedido para poder anularlo.", "Validación del Sistema");
            }
        }

        //BOTON PARA RETROCEDER DE LA ANULACION
        private void btnRetrocederAnulacion_Click(object sender, EventArgs e)
        {
            LimpiarAnulacionPedido();
            panleAnulacion.Visible = false;
            datalistadoTodasPedido.Enabled = true;
        }

        //FUNCION PARA LIMPIAR MIS CONTROLES ORIETADO A ANULACION DE PEDIDO
        public void LimpiarAnulacionPedido()
        {
            txtJustificacionAnulacion.Text = "";
        }
        //-------------------------------------------------------------------------------------------------------

        //CREAR ORDEN DE PRODUCCION 
        private void btnOrdenProduccion_Click(object sender, EventArgs e)
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
                    dtFechaCreacionOP.Value = DateTime.Now;
                    dtFechaTerminoOP.Value = DateTime.Now;
                    dtpFechaGeneraPedido.Value = DateTime.Now;

                    panelGenerarOP.Visible = true;

                    
                    lblIdCliente.Text = datalistadoPedido.SelectedCells[3].Value.ToString();
                    lblIdUnidad.Text = datalistadoPedido.SelectedCells[7].Value.ToString();
                    lblIdSolicitante.Text = datalistadoPedido.SelectedCells[9].Value.ToString();
                    lblLuharEntrega.Text = datalistadoPedido.SelectedCells[14].Value.ToString();

                    lblCodigoPedido.Text = datalistadoPedido.SelectedCells[1].Value.ToString();
                    lblIdPedido.Text = datalistadoPedido.SelectedCells[0].Value.ToString();
                    dtFechaTerminoOP.Value = Convert.ToDateTime(datalistadoPedido.SelectedCells[11].Value.ToString());

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
                MessageBox.Show("Debe seleccionar un registro para poder generar una OP.", "Validación del Sistema");
            }
        }

        //FUNCION PARA ORDENAR MI LISTADO DE PRODICTOS
        public static void SortByColumn(DataGridView dataGridView, int columnIndex, ListSortDirection direction)
        {
            // Verifica que el índice de la columna esté dentro del rango
            if (columnIndex >= 0 && columnIndex < dataGridView.Columns.Count)
            {
                // Ordena el DataGridView por la columna especificada y en la dirección especificada
                dataGridView.Sort(dataGridView.Columns[columnIndex], direction);
            }
            else
            {
                MessageBox.Show("Índice de columna fuera de rango.");
            }
        }

        //VISUALIZAR MI OC
        private void btnVerOC_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(VisualizarOC);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Documento no encontrado, hubo un error al momento de cargar el archivo.", ex.Message);
            }
        }

        //SELECCIONAR UN PRODUCTO - TERMINAR LA SELECCION
        private void datalistadoProductos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
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
            }

            txtProducto.Text = datalistadoProductos.SelectedCells[3].Value.ToString();
            lblIdProducto.Text = datalistadoProductos.SelectedCells[16].Value.ToString();
            txtCodigoBSS.Text = datalistadoProductos.SelectedCells[13].Value.ToString();
            txtCodigoSistema.Text = datalistadoProductos.SelectedCells[12].Value.ToString();
            txtCodigoCliente.Text = datalistadoProductos.SelectedCells[14].Value.ToString();
            string codigoFormulacion = datalistadoProductos.SelectedCells[15].Value.ToString();
            txtCodigoFormulacion.Text = codigoFormulacion;
            int numeroProducir = Convert.ToInt32(datalistadoProductos.SelectedCells[6].Value.ToString());

            BuscarMaterialesFormulacion(codigoFormulacion);
            BuscarMaterialesFormulacionSemi(codigoFormulacion);
            BuscarLineaFormulacion(codigoFormulacion);

            BuscarSemiProducidoFormulacionOP(codigoFormulacion);

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

                decimal totalProductas = Convert.ToDecimal(cantidad) * numeroProducir;

                datalistadoActividadesSemi.Rows.Add(new[] { Convert.ToString(contador), idMaterialDetalleActividad, idProducto, codigoBSS, codigoSistema, descripcionProducto, cantidad, Convert.ToString(totalProductas), medida, stock });
                contador2 = contador2 + 1;
            }

            lblCantidadMaterialesItemsSemi.Text = Convert.ToString(datalistadoActividadesSemi.RowCount);
            lblCantidadItemsMateriales.Text = Convert.ToString(datalistadoActividades.RowCount);
            alternarColorFilas(datalistadoActividades);
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
                            MessageBox.Show("No se ha podido encontrar el archivo o plano, por favor cargar el plano o seleccionarlo al momento de crear la formulación.", "Validación del Sistema", MessageBoxButtons.OK);
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
                            MessageBox.Show("No se ha podido encontrar el archivo o plano, por favor cargar el plano o seleccionarlo al momento de crear la formulación.", "Validación del Sistema", MessageBoxButtons.OK);
                        }
                    }
                }

                txtProducto.Text = datalistadoProductos.SelectedCells[3].Value.ToString();
                lblIdProducto.Text = datalistadoProductos.SelectedCells[16].Value.ToString();
                txtCodigoBSS.Text = datalistadoProductos.SelectedCells[13].Value.ToString();
                txtCodigoSistema.Text = datalistadoProductos.SelectedCells[12].Value.ToString();
                txtCodigoCliente.Text = datalistadoProductos.SelectedCells[14].Value.ToString();
                string codigoFormulacion = datalistadoProductos.SelectedCells[15].Value.ToString();
                dtFechaTerminoOP.Value = Convert.ToDateTime(datalistadoProductos.SelectedCells[11].Value.ToString());
                txtCodigoFormulacion.Text = codigoFormulacion;
                int numeroProducir = Convert.ToInt32(datalistadoProductos.SelectedCells[6].Value.ToString());


                BuscarMaterialesFormulacion(codigoFormulacion);
                BuscarMaterialesFormulacionSemi(codigoFormulacion);
                BuscarLineaFormulacion(codigoFormulacion);

                BuscarSemiProducidoFormulacionOP(codigoFormulacion);


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

                    decimal totalProductas = Convert.ToDecimal(cantidad) * numeroProducir;

                    datalistadoActividadesSemi.Rows.Add(new[] { Convert.ToString(contador), idMaterialDetalleActividad, idProducto, codigoBSS, codigoSistema, descripcionProducto, cantidad, Convert.ToString(totalProductas), medida, stock });
                    contador2 = contador2 + 1;
                }

                lblCantidadMaterialesItemsSemi.Text = Convert.ToString(datalistadoActividadesSemi.RowCount);
                lblCantidadItemsMateriales.Text = Convert.ToString(datalistadoActividades.RowCount);
                alternarColorFilas(datalistadoActividades);
            }
        }

        //BOTON PARA GUARDAR MI OP Y REQUERIMEINTOP
        private void btnGuardarOP_Click(object sender, EventArgs e)
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
                    DialogResult boton = MessageBox.Show("¿Realmente desea generar esta orden de producción?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                    if (boton == DialogResult.OK)
                    {
                        try
                        {
                            //INGRESAR MI ORDEN DE PRODCCUIN-------------------------------------------------
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("InsertarOrdenProduccion", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            //INGRESO - PARTE GENERAL DE ORDEN PRODUCCION
                            GenerarCodigoOrdenProduccion();
                            cmd.Parameters.AddWithValue("@codigoOrdenProduccion", codigoOrdenProduccion);
                            cmd.Parameters.AddWithValue("@fechaInicial", dtFechaCreacionOP.Value);
                            cmd.Parameters.AddWithValue("@fechaEntrega", dtFechaTerminoOP.Value);
                            cmd.Parameters.AddWithValue("@idCliente", lblIdCliente.Text);
                            cmd.Parameters.AddWithValue("@idUnidad", lblIdUnidad.Text);
                            cmd.Parameters.AddWithValue("@idVendedor", lblIdSolicitante.Text);
                            cmd.Parameters.AddWithValue("@lugarEntrega", lblLuharEntrega.Text);
                            cmd.Parameters.AddWithValue("@fechaProduccion", dtFechaTerminoOP.Value);
                            //INGRESAR DATOS DE MI PRODUCTO A FABRICAR
                            cmd.Parameters.AddWithValue("@idArt", lblIdProducto.Text);
                            cmd.Parameters.AddWithValue("@descripcionProducto", txtProducto.Text);
                            cmd.Parameters.AddWithValue("@planoProducto", datalistadoProductos.SelectedCells[17].Value.ToString());
                            cmd.Parameters.AddWithValue("@colorProducto", txtColorProducto.Text);
                            cmd.Parameters.AddWithValue("@codigoBSS", txtCodigoBSS.Text);
                            cmd.Parameters.AddWithValue("@codigoSis", txtCodigoSistema.Text);
                            cmd.Parameters.AddWithValue("@codigoCliente", txtCodigoCliente.Text);
                            //CONDICIONAL PARA DEFINIR SI EL PRODUCTO TIENE SE,MIPRODUCIDO
                            if (datalistadoSemiProducidoFormulacion.Rows.Count > 0)
                            {
                                //INGRESAR DATOS DE MI SEMIPRODUCIDO
                                cmd.Parameters.AddWithValue("@idArtSemi", Convert.ToInt64(datalistadoSemiProducidoFormulacion.SelectedCells[0].Value.ToString()));
                                cmd.Parameters.AddWithValue("@descripcionSemiProducido", datalistadoSemiProducidoFormulacion.SelectedCells[2].Value.ToString());
                                cmd.Parameters.AddWithValue("@planoProductoSemi", datalistadoSemiProducidoFormulacion.SelectedCells[4].Value.ToString());
                                cmd.Parameters.AddWithValue("@colorSemi", DBNull.Value);
                                cmd.Parameters.AddWithValue("@codigoBssSemiProducido", datalistadoSemiProducidoFormulacion.SelectedCells[3].Value.ToString());
                                cmd.Parameters.AddWithValue("@codigoSisSemiProducido", datalistadoSemiProducidoFormulacion.SelectedCells[1].Value.ToString());
                                cmd.Parameters.AddWithValue("@codigoClienteSemiProducido", DBNull.Value);
                            }
                            else
                            {
                                //INGRESAR DATOS NULLOS PARA MI SEMIPRODUCIDO
                                cmd.Parameters.AddWithValue("@idArtSemi", DBNull.Value);
                                cmd.Parameters.AddWithValue("@descripcionSemiProducido", DBNull.Value);
                                cmd.Parameters.AddWithValue("@planoProductoSemi", DBNull.Value);
                                cmd.Parameters.AddWithValue("@colorSemi", DBNull.Value);
                                cmd.Parameters.AddWithValue("@codigoBssSemiProducido", DBNull.Value);
                                cmd.Parameters.AddWithValue("@codigoSisSemiProducido", DBNull.Value);
                                cmd.Parameters.AddWithValue("@codigoClienteSemiProducido", DBNull.Value);
                            }
                            //INGRESAR DATOS DE MI PEDIDO
                            cmd.Parameters.AddWithValue("@idPedido", datalistadoProductos.SelectedCells[19].Value.ToString());
                            cmd.Parameters.AddWithValue("@numeroItem", datalistadoProductos.SelectedCells[21].Value.ToString());
                            cmd.Parameters.AddWithValue("@totalItems", datalistadoProductos.SelectedCells[20].Value.ToString());
                            cmd.Parameters.AddWithValue("@cantidadTotal", datalistadoProductos.SelectedCells[6].Value.ToString());
                            //MODIFICACION DEL ESTADO DE MI DETALLE PEDIDO
                            cmd.Parameters.AddWithValue("@idDetallePedido", datalistadoProductos.SelectedCells[22].Value.ToString());
                            cmd.Parameters.AddWithValue("@observaciones", txtObservacionesOP.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            UltimaOP();
                            ValidarOPparaPedidos(Convert.ToInt32(datalistadoProductos.SelectedCells[19].Value.ToString()), Convert.ToInt32(datalistadoProductos.SelectedCells[20].Value.ToString()));
                            GenerarCodigoRequerimientoSimple();
                            MostrarPedidoPorFecha(DesdeFecha.Value, HastaFecha.Value);

                            //INGRESAR MI REQUERIMIENTO PARA MI ORDEN DE PRODUCCION-----------------------------
                            SqlConnection con2 = new SqlConnection();
                            con2.ConnectionString = Conexion.ConexionMaestra.conexion;
                            SqlCommand cmd2 = new SqlCommand();
                            con2.Open();
                            cmd2 = new SqlCommand("InsertarRequerimientoSimpleOP", con2);

                            cmd2.CommandType = CommandType.StoredProcedure;
                            //INGRESAR LOS DATOS GENERALES DE MI REQUERIMIENTO
                            cmd2.Parameters.AddWithValue("@codigoRequerimeintoSimple", codigoRequerimientoSimple);
                            cmd2.Parameters.AddWithValue("@fechaRequerida", DateTime.Now);
                            cmd2.Parameters.AddWithValue("@fechaSolicitada", DateTime.Now);
                            cmd2.Parameters.AddWithValue("@desJefatura", "EDUARDO LORO");
                            cmd2.Parameters.AddWithValue("@idSolicitante", 1052);
                            cmd2.Parameters.AddWithValue("@idCentroCostos", 8);
                            cmd2.Parameters.AddWithValue("@observaciones", "REQUERIMIENTO PARA ORDEN DE PRODUCCION");
                            cmd2.Parameters.AddWithValue("@idSede", 1);
                            cmd2.Parameters.AddWithValue("@idLocal", 1);
                            cmd2.Parameters.AddWithValue("@idArea", 1);
                            cmd2.Parameters.AddWithValue("@idipo", 2);
                            cmd2.Parameters.AddWithValue("@estadoLogistica", 1);
                            cmd2.Parameters.AddWithValue("@mensajeAnulacion", "");
                            cmd2.Parameters.AddWithValue("@idJefatura", 1052);
                            cmd2.Parameters.AddWithValue("@aliasCargaJefatura", "Jefe de Producción");
                            cmd2.Parameters.AddWithValue("@cantidadItems", Convert.ToInt32(lblCantidadItemsMateriales.Text));
                            cmd2.Parameters.AddWithValue("@idPrioridad", 1);
                            cmd2.Parameters.AddWithValue("@idOP", datalistadoUltimaOP.SelectedCells[0].Value.ToString());
                            cmd2.Parameters.AddWithValue("@idOT", 0);
                            cmd2.ExecuteNonQuery();
                            con2.Close();

                            //VARIABLE PARA CONTAR LA CANTIDAD DE ITEMS QUE HAY
                            int contador = 1;
                            //INGRESO DE LOS DETALLES DEL REQUERIMIENTO SIMPLE CON UN FOREACH
                            foreach (DataGridViewRow row in datalistadoActividades.Rows)
                            {
                                decimal cantidad = Convert.ToDecimal(row.Cells["cantidad"].Value);

                                //PROCEDIMIENTO ALMACENADO PARA GUARDAR LOS PRODUCTOS
                                con.Open();
                                cmd = new SqlCommand("InsertarRequerimientoSimple_DetalleProductos", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@item", contador);
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
                                contador++;
                            }

                            if (datalistadoSemiProducidoFormulacion.Rows.Count > 0)
                            {
                                //INGRESAR MI OT PARA MI ORDEN DE PRODUCCION-----------------------------
                                SqlConnection con3 = new SqlConnection();
                                con3.ConnectionString = Conexion.ConexionMaestra.conexion;
                                SqlCommand cmd3 = new SqlCommand();
                                con3.Open();
                                cmd3 = new SqlCommand("InsertarOrdenTrabajo", con3);
                                cmd3.CommandType = CommandType.StoredProcedure;

                                //INGRESAR LOS DATOS GENERALES DE OT
                                GenerarCodigoOS();
                                cmd3.Parameters.AddWithValue("@codigoOrdenServicio", codigoOS);
                                cmd3.Parameters.AddWithValue("@fechaInicial", dtFechaCreacionOP.Value);
                                cmd3.Parameters.AddWithValue("@fechaEntrega", dtFechaTerminoOP.Value);
                                cmd3.Parameters.AddWithValue("@idArt", datalistadoSemiProducidoFormulacion.SelectedCells[0].Value.ToString());
                                cmd3.Parameters.AddWithValue("@codigoProducto", datalistadoSemiProducidoFormulacion.SelectedCells[1].Value.ToString());
                                cmd3.Parameters.AddWithValue("@descripcionProducto", datalistadoSemiProducidoFormulacion.SelectedCells[2].Value.ToString());
                                cmd3.Parameters.AddWithValue("@planoProducto", datalistadoSemiProducidoFormulacion.SelectedCells[4].Value.ToString());
                                cmd3.Parameters.AddWithValue("@color", txtColorProducto.Text);
                                cmd3.Parameters.AddWithValue("@codigoBSS", datalistadoSemiProducidoFormulacion.SelectedCells[3].Value.ToString());
                                cmd3.Parameters.AddWithValue("@idGeneraUsuario", DBNull.Value);
                                cmd3.Parameters.AddWithValue("@usuarioGenera", txtSolicitante.Text);
                                cmd3.Parameters.AddWithValue("@idSede", cboSede.SelectedValue.ToString());
                                cmd3.Parameters.AddWithValue("@idPrioridad", cboPrioridad.SelectedValue.ToString());
                                cmd3.Parameters.AddWithValue("@idLocal", cboLocal.SelectedValue.ToString());
                                cmd3.Parameters.AddWithValue("@idOperacion", cboOperacion.SelectedValue.ToString());
                                cmd3.Parameters.AddWithValue("@observacion", "");
                                cmd3.Parameters.AddWithValue("@cantidad", datalistadoProductos.SelectedCells[6].Value.ToString());
                                cmd3.Parameters.AddWithValue("@idCliente", Convert.ToInt32(lblIdCliente.Text));
                                cmd3.ExecuteNonQuery();
                                con3.Close();

                                UltimaOP();
                                GenerarCodigoRequerimientoSimple();

                                //INGRESAR MI REQUERIMIENTO PARA MI ORDEN DE PRODUCCION-----------------------------
                                SqlConnection con4 = new SqlConnection();
                                con4.ConnectionString = Conexion.ConexionMaestra.conexion;
                                SqlCommand cmd4 = new SqlCommand();
                                con4.Open();
                                cmd4 = new SqlCommand("InsertarRequerimientoSimpleOT", con4);

                                cmd4.CommandType = CommandType.StoredProcedure;
                                //INGRESAR LOS DATOS GENERALES DE MI REQUERIMIENTO
                                cmd4.Parameters.AddWithValue("@codigoRequerimeintoSimple", codigoRequerimientoSimple);
                                cmd4.Parameters.AddWithValue("@fechaRequerida", DateTime.Now);
                                cmd4.Parameters.AddWithValue("@fechaSolicitada", DateTime.Now);
                                cmd4.Parameters.AddWithValue("@desJefatura", "EDUARDO LORO");
                                cmd4.Parameters.AddWithValue("@idSolicitante", 1052);
                                cmd4.Parameters.AddWithValue("@idCentroCostos", 8);
                                cmd4.Parameters.AddWithValue("@observaciones", "REQUERIMIENTO PARA ORDEN DE SERVICIO");
                                cmd4.Parameters.AddWithValue("@idSede", 1);
                                cmd4.Parameters.AddWithValue("@idLocal", 1);
                                cmd4.Parameters.AddWithValue("@idArea", 1);
                                cmd4.Parameters.AddWithValue("@idipo", 2);
                                cmd4.Parameters.AddWithValue("@estadoLogistica", 1);
                                cmd4.Parameters.AddWithValue("@mensajeAnulacion", "");
                                cmd4.Parameters.AddWithValue("@idJefatura", 1052);
                                cmd4.Parameters.AddWithValue("@aliasCargaJefatura", "Jefe de Producción");
                                cmd4.Parameters.AddWithValue("@cantidadItems", Convert.ToInt32(lblCantidadMaterialesItemsSemi.Text));
                                cmd4.Parameters.AddWithValue("@idPrioridad", 1);
                                cmd4.Parameters.AddWithValue("@idOP", datalistadoUltimaOP.SelectedCells[0].Value.ToString());
                                cmd4.ExecuteNonQuery();
                                con4.Close();

                                //VARIABLE PARA CONTAR LA CANTIDAD DE ITEMS QUE HAY
                                int contadorOT = 1;
                                //INGRESO DE LOS DETALLES DEL REQUERIMIENTO SIMPLE CON UN FOREACH
                                foreach (DataGridViewRow row in datalistadoActividadesSemi.Rows)
                                {
                                    decimal cantidad = Convert.ToDecimal(row.Cells["cantidadSemi"].Value);

                                    //PROCEDIMIENTO ALMACENADO PARA GUARDAR LOS PRODUCTOS
                                    con.Open();
                                    cmd = new SqlCommand("InsertarRequerimientoSimple_DetalleProductos", con);
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
                            }

                            //MENSAJE DE CONFORMIAD CON EL INGRESO DE LA ORDEN DE PRODUCCION
                            MessageBox.Show("Se generó la Orden de producción correctamente.", "Validación del Sistema");

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
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        //BOTON PARA SALÑIR DE MI CREACION DE OP
        private void btnSalirOP_Click(object sender, EventArgs e)
        {
            panelGenerarOP.Visible = false;
        }

        //FUNCION PARA LIMPIAR TODOS MI CAMPOS DE MI ORDEN DE PRODUCCION
        public void LimpiarCamposOrdenProduccion()
        {
            datalistadoActividades.Rows.Clear();
            datalistadoProductos.Rows.Clear();
            dtpFechaGeneraPedido.Value = DateTime.Now;
            dtFechaCreacionOP.Value = DateTime.Now;
            dtFechaTerminoOP.Value = DateTime.Now;
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
            txtObservacionesOP.Text = "";
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
            dtFechaCreacionOP.Value = DateTime.Now;
            dtFechaTerminoOP.Value = DateTime.Now;
            txtProducto.Text = "";
            txtCodigoBSS.Text = "";
            txtCodigoSistema.Text = "";
            txtCodigoCliente.Text = "";
            txtArea.Text = "";
            txtCodigoFormulacion.Text = "";
            txtColorProducto.Text = "";
            txtObservacionesOP.Text = "";
            lblCantidadItemsMateriales.Text = "***";
            lblCantidadItems.Text = "***";
            cboSede.SelectedIndex = 0;
            cboPrioridad.SelectedIndex = 0;
            cboLocal.SelectedIndex = 0;
            cboOperacion.SelectedIndex = 0;
        }

        //FUNCION DE EDICION DE UNA ORDEN DE COMPRA-----------------------------------------------------------------
        //FUNCION PARA ACTIVAR MI EDICION DE ORDEN DE COMPRA DE PEDIUDO
        private void btnModificarOrdenCompra_Click(object sender, EventArgs e)
        {
            LimpiarEdicionOrdenCompra();
            panelModificacionOrdenCompra.Visible = true;
            datalistadoTodasPedido.Enabled = false;
        }

        //FUNCION PARA EDITAR MI ORDEN DE COMPRA DE MI PEDIDO GENERADO
        private void btnCargarOrdenCompra_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Todos los archivos (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtRutaOrdenCompraModi.Text = openFileDialog1.FileName;
            }
        }

        //FUNCION PARA LIMPIAR MI CAJA DE TEXTO LENADO POR MI RUTA
        private void btnLimpiarCargaOrdenCompra_Click(object sender, EventArgs e)
        {
            txtRutaOrdenCompraModi.Text = "";
        }

        //FUNCION PARA REGRESAR O SALIR DE MI EDICION DE ORDEN DE COMPRA
        private void btnRegresarOrdenCompra_Click(object sender, EventArgs e)
        {
            LimpiarEdicionOrdenCompra();
            panelModificacionOrdenCompra.Visible = false;
            datalistadoTodasPedido.Enabled = true;
        }

        //FUNCION PARA PODER PROCEDER CON LA EDICION DE MI ORDEN DE COMPRA
        private void btnEditarOrdenCompra_Click(object sender, EventArgs e)
        {
            if (txtCodigoOrdenCompraModi.Text == "" || txtRutaOrdenCompraModi.Text == "")
            {
                MessageBox.Show("Debe ingresar un código de orden de compra o adjuntar un documento.", "Validación del Sistema");
            }
            else
            {
                try
                {
                    //IMODIFICAR MI ORDEN DE COMPRA DE UN PEDIDO
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("ModificarOrdenCompra", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //MODIFICACION
                    cmd.Parameters.AddWithValue("@idPedido", datalistadoTodasPedido.SelectedCells[1].Value.ToString());
                    cmd.Parameters.AddWithValue("@codigOC", txtCodigoOrdenCompraModi.Text);

                    string NombreGenerado = "ORDEN DE COMPRA " + txtCodigoOrdenCompraModi.Text + " - PEDIDO " + datalistadoTodasPedido.SelectedCells[2].Value.ToString();
                    string RutaOld = txtRutaOrdenCompraModi.Text;
                    string RutaNew = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Comercial\OrdenCompraPedido\" + NombreGenerado + ".pdf";
                    File.Copy(RutaOld, RutaNew, true);

                    cmd.Parameters.AddWithValue("@rutaOC", RutaNew);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    //MENSAJE DE CONFIRMACION DE EDICION DE ORDEN DE COMPRA
                    MessageBox.Show("Se editó correctamente la orden de compra del pedido " + datalistadoTodasPedido.SelectedCells[2].Value.ToString() + ".", "Validación del Sistema");
                    LimpiarEdicionOrdenCompra();
                    MostrarPedidoPorFecha(DesdeFecha.Value, HastaFecha.Value);
                    panelModificacionOrdenCompra.Visible = false;
                    datalistadoTodasPedido.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: ", ex.Message);
                    datalistadoTodasPedido.Enabled = true;
                }
            }
        }

        public void LimpiarEdicionOrdenCompra()
        {
            txtRutaOrdenCompraModi.Text = "";
            txtCodigoOrdenCompraModi.Text = "";
        }
        //----------------------------------------------------------------------------------------------------------

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
                string codigoPedido = datalistadoTodasPedido.SelectedCells[2].Value.ToString(); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
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

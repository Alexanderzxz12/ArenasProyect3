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
using System.Diagnostics;
using ArenasProyect3.Modulos.ManGeneral;
using SpreadsheetLight;
using DocumentFormat.OpenXml.Spreadsheet;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using ArenasProyect3.Modulos.Resourses;

namespace ArenasProyect3.Modulos.Comercial.Ventas
{
    public partial class Pedido : Form
    {
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        string ruta = ManGeneral.Manual.manualAreaComercial;
        private Cursor curAnterior = null;

        //CONMSTRUCTOR DE MI FORMULARIO
        public Pedido()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI FORMULARIO
        private void Pedido_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            datalistadoTodasPedido.DataSource = null;
            cboBusqeuda.SelectedIndex = 0;

            //PREFILES Y PERSIMOS---------------------------------------------------------------
            if (Program.RangoEfecto != 1)
            {
                btnAnularPedido.Visible = false;
                lblAnularPedido.Visible = false;
            }
            //---------------------------------------------------------------------------------
        }

        //FUNCION PARA VERIFICAR SI HAY OP CREADA PARA PROCEDER A ANULAR PEDIDO
        public void VerificarOPxPedidoAnulacion(int idPedido)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Pedido_BuscarOPxPedidoAnulacion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idPedido", idPedido);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoBuscarOPxPedidoAnulacion.DataSource = dt;
            con.Close();
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

        //VERIFICAR SI TODOS LOS ITEMS TIENNE OP
        public void ValidarOPparaPedidos(int IdPedido, int totalItems)
        {
            DataTable dt = new DataTable();
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

        //LISTADO DE PEDIDOS Y SELECCION DE PDF Y ESTADO DE PEDIDOS---------------------
        //MOSTRAR PEDIDOS AL INCIO 
        public void MostrarPedidoPorFecha(DateTime fechaInicio, DateTime fechaTermino)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Pedido_MostrarPorFecha", con);
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
        public void MostrarPedidoCliente(string cliente, DateTime fechaInicio, DateTime fechaTermino)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Pedido_MostrarPorCliente", con);
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
            DGV.Columns[3].Width = 90;
            DGV.Columns[4].Width = 90;
            DGV.Columns[5].Width = 350;
            DGV.Columns[6].Width = 130;
            DGV.Columns[7].Width = 80;
            DGV.Columns[8].Width = 80;
            DGV.Columns[9].Width = 70;
            DGV.Columns[10].Width = 160;
            DGV.Columns[11].Width = 90;
            DGV.Columns[12].Width = 130;

            DGV.Columns[1].Visible = false;
            DGV.Columns[13].Visible = false;
            DGV.Columns[14].Visible = false;
            DGV.Columns[15].Visible = false;

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

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

        //FUNCIÓN PARA COLOREAR MIS REGISTROS Y ITEMS DE MI DASHBOARD
        public void ColoresListadoItemsPedidos(DataGridView DGV, int posicion)
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= DGV.RowCount - 1; i++)
                {
                    if (DGV.Rows[i].Cells[posicion].Value.ToString() == "CULMINADO")
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                    }
                    else
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                }

                foreach (DataGridViewColumn column in DGV.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
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

        //REACCION AL MOMENTO DE ENVONTRAR MI COTIZACION
        private void lblCodigoCotizacionDash_TextChanged(object sender, EventArgs e)
        {
            string codigoCotizacion = lblCodigoCotizacionDash.Text;
            CargarCotizacionDashCodigo(codigoCotizacion);

            txtEstadoCotizacionDash.Text = datalistadoDetalleCotiDash.SelectedCells[2].Value.ToString();
            txtMontoCotizacionDash.Text = datalistadoDetalleCotiDash.SelectedCells[3].Value.ToString();
            txtResponsableCotizacionDash.Text = datalistadoDetalleCotiDash.SelectedCells[4].Value.ToString();

            CargarPedidoDash(codigoCotizacion);
            cboCodigoPedidoDash.Items.Clear(); // Limpia los valores anteriores

            foreach (DataGridViewRow fila in datalistadoDetallePedidoDash.Rows)
            {
                if (fila.Cells["CODIGO PEDIDO"].Value != null)
                {
                    cboCodigoPedidoDash.Items.Add(fila.Cells["CODIGO PEDIDO"].Value.ToString());
                }
            }
            //VALIDAR SI HAY PEDIDO
            if (cboCodigoPedidoDash.Items.Count != 0)
            {
                cboCodigoPedidoDash.SelectedIndex = 0;
            }
            else
            {
                txtEstadoPedidoDash.Text = "";
                txtMontoPedidoDash.Text = "";
                txtResponsablePedidoDash.Text = "";
                flechaPedidoMono.Visible = true;
                flechaPedidoIncompleta.Visible = false;
                flechaPedidoColor.Visible = false;
                lblEstadoPedidoDash.Text = "SIN REGISTRO";
                lblEstadoPedidoDash.ForeColor = System.Drawing.Color.Black;
            }
        }

        //REACCION AL MOMENTO DE ENVONTRAR MI PEDIDO
        private void cboCodigoPedidoDash_SelectedIndexChanged(object sender, EventArgs e)
        {
            string codigoPedido = cboCodigoPedidoDash.Text;
            CargarPedidoDashCodigo(codigoPedido);
            MostrarItemsSegunPedido(cboCodigoPedidoDash.Text);

            txtEstadoPedidoDash.Text = "";
            txtEstadoPedidoDash.Text = datalistadoDetallePedidoDash.SelectedCells[2].Value.ToString();
            txtMontoPedidoDash.Text = "";
            txtMontoPedidoDash.Text = datalistadoDetallePedidoDash.SelectedCells[3].Value.ToString();
            txtResponsablePedidoDash.Text = "";
            txtResponsablePedidoDash.Text = datalistadoDetallePedidoDash.SelectedCells[4].Value.ToString();

            CargarOrdenProduccionDash(codigoPedido);
            cboCodigoOPDash.Items.Clear(); // Limpia los valores anteriores

            foreach (DataGridViewRow fila in datalistadoOrdenProduccionDash.Rows)
            {
                if (fila.Cells["N°. OP"].Value != null)
                {
                    cboCodigoOPDash.Items.Add(fila.Cells["N°. OP"].Value.ToString());
                }
            }
            //VALIDAR SI HAY OP
            if (cboCodigoOPDash.Items.Count != 0)
            {
                cboCodigoOPDash.SelectedIndex = 0;
            }
            else
            {
                txtEstadoOPDash.Text = "";
                txtCantidadOPDash.Text = "";
                txtCantidadRealizadaOPDash.Text = "";
                flechaOPMono.Visible = true;
                flechaOPIncompleto.Visible = false;
                flechaOPColor.Visible = false;
                lblEstadoOPDash.Text = "SIN REGISTRO";
                lblEstadoOPDash.ForeColor = System.Drawing.Color.Black;
            }
        }

        //REACCION AL MOMENTO DE SELECCIONAR LA OP
        private void cboCodigoOPDash_SelectedIndexChanged(object sender, EventArgs e)
        {
            string codigoOP = cboCodigoOPDash.Text;
            CargarOrdenProduccionDashCodigo(codigoOP);
            MostrarItemsSegunOP(cboCodigoOPDash.Text);

            txtEstadoOPDash.Text = datalistadoOrdenProduccionDash.SelectedCells[2].Value.ToString();
            txtCantidadOPDash.Text = datalistadoOrdenProduccionDash.SelectedCells[3].Value.ToString();
            txtCantidadRealizadaOPDash.Text = datalistadoOrdenProduccionDash.SelectedCells[4].Value.ToString();
        }

        //COLORES DE IMAGENES DEPENDIENDO EL ESTAOD --------------------------------------------------------------
        //COTIZACION
        private void lblEstadoCotizacionDash_Click(object sender, EventArgs e)
        {
            if (txtEstadoCotizacionDash.Text == "ANULADO" || txtEstadoCotizacionDash.Text == "PENDIENTE" || txtEstadoCotizacionDash.Text == "ERROR")
            {
                //ACCION DE FLECHAS
                flechaCotizacionMono.Visible = true;
                flechaCotizacionIncompleta.Visible = false;
                flechaCotizacionColor.Visible = false;

                //ACCION DE LA IMGAEN
                imgCotizacionMono.Visible = true;
                imgCotizacionMixto.Visible = false;
                imgCotizacionColor.Visible = false;

                //ACCION DEL TEXTO
                lblEstadoCotizacionDash.Text = "PENDIENTE";
                lblEstadoCotizacionDash.ForeColor = System.Drawing.Color.Black;
            }
            else if (txtEstadoCotizacionDash.Text == "INCOMPLETA" || txtEstadoCotizacionDash.Text == "FUERA DE FECHA")
            {
                //ACCION DE FLECHAS
                flechaCotizacionMono.Visible = false;
                flechaCotizacionIncompleta.Visible = true;
                flechaCotizacionColor.Visible = false;

                //ACCION DE LA IMGAEN
                imgCotizacionMono.Visible = false;
                imgCotizacionMixto.Visible = true;
                imgCotizacionColor.Visible = false;

                //ACCION DEL TEXTO
                lblEstadoCotizacionDash.Text = "INCOMPLETA";
                lblEstadoCotizacionDash.ForeColor = System.Drawing.Color.Peru;
            }
            else
            {
                //ACCION DE FLECHAS
                flechaCotizacionMono.Visible = false;
                flechaCotizacionIncompleta.Visible = false;
                flechaCotizacionColor.Visible = true;

                //ACCION DE LA IMGAEN
                imgCotizacionMono.Visible = false;
                imgCotizacionMixto.Visible = false;
                imgCotizacionColor.Visible = true;

                //ACCION DEL TEXTO
                lblEstadoCotizacionDash.Text = "COMPLETO";
                lblEstadoCotizacionDash.ForeColor = System.Drawing.Color.ForestGreen;
            }
        }

        //ORDEN DE PRODUCCION
        private void lblEstadoPedidoDash_Click(object sender, EventArgs e)
        {
            if (txtEstadoPedidoDash.Text == "ANULADO" || txtEstadoPedidoDash.Text == "PENDIENTE" || txtEstadoPedidoDash.Text == "ERROR")
            {
                //ACCION DE FLECHAS
                flechaPedidoMono.Visible = true;
                flechaPedidoIncompleta.Visible = false;
                flechaPedidoColor.Visible = false;

                //ACCION DE LA IMGAEN
                imgPedidoMono.Visible = true;
                imgPedidoMixto.Visible = false;
                imgPedidoColor.Visible = false;

                //ACCION DEL TEXTO
                lblEstadoPedidoDash.Text = "PENDIENTE";
                lblEstadoPedidoDash.ForeColor = System.Drawing.Color.Black;
            }
            else if (txtEstadoPedidoDash.Text == "INCOMPLETA")
            {
                //ACCION DE FLECHAS
                flechaPedidoMono.Visible = false;
                flechaPedidoIncompleta.Visible = true;
                flechaPedidoColor.Visible = false;

                //ACCION DE LA IMGAEN
                imgPedidoMono.Visible = false;
                imgPedidoMixto.Visible = true;
                imgPedidoColor.Visible = false;

                //ACCION DEL TEXTO
                lblEstadoPedidoDash.Text = "INCOMPLETA";
                lblEstadoPedidoDash.ForeColor = System.Drawing.Color.Peru;
            }
            else if (txtEstadoPedidoDash.Text == "CULMINADA")
            {
                //ACCION DE FLECHAS
                flechaPedidoMono.Visible = false;
                flechaPedidoIncompleta.Visible = false;
                flechaPedidoColor.Visible = true;

                //ACCION DE LA IMGAEN
                imgPedidoMono.Visible = false;
                imgPedidoMixto.Visible = false;
                imgPedidoColor.Visible = true;

                //ACCION DEL TEXTO
                lblEstadoPedidoDash.Text = "COMPLETO";
                lblEstadoPedidoDash.ForeColor = System.Drawing.Color.ForestGreen;
            }
            else
            {
                //ACCION DE FLECHAS
                flechaPedidoMono.Visible = true;
                flechaPedidoIncompleta.Visible = false;
                flechaPedidoColor.Visible = false;

                //ACCION DE LA IMGAEN
                imgPedidoMono.Visible = true;
                imgPedidoMixto.Visible = false;
                imgPedidoColor.Visible = false;

                //ACCION DEL TEXTO
                lblEstadoPedidoDash.Text = "SIN REGISTRO";
                lblEstadoPedidoDash.ForeColor = System.Drawing.Color.Black;
            }
        }
        //--------------------------------------------------------------------------------------------------------

        //VER LOS DETALLES DE MI PEDIDO Y LOS ESTADOS
        private void datalistadoTodasPedido_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoTodasPedido.RowCount != 0)
            {
                int idPedido = Convert.ToInt32(datalistadoTodasPedido.SelectedCells[1].Value.ToString());
                DataGridViewColumn currentColumnT = datalistadoTodasPedido.Columns[e.ColumnIndex];

                if (currentColumnT.Name == "detalles")
                {
                    cboTipoVisualizacion.SelectedIndex = 0;
                    panelDetalleOP.Visible = true;
                    CargarItemsGeneral(idPedido);
                    CargarItemsGeneraoOP(idPedido);
                    CargarCotizacionDash(idPedido);

                    lblIdCotizacion.Text = datalistadoDetalleCotiDash.SelectedCells[0].Value.ToString();
                    lblCodigoCotizacionDash.Text = datalistadoDetalleCotiDash.SelectedCells[1].Value.ToString();

                    MostrarItemsSegunCotizacion(Convert.ToInt32(lblIdCotizacion.Text));
                    MostrarItemsSegunPedido(cboCodigoPedidoDash.Text);
                }
            }
        }

        //VER LOS DETALLES DE MI PEDIDO Y LOS ESTADOS
        private void datalistadoTodasPedido_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //LIMPIAR MI LISTADO
            datalistadooItemsOP.DataSource = null;
            datalistadooItemsPedido.DataSource = null;
            datalistadooItemsCotizacion.DataSource = null;

            if (datalistadoTodasPedido.RowCount != 0)
            {
                int idPedido = Convert.ToInt32(datalistadoTodasPedido.SelectedCells[1].Value.ToString());

                cboTipoVisualizacion.SelectedIndex = 0;
                panelDetalleOP.Visible = true;
                CargarItemsGeneral(idPedido);
                CargarItemsGeneraoOP(idPedido);
                CargarCotizacionDash(idPedido);

                lblIdCotizacion.Text = datalistadoDetalleCotiDash.SelectedCells[0].Value.ToString();
                lblCodigoCotizacionDash.Text = datalistadoDetalleCotiDash.SelectedCells[1].Value.ToString();

                MostrarItemsSegunCotizacion(Convert.ToInt32(lblIdCotizacion.Text));
                MostrarItemsSegunPedido(cboCodigoPedidoDash.Text);

            }
        }

        //COLORES DE IMAGENES DEPENDIENDO EL ESTAOD --------------------------------------------------------------
        //COTIZACION
        private void txtEstadoCotizacionDash_TextChanged(object sender, EventArgs e)
        {
            if (txtEstadoCotizacionDash.Text == "ANULADO" || txtEstadoCotizacionDash.Text == "PENDIENTE" || txtEstadoCotizacionDash.Text == "ERROR")
            {
                //ACCION DE FLECHAS
                flechaCotizacionMono.Visible = true;
                flechaCotizacionIncompleta.Visible = false;
                flechaCotizacionColor.Visible = false;

                //ACCION DE LA IMGAEN
                imgCotizacionMono.Visible = true;
                imgCotizacionMixto.Visible = false;
                imgCotizacionColor.Visible = false;

                //ACCION DEL TEXTO
                lblEstadoCotizacionDash.Text = "PENDIENTE";
                lblEstadoCotizacionDash.ForeColor = System.Drawing.Color.Black;
            }
            else if (txtEstadoCotizacionDash.Text == "INCOMPLETA" || txtEstadoCotizacionDash.Text == "FUERA DE FECHA")
            {
                //ACCION DE FLECHAS
                flechaCotizacionMono.Visible = false;
                flechaCotizacionIncompleta.Visible = true;
                flechaCotizacionColor.Visible = false;

                //ACCION DE LA IMGAEN
                imgCotizacionMono.Visible = false;
                imgCotizacionMixto.Visible = true;
                imgCotizacionColor.Visible = false;

                //ACCION DEL TEXTO
                lblEstadoCotizacionDash.Text = "INCOMPLETA";
                lblEstadoCotizacionDash.ForeColor = System.Drawing.Color.Peru;
            }
            else
            {
                //ACCION DE FLECHAS
                flechaCotizacionMono.Visible = false;
                flechaCotizacionIncompleta.Visible = false;
                flechaCotizacionColor.Visible = true;

                //ACCION DE LA IMGAEN
                imgCotizacionMono.Visible = false;
                imgCotizacionMixto.Visible = false;
                imgCotizacionColor.Visible = true;

                //ACCION DEL TEXTO
                lblEstadoCotizacionDash.Text = "COMPLETO";
                lblEstadoCotizacionDash.ForeColor = System.Drawing.Color.ForestGreen;
            }
        }

        //PEDIDOS
        private void txtEstadoPedidoDash_TextChanged(object sender, EventArgs e)
        {
            if (txtEstadoPedidoDash.Text == "ANULADO" || txtEstadoPedidoDash.Text == "PENDIENTE" || txtEstadoPedidoDash.Text == "ERROR")
            {
                //ACCION DE FLECHAS
                flechaPedidoMono.Visible = true;
                flechaPedidoIncompleta.Visible = false;
                flechaPedidoColor.Visible = false;

                //ACCION DE LA IMGAEN
                imgPedidoMono.Visible = true;
                imgPedidoMixto.Visible = false;
                imgPedidoColor.Visible = false;

                //ACCION DEL TEXTO
                lblEstadoPedidoDash.Text = "PENDIENTE";
                lblEstadoPedidoDash.ForeColor = System.Drawing.Color.Black;
            }
            else if (txtEstadoPedidoDash.Text == "INCOMPLETA")
            {
                //ACCION DE FLECHAS
                flechaPedidoMono.Visible = false;
                flechaPedidoIncompleta.Visible = true;
                flechaPedidoColor.Visible = false;

                //ACCION DE LA IMGAEN
                imgPedidoMono.Visible = false;
                imgPedidoMixto.Visible = true;
                imgPedidoColor.Visible = false;

                //ACCION DEL TEXTO
                lblEstadoPedidoDash.Text = "INCOMPLETA";
                lblEstadoPedidoDash.ForeColor = System.Drawing.Color.Peru;
            }
            else if (txtEstadoPedidoDash.Text == "CULMINADA")
            {
                //ACCION DE FLECHAS
                flechaPedidoMono.Visible = false;
                flechaPedidoIncompleta.Visible = false;
                flechaPedidoColor.Visible = true;

                //ACCION DE LA IMGAEN
                imgPedidoMono.Visible = false;
                imgPedidoMixto.Visible = false;
                imgPedidoColor.Visible = true;

                //ACCION DEL TEXTO
                lblEstadoPedidoDash.Text = "COMPLETO";
                lblEstadoPedidoDash.ForeColor = System.Drawing.Color.ForestGreen;
            }
            else
            {
                //ACCION DE FLECHAS
                flechaPedidoMono.Visible = true;
                flechaPedidoIncompleta.Visible = false;
                flechaPedidoColor.Visible = false;

                //ACCION DE LA IMGAEN
                imgPedidoMono.Visible = true;
                imgPedidoMixto.Visible = false;
                imgPedidoColor.Visible = false;

                //ACCION DEL TEXTO
                lblEstadoPedidoDash.Text = "SIN REGISTRO";
                lblEstadoPedidoDash.ForeColor = System.Drawing.Color.Black;
            }
        }

        //ORDEN DE PRODUCCION
        private void txtEstadoOPDash_TextChanged(object sender, EventArgs e)
        {
            if (txtEstadoOPDash.Text == "ANULADO" || txtEstadoOPDash.Text == "PENDIENTE" || txtEstadoOPDash.Text == "NO DEFINIDO")
            {
                //ACCION DE FLECHAS
                flechaOPMono.Visible = true;
                flechaOPIncompleto.Visible = false;
                flechaOPColor.Visible = false;

                //ACCION DE LA IMGAEN
                imgProduccionMono.Visible = true;
                imgProduccionMixto.Visible = false;
                imgProduccionColor.Visible = false;

                //ACCION DEL TEXTO
                lblEstadoOPDash.Text = "PENDIENTE";
                lblEstadoOPDash.ForeColor = System.Drawing.Color.Black;
            }
            else if (txtEstadoOPDash.Text == "LÍMITE" || txtEstadoOPDash.Text == "FUERA DE FECHA")
            {
                //ACCION DE FLECHAS
                flechaOPMono.Visible = false;
                flechaOPIncompleto.Visible = true;
                flechaOPColor.Visible = false;

                //ACCION DE LA IMGAEN
                imgProduccionMono.Visible = false;
                imgProduccionMixto.Visible = true;
                imgProduccionColor.Visible = false;

                //ACCION DEL TEXTO
                lblEstadoOPDash.Text = "INCOMPLETA";
                lblEstadoOPDash.ForeColor = System.Drawing.Color.Peru;
            }
            else if (txtEstadoOPDash.Text == "CULMINADO")
            {
                //ACCION DE FLECHAS
                flechaOPMono.Visible = false;
                flechaOPIncompleto.Visible = false;
                flechaOPColor.Visible = true;

                //ACCION DE LA IMGAEN
                imgProduccionMono.Visible = false;
                imgProduccionMixto.Visible = false;
                imgProduccionColor.Visible = true;

                //ACCION DEL TEXTO
                lblEstadoOPDash.Text = "CULMINADO";
                lblEstadoOPDash.ForeColor = System.Drawing.Color.ForestGreen;
            }
            else
            {
                //ACCION DE FLECHAS
                flechaOPMono.Visible = true;
                flechaOPIncompleto.Visible = false;
                flechaOPColor.Visible = false;

                //ACCION DE LA IMGAEN
                imgProduccionMono.Visible = true;
                imgProduccionMixto.Visible = false;
                imgProduccionColor.Visible = false;

                //ACCION DEL TEXTO
                lblEstadoOPDash.Text = "SIN REGISTRO";
                lblEstadoOPDash.ForeColor = System.Drawing.Color.Black;
            }
        }

        //FUNCIONES PARA LAS CARGAS DEL SOCHBOARD
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

        //VER DETALLES (ITEMS) DE MI COTIZACION
        public void MostrarItemsSegunCotizacion(int idcotizacion)
        {
            try
            {
                //LIMPIAR MI LISTADO
                datalistadooItemsCotizacion.DataSource = null;

                System.Data.DataTable dt = new System.Data.DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Dashboard_CotizacionMostrarItems", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCotizacion", idcotizacion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadooItemsCotizacion.DataSource = dt;
                con.Close();
                datalistadooItemsCotizacion.Columns[0].Width = 20;
                datalistadooItemsCotizacion.Columns[3].Width = 300;
                datalistadooItemsCotizacion.Columns[4].Width = 70;
                datalistadooItemsCotizacion.Columns[5].Width = 70;
                datalistadooItemsCotizacion.Columns[6].Width = 70;
                datalistadooItemsCotizacion.Columns[7].Width = 70;

                datalistadooItemsCotizacion.Columns[1].Visible = false;
                datalistadooItemsCotizacion.Columns[2].Visible = false;
                datalistadooItemsCotizacion.Columns[8].Visible = false;
                datalistadooItemsCotizacion.Columns[9].Visible = false;
                datalistadooItemsCotizacion.Columns[10].Visible = false;
                datalistadooItemsCotizacion.Columns[11].Visible = false;

                datalistadooItemsCotizacion.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                alternarColorFilas(datalistadooItemsCotizacion);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema", "Validación del Sistema", MessageBoxButtons.OK);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //VER DETALLES (ITEMS) DE MI PEDIDO
        public void MostrarItemsSegunPedido(string codigoPedido)
        {
            try
            {
                //LIMPIAR MI LISTADO
                datalistadooItemsPedido.DataSource = null;

                System.Data.DataTable dt = new System.Data.DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Pedido_MostrarItemsPorCodigo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigoPedido", codigoPedido);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadooItemsPedido.DataSource = dt;
                con.Close();
                datalistadooItemsPedido.Columns[0].Width = 20;
                datalistadooItemsPedido.Columns[3].Width = 300;
                datalistadooItemsPedido.Columns[4].Width = 70;
                datalistadooItemsPedido.Columns[5].Width = 70;
                datalistadooItemsPedido.Columns[6].Width = 70;
                datalistadooItemsPedido.Columns[7].Width = 70;

                datalistadooItemsPedido.Columns[1].Visible = false;
                datalistadooItemsPedido.Columns[2].Visible = false;

                datalistadooItemsPedido.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                alternarColorFilas(datalistadooItemsPedido);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema", "Validación del Sistema", MessageBoxButtons.OK);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //VER DETALLES (ITEMS) DE MI PEDIDO
        public void MostrarItemsSegunOP(string codigoOP)
        {
            try
            {
                //LIMPIAR MI LISTADO
                datalistadooItemsOP.DataSource = null;

                System.Data.DataTable dt = new System.Data.DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Pedido_MostrarItemsPorCodigoOP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigoOP", codigoOP);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadooItemsOP.DataSource = dt;
                con.Close();
                datalistadooItemsOP.Columns[0].Width = 20;
                datalistadooItemsOP.Columns[1].Width = 300;
                datalistadooItemsOP.Columns[2].Width = 70;
                datalistadooItemsOP.Columns[3].Width = 70;
                datalistadooItemsOP.Columns[4].Width = 70;

                datalistadooItemsOP.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                alternarColorFilas(datalistadooItemsOP);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema", "Validación del Sistema", MessageBoxButtons.OK);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //CARGA DE ITEMS GENERTA
        public void CargarItemsGeneral(int idPedido)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Dashboard_PedidoCargaItems", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idPedido", idPedido);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoItemsGeneral.DataSource = dt;
            con.Close();
            datalistadoItemsGeneral.Columns[1].Width = 32;
            datalistadoItemsGeneral.Columns[2].Width = 550;
            datalistadoItemsGeneral.Columns[3].Width = 65;
            datalistadoItemsGeneral.Columns[4].Width = 70;
            datalistadoItemsGeneral.Columns[5].Width = 70;
            datalistadoItemsGeneral.Columns[6].Width = 70;
            datalistadoItemsGeneral.Columns[7].Width = 65;
            datalistadoItemsGeneral.Columns[8].Width = 85;
            datalistadoItemsGeneral.Columns[9].Width = 75;

            //COLUMNAS NO VISIBLES
            datalistadoItemsGeneral.Columns[0].Visible = false;
            ColoresListadoItemsPedidos(datalistadoItemsGeneral, 8);
        }

        //CARGA DE ITEMS GENERTA CONTROL OP
        public void CargarItemsGeneraoOP(int idPedido)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Dashboard_CargaItemsControlOP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idPedido", idPedido);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoItemsProduccion.DataSource = dt;
            con.Close();
            datalistadoItemsProduccion.Columns[0].Width = 85;
            datalistadoItemsProduccion.Columns[1].Width = 130;
            datalistadoItemsProduccion.Columns[1].Width = 130;

            ColoresListadoItemsPedidos(datalistadoItemsProduccion, 1);
        }

        //COTIZACION
        //CARGA DETALLES DE MI COTIZACION
        public void CargarCotizacionDash(int idPedido)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Dashboard_CargarDetallesCoti", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idPedido", idPedido);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoDetalleCotiDash.DataSource = null;
            datalistadoDetalleCotiDash.DataSource = dt;
            con.Close();
        }

        //CARGA DETALLES DE MI COTIZACION CODIGOO
        public void CargarCotizacionDashCodigo(string codigoCotizacion)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Dashboard_CargarCotizacionCodigo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigoCotizacion", codigoCotizacion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoDetalleCotiDash.DataSource = null;
            datalistadoDetalleCotiDash.DataSource = dt;
            con.Close();
        }

        //PEDIDO
        //CARGA DETALLES DE MI COTIZACION CODIGOO
        public void CargarPedidoDash(string codigoCotizacion)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Dashboard_CargarDetallesPedido", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigoCotizacion", codigoCotizacion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoDetallePedidoDash.DataSource = null;
            datalistadoDetallePedidoDash.DataSource = dt;
            con.Close();
        }

        //CARGA DETALLES DE MI PEDIDO
        public void CargarPedidoDashCodigo(string codigoPedido)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Dashboard_CargarPedidoCodigo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigoPedido", codigoPedido);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoDetallePedidoDash.DataSource = null;
            datalistadoDetallePedidoDash.DataSource = dt;
            con.Close();
        }

        //ORDEN DE PRODUCCION
        //CARGA DETALLES DE MI COTIZACION CODIGOO
        public void CargarOrdenProduccionDash(string codigoPedido)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Dashboard_CargarDetallesOP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigoPedido", codigoPedido);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoOrdenProduccionDash.DataSource = null;
            datalistadoOrdenProduccionDash.DataSource = dt;
            con.Close();
        }

        //CARGA DETALLES DE MI PEDIDO
        public void CargarOrdenProduccionDashCodigo(string codigoOP)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Dashboard_CargarOPCodigo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigoOP", codigoOP);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoOrdenProduccionDash.DataSource = null;
            datalistadoOrdenProduccionDash.DataSource = dt;
            con.Close();
        }
        //------------------------------------------------------------------------------------------------------------------

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
            MostrarPedidoCliente(txtBusqueda.Text, DesdeFecha.Value, HastaFecha.Value);
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

        //ABRIR Y VISUALIZAR MI OC
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

        //PRODEDIMEINTO PARA ANULAR MI PEDIDO
        private void btnAnularPedido_Click(object sender, EventArgs e)
        {
            txtJustificacionAnulacion.Text = "";
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
                            cmd = new SqlCommand("Pedido_Anular", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idPedido", idPedido);
                            cmd.Parameters.AddWithValue("@idCotizacion", idCotizacion);
                            cmd.Parameters.AddWithValue("@mensajeAnulado", txtJustificacionAnulacion.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Pedido y cotización asociado a esta, anuladas exitosamente.", "Validación del Sistema");
                            MostrarPedidoPorFecha(DesdeFecha.Value, HastaFecha.Value);


                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }

                panleAnulacion.Visible = false;
                txtJustificacionAnulacion.Text = "";
                datalistadoTodasPedido.Enabled = true;
            }
            else
            {
                MessageBox.Show("Debe seleccionar un pedido para poder anularlo.", "Validación del Sistema");
            }
        }

        //BOTON PARA RETROCEDER DE LA ANULACION
        private void btnRetrocederAnulacion_Click(object sender, EventArgs e)
        {
            txtJustificacionAnulacion.Text = "";
            panleAnulacion.Visible = false;
            datalistadoTodasPedido.Enabled = true;
        }

        //FUNCION DE EDICION DE UNA ORDEN DE COMPRA-----------------------------------------------------------------
        //FUNCION PARA ACTIVAR MI EDICION DE ORDEN DE COMPRA DE PEDIUDO
        private void btnEditarPedido_Click(object sender, EventArgs e)
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
                    cmd = new SqlCommand("Pedido_ModificarOrdenCompra", con);
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

        //LIMPIAR CAMPOS DE EDICION DE ORDEN DE COMPRA
        public void LimpiarEdicionOrdenCompra()
        {
            txtRutaOrdenCompraModi.Text = "";
            txtCodigoOrdenCompraModi.Text = "";
        }

        //SELECCIONAR UN TIPO DE VISUALIZACION
        private void cboTipoVisualizacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoVisualizacion.Text == "DETALLADA")
            {
                panelVisualizacionClaseca.Visible = true;
                panelVisualizacionNueva.Visible = false;
            }
            else
            {
                panelVisualizacionClaseca.Visible = false;
                panelVisualizacionNueva.Visible = true;
            }
        }

        //SALIR DEL VISUALIZADOR
        private void btnSalirSeguimiento_Click(object sender, EventArgs e)
        {
            panelDetalleOP.Visible = false;
        }

        //SALIR DEL VISUALIZADOR
        private void btnSalirSeguimiento2_Click(object sender, EventArgs e)
        {
            panelDetalleOP.Visible = false;
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

        //FUNCION PARA ABRIR EL MANUAL DE USUARIO
        private void btnInfoEdicionOrdenCOmpra_Click(object sender, EventArgs e)
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
                int codigoPedido = Convert.ToInt32(datalistadoTodasPedido.SelectedCells[2].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
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

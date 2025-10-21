using ArenasProyect3.Modulos.ManGeneral;
using ArenasProyect3.Modulos.Resourses;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Office2013.Word;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text;
using iTextSharp.text.pdf;
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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Document = iTextSharp.text.Document;

namespace ArenasProyect3.Modulos.Comercial.Ventas
{
    public partial class Cotizacion : Form
    {
        //COMENTARIO AGREGADO PARA VLAIDACION DE LA HERRAMIENTA GITHUB
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        private Cursor curAnterior = null;
        string ruta = ManGeneral.Manual.manualAreaComercial;

        int CodigoCLiente = 0;
        int codigoCotizacion;
        string CodigoGeneradoCotizacion = "";
        string CodigoGeneradoPedido = "";
        string detalleProducido = "";

        DataView dv;
        DataSet ds = new DataSet();

        //CONSTRUCTOR DEL MANTENIMIENTO - COTIZACION
        public Cotizacion()
        {
            InitializeComponent();
        }

        //INICIO Y CARGA INICIAL DE COTIZACIONES - CONSTRUCTOR--------------------------------------------------------------------------------------
        private void Cotizacion_Load(object sender, EventArgs e)
        {
            //AJUSTAR FECHAS AL INICIO DEL MES Y FINAL DEL MES
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);
            //ASIGNARLE LAS VARIABLES YA CARGADAS A MIS DateTimerPicker
            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;

            //BLOQUEAR MI LISTADO PARA EVITAR MALAS CARGAS Y CARGAS DE DATOS
            datalistadoTodasCotiaciones.DataSource = null;
            datalistadoTodasCotiacionesPendientes.DataSource = null;
            datalistadoTodasCotiacionesParcial.DataSource = null;
            datalistadoTodasCotiacionesCompletado.DataSource = null;
            datalistadoTodasCotiacionesVencidos.DataSource = null;
            cboBusqeudaClienteResponsable.SelectedIndex = 0;

            //PREFILES Y PERSIMOS------------------------------------------------------------------------------------------------------------------
            //SI EL USUARIO TIENE UN RANGO DE EFECTO DE 1 (JEFATURA DEL ÁREA COMERCIAL)
            if (Program.RangoEfecto != 1)
            {
                //BOTÓN Y LEYENDA DE ANULACIÓN DE COTIZACIONES - ACCIÓN PARA QUE APAREZCA Y DESAPAREZCA
                btnAnularCotizacion.Visible = false;
                lblLeyendaAnularCotizacion.Visible = false;
            }
            else
            {
                //BOTÓN Y LEYENDA DE ANULACIÓN DE COTIZACIONES - ACCIÓN PARA QUE APAREZCA Y DESAPAREZCA
                btnAnularCotizacion.Visible = true;
                lblLeyendaAnularCotizacion.Visible = true;
            }
        }

        //VIZUALIZAR DATOS EXCEL--------------------------------------------------------------------
        public void MostrarExcel()
        {
            datalistadoExcel.Rows.Clear();

            foreach (DataGridViewRow dgv in datalistadoTodasCotiaciones.Rows)
            {
                string numeroCoti = dgv.Cells[2].Value.ToString();
                string fechaCoti = dgv.Cells[3].Value.ToString();
                string fechaValidez = dgv.Cells[4].Value.ToString();
                string cliente = dgv.Cells[6].Value.ToString();
                string unidad = dgv.Cells[8].Value.ToString();
                string responsable = dgv.Cells[10].Value.ToString();
                string cotizador = dgv.Cells[12].Value.ToString();
                string tipoMoneda = dgv.Cells[14].Value.ToString();
                string referencia = dgv.Cells[15].Value.ToString();
                string subTotal = dgv.Cells[21].Value.ToString();
                string descuento = dgv.Cells[22].Value.ToString();
                string inafecta = dgv.Cells[23].Value.ToString();
                string exonetado = dgv.Cells[24].Value.ToString();
                string igv = dgv.Cells[25].Value.ToString();
                string totalDescuento = dgv.Cells[26].Value.ToString();
                string total = dgv.Cells[27].Value.ToString();
                string estado = dgv.Cells[33].Value.ToString();

                datalistadoExcel.Rows.Add(new[] { numeroCoti, fechaCoti, fechaValidez, cliente, unidad, responsable, cotizador, tipoMoneda, referencia, subTotal, descuento, inafecta, exonetado, igv, totalDescuento, total, estado });
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------

        //CARGA DE COMBOS PARA LA GENERACIÓN DE LAS COTIZACIONES----------------------------------------------------------------------------
        //MOSTRR LAS MATERIAS DE MI FORMUALCION - DETALLES---------------------------------
        //public void MostrarFormulacionesDetalleTodos(string detalle)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        SqlConnection con = new SqlConnection();
        //        con.ConnectionString = Conexion.ConexionMaestra.conexion;
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand();
        //        cmd = new SqlCommand("Cotizacion_MostrarTodasFormulaciones_DetalleTodo", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@detalle", detalle);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        datalistadoFormulacionesDetalle.DataSource = dt;
        //        con.Close();
        //        //REDIMENSIONAR LAS COLUMNAS SEGUN EL TEMAÑO REQUERIDO
        //        datalistadoFormulacionesDetalle.Columns[0].Width = 110;
        //        datalistadoFormulacionesDetalle.Columns[1].Width = 110;
        //        datalistadoFormulacionesDetalle.Columns[2].Width = 430;
        //        datalistadoFormulacionesDetalle.Columns[3].Width = 110;
        //        datalistadoFormulacionesDetalle.Columns[4].Width = 110;
        //        datalistadoFormulacionesDetalle.Columns[5].Width = 430;
        //        datalistadoFormulacionesDetalle.Columns[6].Width = 110;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error del sistema", "Validación del Sistema", MessageBoxButtons.OK);
        //    }
        //}

        //MOSTRAR MIS FORMULACIONES RELACIONADAS A MI PRODUCTO
        public void MostrarFormulacionesDetalle(string detalle)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Cotizacion_MostrarFormulaciones_Detalle", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@detalle", detalle);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoFormulacionesDetalle.DataSource = dt;
                con.Close();
                //REDIMENSIONAR LAS COLUMNAS SEGUN EL TEMAÑO REQUERIDO
                datalistadoFormulacionesDetalle.Columns[0].Width = 110;
                datalistadoFormulacionesDetalle.Columns[1].Width = 110;
                datalistadoFormulacionesDetalle.Columns[2].Width = 430;
                datalistadoFormulacionesDetalle.Columns[3].Width = 110;
                datalistadoFormulacionesDetalle.Columns[4].Width = 110;
                datalistadoFormulacionesDetalle.Columns[5].Width = 430;
                datalistadoFormulacionesDetalle.Columns[6].Width = 110;
                alternarColorFilas(datalistadoFormulacionesDetalle);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema.", "Validación del Sistema", MessageBoxButtons.OK);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //MOSTRAR LOS MTERIALES DE MI PRODUCTO Y SEMIPRODUCIDO DE MANERA GENERAL DE MI FORMUALCION 
        public void MostrarFormulacionesDetalle2(string detalle)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Cotizacion_MostrarFormulaciones_Detalle2", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigoFormulacion", detalle);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoFormulacionesDetalle2.DataSource = dt;
                con.Close();
                //REDIMENSIONAR LAS COLUMNAS SEGUN EL TEMAÑO REQUERIDO
                datalistadoFormulacionesDetalle2.Columns[0].Width = 100;
                datalistadoFormulacionesDetalle2.Columns[1].Width = 693;
                datalistadoFormulacionesDetalle2.Columns[2].Width = 110;
                datalistadoFormulacionesDetalle2.Columns[3].Width = 90;
                alternarColorFilas(datalistadoFormulacionesDetalle2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema", "Validación del Sistema", MessageBoxButtons.OK);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //BUSCAR DATOS DE LA COTIZACIÓN POR EL CÓDIGO
        public void BuscarCotizacionPorCodigo(int codigo)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Cotizacion_MostrarPorCodigo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigo", codigo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dataListadiCotiXCodigo.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema", "Validación del Sistema", MessageBoxButtons.OK);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //BUSCAR DATOS DE LA COTIZACIÓN POR EL CÓDIGO
        public void BuscarCotizacionDetallePorCodigo(int codigo)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Cotizacion_MostrarTodosDatosDetalleSegunCodigo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigoD", codigo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dataListadiCotiDetallesXCodigo.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema", "Validación del Sistema", MessageBoxButtons.OK);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //BUSCAR DATOS DE LA COTIZACIÓN POR EL CÓDIGO FILTRADO POR ITEMS ADJUDICADOS
        public void BuscarCotizacionDetallePorCodigoAdjudicado(int codigo)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Cotizacion_MostrarTodosDatosDetalleSegunCodigoAdjudicado", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigoD", codigo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dataListadiCotiDetallesXCodigoAdjudicado.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema", "Validación del Sistema", MessageBoxButtons.OK);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //BUSCAR DATOS DE LA COTIZACIÓN POR EL CÓDIGO DETALLE FILTRADO POR ITEMS ADJUDICADOS
        public void BuscarCotizacionDetallePorCodigoDetalleAdjudicado(int codigo)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Cotizacion_MostrarTodosDatosDetalleSegunCodigoItemAdjudicado", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigoD", codigo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dataListadiCotiDetallesXCodigoAdjudicado.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema", "Validación del Sistema", MessageBoxButtons.OK);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //VER DETALLES (ITEMS) DE MI COTIZACION
        public void MostrarItemsSegunCotizacion(int idcotizacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Cotizacion_MostrarItems", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCotizacion", idcotizacion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoItemsCotizacion.DataSource = dt;
                con.Close();
                datalistadoItemsCotizacion.Columns[0].Visible = false;
                datalistadoItemsCotizacion.Columns[1].ReadOnly = true;
                datalistadoItemsCotizacion.Columns[2].ReadOnly = true;
                datalistadoItemsCotizacion.Columns[3].ReadOnly = true;
                datalistadoItemsCotizacion.Columns[4].ReadOnly = true;
                datalistadoItemsCotizacion.Columns[5].ReadOnly = true;
                datalistadoItemsCotizacion.Columns[6].ReadOnly = true;
                datalistadoItemsCotizacion.Columns[8].ReadOnly = true;

                datalistadoItemsCotizacion.Columns[1].Width = 90;
                datalistadoItemsCotizacion.Columns[2].Width = 440;
                datalistadoItemsCotizacion.Columns[3].Width = 70;
                datalistadoItemsCotizacion.Columns[4].Width = 80;
                datalistadoItemsCotizacion.Columns[5].Width = 80;
                datalistadoItemsCotizacion.Columns[6].Width = 80;
                datalistadoItemsCotizacion.Columns[7].Width = 60;
                datalistadoItemsCotizacion.Columns[8].Width = 100;

                datalistadoItemsCotizacion.Columns[9].Visible = false;
                datalistadoItemsCotizacion.Columns[10].Visible = false;
                datalistadoItemsCotizacion.Columns[11].Visible = false;

                alternarColorFilas(datalistadoItemsCotizacion);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema", "Validación del Sistema", MessageBoxButtons.OK);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //BUSCAR SUCURSWALPOR CLIENTE
        public void BuscarSucursalesXCliente(int codigo)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Cotizacion_BuscarSucursalesXCliente", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idCliente", codigo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoSucursalesXCliente.DataSource = dt;
                con.Close();
                datalistadoSucursalesXCliente.Columns[0].Width = 80;
                datalistadoSucursalesXCliente.Columns[1].Width = 200;
                datalistadoSucursalesXCliente.Columns[2].Width = 200;
                datalistadoSucursalesXCliente.Columns[3].Width = 100;
                datalistadoSucursalesXCliente.Columns[4].Width = 100;
                datalistadoSucursalesXCliente.Columns[5].Width = 100;
                datalistadoSucursalesXCliente.Columns[6].Width = 100;
                datalistadoSucursalesXCliente.Columns[7].Width = 100;

                alternarColorFilas(datalistadoSucursalesXCliente);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema", "Validación del Sistema", MessageBoxButtons.OK);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------

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
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //CAPTURAR EL CODIGO DE LA COTIZACION
        public void CodigoCotizacion()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT IdCotizacion FROM Cotizacion WHERE IdCotizacion = (SELECT MAX(IdCotizacion) FROM Cotizacion)", con);
                da.Fill(dt);
                datalistadoCodigoCotizacion.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema.", "Validación del Sistema", MessageBoxButtons.OK);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        public void CodigoPedido()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT IdPedido FROM Pedido WHERE IdPedido = (SELECT MAX(IdPedido) FROM Pedido)", con);
                da.Fill(dt);
                datalistadoCodigoPedido.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema.", "Validación del Sistema", MessageBoxButtons.OK);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //CODIGO PARA GENERAR EL CODIGO DE COTIZACION
        public void CodigoGeneracionCotizacion()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT IdCotizacion FROM Cotizacion WHERE IdCotizacion = (SELECT MAX(IdCotizacion) FROM Cotizacion)", con);
                da.Fill(dt);
                datalistadoCodigoCotizacion.DataSource = dt;
                con.Close();

                string codigo = "";

                if (datalistadoCodigoCotizacion.Rows.Count == 0)
                {
                    codigo = "0";
                }
                else
                {
                    codigo = datalistadoCodigoCotizacion.SelectedCells[0].Value.ToString();
                }

                string anno = DateTime.Now.ToString("yyyy");

                if (codigo.Length == 1)
                {
                    int codigoS = Convert.ToInt32(codigo);
                    codigoS = codigoS + 1;
                    CodigoGeneradoCotizacion = anno + "0000" + Convert.ToString(codigoS);
                }
                else if (codigo.Length == 2)
                {
                    int codigoS = Convert.ToInt32(codigo);
                    codigoS = codigoS + 1;
                    CodigoGeneradoCotizacion = anno + "000" + Convert.ToString(codigoS);
                }
                else if (codigo.Length == 3)
                {
                    int codigoS = Convert.ToInt32(codigo);
                    codigoS = codigoS + 1;
                    CodigoGeneradoCotizacion = anno + "00" + Convert.ToString(codigoS);
                }
                else if (codigo.Length == 4)
                {
                    int codigoS = Convert.ToInt32(codigo);
                    codigoS = codigoS + 1;
                    CodigoGeneradoCotizacion = anno + "0" + Convert.ToString(codigoS);
                }
                else if (codigo.Length == 5)
                {
                    int codigoS = Convert.ToInt32(codigo);
                    codigoS = codigoS + 1;
                    CodigoGeneradoCotizacion = anno + Convert.ToString(codigoS);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema.", "Validación del Sistema", MessageBoxButtons.OK);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //CARGAR BROCHURES
        public void CargarBrochures()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdBrochures,Nombre,Ruta FROM Brochures WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboBrochures.ValueMember = "IdBrochures";
                cboBrochures.DisplayMember = "Nombre";
                cboBrochures.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema.", "Validación del Sistema", MessageBoxButtons.OK);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //LISTADO DE COTIZACIONES Y SELECCION DE DETALLES Y ESTADO DE COTIZACIONES---------------------
        //CARGAR TODAS LAS COTIZACIONES INGRESADAS
        public void CargarCotizaciones(DateTime fechaInicio, DateTime fechaTermino)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Cotizacion_MostrarPorFecha", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoTodasCotiaciones.DataSource = dt;
                con.Close();
                ReordenarFilasMostrarCotizacion(datalistadoTodasCotiaciones);

                DataTable dt2 = new DataTable();
                SqlConnection con2 = new SqlConnection();
                con2.ConnectionString = Conexion.ConexionMaestra.conexion;
                con2.Open();
                SqlCommand cmd2 = new SqlCommand();
                cmd2 = new SqlCommand("Cotizacion_MostrarPorFechaPorEstado", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd2.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                cmd2.Parameters.AddWithValue("@estado", 2);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                da2.Fill(dt2);
                datalistadoTodasCotiacionesPendientes.DataSource = dt2;
                con2.Close();
                ReordenarFilasMostrarCotizacion(datalistadoTodasCotiacionesPendientes);

                DataTable dt3 = new DataTable();
                SqlConnection con3 = new SqlConnection();
                con3.ConnectionString = Conexion.ConexionMaestra.conexion;
                con3.Open();
                SqlCommand cmd3 = new SqlCommand();
                cmd3 = new SqlCommand("Cotizacion_MostrarPorFechaPorEstado", con3);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd3.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                cmd3.Parameters.AddWithValue("@estado", 3);
                SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                da3.Fill(dt3);
                datalistadoTodasCotiacionesParcial.DataSource = dt3;
                con3.Close();
                ReordenarFilasMostrarCotizacion(datalistadoTodasCotiacionesParcial);

                DataTable dt4 = new DataTable();
                SqlConnection con4 = new SqlConnection();
                con4.ConnectionString = Conexion.ConexionMaestra.conexion;
                con4.Open();
                SqlCommand cmd4 = new SqlCommand();
                cmd4 = new SqlCommand("Cotizacion_MostrarPorFechaPorEstado", con4);
                cmd4.CommandType = CommandType.StoredProcedure;
                cmd4.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd4.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                cmd4.Parameters.AddWithValue("@estado", 4);
                SqlDataAdapter da4 = new SqlDataAdapter(cmd4);
                da4.Fill(dt4);
                datalistadoTodasCotiacionesCompletado.DataSource = dt4;
                con4.Close();
                ReordenarFilasMostrarCotizacion(datalistadoTodasCotiacionesCompletado);

                DataTable dt5 = new DataTable();
                SqlConnection con5 = new SqlConnection();
                con5.ConnectionString = Conexion.ConexionMaestra.conexion;
                con5.Open();
                SqlCommand cmd5 = new SqlCommand();
                cmd5 = new SqlCommand("Cotizacion_MostrarPorFechaPorEstado", con5);
                cmd5.CommandType = CommandType.StoredProcedure;
                cmd5.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd5.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                cmd5.Parameters.AddWithValue("@estado", 1);
                SqlDataAdapter da5 = new SqlDataAdapter(cmd5);
                da5.Fill(dt5);
                datalistadoTodasCotiacionesVencidos.DataSource = dt5;
                con5.Close();
                ReordenarFilasMostrarCotizacion(datalistadoTodasCotiacionesVencidos);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema.", "Validación del Sistema", MessageBoxButtons.OK);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //CARGAR TODAS LAS COTIZACIONES POR CLIENTE
        public void CargarCotizacionesPorClienteResponsable(DateTime fechaInicio, DateTime fechaTermino, string variable)
        {
            try
            {
                if (cboBusqeudaClienteResponsable.Text == "CLIENTE")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Cotizacion_MostrarPorCliente", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                    cmd.Parameters.AddWithValue("@cliente", variable);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoTodasCotiaciones.DataSource = dt;
                    con.Close();
                }
                else if (cboBusqeudaClienteResponsable.Text == "RESPONSABLE")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Cotizacion_MostrarPorResponsable", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                    cmd.Parameters.AddWithValue("@responsable", variable);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoTodasCotiaciones.DataSource = dt;
                    con.Close();
                }
                ReordenarFilasMostrarCotizacion(datalistadoTodasCotiaciones);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema.", "Validación del Sistema", MessageBoxButtons.OK);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        public void ReordenarFilasMostrarCotizacion(DataGridView DGV)
        {
            //REDIMENSIONAR LAS COLUMNAS SEGUN EL TEMAÑO REQUERIDO
            DGV.Columns[2].Width = 76;
            DGV.Columns[3].Width = 90;
            DGV.Columns[4].Width = 90;
            DGV.Columns[6].Width = 350;
            DGV.Columns[8].Width = 150;
            DGV.Columns[10].Width = 185;
            DGV.Columns[12].Width = 185;
            DGV.Columns[14].Width = 120;
            DGV.Columns[27].Width = 75;
            DGV.Columns[33].Width = 110;
            //NO MOSTRAR LAS COLUMNAS QUE NO SEAN DE REELEVANCIA PARA EL USUARIO
            DGV.Columns[1].Visible = false;
            DGV.Columns[5].Visible = false;
            DGV.Columns[7].Visible = false;
            DGV.Columns[9].Visible = false;
            DGV.Columns[11].Visible = false;
            DGV.Columns[13].Visible = false;
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
            DGV.Columns[28].Visible = false;
            DGV.Columns[29].Visible = false;
            DGV.Columns[30].Visible = false;
            DGV.Columns[31].Visible = false;
            DGV.Columns[32].Visible = false;
            DGV.Columns[34].Visible = false;
            DGV.Columns[35].Visible = false;

            //CARGAR EL MÉTODO QUE COLOREA LAS FILAS
            CargarColoresListadoCotizacionesGeneral(DGV);

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO Y VER SI ESTAN VENCIDOS
        public void CargarColoresListadoCotizacionesGeneral(DataGridView DGV)
        {
            try
            {
                //VARIABLE DE FECHA
                var DateAndTime = DateTime.Now;
                //RECORRER MI LISTADO PARA VALIDAR MIS COTIZACIONES, SI ESTAN VENCIDAS O NO
                foreach (DataGridViewRow datorecuperado in DGV.Rows)
                {
                    //RECUERAR LA FECHA Y EL CÓDIGO DE MI COTIZACIÓN
                    DateTime fechaValidez = Convert.ToDateTime(datorecuperado.Cells["FECHA DE VALIDEZ"].Value);
                    int codigoCoti = Convert.ToInt32(datorecuperado.Cells["ID"].Value);
                    string estadoCoti = Convert.ToString(datorecuperado.Cells["ESTADO COTIZACIÓN"].Value);
                    //SI LA FECHA DE VALIDEZ ES MAYOR A LA FECHA ACTUAL CONSULTADA
                    if (estadoCoti == "PENDIENTE")
                    {
                        if (fechaValidez < DateAndTime)
                        {
                            //CAMBIAR EL ESTADO DE MI COTIZACIÓN
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("Cotizacion_CambiarEstado", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idCoti", codigoCoti);
                            cmd.Parameters.AddWithValue("@estadoCoti", 1);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }

                //RECORRIDO DE MI LISTADO
                foreach (DataGridViewRow datorecuperado in DGV.Rows)
                {
                    //RECUPERAR EL VALOR DEL ESTADO DE MI COTIZACIÓN
                    int estadoItems = Convert.ToInt32(datorecuperado.Cells["ESTADOCOTI"].Value);

                    //SI MI COTIZACIÓN ESTA EN ESTADO 1
                    if (estadoItems == 1)
                    {
                        //VENCIDO -> PLOMO
                        datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Fuchsia;
                    }
                    //SI MI COTIZACIÓN ESTA EN ESTADO 2
                    else if (estadoItems == 2)
                    {
                        //PENDIENTE -> NEGRO
                        datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    //SI MI COTIZACIÓN ESTA EN ESTADO 3
                    else if (estadoItems == 3)
                    {
                        //PARCIAL -> AMARILLO
                        datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Goldenrod;
                    }
                    //SI MI COTIZACIÓN ESTA EN ESTADO 4
                    else if (estadoItems == 4)
                    {
                        //COMPLETADO -> VERDE
                        datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                    }
                    //SI MI COTIZACIÓN ESTA EN ESTADO 0
                    else if (estadoItems == 0)
                    {
                        //ANULADO -> ROJO
                        datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        MessageBox.Show("Error al cargar los datos.", "Validación del Sistema", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //BÚSQYEDA DE COTIZACIONES POR RESPONSABLE - CLIENTE
        private void txtBusquedaClienteCotizacion_TextChanged(object sender, EventArgs e)
        {
            CargarCotizacionesPorClienteResponsable(DesdeFecha.Value, HastaFecha.Value, txtBusquedaClienteCotizacion.Text);
        }

        //BÚSQYEDA DE COTIZACIONES POR FECHA
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            CargarCotizaciones(DesdeFecha.Value, HastaFecha.Value);
        }

        //BÚSQYEDA DE COTIZACIONES POR FECHA
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            CargarCotizaciones(DesdeFecha.Value, HastaFecha.Value);
        }

        //BÚSQUEDA DE COTIZACIONES POR FECHAS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            CargarCotizaciones(DesdeFecha.Value, HastaFecha.Value);
        }

        //CAMBIO DE CIRTERIO DE BUSQUEDA DE COTIZACIONES
        private void cboBusqeudaClienteResponsable_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusquedaClienteCotizacion.Text = "";
        }

        //ABERTURA DE LOS DETALLES DE TODOS MIS LISTADOS---------------------------------------------------------------------------------------------
        private void datalistadoTodasCotiaciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoTodasCotiaciones.RowCount != 0)
            {
                DataGridViewColumn currentColumnT = datalistadoTodasCotiaciones.Columns[e.ColumnIndex];

                if (currentColumnT.Name == "detalles")
                {
                    if (datalistadoTodasCotiaciones.SelectedCells[33].Value.ToString() == "FUERA DE FECHA" || datalistadoTodasCotiaciones.SelectedCells[33].Value.ToString() == "ANULADO")
                    {
                        MessageBox.Show("No se puede visualizar los detalles de esta cotización.", "Validación del Sistema");
                    }
                    else
                    {
                        //CAPTURAR EL CÓDIGO DE COTIZACIÓN Y FILA DE MI LISTADO
                        codigoCotizacion = Convert.ToInt32(datalistadoTodasCotiaciones.SelectedCells[1].Value.ToString());
                        DataGridViewColumn currentColumn = datalistadoTodasCotiaciones.Columns[e.ColumnIndex];

                        //ABRIR MI PANEL DE DETALLES
                        panelDetalleitemsCotizacion.Visible = true;
                        //COLOCAR EL CÓDIGO DE MI COTIZACIÓN EN LA CAJA DEL PANEL DE DETALLES
                        txtCodigoCotizacion.Text = datalistadoTodasCotiaciones.SelectedCells[2].Value.ToString();
                        //CARGAR LOS ITEMS DEL DETALLE A MI LISTADO
                        MostrarItemsSegunCotizacion(codigoCotizacion);
                        //RECORRER MI LISTADO DE ITEMS DE MI COTIZACIÓN
                        foreach (DataGridViewRow datorecuperado in datalistadoItemsCotizacion.Rows)
                        {
                            //CAPTURAR EL ESTADO DE MIS ITEMS DE MI COTIZACIÓN
                            bool estadoItems = Convert.ToBoolean(datorecuperado.Cells["ESTADO ITEM"].Value);
                            //SI MI ESTADO ES IGUAL FALSE
                            if (estadoItems == false)
                            {
                                //SI MI ESTADO ES FALSE -> EL COLOR DE MI FILA ES NEGRO
                                datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                //SI MI ESTADO ES TRUE U OTRO -> EL COLOR DE MI FILA ES VERDE
                                datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay ninguna cotización para visualizar, por favor filtre por algún criterio contemplado.", "Validación del Sistema");
            }
        }

        //INGRESAR AL DETALLE DE MI COTIZAICON
        private void datalistadoTodasCotiacionesPendientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoTodasCotiacionesPendientes.RowCount != 0)
            {
                DataGridViewColumn currentColumnT = datalistadoTodasCotiacionesPendientes.Columns[e.ColumnIndex];

                if (currentColumnT.Name == "detalles2")
                {
                    if (datalistadoTodasCotiacionesPendientes.SelectedCells[33].Value.ToString() == "FUERA DE FECHA" || datalistadoTodasCotiacionesPendientes.SelectedCells[33].Value.ToString() == "ANULADO")
                    {
                        MessageBox.Show("No se puede visualizar los detalles de esta cotización.", "Validación del Sistema");
                    }
                    else
                    {
                        //CAPTURAR EL CÓDIGO DE COTIZACIÓN Y FILA DE MI LISTADO
                        codigoCotizacion = Convert.ToInt32(datalistadoTodasCotiacionesPendientes.SelectedCells[1].Value.ToString());
                        DataGridViewColumn currentColumn = datalistadoTodasCotiacionesPendientes.Columns[e.ColumnIndex];

                        //ABRIR MI PANEL DE DETALLES
                        panelDetalleitemsCotizacion.Visible = true;
                        //COLOCAR EL CÓDIGO DE MI COTIZACIÓN EN LA CAJA DEL PANEL DE DETALLES
                        txtCodigoCotizacion.Text = datalistadoTodasCotiacionesPendientes.SelectedCells[2].Value.ToString();
                        //CARGAR LOS ITEMS DEL DETALLE A MI LISTADO
                        MostrarItemsSegunCotizacion(codigoCotizacion);
                        //RECORRER MI LISTADO DE ITEMS DE MI COTIZACIÓN
                        foreach (DataGridViewRow datorecuperado in datalistadoItemsCotizacion.Rows)
                        {
                            //CAPTURAR EL ESTADO DE MIS ITEMS DE MI COTIZACIÓN
                            bool estadoItems = Convert.ToBoolean(datorecuperado.Cells["ESTADO ITEM"].Value);
                            //SI MI ESTADO ES IGUAL FALSE
                            if (estadoItems == false)
                            {
                                //SI MI ESTADO ES FALSE -> EL COLOR DE MI FILA ES NEGRO
                                datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                //SI MI ESTADO ES TRUE U OTRO -> EL COLOR DE MI FILA ES VERDE
                                datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                            }
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("No hay ninguna cotización para visualizar, por favor filtre por algún criterio contemplado.", "Validación del Sistema");
            }
        }

        private void datalistadoTodasCotiacionesParcial_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoTodasCotiacionesParcial.RowCount != 0)
            {
                DataGridViewColumn currentColumnT = datalistadoTodasCotiacionesParcial.Columns[e.ColumnIndex];

                if (currentColumnT.Name == "detalles3")
                {
                    if (datalistadoTodasCotiacionesParcial.SelectedCells[33].Value.ToString() == "FUERA DE FECHA" || datalistadoTodasCotiacionesParcial.SelectedCells[33].Value.ToString() == "ANULADO")
                    {
                        MessageBox.Show("No se puede visualizar los detalles de esta cotización.", "Validación del Sistema");
                    }
                    else
                    {
                        //CAPTURAR EL CÓDIGO DE COTIZACIÓN Y FILA DE MI LISTADO
                        codigoCotizacion = Convert.ToInt32(datalistadoTodasCotiacionesParcial.SelectedCells[1].Value.ToString());
                        DataGridViewColumn currentColumn = datalistadoTodasCotiacionesParcial.Columns[e.ColumnIndex];

                        //ABRIR MI PANEL DE DETALLES
                        panelDetalleitemsCotizacion.Visible = true;
                        //COLOCAR EL CÓDIGO DE MI COTIZACIÓN EN LA CAJA DEL PANEL DE DETALLES
                        txtCodigoCotizacion.Text = datalistadoTodasCotiacionesParcial.SelectedCells[2].Value.ToString();
                        //CARGAR LOS ITEMS DEL DETALLE A MI LISTADO
                        MostrarItemsSegunCotizacion(codigoCotizacion);
                        //RECORRER MI LISTADO DE ITEMS DE MI COTIZACIÓN
                        foreach (DataGridViewRow datorecuperado in datalistadoItemsCotizacion.Rows)
                        {
                            //CAPTURAR EL ESTADO DE MIS ITEMS DE MI COTIZACIÓN
                            bool estadoItems = Convert.ToBoolean(datorecuperado.Cells["ESTADO ITEM"].Value);
                            //SI MI ESTADO ES IGUAL FALSE
                            if (estadoItems == false)
                            {
                                //SI MI ESTADO ES FALSE -> EL COLOR DE MI FILA ES NEGRO
                                datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                //SI MI ESTADO ES TRUE U OTRO -> EL COLOR DE MI FILA ES VERDE
                                datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                            }
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("No hay ninguna cotización para visualizar, por favor filtre por algún criterio contemplado.", "Validación del Sistema");
            }
        }

        private void datalistadoTodasCotiacionesCompletado_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoTodasCotiacionesCompletado.RowCount != 0)
            {
                DataGridViewColumn currentColumnT = datalistadoTodasCotiacionesCompletado.Columns[e.ColumnIndex];

                if (currentColumnT.Name == "detalles4")
                {
                    if (datalistadoTodasCotiacionesCompletado.SelectedCells[33].Value.ToString() == "FUERA DE FECHA" || datalistadoTodasCotiacionesCompletado.SelectedCells[33].Value.ToString() == "ANULADO")
                    {
                        MessageBox.Show("No se puede visualizar los detalles de esta cotización.", "Validación del Sistema");
                    }
                    else
                    {
                        //CAPTURAR EL CÓDIGO DE COTIZACIÓN Y FILA DE MI LISTADO
                        codigoCotizacion = Convert.ToInt32(datalistadoTodasCotiacionesCompletado.SelectedCells[1].Value.ToString());
                        DataGridViewColumn currentColumn = datalistadoTodasCotiacionesCompletado.Columns[e.ColumnIndex];

                        //ABRIR MI PANEL DE DETALLES
                        panelDetalleitemsCotizacion.Visible = true;
                        //COLOCAR EL CÓDIGO DE MI COTIZACIÓN EN LA CAJA DEL PANEL DE DETALLES
                        txtCodigoCotizacion.Text = datalistadoTodasCotiacionesCompletado.SelectedCells[2].Value.ToString();
                        //CARGAR LOS ITEMS DEL DETALLE A MI LISTADO
                        MostrarItemsSegunCotizacion(codigoCotizacion);
                        //RECORRER MI LISTADO DE ITEMS DE MI COTIZACIÓN
                        foreach (DataGridViewRow datorecuperado in datalistadoItemsCotizacion.Rows)
                        {
                            //CAPTURAR EL ESTADO DE MIS ITEMS DE MI COTIZACIÓN
                            bool estadoItems = Convert.ToBoolean(datorecuperado.Cells["ESTADO ITEM"].Value);
                            //SI MI ESTADO ES IGUAL FALSE
                            if (estadoItems == false)
                            {
                                //SI MI ESTADO ES FALSE -> EL COLOR DE MI FILA ES NEGRO
                                datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                //SI MI ESTADO ES TRUE U OTRO -> EL COLOR DE MI FILA ES VERDE
                                datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                            }
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("No hay ninguna cotización para visualizar, por favor filtre por algún criterio contemplado.", "Validación del Sistema");
            }
        }

        private void datalistadoTodasCotiacionesVencidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoTodasCotiacionesVencidos.RowCount != 0)
            {
                DataGridViewColumn currentColumnT = datalistadoTodasCotiacionesVencidos.Columns[e.ColumnIndex];

                if (currentColumnT.Name == "detalles5")
                {
                    if (datalistadoTodasCotiacionesVencidos.SelectedCells[33].Value.ToString() == "FUERA DE FECHA" || datalistadoTodasCotiacionesVencidos.SelectedCells[33].Value.ToString() == "ANULADO")
                    {
                        MessageBox.Show("No se puede visualizar los detalles de esta cotización.", "Validación del Sistema");
                    }
                    else
                    {
                        //CAPTURAR EL CÓDIGO DE COTIZACIÓN Y FILA DE MI LISTADO
                        codigoCotizacion = Convert.ToInt32(datalistadoTodasCotiacionesVencidos.SelectedCells[1].Value.ToString());
                        DataGridViewColumn currentColumn = datalistadoTodasCotiacionesVencidos.Columns[e.ColumnIndex];

                        //ABRIR MI PANEL DE DETALLES
                        panelDetalleitemsCotizacion.Visible = true;
                        //COLOCAR EL CÓDIGO DE MI COTIZACIÓN EN LA CAJA DEL PANEL DE DETALLES
                        txtCodigoCotizacion.Text = datalistadoTodasCotiacionesVencidos.SelectedCells[2].Value.ToString();
                        //CARGAR LOS ITEMS DEL DETALLE A MI LISTADO
                        MostrarItemsSegunCotizacion(codigoCotizacion);
                        //RECORRER MI LISTADO DE ITEMS DE MI COTIZACIÓN
                        foreach (DataGridViewRow datorecuperado in datalistadoItemsCotizacion.Rows)
                        {
                            //CAPTURAR EL ESTADO DE MIS ITEMS DE MI COTIZACIÓN
                            bool estadoItems = Convert.ToBoolean(datorecuperado.Cells["ESTADO ITEM"].Value);
                            //SI MI ESTADO ES IGUAL FALSE
                            if (estadoItems == false)
                            {
                                //SI MI ESTADO ES FALSE -> EL COLOR DE MI FILA ES NEGRO
                                datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                            }
                            else
                            {
                                //SI MI ESTADO ES TRUE U OTRO -> EL COLOR DE MI FILA ES VERDE
                                datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                            }
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("No hay ninguna cotización para visualizar, por favor filtre por algún criterio contemplado.", "Validación del Sistema");
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------

        //HACER DOBLE CLICK Y VISUALIZAR LOS ITEMS DE MI COTIZACION----------------------------------------------------------------------------------
        private void datalistadoTodasCotiaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoTodasCotiaciones.SelectedCells[33].Value.ToString() == "FUERA DE FECHA" || datalistadoTodasCotiaciones.SelectedCells[33].Value.ToString() == "ANULADO")
            {
                MessageBox.Show("No se puede visualizar los detalles de esta cotización.", "Validación del Sistema");
            }
            else
            {
                //CAPTURAR EL CÓDIGO DE COTIZACIÓN Y FILA DE MI LISTADO
                codigoCotizacion = Convert.ToInt32(datalistadoTodasCotiaciones.SelectedCells[1].Value.ToString());
                DataGridViewColumn currentColumn = datalistadoTodasCotiaciones.Columns[e.ColumnIndex];

                //ABRIR MI PANEL DE DETALLES
                panelDetalleitemsCotizacion.Visible = true;
                //COLOCAR EL CÓDIGO DE MI COTIZACIÓN EN LA CAJA DEL PANEL DE DETALLES
                txtCodigoCotizacion.Text = datalistadoTodasCotiaciones.SelectedCells[2].Value.ToString();
                //CARGAR LOS ITEMS DEL DETALLE A MI LISTADO
                MostrarItemsSegunCotizacion(codigoCotizacion);
                //RECORRER MI LISTADO DE ITEMS DE MI COTIZACIÓN
                foreach (DataGridViewRow datorecuperado in datalistadoItemsCotizacion.Rows)
                {
                    //CAPTURAR EL ESTADO DE MIS ITEMS DE MI COTIZACIÓN
                    bool estadoItems = Convert.ToBoolean(datorecuperado.Cells["ESTADO ITEM"].Value);
                    //SI MI ESTADO ES IGUAL FALSE
                    if (estadoItems == false)
                    {
                        //SI MI ESTADO ES FALSE -> EL COLOR DE MI FILA ES NEGRO
                        datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        //SI MI ESTADO ES TRUE U OTRO -> EL COLOR DE MI FILA ES VERDE
                        datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                    }
                }
            }
        }

        private void datalistadoTodasCotiacionesPendientes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoTodasCotiacionesPendientes.SelectedCells[33].Value.ToString() == "FUERA DE FECHA" || datalistadoTodasCotiacionesPendientes.SelectedCells[33].Value.ToString() == "ANULADO")
            {
                MessageBox.Show("No se puede visualizar los detalles de esta cotización.", "Validación del Sistema");
            }
            else
            {
                //CAPTURAR EL CÓDIGO DE COTIZACIÓN Y FILA DE MI LISTADO
                codigoCotizacion = Convert.ToInt32(datalistadoTodasCotiacionesPendientes.SelectedCells[1].Value.ToString());
                DataGridViewColumn currentColumn = datalistadoTodasCotiacionesPendientes.Columns[e.ColumnIndex];

                //ABRIR MI PANEL DE DETALLES
                panelDetalleitemsCotizacion.Visible = true;
                //COLOCAR EL CÓDIGO DE MI COTIZACIÓN EN LA CAJA DEL PANEL DE DETALLES
                txtCodigoCotizacion.Text = datalistadoTodasCotiacionesPendientes.SelectedCells[2].Value.ToString();
                //CARGAR LOS ITEMS DEL DETALLE A MI LISTADO
                MostrarItemsSegunCotizacion(codigoCotizacion);
                //RECORRER MI LISTADO DE ITEMS DE MI COTIZACIÓN
                foreach (DataGridViewRow datorecuperado in datalistadoItemsCotizacion.Rows)
                {
                    //CAPTURAR EL ESTADO DE MIS ITEMS DE MI COTIZACIÓN
                    bool estadoItems = Convert.ToBoolean(datorecuperado.Cells["ESTADO ITEM"].Value);
                    //SI MI ESTADO ES IGUAL FALSE
                    if (estadoItems == false)
                    {
                        //SI MI ESTADO ES FALSE -> EL COLOR DE MI FILA ES NEGRO
                        datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        //SI MI ESTADO ES TRUE U OTRO -> EL COLOR DE MI FILA ES VERDE
                        datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                    }
                }
            }
        }

        private void datalistadoTodasCotiacionesParcial_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoTodasCotiacionesParcial.SelectedCells[33].Value.ToString() == "FUERA DE FECHA" || datalistadoTodasCotiacionesParcial.SelectedCells[33].Value.ToString() == "ANULADO")
            {
                MessageBox.Show("No se puede visualizar los detalles de esta cotización.", "Validación del Sistema");
            }
            else
            {
                //CAPTURAR EL CÓDIGO DE COTIZACIÓN Y FILA DE MI LISTADO
                codigoCotizacion = Convert.ToInt32(datalistadoTodasCotiacionesParcial.SelectedCells[1].Value.ToString());
                DataGridViewColumn currentColumn = datalistadoTodasCotiacionesParcial.Columns[e.ColumnIndex];

                //ABRIR MI PANEL DE DETALLES
                panelDetalleitemsCotizacion.Visible = true;
                //COLOCAR EL CÓDIGO DE MI COTIZACIÓN EN LA CAJA DEL PANEL DE DETALLES
                txtCodigoCotizacion.Text = datalistadoTodasCotiacionesParcial.SelectedCells[2].Value.ToString();
                //CARGAR LOS ITEMS DEL DETALLE A MI LISTADO
                MostrarItemsSegunCotizacion(codigoCotizacion);
                //RECORRER MI LISTADO DE ITEMS DE MI COTIZACIÓN
                foreach (DataGridViewRow datorecuperado in datalistadoItemsCotizacion.Rows)
                {
                    //CAPTURAR EL ESTADO DE MIS ITEMS DE MI COTIZACIÓN
                    bool estadoItems = Convert.ToBoolean(datorecuperado.Cells["ESTADO ITEM"].Value);
                    //SI MI ESTADO ES IGUAL FALSE
                    if (estadoItems == false)
                    {
                        //SI MI ESTADO ES FALSE -> EL COLOR DE MI FILA ES NEGRO
                        datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        //SI MI ESTADO ES TRUE U OTRO -> EL COLOR DE MI FILA ES VERDE
                        datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                    }
                }
            }
        }

        private void datalistadoTodasCotiacionesCompletado_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoTodasCotiacionesCompletado.SelectedCells[33].Value.ToString() == "FUERA DE FECHA" || datalistadoTodasCotiacionesCompletado.SelectedCells[33].Value.ToString() == "ANULADO")
            {
                MessageBox.Show("No se puede visualizar los detalles de esta cotización.", "Validación del Sistema");
            }
            else
            {
                //CAPTURAR EL CÓDIGO DE COTIZACIÓN Y FILA DE MI LISTADO
                codigoCotizacion = Convert.ToInt32(datalistadoTodasCotiacionesCompletado.SelectedCells[1].Value.ToString());
                DataGridViewColumn currentColumn = datalistadoTodasCotiacionesCompletado.Columns[e.ColumnIndex];

                //ABRIR MI PANEL DE DETALLES
                panelDetalleitemsCotizacion.Visible = true;
                //COLOCAR EL CÓDIGO DE MI COTIZACIÓN EN LA CAJA DEL PANEL DE DETALLES
                txtCodigoCotizacion.Text = datalistadoTodasCotiacionesCompletado.SelectedCells[2].Value.ToString();
                //CARGAR LOS ITEMS DEL DETALLE A MI LISTADO
                MostrarItemsSegunCotizacion(codigoCotizacion);
                //RECORRER MI LISTADO DE ITEMS DE MI COTIZACIÓN
                foreach (DataGridViewRow datorecuperado in datalistadoItemsCotizacion.Rows)
                {
                    //CAPTURAR EL ESTADO DE MIS ITEMS DE MI COTIZACIÓN
                    bool estadoItems = Convert.ToBoolean(datorecuperado.Cells["ESTADO ITEM"].Value);
                    //SI MI ESTADO ES IGUAL FALSE
                    if (estadoItems == false)
                    {
                        //SI MI ESTADO ES FALSE -> EL COLOR DE MI FILA ES NEGRO
                        datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        //SI MI ESTADO ES TRUE U OTRO -> EL COLOR DE MI FILA ES VERDE
                        datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                    }
                }
            }
        }

        private void datalistadoTodasCotiacionesVencidos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoTodasCotiacionesVencidos.SelectedCells[33].Value.ToString() == "FUERA DE FECHA" || datalistadoTodasCotiacionesVencidos.SelectedCells[33].Value.ToString() == "ANULADO")
            {
                MessageBox.Show("No se puede visualizar los detalles de esta cotización.", "Validación del Sistema");
            }
            else
            {
                //CAPTURAR EL CÓDIGO DE COTIZACIÓN Y FILA DE MI LISTADO
                codigoCotizacion = Convert.ToInt32(datalistadoTodasCotiacionesVencidos.SelectedCells[1].Value.ToString());
                DataGridViewColumn currentColumn = datalistadoTodasCotiacionesVencidos.Columns[e.ColumnIndex];

                //ABRIR MI PANEL DE DETALLES
                panelDetalleitemsCotizacion.Visible = true;
                //COLOCAR EL CÓDIGO DE MI COTIZACIÓN EN LA CAJA DEL PANEL DE DETALLES
                txtCodigoCotizacion.Text = datalistadoTodasCotiacionesVencidos.SelectedCells[2].Value.ToString();
                //CARGAR LOS ITEMS DEL DETALLE A MI LISTADO
                MostrarItemsSegunCotizacion(codigoCotizacion);
                //RECORRER MI LISTADO DE ITEMS DE MI COTIZACIÓN
                foreach (DataGridViewRow datorecuperado in datalistadoItemsCotizacion.Rows)
                {
                    //CAPTURAR EL ESTADO DE MIS ITEMS DE MI COTIZACIÓN
                    bool estadoItems = Convert.ToBoolean(datorecuperado.Cells["ESTADO ITEM"].Value);
                    //SI MI ESTADO ES IGUAL FALSE
                    if (estadoItems == false)
                    {
                        //SI MI ESTADO ES FALSE -> EL COLOR DE MI FILA ES NEGRO
                        datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        //SI MI ESTADO ES TRUE U OTRO -> EL COLOR DE MI FILA ES VERDE
                        datorecuperado.DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN----------------------------------------------------------------------------------
        private void datalistadoTodasCotiaciones_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoTodasCotiaciones.Columns[e.ColumnIndex].Name == "detalles")
            {
                this.datalistadoTodasCotiaciones.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoTodasCotiaciones.Cursor = curAnterior;
            }
        }

        private void datalistadoTodasCotiacionesPendientes_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoTodasCotiacionesPendientes.Columns[e.ColumnIndex].Name == "detalles2")
            {
                this.datalistadoTodasCotiacionesPendientes.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoTodasCotiacionesPendientes.Cursor = curAnterior;
            }
        }

        private void datalistadoTodasCotiacionesParcial_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoTodasCotiacionesParcial.Columns[e.ColumnIndex].Name == "detalles3")
            {
                this.datalistadoTodasCotiacionesParcial.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoTodasCotiacionesParcial.Cursor = curAnterior;
            }
        }

        private void datalistadoTodasCotiacionesCompletado_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoTodasCotiacionesCompletado.Columns[e.ColumnIndex].Name == "detalles4")
            {
                this.datalistadoTodasCotiacionesCompletado.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoTodasCotiacionesCompletado.Cursor = curAnterior;
            }
        }

        private void datalistadoTodasCotiacionesVencidos_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoTodasCotiacionesVencidos.Columns[e.ColumnIndex].Name == "detalles5")
            {
                this.datalistadoTodasCotiacionesVencidos.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoTodasCotiacionesVencidos.Cursor = curAnterior;
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------

        //SALIR DE LOS DETALLES O ITEMS DE MI COTIZACION
        private void btnRegresarRegistroitemsCotizacion_Click(object sender, EventArgs e)
        {
            ////VARIABLES PARA LA VALIDACIÓN
            //int estadoModificacion = 0;
            //var DateAndTime = DateTime.Now;
            //bool vencido = false;

            //SqlConnection con = new SqlConnection();
            //SqlCommand cmd = new SqlCommand();
            //BuscarCotizacionPorCodigo(codigoCotizacion);
            panelDetalleitemsCotizacion.Visible = false;
            //estadoModificacion = Convert.ToInt16(dataListadiCotiXCodigo.SelectedCells[31].Value.ToString());
            //DateTime fechaValidez = Convert.ToDateTime(dataListadiCotiXCodigo.SelectedCells[3].Value);
            //int estadoFinal1 = 0;
            //int estadoFinal2 = 0;
            //List<int> estadoss = new List<int>();

            ////VALIDAR SI LA COTIZACION YA EXPIRO
            //if (fechaValidez < DateAndTime && datalistadoTodasCotiaciones.SelectedCells[33].Value.ToString() == "PENDIENTE")
            //{
            //    vencido = true;
            //}

            ////VALIDAR CUANTOS ITEMS SE HAN MARCADO
            //foreach (DataGridViewRow datorecuperado in datalistadoItemsCotizacion.Rows)
            //{
            //    int idItemCoti = Convert.ToInt32(datorecuperado.Cells["IdDetalleCotizacion"].Value);
            //    bool estadoItems = Convert.ToBoolean(datorecuperado.Cells["ESTADO ITEM"].Value);

            //    try
            //    {
            //        if (estadoItems == true)
            //        {
            //            estadoss.Add(1);

            //            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            //            con.Open();
            //            cmd = new SqlCommand("Cotizacion_CambiarEstadoItems", con);
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Parameters.AddWithValue("@idItemCoti", idItemCoti);
            //            cmd.Parameters.AddWithValue("@estado", 1);
            //            cmd.ExecuteNonQuery();
            //            con.Close();
            //        }
            //        else
            //        {
            //            estadoss.Add(0);

            //            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            //            con.Open();
            //            cmd = new SqlCommand("Cotizacion_CambiarEstadoItems", con);
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Parameters.AddWithValue("@idItemCoti", idItemCoti);
            //            cmd.Parameters.AddWithValue("@estado", 0);
            //            cmd.ExecuteNonQuery();
            //            con.Close();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Error en la operación por: " + ex.Message);
            //        ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            //    }
            //}

            ////HACER UNA SUMA SIMPLE CON LA CANTIDAD DE ITEMS MARCADOS
            //foreach (var n in estadoss)
            //{
            //    if (int.Equals(1, n))
            //    {
            //        estadoFinal1 = estadoFinal1 + 1;
            //    }

            //    if (int.Equals(0, n))
            //    {
            //        estadoFinal2 = estadoFinal2 + 1;
            //    }
            //}

            //try
            //{
            //    //COTIZACION VENCIDA
            //    if (vencido == true)
            //    {
            //        con.ConnectionString = Conexion.ConexionMaestra.conexion;
            //        con.Open();
            //        cmd = new SqlCommand("Cotizacion_CambiarEstadoCoti", con);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@idcoti", codigoCotizacion);
            //        cmd.Parameters.AddWithValue("@estadoCoti", 1);
            //        cmd.ExecuteNonQuery();
            //        con.Close();
            //    }
            //    //COTIZACION COMPLETA
            //    else if (estadoFinal1 > 0 && estadoFinal2 == 0 && vencido == false)
            //    {
            //        con.ConnectionString = Conexion.ConexionMaestra.conexion;
            //        con.Open();
            //        cmd = new SqlCommand("Cotizacion_CambiarEstadoCoti", con);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@idcoti", codigoCotizacion);
            //        cmd.Parameters.AddWithValue("@estadoCoti", 4);
            //        cmd.ExecuteNonQuery();
            //        con.Close();
            //    }
            //    //COTIZAXION CON PARTE DE LOS ITEMS PENDIENTES
            //    else if (estadoFinal1 > 0 && estadoFinal2 > 0 && vencido == false)
            //    {
            //        con.ConnectionString = Conexion.ConexionMaestra.conexion;
            //        con.Open();
            //        cmd = new SqlCommand("Cotizacion_CambiarEstadoCoti", con);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@idcoti", codigoCotizacion);
            //        cmd.Parameters.AddWithValue("@estadoCoti", 3);
            //        cmd.ExecuteNonQuery();
            //        con.Close();
            //    }
            //    //COTIZACION CON TODOS LOS ITEMS PENDIENTES
            //    else if (estadoFinal1 == 0 && vencido == false)
            //    {
            //        con.ConnectionString = Conexion.ConexionMaestra.conexion;
            //        con.Open();
            //        cmd = new SqlCommand("Cotizacion_CambiarEstadoCoti", con);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@idcoti", codigoCotizacion);
            //        cmd.Parameters.AddWithValue("@estadoCoti", 2);
            //        cmd.ExecuteNonQuery();
            //        con.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error en la operación por: " + ex.Message);
            //    ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            //}

            //CargarCotizaciones(DesdeFecha.Value, HastaFecha.Value);
        }

        //VALIDAR SI EL ITEM YA CUENTA CON PEDIDO CREADA, SI ES ASÍ NO SE VA A PODER MODIFICAR.
        private void datalistadoItemsCotizacion_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Verificar
            if (datalistadoItemsCotizacion.Rows[e.RowIndex].Cells[8].Value.ToString() == "PEDIDO GENERADO")
            {
                e.Cancel = true; // Cancelar la edición de la celda
            }
        }

        ////METODO QUE CAMBIA EL VALOR DEL VISIBLE
        //public void OcultarFormulaciones()
        //{
        //    try
        //    {
        //        int idFormulacion = Convert.ToInt32(datalistadoBusquedaFormulaciones.SelectedCells[7].Value.ToString());

        //        SqlConnection con = new SqlConnection();
        //        con.ConnectionString = Conexion.ConexionMaestra.conexion;
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand();
        //        cmd = new SqlCommand("OcultarFormulaciones", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@idFormulacion", idFormulacion);
        //        cmd.ExecuteNonQuery();
        //        con.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //ACCION PARA OCULTAR UNA FORMULACION
        private void btnOcultarFormulacion_Click(object sender, EventArgs e)
        {
            //int valorVisible = Convert.ToInt32(datalistadoBusquedaFormulaciones.SelectedCells[7].Value.ToString());

            //DialogResult boton = MessageBox.Show("¿Realmente desea ocultar esta formulación?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
            //if (boton == DialogResult.OK)
            //{
            //    if (valorVisible == 0)
            //    {
            //        MessageBox.Show("Esta formulación ya se encuentra oculta", "Validación del Sistema", MessageBoxButtons.OK);
            //    }
            //    else
            //    {
            //        OcultarFormulaciones();
            //    }

            //    if (cboFiltroFormulacion.Text == "FORM. HABILITADAS")
            //    {
            //        Filtro_MostrarTodasFormulacionesHabilitadas();
            //    }
            //    else
            //    {
            //        Filtro_MostrarTodasFormulaciones();
            //    }
            //}
        }

        //EVENTO PARA FILTRAR EL COMBO DE FORMULACION
        private void cboFiltroFormulacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            ////VERIFICACION DEL ESTADO DE MI CBOFILTRO
            //if (cboFiltroFormulacion.Text == "FORM. HABILITADAS")
            //{
            //    //SI SE QUIERE VISUALIZAR SOLO LAS FORMULACIONES HBAILITADAS
            //    Filtro_MostrarTodasFormulacionesHabilitadas();
            //    txtBusquedaFormulaciones.Text = "";
            //}
            //else
            //{
            //    //SI SE QUIERE VISUALIZAR TODAS LAS FORMULACIONES
            //    Filtro_MostrarTodasFormulaciones();
            //    txtBusquedaFormulaciones.Text = "";
            //}
        }

        //ACCIONES PARA CREAR UNA NUEVA COTIZACION--------------------------------------------
        //ABRIR VENTANA DE NUEVA COTIZACION
        private void btnNuevaCotizacion_Click(object sender, EventArgs e)
        {
            panelNuevaCotizacion.Visible = true;

            var DateAndTime = DateTime.Now;
            lblTituloCotizacion.Text = "NUEVA COTIZACIÓN";
            DateAndTime = DateAndTime.AddDays(+10);
            dateFechaValidez.Value = DateAndTime;
            dateFechaEmision.Value = DateTime.Now;

            LimpiarCotizacion();
            CargarBrochures();

            btnGuardarCotizacion.Visible = true;
            lblGuardar.Visible = true;
            btnEditarCotizacionAccion.Visible = false;
            lblEditar.Visible = false;
            datalistadoCotizacion.Columns[0].ReadOnly = true;
            datalistadoCotizacion.Columns[1].ReadOnly = true;
            datalistadoCotizacion.Columns[2].ReadOnly = true;
            datalistadoCotizacion.Columns[6].ReadOnly = true;
        }

        //CHECK PARA APLICAR BROCHURES
        private void ckAplicarBrochure_CheckedChanged(object sender, EventArgs e)
        {
            if (ckAplicarBrochure.Checked == true)
            {
                lblAplicaBrochure.Visible = true;
                CargarBrochures();
                cboBrochures.Visible = true;
            }
            else
            {
                lblAplicaBrochure.Visible = false;
                CargarBrochures();
                cboBrochures.Visible = false;
            }
        }

        //REGRESAR Y SALIR DE LA NUEVA COTIZACION
        private void btnRegresarNuevaCotizacion_Click(object sender, EventArgs e)
        {
            LimpiarNuevaCotizacion();
            panelNuevaCotizacion.Visible = false;
            datalistadoTodasCotiaciones.Enabled = true;
        }

        //ABRIR LOS DETALLES DE MI FORMULACION
        private void datalistadoCotizacion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoCotizacion.CurrentRow != null)
            {
                DataGridViewColumn currentColumn = datalistadoCotizacion.Columns[e.ColumnIndex];

                if (currentColumn.Name == "formulacion")
                {
                    panelDetalleFormulacion.Visible = true;

                    txtBusquedaFormulacionDetalle.Text = datalistadoCotizacion.Rows[datalistadoCotizacion.CurrentRow.Index].Cells[1].Value.ToString();
                    detalleProducido = datalistadoCotizacion.Rows[datalistadoCotizacion.CurrentRow.Index].Cells[1].Value.ToString();
                    MostrarFormulacionesDetalle(detalleProducido);
                }
            }
        }

        //SELECCIONAR Y VISUALIZAR LOS MATERIALES DE MI FORMULACION
        private void datalistadoFormulacionesDetalle_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string codigoformulacion = datalistadoFormulacionesDetalle.SelectedCells[0].Value.ToString();
            MostrarFormulacionesDetalle2(codigoformulacion);
        }

        //MOSTAR TODAS LAS FORMUALCIONES SN FILTRO
        private void btnMostrarTodasFormulaciones_Click(object sender, EventArgs e)
        {
            detalleProducido = datalistadoCotizacion.Rows[datalistadoCotizacion.CurrentRow.Index].Cells[1].Value.ToString();
            //MostrarFormulacionesDetalleTodos(detalleProducido);

            btnOcultarFormulaciones.Visible = false;
            lblLeyendaOcultar.Visible = false;
            btnHabilitarFormulaciones.Visible = true;
            lblLeyendaHabilitar.Visible = true;
        }

        //SALIR DE MI DETALLES DE FORMULACION
        private void btnRegresarDetallesFormulacion_Click(object sender, EventArgs e)
        {
            panelDetalleFormulacion.Visible = false;
            datalistadoFormulacionesDetalle2.DataSource = null;

            btnOcultarFormulaciones.Visible = true;
            lblLeyendaOcultar.Visible = true;
            btnHabilitarFormulaciones.Visible = false;
            lblLeyendaHabilitar.Visible = false;
        }

        //ABRIR LA VENTANA DE BSUAQUEDA DE CLIENTES
        private void txtBusquedaClientes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                cboTipoBusquedaClientes.SelectedIndex = 0;
                panelBusquedaClientes.Visible = true;
                txtBusquedaClientes2.Text = txtBusquedaClientes.Text;
                txtBusquedaClientes2.Focus();
            }
        }

        //POSISCIONARSE EN MI CAJA DE BÚISQUEDA
        private void cboTipoBusquedaClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusquedaClientes2.Text = "";
        }

        //CERRAR LA BUSQUEDA DE CLIENTES
        private void btnCerrarBusquedaCLiente_Click(object sender, EventArgs e)
        {
            panelBusquedaClientes.Visible = false;
            txtBusquedaClientes2.Text = "";
        }

        //SELECCIONAR AL CLIENTE PARA LA COTIZACION
        private void datalistadoclientes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            CodigoCLiente = Convert.ToInt32(datalistadoclientes.SelectedCells[30].Value.ToString());
            txtNombreCliente.Text = datalistadoclientes.SelectedCells[2].Value.ToString();
            txtDireccionClente.Text = datalistadoclientes.SelectedCells[21].Value.ToString();
            txtDocumentoCliente.Text = datalistadoclientes.SelectedCells[1].Value.ToString();
            panelBusquedaClientes.Visible = false;
            txtBusquedaClientes.Text = "";
            txtBusquedaClientes2.Text = "";
            txtLugarEntregado.Text = "Campo Opcional";

            CargarUnidad(CodigoCLiente, cboUnidadCliente);
            CargarResponsable(CodigoCLiente, cboResponsableCliente);
            CargarContacto(CodigoCLiente, cboContactoCliente);
            CargarCondicion(CodigoCLiente, cboCondicionPagoCliente);
            CargarForma(CodigoCLiente, cboFormaPagoCliente);
            CargarComercial(cboComercial);
            CargarMoneda(cboMoneda);

            //COLOCACION DE MONEDA
            if (cboMoneda.Text == "DOLARES AMERICANOS")
            {
                imgDolares.Visible = true;
                imgEuros.Visible = false;
                imgSoles.Visible = false;
            }
            else if (cboMoneda.Text == "EUROS")
            {
                imgDolares.Visible = false;
                imgEuros.Visible = true;
                imgSoles.Visible = false;
            }
            else if (cboMoneda.Text == "SOLES")
            {
                imgDolares.Visible = false;
                imgEuros.Visible = false;
                imgSoles.Visible = true;
            }

            CargarAlmacen(cboAlmacen);
        }

        //EVITAR PROBLEMAS CON LA GENERACION Y EDICION DE MI LISTADO
        private void datalistadoCotizacion_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        //CLEINTES SELECCIONA DEWNTRO DE COTIZACIONES----------------------------------------------
        //CARGA DE COMBOS PARA LA SELECCION DEL CLIENTE DE COTIZACION
        public void CargarComercial(ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdUsuarios, Nombres + ' ' + Apellidos AS [COMERCIAL] FROM Usuarios WHERE Estado = 'Activo' AND HabilitadoCotizacion = 1 ORDER BY Nombres + '' + Apellidos", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdUsuarios";
                cbo.DisplayMember = "COMERCIAL";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //CARGAR TIPOS DE MONEDA
        public void CargarMoneda(ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdTipoMonedas,Descripcion FROM TipoMonedas WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdTipoMonedas";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //CAMBIO DE TIPO DE MONEDA
        private void cboMoneda_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //COLOCACION DE MONEDA
            if (cboMoneda.Text == "DOLARES AMERICANOS")
            {
                imgDolares.Visible = true;
                imgEuros.Visible = false;
                imgSoles.Visible = false;
            }
            else if (cboMoneda.Text == "EUROS")
            {
                imgDolares.Visible = false;
                imgEuros.Visible = true;
                imgSoles.Visible = false;
            }
            else if (cboMoneda.Text == "SOLES")
            {
                imgDolares.Visible = false;
                imgEuros.Visible = false;
                imgSoles.Visible = true;
            }
        }

        //CAMBIO DE TIPO DE MONEDA PEDIDO
        private void cboMonedaPedido_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //COLOCACION DE MONEDA
            if (cboMonedaPedido.Text == "DOLARES AMERICANOS")
            {
                imgDolaresPedido.Visible = true;
                imgEurosPedido.Visible = false;
                imgSolesPedido.Visible = false;
            }
            else if (cboMonedaPedido.Text == "EUROS")
            {
                imgDolaresPedido.Visible = false;
                imgEurosPedido.Visible = true;
                imgSolesPedido.Visible = false;
            }
            else if (cboMonedaPedido.Text == "SOLES")
            {
                imgDolaresPedido.Visible = false;
                imgEurosPedido.Visible = false;
                imgSolesPedido.Visible = true;
            }
        }

        //CARGAR TIPOS DE ALMACENES
        public void CargarAlmacen(ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdAlmacen,Descripcion FROM Almacen WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdAlmacen";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //CARGAR COMBO DENTRO DE MI LISTADO
        public void CargarComboData()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                DataTable dt = new DataTable();
                SqlDataAdapter da, daProducts;
                da = new SqlDataAdapter("SELECT * FROM Bonificacion", con);
                daProducts = new SqlDataAdapter("SELECT * FROM Transferencia", con);
                da.Fill(ds, "bonificacion");
                daProducts.Fill(ds, "transferencia");
                //dt.Columns.Add("bonificacion", typeof(int));
                //dt.Columns.Add("transferencia");

                DataGridViewComboBoxColumn dgvCombo = datalistadoCotizacion.Columns["bonificacion"] as DataGridViewComboBoxColumn;
                {
                    var withBlock = dgvCombo;
                    withBlock.Width = 50;
                    withBlock.DataSource = ds.Tables["bonificacion"];
                    withBlock.DisplayMember = "Descripcion";
                    //withBlock.DataPropertyName = "IdBonificacion";
                    withBlock.ValueMember = "IdBonificacion";
                }

                DataGridViewComboBoxColumn dgvFilter = datalistadoCotizacion.Columns["ta"] as DataGridViewComboBoxColumn;
                {
                    var withBlock = dgvFilter;
                    withBlock.Width = 200;
                    withBlock.DataSource = ds.Tables["transferencia"];
                    withBlock.DisplayMember = "Descripcion";
                    //withBlock.DataPropertyName = "Descripcion";
                    withBlock.ValueMember = "IdTransferencia";
                }

                // Establecer valores predeterminados en las columnas ComboBox
                foreach (DataGridViewRow row in datalistadoCotizacion.Rows)
                {
                    if (!row.IsNewRow) // Ignorar la fila nueva (si aplica)
                    {
                        // Establecer el primer valor de la columna 'bonificacion'
                        if (ds.Tables["bonificacion"].Rows.Count > 0)
                        {
                            row.Cells["bonificacion"].Value = ds.Tables["bonificacion"].Rows[1]["IdBonificacion"];
                        }

                        // Establecer el primer valor de la columna 'ta'
                        if (ds.Tables["transferencia"].Rows.Count > 0)
                        {
                            row.Cells["ta"].Value = ds.Tables["transferencia"].Rows[16]["IdTransferencia"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //COMBO DE DETALLES
        //CARGAR UUNIDAD DE MIS CLIENTES
        public void CargarUnidad(int idcliente, ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdDatosAnexosClienteUnidad,Descripcion FROM DatosAnexosCliente_Unidad WHERE IdCliente = @idcliente AND Estado = 1", con);
                comando.Parameters.AddWithValue("@idcliente", idcliente);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdDatosAnexosClienteUnidad";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //CARGAR RESPONSABLE DE MIS CLIENTES
        public void CargarResponsable(int idcliente, ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT USU.IdUsuarios ,USU.Nombres + ' ' + USU.Apellidos AS [RESPONSABLE] FROM DatosAnexosCliente_Unidad DACU INNER JOIN Usuarios USU ON USU.IdUsuarios = DACU.IdResponsable  WHERE IdCliente = @idcliente AND DACU.Estado = 1", con);
                comando.Parameters.AddWithValue("@idcliente", idcliente);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "USU.IdUsuarios";
                cbo.DisplayMember = "RESPONSABLE";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //CARGAR CONTACTO DE MIS CLIENTES
        public void CargarContacto(int idcliente, ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdDatosAnexosClienteContacto,Descripcion FROM DatosAnexosCliente_Contacto WHERE IdCliente = @idcliente AND Estado = 1", con);
                comando.Parameters.AddWithValue("@idcliente", idcliente);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdDatosAnexosClienteContacto";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //CARGAR CONDICION DE MIS CLIENTES
        public void CargarCondicion(int idcliente, ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT C.IdCondicionPago, C.Descripcion FROM DatosAnexosCliente_Cindicion DACC INNER JOIN CondicionPago C ON C.IdCondicionPago = DACC.IdCondicionPago WHERE IdCliente = @idcliente AND DACC.Estado = 1", con);
                comando.Parameters.AddWithValue("@idcliente", idcliente);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "C.IdCondicionPago";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //CARGAR EL COMBO DE FORMA DE CLIENTES
        public void CargarForma(int idcliente, ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT F.IdFormaPago , F.Descripcion FROM DatosAnexosCliente_Cindicion DACC INNER JOIN FormaPago F ON F.IdFormaPago = DACC.IdFormaPago WHERE IdCliente = @idcliente AND DACC.Estado = 1", con);
                comando.Parameters.AddWithValue("@idcliente", idcliente);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "F.IdFormaPago";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //AGREGAR ITEMS A MI COTIZACION--------------------------------------------------------------------------------------------------
        //AGREGAR FORMULACION
        private void btnAgregarFormulacion_Click(object sender, EventArgs e)
        {
            if (txtNombreCliente.Text == "")
            {
                MessageBox.Show("Seleccione un cliente para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                cboFiltroFormulacion.SelectedIndex = 0;
                panelSeleccionarFormulaciones.Visible = true;
                cboBusquedaFormulaciones.SelectedIndex = 0;
                txtBusquedaFormulaciones.Text = "";
                datalistadoFormulacionesSeleccionadas.Rows.Clear();
                datalistadoBusquedaFormulaciones.DataSource = null;
                cboBusquedaFormulaciones.SelectedIndex = 3;
                cboTipoProducto.SelectedIndex = 0;
            }
        }

        //ELIMINAR FORMULACION AGREGADA A MI COTIZACION DE AGREGAR FORMULACION
        private void btnEliminarFormulacion_Click(object sender, EventArgs e)
        {
            if (datalistadoCotizacion.CurrentRow != null)
            {
                datalistadoCotizacion.Rows.Remove(datalistadoCotizacion.CurrentRow);
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un registro para proceder con la eliminación.", "Eliminación de un item", MessageBoxButtons.OK);
            }
        }

        //SELECCIONAR FORMULACION ANTES DE LLEVARLO A LA COTIZACION
        private void datalistadoBusquedaFormulaciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoBusquedaFormulaciones.Columns[e.ColumnIndex];

            if (currentColumn.Name == "Agregar")
            {
                DataGridViewRow filaSeleccionada = datalistadoBusquedaFormulaciones.Rows[e.RowIndex];

                // Extraer los valores que deseas (por ejemplo, columnas 0, 1 y 2)
                object codigoProducto = filaSeleccionada.Cells[3].Value;
                object codigoBSS = filaSeleccionada.Cells[4].Value;
                object detaleproducto = filaSeleccionada.Cells[5].Value;
                object codigoFormlacion = filaSeleccionada.Cells[2].Value;

                // Crear nueva fila para el segundo DataGridView
                int nuevaFila = datalistadoFormulacionesSeleccionadas.Rows.Add();
                datalistadoFormulacionesSeleccionadas.Rows[nuevaFila].Cells[0].Value = codigoProducto;
                datalistadoFormulacionesSeleccionadas.Rows[nuevaFila].Cells[1].Value = codigoBSS;
                datalistadoFormulacionesSeleccionadas.Rows[nuevaFila].Cells[2].Value = detaleproducto;
                datalistadoFormulacionesSeleccionadas.Rows[nuevaFila].Cells[3].Value = codigoFormlacion;

                datalistadoBusquedaFormulaciones.Rows.Remove(datalistadoBusquedaFormulaciones.CurrentRow);
                alternarColorFilas(datalistadoFormulacionesSeleccionadas);
            }
        }

        //LLEVAR LAS FORMULACIONES SELECCIONADAS A MI COTIZACION
        private void btnAceptarSeleccionarFormulacion_Click(object sender, EventArgs e)
        {
            panelSeleccionarFormulaciones.Visible = false;
            cboFiltroFormulacion.SelectedIndex = 0;
            txtBusquedaFormulaciones.Text = "";

            if (datalistadoFormulacionesSeleccionadas.CurrentRow != null)
            {
                foreach (DataGridViewRow row in datalistadoFormulacionesSeleccionadas.Rows)
                {
                    string codigo = row.Cells[0].Value.ToString();
                    string codigoformulacion = row.Cells[3].Value.ToString();
                    string detalle = row.Cells[2].Value.ToString();
                    string codigoBss = row.Cells[2].Value.ToString();

                    datalistadoCotizacion.Rows.Add(new[] { codigo, detalle, codigoformulacion });
                }
            }
            datalistadoFormulacionesSeleccionadas.Rows.Clear();
            alternarColorFilas(datalistadoCotizacion);
            CargarComboData();
        }

        //ELIMINAR LAS FORMULACIONES SELECCIONADOS
        private void btnEliminarUnoSeleccionarProducto_Click(object sender, EventArgs e)
        {
            if (datalistadoFormulacionesSeleccionadas.CurrentRow != null)
            {
                datalistadoFormulacionesSeleccionadas.Rows.Remove(datalistadoFormulacionesSeleccionadas.CurrentRow);
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un registro para proceder con la eliminación.", "Eliminación de un item", MessageBoxButtons.OK);
            }
        }

        //REGRESAR O SALIR DE LA BUSQUEDA DE FORMULACIONES
        private void btnRegresarSeleccionarFormulaciones_Click(object sender, EventArgs e)
        {
            datalistadoBusquedaFormulaciones.DataSource = null;
            datalistadoFormulacionesSeleccionadas.Rows.Clear();
            panelSeleccionarFormulaciones.Visible = false;
            cboFiltroFormulacion.SelectedIndex = 0;
            txtBusquedaFormulaciones.Text = "";
        }

        //MODIFICACION DEL DATAGRIDVIEW DE LA COTIZACION--------------------------------------------------------------------
        private void datalistadoCotizacion_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // ESTE CAMPO ES PARA EL FILTRO DE UNA DEL BONIFICACION A T/A
            if (e.ColumnIndex == 11)
            {
                DataGridViewComboBoxCell dgvCbo = datalistadoCotizacion[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
                if (dgvCbo != null)
                {
                    if (datalistadoCotizacion.Rows[datalistadoCotizacion.CurrentRow.Index].Cells[10].Value != null)
                    {
                        string str = datalistadoCotizacion.Rows[datalistadoCotizacion.CurrentRow.Index].Cells[10].Value.ToString();
                        if (str == "SI")
                        {
                            str = "1";
                        }
                        else if (str == "NO")
                        {
                            str = "2";
                        }

                        if (datalistadoCotizacion.Rows[datalistadoCotizacion.CurrentRow.Index].Cells[10].Value.ToString() == null)
                        {
                            MessageBox.Show("Por favor, seleccione un tipo de bonificación para continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                        }
                        else
                        {
                            dv = new DataView(ds.Tables["transferencia"]);
                            dv.RowFilter = "IdBonificacion = " + str;
                            dgvCbo.DataSource = dv;
                        }
                    }
                }
            }
        }

        //FIN DE LA EDICION DE MI LISTADO DE FORMULACIONES EN UNA NUEVA COTIZACION
        private void datalistadoCotizacion_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            decimal cantidad = 0;
            decimal precio;
            decimal descuento;
            decimal total = 0;
            string codigocliente = "";
            string descripcioncliente = "";

            DataGridViewRow row = (DataGridViewRow)datalistadoCotizacion.Rows[e.RowIndex];

            if (row.Cells[3].Value == null)
            {
                cantidad = Convert.ToInt32("0");
                row.Cells[3].Value = "0";
            }
            else
            {
                cantidad = Convert.ToDecimal(row.Cells[3].Value);
            }

            if (row.Cells[4].Value == null)
            {
                precio = Convert.ToDecimal("0.00");
                row.Cells[4].Value = "0.00";
            }
            else
            {
                precio = Convert.ToDecimal(row.Cells[4].Value.ToString());
            }

            if (row.Cells[5].Value == null)
            {
                descuento = Convert.ToDecimal("0.00");
                row.Cells[5].Value = "0.00";
            }
            else
            {
                descuento = Convert.ToDecimal(row.Cells[5].Value.ToString());
            }

            if (row.Cells[12].Value == null)
            {
                codigocliente = "";
            }
            else
            {
                codigocliente = row.Cells[12].Value.ToString();
            }

            if (row.Cells[13].Value == null)
            {
                descripcioncliente = "";
            }
            else
            {
                descripcioncliente = row.Cells[13].Value.ToString();
            }

            total = (cantidad * precio) - ((cantidad * precio) * (descuento / 100));

            row.Cells[4].Value = String.Format("{0:#,0.00}", precio);
            row.Cells[5].Value = String.Format("{0:#,0.00}", descuento);
            row.Cells[6].Value = String.Format("{0:#,0.00}", total);

            row.Cells[12].Value = String.Format(codigocliente);
            row.Cells[13].Value = String.Format(descripcioncliente);

            SubTotal(datalistadoCotizacion, 6, txtSubTotal);
            DescuentoSub(txtSubTotal, SelectDescuento, txtDescuento);
            IGV(txtSubTotal, txtDescuento, SelectIgv, txtIgv);
            Total(txtSubTotal, txtDescuento, txtIgv, txtTotal);

            txtInafecta.Text = String.Format("0.00");
            txtExonerada.Text = String.Format("0.00");
            txtTotalDescuento.Text = String.Format("0.00");
        }

        //CAMBIAR EL VALOR DEL DESCUENTO TOTAL
        private void SelectDescuento_ValueChanged(object sender, EventArgs e)
        {
            SubTotal(datalistadoCotizacion, 6, txtSubTotal);
            DescuentoSub(txtSubTotal, SelectDescuento, txtDescuento);
            IGV(txtSubTotal, txtDescuento, SelectIgv, txtIgv);
            Total(txtSubTotal, txtDescuento, txtIgv, txtTotal);
        }

        //CAMBIAR EL VALOR DEL IGV TOTAL
        private void SelectIgv_ValueChanged(object sender, EventArgs e)
        {
            SubTotal(datalistadoCotizacion, 6, txtSubTotal);
            DescuentoSub(txtSubTotal, SelectDescuento, txtDescuento);
            IGV(txtSubTotal, txtDescuento, SelectIgv, txtIgv);
            Total(txtSubTotal, txtDescuento, txtIgv, txtTotal);
        }

        //GUARDAR COTIZACION
        private void btnGuardarCotizacion_Click(object sender, EventArgs e)
        {
            int contador = 0;
            bool estadoValidacion = false;

            //VALIDAR SI NO HAY REGISTROS O SI NO SE SELECCIONO EL COMBO
            if (datalistadoCotizacion.Rows.Count == 0)
            {
                estadoValidacion = false;
            }
            else
            {
                int estadoItemsContadosNoValidos = 0;
                int estadoItemsContadosValidos = 0;
                int items = datalistadoCotizacion.Rows.Count;

                foreach (DataGridViewRow fila in datalistadoCotizacion.Rows)
                {
                    if (fila.Cells[11].Value == null)
                    {
                        estadoItemsContadosNoValidos = estadoItemsContadosNoValidos + 1;
                    }
                    else
                    {
                        estadoItemsContadosValidos = estadoItemsContadosValidos + 1;
                    }
                }

                if (estadoItemsContadosValidos == items)
                {
                    estadoValidacion = true;
                }
                else
                {
                    estadoValidacion = false;
                }
            }

            DialogResult boton = MessageBox.Show("¿Esta seguro que desea guardar esta cotización?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {

                if (txtNombreCliente.Text == "" || cboUnidadCliente.SelectedValue == null || cboResponsableCliente.SelectedValue == null || cboContactoCliente.SelectedValue == null || cboCondicionPagoCliente.SelectedValue == null || cboFormaPagoCliente.SelectedValue == null || cboComercial.SelectedValue == null || cboMoneda.SelectedValue == null || cboAlmacen.SelectedValue == null || txtReferencia.Text == "Campo Obligatorio")
                {
                    MessageBox.Show("Debe llenar todos los campos para poder continuar con el ingreso de la cotización.", "Validación del Sistema");
                }
                else
                {
                    //FUNCION PARA 
                    if (estadoValidacion == false)
                    {
                        MessageBox.Show("Debe seleccionar alguna formulación o seleccionar una bonificación y una tranferencia para poder continuar.", "Guardar Cotización", MessageBoxButtons.OK);
                    }
                    else if (estadoValidacion == true && lblTituloCotizacion.Text == "NUEVA COTIZACIÓN")
                    {
                        //INSERTAR COTIZACION
                        try
                        {
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("Cotizacion_Insertar", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@fechaEmision", dateFechaEmision.Value);
                            cmd.Parameters.AddWithValue("@fechaValidez", dateFechaValidez.Value);

                            cmd.Parameters.AddWithValue("@idcliente", CodigoCLiente);
                            cmd.Parameters.AddWithValue("@idcomercial", cboComercial.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@idmoneda", cboMoneda.SelectedValue.ToString());

                            cmd.Parameters.AddWithValue("@referencia", txtReferencia.Text);

                            cmd.Parameters.AddWithValue("@idalmacen", cboAlmacen.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@lugarentrega", txtLugarEntregado.Text);
                            cmd.Parameters.AddWithValue("@garantia", txtGarantia.Text);
                            cmd.Parameters.AddWithValue("@tiempoEntrega", txtTiempoEntrega.Text);
                            cmd.Parameters.AddWithValue("@observaciones", txtObservaciones.Text);

                            cmd.Parameters.AddWithValue("@subtotal", Convert.ToDecimal(txtSubTotal.Text));
                            cmd.Parameters.AddWithValue("@descuento", Convert.ToDecimal(txtDescuento.Text));
                            cmd.Parameters.AddWithValue("@inafecta", Convert.ToDecimal(txtInafecta.Text));
                            cmd.Parameters.AddWithValue("@exonerado", Convert.ToDecimal(txtExonerada.Text));
                            cmd.Parameters.AddWithValue("@igv", Convert.ToDecimal(txtIgv.Text));
                            cmd.Parameters.AddWithValue("@totaldescuento", Convert.ToDecimal(txtTotalDescuento.Text));
                            cmd.Parameters.AddWithValue("@total", Convert.ToDecimal(txtTotal.Text));

                            cmd.Parameters.AddWithValue("@idunidad", cboUnidadCliente.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@idresponsable", cboResponsableCliente.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@idcontacto", cboContactoCliente.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@idformapago", cboFormaPagoCliente.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@idcondicionpago", cboCondicionPagoCliente.SelectedValue.ToString());
                            CodigoGeneracionCotizacion();
                            cmd.Parameters.AddWithValue("@codigocotizacion", CodigoGeneradoCotizacion);
                            cmd.Parameters.AddWithValue("@idBrochure", cboBrochures.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@rutaFinal", "");
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        //INSERTAR DETALLE COTIZACION
                        CodigoCotizacion();
                        int codigocotizacion = Convert.ToInt32(datalistadoCodigoCotizacion.SelectedCells[0].Value.ToString());

                        foreach (DataGridViewRow fila in datalistadoCotizacion.Rows)
                        {
                            try
                            {
                                SqlConnection con = new SqlConnection();
                                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                                con.Open();
                                contador = contador + 1;

                                SqlCommand cmd = new SqlCommand();
                                cmd = new SqlCommand("Cotizacion_InsertarDetalle", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@idcotizacion", codigocotizacion);
                                cmd.Parameters.AddWithValue("@codigoproducto", fila.Cells[0].Value.ToString());
                                cmd.Parameters.AddWithValue("@codigoformulacion", fila.Cells[2].Value.ToString());

                                cmd.Parameters.AddWithValue("@cantidad", Convert.ToInt32(fila.Cells[3].Value.ToString()));
                                cmd.Parameters.AddWithValue("@preciounidad", Convert.ToDecimal(fila.Cells[4].Value.ToString()));
                                cmd.Parameters.AddWithValue("@descuento", Convert.ToDecimal(fila.Cells[5].Value.ToString()));
                                cmd.Parameters.AddWithValue("@total", Convert.ToDecimal(fila.Cells[6].Value.ToString()));

                                cmd.Parameters.AddWithValue("@idbonificacion", Convert.ToDecimal(fila.Cells[10].Value.ToString()));
                                cmd.Parameters.AddWithValue("@ta", fila.Cells[11].Value.ToString());

                                cmd.Parameters.AddWithValue("@codigocliente", Convert.ToString(fila.Cells[12].Value.ToString()));
                                cmd.Parameters.AddWithValue("@descripcioncliente", fila.Cells[13].Value.ToString());
                                cmd.Parameters.AddWithValue("@contador", contador);
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error en la operación por: " + ex.Message);
                                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
                            }
                        }

                        MessageBox.Show("Cotización ingresada con éxito.", "Nueva Cotización", MessageBoxButtons.OK);
                        ClassResourses.RegistrarAuditora(1, this.Name, 2, Program.IdUsuario, "", codigocotizacion);
                        LimpiarNuevaCotizacion();
                    }
                }
            }

            CargarCotizaciones(DesdeFecha.Value, HastaFecha.Value);
        }

        //EDITAR MI COTIZACION YA INGRESADA
        private void btnEditarCotizacion_Click(object sender, EventArgs e)
        {
            codigoCotizacion = int.Parse(datalistadoTodasCotiaciones.Rows[datalistadoTodasCotiaciones.CurrentRow.Index].Cells[1].Value.ToString());
            string estadoCoti = datalistadoTodasCotiaciones.SelectedCells[33].Value.ToString();

            if (estadoCoti == "FUERA DE FECHA" || estadoCoti == "ANULADO" || estadoCoti == "ADJUDICADO PARCIALMENTE" || estadoCoti == "COMPLETADO")
            {
                MessageBox.Show("La cotización que intenta editar ya se encuentra en un estado diferente a PENDIENTE, no se puede editar una cotización que este anulada, vencida o adjudicada parcial/total.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                //TAER LA CABECERA DE MI COTRIZACION
                BuscarCotizacionPorCodigo(codigoCotizacion);
                //BUSCAR ITEMS DE MI COTIZACION
                BuscarCotizacionDetallePorCodigo(codigoCotizacion);

                CargarUnidad(Convert.ToInt32(dataListadiCotiXCodigo.SelectedCells[4].Value.ToString()), cboUnidadCliente);
                CargarResponsable(Convert.ToInt32(dataListadiCotiXCodigo.SelectedCells[4].Value.ToString()), cboResponsableCliente);
                CargarContacto(Convert.ToInt32(dataListadiCotiXCodigo.SelectedCells[4].Value.ToString()), cboContactoCliente);
                CargarCondicion(Convert.ToInt32(dataListadiCotiXCodigo.SelectedCells[4].Value.ToString()), cboCondicionPagoCliente);
                CargarForma(Convert.ToInt32(dataListadiCotiXCodigo.SelectedCells[4].Value.ToString()), cboFormaPagoCliente);
                CargarMoneda(cboMoneda);
                CargarAlmacen(cboAlmacen);
                CargarComercial(cboComercial);

                txtLugarEntregado.Text = dataListadiCotiXCodigo.SelectedCells[16].Value.ToString();
                if (txtLugarEntregado.Text == "Calle El Martillo MZ B Lote 5 Urb. Industrial El Naranjal")
                {
                    ckAlmacenArenas.Checked = true;
                }

                txtGarantia.Text = dataListadiCotiXCodigo.SelectedCells[17].Value.ToString();
                txtTiempoEntrega.Text = dataListadiCotiXCodigo.SelectedCells[18].Value.ToString();
                txtDocumentoCliente.Text = dataListadiCotiXCodigo.SelectedCells[32].Value.ToString();
                txtNombreCliente.Text = dataListadiCotiXCodigo.SelectedCells[5].Value.ToString();
                txtDireccionClente.Text = dataListadiCotiXCodigo.SelectedCells[33].Value.ToString();
                txtObservaciones.Text = dataListadiCotiXCodigo.SelectedCells[19].Value.ToString();
                txtReferencia.Text = dataListadiCotiXCodigo.SelectedCells[14].Value.ToString();

                dateFechaEmision.Text = dataListadiCotiXCodigo.SelectedCells[2].Value.ToString();
                dateFechaValidez.Text = dataListadiCotiXCodigo.SelectedCells[3].Value.ToString();
                txtCodigoCotizacion.Text = dataListadiCotiXCodigo.SelectedCells[1].Value.ToString();

                cboUnidadCliente.SelectedValue = dataListadiCotiXCodigo.SelectedCells[6].Value.ToString();
                cboResponsableCliente.SelectedValue = dataListadiCotiXCodigo.SelectedCells[8].Value.ToString();
                cboContactoCliente.SelectedValue = dataListadiCotiXCodigo.SelectedCells[27].Value.ToString();
                cboCondicionPagoCliente.SelectedValue = dataListadiCotiXCodigo.SelectedCells[29].Value.ToString();
                cboFormaPagoCliente.SelectedValue = dataListadiCotiXCodigo.SelectedCells[28].Value.ToString();
                cboMoneda.SelectedValue = dataListadiCotiXCodigo.SelectedCells[12].Value.ToString();
                cboComercial.SelectedValue = dataListadiCotiXCodigo.SelectedCells[10].Value.ToString();
                cboAlmacen.SelectedValue = dataListadiCotiXCodigo.SelectedCells[15].Value.ToString();

                txtSubTotal.Text = dataListadiCotiXCodigo.SelectedCells[20].Value.ToString();
                txtDescuento.Text = dataListadiCotiXCodigo.SelectedCells[21].Value.ToString();
                txtInafecta.Text = dataListadiCotiXCodigo.SelectedCells[22].Value.ToString();
                txtExonerada.Text = dataListadiCotiXCodigo.SelectedCells[23].Value.ToString();
                txtIgv.Text = dataListadiCotiXCodigo.SelectedCells[24].Value.ToString();
                txtTotalDescuento.Text = dataListadiCotiXCodigo.SelectedCells[25].Value.ToString();
                txtTotal.Text = dataListadiCotiXCodigo.SelectedCells[26].Value.ToString();

                CargarBrochures();
                int estadoBrochure = Convert.ToInt32(dataListadiCotiXCodigo.SelectedCells[34].Value.ToString());
                if (estadoBrochure == 1)
                {
                    ckAplicarBrochure.Checked = false;
                }
                else
                {
                    ckAplicarBrochure.Checked = true;
                    cboBrochures.SelectedValue = estadoBrochure;
                }

                //CARGAR DETALLES DE LA COTIZACION
                datalistadoCotizacion.Rows.Clear();

                if (dataListadiCotiDetallesXCodigo.CurrentRow != null)
                {
                    foreach (DataGridViewRow row in dataListadiCotiDetallesXCodigo.Rows)
                    {
                        string idDetalleCotizacion = row.Cells[0].Value.ToString();
                        string codigo = row.Cells[2].Value.ToString();
                        string codigoformulacion = row.Cells[4].Value.ToString();
                        string detalle = row.Cells[3].Value.ToString();
                        string cantidad = row.Cells[5].Value.ToString();
                        string precioUnitario = row.Cells[6].Value.ToString();
                        string descuento = row.Cells[7].Value.ToString();
                        string total = row.Cells[8].Value.ToString();
                        string codigoCliente = row.Cells[13].Value.ToString();
                        string descripcionCliente = row.Cells[14].Value.ToString();

                        datalistadoCotizacion.Rows.Add(new[] { codigo, detalle, codigoformulacion, cantidad, precioUnitario, descuento, total, null, null, null, null, null, codigoCliente, descripcionCliente, idDetalleCotizacion });
                    }
                }

                datalistadoFormulacionesSeleccionadas.Rows.Clear();
                alternarColorFilas(datalistadoCotizacion);
                CargarComboData();

                btnGuardarCotizacion.Visible = false;
                lblGuardar.Visible = false;
                btnEditarCotizacionAccion.Visible = true;
                lblEditar.Visible = true;

                datalistadoTodasCotiaciones.Enabled = false;
                panelNuevaCotizacion.Visible = true;

                lblTituloCotizacion.Text = "EDICIÓN COTIZACIÓN";

                datalistadoCotizacion.Columns[0].ReadOnly = true;
                datalistadoCotizacion.Columns[1].ReadOnly = true;
                datalistadoCotizacion.Columns[2].ReadOnly = true;
                datalistadoCotizacion.Columns[6].ReadOnly = true;
            }
        }

        //EDITAR COTIZACION
        private void btnEditarCotizacionAccion_Click(object sender, EventArgs e)
        {
            int idCotizacion = int.Parse(datalistadoTodasCotiaciones.Rows[datalistadoTodasCotiaciones.CurrentRow.Index].Cells[1].Value.ToString());
            int contador = 0;
            bool estadoValidacion = false;

            //VALIDAR SI NO HAY REGISTROS O SI NO SE SELECCIONO EL COMBO
            if (datalistadoCotizacion.Rows.Count == 0)
            {
                estadoValidacion = false;
            }
            else
            {
                int estadoItemsContadosNoValidos = 0;
                int estadoItemsContadosValidos = 0;
                int items = datalistadoCotizacion.Rows.Count;

                foreach (DataGridViewRow fila in datalistadoCotizacion.Rows)
                {
                    if (fila.Cells[11].Value == null)
                    {
                        estadoItemsContadosNoValidos = estadoItemsContadosNoValidos + 1;
                    }
                    else
                    {
                        estadoItemsContadosValidos = estadoItemsContadosValidos + 1;
                    }
                }

                if (estadoItemsContadosValidos == items)
                {
                    estadoValidacion = true;
                }
                else
                {
                    estadoValidacion = false;
                }
            }

            DialogResult boton = MessageBox.Show("¿Esta seguro que desea editar esta Cotización?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (txtNombreCliente.Text == "" || cboUnidadCliente.SelectedValue == null || cboResponsableCliente.SelectedValue == null || cboContactoCliente.SelectedValue == null || cboCondicionPagoCliente.SelectedValue == null || cboFormaPagoCliente.SelectedValue == null || cboComercial.SelectedValue == null || cboMoneda.SelectedValue == null || cboAlmacen.SelectedValue == null || txtReferencia.Text == "Campo Obligatorio")
                {
                    MessageBox.Show("Debe llenar todos los campos para poder continuar con la edición de la cotización.", "Validación del Sistema");
                }
                else
                {
                    //FUNCION PARA 
                    if (estadoValidacion == false)
                    {
                        MessageBox.Show("Debe seleccionar alguna formulación o seleccionar una bonificación y una tranferencia para poder continuar.", "Modificar Cotización", MessageBoxButtons.OK);
                    }
                    else if (estadoValidacion == true && lblTituloCotizacion.Text == "EDICIÓN COTIZACIÓN")
                    {
                        //INSERTAR COTIZACION
                        try
                        {
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("Cotizacion_Modificar", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@idcotizacion", idCotizacion);
                            cmd.Parameters.AddWithValue("@fechaValidez", dateFechaValidez.Value);

                            cmd.Parameters.AddWithValue("@idcomercial", cboComercial.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@idmoneda", cboMoneda.SelectedValue.ToString());

                            cmd.Parameters.AddWithValue("@referencia", txtReferencia.Text);

                            cmd.Parameters.AddWithValue("@idalmacen", cboAlmacen.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@lugarentrega", txtLugarEntregado.Text);
                            cmd.Parameters.AddWithValue("@garantia", txtGarantia.Text);
                            cmd.Parameters.AddWithValue("@tiempoEntrega", txtTiempoEntrega.Text);
                            cmd.Parameters.AddWithValue("@observaciones", txtObservaciones.Text);

                            cmd.Parameters.AddWithValue("@subtotal", Convert.ToDecimal(txtSubTotal.Text));
                            cmd.Parameters.AddWithValue("@descuento", Convert.ToDecimal(txtDescuento.Text));
                            cmd.Parameters.AddWithValue("@inafecta", Convert.ToDecimal(txtInafecta.Text));
                            cmd.Parameters.AddWithValue("@exonerado", Convert.ToDecimal(txtExonerada.Text));
                            cmd.Parameters.AddWithValue("@igv", Convert.ToDecimal(txtIgv.Text));
                            cmd.Parameters.AddWithValue("@totaldescuento", Convert.ToDecimal(txtTotalDescuento.Text));
                            cmd.Parameters.AddWithValue("@total", Convert.ToDecimal(txtTotal.Text));

                            cmd.Parameters.AddWithValue("@idunidad", cboUnidadCliente.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@idresponsable", cboResponsableCliente.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@idcontacto", cboContactoCliente.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@idformapago", cboFormaPagoCliente.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@idcondicionpago", cboCondicionPagoCliente.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@idBrochure", cboBrochures.SelectedValue.ToString());

                            cmd.ExecuteNonQuery();
                            con.Close();

                            //MODIFICAR DETALLE COTIZACION
                            foreach (DataGridViewRow fila in datalistadoCotizacion.Rows)
                            {
                                try
                                {
                                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                                    con.Open();
                                    contador = contador + 1;

                                    cmd = new SqlCommand("Cotizacion_ModificarDetalle", con);
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.AddWithValue("@iddetallecotizacion", Convert.ToInt32(fila.Cells[14].Value.ToString()));
                                    cmd.Parameters.AddWithValue("@codigoproducto", fila.Cells[0].Value.ToString());
                                    cmd.Parameters.AddWithValue("@codigoformulacion", fila.Cells[2].Value.ToString());

                                    cmd.Parameters.AddWithValue("@cantidad", Convert.ToInt32(fila.Cells[3].Value.ToString()));
                                    cmd.Parameters.AddWithValue("@preciounidad", Convert.ToDecimal(fila.Cells[4].Value.ToString()));
                                    cmd.Parameters.AddWithValue("@descuento", Convert.ToDecimal(fila.Cells[5].Value.ToString()));
                                    cmd.Parameters.AddWithValue("@total", Convert.ToDecimal(fila.Cells[6].Value.ToString()));

                                    cmd.Parameters.AddWithValue("@idbonificacion", Convert.ToDecimal(fila.Cells[10].Value.ToString()));
                                    cmd.Parameters.AddWithValue("@ta", fila.Cells[11].Value.ToString());

                                    cmd.Parameters.AddWithValue("@codigocliente", Convert.ToString(fila.Cells[12].Value.ToString()));
                                    cmd.Parameters.AddWithValue("@descripcioncliente", fila.Cells[13].Value.ToString());
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error en la operación por: " + ex.Message);
                                    ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        ClassResourses.RegistrarAuditora(8, this.Name, 2, Program.IdUsuario, "", idCotizacion);
                        MessageBox.Show("Cotización editada con éxito.", "Edición Cotización", MessageBoxButtons.OK);
                        LimpiarNuevaCotizacion();
                    }
                }
            }

            datalistadoTodasCotiaciones.Enabled = true;
            CargarCotizaciones(DesdeFecha.Value, HastaFecha.Value);
        }

        //HABILITAR ANULACION-------------------------------------------------------------------------------
        private void btnAnularCotizacion_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasCotiaciones.SelectedCells[33].Value.ToString() == "ANULADO")
            {
                MessageBox.Show("La cotización que intenta anular ya se encuentra anulada.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panleAnulacion.Visible = true;
                datalistadoTodasCotiaciones.Enabled = false;
                txtJustificacionAnulacion.Text = "";
            }
        }

        //ANULAR COTIZACION
        private void btnProcederAnulacion_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasCotiaciones.CurrentRow != null)
            {
                int codigo = int.Parse(datalistadoTodasCotiaciones.Rows[datalistadoTodasCotiaciones.CurrentRow.Index].Cells[1].Value.ToString());
                string estado = datalistadoTodasCotiaciones.Rows[datalistadoTodasCotiaciones.CurrentRow.Index].Cells[33].Value.ToString();

                if (codigo != 0)
                {
                    //VALIDACION DE CARACTERES INGRESADOS
                    if (txtJustificacionAnulacion.Text.Length > 10)
                    {
                        DialogResult boton = MessageBox.Show("¿Esta seguro que desea anular esta cotización?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                        if (boton == DialogResult.OK)
                        {
                            if (estado == "COMPLETADO" || estado == "ADJUDICADO PARCIALMENTE")
                            {
                                MessageBox.Show("Esta cotización ya tiene un pedido generado, por favor anular por el mantenimiento de pedidos.", "Validación del Sistema", MessageBoxButtons.OK);
                                return;
                            }
                            else
                            {
                                try
                                {
                                    SqlConnection con = new SqlConnection();
                                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                                    con.Open();
                                    SqlCommand cmd = new SqlCommand();
                                    cmd = new SqlCommand("Cotizacion_Anular", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@codigo", codigo);
                                    cmd.Parameters.AddWithValue("@mensaje", txtJustificacionAnulacion.Text);
                                    cmd.ExecuteNonQuery();
                                    con.Close();

                                    ClassResourses.RegistrarAuditora(2, this.Name, 2, Program.IdUsuario, "", codigo);
                                    MessageBox.Show("Anulación correcta, operación hecha satisfactoriamente.", "Validación del Sistema", MessageBoxButtons.OK);

                                    panleAnulacion.Visible = false;
                                    txtJustificacionAnulacion.Text = "";
                                    datalistadoTodasCotiaciones.Enabled = true;
                                    CargarCotizaciones(DesdeFecha.Value, HastaFecha.Value);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error en la operación por: " + ex.Message);
                                    ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe ingresar una justificación válida. La cantidad de carácteres debe ser mayor a 10.", "Validación del Sistema", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("No se pudo anular.", "Validación del Sistema", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Seleccione una cotización para poder anularla.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //RETROCEDER EN LA ANULACION
        private void btnRetrocederAnulacion_Click(object sender, EventArgs e)
        {
            txtJustificacionAnulacion.Text = "";
            panleAnulacion.Visible = false;
            datalistadoTodasCotiaciones.Enabled = true;
        }
        //---------------------------------------------------------------------------------------------------

        //VISUALIZAR MI PDF DE MI COTIZACION
        private void btnGenerarCotizacionPdf_Click(object sender, EventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoTodasCotiaciones.CurrentRow != null)
            {
                //SI LA CORIZACIÓN SE ENUENTRA ANULADA
                if (datalistadoTodasCotiaciones.SelectedCells[33].Value.ToString() == "ANULADO")
                {
                    string ccodigoCotizacion = datalistadoTodasCotiaciones.Rows[datalistadoTodasCotiaciones.CurrentRow.Index].Cells[1].Value.ToString();
                    Visualizadores.VisualizarCotizacionVentaAnulada frm = new Visualizadores.VisualizarCotizacionVentaAnulada();
                    frm.lblCodigo.Text = ccodigoCotizacion;

                    frm.Show();
                }
                //SI LA COTIZACIÓN SE ENCUENTRA EN UN ESTADO DIFERENTE
                else
                {
                    if (Convert.ToInt32(datalistadoTodasCotiaciones.SelectedCells[35].Value.ToString()) == 1)
                    {
                        string ccodigoCotizacion = datalistadoTodasCotiaciones.Rows[datalistadoTodasCotiaciones.CurrentRow.Index].Cells[1].Value.ToString();
                        Visualizadores.VisualizarCotizacionVenta frm = new Visualizadores.VisualizarCotizacionVenta();
                        frm.lblCodigo.Text = ccodigoCotizacion;

                        frm.Show();
                    }
                    else
                    {
                        string ccodigoCotizacion = datalistadoTodasCotiaciones.Rows[datalistadoTodasCotiaciones.CurrentRow.Index].Cells[1].Value.ToString();
                        Visualizadores.VisualizarCotizacionVenta frm = new Visualizadores.VisualizarCotizacionVenta();
                        frm.lblCodigo.Text = ccodigoCotizacion;

                        frm.Show();

                        string rutaReporte = @"C:\ArenasSoftBrochure\Cotizacion.pdf";
                        string rutaOtroPDF = datalistadoTodasCotiaciones.SelectedCells[34].Value.ToString();
                        string rutaFinal = @"C:\ArenasSoftBrochure\ReporteFinal.pdf";

                        using (FileStream stream = new FileStream(rutaFinal, FileMode.Create))
                        {
                            Document document = new Document();
                            PdfCopy copy = new PdfCopy(document, stream);
                            document.Open();

                            void AgregarPDF(string path)
                            {
                                PdfReader reader = new PdfReader(path);
                                for (int i = 1; i <= reader.NumberOfPages; i++)
                                {
                                    copy.AddPage(copy.GetImportedPage(reader, i));
                                }
                                reader.Close();
                            }

                            AgregarPDF(rutaOtroPDF);
                            AgregarPDF(rutaReporte);

                            document.Close();
                        }

                        // Abrir el archivo después de la ejecución
                        Process.Start(new ProcessStartInfo(rutaFinal) { UseShellExecute = true });
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una cotización para poder generar el PDF respectivo.", "Validación del Sistema");
            }
        }


        //FUNCION PARA LIMPIAR TODOS LOS CAMPOS DE MI COTIZACION
        public void LimpiarCotizacion()
        {
            //LIMPIESA DE CAMPOS
            txtNombreCliente.Text = "";
            txtDireccionClente.Text = "";
            txtDocumentoCliente.Text = "";
            cboUnidadCliente.DataSource = null;
            cboResponsableCliente.DataSource = null;
            cboContactoCliente.DataSource = null;
            cboCondicionPagoCliente.DataSource = null;
            cboFormaPagoCliente.DataSource = null;
            ckAlmacenArenas.Checked = false;

            datalistadoCotizacion.Rows.Clear();
            txtObservaciones.Text = "Campo Opcional";
            txtLugarEntregado.Text = "Campo Opcional";
            txtReferencia.Text = "Campo Obligatorio";
            txtGarantia.Text = "Campo Opcional";

            txtSubTotal.Text = "0.00";
            txtDescuento.Text = "0.00";
            txtInafecta.Text = "0.00";
            txtExonerada.Text = "0.00";
            txtIgv.Text = "0.00";
            txtTotalDescuento.Text = "0.00";
            txtTotal.Text = "0.00";
            SelectDescuento.Text = "0.00";
            SelectIgv.Text = "18.00";
        }

        //BUSCAR Y SELECCIONAR CLIENTES PARA LA COTIZACION
        private void txtBusquedaClientes2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboTipoBusquedaClientes.Text == "NOMBRES")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Cotizacion_BuscarClientesPorNombres", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", txtBusquedaClientes2.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoclientes.DataSource = dt;
                    con.Close();
                }
                else if (cboTipoBusquedaClientes.Text == "DOCUMENTO")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Cotizacion_BuscarClientesPorDocumento", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@documento", txtBusquedaClientes2.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoclientes.DataSource = dt;
                    con.Close();
                }

                ReordenarColumnasBusquedaClientes(datalistadoclientes);
                alternarColorFilas(datalistadoclientes);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //FUNCION APRA REORDENAR MIS COLUMNAS D EMI BUSQUEDA DE CLIENTE
        public void ReordenarColumnasBusquedaClientes(DataGridView DGV)
        {
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

            DGV.Columns[0].Width = 100;
            DGV.Columns[1].Width = 90;
            DGV.Columns[2].Width = 260;
            DGV.Columns[3].Width = 100;
            DGV.Columns[4].Width = 150;
        }

        //CAMBIO DE TIPO DE BUSQEUDA DE FORMUACIONES
        private void cboBusquedaFormulaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusquedaFormulaciones.Text = "";
        }

        ////MOSTRAR LAS FORMUILACIONES COOMPLETAS
        //public void Filtro_MostrarTodasFormulaciones()
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        SqlConnection con = new SqlConnection();
        //        con.ConnectionString = Conexion.ConexionMaestra.conexion;
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand();
        //        cmd = new SqlCommand("Cotizacion_MostrarTodasFormulaciones", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        datalistadoBusquedaFormulaciones.DataSource = dt;
        //        con.Close();
        //        ColorearBoton(datalistadoBusquedaFormulaciones);
        //        ReordenadoColumnasBusquedaFormulaciones(datalistadoBusquedaFormulaciones);
        //        cboBusquedaFormulaciones.Text = "";
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
        //    }
        //}

        //MOSTRAR LAS FORMULACIONES DE PRODUCTOS
        public void Filtro_MostrarTodasFormulaciones_Formulacion()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Cotizacion_MostrarTodasFormulaciones_Formulacion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoBusquedaFormulaciones.DataSource = dt;
                con.Close();
                //ColorearBoton(datalistadoBusquedaFormulaciones);
                ReordenadoColumnasBusquedaFormulaciones(datalistadoBusquedaFormulaciones);
                cboBusquedaFormulaciones.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //MOSTRAR LAS FORMULACIONES MERCADERIAS Y SERVICIOS
        public void Filtro_MostrarTodasFormulaciones_Producto()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Cotizacion_MostrarTodasFormulaciones_Producto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoBusquedaFormulaciones.DataSource = dt;
                con.Close();
                //ColorearBoton(datalistadoBusquedaFormulaciones);
                ReordenadoColumnasBusquedaFormulaciones(datalistadoBusquedaFormulaciones);
                cboBusquedaFormulaciones.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //BUSCAR FORMULACIONES PARA AGREGAR A MI COTIZAVION
        private void txtBusquedaFormulaciones_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboTipoProducto.Text == "PRODUCTOS FABRICADOS")
                {
                    if (cboBusquedaFormulaciones.Text == "DESCRIPCIÓN")
                    {
                        DataTable dt = new DataTable();
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Cotizacion_MostrarTodasFormulacionesPorNombre_Formulacion", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@detalle", txtBusquedaFormulaciones.Text);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        datalistadoBusquedaFormulaciones.DataSource = dt;
                        con.Close();
                    }
                    if (cboBusquedaFormulaciones.Text == "CÓDIGO")
                    {
                        DataTable dt = new DataTable();
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Cotizacion_MostrarTodasFormulacionesPorCodigo_Formulacion", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@codigo", txtBusquedaFormulaciones.Text);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        datalistadoBusquedaFormulaciones.DataSource = dt;
                        con.Close();
                    }
                    if (cboBusquedaFormulaciones.Text == "C. FORMULACIÓN")
                    {
                        DataTable dt = new DataTable();
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Cotizacion_MostrarTodasFormulacionesPorCodigoFormulacion_Formulacion", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@codigo", txtBusquedaFormulaciones.Text);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        datalistadoBusquedaFormulaciones.DataSource = dt;
                        con.Close();
                    }
                    if (cboBusquedaFormulaciones.Text == "CÓDIGO BSS")
                    {
                        DataTable dt = new DataTable();
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Cotizacion_MostrarTodasFormulacionesPorCodigoBSS_Formulacion", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@codigo", txtBusquedaFormulaciones.Text);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        datalistadoBusquedaFormulaciones.DataSource = dt;
                        con.Close();
                    }
                }
                else
                {
                    if (cboBusquedaFormulaciones.Text == "DESCRIPCIÓN")
                    {
                        DataTable dt = new DataTable();
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Cotizacion_MostrarTodasFormulacionesPorNombre_Producto", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@detalle", txtBusquedaFormulaciones.Text);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        datalistadoBusquedaFormulaciones.DataSource = dt;
                        con.Close();
                    }
                    if (cboBusquedaFormulaciones.Text == "CÓDIGO")
                    {
                        DataTable dt = new DataTable();
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Cotizacion_MostrarTodasFormulacionesPorCodigo_Producto", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@codigo", txtBusquedaFormulaciones.Text);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        datalistadoBusquedaFormulaciones.DataSource = dt;
                        con.Close();
                    }
                    if (cboBusquedaFormulaciones.Text == "C. FORMULACIÓN")
                    {
                        DataTable dt = new DataTable();
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Cotizacion_MostrarTodasFormulacionesPorCodigoFormulacion_Producto", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@codigo", txtBusquedaFormulaciones.Text);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        datalistadoBusquedaFormulaciones.DataSource = dt;
                        con.Close();
                    }
                    if (cboBusquedaFormulaciones.Text == "CÓDIGO BSS")
                    {
                        DataTable dt = new DataTable();
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Cotizacion_MostrarTodasFormulacionesPorCodigoBSS_Producto", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@codigo", txtBusquedaFormulaciones.Text);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        datalistadoBusquedaFormulaciones.DataSource = dt;
                        con.Close();
                    }
                }

                ColorearBoton(datalistadoBusquedaFormulaciones);
                ReordenadoColumnasBusquedaFormulaciones(datalistadoBusquedaFormulaciones);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //FUNCION PARA COLOREAR LOS BOTONES
        public void ColorearBoton(DataGridView DGV)
        {
            //foreach (DataGridViewRow row in DGV.Rows)
            //{
            //    if (row.Cells["VISIBLE"].Value != DBNull.Value)
            //    {
            //        int valorVisible = Convert.ToInt32(row.Cells["VISIBLE"].Value);
            //        DataGridViewCell boton = row.Cells["clumnHabilitado"];

            //        if (valorVisible == 1)
            //        {
            //            boton.Value = "SI";
            //            boton.ReadOnly = false;
            //            boton.Style.BackColor = System.Drawing.Color.LightGreen;
            //        }
            //        else
            //        {
            //            boton.Value = "NO";
            //            boton.ReadOnly = true;
            //            boton.Style.BackColor = System.Drawing.Color.Red;
            //        }
            //    }
            //}
        }

        //REFRESCAR TODAS LAS FORMULACIONESS
        private void btnCargarTodosFormulaciones_Click(object sender, EventArgs e)
        {
            if (cboTipoProducto.Text == "PRODUCTOS FABRICADOS")
            {
                Filtro_MostrarTodasFormulaciones_Formulacion();
            }
            else
            {
                Filtro_MostrarTodasFormulaciones_Producto();
            }
        }

        //REFRESCAR TODAS LAS FORMULACIONESS
        private void cboTipoProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoProducto.Text == "PRODUCTOS FABRICADOS")
            {
                Filtro_MostrarTodasFormulaciones_Formulacion();
            }
            else
            {
                Filtro_MostrarTodasFormulaciones_Producto();
            }
        }

        //EJECUCIONES VARIAS----------------------------------------------------------------------------------------------
        //REORDENAMIENTO DE MIS LISTADO DE BUSQUEDA DE FORULACIONES
        public void ReordenadoColumnasBusquedaFormulaciones(DataGridView DGV)
        {
            DGV.Columns[1].Width = 95;
            DGV.Columns[2].Width = 90;
            DGV.Columns[3].Width = 90;
            DGV.Columns[4].Width = 90;
            DGV.Columns[5].Width = 610;
            DGV.Columns[6].Width = 135;
            DGV.Columns[7].Visible = false;
            alternarColorFilas(DGV);
        }

        //LIMPIEA DE CAMPOS DE MI COTIZACION
        public void LimpiarNuevaCotizacion()
        {
            panelNuevaCotizacion.Visible = false;

            txtBusquedaClientes.Text = "";
            txtNombreCliente.Text = "";
            txtDireccionClente.Text = "";
            txtDocumentoCliente.Text = "";
            txtNombreCliente.Text = "";
            txtDireccionClente.Text = "";

            var DateAndTime = DateTime.Now;
            DateAndTime = DateAndTime.AddDays(+10);
            dateFechaValidez.Value = DateAndTime;

            cboUnidadCliente.DataSource = null;
            cboResponsableCliente.DataSource = null;
            cboContactoCliente.DataSource = null;
            cboCondicionPagoCliente.DataSource = null;
            cboFormaPagoCliente.DataSource = null;

            if (cboComercial.SelectedIndex != -1) { cboComercial.SelectedIndex = 0; }
            if (cboMoneda.SelectedIndex != -1) { cboMoneda.SelectedIndex = 0; }
            if (cboAlmacen.SelectedIndex != -1) { cboAlmacen.SelectedIndex = 0; }

            ckAplicarBrochure.Checked = false;
            cboBrochures.SelectedIndex = 0;

            txtObservaciones.Text = "Campo Opcional";
            txtTiempoEntrega.Text = "Campo Opcional";
            txtGarantia.Text = "Campo Opcional";
            txtSubTotal.Text = "";
            txtDescuento.Text = "";
            txtInafecta.Text = "";
            txtExonerada.Text = "";
            txtIgv.Text = "";
            txtTotalDescuento.Text = "";
            txtTotal.Text = "";

            datalistadoCotizacion.Rows.Clear();
        }

        //FUNCION PARA CALCULAR EL SUBTOTAL
        public void SubTotal(DataGridView DGV, int POSICION, TextBox CAJAMUESTRA)
        {
            double subtotal = 0;
            foreach (DataGridViewRow row in DGV.Rows)
            {
                //6 ES PARA LA COTIZACION - 7 ES PARA EL PEDIDO
                if (row.Cells[POSICION].Value == null)
                    return;
                else
                    subtotal += Convert.ToDouble(row.Cells[POSICION].Value);
            }
            CAJAMUESTRA.Text = String.Format("{0:#,0.00}", subtotal);
        }

        //FUNCION PARA CALCULAR EL DESCUENTO SUB
        public void DescuentoSub(TextBox SUBTOTAL, NumericUpDown SELECDESCUENTO, TextBox CAJAMUESTRA)
        {
            double descuento = 0;
            descuento = ((Convert.ToDouble(SUBTOTAL.Text)) * (Convert.ToDouble(SELECDESCUENTO.Value) / 100));
            CAJAMUESTRA.Text = String.Format("{0:#,0.00}", descuento);
        }

        //FUNCION PARA CALCULAR EL IGV
        public void IGV(TextBox SUBTOTAL, TextBox DESCUENTO, NumericUpDown SELECTIGV, TextBox CAJAMUESTRA)
        {
            Decimal igv;
            igv = ((Convert.ToDecimal(SUBTOTAL.Text) - Convert.ToDecimal(DESCUENTO.Text)) * (SELECTIGV.Value / 100));
            CAJAMUESTRA.Text = String.Format("{0:#,0.00}", igv);
        }

        //FUNCION PARA CALCULAR EL TOTAL
        public void Total(TextBox SUBTOTAL, TextBox DESCUENTO, TextBox IGV, TextBox CAJAMUESTRA)
        {
            double total = 0;
            total = ((Convert.ToDouble(SUBTOTAL.Text) - Convert.ToDouble(DESCUENTO.Text)) + Convert.ToDouble(IGV.Text));
            CAJAMUESTRA.Text = String.Format("{0:#,0.00}", total);
        }

        //FUNCION PARA HACER CLICK EN MI CAJA DE OBSERVACIONES
        private void txtObservaciones_Click(object sender, EventArgs e)
        {
            if (txtObservaciones.Text == "Campo Opcional")
            {
                txtObservaciones.Text = "";
                txtObservaciones.ReadOnly = false;
            }
            else
            {
                txtObservaciones.ReadOnly = false;
            }
        }

        //FUNCION PARA HACER CLICK EN MI CAJA DE LA REFERENCIA
        private void txtReferencia_Click(object sender, EventArgs e)
        {
            if (txtReferencia.Text == "Campo Obligatorio")
            {
                txtReferencia.Text = "";
                txtReferencia.ReadOnly = false;
            }
            else
            {
                txtReferencia.ReadOnly = false;
            }
        }

        //CUANDO SE DESELECCIONA LAS OBSERVACIONES
        private void txtObservaciones_Leave(object sender, EventArgs e)
        {
            if (txtObservaciones.Text == "")
            {
                txtObservaciones.ReadOnly = true;
                txtObservaciones.Text = "Campo Opcional";
                txtObservaciones.ForeColor = SystemColors.WindowText;
            }
        }

        //CUANDO SE DESELECCIONA LAS LA REFERENCIA
        private void txtReferencia_Leave(object sender, EventArgs e)
        {
            if (txtReferencia.Text == "")
            {
                txtReferencia.ReadOnly = true;
                txtReferencia.Text = "Campo Obligatorio";
                txtReferencia.ForeColor = SystemColors.WindowText;
            }
        }

        //ELEGIR ALMACEN DE ARENAS
        private void ckAlmacenArenas_CheckedChanged(object sender, EventArgs e)
        {
            if (ckAlmacenArenas.Checked == true)
            {
                txtLugarEntregado.Text = "Calle El Martillo MZ B Lote 5 Urb. Industrial El Naranjal";
            }
            else
            {
                txtLugarEntregado.ReadOnly = true;
                txtLugarEntregado.Text = "Campo Opcional";
                txtLugarEntregado.ForeColor = SystemColors.WindowText;
            }
        }

        //BUSCAR  LUGAR DE ENTREGA
        private void btnBuscarSucursalF_Click(object sender, EventArgs e)
        {
            if (txtDocumentoCliente.Text != "")
            {
                panelBusquedaSucursal.Visible = true;
                txtClienteBusquedaSucursal.Text = txtNombreCliente.Text;
                txtDocumentoBusquedaSucursal.Text = txtDocumentoCliente.Text;
                BuscarSucursalesXCliente(CodigoCLiente);
            }
            else
            {
                MessageBox.Show("Debe seleccionar a un cliente para poder desplegar las sucursales.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //SELECCIONAR UNA SUCURSAL DE MI CLIENTE
        private void datalistadoSucursalesXCliente_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtLugarEntregado.Text = datalistadoSucursalesXCliente.SelectedCells[2].Value.ToString();
            panelBusquedaSucursal.Visible = false;
        }

        //LIMPIAR LUGAR DE ENTREGA
        private void btnLimpiarLugarEntregaF_Click(object sender, EventArgs e)
        {
            txtLugarEntregado.ReadOnly = true;
            txtLugarEntregado.Text = "Campo Opcional";
            txtLugarEntregado.ForeColor = SystemColors.WindowText;
            ckAlmacenArenas.Checked = false;
        }

        //CEERAR EL PANEL DE BUSQUEDA DE CLIENTES
        private void txtCerrarBusquedaSucursal_Click(object sender, EventArgs e)
        {
            panelBusquedaSucursal.Visible = false;
            txtClienteBusquedaSucursal.Text = "";
            txtDocumentoBusquedaSucursal.Text = "";
        }
        //------------------------------------------------------------------------------------------------

        //---------------------------------------------PEDIDO----------------------------------------------
        //CODIGO PARA GENERAR EL CODIGO DE COTIZACION
        public void CodigoGeneracionPedido()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT IdPedido FROM Pedido WHERE IdPedido = (SELECT MAX(IdPedido) FROM Pedido)", con);
                da.Fill(dt);
                datalistadoCodigoPedido.DataSource = dt;
                con.Close();

                string codigo = "";

                if (datalistadoCodigoPedido.Rows.Count == 0)
                {
                    codigo = "0";
                }
                else
                {
                    codigo = datalistadoCodigoPedido.SelectedCells[0].Value.ToString();
                }

                string anno = DateTime.Now.ToString("yyyy");

                if (codigo.Length == 1)
                {
                    int codigoS = Convert.ToInt32(codigo);
                    codigoS = codigoS + 1;
                    CodigoGeneradoPedido = anno + "0000" + Convert.ToString(codigoS);
                }
                else if (codigo.Length == 2)
                {
                    int codigoS = Convert.ToInt32(codigo);
                    codigoS = codigoS + 1;
                    CodigoGeneradoPedido = anno + "000" + Convert.ToString(codigoS);
                }
                else if (codigo.Length == 3)
                {
                    int codigoS = Convert.ToInt32(codigo);
                    codigoS = codigoS + 1;
                    CodigoGeneradoPedido = anno + "00" + Convert.ToString(codigoS);
                }
                else if (codigo.Length == 4)
                {
                    int codigoS = Convert.ToInt32(codigo);
                    codigoS = codigoS + 1;
                    CodigoGeneradoPedido = anno + "0" + Convert.ToString(codigoS);
                }
                else if (codigo.Length == 5)
                {
                    int codigoS = Convert.ToInt32(codigo);
                    codigoS = codigoS + 1;
                    CodigoGeneradoPedido = anno + Convert.ToString(codigoS);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //GENERAR PEDIDO A MI COTIZACION
        private void btnGenerarPedido_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasCotiaciones.CurrentRow != null)
            {
                codigoCotizacion = int.Parse(datalistadoTodasCotiaciones.Rows[datalistadoTodasCotiaciones.CurrentRow.Index].Cells[1].Value.ToString());

                //BUSCAR ITEMS DE MI COTIZACION
                BuscarCotizacionDetallePorCodigo(codigoCotizacion);

                List<int> estados = new List<int>();
                foreach (DataGridViewRow row in dataListadiCotiDetallesXCodigo.Rows)
                {
                    string idPedido = row.Cells[16].Value.ToString();

                    if (idPedido != "0")
                    {
                        estados.Add(1);
                    }
                }

                //SI LA COTIZACION ESTA ANULADA
                if (datalistadoTodasCotiaciones.SelectedCells[33].Value.ToString() != "ANULADO" || estados.Count > 0)
                {
                    BuscarCotizacionPorCodigo(codigoCotizacion);
                    BuscarCotizacionDetallePorCodigoAdjudicado(codigoCotizacion);

                    CargarUnidad(Convert.ToInt32(dataListadiCotiXCodigo.SelectedCells[4].Value.ToString()), cboUnidadClientePedido);
                    CargarResponsable(Convert.ToInt32(dataListadiCotiXCodigo.SelectedCells[4].Value.ToString()), cboResponsableClientePedido);
                    CargarContacto(Convert.ToInt32(dataListadiCotiXCodigo.SelectedCells[4].Value.ToString()), cboContactoClientePedido);
                    CargarCondicion(Convert.ToInt32(dataListadiCotiXCodigo.SelectedCells[4].Value.ToString()), cboCondicionPagoClientePedido);
                    CargarForma(Convert.ToInt32(dataListadiCotiXCodigo.SelectedCells[4].Value.ToString()), cboFormaPagoClientePedido);
                    CargarMoneda(cboMonedaPedido);
                    CargarAlmacen(cboAlmacenPedido);

                    FechaAhoraPedido.Value = DateTime.Now;
                    FechaPedidoPedido.Value = DateTime.Now;
                    FechaEntregaPedido.Value = DateTime.Now;
                    dateTimeFechaTermino.Value = DateTime.Now;
                    txtCodigoClientePedido.Text = dataListadiCotiXCodigo.SelectedCells[32].Value.ToString();
                    txtClientePedido.Text = dataListadiCotiXCodigo.SelectedCells[5].Value.ToString();
                    txtDireccionCLientePedido.Text = dataListadiCotiXCodigo.SelectedCells[33].Value.ToString();
                    txtLugarEntregaPedido.Text = dataListadiCotiXCodigo.SelectedCells[16].Value.ToString();


                    FechaCotizacionPedido.Text = dataListadiCotiXCodigo.SelectedCells[2].Value.ToString();
                    txtCodigoCotizacionPedido.Text = dataListadiCotiXCodigo.SelectedCells[1].Value.ToString();
                    txtIdCotizacionPedido.Text = dataListadiCotiXCodigo.SelectedCells[0].Value.ToString();

                    cboUnidadClientePedido.SelectedValue = dataListadiCotiXCodigo.SelectedCells[6].Value.ToString();
                    cboResponsableClientePedido.SelectedValue = dataListadiCotiXCodigo.SelectedCells[8].Value.ToString();
                    cboContactoClientePedido.SelectedValue = dataListadiCotiXCodigo.SelectedCells[27].Value.ToString();
                    cboCondicionPagoClientePedido.SelectedValue = dataListadiCotiXCodigo.SelectedCells[29].Value.ToString();
                    cboFormaPagoClientePedido.SelectedValue = dataListadiCotiXCodigo.SelectedCells[28].Value.ToString();
                    cboMonedaPedido.SelectedValue = dataListadiCotiXCodigo.SelectedCells[12].Value.ToString();
                    cboAlmacenPedido.SelectedValue = dataListadiCotiXCodigo.SelectedCells[15].Value.ToString();

                    //DETALLES DEL PEDIDO
                    datalistadoGeneracionPedido.Rows.Clear();
                    try
                    {
                        if (dataListadiCotiDetallesXCodigoAdjudicado.CurrentRow != null)
                        {
                            foreach (DataGridViewRow row in dataListadiCotiDetallesXCodigoAdjudicado.Rows)
                            {
                                string idDetalleCotizacion = row.Cells[0].Value.ToString();
                                string item = row.Cells[15].Value.ToString();
                                string codigoProducto = row.Cells[2].Value.ToString();
                                string descripcion = row.Cells[3].Value.ToString();
                                string cantidad = row.Cells[5].Value.ToString();
                                string preciounidad = row.Cells[6].Value.ToString();
                                string descuento = row.Cells[7].Value.ToString();
                                string total = row.Cells[8].Value.ToString();
                                string idProducto = row.Cells[17].Value.ToString();
                                string codigoFormulacion = row.Cells[4].Value.ToString();

                                datalistadoGeneracionPedido.Rows.Add(new[] { null, item, codigoProducto, descripcion, cantidad, preciounidad, descuento, total, null, idProducto, null, null, codigoFormulacion, idDetalleCotizacion });
                            }
                        }
                        else
                        {
                            MessageBox.Show("No hay items de la cotización para poder generar el pedido respectivo seleccionados.", "Validación del Sistema", MessageBoxButtons.OK);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
                    }

                    alternarColorFilas(datalistadoGeneracionPedido);
                    panelGenerarPedido.Visible = true;
                    panelDetalleitemsCotizacion.Visible = false;
                    FechaEntregaPedido.Value = DateTime.Now;
                    FechaPedidoPedido.Value = DateTime.Now;
                    datalistadoGeneracionPedido.Columns[1].ReadOnly = true;
                    datalistadoGeneracionPedido.Columns[2].ReadOnly = true;
                    datalistadoGeneracionPedido.Columns[3].ReadOnly = true;
                    datalistadoGeneracionPedido.Columns[7].ReadOnly = true;
                    datalistadoGeneracionPedido.Columns[10].ReadOnly = true;
                }
                else
                {
                    MessageBox.Show("La cotización se encuentra anulada o ya se generó un pedido con todos los items.", "Validación del Sistema", MessageBoxButtons.OK);
                }
            }
        }

        //GENERAR PEDIDO A MI COTIZACION - DETALLE
        private void btnGenerarPedidoDetalle_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasCotiaciones.CurrentRow != null)
            {
                int habilitado = 0;
                //VERIFICARSI HAY ITEMS SELECCIOANDOS
                foreach (DataGridViewRow row in datalistadoItemsCotizacion.Rows)
                {
                    bool skPedido = Convert.ToBoolean(row.Cells[7].Value);
                    string estadoPedido = Convert.ToString(row.Cells[8].Value);

                    if (skPedido == true && estadoPedido == "SIN PEDIDO")
                    {
                        habilitado = 1;
                        break;
                    }
                }

                //VALIDAR SI HAY ITEMS SELECCIOANDOS
                if (habilitado == 1)
                {
                    codigoCotizacion = int.Parse(datalistadoTodasCotiaciones.Rows[datalistadoTodasCotiaciones.CurrentRow.Index].Cells[1].Value.ToString());

                    //BUSCAR ITEMS DE MI COTIZACION
                    BuscarCotizacionPorCodigo(codigoCotizacion);

                    CargarUnidad(Convert.ToInt32(dataListadiCotiXCodigo.SelectedCells[4].Value.ToString()), cboUnidadClientePedido);
                    CargarResponsable(Convert.ToInt32(dataListadiCotiXCodigo.SelectedCells[4].Value.ToString()), cboResponsableClientePedido);
                    CargarContacto(Convert.ToInt32(dataListadiCotiXCodigo.SelectedCells[4].Value.ToString()), cboContactoClientePedido);
                    CargarCondicion(Convert.ToInt32(dataListadiCotiXCodigo.SelectedCells[4].Value.ToString()), cboCondicionPagoClientePedido);
                    CargarForma(Convert.ToInt32(dataListadiCotiXCodigo.SelectedCells[4].Value.ToString()), cboFormaPagoClientePedido);
                    CargarMoneda(cboMonedaPedido);
                    CargarAlmacen(cboAlmacenPedido);

                    FechaAhoraPedido.Value = DateTime.Now;
                    FechaPedidoPedido.Value = DateTime.Now;
                    FechaEntregaPedido.Value = DateTime.Now;
                    dateTimeFechaTermino.Value = DateTime.Now;
                    txtCodigoClientePedido.Text = dataListadiCotiXCodigo.SelectedCells[32].Value.ToString();
                    txtClientePedido.Text = dataListadiCotiXCodigo.SelectedCells[5].Value.ToString();
                    txtDireccionCLientePedido.Text = dataListadiCotiXCodigo.SelectedCells[33].Value.ToString();
                    txtLugarEntregaPedido.Text = dataListadiCotiXCodigo.SelectedCells[16].Value.ToString();


                    FechaCotizacionPedido.Text = dataListadiCotiXCodigo.SelectedCells[2].Value.ToString();
                    txtCodigoCotizacionPedido.Text = dataListadiCotiXCodigo.SelectedCells[1].Value.ToString();
                    txtIdCotizacionPedido.Text = dataListadiCotiXCodigo.SelectedCells[0].Value.ToString();

                    cboUnidadClientePedido.SelectedValue = dataListadiCotiXCodigo.SelectedCells[6].Value.ToString();
                    cboResponsableClientePedido.SelectedValue = dataListadiCotiXCodigo.SelectedCells[8].Value.ToString();
                    cboContactoClientePedido.SelectedValue = dataListadiCotiXCodigo.SelectedCells[27].Value.ToString();
                    cboCondicionPagoClientePedido.SelectedValue = dataListadiCotiXCodigo.SelectedCells[29].Value.ToString();
                    cboFormaPagoClientePedido.SelectedValue = dataListadiCotiXCodigo.SelectedCells[28].Value.ToString();
                    cboMonedaPedido.SelectedValue = dataListadiCotiXCodigo.SelectedCells[12].Value.ToString();
                    cboAlmacenPedido.SelectedValue = dataListadiCotiXCodigo.SelectedCells[15].Value.ToString();

                    //DETALLES DEL PEDIDO
                    datalistadoGeneracionPedido.Rows.Clear();
                    try
                    {
                        foreach (DataGridViewRow row in datalistadoItemsCotizacion.Rows)
                        {
                            bool skPedido = Convert.ToBoolean(row.Cells[7].Value);
                            string estadoPedido = Convert.ToString(row.Cells[8].Value);

                            if (skPedido == true && estadoPedido == "SIN PEDIDO")
                            {
                                string idDetalleCotizacion = row.Cells[0].Value.ToString();
                                string item = row.Cells[9].Value.ToString();
                                string codigoProducto = row.Cells[1].Value.ToString();
                                string descripcion = row.Cells[2].Value.ToString();
                                string cantidad = row.Cells[3].Value.ToString();
                                string preciounidad = row.Cells[4].Value.ToString();
                                string descuento = row.Cells[5].Value.ToString();
                                string total = row.Cells[6].Value.ToString();
                                string idProducto = row.Cells[11].Value.ToString();
                                string codigoFormulacion = row.Cells[10].Value.ToString();

                                datalistadoGeneracionPedido.Rows.Add(new[] { null, item, codigoProducto, descripcion, cantidad, preciounidad, descuento, total, null, idProducto, null, null, codigoFormulacion, idDetalleCotizacion });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
                    }

                    alternarColorFilas(datalistadoGeneracionPedido);
                    panelGenerarPedido.Visible = true;
                    panelDetalleitemsCotizacion.Visible = false;
                    FechaEntregaPedido.Value = DateTime.Now;
                    FechaPedidoPedido.Value = DateTime.Now;
                    datalistadoGeneracionPedido.Columns[1].ReadOnly = true;
                    datalistadoGeneracionPedido.Columns[2].ReadOnly = true;
                    datalistadoGeneracionPedido.Columns[3].ReadOnly = true;
                    datalistadoGeneracionPedido.Columns[7].ReadOnly = true;
                    datalistadoGeneracionPedido.Columns[10].ReadOnly = true;
                }
                else
                {
                    MessageBox.Show("Debe seleccionar uno o más items para poder continuar con la generación del pedido", "Validación del SISTEMA", MessageBoxButtons.OK);
                }

            }
        }

        //BOTON PARA SALOR Y REGRESAR DE MI PEDIDO
        private void BtnRegresarGenerarPedido_Click(object sender, EventArgs e)
        {
            RegresarGenerarPedido();
        }

        //FUNCION PARA REGRESAR DE GENERAR PEDIDO
        public void RegresarGenerarPedido()
        {
            panelGenerarPedido.Visible = false;
            LimpiarCamposPedido();
        }

        //LIMPIEZA DE CAMPOS DE PEDIDO
        public void LimpiarCamposPedido()
        {
            //CABECERA
            txtCodigoOrdenCompraPedido.Text = "";
            txtArchivoAdjuntoPedido.Text = "";
            txtObservacionesPedido.Text = "";
            txtDetallePedido.Text = "";
            txtPesoPedido.Text = "0.00";

            //CAMPOS NUMERICOS
            txtSubTotalPedido.Text = "";
            txtDescuentoPedido.Text = "";
            txtInafectaPedido.Text = "";
            txtExoneradaPedido.Text = "";
            txtIgvPedido.Text = "";
            txtTotalDescuentoPedido.Text = "";
            txtTotalPedido.Text = "";
            dateTimeFechaTermino.Value = DateTime.Now;

            //DETALLES
            datalistadoGeneracionPedido.Rows.Clear();
        }

        //GENERACIÓN DEL PEDIDO----------------------------------------------------------------------
        //CARGA DE DOCMENTOS DE LA ORDEN DE COMPRA---------------------
        private void btnCargarPdfPedido_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Todos los archivos (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtArchivoAdjuntoPedido.Text = openFileDialog1.FileName;
            }
        }

        //QUITAR EL DOCUMENTO  QUE INGRESE
        private void btnQuitarArchivoPedido_Click(object sender, EventArgs e)
        {
            txtArchivoAdjuntoPedido.Text = "";
            txtCodigoOrdenCompraPedido.Text = "";
        }

        //FUNCION PARA PODER RECUPERA LA MAYOR FECHA DE MI DATAGRIDVIEW
        private void GetMaxDateFromDataGridView()
        {
            DateTime maxDate = DateTime.MinValue;

            foreach (DataGridViewRow row in datalistadoGeneracionPedido.Rows)
            {
                if (row.Cells["fechaEntrega"].Value != null && DateTime.TryParse(row.Cells["fechaEntrega"].Value.ToString(), out DateTime dateValue))
                {
                    if (dateValue > maxDate)
                    {
                        maxDate = dateValue;
                    }
                }
            }

            if (maxDate != Convert.ToDateTime("1/01/0001 00:00:00"))
            {
                FechaEntregaPedido.Value = maxDate;
            }
        }

        //EDITAR MI PEDIDO, VALORES
        private void datalistadoGeneracionPedido_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //RECALCULOS DE LOS ITEMS-----------------------------------------
            int cantidad = 0;
            decimal precio;
            decimal descuento;
            decimal total = 0;
            //string codigocliente = "";
            //string descripcioncliente = "";

            DataGridViewRow row = (DataGridViewRow)datalistadoGeneracionPedido.Rows[e.RowIndex];

            if (row.Cells[4].Value == null)
            {
                cantidad = Convert.ToInt32("0");
                row.Cells[4].Value = "0";
            }
            else
            {
                cantidad = Convert.ToInt32(row.Cells[4].Value);
            }

            if (row.Cells[5].Value == null)
            {
                precio = Convert.ToDecimal("0.00");
                row.Cells[5].Value = "0.00";
            }
            else
            {
                precio = Convert.ToDecimal(row.Cells[5].Value.ToString());
            }

            if (row.Cells[6].Value == null)
            {
                descuento = Convert.ToDecimal("0.00");
                row.Cells[6].Value = "0.00";
            }
            else
            {
                descuento = Convert.ToDecimal(row.Cells[6].Value.ToString());
            }

            total = (cantidad * precio) - ((cantidad * precio) * (descuento / 100));

            row.Cells[5].Value = String.Format("{0:#,0.00}", precio);
            row.Cells[6].Value = String.Format("{0:#,0.00}", descuento);
            row.Cells[7].Value = String.Format("{0:#,0.00}", total);

            SubTotal(datalistadoGeneracionPedido, 7, txtSubTotalPedido);
            DescuentoSub(txtSubTotalPedido, SelectDescuentoPedido, txtDescuentoPedido);
            IGV(txtSubTotalPedido, txtDescuentoPedido, SelectIgvPedido, txtIgvPedido);
            Total(txtSubTotalPedido, txtDescuentoPedido, txtIgvPedido, txtTotalPedido);

            txtInafectaPedido.Text = String.Format("0.00");
            txtExoneradaPedido.Text = String.Format("0.00");
            txtTotalDescuentoPedido.Text = String.Format("0.00");

            GetMaxDateFromDataGridView();
        }

        //CAMBIAR EL DESCUENTO DE MI PEDIDO
        private void SelectDescuentoPedido_ValueChanged(object sender, EventArgs e)
        {
            SubTotal(datalistadoGeneracionPedido, 7, txtSubTotalPedido);
            DescuentoSub(txtSubTotalPedido, SelectDescuentoPedido, txtDescuentoPedido);
            IGV(txtSubTotalPedido, txtDescuentoPedido, SelectIgvPedido, txtIgvPedido);
            Total(txtSubTotalPedido, txtDescuentoPedido, txtIgvPedido, txtTotalPedido);
        }

        //CAMBIAR EL IGV DE MI PEDIDO
        private void SelectIgvPedido_ValueChanged(object sender, EventArgs e)
        {
            SubTotal(datalistadoGeneracionPedido, 7, txtSubTotalPedido);
            DescuentoSub(txtSubTotalPedido, SelectDescuentoPedido, txtDescuentoPedido);
            IGV(txtSubTotalPedido, txtDescuentoPedido, SelectIgvPedido, txtIgvPedido);
            Total(txtSubTotalPedido, txtDescuentoPedido, txtIgvPedido, txtTotalPedido);
        }

        //MOSTRAR EL PANEL DE FECHA PARA PODER INGRESARLA
        private void datalistadoGeneracionPedido_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 11)
            {
                panelFechaEntrega.Visible = true;
            }
        }

        //BTON PARA SALIR DE LA FECHA DE ENTREGA
        private void btnSalirFechaEntrega_Click(object sender, EventArgs e)
        {
            panelFechaEntrega.Visible = false;
        }

        //BOTON PARA CARGAR LA FECHA DE ENTREGA
        private void btnCargarFechaEntrega_Click(object sender, EventArgs e)
        {
            datalistadoGeneracionPedido.CurrentRow.Cells[10].Value = dateTimeFechaTermino.Text;
            panelFechaEntrega.Visible = false;
            GetMaxDateFromDataGridView();
        }

        //HABILITAR EL INGRESO DE TEXTO
        private void txtObservacionesPedido_Click(object sender, EventArgs e)
        {
            if (txtObservacionesPedido.Text == "Campo Opcional")
            {
                txtObservacionesPedido.Text = "";
                txtObservacionesPedido.ReadOnly = false;
            }
            else
            {
                txtObservacionesPedido.ReadOnly = false;
            }
        }

        //DESABILITAR LAS OBSERVACIONES
        private void txtObservacionesPedido_Leave(object sender, EventArgs e)
        {
            if (txtObservacionesPedido.Text == "")
            {
                txtObservacionesPedido.ReadOnly = true;
                txtObservacionesPedido.Text = "Campo Opcional";
                txtObservacionesPedido.ForeColor = SystemColors.WindowText;
            }
        }

        //HABILITAR EL INGRESO DE TEXTO
        private void txtDetallePedido_Click(object sender, EventArgs e)
        {
            if (txtDetallePedido.Text == "Campo Opcional")
            {
                txtDetallePedido.Text = "";
                txtDetallePedido.ReadOnly = false;
            }
            else
            {
                txtDetallePedido.ReadOnly = false;
            }
        }

        //DESHABILITAR EL DETALLE DE PEDIDO
        private void txtDetallePedido_Leave(object sender, EventArgs e)
        {
            if (txtDetallePedido.Text == "")
            {
                txtDetallePedido.ReadOnly = true;
                txtDetallePedido.Text = "Campo Opcional";
                txtDetallePedido.ForeColor = SystemColors.WindowText;
            }
        }

        //HABILITAR EL INGRESO DE TEXTO
        private void txtGarantia_Click(object sender, EventArgs e)
        {
            if (txtGarantia.Text == "Campo Opcional")
            {
                txtGarantia.Text = "";
                txtGarantia.ReadOnly = false;
            }
            else
            {
                txtGarantia.ReadOnly = false;
            }
        }

        //DESHABILITAR 
        private void txtGarantia_Leave(object sender, EventArgs e)
        {
            if (txtGarantia.Text == "")
            {
                txtGarantia.ReadOnly = true;
                txtGarantia.Text = "Campo Opcional";
                txtGarantia.ForeColor = SystemColors.WindowText;
            }
        }

        //HABILITAR EL INGRESO DE TEXTO
        private void txtTiempoEntrega_Click(object sender, EventArgs e)
        {
            if (txtTiempoEntrega.Text == "Campo Opcional")
            {
                txtTiempoEntrega.Text = "";
                txtTiempoEntrega.ReadOnly = false;
            }
            else
            {
                txtTiempoEntrega.ReadOnly = false;
            }
        }

        //DESHABILITAR EL DETALLE DE PEDIDO
        private void txtTiempoEntrega_Leave(object sender, EventArgs e)
        {
            if (txtTiempoEntrega.Text == "")
            {
                txtTiempoEntrega.ReadOnly = true;
                txtTiempoEntrega.Text = "Campo Opcional";
                txtTiempoEntrega.ForeColor = SystemColors.WindowText;
            }
        }

        //BOTON PARA GUARADR EL MI PEDIDO Y GENERAR EL DOCUMENTO RESPECTIVO
        private void btnGuardarPedido_Click(object sender, EventArgs e)
        {
            if (txtTotalPedido.Text == "0" || txtTotalPedido.Text == "" || txtSubTotalPedido.Text == "0" || txtSubTotalPedido.Text == "" || datalistadoGeneracionPedido.Rows.Count == 0)
            {
                MessageBox.Show("Debe ingresar o cargar el subtotal y total o debe seleccionar los respectivos productos.", "Validación del Sistema");
            }
            else
            {
                DialogResult boton = MessageBox.Show("¿Realmente desea guardar este pedido con estos Items?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    bool sinFecha = false;

                    //VALIDAR SI SE INGRESARON FECHAS
                    foreach (DataGridViewRow row in datalistadoGeneracionPedido.Rows)
                    {
                        DateTime fechaInicio = Convert.ToDateTime(row.Cells["fechaEntrega"].Value);

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
                        cmd = new SqlCommand("Cotizacion_InsertarPedido", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        CodigoGeneracionPedido();
                        int cantidaditems = datalistadoGeneracionPedido.RowCount;

                        cmd.Parameters.AddWithValue("@codigoPedido", Convert.ToInt32(CodigoGeneradoPedido));
                        cmd.Parameters.AddWithValue("@fechaPedido", FechaPedidoPedido.Value);
                        cmd.Parameters.AddWithValue("@idCliente", datalistadoTodasCotiaciones.SelectedCells[5].Value.ToString());
                        cmd.Parameters.AddWithValue("@direccion", txtDireccionCLientePedido.Text);
                        cmd.Parameters.AddWithValue("@lugarEntrega", txtLugarEntregaPedido.Text);
                        cmd.Parameters.AddWithValue("@idUnidad", cboUnidadClientePedido.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@idResponsable", cboResponsableClientePedido.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@idContacto", cboContactoClientePedido.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@idCondicion", cboCondicionPagoClientePedido.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@idFormaPago", cboFormaPagoClientePedido.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@idMoneda", cboMonedaPedido.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@idAlmacen", cboAlmacenPedido.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@fechaEntrega", FechaEntregaPedido.Value);
                        cmd.Parameters.AddWithValue("@peso", Convert.ToDecimal(txtPesoPedido.Text));

                        if (txtArchivoAdjuntoPedido.Text != "")
                        {
                            string NombreGenerado = "ORDEN DE COMPRA " + txtCodigoOrdenCompraPedido.Text + " - PEDIDO " + CodigoGeneradoPedido;
                            string RutaOld = txtArchivoAdjuntoPedido.Text;
                            string RutaNew = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Comercial\OrdenCompraPedido\" + NombreGenerado + ".pdf";
                            File.Copy(RutaOld, RutaNew);
                            cmd.Parameters.AddWithValue("@ordenCompra", txtCodigoOrdenCompraPedido.Text);
                            cmd.Parameters.AddWithValue("@rutaOrdenCompra", RutaNew);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@ordenCompra", "");
                            cmd.Parameters.AddWithValue("@rutaOrdenCompra", "");
                        }

                        cmd.Parameters.AddWithValue("@observaciones", txtObservacionesPedido.Text);
                        cmd.Parameters.AddWithValue("@detallePedido", txtDetallePedido.Text);
                        cmd.Parameters.AddWithValue("@subTotal", Convert.ToDecimal(txtSubTotalPedido.Text));
                        cmd.Parameters.AddWithValue("@descuento", Convert.ToDecimal(txtDescuentoPedido.Text));
                        cmd.Parameters.AddWithValue("@inafecta", Convert.ToDecimal(txtInafectaPedido.Text));
                        cmd.Parameters.AddWithValue("@exonerado", Convert.ToDecimal(txtExoneradaPedido.Text));
                        cmd.Parameters.AddWithValue("@IGV", Convert.ToDecimal(txtIgvPedido.Text));
                        cmd.Parameters.AddWithValue("@totalDescuento", Convert.ToDecimal(txtTotalDescuentoPedido.Text));
                        cmd.Parameters.AddWithValue("@total", Convert.ToDecimal(txtTotalPedido.Text));
                        cmd.Parameters.AddWithValue("@idCotizacion", txtIdCotizacionPedido.Text);
                        cmd.Parameters.AddWithValue("@cantidadItems", cantidaditems);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Se registró el nuevo pedido.", "Validación del Sistema");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
                    }

                    //CAMBIO DE ESTADO--------
                    try
                    {
                        CodigoPedido();
                        int codigopedido = Convert.ToInt32(datalistadoCodigoPedido.SelectedCells[0].Value.ToString());
                        int contador = 0;


                        foreach (DataGridViewRow fila in datalistadoGeneracionPedido.Rows)
                        {

                            contador = contador + 1;

                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("Cotizacion_InsertarDetallePedido", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@idPedido", codigopedido);
                            cmd.Parameters.AddWithValue("@codigoproducto", fila.Cells[2].Value.ToString());
                            cmd.Parameters.AddWithValue("@descripcionProducto", fila.Cells[3].Value.ToString());
                            cmd.Parameters.AddWithValue("@cantidad", Convert.ToInt32(fila.Cells[4].Value.ToString()));
                            cmd.Parameters.AddWithValue("@preciounidad", Convert.ToDecimal(fila.Cells[5].Value.ToString()));
                            cmd.Parameters.AddWithValue("@descuento", Convert.ToDecimal(fila.Cells[6].Value.ToString()));
                            cmd.Parameters.AddWithValue("@total", Convert.ToDecimal(fila.Cells[7].Value.ToString()));
                            cmd.Parameters.AddWithValue("@fechaEntrega", Convert.ToDateTime(fila.Cells[10].Value.ToString()));
                            cmd.Parameters.AddWithValue("@codigoFormulacion", fila.Cells[12].Value.ToString());
                            cmd.Parameters.AddWithValue("@item", contador);
                            cmd.Parameters.AddWithValue("@idDetalleCotizacion", fila.Cells[13].Value.ToString());
                            cmd.Parameters.AddWithValue("@idArt", fila.Cells[9].Value.ToString());
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }

                        ClassResourses.RegistrarAuditora(1, this.Name, 2, Program.IdUsuario, "Generación del pedido", codigopedido);

                        foreach (DataGridViewRow row in datalistadoGeneracionPedido.Rows)
                        {
                            contador = 0;
                            contador = contador + 1;

                            int codigodetallecotizacion = Convert.ToInt32(row.Cells["idDetallePedido"].Value);

                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("Cotizacion_CambiarEstadoDetalle", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idDetalleItems", codigodetallecotizacion);
                            cmd.Parameters.AddWithValue("@idPedido", codigopedido);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }

                        RegresarGenerarPedido();
                        CambioEstadoCotizacion();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
                    }
                }
            }
        }

        public void CambioEstadoCotizacion()
        {
            //VARIABLES PARA LA VALIDACIÓN
            int estadoModificacion = 0;
            var DateAndTime = DateTime.Now;
            bool vencido = false;

            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            BuscarCotizacionPorCodigo(codigoCotizacion);
            panelDetalleitemsCotizacion.Visible = false;
            estadoModificacion = Convert.ToInt16(dataListadiCotiXCodigo.SelectedCells[31].Value.ToString());
            DateTime fechaValidez = Convert.ToDateTime(dataListadiCotiXCodigo.SelectedCells[3].Value);
            int estadoFinal1 = 0;
            int estadoFinal2 = 0;
            List<int> estadoss = new List<int>();

            //VALIDAR SI LA COTIZACION YA EXPIRO
            if (fechaValidez < DateAndTime && datalistadoTodasCotiaciones.SelectedCells[33].Value.ToString() == "PENDIENTE")
            {
                vencido = true;
            }

            //VALIDAR CUANTOS ITEMS SE HAN MARCADO
            foreach (DataGridViewRow datorecuperado in datalistadoItemsCotizacion.Rows)
            {
                int idItemCoti = Convert.ToInt32(datorecuperado.Cells["IdDetalleCotizacion"].Value);
                bool estadoItems = Convert.ToBoolean(datorecuperado.Cells["ESTADO ITEM"].Value);

                try
                {
                    if (estadoItems == true)
                    {
                        estadoss.Add(1);

                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        cmd = new SqlCommand("Cotizacion_CambiarEstadoItems", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idItemCoti", idItemCoti);
                        cmd.Parameters.AddWithValue("@estado", 1);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    else
                    {
                        estadoss.Add(0);

                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        cmd = new SqlCommand("Cotizacion_CambiarEstadoItems", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idItemCoti", idItemCoti);
                        cmd.Parameters.AddWithValue("@estado", 0);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en la operación por: " + ex.Message);
                    ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
                }
            }

            //HACER UNA SUMA SIMPLE CON LA CANTIDAD DE ITEMS MARCADOS
            foreach (var n in estadoss)
            {
                if (int.Equals(1, n))
                {
                    estadoFinal1 = estadoFinal1 + 1;
                }

                if (int.Equals(0, n))
                {
                    estadoFinal2 = estadoFinal2 + 1;
                }
            }

            try
            {
                //COTIZACION VENCIDA
                if (vencido == true)
                {
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    cmd = new SqlCommand("Cotizacion_CambiarEstado", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idcoti", codigoCotizacion);
                    cmd.Parameters.AddWithValue("@estadoCoti", 1);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                //COTIZACION COMPLETA
                else if (estadoFinal1 > 0 && estadoFinal2 == 0 && vencido == false)
                {
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    cmd = new SqlCommand("Cotizacion_CambiarEstado", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idcoti", codigoCotizacion);
                    cmd.Parameters.AddWithValue("@estadoCoti", 4);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                //COTIZAXION CON PARTE DE LOS ITEMS PENDIENTES
                else if (estadoFinal1 > 0 && estadoFinal2 > 0 && vencido == false)
                {
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    cmd = new SqlCommand("Cotizacion_CambiarEstado", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idcoti", codigoCotizacion);
                    cmd.Parameters.AddWithValue("@estadoCoti", 3);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                //COTIZACION CON TODOS LOS ITEMS PENDIENTES
                else if (estadoFinal1 == 0 && vencido == false)
                {
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    cmd = new SqlCommand("Cotizacion_CambiarEstado", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idcoti", codigoCotizacion);
                    cmd.Parameters.AddWithValue("@estadoCoti", 2);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }

            CargarCotizaciones(DesdeFecha.Value, HastaFecha.Value);
        }

        //METODO PARA EXPORTAR A EXCEL MI LISTADO
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
            sl.SetColumnWidth(6, 35);
            sl.SetColumnWidth(7, 35);
            sl.SetColumnWidth(8, 30);
            sl.SetColumnWidth(9, 20);
            sl.SetColumnWidth(10, 20);
            sl.SetColumnWidth(11, 20);
            sl.SetColumnWidth(12, 20);
            sl.SetColumnWidth(13, 20);
            sl.SetColumnWidth(14, 20);
            sl.SetColumnWidth(15, 20);
            sl.SetColumnWidth(16, 20);
            sl.SetColumnWidth(17, 30);


            //CABECERA
            style.Font.FontSize = 11;
            style.Font.Bold = true;
            //style.Alignment.Horizontal = HorizontalAlignmentValues.Center;
            style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.Beige, System.Drawing.Color.Beige);
            style.Border.LeftBorder.BorderStyle = BorderStyleValues.Hair;
            style.Border.RightBorder.BorderStyle = BorderStyleValues.Hair;
            style.Border.BottomBorder.BorderStyle = BorderStyleValues.Hair;
            style.Border.TopBorder.BorderStyle = BorderStyleValues.Hair;

            //FILAS
            styleC.Font.FontSize = 10;
            //styleC.Alignment.Horizontal = HorizontalAlignmentValues.Center;

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
                sl.SetCellValue(ir, 14, row.Cells[13].Value.ToString());
                sl.SetCellValue(ir, 15, row.Cells[14].Value.ToString());
                sl.SetCellValue(ir, 16, row.Cells[15].Value.ToString());
                sl.SetCellValue(ir, 17, row.Cells[16].Value.ToString());
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
                sl.SetCellStyle(ir, 14, styleC);
                sl.SetCellStyle(ir, 15, styleC);
                sl.SetCellStyle(ir, 16, styleC);
                sl.SetCellStyle(ir, 17, styleC);
                ir++;
            }

            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                sl.SaveAs(desktopPath + @"\Reporte de cotizaciones.xlsx");
                MessageBox.Show("Se exportó los datos a un archivo de Microsoft Excel en la siguiente ubicación: " + desktopPath, "Validación del Sistema", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //FUNCION PARA EXPORTAR  EL PDF A MI ESCRITORIO
        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                //Crear una instancia del reporte
                ReportDocument crystalReport = new ReportDocument();

                // Ruta del reporte .rpt
                //string rutaBase = Application.StartupPath;
                string rutaBase = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Recursos y Programas\";
                string rutaReporte = "";

                if (datalistadoTodasCotiaciones.SelectedCells[33].Value.ToString() == "ANULADO")
                {
                    rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeCotizacionVentaAnulada.rpt");
                }
                else
                {
                    rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeCotizacionVenta.rpt");
                }

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
                int IdCotizacion = Convert.ToInt32(datalistadoTodasCotiaciones.SelectedCells[1].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string CodigoCotizacion = Convert.ToString(datalistadoTodasCotiaciones.SelectedCells[2].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string Cliente = datalistadoTodasCotiaciones.SelectedCells[6].Value.ToString(); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                DateTime FechaCreacion = Convert.ToDateTime(datalistadoTodasCotiaciones.SelectedCells[3].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string FecahCreacionFormart = FechaCreacion.ToString("dd-MM-yy");
                crystalReport.SetParameterValue("@idCotizacion", IdCotizacion);

                // Ruta de salida en el escritorio
                string rutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string rutaSalida = System.IO.Path.Combine(rutaEscritorio, "COTIZACIÓN N " + CodigoCotizacion + "-" + Cliente + "-" + FecahCreacionFormart + ".pdf");

                // Exportar a PDF
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaSalida);

                MessageBox.Show($"Reporte exportado correctamente a: {rutaSalida}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al exportar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //BLOQUEAR LA OPCIÓN DE BUSQUEDA POR HISTORIA
        private void btnHistorialCotizaciones_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Función no disponible.", "Validación del Sistema");
        }

        //BOTON PARA ABRIR EL MANAUL
        private void btnInfoPedido_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //BOTON PARA ABRIR EL MANAUL
        private void btnInfoBusquedaFormulacion_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //BOTON PARA ABRIR EL MANAUL
        private void btnInfoDetalleFormulacion_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //BOTON PARA ABRIR EL MANAUL
        private void btnInfoNuevaCotizacion_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //BOTON PARA ABRIR EL MANAUL
        private void btnInfoCoti_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //VALIDACIO DE SOLO INGRESO DE NUMEROS O LETRAS NELAS COULMAS CORRECTAS
        private void DataGridViewTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Obtener el índice de la columna actual
            int columnIndex = datalistadoCotizacion.CurrentCell.ColumnIndex;

            // Permitir solo números, puntos, comas y teclas de control (como retroceso) en todas las columnas excepto la 8 y 9
            if (columnIndex != 12 && columnIndex != 13)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
                {
                    e.Handled = true;
                }
            }
        }

        //VALIDACIÓN DE SOLO NÚMEROS - DETALLES DE MI COTIZACION
        private void datalistadoCotizacion_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox textBox)
            {
                textBox.KeyPress -= DataGridViewTextBox_KeyPress;
                textBox.KeyPress += DataGridViewTextBox_KeyPress;
            }
        }

        //SOLO PERMITE INRGRESO DE NUEMROS
        private void txtPesoPedido_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, puntos, comas y teclas de control (como retroceso)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }
        }

        //CARGAR COLORES DE MIS LISTADOS
        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CARGAR EL MÉTODO QUE COLOREA LAS FILAS
            CargarColoresListadoCotizacionesGeneral(datalistadoTodasCotiaciones);
            CargarColoresListadoCotizacionesGeneral(datalistadoTodasCotiacionesPendientes);
            CargarColoresListadoCotizacionesGeneral(datalistadoTodasCotiacionesParcial);
            CargarColoresListadoCotizacionesGeneral(datalistadoTodasCotiacionesCompletado);
            CargarColoresListadoCotizacionesGeneral(datalistadoTodasCotiacionesVencidos);
        }
    }
}
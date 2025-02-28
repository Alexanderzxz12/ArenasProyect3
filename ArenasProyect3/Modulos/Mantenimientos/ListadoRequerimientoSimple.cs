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
using SpreadsheetLight;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using ArenasProyect3.Modulos.ManGeneral;
using System.Diagnostics;

namespace ArenasProyect3.Modulos.Mantenimientos
{
    public partial class ListadoRequerimientoSimple : Form
    {
        //VARIABLES GLOBALES
        int idJefatura = 0;
        string alias = "";
        private Cursor curAnterior = null;
        string ruta = ManGeneral.Manual.manualRequerimientosSimples;

        //CONSTRUCTRO DE MI FORMULAROI
        public ListadoRequerimientoSimple()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI REQUERIMEINTO SIMPLE
        private void ListadoRequerimientoSimple_Load(object sender, EventArgs e)
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

        //CARGAR JEFATURA Y RECONOCER EL TIPO DE USUARIO PARA LA APROBACIÓN Y ANULACIÓN
        public void CargarJefaturaActual()
        {
            //SI EL ÁREA DEL USUARIO ES ADMINISTRADOR
            if (Program.AreaUsuario == "Administrador")
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT USU.IdUsuarios, R.Alias FROM Usuarios USU INNER JOIN Perfil R ON R.IdPerfil = USU.Rol WHERE USU.IdUsuarios = @idusuario ", con);
                comando.Parameters.AddWithValue("@idusuario", Program.IdUsuario);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                datalistadoJefatura.DataSource = dt;
                con.Close();
                //CARGAR EL CÓDIGO DEL USUARIO Y SU ALIAS O CARGO
                idJefatura = Convert.ToInt32(datalistadoJefatura.SelectedCells[0].Value.ToString());
                alias = datalistadoJefatura.SelectedCells[1].Value.ToString();
            }
            //SI EL ÁREA DEL USUARIO ES DIFERENTE, OSEA ES UN USUARIO PERTENECE AL ÁREA COMERCIAL
            else
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT USU.IdUsuarios, R.Alias FROM Usuarios USU INNER JOIN Perfil R ON R.IdPerfil = USU.Rol WHERE Rol = 1", con);
                da.Fill(dt);
                datalistadoJefatura.DataSource = dt;
                con.Close();
                //CARGAR EL CÓDIGO DEL USUARIO Y SU ALIAS O CARGO
                idJefatura = Convert.ToInt32(datalistadoJefatura.SelectedCells[0].Value.ToString());
                alias = datalistadoJefatura.SelectedCells[1].Value.ToString();
            }
        }

        //BUSCAR DETALLES DE MI REQUERIMIENTO PARA CARGARLO
        public void BuscarDetallesRequerimiento(int codigoRequerimientoSimple)
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
            datallistadoDetalles.DataSource = dt;
            con.Close();
        }

        //LISTAR TODOS LOS PRODUTOS PARA SELECCIONAR EN MI REQUERIMIENTO
        public void MostrarProductosRequerimientoGeneral()
        {
            //PROCEDIMIENTO ALMACENADO PARA LISTAR LOS PRODUCTOS
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("ListarProductosRequerimientoGeneral_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoBusquedaProducto.DataSource = dt;
            con.Close();
            Rediemnsion(datalistadoBusquedaProducto);
        }

        //FUNCION DE REDIEMNSION
        public void Rediemnsion(DataGridView DGV)
        {
            //SE REDIMENSIONA EL TAMAÑO DE CADA COLUMNA DE MI LISTADO DE PRODUCTOS
            DGV.Columns[1].Width = 100;
            DGV.Columns[2].Width = 520;
            DGV.Columns[3].Width = 150;
            DGV.Columns[8].Width = 88;
            DGV.Columns[9].Width = 80;
            DGV.Columns[10].Width = 87;
            //SE QUITA LAS COLUMNAS QUE NO SON RELEVANTES PARA EL USUARIO
            DGV.Columns[4].Visible = false;
            DGV.Columns[5].Visible = false;
            DGV.Columns[6].Visible = false;
            DGV.Columns[7].Visible = false;
            DGV.Columns[11].Visible = false;

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
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
                datalistadoDetallesRequerimiento.DataSource = dt;
                con.Close();
                //NO MOSTRAR LAS COLUMNAS QUE NO SEAN DE REELEVANCIA PARA EL USUARIO
                datalistadoDetallesRequerimiento.Columns[1].Visible = false;
                datalistadoDetallesRequerimiento.Columns[8].Visible = false;
                //REDIMENSIONAR LAS COLUMNAS SEGUN EL TEMAÑO REQUERIDO
                datalistadoDetallesRequerimiento.Columns[0].Width = 70;
                datalistadoDetallesRequerimiento.Columns[2].Width = 100;
                datalistadoDetallesRequerimiento.Columns[3].Width = 250;
                datalistadoDetallesRequerimiento.Columns[4].Width = 100;
                datalistadoDetallesRequerimiento.Columns[5].Width = 90;
                datalistadoDetallesRequerimiento.Columns[6].Width = 90;
                datalistadoDetallesRequerimiento.Columns[7].Width = 90;
                datalistadoDetallesRequerimiento.Columns[9].Width = 110;
                //CARGAR METODO PARA COLOREAR
                ColoresListadoItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------


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
                cmd = new SqlCommand("MostrarRequerimientoSimplePorFecha1_Jefatura", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoRequerimiento.DataSource = dt;
                con.Close();
                RedimensionRequeSimple(datalistadoRequerimiento);
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

        //MOSTRAR REQUERIMIENTOS POR NÚMERO DE REQUERIMIENTO
        public void MostrarRequerimientoPorCodigo(DateTime fechaInicio, DateTime fechaTermino, string codigo)
        {
            if (lblCarga.Text == "0")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarRequerimientoSimplePorCodigo1_Jefatura", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                cmd.Parameters.AddWithValue("@codigo", codigo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoRequerimiento.DataSource = dt;
                con.Close();
                RedimensionRequeSimple(datalistadoRequerimiento);
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
        public void MostrarRequerimientoPorSolicitante(DateTime fechaInicio, DateTime fechaTermino, string solicitante)
        {
            if (lblCarga.Text == "0")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarRequerimientoSimplePorSolicitante1_Jefatura", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                cmd.Parameters.AddWithValue("@solicitante", solicitante);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoRequerimiento.DataSource = dt;
                con.Close();
                RedimensionRequeSimple(datalistadoRequerimiento);
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

        //MOSTRAR REQUERIMIENTOS POR ÁREA
        public void MostrarRequerimientoPorArea(DateTime fechaInicio, DateTime fechaTermino, string area)
        {
            if (lblCarga.Text == "0")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarRequerimientoSimplePorArea1_Jefatura", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                cmd.Parameters.AddWithValue("@area", area);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoRequerimiento.DataSource = dt;
                con.Close();
                RedimensionRequeSimple(datalistadoRequerimiento);
            }
            else
            {
                lblCarga.Text = "0";
            }
        }

        public void RedimensionRequeSimple(DataGridView DGV)
        {
            //NO MOSTRAR LAS COLUMNAS QUE NO SEAN DE REELEVANCIA PARA EL USUARIO
            DGV.Columns[1].Visible = false;
            DGV.Columns[2].Visible = false;
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
            DGV.Columns[3].Width = 110;
            DGV.Columns[4].Width = 95;
            DGV.Columns[5].Width = 95;
            DGV.Columns[6].Width = 250;
            DGV.Columns[8].Width = 250;
            DGV.Columns[10].Width = 200;
            DGV.Columns[12].Width = 200;
            DGV.Columns[13].Width = 100;
            //CARGAR EL MÉTODO QUE COLOREA LAS FILAS
            ColoresListado();

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //COLOREAR REGISTROS
        public void ColoresListado()
        {
            try
            {
                for (var i = 0; i <= datalistadoRequerimiento.RowCount - 1; i++)
                {
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
                for (var i = 0; i <= datalistadoDetallesRequerimiento.RowCount - 1; i++)
                {
                    decimal cantidadTotal = 0;
                    cantidadTotal = Convert.ToDecimal(datalistadoDetallesRequerimiento.Rows[i].Cells[5].Value.ToString());
                    decimal cantidadRetirada = 0;
                    cantidadRetirada = Convert.ToDecimal(datalistadoDetallesRequerimiento.Rows[i].Cells[6].Value.ToString());
                    decimal resultadoRestante = 0;

                    resultadoRestante = cantidadTotal - cantidadRetirada;

                    if (resultadoRestante > Convert.ToDecimal(datalistadoDetallesRequerimiento.Rows[i].Cells[7].Value.ToString()))
                    {
                        //PRODUCTOS SIN STOCK
                        datalistadoDetallesRequerimiento.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                    }
                    if (resultadoRestante < Convert.ToDecimal(datalistadoDetallesRequerimiento.Rows[i].Cells[7].Value.ToString()))
                    {
                        //PRODUCTOS CON STOCK
                        datalistadoDetallesRequerimiento.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    if (datalistadoDetallesRequerimiento.Rows[i].Cells[9].Value.ToString() == "ENTREGADO")
                    {
                        //PRODUCTOS ENTREGADO
                        datalistadoDetallesRequerimiento.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //VISULIZAR EL PDF DEL REQUERIMEINTO SIMPLE
        private void btnVerReque_Click(object sender, EventArgs e)
        {
            if (datalistadoRequerimiento.CurrentRow != null)
            {
                if (datalistadoRequerimiento.SelectedCells[13].Value.ToString() == "ANULADO")
                {
                    string codigoActaReporte = datalistadoRequerimiento.Rows[datalistadoRequerimiento.CurrentRow.Index].Cells[1].Value.ToString();
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

        //MOSTRAR TODOS LOS REQUERIMIENTOS SEGÚN LA FECHA
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarRequerimientoPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR TODOS LOS REQUERIMIENTOS SEGÚN EL NÚMERO DE REQUERIMIENTO
        private void txtBusquedaNumeroRequerimiento_TextChanged(object sender, EventArgs e)
        {
            MostrarRequerimientoPorCodigo(DesdeFecha.Value, HastaFecha.Value, txtBusquedaNumeroRequerimiento.Text);

        }

        //MOSTRAR TODOS LOS REQIERIMIENTOS SEGÚN EL ÁREA
        private void txtBusquedaArea_TextChanged(object sender, EventArgs e)
        {
            MostrarRequerimientoPorArea(DesdeFecha.Value, HastaFecha.Value, txtBusquedaArea.Text);
        }

        //MOSTRAR TODOS LOS REQUERIMEINTOS SEGÚN EL SOLICITANTE DE ESTOS
        private void txtBusquedaSolicitante_TextChanged(object sender, EventArgs e)
        {
            MostrarRequerimientoPorSolicitante(DesdeFecha.Value, HastaFecha.Value, txtBusquedaSolicitante.Text);

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

        //HACER QUE RESALTE EL CURSOR AL MOMENTO DE PASAR SOBRE EL BOTÓN
        private void datalistadoRequerimiento_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoRequerimiento.Columns[e.ColumnIndex].Name == "Seleccionar")
            {
                this.datalistadoRequerimiento.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoRequerimiento.Cursor = curAnterior;
            }
        }

        //ANULAR REQUERIMIENTO SIMPLE
        private void btnDesaprobarReque_Click(object sender, EventArgs e)
        {
            //SI NO HAY UN REQUERIMIENTO SELECCIOANOD
            if (datalistadoRequerimiento.CurrentRow != null)
            {
                //CAPTURAR EL NOMBRE DE USUARIO
                string usuarioEncargado = datalistadoRequerimiento.SelectedCells[8].Value.ToString();

                //SI EL USUARIO LOGEADO ES IGUAL AL USUARIO ENCARGADO DE DEL REQUERIMINTO
                if (usuarioEncargado == Program.NombreUsuarioCompleto)
                {
                    panleAnulacion.Visible = true;
                }
                else
                {
                    MessageBox.Show("El usuario que desea anular este requerimiento debe ser el mismo que lo ha creado.", "Validación del Sistema");
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un requerimiento para poder anularlo.", "Validación del Sistema");
            }
        }

        //ACCCION PARA PROCEDER A ANULAR MI REQUERIMEINTO
        private void btnProcederAnulacion_Click(object sender, EventArgs e)
        {
            //SI NO HAY UN REQUERIMIENTO SELECCIOANOD
            if (txtJustificacionAnulacion.Text != "")
            {
                //MENSAJE DE CONFIRMACIÓN PARA LA ANLACIÓN DE UN REQUERIMIENTO
                DialogResult boton = MessageBox.Show("¿Realmente desea anular este requerimiento?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    //RECOPILACIÓN DE VARIABLES
                    int idRequerimiento = Convert.ToInt32(datalistadoRequerimiento.SelectedCells[1].Value.ToString());
                    string estadoRequerimiento = datalistadoRequerimiento.SelectedCells[13].Value.ToString();

                    //SI EL ESTADO DE MI REQUERIMIENTO ES 
                    if (estadoRequerimiento == "ANULADO" || estadoRequerimiento == "ATENDIDO PARCIAL" || estadoRequerimiento == "ATENDIDO TOTAL" || estadoRequerimiento == "OC EN CURSO")
                    {
                        MessageBox.Show("Este requerimiento ya está anulado, se ha generado una orden de compra, atendido parcialmente o atendido totalmente.", "Validación del Sistema");
                        txtJustificacionAnulacion.Text = "";
                        panleAnulacion.Visible = false;
                    }
                    else
                    {
                        //CARGAR FUNCIÓN PARA RECUPERAR A LA JEFATURA O AL USUARIO ADMINISTRADOR
                        CargarJefaturaActual();

                        try
                        {
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("CambioEstadoRequerimientoSimple_Jefatura", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idRequerimientoSimple", idRequerimiento);
                            cmd.Parameters.AddWithValue("@estado", 0);
                            cmd.Parameters.AddWithValue("@mensajeAnulacion", txtJustificacionAnulacion.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Requerimiento anulado exitosamente.", "Validación del Sistema");

                            MostrarRequerimientoPorFecha(DesdeFecha.Value, HastaFecha.Value);
                            panleAnulacion.Visible = false;
                            txtJustificacionAnulacion.Text = "";
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
                MessageBox.Show("Debe ingresar una justificación para poder anular este requerimiento.", "Validación del Sistema");
            }
        }

        //MÉTODO PARA SALIR DE LA ANULACIÓN DE UN REQUERIMIENTO
        private void btnRetrocederAnulacion_Click(object sender, EventArgs e)
        {
            txtJustificacionAnulacion.Text = "";
            panleAnulacion.Visible = false;
        }

        //SELECCIONAR LOS DETALLES DE MI REQUERIMIENT
        private void datalistadoRequerimiento_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoRequerimiento.Columns[e.ColumnIndex];

            //SI SE PRECIONA SOBRE LA COLUMNA CON EL NOMBRE SELECCIOANDO
            if (currentColumn.Name == "Seleccionar")
            {
                //SI NO HAY UN REGISTRO SELECCIONADO
                if (datalistadoRequerimiento.CurrentRow != null)
                {
                    //CAPTURAR EL CÓDIFO DE MI REQUERIMIENTO SIMPLE
                    int idRequerimiento = Convert.ToInt32(datalistadoRequerimiento.SelectedCells[1].Value.ToString());
                    string estadoReque = datalistadoRequerimiento.SelectedCells[13].Value.ToString();
                    //VER EL PANEL DE LOS DETALLES DEL REQUERIMIENTO
                    panelDetallesRequerimiento.Visible = true;
                    txtCodigoRequerimiento.Text = datalistadoRequerimiento.SelectedCells[3].Value.ToString();
                    txtCantidadItems.Text = datalistadoRequerimiento.SelectedCells[14].Value.ToString();

                    if (estadoReque == "OC EN CURSO")
                    {
                        lblFechaOC.Visible = true;
                        dataFechaOC.Visible = true;
                    }
                    else
                    {
                        lblFechaOC.Visible = false;
                        dataFechaOC.Visible = false;
                    }
                    //MOSTRAR LOS ITEMS DEL REQUERIMIENTO SIMPLE
                    CargarDetallesItems(idRequerimiento);
                }
            }
        }

        //OCULTAR EL PANEL DE LOS DETALLES DEL REQUERIMIENTO
        private void btnSalirDetallesRequerimiento_Click(object sender, EventArgs e)
        {
            //OCULTAR EL PANEL DE LOS DETALLES DEL REQUERIMIENTO
            panelDetallesRequerimiento.Visible = false;
        }

        //-----------------------------------------------------------------------------------
        //CARGA DE DATOS NECESARIOS PARA LA EDICIÓN DE MI REQUERIMEINTO SIMPLE---------------
        //CARGAR TIPO DE REQUERIMIENTO
        public void CargarTipoRequerimiento()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdTipoRequerimiento, Descripcion FROM TipoRequerimientoGeneral WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboTipoRequerimiento.DisplayMember = "Descripcion";
            cboTipoRequerimiento.ValueMember = "IdTipoRequerimiento";
            cboTipoRequerimiento.DataSource = dt;
        }

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
            cboSede.DisplayMember = "Descripcion";
            cboSede.ValueMember = "IdSede";
            cboSede.DataSource = dt;
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
            cboLocal.DisplayMember = "Descripcion";
            cboLocal.ValueMember = "IdLocal";
            cboLocal.DataSource = dt;
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
            cboPrioridad.DisplayMember = "Descripcion";
            cboPrioridad.ValueMember = "IdPrioridad";
            cboPrioridad.DataSource = dt;
        }

        //CARGAR CENTRO COSTOS
        public void CargarCentroCostos()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdCentroCostos, Descripcion FROM CentroCostos WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboCentroCostos.DisplayMember = "Descripcion";
            cboCentroCostos.ValueMember = "IdCentroCostos";
            cboCentroCostos.DataSource = dt;
        }

        //CARGAR CENTRO AREA
        public void CargarArea()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdArea, Descripcion FROM AreaGeneral WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboAreaGeneral.DisplayMember = "Descripcion";
            cboAreaGeneral.ValueMember = "IdArea";
            cboAreaGeneral.DataSource = dt;
        }

        //ACCIÓN DE EDITAR UN REQUERIMIENTO SIMPLE
        private void btnEditarReque_Click(object sender, EventArgs e)
        {
            //SI NO HAY UN REQUERIMIENTO SELECCIOANOD
            if (datalistadoRequerimiento.CurrentRow != null)
            {
                //BLOQUEAR MI LISTADO PARA EVITAR LA SELECCIÓN DE OTRO REQUERIMEINTO
                datalistadoRequerimiento.Enabled = false;
                //CAPTURAR EL NOMBRE DE USUARIO
                string usuarioEncargado = datalistadoRequerimiento.SelectedCells[8].Value.ToString();
                int codigoRequerimientoSimple = Convert.ToInt32(datalistadoRequerimiento.SelectedCells[1].Value.ToString());
                string estadoRequerimiento = datalistadoRequerimiento.SelectedCells[13].Value.ToString();

                if (estadoRequerimiento == "POR ATENDER")
                {
                    //SI EL USUARIO LOGEADO ES IGUAL AL USUARIO ENCARGADO DE DEL REQUERIMINTO
                    if (usuarioEncargado == Program.NombreUsuarioCompleto)
                    {
                        panelEditarRequerimiento.Visible = true;
                        //PRECARGA DE DATOS Y COMBOS
                        CargarTipoRequerimiento();
                        CargarSede();
                        CargarLocal();
                        CargarPrioridad();
                        CargarCentroCostos();
                        CargarArea();

                        //CAPTURA DE VALORES A LOS CAMPOS DE MI REQUERIMIENTO
                        txtJefatura.Text = datalistadoRequerimiento.SelectedCells[6].Value.ToString();
                        cboSede.SelectedValue = datalistadoRequerimiento.SelectedCells[15].Value.ToString();
                        cboLocal.SelectedValue = datalistadoRequerimiento.SelectedCells[16].Value.ToString();
                        cboTipoRequerimiento.SelectedValue = datalistadoRequerimiento.SelectedCells[17].Value.ToString();
                        cboPrioridad.SelectedValue = datalistadoRequerimiento.SelectedCells[18].Value.ToString();
                        dateTimeFechaRequerida.Value = Convert.ToDateTime(datalistadoRequerimiento.SelectedCells[4].Value);
                        dateTimeFechaSolicitada.Value = Convert.ToDateTime(datalistadoRequerimiento.SelectedCells[5].Value);
                        txtSolicitante.Text = datalistadoRequerimiento.SelectedCells[8].Value.ToString();
                        cboCentroCostos.SelectedValue = datalistadoRequerimiento.SelectedCells[9].Value.ToString();
                        cboAreaGeneral.SelectedValue = datalistadoRequerimiento.SelectedCells[11].Value.ToString();
                        txtObservaciones.Text = datalistadoRequerimiento.SelectedCells[19].Value.ToString();
                        //BUSCAR DETALLES DE MI REQUERIMIENTO PARA CARGARLO
                        BuscarDetallesRequerimiento(codigoRequerimientoSimple);
                        //RECUPERAR DATOS A MI LISTADO
                        //SE USA EL FOREACH PARA RECORRER TODAS LAS FILAS SELECCIOANDAS
                        foreach (DataGridViewRow row in datallistadoDetalles.Rows)
                        {
                            //SE CAPTURA LAS VARIABLES 
                            string idDetalle = Convert.ToString(row.Cells[0].Value);
                            string item = Convert.ToString(row.Cells[2].Value);
                            string idArt = Convert.ToString(row.Cells[3].Value);
                            string codigo = Convert.ToString(row.Cells[4].Value);
                            string producto = Convert.ToString(row.Cells[5].Value);
                            string tipoMedida = Convert.ToString(row.Cells[6].Value);
                            string cantidadTotal = Convert.ToString(row.Cells[7].Value);
                            string stock = Convert.ToString(row.Cells[8].Value);
                            //SE AGREGA A LA NUEVA LISTA
                            datalistadoProductosRequerimiento.Rows.Add(new[] { idDetalle, codigo, producto, tipoMedida, cantidadTotal, stock, idArt, item });
                        }

                        //DEFINICIÓND DE SOLO LECTURA DE MI LISTADO DE PRODUCTOS
                        datalistadoProductosRequerimiento.Columns[1].ReadOnly = true;
                        datalistadoProductosRequerimiento.Columns[2].ReadOnly = true;
                        datalistadoProductosRequerimiento.Columns[3].ReadOnly = true;
                    }
                    else
                    {
                        MessageBox.Show("El usuario que desea editar este requerimiento debe ser el mismo que lo ha creado.", "Validación del Sistema");
                        //DESBLOQUEAR MI LISTADO PARA EVITAR LA SELECCIÓN DE OTRO REQUERIMEINTO
                        datalistadoRequerimiento.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("El requerimiento no se puede editar ya que se encuentra en una etapa avanzada o está anuado.", "Validación del Sistema");
                    //DESBLOQUEAR MI LISTADO PARA EVITAR LA SELECCIÓN DE OTRO REQUERIMEINTO
                    datalistadoRequerimiento.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un requerimiento para poder editarlo.", "Validación del Sistema");
                //DESBLOQUEAR MI LISTADO PARA EVITAR LA SELECCIÓN DE OTRO REQUERIMEINTO
                datalistadoRequerimiento.Enabled = true;
            }
        }

        //ACCIÓN DE AÑADOR PRODUCTOS
        private void btnAgregarProductos_Click(object sender, EventArgs e)
        {
            panelBuscarProductos.Visible = true;
            //HACER QUE EL COMBO TENGA UN DATO SELECCIONADO POR DEFAULT
            cboTipoBusquedaProducto.SelectedIndex = 0;
            MostrarProductosRequerimientoGeneral();
            //DEFINIR LAS COLUMNAS DE MI LISTADO COMO SOLO LECTURA
            datalistadoBusquedaProducto.Columns[1].ReadOnly = true;
            datalistadoBusquedaProducto.Columns[2].ReadOnly = true;
            datalistadoBusquedaProducto.Columns[3].ReadOnly = true;
            datalistadoBusquedaProducto.Columns[8].ReadOnly = true;
            datalistadoBusquedaProducto.Columns[9].ReadOnly = true;
            datalistadoBusquedaProducto.Columns[10].ReadOnly = true;
        }

        //CAMBIAR EL CRITERIO DE BÚSQUEDA
        private void cboTipoBusquedaProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusquedaProducto.Text = "";
        }

        //ACCIÓN DE SELECCIOANR UNA FILA Y LLEVARLA A MI OTRO LISTADO
        private void datalistadoBusquedaProducto_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoBusquedaProducto.Columns[e.ColumnIndex];

            //SI SE PRECIONA SOBRE LA COLUMNA CON ESE NOMBRE
            if (currentColumn.Name == "ckSeleccionarProducto")
            {
                //SE CAPTURA LAS VARIABLES 
                string id = datalistadoBusquedaProducto.SelectedCells[11].Value.ToString();
                string codigo = datalistadoBusquedaProducto.SelectedCells[1].Value.ToString();
                string producto = datalistadoBusquedaProducto.SelectedCells[2].Value.ToString();
                string tipoMedida = datalistadoBusquedaProducto.SelectedCells[3].Value.ToString();
                string stock = datalistadoBusquedaProducto.SelectedCells[8].Value.ToString();

                //SE AGREGA A LA NUEVA LISTA
                datalistadoSeleccionBusquedaProducto.Rows.Add(new[] { id, codigo, producto, tipoMedida, stock });
                //SE BORRA EL REGISTRO SELECCIONADO
                datalistadoBusquedaProducto.Rows.Remove(datalistadoBusquedaProducto.CurrentRow);
            }
        }

        //LLEVAR LOS PRODUCTOS A MI OTRO FORMULARIO
        private void btnConfirmarBusquedaProductos_Click(object sender, EventArgs e)
        {
            //SE USA EL FOREACH PARA RECORRER TODAS LAS FILAS SELECCIOANDAS
            foreach (DataGridViewRow row in datalistadoSeleccionBusquedaProducto.Rows)
            {
                //SE CAPTURA LAS VARIABLES 
                string idProducto = Convert.ToString(row.Cells[0].Value);
                string codigo = Convert.ToString(row.Cells[1].Value);
                string producto = Convert.ToString(row.Cells[2].Value);
                string tipoMedida = Convert.ToString(row.Cells[3].Value);
                string stock = Convert.ToString(row.Cells[4].Value);

                int cantidadItems = datalistadoProductosRequerimiento.RowCount;
                cantidadItems = cantidadItems + 1;

                //SE AGREGA A LA NUEVA LISTA
                datalistadoProductosRequerimiento.Rows.Add(new[] { null, codigo, producto, tipoMedida, null, stock, idProducto, Convert.ToString(cantidadItems) });
            }

            //LIMPIAR Y REINICIAR LA BÚSQUEDA DE PRODUCTOS
            panelBuscarProductos.Visible = false;
            panelBuscarProductos.Visible = false;
            txtBusquedaProducto.Text = "";
            cboTipoBusquedaProducto.SelectedIndex = 1;
            datalistadoSeleccionBusquedaProducto.Rows.Clear();
        }

        //RECARGAR EL LISTADO DE PRODUCTOS Y LIMPIAR LA BARRA DE BÚSQUEDA
        private void btnRegresarBusqeudaProductos_Click(object sender, EventArgs e)
        {
            panelBuscarProductos.Visible = false;
            txtBusquedaProducto.Text = "";
            cboTipoBusquedaProducto.SelectedIndex = 1;
            datalistadoSeleccionBusquedaProducto.Rows.Clear();
        }

        //ACCIÓN DE BORAR PRODUCTOS EN LA BÚSQUEDA
        private void btnBorrarBusquedaProductps_Click(object sender, EventArgs e)
        {
            //SI EN EL LISTADO DE CLIENTES NO HAY REGIUSTROS
            if (datalistadoSeleccionBusquedaProducto.Rows.Count > 0)
            {
                //MENSAJE DE CONFIRMACIÓN DE ELIMINACIÓN DE PRODUCTOS
                DialogResult resul = MessageBox.Show("¿Seguro que desea borrar este producto?.", "Validación del Sistema", MessageBoxButtons.YesNo);
                if (resul == DialogResult.Yes)
                {
                    //BORRAR EL REGISTRO SELECCIONADO
                    datalistadoSeleccionBusquedaProducto.Rows.Remove(datalistadoSeleccionBusquedaProducto.CurrentRow);
                }
            }
            else
            {
                MessageBox.Show("No hay productos agregados para poder borrarlos.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //ACCIÓN DE SALIR DE LA EDICIÓN DEL REQUERIMIENTO
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            panelEditarRequerimiento.Visible = false;
            datalistadoProductosRequerimiento.Rows.Clear();
            txtObservaciones.Text = "";
            //DESBLOQUEAR MI LISTADO PARA EVITAR LA SELECCIÓN DE OTRO REQUERIMEINTO
            datalistadoRequerimiento.Enabled = true;
        }

        //VALIDACIÓN DEL LISTADO DE PRESUPUESTO
        private void datalistadoProductosRequerimiento_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //VARIABLES PARA ALMACENAR LOS DATOS
            decimal a;
            //ALMACENAMIENTO DE DATOS PARA LA VALIDACIÓN
            //RECORRIDO DE MI LISTADO PARA ALMACENAR LAS DIFERENTES COLUMNAS
            DataGridViewRow row = (DataGridViewRow)datalistadoProductosRequerimiento.Rows[e.RowIndex];
            //ALMACENAMIENTOS DE COLUMNAS
            a = Convert.ToDecimal(row.Cells[4].Value);

            //VALIDACIÓN DE VIÁTICOS
            if (row.Cells[4].Value == DBNull.Value)
            {
                //REINICIO DE CAMPO
                a = Convert.ToDecimal("0.000");
            }
            else
            {
                //CAPTURA DEL VALOR
                a = Convert.ToDecimal(row.Cells[4].Value);
            }

            row.Cells[4].Value = String.Format("{0:#,0.000}", a);
        }

        //SALIR DE GUARDAR EL REQUERIMIENTO SIMPLE
        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea editar este requerimiento?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("EditarRequerimientoSimple", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //EDITAR - PARTE GENERAL DEL REQUERIMIENTO SIMPLE
                cmd.Parameters.AddWithValue("@idReuqerimientoSimple", datalistadoRequerimiento.SelectedCells[1].Value.ToString());
                cmd.Parameters.AddWithValue("@fechaSolicitada", dateTimeFechaSolicitada.Value);
                cmd.Parameters.AddWithValue("@observaciones", txtObservaciones.Text);
                cmd.Parameters.AddWithValue("@idSede", cboSede.SelectedValue);
                cmd.Parameters.AddWithValue("@idLocal", cboLocal.SelectedValue);
                cmd.Parameters.AddWithValue("@idipo", cboTipoRequerimiento.SelectedValue);
                cmd.Parameters.AddWithValue("@cantidadItems", datalistadoProductosRequerimiento.RowCount);
                cmd.Parameters.AddWithValue("@idPrioridad", cboPrioridad.SelectedValue);
                cmd.ExecuteNonQuery();
                con.Close();

                //EDICIÓN DE LOS DETALLES DEL REQUERIMIENTO SIMPLE CON UN FOREACH
                foreach (DataGridViewRow row in datalistadoProductosRequerimiento.Rows)
                {
                    decimal cantidad = Convert.ToDecimal(row.Cells["CANTIDAD"].Value);
                    //PROCEDIMIENTO ALMACENADO PARA EDITAR LOS PRODUCTOS
                    int? idDetalleRequerimientoSimple = Convert.ToInt32(row.Cells[0].Value);

                    if (idDetalleRequerimientoSimple == 0)
                    {
                        con.Open();
                        cmd = new SqlCommand("InsertarDetallesRequerimientoSimple", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idRequerimientoSimple", datalistadoRequerimiento.SelectedCells[1].Value);
                        cmd.Parameters.AddWithValue("@item", Convert.ToString(row.Cells[7].Value));
                        cmd.Parameters.AddWithValue("@idArt", Convert.ToString(row.Cells[6].Value));
                        //SI NO HAN PUESTO UN VALOR AL PRODUCTO
                        if (cantidad == 0)
                        {
                            cmd.Parameters.AddWithValue("@cantidad", 0.000);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@cantidad", cantidad);
                        }

                        cmd.Parameters.AddWithValue("@stock", Convert.ToString(row.Cells[5].Value));
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    else
                    {
                        con.Open();
                        cmd = new SqlCommand("EditarDetallesRequerimientoSimple", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idDetalleReuqerimientoSimple", Convert.ToString(row.Cells[0].Value));
                        cmd.Parameters.AddWithValue("@idRequerimientoSimple", datalistadoRequerimiento.SelectedCells[1].Value);
                        cmd.Parameters.AddWithValue("@item", Convert.ToString(row.Cells[7].Value));
                        cmd.Parameters.AddWithValue("@idArt", Convert.ToString(row.Cells[6].Value));
                        //SI NO HAN PUESTO UN VALOR AL PRODUCTO
                        if (cantidad == 0)
                        {
                            cmd.Parameters.AddWithValue("@cantidad", 0.000);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@cantidad", cantidad);
                        }

                        cmd.Parameters.AddWithValue("@stock", Convert.ToString(row.Cells[5].Value));
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

                MessageBox.Show("Se editó el requerimiento correctamente.", "Validación del Sistema");
                MostrarRequerimientoPorFecha(DesdeFecha.Value, HastaFecha.Value);
                panelEditarRequerimiento.Visible = false;
                datalistadoProductosRequerimiento.Rows.Clear();
                txtObservaciones.Text = "";
                //DESBLOQUEAR MI LISTADO PARA EVITAR LA SELECCIÓN DE OTRO REQUERIMEINTO
                datalistadoRequerimiento.Enabled = true;
            }
        }

        //CARGA DE METODOS - GENERAL----------------------------------------------------------------------------------
        //CARGA DE DATOS DEL USUARIO QUE INICIO SESIÓN
        //BUSQUEDA DE USUARIO
        private void txtBusquedaProducto_TextChanged(object sender, EventArgs e)
        {
            if (cboTipoBusquedaProducto.Text == "CÓDIGO")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("ListarProductosRequerimientoGeneral_PorCodigo_SP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigo", txtBusquedaProducto.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoBusquedaProducto.DataSource = dt;
                con.Close();
                Rediemnsion(datalistadoBusquedaProducto);
            }
            else if (cboTipoBusquedaProducto.Text == "DESCRIPCIÓN")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("ListarProductosRequerimientoGeneral_PorDescripcion_SP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@descripcion", txtBusquedaProducto.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoBusquedaProducto.DataSource = dt;
                con.Close();
                Rediemnsion(datalistadoBusquedaProducto);
            }
            else if (cboTipoBusquedaProducto.Text == "CÓDIGO BSS")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("[ListarProductosRequerimientoGeneral_PorCodigoBSS_SP]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@descripcion", txtBusquedaProducto.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoBusquedaProducto.DataSource = dt;
                con.Close();
                Rediemnsion(datalistadoBusquedaProducto);
            }
        }

        //VALIDACIÓN DE SOLO NGRESO DE NÚMEROS A MI DATAGRIDVIEW
        private void Columns_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // solo 1 punto decimal
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        //VALIDACIÓN DE SOLO NÚMEROS
        private void datalistadoProductosRequerimiento_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                e.Control.KeyPress -= Columns_KeyPress;
                e.Control.KeyPress += Columns_KeyPress;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error something went wrong!!", ex.Message);
            }
        }

        //BOTONES RANDOMS
        private void btnInformacionFechas_Click(object sender, EventArgs e)
        {
            if (panelInformacionFecha.Visible == true)
            {
                panelInformacionFecha.Visible = false;
            }
            else
            {
                panelInformacionFecha.Visible = true;
            }
        }

        //BORRAR UN PRODICTO DE MI LISTADO DE MI REQUERIMIENTO
        private void btnBorrarProducto_Click(object sender, EventArgs e)
        {
            //SI EN EL LISTADO DE CLIENTES NO HAY REGIUSTROS
            if (datalistadoProductosRequerimiento.Rows.Count > 0)
            {
                //MENSAJE DE CONFIRMACIÓN DE ELIMINACIÓN DE PRODUCTOS
                DialogResult resul = MessageBox.Show("¿Seguro que desea borrar este producto?.", "Validación del Sistema", MessageBoxButtons.YesNo);
                if (resul == DialogResult.Yes)
                {
                    //BORRAR EL REGISTRO SELECCIONADO
                    datalistadoProductosRequerimiento.Rows.Remove(datalistadoProductosRequerimiento.CurrentRow);
                }
            }
            else
            {
                MessageBox.Show("No hay productos agregados para poder borrarlos.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //RECARGAR EL LISTADO DE PRODUCTOS Y LIMPIAR LA BARRA DE BÚSQUEDA
        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            //HACER EL LLAMADO AL MÉTODO DE LISTAD DE NUEVO
            MostrarProductosRequerimientoGeneral();
            //LIMPIAR LA BARRA DE BÚSQUEDA Y REINICIAR EL CBO
            txtBusquedaProducto.Text = "";
            cboTipoBusquedaProducto.SelectedIndex = 0;
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
            sl.SaveAs(desktopPath + @"\Reporte de Requerimento Simple.xlsx");
            MessageBox.Show("Se exportó los datos a un archivo de Microsoft Excel en la siguiente ubicación: " + desktopPath, "Validación del Sistema", MessageBoxButtons.OK);
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

                MessageBox.Show($"Reporte exportado correctamente a: {rutaSalida}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al exportar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //ABRIR EL MANUAL DE USUARIO
        private void btnInfo_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //ABRIR EL MANUAL DE USUARIO
        private void btnInfoBusquedaProductos_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }
    }
}

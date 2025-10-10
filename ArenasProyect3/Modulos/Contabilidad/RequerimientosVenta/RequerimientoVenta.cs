using ArenasProyect3.Modulos.ManGeneral;
using ArenasProyect3.Modulos.Resourses;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DocumentFormat.OpenXml;
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

namespace ArenasProyect3.Modulos.Contabilidad.RequerimientosVenta
{
    public partial class RequerimientoVenta : Form
    {
        //VARIABLES GLOBALES PARA MIS ACTAS DE VISITA
        private Cursor curAnterior = null;
        string ruta = ManGeneral.Manual.manualAreaComercial;

        //CONSTRUCTOR DEL MANTENIMIENTO - REQUERIMIENTOS DE VENTA
        public RequerimientoVenta()
        {
            InitializeComponent();
        }

        //INICIO Y CARGA INICIAL DEL REQUERIMEINTO----------------------------------------------------------
        private void RequerimientoVenta_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            datalistadoTodasRequerimientos.DataSource = null;


            //PREFILES Y PERSIMOS---------------------------------------------------------------
            if (Program.RangoEfecto != 8)
            {
                btnAtenderRequerimeinto.Visible = true;
                lblAnotacionAtendido.Visible = true;
            }
            //---------------------------------------------------------------------------------

            CargarCantidadLiquidacionesNoAprobadas();

            if (Convert.ToInt32(datalistadoCantidadLiquidacionesNoAprobadas.SelectedCells[0].Value.ToString()) >= 5)
            {
                MessageBox.Show("Se han detectado en el sistema más de 5 liquidaciones sin la atención respectiva, por favor regularizar las liquidaciones faltantes.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //CARGA VALIDACIÓN DE CANTIDAD DE LIQUIDACIONES----------------------------
        public void CargarCantidadLiquidacionesNoAprobadas()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT COUNT(IdLiquidacion) FROM LiquidacionVenta LIQUI WHERE EstadoContabilidad = 0 AND LIQUI.Estado = 1", con);
                da.Fill(dt);
                datalistadoCantidadLiquidacionesNoAprobadas.DataSource = dt;
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }

        }

        //VIZUALIZAR DATOS EXCEL--------------------------------------------------------------------
        public void MostrarExcel()
        {
            datalistadoExcel.Rows.Clear();

            foreach (DataGridViewRow dgv in datalistadoTodasRequerimientos.Rows)
            {
                string numeroReque = dgv.Cells[1].Value.ToString();
                string FechaGen = Convert.ToDateTime(dgv.Cells[2].Value).ToString("yyyy/MM/dd");
                string fechaInicio = Convert.ToDateTime(dgv.Cells[3].Value).ToString("yyyy/MM/dd");
                string fechaTermino = Convert.ToDateTime(dgv.Cells[4].Value).ToString("yyyy/MM/dd");
                string responsable = dgv.Cells[5].Value.ToString();
                string motivoViaje = dgv.Cells[6].Value.ToString();
                string tipoMoneda = dgv.Cells[7].Value.ToString();
                string total = dgv.Cells[8].Value.ToString();
                string estadoJefatura = dgv.Cells[9].Value.ToString();
                string estadoContabilidad = dgv.Cells[10].Value.ToString();

                datalistadoExcel.Rows.Add(new[] { numeroReque, FechaGen, fechaInicio, fechaTermino, responsable, motivoViaje, tipoMoneda, total, estadoJefatura, estadoContabilidad });
            }
        }
        //-----------------------------------------------------------------------------

        //LISTADO DE REQUERIMIENTOS Y SELECCION DE PDF Y ESTADO DE LIQUIDACIÓN-------------------------------
        //MOSTRAR REQUERIMIENTOS AL INCIO 
        public void MostrarRequerimientos(DateTime fechaInicio, DateTime fechaTermino)
        {
            if (lblCarga.Text == "0")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("MostrarRequerimientosVentasPorFecha_JefaturaCon", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoTodasRequerimientos.DataSource = dt;
                    con.Close();
                    OrdenarColumnasRequerimiento(datalistadoTodasRequerimientos);
                }
                catch (Exception ex)
                {
                    //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                    ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
                }
            }
            else
            {
                lblCarga.Text = "0";
            }
        }

        //MOSTRAR REQUERIMIENTOS POR RESPONSABLE
        public void MostrarRequerimientosResponsable(string resopnsable, DateTime fechaInicio, DateTime fechaTermino)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarRequerimientosVentasPorResponsableCon", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@responsable", resopnsable);
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoTodasRequerimientos.DataSource = dt;
                con.Close();
                OrdenarColumnasRequerimiento(datalistadoTodasRequerimientos);
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //MOSTRAR REQUERIMIENTOS POR ESTADOS
        public void MostrarRequerimientosEstadosPendiente(DateTime fechaInicio, DateTime fechaTermino)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarRequerimientosVentasPorEstados_Pendiente_Jefatura", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoTodasRequerimientos.DataSource = dt;
                con.Close();
                OrdenarColumnasRequerimiento(datalistadoTodasRequerimientos);
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //MOSTRAR REQUERIMIENTOS POR ESTADOS
        public void MostrarRequerimientosEstados(int estado, DateTime fechaInicio, DateTime fechaTermino)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarRequerimientosVentasPorEstados_Jefatura", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@estado", estado);
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoTodasRequerimientos.DataSource = dt;
                con.Close();
                OrdenarColumnasRequerimiento(datalistadoTodasRequerimientos);
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //MOSTRAR REQUERIMIENTOS POR ESTADO - SEMIAPROBADOS
        public void MostrarRequerimientosEstadoDesaprobado(DateTime fechaInicio, DateTime fechaTermino)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarRequerimientosVentasPorEstadosDesaprobado_Jefatura", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoTodasRequerimientos.DataSource = dt;
                con.Close();
                OrdenarColumnasRequerimiento(datalistadoTodasRequerimientos);
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //REORDENAR MIS COLUMNAS
        public void OrdenarColumnasRequerimiento(DataGridView DGV)
        {
            //SE REDIMENSIONA EL TAMAÑO DE CADA COLUMNA DE MI LISTADO DE REQUERIMIENTOS
            DGV.Columns[1].Width = 50;
            DGV.Columns[2].Width = 90;
            DGV.Columns[3].Width = 80;
            DGV.Columns[4].Width = 80;
            DGV.Columns[5].Width = 150;
            DGV.Columns[6].Width = 350;
            DGV.Columns[7].Width = 60;
            DGV.Columns[8].Width = 70;
            DGV.Columns[9].Width = 95;
            DGV.Columns[10].Width = 95;
            DGV.Columns[11].Width = 85;
            //SE QUITA LAS COLUMNAS QUE NO SON RELEVANTES PARA EL USUARIO
            DGV.Columns[12].Visible = false;
            DGV.Columns[13].Visible = false;
            DGV.Columns[14].Visible = false;
            //CARGAR LOS COLORES DE ACUERDO A SU ESTADO
            ColoresListado();

            //deshabilitar el click y  reordenamiento por columnas
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
                for (var i = 0; i <= datalistadoTodasRequerimientos.RowCount - 1; i++)
                {
                    if (datalistadoTodasRequerimientos.Rows[i].Cells[9].Value.ToString() == "APROBADO" && datalistadoTodasRequerimientos.Rows[i].Cells[10].Value.ToString() == "ATENDIDO")
                    {
                        //APROBADP
                        datalistadoTodasRequerimientos.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                    }
                    else if (datalistadoTodasRequerimientos.Rows[i].Cells[9].Value.ToString() == "PENDIENTE" && datalistadoTodasRequerimientos.Rows[i].Cells[10].Value.ToString() == "NO ATENDIDO" || datalistadoTodasRequerimientos.Rows[i].Cells[9].Value.ToString() == "APROBADO" && datalistadoTodasRequerimientos.Rows[i].Cells[10].Value.ToString() == "NO ATENDIDO" || datalistadoTodasRequerimientos.Rows[i].Cells[9].Value.ToString() == "PENDIENTE" && datalistadoTodasRequerimientos.Rows[i].Cells[10].Value.ToString() == "ATENDIDO")
                    {
                        //PENDIENTE
                        datalistadoTodasRequerimientos.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        //DESAPROBADO
                        datalistadoTodasRequerimientos.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //MOSTRAR REQUERIMIENTOS POR RESPONSABLE
        private void txtBusquedaResponsable_TextChanged(object sender, EventArgs e)
        {
            MostrarRequerimientosResponsable(txtBusquedaResponsable.Text, DesdeFecha.Value, HastaFecha.Value);
        }

        //BÚSQUEDA DE REQUERIMEINTOS POR FECHAS
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarRequerimientos(DesdeFecha.Value, HastaFecha.Value);
        }

        //BÚSQUEDA DE REQUERIMEINTOS POR FECHAS
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarRequerimientos(DesdeFecha.Value, HastaFecha.Value);
        }

        //BÚSQUEDA DE REQUERIMEINTOS SIN FILTROS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarRequerimientos(DesdeFecha.Value, HastaFecha.Value);
        }

        //BÚSQUEDA DE REQUERIMEINTOS POR ESTADO DE PENDIENTES
        private void btnBusquedaPendientes_Click(object sender, EventArgs e)
        {
            MostrarRequerimientosEstadosPendiente(DesdeFecha.Value, HastaFecha.Value);
        }

        //BÚSQUEDA DE REQUERIMEINTOS POR ESTADO DE APROBADOS
        private void btnBusquedaAprobados_Click(object sender, EventArgs e)
        {
            MostrarRequerimientosEstados(2, DesdeFecha.Value, HastaFecha.Value);
        }

        //BÚSQUEDA DE REQUERIMEINTOS POR ESTADO DE DESAPROBADOS
        private void btnBusquedaDesaprobado_Click(object sender, EventArgs e)
        {
            MostrarRequerimientosEstadoDesaprobado(DesdeFecha.Value, HastaFecha.Value);
        }
        //----------------------------------------------------------------------------------------------------

        //GENERACIÓN DE LOS PDFs-----------------------------------------------------------------------------
        //GENERACIÓN DEL PDF DEL REQUERIMEINTO
        private void btnVerRequerimiento_Click(object sender, EventArgs e)
        {
            try
            {
                //SI NO HAY NINGUN REGISTRO SELECCIONADO
                if (datalistadoTodasRequerimientos.CurrentRow != null)
                {
                    string codigoRequerimientoReporte = "0";
                    //SI EL REQUERIMEINTO ESTÁ ANULADO POR EL ÁREA COMERCIAL Y YA TIENE LIQUIDACIÓN CREADA
                    if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "ANULADO" && Convert.ToBoolean(datalistadoTodasRequerimientos.SelectedCells[11].Value.ToString()) == true)
                    {
                        //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO DESAPROBADO
                        codigoRequerimientoReporte = datalistadoTodasRequerimientos.Rows[datalistadoTodasRequerimientos.CurrentRow.Index].Cells[1].Value.ToString();
                        Visualizadores.VisualizarRequerimientoDesaprobado frm = new Visualizadores.VisualizarRequerimientoDesaprobado();
                        frm.lblCodigo.Text = codigoRequerimientoReporte;
                        //CARGAR VENTANA
                        frm.Show();
                    }
                    //SI EL REQUERIMEINTO ESTÁ EN PENDIENTE
                    else if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "PENDIENTE")
                    {
                        //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO GENERAL
                        codigoRequerimientoReporte = datalistadoTodasRequerimientos.Rows[datalistadoTodasRequerimientos.CurrentRow.Index].Cells[1].Value.ToString();
                        Visualizadores.VisualizarRequerimientoVenta frm = new Visualizadores.VisualizarRequerimientoVenta();
                        frm.lblCodigo.Text = codigoRequerimientoReporte;
                        //CARGAR VENTANA
                        frm.Show();
                    }
                    //SI EL REQUERIMEINTO ESTÁ APROBADO POR EL ÁREA COMERCIAL O ESTA EN PENDIENTE
                    else if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "APROBADO")
                    {
                        //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO GENERAL
                        codigoRequerimientoReporte = datalistadoTodasRequerimientos.Rows[datalistadoTodasRequerimientos.CurrentRow.Index].Cells[1].Value.ToString();
                        Visualizadores.VisualizarRequerimientoAprobado frm = new Visualizadores.VisualizarRequerimientoAprobado();
                        frm.lblCodigo.Text = codigoRequerimientoReporte;
                        //CARGAR VENTANA
                        frm.Show();
                    }
                    //SI EL REQUERIMEINTO NO ENTRA A NINGUNA DE LAS OPCIONES ANTERIORES
                    else
                    {
                        //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO DESAPROBADO
                        codigoRequerimientoReporte = datalistadoTodasRequerimientos.Rows[datalistadoTodasRequerimientos.CurrentRow.Index].Cells[1].Value.ToString();
                        Visualizadores.VisualizarRequerimientoDesaprobado frm = new Visualizadores.VisualizarRequerimientoDesaprobado();
                        frm.lblCodigo.Text = codigoRequerimientoReporte;
                        //CARGAR VENTANA
                        frm.Show();
                    }

                    //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                    ClassResourses.RegistrarAuditora(6, this.Name, 4, Program.IdUsuario, "Visualización de requerimiento de viaje PDF", Convert.ToInt32(codigoRequerimientoReporte));
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un requerimiento para poder generar el PDF respectivo.", "Validación del Sistema", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN DE GENERACIÓN DEL PDF
        private void datalistadoTodasRequerimientos_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoTodasRequerimientos.Columns[e.ColumnIndex].Name == "btnGenerarPdf")
            {
                this.datalistadoTodasRequerimientos.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoTodasRequerimientos.Cursor = curAnterior;
            }
        }

        //GENERACIÓN DEL PDF DEL REQUERIMEINTO YA APROBADO CON CONFIRMACIÓN DE LAS JJEFATURAS
        private void datalistadoTodasRequerimientos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoTodasRequerimientos.Columns[e.ColumnIndex];

            try
            {
                //SI SE PRECIONA SOBRE LA COLUMNA CON EL NOMBRE SELECCIOANDO
                if (currentColumn.Name == "btnGenerarPdf")
                {
                    string codigoRequerimientoReporte = "0";

                    if (datalistadoTodasRequerimientos.CurrentRow != null)
                    {
                        //SI EL REQUERIMEINTO ESTÁ APROBADO POR EL ÁREA COMERCIAL
                        if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "ANULADO")
                        {
                            //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO DESAPROBADO
                            codigoRequerimientoReporte = datalistadoTodasRequerimientos.Rows[datalistadoTodasRequerimientos.CurrentRow.Index].Cells[1].Value.ToString();
                            Visualizadores.VisualizarRequerimientoDesaprobado frm = new Visualizadores.VisualizarRequerimientoDesaprobado();
                            frm.lblCodigo.Text = codigoRequerimientoReporte;
                            //CARGAR VENTANA
                            frm.Show();
                        }
                        //SI EL REQUERIMEINTO ESTÁ APROBADO POR EL ÁREA COMERCIAL
                        else if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "APROBADO")
                        {
                            codigoRequerimientoReporte = datalistadoTodasRequerimientos.Rows[datalistadoTodasRequerimientos.CurrentRow.Index].Cells[1].Value.ToString();
                            Visualizadores.VisualizarRequerimientoAprobado frm = new Visualizadores.VisualizarRequerimientoAprobado();
                            frm.lblCodigo.Text = codigoRequerimientoReporte;
                            //CARGAR VENTANA
                            frm.Show();
                        }
                        //SI EL REQUERIMEINTO ESTÁ PENDIENTE POR EL ÁREA COMERCIAL
                        else if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "PENDIENTE")
                        {
                            codigoRequerimientoReporte = datalistadoTodasRequerimientos.Rows[datalistadoTodasRequerimientos.CurrentRow.Index].Cells[1].Value.ToString();
                            Visualizadores.VisualizarRequerimientoVenta frm = new Visualizadores.VisualizarRequerimientoVenta();
                            frm.lblCodigo.Text = codigoRequerimientoReporte;
                            //CARGAR VENTANA
                            frm.Show();
                        }
                        //SI EL REQUERIMEINTO NO ENTRA A NINGUNA DE LAS OPCIONES ANTERIORES
                        else
                        {
                            //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO DESAPROBADO
                            codigoRequerimientoReporte = datalistadoTodasRequerimientos.Rows[datalistadoTodasRequerimientos.CurrentRow.Index].Cells[1].Value.ToString();
                            Visualizadores.VisualizarRequerimientoDesaprobado frm = new Visualizadores.VisualizarRequerimientoDesaprobado();
                            frm.lblCodigo.Text = codigoRequerimientoReporte;
                            //CARGAR VENTANA
                            frm.Show();
                        }
                        //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                        ClassResourses.RegistrarAuditora(6, this.Name, 4, Program.IdUsuario, "Visualización de requerimiento de viaje PDF", Convert.ToInt32(codigoRequerimientoReporte));
                    }
                    else
                    {
                        MessageBox.Show("Debe seleccionar un requerimiento para poder generar el PDF con firmas.", "Validación del Sistema", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }
        //--------------------------------------------------------------------------------------

        //APROBAR Y DESAPBROBAR REQUERIMEINTOS POR LA JEFATURA---------------------------------
        //APRIBAR REQUERIMIENTO
        private void btnAtenderRequerimeinto_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasRequerimientos.CurrentRow != null)
            {
                DialogResult boton = MessageBox.Show("¿Realmente desea atender este requerimiento?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    int idRequerimiento = Convert.ToInt32(datalistadoTodasRequerimientos.SelectedCells[1].Value.ToString());
                    string estadoJefatura = datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString();
                    string estadoContabilidad = datalistadoTodasRequerimientos.SelectedCells[10].Value.ToString();

                    if (estadoJefatura == "APROBADO" && estadoContabilidad == "ATENDIDO")
                    {
                        MessageBox.Show("Este requerimiento ya está aprobado.", "Validación del Sistema",MessageBoxButtons.OK);
                    }
                    else
                    {
                        if (Program.AreaUsuario == "Contabilidad")
                        {
                            if (estadoContabilidad == "ATENDIDO")
                            {
                                MessageBox.Show("Este requerimiento ya ha sido atendido por la jefatura del área contable.", "Validación del Sistema", MessageBoxButtons.OK);
                            }
                            else
                            {
                                AprobacionJefaturas(idRequerimiento, 2);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un requerimiento para poder aprobarlo.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //DESAPROBAR REQUERIMIENTO
        private void btnDesaprobaRequerimiento_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasRequerimientos.CurrentRow != null)
            {
                DialogResult boton = MessageBox.Show("¿Realmente desea anular este requerimiento?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    int idRequerimiento = Convert.ToInt32(datalistadoTodasRequerimientos.SelectedCells[1].Value.ToString());
                    string estadoJefatura = datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString();
                    string estadoContabilidad = datalistadoTodasRequerimientos.SelectedCells[10].Value.ToString();

                    if (estadoJefatura == "APROBADO" && estadoContabilidad == "ATENDIDO")
                    {
                        MessageBox.Show("Este requerimiento ya está aprobado.", "Validación del Sistema",MessageBoxButtons.OK);
                    }
                    else
                    {
                        if (Program.AreaUsuario == "Contabilidad")
                        {
                            if (estadoContabilidad == "ATENDIDO")
                            {
                                MessageBox.Show("Este requerimiento ya ha sido atendido por la jefatura del área contable.", "Validación del Sistema", MessageBoxButtons.OK);
                            }
                            else
                            {
                                AprobacionJefaturas(idRequerimiento, 0);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un requerimiento para anularlo.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //FUNCION PARA APROBAR EL REQUERIEMINTO
        public void AprobacionJefaturas(int idRequerimiento, int estadoContabilidad)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                SqlCommand cmd = new SqlCommand();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                cmd = new SqlCommand("CambioEstadoRequerimientoVenta_Contabilidad", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idrequerimiento", idRequerimiento);
                cmd.Parameters.AddWithValue("@estadoContabilidad", estadoContabilidad);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Se hizo el cambio exitosamente.", "Validación del sistema",MessageBoxButtons.OK);
                MostrarRequerimientos(DesdeFecha.Value, HastaFecha.Value);

                if(estadoContabilidad == 2)
                {
                    //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                    ClassResourses.RegistrarAuditora(3, this.Name, 4, Program.IdUsuario, "Aprobación de requerimiento de viaje.", Convert.ToInt32(idRequerimiento));
                }
                else
                {
                    //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                    ClassResourses.RegistrarAuditora(2, this.Name, 4, Program.IdUsuario, "Anular requerimiento de viaje.", Convert.ToInt32(idRequerimiento));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //BOTON PARA VISUALIZAR EL REPORTE
        private void btnInfo_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //FUNCION PAARA EXPORTAR A EXCEL MI LISTADO COMPLETO
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                MostrarExcel();

                SLDocument sl = new SLDocument();
                SLStyle style = new SLStyle();
                SLStyle styleC = new SLStyle();

                //COLUMNAS
                sl.SetColumnWidth(1, 15);
                sl.SetColumnWidth(2, 20);
                sl.SetColumnWidth(3, 20);
                sl.SetColumnWidth(4, 20);
                sl.SetColumnWidth(5, 35);
                sl.SetColumnWidth(6, 50);
                sl.SetColumnWidth(7, 20);
                sl.SetColumnWidth(8, 20);
                sl.SetColumnWidth(9, 35);
                sl.SetColumnWidth(10, 35);

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
                    ir++;
                }

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                sl.SaveAs(desktopPath + @"\Reporte de Requerimientos.xlsx");
                MessageBox.Show("Se exportó los datos a un archivo de Microsoft Excel en la siguiente ubicación: " + desktopPath, "Validación del Sistema", MessageBoxButtons.OK);

                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(5, this.Name, 4, Program.IdUsuario, "Exportar listado de requerimientos de ventas EXCEL", 0);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
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

                //SI EL REQUERIMEINTO ESTÁ ANULADO POR EL ÁREA COMERCIAL Y YA TIENE LIQUIDACIÓN CREADA
                if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "ANULADO" && Convert.ToBoolean(datalistadoTodasRequerimientos.SelectedCells[11].Value.ToString()) == true)
                {
                    rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeRequerimientoVentaAnulada.rpt");
                }
                //SI EL REQUERIMEINTO ESTÁ EN PENDIENTE
                else if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "PENDIENTE")
                {
                    rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeRequerimientoVenta.rpt");
                }
                //SI EL REQUERIMEINTO ESTÁ APROBADO POR EL ÁREA COMERCIAL O ESTA EN PENDIENTE
                else if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "APROBADO")
                {
                    rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeRequerimientoVentaAprobado.rpt");
                }
                //SI EL REQUERIMEINTO NO ENTRA A NINGUNA DE LAS OPCIONES ANTERIORES
                else
                {
                    rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeRequerimientoVentaAnulada.rpt");
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
                int idRequerimiento = Convert.ToInt32(datalistadoTodasRequerimientos.SelectedCells[1].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                crystalReport.SetParameterValue("@idRequerimiento", idRequerimiento);

                // Ruta de salida en el escritorio
                string rutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string rutaSalida = System.IO.Path.Combine(rutaEscritorio, "Requerimiento de viaje número " + idRequerimiento + ".pdf");

                // Exportar a PDF
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaSalida);

                MessageBox.Show($"Reporte exportado correctamente a: {rutaSalida}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(5, this.Name, 4, Program.IdUsuario, "Exportar listado de requerimientos de ventas EXCEL", 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al exportar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }
    }
}

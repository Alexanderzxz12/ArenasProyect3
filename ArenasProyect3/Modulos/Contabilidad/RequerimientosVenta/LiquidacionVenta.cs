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
    public partial class LiquidacionVenta : Form
    {
        //VARIABLES GLOBALES PARA MIS ACTAS DE VISITA
        string ruta = ManGeneral.Manual.manualAreaComercial;

        //CONSTRUCTOR DEL MANTENIMIENTO - LIQUIDACION DE VENTA
        public LiquidacionVenta()
        {
            InitializeComponent();
        }

        //INICIO Y CARGA INICIAL DEL LIQUIDACION----------------------------------------------------------
        private void MenuLiquidacionVenta_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            datalistadoTodasLiquidacion.DataSource = null;

            //PREFILES Y PERSIMOS---------------------------------------------------------------
            if (Program.RangoEfecto != 8)
            {
                btnAtenderLiquidacion.Visible = false;
                lblTexoAprobado.Visible = false;
            }
            //---------------------------------------------------------------------------------

            CargarCantidadLiquidacionesNoAprobadas();

            if (Convert.ToInt32(datalistadoCantidadLiquidacionesNoAprobadas.SelectedCells[0].Value.ToString()) >= 5)
            {
                MessageBox.Show("Se han detectado en el sistema más de 5 liquidaciones sin la atención respectiva, por favor regularizar las liquidaciones faltantes.", "Validación del Sistema",MessageBoxButtons.OK);
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
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
            }
        }

        //VIZUALIZAR DATOS EXCEL COMPLETO--------------------------------------------------------------------
        public void MostrarExcel()
        {
            datalistadoTodasLiquidacionExcel.Rows.Clear();

            foreach (DataGridViewRow dgv in datalistadoTodasLiquidacion.Rows)
            {
                string numeroReque = dgv.Cells[1].Value.ToString();
                string numeroLiqui = dgv.Cells[2].Value.ToString();
                string FechaGen = dgv.Cells[3].Value.ToString();
                string fechaInicio = dgv.Cells[4].Value.ToString();
                string fechaTermino = dgv.Cells[5].Value.ToString();
                string responsable = dgv.Cells[7].Value.ToString();
                string motivoVisita = dgv.Cells[8].Value.ToString();
                string total = dgv.Cells[9].Value.ToString();
                string adelanto = dgv.Cells[10].Value.ToString();
                string saldo = dgv.Cells[11].Value.ToString();
                string estadoComercial = dgv.Cells[12].Value.ToString();
                string estadoContabilidad = dgv.Cells[13].Value.ToString();

                bool estadoActas = Convert.ToBoolean(dgv.Cells[14].Value.ToString());
                string desEstadoActas = "";
                if (estadoActas == true) { desEstadoActas = "REALIZADA"; } else { desEstadoActas = "NO REALIZADA"; }

                datalistadoTodasLiquidacionExcel.Rows.Add(new[] { numeroReque, numeroLiqui, FechaGen, fechaInicio, fechaTermino, responsable, motivoVisita, total, adelanto, saldo, estadoComercial, estadoContabilidad, desEstadoActas });
            }
        }
        //-----------------------------------------------------------------------------

        //LISTADO DE LIQUIDACIONES Y SELECCION DE PDF Y ESTADO DE ACTAS---------------------
        //MOSTRAR REQUERIMIENTOS AL INCIO 
        public void MostrarLiquidación(DateTime fechaInicio, DateTime fechaTermino)
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
                    cmd = new SqlCommand("MostrarLiquidacionesVentasPorFecha_Jefatura", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoTodasLiquidacion.DataSource = dt;
                    con.Close();
                    OrdenarListadoLiqui(datalistadoTodasLiquidacion);
                }
                catch (Exception ex)
                {
                    //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                    ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                    MessageBox.Show("Error en la operación por: " + ex.Message);
                }
            }
            else
            {
                lblCarga.Text = "0";
            }

            //deshabilitar el click y  reordenamiento por columnas
            foreach (DataGridViewColumn column in datalistadoTodasLiquidacion.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //MOSTRAR REQUERIMIENTOS POR RESPONSABLE
        public void MostrarLiquidacionesResponsable(string resopnsable, DateTime fechaInicio, DateTime fechaTermino)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarLiquidacionVentasPorResponsable", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@responsable", resopnsable);
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoTodasLiquidacion.DataSource = dt;
                con.Close();
                OrdenarListadoLiqui(datalistadoTodasLiquidacion);
            }
            catch(Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show(ex.Message);
            }
        }

        //MOSTRAR REQUERIMIENTOS POR ESTADOS
        public void MostrarLiquidacionesEstados(int estados, DateTime fechaInicio, DateTime fechaTermino)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarLiquidacionesVentasPorEstados_Jefatura", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@estado", estados);
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoTodasLiquidacion.DataSource = dt;
                con.Close();
                OrdenarListadoLiqui(datalistadoTodasLiquidacion);
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show(ex.Message);
            }
        }

        //ORDENAR MIS COLUMNAS
        public void OrdenarListadoLiqui(DataGridView DGV)
        {
            DGV.Columns[6].Visible = false;
            DGV.Columns[15].Visible = false;
            DGV.Columns[1].Width = 55;
            DGV.Columns[2].Width = 55;
            DGV.Columns[3].Width = 90;
            DGV.Columns[4].Width = 90;
            DGV.Columns[5].Width = 90;
            DGV.Columns[7].Width = 150;
            DGV.Columns[8].Width = 350;
            DGV.Columns[9].Width = 75;
            DGV.Columns[10].Width = 75;
            DGV.Columns[11].Width = 75;
            DGV.Columns[12].Width = 100;
            DGV.Columns[13].Width = 100;
            DGV.Columns[14].Width = 80;
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
                for (var i = 0; i <= datalistadoTodasLiquidacion.RowCount - 1; i++)
                {
                    if (datalistadoTodasLiquidacion.Rows[i].Cells[12].Value.ToString() == "APROBADO")
                    {
                        //APROBADP
                        datalistadoTodasLiquidacion.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                    }
                    else if (datalistadoTodasLiquidacion.Rows[i].Cells[12].Value.ToString() == "PENDIENTE")
                    {
                        //PENDIENTE
                        datalistadoTodasLiquidacion.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        //DESAPROBADO
                        datalistadoTodasLiquidacion.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //MOSTRAR LIQUIDACIONES POR RESPONSABLE
        private void txtBusquedaResponsable_TextChanged(object sender, EventArgs e)
        {
            MostrarLiquidacionesResponsable(txtBusquedaResponsable.Text, DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR LIQUIDACIONES AL MONENTO DE CAMBIO DE FECHAS
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarLiquidación(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR LIQUIDACIONES AL MONENTO DE CAMBIO DE FECHAS
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarLiquidación(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR LIQUIDACOPMES A MOEMNTO DE APLICAR UN FILTRO
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarLiquidación(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR LIQUIDACOPMES A MOEMNTO DE APLICAR UN FILTRO
        private void btnBusquedaAprobados_Click(object sender, EventArgs e)
        {
            MostrarLiquidacionesEstados(2, DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR LIQUIDACOPMES A MOEMNTO DE APLICAR UN FILTRO
        private void btnBusquedaPendientes_Click(object sender, EventArgs e)
        {
            MostrarLiquidacionesEstados(1, DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR LIQUIDACOPMES A MOEMNTO DE APLICAR UN FILTRO
        private void btnBusquedaDesaprobado_Click(object sender, EventArgs e)
        {
            MostrarLiquidacionesEstados(0, DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR PDF DE LA LIQUIDACION SIN FIRMA DE JEFATURA
        private void btnVerRequerimiento_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasLiquidacion.CurrentRow != null)
            {
                try
                {
                    string codigoLiquidacionReporte = "";

                    if (datalistadoTodasLiquidacion.SelectedCells[12].Value.ToString() == "ANULADO")
                    {
                        codigoLiquidacionReporte = datalistadoTodasLiquidacion.Rows[datalistadoTodasLiquidacion.CurrentRow.Index].Cells[1].Value.ToString();
                        Visualizadores.VisualizarLiquidacionDesaprobada frm = new Visualizadores.VisualizarLiquidacionDesaprobada();
                        frm.lblCodigo.Text = codigoLiquidacionReporte;

                        frm.Show();
                    }
                    else if (datalistadoTodasLiquidacion.SelectedCells[12].Value.ToString() == "APROBADO")
                    {
                        codigoLiquidacionReporte = datalistadoTodasLiquidacion.Rows[datalistadoTodasLiquidacion.CurrentRow.Index].Cells[1].Value.ToString();
                        Visualizadores.VisualizarLiquidacionAprobada frm = new Visualizadores.VisualizarLiquidacionAprobada();
                        frm.lblCodigo.Text = codigoLiquidacionReporte;

                        frm.Show();
                    }
                    else
                    {
                        codigoLiquidacionReporte = datalistadoTodasLiquidacion.Rows[datalistadoTodasLiquidacion.CurrentRow.Index].Cells[1].Value.ToString();
                        Visualizadores.VisualizarLiquidacionesVenta frm = new Visualizadores.VisualizarLiquidacionesVenta();
                        frm.lblCodigo.Text = codigoLiquidacionReporte;

                        frm.Show();
                    }

                    //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                    ClassResourses.RegistrarAuditora(6, this.Name, 5, Program.IdUsuario, "Visualización de liquidación de viaje PDF", Convert.ToInt32(codigoLiquidacionReporte));
                }
                catch(Exception ex)
                {
                    //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                    ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una liquidación para poder generar el PDF.", "Validación del Sistema",MessageBoxButtons.OK);
            }
        }
        //------------------------------------------------------------------------------------------------------------

        //SELECCION DE LA LIQUIDACION Y CARGA DE SUS DETALLES---------------------------
        //PROCESO PARA BUSCAR LOS DETALLES DEL CLIENTE DE LA LIQUIDACIÓN
        public void BuscarLiquidacionDetalles(int codigoLiquidacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarDetallesClientesLiquidacion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigo", codigoLiquidacion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoLiquidacionActas.DataSource = dt;
                con.Close();

                datalistadoLiquidacionActas.Columns[1].Visible = false;
                datalistadoLiquidacionActas.Columns[4].Visible = false;
                datalistadoLiquidacionActas.Columns[6].Visible = false;
                datalistadoLiquidacionActas.Columns[11].Visible = false;
                datalistadoLiquidacionActas.Columns[2].Width = 80;
                datalistadoLiquidacionActas.Columns[3].Width = 80;
                datalistadoLiquidacionActas.Columns[5].Width = 340;
                datalistadoLiquidacionActas.Columns[7].Width = 100;
                datalistadoLiquidacionActas.Columns[8].Width = 100;
                datalistadoLiquidacionActas.Columns[9].Width = 75;
                datalistadoLiquidacionActas.Columns[10].Width = 80;

                ColoresListadoDetalleLiquidación();
            }
            catch(Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show(ex.Message);
            }
        }

        //ABRIR DETALLES DE LA LIQUIDACIÓN------------------------------------------------------
        //SELECCION DEL PDF GENERADO CON SUS RESPECTIVAS FIRMAS, INCLUIDO LA JEFATURA
        private void datalistadoTodasLiquidacion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoTodasLiquidacion.Columns[e.ColumnIndex];

            if (currentColumn.Name == "btnGenerarPdf")
            {
                if (datalistadoTodasLiquidacion.CurrentRow != null)
                {
                    try
                    {
                        string codigoLiquidacionReporte = "";
                        if (datalistadoTodasLiquidacion.SelectedCells[12].Value.ToString() == "ANULADO")
                        {
                            codigoLiquidacionReporte = datalistadoTodasLiquidacion.Rows[datalistadoTodasLiquidacion.CurrentRow.Index].Cells[1].Value.ToString();
                            Visualizadores.VisualizarLiquidacionDesaprobada frm = new Visualizadores.VisualizarLiquidacionDesaprobada();
                            frm.lblCodigo.Text = codigoLiquidacionReporte;
                            frm.Show();
                        }
                        else if (datalistadoTodasLiquidacion.SelectedCells[12].Value.ToString() == "APROBADO")
                        {
                            codigoLiquidacionReporte = datalistadoTodasLiquidacion.Rows[datalistadoTodasLiquidacion.CurrentRow.Index].Cells[1].Value.ToString();
                            Visualizadores.VisualizarLiquidacionAprobada frm = new Visualizadores.VisualizarLiquidacionAprobada();
                            frm.lblCodigo.Text = codigoLiquidacionReporte;
                            frm.Show();
                        }
                        else
                        {
                            codigoLiquidacionReporte = datalistadoTodasLiquidacion.Rows[datalistadoTodasLiquidacion.CurrentRow.Index].Cells[1].Value.ToString();
                            Visualizadores.VisualizarLiquidacionesVenta frm = new Visualizadores.VisualizarLiquidacionesVenta();
                            frm.lblCodigo.Text = codigoLiquidacionReporte;
                            frm.Show();
                        }

                        //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                        ClassResourses.RegistrarAuditora(6, this.Name, 5, Program.IdUsuario, "Visualización de liquidación de viaje PDF", Convert.ToInt32(codigoLiquidacionReporte));
                    }
                    catch(Exception ex)
                    {
                        //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                        ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar una liquidación para poder generar el PDF.", "Validación del Sistema",MessageBoxButtons.OK);
                }
            }
        }

        //VUSUALIZAR MI ACTA
        private void btnVisualizarLiquidacionActas_Click(object sender, EventArgs e)
        {
            if (datalistadoLiquidacionActas.CurrentRow != null)
            {
                if (datalistadoLiquidacionActas.SelectedCells[10].Value.ToString() == "APROBADO")
                {
                    try
                    {
                        string codigoActaReporte = datalistadoLiquidacionActas.Rows[datalistadoLiquidacionActas.CurrentRow.Index].Cells[11].Value.ToString();
                        Visualizadores.VisualizarActa frm = new Visualizadores.VisualizarActa();
                        frm.lblCodigo.Text = codigoActaReporte;

                        frm.Show();

                        //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                        ClassResourses.RegistrarAuditora(6, this.Name, 5, Program.IdUsuario, "Visualización de acta aprovada de viaje PDF", Convert.ToInt32(codigoActaReporte));
                    }
                    catch (Exception ex)
                    {
                        //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                        ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                        MessageBox.Show("Error en la operación por: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Debe tener la aprobación de las jefaturas para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una acta para poder generar el PDF.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //ABRIR LOS DETALLES DE LA LIQUIDACION CON EL EVENTO DOBLE CLICK
        private void datalistadoLiquidacionActas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoLiquidacionActas.CurrentRow != null)
            {
                int codigoLiquidacion = Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[1].Value.ToString());
                txtCodigoLiquidacion.Text = Convert.ToString(codigoLiquidacion);
                panelLiquidacionActas.Visible = true;
                BuscarLiquidacionDetalles(codigoLiquidacion);
            }
            else
            {
                MessageBox.Show("Debe seleccionar una liquidación para poder ver sus detalles.", "Validación del Sistema",MessageBoxButtons.OK);
            }
        }

        //CERRAR LOS DETALLES DE LA LIQUIDACIÓN
        private void btnCerrarLiquidacionActas_Click(object sender, EventArgs e)
        {
            panelLiquidacionActas.Visible = false;
        }
        //-----------------------------------------------------------------------------------------

        //GENERACIÓN DEL ACTA DE VISITA POR DETALLE--------------------------------------------
        //COLOREAR LISTADO DE DETALLES DE LA LIQUIDACIÓN
        //COLOREAR REGISTROS
        public void ColoresListadoDetalleLiquidación()
        {
            try
            {
                for (var i = 0; i <= datalistadoLiquidacionActas.RowCount - 1; i++)
                {
                    if (datalistadoLiquidacionActas.Rows[i].Cells[10].Value.ToString() == "APROBADO")
                    {
                        //APROBADP
                        datalistadoLiquidacionActas.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                    }
                    else if (datalistadoLiquidacionActas.Rows[i].Cells[10].Value.ToString() == "PENDIENTE")
                    {
                        //PENDIENTE
                        datalistadoLiquidacionActas.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        //DESAPROBADO
                        datalistadoLiquidacionActas.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                    }
                }
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //FUNCION PARA ATENDER LALIQUIDACION SELECCIONADA
        private void btnAtenderLiquidacion_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasLiquidacion.CurrentRow != null)
            {
                DialogResult boton = MessageBox.Show("¿Realmente desea atender esta liquidación?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    int idLiquidacion = Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[1].Value.ToString());
                    string estadoContabilidad = datalistadoTodasLiquidacion.SelectedCells[12].Value.ToString();

                    if (estadoContabilidad == "LIQUIDADO")
                    {
                        MessageBox.Show("Esta liquidación ya ha sido liquidada.", "Validación del Sistema",MessageBoxButtons.OK);
                    }
                    else
                    {
                        try
                        {
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("CambioEstadoLiquidacionVenta_Contabilidad", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idLiquidacion", idLiquidacion);
                            cmd.Parameters.AddWithValue("@estadoContabilidad", 1);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Liquidación atendida exitosamente.", "Validación del Sistema",MessageBoxButtons.OK);

                            MostrarLiquidación(DesdeFecha.Value, HastaFecha.Value);

                            //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                            ClassResourses.RegistrarAuditora(3, this.Name, 5, Program.IdUsuario, "Atención de una liquidación de viaje.", Convert.ToInt32(idLiquidacion));
                        }
                        catch (Exception ex)
                        {
                            //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                            ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un requerimiento para poder generar una liquidación.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //FUNCION PARA PODER ANULAR NA LIQUIDACION
        private void btnAnularRequerimiento_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasLiquidacion.CurrentRow != null)
            {
                int idLiquidacion = Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[1].Value.ToString());
                int idRequerimiento = Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[13].Value.ToString());

                DialogResult boton = MessageBox.Show("¿Realmente desea anular esta liquidación?. Se desaprobará el requerimiento asociado a esta liquidación.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        SqlCommand cmd = new SqlCommand();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        cmd = new SqlCommand("DesaprobarLiquidacion", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idliquidacion", idLiquidacion);
                        cmd.Parameters.AddWithValue("@idrequerimiento", idRequerimiento);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Liquidación y requerimiento asociado a esta, anulados exitosamente.", "Validación del Sistema", MessageBoxButtons.OK);
                        MostrarLiquidación(DesdeFecha.Value, HastaFecha.Value);

                        //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                        ClassResourses.RegistrarAuditora(2, this.Name, 5, Program.IdUsuario, "Anular liquidación de viaje.", Convert.ToInt32(idLiquidacion));
                    }
                    catch (Exception ex)
                    {
                        //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                        ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una liquidación para poder anularla.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //BOTON PARA ABRORO EL MANUAL DE USUARIO DEL SISTEMA
        private void btnInfoDetalle_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //BOTON PARA ABRORO EL MANUAL DE USUARIO DEL SISTEMA
        private void btnInfo_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //BOTON PARA LLAMAR A LA FUNCION DE EXPORTAR EXCEL
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
                sl.SetColumnWidth(2, 15);
                sl.SetColumnWidth(3, 20);
                sl.SetColumnWidth(4, 20);
                sl.SetColumnWidth(5, 20);
                sl.SetColumnWidth(6, 35);
                sl.SetColumnWidth(7, 75);
                sl.SetColumnWidth(8, 20);
                sl.SetColumnWidth(9, 20);
                sl.SetColumnWidth(10, 20);
                sl.SetColumnWidth(11, 35);
                sl.SetColumnWidth(12, 35);
                sl.SetColumnWidth(13, 35);

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
                foreach (DataGridViewColumn column in datalistadoTodasLiquidacionExcel.Columns)
                {
                    sl.SetCellValue(1, ic, column.HeaderText.ToString());
                    sl.SetCellStyle(1, ic, style);
                    ic++;
                }

                int ir = 2;
                foreach (DataGridViewRow row in datalistadoTodasLiquidacionExcel.Rows)
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
                    ir++;
                }

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                sl.SaveAs(desktopPath + @"\Reporte de Liquidaciones.xlsx");
                MessageBox.Show("Se exportó los datos a un archivo de Microsoft Excel en la siguiente ubicación: " + desktopPath, "Validación del Sistema", MessageBoxButtons.OK);

                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(5, this.Name, 5, Program.IdUsuario, "Exportar listado de liquidaciones de ventas EXCEL", 0);
            }
            catch(Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show(ex.Message);
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

                if (datalistadoTodasLiquidacion.SelectedCells[12].Value.ToString() == "ANULADO")
                {
                    rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeLiquidacionVentaAnulada.rpt");
                }
                else if (datalistadoTodasLiquidacion.SelectedCells[12].Value.ToString() == "APROBADO")
                {
                    rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeLiquidacionVentaAprobada.rpt");
                }
                else
                {
                    rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeLiquidacionVenta.rpt");
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
                int idLiquidacion = Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[1].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                crystalReport.SetParameterValue("@idLiquidacion", idLiquidacion);

                // Ruta de salida en el escritorio
                string rutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string rutaSalida = System.IO.Path.Combine(rutaEscritorio, "Liquidación de viaje número " + idLiquidacion + ".pdf");

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

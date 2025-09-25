using ArenasProyect3.Modulos.Mantenimientos;
using ArenasProyect3.Modulos.Resourses;
using ArenasProyect3.Reportes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2013.Drawing.Chart;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using FlashControlV71;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Management;
using System.Web.UI;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;


namespace ArenasProyect3.Modulos.Comercial
{
    public partial class ReportesComercial : Form
    {

        bool checksdesactivados = false;
        //CONSTRUCTOR DEL MANTENIMIENTO - MANTENIEMINTO
        public ReportesComercial()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI MANTENIMIENTOS
        private void ReportesComercial_Load(object sender, EventArgs e)
        {
            cboCriterioBusquedaReque.SelectedIndex = 0;

            cboCriterioBusquedaLiqui.SelectedIndex = 0;

            cboCriterioBusquedaActas.SelectedIndex = 0;
        }

        //REPORTES DE REQEURIEMITNOS---------------------------------------------------------------
        //HABILITAR REQUERIEMINTOS
        private void btnReportesRequerimeinto_Click(object sender, EventArgs e)
        {
            tbReportesReque.Visible = true;
            panelReportesRequerimiento.Visible = true;


            panelReportesLiquidaciones.Visible = false;
            panelReportesActas.Visible = false;


            //DEFINICIÓN PARA LAS FECHAS DE LA SECCIÓN DE GRAFICOS CON REQUERIMIENTOS
            DateTime fechaactual = DateTime.Now;
            DateTime primerdiaMes = new DateTime(fechaactual.Year, fechaactual.Month, 1).AddMonths(-1);
            DateTime ultimodiaMes = new DateTime(fechaactual.Year, fechaactual.Month, 1).AddMonths(1).AddDays(-1); DesdeGrafi.Value = primerdiaMes;
            HastaGrafi.Value = ultimodiaMes;

            //CARGA DE GRAFICOS A TRAVES DE LAS FECHAS ESTABLECIDAS
            Requerimientos_MostrarGraficoBarras(DesdeGrafi.Value, HastaGrafi.Value);
            Requerimientos_MostrarGraficoCircular(DesdeGrafi.Value, HastaGrafi.Value);

        }

        //METODO QUE AGREGAR LOS ITEMS A MI COMBOBOX DE SELECCION DE TIPOS
        public void Requerimientos_CargaItemsTipos_DeBusqueda(ComboBox cbo1, ComboBox cbo2,GroupBox EstadoComercial,GroupBox EstadoContabilidad)
        {
            if (cbo1.Text == "SELECCIONE UNA BUSQUEDA" || cbo1.Text == "RESPONSABLE" || cbo1.Text == "CLIENTE" || cbo1.Text == "SIN FILTROS")
            {
                EstadoComercial.Visible = false;
                EstadoContabilidad.Visible = false;

                cbo2.Enabled = false;
                cbo2.DataSource = null;
            }
            else
            {
                cbo2.Enabled = true;
            }

            if (cbo1.Text == "ESTADO COMERCIAL")
            {
                cbo2.DataSource = null;
                cbo2.Enabled = false;
                EstadoComercial.Visible = true;
                EstadoContabilidad.Visible = false;
            }


            if (cbo1.Text == "ESTADO CONTABILIDAD")
            {
                cbo2.DataSource = null;
                cbo2.Enabled = false;
                EstadoContabilidad.Visible = true;
                EstadoComercial.Visible = false;
            }

            if (cbo1.Text == "VEHICULO")
            {
                EstadoComercial.Visible = false;
                EstadoContabilidad.Visible = false;

                cbo2.DataSource = null;
                CargarVehiculos(cbo2);
                cbo2.SelectedIndex = 0;
            }
            if (cbo1.Text == "MONEDA")
            {
                EstadoComercial.Visible = false;
                EstadoContabilidad.Visible = false;

                cbo2.DataSource = null;
                CargarTiposMonedas(cbo2);
                cbo2.SelectedIndex = 0;
            }

        }

        //BLOQUEO DE CONTROLES X MEDIO DEL TIPO DE BUSQUEDA SELECCIONADA
        private void Requerimiento_LimpiarCombo_BLoquear_BusquedaSeleccionada(TextBox busquedaxdescripcion, DataGridView DGV, string criteriobusqueda, Button mostrartodo,CheckBox ckaprobado,CheckBox ckpendiente
            , CheckBox ckdesaprobado, CheckBox ckatendido, CheckBox cknoatendido, CheckBox ckanulado)
        {
            busquedaxdescripcion.Text = "";
            DGV.DataSource = null;
            ckaprobado.Checked = false;
            ckpendiente.Checked = false;
            ckdesaprobado.Checked = false;
            ckatendido.Checked = false;
            cknoatendido.Checked = false;
            ckanulado.Checked = false;

            if (criteriobusqueda == "SELECCIONE UNA BUSQUEDA")
            {
                mostrartodo.Enabled = false;
                busquedaxdescripcion.Enabled = false;
            }
            else
            {
                mostrartodo.Enabled = true;
                busquedaxdescripcion.Enabled = true;
            }

            if (criteriobusqueda == "ESTADO COMERCIAL" || criteriobusqueda == "ESTADO CONTABILIDAD" || criteriobusqueda == "VEHICULO" || criteriobusqueda == "MONEDA" || criteriobusqueda == "SIN FILTROS")
            {
                busquedaxdescripcion.Enabled = false;
            }
        }

        //CARGA DEL COMBOBOX CON LOS VEHICULOS REGISTRADOS EN LA BASE DE DATOS
        public void CargarVehiculos(ComboBox cbo)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT IdVehiculo,Descripcion FROM Vehiculos where Estado = 1", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cbo.DisplayMember = "Descripcion";
                cbo.ValueMember = "IdVehiculo";
                cbo.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGA DE LOS TIPOS DE MONEDA REGISTRADOS EN LA BASE DE DATOS
        public void CargarTiposMonedas(ComboBox cbo)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand("select IdTipoMonedas,Descripcion from TipoMonedas where Estado = 1", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cbo.DisplayMember = "Descripcion";
                cbo.ValueMember = "IdTipoMonedas";
                cbo.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGA DE LOS REQUERIMIENTOS SI NO SE UNA DESCRIPCION EN RESPONSALBE O CLIENTE
        public void MostrarRequerimientos_PorFecha(DateTime desde, DateTime hasta, DataGridView DGV, string criteriobusqueda)
        {
            try
            {
                if (criteriobusqueda == "SIN FILTROS")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("ReporteComercial_MostrarRequerimientos", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechaInicio", desde);
                    cmd.Parameters.AddWithValue("@fechaTermino", hasta);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                    ColoresListado(DGV, criteriobusqueda);
                }
            }

            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //CARGA DE LOS REQUERIMIENTOS CON ESTADO COMERCIAL 
        public void MostrarRequerimientosPor_EstadoComercial(DateTime desde, DateTime hasta, string criteriobusqueda, DataGridView DGV, CheckBox ckaprobado,
            CheckBox ckpendiente, CheckBox ckdesaprobado)
        {
            try
            {
                if (criteriobusqueda == "ESTADO COMERCIAL")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();


                    if (ckaprobado.Checked == true)
                    {
                        SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarRequerimientosXEstadosComercial", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fechainicio", desde);
                        cmd.Parameters.AddWithValue("@fechatermino", hasta);
                        cmd.Parameters.AddWithValue("@estadocomercial", 2);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    if (ckpendiente.Checked == true)
                    {
                        SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarRequerimientosXEstadosComercial", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fechainicio", desde);
                        cmd.Parameters.AddWithValue("@fechatermino", hasta);
                        cmd.Parameters.AddWithValue("@estadocomercial", 1);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    if (ckdesaprobado.Checked == true)
                    {
                        SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarRequerimientosXEstadosComercial", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fechainicio", desde);
                        cmd.Parameters.AddWithValue("@fechatermino", hasta);
                        cmd.Parameters.AddWithValue("@estadocomercial", 0);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    DGV.DataSource = dt;
                    con.Close();

                    ColoresListado(DGV, criteriobusqueda);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGA DE LOS REQUERIMIENTOS CON ESTADO CONTABILIDAD 
        public void MostrarRequerimientosPor_EstadoContabilidad(DateTime desde, DateTime hasta, string criteriobusqueda, DataGridView DGV
            , CheckBox ckatendido, CheckBox cknoatendido, CheckBox ckanulado)
        {
            try
            {
                if (criteriobusqueda == "ESTADO CONTABILIDAD")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();


                    if (ckatendido.Checked == true)
                    {
                        SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarRequerimientosXEstadosContabilidad", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fechadesde", desde);
                        cmd.Parameters.AddWithValue("@fechahasta", hasta);
                        cmd.Parameters.AddWithValue("@estadocontabilidad", 2);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    if (cknoatendido.Checked == true)
                    {
                        SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarRequerimientosXEstadosContabilidad", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fechadesde", desde);
                        cmd.Parameters.AddWithValue("@fechahasta", hasta);
                        cmd.Parameters.AddWithValue("@estadocontabilidad", 1);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    if (ckanulado.Checked == true)
                    {
                        SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarRequerimientosXEstadosContabilidad", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fechadesde", desde);
                        cmd.Parameters.AddWithValue("@fechahasta", hasta);
                        cmd.Parameters.AddWithValue("@estadocontabilidad", 0);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }

                    DGV.DataSource = dt;
                    con.Close();

                    ColoresListado(DGV, criteriobusqueda);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //CARGA DE LOS REQUERIMIENTOS MEDIANTE LA DESCRIPCION
        public void MostrarRequerimientosPor_Descripcion(DateTime desde, DateTime hasta, string descripcion, string criteriobusqueda, DataGridView DGV)
        {
            try
            {
                if (criteriobusqueda == "RESPONSABLE")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarRequerimientos_BusquedaXResponsable", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechadesde", desde);
                    cmd.Parameters.AddWithValue("@fechahasta", hasta);
                    cmd.Parameters.AddWithValue("@responsable", descripcion);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();

                    ColoresListado(DGV, criteriobusqueda);
                }
                else if (criteriobusqueda == "CLIENTE")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarRequerimientos_BusquedaXCliente", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechadesde", desde);
                    cmd.Parameters.AddWithValue("@fechahasta", hasta);
                    cmd.Parameters.AddWithValue("@cliente", descripcion);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                    ColoresListado(DGV, criteriobusqueda);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGA DE LOS REQUERIMIENTOS MEDIANTE EL VEHICULO SELECCIONADO
        public void MostrarRequerimientosPor_Vehiculo(DateTime desde, DateTime hasta, int vehiculoseleccionado, string criteriobusqueda, DataGridView DGV)
        {
            try
            {
                if (criteriobusqueda == "VEHICULO")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarRequerimientos_BusquedaXVehiculo", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechadesde", desde);
                    cmd.Parameters.AddWithValue("@fechahasta", hasta);
                    cmd.Parameters.AddWithValue("@vehiculo", vehiculoseleccionado);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();

                    ColoresListado(DGV, criteriobusqueda);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGA DE LOS REQUERIMIENTOS MEDIANTE EL TIPO DE MONEDA SELECCIONADO
        public void MostrarRequerimientosPor_TipoMoneda(DateTime desde, DateTime hasta, int tipomoneda, string criteriobusqueda, DataGridView DGV)
        {
            try
            {
                if (criteriobusqueda == "MONEDA")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarRequerimientos_BusquedaXTipoMoneda", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechadesde", desde);
                    cmd.Parameters.AddWithValue("@fechahasta", hasta);
                    cmd.Parameters.AddWithValue("@tipomoneda", tipomoneda);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                    ColoresListado(DGV, criteriobusqueda);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //METODO PARA VALIDAR QUE LOS CHECKS ESTEN ACTIVOS
        public void Requerimientos_ValidarChecksActivos(DateTime Desde,DateTime Hasta,DataGridView DGV,string criteriobusqueda, CheckBox ckaprobado, CheckBox ckpendiente, CheckBox ckdesaprobado, CheckBox ckatendido
            , CheckBox cknoatendido, CheckBox ckanulado)
        {
            if (criteriobusqueda == "ESTADO COMERCIAL")
            {
                if (ckaprobado.Checked == false && ckpendiente.Checked == false && ckdesaprobado.Checked == false)
                {
                    MessageBox.Show("Marque el tipo de busqueda que desee visualizar.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    MostrarRequerimientosPor_EstadoComercial(Desde, Hasta, criteriobusqueda, DGV, ckaprobado,ckpendiente, ckdesaprobado);
                }
            }
            if (criteriobusqueda == "ESTADO CONTABILIDAD")
            {
                if (ckatendido.Checked == false && cknoatendido.Checked == false && ckanulado.Checked == false)
                {
                    MessageBox.Show("Marque el tipo de busqueda que desee visualizar.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    MostrarRequerimientosPor_EstadoContabilidad(Desde, Hasta, criteriobusqueda, DGV,ckatendido, cknoatendido, ckanulado);
                }
            }
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO
        public void ColoresListado(DataGridView DGV, string criteriobusqueda)
        {
            try
            {
                //COLOR PARA LOS ESTADOS DE COMERCIAL
                if (criteriobusqueda == "ESTADO COMERCIAL" || criteriobusqueda == "ESTADO CONTABILIDAD")
                {
                    //RECORRIDO DE MI LISTADO
                    for (var i = 0; i <= DGV.RowCount - 1; i++)
                    {
                        if (DGV.Rows[i].Cells[10].Value.ToString() == "APROBADO" || DGV.Rows[i].Cells[10].Value.ToString() == "ATENDIDO")
                        {
                            DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                        }
                        else if (DGV.Rows[i].Cells[10].Value.ToString() == "PENDIENTE" || DGV.Rows[i].Cells[10].Value.ToString() == "NO ATENDIDO")
                        {
                            DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                        }
                        else
                        {
                            DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }

                //COLOR PARA RESPONSABLE,CLIENTE,VEHICULO Y MONEDA
                if (criteriobusqueda == "RESPONSABLE" || criteriobusqueda == "VEHICULO" || criteriobusqueda == "MONEDA" || criteriobusqueda == "CLIENTE" || criteriobusqueda == "SIN FILTROS")
                {
                    //RECORRIDO DE MI LISTADO
                    for (var i = 0; i <= DGV.RowCount - 1; i++)
                    {
                        if (DGV.Rows[i].Cells[10].Value.ToString() == "APROBADO" && DGV.Rows[i].Cells[11].Value.ToString() == "ATENDIDO")
                        {
                            DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                        }
                        else if (DGV.Rows[i].Cells[10].Value.ToString() == "PENDIENTE" && DGV.Rows[i].Cells[11].Value.ToString() == "NO ATENDIDO" || DGV.Rows[i].Cells[10].Value.ToString() == "APROBADO" && DGV.Rows[i].Cells[11].Value.ToString() == "NO ATENDIDO" || DGV.Rows[i].Cells[10].Value.ToString() == "PENDIENTE" && DGV.Rows[i].Cells[11].Value.ToString() == "ATENDIDO")
                        {
                            DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                        }
                        else
                        {
                            DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        private void cboCriterioBusqueda_SelectedIndexChanged(object sender, EventArgs e)
        {
            Requerimiento_LimpiarCombo_BLoquear_BusquedaSeleccionada(txtBusquedaReque, datalistadoRequerimiento, cboCriterioBusquedaReque.Text, btnMostrarTodoReque,ckAprobadoReque,ckPendienteReque,ckDesaprobadoReque
                ,ckAtendidosReque,ckNoAtendidosReque,ckAnuladosReque);

            //CARGA DE ITEMS PARA LA ELECCION DE BUSQUEDAS
            Requerimientos_CargaItemsTipos_DeBusqueda(cboCriterioBusquedaReque, cBobusquedaSeleccionadaReque,grpEstadoComercialReque,grpEstadoContabilidadReque);
        }


        //EVENTRO DE CARGA DE MI LISTADO DE REQUERIMIENTOS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            Requerimientos_ValidarChecksActivos(DesdeReque.Value,HastaReque.Value,datalistadoRequerimiento,cboCriterioBusquedaReque.Text, ckAprobadoReque, ckPendienteReque, ckDesaprobadoReque, ckAtendidosReque, ckNoAtendidosReque, ckAnuladosReque);

            //METODOS DE CARGA PARA EL LISTADO
            MostrarRequerimientos_PorFecha(DesdeReque.Value, HastaReque.Value, datalistadoRequerimiento, cboCriterioBusquedaReque.Text);
            MostrarRequerimientosPor_Descripcion(DesdeReque.Value, HastaReque.Value, txtBusquedaReque.Text, cboCriterioBusquedaReque.Text, datalistadoRequerimiento);
            MostrarRequerimientosPor_Vehiculo(DesdeReque.Value, HastaReque.Value, Convert.ToInt32(cBobusquedaSeleccionadaReque.SelectedValue), cboCriterioBusquedaReque.Text, datalistadoRequerimiento);
            MostrarRequerimientosPor_TipoMoneda(DesdeReque.Value, HastaReque.Value, Convert.ToInt32(cBobusquedaSeleccionadaReque.SelectedValue), cboCriterioBusquedaReque.Text, datalistadoRequerimiento);
        }

        //EVENTO QUE PASA LA DEFINICIÓN DE NOMBREARCHIVO AL LABEL
        private void cboSeleccionEstadosReque_SelectedIndexChanged(object sender, EventArgs e)
        {
            datalistadoRequerimiento.DataSource = null;
        }

        //BUSQUEDA POR DESCRIPCION PARA EL CLIENTE Y RESPONSABLE
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            MostrarRequerimientosPor_Descripcion(DesdeReque.Value, HastaReque.Value, txtBusquedaReque.Text, cboCriterioBusquedaReque.Text, datalistadoRequerimiento);
        }


        ///-------------------------------------------------------------
        ///
        ///METODO PARA EXPORTAR EL LISTADO A EXCEL

        public void Requerimientos_NombreArchivos_Exportado(Label nombrearchivo, string criteriobusqueda, string busquedaseleccionada, CheckBox ckaprobado, CheckBox ckpendiente, CheckBox ckdesaprobado, CheckBox ckatendido, CheckBox cknoatendido
            , CheckBox ckanulado)
        {
            //DEFINICIÓN DE NOMBRE DE ARCHIVO PARA EL TIPO DE BUSQUEDA DE ESTADO COMERCIAL 
            if (criteriobusqueda == "ESTADO COMERCIAL")
            {
                if (ckaprobado.Checked == true && ckpendiente.Checked == true && ckdesaprobado.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Requerimientos_TodosEstadosComercial";
                }

                else if (ckaprobado.Checked == true && ckpendiente.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Requerimientos_Aprobados_Pendientes";
                }

                else if (ckaprobado.Checked == true && ckdesaprobado.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Requerimientos_Aprobados_Desaprobado";
                }

                else if (ckpendiente.Checked == true && ckdesaprobado.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Requerimientos_Pendientes_Desaprobado";
                }

                else if (ckaprobado.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Requerimientos_Aprobados";
                }
                else if (ckpendiente.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Requerimientos_Pendientes";
                }
                else if (ckdesaprobado.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Requerimientos_Anulados";
                }


            }

            //DEFINICIÓN DE NOMBRE DE ARCHIVO PARA EL TIPO DE BUSQUEDA DE ESTADO CONTABILIDAD 
            if (criteriobusqueda == "ESTADO CONTABILIDAD")
            {
                if (ckatendido.Checked == true && cknoatendido.Checked == true && ckanulado.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Requerimientos_TodosEstadosContabilidad";
                }

                else if (ckatendido.Checked == true && cknoatendido.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Requerimientos_Atendidos_NoAtendidos";
                }

                else if (ckatendido.Checked == true && ckanulado.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Requerimientos_Atendidos_Anulados";
                }

                else if (cknoatendido.Checked == true && ckanulado.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Requerimientos_NoAtendidos_Anulados";
                }

                else if (ckatendido.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Requerimientos_Atendidos";
                }
                else if (cknoatendido.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Requerimientos_NoAtendidos";
                }
                else if (ckanulado.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Requerimientos_Anulados";
                }


            }

            //DEFINICIÓN DE NOMBRE DE ARCHIVO PARA EL TIPO DE BUSQUEDA RESPONSABLE,CLIENTE,VEHICULO,MONEDA Y SIN FILTROS
            if (criteriobusqueda == "RESPONSABLE")
            {
                nombrearchivo.Text = "Reporte_Requerimientos_PorResponsable";
            }

            if (criteriobusqueda == "CLIENTE")
            {
                nombrearchivo.Text = "Reporte_Requerimientos_PorCliente";
            }

            if (criteriobusqueda == "VEHICULO")
            {
                nombrearchivo.Text = "Reporte_Requerimientos_PorVehiculo_" + busquedaseleccionada;
            }

            if (criteriobusqueda == "MONEDA")
            {
                nombrearchivo.Text = "Reporte_Requerimientos_PorMoneda_" + busquedaseleccionada;
            }

            if (criteriobusqueda == "SIN FILTROS")
            {
                nombrearchivo.Text = "Reporte_Requerimientos_Generales";
            }

        }
        //METODO QUE CAPTURA LOS DATOS RELEVANTES DEL LISTADO PRINCIPAL Y LOS REDIRIGE A MI LISTADO CON EL FIN DE EXPORTAR
        public void Requerimientos_MostrarExcel(string criteriobusqueda, DataGridView DGVExcel,DataGridView DGVListadoPrin)
        {

            if (DGVListadoPrin.DataSource == null || DGVListadoPrin.RowCount == 0)
            {
                MessageBox.Show("No se puede exportar un listado vacio.", "Validación del Sistema", MessageBoxButtons.OK);
                return;
            }

            ///INSERCIÓN DE COLUMNAS A MI LISTADO QUE PERMITIRA LA EXPORTACIÓN
            else
            {
                DGVExcel.Columns.Clear();
                //COLUMNAS QUE VENDRAN POR DEFECTO EN CADA LISTADO
                DGVExcel.Columns.Add("colNroReque", "N° RQ");

                DGVExcel.Columns.Add("colFechaGen", "FECHA DE GENERACIÓN");
                DGVExcel.Columns.Add("colFechaIni", "FECHA DE INICIO");
                DGVExcel.Columns.Add("colFechaTerm", "FECHA DE TÉRMINO");

                //COLUMNA AGREGADA SOLO SI ESTA EN EL TIPO DE BUSQUEDA DE CLIENTE


                DGVExcel.Columns.Add("colCliente", "CLIENTE");
                DGVExcel.Columns.Add("colResponsable", "RESPONSABLE");
                DGVExcel.Columns.Add("colMotivoVisi", "MOTIVO DE VISITA");
                DGVExcel.Columns.Add("colVehiculo", "VEHICULOS");
                DGVExcel.Columns.Add("colTipoMone", "TIPO MONEDA");
                DGVExcel.Columns.Add("colTotal", "TOTAL");

                //COLUMNAS DE ESTADO SOLAMENTE AGREGADAS DEPENDIENDO DEL TIPO DE BUSQUEDA QUE SE SELECCIONO
                if (criteriobusqueda == "ESTADO COMERCIAL")
                {
                    DGVExcel.Columns.Add("colEstadoJefa", "ESTADO DE JEFATURA");
                }
                else if (criteriobusqueda == "ESTADO CONTABILIDAD")
                {
                    DGVExcel.Columns.Add("colEstadoConta", "ESTADO DE CONTABILIDAD");
                }
                else
                {
                    DGVExcel.Columns.Add("colEstadoJefa", "ESTADO DE JEFATURA");
                    DGVExcel.Columns.Add("colEstadoConta", "ESTADO DE CONTABILIDAD");
                }
                DGVExcel.Columns.Add("colObservaciones", "OBSERVACIONES");



                ///////////////CAPTURA COLUMNAS A UTILIZAR PARA LA EXPORTACIÓN DEPENDIENDO DEL TIPO DE BUSQUEDA SELECCIONADO

                //DICCIONARIO EN EL QUE LAS CLAVES SERAN LOS TIPOS DE BUSQUEDA Y EL VALOR SERA LOS INDICES DE LAS COLUMNAS
                Dictionary<string, int[]> columnastipobusqueda = new Dictionary<string, int[]>
            {
                //ARREGLO DE ENTEROS PARA LOS INDICES
                {"ESTADO COMERCIAL" , new [] {0,1,2,3,4,5,6,7,8,9,10,12 } },
                {"ESTADO CONTABILIDAD" , new [] {0,1,2,3,4,5,6,7,8,9,10,12 } },
                {"RESPONSABLE" , new [] {0,1,2,3,4,5,6,7,8,9,10,11,13 } },
                {"CLIENTE" , new [] {0,1,2,3,4,5,6,7,8,9,10,11,13 } },
                {"VEHICULO" , new[] {0,1,2,3,4,5,6,7,8,9,10,11,13 } },
                {"MONEDA" , new[] {0,1,2,3,4,5,6,7,8,9,10,11,13 } },
                {"SIN FILTROS" , new[] {0,1,2,3,4,5,6,7,8,9,10,11,13 } },

            };

                //PASAMOS LOS INDICES DEPENDIENDO EL TIPO DE BUSQUEDA QUE SE SELECCIONO
                int[] columnas = columnastipobusqueda[criteriobusqueda];



                foreach (DataGridViewRow dgv in datalistadoRequerimiento.Rows)
                {
                    //LISTA QUE ALMACENARA LOS VALORES DE LAS CELDAS
                    List<string> fila = new List<string>();

                    //RECORRIDO AL ARREGLO QUE ALMACENA LOS INDICES DEL LISTADO PRINCIPAL
                    foreach (int i in columnas)
                    {
                        //SI HAY UNO NULL, SE GUARDA COMO "" CADENA VACIA
                        string valor = dgv.Cells[i].Value?.ToString() ?? "";
                        //AGREGAMOS VALORES AL LISTADO
                        fila.Add(valor);
                    }
                    //SE AÑADE A MI LISTADO CONVIRITNEDO LA LISTA EN UN ARREGLO (SE CONVIRTIO PORQUE ROWS.ADD NECESITA UN ARRAY)
                    DGVExcel.Rows.Add(fila.ToArray());
                }
            }
        }

        //METODO QUE REALIZARA LA EXPORTACIÓN A EXCEL
        public void Requerimiento_ExportarExcel_XTipoBusqueda(string criteriobusqueda, DataGridView DGVExcel, DataGridView DGVListadoPrinci, Label nombrearchivo, string busquedaseleccionada
            , CheckBox ckaprobado, CheckBox ckpendiente, CheckBox ckdesaprobado, CheckBox ckatendido, CheckBox cknoatendido, CheckBox ckanulado)
        {
            Requerimientos_MostrarExcel(criteriobusqueda, DGVExcel,DGVListadoPrinci);

            if (DGVListadoPrinci.DataSource == null || DGVListadoPrinci.RowCount == 0)
            {
                return;
            }
            else
            {


                try
                {
                    SLDocument sl = new SLDocument();
                    SLStyle style = new SLStyle();
                    SLStyle styleC = new SLStyle();

                    //COLUMNAS

                    if (criteriobusqueda == "ESTADO COMERCIAL" || criteriobusqueda == "ESTADO CONTABILIDAD")
                    {
                        sl.SetColumnWidth(1, 15);
                        sl.SetColumnWidth(2, 20);
                        sl.SetColumnWidth(3, 20);
                        sl.SetColumnWidth(4, 20);
                        sl.SetColumnWidth(5, 50);
                        sl.SetColumnWidth(6, 50);
                        sl.SetColumnWidth(7, 60);
                        sl.SetColumnWidth(8, 20);
                        sl.SetColumnWidth(9, 35);
                        sl.SetColumnWidth(10, 35);
                        sl.SetColumnWidth(11, 35);
                        sl.SetColumnWidth(12, 70);


                    }

                    else if (criteriobusqueda == "RESPONSABLE" || criteriobusqueda == "VEHICULO" || criteriobusqueda == "MONEDA" || criteriobusqueda == "CLIENTE" || criteriobusqueda == "SIN FILTROS")
                    {
                        sl.SetColumnWidth(1, 15);
                        sl.SetColumnWidth(2, 20);
                        sl.SetColumnWidth(3, 20);
                        sl.SetColumnWidth(4, 20);
                        sl.SetColumnWidth(5, 50);
                        sl.SetColumnWidth(6, 50);
                        sl.SetColumnWidth(7, 60);
                        sl.SetColumnWidth(8, 20);
                        sl.SetColumnWidth(9, 35);
                        sl.SetColumnWidth(10, 35);
                        sl.SetColumnWidth(11, 35);
                        sl.SetColumnWidth(12, 35);
                        sl.SetColumnWidth(13, 70);

                    }


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
                    foreach (DataGridViewColumn column in DGVExcel.Columns)
                    {
                        sl.SetCellValue(1, ic, column.HeaderText.ToString());
                        sl.SetCellStyle(1, ic, style);
                        ic++;
                    }

                    int ir = 2;

                    if (criteriobusqueda == "ESTADO COMERCIAL" || criteriobusqueda == "ESTADO CONTABILIDAD")
                    {
                        foreach (DataGridViewRow row in DGVExcel.Rows)
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
                            ir++;
                        }
                    }

                    else if (criteriobusqueda == "RESPONSABLE" || criteriobusqueda == "VEHICULO" || criteriobusqueda == "MONEDA" || criteriobusqueda == "CLIENTE" || criteriobusqueda == "SIN FILTROS")
                    {
                        foreach (DataGridViewRow row in DGVExcel.Rows)
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
                    }

                    Requerimientos_NombreArchivos_Exportado(nombrearchivo, criteriobusqueda, busquedaseleccionada, ckaprobado, ckpendiente, ckdesaprobado, ckatendido, cknoatendido, ckanulado);

                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    sl.SaveAs(desktopPath + @"\" + nombrearchivo.Text + ".xlsx");
                    MessageBox.Show("Se exportó los datos a un archivo de Microsoft Excel en la siguiente ubicación: " + desktopPath, "Validación del Sistema", MessageBoxButtons.OK);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //EVENTO QUE EXPORTARA EL LISTADOA EXCEL
        private void btnExportarExcelReque_Click(object sender, EventArgs e)
        {
            Requerimiento_ExportarExcel_XTipoBusqueda(cboCriterioBusquedaReque.Text, datalistadoExcelReque, datalistadoRequerimiento, lblNombreArchivosReque, cBobusquedaSeleccionadaReque.Text
                , ckAprobadoReque, ckPendienteReque, ckDesaprobadoReque, ckAtendidosReque, ckNoAtendidosReque, ckAnuladosReque);
        }

        ///-----------------------------------------------------
        ///
        ///METODO PARA GENERAR UN REPORTE EN PDF
        ///

        //METODO PARA ASIGNAR UN TITULO AL REPORTE DEPENDIENDO DE LA BUSQUEDA SELECCIONADA
        public void Requerimientos_Reporte_Titulo(string criteriobusqueda, Label tituloreporte, string busquedaseleccionada, CheckBox ckaprobado, CheckBox ckpendiente, CheckBox ckdesaprobado
            , CheckBox ckatendido, CheckBox cknoatendido, CheckBox ckanulado)
        {

            //DEFINICIÓN DE TITULO DEL REPORTE CON EL TIPO DE BUSQUEDA ESTADO COMERCIAL
            if (criteriobusqueda == "ESTADO COMERCIAL")
            {
                if (ckaprobado.Checked == true && ckpendiente.Checked == true && ckdesaprobado.Checked == true)
                {
                    tituloreporte.Text = "con Todos los Estados Comerciales";
                }

                else if (ckaprobado.Checked == true && ckpendiente.Checked == true)
                {
                    tituloreporte.Text = "Con los Estados Comerciales Aprobado y Pendiente";
                }

                else if (ckaprobado.Checked == true && ckdesaprobado.Checked == true)
                {
                    tituloreporte.Text = "Con los Estados Comerciales Aprobado y Desaprobado";
                }

                else if (ckpendiente.Checked == true && ckanulado.Checked == true)
                {
                    tituloreporte.Text = "Con los Estados de Comerciales Pendiente y Anulado";
                }

                else if (ckaprobado.Checked == true)
                {
                    tituloreporte.Text = "Aprobados según Estado Comercial";
                }
                else if (ckpendiente.Checked == true)
                {
                    tituloreporte.Text = "Pendientes según Estado Comercial";
                }

                else if (ckdesaprobado.Checked == true)
                {
                    tituloreporte.Text = "Desaprobados según Estado Comercial";
                }
            }

            //DEFINICIÓN DE TITULO DEL REPORTE CON EL TIPO DE BUSQUEDA ESTADO CONTABILIDAD
            if (criteriobusqueda == "ESTADO CONTABILIDAD")
            {
                if (ckatendido.Checked == true && cknoatendido.Checked == true && ckanulado.Checked == true)
                {
                    tituloreporte.Text = "Con Todos los Estados de Contabilidad";
                }

                else if (ckatendido.Checked == true && cknoatendido.Checked == true)
                {
                    tituloreporte.Text = "con los Estados Contabilidad Atendido y No Atendido";
                }

                else if (ckatendido.Checked == true && ckanulado.Checked == true)
                {
                    tituloreporte.Text = "con los Estados Contabilidad Atendido y Anulado";
                }

                else if (cknoatendido.Checked == true && ckanulado.Checked == true)
                {
                    tituloreporte.Text = "Con los Estados Contabilidad No Atendido y Anulado";
                }

                else if (ckatendido.Checked == true)
                {
                    tituloreporte.Text = "Atendidos según Estado Contabilidad";
                }
                else if (cknoatendido.Checked == true)
                {
                    tituloreporte.Text = "No Atendidos según Estado Contabilidad";
                }
                else if (ckanulado.Checked == true)
                {
                    tituloreporte.Text = "Anulados según Estado Contabilidad";
                }
            }

            //DEFINICIÓN DE TITULO DEL REPORTE CON EL TIPO DE BUSQUEDA RESPONSABLE,CLIENTE,VEHICULO,MONEDA Y SIN FILTROS
            if (criteriobusqueda == "RESPONSABLE")
            {
                tituloreporte.Text = "filtrado por Responsable";
            }
            if (criteriobusqueda == "CLIENTE")
            {
                tituloreporte.Text = "filtrado por Cliente";
            }
            if (criteriobusqueda == "VEHICULO")
            {
                tituloreporte.Text = "filtrado por el Vehiculo: " + busquedaseleccionada;
            }
            if (criteriobusqueda == "MONEDA")
            {
                tituloreporte.Text = "filtrado por la Moneda: " + busquedaseleccionada;
            }

            if (criteriobusqueda == "SIN FILTROS")
            {
                tituloreporte.Text = "Generales";
            }
        }

        public void ExportarRequerimientosPDF_XTipoBusqueda(DateTime desde, DateTime hasta, Label tituloreporte, Label nombrearchivo, string criteriobusqueda, ComboBox busquedaseleccionada, string busquedaxdescripcion,
            CheckBox ckaprobado, CheckBox ckpendiente, CheckBox ckdesaprobado, CheckBox ckatendido, CheckBox cknoatendido, CheckBox ckanulado)
        {
            Requerimientos_Reporte_Titulo(criteriobusqueda, tituloreporte, busquedaseleccionada.Text, ckaprobado, ckpendiente, ckdesaprobado, ckatendido, cknoatendido, ckanulado);
            try
            {
                ReportDocument crystalreport = new ReportDocument();

                string rutareporte = "C:\\Users\\kevin\\Desktop\\ArenasProyect3\\ArenasProyect3\\Reportes\\InformeListarRequerimientos.rpt";

                crystalreport.Load(rutareporte);

                ConnectionInfo connectioninfo = new ConnectionInfo
                {
                    ServerName = "DESKTOP-ABO4DEQ\\SQLEXPRESS",
                    DatabaseName = "BD_VENTAS_2",
                    UserID = "sa",
                    Password = "12345"
                };

                crystalreport.SetParameterValue("@fechadesde", desde);
                crystalreport.SetParameterValue("@fechahasta", hasta);
                crystalreport.SetParameterValue("@tituloreporte", tituloreporte.Text);

                //EXPORTACIÓN PARA ESTADOS DE COMERCIAL

                if (criteriobusqueda == "ESTADO COMERCIAL")
                {
                    //SI TODOS LOS CHECKS ESTAN MARCADOS
                    if (ckaprobado.Checked == true && ckpendiente.Checked == true && ckdesaprobado.Checked == true)
                    {
                        crystalreport.SetParameterValue("@aprobado", 2);
                        crystalreport.SetParameterValue("@pendiente", 1);
                        crystalreport.SetParameterValue("@desaprobado", 0);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@atendido", DBNull.Value);
                        crystalreport.SetParameterValue("@noatendido", DBNull.Value);
                        crystalreport.SetParameterValue("@anulado", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);

                    }

                    //SI DOS CHECKS ESTAN MARCADOS
                    else if (ckaprobado.Checked == true && ckpendiente.Checked == true)
                    {
                        crystalreport.SetParameterValue("@aprobado", 2);
                        crystalreport.SetParameterValue("@pendiente", 1);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@desaprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@atendido", DBNull.Value);
                        crystalreport.SetParameterValue("@noatendido", DBNull.Value);
                        crystalreport.SetParameterValue("@anulado", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);

                    }

                    else if (ckaprobado.Checked == true && ckdesaprobado.Checked == true)
                    {
                        crystalreport.SetParameterValue("@aprobado", 2);
                        crystalreport.SetParameterValue("@desaprobado", 1);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                        crystalreport.SetParameterValue("@atendido", DBNull.Value);
                        crystalreport.SetParameterValue("@noatendido", DBNull.Value);
                        crystalreport.SetParameterValue("@anulado", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);

                    }


                    else if (ckpendiente.Checked == true && ckdesaprobado.Checked == true)
                    {
                        crystalreport.SetParameterValue("@pendiente", 1);
                        crystalreport.SetParameterValue("@desaprobado", 0);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@atendido", DBNull.Value);
                        crystalreport.SetParameterValue("@noatendido", DBNull.Value);
                        crystalreport.SetParameterValue("@anulado", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);

                    }

                    //SI SOLO UN CHECK ESTA MARCADO
                    else if (ckaprobado.Checked == true)
                    {
                        crystalreport.SetParameterValue("@aprobado", 2);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                        crystalreport.SetParameterValue("@desaprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@atendido", DBNull.Value);
                        crystalreport.SetParameterValue("@noatendido", DBNull.Value);
                        crystalreport.SetParameterValue("@anulado", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);



                    }
                    else if (ckpendiente.Checked == true)
                    {
                        crystalreport.SetParameterValue("@pendiente", 1);


                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@desaprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@atendido", DBNull.Value);
                        crystalreport.SetParameterValue("@noatendido", DBNull.Value);
                        crystalreport.SetParameterValue("@anulado", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);



                    }
                    else if (ckdesaprobado.Checked == true)
                    {

                        crystalreport.SetParameterValue("@desaprobado", 0);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                        crystalreport.SetParameterValue("@atendido", DBNull.Value);
                        crystalreport.SetParameterValue("@noatendido", DBNull.Value);
                        crystalreport.SetParameterValue("@anulado", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);


                    }
                }

                //////////////////////////////////////////////////////////////////
                ///EXPORTACIÓN PARA LOS ESTADOS DE CONTABILIDAD
                ///

                if (criteriobusqueda == "ESTADO CONTABILIDAD")
                {

                    if (ckatendido.Checked == true && cknoatendido.Checked == true && ckanulado.Checked == true)
                    {
                        crystalreport.SetParameterValue("@atendido", 2);
                        crystalreport.SetParameterValue("@noatendido", 1);
                        crystalreport.SetParameterValue("@anulado", 0);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                        crystalreport.SetParameterValue("@desaprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);

                    }

                    else if (ckatendido.Checked == true && cknoatendido.Checked == true)
                    {
                        crystalreport.SetParameterValue("@atendido", 2);
                        crystalreport.SetParameterValue("@noatendido", 1);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@anulado", DBNull.Value);
                        crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                        crystalreport.SetParameterValue("@desaprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);

                    }

                    else if (ckatendido.Checked == true && ckanulado.Checked == true)
                    {
                        crystalreport.SetParameterValue("@atendido", 2);
                        crystalreport.SetParameterValue("@anulado", 0);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@noatendido", DBNull.Value);
                        crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                        crystalreport.SetParameterValue("@desaprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);

                    }

                    else if (ckatendido.Checked == true)
                    {
                        crystalreport.SetParameterValue("@atendido", 2);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@anulado", DBNull.Value);
                        crystalreport.SetParameterValue("@noatendido", DBNull.Value);
                        crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                        crystalreport.SetParameterValue("@desaprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);


                    }
                    else if (cknoatendido.Checked == true)
                    {
                        crystalreport.SetParameterValue("@noatendido", 1);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@atendido", DBNull.Value);
                        crystalreport.SetParameterValue("@anulado", DBNull.Value);
                        crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                        crystalreport.SetParameterValue("@desaprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);


                    }
                    else if (ckanulado.Checked == true)
                    {
                        crystalreport.SetParameterValue("@anulado", 0);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@noatendido", DBNull.Value);
                        crystalreport.SetParameterValue("@atendido", DBNull.Value);
                        crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                        crystalreport.SetParameterValue("@desaprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);

                    }
                }

                //////////////////////////////////////////////////////////////////
                ///EXPORTACIÓN DE LOS REQUERIMIENTOS POR MEDIO DEL NOMBRE DEL RESPONSABLE
                ///

                if (criteriobusqueda == "RESPONSABLE")
                {
                    crystalreport.SetParameterValue("@responsable", busquedaxdescripcion);

                    //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                    crystalreport.SetParameterValue("@atendido", DBNull.Value);
                    crystalreport.SetParameterValue("@noatendido", DBNull.Value);
                    crystalreport.SetParameterValue("@anulado", DBNull.Value);
                    crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                    crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                    crystalreport.SetParameterValue("@desaprobado", DBNull.Value);
                    crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                    crystalreport.SetParameterValue("@cliente", DBNull.Value);
                    crystalreport.SetParameterValue("@moneda", DBNull.Value);

                }

                //////////////////////////////////////////////////////////////////
                ///EXPORTACIÓN DE LOS REQUERIMIENTOS POR MEDIO DEL NOMBRE DEL CLIENTE
                ///

                if (criteriobusqueda == "CLIENTE")
                {
                    crystalreport.SetParameterValue("@cliente", busquedaxdescripcion);

                    //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                    crystalreport.SetParameterValue("@atendido", DBNull.Value);
                    crystalreport.SetParameterValue("@noatendido", DBNull.Value);
                    crystalreport.SetParameterValue("@anulado", DBNull.Value);
                    crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                    crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                    crystalreport.SetParameterValue("@desaprobado", DBNull.Value);
                    crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                    crystalreport.SetParameterValue("@responsable", DBNull.Value);
                    crystalreport.SetParameterValue("@moneda", DBNull.Value);

                }
                //////////////////////////////////////////////////////////////////
                ///EXPORTACIÓN DE LOS REQUERIMIENTOS PARA EL VEHICULO SELECCIONADO
                ///

                if (criteriobusqueda == "VEHICULO")
                {
                    crystalreport.SetParameterValue("@vehiculo", busquedaseleccionada.SelectedValue);

                    //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                    crystalreport.SetParameterValue("@atendido", DBNull.Value);
                    crystalreport.SetParameterValue("@noatendido", DBNull.Value);
                    crystalreport.SetParameterValue("@anulado", DBNull.Value);
                    crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                    crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                    crystalreport.SetParameterValue("@desaprobado", DBNull.Value);
                    crystalreport.SetParameterValue("@responsable", DBNull.Value);
                    crystalreport.SetParameterValue("@cliente", DBNull.Value);
                    crystalreport.SetParameterValue("@moneda", DBNull.Value);

                }

                //////////////////////////////////////////////////////////////////
                ///EXPORTACIÓN DE LOS REQUERIMIENTOS PARa LA MONEDA SELECCIONADO
                ///

                if (criteriobusqueda == "MONEDA")
                {
                    crystalreport.SetParameterValue("@moneda", busquedaseleccionada.SelectedValue);

                    //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                    crystalreport.SetParameterValue("@atendido", DBNull.Value);
                    crystalreport.SetParameterValue("@noatendido", DBNull.Value);
                    crystalreport.SetParameterValue("@anulado", DBNull.Value);
                    crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                    crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                    crystalreport.SetParameterValue("@desaprobado", DBNull.Value);
                    crystalreport.SetParameterValue("@responsable", DBNull.Value);
                    crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                    crystalreport.SetParameterValue("@cliente", DBNull.Value);

                }

                //////////////////////////////////////////////////////////////////
                ///EXPORTACIÓN DE LOS REQUERIMIENTOS SI NO ELIJE NINGUN FILTRO
                ///

                if (criteriobusqueda == "SIN FILTROS")
                {
                    crystalreport.SetParameterValue("@moneda", DBNull.Value);
                    crystalreport.SetParameterValue("@atendido", DBNull.Value);
                    crystalreport.SetParameterValue("@noatendido", DBNull.Value);
                    crystalreport.SetParameterValue("@anulado", DBNull.Value);
                    crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                    crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                    crystalreport.SetParameterValue("@desaprobado", DBNull.Value);
                    crystalreport.SetParameterValue("@responsable", DBNull.Value);
                    crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                    crystalreport.SetParameterValue("@cliente", DBNull.Value);

                }

                // Aplicar la conexión a cada tabla del reporte
                foreach (CrystalDecisions.CrystalReports.Engine.Table table in crystalreport.Database.Tables)
                {
                    TableLogOnInfo logOnInfo = table.LogOnInfo;
                    logOnInfo.ConnectionInfo = connectioninfo;
                    table.ApplyLogOnInfo(logOnInfo);
                }


                Requerimientos_NombreArchivos_Exportado(nombrearchivo, criteriobusqueda, busquedaseleccionada.Text, ckaprobado, ckpendiente, ckdesaprobado, ckatendido, cknoatendido, ckanulado);

                string rutaescritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string rutasalida = System.IO.Path.Combine(rutaescritorio, nombrearchivo.Text + ".pdf");

                crystalreport.ExportToDisk(ExportFormatType.PortableDocFormat, rutasalida);

                MessageBox.Show("Listado exportado correctamente a: {rutasalida} ", "Exportado Exitsoamente", MessageBoxButtons.OK, MessageBoxIcon.Information);



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //METODO PARA VALIDAR SI SE SELECCIONO UN ESTADO SEGUN EL TIPO DE BUSQUEDA SELECCIONADO PARA LA GENERACIÓN DEL REPORTE
        public void Requerimiento_ValidarEnvioDatos_GenerarReportePdf(DateTime Desde, DateTime Hasta, Label TituloReporte, Label nombrearchivo, string criteriobusqueda, string busquedaxdescripcion, ComboBox busquedaseleccionada, CheckBox ckaprobado, CheckBox ckpendiente, CheckBox ckdesaprobado
            , CheckBox ckatendido, CheckBox cknoatendido, CheckBox ckanulado)
        {
            if (criteriobusqueda == "SELECCIONE UNA BUSQUEDA")
            {
                MessageBox.Show("Debe seleccionar un Tipo de Búsqueda diferente para poder generar el Reporte.", "Validación del Sistema", MessageBoxButtons.OK);
                return;
            }

            if (criteriobusqueda == "ESTADO COMERCIAL")
            {
                if (ckaprobado.Checked == false && ckpendiente.Checked == false && ckdesaprobado.Checked == false)
                {
                    MessageBox.Show("Debe seleccionar el Estado Comercial que desea incluir en la generación del Reporte.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    ExportarRequerimientosPDF_XTipoBusqueda(Desde, Hasta, TituloReporte, nombrearchivo, criteriobusqueda, busquedaseleccionada, busquedaxdescripcion, ckaprobado, ckpendiente, ckdesaprobado, ckatendido, cknoatendido, ckanulado);
                }
            }

            if (criteriobusqueda == "ESTADO CONTABILIDAD")
            {
                if (ckatendido.Checked == false && cknoatendido.Checked == false && ckanulado.Checked == false)
                {
                    MessageBox.Show("Debe seleccionar el Estado Contabilidad que desea incluir en la generación del Reporte.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    ExportarRequerimientosPDF_XTipoBusqueda(Desde, Hasta, TituloReporte, nombrearchivo, criteriobusqueda, busquedaseleccionada, busquedaxdescripcion, ckaprobado, ckpendiente, ckdesaprobado, ckatendido, cknoatendido, ckanulado);
                }
            }

            if (criteriobusqueda == "RESPONSABLE")
            {
                if (busquedaxdescripcion == "")
                {
                    MessageBox.Show("Debe ingresar un nombre de responsable para poder generar la exportación del Reporte", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    ExportarRequerimientosPDF_XTipoBusqueda(Desde, Hasta, TituloReporte, nombrearchivo, criteriobusqueda, busquedaseleccionada, busquedaxdescripcion, ckaprobado, ckpendiente, ckdesaprobado, ckatendido, cknoatendido, ckanulado);
                }
            }

            if (criteriobusqueda == "CLIENTE")
            {
                if (busquedaxdescripcion == "")
                {
                    MessageBox.Show("Debe ingresar un nombre de Cliente para poder generar al exportación del Reporte", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    ExportarRequerimientosPDF_XTipoBusqueda(Desde, Hasta, TituloReporte, nombrearchivo, criteriobusqueda, busquedaseleccionada, busquedaxdescripcion, ckaprobado, ckpendiente, ckdesaprobado, ckatendido, cknoatendido, ckanulado);
                }
            }

            if (criteriobusqueda == "VEHICULO")
            {
                if (busquedaseleccionada.SelectedValue != null)
                {
                    ExportarRequerimientosPDF_XTipoBusqueda(Desde, Hasta, TituloReporte, nombrearchivo, criteriobusqueda, busquedaseleccionada, busquedaxdescripcion, ckaprobado, ckpendiente, ckdesaprobado, ckatendido, cknoatendido, ckanulado);
                }
            }

            if (criteriobusqueda == "MONEDA")
            {
                if (busquedaseleccionada.SelectedValue != null)
                {
                    ExportarRequerimientosPDF_XTipoBusqueda(Desde, Hasta, TituloReporte, nombrearchivo, criteriobusqueda, busquedaseleccionada, busquedaxdescripcion, ckaprobado, ckpendiente, ckdesaprobado, ckatendido, cknoatendido, ckanulado);
                }
            }

            if (criteriobusqueda == "SIN FILTROS")
            {
                ExportarRequerimientosPDF_XTipoBusqueda(Desde, Hasta, TituloReporte, nombrearchivo, criteriobusqueda, busquedaseleccionada, busquedaxdescripcion, ckaprobado, ckpendiente, ckdesaprobado, ckatendido, cknoatendido, ckanulado);
            }
        }

        //ACCION QUE GENERARA EL REPORTE
        private void btnExportarRequePDF_Click(object sender, EventArgs e)
        {
            Requerimiento_ValidarEnvioDatos_GenerarReportePdf(DesdeReque.Value, HastaReque.Value, lblTituloReporteReque, lblNombreArchivosReque, cboCriterioBusquedaReque.Text, txtBusquedaReque.Text, cBobusquedaSeleccionadaReque, ckAprobadoReque, ckPendienteReque, ckDesaprobadoReque, ckAtendidosReque, ckNoAtendidosReque
                , ckAnuladosReque);
        }

        ///-----------------------------------------------------
        ///
        ///METODOS PARA LA EXPORTACIÓN EN TEXTO PLANO

        public string Requerimientos_LimpiarCabecera_XML(string texto)
        {
            var limpio = new string(texto.Where(c => char.IsLetterOrDigit(c) || c == '_').ToArray());

            if (string.IsNullOrWhiteSpace(limpio))
            {
                limpio = "Columna";
            }

            if (char.IsDigit(limpio[0]))
            {
                limpio = "_" + limpio;
            }

            return limpio;
        }


        //METODO PARA LA EXPORTACIÓN EN TEXTO PLANO
        public void Requerimientos_ExportarListadoXML(string criteriobusqueda, DataGridView DGVExcel, DataGridView DGVListadoPrin, Label nombrearchivo, string busquedaseleccionada, CheckBox ckaprobado, CheckBox ckpendiente
            , CheckBox ckdesaprobado, CheckBox ckatendido, CheckBox cknoatendido, CheckBox ckanulado)
        {

            Requerimientos_MostrarExcel(criteriobusqueda, DGVExcel,DGVListadoPrin);

            if (DGVListadoPrin.DataSource != null)
            {
                //NODO DE INICIO PARA ALMACENAR LOS ELEMENTOS DE REQUERIMIENTO
                XElement inicio = new XElement("Registros");

                foreach (DataGridViewRow fila in DGVExcel.Rows)
                {
                    //SEGUNDO NODO QUE ALMACENARA LAS FILAS DEL DATAGRIDVIEW
                    XElement registros = new XElement("Requerimientos");

                    foreach (DataGridViewColumn columnas in DGVExcel.Columns)
                    {
                        //LIMPIEZA DE CABECERA DEL DATAGRIDVIEW Y VALIDACION SI TIENE NULL QUE APARESCA COMO UNA CADENA VACIA ""
                        string encabezado = Requerimientos_LimpiarCabecera_XML(columnas.HeaderText);
                        string valorcelda = fila.Cells[columnas.Index].Value?.ToString() ?? "";

                        //SE AGREGA CABECERAS Y VALORES
                        registros.Add(new XElement(encabezado, valorcelda));
                    }
                    //AGREGADO DE NODO DE REQUERIMIENTOS A EL NODO RAIZ
                    inicio.Add(registros);
                }

                //CREACION DE DOCUMENTO XML CON LA DECLARACION INICIAL Y CON EL CONTENIDO DEL NODO RAIZ
                XDocument documento = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), inicio);  //VERSION, CONDIFICACION Y STANDALONE

                Requerimientos_NombreArchivos_Exportado(nombrearchivo, criteriobusqueda, busquedaseleccionada, ckaprobado, ckpendiente, ckdesaprobado, ckatendido, cknoatendido, ckanulado);

                string rutaescritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string rutasalida = System.IO.Path.Combine(rutaescritorio, nombrearchivo.Text + ".xml");

                //SE GUARDA EL XML EN EL ESCRITORIO
                documento.Save(rutasalida);
                MessageBox.Show("Exportación en texto plano exitosamente", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //BOTON QUE REALIZARA LA EXPORTACIÓN
        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            Requerimientos_ExportarListadoXML(cboCriterioBusquedaReque.Text, datalistadoExcelReque, datalistadoRequerimiento, lblNombreArchivosReque, cBobusquedaSeleccionadaReque.Text
                , ckAprobadoReque, ckPendienteReque, ckDesaprobadoReque, ckAtendidosReque, ckNoAtendidosReque, ckAnuladosReque);
        }


        ///-----------------------------------------------------
        ///
        ///ACCIONES DE LOS CHECKBOX PARA LISTAR LOS REQUERIMIENTOS EN TIEMPO REAL
        ///

        //ACCIONES DE LOS CHECKBOS EN TIEMPO REAL PARA LA SECCIÓN DE LIQUIDACIONES
        public void MostrarRequerimientos_PorCheckbox(string criteriobusqueda)
        {
            if (cboCriterioBusquedaReque.Text == "ESTADO COMERCIAL")
            {
                MostrarRequerimientosPor_EstadoComercial(DesdeReque.Value, HastaReque.Value, cboCriterioBusquedaReque.Text, datalistadoRequerimiento, ckAprobadoReque
                    , ckPendienteReque, ckDesaprobadoReque);
            }
            else if (cboCriterioBusquedaReque.Text == "ESTADO CONTABILIDAD")
            {
                MostrarRequerimientosPor_EstadoContabilidad(DesdeReque.Value, HastaReque.Value, cboCriterioBusquedaReque.Text, datalistadoRequerimiento
                     , ckAtendidosReque, ckNoAtendidosReque, ckAnuladosReque);
            }
        }
        private void ckAprobadoReque_CheckedChanged(object sender, EventArgs e)
        {
            MostrarRequerimientos_PorCheckbox(cboCriterioBusquedaReque.Text);
        }

        private void ckPendienteReque_CheckedChanged(object sender, EventArgs e)
        {
            MostrarRequerimientos_PorCheckbox(cboCriterioBusquedaReque.Text);
        }

        private void ckDesaprobadoReque_CheckedChanged(object sender, EventArgs e)
        {
            MostrarRequerimientos_PorCheckbox(cboCriterioBusquedaReque.Text);
        }

        private void ckAtendidosReque_CheckedChanged(object sender, EventArgs e)
        {
            MostrarRequerimientos_PorCheckbox(cboCriterioBusquedaReque.Text);
        }

        private void ckNoAtendidosReque_CheckedChanged(object sender, EventArgs e)
        {
            MostrarRequerimientos_PorCheckbox(cboCriterioBusquedaReque.Text);
        }

        private void ckAnuladosReque_CheckedChanged(object sender, EventArgs e)
        {
            MostrarRequerimientos_PorCheckbox(cboCriterioBusquedaReque.Text);
        }

        //////-----------------------------------------------------
        ///SECCIÓN DE REQUERIMIENTOS CON GRAFICOS
        ///
        public void Requerimientos_MostrarGraficoBarras(DateTime Desde,DateTime Hasta)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();

                SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarRequerimientosXGrafico", con);
                cmd.Parameters.AddWithValue("@fechadesde",Desde);
                cmd.Parameters.AddWithValue("@fechahasta",Hasta);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                con.Close();


                if (dt.Rows.Count > 0)
                {
                    //SE TOMA LA PRIMERA FILA DEL DATABLE 
                    DataRow row = dt.Rows[0];

                    //LIMPIEZA DE LAS SERIES PARA EVITAR DUPLICADOS
                    foreach (var serie in new[] { "Aprobado", "Pendiente", "Desaprobado" })
                    {
                        chBarrasTotalRequeXEstado.Series[serie].Points.Clear();
                    }

                    //OBTENCION DE LOS VALORES DE FILA A TRAVES DE SUS RESPECTIVAS COLUMNAS
                    int aprobados = Convert.ToInt32(row["RequerimientosAprobados"]);
                    int pendientes = Convert.ToInt32(row["RequerimientosPendientes"]);
                    int desaprobados = Convert.ToInt32(row["RequerimientosDesaprobados"]);
                    int total = aprobados + pendientes + desaprobados;

                    //AGREGAR LOS VALORES A CADA SERIE CORRESPONDIENTE
                    chBarrasTotalRequeXEstado.Series["Aprobado"].Points.AddY(aprobados);
                    chBarrasTotalRequeXEstado.Series["Pendiente"].Points.AddY(pendientes);
                    chBarrasTotalRequeXEstado.Series["Desaprobado"].Points.AddY(desaprobados);


                    //RECORRIDO DE TODAS LAS SERIES PARA AÑADIR LAS ETIQUETAS DE CADA BARRA (VALOR + PORCENTAJE)
                    foreach (var serie in chBarrasTotalRequeXEstado.Series)
                    {
                        if (serie.Points.Count > 0)
                        {
                            //CANTIDAD REQUERIMIENTOS DEL ESTADO X TOTAL CANTIDAD DE REQUERIMIENTOS / DIVIDIDO ENTRE 100
                            int valor = (int)serie.Points[0].YValues[0];
                            double porcentaje = total > 0
                                ? (Convert.ToDouble(valor) / total) * 100
                                : 0;

                            serie.Points[0].Label = $"{valor} ({porcentaje:0}%)";

                            serie.LegendText = $"{serie.Name}: {valor} ({porcentaje:0}%)";
                        }
                    }
                
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Requerimientos_MostrarGraficoCircular(DateTime Desde,DateTime Hasta)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();

                SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarRequerimientosXGrafico", con);
                cmd.Parameters.AddWithValue("@fechadesde", Desde);
                cmd.Parameters.AddWithValue("@fechahasta", Hasta);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                con.Close();

                if(dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    
                    int aprobados = Convert.ToInt32(row["RequerimientosAprobados"]);
                    int pendientes = Convert.ToInt32(row["RequerimientosPendientes"]);
                    int desaprobados = Convert.ToInt32(row["RequerimientosDesaprobados"]);
                    int total = aprobados + pendientes + desaprobados;

                    //USO DE 1 SOLA SERIE DEL GRAFICO
                    var serie = chCircularTotalRequeXEstado.Series["Estados"];
                    serie.Points.Clear();

                    //AGREGADO DE PUNTOS PARA LA SERIE (PORCIONES DEL GRAFICO)
                    serie.Points.AddXY("Aprobados", aprobados);
                    serie.Points.AddXY("Pendientes", pendientes);
                    serie.Points.AddXY("Desaprobados", desaprobados);

                    //COLOR PARA LAS PORCIONES DEL GRAFICO
                    serie.Points[0].Color = System.Drawing.Color.SeaGreen;
                    serie.Points[1].Color = System.Drawing.Color.Goldenrod;
                    serie.Points[2].Color = System.Drawing.Color.Firebrick;

                    //MUESTRA DE ETIQUETAS CON VALOR + PORCENTAJE
                    if (total > 0)
                    {
                        foreach (var point in serie.Points)
                        {
                            //Cantidad de aprobados X Cantidad de Requerimeientos / Dividios entre 100
                            double porcentaje = (point.YValues[0] / total) * 100;

                            point.Label = $"{point.YValues[0]} ({porcentaje:0}%)";

                            point.LegendText = $"{point.AxisLabel}: {point.YValues[0]} ({porcentaje:0}%)";

                        }
                    }              
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //EVENTO PARA VISUALIZAR LOS GRAFICOS CON DATOS
        private void btnMostrarGrafico_Click(object sender, EventArgs e)
        {
            if(DesdeGrafi.Value.Date > HastaGrafi.Value.Date)
            {
                MessageBox.Show("Verifique las fechas: la fecha 'Desde' debe ser menor o igual a la fecha 'Hasta'", "Validación del Sistema", MessageBoxButtons.OK);
                return;
            }
            else
            {
                Requerimientos_MostrarGraficoBarras(DesdeGrafi.Value, HastaGrafi.Value);
                Requerimientos_MostrarGraficoCircular(DesdeGrafi.Value, HastaGrafi.Value);
            }        
        }

        //REPORTES DE LIQUIDACIONES---------------------------------------------------------------
        //HABILITAR LIQUIDACIONES

        private void btnReporteLiquidaciones_Click(object sender, EventArgs e)
        {
            panelReportesLiquidaciones.Visible = true;

            panelReportesRequerimiento.Visible = false;
            panelReportesActas.Visible = false;
        }

        //CARGA DE LOS ITEMS DE MI COMBO EN EL PANEL DE LIQUIDACIONES
        public void Liquidaciones_CargaItemsTipos_DeBusqueda(ComboBox cbo1, ComboBox cbo2,GroupBox EstadoComercial, GroupBox EstadoContabilidad)
        {
            if (cbo1.Text == "SELECCIONE UNA BUSQUEDA" || cbo1.Text == "RESPONSABLE" || cbo1.Text == "CLIENTE" || cbo1.Text == "SIN FILTROS")
            {
                EstadoContabilidad.Visible = false;
                EstadoComercial.Visible = false;

                cbo2.Enabled = false;
                cbo2.DataSource = null;
            }
            else
            {
                cbo2.Enabled = true;
            }

            if (cbo1.Text == "ESTADO COMERCIAL")
            {
                cbo2.Enabled = false;
                EstadoComercial.Visible = true;
                EstadoContabilidad.Visible = false;
            }

            if (cbo1.Text == "ESTADO CONTABILIDAD")
            {
                cbo2.Enabled = false;
                EstadoContabilidad.Visible = true;
                EstadoComercial.Visible = false;
            }

            if (cbo1.Text == "VEHICULO")
            {
                EstadoContabilidad.Visible = false;
                EstadoComercial.Visible = false;

                cbo2.DataSource = null;
                CargarVehiculos(cbo2);
                cbo2.SelectedIndex = 0;
            }

            if (cbo1.Text == "MONEDA")
            {
                EstadoContabilidad.Visible = false;
                EstadoComercial.Visible = false;

                cbo2.DataSource = null;
                CargarTiposMonedas(cbo2);
                cbo2.SelectedIndex = 0;
            }
        }

        //BLOQUEOS DE LOS CONTROLES DEPENDIENDO DEL TIPO DE BUSQUEDA SELECCIONADO
        public void Liquidaciones_LimpiarCombo_BLoquear_BusquedaSeleccionada(TextBox busquedaxdescripcion, DataGridView DGV, string criteriobusqueda, ComboBox cbo2, Button mostrartodo
            ,CheckBox ckaprobado, CheckBox ckpendiente, CheckBox ckanulado, CheckBox ckliquidado, CheckBox ckporliquidar)
        {
            DGV.DataSource = null;
            busquedaxdescripcion.Text = "";
            ckaprobado.Checked = false;
            ckpendiente.Checked = false;
            ckanulado.Checked = false;
            ckliquidado.Checked = false;
            ckporliquidar.Checked = false;

            if (criteriobusqueda == "SELECCIONE UNA BUSQUEDA")
            {
                mostrartodo.Enabled = false;
                busquedaxdescripcion.Enabled = false;
            }
            else
            {
                mostrartodo.Enabled = true;
                busquedaxdescripcion.Enabled = true;
            }

            if (criteriobusqueda == "ESTADO COMERCIAL" || criteriobusqueda == "ESTADO CONTABILIDAD" || criteriobusqueda == "VEHICULO" || criteriobusqueda == "MONEDA" || criteriobusqueda == "SIN FILTROS")
            {
                busquedaxdescripcion.Enabled = false;
            }
        }

        private void cboCriterioBusquedaLiqui_SelectedIndexChanged(object sender, EventArgs e)
        {
            Liquidaciones_LimpiarCombo_BLoquear_BusquedaSeleccionada(txtBusquedaLiqui, datalistadoliquidaciones, cboCriterioBusquedaLiqui.Text
                  , cBobusquedaSeleccionadaReque, btnMostrarTodasLiqui,ckAprobadosLiqui,ckPendienteLiqui,ckAnuladoLiqui,ckliquidadoLiqui,ckporliquidarLiqui);

            Liquidaciones_CargaItemsTipos_DeBusqueda(cboCriterioBusquedaLiqui, cboBusquedaSeleccionadaLiqui,grpEstadoComercialLiqui,grpEstadoContabilidadLiqui);
            
        }

        //CARGA DE LAS LIQUIDACIONES POR EL ESTADO COMERCIAL SELECCIONADO
        public void MostrarLiquidacionesPor_EstadoComercial(DateTime desde, DateTime hasta, string criteriobusqueda, DataGridView DGV
            , CheckBox ckaprobado, CheckBox ckpendiente, CheckBox ckanulado)
        {
            try
            {
                if (criteriobusqueda == "ESTADO COMERCIAL")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();

                    if (ckaprobado.Checked == true)
                    {
                        SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarLiquidacionesXEstadosComercial", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fechadesde", desde);
                        cmd.Parameters.AddWithValue("@fechahasta", hasta);
                        cmd.Parameters.AddWithValue("@estadocomercial", 2);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    if (ckpendiente.Checked == true)
                    {
                        SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarLiquidacionesXEstadosComercial", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fechadesde", desde);
                        cmd.Parameters.AddWithValue("@fechahasta", hasta);
                        cmd.Parameters.AddWithValue("@estadocomercial", 1);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    if (ckanulado.Checked == true)
                    {
                        SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarLiquidacionesXEstadosComercial", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fechadesde", desde);
                        cmd.Parameters.AddWithValue("@fechahasta", hasta);
                        cmd.Parameters.AddWithValue("@estadocomercial", 0);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }

                    DGV.DataSource = dt;
                    con.Close();
                    ColoresListado(criteriobusqueda, DGV);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGA DE LAS LIQUIDACIONES POR EL ESTADO DE CONTABILIDAD SELECCIONADO
        public void MostrarLiquidacionesPor_EstadosContabilidad(DateTime desde, DateTime hasta, string criteriobusqueda, DataGridView DGV
            , CheckBox ckliquidado, CheckBox ckporliquidar)
        {
            try
            {

                if (criteriobusqueda == "ESTADO CONTABILIDAD")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();


                    if (ckliquidado.Checked == true)
                    {
                        SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarLiquidacionesXEstadosContabilidad", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fechadesde", desde);
                        cmd.Parameters.AddWithValue("@fechahasta", hasta);
                        cmd.Parameters.AddWithValue("@estadocontabilidad", 1);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                    }
                    if (ckporliquidar.Checked == true)
                    {
                        SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarLiquidacionesXEstadosContabilidad", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fechadesde", desde);
                        cmd.Parameters.AddWithValue("@fechahasta", hasta);
                        cmd.Parameters.AddWithValue("@estadocontabilidad", 0);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    DGV.DataSource = dt;
                    con.Close();
                    ColoresListado(criteriobusqueda, DGV);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //CARGA DE LAS LIQUIDACIONES X MEDIO DE EL RESPONSABLE O EL CLIENTE
        public void MostrarLiquidacionesPor_Descripcion(DateTime desde, DateTime hasta, string descripcion, string criteriobusqueda, DataGridView DGV)
        {
            try
            {
                if (criteriobusqueda == "RESPONSABLE")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarLiquidaciones_BusquedaXResponsable", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechadesde", desde);
                    cmd.Parameters.AddWithValue("@fechahasta", hasta);
                    cmd.Parameters.AddWithValue("@responsable", descripcion);

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                }
                else if (criteriobusqueda == "CLIENTE")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarLiquidaciones_BusquedaXCliente", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechadesde", desde);
                    cmd.Parameters.AddWithValue("@fechahasta", hasta);
                    cmd.Parameters.AddWithValue("@cliente", descripcion);

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                }

                ColoresListado(criteriobusqueda, DGV);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGA DE LAS LIQUIDACIONES POR EL VEHICULO SELECCIONADO
        public void MostrarLiquidacionesPor_Vehiculo(DateTime desde, DateTime hasta, int vehiculoseleccionado, string criteriobusqueda, DataGridView DGV)
        {
            try
            {
                if (criteriobusqueda == "VEHICULO")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarLiquidaciones_BusquedaXVehiculo", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechadesde", desde);
                    cmd.Parameters.AddWithValue("@fechahasta", hasta);
                    cmd.Parameters.AddWithValue("@vehiculo", vehiculoseleccionado);

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                }
                ColoresListado(criteriobusqueda, DGV);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGA DE LAS LIQUIDACIONES POR EL TIPO DE MONEDA SELECCIONADO
        public void MostrarLiquidacionesPor_TipoMoneda(DateTime desde, DateTime hasta, int tipomoneda, string criteriobusqueda, DataGridView DGV)
        {
            try
            {
                if (criteriobusqueda == "MONEDA")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarLiquidaciones_BusquedaXTipoMoneda", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechadesde", desde);
                    cmd.Parameters.AddWithValue("@fechahasta", hasta);
                    cmd.Parameters.AddWithValue("@moneda", tipomoneda);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                }
                ColoresListado(criteriobusqueda, DGV);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGA DE TODAS LAS LIQUIDACIONES SIN SELECCIONAR NINGUN FILTRO
        public void MostrarLiquidaciones_PorFecha(DateTime desde, DateTime hasta, DataGridView DGV, string criteriobusqueda)
        {
            try
            {
                if (criteriobusqueda == "SIN FILTROS")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarLiquidaciones", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechadesde", desde);
                    cmd.Parameters.AddWithValue("@fechahasta", hasta);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                    ColoresListado(criteriobusqueda, DGV);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //METODO PARA VALIDAR QUE LOS CHECKS ESTEN ACTIVOS
        public void Liquidaciones_ValidarChecksActivos(DateTime Desde,DateTime Hasta,DataGridView DGV,string criteriobusqueda, CheckBox ckaprobado, CheckBox ckpendiente, CheckBox ckanulado
            , CheckBox ckliquidado, CheckBox ckporliquidar)
        {
            if (criteriobusqueda == "ESTADO COMERCIAL")
            {
                if (ckaprobado.Checked == false && ckpendiente.Checked == false && ckanulado.Checked == false)
                {
                    MessageBox.Show("Marque el tipo de busqueda que desee visualizar.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    MostrarLiquidacionesPor_EstadoComercial(Desde, Hasta, criteriobusqueda, DGV,ckaprobado,ckpendiente, ckanulado);
                }
            }
            if (criteriobusqueda == "ESTADO CONTABILIDAD")
            {
                if (ckliquidado.Checked == false && ckporliquidar.Checked == false)
                {
                    MessageBox.Show("Marque el tipo de busqueda que desee visualizar.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    MostrarLiquidacionesPor_EstadosContabilidad(Desde, Hasta, criteriobusqueda, DGV,ckliquidado,ckporliquidar);
                }
            }
        }

        //COLOREAR REGISTROS
        public void ColoresListado(string criteriobusqueda, DataGridView DGV)
        {
            try
            {
                //COLOR PARA LOS ESTADOS DE COMERCIAL Y CONTABILIDAD
                if (criteriobusqueda == "ESTADO COMERCIAL" || criteriobusqueda == "ESTADO CONTABILIDAD")
                {
                    for (var i = 0; i <= DGV.RowCount - 1; i++)
                    {
                        if (DGV.Rows[i].Cells[14].Value.ToString() == "APROBADO" || DGV.Rows[i].Cells[14].Value.ToString() == "LIQUIDADO")
                        {
                            //APROBADP
                            DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                        }
                        else if (DGV.Rows[i].Cells[14].Value.ToString() == "PENDIENTE" || DGV.Rows[i].Cells[14].Value.ToString() == "POR LIQUIDAR")
                        {
                            //PENDIENTE
                            DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                        }
                        else
                        {
                            //DESAPROBADO
                            DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }

                //COLOR PARA LOS REGISTROS DE RESPONSABLE, CLIENTE, VEHICULO Y MONEDA
                if (criteriobusqueda == "CLIENTE" || criteriobusqueda == "RESPONSABLE" || criteriobusqueda == "VEHICULO" || criteriobusqueda == "MONEDA" || criteriobusqueda == "SIN FILTROS")
                {

                    for (var i = 0; i <= DGV.RowCount - 1; i++)
                    {
                        if (DGV.Rows[i].Cells[14].Value.ToString() == "APROBADO" || DGV.Rows[i].Cells[15].Value.ToString() == "LIQUIDADO")
                        {
                            //APROBADP
                            DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                        }
                        else if (DGV.Rows[i].Cells[14].Value.ToString() == "PENDIENTE" || DGV.Rows[i].Cells[15].Value.ToString() == "POR LIQUIDAR")
                        {
                            //PENDIENTE
                            DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                        }
                        else
                        {
                            //DESAPROBADO
                            DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }


        private void btnMostrarTodasLiqui_Click(object sender, EventArgs e)
        {
            Liquidaciones_ValidarChecksActivos(DesdeLiqui.Value,HastaLiqui.Value,datalistadoliquidaciones,cboCriterioBusquedaLiqui.Text, ckAprobadosLiqui, ckPendienteLiqui, ckAnuladoLiqui, ckliquidadoLiqui, ckporliquidarLiqui);

            //METODOS DE CARGA PARA EL LISTADO
            MostrarLiquidacionesPor_Descripcion(DesdeLiqui.Value, HastaLiqui.Value, txtBusquedaLiqui.Text, cboCriterioBusquedaLiqui.Text, datalistadoliquidaciones);
            MostrarLiquidacionesPor_TipoMoneda(DesdeLiqui.Value, HastaLiqui.Value, Convert.ToInt32(cboBusquedaSeleccionadaLiqui.SelectedValue), cboCriterioBusquedaLiqui.Text, datalistadoliquidaciones);
            MostrarLiquidacionesPor_Vehiculo(DesdeLiqui.Value, HastaLiqui.Value, Convert.ToInt32(cboBusquedaSeleccionadaLiqui.SelectedValue), cboCriterioBusquedaLiqui.Text, datalistadoliquidaciones);
            MostrarLiquidaciones_PorFecha(DesdeLiqui.Value, HastaLiqui.Value, datalistadoliquidaciones, cboCriterioBusquedaLiqui.Text);
        }

        private void cboBusquedaSeleccionadaLiqui_SelectedIndexChanged(object sender, EventArgs e)
        {
            datalistadoliquidaciones.DataSource = null;
        }

        private void txtBusquedaLiqui_TextChanged(object sender, EventArgs e)
        {
            MostrarLiquidacionesPor_Descripcion(DesdeLiqui.Value, HastaLiqui.Value, txtBusquedaLiqui.Text, cboCriterioBusquedaLiqui.Text, datalistadoliquidaciones);
        }

        public void Liquidaciones_NombreArchivos_Exportados(string criteriobusqueda, string busquedaseleccionada, Label nombrearchivo, CheckBox ckaprobado, CheckBox ckpendiente, CheckBox ckanulado, CheckBox ckliquidado
            , CheckBox ckporliquidar)
        {
            //DEFINICIÓN PARA EL NOMBRE DE ARCHIVO CON EL TIPO DE BUSQUEDA ESTADO COMERCIAL
            if (criteriobusqueda == "ESTADO COMERCIAL")
            {
                if (ckaprobado.Checked == true && ckpendiente.Checked == true && ckanulado.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Liquidaciones_Todos_EstadosComerciales";
                }

                else if (ckaprobado.Checked == true && ckpendiente.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Liquidaciones_Aprobado_Pendiente";
                }

                else if (ckaprobado.Checked == true && ckanulado.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Liquidaciones_Aprobado_Anulado";
                }

                else if (ckpendiente.Checked == true && ckanulado.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Liquidaciones_Pendiente_Anulado";
                }

                else if (ckaprobado.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Liquidaciones_Aprobados";
                }
                else if (ckpendiente.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Liquidaciones_Pendientes";
                }
                else if (ckanulado.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Liquidaciones_Anulados";
                }

            }

            //DEFINICIÓN PARA EL NOMBRE DE ARCHIVO CON EL TIPO DE BUSQUEDA ESTADO CONTABILIDAD
            if (criteriobusqueda == "ESTADO CONTABILIDAD")
            {
                if (ckliquidado.Checked == true && ckporliquidar.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Liquidaciones_Todos_EstadosContabilidad";
                }

                else if (ckliquidado.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Liquidaciones_Liquidadas";
                }
                else if (ckporliquidar.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Liquidaciones_PorLiquidar";
                }

            }

            //DEFINICIÓN PARA EL NOMBRE DE ARCHIVO CON EL TIPO DE BUSQUEDA RESPONSABLE,CLIENTE,VEHICULO Y MONEDA,SIN FILTROS
            if (criteriobusqueda == "RESPONSABLE")
            {
                nombrearchivo.Text = "Reporte_Liquidaciones_Responsable";
            }

            if (criteriobusqueda == "CLIENTE")
            {
                nombrearchivo.Text = "Reporte_Liquidaciones_Cliente";
            }

            if (criteriobusqueda == "VEHICULO")
            {
                nombrearchivo.Text = "Reporte_Liquidaciones_Vehiculo_" + busquedaseleccionada;
            }

            if (criteriobusqueda == "MONEDA")
            {
                nombrearchivo.Text = "Reporte_Liquidaciones_Moneda_" + busquedaseleccionada;
            }

            if (criteriobusqueda == "SIN FILTROS")
            {
                nombrearchivo.Text = "Reporte_Liquidaciones_Generales";
            }
        }


        ///-----------------------------------------------------
        ///
        ///METODO Y EVENTO PARA EXPORTAR EL LISTADO A UN EXCEL
        ///    

        //METODO PARA AGREGAR LAS COLUMNAS Y FILAS AL DATAGRIDVIEW QUE SE EXPORTARA A EXCEL
        public void Liquidaciones_MostrarExcel(string criteriobusqueda, DataGridView DGV, DataGridView DGV2)
        {
            if (DGV2.DataSource == null || DGV2.Rows.Count == 0)
            {
                MessageBox.Show("No se puede exportar un listado vacio.", "Validación del Sistema", MessageBoxButtons.OK);
                return;
            }

            else
            {
                DGV.Columns.Clear();

                DGV.Columns.Add("colNroLiqui", "N° LIQUI");
                DGV.Columns.Add("colNroReque", "N° REQUE");

                DGV.Columns.Add("colFechaGen", "FECHA DE GENERACIÓN");
                DGV.Columns.Add("colFechaIni", "FECHA DE INICIO");
                DGV.Columns.Add("colFechaTerm", "FECHA DE TÉRMINO");

                DGV.Columns.Add("colCliente", "CLIENTE");
                DGV.Columns.Add("colResponsable", "RESPONSABLE");
                DGV.Columns.Add("colMotivoVisi", "MOTIVO DE VISITA");
                DGV.Columns.Add("colMotivoVisi", "ITINERARIO");
                DGV.Columns.Add("colTipoMone", "MONEDA");
                DGV.Columns.Add("colVehiculo", "VEHICULOS");
                DGV.Columns.Add("colTotal", "TOTAL");
                DGV.Columns.Add("colAdelan", "ADELANTO");
                DGV.Columns.Add("colSal", "SALDO");

                //COLUMNAS DE ESTADO SOLAMENTE AGREGADAS DEPENDIENDO DEL TIPO DE BUSQUEDA QUE SE SELECCIONO
                if (criteriobusqueda == "ESTADO COMERCIAL")
                {
                    DGV.Columns.Add("colEstadoComerci", "ESTADO DE COMERCIAL");

                }
                else if (criteriobusqueda == "ESTADO CONTABILIDAD")
                {
                    DGV.Columns.Add("colEstadoConta", "ESTADO DE CONTABILIDAD");
                }
                else
                {
                    DGV.Columns.Add("colEstadoComerci", "ESTADO DE COMERCIAL");
                    DGV.Columns.Add("colEstadoConta", "ESTADO DE CONTABILIDAD");
                }

                //CAPTURA DE LAS COLUMNAS QUE SE VAN A EXPORTAR DEPENDIENDO DEL TIPO DE BUSQUEDA
                Dictionary<string, int[]> columnastipobusqueda = new Dictionary<string, int[]>
                {
                    {"ESTADO COMERCIAL" ,new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,13,14}},
                    {"ESTADO CONTABILIDAD", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,13,14 } },
                    {"RESPONSABLE", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13,14,15 } },
                    {"CLIENTE", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13,14,15 } },
                    {"VEHICULO", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13,14,15 } },
                    {"MONEDA", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13,14,15 } },
                    {"SIN FILTROS", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 ,14, 15 } }
                };

                int[] columnas = columnastipobusqueda[criteriobusqueda];

                foreach (DataGridViewRow dgv in DGV2.Rows)
                {
                    List<string> fila = new List<string>();

                    foreach (int i in columnas)
                    {
                        string valor = dgv.Cells[i].Value?.ToString() ?? "";

                        fila.Add(valor);
                    }
                    DGV.Rows.Add(fila.ToArray());
                }
            }
        }

        public void Liquidaciones_ExportarExcel_XTipoBusqueda(string criteriobusqueda, Label nombrearchivo, DataGridView DGVExcel, DataGridView DGVListadoPrinci, string busquedaseleccionada
            , CheckBox ckaprobado, CheckBox ckpendiente, CheckBox ckanulado, CheckBox ckliquidado, CheckBox ckporliquidar)
        {
            Liquidaciones_MostrarExcel(criteriobusqueda, DGVExcel, DGVListadoPrinci);

            if (DGVListadoPrinci.DataSource == null || DGVListadoPrinci.RowCount == 0)
            {
                return;
            }
            else
            {
                try
                {
                    SLDocument sl = new SLDocument();
                    SLStyle style = new SLStyle();
                    SLStyle styleC = new SLStyle();

                    //COLUMNAS

                    if (criteriobusqueda == "ESTADO COMERCIAL" || criteriobusqueda == "ESTADO CONTABILIDAD")
                    {
                        sl.SetColumnWidth(1, 15);
                        sl.SetColumnWidth(2, 15);
                        sl.SetColumnWidth(3, 20);
                        sl.SetColumnWidth(4, 20);
                        sl.SetColumnWidth(5, 20);
                        sl.SetColumnWidth(6, 50);
                        sl.SetColumnWidth(7, 60);
                        sl.SetColumnWidth(8, 100);
                        sl.SetColumnWidth(9, 100);
                        sl.SetColumnWidth(10, 20);
                        sl.SetColumnWidth(11, 20);
                        sl.SetColumnWidth(12, 20);
                        sl.SetColumnWidth(13, 30);
                        sl.SetColumnWidth(14, 30);
                        sl.SetColumnWidth(15, 30);

                    }

                    else if (criteriobusqueda == "RESPONSABLE" || criteriobusqueda == "VEHICULO" || criteriobusqueda == "MONEDA" || criteriobusqueda == "CLIENTE" || criteriobusqueda == "SIN FILTROS")
                    {
                        sl.SetColumnWidth(1, 15);
                        sl.SetColumnWidth(2, 15);
                        sl.SetColumnWidth(3, 19);
                        sl.SetColumnWidth(4, 19);
                        sl.SetColumnWidth(5, 19);
                        sl.SetColumnWidth(6, 50);
                        sl.SetColumnWidth(7, 60);
                        sl.SetColumnWidth(8, 100);
                        sl.SetColumnWidth(9, 100);
                        sl.SetColumnWidth(10, 20);
                        sl.SetColumnWidth(11, 20);
                        sl.SetColumnWidth(12, 20);
                        sl.SetColumnWidth(13, 20);
                        sl.SetColumnWidth(14, 30);
                        sl.SetColumnWidth(15, 30);
                        sl.SetColumnWidth(16, 30);

                    }


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
                    foreach (DataGridViewColumn column in DGVExcel.Columns)
                    {
                        sl.SetCellValue(1, ic, column.HeaderText.ToString());
                        sl.SetCellStyle(1, ic, style);
                        ic++;
                    }

                    int ir = 2;

                    if (criteriobusqueda == "ESTADO COMERCIAL" || criteriobusqueda == "ESTADO CONTABILIDAD")
                    {
                        foreach (DataGridViewRow row in DGVExcel.Rows)
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

                            ir++;
                        }
                    }

                    else if (criteriobusqueda == "RESPONSABLE" || criteriobusqueda == "VEHICULO" || criteriobusqueda == "MONEDA" || criteriobusqueda == "CLIENTE" || criteriobusqueda == "SIN FILTROS")
                    {
                        foreach (DataGridViewRow row in DGVExcel.Rows)
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

                            ir++;
                        }
                    }

                    Liquidaciones_NombreArchivos_Exportados(criteriobusqueda, busquedaseleccionada, nombrearchivo, ckaprobado, ckpendiente, ckanulado, ckliquidado, ckporliquidar);

                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    sl.SaveAs(desktopPath + @"\" + nombrearchivo.Text + ".xlsx");
                    MessageBox.Show("Se exportó los datos a un archivo de Microsoft Excel en la siguiente ubicación: " + desktopPath, "Validación del Sistema", MessageBoxButtons.OK);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        //EVENTO QUE REALIZARA LA EXPORTACIÓN
        private void btnExportarExcelLiqui_Click(object sender, EventArgs e)
        {
            Liquidaciones_ExportarExcel_XTipoBusqueda(cboCriterioBusquedaLiqui.Text, lblNombreArchivosLiqui, datalistadoExcelLiqui, datalistadoliquidaciones, cboBusquedaSeleccionadaLiqui.Text
                , ckAprobadosLiqui, ckPendienteLiqui, ckAnuladoLiqui, ckliquidadoLiqui, ckporliquidarLiqui);
        }

       
        ///-----------------------------------------------------
        ///
        ///METODO PARA EXPORTAR EN PDF
        ///

        //METODO PARA ASIGNAR UN TITULO AL REPORTE DEPENDIENDO DE LA BUSQUEDA SELECCIONADA
        public void Liquidaciones_Reporte_Titulo(string criteriobusqueda, Label tituloreporte, string busquedaseleccionada, CheckBox ckaprobado, CheckBox ckpendiente
            , CheckBox ckanulado, CheckBox ckliquidado, CheckBox ckporliquidar)
        {
            //DEFINICIÓN DEL TITULO PARA EL REPORTE SEGUN EL TIPO DE BUSQUEDA ESTADO COMERCIAL
            if (criteriobusqueda == "ESTADO COMERCIAL")
            {
                if (ckaprobado.Checked == true && ckpendiente.Checked == true && ckanulado.Checked == true)
                {
                    tituloreporte.Text = "con Todos los Estados de Comercial";
                }

                else if (ckaprobado.Checked == true && ckpendiente.Checked == true)
                {
                    tituloreporte.Text = "con los Estados Comercial Aprobado y Pendiente";
                }
                else if (ckaprobado.Checked == true && ckanulado.Checked == true)
                {
                    tituloreporte.Text = "con los Estados Comercial Aprobado y Anulado";
                }
                else if (ckpendiente.Checked == true && ckanulado.Checked == true)
                {
                    tituloreporte.Text = "con los Estados Comercial Pendiente y Anulado";
                }

                else if (ckaprobado.Checked == true)
                {
                    tituloreporte.Text = "Aprobados según Estado Comercial";
                }

                else if (ckpendiente.Checked == true)
                {
                    tituloreporte.Text = "Pendientes según Estado Comercial";
                }

                else if (ckanulado.Checked == true)
                {
                    tituloreporte.Text = "Anulados según Estado Comercial";
                }
            }

            //DEFINICIÓN DEL TITULO PARA EL REPORTE SEGUN EL TIPO DE BUSQUEDA ESTADO CONTABILIDAD
            if (criteriobusqueda == "ESTADO CONTABILIDAD")
            {
                if (ckliquidado.Checked == true && ckporliquidar.Checked == true)
                {
                    tituloreporte.Text = "con Todos los Estados de Contabilidad";
                }

                else if (ckliquidado.Checked == true)
                {
                    tituloreporte.Text = "Liquidados según Estado Contabilidad";
                }

                else if (ckporliquidar.Checked == true)
                {
                    tituloreporte.Text = "Pendientes por Liquidar según Estado de Contabilidad";
                }
            }

            //DEFINICIÓN DEL TITULO PARA EL REPORTE SEGUN EL TIPO DE BUSQUEDA RESPONSABLE,CLIENTE,VEHICULO,MONEDA,SIN FILTROS
            if (criteriobusqueda == "RESPONSABLE")
            {
                tituloreporte.Text = "filtrado por Responsable";
            }
            if (criteriobusqueda == "CLIENTE")
            {
                tituloreporte.Text = "filtrado por Cliente";
            }
            if (criteriobusqueda == "VEHICULO")
            {
                tituloreporte.Text = "filtrado por Vehículo: " + busquedaseleccionada;
            }
            if (criteriobusqueda == "MONEDA")
            {
                tituloreporte.Text = "filtrado por Moneda: " + busquedaseleccionada;
            }
            if (criteriobusqueda == "SIN FILTROS")
            {
                tituloreporte.Text = "Generales";
            }
        }


        public void Liquidaciones_ExportarPDF_XTipoBusqueda(DateTime desde, DateTime hasta, Label tituloreporte,Label nombrearchivo, string criteriobusqueda, ComboBox busquedaseleccionada
            , string busquedaxdescripcion, CheckBox ckaprobado, CheckBox ckpendiente, CheckBox ckanulado, CheckBox ckliquidado, CheckBox ckporliquidar)
        {
            Liquidaciones_Reporte_Titulo(criteriobusqueda, tituloreporte, busquedaseleccionada.Text, ckaprobado, ckpendiente, ckanulado, ckliquidado, ckporliquidar);

            try
            {
                ReportDocument crystalreport = new ReportDocument();

                string rutareporte = "C:\\Users\\kevin\\Desktop\\ArenasProyect3\\ArenasProyect3\\Reportes\\InformeListarLiquidaciones.rpt";
                crystalreport.Load(rutareporte);

                ConnectionInfo connectioninfo = new ConnectionInfo
                {
                    ServerName = "DESKTOP-ABO4DEQ\\SQLEXPRESS",
                    DatabaseName = "BD_VENTAS_2",
                    UserID = "sa",
                    Password = "12345"
                };

                crystalreport.SetParameterValue("@fechadesde", desde);
                crystalreport.SetParameterValue("@fechahasta", hasta);
                crystalreport.SetParameterValue("@tituloreporte", tituloreporte.Text);

                //EXPORTACIÓN PARA ESTADOS DE COMERCIAL

                //SI TODOS LOS CHECKS ESTAN MARCADOS
                if (criteriobusqueda == "ESTADO COMERCIAL")
                {
                    if (ckaprobado.Checked == true && ckpendiente.Checked == true && ckanulado.Checked == true)
                    {
                        crystalreport.SetParameterValue("@aprobado", 2);
                        crystalreport.SetParameterValue("@pendiente", 1);
                        crystalreport.SetParameterValue("@anulado", 0);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@liquidado", DBNull.Value);
                        crystalreport.SetParameterValue("@porliquidar", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);
                    }

                    //SI DOS CHECKS ESTAN MARCADOS
                    else if (ckaprobado.Checked == true && ckpendiente.Checked == true)
                    {
                        crystalreport.SetParameterValue("@aprobado", 2);
                        crystalreport.SetParameterValue("@pendiente", 1);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@anulado", DBNull.Value);
                        crystalreport.SetParameterValue("@liquidado", DBNull.Value);
                        crystalreport.SetParameterValue("@porliquidar", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);
                    }

                    else if (ckaprobado.Checked == true && ckanulado.Checked == true)
                    {
                        crystalreport.SetParameterValue("@aprobado", 2);
                        crystalreport.SetParameterValue("@anulado", 0);


                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                        crystalreport.SetParameterValue("@liquidado", DBNull.Value);
                        crystalreport.SetParameterValue("@porliquidar", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);
                    }

                    else if (ckpendiente.Checked == true && ckanulado.Checked == true)
                    {
                        crystalreport.SetParameterValue("@pendiente", 1);
                        crystalreport.SetParameterValue("@anulado", 0);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@liquidado", DBNull.Value);
                        crystalreport.SetParameterValue("@porliquidar", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);
                    }

                    //SI UN CHECK ESTA MARCADO
                    else if (ckaprobado.Checked == true)
                    {
                        crystalreport.SetParameterValue("@aprobado", 2);


                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                        crystalreport.SetParameterValue("@anulado", DBNull.Value);
                        crystalreport.SetParameterValue("@liquidado", DBNull.Value);
                        crystalreport.SetParameterValue("@porliquidar", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);
                    }
                    else if (ckpendiente.Checked == true)
                    {
                        crystalreport.SetParameterValue("@pendiente", 1);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@anulado", DBNull.Value);
                        crystalreport.SetParameterValue("@liquidado", DBNull.Value);
                        crystalreport.SetParameterValue("@porliquidar", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);
                    }
                    else if (ckanulado.Checked == true)
                    {
                        crystalreport.SetParameterValue("@anulado", 0);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                        crystalreport.SetParameterValue("@liquidado", DBNull.Value);
                        crystalreport.SetParameterValue("@porliquidar", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);
                    }
                }

                //////////////////////////////////////////////////////////////////
                ///EXPORTACIÓN PARA LOS ESTADOS DE CONTABILIDAD
                ///

                if (criteriobusqueda == "ESTADO CONTABILIDAD")
                {
                    if (ckliquidado.Checked == true && ckporliquidar.Checked == true)
                    {
                        crystalreport.SetParameterValue("@liquidado", 1);
                        crystalreport.SetParameterValue("@porliquidar", 0);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@anulado", DBNull.Value);
                        crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);
                    }


                    else if (ckliquidado.Checked == true)
                    {
                        crystalreport.SetParameterValue("@liquidado", 1);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@porliquidar", DBNull.Value);
                        crystalreport.SetParameterValue("@anulado", DBNull.Value);
                        crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);
                    }
                    else if (ckporliquidar.Checked == true)
                    {
                        crystalreport.SetParameterValue("@porliquidar", 0);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@liquidado", DBNull.Value);
                        crystalreport.SetParameterValue("@anulado", DBNull.Value);
                        crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@moneda", DBNull.Value);
                    }
                }

                //////////////////////////////////////////////////////////////////
                ///EXPORTACIÓN DE LOS LIQUIDACIONES POR MEDIO DEL NOMBRE DEL RESPONSABLE
                ///

                if (criteriobusqueda == "RESPONSABLE")
                {
                    crystalreport.SetParameterValue("@responsable", busquedaxdescripcion);

                    //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                    crystalreport.SetParameterValue("@porliquidar", DBNull.Value);
                    crystalreport.SetParameterValue("@liquidado", DBNull.Value);
                    crystalreport.SetParameterValue("@anulado", DBNull.Value);
                    crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                    crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                    crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                    crystalreport.SetParameterValue("@cliente", DBNull.Value);
                    crystalreport.SetParameterValue("@moneda", DBNull.Value);
                }

                //////////////////////////////////////////////////////////////////
                ///EXPORTACIÓN DE LOS LIQUIDACIONES POR MEDIO DEL NOMBRE DEL CLIENTE
                ///

                if (criteriobusqueda == "CLIENTE")
                {
                    crystalreport.SetParameterValue("@cliente", busquedaxdescripcion);

                    //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                    crystalreport.SetParameterValue("@porliquidar", DBNull.Value);
                    crystalreport.SetParameterValue("@liquidado", DBNull.Value);
                    crystalreport.SetParameterValue("@anulado", DBNull.Value);
                    crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                    crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                    crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                    crystalreport.SetParameterValue("@moneda", DBNull.Value);
                    crystalreport.SetParameterValue("@responsable", DBNull.Value);
                }
                //////////////////////////////////////////////////////////////////
                ///EXPORTACIÓN DE LAS LIQUIDACIONES PARA EL VEHICULO SELECCIONADO
                ///

                if (criteriobusqueda == "VEHICULO")
                {
                    crystalreport.SetParameterValue("@vehiculo", busquedaseleccionada.SelectedValue);

                    //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                    crystalreport.SetParameterValue("@porliquidar", DBNull.Value);
                    crystalreport.SetParameterValue("@liquidado", DBNull.Value);
                    crystalreport.SetParameterValue("@anulado", DBNull.Value);
                    crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                    crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                    crystalreport.SetParameterValue("@cliente", DBNull.Value);
                    crystalreport.SetParameterValue("@moneda", DBNull.Value);
                    crystalreport.SetParameterValue("@responsable", DBNull.Value);
                }

                //////////////////////////////////////////////////////////////////
                ///EXPORTACIÓN DE LOS LIQUIDACIONES PARa LA MONEDA SELECCIONADO
                ///

                if (criteriobusqueda == "MONEDA")
                {
                    if (busquedaseleccionada.SelectedValue != null)
                    {
                        crystalreport.SetParameterValue("@moneda", busquedaseleccionada.SelectedValue);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@porliquidar", DBNull.Value);
                        crystalreport.SetParameterValue("@liquidado", DBNull.Value);
                        crystalreport.SetParameterValue("@anulado", DBNull.Value);
                        crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                        crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                    }
                }

                if (criteriobusqueda == "SIN FILTROS")
                {
                    crystalreport.SetParameterValue("@porliquidar", DBNull.Value);
                    crystalreport.SetParameterValue("@liquidado", DBNull.Value);
                    crystalreport.SetParameterValue("@anulado", DBNull.Value);
                    crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                    crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                    crystalreport.SetParameterValue("@vehiculo", DBNull.Value);
                    crystalreport.SetParameterValue("@cliente", DBNull.Value);
                    crystalreport.SetParameterValue("@moneda", DBNull.Value);
                    crystalreport.SetParameterValue("@responsable", DBNull.Value);
                }

                // Aplicar la conexión a cada tabla del reporte
                foreach (CrystalDecisions.CrystalReports.Engine.Table table in crystalreport.Database.Tables)
                {
                    TableLogOnInfo logOnInfo = table.LogOnInfo;
                    logOnInfo.ConnectionInfo = connectioninfo;
                    table.ApplyLogOnInfo(logOnInfo);
                }

                Liquidaciones_NombreArchivos_Exportados(criteriobusqueda, busquedaseleccionada.Text, nombrearchivo, ckaprobado, ckpendiente, ckanulado, ckliquidado, ckporliquidar);


                string rutaescritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string rutasalida = System.IO.Path.Combine(rutaescritorio, lblNombreArchivosLiqui.Text + ".pdf");

                crystalreport.ExportToDisk(ExportFormatType.PortableDocFormat, rutasalida);

                MessageBox.Show($"Listado exportado correctamente a: {rutasalida} ", "Exportado Exitsoamente", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //METODO PARA VALIDAR SI SE SELECCIONO UN ESTADO SEGUN EL TIPO DE BUSQUEDA SELECCIONADO PARA LA GENERACIÓN DEL REPORTE
        public void Liquidaciones_ValidarEnvioDatos_GenerarReportePdf(DateTime Desde, DateTime Hasta, Label Tituloreporte,Label nombrearchivo, string criteriobusqueda, string busquedaxdescripcion, ComboBox busquedaseleccionada
            , CheckBox ckaprobado, CheckBox ckpendiente, CheckBox ckanulado, CheckBox ckliquidado, CheckBox ckporliquidar)
        {
            if (criteriobusqueda == "SELECCIONE UNA BUSQUEDA")
            {
                MessageBox.Show("Debe seleccionar un Tipo de Búsqueda diferente para poder generar el Reporte.", "Validación del Sistema", MessageBoxButtons.OK);
                return;
            }

            if (criteriobusqueda == "ESTADO COMERCIAL")
            {
                if (ckaprobado.Checked == false && ckpendiente.Checked == false && ckanulado.Checked == false)
                {
                    MessageBox.Show("Debe seleccionar el Estado Comercial que desea incluir en la generación del Reporte.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    Liquidaciones_ExportarPDF_XTipoBusqueda(Desde, Hasta, Tituloreporte, nombrearchivo, criteriobusqueda, busquedaseleccionada, busquedaxdescripcion, ckaprobado, ckpendiente, ckanulado, ckliquidado, ckporliquidar);
                }
            }

            if (criteriobusqueda == "ESTADO CONTABILIDAD")
            {
                if (ckliquidado.Checked == false && ckporliquidar.Checked == false)
                {
                    MessageBox.Show("Debe seleccionar el Estado Contabilidad que desea incluir en la generación del Reporte.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    Liquidaciones_ExportarPDF_XTipoBusqueda(Desde, Hasta, Tituloreporte, nombrearchivo, criteriobusqueda, busquedaseleccionada, busquedaxdescripcion, ckaprobado, ckpendiente, ckanulado, ckliquidado, ckporliquidar);
                }
            }

            if (criteriobusqueda == "RESPONSABLE")
            {
                if (busquedaxdescripcion == "")
                {
                    MessageBox.Show("Debe ingresar un nombre de responsable para poder generar la exportación del Reporte", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    Liquidaciones_ExportarPDF_XTipoBusqueda(Desde, Hasta, Tituloreporte, nombrearchivo, criteriobusqueda, busquedaseleccionada, busquedaxdescripcion, ckaprobado, ckpendiente, ckanulado, ckliquidado, ckporliquidar);
                }
            }

            if (criteriobusqueda == "CLIENTE")
            {
                if (busquedaxdescripcion == "")
                {
                    MessageBox.Show("Debe ingresar un nombre de Cliente para poder generar al exportación del Reporte", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    Liquidaciones_ExportarPDF_XTipoBusqueda(Desde, Hasta, Tituloreporte, nombrearchivo, criteriobusqueda, busquedaseleccionada, busquedaxdescripcion, ckaprobado, ckpendiente, ckanulado, ckliquidado, ckporliquidar);
                }
            }

            if (criteriobusqueda == "VEHICULO")
            {
                if (busquedaseleccionada.SelectedValue != null)
                {
                    Liquidaciones_ExportarPDF_XTipoBusqueda(Desde, Hasta, Tituloreporte, nombrearchivo, criteriobusqueda, busquedaseleccionada, busquedaxdescripcion, ckaprobado, ckpendiente, ckanulado, ckliquidado, ckporliquidar);
                }
            }

            if (criteriobusqueda == "MONEDA")
            {
                if (busquedaseleccionada.SelectedValue != null)
                {
                    Liquidaciones_ExportarPDF_XTipoBusqueda(Desde, Hasta, Tituloreporte, nombrearchivo, criteriobusqueda, busquedaseleccionada, busquedaxdescripcion, ckaprobado, ckpendiente, ckanulado, ckliquidado, ckporliquidar);
                }
            }

            if (criteriobusqueda == "SIN FILTROS")
            {
                Liquidaciones_ExportarPDF_XTipoBusqueda(Desde, Hasta, Tituloreporte, nombrearchivo, criteriobusqueda, busquedaseleccionada, busquedaxdescripcion, ckaprobado, ckpendiente, ckanulado, ckliquidado, ckporliquidar);
            }
        }

        private void btnExportarPDFLiqui_Click(object sender, EventArgs e)
        {
            Liquidaciones_ValidarEnvioDatos_GenerarReportePdf(DesdeLiqui.Value, HastaLiqui.Value, lblTituloReporteLiqui,lblNombreArchivosLiqui, cboCriterioBusquedaLiqui.Text, txtBusquedaLiqui.Text, cboBusquedaSeleccionadaLiqui, ckAprobadosLiqui
                , ckPendienteLiqui, ckAnuladoLiqui, ckliquidadoLiqui, ckporliquidarLiqui);
        }



        ///-----------------------------------------------------
        ///
        ///ACCIONES DE LOS CHECKBOX PARA LISTAR LAS LIQUIDACIONES EN TIEMPO REAL
        ///

        //ACCIONES DE LOS CHECKBOS EN TIEMPO REAL PARA LA SECCIÓN DE LIQUIDACIONES
        public void MostrarLiquidaciones_PorCheckbox(string criteriobusqqueda)
        {
            if (criteriobusqqueda == "ESTADO COMERCIAL")
            {
                MostrarLiquidacionesPor_EstadoComercial(DesdeLiqui.Value, HastaLiqui.Value, cboCriterioBusquedaLiqui.Text, datalistadoliquidaciones, ckAprobadosLiqui
                              , ckPendienteLiqui, ckAnuladoLiqui);
            }
            else if (criteriobusqqueda == "ESTADO CONTABILIDAD")
            {
                MostrarLiquidacionesPor_EstadosContabilidad(DesdeLiqui.Value, HastaLiqui.Value, cboCriterioBusquedaLiqui.Text, datalistadoliquidaciones
                      , ckliquidadoLiqui, ckporliquidarLiqui);
            }
        }


        private void ckAprobadosLiqui_CheckedChanged(object sender, EventArgs e)
        {
            MostrarLiquidaciones_PorCheckbox(cboCriterioBusquedaLiqui.Text);
        }

        private void ckPendienteLiqui_CheckedChanged(object sender, EventArgs e)
        {
            MostrarLiquidaciones_PorCheckbox(cboCriterioBusquedaLiqui.Text);
        }

        private void ckAnuladoLiqui_CheckedChanged(object sender, EventArgs e)
        {

            MostrarLiquidaciones_PorCheckbox(cboCriterioBusquedaLiqui.Text);
        }
        private void ckliquidadoLiqui_CheckedChanged(object sender, EventArgs e)
        {
            MostrarLiquidaciones_PorCheckbox(cboCriterioBusquedaLiqui.Text);

        }
        private void ckporliquidarLiqui_CheckedChanged(object sender, EventArgs e)
        {
            MostrarLiquidaciones_PorCheckbox(cboCriterioBusquedaLiqui.Text);
        }


        ///-----------------------------------------------------
        ///
        ///METODO PARA EXPORTAR EL LISTADO EN TEXTO PLANO
        ///

        //METODO PARA LIMPIAR LAS CABECERAS DE LAS COLUMNAS
        public string Liquidaciones_LimpiarCabecera_XML(string texto)
        {
            var limpio = new string(texto.Where(c => char.IsLetterOrDigit(c) || c == '_').ToArray());

            if (string.IsNullOrWhiteSpace(limpio))
            {
                limpio = "Columna";
            }

            if (char.IsDigit(limpio[0]))
            {
                limpio = "_" + limpio;
            }

            return limpio;
        }


        //METODO PARA LA EXPORTACIÓN EN TEXTO PLANO
        public void Liquidaciones_ExportarListadoXML(string criteriobusqueda, DataGridView DGVExcel, DataGridView DGVListadoPrin, string busquedaseleccionada, Label nombrearchivo, CheckBox ckaprobado
            , CheckBox ckpendiente, CheckBox ckanulado, CheckBox ckliquidado, CheckBox ckporliquidar)
        {

            Liquidaciones_MostrarExcel(criteriobusqueda, DGVExcel, DGVListadoPrin);

            if (DGVListadoPrin.DataSource != null)
            {
                XElement inicio = new XElement("Registros");

                foreach (DataGridViewRow fila in DGVExcel.Rows)
                {
                    XElement registros = new XElement("Liquidaciones");

                    foreach (DataGridViewColumn columnas in DGVExcel.Columns)
                    {
                        string encabezado = Liquidaciones_LimpiarCabecera_XML(columnas.HeaderText);
                        string valorcelda = fila.Cells[columnas.Index].Value?.ToString() ?? "";

                        registros.Add(new XElement(encabezado, valorcelda));
                    }
                    inicio.Add(registros);
                }

                XDocument documento = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), inicio);

                Liquidaciones_NombreArchivos_Exportados(criteriobusqueda, busquedaseleccionada, nombrearchivo, ckaprobado, ckpendiente, ckanulado, ckliquidado, ckporliquidar);

                string rutaescritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string rutasalida = System.IO.Path.Combine(rutaescritorio, nombrearchivo.Text);

                //SE GUARDA EL XML EN EL ESCRITORIO
                documento.Save(rutasalida);
                MessageBox.Show("La exportación de Liquidaciones en texto plano se realizo correctamente.", "Exportación Exitosa", MessageBoxButtons.OK);
            }
        }

        //EVENTO QUE REALIZARA LA EXPORTACIÓN EN TEXTO PLANO
        private void btnExportarXMLLiqui_Click(object sender, EventArgs e)
        {
            Liquidaciones_ExportarListadoXML(cboCriterioBusquedaLiqui.Text, datalistadoExcelLiqui, datalistadoliquidaciones, cboBusquedaSeleccionadaLiqui.Text, lblNombreArchivosLiqui, ckAprobadosLiqui
                , ckPendienteLiqui, ckAnuladoLiqui, ckliquidadoLiqui, ckporliquidarLiqui);
        }

        //REPORTES DE ACTAS---------------------------------------------------------------
        //HABILITAR ACTAS

        private void btnReportesActas_Click(object sender, EventArgs e)
        {
            panelReportesActas.Visible = true;
            panelReportesRequerimiento.Visible = false;
            panelReportesLiquidaciones.Visible = false;
        }

        public void Actas_LimpiarCombo_Bloquear_BusquedaSeleccionada(TextBox busquedaxdescripcion, DataGridView DGV, string criteriobusqueda, Button mostrartodo, CheckBox ckaprobado, CheckBox ckculminado
            , CheckBox ckpendiente, GroupBox Estadoactas)
        {
            ckaprobado.Checked = false;
            ckculminado.Checked = false;
            ckpendiente.Checked = false;

            DGV.DataSource = null;
            busquedaxdescripcion.Text = "";

            if (criteriobusqueda == "SELECCIONE UNA BUSQUEDA")
            {
                mostrartodo.Enabled = false;
                busquedaxdescripcion.Enabled = false;
                Estadoactas.Visible = false;
            }
            else
            {
                mostrartodo.Enabled = true;
                busquedaxdescripcion.Enabled = true;
            }

            if (criteriobusqueda == "ESTADO ACTAS")
            {
                busquedaxdescripcion.Enabled = false;
                Estadoactas.Visible = true;
            }

            if (criteriobusqueda == "CLIENTE" || criteriobusqueda == "RESPONSABLE")
            {
                Estadoactas.Visible = false;
            }
        }

        private void cboCriterioBusquedaActas_SelectedIndexChanged(object sender, EventArgs e)
        {
            Actas_LimpiarCombo_Bloquear_BusquedaSeleccionada(txtBusquedaActas, datalistadoActas, cboCriterioBusquedaActas.Text, btnMostrarTodasActas, ckAprobadoActas, ckCulminadoActas, ckPendienteActas, grpEstadoActas);
        }

        public void MostrarActaasPor_EstadoActas(DateTime Desde, DateTime Hasta, string criteriobusqueda, DataGridView DGV, CheckBox ckaprobado, CheckBox ckculminado, CheckBox ckpendiente)
        {
            try
            {
                if (criteriobusqueda == "ESTADO ACTAS")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();

                    if (ckaprobado.Checked == true)
                    {
                        SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarActasXEstadoActas", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fechadesde", Desde);
                        cmd.Parameters.AddWithValue("@fechahasta", Hasta);
                        cmd.Parameters.AddWithValue("@estadoactas", 2);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }

                    if (ckculminado.Checked == true)
                    {
                        SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarActasXEstadoActas", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fechadesde", Desde);
                        cmd.Parameters.AddWithValue("@fechahasta", Hasta);
                        cmd.Parameters.AddWithValue("@estadoactas", 1);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }

                    if (ckpendiente.Checked == true)
                    {
                        SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarActasXEstadoActas", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fechadesde", Desde);
                        cmd.Parameters.AddWithValue("@fechahasta", Hasta);
                        cmd.Parameters.AddWithValue("@estadoactas", 0);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                  
                    DGV.DataSource = dt;
                    con.Close();

                    RedimensionarColumnasListado(DGV);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void MostrarActasPor_Descripcion(DateTime Desde, DateTime Hasta, string criteriobusqueda, string descripcion, DataGridView DGV)
        {
            try
            {
                if (criteriobusqueda == "RESPONSABLE")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarActas_BusquedaXResponsable", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechadesde", Desde);
                    cmd.Parameters.AddWithValue("@fechahasta", Hasta);
                    cmd.Parameters.AddWithValue("@responsable", descripcion);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                    RedimensionarColumnasListado(DGV);

                }
                else if (criteriobusqueda == "CLIENTE")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarActas_BusquedaXCliente", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechadesde", Desde);
                    cmd.Parameters.AddWithValue("@fechahasta", Hasta);
                    cmd.Parameters.AddWithValue("@cliente", descripcion);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                    RedimensionarColumnasListado(DGV);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void MostrarActas_PorFecha(DateTime Desde, DateTime Hasta, DataGridView DGV, string criteriobusqueda)
        {
            try
            {
                if (criteriobusqueda == "SIN FILTROS")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("ReporteComercial_MostrarActas", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechadesde", Desde);
                    cmd.Parameters.AddWithValue("@fechahasta", Hasta);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                    RedimensionarColumnasListado(DGV);
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void RedimensionarColumnasListado(DataGridView DGV)
        {
            if(DGV.Rows.Count > 0)
            {
                DGV.Columns[0].Width = 55;
                DGV.Columns[1].Width = 55;
                DGV.Columns[2].Width = 90;
                DGV.Columns[3].Width = 90;
                DGV.Columns[4].Width = 90;
                DGV.Columns[5].Width = 250;
                DGV.Columns[6].Width = 150;
                DGV.Columns[7].Width = 250;
                DGV.Columns[8].Width = 120;

                Actas_ColoresListado(DGV);
            }
           
        }

        //COLOREAR REGISTROS
        public void Actas_ColoresListado(DataGridView DGV)
        {
            try
            {
                for (var i = 0; i <= DGV.RowCount - 1; i++)
                {
                    if (DGV.Rows[i].Cells[8].Value.ToString() == "PENDIENTE")
                    {
                        //PENDIENTE
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else if (DGV.Rows[i].Cells[8].Value.ToString() == "APROBADO")
                    {
                        //APROBADO
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                    }
                    else if (DGV.Rows[i].Cells[8].Value.ToString() == "ANULADO")
                    {
                        //DESAPROBADO
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        //CULMINADO
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        public void Actas_ValidarChecksActivos(string criteriobusqueda, CheckBox ckaprobado, CheckBox ckculminado, CheckBox ckpendiente, DateTime Desde, DateTime Hasta
            , DataGridView DGV)
        {
            if (criteriobusqueda == "ESTADO ACTAS")
            {
                if (ckaprobado.Checked == false && ckculminado.Checked == false && ckpendiente.Checked == false)
                {
                    MessageBox.Show("Marque el estado que desea visualizar.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    MostrarActaasPor_EstadoActas(Desde, Hasta, criteriobusqueda, DGV, ckaprobado, ckculminado, ckpendiente);
                }
            }
        }

        private void btnMostrarTodasActas_Click(object sender, EventArgs e)
        {
            Actas_ValidarChecksActivos(cboCriterioBusquedaActas.Text, ckAprobadoActas, ckCulminadoActas, ckPendienteActas, DesdeActas.Value, HastaActas.Value, datalistadoActas);
            MostrarActasPor_Descripcion(DesdeActas.Value, HastaActas.Value, cboCriterioBusquedaActas.Text, txtBusquedaActas.Text, datalistadoActas);
            MostrarActas_PorFecha(DesdeActas.Value, HastaActas.Value, datalistadoActas, cboCriterioBusquedaActas.Text);
        }

        public void MostrarActas_PorCheckbox(string criteriobusqueda, DateTime Desde, DateTime Hasta, DataGridView DGV, CheckBox ckaprobado, CheckBox ckculminado, CheckBox ckpendiente)
        {
            if (criteriobusqueda == "ESTADO ACTAS")
            {
                MostrarActaasPor_EstadoActas(Desde, Hasta, criteriobusqueda, DGV, ckaprobado, ckculminado, ckpendiente);
            }
        }

        private void ckAprobadoActas_CheckedChanged(object sender, EventArgs e)
        {
            MostrarActas_PorCheckbox(cboCriterioBusquedaActas.Text, DesdeActas.Value, HastaActas.Value, datalistadoActas, ckAprobadoActas, ckCulminadoActas, ckPendienteActas);
        }

        private void ckCulminadoActas_CheckedChanged(object sender, EventArgs e)
        {
            MostrarActas_PorCheckbox(cboCriterioBusquedaActas.Text, DesdeActas.Value, HastaActas.Value, datalistadoActas, ckAprobadoActas, ckCulminadoActas, ckPendienteActas);
        }

        private void ckPendienteActas_CheckedChanged(object sender, EventArgs e)
        {
            MostrarActas_PorCheckbox(cboCriterioBusquedaActas.Text, DesdeActas.Value, HastaActas.Value, datalistadoActas, ckAprobadoActas, ckCulminadoActas, ckPendienteActas);
        }


        ///-----------------------------------------------------
        ///
        ///METODO Y EVENTO PARA EXPORTAR EL LISTADO A UN EXCEL
        ///    

        //METODO PARA AGREGAR LAS COLUMNAS Y FILAS AL DATAGRIDVIEW QUE SE EXPORTARA A EXCEL
        public void Actas_MostrarExcel(string criteriobusqueda, DataGridView DGV, DataGridView DGV2, CheckBox ckaprobado, CheckBox ckculminado, CheckBox ckpendiente)
        {
            if (DGV2.DataSource == null || DGV2.Rows.Count == 0)
            {
                MessageBox.Show("No se puede exportar un listado vacio.", "Validación del Sistema", MessageBoxButtons.OK);
                return;
            }

            else
            {
                DGV.Columns.Clear();

                DGV.Columns.Add("colNroActa", "N°. ACTA");
                DGV.Columns.Add("colNroLiqui", "N°. LIQUI");
                DGV.Columns.Add("colValidar", "VALIDAR");
                DGV.Columns.Add("colFechaIni", "FECHA INICIO");
                DGV.Columns.Add("colFechaTerm", "FECHA TÉRMINO");
                DGV.Columns.Add("colCliente", "CLIENTE");
                DGV.Columns.Add("colUnidad", "UNIDAD");
                DGV.Columns.Add("colResponsable", "RESPONSABLE");
                DGV.Columns.Add("colEstado", "ESTADO");

                //CAPTURA DE LAS COLUMNAS QUE SE VAN A EXPORTAR DEPENDIENDO DEL TIPO DE BUSQUEDA
                Dictionary<string, int[]> columnastipobusqueda = new Dictionary<string, int[]>
                {
                    {"ESTADO ACTAS" ,new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8}},
                    {"RESPONSABLE", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8} },
                    {"CLIENTE", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8} },
                    {"SIN FILTROS", new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8} }
                };

                int[] columnas = columnastipobusqueda[criteriobusqueda];

                foreach (DataGridViewRow dgv in DGV2.Rows)
                {
                    List<string> fila = new List<string>();

                    foreach (int i in columnas)
                    {
                        string valor = dgv.Cells[i].Value?.ToString() ?? "";

                        fila.Add(valor);
                    }
                    DGV.Rows.Add(fila.ToArray());
                }
            }
        }
        

        public void Actas_NombreArchivos_Exportados(string criteriobusqueda, Label nombrearchivo, CheckBox ckaprobado, CheckBox ckculminado, CheckBox ckpendiente)
        {
            //DEFINICIÓN PARA EL NOMBRE DE ARCHIVO CON EL TIPO DE BUSQUEDA ESTADO COMERCIAL
            if (criteriobusqueda == "ESTADO ACTAS")
            {
                if (ckaprobado.Checked == true && ckculminado.Checked == true && ckpendiente.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Actas_Todos_EstadosActas";
                }

                else if (ckaprobado.Checked == true && ckculminado.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Actas_Aprobado_Culminado";
                }

                else if (ckaprobado.Checked == true && ckpendiente.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Actas_Aprobado_Pendiente";
                }

                else if (ckculminado.Checked == true && ckpendiente.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Actas_Culminado_Pendiente";
                }

                else if (ckaprobado.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Actas_Aprobados";
                }

                else if (ckculminado.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Actas_Culminados";
                }

                else if (ckpendiente.Checked == true)
                {
                    nombrearchivo.Text = "Reporte_Actas_Pendientes";
                }
            }

            if(ckaprobado.Checked == false && ckculminado.Checked == false && ckpendiente.Checked == false)
            {
                nombrearchivo.Text = "Reporte_Actas_Generales";
            }

            //DEFINICIÓN PARA EL NOMBRE DE ARCHIVO CON EL TIPO DE BUSQUEDA RESPONSABLE,CLIENTE,VEHICULO Y MONEDA,SIN FILTROS
            if (criteriobusqueda == "RESPONSABLE")
            {
                nombrearchivo.Text = "Reporte_Actas_Responsable";
            }

            if (criteriobusqueda == "CLIENTE")
            {
                nombrearchivo.Text = "Reporte_Actas_Cliente";
            }
          
            if (criteriobusqueda == "SIN FILTROS")
            {
                nombrearchivo.Text = "Reporte_Actas_Generales";
            }

        }




        public void Actas_ExportarExcel_XTipoBusqueda(string criteriobusqueda, Label nombrearchivo, DataGridView DGVExcel, DataGridView DGVListadoPrinci, CheckBox ckaprobado, CheckBox ckculminado, CheckBox ckpendiente)
        {
            Actas_MostrarExcel(criteriobusqueda, DGVExcel, DGVListadoPrinci, ckaprobado, ckculminado, ckpendiente);

            if (DGVListadoPrinci.DataSource == null || DGVListadoPrinci.RowCount == 0)
            {
                return;
            }

            else
            {
                try
                {
                    SLDocument sl = new SLDocument();
                    SLStyle style = new SLStyle();
                    SLStyle styleC = new SLStyle();

                    //COLUMNAS

                    if (criteriobusqueda == "ESTADO ACTAS" || criteriobusqueda == "CLIENTE" || criteriobusqueda == "RESPONSABLE" || criteriobusqueda == "SIN FILTROS")
                    {
                        sl.SetColumnWidth(1, 15);
                        sl.SetColumnWidth(2, 15);
                        sl.SetColumnWidth(3, 20);
                        sl.SetColumnWidth(4, 20);
                        sl.SetColumnWidth(5, 20);
                        sl.SetColumnWidth(6, 50);
                        sl.SetColumnWidth(7, 60);
                        sl.SetColumnWidth(8, 100);
                        sl.SetColumnWidth(9, 70);

                    }

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
                    foreach (DataGridViewColumn column in DGVExcel.Columns)
                    {
                        sl.SetCellValue(1, ic, column.HeaderText.ToString());
                        sl.SetCellStyle(1, ic, style);
                        ic++;
                    }

                    int ir = 2;

                    if (criteriobusqueda == "ESTADO ACTAS" || criteriobusqueda == "CLIENTE" || criteriobusqueda == "RESPONSABLE" || criteriobusqueda == "SIN FILTROS")
                    {
                        foreach (DataGridViewRow row in DGVExcel.Rows)
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

                            sl.SetCellStyle(ir, 1, styleC);
                            sl.SetCellStyle(ir, 2, styleC);
                            sl.SetCellStyle(ir, 3, styleC);
                            sl.SetCellStyle(ir, 4, styleC);
                            sl.SetCellStyle(ir, 5, styleC);
                            sl.SetCellStyle(ir, 6, styleC);
                            sl.SetCellStyle(ir, 7, styleC);
                            sl.SetCellStyle(ir, 8, styleC);
                            sl.SetCellStyle(ir, 9, styleC);

                            ir++;
                        }
                    }


                    Actas_NombreArchivos_Exportados(criteriobusqueda, nombrearchivo, ckaprobado, ckculminado, ckpendiente);

                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    sl.SaveAs(desktopPath + @"\" + nombrearchivo.Text + ".xlsx");
                    MessageBox.Show("Se exportó los datos a un archivo de Microsoft Excel en la siguiente ubicación: " + desktopPath, "Validación del Sistema", MessageBoxButtons.OK);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        

        private void btnExportarExcelActas_Click(object sender, EventArgs e)
        {
            Actas_ExportarExcel_XTipoBusqueda(cboCriterioBusquedaActas.Text, lblNombreArchivosActas, datalistadoExcelActas, datalistadoActas, ckAprobadoActas, ckCulminadoActas, ckPendienteActas);
        }

        ///-----------------------------------------------------
        ///
        ///METODO PARA EXPORTAR EN PDF
        ///


        //METODO PARA ASIGNAR UN TITULO AL REPORTE DEPENDIENDO DE LA BUSQUEDA SELECCIONADA
        public void Actas_Reporte_Titulo(string criteriobusqueda, Label tituloreporte,CheckBox ckaprobado, CheckBox ckculminado, CheckBox ckpendiente)
        {
            //DEFINICIÓN DEL TITULO PARA EL REPORTE SEGUN EL TIPO DE BUSQUEDA ESTADO COMERCIAL
            if (criteriobusqueda == "ESTADO ACTAS")
            {
                if (ckaprobado.Checked == true && ckculminado.Checked == true && ckpendiente.Checked == true)
                {
                    tituloreporte.Text = "con Todos los Estados de Actas";
                }

                else if (ckaprobado.Checked == true && ckculminado.Checked == true)
                {
                    tituloreporte.Text = "con los Estados Actas Aprobado y Culminado";
                }

                else if (ckaprobado.Checked == true && ckpendiente.Checked == true)
                {
                    tituloreporte.Text = "con los Estados Actas Aprobado y Pendiente";
                }

               
                else if (ckculminado.Checked == true && ckpendiente.Checked == true)
                {
                    tituloreporte.Text = "con los Estados Actas Culminado y Pendiente";
                }

                else if (ckaprobado.Checked == false && ckculminado.Checked == false && ckpendiente.Checked == false)
                {
                    tituloreporte.Text = "Generales";
                }

                else if (ckaprobado.Checked == true)
                {
                    tituloreporte.Text = "Aprobados según Estado Actas";
                }

                else if (ckculminado.Checked == true)
                {
                    tituloreporte.Text = "Anulados según Estado Actas";
                }

                else if (ckpendiente.Checked == true)
                {
                    tituloreporte.Text = "Pendientes según Estado Actas";
                }

              
            }
            
            //DEFINICIÓN DEL TITULO PARA EL REPORTE SEGUN EL TIPO DE BUSQUEDA RESPONSABLE,CLIENTE,VEHICULO,MONEDA,SIN FILTROS
            if (criteriobusqueda == "RESPONSABLE")
            {
                tituloreporte.Text = "filtrado por Responsable";
            }

            if (criteriobusqueda == "CLIENTE")
            {
                tituloreporte.Text = "filtrado por Cliente";
            }
            
            if (criteriobusqueda == "SIN FILTROS")
            {
                tituloreporte.Text = "Generales";
            }
        }

        public void Actas_ExportarPDF_XTipoBusqueda(DateTime desde, DateTime hasta, Label tituloreporte,Label nombrearchivo, string criteriobusqueda,string busquedaxdescripcion, CheckBox ckaprobado,CheckBox ckculminado ,CheckBox ckpendiente)
        {
            Actas_Reporte_Titulo(criteriobusqueda, tituloreporte, ckaprobado, ckculminado, ckpendiente);
            Actas_NombreArchivos_Exportados(criteriobusqueda, nombrearchivo, ckaprobado, ckculminado, ckpendiente);

            try
            {
                ReportDocument crystalreport = new ReportDocument();

                string rutareporte = "C:\\Users\\kevin\\Desktop\\ArenasProyect3\\ArenasProyect3\\Reportes\\InformeListarActas.rpt";
                crystalreport.Load(rutareporte);

                ConnectionInfo connectioninfo = new ConnectionInfo
                {
                    ServerName = "DESKTOP-ABO4DEQ\\SQLEXPRESS",
                    DatabaseName = "BD_VENTAS_2",
                    UserID = "sa",
                    Password = "12345"
                };

                crystalreport.SetParameterValue("@fechadesde", desde);
                crystalreport.SetParameterValue("@fechahasta", hasta);
                crystalreport.SetParameterValue("@tituloreporte", tituloreporte.Text);

                //EXPORTACIÓN PARA LOS ESTADOS DE ACTAS

                //SI TODOS LOS CHECKS ESTAN MARCADOS
                if (criteriobusqueda == "ESTADO ACTAS")
                {
                    if (ckaprobado.Checked == true && ckculminado.Checked == true && ckpendiente.Checked == true )
                    {
                        crystalreport.SetParameterValue("@aprobado", 2);
                        crystalreport.SetParameterValue("@culminado", 1);
                        crystalreport.SetParameterValue("@pendiente", 0);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                    }

                    //SI DOS CHECKS ESTAN MARCADOS
                    else if (ckaprobado.Checked == true && ckculminado.Checked == true)
                    {
                        crystalreport.SetParameterValue("@aprobado", 2);
                        crystalreport.SetParameterValue("@culminado", 1);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                    }

                    else if (ckaprobado.Checked == true && ckpendiente.Checked == true)
                    {
                        crystalreport.SetParameterValue("@aprobado", 2);
                        crystalreport.SetParameterValue("@pendiente", 0);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@culminado", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                    }



                    else if (ckculminado.Checked == true && ckpendiente.Checked == true  )
                    {
                        crystalreport.SetParameterValue("@culminado", 1);
                        crystalreport.SetParameterValue("@pendiente", 0);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                    }
                
                    //SI UN CHECK ESTA MARCADO
                    else if (ckaprobado.Checked == true)
                    {
                        crystalreport.SetParameterValue("@aprobado", 2);


                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@culminado", DBNull.Value);
                        crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                    }

                    else if (ckculminado.Checked == true)
                    {
                        crystalreport.SetParameterValue("@culminado", 1);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                    }

                    else if (ckpendiente.Checked == true)
                    {
                        crystalreport.SetParameterValue("@pendiente", 0);

                        //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                        crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                        crystalreport.SetParameterValue("@culminado", DBNull.Value);
                        crystalreport.SetParameterValue("@responsable", DBNull.Value);
                        crystalreport.SetParameterValue("@cliente", DBNull.Value);
                    }                        
                }

              
                //////////////////////////////////////////////////////////////////
                ///EXPORTACIÓN DE LOS LIQUIDACIONES POR MEDIO DEL NOMBRE DEL RESPONSABLE
                ///

                if (criteriobusqueda == "RESPONSABLE")
                {
                    crystalreport.SetParameterValue("@responsable", busquedaxdescripcion);

                    //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                    crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                    crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                    crystalreport.SetParameterValue("@culminado", DBNull.Value);
                    crystalreport.SetParameterValue("@cliente", DBNull.Value);
                }

                //////////////////////////////////////////////////////////////////
                ///EXPORTACIÓN DE LOS LIQUIDACIONES POR MEDIO DEL NOMBRE DEL CLIENTE
                ///

                if (criteriobusqueda == "CLIENTE")
                {
                    crystalreport.SetParameterValue("@cliente", busquedaxdescripcion);

                    //PARAMETROS INNECESARIOS PARA LA EXPORTACIÓN
                    crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                    crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                    crystalreport.SetParameterValue("@culminado", DBNull.Value);
                    crystalreport.SetParameterValue("@responsable", DBNull.Value);
                }
              
                if (criteriobusqueda == "SIN FILTROS")
                {
                    crystalreport.SetParameterValue("@aprobado", DBNull.Value);
                    crystalreport.SetParameterValue("@culminado", DBNull.Value);
                    crystalreport.SetParameterValue("@pendiente", DBNull.Value);
                    crystalreport.SetParameterValue("@responsable", DBNull.Value);
                    crystalreport.SetParameterValue("@cliente", DBNull.Value);
                }

                // Aplicar la conexión a cada tabla del reporte
                foreach (CrystalDecisions.CrystalReports.Engine.Table table in crystalreport.Database.Tables)
                {
                    TableLogOnInfo logOnInfo = table.LogOnInfo;
                    logOnInfo.ConnectionInfo = connectioninfo;
                    table.ApplyLogOnInfo(logOnInfo);
                }

            

                string rutaescritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string rutasalida = System.IO.Path.Combine(rutaescritorio, nombrearchivo.Text + ".pdf");

                crystalreport.ExportToDisk(ExportFormatType.PortableDocFormat, rutasalida);

                MessageBox.Show($"Listado exportado correctamente a: {rutasalida} ", "Exportado Exitsoamente", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //METODO PARA VALIDAR SI SE SELECCIONO UN ESTADO SEGUN EL TIPO DE BUSQUEDA SELECCIONADO PARA LA GENERACIÓN DEL REPORTE
        public void Actas_ValidarEnvioDatos_GenerarReportePdf(DateTime Desde, DateTime Hasta, Label Tituloreporte, Label nombrearchivo, string criteriobusqueda, string busquedaxdescripcion,CheckBox ckaprobado
            ,CheckBox ckculminado ,CheckBox ckpendiente)
        {
            if (criteriobusqueda == "SELECCIONE UNA BUSQUEDA")
            {
                MessageBox.Show("Debe seleccionar un Tipo de Búsqueda diferente para poder generar el Reporte.", "Validación del Sistema", MessageBoxButtons.OK);
                return;
            }

            if(criteriobusqueda == "ESTADO ACTAS")
            {
                if(ckaprobado.Checked == false && ckculminado.Checked == false && ckpendiente.Checked == false)
                {
                    MessageBox.Show("Seleccione un estado para continuar con la exportación.","Validación del Sistema",MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    Actas_ExportarPDF_XTipoBusqueda(Desde, Hasta, Tituloreporte, nombrearchivo, criteriobusqueda, busquedaxdescripcion, ckaprobado, ckculminado, ckpendiente);
                }
            }
            
            if (criteriobusqueda == "RESPONSABLE")
            {
                if (busquedaxdescripcion == "")
                {
                    MessageBox.Show("Debe ingresar un nombre de responsable para poder generar la exportación del Reporte.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    Actas_ExportarPDF_XTipoBusqueda(Desde, Hasta, Tituloreporte, nombrearchivo, criteriobusqueda, busquedaxdescripcion,ckaprobado, ckculminado, ckpendiente);
                }
            }

            if (criteriobusqueda == "CLIENTE")
            {
                if (busquedaxdescripcion == "")
                {
                    MessageBox.Show("Debe ingresar un nombre de Cliente para poder generar al exportación del Reporte.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    Actas_ExportarPDF_XTipoBusqueda(Desde, Hasta, Tituloreporte, nombrearchivo, criteriobusqueda, busquedaxdescripcion, ckaprobado, ckculminado, ckpendiente);
                }
            }
           
            if (criteriobusqueda == "SIN FILTROS")
            {
                Actas_ExportarPDF_XTipoBusqueda(Desde, Hasta, Tituloreporte, nombrearchivo, criteriobusqueda, busquedaxdescripcion, ckaprobado, ckculminado, ckpendiente);
            }
        }


        private void btnExportarPDFActas_Click(object sender, EventArgs e)
        {
            Actas_ValidarEnvioDatos_GenerarReportePdf(DesdeActas.Value,HastaActas.Value,lblTituloReporteActas,lblNombreArchivosActas,cboCriterioBusquedaActas.Text,txtBusquedaActas.Text,ckAprobadoActas,ckCulminadoActas
                ,ckPendienteActas);
        }

        ///-----------------------------------------------------
        ///
        ///METODO PARA EXPORTAR EL LISTADO EN TEXTO PLANO
        ///

        //METODO PARA LIMPIAR LAS CABECERAS DE LAS COLUMNAS
        public string Actas_LimpiarCabecera_XML(string texto)
        {
            var limpio = new string(texto.Where(c => char.IsLetterOrDigit(c) || c == '_').ToArray());

            if (string.IsNullOrWhiteSpace(limpio))
            {
                limpio = "Columna";
            }

            if (char.IsDigit(limpio[0]))
            {
                limpio = "_" + limpio;
            }

            return limpio;
        }


        //METODO PARA LA EXPORTACIÓN EN TEXTO PLANO
        public void Actas_ExportarListadoXML(string criteriobusqueda, DataGridView DGVExcel, DataGridView DGVListadoPrin, Label nombrearchivo, CheckBox ckaprobado, CheckBox ckculminado, CheckBox ckpendiente)
        {

            Actas_MostrarExcel(criteriobusqueda, DGVExcel, DGVListadoPrin, ckaprobado, ckculminado, ckpendiente);


            if (DGVListadoPrin.DataSource != null)
            {
                XElement inicio = new XElement("Registros");

                foreach (DataGridViewRow fila in DGVExcel.Rows)
                {
                    XElement registros = new XElement("Actas");

                    foreach (DataGridViewColumn columnas in DGVExcel.Columns)
                    {
                        string encabezado = Actas_LimpiarCabecera_XML(columnas.HeaderText);
                        string valorcelda = fila.Cells[columnas.Index].Value?.ToString() ?? "";

                        registros.Add(new XElement(encabezado, valorcelda));
                    }
                    inicio.Add(registros);
                }

                XDocument documento = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), inicio);

                Actas_NombreArchivos_Exportados(criteriobusqueda, nombrearchivo, ckaprobado, ckculminado, ckpendiente);

                string rutaescritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string rutasalida = System.IO.Path.Combine(rutaescritorio, nombrearchivo.Text);

                //SE GUARDA EL XML EN EL ESCRITORIO
                documento.Save(rutasalida);
                MessageBox.Show("La exportación de Actas en texto plano se realizo correctamente.", "Exportación Exitosa", MessageBoxButtons.OK);
            }
        }
        
        private void btnExportarXMLActas_Click(object sender, EventArgs e)
        {
            Actas_ExportarListadoXML(cboCriterioBusquedaActas.Text,datalistadoExcelActas,datalistadoActas,lblNombreArchivosActas,ckAprobadoActas,ckCulminadoActas,ckPendienteActas);
        }

      
    }
     
}


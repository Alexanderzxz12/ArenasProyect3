using ArenasProyect3.Modulos.ManGeneral;
using ArenasProyect3.Modulos.Resourses;
using ArenasProyect3.Visualizadores;
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

namespace ArenasProyect3.Modulos.Comercial.RequerimientosVentas
{
    public partial class RequerimientoVenta : Form
    {
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        bool estadoResopnsable = false;
        int idJefatura = 0;
        string alias = "";
        private Cursor curAnterior = null;
        int numeroLiquidacion = 0;
        int numeroRequerimiento = 0;
        string ruta = ManGeneral.Manual.manualAreaComercial;

        //CONSTRUCTOR DEL MANTENIMIENTO - REQUERIMIENTOS DE VENTA
        public RequerimientoVenta()
        {
            InitializeComponent();
        }

        //INICIO Y CARGA INICIAL DEL REQUERIMEINTO - CONSTRUCTOR--------------------------------------------------------------------------------------
        private void RequerimientoVenta_Load(object sender, EventArgs e)
        {
            //AJUSTAR FECHAS AL INICIO DEL MES Y FINAL DEL MES
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);
            //ASIGNARLE LAS VARIABLES YA CARGADAS A MIS DateTimerPicker
            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;

            //BLOQUEAR MI LISTADO PARA EVITAR MALAS CARGAS DE PDFs Y CARGAS DE DATOS
            datalistadoTodasRequerimientos.DataSource = null;
            panelAprobacionDetalleFueraFecha.Visible = false;
            panelObservacionesRequeAtrasado.Visible = false;
            panelObservacionesLiquiFueraFecha.Visible = false;

            //COLOCAR SOLO LECTURA PARA EVITAR CAMBIOS EN MI LISTADO
            datalistadoPresupuestoViaje.Columns[1].ReadOnly = true;
            datalistadoPresupuestoViaje.Columns[8].ReadOnly = true;

            //PREFILES Y PERSIMOS------------------------------------------------------------------------------------------------------------------
            //SI EL USUARIO TIENE UN RANGO DE EFECTO DE 1 (JEFATURA DEL ÁREA COMERCIAL) O 3 (ADMINISTRADOR)
            if (Program.RangoEfecto == 1 || Program.RangoEfecto == 3)
            {
                //BOTÓN Y LEYENDA DE APROBACIÓN DE REQUERIMIENTO - ACCIÓN PARA QUE APAREZCA Y DESAPAREZCA
                btnAprobarRequerimiento.Visible = true;
                lblAproarRequerimiento.Visible = true;
                //BOTÓN Y LEYENDA DE ANULACIÓN DE REQUERIMIENTO - ACCIÓN PARA QUE APAREZCA Y DESAPAREZCA
                btnDesaprobaRequerimiento.Visible = true;
                lblDesaprobarRequerimiento.Visible = true;
                //BOTÓN Y LEYENDA DE LIBERACIÓN DE REQUERIMIENTO - ACCIÓN PARA QUE APAREZCA Y DESAPAREZCA
                btnLiberarRequerimiento.Visible = true;
                lblLiberarRequerimeinto.Visible = true;
                //QUITAR EL MENSAJE YA QUE JEFAFTURA TIENE TODOS LOS PERMISOSO
                lblMensajeHabilitacion.Visible = false;
                lblSeparacion.Visible = true;
            }
            else
            {
                //BOTÓN Y LEYENDA DE APROBACIÓN DE REQUERIMIENTO - ACCIÓN PARA QUE APAREZCA Y DESAPAREZCA
                btnAprobarRequerimiento.Visible = false;
                lblAproarRequerimiento.Visible = false;
                //BOTÓN Y LEYENDA DE ANULACIÓN DE REQUERIMIENTO - ACCIÓN PARA QUE APAREZCA Y DESAPAREZCA
                btnDesaprobaRequerimiento.Visible = false;
                lblDesaprobarRequerimiento.Visible = false;
                //BOTÓN Y LEYENDA DE LIBERACIÓN DE REQUERIMIENTO - ACCIÓN PARA QUE APAREZCA Y DESAPAREZCA
                btnLiberarRequerimiento.Visible = false;
                lblLiberarRequerimeinto.Visible = false;
                //COLOCAR EL MENSAJE YA NOT IENE AUTORIZACION
                lblMensajeHabilitacion.Visible = true;
                lblSeparacion.Visible = false;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------

        //CARGA DE COMBOS PARA VEHICULOS, RESPONSABLES Y TIPO DE MONEDA----------------------------------------------------------------------------
        //CARGAR RESPONSABLES PARA GENERAR LA LIQUIDACION Y REQUERIMEINTO
        public void CargarResponsables(ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdUsuarios, Nombres + ' ' + Apellidos AS [NOMBRES] FROM Usuarios WHERE Estado = 'Activo' AND HabilitadoRequerimientoVenta = 1 ORDER BY Nombres", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.DisplayMember = "NOMBRES";
                cbo.ValueMember = "IdUsuarios";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //CARGAR VEHIVULOS PARA GENERAR EL REQUERIMEINTO
        public void CargarVehiculosReque(ComboBox cbo)
        {
            try
            {
                //CARGAR EL COMBO
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdVehiculo, Descripcion FROM Vehiculos WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.DisplayMember = "Descripcion";
                cbo.ValueMember = "IdVehiculo";
                cbo.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //CARGAR VEHIVULOS PARA GENERAR EL LIQUIDACIÓN
        public void CargarVehiculosLiqui(ComboBox cbo)
        {
            try
            {
                //CARGAR EL COMBO
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdVehiculo, Descripcion FROM Vehiculos WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.DisplayMember = "Descripcion";
                cbo.ValueMember = "IdVehiculo";
                cbo.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //CARGAR TIPO DE MONEDA PARA GENERAR LA LIQUIDACIÓN Y REQUERIMEINTO
        public void CargarTipoMoneda(ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdTipoMonedas, Abreviatura FROM TipoMonedas WHERE Estado = 1 ORDER BY Abreviatura DESC", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.DisplayMember = "Abreviatura";
                cbo.ValueMember = "IdTipoMonedas";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //CARGAR JEFATURA Y RECONOCER EL TIPO DE USUARIO PARA LA APROBACIÓN Y ANULACIÓN
        public void CargarJefaturaActual()
        {
            try
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
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //CARGAR CODIGOS PARA ALMACENAR EL NUEVO DE LIQUIDACIÓN Y LA RESPECTIVA VALIDACION
        public void codigoLiquidacion()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT IdLiquidacion FROM LiquidacionVenta WHERE IdLiquidacion = (SELECT MAX(IdLiquidacion) FROM LiquidacionVenta)", con);
                da.Fill(dt);
                datalistadoCodigoLiquidacion.DataSource = dt;
                con.Close();

                if (datalistadoCodigoLiquidacion.Rows.Count != 0)
                {
                    numeroLiquidacion = Convert.ToInt32(datalistadoCodigoLiquidacion.SelectedCells[0].Value.ToString());
                    int numeroLiquidacion2 = 0;
                    numeroLiquidacion2 = Convert.ToInt32(numeroLiquidacion);
                    numeroLiquidacion2 = numeroLiquidacion2 + 1;

                    numeroLiquidacion = numeroLiquidacion2;
                }
                else
                {
                    MessageBox.Show("Se debe inicializar la tabla LIQUIDACIONES.", "Validación del Sistema", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //CARGAR CODIGOS PARA ALMACENAR EL NUEVO DE REQUERIMIENTO Y LA RESPECTIVA VALIDACION
        public void codigoRequerimeinto()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT IdRequerimientoVenta FROM RequerimientoVenta WHERE IdRequerimientoVenta = (SELECT MAX(IdRequerimientoVenta) FROM RequerimientoVenta)", con);
                da.Fill(dt);
                datalistadoCodigoRequerimiento.DataSource = dt;
                con.Close();

                if (datalistadoCodigoRequerimiento.Rows.Count != 0)
                {
                    numeroRequerimiento = Convert.ToInt32(datalistadoCodigoRequerimiento.SelectedCells[0].Value.ToString());
                    int numeroRequerimiento2 = 0;
                    numeroRequerimiento2 = Convert.ToInt32(numeroRequerimiento);
                    numeroRequerimiento2 = numeroRequerimiento2 + 1;

                    numeroRequerimiento = numeroRequerimiento2;
                }
                else
                {
                    MessageBox.Show("Se debe inicializar la tabla LIQUIDACIONES.", "Validación del Sistema", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
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
                string FechaGen = dgv.Cells[2].Value.ToString();
                string fechaInicio = dgv.Cells[3].Value.ToString();
                string fechaTermino = dgv.Cells[4].Value.ToString();
                string responsable = dgv.Cells[5].Value.ToString();
                string motivoViaje = dgv.Cells[6].Value.ToString();
                string tipoMoneda = dgv.Cells[7].Value.ToString();
                string total = dgv.Cells[8].Value.ToString();
                string estadoJefatura = dgv.Cells[9].Value.ToString();
                string estadoContabilidad = dgv.Cells[10].Value.ToString();

                datalistadoExcel.Rows.Add(new[] { numeroReque, FechaGen, fechaInicio, fechaTermino, responsable, motivoViaje, tipoMoneda, total, estadoJefatura, estadoContabilidad });
            }
        }

        //VIZUALIZAR DATOS EXCEL COMPLETO--------------------------------------------------------------------
        public void MostrarExcelCompleto()
        {
            datalistadoExcelCompleto2.Rows.Clear();

            foreach (DataGridViewRow dgv in datalistadoExcelCompleto.Rows)
            {
                string numeroReque = dgv.Cells[0].Value.ToString();
                string FechaGen = dgv.Cells[1].Value.ToString();
                string fechaInicio = dgv.Cells[2].Value.ToString();
                string fechaTermino = dgv.Cells[3].Value.ToString();
                string responsable = dgv.Cells[4].Value.ToString();
                string colaboradores = dgv.Cells[5].Value.ToString();
                string cliente = dgv.Cells[6].Value.ToString();
                string unidad = dgv.Cells[7].Value.ToString();
                string destino = dgv.Cells[8].Value.ToString();
                string motivoViaje = dgv.Cells[9].Value.ToString();
                string tipoMoneda = dgv.Cells[10].Value.ToString();
                string total = dgv.Cells[11].Value.ToString();
                string estadoJefatura = dgv.Cells[12].Value.ToString();

                datalistadoExcelCompleto2.Rows.Add(new[] { numeroReque, FechaGen, fechaInicio, fechaTermino, responsable, colaboradores, cliente, unidad, destino, motivoViaje, tipoMoneda, total, estadoJefatura });
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------

        //LISTADO DE REQUERIMIENTOS Y SELECCIÓN DE PDF Y ESTADO DE LIQUIDACIÓN---------------------------------------------------------------
        //MOSTRAR REQUERIMIENTOS AL INCIO Y POR FECHAS
        public void MostrarRequerimientos(DateTime fechaInicio, DateTime fechaTermino)
        {
            try
            {
                //SI EL NÚMERO DE CARGA ESTA EN 0, ESTO SE HACE PARA EVIAR LA CARGA DE LOS REQUERIMIENTOS SIN COLORES
                if (lblCarga.Text == "0")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("RequerimientoViaje_MostrarPorFecha", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoTodasRequerimientos.DataSource = dt;
                    con.Close();
                    RedimensionarListado(datalistadoTodasRequerimientos);
                }
                else
                {
                    lblCarga.Text = "0";
                }
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //MOSTRAR REQUERIMIENTOS AL INCIO Y POR FECHAS PARA EXPORTAR
        public void MostrarRequerimientosExcel(DateTime fechaInicio, DateTime fechaTermino)
        {
            try
            {
                //SI EL NÚMERO DE CARGA ESTA EN 0, ESTO SE HACE PARA EVIAR LA CARGA DE LOS REQUERIMIENTOS SIN COLORES
                if (lblCarga.Text == "0")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("RequerimientoViaje_MostrarPorFechaExcel", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoTodasRequerimientosExportacion.DataSource = dt;
                    con.Close();
                }
                else
                {
                    lblCarga.Text = "0";
                }
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //MOSTRAR REQUERIMIENTOS POR RESPONSABLE DE ACUERDO A LAS FECHAS SELECCIONADAS
        public void MostrarRequerimientosResponsable(string resopnsable, DateTime fechaInicio, DateTime fechaTermino)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("RequerimientoVenta_MostrarPorResponsable", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@responsable", resopnsable);
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoTodasRequerimientos.DataSource = dt;
                con.Close();
                RedimensionarListado(datalistadoTodasRequerimientos);
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //MOSTRAR REQUERIMIENTOS POR ESTADOS---------------------------------------------------------------------------------------------------
        //MOSTRAR REQUERIMIENTOS PEDIENTE DE ACUERDO A LAS FECHAS SELECCIONADAS
        public void MostrarRequerimientosEstadosPendiente(DateTime fechaInicio, DateTime fechaTermino)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("RequerimientoViaje_MostrarPorEstadosPendiente", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoTodasRequerimientos.DataSource = dt;
                con.Close();
                RedimensionarListado(datalistadoTodasRequerimientos);
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //MOSTRAR REQUERIMIENTOS POR ESTADO DE ACUERDO A LAS FECHAS SELECCIONADAS
        public void MostrarRequerimientosEstados(int estado, DateTime fechaInicio, DateTime fechaTermino)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("RequerimientoViaje_MostrarPorEstados", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@estado", estado);
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoTodasRequerimientos.DataSource = dt;
                con.Close();
                RedimensionarListado(datalistadoTodasRequerimientos);
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //MOSTRAR REQUERIMIENTOS DESAPROBADOS
        public void MostrarRequerimientosEstadoDesaprobado(DateTime fechaInicio, DateTime fechaTermino)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("RequerimientoViaje_MostrarPorEstadosDesaprobado", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoTodasRequerimientos.DataSource = dt;
                con.Close();
                RedimensionarListado(datalistadoTodasRequerimientos);
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //FUNCION PARA REDIMENSIONAR MI LISTADO
        public void RedimensionarListado(DataGridView DGV)
        {
            //SE REDIMENSIONA EL TAMAÑO DE CADA COLUMNA DE MI LISTADO DE REQUERIMIENTOS
            DGV.Columns[1].Width = 50;
            DGV.Columns[2].Width = 100;
            DGV.Columns[3].Width = 100;
            DGV.Columns[4].Width = 100;
            DGV.Columns[5].Width = 180;
            DGV.Columns[6].Width = 370;
            DGV.Columns[7].Width = 60;
            DGV.Columns[8].Width = 80;
            DGV.Columns[9].Width = 95;
            DGV.Columns[10].Width = 95;
            DGV.Columns[11].Width = 85;
            //SE QUITA LAS COLUMNAS QUE NO SON RELEVANTES PARA EL USUARIO
            DGV.Columns[12].Visible = false;
            DGV.Columns[13].Visible = false;
            DGV.Columns[14].Visible = false;
            //CARGAR LOS COLORES DE ACUERDO A SU ESTADO
            ColoresListado();

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO
        public void ColoresListado()
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= datalistadoTodasRequerimientos.RowCount - 1; i++)
                {
                    //SI MI REQUERIMIENTO ESTA APROBAOD POR JEFATURA DEL ÁREA COMNERCIAL Y POR LA JEFATURA DEL ÁREA CONTABLE
                    if (datalistadoTodasRequerimientos.Rows[i].Cells[9].Value.ToString() == "APROBADO" && datalistadoTodasRequerimientos.Rows[i].Cells[10].Value.ToString() == "ATENDIDO")
                    {
                        //REQUERIMEINTO APROBADO - COLR VERDE
                        datalistadoTodasRequerimientos.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                    }
                    //SI MI REQUERIMIENTO ESTA PENDIENTE POR EL ÁREA COMERCIAL O NO ESTA ATENDIDO POR EL ÁREA CONTABLE | SI MI REQUERIMEINTO ESTA APROBADO POR EL ÁREA COMERCIAL PERO NO ESTA ATENIDO PO EL ÁREA CONTABLE | SI MI REQUERIMIENTO NO ESTA APROBADO POR EL ÁREA COMERCIAL Y SI ESTA ATENDIDO POR EL ÁREA CONTABLE
                    else if (datalistadoTodasRequerimientos.Rows[i].Cells[9].Value.ToString() == "PENDIENTE" && datalistadoTodasRequerimientos.Rows[i].Cells[10].Value.ToString() == "NO ATENDIDO" || datalistadoTodasRequerimientos.Rows[i].Cells[9].Value.ToString() == "APROBADO" && datalistadoTodasRequerimientos.Rows[i].Cells[10].Value.ToString() == "NO ATENDIDO" || datalistadoTodasRequerimientos.Rows[i].Cells[9].Value.ToString() == "PENDIENTE" && datalistadoTodasRequerimientos.Rows[i].Cells[10].Value.ToString() == "ATENDIDO")
                    {
                        //REQUERIMIENTO PENDIENTE - COLOR NEGRO
                        datalistadoTodasRequerimientos.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    //SI MP SE CÚMPLE NINGUNA DE LAS CONDICIONES ANTERIORES
                    else
                    {
                        //REQUERIMIENTO DESAPROBADO - COLOR ROJO
                        datalistadoTodasRequerimientos.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //SIRVE PARA EVALUAR SI BUSCAR POR TRES FILTROS O DOS
        public void BusquedaDependiente()
        {
            if (txtBusquedaResponsable.Text == "")
            {
                MostrarRequerimientos(DesdeFecha.Value, HastaFecha.Value);
            }
            else
            {
                MostrarRequerimientosResponsable(txtBusquedaResponsable.Text, DesdeFecha.Value, HastaFecha.Value);
            }
        }

        //BÚSQUEDA DE REQUERIMIENTOS POR RESPONSABLE DE ACUERDO A LAS FECHAS SELECCIONADAS
        private void txtBusquedaResponsable_TextChanged(object sender, EventArgs e)
        {
            BusquedaDependiente();
        }

        //BÚSQUEDA DE REQUERIMEINTOS POR FECHAS
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            BusquedaDependiente();
            MostrarRequerimientosExcel(DesdeFecha.Value, HastaFecha.Value);
        }

        //BÚSQUEDA DE REQUERIMEINTOS POR FECHAS
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            BusquedaDependiente();
            MostrarRequerimientosExcel(DesdeFecha.Value, HastaFecha.Value);
        }

        //BÚSQUEDA DE REQUERIMEINTOS POR FECHAS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            BusquedaDependiente();
            MostrarRequerimientosExcel(DesdeFecha.Value, HastaFecha.Value);
        }

        //BÚSQUEDA DE REQUERIMIENTOS PEDIENTE DE ACUERDO A LAS FECHAS SELECCIONADAS
        private void btnBusquedaPendientes_Click(object sender, EventArgs e)
        {
            //
        }

        //BÚSQUEDA DE REQUERIMIENTOS APROBADOS DE ACUERDO A LAS FECHAS SELECCIONADAS
        private void btnBusquedaAprobados_Click(object sender, EventArgs e)
        {
            //;
        }

        //BÚSQUEDA DE REQUERIMEINTOS DESAPROBADOS DE ACUERDO A LAS FECHAS SELECCIONADAS
        private void btnBusquedaDesaprobado_Click(object sender, EventArgs e)
        {
            //
        }
        //------------------------------------------------------------------------------------------------------------------------------------

        //GENERACIÓN DE LOS PDFs---------------------------------------------------------------------------------------------------------------
        //GENERACIÓN DEL PDF DEL REQUERIMEINTO
        private void btnVerRequerimiento_Click(object sender, EventArgs e)
        {
            try
            {
                ClassResourses.VisualizarReporte(datalistadoTodasRequerimientos, 1, 9, new Dictionary<string, Func<int, Form>>
                {
                {"ANULADO", id => new Visualizadores.VisualizarRequerimientoDesaprobado(id) },
                { "PENDIENTE", id => new Visualizadores.VisualizarRequerimientoVenta(id) },
                { "APROBADO",id => new Visualizadores.VisualizarRequerimientoAprobado(id) }
                });
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }


            //try
            //{
            //    //SI NO HAY NINGUN REGISTRO SELECCIONADO
            //    if (datalistadoTodasRequerimientos.CurrentRow != null)
            //    {
            //        //string codigoCotizacionReporte = "0";

            //        int codigoRequerimiento = Convert.ToInt32(datalistadoTodasRequerimientos.Rows[datalistadoTodasRequerimientos.CurrentRow.Index].Cells[1].Value.ToString());

            //        //SI EL REQUERIMEINTO ESTÁ ANULADO POR EL ÁREA COMERCIAL Y YA TIENE LIQUIDACIÓN CREADA
            //        if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "ANULADO" && Convert.ToBoolean(datalistadoTodasRequerimientos.SelectedCells[11].Value.ToString()) == true)
            //        {
            //            //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO DESAPROBADO
            //            //codigoCotizacionReporte = datalistadoTodasRequerimientos.Rows[datalistadoTodasRequerimientos.CurrentRow.Index].Cells[1].Value.ToString();
            //            Visualizadores.VisualizarRequerimientoDesaprobado frm = new Visualizadores.VisualizarRequerimientoDesaprobado(codigoRequerimiento);
            //            //frm.lblCodigo.Text = codigoCotizacionReporte;
            //            //CARGAR VENTANA
            //            frm.Show();
            //        }
            //        //SI EL REQUERIMEINTO ESTÁ EN PENDIENTE
            //        else if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "PENDIENTE")
            //        {
            //            //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO GENERAL
            //            //codigoCotizacionReporte = datalistadoTodasRequerimientos.Rows[datalistadoTodasRequerimientos.CurrentRow.Index].Cells[1].Value.ToString();
            //            Visualizadores.VisualizarRequerimientoVenta frm = new Visualizadores.VisualizarRequerimientoVenta(codigoRequerimiento);
            //            //frm.lblCodigo.Text = codigoCotizacionReporte;
            //            //CARGAR VENTANA
            //            frm.Show();
            //        }
            //        //SI EL REQUERIMEINTO ESTÁ APROBADO POR EL ÁREA COMERCIAL O ESTA EN PENDIENTE
            //        else if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "APROBADO")
            //        {
            //            //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO GENERAL
            //            //codigoCotizacionReporte = datalistadoTodasRequerimientos.Rows[datalistadoTodasRequerimientos.CurrentRow.Index].Cells[1].Value.ToString();
            //            Visualizadores.VisualizarRequerimientoAprobado frm = new Visualizadores.VisualizarRequerimientoAprobado(codigoRequerimiento);
            //            //frm.lblCodigo.Text = codigoCotizacionReporte;
            //            //CARGAR VENTANA
            //            frm.Show();
            //        }
            //        //SI EL REQUERIMEINTO NO ENTRA A NINGUNA DE LAS OPCIONES ANTERIORES
            //        else
            //        {
            //            //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO DESAPROBADO
            //            //codigoCotizacionReporte = datalistadoTodasRequerimientos.Rows[datalistadoTodasRequerimientos.CurrentRow.Index].Cells[1].Value.ToString();
            //            Visualizadores.VisualizarRequerimientoDesaprobado frm = new Visualizadores.VisualizarRequerimientoDesaprobado(codigoRequerimiento);
            //            //frm.lblCodigo.Text = codigoCotizacionReporte;
            //            //CARGAR VENTANA
            //            frm.Show();
            //        }

            //        //ClassResourses.RegistrarAuditora(6, this.Name, 4, Program.IdUsuario, "Visualización de requerimiento de viaje PDF", Convert.ToInt32(codigoCotizacionReporte));
            //        ClassResourses.RegistrarAuditora(6, this.Name, 4, Program.IdUsuario, "Visualización de requerimiento de viaje PDF", Convert.ToInt32(codigoRequerimiento));

            //    }
            //    else
            //    {
            //        MessageBox.Show("Debe seleccionar un requerimiento para poder generar el PDF respectivo.", "Validación del Sistema");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            //}
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

        //GENERACIÓN DEL PDF DEL REQUERIMEINTO YA APROBADO CON CONFIRMACIÓN DE LA JEFATURA COMERCIAL
        private void datalistadoTodasRequerimientos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int codigoRequerimiento = Convert.ToInt32(datalistadoTodasRequerimientos.Rows[datalistadoTodasRequerimientos.CurrentRow.Index].Cells[1].Value.ToString());

                DataGridViewColumn currentColumn = datalistadoTodasRequerimientos.Columns[e.ColumnIndex];

                if(currentColumn.Name == "btnGenerarPdf")
                {
                    if(datalistadoTodasRequerimientos.CurrentRow != null)
                    {
                        ClassResourses.VisualizarReporte(datalistadoTodasRequerimientos,1,9,new Dictionary<string, Func<int, Form>>
                        {
                            {"ANULADO", id => new Visualizadores.VisualizarRequerimientoDesaprobado(id)},
                            {"PENDIENTE",id => new Visualizadores.VisualizarRequerimientoVenta(id)},
                            {"APROBADO", id => new Visualizadores.VisualizarRequerimientoAprobado(id)}
                        });

                        ClassResourses.RegistrarAuditora(6, this.Name, 4, Program.IdUsuario, "Visualización de requerimiento de viaje PDF", Convert.ToInt32(codigoRequerimiento));
                    }
                }

            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show(ex.Message);
            }

            //try
            //{
            //    DataGridViewColumn currentColumn = datalistadoTodasRequerimientos.Columns[e.ColumnIndex];

            //    //SI SE PRECIONA SOBRE LA COLUMNA CON EL NOMBRE SELECCIOANDO
            //    if (currentColumn.Name == "btnGenerarPdf")
            //    {
            //        if (datalistadoTodasRequerimientos.CurrentRow != null)
            //        {
            //            //string codigoCotizacionReporte = "0";

            //            int CodigoRequerimiento = Convert.ToInt32(datalistadoTodasRequerimientos.Rows[datalistadoTodasRequerimientos.CurrentRow.Index].Cells[1].Value.ToString());


            //            //SI EL REQUERIMEINTO ESTÁ APROBADO POR EL ÁREA COMERCIAL
            //            if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "ANULADO")
            //            {
            //                //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO DESAPROBADO
            //                //codigoCotizacionReporte = datalistadoTodasRequerimientos.Rows[datalistadoTodasRequerimientos.CurrentRow.Index].Cells[1].Value.ToString();
            //                Visualizadores.VisualizarRequerimientoDesaprobado frm = new Visualizadores.VisualizarRequerimientoDesaprobado(CodigoRequerimiento);
            //                //frm.lblCodigo.Text = codigoCotizacionReporte;
            //                //CARGAR VENTANA
            //                frm.Show();
            //            }
            //            //SI EL REQUERIMEINTO ESTÁ APROBADO POR EL ÁREA COMERCIAL
            //            else if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "APROBADO")
            //            {
            //                //codigoCotizacionReporte = datalistadoTodasRequerimientos.Rows[datalistadoTodasRequerimientos.CurrentRow.Index].Cells[1].Value.ToString();
            //                Visualizadores.VisualizarRequerimientoAprobado frm = new Visualizadores.VisualizarRequerimientoAprobado(CodigoRequerimiento);
            //                //frm.lblCodigo.Text = codigoCotizacionReporte;
            //                //CARGAR VENTANA
            //                frm.Show();
            //            }
            //            //SI EL REQUERIMEINTO ESTÁ PENDIENTE POR EL ÁREA COMERCIAL
            //            else if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "PENDIENTE")
            //            {
            //                //codigoCotizacionReporte = datalistadoTodasRequerimientos.Rows[datalistadoTodasRequerimientos.CurrentRow.Index].Cells[1].Value.ToString();
            //                Visualizadores.VisualizarRequerimientoVenta frm = new Visualizadores.VisualizarRequerimientoVenta(CodigoRequerimiento);
            //                //frm.lblCodigo.Text = codigoCotizacionReporte;
            //                //CARGAR VENTANA
            //                frm.Show();
            //            }
            //            //SI EL REQUERIMEINTO NO ENTRA A NINGUNA DE LAS OPCIONES ANTERIORES
            //            else
            //            {
            //                //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO DESAPROBADO
            //                //codigoCotizacionReporte = datalistadoTodasRequerimientos.Rows[datalistadoTodasRequerimientos.CurrentRow.Index].Cells[1].Value.ToString();
            //                Visualizadores.VisualizarRequerimientoDesaprobado frm = new Visualizadores.VisualizarRequerimientoDesaprobado(CodigoRequerimiento);
            //                //frm.lblCodigo.Text = codigoCotizacionReporte;
            //                //CARGAR VENTANA
            //                frm.Show();
            //            }

            //            //ClassResourses.RegistrarAuditora(6, this.Name, 4, Program.IdUsuario, "Visualización de requerimiento de viaje PDF", Convert.ToInt32(codigoCotizacionReporte));
            //            ClassResourses.RegistrarAuditora(6, this.Name, 4, Program.IdUsuario, "Visualización de requerimiento de viaje PDF", Convert.ToInt32(CodigoRequerimiento));

            //        }
            //        else
            //        {
            //            MessageBox.Show("Debe seleccionar un requerimiento para poder generar el PDF con firmas.", "Validación del Sistema");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            //}
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        //CREACIÓN DE UN NUEVO REQUERIMEINTO--------------------------------------------------------------------------------------------------
        //CARGA DEL CLIENTE SELCCIONADO AL OTRO LISTADO
        private void datalistadoClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoClientes.Columns[e.ColumnIndex];

            //SI SE PRESIONA SOBRE LA COLUMNA QUE CONTIENE LA FLECHA PARA COLOCAR LOS CLIENTES
            if (currentColumn.Name == "btnSeleccionarCliente")
            {                //SI HAY 3 O MÁS CLIENTES, YA NOS E PODRAN INGRESAR MÁS
                if (datalistadoSeleccionCliente.RowCount >= 3)
                {
                    MessageBox.Show("Solo se pueden ingresar un máximo de 3 clientes por liquidación.", "Validación del Sistema");
                }
                else
                {
                    //RECOPILACIÓN DE DATOS Y ALMACENAMIENTO
                    string codigo = datalistadoClientes.SelectedCells[1].Value.ToString();
                    string cliente = datalistadoClientes.SelectedCells[2].Value.ToString();
                    string idunidad = datalistadoClientes.SelectedCells[3].Value.ToString();
                    string unidad = datalistadoClientes.SelectedCells[4].Value.ToString();
                    string iddestino = datalistadoClientes.SelectedCells[5].Value.ToString();
                    string destino = datalistadoClientes.SelectedCells[6].Value.ToString();
                    //CARGA DE DATOS AL NUEVO LISTADO
                    datalistadoSeleccionCliente.Rows.Add(new[] { codigo, cliente, idunidad, unidad, iddestino, destino });
                }
            }
        }

        //LIMPIEZA DEL CLIENTE SELCCIONADO Y CARGADO
        private void btnBorrarSeleccionCliente_Click(object sender, EventArgs e)
        {
            //SI NO HAY CLIENTES CARGADOS
            if (datalistadoSeleccionCliente.Rows.Count > 0)
            {
                //MENSAJE DE CONFIRMACIÓN PARA BORRAR AL CLIENTE SELECCIOANDO
                DialogResult resul = MessageBox.Show("¿Seguro que desea borrar ha este cliente?.", "Validación del Sistema", MessageBoxButtons.YesNo);
                if (resul == DialogResult.Yes)
                {
                    //ACCIÓN DE ELIMINAR
                    datalistadoSeleccionCliente.Rows.Remove(datalistadoSeleccionCliente.CurrentRow);
                }
            }
            else
            {
                MessageBox.Show("No hay clientes agregados para poder borrarlos.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //CARGA DEL COLABORADOR SELCCIONADO AL OTRO LISTADO
        private void datalistadoColaboradores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoColaboradores.Columns[e.ColumnIndex];

            //SI SE PRESIONA SOBRE LA COLUMNA QUE CONTIENE LA FLECHA PARA COLOCAR LOS COLABORADORES
            if (currentColumn.Name == "btnSeleccionarColaboradores")
            {
                //RECOPILACIÓN DE DATOS Y ALMACENAMIENTO
                string codigo = datalistadoColaboradores.SelectedCells[1].Value.ToString();
                string colaborador = datalistadoColaboradores.SelectedCells[2].Value.ToString();
                //CARGA DE DATOS AL NUEVO LISTADO
                datalistadoSeleccionColaborador.Rows.Add(new[] { codigo, colaborador });
            }
        }

        //LIMPIEZA DEL COLABORADOR SELCCIONADO Y CARGADO
        private void btnBorrarSeleccionColaboradores_Click(object sender, EventArgs e)
        {
            //SI NO HAY COLABORADORES CARGADOS
            if (datalistadoSeleccionColaborador.Rows.Count > 0)
            {
                //MENSAJE DE CONFIRMACIÓN PARA BORRAR A LOS COLABORADORES SELECCIOANDO
                DialogResult resul = MessageBox.Show("¿Seguro que desea borrar ha este colaborador?.", "Validación del Sistema", MessageBoxButtons.YesNo);
                if (resul == DialogResult.Yes)
                {
                    //ACCIÓN DE ELIMINAR
                    datalistadoSeleccionColaborador.Rows.Remove(datalistadoSeleccionColaborador.CurrentRow);
                }
            }
            else
            {
                MessageBox.Show("No hay colaboradores agregados para poder borrarlos.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //HABILITAR LA VENTANA DE NUEVO REQUERIMIENTO
        private void btnNuevoRequerimiento_Click(object sender, EventArgs e)
        {
            CargarResponsables(cboResponsable);
            cboResponsable.SelectedValue = Program.IdUsuario;

            if (cboResponsable.SelectedValue == null)
            {
                MessageBox.Show("Usted no esta autorizado para generar un requerimiento de viaje, por favor pedir más información a su jefatura inmediata.", "Validación del Sistema");
            }
            else
            {
                //CARGA INICIAL DE LA VENTANA DE NUEVO REQUERIMIENTO
                lblTituloRequerimiento.Text = "Nuevo Requerimiento";
                panelNuevoRequerimiento.Visible = true;
                cboBusquedaClientes.SelectedIndex = 0;
                cboBusqeudaColaborador.SelectedIndex = 0;
                //CARGA DE USUARIO LOGEADO AL LISTADO DE COLABORADORES

                string codigoColaborador = cboResponsable.SelectedValue.ToString();
                string NombreColaborador = cboResponsable.Text;
                datalistadoSeleccionColaborador.Rows.Add(new[] { codigoColaborador, NombreColaborador });

                //CARGA DE COMBOS GENERAES NECESARIOS PARA EL NUEVO REQUERIMIENTO
                CargarResponsables(cboResponsable);
                CargarTipoMoneda(cboTipoMoneda);
                CargarVehiculosReque(cboVehiculo);

                //BLOQUEO DEL LISTADO DE TODOS LOS REQUERIMEINTOS PARA EVITAR CRUCE DE INFORMACIÓN
                datalistadoTodasRequerimientos.Enabled = false;
            }
        }

        //HABILITAR LA VENTANA DE EDIDCION DEL REQUERIMIENTO
        private void btnEditarRequerimiento_Click(object sender, EventArgs e)
        {
            //SI NO HAY REQUERIMIENTO SELECCIONADO
            if (datalistadoTodasRequerimientos.CurrentRow != null)
            {
                //CAPTURAR EL NOMBRE DE USUARIO
                string usuarioEncargado = datalistadoTodasRequerimientos.SelectedCells[5].Value.ToString();

                //SI EL USUARIO LOGEADO ES IGUAL AL USUARIO ENCARGADO DE DEL REQUERIMINTO
                if (usuarioEncargado == Program.NombreUsuarioCompleto || Program.RangoEfecto == 3 || Program.RangoEfecto == 1)
                {
                    if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "PENDIENTE")
                    {
                        int codigoRequerimeinto = Convert.ToInt32(datalistadoTodasRequerimientos.SelectedCells[1].Value.ToString());

                        //CARGA Y BUSQEUDA DE CAMPOS ESENCAILES PARA LA CARGA Y EL GUARDADO DE LA LIQUIDACION
                        CargarTipoMoneda(cboTipoMoneda);
                        CargarResponsables(cboResponsable);
                        CargarVehiculosLiqui(cboVehiculo);
                        BuscarRequerimeintoGeneral(codigoRequerimeinto);
                        BuscarRequerimeintoClientes(codigoRequerimeinto);
                        BuscarRequerimeintoColaboradores(codigoRequerimeinto);
                        BuscarRequerimeintoDetalles(codigoRequerimeinto);

                        cboBusquedaClientes.SelectedIndex = 0;
                        cboBusqeudaColaborador.SelectedIndex = 0;
                        //CARGA INICIAL DE LA VENTANA DE EDICION REQUERIMIENTO
                        lblTituloRequerimiento.Text = "Edición Requerimiento";
                        panelNuevoRequerimiento.Visible = true;

                        //CARGA DE DATOS DE LOS LISTADO AL FORMUALRIO DE INGRESO DE LIQUIDACION
                        int tipoRequerimiento = Convert.ToInt32(datalistadoBusquedaReuqerimientoGeneral.SelectedCells[2].Value.ToString());

                        if (tipoRequerimiento == 1)
                        {
                            rbNacional.Checked = true;
                            rbExterior.Checked = false;
                        }
                        else
                        {
                            rbNacional.Checked = false;
                            rbExterior.Checked = true;
                        }

                        //DATOS GENERALES DEL REQUERIMEINTO
                        datatimeFechaRequerimientoLiquidacion.Value = Convert.ToDateTime(datalistadoBusquedaReuqerimientoGeneral.SelectedCells[1].Value.ToString());
                        cboResponsable.SelectedValue = datalistadoBusquedaReuqerimientoGeneral.SelectedCells[5].Value.ToString();
                        cboResponsable.SelectedValue = datalistadoBusquedaReuqerimientoGeneral.SelectedCells[5].Value.ToString();
                        cboVehiculo.SelectedValue = datalistadoBusquedaReuqerimientoGeneral.SelectedCells[6].Value.ToString();
                        datetimeDesde.Value = Convert.ToDateTime(datalistadoBusquedaReuqerimientoGeneral.SelectedCells[7].Value.ToString());
                        datetiemHasta.Value = Convert.ToDateTime(datalistadoBusquedaReuqerimientoGeneral.SelectedCells[8].Value.ToString());
                        txtMotivoViaje.Text = datalistadoBusquedaReuqerimientoGeneral.SelectedCells[9].Value.ToString();
                        txtItinerarioViaje.Text = datalistadoBusquedaReuqerimientoGeneral.SelectedCells[10].Value.ToString();
                        cboTipoMoneda.SelectedValue = datalistadoBusquedaReuqerimientoGeneral.SelectedCells[12].Value.ToString();
                        lblTipoMoneda.Text = datalistadoBusquedaReuqerimientoGeneral.SelectedCells[13].Value.ToString();
                        txtSubTotal.Text = datalistadoBusquedaReuqerimientoGeneral.SelectedCells[11].Value.ToString();


                        //DATOS Y CLIENTES DEL REQUERIMEINTO
                        foreach (DataGridViewRow row in datalistadoBusquedaRequerimientoCLientes.Rows)
                        {
                            string codigoCliente = row.Cells[2].Value.ToString();
                            string ClienteDes = row.Cells[3].Value.ToString();
                            string codigoUnidad = row.Cells[4].Value.ToString();
                            string UnidadDes = row.Cells[5].Value.ToString();
                            string codigoDepartamento = row.Cells[6].Value.ToString();
                            string DepartamentoDes = row.Cells[7].Value.ToString();

                            datalistadoSeleccionCliente.Rows.Add(new[] { codigoCliente, ClienteDes, codigoUnidad, UnidadDes, codigoDepartamento, DepartamentoDes });
                        }
                        //DATOS Y COLABORADORES DEL REQUERIMEINTO
                        foreach (DataGridViewRow row in datalistadoBusquedaRequerimeintoColaboradores.Rows)
                        {
                            string codigoVendedor = row.Cells[2].Value.ToString();
                            string VendedorDes = row.Cells[3].Value.ToString();

                            datalistadoSeleccionColaborador.Rows.Add(new[] { codigoVendedor, VendedorDes });
                        }

                        //DATOS Y DETALLES DEL REQUERIMEINTO
                        foreach (DataGridViewRow row in dataliostadoBusquedaRequerimientoDetalles.Rows)
                        {
                            string fechaRequerimeintoDetalle = row.Cells[2].Value.ToString();
                            string conbustible = row.Cells[3].Value.ToString();
                            string hospedaje = row.Cells[4].Value.ToString();
                            string viatico = row.Cells[5].Value.ToString();
                            string peaje = row.Cells[6].Value.ToString();
                            string movilidad = row.Cells[7].Value.ToString();
                            string otros = row.Cells[8].Value.ToString();
                            string subTotal = row.Cells[9].Value.ToString();

                            datalistadoPresupuestoViaje.Rows.Add(new[] { null, fechaRequerimeintoDetalle, conbustible, hospedaje, viatico, peaje, movilidad, otros, subTotal });
                        }

                        datalistadoTodasRequerimientos.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("El requerimiento que desea editar se encuentra en un estado diferente a pendiente.", "Validación del Sistema");
                    }
                }
                else
                {
                    MessageBox.Show("Solo puede hacer el proceso el responsable de este.", "Validación del Sistema");
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un requerimiento para poder editarlo.", "Validación del Sistema");
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        //PRIMER PASO DEL INGRESO DE UN NUEVO REQUERIMIENTO----------------------------------------------------------------------------------
        //CARGA Y GENERACIÓN DEL RANGO DE DIAS SELECCIOANDOS
        private void btnEnviarHbilitar_Click(object sender, EventArgs e)
        {
            //SI NO HAY COLABORADORES INGRESADOS EN EL LISTADI
            if (datalistadoSeleccionColaborador.Rows.Count < 0)
            {
                MessageBox.Show("Debe ingresar colaboradores para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                //CAPTURAR AL USUARIO SELECCIONADO COMO ENCARGADO DEL REQUERIMIENTO
                string responsable = cboResponsable.Text;
                //RECORRER TODOS LOS USUARIOS PARA VERIFICAR LA EXISTENCIA DE ESTE EN EL LISTADO
                foreach (DataGridViewRow row in datalistadoSeleccionColaborador.Rows)
                {
                    string colaboradores = row.Cells[1].Value.ToString();

                    if (colaboradores == responsable)
                    {
                        estadoResopnsable = true;
                    }
                }
            }

            //VALIDAR LA FECHA DE INICIO DEL REQUERIMIENTO CON MI FECHA ACTUAL DE MI REQUERIMIENTO
            if (datetimeDesde.Text == datatimeCalculador.Text || datetimeDesde.Value <= datatimeCalculador.Value)
            {
                //SI NO HAY MOTIVO DE VIAJE O ITINERARIO O EL RESPONSABLE NO ESTA SELECCIONADO
                if (txtMotivoViaje.Text == "" || txtItinerarioViaje.Text == "" || estadoResopnsable == false)
                {
                    MessageBox.Show("Debe ingresar un motívo y un itinerario o el responsable debe estar colocado en la lista de colaboradores.", "Validación del Sistema");
                }
                //GENERACIÓN DE FILAS PARA PODER AGREGAR MIS MONTOS DE ACUEDO AL RANGO DE DIAS SELCCIONADO
                else
                {
                    DataGridViewRow fila = new DataGridViewRow();
                    fila.CreateCells(datalistadoPresupuestoViaje);
                    fila.Cells[1].Value = this.datatimeCalculador.Text;
                    datalistadoPresupuestoViaje.Rows.Add(fila);
                    // PARA RESTAR LA FECHA DE dtCalculo DE 1 EN 1 POR EL txtNunFecha
                    datatimeCalculador.Value = datatimeCalculador.Value.Subtract(TimeSpan.FromDays(Convert.ToDouble(txtNumFecha.Text)));
                    //LÍNEA PARA ORDENAR LAS COLUMNAS DE ACUERDO A LAS FECHAS
                    //direccion(datalistadoPresupuestoViaje);
                }
            }
        }

        //ASIGNO EL VALOR DE LA FECHA FINAL A UN DATATIME ESCONDIDO PARA QUE HAGA LOS CALCULOS
        private void datetiemHasta_ValueChanged(object sender, EventArgs e)
        {
            datatimeCalculador.Value = datetiemHasta.Value;
        }

        //CÓDIGO QUE NO SE BIEN PARA QUE SIRVE PERO CREO QUE ES PARA REORDENAR LAS FILAS
        public void direccion(DataGridView dgv)
        {
            dgv.Sort(dgv.Columns[1], ListSortDirection.Ascending);
        }

        //VALIDACIÓN DEL LISTADO DE PRESUPUESTO DE MI NUEVO REQUERIMIENTO
        private void datalistadoPresupuestoViaje_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //VARIABLES PARA ALMACENAR LOS DATOS
            decimal a;
            decimal b;
            decimal c;
            decimal d;
            decimal f;
            decimal g;
            decimal total;
            //ALMACENAMIENTO DE DATOS PARA LA VALIDACIÓN
            //RECORRIDO DE MI LISTADO PARA ALMACENAR LAS DIFERENTES COLUMNAS
            DataGridViewRow row = (DataGridViewRow)datalistadoPresupuestoViaje.Rows[e.RowIndex];
            //ALMACENAMIENTOS DE COLUMNAS
            a = Convert.ToDecimal(row.Cells[2].Value);
            b = Convert.ToDecimal(row.Cells[3].Value);
            c = Convert.ToDecimal(row.Cells[4].Value);
            d = Convert.ToDecimal(row.Cells[5].Value);
            f = Convert.ToDecimal(row.Cells[6].Value);
            g = Convert.ToDecimal(row.Cells[7].Value);

            //VALIDACIÓN DE COMBUSTIBLE 
            if (row.Cells[2].Value == DBNull.Value)
            {
                //REINICIO DE CAMPO
                a = Convert.ToDecimal("0.00");
            }
            else
            {
                //CAPTURA DEL VALOR
                a = Convert.ToDecimal(row.Cells[2].Value);
            }

            //VALIDACIÓN DE HOSPEDAJE
            if (row.Cells[3].Value == DBNull.Value)
            {
                //REINICIO DE CAMPO
                b = Convert.ToDecimal("0.00");
            }
            else
            {
                //CAPTURA DEL VALOR
                b = Convert.ToDecimal(row.Cells[3].Value);
            }

            //VALIDACIÓN DE VIÁTICOS
            if (row.Cells[4].Value == DBNull.Value)
            {
                //REINICIO DE CAMPO
                c = Convert.ToDecimal("0.00");
            }
            else
            {
                //CAPTURA DEL VALOR
                c = Convert.ToDecimal(row.Cells[4].Value);
            }

            //VALIDACIÓN DE PEAJES
            if (row.Cells[5].Value == DBNull.Value)
            {
                //REINICIO DE CAMPO
                d = Convert.ToDecimal("0.00");
            }
            else
            {
                //CAPTURA DEL VALOR
                d = Convert.ToDecimal(row.Cells[5].Value);
            }

            //VALIDACIÓN DE MOVILIDAD
            if (row.Cells[6].Value == DBNull.Value)
            {
                //RENICIO DE CAMPO
                f = Convert.ToDecimal("0.00");
            }
            else
            {
                //CAPTURA DEL VALOR
                f = Convert.ToDecimal(row.Cells[6].Value);
            }

            //VALIDACIÓN DE OTROS
            if (row.Cells[7].Value == DBNull.Value)
            {
                //REINICIO DE CAMPO
                g = Convert.ToDecimal("0.00");
            }
            else
            {
                //CAPTURA DEL VALOR
                g = Convert.ToDecimal(row.Cells[7].Value);
            }

            //SUMA DE LOS CAMPOS PARA DARLE EL VALOR AL SUBTOTAL
            total = a + b + c + d + f + g;
            //REORDENAMIENTO DE VALORES INGRESADOS A CADA CAMPO
            row.Cells[2].Value = String.Format("{0:#,0.00}", a);
            row.Cells[3].Value = String.Format("{0:#,0.00}", b);
            row.Cells[4].Value = String.Format("{0:#,0.00}", c);
            row.Cells[5].Value = String.Format("{0:#,0.00}", d);
            row.Cells[6].Value = String.Format("{0:#,0.00}", f);
            row.Cells[7].Value = String.Format("{0:#,0.00}", g);
            row.Cells[8].Value = String.Format("{0:#,0.00}", total);
            //LAMADA AL MÉTODO DE SUBTOTAL
            SubTotal(datalistadoPresupuestoViaje);
        }

        //METODO PARA HAYAR EL TOTAL DEL REQUERIEMINTO
        public void SubTotal(DataGridView dgv)
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
            txtSubTotal.Text = String.Format("{0:#,0.00}", subtotal);
        }

        //METODO PARA HAYAR EL SUBTOTAL LIQUIDACIÍN
        public void SubTotalLiquidacion(DataGridView dgv)
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

            txtTotaLiquidaciones.Text = String.Format("{0:#,0.00}", subtotal);
        }

        //METODO PARA HAYAR EL SALDO Y LIQUIDACIÓN
        public void saldoLiquidacion()
        {
            decimal subtotal;
            decimal adelanto;
            decimal saldo;

            subtotal = System.Convert.ToDecimal(txtTotaLiquidaciones.Text);
            adelanto = System.Convert.ToDecimal(txtAdelantoLiquidaciones.Text);
            saldo = subtotal - adelanto;

            txtSaldoLiquidaciones.Text = String.Format("{0:#,0.00}", saldo);
        }

        //BORRAR UNA FILA DEL PRESUPUESTO GENERADO
        private void btnBorrarPresupuesto_Click(object sender, EventArgs e)
        {
            //SI NO HAY PRESUPUESTO DE VIAJE CARGADOS
            if (datalistadoPresupuestoViaje.Rows.Count > 0)
            {
                //ACCIÓN DE ELIMINAR
                datalistadoPresupuestoViaje.Rows.Remove(datalistadoPresupuestoViaje.CurrentRow);
                //CARGA Y RECALCULO DEL PRESUPUESTO
                SubTotal(datalistadoPresupuestoViaje);
            }
            else
            {
                MessageBox.Show("No hay registro en el presupuesto para poder remover.", "Validación del Sistema");
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------

        //PROCESO DE GUARDADO DEL REQUERIMEINTO---------------------------------------------------------------------------------------------
        //ACCIÓN DE GUARDAR UN NUEVO REQUERIMIENTO
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (lblTituloRequerimiento.Text == "Nuevo Requerimiento")
            {
                //INSERTAR NUEVO REQUERIMIENTO
                //SI NO SE HA SELECCIONADO NINGUNA OPCIÓN
                if (rbNacional.Checked == false && rbExterior.Checked == false)
                {
                    MessageBox.Show("Por favor, seleccione el tipo de requerimiento.", "Validación del Sistema");
                }
                else
                {
                    //SI NO HAY CLIENTES O COLABORADORES AGREGADOS
                    if (datalistadoSeleccionCliente.RowCount == 0 || datalistadoSeleccionColaborador.RowCount == 0)
                    {
                        MessageBox.Show("Por favor, seleccione a un cliente o ha un colaborador.", "Validación del Sistema");
                    }
                    else
                    {
                        //SI NO HAY NINGUNA FILA DE PRESUPUESTO
                        if (datalistadoPresupuestoViaje.RowCount == 0)
                        {
                            MessageBox.Show("Por favor, debe colocar el presupuesto para el viaje.", "Validación del Sistema");
                        }
                        else
                        {
                            //SI NO HAY MOTIVO DE VIAJE O ITINERARIO
                            if (txtMotivoViaje.Text == "" || txtItinerarioViaje.Text == "")
                            {
                                MessageBox.Show("Por favor, debe ingresar el motivo o itinerario del viaje.", "Validación del Sistema");
                            }
                            else
                            {
                                //SI EL SUBTOTAL NO ESTA CARGADO
                                if (txtSubTotal.Text == "")
                                {
                                    MessageBox.Show("Por favor, debe ingresar valores al presupuesto del viaje.", "Validación del Sistema");
                                }
                                else
                                {
                                    //SI LA FECHA DESDE ES MENOR A LA FECHA ACTUAL DE CREACIÓN
                                    if (datetimeDesde.Value.Date < datatimeFechaRequerimiento.Value.Date)
                                    {
                                        //PANEL PARA INGRESAR OBSERVACIONES
                                        panelObservacionesRequeAtrasado.Visible = true;
                                    }
                                    else
                                    {
                                        //SI TODO ESTA OK
                                        GuardarRequerimiento(0, "");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (rbNacional.Checked == false && rbExterior.Checked == false)
                {
                    MessageBox.Show("No se ha seleccionado el tipo de requerimiento correctamente.", "Validación del Sistema");
                }
                else
                {
                    if (datalistadoSeleccionColaborador.RowCount == 0 || datalistadoSeleccionCliente.RowCount == 0)
                    {
                        MessageBox.Show("No se han cargado los clientes o colaboradores correctamnete.", "Validación del Sistema");
                    }
                    else
                    {
                        if (datalistadoPresupuestoViaje.RowCount == 0)
                        {
                            MessageBox.Show("No se han cargado los detalles del requerimiento correctamente.", "Validación del Sistema");
                        }
                        else
                        {
                            if (txtMotivoViaje.Text == "" || txtItinerarioViaje.Text == "")
                            {
                                MessageBox.Show("No se ha cargado el itinerario o motivo del requerimiento.", "Validación del Sistema");
                            }
                            else
                            {
                                if (txtSubTotal.Text == "")
                                {
                                    MessageBox.Show("No se ha cargado el sub-total del requerimiento.", "Validación del Sistema");
                                }
                                else
                                {
                                    EditarRequerimiento();
                                }
                            }
                        }
                    }
                }
            }
        }

        //SI ESTA ATRAZADO, MUESTRA Y PUEDE SEGUIR CON EL INGRESO CONTEMPLANDO UN MENSAJHE DE JSUTIFICAICÓN
        private void btnProcederGuardatoObservaciones_Click(object sender, EventArgs e)
        {
            //SI NO HAY JUSTIFICACIÓN INGRESADA
            if (txtRazononObservaciones.Text == "")
            {
                MessageBox.Show("Debe ingresar un mensaje que justifique el requerimiento fuera de fecha.", "Validación del Sistema");
            }
            else
            {
                GuardarRequerimiento(1, txtRazononObservaciones.Text);
            }
        }

        //SI ESTA ATRASADO, MUESTRA Y PUEDE RETROCEDER CON EL OBJETIVO DE NO CONTINUAR EL INGRESO
        private void btnRetrocederGuardadoObservaciones_Click(object sender, EventArgs e)
        {
            //LIMPIAR Y CERRAR
            txtRazononObservaciones.Text = "";
            panelObservacionesRequeAtrasado.Visible = false;
        }

        //METODO PARA INGRESAR CON OBSERVACIONES Y SIN OBSERVACIONES
        public void GuardarRequerimiento(int estadoAtrasado, string mensajeAtrasado)
        {
            try
            {
                //CONFIRMACIÓN PARA PODER GUARDAR EL REQUERIMEINTO
                DialogResult boton = MessageBox.Show("¿Realmente desea guardar este requerimiento?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    //PROCEDIMEINTO ALMACENADO PARA HACER LA ACCIÓN DE GUARDAR
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("RequerimientoViaje_Insertar", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    codigoRequerimeinto();
                    //INGRESO DEL ENCABEZADO DEL REQUERIMIENTO
                    cmd.Parameters.AddWithValue("@idRequerimientoVenta", numeroRequerimiento);
                    cmd.Parameters.AddWithValue("@fechaRequerimiento", datatimeFechaRequerimiento.Value);
                    cmd.Parameters.AddWithValue("@fechaInicio", datetimeDesde.Value);
                    cmd.Parameters.AddWithValue("@fechaTermino", datetiemHasta.Value);

                    if (rbNacional.Checked == true)
                    {
                        cmd.Parameters.AddWithValue("@nacional", 1);
                        cmd.Parameters.AddWithValue("@extranjeto", 0);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@nacional", 0);
                        cmd.Parameters.AddWithValue("@extranjeto", 1);
                    }

                    cmd.Parameters.AddWithValue("@motivoVisita", txtMotivoViaje.Text);
                    cmd.Parameters.AddWithValue("@idvendedor", cboResponsable.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@idvehiculo", cboVehiculo.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@itinerarioViaje", txtItinerarioViaje.Text);
                    cmd.Parameters.AddWithValue("@total", txtSubTotal.Text);
                    cmd.Parameters.AddWithValue("@estadoAtrasado", estadoAtrasado);
                    cmd.Parameters.AddWithValue("@mensajeAtrasado", mensajeAtrasado);
                    cmd.Parameters.AddWithValue("@idTipoMoneda", cboTipoMoneda.SelectedValue.ToString());
                    CargarJefaturaActual();
                    cmd.Parameters.AddWithValue("@idJefatura", idJefatura);
                    cmd.Parameters.AddWithValue("@aliasCargoComercial", Program.Alias);
                    cmd.Parameters.AddWithValue("@aliasCargoJefatura", alias);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    //INGRESO DE LOS DETALLES DEL VAIJE/PRESUPEUSTO CON UN FOREACH
                    foreach (DataGridViewRow row in datalistadoPresupuestoViaje.Rows)
                    {
                        //PROCEDIMIENTO ALMACENADO PARA GUARDAR EL PRESUPUESTO DEL VIAJE
                        con.Open();
                        cmd = new SqlCommand("RequerimientoViaje_InsertarDetalles", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idRequerimiento", numeroRequerimiento);
                        cmd.Parameters.AddWithValue("@fechaRequerimeinto", Convert.ToString(row.Cells[1].Value));
                        cmd.Parameters.AddWithValue("@combustible", Convert.ToString(row.Cells[2].Value));
                        cmd.Parameters.AddWithValue("@hospedaje", Convert.ToString(row.Cells[3].Value));
                        cmd.Parameters.AddWithValue("@viatico", Convert.ToString(row.Cells[4].Value));
                        cmd.Parameters.AddWithValue("@peaje", Convert.ToString(row.Cells[5].Value));
                        cmd.Parameters.AddWithValue("@movilidad", Convert.ToString(row.Cells[6].Value));
                        cmd.Parameters.AddWithValue("@otros", Convert.ToString(row.Cells[7].Value));
                        cmd.Parameters.AddWithValue("@subtotal", Convert.ToString(row.Cells[8].Value));
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    //INGRESO DE LOS CLIENTES Y SUS DATOS ANEXOS CON UN FOREACH
                    foreach (DataGridViewRow row in datalistadoSeleccionCliente.Rows)
                    {
                        //SELECCIONAMOS LOS CÓDIGOS QUE TIENE NUESTRO LISTADO
                        int codigoDetalleCliente = Convert.ToInt32(row.Cells["idCliente"].Value);
                        int codigoDetalleUnidad = Convert.ToInt32(row.Cells["IdUnidad"].Value);
                        string codigoDetalleDestino = Convert.ToString(row.Cells["IdDestino"].Value);

                        //PROCEDIMIENTO ALMACENADO PARA GUARDAR A LOS CLIENTES Y SUS DATOS ANEXOS
                        con.Open();
                        cmd = new SqlCommand("RequerimientoViaje_InsertarDetalleCliente", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idRequerimiento", numeroRequerimiento);
                        cmd.Parameters.AddWithValue("@idClienteDetalle", codigoDetalleCliente);
                        cmd.Parameters.AddWithValue("@idUnidadDetalle", codigoDetalleUnidad);
                        cmd.Parameters.AddWithValue("@codigoDestinoDetalle", codigoDetalleDestino);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    //INGRESO DE LOS COLABORADORES O VENDEDORES CON UN FOREACH
                    foreach (DataGridViewRow row in datalistadoSeleccionColaborador.Rows)
                    {
                        //SELECCIONAMOS LOS CÓDIGOS QUE TIENE NUESTRO LISTADO
                        int codigoDetalleColaborador = Convert.ToInt32(row.Cells["idvendedor"].Value);

                        //PROCEDIMIENTO ALMACENADO PARA GUARDAR A LOS VENDEODRES O COLABORADORES
                        con.Open();
                        cmd = new SqlCommand("RequerimientoViaje_InsertarDetalleVendedores", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idRequerimiento", numeroRequerimiento);
                        cmd.Parameters.AddWithValue("@idvendedordetalle", codigoDetalleColaborador);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    MessageBox.Show("Se registró el requerimiento exitosamente.", "Validación del Sistema");

                    //REINICIAR FORMULARIO DE INGRESO DE REQUERIMIENTO
                    panelNuevoRequerimiento.Visible = false;
                    panelObservacionesRequeAtrasado.Visible = false;
                    txtBusqeudaCliente.Text = "";
                    txtBusquedaColaborador.Text = "";

                    datalistadoSeleccionCliente.Rows.Clear();
                    datalistadoSeleccionColaborador.Rows.Clear();
                    datalistadoPresupuestoViaje.Rows.Clear();
                    rbNacional.Checked = true;
                    rbExterior.Checked = false;
                    txtMotivoViaje.Text = "";
                    txtItinerarioViaje.Text = "";
                    txtSubTotal.Text = "";

                    datalistadoClientes.DataSource = null;
                    datalistadoColaboradores.DataSource = null;
                    datalistadoTodasRequerimientos.Enabled = true;

                    BusquedaDependiente();
                    ClassResourses.RegistrarAuditora(1, this.Name, 4, Program.IdUsuario, "Guardar requerimiento de viaje", numeroRequerimiento);
                }
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show(ex.Message, "Error en el servidor.");
            }
        }

        //SALIR DEL NUEVO REQUERIMIENTO
        private void btnSalir_Click(object sender, EventArgs e)
        {
            //REINICIAR FORMULARIO DE INGRESO DE REQUERIMIENTO
            panelNuevoRequerimiento.Visible = false;
            txtNumFecha.Text = "1";
            datatimeCalculador.Value = datetiemHasta.Value;
            txtMotivoViaje.Text = "";
            txtItinerarioViaje.Text = "";

            txtBusquedaColaborador.Text = "";
            txtBusqeudaCliente.Text = "";
            datalistadoClientes.DataSource = null;
            datalistadoColaboradores.DataSource = null;
            datalistadoSeleccionCliente.Rows.Clear();
            datalistadoSeleccionColaborador.Rows.Clear();
            datalistadoPresupuestoViaje.Rows.Clear();

            datalistadoTodasRequerimientos.Enabled = true;
        }

        //------------------------------------------------------------------------------------------------------------------------------

        //APROBAR Y DESAPBROBAR REQUERIMEINTOS POR LA JEFATURA--------------------------------------------------------------------------
        //APROBAR REQUERIMIENTO
        private void btnAprobarRequerimiento_Click(object sender, EventArgs e)
        {
            try
            {
                //SI NO SE HA SELECCIONADO NINGUN REQUERIMIENTO
                if (datalistadoTodasRequerimientos.CurrentRow != null)
                {
                    //MENSAJE DE CONFIRMACIÓN PARA LA APROBACIÓN DEL REQUERIMIENTO
                    DialogResult boton = MessageBox.Show("¿Realmente desea aprobar este requerimiento?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                    if (boton == DialogResult.OK)
                    {
                        //RECOPILACIÓN DE LOS DATOS PARA LA VALIDACIÓN
                        int idRequerimiento = Convert.ToInt32(datalistadoTodasRequerimientos.SelectedCells[1].Value.ToString());
                        string estadoJefatura = datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString();
                        string estadoContabilidad = datalistadoTodasRequerimientos.SelectedCells[10].Value.ToString();
                        int estadoAtrasado = Convert.ToInt32(datalistadoTodasRequerimientos.SelectedCells[12].Value.ToString());
                        string mensajeAtrasado = datalistadoTodasRequerimientos.SelectedCells[13].Value.ToString();

                        //SI ESTADO DE COMERCIAL ESTA APROBADO Y EL ESTADO DE CONTABILIDAD ESTA ATENDIDO
                        if (estadoJefatura == "APROBADO" && estadoContabilidad == "ATENDIDO")
                        {
                            MessageBox.Show("Este requerimiento ya está aprobado por las diferentes áreas.", "Validación del Sistema");
                        }
                        else
                        {
                            //SI EL USUARIO A QUIEN VA A REALIZAR ESTA ACCIÓN PERTENECE AL ÁREA COMERCIAL O AL ÁREA DE GENRENCIA (ADMINISTRADOR)
                            if (Program.AreaUsuario == "Comercial" || Program.AreaUsuario == "Administrador")
                            {
                                //SI EL ESTADO DE JEFATURA COMERCIAL ESTA APROBADO
                                if (estadoJefatura == "APROBADO")
                                {
                                    MessageBox.Show("Este requerimiento ya ha sido aprobado por la jefatura del área comercial.", "Validación del Sistema");
                                }
                                //SI EL ESTADO DE JEFATURA COMERCIAL ESTA ANULADO O EL ESTADO DE CONTABILIDAD ESTA ANULADA
                                else if (estadoJefatura == "ANULADO" || estadoContabilidad == "ANULADO")
                                {
                                    MessageBox.Show("Este requerimiento ha sido desaprobado por el área comercial o el área contable.", "Validación del Sistema");
                                }
                                else
                                {
                                    //SI EL REQUERIMIENTO ESTA ATRASADO
                                    if (estadoAtrasado == 1)
                                    {
                                        //SE MUESTRA EL PANEL DE OBERVACIONES Y EL MENSAJE
                                        panelAprobacionDetalleFueraFecha.Visible = true;
                                        txtAprobacionRazonesFueraFecha.Text = mensajeAtrasado;
                                    }
                                    else
                                    {
                                        //SI TODO ESTA OK
                                        AprobacionJefaturas(idRequerimiento, 2);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un requerimiento para poder aprobarlo.", "Validación del Sistema");
                }
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show(ex.Message, "Error en el servidor.");
            }
        }

        //SI EL REQUERIMEINTO ESTA ATRASADO, MUESTRA UN MENSAJE CON LA JUSTIFICACIÓN Y ESTA ES LA ACCIÓN PARA CONFIRMAR LA APROBACIÓN
        private void btnAprobacionAprobacion_Click(object sender, EventArgs e)
        {
            int idRequerimiento = Convert.ToInt32(datalistadoTodasRequerimientos.SelectedCells[1].Value.ToString());

            AprobacionJefaturas(idRequerimiento, 2);
            panelAprobacionDetalleFueraFecha.Visible = false;
        }

        //MÉTODO PARA SALIR DE LA APROBACIÓN DE UN REQUERIMIENTO ATRASADO
        private void btnAprobacionAtrasadoSalir_Click(object sender, EventArgs e)
        {
            panelAprobacionDetalleFueraFecha.Visible = false;
        }

        //METODO DE APROBACIÓN REQUERIMEINTO
        public void AprobacionJefaturas(int idRequerimiento, int estadoJefatura)
        {
            CargarJefaturaActual();

            try
            {
                //PROCEDIMEINTO ALMACENADO PARA HACER LA APROBACIÓN DEL REQUERIMEINTO
                SqlConnection con = new SqlConnection();
                SqlCommand cmd = new SqlCommand();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                cmd = new SqlCommand("RequerimientoVenta_CambioEstado", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idRequerimiento", idRequerimiento);
                cmd.Parameters.AddWithValue("@estado", estadoJefatura);
                cmd.Parameters.AddWithValue("@idJefatura", idJefatura);
                cmd.Parameters.AddWithValue("@aliasJefatura", alias);
                cmd.Parameters.AddWithValue("@mensajeAnulacion", DBNull.Value);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Requerimiento aprobado exitosamente.", "Validación del Sistema");
                BusquedaDependiente();

                ClassResourses.RegistrarAuditora(3, this.Name, 4, Program.IdUsuario, "Aprobar requerimiento de viaje", idRequerimiento);
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show(ex.Message);
            }
        }

        //ANULACIÓN DE REQUERIMEINTO
        private void btnDesaprobaRequerimiento_Click(object sender, EventArgs e)
        {
            //SI NO HAY UN REQUERIMIENTO SELECCIOANOD
            if (datalistadoTodasRequerimientos.CurrentRow != null)
            {
                panleAnulacion.Visible = true;
            }
            else
            {
                MessageBox.Show("Debe seleccionar un requerimiento para poder anularlo.", "Validación del Sistema");
            }
        }

        private void btnProcederAnulacion_Click(object sender, EventArgs e)
        {
            //SI NO HAY UNA JUSTIFICACIÓN 
            if (txtJustificacionAnulacion.Text != "")
            {
                //MENSAJE DE CONFIRMACIÓN PARA LA ANLACIÓN DE UN REQUERIMIENTO
                DialogResult boton = MessageBox.Show("¿Realmente desea anular este requerimiento?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    //RECOPILACIÓN DE VARIABLES
                    int idRequerimiento = Convert.ToInt32(datalistadoTodasRequerimientos.SelectedCells[1].Value.ToString());
                    string estadoReque = datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString();
                    bool estadoLiquidacion = Convert.ToBoolean(datalistadoTodasRequerimientos.SelectedCells[11].Value.ToString());

                    //SI EL ESTADO DE LIQUIDACIÓN DE MI REQUERIMIENTO ES TRUE
                    if (estadoLiquidacion == true)
                    {
                        MessageBox.Show("Este requerimiento tiene una liquidación hecha, por favor anular por la liquidación o en su defecto por el acta.", "Validación del Sistema");
                        txtJustificacionAnulacion.Text = "";
                        panleAnulacion.Visible = false;
                    }
                    else if (estadoReque == "ANULADO")
                    {
                        MessageBox.Show("Este requerimiento ya se encuentra anulado.", "Validación del Sistema");
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
                            cmd = new SqlCommand("RequerimientoVenta_CambioEstado", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idRequerimiento", idRequerimiento);
                            cmd.Parameters.AddWithValue("@estado", 0);
                            cmd.Parameters.AddWithValue("@idJefatura", idJefatura);
                            cmd.Parameters.AddWithValue("@aliasJefatura", alias);
                            cmd.Parameters.AddWithValue("@mensajeAnulacion", txtJustificacionAnulacion.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Requerimiento anulado exitosamente.", "Validación del Sistema");

                            BusquedaDependiente();
                            panleAnulacion.Visible = false;
                            txtJustificacionAnulacion.Text = "";

                            ClassResourses.RegistrarAuditora(2, this.Name, 4, Program.IdUsuario, "Anular requerimiento de viaje", idRequerimiento);

                            Enviar("vcornejo@arenassrl.com.pe", "CORREO AUTOMATIZADO - ANULACIÓN DEL REQUERIMIENTO N°. " + idRequerimiento, "Correo de verificación de anulación de un requerimiento por parte del usuario '" + Program.UnoNombreUnoApellidoUsuario + "' el la fecha siguiente: " + DateTime.Now + ". Por favor no responder.");
                            Enviar("jhoalexxxcc@gmail.com", "CORREO AUTOMATIZADO - ANULACIÓN DEL REQUERIMIENTO N°. " + idRequerimiento, "Correo de verificación de anulación de un requerimiento por parte del usuario '" + Program.UnoNombreUnoApellidoUsuario + "' el la fecha siguiente: " + DateTime.Now + ". Por favor no responder.");
                        }
                        catch (Exception ex)
                        {
                            ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
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

        //MÉTODO PARA PODER LIBERAR UN REQUERIMINTO FUERA DE FECHA
        private void btnLiberarRequerimiento_Click(object sender, EventArgs e)
        {
            //SI NO HAY REQUERMIENTOS SELECCIONADOS
            if (datalistadoTodasRequerimientos.CurrentRow != null)
            {
                //RECOPILACIÓN DE VARIABLES
                int idRequerimiento = Convert.ToInt32(datalistadoTodasRequerimientos.SelectedCells[1].Value.ToString());
                int estadoHabilitadoJefatura = Convert.ToInt32(datalistadoTodasRequerimientos.SelectedCells[14].Value.ToString());

                //MENSAJE DE CONFIRMACIÓN PARA LA LIBERACIÓN DE UN REQUERIMIENTO
                DialogResult boton = MessageBox.Show("¿Realmente desea liberar este requerimiento?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        //SI EL ESTADO DE HABILITACIÓN DE LA JEFATURA ES IGUAL A 1
                        if (estadoHabilitadoJefatura == 1)
                        {
                            MessageBox.Show("Este requerimiento ya ha sido liberado.", "Validación del Sistema");
                        }
                        else
                        {
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("RequerimientoVenta_CambiarEstadoHabilitacion", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idRequerimiento", idRequerimiento);
                            cmd.ExecuteNonQuery();
                            con.Close();
                            BusquedaDependiente();

                            MessageBox.Show("Requerimiento liberado exitosamente.", "Validación del Sistema");
                            ClassResourses.RegistrarAuditora(14, this.Name, 4, Program.IdUsuario, "Liberar requerimiento de viaje", idRequerimiento);
                        }
                    }
                    catch (Exception ex)
                    {
                        ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un requerimiento para poder liberarlo.", "Validación del Sistema");
            }
        }

        private void btnOpcionExportaciones_Click(object sender, EventArgs e)
        {
            if (panelExportacionOpciones.Visible == false)
            {
                panelExportacionOpciones.Visible = true;
            }
            else
            {
                panelExportacionOpciones.Visible = false;
            }
        }

        //GENERACIÓN DE LA LIQUIDACIÓN - PROCESOS--------------------------------------------------------------------------
        //CARGA Y BÚSQUEDA DE DATOS
        //CARGAR PARTE GENERAL DEL REQUERIEINTO
        public void BuscarRequerimeintoGeneral(int codigoRequerimiento)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarRequerimeintoVentaPorCodigo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigo", codigoRequerimiento);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoBusquedaReuqerimientoGeneral.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //CARGA DE CLIENTES DEL REQUERIMIENTO
        public void BuscarRequerimeintoClientes(int codigoRequerimiento)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarRequerimeintoVentaPorCodigoClientes", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigo", codigoRequerimiento);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoBusquedaRequerimientoCLientes.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //CARGA DE COLABORADORES DEL REQUERIMIETNO
        public void BuscarRequerimeintoColaboradores(int codigoRequerimiento)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarRequerimeintoVentaPorCodigoColaboradores", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigo", codigoRequerimiento);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoBusquedaRequerimeintoColaboradores.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //CARGA DE DETALLES DEL REQUERIMEINTO
        public void BuscarRequerimeintoDetalles(int codigoRequerimiento)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarRequerimeintoVentaPorCodigoDetalles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigo", codigoRequerimiento);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dataliostadoBusquedaRequerimientoDetalles.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //GENERACIÓN DE LA LIQUIDACIÓN Y CARGA DE LOS DATOS
        private void btnGenerarLiquidacion_Click(object sender, EventArgs e)
        {
            //SE ASIGNA EL VALOR DEL DATA Y SE BLOQUE LAS COLUMNAS PARA QUE SOLO SE PUEDAN LEER
            //DATALISTADO LIQUIDACION CLIENTES
            datatimeCalculador2.Value = datetiemHastaLiquidacion.Value;
            datalistadoClientesLiquidacion.Columns[2].ReadOnly = true;
            datalistadoClientesLiquidacion.Columns[4].ReadOnly = true;
            datalistadoClientesLiquidacion.Columns[6].ReadOnly = true;
            datalistadoClientesLiquidacion.Columns[8].ReadOnly = true;
            datalistadoClientesLiquidacion.Columns[10].ReadOnly = true;
            //DATALISTADO LIQUIDACION COLABORADORES
            datalistadoColaboradoresLiquidacion.Columns[1].ReadOnly = true;
            datalistadoColaboradoresLiquidacion.Columns[1].ReadOnly = true;

            //SI NO HAY REQUERIMIENTO SELECCIONADO
            if (datalistadoTodasRequerimientos.CurrentRow != null)
            {
                //CAPTURAR EL NOMBRE DE USUARIO
                string usuarioEncargado = datalistadoTodasRequerimientos.SelectedCells[5].Value.ToString();

                //SI EL USUARIO LOGEADO ES IGUAL AL USUARIO ENCARGADO DE DEL REQUERIMINTO
                if (usuarioEncargado == Program.NombreUsuarioCompleto)
                {
                    //RECOJO DE VARIABLES PARA LA VALIDACIÓN
                    int codigoRequerimeinto = Convert.ToInt32(datalistadoTodasRequerimientos.SelectedCells[1].Value.ToString());
                    bool estadoLiquidacion = Convert.ToBoolean(datalistadoTodasRequerimientos.SelectedCells[11].Value.ToString());
                    DateTime fechaTermino = Convert.ToDateTime(datalistadoTodasRequerimientos.SelectedCells[4].Value.ToString());
                    int estadoHabilitadoJefatura = Convert.ToInt32(datalistadoTodasRequerimientos.SelectedCells[14].Value.ToString());
                    fechaTermino = fechaTermino.AddDays(10);

                    //SI EL REQUERIMIENTO TIENE LIQUIDACIÓN
                    if (estadoLiquidacion == true)
                    {
                        MessageBox.Show("Este requerimiento ya tiene una liquidación hecha.", "Validación del Sistema");
                    }
                    //SI LA FECHA DE TÉRMINO MÁS 10 DIAS ES MAYOR 
                    else if (fechaTermino < DateTime.Now && estadoHabilitadoJefatura == 0)
                    {
                        try
                        {
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("CambiarEstadoFechaLiquidacion", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idrequerimiento", codigoRequerimeinto);
                            cmd.ExecuteNonQuery();
                            con.Close();
                            BusquedaDependiente();

                            MessageBox.Show("Este requerimiento se pasó de la cantidad de dias habilitados para poder generar la liquidación, por favor comunicar a su jefatura para que lo pueda liberar.", "Validación del Sistema");
                        }
                        catch (Exception ex)
                        {
                            ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
                        }
                    }
                    else
                    {
                        if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "APROBADO" && datalistadoTodasRequerimientos.SelectedCells[10].Value.ToString() == "ATENDIDO")
                        {
                            //CARGA Y BUSQEUDA DE CAMPOS ESENCAILES PARA LA CARGA Y EL GUARDADO DE LA LIQUIDACION
                            CargarTipoMoneda(cboTipoMonedaLiquidacion);
                            CargarResponsables(cboResponsableLiquidacion);
                            CargarVehiculosLiqui(cboVehiculoLiquidacion);
                            BuscarRequerimeintoGeneral(codigoRequerimeinto);
                            BuscarRequerimeintoClientes(codigoRequerimeinto);
                            BuscarRequerimeintoColaboradores(codigoRequerimeinto);
                            BuscarRequerimeintoDetalles(codigoRequerimeinto);

                            cboBusquedaClientesLiquidacion.SelectedIndex = 0;
                            cboBusquedaColaboradorLiquidacion.SelectedIndex = 0;
                            panelNuevaLiquidadcion.Visible = true;

                            //CARGA DE DATOS DE LOS LISTADO AL FORMUALRIO DE INGRESO DE LIQUIDACION
                            int tipoRequerimiento = Convert.ToInt32(datalistadoBusquedaReuqerimientoGeneral.SelectedCells[2].Value.ToString());

                            if (tipoRequerimiento == 1)
                            {
                                rbNacionalLiquidacion.Checked = true;
                                rbExteriorLiquidacion.Checked = false;
                            }
                            else
                            {
                                rbNacionalLiquidacion.Checked = false;
                                rbExteriorLiquidacion.Checked = true;
                            }

                            //DATOS GENERALES DEL REQUERIMEINTO
                            cboResponsableLiquidacion.SelectedValue = datalistadoBusquedaReuqerimientoGeneral.SelectedCells[5].Value.ToString();
                            cboResponsableLiquidacion.SelectedValue = datalistadoBusquedaReuqerimientoGeneral.SelectedCells[5].Value.ToString();
                            cboVehiculoLiquidacion.SelectedValue = datalistadoBusquedaReuqerimientoGeneral.SelectedCells[6].Value.ToString();
                            datetimeDesdeLiquidacion.Value = Convert.ToDateTime(datalistadoBusquedaReuqerimientoGeneral.SelectedCells[7].Value.ToString());
                            datetiemHastaLiquidacion.Value = Convert.ToDateTime(datalistadoBusquedaReuqerimientoGeneral.SelectedCells[8].Value.ToString());
                            txtMotivoViajeLiquidacion.Text = datalistadoBusquedaReuqerimientoGeneral.SelectedCells[9].Value.ToString();
                            txtItinerarioViajeLiqudiacion.Text = datalistadoBusquedaReuqerimientoGeneral.SelectedCells[10].Value.ToString();
                            txtAdelantoLiquidaciones.Text = datalistadoBusquedaReuqerimientoGeneral.SelectedCells[11].Value.ToString();
                            cboTipoMonedaLiquidacion.SelectedValue = datalistadoBusquedaReuqerimientoGeneral.SelectedCells[12].Value.ToString();
                            lblTipoMoneda.Text = datalistadoBusquedaReuqerimientoGeneral.SelectedCells[13].Value.ToString();
                            lblTipoMoneda2.Text = datalistadoBusquedaReuqerimientoGeneral.SelectedCells[13].Value.ToString();
                            lblTipoMoneda3.Text = datalistadoBusquedaReuqerimientoGeneral.SelectedCells[13].Value.ToString();
                            txtNumeroRequerimeintoLiquidacion.Text = datalistadoBusquedaReuqerimientoGeneral.SelectedCells[0].Value.ToString();

                            //DATOS Y CLIENTES DEL REQUERIMEINTO
                            foreach (DataGridViewRow row in datalistadoBusquedaRequerimientoCLientes.Rows)
                            {
                                string codigoCliente = row.Cells[2].Value.ToString();
                                string ClienteDes = row.Cells[3].Value.ToString();
                                string codigoUnidad = row.Cells[4].Value.ToString();
                                string UnidadDes = row.Cells[5].Value.ToString();
                                string codigoDepartamento = row.Cells[6].Value.ToString();
                                string DepartamentoDes = row.Cells[7].Value.ToString();

                                datalistadoClientesLiquidacion.Rows.Add(new[] { null, null, null, null, null, codigoCliente, ClienteDes, codigoUnidad, UnidadDes, codigoDepartamento, DepartamentoDes });
                            }
                            //DATOS Y COLABORADORES DEL REQUERIMEINTO
                            foreach (DataGridViewRow row in datalistadoBusquedaRequerimeintoColaboradores.Rows)
                            {
                                string codigoVendedor = row.Cells[2].Value.ToString();
                                string VendedorDes = row.Cells[3].Value.ToString();

                                datalistadoColaboradoresLiquidacion.Rows.Add(new[] { null, codigoVendedor, VendedorDes });
                            }

                            //DATOS Y DETALLES DEL REQUERIMEINTO
                            foreach (DataGridViewRow row in dataliostadoBusquedaRequerimientoDetalles.Rows)
                            {
                                string fechaRequerimeintoDetalle = row.Cells[2].Value.ToString();
                                string conbustible = row.Cells[3].Value.ToString();
                                string hospedaje = row.Cells[4].Value.ToString();
                                string viatico = row.Cells[5].Value.ToString();
                                string peaje = row.Cells[6].Value.ToString();
                                string movilidad = row.Cells[7].Value.ToString();
                                string otros = row.Cells[8].Value.ToString();
                                string subTotal = row.Cells[9].Value.ToString();

                                datalistadoDetallesLiquidacion.Rows.Add(new[] { null, fechaRequerimeintoDetalle, conbustible, hospedaje, viatico, peaje, movilidad, otros, subTotal });
                            }

                            datalistadoTodasRequerimientos.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("Debe tener la aprobación de la jefatura comercial o del área contable para poder continuar.", "Validación del Sistema");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Solo puede hacer el proceso el responsable de este.", "Validación del Sistema");
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un requerimiento para poder generar una liquidación.", "Validación del Sistema");
            }
        }

        //AGREGAR MÁS CLIENTES A MI LIQUIDACIÓN
        private void datalistadoBusquedaClietneLiquidacion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoBusquedaClietneLiquidacion.Columns[e.ColumnIndex];
            //SI HAY 3 O MÁS CLIENTES, YA NOS E PODRAN INGRESAR MÁS
            if (datalistadoClientesLiquidacion.RowCount >= 3)
            {
                MessageBox.Show("Solo se pueden ingresar un máximo de 3 clientes por liquidación.", "Validación del Sistema");
            }
            else
            {
                //SI SE PRECIONA SOBRE LA COLUMNA CON ESE NOMBRE
                if (currentColumn.Name == "btnSeleccionarClienteLiquidacion")
                {
                    //SE CAPTURA LAS VARIABLES 
                    string codigo = datalistadoBusquedaClietneLiquidacion.SelectedCells[1].Value.ToString();
                    string cliente = datalistadoBusquedaClietneLiquidacion.SelectedCells[2].Value.ToString();
                    string idunidad = datalistadoBusquedaClietneLiquidacion.SelectedCells[3].Value.ToString();
                    string unidad = datalistadoBusquedaClietneLiquidacion.SelectedCells[4].Value.ToString();
                    string iddestino = datalistadoBusquedaClietneLiquidacion.SelectedCells[5].Value.ToString();
                    string destino = datalistadoBusquedaClietneLiquidacion.SelectedCells[6].Value.ToString();
                    //SE AGREGA A LA NUEVA LISTA
                    datalistadoClientesLiquidacion.Rows.Add(new[] { null, null, null, null, null, codigo, cliente, idunidad, unidad, iddestino, destino });
                }
            }
        }

        //LIMPIEZA DEL CLIENTE SELCCIONADO Y CARGADO
        private void btnBorrarSeleccionClienteLiquidacion_Click(object sender, EventArgs e)
        {
            //SI EN EL LISTADO DE CLIENTES NO HAY REGIUSTROS
            if (datalistadoClientesLiquidacion.Rows.Count > 0)
            {
                //MENSAJE DE CONFIRMACIÓN DE ELIMINACIÓN DE CLIENTES
                DialogResult resul = MessageBox.Show("¿Seguro que desea borrar ha este cliente?.", "Validación del Sistema", MessageBoxButtons.YesNo);
                if (resul == DialogResult.Yes)
                {
                    //ACCIÓN DE REMOVER AL CLIENTE SELECCIOANDO
                    datalistadoClientesLiquidacion.Rows.Remove(datalistadoClientesLiquidacion.CurrentRow);
                }
            }
            else
            {
                MessageBox.Show("No hay clientes agregados para poder borrarlos.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //AGREGAR MÁS COLABORADORES A MI LIQUIDACIÓN
        private void datalistadoBusquedaColaboradorLiquidacion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoBusquedaColaboradorLiquidacion.Columns[e.ColumnIndex];

            //SI SE PRECIONA SOBRE LA COLUMNA CON ESE NOMBRE
            if (currentColumn.Name == "btnSeleccionarColaboradorLiquidacion")
            {
                //SE CAPTURA LAS VARIABLES 
                string codigo = datalistadoBusquedaColaboradorLiquidacion.SelectedCells[1].Value.ToString();
                string colaborador = datalistadoBusquedaColaboradorLiquidacion.SelectedCells[2].Value.ToString();
                //SE AGREGA A LA NUEVA LISTA
                datalistadoColaboradoresLiquidacion.Rows.Add(new[] { null, codigo, colaborador });
            }
        }

        //CARGA DEL COLABORADOR SELCCIONADO AL OTRO LISTADO
        private void btnBorrarColaboradorLiquidacion_Click(object sender, EventArgs e)
        {
            //SI EN EL LISTADO DE COLABORADORES NO HAY REGISTROS
            if (datalistadoColaboradoresLiquidacion.Rows.Count > 0)
            {
                //MENSAJE DE CONFIRMACIÓN DE ELIMINACIÓN DE CLIENTES
                DialogResult resul = MessageBox.Show("¿Seguro que desea borrar ha este colaborador?.", "Validación del Sistema", MessageBoxButtons.YesNo);
                if ((resul == DialogResult.Yes))
                {
                    //SI EL COLABORADOR QUE SE QUIERE REMOVER ES EL USUARIO QUE ESTA CREANDO LA LIQUIDACIÓN
                    if (cboResponsableLiquidacion.Text == datalistadoColaboradoresLiquidacion.SelectedCells[2].Value.ToString())
                    {
                        MessageBox.Show("No se puede borrar al encargado del requerimiento.", "Validación del Sistema", MessageBoxButtons.OK);
                    }
                    else
                    {
                        //ACCIÓN DE REMOVER AL CLIENTE SELECCIOANDO
                        datalistadoColaboradoresLiquidacion.Rows.Remove(datalistadoColaboradoresLiquidacion.CurrentRow);
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay colaboradores agregados para poder borrarlos.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //FUNCIONES DE LIQUIDACIÓN-----------------------------------------------------
        //CARGA Y GENERACIÓN DEL RANGO DE DIAS SELECCIOANDOS
        private void btnEnviarHbilitarLiquidacion_Click(object sender, EventArgs e)
        {
            //SI NO HAY COLABORADORES
            if (datalistadoColaboradoresLiquidacion.Rows.Count < 0)
            {
                MessageBox.Show("Debe ingresar colaboradores para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                //CAPTURA DEL RESPONSABLE
                string responsable = cboResponsableLiquidacion.Text;
                //bool estadoResopnsable = false;
                //RECORRIDO PARA DEFINIR SI EL RESPONSABLE SE ENCUENTRA EN LA LISTA DE COLABORADORES
                foreach (DataGridViewRow row in datalistadoColaboradoresLiquidacion.Rows)
                {
                    string colaboradores = row.Cells[2].Value.ToString();

                    if (colaboradores == responsable)
                    {
                        //estadoResopnsable = true;
                    }
                }
            }

            //SI LA FECHA DE LA LIQUIDACIÓN ES IGUAL A LA FECHA ACTUAL O SI LA FECHA DE LA LIQUIDACIÓN ES MENOR A LA FECHA ACTUAL
            if (datetimeDesdeLiquidacion.Text == datatimeCalculador2.Text || datetimeDesdeLiquidacion.Value <= datatimeCalculador2.Value)
            {
                //SI NO HAY NADA INGRESADO EN EL ITINERARIO O EN EL MOTIVO
                if (txtMotivoViajeLiquidacion.Text == "" || txtItinerarioViajeLiqudiacion.Text == "")
                {
                    MessageBox.Show("Debe ingresar un motívo y un itinerario.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    //GENERACIÓN DE FILAS PARA MI PRESUPUESTO DE VIAJE
                    DataGridViewRow fila = new DataGridViewRow();
                    fila.CreateCells(datalistadoDetallesLiquidacion);
                    fila.Cells[1].Value = this.datatimeCalculador2.Text;
                    datalistadoDetallesLiquidacion.Rows.Add(fila);
                    // para restar la fecha de dtcalculo de 1 en 1 por el txtNumFecha
                    datatimeCalculador2.Value = datatimeCalculador2.Value.Subtract(TimeSpan.FromDays(Convert.ToDouble(txtNumFecha2.Text)));
                    //direccion(datalistadoDetallesLiquidacion);
                }
            }
        }

        //ASIGNO EL VALOR DE LA FECHA FINAL A UN DATATIME ESCONDIDO PARA QUE HAGA LOS CALCULOS
        private void datetiemHastaLiquidacion_ValueChanged(object sender, EventArgs e)
        {
            datatimeCalculador2.Value = datetiemHastaLiquidacion.Value;
        }

        //VALIDACIÓN DEL LISTADO DE PRESUPUESTO
        private void datalistadoDetallesLiquidacion_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //VARIABLES PARA ALMACENAR LOS DATOS
            decimal a;
            decimal b;
            decimal c;
            decimal d;
            decimal f;
            decimal g;
            decimal total;
            //ALMACENAMIENTO DE DATOS PARA LA VALIDACIÓN
            //RECORRIDO DE MI LISTADO PARA ALMACENAR LAS DIFERENTES COLUMNAS
            DataGridViewRow row = (DataGridViewRow)datalistadoDetallesLiquidacion.Rows[e.RowIndex];
            //ALMACENAMIENTOS DE COLUMNAS
            a = Convert.ToDecimal(row.Cells[2].Value);
            b = Convert.ToDecimal(row.Cells[3].Value);
            c = Convert.ToDecimal(row.Cells[4].Value);
            d = Convert.ToDecimal(row.Cells[5].Value);
            f = Convert.ToDecimal(row.Cells[6].Value);
            g = Convert.ToDecimal(row.Cells[7].Value);

            //VALIDACIÓN DE COMBUSTIBLE 
            if (row.Cells[2].Value == DBNull.Value)
            {
                //REINICIO DE CAMPO
                a = Convert.ToDecimal("0.00");
            }
            else
            {
                //CAPTURA DEL VALOR
                a = Convert.ToDecimal(row.Cells[2].Value);
            }

            //VALIDACIÓN DE HOSPEDAJE
            if (row.Cells[3].Value == DBNull.Value)
            {
                //REINICIO DE CAMPO
                b = Convert.ToDecimal("0.00");
            }
            else
            {
                //CAPTURA DEL VALOR
                b = Convert.ToDecimal(row.Cells[3].Value);
            }

            //VALIDACIÓN DE VIÁTICOS
            if (row.Cells[4].Value == DBNull.Value)
            {
                //REINICIO DE CAMPO
                c = Convert.ToDecimal("0.00");
            }
            else
            {
                //CAPTURA DEL VALOR
                c = Convert.ToDecimal(row.Cells[4].Value);
            }

            //VALIDACIÓN DE PEAJES
            if (row.Cells[5].Value == DBNull.Value)
            {
                //REINICIO DE CAMPO
                d = Convert.ToDecimal("0.00");
            }
            else
            {
                //CAPTURA DEL VALOR
                d = Convert.ToDecimal(row.Cells[5].Value);
            }

            //VALIDACIÓN DE MOVILIDAD
            if (row.Cells[6].Value == DBNull.Value)
            {
                //REINICIO DE CAMPO
                f = Convert.ToDecimal("0.00");
            }
            else
            {
                //CAPTURA DEL VALOR
                f = Convert.ToDecimal(row.Cells[6].Value);
            }

            //VALIDACIÓN DE OTROS
            if (row.Cells[7].Value == DBNull.Value)
            {
                //REINICIO DE CAMPO
                g = Convert.ToDecimal("0.00");
            }
            else
            {
                //CAPTURA DEL VALOR
                g = Convert.ToDecimal(row.Cells[7].Value);
            }


            //SUMA DE LOS CAMPOS PARA DARLE EL VALOR AL SUBTOTAL
            total = a + b + c + d + f + g;
            //REORDENAMIENTO DE VALORES INGRESADOS A CADA CAMPO
            row.Cells[2].Value = String.Format("{0:#,0.00}", a);
            row.Cells[3].Value = String.Format("{0:#,0.00}", b);
            row.Cells[4].Value = String.Format("{0:#,0.00}", c);
            row.Cells[5].Value = String.Format("{0:#,0.00}", d);
            row.Cells[6].Value = String.Format("{0:#,0.00}", f);
            row.Cells[7].Value = String.Format("{0:#,0.00}", g);
            row.Cells[8].Value = String.Format("{0:#,0.00}", total);
            //LAMADA AL MÉTODO DE SUBTOTAL
            SubTotalLiquidacion(datalistadoDetallesLiquidacion);
            saldoLiquidacion();
        }

        //BORRAR UNA FILA DEL PRESUPUESTO GENERADO
        private void btnBorrarPresupuestoLiquidacion_Click(object sender, EventArgs e)
        {
            if (datalistadoDetallesLiquidacion.Rows.Count > 0)
            {
                datalistadoDetallesLiquidacion.Rows.Remove(datalistadoDetallesLiquidacion.CurrentRow);
                SubTotalLiquidacion(datalistadoDetallesLiquidacion);
            }
            else
            {
                MessageBox.Show("No hay registro en el detalle para poder remover.", "Validación del Sistema");
            }
        }

        //MOSTRAR LA POSIBILIDAD DE ELEJIR LAS FECHAS SEGÚN EL CAMPO SEELCCIOANDO
        private void datalistadoClientesLiquidacion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                panelFechaInicio.Visible = true;
            }

            if (e.ColumnIndex == 3)
            {
                panelFechaTermino.Visible = true;
            }
        }

        //CARGAR FECHA DE INICIO AL CLIENTE SELECCIONADO
        private void btnCargarFechaInicio_Click(object sender, EventArgs e)
        {
            datalistadoClientesLiquidacion.CurrentRow.Cells[2].Value = dateTimeFechaInicio.Text;
            panelFechaInicio.Visible = false;
        }

        //SALIR DE LA FECHA DE TÉRMINO - CARGA
        private void btnSalirFechaInicio_Click(object sender, EventArgs e)
        {
            panelFechaInicio.Visible = false;
        }

        //CARGAR FECHA DE TÉRMINO AL CLIENTE SELECCIONADO
        private void btnCargarFechaTermino_Click(object sender, EventArgs e)
        {
            datalistadoClientesLiquidacion.CurrentRow.Cells[4].Value = dateTimeFechaTermino.Text;
            panelFechaTermino.Visible = false;
        }

        //SALIR DE LA FECHA DE TÉRMINO - CARGA
        private void btnSalirFechaTermino_Click(object sender, EventArgs e)
        {
            panelFechaInicio.Visible = false;
        }

        private Boolean EsFecha(String fecha)
        {
            try
            {
                DateTime.Parse(fecha);
                return true;
            }
            catch
            {
                return false;
            }
        }

        ////VALIDAR SI LAS FECHAS SE HAN INGRESADO .SelectedCells[1].Value.ToString()
        private void datalistadoClientesLiquidacion_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //Validamos si no es una fila nueva
            if (!datalistadoClientesLiquidacion.Rows[e.RowIndex].IsNewRow)
            {
                //Sólo controlamos el dato de la columna 0
                if (e.ColumnIndex == 2)
                {
                    if (!this.EsFecha(e.FormattedValue.ToString()))
                    {
                        MessageBox.Show("El dato introducido no es de tipo fecha.", "Error de validación",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        datalistadoClientesLiquidacion.Rows[e.RowIndex].ErrorText = "El dato introducido no es de tipo fecha.";
                        e.Cancel = true;
                    }
                }
            }
        }

        //PROCESO DE GAUREDAR LIQUIDACIÓN
        private void btnGuardarLiquidacion_Click(object sender, EventArgs e)
        {
            if (rbNacionalLiquidacion.Checked == false && rbExteriorLiquidacion.Checked == false)
            {
                MessageBox.Show("No se ha seleccionado el tipo de liquidación correctamente.", "Validación del Sistema");
            }
            else
            {
                if (datalistadoClientesLiquidacion.RowCount == 0 || datalistadoColaboradoresLiquidacion.RowCount == 0)
                {
                    MessageBox.Show("No se han cargado los clientes correctamnete.", "Validación del Sistema");
                }
                else
                {
                    if (datalistadoDetallesLiquidacion.RowCount == 0)
                    {
                        MessageBox.Show("No se han cargado los detalles de la liquidación correctamente.", "Validación del Sistema");
                    }
                    else
                    {
                        if (txtMotivoViajeLiquidacion.Text == "" || txtItinerarioViajeLiqudiacion.Text == "")
                        {
                            MessageBox.Show("No se ha cargado el itinerario o motivo de la liquidación.", "Validación del Sistema");
                        }
                        else
                        {
                            if (txtTotaLiquidaciones.Text == "" || txtSaldoLiquidaciones.Text == "")
                            {
                                MessageBox.Show("No se ha cargado el total ni el saldo de la liquidación.", "Validación del Sistema");
                            }
                            else
                            {
                                if (ckEstadoFueraFecha.Checked == true)
                                {
                                    panelObservacionesLiquiFueraFecha.Visible = true;
                                }
                                else
                                {
                                    GuardarLiquidacion(txtRazononObservaciones2.Text);
                                }
                            }
                        }
                    }
                }
            }
        }

        //BOTON PARA GUARDAR OBSERVACIONES 2
        private void btnProcederGuardatoObservaciones2_Click(object sender, EventArgs e)
        {
            if (txtRazononObservaciones2.Text == "")
            {
                MessageBox.Show("Debe ingresar un mensaje que justifique el requerimiento fuera de fecha.", "Validación del Sistema");
            }
            else
            {
                GuardarLiquidacion(txtRazononObservaciones2.Text);
            }
        }

        //BTON PARA SALIR Y REGRESAR DE OBSERVACIONES 2
        private void btnRetrocederGuardadoObservaciones2_Click(object sender, EventArgs e)
        {
            txtRazononObservaciones2.Text = "";
            panelObservacionesLiquiFueraFecha.Visible = false;
        }

        //METODO APRA INGRESAR CON OBSERVACIONES Y SIN OBSERVACIONES-
        public void GuardarLiquidacion(string mensajeAtrasado)
        {
            try
            {
                DialogResult boton = MessageBox.Show("¿Realmente desea generar una liquidación para este requerimiento?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {

                    bool sinFecha = false;

                    //VALIDAR SI SE INGRESARON FECHAS
                    foreach (DataGridViewRow row in datalistadoClientesLiquidacion.Rows)
                    {
                        DateTime fechaInicio = Convert.ToDateTime(row.Cells["txtFechaInicioLiquidacionF"].Value);
                        DateTime fechaTermino = Convert.ToDateTime(row.Cells["txtFechaTerminoLiquidacionF"].Value);

                        if (fechaInicio == null || fechaTermino == null || fechaInicio == Convert.ToDateTime("1/01/0001 00:00:00") || fechaTermino == Convert.ToDateTime("1/01/0001 00:00:00"))
                        {
                            sinFecha = true;
                        }
                    }

                    if (sinFecha == false)
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("InsertarLiquidacionVenta", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        codigoLiquidacion();
                        //INGRESO DEL ENCABEZADO DE LA LIQUIDACIÓN
                        cmd.Parameters.AddWithValue("@idLiquidacion", numeroLiquidacion);
                        cmd.Parameters.AddWithValue("@fechaLiquidacion", datatimeFechaRequerimientoLiquidacion.Value);
                        cmd.Parameters.AddWithValue("@fechaInicio", datetimeDesdeLiquidacion.Value);
                        cmd.Parameters.AddWithValue("@fechaTermino", datetiemHastaLiquidacion.Value);

                        if (rbNacionalLiquidacion.Checked == true)
                        {
                            cmd.Parameters.AddWithValue("@nacional", 1);
                            cmd.Parameters.AddWithValue("@extranjeto", 0);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@nacional", 0);
                            cmd.Parameters.AddWithValue("@extranjeto", 1);
                        }

                        cmd.Parameters.AddWithValue("@motivoVisita", txtMotivoViajeLiquidacion.Text);
                        cmd.Parameters.AddWithValue("@idvendedor", cboResponsableLiquidacion.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@idvehiculo", cboVehiculoLiquidacion.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@itinerarioViaje", txtItinerarioViajeLiqudiacion.Text);
                        cmd.Parameters.AddWithValue("@total", txtTotaLiquidaciones.Text);
                        cmd.Parameters.AddWithValue("@adelanto", txtAdelantoLiquidaciones.Text);
                        cmd.Parameters.AddWithValue("@saldo", txtSaldoLiquidaciones.Text);
                        cmd.Parameters.AddWithValue("@idrquequerimeinto", Convert.ToInt32(txtNumeroRequerimeintoLiquidacion.Text));
                        cmd.Parameters.AddWithValue("@idTipoMoneda", cboTipoMonedaLiquidacion.SelectedValue.ToString());
                        CargarJefaturaActual();
                        cmd.Parameters.AddWithValue("@idJefatura", idJefatura);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        //INGRESO DE LOS DETALLES DEL VAIJE/PRESUPEUSTO CON UN FOREACH
                        foreach (DataGridViewRow row in datalistadoDetallesLiquidacion.Rows)
                        {
                            //PROCEDIMIENTO ALMACENADO PARA GUARDAR EL PRESUPUESTO DEL VIAJE
                            con.Open();
                            cmd = new SqlCommand("InsertarLiquidacionVenta_DetalleLiquidacion", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idLiquidacion", numeroLiquidacion);
                            cmd.Parameters.AddWithValue("@fechaLiquiracion", Convert.ToString(row.Cells[1].Value));
                            cmd.Parameters.AddWithValue("@combustible", Convert.ToString(row.Cells[2].Value));
                            cmd.Parameters.AddWithValue("@hospedaje", Convert.ToString(row.Cells[3].Value));
                            cmd.Parameters.AddWithValue("@viatico", Convert.ToString(row.Cells[4].Value));
                            cmd.Parameters.AddWithValue("@peaje", Convert.ToString(row.Cells[5].Value));
                            cmd.Parameters.AddWithValue("@movilidad", Convert.ToString(row.Cells[6].Value));
                            cmd.Parameters.AddWithValue("@otros", Convert.ToString(row.Cells[7].Value));
                            cmd.Parameters.AddWithValue("@subtotal", Convert.ToString(row.Cells[8].Value));
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }

                        //INGRESO DE LOS CLIENTES Y SUS DATOS ANEXOS CON UN FOREACH
                        foreach (DataGridViewRow row in datalistadoClientesLiquidacion.Rows)
                        {
                            //SELECCIONAMOS LOS CÓDIGOS QUE TIENE NUESTRO LISTADO
                            bool estadoCliente = Convert.ToBoolean(row.Cells["btnAsistioClienteLiquidacionF"].Value);
                            DateTime fechaInicio = Convert.ToDateTime(row.Cells["txtFechaInicioLiquidacionF"].Value);
                            DateTime fechaTermino = Convert.ToDateTime(row.Cells["txtFechaTerminoLiquidacionF"].Value);
                            int codigoDetalleCliente = Convert.ToInt32(row.Cells["txtCodigoClietneLiquidacionF"].Value);
                            int codigoDetalleUnidad = Convert.ToInt32(row.Cells["txtCodigoUnidadLiquidadcionF"].Value);
                            string codigoDetalleDestino = Convert.ToString(row.Cells["txtCodigoDepartamentoF"].Value);

                            //PROCEDIMIENTO ALMACENADO PARA GUARDAR A LOS CLIENTES Y SUS DATOS ANEXOS
                            con.Open();
                            cmd = new SqlCommand("InsertarLiquidacionVenta_DetalleCliente", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idLiquidacion", numeroLiquidacion);
                            cmd.Parameters.AddWithValue("@asistencia", estadoCliente);


                            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                            cmd.Parameters.AddWithValue("@datetimeTermino", fechaTermino);

                            cmd.Parameters.AddWithValue("@idClienteDetalle", codigoDetalleCliente);
                            cmd.Parameters.AddWithValue("@idUnidadDetalle", codigoDetalleUnidad);
                            cmd.Parameters.AddWithValue("@codigoDestinoDetalle", codigoDetalleDestino);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }

                        //INGRESO DE LOS COLABORADORES O VENDEDORES CON UN FOREACH
                        foreach (DataGridViewRow row in datalistadoColaboradoresLiquidacion.Rows)
                        {
                            //SELECCIONAMOS LOS CÓDIGOS QUE TIENE NUESTRO LISTADO
                            bool estadoAsistencia = Convert.ToBoolean(row.Cells["btnAsistioColaboradorLiquidacion"].Value);
                            int codigoDetalleColaborador = Convert.ToInt32(row.Cells["txtIdVendedorLiquidacion"].Value);

                            //PROCEDIMIENTO ALMACENADO PARA GUARDAR A LOS VENDEODRES O COLABORADORES
                            con.Open();
                            cmd = new SqlCommand("InsertarLiquidacionVenta_DetalleVendedores", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idLiquidacion", numeroLiquidacion);
                            cmd.Parameters.AddWithValue("@estadoAsistencia", estadoAsistencia);
                            cmd.Parameters.AddWithValue("@idvendedordetalle", codigoDetalleColaborador);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }

                        //MODIFICAR EL ESTADO DEL REQUERIMIENTO DE 0 A 1
                        //COLOCAR EL MENSAJE DE GENERACIÓN DE LIQUIDACIÓN FUERA DE FECHA
                        con.Open();
                        cmd = new SqlCommand("ModificarEstadoRequerimeinto", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idRequerimeinto", Convert.ToInt32(txtNumeroRequerimeintoLiquidacion.Text));
                        cmd.Parameters.AddWithValue("@mensajeFueraFecha", mensajeAtrasado);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Se registró la liquidación exitosamente.", "Validación del Sistema");

                        //REINICIAR FORMULARIO DE INGRESO DE REQUERIMIENTO
                        panelNuevaLiquidadcion.Visible = false;
                        datalistadoTodasRequerimientos.Enabled = true;

                        datalistadoDetallesLiquidacion.Rows.Clear();
                        datalistadoClientesLiquidacion.Rows.Clear();
                        datalistadoColaboradoresLiquidacion.Rows.Clear();
                        rbNacionalLiquidacion.Checked = false;
                        rbExteriorLiquidacion.Checked = false;
                        txtMotivoViajeLiquidacion.Text = "";
                        txtItinerarioViajeLiqudiacion.Text = "";
                        txtTotaLiquidaciones.Text = "";
                        txtAdelantoLiquidaciones.Text = "";
                        txtSaldoLiquidaciones.Text = "";
                        panelObservacionesLiquiFueraFecha.Visible = false;
                        txtRazononObservaciones2.Text = "";

                        BusquedaDependiente();
                        ClassResourses.RegistrarAuditora(4, this.Name, 4, Program.IdUsuario, "Generar liquidación de viaje", numeroLiquidacion);
                    }
                    else
                    {
                        MessageBox.Show("Debe ingresar las fechas correspondientes a la visita, si no se realizó la visita, debe colocar las fechas tentativas y no marcar el cuadro 'Asistió'.", "Validación del Sistema");
                    }
                }
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show(ex.Message, "Error en el servidor.");
            }
        }

        //METODO PARA EDITAR MI REQUERIMIENTO
        public void EditarRequerimiento()
        {
            try
            {
                DialogResult boton = MessageBox.Show("¿Realmente desea editar este requerimiento?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("EditarRequerimientoVenta", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    int codigoRequerimeinto = Convert.ToInt32(datalistadoTodasRequerimientos.SelectedCells[1].Value.ToString());

                    //INGRESO DEL ENCABEZADO DE LA LIQUIDACIÓN
                    cmd.Parameters.AddWithValue("@idRequerimiento", codigoRequerimeinto);
                    cmd.Parameters.AddWithValue("@fechaInicio", datetimeDesde.Value);
                    cmd.Parameters.AddWithValue("@fechaTermino", datetiemHasta.Value);

                    if (rbNacional.Checked == true)
                    {
                        cmd.Parameters.AddWithValue("@nacional", 1);
                        cmd.Parameters.AddWithValue("@extranjeto", 0);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@nacional", 0);
                        cmd.Parameters.AddWithValue("@extranjeto", 1);
                    }

                    cmd.Parameters.AddWithValue("@motivoVisita", txtMotivoViaje.Text);
                    cmd.Parameters.AddWithValue("@idvendedor", cboResponsable.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@idvehiculo", cboVehiculo.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@itinerarioViaje", txtItinerarioViaje.Text);
                    cmd.Parameters.AddWithValue("@total", txtSubTotal.Text);
                    cmd.Parameters.AddWithValue("@idTipoMoneda", cboTipoMoneda.SelectedValue.ToString());
                    CargarJefaturaActual();
                    cmd.Parameters.AddWithValue("@idJefatura", idJefatura);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    //INGRESO DE LOS DETALLES DEL VAIJE/PRESUPEUSTO CON UN FOREACH
                    foreach (DataGridViewRow row in datalistadoPresupuestoViaje.Rows)
                    {
                        //PROCEDIMIENTO ALMACENADO PARA GUARDAR EL PRESUPUESTO DEL VIAJE
                        con.Open();
                        cmd = new SqlCommand("InsertarRequerimientoVenta_DetalleRequerimiento", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idRequerimiento", codigoRequerimeinto);
                        cmd.Parameters.AddWithValue("@fechaRequerimeinto", Convert.ToString(row.Cells[1].Value));
                        cmd.Parameters.AddWithValue("@combustible", Convert.ToString(row.Cells[2].Value));
                        cmd.Parameters.AddWithValue("@hospedaje", Convert.ToString(row.Cells[3].Value));
                        cmd.Parameters.AddWithValue("@viatico", Convert.ToString(row.Cells[4].Value));
                        cmd.Parameters.AddWithValue("@peaje", Convert.ToString(row.Cells[5].Value));
                        cmd.Parameters.AddWithValue("@movilidad", Convert.ToString(row.Cells[6].Value));
                        cmd.Parameters.AddWithValue("@otros", Convert.ToString(row.Cells[7].Value));
                        cmd.Parameters.AddWithValue("@subtotal", Convert.ToString(row.Cells[8].Value));
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    //INGRESO DE LOS CLIENTES Y SUS DATOS ANEXOS CON UN FOREACH
                    foreach (DataGridViewRow row in datalistadoSeleccionCliente.Rows)
                    {
                        //SELECCIONAMOS LOS CÓDIGOS QUE TIENE NUESTRO LISTADO
                        int codigoDetalleCliente = Convert.ToInt32(row.Cells["idCliente"].Value);
                        int codigoDetalleUnidad = Convert.ToInt32(row.Cells["IdUnidad"].Value);
                        string codigoDetalleDestino = Convert.ToString(row.Cells["IdDestino"].Value);

                        //PROCEDIMIENTO ALMACENADO PARA GUARDAR A LOS CLIENTES Y SUS DATOS ANEXOS
                        con.Open();
                        cmd = new SqlCommand("InsertarRequerimientoVenta_DetalleCliente", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idRequerimiento", codigoRequerimeinto);
                        cmd.Parameters.AddWithValue("@idClienteDetalle", codigoDetalleCliente);
                        cmd.Parameters.AddWithValue("@idUnidadDetalle", codigoDetalleUnidad);
                        cmd.Parameters.AddWithValue("@codigoDestinoDetalle", codigoDetalleDestino);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    //INGRESO DE LOS COLABORADORES O VENDEDORES CON UN FOREACH
                    foreach (DataGridViewRow row in datalistadoSeleccionColaborador.Rows)
                    {
                        //SELECCIONAMOS LOS CÓDIGOS QUE TIENE NUESTRO LISTADO
                        int codigoDetalleColaborador = Convert.ToInt32(row.Cells["idvendedor"].Value);

                        //PROCEDIMIENTO ALMACENADO PARA GUARDAR A LOS VENDEODRES O COLABORADORES
                        con.Open();
                        cmd = new SqlCommand("InsertarRequerimientoVenta_DetalleVendedores", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idRequerimiento", codigoRequerimeinto);
                        cmd.Parameters.AddWithValue("@idvendedordetalle", codigoDetalleColaborador);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    MessageBox.Show("Se registró el requerimiento exitosamente.", "Validación del Sistema");

                    //REINICIAR FORMULARIO DE INGRESO DE REQUERIMIENTO
                    panelNuevoRequerimiento.Visible = false;
                    panelObservacionesRequeAtrasado.Visible = false;
                    txtBusqeudaCliente.Text = "";
                    txtBusquedaColaborador.Text = "";

                    datalistadoSeleccionCliente.Rows.Clear();
                    datalistadoSeleccionColaborador.Rows.Clear();
                    datalistadoPresupuestoViaje.Rows.Clear();
                    rbNacional.Checked = true;
                    rbExterior.Checked = false;
                    txtMotivoViaje.Text = "";
                    txtItinerarioViaje.Text = "";
                    txtSubTotal.Text = "";

                    datalistadoClientes.DataSource = null;
                    datalistadoColaboradores.DataSource = null;
                    datalistadoTodasRequerimientos.Enabled = true;

                    BusquedaDependiente();
                    ClassResourses.RegistrarAuditora(8, this.Name, 4, Program.IdUsuario, "Editar requerimiento de viaje", 0);
                }
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show(ex.Message, "Error en el servidor.");
            }
        }

        //SALIR DE LA LIQUIDACIÓN
        private void btnSalirLiquidacion_Click(object sender, EventArgs e)
        {
            panelNuevaLiquidadcion.Visible = false;
            txtNumFecha2.Text = "1";
            datatimeCalculador2.Value = datetiemHastaLiquidacion.Value;

            //REINICIAR FORMULARIO DE INGRESO DE REQUERIMIENTO
            datalistadoClientesLiquidacion.Rows.Clear();
            datalistadoColaboradoresLiquidacion.Rows.Clear();
            datalistadoDetallesLiquidacion.Rows.Clear();

            datalistadoTodasRequerimientos.Enabled = true;
        }

        //------------------------------------------------------------------------------------------------

        //BUSQEUDAS Y VALIDACIONES------------------------------------------------
        //LIMPIEZA DE LA CAJA BUSQUEDA DEL CLIENTE LIQUIDACIÓN
        private void cboBusquedaClientesLiquidacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusquedaCLienteLiquidacion.Text = "";
        }

        //LIMPIEZA DE LA CAJA BUSQUEDA DEL CLIENTE 
        private void cboBusquedaColaboradorLiquidacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusquedaColaboradorLiquidacion.Text = "";
        }

        //LIMPIEZA DE LA CAJA BUSQUEDA DEL CLIENTE 
        private void cboBusquedaClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusqeudaCliente.Text = "";
        }

        //LIMPIEZA DE LA CAJA BUSQUEDA DEL COLABORADOR
        private void cboBusqeudaColaborador_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusquedaColaborador.Text = "";
        }

        //BOTON PARA ABRIR EL MAUAL DE SUAURIO
        private void btnInfo_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //BOTON PARA ABRIR EL MAUAL DE SUAURIO
        private void btnInfoLiquidacion_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //BOTON PARA ABRIR EL MAUAL DE SUAURIO
        private void btnInfoRequerimeinto_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //BUSQUEDA DE CLIENTE POR NOMBRE/APELLIDOS Y DNI
        private void txtBusqeudaCliente_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboBusquedaClientes.Text == "NOMBRES")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("BuscarClientePorNombre_Requerimiento", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", txtBusqeudaCliente.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoClientes.DataSource = dt;
                    con.Close();

                    datalistadoClientes.Columns[1].Visible = false;
                    datalistadoClientes.Columns[3].Visible = false;
                    datalistadoClientes.Columns[3].Visible = false;
                    datalistadoClientes.Columns[5].Visible = false;

                    datalistadoClientes.Columns[2].Width = 300;
                    datalistadoClientes.Columns[4].Width = 150;
                    datalistadoClientes.Columns[6].Width = 150;

                }
                else if (cboBusquedaClientes.Text == "DOCUMENTO")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("BuscarClientePorDocumento_Requerimiento", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@documento", txtBusqeudaCliente.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoClientes.DataSource = dt;
                    con.Close();

                    datalistadoClientes.Columns[1].Visible = false;
                    datalistadoClientes.Columns[3].Visible = false;
                    datalistadoClientes.Columns[3].Visible = false;
                    datalistadoClientes.Columns[5].Visible = false;

                    datalistadoClientes.Columns[2].Width = 300;
                    datalistadoClientes.Columns[4].Width = 150;
                    datalistadoClientes.Columns[6].Width = 150;
                }
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //BUSQUEDA DE LOS COLABORADORES POR NOMBRE/APELLIDOS Y DNI
        private void txtBusquedaColaborador_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboBusqeudaColaborador.Text == "NOMBRES")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("BuscarColaboradorPorNombre_Requerimiento", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", txtBusquedaColaborador.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoColaboradores.DataSource = dt;
                    con.Close();

                    datalistadoColaboradores.Columns[1].Visible = false;

                    datalistadoColaboradores.Columns[2].Width = 420;

                }
                else if (cboBusqeudaColaborador.Text == "DOCUMENTO")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("BuscarColaboradorPorDocumento_Requerimiento", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@documento", txtBusquedaColaborador.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoColaboradores.DataSource = dt;
                    con.Close();

                    datalistadoColaboradores.Columns[1].Visible = false;

                    datalistadoColaboradores.Columns[2].Width = 420;
                }
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //BUSQUEDA DE CLIENTE POR NOMBRE/APELLIDOS Y DNI
        private void txtBusquedaCLienteLiquidacion_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboBusquedaClientesLiquidacion.Text == "NOMBRES")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("BuscarClientePorNombre_Requerimiento", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", txtBusquedaCLienteLiquidacion.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoBusquedaClietneLiquidacion.DataSource = dt;
                    con.Close();

                    datalistadoBusquedaClietneLiquidacion.Columns[1].Visible = false;
                    datalistadoBusquedaClietneLiquidacion.Columns[3].Visible = false;
                    datalistadoBusquedaClietneLiquidacion.Columns[3].Visible = false;
                    datalistadoBusquedaClietneLiquidacion.Columns[5].Visible = false;

                    datalistadoBusquedaClietneLiquidacion.Columns[2].Width = 300;
                    datalistadoBusquedaClietneLiquidacion.Columns[4].Width = 150;
                    datalistadoBusquedaClietneLiquidacion.Columns[6].Width = 150;

                }
                else if (cboBusquedaClientesLiquidacion.Text == "DOCUMENTO")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("BuscarClientePorDocumento_Requerimiento", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@documento", txtBusquedaCLienteLiquidacion.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoBusquedaClietneLiquidacion.DataSource = dt;
                    con.Close();

                    datalistadoBusquedaClietneLiquidacion.Columns[1].Visible = false;
                    datalistadoBusquedaClietneLiquidacion.Columns[3].Visible = false;
                    datalistadoBusquedaClietneLiquidacion.Columns[3].Visible = false;
                    datalistadoBusquedaClietneLiquidacion.Columns[5].Visible = false;

                    datalistadoBusquedaClietneLiquidacion.Columns[2].Width = 300;
                    datalistadoBusquedaClietneLiquidacion.Columns[4].Width = 150;
                    datalistadoBusquedaClietneLiquidacion.Columns[6].Width = 150;
                }
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //BUSQUEDA DE LOS COLABORADORES POR NOMBRE/APELLIDOS Y DNI
        private void txtBusquedaColaboradorLiquidacion_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboBusquedaColaboradorLiquidacion.Text == "NOMBRES")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("BuscarColaboradorPorNombre_Requerimiento", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", txtBusquedaColaboradorLiquidacion.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoBusquedaColaboradorLiquidacion.DataSource = dt;
                    con.Close();

                    datalistadoBusquedaColaboradorLiquidacion.Columns[1].Visible = false;

                    datalistadoBusquedaColaboradorLiquidacion.Columns[2].Width = 420;

                }
                else if (cboBusquedaColaboradorLiquidacion.Text == "DOCUMENTO")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("BuscarColaboradorPorDocumento_Requerimiento", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@documento", txtBusquedaColaboradorLiquidacion.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoBusquedaColaboradorLiquidacion.DataSource = dt;
                    con.Close();

                    datalistadoBusquedaColaboradorLiquidacion.Columns[1].Visible = false;

                    datalistadoBusquedaColaboradorLiquidacion.Columns[2].Width = 420;
                }
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
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

        //VALIDACIÓN DE SOLO NÚMEROS - PRESUPUESTO LIQUIDACIÓN
        private void datalistadoDetallesLiquidacion_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
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

        //VALIDACIÓN DE SOLO NÚMEROS - PRESUPUESTO LIQUIDACIÓN
        private void datalistadoPresupuestoViaje_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
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

        //METODO PARA EXPORTAR A EXCEL MI LISTADO
        public void btnExportacionBasica_Click(object sender, EventArgs e)
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

                panelExportacionOpciones.Visible = false;
                ClassResourses.RegistrarAuditora(5, this.Name, 4, Program.IdUsuario, "Exportar listado de requerimientos de ventas EXCEL", 0);
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //FUNCION PAARA EXPORTAR A EXCEL MI LISTADO COMPLETO
        private void btnExportacionCompleta_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarRequerimientosVentasPorFecha_Excel", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", DesdeFecha.Value);
                cmd.Parameters.AddWithValue("@fechaTermino", HastaFecha.Value);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoExcelCompleto.DataSource = dt;
                con.Close();

                MostrarExcelCompleto();

                SLDocument sl = new SLDocument();
                SLStyle style = new SLStyle();
                SLStyle styleC = new SLStyle();

                //COLUMNAS
                sl.SetColumnWidth(1, 15);
                sl.SetColumnWidth(2, 20);
                sl.SetColumnWidth(3, 20);
                sl.SetColumnWidth(4, 20);
                sl.SetColumnWidth(5, 35);
                sl.SetColumnWidth(6, 35);
                sl.SetColumnWidth(7, 50);
                sl.SetColumnWidth(8, 20);
                sl.SetColumnWidth(9, 20);
                sl.SetColumnWidth(10, 60);
                sl.SetColumnWidth(11, 20);
                sl.SetColumnWidth(12, 20);
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
                foreach (DataGridViewColumn column in datalistadoExcelCompleto.Columns)
                {
                    sl.SetCellValue(1, ic, column.HeaderText.ToString());
                    sl.SetCellStyle(1, ic, style);
                    ic++;
                }

                int ir = 2;
                foreach (DataGridViewRow row in datalistadoExcelCompleto.Rows)
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
                sl.SaveAs(desktopPath + @"\Reporte de Requerimientos Completo.xlsx");
                MessageBox.Show("Se exportó los datos a un archivo de Microsoft Excel en la siguiente ubicación: " + desktopPath, "Validación del Sistema", MessageBoxButtons.OK);

                panelExportacionOpciones.Visible = false;
                ClassResourses.RegistrarAuditora(5, this.Name, 4, Program.IdUsuario, "Exportar listado de requerimientos de ventas EXCEL", 0);
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            }
        }

        //MÉTODO PARA ENVIAR CORREOS POR LA ANULACIÓN DE UN REQUERIMIENTO
        public void Enviar(string para, string asunto, string mensaje)
        {
            var outlokkApp = new Microsoft.Office.Interop.Outlook.Application();
            var mailItem = (Microsoft.Office.Interop.Outlook.MailItem)outlokkApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            mailItem.To = para;
            mailItem.Subject = asunto;
            mailItem.Body = mensaje;

            mailItem.Send();
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(mailItem);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(outlokkApp);
        }

        //ELIMINAR UN COLABORADOR SELECCIOANDO
        private void btnBorrarSeleccionColaboradorLiquidacion_Click(object sender, EventArgs e)
        {
            //SI EN EL LISTADO DE CLIENTES NO HAY REGIUSTROS
            if (datalistadoColaboradoresLiquidacion.Rows.Count > 0)
            {
                //MENSAJE DE CONFIRMACIÓN DE ELIMINACIÓN DE CLIENTES
                DialogResult resul = MessageBox.Show("¿Seguro que desea borrar ha este colaborador?.", "Validación del Sistema", MessageBoxButtons.YesNo);
                if (resul == DialogResult.Yes)
                {
                    //ACCIÓN DE REMOVER AL CLIENTE SELECCIOANDO
                    datalistadoColaboradoresLiquidacion.Rows.Remove(datalistadoColaboradoresLiquidacion.CurrentRow);
                }
            }
            else
            {
                MessageBox.Show("No hay colaboradores agregados para poder borrarlos.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //FUNCION PARA EXPORTAR  EL PDF A MI ESCRITORIO
        private void btnExportar_Click(object sender, EventArgs e)
        {
            ClassResourses.ExportarDesdeVisualizador(datalistadoTodasRequerimientos,1,9,new Dictionary<string, Func<int, Form>>
            {
                 { "ANULADO", id => new Visualizadores.VisualizarRequerimientoDesaprobado(id)},
                 { "APROBADO", id => new Visualizadores.VisualizarRequerimientoAprobado(id) },
                { "PENDIENTE", id => new Visualizadores.VisualizarRequerimientoVenta(id) },
            }
            ,"Requerimiento de Viaje");


            //ExportarDesdeVisualizador();

            //try
            //{
            //    // Crear una instancia del reporte
            //    ReportDocument crystalReport = new ReportDocument();

            //    // Ruta del reporte .rpt
            //    //string rutaBase = Application.StartupPath;
            //    //string rutaBase = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Recursos y Programas\";

            //    string rutaBase = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\.."));


            //    string rutaReporte = "";

            //    //SI EL REQUERIMEINTO ESTÁ APROBADO POR EL ÁREA COMERCIAL
            //    if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "ANULADO")
            //    {
            //        //rutaReporte = Path.Combine(rutaBase, "..", "..", "Reportes", "InformeRequerimientoVentaAnulada.rpt");
            //        rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeRequerimientoVentaAnulada.rpt");
            //    }
            //    //SI EL REQUERIMEINTO ESTÁ APROBADO POR EL ÁREA COMERCIAL
            //    else if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "APROBADO")
            //    {
            //        //rutaReporte = Path.Combine(rutaBase, "..", "..", "Reportes", "InformeRequerimientoVentaAprobado.rpt");
            //        rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeRequerimientoVentaAprobado.rpt");
            //    }
            //    //SI EL REQUERIMEINTO ESTÁ PENDIENTE POR EL ÁREA COMERCIAL
            //    else if (datalistadoTodasRequerimientos.SelectedCells[9].Value.ToString() == "PENDIENTE")
            //    {
            //        //rutaReporte = Path.Combine(rutaBase, "..", "..", "Reportes", "InformeRequerimientoVenta.rpt");
            //        rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeRequerimientoVenta.rpt");
            //    }
            //    //SI EL REQUERIMEINTO NO ENTRA A NINGUNA DE LAS OPCIONES ANTERIORES
            //    else
            //    {
            //        //rutaReporte = Path.Combine(rutaBase, "..", "..", "Reportes", "InformeRequerimientoVentaAnulada.rpt");
            //        rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeRequerimientoVentaAnulada.rpt");
            //    }

            //    crystalReport.Load(rutaReporte);

            //    ClassResourses.AplicarConexionReportes(crystalReport);

            //    // Configurar la conexión a la base de datos
            //    //ConnectionInfo connectionInfo = new ConnectionInfo
            //    //{
            //    //    ServerName = "192.168.1.154,1433", // Ejemplo: "localhost" o "192.168.1.100"
            //    //    DatabaseName = "BD_VENTAS_2", // Nombre de la base de datos
            //    //    UserID = "sa", // Usuario de la base de datos
            //    //    Password = "Arenas.2020!" // Contraseña del usuario
            //    //};

            //    // Aplicar la conexión a cada tabla del reporte
            //    //foreach (CrystalDecisions.CrystalReports.Engine.Table table in crystalReport.Database.Tables)
            //    //{
            //    //    TableLogOnInfo logOnInfo = table.LogOnInfo;
            //    //    logOnInfo.ConnectionInfo = connectionInfo;
            //    //    table.ApplyLogOnInfo(logOnInfo);
            //    //}

            //    // **Enviar parámetro al reporte**
            //    // Cambia "NombreParametro" por el nombre exacto del parámetro en tu reporte
            //    int idRequerimiento = Convert.ToInt32(datalistadoTodasRequerimientos.SelectedCells[1].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
            //    crystalReport.SetParameterValue("@idRequerimiento", idRequerimiento);

            //    // Ruta de salida en el escritorio
            //    string rutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //    string rutaSalida = System.IO.Path.Combine(rutaEscritorio, "Requerimiento de viaje número " + idRequerimiento + ".pdf");

            //    // Exportar a PDF
            //    crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaSalida);

            //    MessageBox.Show($"Reporte exportado correctamente a: {rutaSalida}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //    ClassResourses.RegistrarAuditora(5, this.Name, 4, Program.IdUsuario, "Exportar requerimiento de viaje PDF", idRequerimiento);
            //}
            //catch (Exception ex)
            //{
            //    ClassResourses.RegistrarAuditora(13, this.Name, 4, Program.IdUsuario, ex.Message, 0);
            //    MessageBox.Show("Ocurrió un error al exportar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }
    }
}

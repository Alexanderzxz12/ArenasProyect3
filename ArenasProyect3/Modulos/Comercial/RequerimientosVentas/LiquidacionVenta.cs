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
    public partial class LiquidacionVenta : Form
    {
        //VARIABLES GLOBALES PARA MI LIQUIDACION
        int numeroActa = 0;
        int idLiquidacion = 0;
        private Cursor curAnterior = null;
        string ruta = Manual.manualAreaComercial;
        int idJefatura = 0;

        //CONSTRUCTOR DEL MANTENIMIENTO - LIQUIDACION DE VENTA
        public LiquidacionVenta()
        {
            InitializeComponent();
        }

        //INICIO Y CARGA INICIAL DE LIQUIDACIONES - CONSTRUCTOR--------------------------------------------------------------------------------------
        private void LiquidacionVenta_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            datalistadoTodasLiquidacion.DataSource = null;

            //PREFILES Y PERSIMOS---------------------------------------------------------------
            if (Program.RangoEfecto != 1)
            {
                //DESAPROBAR
                btnDesaprobaLiquidacion.Visible = false;
                lblDesaprobarLiquidacion.Visible = false;
                //APROBAR
                btnAprobarLiquidacion.Visible = false;
                lblAproarRequerimiento.Visible = false;
            }
            //---------------------------------------------------------------------------------
        }

        //CARGA DE COMBOS PARA VEHICULOS, RESPONSABLES Y TIPO DE MONEDA-----------------------------------------
        //CARGAR RESPONSABLES PARA GENERAR LA LIQUIDACION Y REQUERIMEINTO
        public void CargarResponsableLiqui(ComboBox cbo)
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
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
            }
        }

        //CARGAR VEHIVULOS PARA GENERAR LA LIQUIDACIÓN Y REQUERIMEINTO
        public void CargarVehiculos(ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdVehiculo, Descripcion FROM Vehiculos WHERE Estado = 1 ORDER BY Descripcion", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.DisplayMember = "Descripcion";
                cbo.ValueMember = "IdVehiculo";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
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
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
            }
        }

        //CARGA Y BUSQUEDA DE DATOS - CARGA DE COMBOS Y DATOS ANEXOS----------------------------------------------------
        //CARGA DE COMBOS PARA VEHICULOS Y RESPONSABLES
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
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
            }
        }

        //fFUNCION PARA CARGAR LA JEFATURA ACTUAL
        public void CargarJefaturaActual()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT IdUsuarios FROM Usuarios WHERE Rol = 1  AND Area = 'Comercial' AND Estado = 'Activo'", con);
                da.Fill(dt);
                datalistadoJefatura.DataSource = dt;
                con.Close();

                idJefatura = Convert.ToInt32(datalistadoJefatura.SelectedCells[0].Value.ToString());
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
            }
        }

        //CARGAR Y VALIDAR LA CANTIDAD DE LIQUIDACIONES APROBADAS
        public void CargarCantidadLiquidacionesNoAprobadas()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT COUNT(IdLiquidacion) FROM LiquidacionVenta LIQUI INNER JOIN Usuarios USU ON USU.IdUsuarios = LIQUI.IdVendedor WHERE EstadoComercial = 1 AND LIQUI.Estado = 1 AND LIQUI.IdVendedor = @idusuario", con);
                comando.Parameters.AddWithValue("@idusuario", Program.IdUsuario);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                data.Fill(dt);
                datalistadoCantidadLiquidacionesNoAprobadas.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
            }
        }

        //CARGA CONTACTOS DEL CLIENTE
        public void CargarContactoSegunCLiente(ComboBox cbo, int idClinete, Label lblTelefono, Label lblCargo, Label lblCorreo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT DACC.IdDatosAnexosClienteContacto, DACC.Descripcion, DACC.Telefono, C.Descripcion AS CARGO, DACC.Correo FROM DatosAnexosCliente_Contacto DACC INNER JOIN Cargo C on C.IdCargo = DACC.IdCargo WHERE IdCliente = @idcliente ORDER BY  DACC.Descripcion", con);
                comando.Parameters.AddWithValue("@idcliente", idClinete);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.DisplayMember = "Descripcion";
                cbo.ValueMember = "IdDatosAnexosClienteContacto";
                DataRow row = dt.Rows[0];
                lblTelefono.Text = System.Convert.ToString(row["Telefono"]);
                lblCargo.Text = System.Convert.ToString(row["Descripcion"]);
                lblCorreo.Text = System.Convert.ToString(row["Correo"]);
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error de carga de datos, no se tiene un contacto registrado para este cliente, " + ex.Message, "Validación del Sistema");
            }
        }

        //SELECCION Y CARGA DE DATOS DEL CLIENTE SELECCIONADO 1
        private void txtContactoCliente1NuevaActa_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT DACC.IdDatosAnexosClienteContacto, DACC.Descripcion, DACC.Telefono, C.Descripcion AS CARGO, DACC.Correo FROM DatosAnexosCliente_Contacto DACC INNER JOIN Cargo C on C.IdCargo = DACC.IdCargo WHERE IdDatosAnexosClienteContacto = @id ORDER BY  DACC.Descripcion", con);
                comando.Parameters.AddWithValue("@id", System.Convert.ToString(txtContactoCliente1NuevaActa.SelectedValue));
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lblContactoTelefono1.Text = System.Convert.ToString(row["Telefono"]);
                    lblClienteCargo1.Text = System.Convert.ToString(row["CARGO"]);
                    lblContactoCorreo1.Text = System.Convert.ToString(row["Correo"]);
                }
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error de carga de datos, no se tiene un contacto registrado para este cliente, " + ex.Message, "Validación del Sistema");
            }
        }

        //SELECCION Y CARGA DE DATOS DEL CLIENTE SELECCIONADO 2
        private void txtContactoCliente2NuevaActa_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT DACC.IdDatosAnexosClienteContacto, DACC.Descripcion, DACC.Telefono, C.Descripcion AS CARGO, DACC.Correo FROM DatosAnexosCliente_Contacto DACC INNER JOIN Cargo C on C.IdCargo = DACC.IdCargo WHERE IdDatosAnexosClienteContacto = @id ORDER BY  DACC.Descripcion", con);
                comando.Parameters.AddWithValue("@id", System.Convert.ToString(txtContactoCliente2NuevaActa.SelectedValue));
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lblContactoTelefono2.Text = System.Convert.ToString(row["Telefono"]);
                    lblClienteCargo2.Text = System.Convert.ToString(row["CARGO"]);
                    lblContactoCorreo2.Text = System.Convert.ToString(row["Correo"]);
                }
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error de carga de datos, no se tiene un contacto registrado para este cliente, " + ex.Message, "Validación del Sistema");
            }
        }

        //SELECCION Y CARGA DE DATOS DEL CLIENTE SELECCIONADO 3
        private void txtContactoCliente3NuevaActa_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT DACC.IdDatosAnexosClienteContacto, DACC.Descripcion, DACC.Telefono, C.Descripcion AS CARGO, DACC.Correo FROM DatosAnexosCliente_Contacto DACC INNER JOIN Cargo C on C.IdCargo = DACC.IdCargo WHERE IdDatosAnexosClienteContacto = @id ORDER BY  DACC.Descripcion", con);
                comando.Parameters.AddWithValue("@id", System.Convert.ToString(txtContactoCliente3NuevaActa.SelectedValue));
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lblContactoTelefono3.Text = System.Convert.ToString(row["Telefono"]);
                    lblClienteCargo3.Text = System.Convert.ToString(row["CARGO"]);
                    lblContactoCorreo3.Text = System.Convert.ToString(row["Correo"]);
                }
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error de carga de datos, no se tiene un contacto registrado para este cliente, " + ex.Message, "Validación del Sistema");
            }
        }

        //CARGAR CODIGOS PARA ALMACENAR LA NUEVA ACTA Y LA RESPECTIVA VALIDACION
        public void codigoActa()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT IdActa FROM Acta WHERE IdActa = (SELECT MAX(IdActa) FROM Acta)", con);
                da.Fill(dt);
                datalistadoCodigoActa.DataSource = dt;
                con.Close();

                if (datalistadoCodigoActa.Rows.Count != 0)
                {
                    numeroActa = Convert.ToInt32(datalistadoCodigoActa.SelectedCells[0].Value.ToString());
                    int numeroActa2 = 0;
                    numeroActa2 = Convert.ToInt32(numeroActa);
                    numeroActa2 = numeroActa2 + 1;

                    numeroActa = numeroActa2;
                }
                else
                {
                    MessageBox.Show("Se debe inicializar la tabla ACTAS.", "Validación del Sistema", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error de carga de datos, no se tiene un contacto registrado para este cliente, " + ex.Message, "Validación del Sistema");
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
        //-----------------------------------------------------------------------------------------------

        //LISTADO DE LIQUIDACIONES Y SELECCION DE PDF Y ESTADO DE ACTAS---------------------
        //MOSTRAR REQUERIMIENTOS AL INCIO 
        public void MostrarLiquidación(DateTime fechaInicio, DateTime fechaTermino)
        {
            try
            {
                if (lblCarga.Text == "0")
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
                    RedimensionarListado(datalistadoTodasLiquidacion);
                }
                else
                {
                    lblCarga.Text = "0";
                }
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error de carga de datos, no se tiene un contacto registrado para este cliente, " + ex.Message, "Validación del Sistema");
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
                RedimensionarListado(datalistadoTodasLiquidacion);
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
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
                RedimensionarListado(datalistadoTodasLiquidacion);
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
            }
        }

        //FINCION PARA REDIMENCIONAR MI LISTADO
        public void RedimensionarListado(DataGridView DGV)
        {
            DGV.Columns[6].Visible = false;
            DGV.Columns[15].Visible = false;

            DGV.Columns[1].Width = 55;
            DGV.Columns[2].Width = 55;
            DGV.Columns[3].Width = 100;
            DGV.Columns[4].Width = 100;
            DGV.Columns[5].Width = 100;
            DGV.Columns[7].Width = 180;
            DGV.Columns[8].Width = 370;
            DGV.Columns[9].Width = 80;
            DGV.Columns[10].Width = 80;
            DGV.Columns[11].Width = 80;
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
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //SIRVE PARA EVALUAR SI BUSCAR POR TRES FILTROS O DOS
        public void BusquedaDependiente()
        {
            if (txtBusquedaResponsable.Text == "")
            {
                MostrarLiquidación(DesdeFecha.Value, HastaFecha.Value);
            }
            else
            {
                MostrarLiquidacionesResponsable(txtBusquedaResponsable.Text, DesdeFecha.Value, HastaFecha.Value);
            }
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN DE GENERACIÓN DEL PDF
        private void datalistadoTodasLiquidacion_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoTodasLiquidacion.Columns[e.ColumnIndex].Name == "btnGenerarPdf")
            {
                this.datalistadoTodasLiquidacion.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoTodasLiquidacion.Cursor = curAnterior;
            }
        }

        //MOSTRAR LIQUIDACIONES POR RESPONSABLE
        private void txtBusquedaResponsable_TextChanged(object sender, EventArgs e)
        {
            BusquedaDependiente();
        }

        //MOSTRAR LIQUIDACIONES AL MONENTO DE CAMBIO DE FECHAS
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            BusquedaDependiente();
        }

        //MOSTRAR LIQUIDACIONES AL MONENTO DE CAMBIO DE FECHAS
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            BusquedaDependiente();
        }

        //MOSTRAR LIQUIDACIONES AL MONENTO DE CAMBIO DE FECHAS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            BusquedaDependiente();
        }

        //MOSTRAR LIQUIDACOPMES A MOEMNTO DE APLICAR UN FILTRO
        private void btnBusquedaPendientes_Click(object sender, EventArgs e)
        {
            //
        }

        //MOSTRAR LIQUIDACOPMES A MOEMNTO DE APLICAR UN FILTRO
        private void btnBusquedaAprobados_Click(object sender, EventArgs e)
        {
            //
        }

        //MOSTRAR LIQUIDACOPMES A MOEMNTO DE APLICAR UN FILTRO
        private void btnBusquedaDesaprobado_Click(object sender, EventArgs e)
        {
            //
        }

        //MOSTRAR PDF DE LA LIQUIDACION SIN FIRMA DE JEFATURA
        private void btnVisualizarLiquidacion_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasLiquidacion.CurrentRow != null)
            {
                if (datalistadoTodasLiquidacion.SelectedCells[12].Value.ToString() == "ANULADO")
                {
                    string codigoLiquidacionReporte = datalistadoTodasLiquidacion.Rows[datalistadoTodasLiquidacion.CurrentRow.Index].Cells[1].Value.ToString();
                    Visualizadores.VisualizarLiquidacionDesaprobada frm = new Visualizadores.VisualizarLiquidacionDesaprobada();
                    frm.lblCodigo.Text = codigoLiquidacionReporte;

                    frm.Show();
                }
                else if (datalistadoTodasLiquidacion.SelectedCells[12].Value.ToString() == "APROBADO")
                {
                    string codigoLiquidacionReporte = datalistadoTodasLiquidacion.Rows[datalistadoTodasLiquidacion.CurrentRow.Index].Cells[1].Value.ToString();
                    Visualizadores.VisualizarLiquidacionAprobada frm = new Visualizadores.VisualizarLiquidacionAprobada();
                    frm.lblCodigo.Text = codigoLiquidacionReporte;

                    frm.Show();
                }
                else
                {
                    string codigoLiquidacionReporte = datalistadoTodasLiquidacion.Rows[datalistadoTodasLiquidacion.CurrentRow.Index].Cells[1].Value.ToString();
                    Visualizadores.VisualizarLiquidacionesVenta frm = new Visualizadores.VisualizarLiquidacionesVenta();
                    frm.lblCodigo.Text = codigoLiquidacionReporte;

                    frm.Show();
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una liquidación para poder generar el PDF.", "Validación del Sistema");
            }
        }

        //GENERACIÓN LIQUIDACION - PROCESOS--------------------------------------------------------------------------
        //CARGA Y BUSQUEDA DE DATOS
        public void BuscarLiquidacionGeneral(int codigoLiquidacion)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarLiquidacionVentaPorCodigo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigo", codigoLiquidacion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoBusquedaLiquidacionGeneral.DataSource = dt;
            con.Close();
        }

        //CARGA DE CLIENTES DEL REQUERIMIENTO
        public void BuscarLiquidacionClientes(int codigoRequerimiento)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarLiquidacionVentaPorCodigoClientes", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigo", codigoRequerimiento);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoBusquedaLiquidacionCLientes.DataSource = dt;
            con.Close();
        }

        //CARGA DE COLABORADORES DEL REQUERIMIETNO
        public void BuscarLiquidacionColaboradores(int codigoRequerimiento)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarLiquidacionVentaPorCodigoColaboradores", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigo", codigoRequerimiento);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoBusquedaLiquidacionColaboradores.DataSource = dt;
            con.Close();
        }

        //CARGA DE DETALLES DEL REQUERIMEINTO
        public void BuscarLiquidacionDetallesLiqui(int codigoRequerimiento)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarLiquidacionVentaPorCodigoDetalles", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigo", codigoRequerimiento);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataliostadoBusquedaLiquidacionDetalles.DataSource = dt;
            con.Close();
        }

        //EDITAR LIQUIDACION
        private void btnEditarLiquidacion_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasLiquidacion.CurrentRow != null)
            {
                idLiquidacion = Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[1].Value.ToString());

                if (datalistadoTodasLiquidacion.SelectedCells[13].Value.ToString() == "LOQUIDADO" || datalistadoTodasLiquidacion.SelectedCells[12].Value.ToString() == "ANULADO")
                {
                    MessageBox.Show("Esta liquidación ya ha sido revisada por el área contable o esta anulada por este.", "Validación del Sistema");
                }
                else
                {
                    //CARGA Y BÚSQUEDA DE CAMPOS ESENCAILES PARA LA CARGA Y EL GUARDADO DE LA LIQUIDACION
                    CargarTipoMoneda(cboTipoMonedaLiquidacion);
                    CargarResponsables(cboResponsableLiquidacion);
                    CargarVehiculos(cboVehiculoLiquidacion);
                    BuscarLiquidacionGeneral(idLiquidacion);
                    BuscarLiquidacionClientes(idLiquidacion);
                    BuscarLiquidacionColaboradores(idLiquidacion);
                    BuscarLiquidacionDetallesLiqui(idLiquidacion);

                    cboBusquedaClientesLiquidacion.SelectedIndex = 0;
                    cboBusquedaColaboradorLiquidacion.SelectedIndex = 0;
                    panelNuevaLiquidadcion.Visible = true;

                    //CARGA DE DATOS DE LOS LISTADO AL FORMUALRIO DE INGRESO DE LIQUIDACION
                    int tipoRequerimiento = Convert.ToInt32(datalistadoBusquedaLiquidacionGeneral.SelectedCells[2].Value.ToString());

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
                    //datatimeFechaRequerimientoLiquidacion.Value = Convert.ToDateTime(datalistadoBusquedaReuqerimientoGeneral.SelectedCells[1].Value.ToString());
                    cboResponsableLiquidacion.SelectedValue = datalistadoBusquedaLiquidacionGeneral.SelectedCells[5].Value.ToString();
                    cboResponsableLiquidacion.SelectedValue = datalistadoBusquedaLiquidacionGeneral.SelectedCells[5].Value.ToString();
                    cboVehiculoLiquidacion.SelectedValue = datalistadoBusquedaLiquidacionGeneral.SelectedCells[6].Value.ToString();
                    datetimeDesdeLiquidacion.Value = Convert.ToDateTime(datalistadoBusquedaLiquidacionGeneral.SelectedCells[7].Value.ToString());
                    datetiemHastaLiquidacion.Value = Convert.ToDateTime(datalistadoBusquedaLiquidacionGeneral.SelectedCells[8].Value.ToString());
                    txtMotivoViajeLiquidacion.Text = datalistadoBusquedaLiquidacionGeneral.SelectedCells[9].Value.ToString();
                    txtItinerarioViajeLiqudiacion.Text = datalistadoBusquedaLiquidacionGeneral.SelectedCells[10].Value.ToString();
                    txtAdelantoLiquidaciones.Text = datalistadoBusquedaLiquidacionGeneral.SelectedCells[11].Value.ToString();
                    cboTipoMonedaLiquidacion.SelectedValue = datalistadoBusquedaLiquidacionGeneral.SelectedCells[12].Value.ToString();
                    lblTipoMoneda.Text = datalistadoBusquedaLiquidacionGeneral.SelectedCells[13].Value.ToString();
                    lblTipoMoneda2.Text = datalistadoBusquedaLiquidacionGeneral.SelectedCells[13].Value.ToString();
                    lblTipoMoneda3.Text = datalistadoBusquedaLiquidacionGeneral.SelectedCells[13].Value.ToString();
                    txtNumeroRequerimeintoLiquidacion.Text = datalistadoBusquedaLiquidacionGeneral.SelectedCells[0].Value.ToString();

                    //DATOS Y CLIENTES DEL REQUERIMEINTO
                    foreach (DataGridViewRow row in datalistadoBusquedaLiquidacionCLientes.Rows)
                    {
                        string codigoCliente = row.Cells[2].Value.ToString();
                        string ClienteDes = row.Cells[3].Value.ToString();
                        string codigoUnidad = row.Cells[4].Value.ToString();
                        string UnidadDes = row.Cells[5].Value.ToString();
                        string codigoDepartamento = row.Cells[6].Value.ToString();
                        string DepartamentoDes = row.Cells[7].Value.ToString();
                        string ClienteAsis = row.Cells[8].Value.ToString();
                        string fechaInicio = row.Cells[9].Value.ToString();
                        string fechaTermino = row.Cells[10].Value.ToString();

                        datalistadoClientesLiquidacion.Rows.Add(new[] { ClienteAsis, null, fechaInicio, null, fechaTermino, codigoCliente, ClienteDes, codigoUnidad, UnidadDes, codigoDepartamento, DepartamentoDes });
                    }

                    //DATOS Y COLABORADORES DEL REQUERIMEINTO
                    foreach (DataGridViewRow row in datalistadoBusquedaLiquidacionColaboradores.Rows)
                    {
                        string codigoVendedor = row.Cells[2].Value.ToString();
                        string VendedorDes = row.Cells[3].Value.ToString();
                        string VendedorAsis = row.Cells[4].Value.ToString();

                        datalistadoColaboradoresLiquidacion.Rows.Add(new[] { VendedorAsis, codigoVendedor, VendedorDes });
                    }

                    //DATOS Y DETALLES DEL REQUERIMEINTO
                    foreach (DataGridViewRow row in dataliostadoBusquedaLiquidacionDetalles.Rows)
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
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una liquidación para poder editarla.", "Validación del Sistema");
            }
        }

        //ACCIONES DE LA LIQUIDACIÓN-----------------------------------------------------
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

        //CARGAR FECHA DE TÉRMINO AL CLIENTE SELECCIONADO
        private void btnCargarFechaTermino_Click(object sender, EventArgs e)
        {
            datalistadoClientesLiquidacion.CurrentRow.Cells[4].Value = dateTimeFechaTermino.Text;
            panelFechaTermino.Visible = false;
        }

        //SALIR DE LA FECHA DE INICIO - CARGA
        private void btnSalirFechaInicio_Click(object sender, EventArgs e)
        {
            panelFechaInicio.Visible = false;
        }

        //SALIR DE LA FECHA DE TÉRMINO - CARGA
        private void btnSalirFechaTermino_Click(object sender, EventArgs e)
        {
            panelFechaTermino.Visible = false;
        }

        //CARGA Y GENERACIÓN DEL RANGO DE DIAS SELECCIOANDOS
        private void btnEnviarHbilitarLiquidacion_Click(object sender, EventArgs e)
        {
            if (datalistadoColaboradoresLiquidacion.Rows.Count < 0)
            {
                MessageBox.Show("Debe ingresar colaboradores para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                string responsable = cboResponsableLiquidacion.Text;
                bool estadoResopnsable = false;

                foreach (DataGridViewRow row in datalistadoColaboradoresLiquidacion.Rows)
                {
                    string colaboradores = row.Cells[2].Value.ToString();

                    if (colaboradores == responsable)
                    {
                        estadoResopnsable = true;
                    }
                }
            }

            if (datetimeDesdeLiquidacion.Text == datatimeCalculador2.Text || datetimeDesdeLiquidacion.Value <= datatimeCalculador2.Value)
            {
                if (txtMotivoViajeLiquidacion.Text == "" || txtItinerarioViajeLiqudiacion.Text == "")
                {
                    MessageBox.Show("Debe ingresar un motívo y un itinerario.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
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

        //BORRA MI DETALLE DE LA LIQUIDACION
        private void btnBorrarPresupuestoLiquidacion_Click(object sender, EventArgs e)
        {
            if (datalistadoDetallesLiquidacion.Rows.Count > 0)
            {
                datalistadoDetallesLiquidacion.Rows.Remove(datalistadoDetallesLiquidacion.CurrentRow);
                SubTotalLiquidacion(datalistadoDetallesLiquidacion);
            }
            else
            {
                MessageBox.Show("No hay registro en el detalle para poder remover.", "Validación del sistema");
            }
        }

        //FUNCION PARA RECALCULAR Y HAYAR EL TOTAL Y SUBTOTAL DE MI DETALLE DE LIQUIDACION
        private void datalistadoDetallesLiquidacion_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            decimal a;
            decimal b;
            decimal c;
            decimal d;
            decimal f;
            decimal g;
            decimal total;
            DataGridViewRow row = (DataGridViewRow)datalistadoDetallesLiquidacion.Rows[e.RowIndex];
            a = Convert.ToDecimal(row.Cells[2].Value);
            b = Convert.ToDecimal(row.Cells[3].Value);
            c = Convert.ToDecimal(row.Cells[4].Value);
            d = Convert.ToDecimal(row.Cells[5].Value);
            f = Convert.ToDecimal(row.Cells[6].Value);
            g = Convert.ToDecimal(row.Cells[7].Value);

            if (row.Cells[2].Value == DBNull.Value)
            {
                a = Convert.ToDecimal("0.00");
            }
            else
            {
                a = Convert.ToDecimal(row.Cells[2].Value);
            }

            if (row.Cells[3].Value == DBNull.Value)
            {
                b = Convert.ToDecimal("0.00");
            }
            else
            {
                b = Convert.ToDecimal(row.Cells[3].Value);
            }

            if (row.Cells[4].Value == DBNull.Value)
            {
                c = Convert.ToDecimal("0.00");
            }
            else
            {
                c = Convert.ToDecimal(row.Cells[4].Value);
            }

            if (row.Cells[5].Value == DBNull.Value)
            {
                d = Convert.ToDecimal("0.00");
            }
            else
            {
                d = Convert.ToDecimal(row.Cells[5].Value);
            }

            if (row.Cells[6].Value == DBNull.Value)
            {
                f = Convert.ToDecimal("0.00");
            }
            else
            {
                f = Convert.ToDecimal(row.Cells[6].Value);
            }

            if (row.Cells[7].Value == DBNull.Value)
            {
                g = Convert.ToDecimal("0.00");
            }
            else
            {
                g = Convert.ToDecimal(row.Cells[7].Value);
            }

            total = a + b + c + d + f + g;
            row.Cells[2].Value = String.Format("{0:#,0.00}", a);
            row.Cells[3].Value = String.Format("{0:#,0.00}", b);
            row.Cells[4].Value = String.Format("{0:#,0.00}", c);
            row.Cells[5].Value = String.Format("{0:#,0.00}", d);
            row.Cells[6].Value = String.Format("{0:#,0.00}", f);
            row.Cells[7].Value = String.Format("{0:#,0.00}", g);
            row.Cells[8].Value = String.Format("{0:#,0.00}", total);
            SubTotalLiquidacion(datalistadoDetallesLiquidacion);
            saldoLiquidacion();
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
                    subtotal += Convert.ToDecimal(row.Cells[8].Value);
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

        //ASIGNO EL VALOR DE LA FECHA FINAL A UN DATATIME ESCONDIDO PARA QUE HAGA LOS CALCULOS
        private void datetiemHastaLiquidacion_ValueChanged(object sender, EventArgs e)
        {
            datatimeCalculador2.Value = datetiemHastaLiquidacion.Value;
        }

        //;(
        public void direccion(DataGridView dgv)
        {
            dgv.Sort(dgv.Columns[1], ListSortDirection.Ascending);
        }

        //GUARDAR LIQUIDAICON - FUNCION DE EDICION
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
                            if (txtTotaLiquidaciones.Text == "")
                            {
                                MessageBox.Show("No se ha cargado el total ni el saldo de la liquidación.", "Validación del Sistema");
                            }
                            else
                            {
                                try
                                {
                                    DialogResult boton = MessageBox.Show("¿Realmente desea editar esta liquidación?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                                    if (boton == DialogResult.OK)
                                    {
                                        SqlConnection con = new SqlConnection();
                                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                                        con.Open();
                                        SqlCommand cmd = new SqlCommand();
                                        cmd = new SqlCommand("EditarLiquidacionVenta", con);
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        //INGRESO DEL ENCABEZADO DE LA LIQUIDACIÓN
                                        cmd.Parameters.AddWithValue("@idliquidacion", idLiquidacion);
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

                                        cmd.Parameters.AddWithValue("@idvehiculo", cboVehiculoLiquidacion.SelectedValue.ToString());
                                        cmd.Parameters.AddWithValue("@itinerarioViaje", txtItinerarioViajeLiqudiacion.Text);
                                        cmd.Parameters.AddWithValue("@total", txtTotaLiquidaciones.Text);
                                        cmd.Parameters.AddWithValue("@adelanto", txtAdelantoLiquidaciones.Text);
                                        cmd.Parameters.AddWithValue("@saldo", txtSaldoLiquidaciones.Text);
                                        cmd.ExecuteNonQuery();
                                        con.Close();

                                        //LIMPIAR REGISTROS ANTERIORES
                                        con.Open();
                                        cmd = new SqlCommand("EliminarDetallesLiquidacion", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@idliquidacion", idLiquidacion);
                                        cmd.ExecuteNonQuery();
                                        con.Close();

                                        //INGRESO DE LOS DETALLES DEL VAIJE/PRESUPEUSTO CON UN FOREACH
                                        foreach (DataGridViewRow row in datalistadoDetallesLiquidacion.Rows)
                                        {
                                            //PROCEDIMIENTO ALMACENADO PARA GUARDAR EL PRESUPUESTO DEL VIAJE
                                            con.Open();
                                            cmd = new SqlCommand("InsertarEdicionLiquidacionVenta_DetalleLiquidacion", con);
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@idliquidacion", idLiquidacion);
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

                                        MessageBox.Show("Se registró la liquidación exitosamente.", "Validación del Sistema");

                                        //INGRESO DE LA TABLA AUDITORA
                                        con.Open();
                                        cmd = new SqlCommand("InsertarDatosTablaAuditora_Comercial", con);
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.AddWithValue("@idUsuario", Program.IdUsuario);
                                        cmd.Parameters.AddWithValue("@mantenimiento", "Área comercial - Menú Requerimientos y Liquidación - Liquidación de Venta");
                                        cmd.Parameters.AddWithValue("@accion", "Edición de liquidación con código " + idLiquidacion);
                                        cmd.Parameters.AddWithValue("@descripcion", "Liquidación editado por el usuario " + Program.UnoNombreUnoApellidoUsuario + " en la fecha " + DateTime.Now);
                                        cmd.Parameters.AddWithValue("@maquina", Environment.MachineName);
                                        cmd.Parameters.AddWithValue("@fechaAccion", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@nameUsuarioSesion", Environment.UserName);
                                        cmd.Parameters.AddWithValue("@codigoRequerimiento", Convert.ToInt32(txtNumeroRequerimeintoLiquidacion.Text));
                                        cmd.Parameters.AddWithValue("@codigoLiquidacion", idLiquidacion);
                                        cmd.Parameters.AddWithValue("@codigoActa", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@codigoLineaTrabajo", DBNull.Value);
                                        cmd.ExecuteNonQuery();
                                        con.Close();

                                        //REINICIAR FORMULARIO DE INGRESO DE REQUERIMIENTO
                                        panelNuevaLiquidadcion.Visible = false;

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
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "Error en el servidor.");
                                }
                            }
                        }
                    }
                }
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
        }

        //FUNCIONARPARA ABROBAR MI LIQUIDACION POR PARTE DEL AREA COMERCIAL
        private void btnAprobarLiquidacion_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasLiquidacion.CurrentRow != null)
            {
                DialogResult boton = MessageBox.Show("¿Realmente desea aprobar esta liquidación?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    int idLiquidacion = Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[1].Value.ToString());
                    string estadoJefatura = datalistadoTodasLiquidacion.SelectedCells[12].Value.ToString();

                    if (estadoJefatura == "APROBADO")
                    {
                        MessageBox.Show("Esta liquidación ya está aprobada.", "Validación del Sistema");
                    }
                    else
                    {
                        if (Program.AreaUsuario == "Comercial")
                        {

                            try
                            {
                                SqlConnection con = new SqlConnection();
                                SqlCommand cmd = new SqlCommand();
                                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                                con.Open();
                                cmd = new SqlCommand("CambioEstadoLiquidacionoVenta_Comercial", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idliquidacion", idLiquidacion);
                                cmd.Parameters.AddWithValue("@estado", 2);
                                cmd.ExecuteNonQuery();
                                con.Close();

                                MessageBox.Show("Liquidación aprobada exitosamente.", "Validación del Sistema");

                                //INGRESO DE LA TABLA AUDITORA
                                con.Open();
                                cmd = new SqlCommand("InsertarDatosTablaAuditora_Comercial", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@idUsuario", Program.IdUsuario);
                                cmd.Parameters.AddWithValue("@mantenimiento", "Área comercial - Menú Requerimientos y Liquidación - Liquidación de Venta");
                                cmd.Parameters.AddWithValue("@accion", "Aprobarción de una liquidación de viaje número " + idLiquidacion);
                                cmd.Parameters.AddWithValue("@descripcion", "Liquidación de viaje aprobada por el usuario " + Program.UnoNombreUnoApellidoUsuario + " en la fecha " + DateTime.Now);
                                cmd.Parameters.AddWithValue("@maquina", Environment.MachineName);
                                cmd.Parameters.AddWithValue("@fechaAccion", DateTime.Now);
                                cmd.Parameters.AddWithValue("@nameUsuarioSesion", Environment.UserName);
                                cmd.Parameters.AddWithValue("@codigoRequerimiento", Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[2].Value.ToString()));
                                cmd.Parameters.AddWithValue("@codigoLiquidacion", idLiquidacion);
                                cmd.Parameters.AddWithValue("@codigoActa", DBNull.Value);
                                cmd.Parameters.AddWithValue("@codigoLineaTrabajo", DBNull.Value);
                                cmd.ExecuteNonQuery();
                                con.Close();

                                BusquedaDependiente();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una liquidación para poder aprobarla.", "Validación del Sistema");
            }
        }

        //ANULAR LIQUIDACIÓN SELECCIOANDA Y REQUERIMIENTO ASOCAIDO
        private void btnDesaprobaLiquidacion_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasLiquidacion.CurrentRow != null)
            {
                int idLiquidacion = Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[1].Value.ToString());
                int idRequerimiento = Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[15].Value.ToString());
                bool estadoActa = Convert.ToBoolean(datalistadoTodasLiquidacion.SelectedCells[14].Value.ToString());

                DialogResult boton = MessageBox.Show("¿Realmente desea anular esta liquidación?. Se desaprobará el requerimeinto asociado a esta liquidación.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    if (estadoActa == true)
                    {
                        MessageBox.Show("Esta liquidación tiene una o varias actas generadas o aprobadas, por favor anular por el mantenimiento de actas.", "Validación del Sistema");
                    }
                    else
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

                            MessageBox.Show("Liquidación y requerimiento asociado a esta, anuladas exitosamente.", "Validación del Sistema");

                            //INGRESO DE LA TABLA AUDITORA
                            con.Open();
                            cmd = new SqlCommand("InsertarDatosTablaAuditora_Comercial", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@idUsuario", Program.IdUsuario);
                            cmd.Parameters.AddWithValue("@mantenimiento", "Área comercial - Menú Requerimientos y Liquidación - Liquidación de Venta");
                            cmd.Parameters.AddWithValue("@accion", "Anulación de una liquidación de viaje número " + idLiquidacion);
                            cmd.Parameters.AddWithValue("@descripcion", "Liquidación de viaje anulada por el usuario " + Program.UnoNombreUnoApellidoUsuario + " en la fecha " + DateTime.Now);
                            cmd.Parameters.AddWithValue("@maquina", Environment.MachineName);
                            cmd.Parameters.AddWithValue("@fechaAccion", DateTime.Now);
                            cmd.Parameters.AddWithValue("@nameUsuarioSesion", Environment.UserName);
                            cmd.Parameters.AddWithValue("@codigoRequerimiento", Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[2].Value.ToString()));
                            cmd.Parameters.AddWithValue("@codigoLiquidacion", idLiquidacion);
                            cmd.Parameters.AddWithValue("@codigoActa", DBNull.Value);
                            cmd.Parameters.AddWithValue("@codigoLineaTrabajo", DBNull.Value);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            BusquedaDependiente();
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
                MessageBox.Show("Debe seleccionar una liquidación para poder anular.", "Validación del Sistema");
            }
        }
        //------------------------------------------------------------------------------------------------------------

        //SELECCION DE LA LIQUIDACION Y CARGA DE SUS DETALLES---------------------------
        //PROCESO PARA BUSCAR LOS DETALLES DEL CLIENTE DE LA LIQUIDACIÓN
        public void BuscarLiquidacionDetalles(int codigoLiquidacion)
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

        //ABRIR DETALLES DE LA LIQUIDACIÓN------------------------------------------------------
        //SELECCION DEL PDF GENERADO CON SUS RESPECTIVAS FIRMAS, INCLUIDO LA JEFATURA
        private void datalistadoTodasLiquidacion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoTodasLiquidacion.Columns[e.ColumnIndex];

            if (currentColumn.Name == "btnGenerarPdf")
            {
                if (datalistadoTodasLiquidacion.CurrentRow != null)
                {
                    if (datalistadoTodasLiquidacion.SelectedCells[12].Value.ToString() == "ANULADO")
                    {
                        string codigoLiquidacionReporte = datalistadoTodasLiquidacion.Rows[datalistadoTodasLiquidacion.CurrentRow.Index].Cells[1].Value.ToString();
                        Visualizadores.VisualizarLiquidacionDesaprobada frm = new Visualizadores.VisualizarLiquidacionDesaprobada();
                        frm.lblCodigo.Text = codigoLiquidacionReporte;

                        frm.Show();
                    }
                    else if (datalistadoTodasLiquidacion.SelectedCells[12].Value.ToString() == "APROBADO")
                    {
                        string codigoLiquidacionReporte = datalistadoTodasLiquidacion.Rows[datalistadoTodasLiquidacion.CurrentRow.Index].Cells[1].Value.ToString();
                        Visualizadores.VisualizarLiquidacionAprobada frm = new Visualizadores.VisualizarLiquidacionAprobada();
                        frm.lblCodigo.Text = codigoLiquidacionReporte;

                        frm.Show();
                    }
                    else
                    {
                        string codigoLiquidacionReporte = datalistadoTodasLiquidacion.Rows[datalistadoTodasLiquidacion.CurrentRow.Index].Cells[1].Value.ToString();
                        Visualizadores.VisualizarLiquidacionesVenta frm = new Visualizadores.VisualizarLiquidacionesVenta();
                        frm.lblCodigo.Text = codigoLiquidacionReporte;

                        frm.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar una liquidación para poder generar el PDF.", "Validación del Sistema");
                }
            }
        }

        //ABRIR LOS DETALLES DE LA LIQUIDACION CON EL EVENTO DOBLE CLICK
        private void datalistadoTodasLiquidacion_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int codigoLiquidacion = Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[1].Value.ToString());
            txtCodigoLiquidacion.Text = Convert.ToString(codigoLiquidacion);
            panelLiquidacionActas.Visible = true;
            BuscarLiquidacionDetalles(codigoLiquidacion);

            datalistadoTodasLiquidacion.Enabled = false;
        }

        //VISUALIZAR Y GENERAR EL ACTA POR CLIENTE GENERADO SOLO CUENDO ESTA APROBADA
        private void btnVisualizarLiquidacionActas_Click(object sender, EventArgs e)
        {
            if (datalistadoLiquidacionActas.CurrentRow != null)
            {
                if (datalistadoLiquidacionActas.SelectedCells[10].Value.ToString() == "APROBADO")
                {
                    string codigoActaReporte = datalistadoLiquidacionActas.Rows[datalistadoLiquidacionActas.CurrentRow.Index].Cells[11].Value.ToString();
                    Visualizadores.VisualizarActa frm = new Visualizadores.VisualizarActa();
                    frm.lblCodigo.Text = codigoActaReporte;

                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Debe tener la aprobación de las jefaturas para poder continuar.", "Validación del Sistema");
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una acta para poder generar el PDF.", "Validación del Sistema");
            }
        }

        //CERRAR LOS DETALLES DE LA LIQUIDACIÓN Y VERIFICA DE LOS ITEMS Y DEFINE SI SE ACABO
        private void btnCerrarLiquidacionActas_Click(object sender, EventArgs e)
        {
            panelLiquidacionActas.Visible = false;
            List<int> estadoss = new List<int>();
            int estadoFinal1 = 0;
            int estadoFinal2 = 0;
            int idLiquidacion = Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[1].Value.ToString());

            foreach (DataGridViewRow datorecuperado in datalistadoLiquidacionActas.Rows)
            {
                string estado = Convert.ToString(datorecuperado.Cells["ESTADO"].Value);

                if (estado == "APROBADO" || estado == "GENERADO")
                {
                    estadoss.Add(1);
                }
                else
                {
                    estadoss.Add(0);
                }
            }

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

            if (estadoFinal1 > 0 && estadoFinal2 == 0)
            {
                SqlConnection con = new SqlConnection();
                SqlCommand cmd = new SqlCommand();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                cmd = new SqlCommand("CambioEstadoLiquidacionVenta", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idLiquidacion", idLiquidacion);
                cmd.ExecuteNonQuery();
                con.Close();
            }

            datalistadoTodasLiquidacion.Enabled = true;

            BusquedaDependiente();
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
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //ABRIR FORMULARIO QUE NOS AYUDARA A HACERLO
        private void datalistadoLiquidacionActas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoLiquidacionActas.Columns[e.ColumnIndex];

            if (currentColumn.Name == "btnGenerarActaVisita")
            {
                string usuarioEncargado = datalistadoTodasLiquidacion.SelectedCells[7].Value.ToString();
                string estadoComercial = datalistadoTodasLiquidacion.SelectedCells[12].Value.ToString();

                if (usuarioEncargado == Program.NombreUsuarioCompleto)
                {
                    CargarCantidadLiquidacionesNoAprobadas();
                    if (estadoComercial != "ANULADO")
                    {
                        if (Convert.ToInt32(datalistadoCantidadLiquidacionesNoAprobadas.SelectedCells[0].Value.ToString()) >= 3)
                        {
                            MessageBox.Show("No se puede continuar con la generación de esta acta hasta que se observe el resto de liquidaciones faltantes.", "Validación del Sistema");
                        }
                        else
                        {
                            if (datalistadoLiquidacionActas.SelectedCells[10].Value.ToString() == "GENERADO" || datalistadoLiquidacionActas.SelectedCells[10].Value.ToString() == "APROBADO")
                            {
                                MessageBox.Show("Ya se generó un acta para este registro.", "Validación del Sistema");
                            }
                            else
                            {
                                int idCLiente = Convert.ToInt32(datalistadoLiquidacionActas.SelectedCells[4].Value.ToString());
                                datatimeFechaInicioNuevaActa.Value = Convert.ToDateTime(datalistadoLiquidacionActas.SelectedCells[2].Value.ToString());
                                datetimeFechaTerminoNuevaActa.Value = Convert.ToDateTime(datalistadoLiquidacionActas.SelectedCells[3].Value.ToString());
                                txtClienteNuevaActa.Text = datalistadoLiquidacionActas.SelectedCells[5].Value.ToString();
                                txtUnidadNuevaActa.Text = datalistadoLiquidacionActas.SelectedCells[7].Value.ToString();
                                txtAsistentes1NuevaActa.Text = datalistadoTodasLiquidacion.SelectedCells[7].Value.ToString();
                                CargarResponsables(txtAsistentes2NuevaActa);
                                CargarResponsables(txtAsistentes3NuevaActa);
                                txtAsistentes2NuevaActa.SelectedIndex = -1;
                                txtAsistentes3NuevaActa.SelectedIndex = -1;
                                CargarContactoSegunCLiente(txtContactoCliente1NuevaActa, idCLiente, lblContactoTelefono1, lblClienteCargo1, lblContactoCorreo1);
                                CargarContactoSegunCLiente(txtContactoCliente2NuevaActa, idCLiente, lblContactoTelefono2, lblClienteCargo2, lblContactoCorreo2);
                                CargarContactoSegunCLiente(txtContactoCliente3NuevaActa, idCLiente, lblContactoTelefono3, lblClienteCargo3, lblContactoCorreo3);
                                txtContactoCliente1NuevaActa.SelectedIndex = -1;
                                txtContactoCliente2NuevaActa.SelectedIndex = -1;
                                txtContactoCliente3NuevaActa.SelectedIndex = -1;

                                panelNuevaActa.Visible = true;
                                datalistadoLiquidacionActas.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Esta liquidación ha sido anulada.", "Validación del Sistema");
                    }
                }
                else
                {
                    MessageBox.Show("Solo puede hacer el proceso el responsable de este.", "Validación del Sistema");
                }

            }
        }
        //-------------------------------------------------------------------------------------

        //GENERACIÓN Y GUARDADO DEL ACTA--------------------------------------------------------
        private void btnGuardarNuevaActa_Click(object sender, EventArgs e)
        {
            if (rbTipoClienteActualNuevaActa.Checked == false && rbTipoClienteFuturoNuevaActa.Checked == false)
            {
                MessageBox.Show("Debe seleccionar un tipo de cliente.", "Validación del Sistema");
            }
            else
            {
                if (rbFrecuenciaAltaNuevaActa.Checked == false && rbFrecuenciaMediaNuevaActa.Checked == false && rbFrecuenduaBajaNuevaActa.Checked == false)
                {
                    MessageBox.Show("Debe seleccionar una frecuencia y volúmen de compra.", "Validación del Sistema");
                }
                else
                {
                    if (txtContactoCliente1NuevaActa.Text == "")
                    {
                        MessageBox.Show("Debe seleccionar al menos un contacto del cliente.", "Validación del Sistema");
                    }
                    else
                    {
                        try
                        {
                            DialogResult boton = MessageBox.Show("¿Realmente desea guardar esta acta?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                            if (boton == DialogResult.OK)
                            {
                                SqlConnection con = new SqlConnection();
                                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                                con.Open();
                                SqlCommand cmd = new SqlCommand();
                                cmd = new SqlCommand("InsertarActa", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                codigoActa();
                                //INGRESO DEL ENCABEZADO DEL REQUERIMIENTO
                                cmd.Parameters.AddWithValue("@idActa", numeroActa);
                                cmd.Parameters.AddWithValue("@idClienteDetalleLiquidacion", Convert.ToInt32(datalistadoLiquidacionActas.SelectedCells[1].Value.ToString()));

                                cmd.Parameters.AddWithValue("@fechaInicio", datatimeFechaInicioNuevaActa.Value);
                                cmd.Parameters.AddWithValue("@fechaTermino", datetimeFechaTerminoNuevaActa.Value);

                                if (rbTipoClienteActualNuevaActa.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@ckActual", 1);
                                    cmd.Parameters.AddWithValue("@ckFuturoPotencial", 0);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ckActual", 0);
                                    cmd.Parameters.AddWithValue("@ckFuturoPotencial", 1);
                                }

                                if (rbFrecuenciaAltaNuevaActa.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@ckAlto", 1);
                                    cmd.Parameters.AddWithValue("@ckMedia", 0);
                                    cmd.Parameters.AddWithValue("@ckBaja", 0);
                                }
                                else if (rbFrecuenciaMediaNuevaActa.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@ckAlto", 0);
                                    cmd.Parameters.AddWithValue("@ckMedia", 1);
                                    cmd.Parameters.AddWithValue("@ckBaja", 0);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ckAlto", 0);
                                    cmd.Parameters.AddWithValue("@ckMedia", 0);
                                    cmd.Parameters.AddWithValue("@ckBaja", 1);
                                }

                                cmd.Parameters.AddWithValue("@asistente1", txtAsistentes1NuevaActa.Text);
                                cmd.Parameters.AddWithValue("@asistente2", txtAsistentes2NuevaActa.Text);
                                cmd.Parameters.AddWithValue("@asistente3", txtAsistentes3NuevaActa.Text);

                                cmd.Parameters.AddWithValue("@idCliente", Convert.ToInt32(datalistadoLiquidacionActas.SelectedCells[4].Value.ToString()));

                                if (txtContactoCliente1NuevaActa.Text == "")
                                {
                                    cmd.Parameters.AddWithValue("@ContactoCliente1", "");
                                    cmd.Parameters.AddWithValue("@correocliente1", "");
                                    cmd.Parameters.AddWithValue("@cargocliente1", "");
                                    cmd.Parameters.AddWithValue("@telefonocliente1", "");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ContactoCliente1", txtContactoCliente1NuevaActa.Text);
                                    cmd.Parameters.AddWithValue("@correocliente1", lblContactoCorreo1.Text);
                                    cmd.Parameters.AddWithValue("@cargocliente1", lblClienteCargo1.Text);
                                    cmd.Parameters.AddWithValue("@telefonocliente1", lblContactoTelefono1.Text);
                                }

                                if (txtContactoCliente2NuevaActa.Text == "")
                                {
                                    cmd.Parameters.AddWithValue("@ContactoCliente2", "");
                                    cmd.Parameters.AddWithValue("@correocliente2", "");
                                    cmd.Parameters.AddWithValue("@cargocliente2", "");
                                    cmd.Parameters.AddWithValue("@telefonocliente2", "");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ContactoCliente2", txtContactoCliente2NuevaActa.Text);
                                    cmd.Parameters.AddWithValue("@correocliente2", lblContactoCorreo2.Text);
                                    cmd.Parameters.AddWithValue("@cargocliente2", lblClienteCargo2.Text);
                                    cmd.Parameters.AddWithValue("@telefonocliente2", lblContactoTelefono2.Text);
                                }

                                if (txtContactoCliente3NuevaActa.Text == "")
                                {
                                    cmd.Parameters.AddWithValue("@ContactoCliente3", "");
                                    cmd.Parameters.AddWithValue("@correocliente3", "");
                                    cmd.Parameters.AddWithValue("@cargocliente3", "");
                                    cmd.Parameters.AddWithValue("@telefonocliente3", "");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ContactoCliente3", txtContactoCliente3NuevaActa.Text);
                                    cmd.Parameters.AddWithValue("@correocliente3", lblContactoCorreo3.Text);
                                    cmd.Parameters.AddWithValue("@cargocliente3", lblClienteCargo3.Text);
                                    cmd.Parameters.AddWithValue("@telefonocliente3", lblContactoTelefono3.Text);
                                }

                                cmd.Parameters.AddWithValue("@idUnidad", Convert.ToInt32(datalistadoLiquidacionActas.SelectedCells[6].Value.ToString()));

                                if (ckSostenimientoNuevaActa.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@ckSostenimiento", 1);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ckSostenimiento", 0);
                                }

                                if (ckCaptacionNuevaActa.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@ckCapacitacion", 1);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ckCapacitacion", 0);
                                }

                                if (ckRecuperacionNuevaActa.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@ckRecuperacion", 1);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ckRecuperacion", 0);
                                }

                                if (ckReclamoNuevaActa.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@ckReclamo", 1);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ckReclamo", 0);
                                }

                                cmd.Parameters.AddWithValue("@fechaActa", datetimeActa.Value);

                                if (ckPresenteAsistente1.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@presenciaAsistente1Encargado", 1);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@presenciaAsistente1Encargado", 0);
                                }

                                cmd.Parameters.AddWithValue("@idResponsable", Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[6].Value.ToString()));
                                CargarJefaturaActual();
                                cmd.Parameters.AddWithValue("@idJefatura", idJefatura);

                                cmd.ExecuteNonQuery();
                                con.Close();

                                MessageBox.Show("Se generó el acta correctamente en el sistema.", "Validación del Sistema");

                                //INGRESO DE LA TABLA AUDITORA
                                con.Open();
                                cmd = new SqlCommand("InsertarDatosTablaAuditora_Comercial", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@idUsuario", Program.IdUsuario);
                                cmd.Parameters.AddWithValue("@mantenimiento", "Área comercial - Menú Requerimientos y Liquidación - Liquidación de Venta");
                                cmd.Parameters.AddWithValue("@accion", "Nueva acta");
                                cmd.Parameters.AddWithValue("@descripcion", "Acta creada por el usuario " + Program.UnoNombreUnoApellidoUsuario + " en la fecha " + DateTime.Now);
                                cmd.Parameters.AddWithValue("@maquina", Environment.MachineName);
                                cmd.Parameters.AddWithValue("@fechaAccion", DateTime.Now);
                                cmd.Parameters.AddWithValue("@nameUsuarioSesion", Environment.UserName);
                                cmd.Parameters.AddWithValue("@codigoRequerimiento", Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[1].Value.ToString()));
                                cmd.Parameters.AddWithValue("@codigoLiquidacion", Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[2].Value.ToString()));
                                cmd.Parameters.AddWithValue("@codigoActa", numeroActa);
                                cmd.Parameters.AddWithValue("@codigoLineaTrabajo", DBNull.Value);
                                cmd.ExecuteNonQuery();
                                con.Close();

                                panelNuevaActa.Visible = false;
                                int codigoLiquidacion = Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[1].Value.ToString());
                                BuscarLiquidacionDetalles(codigoLiquidacion);

                                rbTipoClienteActualNuevaActa.Checked = false;
                                rbTipoClienteFuturoNuevaActa.Checked = false;
                                rbFrecuenciaAltaNuevaActa.Checked = false;
                                rbFrecuenciaMediaNuevaActa.Checked = false;
                                rbFrecuenduaBajaNuevaActa.Checked = false;
                                ckSostenimientoNuevaActa.Checked = false;
                                ckCaptacionNuevaActa.Checked = false;
                                ckRecuperacionNuevaActa.Checked = false;
                                ckReclamoNuevaActa.Checked = false;
                                datalistadoLiquidacionActas.Enabled = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error en el servidor");
                        }
                    }
                }
            }
        }

        //REGRESAR Y LIMPIAR ELINTENTO DE GENERACIÓN DE ACTA
        private void btnRegresarNuevaActa_Click(object sender, EventArgs e)
        {
            panelNuevaActa.Visible = false;
            datalistadoLiquidacionActas.Enabled = true;

            rbTipoClienteActualNuevaActa.Checked = false;
            rbTipoClienteFuturoNuevaActa.Checked = false;
            rbFrecuenciaAltaNuevaActa.Checked = false;
            rbFrecuenciaMediaNuevaActa.Checked = false;
            rbFrecuenduaBajaNuevaActa.Checked = false;
            ckSostenimientoNuevaActa.Checked = false;
            ckCaptacionNuevaActa.Checked = false;
            ckRecuperacionNuevaActa.Checked = false;
            ckReclamoNuevaActa.Checked = false;
        }

        //LIMPIEZA DE CAMPOS LLENADOS - ASISTENTES Y CONTACTOS DEL CLIENTE
        private void btnCargarDatosAsistente2NuevaActa_Click(object sender, EventArgs e)
        {
            txtAsistentes2NuevaActa.Text = "";
        }

        //LIMPIEZA DE CAMPOS LLENADOS - ASISTENTES Y CONTACTOS DEL CLIENTE
        private void btnCargarDatosAsistente3NuevaActa_Click(object sender, EventArgs e)
        {
            txtAsistentes3NuevaActa.Text = "";
        }

        //LIMPIEZA DE CAMPOS LLENADOS - ASISTENTES Y CONTACTOS DEL CLIENTE
        private void btnCargarDatosClietne1NuevaActa_Click(object sender, EventArgs e)
        {
            txtContactoCliente1NuevaActa.Text = "";
            txtContactoCliente1NuevaActa.SelectedIndex = -1;
        }

        //LIMPIEZA DE CAMPOS LLENADOS - ASISTENTES Y CONTACTOS DEL CLIENTE
        private void btnCargarDatosClietne2NuevaActa_Click(object sender, EventArgs e)
        {
            txtContactoCliente2NuevaActa.Text = "";
            txtContactoCliente2NuevaActa.SelectedIndex = -1;
        }

        //LIMPIEZA DE CAMPOS LLENADOS - ASISTENTES Y CONTACTOS DEL CLIENTE
        private void btnCargarDatosClietne3NuevaActa_Click(object sender, EventArgs e)
        {
            txtContactoCliente3NuevaActa.Text = "";
            txtContactoCliente3NuevaActa.SelectedIndex = -1;
        }

        //LLAMADO DEL BOTON INFO AL M,ANMUAL DE USUARIO
        private void btnInfo_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //LLAMADO DEL BOTON INFO AL M,ANMUAL DE USUARIO
        private void btnInfoActa_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //LLAMADO DEL BOTON INFO AL M,ANMUAL DE USUARIO
        private void btnInfoDetalle_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //LLAMADO DEL BOTON INFO AL M,ANMUAL DE USUARIO
        private void btnInfoLiquidacion_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //BOTON PARA LLAMAR A LA FUNCION DE EXPORTAR EXCEL
        private void btnExportarExcel_Click(object sender, EventArgs e)
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
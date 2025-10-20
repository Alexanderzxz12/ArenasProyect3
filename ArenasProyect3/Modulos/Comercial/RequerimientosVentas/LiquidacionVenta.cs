using ArenasProyect3.Modulos.Comercial.Ventas;
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
using System.Runtime.InteropServices.WindowsRuntime;
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

        //VARIABLES PARA LAS VALIDACIONES
        bool filasvacias;
        bool fechasvacias;
        bool contactosvacios;


        List<string> asistentes = new List<string>();

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
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
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
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
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
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
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
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
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
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
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
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
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

                if (dt.Rows.Count == 0)
                {
                    contactosvacios = true;
                    return;
                }
                else
                {
                    contactosvacios = false;
                    cbo.DisplayMember = "Descripcion";
                    cbo.ValueMember = "IdDatosAnexosClienteContacto";
                    DataRow row = dt.Rows[0];
                    lblTelefono.Text = System.Convert.ToString(row["Telefono"]);
                    lblCargo.Text = System.Convert.ToString(row["Descripcion"]);
                    lblCorreo.Text = System.Convert.ToString(row["Correo"]);
                    cbo.DataSource = dt;
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error de carga de datos, no se tiene un contacto registrado para este cliente, " + ex.Message, "Validación del Sistema", MessageBoxButtons.OK);
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
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error de carga de datos, no se tiene un contacto registrado para este cliente, " + ex.Message, "Validación del Sistema", MessageBoxButtons.OK);
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
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error de carga de datos, no se tiene un contacto registrado para este cliente, " + ex.Message, "Validación del Sistema", MessageBoxButtons.OK);
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
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error de carga de datos, no se tiene un contacto registrado para este cliente, " + ex.Message, "Validación del Sistema", MessageBoxButtons.OK);
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
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error de carga de datos, no se tiene un contacto registrado para este cliente, " + ex.Message, "Validación del Sistema", MessageBoxButtons.OK);
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
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error de carga de datos, no se tiene un contacto registrado para este cliente, " + ex.Message, "Validación del Sistema", MessageBoxButtons.OK);
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
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
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
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
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
            DGV.Columns[3].Width = 90;
            DGV.Columns[4].Width = 90;
            DGV.Columns[5].Width = 90;
            DGV.Columns[7].Width = 190;
            DGV.Columns[8].Width = 380;
            DGV.Columns[9].Width = 80;
            DGV.Columns[10].Width = 80;
            DGV.Columns[11].Width = 80;
            DGV.Columns[12].Width = 100;
            DGV.Columns[13].Width = 100;
            DGV.Columns[14].Width = 65;

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
            if (this.datalistadoTodasLiquidacion.Columns[e.ColumnIndex].Name == "btnSeguimiento")
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
            try
            {
                if (datalistadoTodasLiquidacion.CurrentRow != null)
                {
                    string codigoLiquidacionReporte = "0";

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
                else
                {
                    MessageBox.Show("Debe seleccionar una liquidación para poder generar el PDF.", "Validación del Sistema", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //GENERACIÓN LIQUIDACION - PROCESOS--------------------------------------------------------------------------
        //CARGA Y BUSQUEDA DE DATOS
        public void BuscarLiquidacionGeneral(int codigoLiquidacion)
        {
            try
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
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //CARGA DE CLIENTES DEL REQUERIMIENTO
        public void BuscarLiquidacionClientes(int codigoRequerimiento)
        {
            try
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
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //CARGA DE COLABORADORES DEL REQUERIMIETNO
        public void BuscarLiquidacionColaboradores(int codigoRequerimiento)
        {
            try
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
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //CARGA DE DETALLES DEL REQUERIMEINTO
        public void BuscarLiquidacionDetallesLiqui(int codigoRequerimiento)
        {
            try
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
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //EDITAR LIQUIDACION
        private void btnEditarLiquidacion_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasLiquidacion.CurrentRow != null)
            {
                idLiquidacion = Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[1].Value.ToString());
                bool estadoActa = Convert.ToBoolean(datalistadoTodasLiquidacion.SelectedCells[14].Value.ToString());

                //////////////////////////--------------------------------------------------------------------------
                //CODIGO IMPLEMENTADO  
                foreach(DataGridViewColumn columns in datalistadoClientesLiquidacion.Columns)
                {
                    if(columns.Index == 0)
                    {
                        columns.ReadOnly = false;
                    }
                    else
                    {
                        columns.ReadOnly = true;
                    }
                }
                
                foreach(DataGridViewColumn columns in datalistadoColaboradoresLiquidacion.Columns)
                {
                    if(columns.Index == 0)
                    {
                        columns.ReadOnly = false;
                    }
                    else
                    {
                        columns.ReadOnly = true;
                    }
                }
                //////////////////////////--------------------------------------------------------------------------


                if (datalistadoTodasLiquidacion.SelectedCells[13].Value.ToString() == "LIQUIDADO" || datalistadoTodasLiquidacion.SelectedCells[12].Value.ToString() == "ANULADO" || estadoActa == true)
                {
                    MessageBox.Show("Esta liquidación ya ha sido revisada por el área contable, esta anulada, aprobada o se genero un acta.", "Validación del Sistema", MessageBoxButtons.OK);
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

                    ////////////////////////////////-----------------------------------------------------------
                    //CODIGO IMPLEMENTADO-----------------------------------------------
                    Evitar_Clientes_Duplicados(datalistadoBusquedaClietneLiquidacion, datalistadoClientesLiquidacion);
                    Evitar_Colaboradores_Duplicados(datalistadoBusquedaColaboradorLiquidacion, datalistadoColaboradoresLiquidacion);
                    ////////////////////////////////-----------------------------------------------------------
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una liquidación para poder editarla.", "Validación del Sistema", MessageBoxButtons.OK);
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
                MessageBox.Show("No hay registro en el detalle para poder remover.", "Validación del sistema", MessageBoxButtons.OK);
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
            //////////////////////////--------------------------------------------------------------------------
            //CODIGO IMPLEMENTADO
            ValidarFilasVacias(datalistadoDetallesLiquidacion);
            ValidarFechaVacias_Liquidacion(datalistadoClientesLiquidacion);
            if(filasvacias == true)
            {
                return;
            }
            else
            {
                //////////////////////////--------------------------------------------------------------------------
                if (rbNacionalLiquidacion.Checked == false && rbExteriorLiquidacion.Checked == false)
                {
                    MessageBox.Show("No se ha seleccionado el tipo de liquidación correctamente.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    if (datalistadoClientesLiquidacion.RowCount == 0 || datalistadoColaboradoresLiquidacion.RowCount == 0)
                    {
                        MessageBox.Show("No se han cargado los clientes correctamnete.", "Validación del Sistema", MessageBoxButtons.OK);
                    }
                    else
                    {
                        if (datalistadoDetallesLiquidacion.RowCount == 0)
                        {
                            MessageBox.Show("No se han cargado los detalles de la liquidación correctamente.", "Validación del Sistema", MessageBoxButtons.OK);
                        }
                        else
                        {
                            //////////////////////////--------------------------------------------------------------------------
                            //CODIGO IMPLEMENTADO 
                            if (string.IsNullOrWhiteSpace(txtMotivoViajeLiquidacion.Text) /*== ""*/ || string.IsNullOrWhiteSpace(txtItinerarioViajeLiqudiacion.Text) /*== ""*/)
                            {
                                MessageBox.Show("No se ha cargado el itinerario o motivo de la liquidación.", "Validación del Sistema", MessageBoxButtons.OK);
                            }
                            //////////////////////////--------------------------------------------------------------------------
                            else
                            {
                                if (txtTotaLiquidaciones.Text == "")
                                {
                                    MessageBox.Show("No se ha cargado el total ni el saldo de la liquidación.", "Validación del Sistema", MessageBoxButtons.OK);
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
                                                cmd.Parameters.AddWithValue("@idLiquidacion", idLiquidacion);
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
                                                cmd.Parameters.AddWithValue("@idLiquidacion", idLiquidacion);
                                                cmd.Parameters.AddWithValue("@estadoAsistencia", estadoAsistencia);
                                                cmd.Parameters.AddWithValue("@idvendedordetalle", codigoDetalleColaborador);
                                                cmd.ExecuteNonQuery();
                                                con.Close();
                                            }

                                            MessageBox.Show("Se registró la liquidación exitosamente.", "Validación del Sistema", MessageBoxButtons.OK);

                                            //REINICIAR FORMULARIO DE INGRESO DE REQUERIMIENTO
                                            panelNuevaLiquidadcion.Visible = false;

                                            datalistadoDetallesLiquidacion.Rows.Clear();
                                            datalistadoClientesLiquidacion.Rows.Clear();
                                            datalistadoColaboradoresLiquidacion.Rows.Clear();
                                            datalistadoBusquedaClietneLiquidacion.DataSource = null;
                                            datalistadoBusquedaColaboradorLiquidacion.DataSource = null;
                                            rbNacionalLiquidacion.Checked = false;
                                            rbExteriorLiquidacion.Checked = false;
                                            txtMotivoViajeLiquidacion.Text = "";
                                            txtItinerarioViajeLiqudiacion.Text = "";
                                            txtTotaLiquidaciones.Text = "";
                                            txtAdelantoLiquidaciones.Text = "";
                                            txtSaldoLiquidaciones.Text = "";
                                            txtBusquedaCLienteLiquidacion.Text = "";
                                            txtBusquedaColaboradorLiquidacion.Text = "";
                                            //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                                            ClassResourses.RegistrarAuditora(8, this.Name, 5, Program.IdUsuario, "Editar liquidación de viaje.", Convert.ToInt32(idLiquidacion));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                                        ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                                        MessageBox.Show(ex.Message, "Error en el servidor.", MessageBoxButtons.OK);
                                    }
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

            txtBusquedaCLienteLiquidacion.Text = "";
            txtBusquedaColaboradorLiquidacion.Text = "";
            datalistadoBusquedaClietneLiquidacion.DataSource = null;
            datalistadoBusquedaColaboradorLiquidacion.DataSource = null;    

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

                    if (estadoJefatura == "APROBADO" || estadoJefatura == "ANULADO")
                    {
                        MessageBox.Show("Esta liquidación ya está aprobada o se encuentra anulada.", "Validación del Sistema", MessageBoxButtons.OK);
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

                                MessageBox.Show("Liquidación aprobada exitosamente.", "Validación del Sistema", MessageBoxButtons.OK);
                                BusquedaDependiente();

                                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                                ClassResourses.RegistrarAuditora(3, this.Name, 5, Program.IdUsuario, "Aprobación de liquidación de viaje.", Convert.ToInt32(idLiquidacion));
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
            }
            else
            {
                MessageBox.Show("Debe seleccionar una liquidación para poder aprobarla.", "Validación del Sistema", MessageBoxButtons.OK);
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
                        MessageBox.Show("Esta liquidación tiene una o varias actas generadas o aprobadas, por favor anular por el mantenimiento de actas.", "Validación del Sistema", MessageBoxButtons.OK);
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

                            MessageBox.Show("Liquidación y requerimiento asociado a esta, anuladas exitosamente.", "Validación del Sistema", MessageBoxButtons.OK);
                            BusquedaDependiente();

                            //ClassResourses.Enviar("ynunahuanca@arenassrl.com.pe", "CORREO AUTOMATIZADO - ANULACIÓN DEL LIQUIDACIÓN N°. " + idLiquidacion, "Correo de verificación de anulación de una liquidación por parte del usuario '" + Program.UnoNombreUnoApellidoUsuario + "' el la fecha siguiente: " + DateTime.Now + ". Por favor no responder.");
                            //ClassResourses.Enviar("jhoalexxxcc@gmail.com", "CORREO AUTOMATIZADO - ANULACIÓN DEL LIQUIDACIÓN N°. " + idLiquidacion, "Correo de verificación de anulación de una liquidación por parte del usuario '" + Program.UnoNombreUnoApellidoUsuario + "' el la fecha siguiente: " + DateTime.Now + ". Por favor no responder.");

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
            }
            else
            {
                MessageBox.Show("Debe seleccionar una liquidación para poder anular.", "Validación del Sistema", MessageBoxButtons.OK);
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
                datalistadoLiquidacionActas.Columns[9].Width = 70;
                datalistadoLiquidacionActas.Columns[10].Width = 80;

                ColoresListadoDetalleLiquidación();
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //ABRIR DETALLES DE LA LIQUIDACIÓN----------------------------------------------------------------------------------------------------------
        //BUSCAR REQUERIMEINTO DE LA LIQUDAICIONES
        public void TraerDatosRequePorId(int idReque)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Liquidacion_BuscarRequerimientoDash", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idRequerimiento", idReque);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoDatosReque.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //BUSCAR CLIENTES DE MI REQUERIMIENTO
        public void TraerDatosRequeClientesPorId(int idReque)
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
                cmd.Parameters.AddWithValue("@codigo", idReque);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoClientesRequeDash.DataSource = dt;
                con.Close();
                datalistadoClientesRequeDash.Columns[0].Visible = false;
                datalistadoClientesRequeDash.Columns[1].Visible = false;
                datalistadoClientesRequeDash.Columns[2].Visible = false;
                datalistadoClientesRequeDash.Columns[4].Visible = false;
                datalistadoClientesRequeDash.Columns[6].Visible = false;
                datalistadoClientesRequeDash.Columns[3].Width = 235;
                datalistadoClientesRequeDash.Columns[5].Width = 80;
                datalistadoClientesRequeDash.Columns[7].Width = 80;
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //BUSCAR CLIENTES DE MI LIQUIDACION
        public void TraerDatosLiquiClientesPorId(int idLiqui)
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
                cmd.Parameters.AddWithValue("@codigo", idLiqui);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoClientesLiquiDash.DataSource = dt;
                con.Close();
                datalistadoClientesLiquiDash.Columns[0].Visible = false;
                datalistadoClientesLiquiDash.Columns[1].Visible = false;
                datalistadoClientesLiquiDash.Columns[2].Visible = false;
                datalistadoClientesLiquiDash.Columns[3].Visible = false;
                datalistadoClientesLiquiDash.Columns[5].Visible = false;
                datalistadoClientesLiquiDash.Columns[8].Visible = false;
                datalistadoClientesLiquiDash.Columns[9].Visible = false;
                datalistadoClientesLiquiDash.Columns[10].Visible = false;
                datalistadoClientesLiquiDash.Columns[4].Width = 235;
                datalistadoClientesLiquiDash.Columns[6].Width = 80;
                datalistadoClientesLiquiDash.Columns[7].Width = 80;
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //BUSCAR REQUERIMEINTO VENDEODRES DE LA LIQUDAICIONES
        public void TraerDatosRequeVendedoresPorId(int idReque)
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
                cmd.Parameters.AddWithValue("@codigo", idReque);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoVendedoresRequeDash.DataSource = dt;
                con.Close();
                datalistadoVendedoresRequeDash.Columns[0].Visible = false;
                datalistadoVendedoresRequeDash.Columns[1].Visible = false;
                datalistadoVendedoresRequeDash.Columns[2].Visible = false;
                datalistadoVendedoresRequeDash.Columns[3].Width = 235;
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //BUSCAR LIQUIDACION VENDEODRES DE LA LIQUDAICIONES
        public void TraerDatosLiquiVendedoresPorId(int idLiqui)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Reporte_MostrarLiquidacionColaboradoresSegunCodigo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idLiquidacion", idLiqui);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoVendedoresLiquiDash.DataSource = dt;
                con.Close();
                datalistadoVendedoresLiquiDash.Columns[0].Width = 235;
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //BUSCAR ACTA POR LIQUDIACION
        public void TraerDatosActaPorIdLiqui(int idLiqui)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarActaDatos_Dash", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idLiquidacion", idLiqui);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoDatosActaDahs.DataSource = null;
                datalistadoDatosActaDahs.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //BUSCAR ACTA POR LIQUDIACION
        public void TraerDatosActaPorId(int idLiqui)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarActaDatos_Dash2", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idActa", idLiqui);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoDatosActaDahs.DataSource = null;
                datalistadoDatosActaDahs.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //SELECCION DEL DETALLE Y SEGUIMEINTO DE MI LIQUIDACION
        private void datalistadoTodasLiquidacion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoTodasLiquidacion.CurrentRow != null)
            {
                int idLiquidacion = Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[1].Value.ToString());
                int idRequerimiento = Convert.ToInt32(datalistadoTodasLiquidacion.SelectedCells[2].Value.ToString());
                DataGridViewColumn currentColumnT = datalistadoTodasLiquidacion.Columns[e.ColumnIndex];

                if (currentColumnT.Name == "btnSeguimiento")
                {
                    cboTipoVisualizacion.SelectedIndex = 0;
                    panelDetalleRQ.Visible = true;

                    //CONSULTAR MI REQUERIMEINTO
                    TraerDatosRequePorId(idRequerimiento);
                    TraerDatosRequeClientesPorId(idRequerimiento);
                    TraerDatosRequeVendedoresPorId(idRequerimiento);
                    TraerDatosLiquiClientesPorId(idLiquidacion);
                    TraerDatosLiquiVendedoresPorId(idLiquidacion);
                    TraerDatosActaPorIdLiqui(idLiquidacion);
                    txtEstadoComerRequeDash.Text = datalistadoDatosReque.SelectedCells[8].Value.ToString();
                    txtEstadoContaRequeDash.Text = datalistadoDatosReque.SelectedCells[9].Value.ToString();
                    lblCodigoRequeDash.Text = Convert.ToString(idRequerimiento);
                    txtMontoRequeDash.Text = datalistadoDatosReque.SelectedCells[7].Value.ToString();
                    txtResponsableRequeDash.Text = datalistadoDatosReque.SelectedCells[4].Value.ToString();

                    if (txtEstadoComerRequeDash.Text == "APROBADO" && txtEstadoContaRequeDash.Text == "ATENDIDO")
                    {
                        imgRequerimientoColor.Visible = true;
                        imgRequerimientoMixto.Visible = false;
                        imgRequerimientoMono.Visible = false;
                        flechaRequeColor.Visible = true;
                        flechaRequeSemi.Visible = false;
                        flechaRequeMono.Visible = false;
                        lblEstadoRequeDash.Text = "REQUE. APROBADO Y ATENDIDO";
                        lblEstadoRequeDash.ForeColor = System.Drawing.Color.ForestGreen;
                    }
                    else if (txtEstadoComerRequeDash.Text == "APROBADO" && txtEstadoContaRequeDash.Text == "NO ATENDIDO")
                    {
                        imgRequerimientoColor.Visible = false;
                        imgRequerimientoMixto.Visible = true;
                        imgRequerimientoMono.Visible = false;
                        flechaRequeColor.Visible = false;
                        flechaRequeSemi.Visible = true;
                        flechaRequeMono.Visible = false;
                        lblEstadoRequeDash.Text = "REQUE. APROBADO SIN ATENCIÓN";
                        lblEstadoRequeDash.ForeColor = System.Drawing.Color.Peru;
                    }
                    else if (txtEstadoComerRequeDash.Text == "PENDIENTE" && txtEstadoContaRequeDash.Text == "NO ATENDIDO" || txtEstadoComerRequeDash.Text == "ANULADO" || txtEstadoContaRequeDash.Text == "ANULADO")
                    {
                        imgRequerimientoColor.Visible = false;
                        imgRequerimientoMixto.Visible = false;
                        imgRequerimientoMono.Visible = true;
                        flechaRequeColor.Visible = false;
                        flechaRequeSemi.Visible = false;
                        flechaRequeMono.Visible = true;
                        lblEstadoRequeDash.Text = "REQUE. PENDIENTE SIN ATENCIÓN";
                        lblEstadoRequeDash.ForeColor = System.Drawing.Color.Black;
                    }

                    //DATOS DE LA LIQUIDACIÓN
                    lblCodigoLiquiDash.Text = Convert.ToString(idLiquidacion);
                    txtMontoLiquiDash.Text = datalistadoTodasLiquidacion.SelectedCells[9].Value.ToString();
                    txtAdelantoLiquiDash.Text = datalistadoTodasLiquidacion.SelectedCells[10].Value.ToString();
                    txtSaldoLiquiDash.Text = datalistadoTodasLiquidacion.SelectedCells[11].Value.ToString();
                    txtResponsableLiquiDash.Text = datalistadoTodasLiquidacion.SelectedCells[7].Value.ToString();

                    txtEstadoComerLiquiDash.Text = datalistadoTodasLiquidacion.SelectedCells[12].Value.ToString();
                    txtEstadoContaLiquiDash.Text = datalistadoTodasLiquidacion.SelectedCells[13].Value.ToString();

                    if (txtEstadoComerLiquiDash.Text == "APROBADO" && txtEstadoContaLiquiDash.Text == "LIQUIDADO")
                    {
                        imgLiquidacionColor.Visible = true;
                        imgLiquidacionMixto.Visible = false;
                        imgLiquidacionMono.Visible = false;
                        flechaLiquiColor.Visible = true;
                        flechaLiquiMixto.Visible = false;
                        flechaLiquiMono.Visible = false;
                        lblEstadoLiquiDash.Text = "LIQUI. APROBADO Y LIQUIDADA";
                        lblEstadoLiquiDash.ForeColor = System.Drawing.Color.ForestGreen;
                    }
                    else if (txtEstadoComerLiquiDash.Text == "APROBADO" && txtEstadoContaLiquiDash.Text == "POR LIQUIDAR")
                    {
                        imgLiquidacionColor.Visible = false;
                        imgLiquidacionMixto.Visible = true;
                        imgLiquidacionMono.Visible = false;
                        flechaLiquiColor.Visible = false;
                        flechaLiquiMixto.Visible = true;
                        flechaLiquiMono.Visible = false;
                        lblEstadoLiquiDash.Text = "LIQUI. APROBADO Y POR LIQUIDAR";
                        lblEstadoLiquiDash.ForeColor = System.Drawing.Color.Peru;
                    }
                    else if (txtEstadoComerLiquiDash.Text == "PENDIENTE" && txtEstadoContaLiquiDash.Text == "POR LIQUIDAR" || txtEstadoComerLiquiDash.Text == "ANULADO" || txtEstadoContaLiquiDash.Text == "ANULADO")
                    {
                        imgLiquidacionColor.Visible = false;
                        imgLiquidacionMixto.Visible = false;
                        imgLiquidacionMono.Visible = true;
                        flechaLiquiColor.Visible = false;
                        flechaLiquiMixto.Visible = false;
                        flechaLiquiMono.Visible = true;
                        lblEstadoLiquiDash.Text = "LIQUI. PENDIENTE Y POR LIQUIDAR";
                        lblEstadoLiquiDash.ForeColor = System.Drawing.Color.Black;
                    }

                    //CARGA DE DATOS DE ACTA
                    cboCodigoActaDash.Items.Clear(); // Limpia los valores anteriores

                    foreach (DataGridViewRow fila in datalistadoDatosActaDahs.Rows)
                    {
                        if (fila.Cells["ID ACTA"].Value != null)
                        {
                            cboCodigoActaDash.Items.Add(fila.Cells["ID ACTA"].Value.ToString());
                        }
                    }

                    //VALIDAR SI HAY PEDIDO
                    if (cboCodigoActaDash.Items.Count != 0)
                    {
                        cboCodigoActaDash.SelectedIndex = 0;
                    }
                    else
                    {
                        txtClienteActaDas.Text = "";
                        txtUnidadActaDash.Text = "";
                        txtAsistente1ActaDash.Text = "";
                        txtAsistente2ActaDash.Text = "";
                        txtAsistente3ActaDash.Text = "";

                        imgActaColor.Visible = false;
                        imgActaMixto.Visible = false;
                        imgActaMono.Visible = true;
                        flechaActaColor.Visible = false;
                        flechaActaMixto.Visible = false;
                        flechaActaMono.Visible = true;
                        lblEstadoActaDash.Text = "SIN REGISTRO";
                        lblEstadoActaDash.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
        }

        //CARGA DE DATOSW DE MI ACTA SEGUN EL CODIGO SELECCIONADO
        private void cboCodigoActaDash_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCodigoActaDash.Text != null)
            {
                TraerDatosActaPorId(Convert.ToInt32(cboCodigoActaDash.Text));

                dtDesde.Value = Convert.ToDateTime(datalistadoDatosActaDahs.SelectedCells[1].Value.ToString());
                dtHasta.Value = Convert.ToDateTime(datalistadoDatosActaDahs.SelectedCells[2].Value.ToString());
                txtClienteActaDas.Text = datalistadoDatosActaDahs.SelectedCells[6].Value.ToString();
                txtUnidadActaDash.Text = datalistadoDatosActaDahs.SelectedCells[7].Value.ToString();
                txtAsistente1ActaDash.Text = datalistadoDatosActaDahs.SelectedCells[3].Value.ToString();
                txtAsistente2ActaDash.Text = datalistadoDatosActaDahs.SelectedCells[4].Value.ToString();
                txtAsistente3ActaDash.Text = datalistadoDatosActaDahs.SelectedCells[5].Value.ToString();
                txtEstadoDashDash.Text = datalistadoDatosActaDahs.SelectedCells[8].Value.ToString();

                if (txtEstadoDashDash.Text == "APROBADO")
                {
                    imgActaColor.Visible = true;
                    imgActaMixto.Visible = false;
                    imgActaMono.Visible = false;
                    flechaActaColor.Visible = true;
                    flechaActaMixto.Visible = false;
                    flechaActaMono.Visible = false;
                    lblEstadoActaDash.Text = "ACTA APROBADA";
                    lblEstadoActaDash.ForeColor = System.Drawing.Color.ForestGreen;
                }
                else if (txtEstadoDashDash.Text == "CULMINADO")
                {
                    imgActaColor.Visible = false;
                    imgActaMixto.Visible = true;
                    imgActaMono.Visible = false;
                    flechaActaColor.Visible = false;
                    flechaActaMixto.Visible = true;
                    flechaActaMono.Visible = false;
                    lblEstadoActaDash.Text = "ACTA CULMINADA";
                    lblEstadoActaDash.ForeColor = System.Drawing.Color.Peru;
                }
                else if (txtEstadoDashDash.Text == "PENDIENTE" || txtEstadoDashDash.Text == "ANULADO")
                {
                    imgActaColor.Visible = false;
                    imgActaMixto.Visible = false;
                    imgActaMono.Visible = true;
                    flechaActaColor.Visible = false;
                    flechaActaMixto.Visible = false;
                    flechaActaMono.Visible = true;
                    lblEstadoActaDash.Text = "ACTA PENDIENTE";
                    lblEstadoActaDash.ForeColor = System.Drawing.Color.Black;
                }
            }
            else
            {
                txtClienteActaDas.Text = "";
                txtUnidadActaDash.Text = "";
                txtAsistente1ActaDash.Text = "";
                txtAsistente2ActaDash.Text = "";
                txtAsistente3ActaDash.Text = "";

                imgActaColor.Visible = false;
                imgActaMixto.Visible = false;
                imgActaMono.Visible = true;
                flechaActaColor.Visible = false;
                flechaActaMixto.Visible = false;
                flechaActaMono.Visible = true;
                lblEstadoActaDash.Text = "SIN REGISTRO";
                lblEstadoActaDash.ForeColor = System.Drawing.Color.Black;
            }
        }

        //CARGA DE DATOSW DE MI ACTA SEGUN EL CODIGO SELECCIONADO
        private void cboCodigoActaDash_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboCodigoActaDash.Text != null)
            {
                TraerDatosActaPorId(Convert.ToInt32(cboCodigoActaDash.Text));

                dtDesde.Value = Convert.ToDateTime(datalistadoDatosActaDahs.SelectedCells[1].Value.ToString());
                dtHasta.Value = Convert.ToDateTime(datalistadoDatosActaDahs.SelectedCells[2].Value.ToString());
                txtClienteActaDas.Text = datalistadoDatosActaDahs.SelectedCells[6].Value.ToString();
                txtUnidadActaDash.Text = datalistadoDatosActaDahs.SelectedCells[7].Value.ToString();
                txtAsistente1ActaDash.Text = datalistadoDatosActaDahs.SelectedCells[3].Value.ToString();
                txtAsistente2ActaDash.Text = datalistadoDatosActaDahs.SelectedCells[4].Value.ToString();
                txtAsistente3ActaDash.Text = datalistadoDatosActaDahs.SelectedCells[5].Value.ToString();
                txtEstadoDashDash.Text = datalistadoDatosActaDahs.SelectedCells[8].Value.ToString();

                if (txtEstadoDashDash.Text == "APROBADO")
                {
                    imgActaColor.Visible = true;
                    imgActaMixto.Visible = false;
                    imgActaMono.Visible = false;
                    flechaActaColor.Visible = true;
                    flechaActaMixto.Visible = false;
                    flechaActaMono.Visible = false;
                    lblEstadoActaDash.Text = "ACTA APROBADA";
                    lblEstadoActaDash.ForeColor = System.Drawing.Color.ForestGreen;
                }
                else if (txtEstadoDashDash.Text == "CULMINADO")
                {
                    imgActaColor.Visible = false;
                    imgActaMixto.Visible = true;
                    imgActaMono.Visible = false;
                    flechaActaColor.Visible = false;
                    flechaActaMixto.Visible = true;
                    flechaActaMono.Visible = false;
                    lblEstadoActaDash.Text = "ACTA CULMINADA";
                    lblEstadoActaDash.ForeColor = System.Drawing.Color.Peru;
                }
                else if (txtEstadoDashDash.Text == "PENDIENTE" || txtEstadoDashDash.Text == "ANULADO")
                {
                    imgActaColor.Visible = false;
                    imgActaMixto.Visible = false;
                    imgActaMono.Visible = true;
                    flechaActaColor.Visible = false;
                    flechaActaMixto.Visible = false;
                    flechaActaMono.Visible = true;
                    lblEstadoActaDash.Text = "ACTA PENDIETE";
                    lblEstadoActaDash.ForeColor = System.Drawing.Color.Black;
                }
            }
            else
            {
                txtClienteActaDas.Text = "";
                txtUnidadActaDash.Text = "";
                txtAsistente1ActaDash.Text = "";
                txtAsistente2ActaDash.Text = "";
                txtAsistente3ActaDash.Text = "";

                imgActaColor.Visible = false;
                imgActaMixto.Visible = false;
                imgActaMono.Visible = true;
                flechaActaColor.Visible = false;
                flechaActaMixto.Visible = false;
                flechaActaMono.Visible = true;
                lblEstadoActaDash.Text = "SIN REGISTRO";
                lblEstadoActaDash.ForeColor = System.Drawing.Color.Black;
            }
        }

        //VISUALIZAR EL REQUERIMIENTO
        private void btnVisualizarRequerimientoDash_Click(object sender, EventArgs e)
        {
            try
            {
                string codigoRequerimientoReporte = lblCodigoRequeDash.Text;

                //SI EL REQUERIMEINTO ESTÁ ANULADO POR EL ÁREA COMERCIAL Y YA TIENE LIQUIDACIÓN CREADA
                if (txtEstadoComerRequeDash.Text == "ANULADO")
                {
                    //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO DESAPROBADO
                    Visualizadores.VisualizarRequerimientoDesaprobado frm = new Visualizadores.VisualizarRequerimientoDesaprobado();
                    frm.lblCodigo.Text = codigoRequerimientoReporte;
                    //CARGAR VENTANA
                    frm.Show();
                }
                //SI EL REQUERIMEINTO ESTÁ EN PENDIENTE
                else if (txtEstadoComerRequeDash.Text == "PENDIENTE")
                {
                    //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO GENERAL
                    Visualizadores.VisualizarRequerimientoVenta frm = new Visualizadores.VisualizarRequerimientoVenta();
                    frm.lblCodigo.Text = codigoRequerimientoReporte;
                    //CARGAR VENTANA
                    frm.Show();
                }
                //SI EL REQUERIMEINTO ESTÁ APROBADO POR EL ÁREA COMERCIAL O ESTA EN PENDIENTE
                else if (txtEstadoComerRequeDash.Text == "APROBADO")
                {
                    //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO GENERAL
                    Visualizadores.VisualizarRequerimientoAprobado frm = new Visualizadores.VisualizarRequerimientoAprobado();
                    frm.lblCodigo.Text = codigoRequerimientoReporte;
                    //CARGAR VENTANA
                    frm.Show();
                }
                //SI EL REQUERIMEINTO NO ENTRA A NINGUNA DE LAS OPCIONES ANTERIORES
                else
                {
                    //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO DESAPROBADO
                    Visualizadores.VisualizarRequerimientoDesaprobado frm = new Visualizadores.VisualizarRequerimientoDesaprobado();
                    frm.lblCodigo.Text = codigoRequerimientoReporte;
                    //CARGAR VENTANA
                    frm.Show();
                }

                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(6, this.Name, 5, Program.IdUsuario, "Visualización de requerimiento de viaje PDF", Convert.ToInt32(codigoRequerimientoReporte));
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
            }
        }

        //VISUALIZAR LA LIQUIDACION
        private void btnVisualizarLiquidacionDash_Click(object sender, EventArgs e)
        {
            try
            {
                string codigoLiquidacionReporte = lblCodigoLiquiDash.Text;

                if (datalistadoTodasLiquidacion.SelectedCells[12].Value.ToString() == "ANULADO")
                {
                    Visualizadores.VisualizarLiquidacionDesaprobada frm = new Visualizadores.VisualizarLiquidacionDesaprobada();
                    frm.lblCodigo.Text = codigoLiquidacionReporte;

                    frm.Show();
                }
                else if (datalistadoTodasLiquidacion.SelectedCells[12].Value.ToString() == "APROBADO")
                {
                    Visualizadores.VisualizarLiquidacionAprobada frm = new Visualizadores.VisualizarLiquidacionAprobada();
                    frm.lblCodigo.Text = codigoLiquidacionReporte;

                    frm.Show();
                }
                else
                {
                    Visualizadores.VisualizarLiquidacionesVenta frm = new Visualizadores.VisualizarLiquidacionesVenta();
                    frm.lblCodigo.Text = codigoLiquidacionReporte;

                    frm.Show();
                }

                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(6, this.Name, 5, Program.IdUsuario, "Visualización de liquidación de viaje PDF", Convert.ToInt32(codigoLiquidacionReporte));
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //VISUALIZAR EL ACTA
        private void btnVisualizarActaDash_Click(object sender, EventArgs e)
        {
            try
            {
                string codigoActaReporte = cboCodigoActaDash.Text;

                if (codigoActaReporte != "")
                {
                    if (txtEstadoDashDash.Text == "PENDIENTE")
                    {
                        Visualizadores.VisualizarActa frm = new Visualizadores.VisualizarActa();
                        frm.lblCodigo.Text = codigoActaReporte;

                        frm.Show();
                    }
                    else if (txtEstadoDashDash.Text == "APROBADO")
                    {
                        Visualizadores.VisualizarActaAprobada frm = new Visualizadores.VisualizarActaAprobada();
                        frm.lblCodigo.Text = codigoActaReporte;

                        frm.Show();
                    }
                    else if (txtEstadoDashDash.Text == "CULMINADO")
                    {
                        Visualizadores.VisualizarActa frm = new Visualizadores.VisualizarActa();
                        frm.lblCodigo.Text = codigoActaReporte;

                        frm.Show();
                    }
                    else
                    {
                        Visualizadores.VisualizarActaDesaprobada frm = new Visualizadores.VisualizarActaDesaprobada();
                        frm.lblCodigo.Text = codigoActaReporte;

                        frm.Show();
                    }

                    //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                    ClassResourses.RegistrarAuditora(6, this.Name, 5, Program.IdUsuario, "Visualización del acta de viaje PDF", Convert.ToInt32(codigoActaReporte));
                }
                else
                {
                    MessageBox.Show("No hay acta registrada.", "Validación del Sistema", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
            }
        }

        //SALIR DE MI DASH
        private void btnSalirSeguimiento_Click(object sender, EventArgs e)
        {
            panelDetalleRQ.Visible = false;
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
                try
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
                catch (Exception ex)
                {
                    //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                    ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                    MessageBox.Show("Error en la operación por: " + ex.Message);
                }
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
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
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
                            MessageBox.Show("No se puede continuar con la generación de esta acta hasta que se observe el resto de liquidaciones faltantes.", "Validación del Sistema", MessageBoxButtons.OK);
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

                                if (contactosvacios == true)
                                {
                                    MessageBox.Show("El cliente seleccionado no tiene contactos ingresados", "Validación del Sistema", MessageBoxButtons.OK);                                  
                                    return;
                                }

                                else
                                {
                                    txtContactoCliente1NuevaActa.SelectedIndex = -1;
                                    txtContactoCliente2NuevaActa.SelectedIndex = -1;
                                    txtContactoCliente3NuevaActa.SelectedIndex = -1;

                                    panelNuevaActa.Visible = true;
                                    datalistadoLiquidacionActas.Enabled = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Esta liquidación ha sido anulada.", "Validación del Sistema", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("Solo puede hacer el proceso el responsable de este.", "Validación del Sistema", MessageBoxButtons.OK);
                }

            }
        }
        //-------------------------------------------------------------------------------------

        //GENERACIÓN Y GUARDADO DEL ACTA--------------------------------------------------------
        private void btnGuardarNuevaActa_Click(object sender, EventArgs e)
        {
            if (rbTipoClienteActualNuevaActa.Checked == false && rbTipoClienteFuturoNuevaActa.Checked == false)
            {
                MessageBox.Show("Debe seleccionar un tipo de cliente.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                if (rbFrecuenciaAltaNuevaActa.Checked == false && rbFrecuenciaMediaNuevaActa.Checked == false && rbFrecuenduaBajaNuevaActa.Checked == false)
                {
                    MessageBox.Show("Debe seleccionar una frecuencia y volúmen de compra.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    if (txtContactoCliente1NuevaActa.Text == "" && txtContactoCliente2NuevaActa.Text == "" && txtContactoCliente3NuevaActa.Text == "")
                    {
                        MessageBox.Show("Debe seleccionar al menos un contacto del cliente.", "Validación del Sistema", MessageBoxButtons.OK);
                    }
                    else
                    {
                        if (txtAsistentes1NuevaActa.Text == "" && txtAsistentes2NuevaActa.Text == "" && txtAsistentes3NuevaActa.Text == "")
                        {
                            MessageBox.Show("Debe seleccionar al menos un Asistente", "Validación del Sistema", MessageBoxButtons.OK);
                        }
                        else
                        {
                            if (ckSostenimientoNuevaActa.Checked == false && ckCaptacionNuevaActa.Checked == false && ckRecuperacionNuevaActa.Checked == false && ckReclamoNuevaActa.Checked == false)
                            {
                                MessageBox.Show("Debe seleccionar un objetivo de viaje", "Validación del Sistema", MessageBoxButtons.OK);
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

                                        MessageBox.Show("Se generó el acta correctamente en el sistema.", "Validación del Sistema", MessageBoxButtons.OK);

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
                                        ckPresenteAsistente1.Checked = false;
                                        datalistadoLiquidacionActas.Enabled = true;

                                        //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                                        ClassResourses.RegistrarAuditora(4, this.Name, 5, Program.IdUsuario, "Generar acta de viaje", Convert.ToInt32(numeroActa));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                                    ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                                    MessageBox.Show(ex.Message, "Error en el servidor");
                                }
                            }
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
            ckPresenteAsistente1.Checked = false;
        }

        //LIMPIEZA DE CAMPOS LLENADOS - ASISTENTES Y CONTACTOS DEL CLIENTE
        private void btnCargarDatosAsistente2NuevaActa_Click(object sender, EventArgs e)
        {
            txtAsistentes2NuevaActa.SelectedIndex = -1;
            //txtAsistentes2NuevaActa.Text = "";
        }

        //LIMPIEZA DE CAMPOS LLENADOS - ASISTENTES Y CONTACTOS DEL CLIENTE
        private void btnCargarDatosAsistente3NuevaActa_Click(object sender, EventArgs e)
        {
            txtAsistentes3NuevaActa.SelectedIndex = -1;
            //txtAsistentes3NuevaActa.Text = "";
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
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
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

                ClassResourses.RegistrarAuditora(5, this.Name, 5, Program.IdUsuario, "Exportar liquidación de viaje PDF", idLiquidacion);

            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show($"Ocurrió un error al exportar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //////////////////////////////////////////////---------------------------------------------------------------------
        ///////////////////--------------------------------------------
        //METODOS VALIDACIONES LIQUIDACIONES

        //METODO QUE BORRA LOS CLIENTES SELECCIONADOS
        public void BorrarSeleccionCliente(DataGridView DGV)
        {
            //SI NO HAY CLIENTES CARGADOS
            if (DGV.Rows.Count > 0)
            {
                //MENSAJE DE CONFIRMACIÓN PARA BORRAR AL CLIENTE SELECCIOANDO
                DialogResult resul = MessageBox.Show("¿Seguro que desea borrar ha este cliente?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (resul == DialogResult.OK)
                {
                    //ACCIÓN DE ELIMINAR
                    DGV.Rows.Remove(DGV.CurrentRow);
                }
            }
            else
            {
                MessageBox.Show("No hay clientes agregados para poder borrarlos.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //METODO QUE BORRA LOS COLABORADORES SELECCIONADOS
        public void BorrarSeleccionColaborador(DataGridView DGV)
        {
            //SI NO HAY COLABORADORES CARGADOS
            if (DGV.Rows.Count > 0)
            {
                //MENSAJE DE CONFIRMACIÓN PARA BORRAR A LOS COLABORADORES SELECCIOANDO
                DialogResult resul = MessageBox.Show("¿Seguro que desea borrar ha este colaborador?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (resul == DialogResult.OK)
                {
                    //ACCIÓN DE ELIMINAR
                    DGV.Rows.Remove(DGV.CurrentRow);
                }
            }
            else
            {
                MessageBox.Show("No hay colaboradores agregados para poder borrarlos.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //CONTROLES QUE BORRAN CLIENTES Y COLABORADORES SELECCIONADOS
        private void btnBorrarSeleccionClienteLiquidacion_Click(object sender, EventArgs e)
        {
            BorrarSeleccionCliente(datalistadoClientesLiquidacion);
        }

        private void btnBorrarColaboradorLiquidacion_Click(object sender, EventArgs e)
        {
            BorrarSeleccionColaborador(datalistadoColaboradoresLiquidacion);
        }

        //METODO QUE VALIDA SI LAS FILAS TIENEN VALORES VACIOS
        public void ValidarFilasVacias(DataGridView DGV)
        {
            foreach (DataGridViewRow rows in DGV.Rows)
            {
                int indice = 0;

                foreach (DataGridViewCell cell in rows.Cells)
                {
                    if (indice >= 1)
                    {
                        if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value.ToString()))
                        {
                            MessageBox.Show("Falta ingresar valores en el Presupuesto de Viaje", "Validación del Sistema", MessageBoxButtons.OK);
                            filasvacias = true;
                            return;
                        }
                        else
                        {
                            filasvacias = false;
                        }
                    }
                    indice++;
                }
            }
        }

        //METODO QUE VALIDA QUE SE SELECCIONE LAS FECHAS EN LA EDICIÓN DE LIQUIDACION
        public void ValidarFechaVacias_Liquidacion(DataGridView DGV)
        {
            foreach (DataGridViewRow rows in DGV.Rows)
            {
                int indicecolumn = 0;

                foreach (DataGridViewCell cell in rows.Cells)
                {
                    if (indicecolumn == 2 || indicecolumn == 4)
                    {
                        if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value.ToString()))
                        {
                            MessageBox.Show("No se ha seleccionado ninguna fecha", "Validación del Sistema");
                            fechasvacias = true;
                            return;
                        }
                        else
                        {
                            fechasvacias = false;
                        }
                    }
                    indicecolumn++;
                }
            }

        }

        ///-------------------------------------------------------
        //------------------------------------
        //METODOS PARA EVITAR CLIENTES Y COLABORADORES DUPLICADOS

        //FUNCION PARA EVITAR CLIENTES DUPLICADOS
        public void Evitar_Clientes_Duplicados(DataGridView DGV1, DataGridView DGV2)
        {

            List<string> codigoseleccionados = new List<string>();

            //recorrido para el listado que contiene los clientes ingresados a la liquidacion
            foreach (DataGridViewRow rows in DGV2.Rows)
            {
                if (rows.Cells[5].Value != null)
                {
                    codigoseleccionados.Add(rows.Cells[5].Value.ToString());
                }
            }

            //recorrido del listado de colaboradores donde verificara si el codigo colaborador es el mismo
            for (int i = DGV1.Rows.Count - 1; i >= 0; i--)
            {
                var fila = DGV1.Rows[i];

                if (fila.Cells[1].Value != null)
                {
                    string codigo = fila.Cells[1].Value.ToString();

                    if (codigoseleccionados.Contains(codigo))
                    {
                        DGV1.Rows.RemoveAt(i);
                    }
                }
            }
        }

        //EVENTO DE AGREGADO DE CLIENTE SELECCIONADO Y BORRADO DEL CLIENTE DE LA LISTA DE BUSQUEDA
        private void datalistadoBusquedaClietneLiquidacion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            else
            {
                DataGridViewColumn currentColumn = datalistadoBusquedaClietneLiquidacion.Columns[e.ColumnIndex];
                //SI HAY 3 O MÁS CLIENTES, YA NOS E PODRAN INGRESAR MÁS
                if (datalistadoClientesLiquidacion.RowCount >= 3)
                {
                    MessageBox.Show("Solo se pueden ingresar un máximo de 3 clientes por liquidación.", "Validación del Sistema", MessageBoxButtons.OK);
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


                        datalistadoBusquedaClietneLiquidacion.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
        }


        //FUNCION PARA EVITAR COLABORADORES DUPLICADOS
        public void Evitar_Colaboradores_Duplicados(DataGridView DGV1, DataGridView DGV2)
        {
            List<string> codigosseleccionados = new List<string>();

            //recorrido para el listado que contiene los colaboradores ingresados a la liquidacion
            foreach (DataGridViewRow rows in DGV2.Rows)
            {
                if (rows.Cells[1].Value != null)
                {
                    codigosseleccionados.Add(rows.Cells[1].Value.ToString());
                }
            }

            //recorrido del listado de colaboradores donde verificara si el codigo colaborador es el mismo
            for (int i = DGV1.Rows.Count - 1; i >= 0; i--)
            {
                var fila = DGV1.Rows[i];

                if (fila.Cells[1].Value != null)
                {
                    string codigo = fila.Cells[1].Value.ToString();

                    if (codigosseleccionados.Contains(codigo))
                    {
                        DGV1.Rows.RemoveAt(i);
                    }
                }
            }
        }

        //EVENTO DE AGREGADO DE COLABORADOR SELECCIONADO Y BORRADO DEL COLABORADOR DE LA LISTA DE BUSQUEDA
        private void datalistadoBusquedaColaboradorLiquidacion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            else
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

                    datalistadoBusquedaColaboradorLiquidacion.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

        //------------------------------------------------------------------------------------------------------
        //---------------------METODOS PARA LOS FILTROS DE BUSQUEDA POR CLIENTE Y COLABORADOR

        //METODO PARA AJUSTAR MIS COLUMNAS DE MI BUSQUEDA DE CLIENTE
        public void AjustarColumnasBusquedaClienteDcoumento(DataGridView DGV)
        {
            DGV.Columns[1].Visible = false;
            DGV.Columns[3].Visible = false;
            DGV.Columns[3].Visible = false;
            DGV.Columns[5].Visible = false;
            DGV.Columns[2].Width = 300;
            DGV.Columns[4].Width = 150;
            DGV.Columns[6].Width = 150;
        }

        //METODO DE FILTROS BUSQUEDA DE CLIENTES
        public void FiltroClientes(string tipobusqueda, string descripcion, DataGridView DGVClien)
        {
            try
            {
                if (tipobusqueda == "NOMBRES")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("BuscarClientePorNombre_Requerimiento", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", descripcion);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGVClien.DataSource = dt;
                    con.Close();
                    AjustarColumnasBusquedaClienteDcoumento(DGVClien);
                }
                else if (tipobusqueda == "DOCUMENTO")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("BuscarClientePorDocumento_Requerimiento", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@documento", descripcion);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGVClien.DataSource = dt;
                    con.Close();
                    AjustarColumnasBusquedaClienteDcoumento(DGVClien);
                }


                Evitar_Clientes_Duplicados(DGVClien, datalistadoClientesLiquidacion);
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
            }
        }

        //EVENTOS DE BUSQUEDA POR DESCRIPCION PARA CLIENTES
        private void txtBusquedaCLienteLiquidacion_TextChanged(object sender, EventArgs e)
        {
            FiltroClientes(cboBusquedaClientesLiquidacion.Text, txtBusquedaCLienteLiquidacion.Text, datalistadoBusquedaClietneLiquidacion);
        }

        //AJUSTAR MIS COLUMNAS DE MI BUSQUEDA DE COLABORADORES
        public void AjusteBusquedaCloba(DataGridView DGV)
        {
            DGV.Columns[1].Visible = false;
            DGV.Columns[2].Width = 420;
        }

        //METODO DE FILTROS BUSQUEDA DE COLABORADORES
        public void FiltroColaboradores(string tipobusqueda, string descripcion, DataGridView DGVColabora)
        {
            try
            {
                if (tipobusqueda == "NOMBRES")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("BuscarColaboradorPorNombre_Requerimiento", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", descripcion);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGVColabora.DataSource = dt;
                    con.Close();
                    AjusteBusquedaCloba(DGVColabora);
                }
                else if (tipobusqueda == "DOCUMENTO")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("BuscarColaboradorPorDocumento_Requerimiento", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@documento", descripcion);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGVColabora.DataSource = dt;
                    con.Close();
                    AjusteBusquedaCloba(DGVColabora);
                }
                Evitar_Colaboradores_Duplicados(DGVColabora, datalistadoColaboradoresLiquidacion);
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 5, Program.IdUsuario, ex.Message, 0);
            }
        }

        //EVENTOS DE BUSQUEDA POR DESCRIPCION PARA COLABORADORES
        private void txtBusquedaColaboradorLiquidacion_TextChanged(object sender, EventArgs e)
        {
            FiltroColaboradores(cboBusquedaColaboradorLiquidacion.Text, txtBusquedaColaboradorLiquidacion.Text, datalistadoBusquedaColaboradorLiquidacion);
        }    
    }
}
    
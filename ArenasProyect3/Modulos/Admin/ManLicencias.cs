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

namespace ArenasProyect3.Modulos.Admin
{
    public partial class ManLicencias : Form
    {
        //CONSTRUCTOR DEL MANTENIMIENTO - MANTENIEMINTO DE LICENCIAS
        public ManLicencias()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI MANTENIMIENTOS
        private void ManLicencias_Load(object sender, EventArgs e)
        {
            Mostrar();
            CargarUsuarios();
            alternarColorFilas(datalistado);

            cboBusquedaLicencia.SelectedIndex = 0;
        }

        //METODO PARA PINTAR DE COLORES LAS FILAS DE MI LSITADO
        public void alternarColorFilas(DataGridView dgv)
        {
            try
            {
                {
                    var withBlock = dgv;
                    withBlock.RowsDefaultCellStyle.BackColor = Color.LightBlue;
                    withBlock.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an unexpected error, " + ex.Message);
            }
        }

        //MÉTODO PARA CARGAR MIS USUARIOS
        public void CargarUsuarios()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdUsuarios , Nombres + ' ' + Apellidos AS [USUARIO] FROM Usuarios WHERE Estado = 'ACTIVO' ORDER BY [USUARIO]", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboPersonalAsignado.DisplayMember = "USUARIO";
            cboPersonalAsignado.ValueMember = "IdUsuarios";
            cboPersonalAsignado.DataSource = dt;
        }

        //METODO PARA VISUALIZAR LOS DATOS, LISTADO DE DATOS EN MI GRILLA
        public void Mostrar()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            da = new SqlDataAdapter("SELECT IdLicencia AS [CODE], Titulo AS [LICENSE TITLE], Maquina AS [MACHINE],  Placa AS [MOTHERBOARD], NumeroIdentificador AS [IDENTIFICATION NUMBER], Usuario AS [DEVICE USER], PersonalAsignado AS [ASSIGNED PERSONNEL], Anotaciones AS [OBSERVATIONS], Estado AS [STATE] FROM TablaLicencias WHERE Estado = 1", con);
            da.Fill(dt);
            datalistado.DataSource = dt;
            con.Close();
            Redimencionar(datalistado);
        }

        //EVENTO DE DOBLE CLICK PARA PODER VISUALIZAR LOS DATOS DE UN REGISTRO
        private void datalistado_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Reiniciar();

            lblCodigo.Text = datalistado.SelectedCells[0].Value.ToString();
            txtTituloLicencia.Text = datalistado.SelectedCells[1].Value.ToString();
            txtMaquina.Text = datalistado.SelectedCells[2].Value.ToString();
            txtPlaca.Text = datalistado.SelectedCells[3].Value.ToString();
            txtNumeroIdentificador.Text = datalistado.SelectedCells[4].Value.ToString();
            txtDispositivoUsuario.Text = datalistado.SelectedCells[5].Value.ToString();
            cboPersonalAsignado.Text = datalistado.SelectedCells[6].Value.ToString();
            txtObservaciones.Text = datalistado.SelectedCells[7].Value.ToString();
            string estado = datalistado.SelectedCells[8].Value.ToString();

            if (estado == "1") { cboEstado.Text = "ACTIVO"; } else { cboEstado.Text = "INACTIVO"; }
        }

        //ACCIONES Y PROCESOS DEL MANTENIMIENTO*--------------------------------------
        //HABILITAR EL GUARDAR DE MI MANTENIMIENTO
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            txtTituloLicencia.ReadOnly = false;
            txtPlaca.ReadOnly = false;
            txtNumeroIdentificador.ReadOnly = false;
            txtObservaciones.ReadOnly = false;

            btnGuardar.Visible = false;
            btnGuardar2.Visible = true;
            btnEditar.Visible = true;
            btnEditar2.Visible = true;

            Cancelar.Visible = true;
            lblCancelar.Visible = true;

            cboEstado.Text = "ACTIVO";
            txtTituloLicencia.Text = "";
            txtMaquina.Text = Environment.MachineName;
            txtPlaca.Text = "";
            txtNumeroIdentificador.Text = "";
            txtDispositivoUsuario.Text = Environment.UserName;
            cboPersonalAsignado.Text = "";
            txtObservaciones.Text = "";
            lblCodigo.Text = "N";
        }

        //ACCION DE GAURDAR EN MI BASE DE DATOS
        private void btnGuardar2_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("Do you really want to save this new license?", "System Validation", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                try
                {
                    if (txtTituloLicencia.Text != "" && txtMaquina.Text != "" && txtPlaca.Text != "" && txtNumeroIdentificador.Text != "" && txtDispositivoUsuario.Text != "" && cboPersonalAsignado.Text != "")
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("InsertarLicenciaSistema", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tituloLicencia", txtTituloLicencia.Text);
                        cmd.Parameters.AddWithValue("@maquina", txtMaquina.Text);
                        cmd.Parameters.AddWithValue("@placa", txtPlaca.Text);
                        cmd.Parameters.AddWithValue("@numeroIdentificador", txtNumeroIdentificador.Text);
                        cmd.Parameters.AddWithValue("@dispositivoUsuario", txtDispositivoUsuario.Text);
                        cmd.Parameters.AddWithValue("@personalAsignado", cboPersonalAsignado.Text);
                        cmd.Parameters.AddWithValue("@observaciones", txtObservaciones.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("The new record was entered correctly, operation record: 148x17c8478q945v7484vbbe84846125", "New Registration", MessageBoxButtons.OK);
                        Mostrar();
                        Reiniciar();
                    }
                    else
                    {
                        MessageBox.Show("You must enter all required fields", "System Validation", MessageBoxButtons.OK);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //HABILITAR EL EDITADO DE MI MANTENIMIENTO
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (lblCodigo.Text != "N")
            {
                txtTituloLicencia.ReadOnly = false;
                txtPlaca.ReadOnly = false;
                txtNumeroIdentificador.ReadOnly = false;
                txtObservaciones.ReadOnly = false;

                btnGuardar.Visible = true;
                btnGuardar2.Visible = true;
                btnEditar.Visible = false;
                btnEditar2.Visible = true;

                Cancelar.Visible = true;
                lblCancelar.Visible = true;

                cboEstado.Text = "ACTIVO";
            }
            else
            {
                MessageBox.Show("You must select a record to be able to edit it.", "System Validation");
            }
        }

        //ACCION DE EDITADO EN MI BASE DE DATOS
        private void btnEditar2_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("Do you really want to edit this license?", "System Validation", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                try
                {
                    if (txtTituloLicencia.Text != "" && txtMaquina.Text != "" && txtPlaca.Text != "" && txtNumeroIdentificador.Text != "" && txtDispositivoUsuario.Text != "" && cboPersonalAsignado.Text != "")
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("EditarLicenciaSistema", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idLicencia", lblCodigo.Text);
                        cmd.Parameters.AddWithValue("@tituloLicencia", txtTituloLicencia.Text);
                        cmd.Parameters.AddWithValue("@placa", txtPlaca.Text);
                        cmd.Parameters.AddWithValue("@numeroIdentificador", txtNumeroIdentificador.Text);
                        cmd.Parameters.AddWithValue("@personalAsignado", cboPersonalAsignado.Text);
                        cmd.Parameters.AddWithValue("@observaciones", txtObservaciones.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("New record edited successfully, operation log: 148x17c8478q945v7484vbbe84846125", "Edited Record", MessageBoxButtons.OK);
                        Mostrar();
                        Reiniciar();
                    }
                    else
                    {
                        MessageBox.Show("You must enter all required fields", "System Validation", MessageBoxButtons.OK);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //ACCIÓN DE CANCELAR LA OPERACIÓN 
        private void Cancelar_Click(object sender, EventArgs e)
        {
            Reiniciar();
        }

        //FUNCION PARA REINICIAR TODOS LOS CAMPOS DESPUES DE USARLOS
        public void Reiniciar()
        {
            txtTituloLicencia.ReadOnly = true;
            txtMaquina.ReadOnly = true;
            txtPlaca.ReadOnly = true;
            txtNumeroIdentificador.ReadOnly = true;
            txtDispositivoUsuario.ReadOnly = true;
            txtObservaciones.ReadOnly = true;

            btnGuardar.Visible = true;
            btnGuardar2.Visible = true;
            btnEditar.Visible = true;
            btnEditar2.Visible = true;

            Cancelar.Visible = false;
            lblCancelar.Visible = false;

            cboEstado.Text = "ACTIVO";
            txtTituloLicencia.Text = "";
            txtMaquina.Text = "";
            txtPlaca.Text = "";
            txtNumeroIdentificador.Text = "";
            txtDispositivoUsuario.Text = "";
            cboPersonalAsignado.Text = "";
            txtObservaciones.Text = "";
            lblCodigo.Text = "N";
        }

        //FUNCION PARA REDIMENCIONAR MI LISTADO
        public void Redimencionar(DataGridView DGV)
        {
            DGV.Columns[0].Width = 70;
            DGV.Columns[1].Width = 250;
            DGV.Columns[2].Width = 160;
            DGV.Columns[3].Width = 160;
            DGV.Columns[4].Width = 200;
            DGV.Columns[5].Width = 160;
            DGV.Columns[6].Width = 200;
            DGV.Columns[7].Width = 200;
            DGV.Columns[8].Width = 70;
        }

        //ACCION PARA HACER UNA BUSQUEDA INTELIGENTE SEGUN LE CRITEIRO ELEGIDO
        private void txtBusquedaLicencia_TextChanged(object sender, EventArgs e)
        {
            if (cboBusquedaLicencia.Text  == "LICENSE TITLE")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarLicenciaSegunTitulo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@descripcion", txtBusquedaLicencia.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistado.DataSource = dt;
                con.Close();

                Redimencionar(datalistado);
            }
            else if (cboBusquedaLicencia.Text == "ASSIGNED PERSONNEL")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarLicenciaSegunPersonalAsignado", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@descripcion", txtBusquedaLicencia.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistado.DataSource = dt;
                con.Close();

                Redimencionar(datalistado);
            }
            else if (cboBusquedaLicencia.Text == "DEVICE USER")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarLicenciaSegunUsuarioDispositivo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@descripcion", txtBusquedaLicencia.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistado.DataSource = dt;
                con.Close();

                Redimencionar(datalistado);
            }
            else if (cboBusquedaLicencia.Text == "MACHINE")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarLicenciaSegunMaquina", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@descripcion", txtBusquedaLicencia.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistado.DataSource = dt;
                con.Close();

                Redimencionar(datalistado);
            }
        }
    }
}

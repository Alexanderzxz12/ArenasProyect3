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
    public partial class ManEstadoSistema : Form
    {
        //CONSTRUCTOR DEL MANTENIMIENTO - MANTENIEMINTO DE ESTADO SISTEMA
        public ManEstadoSistema()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI MANTENIMIENTOS
        private void ManEstadoSistema_Load(object sender, EventArgs e)
        {
            Mostrar();
            alternarColorFilas(datalistado);

            cboEstado.SelectedIndex = 0;
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

        //METODO PARA VISUALIZAR LOS DATOS, LISTADO DE DATOS EN MI GRILLA
        public void Mostrar()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            da = new SqlDataAdapter("SELECT IdEstadoSistema AS [CÓDIGO], Descripcion AS [DESCRIPCIÓN], EstadoSistema AS [ESTADO],  FechaRegistro AS [FECHA REGISTRO] FROM EstadoSistema", con);
            da.Fill(dt);
            datalistado.DataSource = dt;
            con.Close();
            Redimencionar(datalistado);
        }

        //ACCIONES Y PROCESOS DEL MANTENIMIENTO*--------------------------------------
        //HABILITAR EL GUARDAR DE MI MANTENIMIENTO
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            txtDescripcion.ReadOnly = false;
            txtMaquina.Text = Environment.MachineName;
            txtUusarioDispositivo.Text = Environment.UserName;
            txtUsuarioSistema.ReadOnly = false;

            btnGuardar.Visible = false;
            btnGuardar2.Visible = true;

            Cancelar.Visible = true;
            lblCancelar.Visible = true;

            lblCodigo.Text = "N";
        }

        //ACCION DE GAURDAR EN MI BASE DE DATOS
        private void btnGuardar2_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea ingresar un nuevo estado al sistema?, el cambio afectará a todos los usuarios que utilizan el sistema", "Validación del Sistema", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (boton == DialogResult.OK)
            {
                try
                {
                    if (txtDescripcion.Text != "" && txtMaquina.Text != "" && txtUusarioDispositivo.Text != "" && txtUsuarioSistema.Text != "")
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("InsertarEstadoSistema", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text);
                        cmd.Parameters.AddWithValue("@maquina", txtMaquina.Text);
                        cmd.Parameters.AddWithValue("@usuarioDispositivo", txtUusarioDispositivo.Text);
                        cmd.Parameters.AddWithValue("@usuarioSistema", txtUsuarioSistema.Text);
                        cmd.Parameters.AddWithValue("@estado", cboEstado.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("The new record was entered correctly, operation record: 148x17c8478q945v7484vbbe84846125", "Nuevo Registro de Estado", MessageBoxButtons.OK);
                        Mostrar();
                        Reiniciar();
                    }
                    else
                    {
                        MessageBox.Show("Debe llenar todo los datos para poder continuar", "Validación del Sistema", MessageBoxButtons.OK);
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
            txtDescripcion.ReadOnly = true;
            txtDescripcion.Text = "";
            txtMaquina.ReadOnly = true;
            txtMaquina.Text = "";
            txtUusarioDispositivo.ReadOnly = true;
            txtUusarioDispositivo.Text = "";
            txtUsuarioSistema.ReadOnly = true;
            txtUsuarioSistema.Text = "";

            btnGuardar.Visible = true;
            btnGuardar2.Visible = true;

            Cancelar.Visible = false;
            lblCancelar.Visible = false;

            lblCodigo.Text = "N";
        }

        //FUNCION PARA REDIMENCIONAR MI LISTADO
        public void Redimencionar(DataGridView DGV)
        {
            DGV.Columns[0].Width = 70;
            DGV.Columns[1].Width = 400;
            DGV.Columns[2].Width = 220;
            DGV.Columns[3].Width = 120;
        }
    }
}

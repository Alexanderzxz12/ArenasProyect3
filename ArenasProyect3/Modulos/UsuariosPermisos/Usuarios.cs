using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.UsuariosPermisos
{
    public partial class Usuarios : Form
    {
        //CONSTRUCTOR DEL MANTENIMIENTO - USUARIOS
        public Usuarios()
        {
            InitializeComponent();
        }

        //Drag Form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int LParam);

        //EVENTO PARA MOVER MI FORMUALRIO
        private void panelPrincipal_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        //EVENTO DE INICIO Y DE CARGA DEL MANTENIMEINTOS DE USUARIO
        private void Usuarios_Load(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panelIcono.Visible = false;
            mostrar();
        }

        //CARGAR ROLAES
        public void CargarRoles(string area)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdPerfil, Perfil FROM Perfil WHERE Area = @area", con);
            comando.Parameters.AddWithValue("@area", area);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboRol.ValueMember = "IdPerfil";
            cboRol.DisplayMember = "Perfil";
            cboRol.DataSource = dt;
        }

        //BOTON PARA GUARDAR MI NUEVO USUARIO
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text == "" || txtApellidos.Text == "" || txtLogin.Text == "" || txtContrasena.Text == "" || txtDocumento.Text == "" || txtRutaFirma.Text == "" || cboHabilitarRequerimeinto.Text == "" || txtArea.Text == "" || cboRol.Text == "")
            {
                MessageBox.Show("Debe ingresar datos válidos para poder registrar un nuevo usuario", "Registro de Usuario", MessageBoxButtons.OKCancel);
            }
            else
            {
                if (lblAnuncioIcono.Visible == false)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Insertar_Usuario", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@nombres", txtNombre.Text);
                        cmd.Parameters.AddWithValue("@apellidos", txtApellidos.Text);
                        cmd.Parameters.AddWithValue("@login", txtLogin.Text);
                        cmd.Parameters.AddWithValue("@password", txtContrasena.Text);
                        cmd.Parameters.AddWithValue("@documento", Convert.ToInt32(txtDocumento.Text));
                        cmd.Parameters.AddWithValue("@rutaFirma", txtRutaFirma.Text);

                        int HabilitarRequerimeinto = 0;
                        if (cboHabilitarRequerimeinto.Text == "SI")
                        {
                            HabilitarRequerimeinto = 1;
                        }
                        else
                        {
                            HabilitarRequerimeinto = 0;
                        }

                        cmd.Parameters.AddWithValue("@habilitarRequerimeinto", HabilitarRequerimeinto);
                        cmd.Parameters.AddWithValue("@area", txtArea.Text);
                        cmd.Parameters.AddWithValue("@rol", cboRol.SelectedValue.ToString());

                        System.IO.MemoryStream ms = new System.IO.MemoryStream();
                        Icono.Image.Save(ms, Icono.Image.RawFormat);

                        cmd.Parameters.AddWithValue("@icono", ms.GetBuffer());
                        cmd.Parameters.AddWithValue("@nombre_icono", lblNumeroIcono.Text);
                        cmd.Parameters.AddWithValue("@estado", "Activo");

                        cmd.ExecuteNonQuery();
                        con.Close();
                        mostrar();
                        MessageBox.Show("Se ingresó el registro correctamente", "Registro", MessageBoxButtons.OKCancel);
                        panel4.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar una imagen o cargar una", "Registro", MessageBoxButtons.OKCancel);
                }
            }
        }

        //MOSTRAR TODOS MI SUUARIOS
        private void mostrar()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;

                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();

                da = new SqlDataAdapter("Mostrar_Usuarios", con);
                da.Fill(dt);
                dataListado.DataSource = dt;
                con.Close();
                dataListado.Columns[1].Visible = false;
                dataListado.Columns[8].Visible = false;
                dataListado.Columns[9].Visible = false;
                dataListado.Columns[10].Visible = false;
                dataListado.Columns[11].Visible = false;
                dataListado.Columns[12].Visible = false;
                dataListado.Columns[13].Visible = false;

                dataListado.Columns[2].Width = 200;
                dataListado.Columns[3].Width = 200;
                dataListado.Columns[4].Width = 120;
                dataListado.Columns[5].Width = 120;
                dataListado.Columns[6].Width = 150;
                dataListado.Columns[7].Width = 200;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //EVENTO DE AGREGAR UNA IMAGEN A MI PEFIL DE USUARIO
        private void lblAnuncioIcono_Click(object sender, EventArgs e)
        {
            panelIcono.Visible = true;
        }

        //SELECCIONAR UNA IMAGEN PARA MI PERFIL
        private void pbImagen1_Click(object sender, EventArgs e)
        {
            Icono.Image = pbImagen1.Image;
            lblNumeroIcono.Text = "1";
            lblAnuncioIcono.Visible = false;
            panelIcono.Visible = false;
        }

        //SELECCIONAR UNA IMAGEN PARA MI PERFIL
        private void pbImagen2_Click(object sender, EventArgs e)
        {
            Icono.Image = pbImagen2.Image;
            lblNumeroIcono.Text = "2";
            lblAnuncioIcono.Visible = false;
            panelIcono.Visible = false;
        }

        //SELECCIONAR UNA IMAGEN PARA MI PERFIL
        private void pbImagen3_Click(object sender, EventArgs e)
        {
            Icono.Image = pbImagen3.Image;
            lblNumeroIcono.Text = "3";
            lblAnuncioIcono.Visible = false;
            panelIcono.Visible = false;
        }

        //SELECCIONAR UNA IMAGEN PARA MI PERFILS
        private void pbImagen4_Click(object sender, EventArgs e)
        {
            Icono.Image = pbImagen4.Image;
            lblNumeroIcono.Text = "4";
            lblAnuncioIcono.Visible = false;
            panelIcono.Visible = false;
        }

        //SELECCIONAR UNA IMAGEN PARA MI PERFIL
        private void pbImagen5_Click(object sender, EventArgs e)
        {
            Icono.Image = pbImagen5.Image;
            lblNumeroIcono.Text = "5";
            lblAnuncioIcono.Visible = false;
            panelIcono.Visible = false;
        }

        //SELECCIONAR UNA IMAGEN PARA MI PERFIL
        private void pbImagen6_Click(object sender, EventArgs e)
        {
            Icono.Image = pbImagen6.Image;
            lblNumeroIcono.Text = "6";
            lblAnuncioIcono.Visible = false;
            panelIcono.Visible = false;
        }

        //SELECCIONAR UNA IMAGEN PARA MI PERFIL
        private void pbImagen7_Click(object sender, EventArgs e)
        {
            Icono.Image = pbImagen7.Image;
            lblNumeroIcono.Text = "7";
            lblAnuncioIcono.Visible = false;
            panelIcono.Visible = false;
        }

        //SELECCIONAR UNA IMAGEN PARA MI PERFIL
        private void pbImagen8_Click(object sender, EventArgs e)
        {
            Icono.Image = pbImagen8.Image;
            lblNumeroIcono.Text = "8";
            lblAnuncioIcono.Visible = false;
            panelIcono.Visible = false;
        }

        //BOTON PARA AGREGAR UN NUEVO USUARIO
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
            lblAnuncioIcono.Visible = true;
            txtNombre.Text = "";
            txtApellidos.Text = "";
            txtLogin.Text = "";
            txtContrasena.Text = "";
            txtDocumento.Text = "";
            txtRutaFirma.Text = "";
            btnGuardar.Visible = true;
            btnGuardarCambios.Visible = false;
        }

        //BOTRON PARA SÑLAIR DEL FOMRULARIO DE NUEVO USUARIO
        private void btnVolver_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
        }

        //BOTON PARA CERRAR MI FORMULARIO
        private void btnCerrarUsuarios_Click(object sender, EventArgs e)
        {
            Close();
        }

        //SELECIONAR UN USUARIO PAR APODER VISUALIZARLO
        private void dataListado_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            lblIdUsuario.Text = dataListado.SelectedCells[1].Value.ToString();
            txtNombre.Text = dataListado.SelectedCells[2].Value.ToString();
            txtApellidos.Text = dataListado.SelectedCells[3].Value.ToString();

            txtLogin.Text = dataListado.SelectedCells[5].Value.ToString();
            txtContrasena.Text = dataListado.SelectedCells[8].Value.ToString();

            Icono.BackgroundImage = null;
            byte[] b = (Byte[])dataListado.SelectedCells[9].Value;
            MemoryStream ms = new MemoryStream(b);
            Icono.Image = Image.FromStream(ms);

            lblAnuncioIcono.Visible = false;

            txtDocumento.Text = dataListado.SelectedCells[4].Value.ToString();
            txtRutaFirma.Text = dataListado.SelectedCells[11].Value.ToString();
            lblNumeroIcono.Text = dataListado.SelectedCells[10].Value.ToString();
            txtArea.Text = dataListado.SelectedCells[6].Value.ToString();
            cboRol.SelectedValue = dataListado.SelectedCells[13].Value.ToString();

            int habilitadoRequerimiento = Convert.ToInt32(dataListado.SelectedCells[12].Value.ToString());
            if (habilitadoRequerimiento == 1)
            {
                cboHabilitarRequerimeinto.Text = "SI";
            }
            else
            {
                cboHabilitarRequerimeinto.Text = "NO";
            }

            panel4.Visible = true;
            btnGuardar.Visible = false;
            btnGuardarCambios.Visible = true;
        }

        //BOTON PARA PODER EDITAR MI USUARIO
        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text != "")
            {
                try
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Editar_Usuario", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idusuario", lblIdUsuario.Text);
                    cmd.Parameters.AddWithValue("@nombres", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@apellidos", txtApellidos.Text);
                    cmd.Parameters.AddWithValue("@login", txtLogin.Text);
                    cmd.Parameters.AddWithValue("@password", txtContrasena.Text);
                    cmd.Parameters.AddWithValue("@documento", Convert.ToInt32(txtDocumento.Text));
                    cmd.Parameters.AddWithValue("@rutaFirma", txtRutaFirma.Text);

                    int HabilitarRequerimeinto = 0;
                    if (cboHabilitarRequerimeinto.Text == "SI")
                    {
                        HabilitarRequerimeinto = 1;
                    }
                    else
                    {
                        HabilitarRequerimeinto = 0;
                    }

                    cmd.Parameters.AddWithValue("@habilitarRequerimeinto", HabilitarRequerimeinto);
                    cmd.Parameters.AddWithValue("@area", txtArea.Text);
                    cmd.Parameters.AddWithValue("@rol", cboRol.SelectedValue.ToString());

                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    Icono.Image.Save(ms, Icono.Image.RawFormat);

                    cmd.Parameters.AddWithValue("@icono", ms.GetBuffer());
                    cmd.Parameters.AddWithValue("@nombre_icono", lblNumeroIcono.Text);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    mostrar();
                    MessageBox.Show("Se editó el registro correctamente", "Registro", MessageBoxButtons.OKCancel);
                    panel4.Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        //SELECCIONAR MI IAMGEN DE USUARIO PARA PODER EDITARLO
        private void Icono_Click(object sender, EventArgs e)
        {
            panelIcono.Visible = true;
        }

        //PODER INABILITAR UN USUARIOS
        private void dataListado_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == this.dataListado.Columns["Eli"].Index)
            {
                DialogResult result;
                result = MessageBox.Show("¿Realmente desea inhabilitar este usuario?", "Inhabilitar de Registros", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    SqlCommand cmd;
                    try
                    {
                        int onekey = Convert.ToInt32(dataListado.SelectedCells[1].Value.ToString());

                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        cmd = new SqlCommand("Eliminar_Usuarios", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idusuario", onekey);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        mostrar();
                        MessageBox.Show("Se inhabilitar el registro correctamente", "Registro", MessageBoxButtons.OKCancel);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //cCARGAR IMAGEN PROPIA
        private void pbCarga_Click(object sender, EventArgs e)
        {
            dlg.InitialDirectory = "";
            dlg.Filter = "Todos los archivos (*.*)|*.*";
            dlg.FilterIndex = 2;
            dlg.Title = "Cargador de imagenes";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Icono.BackgroundImage = null;
                Icono.Image = new Bitmap(dlg.FileName);
                Icono.SizeMode = PictureBoxSizeMode.Zoom;
                lblNumeroIcono.Text = Path.GetDirectoryName(dlg.FileName);
                lblAnuncioIcono.Visible = false;
                panelIcono.Visible = false;
            }
        }

        //EVENTO PARA PODER BUSCAR UN USUARIO POR NOMBRE
        private void BuscarUsuario()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();

                da = new SqlDataAdapter("Buscar_Usuarios", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@letra", txtBuscar.Text);
                da.Fill(dt);
                dataListado.DataSource = dt;
                con.Close();
                dataListado.Columns[1].Visible = false;
                dataListado.Columns[8].Visible = false;
                dataListado.Columns[9].Visible = false;
                dataListado.Columns[10].Visible = false;
                dataListado.Columns[11].Visible = false;
                dataListado.Columns[12].Visible = false;
                dataListado.Columns[13].Visible = false;

                dataListado.Columns[2].Width = 200;
                dataListado.Columns[3].Width = 200;
                dataListado.Columns[4].Width = 120;
                dataListado.Columns[5].Width = 120;
                dataListado.Columns[6].Width = 150;
                dataListado.Columns[7].Width = 200;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        //ESCRIBIR Y BUSCAR - BUSQUEDA SENSITIVA
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            BuscarUsuario();
        }

        //CARGAR ROLES SEGUN AREA
        private void txtArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarRoles(txtArea.Text);
        }

        //CARGA DE FIRMA
        private void btnCargarImagen_Click(object sender, EventArgs e)
        {
            dlgFirma.InitialDirectory = "c:\\";
            dlgFirma.Filter = "Todos los archivos (*.*)|*.*";
            dlgFirma.FilterIndex = 1;
            dlgFirma.RestoreDirectory = true;

            if (dlgFirma.ShowDialog() == DialogResult.OK)
            {
                txtRutaFirma.Text = dlgFirma.FileName;
            }
        }

        //LIMPIAR CARGA DE FIRMA
        private void btnLimpiarRuta_Click(object sender, EventArgs e)
        {
            txtRutaFirma.Text = "";
        }
    }
}

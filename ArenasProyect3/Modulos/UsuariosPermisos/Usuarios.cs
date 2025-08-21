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
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        private Cursor curAnterior = null;

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

        //BOTON PARA CERRAR MI FORMULARIO
        private void btnCerrarUsuarios_Click(object sender, EventArgs e)
        {
            Close();
        }

        //EVENTO DE INICIO Y DE CARGA DEL MANTENIMEINTOS DE USUARIO
        private void Usuarios_Load(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panelIcono.Visible = false;
            BuscarUsuario(cboBusquedaUsuarios.Text, txtBuscar.Text, dataListado);
            cboBusquedaUsuarios.SelectedIndex = 0;
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

        //EVENTO PARA PODER BUSCAR UN USUARIO POR NOMBRE
        private void BuscarUsuario(string tipo, string valor, DataGridView DGV)
        {
            try
            {
                if (valor == "" || valor == null)
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("Usuario_Mostrar", con);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                }
                else if (tipo == "NOMBRES Y APELLIDOS" && valor != "")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("Usuario_MostrarPorNombreApellidos", con);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                }
                else if (tipo == "USUARIO" && valor != "")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("Usuario_MostrarPorUsuario", con);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                }
                else if (tipo == "ÁREA" && valor != "")
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("Usuario_MostrarPorArea", con);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                }
                RedimensionarColumnas(DGV);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        //fFUNCION PARA REDIMENSIONAR MI LISTADO DE USUARIOS
        public void RedimensionarColumnas(DataGridView DGV)
        {
            DGV.Columns[1].Visible = false;
            DGV.Columns[8].Visible = false;
            DGV.Columns[9].Visible = false;
            DGV.Columns[10].Visible = false;
            DGV.Columns[11].Visible = false;
            DGV.Columns[12].Visible = false;
            DGV.Columns[13].Visible = false;

            DGV.Columns[2].Width = 200;
            DGV.Columns[3].Width = 200;
            DGV.Columns[4].Width = 120;
            DGV.Columns[5].Width = 120;
            DGV.Columns[6].Width = 150;
            DGV.Columns[7].Width = 200;
        }

        //ESCRIBIR Y BUSCAR - BUSQUEDA SENSITIVA
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            BuscarUsuario(cboBusquedaUsuarios.Text, txtBuscar.Text, dataListado);
        }

        //PODER INABILITAR UN USUARIOS
        private void dataListado_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == this.dataListado.Columns["Eli"].Index)
            {
                int onekey = Convert.ToInt32(dataListado.SelectedCells[1].Value.ToString());
                InhabilitarUsuario(onekey);
            }
        }

        //FUNICON PARA INHABILITAR MII USUARIOS
        public void InhabilitarUsuario(int idUsuario)
        {
            DialogResult result = MessageBox.Show("¿Realmente desea inhabilitar este usuario?.", "Inhabilitar de Registros", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                try
                {
                    SqlCommand cmd;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    cmd = new SqlCommand("Usuario_Eliminar", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    BuscarUsuario(cboBusquedaUsuarios.Text, txtBuscar.Text, dataListado);
                    MessageBox.Show("Se inhabilitó el registro correctamente.", "Registro", MessageBoxButtons.OKCancel);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
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

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN
        private void dataListado_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.dataListado.Columns[e.ColumnIndex].Name == "Eli")
            {
                this.dataListado.Cursor = Cursors.Hand;
            }
            else
            {
                this.dataListado.Cursor = curAnterior;
            }
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

        //SELECCIONAR MI IAMGEN DE USUARIO PARA PODER EDITARLO
        private void Icono_Click(object sender, EventArgs e)
        {
            panelIcono.Visible = true;
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

        //CARGAR ROLES SEGUN AREA
        private void txtArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarRoles(txtArea.Text);
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

        //BOTON PARA GUARDAR MI NUEVO USUARIO
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            int valorReque = 0;
            if (cboHabilitarRequerimeinto.Text == "SI")
            {
                valorReque = 1;
            }
            else
            {
                valorReque = 0;
            }

            GuardarNuevoUsuario(txtNombre.Text, txtApellidos.Text, txtLogin.Text, txtContrasena.Text, txtDocumento.Text, txtRutaFirma.Text, txtArea.Text, Convert.ToInt16(cboRol.SelectedValue.ToString()), valorReque, lblNumeroIcono.Text);
        }

        //FUNCION PARA PODER GUARDAR LOS NUEVOS USUARIOS
        public void GuardarNuevoUsuario(string nombres, string apellidos, string login, string password, string documento, string rutaFirma, string area, int rol, int hbailitadoReque, string nIcono)
        {
            if (nombres == "" || apellidos == "" || login == "" || password == "" || documento == "" || rutaFirma == "" || area == "" || rol == null || nIcono == "")
            {
                MessageBox.Show("Debe ingresar todos los datos necesarios para poder continuar con el registro.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Usuario_Insertar", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombres", nombres);
                    cmd.Parameters.AddWithValue("@apellidos", apellidos);
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@password", password);

                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    Icono.Image.Save(ms, Icono.Image.RawFormat);
                    cmd.Parameters.AddWithValue("@icono", ms.GetBuffer());

                    cmd.Parameters.AddWithValue("@nombre_icono", nIcono);
                    cmd.Parameters.AddWithValue("@area", area);
                    cmd.Parameters.AddWithValue("@rol", rol);
                    cmd.Parameters.AddWithValue("@habilitarRequerimeinto", hbailitadoReque);
                    cmd.Parameters.AddWithValue("@documento", documento);
                    cmd.Parameters.AddWithValue("@rutaFirma", rutaFirma);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    BuscarUsuario(cboBusquedaUsuarios.Text, txtBuscar.Text, dataListado);
                    panel4.Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //BOTON PARA PODER EDITAR MI USUARIO
        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            //if (txtNombre.Text != "")
            //{
            //    try
            //    {
            //        SqlConnection con = new SqlConnection();
            //        con.ConnectionString = Conexion.ConexionMaestra.conexion;
            //        con.Open();
            //        SqlCommand cmd = new SqlCommand();
            //        cmd = new SqlCommand("Editar_Usuario", con);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@idusuario", lblIdUsuario.Text);
            //        cmd.Parameters.AddWithValue("@nombres", txtNombre.Text);
            //        cmd.Parameters.AddWithValue("@apellidos", txtApellidos.Text);
            //        cmd.Parameters.AddWithValue("@login", txtLogin.Text);
            //        cmd.Parameters.AddWithValue("@password", txtContrasena.Text);
            //        cmd.Parameters.AddWithValue("@documento", Convert.ToInt32(txtDocumento.Text));
            //        cmd.Parameters.AddWithValue("@rutaFirma", txtRutaFirma.Text);

            //        int HabilitarRequerimeinto = 0;
            //        if (cboHabilitarRequerimeinto.Text == "SI")
            //        {
            //            HabilitarRequerimeinto = 1;
            //        }
            //        else
            //        {
            //            HabilitarRequerimeinto = 0;
            //        }

            //        cmd.Parameters.AddWithValue("@habilitarRequerimeinto", HabilitarRequerimeinto);
            //        cmd.Parameters.AddWithValue("@area", txtArea.Text);
            //        cmd.Parameters.AddWithValue("@rol", cboRol.SelectedValue.ToString());

            //        System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //        Icono.Image.Save(ms, Icono.Image.RawFormat);

            //        cmd.Parameters.AddWithValue("@icono", ms.GetBuffer());
            //        cmd.Parameters.AddWithValue("@nombre_icono", lblNumeroIcono.Text);
            //        cmd.ExecuteNonQuery();
            //        con.Close();
            //        mostrar();
            //        MessageBox.Show("Se editó el registro correctamente", "Registro", MessageBoxButtons.OKCancel);
            //        panel4.Visible = false;
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }

            //}
        }

        //BOTRON PARA SÑLAIR DEL FOMRULARIO DE NUEVO USUARIO
        private void btnVolver_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
        }


        //FUNCION PARA DESCAARGAR LA IMAGEN MOSTRADA
        private void btnDescargarImagen_Click(object sender, EventArgs e)
        {
            // Verificar si el PictureBox contiene una imagen
            if (Icono.Image != null)
            {
                // Crear un cuadro de diálogo para guardar la imagen
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Archivos de Imagen|*.jpg;*.png;*.bmp",
                    Title = "Guardar Imagen"
                };

                // Mostrar el cuadro de diálogo y verificar si el usuario seleccionó una ubicación
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Guardar la imagen en la ubicación seleccionada
                    Icono.Image.Save(saveFileDialog.FileName);
                    MessageBox.Show("¡Imagen guardada exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("No hay ninguna imagen para guardar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

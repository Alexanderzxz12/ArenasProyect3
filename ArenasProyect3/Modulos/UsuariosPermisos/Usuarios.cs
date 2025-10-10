using ArenasProyect3.Modulos.Resourses;
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

        //EVENTO DE INICIO Y DE CARGA DEL MANTENIMEINTOS DE USUARIO
        private void Usuarios_Load(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panelIcono.Visible = false;
            BuscarUsuario(cboBusquedaUsuarios.Text, txtBuscar.Text, dataListado);
            cboBusquedaUsuarios.SelectedIndex = 0;
        }

        //METODO PARA PINTAR DE COLORES LAS FILAS DE MI LSITADO
        public void alternarColorFilas(DataGridView dgv)
        {
            try
            {
                {
                    var withBlock = dgv;
                    withBlock.RowsDefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
                    withBlock.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //CARGAR ROLAES
        public void CargarRoles(string area)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //EVENTO PARA PODER BUSCAR UN USUARIO POR NOMBRE
        private void BuscarUsuario(string tipo, string valor, DataGridView DGV)
        {
            try
            {
                if (valor == "" || valor == null)
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Usuario_Mostrar", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                }
                else if (tipo == "NOMBRES Y APELLIDOS" && valor != "")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Usuario_MostrarPorNombreApellidos", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombreApellidos", valor);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                }
                else if (tipo == "USUARIO" && valor != "")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Usuario_MostrarPorUsuario", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usuario", valor);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                }
                else if (tipo == "ÁREA" && valor != "")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Usuario_MostrarPorArea", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@area", valor);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                }
                RedimensionarColumnas(DGV);
                alternarColorFilas(DGV);
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
            DGV.Columns[15].Visible = false;
            DGV.Columns[16].Visible = false;

            DGV.Columns[2].Width = 200;
            DGV.Columns[3].Width = 200;
            DGV.Columns[4].Width = 125;
            DGV.Columns[5].Width = 130;
            DGV.Columns[6].Width = 150;
            DGV.Columns[7].Width = 200;
        }

        //LIMPIAR MI TIPO DE BUSQUEDA
        private void cboBusquedaUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBuscar.Text = "";
        }

        //ESCRIBIR Y BUSCAR - BUSQUEDA SENSITIVA
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            BuscarUsuario(cboBusquedaUsuarios.Text, txtBuscar.Text, dataListado);
        }

        //PODER INABILITAR UN USUARIOS
        private void dataListado_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataListado.RowCount != 0)
            {
                if (e.ColumnIndex == this.dataListado.Columns["Eli"].Index)
                {
                    int onekey = Convert.ToInt32(dataListado.SelectedCells[1].Value.ToString());
                    InhabilitarUsuario(onekey);
                }
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
        private void dataListado_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataListado.RowCount != 0)
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

                int habilitadoCoti = Convert.ToInt32(dataListado.SelectedCells[15].Value.ToString());
                if (habilitadoCoti == 1)
                {
                    cboHabilitarCotizacion.Text = "SI";
                }
                else
                {
                    cboHabilitarCotizacion.Text = "NO";
                }

                int visibleUsuario = Convert.ToInt32(dataListado.SelectedCells[16].Value.ToString());
                if (visibleUsuario == 1)
                {
                    cboUusarioVisible.Text = "SI";
                }
                else
                {
                    cboUusarioVisible.Text = "NO";
                }

                panel4.Visible = true;
                btnGuardar.Visible = false;
                lblGuardar.Visible = false;
                btnGuardarCambios.Visible = true;
                lblGuardarCambios.Visible = true;
            }
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
            lblGuardar.Visible = true;
            lblGuardarCambios.Visible = false;
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
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGA DE FIRMA
        private void btnCargarImagen_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

            if (cboRol.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un rol para el usuario.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                GuardarNuevoUsuario(txtNombre.Text, txtApellidos.Text, txtLogin.Text, txtContrasena.Text, lblNumeroIcono.Text, txtArea.Text, Convert.ToInt16(cboRol.SelectedValue.ToString()), valorReque, txtDocumento.Text, txtRutaFirma.Text, txtNombre.Text, txtApellidos.Text);
            }
        }

        //FUNCION PARA PODER GUARDAR LOS NUEVOS USUARIOS
        public void GuardarNuevoUsuario(string nombres, string apellidos, string login, string password, string nIcono, string area, int rol, int hbailitadoReque, string documento, string rutaFirma, string primerNombres, string apellidosDes)
        {
            if (nombres == "" || apellidos == "" || login == "" || password == "" || documento == "" || rutaFirma == "" || area == "" || rol == null || nIcono == "")
            {
                MessageBox.Show("Debe ingresar todos los datos necesarios para poder continuar con el registro.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                DialogResult boton = MessageBox.Show("¿Esta seguro que desea guardar un nuevo usuario?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
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

                        //CAPTURAR MIS NOMBRES
                        string textoNombres = primerNombres.Trim();
                        string[] partesNom = textoNombres.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        string primerNombre = partesNom.Length > 0 ? partesNom[0] : "";
                        string segundoNombre = partesNom.Length > 1 ? string.Join(" ", partesNom.Skip(1)) : "";
                        cmd.Parameters.AddWithValue("@primerNombre", primerNombre);
                        cmd.Parameters.AddWithValue("@segundoNombre", segundoNombre);

                        //CAPTURAR MIS APELLIDOS
                        string textoApellidos = apellidosDes.Trim();
                        string[] partesApe = textoApellidos.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        string apellidoPaterno = partesApe.Length > 0 ? partesApe[0] : "";
                        string apellidoMaterno = partesApe.Length > 1 ? partesApe[1] : "";
                        cmd.Parameters.AddWithValue("@apellidoPaterno", apellidoPaterno);
                        cmd.Parameters.AddWithValue("@apellidoMaterno", apellidoMaterno);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Se registró el nuevo usuario correctamente.", "Registro", MessageBoxButtons.OK);
                        BuscarUsuario(cboBusquedaUsuarios.Text, txtBuscar.Text, dataListado);
                        panel4.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //BOTON PARA PODER EDITAR MI USUARIO
        private void btnGuardarCambios_Click(object sender, EventArgs e)
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

            int valorCoti = 0;
            if (cboHabilitarCotizacion.Text == "SI")
            {
                valorCoti = 1;
            }
            else
            {
                valorCoti = 0;
            }

            int visibleUsuario = 0;
            if (cboUusarioVisible.Text == "SI")
            {
                visibleUsuario = 1;
            }
            else
            {
                visibleUsuario = 0;
            }

            if (cboRol.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un rol para el usuario.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                EditarUsuario(Convert.ToInt32(lblIdUsuario.Text), txtNombre.Text, txtApellidos.Text, txtLogin.Text,txtContrasena.Text, lblNumeroIcono.Text, txtArea.Text, Convert.ToInt16(cboRol.SelectedValue.ToString()), valorReque, txtDocumento.Text, txtRutaFirma.Text, txtNombre.Text, txtApellidos.Text, visibleUsuario, valorCoti );
            }
        }

        //FUNCION PARA EDITAR MI USUARIO
        public void EditarUsuario(int idUsuario, string nombres, string apellidos, string login,string password, string nIcono, string area, int rol, int hbailitadoReque, string documento, string rutaFirma, string primerNombres, string apellidosDes, int visible, int habilitarCotizacion)
        {
            if (nombres == "" || apellidos == "" || password == "" || documento == "" || rutaFirma == "" || area == "" || rol == null || nIcono == "")
            {
                MessageBox.Show("Debe ingresar todos los datos necesarios para poder continuar con la edición.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                DialogResult boton = MessageBox.Show("¿Esta seguro que desea editar este usuario?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Usuario_Editar", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                        cmd.Parameters.AddWithValue("@nombres", nombres);
                        cmd.Parameters.AddWithValue("@apellidos", apellidos);
                        cmd.Parameters.AddWithValue("@Login", login);
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

                        //CAPTURAR MIS NOMBRES
                        string textoNombres = primerNombres.Trim();
                        string[] partesNom = textoNombres.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        string primerNombre = partesNom.Length > 0 ? partesNom[0] : "";
                        string segundoNombre = partesNom.Length > 1 ? string.Join(" ", partesNom.Skip(1)) : "";
                        cmd.Parameters.AddWithValue("@primerNombre", primerNombre);
                        cmd.Parameters.AddWithValue("@segundoNombre", segundoNombre);

                        //CAPTURAR MIS APELLIDOS
                        string textoApellidos = apellidosDes.Trim();
                        string[] partesApe = textoApellidos.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        string apellidoPaterno = partesApe.Length > 0 ? partesApe[0] : "";
                        string apellidoMaterno = partesApe.Length > 1 ? partesApe[1] : "";
                        cmd.Parameters.AddWithValue("@apellidoPaterno", apellidoPaterno);
                        cmd.Parameters.AddWithValue("@apellidoMaterno", apellidoMaterno);

                        cmd.Parameters.AddWithValue("@visible", visible);
                        cmd.Parameters.AddWithValue("@habilitarCotizacion", habilitarCotizacion);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Se editó el usuario correctamente.", "Registro", MessageBoxButtons.OK);
                        BuscarUsuario(cboBusquedaUsuarios.Text, txtBuscar.Text, dataListado);
                        panel4.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
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

using ArenasProyect3.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArenasProyect3.Modulos.Resourses;

namespace ArenasProyect3.Modulos
{
    public partial class Login : Form
    {
        //CONSTRUCTOR DEL MANTENIMIENTO - LOGIN
        public Login()
        {
            InitializeComponent();
        }

        //VARIABLES DE CREACIÓN Y FUNCIONAMINETO - INICIO DE SESIÓN
        int contador;
        string area;
        int contadorInicioAdmin;
        int contadorInicioProcesos;
        int contadorInicioComercial;
        int contadorInicioProduccion;
        int contadorInicioIngieneria;
        int contadorInicioLogistica;
        int contadorInicioContabilidad;
        int contadorInicioMantenimiento;
        int contadorInicioCalidad;
        //int contadorInicioSIG;
        //int contadorInicioMantenimiento;
        //ESTADO INICIAL DEL SISTEMA
        string estadoSistema = "INACTIVO - EN MANTENIMIENTO";

        //PRIEMRA CARGA DEL LOGIN
        private void Login_Load(object sender, EventArgs e)
        {
            try
            {
                //BUSCAR EL ESTADO DEL SISTEMA EN EL SERVIDOR DE BASE DE DATOS
                CargarEstadoSitema();
                //OCULTAR EL PANEL DE LOGIN
                panelLogin.Visible = false;
                //RECUPERAR EL NOMBRE DE LA MAQUINA EN DONDE ESTA CORRIENDO
                string maquinaInicioLicencia = Environment.MachineName;

                //CONSULTA AL SERVIDOR - VALIDAR LICENCIA Y EXSISTENCIA DE LA MAQUINA EN EL SERVIDOR
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("ValidarLicencia", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@maquina", maquinaInicioLicencia);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoVerificacionLicencia.DataSource = dt;
                con.Close();
                //VALIDAR SI LA MAQUINA ESTA REGISTRADA EN EL SERVIDOR
                if (datalistadoVerificacionLicencia.RowCount > 0)
                {
                    //SI EXSITE
                    panelValidacionLicencia.Visible = false;
                    imgValidacionCorrecta.Visible = true;
                    imgValidacionIncorrecta.Visible = false;
                }
                else
                {
                    //SI NO EXISTE
                    MessageBox.Show("El dispositivo en donde está corriendo el sistema no tiene la licencia o autorización respectiva, por favor comunicarse con el área de sistemas para poder solucionar este error, Error: InvalidKeyToRun.", "Validación del Sistema");
                    panelValidacionLicencia.Visible = true;
                    imgValidacionCorrecta.Visible = false;
                    imgValidacionIncorrecta.Visible = true;
                }

                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(9, this.Name, 1, Program.IdUsuario = 0, "", 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión con el servidor de base de datos, no se encuentra conexión a internet o a la red. " + ex.Message, "Validación del Sistema", MessageBoxButtons.OK);
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 1, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //FUNCION PARA CONSULTAR EL ESTADO DEL SISTEMA EN EL SERVIDOR - GENERAL
        public void CargarEstadoSitema()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT IdEstadoSistema, Descripcion, EstadoSistema FROM EstadoSistema WHERE IdEstadoSistema = (SELECT MAX(IdEstadoSistema) FROM EstadoSistema)", con);
                da.Fill(dt);
                datalistadoEstadoSistema.DataSource = dt;
                con.Close();

                //GUARDAR EL ESTADO EN UNA VARIABLE ESTADO
                estadoSistema = datalistadoEstadoSistema.SelectedCells[2].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión con el servidor de base de datos, no se encuentra conexión a internet o a la red. " + ex.Message, "Validación del Sistema", MessageBoxButtons.OK);
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 1, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //ACCIONES DE SELECCIÓN DEL ÁREA CORRESPONDIENTE-------------------------------------------------------------
        //SELECCION DEL ÁREA DE PROCESOS
        private void btnProcesos_Click(object sender, EventArgs e)
        {
            //VALIDAR SI EL USUARIO TIENE LICENCIA
            if (datalistadoVerificacionLicencia.RowCount > 0)
            {
                //SI EL ESTADO DEL SISTEMA ES ACTIVO
                if (estadoSistema == "ACTIVO - CORRIENDO")
                {
                    //ABRIR EL PANEL DEL AREA SELECCIONADA
                    area = "Procesos";
                    if (panelUsuarios.Controls.Count < 1)
                    {
                        panelUsuarios.Controls.Clear();
                        flpAreas.Visible = false;
                        panelCuentas.Visible = true;
                        ImgRegresar.Visible = true;
                        DibujarUsuario();
                    }
                    else
                    {
                        panelUsuarios.Controls.Clear();
                        flpAreas.Visible = false;
                        panelCuentas.Visible = true;
                        ImgRegresar.Visible = true;
                        DibujarUsuario();
                    }
                }
                else
                {
                    //SI EL ESTADO DEL SISITEMA NO ESTA ACTIVO
                    panelMantenimiento.Visible = true;
                }
            }
            else
            {
                //SI EL USUARIO NO TIENE LICENCIA O NO ESTA REGISTRADO
                MessageBox.Show("El dispositivo en donde está corriendo el sistema no tiene la licencia o autorización respectiva, por favor comunicarse con el área de sistemas para poder solucionar este error, Error: InvalidKeyToRun.", "Validación del Sistema");
            }
        }

        //SELECCION DEL ÁREA COMERCIAL
        private void btnComercial_Click(object sender, EventArgs e)
        {
            //VALIDAR SI EL USUARIO TIENE LICENCIA
            if (datalistadoVerificacionLicencia.RowCount > 0)
            {
                //SI EL ESTADO DEL SISTEMA ES ACTIVO
                if (estadoSistema == "ACTIVO - CORRIENDO")
                {
                    //ABRIR EL PANEL DEL AREA SELECCIONADA
                    area = "Comercial";
                    if (panelUsuarios.Controls.Count < 1)
                    {
                        panelUsuarios.Controls.Clear();
                        flpAreas.Visible = false;
                        panelCuentas.Visible = true;
                        ImgRegresar.Visible = true;
                        DibujarUsuario();
                    }
                    else
                    {
                        panelUsuarios.Controls.Clear();
                        flpAreas.Visible = false;
                        panelCuentas.Visible = true;
                        ImgRegresar.Visible = true;
                        DibujarUsuario();
                    }
                }
                else
                {
                    //SI EL ESTADO DEL SISITEMA NO ESTA ACTIVO
                    panelMantenimiento.Visible = true;
                }
            }
            else
            {
                //SI EL USUARIO NO TIENE LICENCIA O NO ESTA REGISTRADO
                MessageBox.Show("El dispositivo en donde está corriendo el sistema no tiene la licencia o autorización respectiva, por favor comunicarse con el área de sistemas para poder solucionar este error, Error: InvalidKeyToRun.", "Validación del Sistema");
            }
        }

        //SELECCION DEL ÁREA DE PRODUCCIÓN
        private void btnProduccion_Click(object sender, EventArgs e)
        {
            //VALIDAR SI EL USUARIO TIENE LICENCIA
            if (datalistadoVerificacionLicencia.RowCount > 0)
            {
                //SI EL ESTADO DEL SISTEMA ES ACTIVO
                if (estadoSistema == "ACTIVO - CORRIENDO")
                {
                    //ABRIR EL PANEL DEL AREA SELECCIONADA
                    area = "Producción";
                    if (panelUsuarios.Controls.Count < 1)
                    {
                        panelUsuarios.Controls.Clear();
                        flpAreas.Visible = false;
                        panelCuentas.Visible = true;
                        ImgRegresar.Visible = true;
                        DibujarUsuario();
                    }
                    else
                    {
                        panelUsuarios.Controls.Clear();
                        flpAreas.Visible = false;
                        panelCuentas.Visible = true;
                        ImgRegresar.Visible = true;
                        DibujarUsuario();
                    }
                }
                else
                {
                    //SI EL ESTADO DEL SISITEMA NO ESTA ACTIVO
                    panelMantenimiento.Visible = true;
                }
            }
            else
            {
                //SI EL USUARIO NO TIENE LICENCIA O NO ESTA REGISTRADO
                MessageBox.Show("El dispositivo en donde está corriendo el sistema no tiene la licencia o autorización respectiva, por favor comunicarse con el área de sistemas para poder solucionar este error, Error: InvalidKeyToRun.", "Validación del Sistema");
            }
        }

        //SELECCION DEL ÁREA DE INGIENERÍA
        private void btnIngieneria_Click(object sender, EventArgs e)
        {
            //VALIDAR SI EL USUARIO TIENE LICENCIA
            if (datalistadoVerificacionLicencia.RowCount > 0)
            {
                //SI EL ESTADO DEL SISTEMA ES ACTIVO
                if (estadoSistema == "ACTIVO - CORRIENDO")
                {
                    //ABRIR EL PANEL DEL AREA SELECCIONADA
                    area = "Ingeniería";
                    if (panelUsuarios.Controls.Count < 1)
                    {
                        panelUsuarios.Controls.Clear();
                        flpAreas.Visible = false;
                        panelCuentas.Visible = true;
                        ImgRegresar.Visible = true;
                        DibujarUsuario();
                    }
                    else
                    {
                        panelUsuarios.Controls.Clear();
                        flpAreas.Visible = false;
                        panelCuentas.Visible = true;
                        ImgRegresar.Visible = true;
                        DibujarUsuario();
                    }
                }
                else
                {
                    //SI EL ESTADO DEL SISITEMA NO ESTA ACTIVO
                    panelMantenimiento.Visible = true;
                }
            }
            else
            {
                //SI EL USUARIO NO TIENE LICENCIA O NO ESTA REGISTRADO
                MessageBox.Show("El dispositivo en donde está corriendo el sistema no tiene la licencia o autorización respectiva, por favor comunicarse con el área de sistemas para poder solucionar este error, Error: InvalidKeyToRun.", "Validación del Sistema");
            }
        }

        //SELECCION DEL ÁREA DE LOGÍSTICA
        private void btnLogistica_Click(object sender, EventArgs e)
        {
            //VALIDAR SI EL USUARIO TIENE LICENCIA
            if (datalistadoVerificacionLicencia.RowCount > 0)
            {
                //SI EL ESTADO DEL SISTEMA ES ACTIVO
                if (estadoSistema == "ACTIVO - CORRIENDO")
                {
                    //ABRIR EL PANEL DEL AREA SELECCIONADA
                    area = "Logística";
                    if (panelUsuarios.Controls.Count < 1)
                    {
                        panelUsuarios.Controls.Clear();
                        flpAreas.Visible = false;
                        panelCuentas.Visible = true;
                        ImgRegresar.Visible = true;
                        DibujarUsuario();
                    }
                    else
                    {
                        panelUsuarios.Controls.Clear();
                        flpAreas.Visible = false;
                        panelCuentas.Visible = true;
                        ImgRegresar.Visible = true;
                        DibujarUsuario();
                    }
                }
                else
                {
                    //SI EL ESTADO DEL SISITEMA NO ESTA ACTIVO
                    panelMantenimiento.Visible = true;
                }
            }
            else
            {
                //SI EL USUARIO NO TIENE LICENCIA O NO ESTA REGISTRADO
                MessageBox.Show("El dispositivo en donde está corriendo el sistema no tiene la licencia o autorización respectiva, por favor comunicarse con el área de sistemas para poder solucionar este error, Error: InvalidKeyToRun.", "Validación del Sistema");
            }
        }

        //SELECCION DEL´ÁREA DE CONTABILIDAD
        private void btnContabilidad_Click(object sender, EventArgs e)
        {
            //VALIDAR SI EL USUARIO TIENE LICENCIA
            if (datalistadoVerificacionLicencia.RowCount > 0)
            {
                //SI EL ESTADO DEL SISTEMA ES ACTIVO
                if (estadoSistema == "ACTIVO - CORRIENDO")
                {
                    //ABRIR EL PANEL DEL AREA SELECCIONADA
                    area = "Contabilidad";
                    if (panelUsuarios.Controls.Count < 1)
                    {
                        panelUsuarios.Controls.Clear();
                        flpAreas.Visible = false;
                        panelCuentas.Visible = true;
                        ImgRegresar.Visible = true;
                        DibujarUsuario();
                    }
                    else
                    {
                        panelUsuarios.Controls.Clear();
                        flpAreas.Visible = false;
                        panelCuentas.Visible = true;
                        ImgRegresar.Visible = true;
                        DibujarUsuario();
                    }
                }
                else
                {
                    //SI EL ESTADO DEL SISITEMA NO ESTA ACTIVO
                    panelMantenimiento.Visible = true;
                }
            }
            else
            {
                //SI EL USUARIO NO TIENE LICENCIA O NO ESTA REGISTRADO
                MessageBox.Show("El dispositivo en donde está corriendo el sistema no tiene la licencia o autorización respectiva, por favor comunicarse con el área de sistemas para poder solucionar este error, Error: InvalidKeyToRun.", "Validación del Sistema");
            }
        }

        //SELECCION DEL ÁREA DE CALIDAD
        private void btnCalidad_Click(object sender, EventArgs e)
        {
            //VALIDAR SI EL USUARIO TIENE LICENCIA
            if (datalistadoVerificacionLicencia.RowCount > 0)
            {
                //SI EL ESTADO DEL SISTEMA ES ACTIVO
                if (estadoSistema == "ACTIVO - CORRIENDO")
                {
                    //ABRIR EL PANEL DEL AREA SELECCIONADA
                    area = "Calidad";
                    if (panelUsuarios.Controls.Count < 1)
                    {
                        panelUsuarios.Controls.Clear();
                        flpAreas.Visible = false;
                        panelCuentas.Visible = true;
                        ImgRegresar.Visible = true;
                        DibujarUsuario();
                    }
                    else
                    {
                        panelUsuarios.Controls.Clear();
                        flpAreas.Visible = false;
                        panelCuentas.Visible = true;
                        ImgRegresar.Visible = true;
                        DibujarUsuario();
                    }
                }
                else
                {
                    //SI EL ESTADO DEL SISITEMA NO ESTA ACTIVO
                    panelMantenimiento.Visible = true;
                }
            }
            else
            {
                //SI EL USUARIO NO TIENE LICENCIA O NO ESTA REGISTRADO
                MessageBox.Show("El dispositivo en donde está corriendo el sistema no tiene la licencia o autorización respectiva, por favor comunicarse con el área de sistemas para poder solucionar este error, Error: InvalidKeyToRun.", "Validación del Sistema");
            }
        }

        //SELECCION DEL ÁREA DE SIG
        private void btnSIG_Click(object sender, EventArgs e)
        {
            //AUN EN PROCESO
        }

        //SELECCION DEL ÁREA DE MANTENIMIENTO
        private void btnMantenimiento_Click(object sender, EventArgs e)
        {
            //VALIDAR SI EL USUARIO TIENE LICENCIA
            if (datalistadoVerificacionLicencia.RowCount > 0)
            {
                //SI EL ESTADO DEL SISTEMA ES ACTIVO
                if (estadoSistema == "ACTIVO - CORRIENDO")
                {
                    //ABRIR EL PANEL DEL AREA SELECCIONADA
                    area = "Mantenimiento";
                    if (panelUsuarios.Controls.Count < 1)
                    {
                        panelUsuarios.Controls.Clear();
                        flpAreas.Visible = false;
                        panelCuentas.Visible = true;
                        ImgRegresar.Visible = true;
                        DibujarUsuario();
                    }
                    else
                    {
                        panelUsuarios.Controls.Clear();
                        flpAreas.Visible = false;
                        panelCuentas.Visible = true;
                        ImgRegresar.Visible = true;
                        DibujarUsuario();
                    }
                }
                else
                {
                    //SI EL ESTADO DEL SISITEMA NO ESTA ACTIVO
                    panelMantenimiento.Visible = true;
                }
            }
            else
            {
                //SI EL USUARIO NO TIENE LICENCIA O NO ESTA REGISTRADO
                MessageBox.Show("El dispositivo en donde está corriendo el sistema no tiene la licencia o autorización respectiva, por favor comunicarse con el área de sistemas para poder solucionar este error, Error: InvalidKeyToRun.", "Validación del Sistema");
            }
        }

        //SELECCION DEL´ÁREA DE ADMINISTRADOR
        private void btnAdministrador_Click(object sender, EventArgs e)
        {
            //VALIDAR SI EL USUARIO TIENE LICENCIA
            if (datalistadoVerificacionLicencia.RowCount > 0)
            {

                //ABRIR EL PANEL DEL AREA SELECCIONADA
                area = "Administrador";
                if (panelUsuarios.Controls.Count < 1)
                {
                    panelUsuarios.Controls.Clear();
                    flpAreas.Visible = false;
                    panelCuentas.Visible = true;
                    ImgRegresar.Visible = true;
                    DibujarUsuario();
                }
                else
                {
                    panelUsuarios.Controls.Clear();
                    flpAreas.Visible = false;
                    panelCuentas.Visible = true;
                    ImgRegresar.Visible = true;
                    DibujarUsuario();
                }
            }
            else
            {
                //SI EL USUARIO NO TIENE LICENCIA O NO ESTA REGISTRADO
                MessageBox.Show("El dispositivo en donde está corriendo el sistema no tiene la licencia o autorización respectiva, por favor comunicarse con el área de sistemas para poder solucionar este error, Error: InvalidKeyToRun.", "Validación del Sistema");
            }
        }
        //-------------------------------------------------------------------------------------------------------

        //ACCIONES GENERALES DEL LOGIN-------------------------------------------------------------------------------
        //REGRESAR
        private void ImgRegresar_Click(object sender, EventArgs e)
        {
            panelCuentas.Visible = false;
            panelUsuarios.Controls.Clear();
            panelCuentas.Visible = false;
            ImgRegresar.Visible = false;
            flpAreas.Visible = true;
            panelLogin.Visible = false;
            lblContrasenaIncorrecta.Visible = false;
            txtPassword.Clear();
        }

        //VER CONTRASEÑA
        private void tsmVer_Click(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '\0';
            tsmEsconder.Visible = true;
            tsmVer.Visible = false;
        }

        //OCULTAR CONTRASEÑA
        private void tsmEsconder_Click(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '*';
            tsmEsconder.Visible = false;
            tsmVer.Visible = true;
        }

        //CAMBIAR DE CUENTA DE USUARIOAS
        private void lblCambiarCuenta_Click(object sender, EventArgs e)
        {
            panelCuentas.Visible = true;
            panelLogin.Visible = false;
            txtPassword.Clear();
            lblContrasenaIncorrecta.Visible = false;
        }

        //CAMBIAR DE CUENTA DE USUARIOAS - ICONO
        private void btnCambiarusuarioIcono_Click(object sender, EventArgs e)
        {
            panelCuentas.Visible = true;
            panelLogin.Visible = false;
            txtPassword.Clear();
            lblContrasenaIncorrecta.Visible = false;
        }

        //INICIAR SESION DE USUARIO
        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            IniciarSesionCorrecto();
        }

        //IR A LÑA PÁGINA DE ARENAS
        private void btnLogoArenas_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.arenassrl.com.pe/");
        }

        //OCULTAR LO QUE SE ESCRIBE EN LA CONTRASEÑA
        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == (int)Keys.Enter)
            {
                IniciarSesionCorrecto();
            }
        }
        //----------------------------------------------------------------------------------------------------------

        //ACCIONES DEL RELOJ PARA LA CARGA DE LOS MENÚS PRINCIPALES-------------------------------------------------
        //PROCESOS
        private void timerProcesos_Tick(object sender, EventArgs e)
        {
            contadorInicioProcesos = contadorInicioProcesos - 1;
            this.lblContadorInicio.Text = contadorInicioAdmin.ToString();
            if (contadorInicioProcesos == 0)
            {
                this.timerProcesos.Enabled = false;
                Modulos.Procesos.MenuProcesos Comercial = new Modulos.Procesos.MenuProcesos();

                this.Hide();
                Comercial.ShowDialog();
            }
        }

        //COMERCIAL
        private void timerComercial_Tick(object sender, EventArgs e)
        {
            contadorInicioComercial = contadorInicioComercial - 1;
            this.lblContadorInicio.Text = contadorInicioAdmin.ToString();
            if (contadorInicioComercial == 0)
            {
                this.timerComercial.Enabled = false;
                Modulos.Comercial.MenuComercial Comercial = new Modulos.Comercial.MenuComercial();

                this.Hide();
                Comercial.ShowDialog();
            }
        }

        //LOGISTICA
        private void timerLogistica_Tick(object sender, EventArgs e)
        {
            contadorInicioLogistica = contadorInicioLogistica - 1;
            this.lblContadorInicio.Text = contadorInicioAdmin.ToString();
            if (contadorInicioLogistica == 0)
            {
                this.timerLogistica.Enabled = false;
                Modulos.Logistica.MenuLogistcia Logistica = new Modulos.Logistica.MenuLogistcia();

                this.Hide();
                Logistica.ShowDialog();
            }
        }

        //CONTABILIDAD
        private void timerContabilidad_Tick(object sender, EventArgs e)
        {
            contadorInicioContabilidad = contadorInicioContabilidad - 1;
            this.lblContadorInicio.Text = contadorInicioAdmin.ToString();
            if (contadorInicioContabilidad == 0)
            {
                this.timerContabilidad.Enabled = false;
                Modulos.Contabilidad.MenuContabilidad Contabilidad = new Modulos.Contabilidad.MenuContabilidad();

                this.Hide();
                Contabilidad.ShowDialog();
            }
        }

        //PRODUCCION
        private void timerProduccion_Tick(object sender, EventArgs e)
        {
            contadorInicioProduccion = contadorInicioProduccion - 1;
            this.lblContadorInicio.Text = contadorInicioProduccion.ToString();
            if (contadorInicioProduccion == 0)
            {
                this.timerProduccion.Enabled = false;
                Modulos.Produccion.MenuProduccion Produccion = new Modulos.Produccion.MenuProduccion();

                this.Hide();
                Produccion.ShowDialog();
            }
        }

        //ADMINISTRADOR
        private void timerAdmi_Tick(object sender, EventArgs e)
        {
            contadorInicioAdmin = contadorInicioAdmin - 1;
            this.lblContadorInicio.Text = contadorInicioAdmin.ToString();
            if (contadorInicioAdmin == 0)
            {
                this.timerAdmi.Enabled = false;
                Modulos.Admin.MenuPrincipal Produccion = new Modulos.Admin.MenuPrincipal();

                this.Hide();
                Produccion.ShowDialog();
            }
        }

        //INGENIERUA
        private void timerIngenieria_Tick(object sender, EventArgs e)
        {
            contadorInicioIngieneria = contadorInicioIngieneria - 1;
            this.lblContadorInicio.Text = contadorInicioIngieneria.ToString();
            if (contadorInicioIngieneria == 0)
            {
                this.timerIngenieria.Enabled = false;
                Modulos.Ingenieria.MenuIngenieria Ingenieria = new Modulos.Ingenieria.MenuIngenieria();

                this.Hide();
                Ingenieria.ShowDialog();
            }
        }

        //MANTENIMIENO
        private void timerMantenimiento_Tick(object sender, EventArgs e)
        {
            contadorInicioMantenimiento = contadorInicioMantenimiento - 1;
            this.lblContadorInicio.Text = contadorInicioMantenimiento.ToString();
            if (contadorInicioMantenimiento == 0)
            {
                this.timerMantenimiento.Enabled = false;
                Modulos.Mantenimiento.MenuMantenimiento Mantenimiento = new Modulos.Mantenimiento.MenuMantenimiento();

                this.Hide();
                Mantenimiento.ShowDialog();
            }
        }

        //CALIDAD
        private void timerCalidad_Tick(object sender, EventArgs e)
        {
            contadorInicioCalidad = contadorInicioCalidad - 1;
            this.lblContadorInicio.Text = contadorInicioCalidad.ToString();
            if (contadorInicioCalidad == 0)
            {
                this.timerCalidad.Enabled = false;
                Modulos.Calidad.MenuCalidad Calidad = new Modulos.Calidad.MenuCalidad();

                this.Hide();
                Calidad.ShowDialog();
            }
        }
        //----------------------------------------------------------------------------------------------------------

        //CARGA DE METODOS PARA EL FUNCIONAMIENTO DEL LOGIN------------------------------------------------------------
        //CARGA DE USUARIOS
        public void DibujarUsuario()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Select * from Usuarios where Estado = 'Activo' AND VisibleUsuario = 1 AND Area = '" + area + "'", con);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Label b = new Label();
                    Panel pl = new Panel();
                    PictureBox I1 = new PictureBox();

                    b.Text = rdr["Login"].ToString();
                    b.Name = rdr["IdUsuarios"].ToString();
                    b.Size = new System.Drawing.Size(160, 30);
                    b.Font = new System.Drawing.Font("Calibri", 16);
                    b.BackColor = Color.FromArgb(20, 20, 20);
                    b.ForeColor = Color.White;
                    b.Dock = DockStyle.Bottom;
                    b.TextAlign = ContentAlignment.MiddleCenter;
                    b.Cursor = Cursors.Hand;

                    pl.Size = new System.Drawing.Size(160, 184);
                    pl.BorderStyle = BorderStyle.None;
                    pl.BackColor = Color.FromArgb(20, 20, 20);

                    I1.Size = new System.Drawing.Size(160, 145);
                    I1.Dock = DockStyle.Top;
                    I1.BackgroundImage = null;
                    byte[] bi = (Byte[])rdr["Icono"];
                    MemoryStream ms = new MemoryStream(bi);
                    I1.Image = Image.FromStream(ms);
                    I1.SizeMode = PictureBoxSizeMode.Zoom;
                    I1.Tag = rdr["Login"].ToString();
                    I1.Cursor = Cursors.Hand;

                    pl.Controls.Add(b);
                    pl.Controls.Add(I1);
                    b.BringToFront();
                    panelUsuarios.Controls.Add(pl);

                    b.Click += new EventHandler(mieventolabel);
                    I1.Click += new EventHandler(mieventoimagen);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message, "Validación del sistema", MessageBoxButtons.OK);
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 1, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //COMPLEMENTO PARA EL METODO DIBUJAR USUARIOS
        private void mieventolabel(System.Object sender, EventArgs e)
        {
            txtLogin.Text = ((Label)sender).Text;
            panelLogin.Visible = true;
            panelCuentas.Visible = false;
            txtPassword.Focus();
        }

        //COMPLEMENTO PARA EL METODO DIBUJAR USUARIOS
        private void mieventoimagen(System.Object sender, EventArgs e)
        {
            txtLogin.Text = ((PictureBox)sender).Tag.ToString();
            panelLogin.Visible = true;
            panelCuentas.Visible = false;
            txtPassword.Focus();
        }

        //CARGAR USUARIO CON LA CONTRASEÑA Y LOGIN CORRECTOS
        private void CargarUsuarios()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();

                da = new SqlDataAdapter("Validar_Usuario", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@password", txtPassword.Text);
                da.SelectCommand.Parameters.AddWithValue("@login", txtLogin.Text);

                da.Fill(dt);
                dataListado.DataSource = dt;
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 1, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //FUNCION PARA EVALUAR EL INICIO DE SESION
        private void IniciarSesionCorrecto()
        {
            CargarUsuarios();
            contar();

            if (contador > 0)
            {
                Program.IdUsuario = Convert.ToInt32(dataListado.SelectedCells[1].Value.ToString());
                Program.AreaUsuario = dataListado.SelectedCells[8].Value.ToString();
                Program.RangoEfecto = Convert.ToInt32(dataListado.SelectedCells[10].Value.ToString());
                Program.NombreUsuario = dataListado.SelectedCells[2].Value.ToString();
                Program.Alias = dataListado.SelectedCells[14].Value.ToString();
                Program.UnoNombreUnoApellidoUsuario = dataListado.SelectedCells[15].Value.ToString();

                if (Program.AreaUsuario == "Administrador")
                {
                    lblNombre.Text = Program.UnoNombreUnoApellidoUsuario;
                    panelBienvenida.Visible = true;
                    contadorInicioAdmin = 20;
                    this.lblContadorInicio.Text = Convert.ToInt32(contadorInicioAdmin).ToString();
                    this.timerAdmi.Enabled = true;

                }
                else if (Program.AreaUsuario == "Procesos")
                {
                    lblNombre.Text = Program.UnoNombreUnoApellidoUsuario;
                    panelBienvenida.Visible = true;
                    contadorInicioProcesos = 20;
                    this.lblContadorInicio.Text = Convert.ToInt32(contadorInicioProcesos).ToString();
                    this.timerProcesos.Enabled = true;
                }
                else if (Program.AreaUsuario == "Comercial")
                {
                    lblNombre.Text = Program.UnoNombreUnoApellidoUsuario;
                    panelBienvenida.Visible = true;
                    contadorInicioComercial = 20;
                    this.lblContadorInicio.Text = Convert.ToInt32(contadorInicioComercial).ToString();
                    this.timerComercial.Enabled = true;
                }
                else if (Program.AreaUsuario == "Contabilidad")
                {
                    lblNombre.Text = Program.UnoNombreUnoApellidoUsuario;
                    panelBienvenida.Visible = true;
                    contadorInicioContabilidad = 20;
                    this.lblContadorInicio.Text = Convert.ToInt32(contadorInicioContabilidad).ToString();
                    this.timerContabilidad.Enabled = true;
                }
                else if (Program.AreaUsuario == "Logística")
                {
                    lblNombre.Text = Program.UnoNombreUnoApellidoUsuario;
                    panelBienvenida.Visible = true;
                    contadorInicioLogistica = 20;
                    this.lblContadorInicio.Text = Convert.ToInt32(contadorInicioLogistica).ToString();
                    this.timerLogistica.Enabled = true;
                }
                else if (Program.AreaUsuario == "Producción")
                {
                    lblNombre.Text = Program.UnoNombreUnoApellidoUsuario;
                    panelBienvenida.Visible = true;
                    contadorInicioProduccion = 20;
                    this.lblContadorInicio.Text = Convert.ToInt32(contadorInicioProduccion).ToString();
                    this.timerProduccion.Enabled = true;
                }
                else if (Program.AreaUsuario == "Ingeniería")
                {
                    lblNombre.Text = Program.UnoNombreUnoApellidoUsuario;
                    panelBienvenida.Visible = true;
                    contadorInicioIngieneria = 20;
                    this.lblContadorInicio.Text = Convert.ToInt32(contadorInicioIngieneria).ToString();
                    this.timerIngenieria.Enabled = true;
                }
                else if (Program.AreaUsuario == "Mantenimiento")
                {
                    lblNombre.Text = Program.UnoNombreUnoApellidoUsuario;
                    panelBienvenida.Visible = true;
                    contadorInicioMantenimiento = 20;
                    this.lblContadorInicio.Text = Convert.ToInt32(contadorInicioMantenimiento).ToString();
                    this.timerMantenimiento.Enabled = true;
                }
                else if (Program.AreaUsuario == "Calidad")
                {
                    lblNombre.Text = Program.UnoNombreUnoApellidoUsuario;
                    panelBienvenida.Visible = true;
                    contadorInicioCalidad = 20;
                    this.lblContadorInicio.Text = Convert.ToInt32(contadorInicioCalidad).ToString();
                    this.timerCalidad.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Ocurrio un error inesperado.", "Ingreso al Sistema", MessageBoxButtons.OK);
                    txtPassword.Focus();
                }

                ClassResourses.RegistrarAuditora(12, this.Name, 1, Program.IdUsuario, "", 0);
            }
            else if (txtPassword.Text == "")
            {
                MessageBox.Show("Debe ingresar una contraseña, ingrese una contraseña válida.", "Ingreso al Sistema", MessageBoxButtons.OK);
                txtPassword.Focus();
            }
            else
            {
                lblContrasenaIncorrecta.Visible = true;
            }
        }

        //FUNCION PARA CONTAR 
        private void contar()
        {
            int x;
            x = dataListado.Rows.Count;
            contador = (x);
        }

        //SOMBREADO DE BOTONES---------------------------------------------------------------------
        //BOTON DE PROCESOS
        private void btnProcesos_MouseHover(object sender, EventArgs e)
        {
            btnProcesos.BackColor = Color.FromArgb(48, 48, 48);
            lblLeyendaProcesos.BackColor = Color.FromArgb(48, 48, 48);
        }

        //LEYENDA DE PROCESOS
        private void btnProcesos_MouseLeave(object sender, EventArgs e)
        {
            btnProcesos.BackColor = Color.Black;
            lblLeyendaProcesos.BackColor = Color.Black;
        }

        //BOTON DE COMERCIAL
        private void btnComercial_MouseHover(object sender, EventArgs e)
        {
            btnComercial.BackColor = Color.FromArgb(48, 48, 48);
            lblleyendaComercial.BackColor = Color.FromArgb(48, 48, 48);
        }

        //LEYENDA DE COMERCIAL
        private void btnComercial_MouseLeave(object sender, EventArgs e)
        {
            btnComercial.BackColor = Color.Black;
            lblleyendaComercial.BackColor = Color.Black;
        }

        //BOTON DE PRODUCCION
        private void btnProduccion_MouseHover(object sender, EventArgs e)
        {
            btnProduccion.BackColor = Color.FromArgb(48, 48, 48);
            lblLeyendaProduccion.BackColor = Color.FromArgb(48, 48, 48);
        }

        //LEYENDA D EPRODICCION
        private void btnProduccion_MouseLeave(object sender, EventArgs e)
        {
            btnProduccion.BackColor = Color.Black;
            lblLeyendaProduccion.BackColor = Color.Black;
        }

        //BOTON DE INGENIERIA
        private void btnIngieneria_MouseHover(object sender, EventArgs e)
        {
            btnIngieneria.BackColor = Color.FromArgb(48, 48, 48);
            lblLeyendaIngieneria.BackColor = Color.FromArgb(48, 48, 48);
        }

        //LEYENDA DE INGIENERIA
        private void btnIngieneria_MouseLeave(object sender, EventArgs e)
        {
            btnIngieneria.BackColor = Color.Black;
            lblLeyendaIngieneria.BackColor = Color.Black;
        }

        //BOTON DE LOGISTICA
        private void btnLogistica_MouseHover(object sender, EventArgs e)
        {
            btnLogistica.BackColor = Color.FromArgb(48, 48, 48);
            lblLeyendaLogistica.BackColor = Color.FromArgb(48, 48, 48);
        }

        //LEYENDA DE LOGISTICA
        private void btnLogistica_MouseLeave(object sender, EventArgs e)
        {
            btnLogistica.BackColor = Color.Black;
            lblLeyendaLogistica.BackColor = Color.Black;
        }

        //BOTON DE CONTABILIDAD
        private void btnContabilidad_MouseHover(object sender, EventArgs e)
        {
            btnContabilidad.BackColor = Color.FromArgb(48, 48, 48);
            lblLeyendaContabilidad.BackColor = Color.FromArgb(48, 48, 48);
        }

        //LEYENDA DE CONTABILIDAD
        private void btnContabilidad_MouseLeave(object sender, EventArgs e)
        {
            btnContabilidad.BackColor = Color.Black;
            lblLeyendaContabilidad.BackColor = Color.Black;
        }

        //BOTON DE CALIDAD
        private void btnCalidad_MouseHover(object sender, EventArgs e)
        {
            btnCalidad.BackColor = Color.FromArgb(48, 48, 48);
            lblLeyendaCalidad.BackColor = Color.FromArgb(48, 48, 48);
        }

        //LEYENDA DE CALIDAD
        private void btnCalidad_MouseLeave(object sender, EventArgs e)
        {
            btnCalidad.BackColor = Color.Black;
            lblLeyendaCalidad.BackColor = Color.Black;
        }

        //BOTON DE SIG
        private void btnSIG_MouseHover(object sender, EventArgs e)
        {
            btnSIG.BackColor = Color.FromArgb(48, 48, 48);
            lblLeyendaSIG.BackColor = Color.FromArgb(48, 48, 48);
        }

        //LEYENDA DE SIG
        private void btnSIG_MouseLeave(object sender, EventArgs e)
        {
            btnSIG.BackColor = Color.Black;
            lblLeyendaSIG.BackColor = Color.Black;
        }

        //BOTON DE MANTENIMIENTO
        private void btnMantenimiento_MouseHover(object sender, EventArgs e)
        {
            btnMantenimiento.BackColor = Color.FromArgb(48, 48, 48);
            lblLeyendaMantenimiento.BackColor = Color.FromArgb(48, 48, 48);
        }

        //LEYENDA DE MANTENIMIENTO
        private void btnMantenimiento_MouseLeave(object sender, EventArgs e)
        {
            btnMantenimiento.BackColor = Color.Black;
            lblLeyendaMantenimiento.BackColor = Color.Black;
        }

        //BOTON ADMINISTRADOR
        private void btnAdministrador_MouseHover(object sender, EventArgs e)
        {
            btnAdministrador.BackColor = Color.FromArgb(48, 48, 48);
            lblLeyendaAdmin.BackColor = Color.FromArgb(48, 48, 48);
        }

        //LEYENDA ADMIN
        private void btnAdministrador_MouseLeave(object sender, EventArgs e)
        {
            btnAdministrador.BackColor = Color.Black;
            lblLeyendaAdmin.BackColor = Color.Black;
        }

        //EVENTAO PARA CERRAR MI FORMULARIO
        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
            ClassResourses.RegistrarAuditora(10, this.Name, 1, Program.IdUsuario = 0, "", 0);
        }
        //-----------------------------------------------------------------------------------------------
    }
}

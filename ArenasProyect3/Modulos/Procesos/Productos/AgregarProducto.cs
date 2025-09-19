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

namespace ArenasProyect3.Modulos.Procesos.Productos
{
    public partial class AgregarProducto : Form
    {
        //CREACIÓN DE VARIABLES PARA LA VALIDACIÓN, ALMACENAMIENTO Y RECONOCIMEINTO DE DATOS--
        //DETALLES DEL PRODUCTO
        string nuemroProducto = "";
        int semirpoducido = 0;
        string nombreInicial = "";

        //VARIABLES PARA ALMACENAR LOS CÓDIGOS DE LOS CAMPOS
        string idmercaderias;
        string idlinea;
        string idmodelo;
        string idTipoNN;
        string idtipocaracteristica;
        string idtipomedida;
        string iddiametros;
        string idformas;
        string idespesores;
        string iddiseñoacabado;
        string idntipos;
        string idvarioso;

        //VARIABLES PARA LOS DATOS ANEXOS
        int afectadoIGV = 0;
        int controlarstock = 0;
        int juego = 0;
        int servicio = 0;
        int controlarlotes = 0;
        int controlarserie = 0;
        int reposicion = 0;
        int sujetropercepcion = 0;
        int sujetodetraccion = 0;
        int sujetoisc = 0;

        //VARIABLES PARA CONTAR LOS ESTADOS DE LOS CAMPOS DE MI PRODUCTO
        int campocaracteristicas1 = 0;
        int campocaracteristicas2 = 0;

        int campomedidas1 = 0;
        int campomedidas2 = 0;

        int campodiametros1 = 0;
        int campodiametros2 = 0;

        int campoformas1 = 0;
        int campoformas2 = 0;

        int campoespesor1 = 0;
        int campoespesor2 = 0;

        int campodiseñoacabado1 = 0;
        int campodiseñoacabado2 = 0;

        int campontipos1 = 0;
        int campontipos2 = 0;

        int campovarioso1 = 0;
        int campovarioso2 = 0;

        int campogeneral = 0;

        //ESPACIADOS
        string espacio1 = "";
        string espacio2 = "";
        string espacio3 = "";
        string espacio4 = "";
        string espacio5 = "";
        string espacio6 = "";
        string espacio7 = "";
        string espacio8 = "";
        string espacio9 = "";
        string espacio10 = "";
        string espacio11 = "";
        string espacio12 = "";
        string espacio13 = "";
        string espacio14 = "";
        string espacio15 = "";
        string espacio16 = "";
        string espacio17 = "";
        string espacio18 = "";
        string espacio19 = "";
        string espacio20 = "";
        string espacio21 = "";
        string espacio22 = "";
        string espacio23 = "";
        string espacio24 = "";
        string espacio25 = "";
        string espacio26 = "";
        string espacio27 = "";
        string espacio28 = "";
        string espacio29 = "";
        string espacio30 = "";
        string espacio31 = " ";

        //VARIABLE DE VALIDACION GLOBAL DE INGRESO DE PRODUCTO
        bool EstadoCaracteristicas1 = true;
        bool EstadoCaracteristicas2 = true;
        bool EstadoMedidas1 = true;
        bool EstadoMedidas2 = true;
        bool EstadoDiametros1 = true;
        bool EstadoDiametros2 = true;
        bool EstadoFormas1 = true;
        bool EstadoFormas2 = true;
        bool EstadoEspesores1 = true;
        bool EstadoEspesores2 = true;
        bool EstadoDiseñoAcabados1 = true;
        bool EstadoDiseñoAcabados2 = true;
        bool EstadoNTipos1 = true;
        bool EstadoNTipos2 = true;
        bool EstadoVarios01 = true;
        bool EstadoVarios02 = true;
        bool EstadoGeneral = true;

        bool EstadoNombreProducto = true;

        bool ValidadorProducto = true;

        int EstadoValidacionCampoVacios = 0;

        string DescripicionProducto = "";

        //CONSTRUCTOR DEL MANTENIMIENTO - MANTENIEMINTO DE INGRESO DE PRODUCTOS
        public AgregarProducto()
        {
            InitializeComponent();
        }

        //Drag Form - CÓDIGO PARA MOVER EL FORMUALRIO
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int LParam);

        //EVENTO PARA MOVER MI FORMUARIO 
        private void panelPrincipal_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        //EVENTO PARA MOVER MI FORMUARIO 
        private void lblTituloNuevoProducto_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        //PRIMERA CARGA DE MI MANTENIMIENTOS DEL INGRESO DE PRODUCTOS
        private void AgregarProducto_Load(object sender, EventArgs e)
        {
            codigoProducto();
            CargarTipoMercaderia();
            CargarTipoMedida();
            cboTipoMedida.SelectedIndex = 36;
            CargarDiferencial();
            CargarOrigen();
            CargarTerminosCompra();
            TipoExistencia();
            BienesSujetoPercepcion();
        }

        //CARGAR COMBOS - PRINCIPALES-----------------------------------------------------------------------------------
        //CARGA DE CUENTAS / TIPO MERCADERIAS
        public void CargarTipoMercaderia()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdTipoMercaderias,Desciripcion,Abreviatura FROM TIPOMERCADERIAS WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboTipoMercaderia.DisplayMember = "Desciripcion";
                cboTipoMercaderia.ValueMember = "IdTipoMercaderias";
                DataRow row = dt.Rows[0];
                lblTipMercaderia.Text = System.Convert.ToString(row["Abreviatura"]);
                cboTipoMercaderia.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        //EVENTO PARA EL CAMBIO DE TIPO DE MERCADERIAS Y CAMBIO DE ABREVIATURA
        private void cboTipoMercaderia_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdTipoMercaderias,Desciripcion,Abreviatura FROM TIPOMERCADERIAS WHERE Estado = 1 AND IdTipoMercaderias = @idtipomercaderia", con);
                comando.Parameters.AddWithValue("@idtipomercaderia", System.Convert.ToString(cboTipoMercaderia.SelectedValue));
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lblTipMercaderia.Text = System.Convert.ToString(row["Abreviatura"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        //CARGAR TIPO DE MEDIDAS
        public void CargarTipoMedida()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdMedida,Descripcion FROM MEDIDA WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboTipoMedida.DisplayMember = "Descripcion";
                cboTipoMedida.ValueMember = "IdMedida";
                cboTipoMedida.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        //CARGAR LINEAS SEGUN EL TIPO DE MERCADERIAS
        public void CargarLineas(string idtipomercaderias)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdLinea,Descripcion, Abreviatura FROM LINEAS WHERE Estado = 1 AND IdTipMer = @idtipomercaderias", con);
                comando.Parameters.AddWithValue("@idtipomercaderias", idtipomercaderias);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboLineas.DisplayMember = "Descripcion";
                cboLineas.ValueMember = "IdLinea";
                DataRow row = dt.Rows[0];
                lblLinea.Text = System.Convert.ToString(row["Abreviatura"]);
                cboLineas.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        //EVENTO PARA EL CAMBIO DE LA LÍNEA Y EL CAMBIO DE LA ABREVIATURA DE ESTA
        private void cboLineas_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdLinea,Descripcion, Abreviatura FROM LINEAS WHERE Estado = 1 AND IdLinea = @id", con);
                comando.Parameters.AddWithValue("@id", System.Convert.ToString(cboLineas.SelectedValue));
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lblLinea.Text = System.Convert.ToString(row["Abreviatura"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        //CARGAR DIFERENCIALES
        public void CargarDiferencial()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdDiferencial,Descripcion FROM DIFERENCIAL WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboDiferencial.DisplayMember = "Descripcion";
                cboDiferencial.ValueMember = "IdDiferencial";
                cboDiferencial.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        //CARGAR MODELOS SEGÚN LA LÍNEA SELECCIOANDA
        public void CargarModelos(string idlinea)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdModelo,Descripcion,Abreviatura FROM MODELOS WHERE Estado = 1 AND IdLinea = @idlinea", con);
                comando.Parameters.AddWithValue("@idlinea", idlinea);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboModelos.DisplayMember = "Descripcion";
                cboModelos.ValueMember = "IdModelo";
                DataRow row = dt.Rows[0];
                lblModelo.Text = System.Convert.ToString(row["Abreviatura"]);
                cboModelos.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        //EVENTO PARA EL CAMBIO DEL MODELO Y EL CAMBIO DE LA ABREVIATURA DE ESTA
        private void cboModelos_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdModelo,Descripcion,Abreviatura FROM MODELOS WHERE Estado = 1 AND IdModelo = @id", con);
                comando.Parameters.AddWithValue("@id", System.Convert.ToString(cboModelos.SelectedValue));
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lblModelo.Text = System.Convert.ToString(row["Abreviatura"]);
                }

                //GenerarCodigoProducto();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        //COMBOS DE DATOS ANEXOS----------------------------------------------
        //CARGAR EL ORIGEN DE MIS DATOS ANEXOS
        public void CargarOrigen()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdOrigen,Descripcion FROM DatosAnexos_Origen ORDER BY(Descripcion)", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboOrigen.DisplayMember = "Descripcion";
                cboOrigen.ValueMember = "IdOrigen";
                cboOrigen.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        //CARGAR LOS TERMINOS DE COMPRA DE MIS DATOS ANEXOS
        public void CargarTerminosCompra()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdTerminosCompra,Abreviatura FROM DatosAnexos_TerminosCompra ORDER BY Descripcion DESC", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboTerminosCompra.DisplayMember = "Abreviatura";
                cboTerminosCompra.ValueMember = "IdTerminosCompra";
                cboTerminosCompra.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        //CARGA DE TIPO DE EXISTENCIA DE MIS DATOS ANEXOS
        public void TipoExistencia()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdTipoExistencia,Descripcion FROM DatosAnexos_TipoExistencia ORDER BY Descripcion", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboTipoExistencia.DisplayMember = "Descripcion";
                cboTipoExistencia.ValueMember = "IdTipoExistencia";
                cboTipoExistencia.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        //CARGAR LOS BIENES SUJETO PERCEPCIÓN
        public void BienesSujetoPercepcion()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdBienesSujetoPercepcion,Descripcion FROM DatosAnexos_BienesSujetoPercepcion ORDER BY Descripcion", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboBienesSujetoPercepcion.DisplayMember = "Descripcion";
                cboBienesSujetoPercepcion.ValueMember = "IdBienesSujetoPercepcion";
                cboBienesSujetoPercepcion.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        //CARGAR CODIGOS PARA ALMACENAR EL NUEVO PRODUCTO Y LA RESPECTIVA VALIDACION
        public void codigoProducto()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT IdArt FROM PRODUCTOS WHERE IdArt = (SELECT MAX(IdArt) FROM PRODUCTOS)", con);
                da.Fill(dt);
                dataListadoCdigoProducto.DataSource = dt;
                con.Close();

                if (dataListadoCdigoProducto.Rows.Count != 0)
                {
                    nuemroProducto = dataListadoCdigoProducto.SelectedCells[0].Value.ToString();
                    int nuemroProducto2 = 0;
                    nuemroProducto2 = Convert.ToInt32(nuemroProducto);
                    nuemroProducto2 = nuemroProducto2 + 1;

                    nuemroProducto = Convert.ToString(nuemroProducto2);
                }
                else
                {
                    MessageBox.Show("Se debe inicializar la tabla PRODUCTOS.", "Validación del Sistema", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        //CARGAR CÓDIGO DE PLANO PARA PODER INGRESAR UNO NUEVO
        public void codigoPlano()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT IdPlano FROM PlanoProducto WHERE IdPlano = (SELECT MAX(IdPlano) FROM PlanoProducto)", con);
                da.Fill(dt);
                dataListadoCdigoPlano.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        //VALIDACIÓN DE PRODUCTO POR MODELO
        public void ValidacionProducto()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("ValidacionProducto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idmodelo", cboModelos.SelectedValue.ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoValidacionProducto.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        //CARGAR CAMPOS----------------------------------------------------------------------
        //CARGA DE GRUPOS DE CAMPOS
        public void CargarGrupoCamposPredeterminados()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarGrupoCamposPredeterminados", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idmodelo", cboModelos.SelectedValue.ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoCamposPredeterminados.DataSource = dt;
                con.Close();

                //CAMPOS QUE SE GUARDAN - GRUPOS DE CAMPOS
                if (datalistadoCamposPredeterminados.Rows.Count == 0)
                {
                    MessageBox.Show("El modelo elegido no tiene campos definidos, por favor defina los campos.", "Validación del Sistema", MessageBoxButtons.OK);
                    flowLayoutPanel.Controls.Clear();
                }
                else
                {
                    //CARACTERISTICAS - 1
                    int CampCaracteristicas1 = Convert.ToInt32(datalistadoCamposPredeterminados.SelectedCells[1].Value.ToString());
                    if (CampCaracteristicas1 == 1)
                    {
                        ckCaracteristicas1.Checked = true;
                    }
                    else
                    {
                        ckCaracteristicas1.Checked = false;
                    }

                    //CARACTERISTICAS - 2
                    int CampCaracteristicas2 = Convert.ToInt32(datalistadoCamposPredeterminados.SelectedCells[2].Value.ToString());
                    if (CampCaracteristicas2 == 1)
                    {
                        ckCaracteristicas2.Checked = true;
                    }
                    else
                    {
                        ckCaracteristicas2.Checked = false;
                    }

                    //MEDIDAS - 1
                    int CampMedidas1 = Convert.ToInt32(datalistadoCamposPredeterminados.SelectedCells[3].Value.ToString());
                    if (CampMedidas1 == 1)
                    {
                        ckCamposMedida1.Checked = true;
                    }
                    else
                    {
                        ckCamposMedida1.Checked = false;
                    }

                    //MEDIDAS - 2
                    int CampMedidas2 = Convert.ToInt32(datalistadoCamposPredeterminados.SelectedCells[4].Value.ToString());
                    if (CampMedidas2 == 1)
                    {
                        ckCamposMedida2.Checked = true;
                    }
                    else
                    {
                        ckCamposMedida2.Checked = false;
                    }

                    //DIAMETROS - 1
                    int CampDiametros1 = Convert.ToInt32(datalistadoCamposPredeterminados.SelectedCells[5].Value.ToString());
                    if (CampDiametros1 == 1)
                    {
                        ckCamposDiametros1.Checked = true;
                    }
                    else
                    {
                        ckCamposDiametros1.Checked = false;
                    }

                    //DIAMETROS - 2
                    int CampDiametros2 = Convert.ToInt32(datalistadoCamposPredeterminados.SelectedCells[6].Value.ToString());
                    if (CampDiametros2 == 1)
                    {
                        ckCamposDiametros2.Checked = true;
                    }
                    else
                    {
                        ckCamposDiametros2.Checked = false;
                    }

                    //FORMAS - 1
                    int CampFormas1 = Convert.ToInt32(datalistadoCamposPredeterminados.SelectedCells[7].Value.ToString());
                    if (CampFormas1 == 1)
                    {
                        ckCamposFormas1.Checked = true;
                    }
                    else
                    {
                        ckCamposFormas1.Checked = false;
                    }

                    //FORMAS - 2
                    int CampFormas2 = Convert.ToInt32(datalistadoCamposPredeterminados.SelectedCells[8].Value.ToString());
                    if (CampFormas2 == 1)
                    {
                        ckCamposFormas2.Checked = true;
                    }
                    else
                    {
                        ckCamposFormas2.Checked = false;
                    }

                    //ESPESORES - 1
                    int CampEspesores1 = Convert.ToInt32(datalistadoCamposPredeterminados.SelectedCells[9].Value.ToString());
                    if (CampEspesores1 == 1)
                    {
                        ckCamposEspesores1.Checked = true;
                    }
                    else
                    {
                        ckCamposEspesores1.Checked = false;
                    }

                    //ESPESORES - 2
                    int CampEspesores2 = Convert.ToInt32(datalistadoCamposPredeterminados.SelectedCells[10].Value.ToString());
                    if (CampEspesores2 == 1)
                    {
                        ckCamposEspesores2.Checked = true;
                    }
                    else
                    {
                        ckCamposEspesores2.Checked = false;
                    }

                    //DISEÑO Y ACABADOS - 1
                    int CampDiseñoAcabado1 = Convert.ToInt32(datalistadoCamposPredeterminados.SelectedCells[11].Value.ToString());
                    if (CampDiseñoAcabado1 == 1)
                    {
                        ckCamposDiseñoAcabado1.Checked = true;
                    }
                    else
                    {
                        ckCamposDiseñoAcabado1.Checked = false;
                    }

                    //DISEÑO Y ACABADOS - 2
                    int CampDiseñoAcabado2 = Convert.ToInt32(datalistadoCamposPredeterminados.SelectedCells[12].Value.ToString());
                    if (CampDiseñoAcabado2 == 1)
                    {
                        ckCamposDiseñoAcabado2.Checked = true;
                    }
                    else
                    {
                        ckCamposDiseñoAcabado2.Checked = false;
                    }

                    //NUMEROS Y TIPOS - 1
                    int CampNTipos1 = Convert.ToInt32(datalistadoCamposPredeterminados.SelectedCells[13].Value.ToString());
                    if (CampNTipos1 == 1)
                    {
                        ckCamposNTipos1.Checked = true;
                    }
                    else
                    {
                        ckCamposNTipos1.Checked = false;
                    }

                    //NUMEROS Y TIPOS - 2
                    int CampNTipos2 = Convert.ToInt32(datalistadoCamposPredeterminados.SelectedCells[14].Value.ToString());
                    if (CampNTipos2 == 1)
                    {
                        ckCamposNTipos2.Checked = true;
                    }
                    else
                    {
                        ckCamposNTipos2.Checked = false;
                    }

                    //VARIOS - 1
                    int CampVarios1 = Convert.ToInt32(datalistadoCamposPredeterminados.SelectedCells[15].Value.ToString());
                    if (CampVarios1 == 1)
                    {
                        ckVariosO1.Checked = true;
                    }
                    else
                    {
                        ckVariosO1.Checked = false;
                    }

                    //VARIOS - 2
                    int CampVarios2 = Convert.ToInt32(datalistadoCamposPredeterminados.SelectedCells[16].Value.ToString());
                    if (CampVarios2 == 1)
                    {
                        ckVariosO2.Checked = true;
                    }
                    else
                    {
                        ckVariosO2.Checked = false;
                    }

                    //GENERALES
                    int CampGenerales = Convert.ToInt32(datalistadoCamposPredeterminados.SelectedCells[17].Value.ToString());
                    if (CampGenerales == 1)
                    {
                        ckGenerales.Checked = true;
                    }
                    else
                    {
                        ckGenerales.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        //CARGA DE CAMPOS Y SUS DETALLES
        public void CargarCamposPredeterminados()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarCamposPredeterminados", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idmodelo", cboModelos.SelectedValue.ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoCamposPredeterminadosDetalle.DataSource = dt;
                con.Close();

                if (datalistadoCamposPredeterminadosDetalle.RowCount == 0)
                {
                    MessageBox.Show("El modelo elegido no tiene detalles definidos, por favor defina los campos.", "Validación del Sistema", MessageBoxButtons.OK);
                    flowLayoutPanel.Controls.Clear();
                }
                else
                {
                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[1].Value != null)
                    {
                        cboTipoCaracteristicas1.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[1].Value;
                        cboDescripcionCaracteristicas1.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[2].Value != null)
                    {
                        cboTipoCaracteristicas2.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[2].Value;
                        cboDescripcionCaracteristicas2.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[3].Value != null)
                    {
                        cboTipoCaracteristicas3.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[3].Value;
                        cboDescripcionCaracteristicas3.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[4].Value != null)
                    {
                        cboTipoCaracteristicas4.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[4].Value;
                        cboDescripcionCaracteristicas4.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[5].Value != null)
                    {
                        cboTipoMedida1.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[5].Value;
                        cboDescripcionMedida1.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[6].Value != null)
                    {
                        cboTipoMedida2.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[6].Value;
                        cboDescripcionMedida2.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[7].Value != null)
                    {
                        cboTipoMedida3.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[7].Value;
                        cboDescripcionMedida3.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[8].Value != null)
                    {
                        cboTipoMedida4.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[8].Value;
                        cboDescripcionMedida4.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[9].Value != null)
                    {
                        cboTiposDiametros1.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[9].Value;
                        cboDescripcionDiametros1.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[10].Value != null)
                    {
                        cboTiposDiametros2.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[10].Value;
                        cboDescripcionDiametros2.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[11].Value != null)
                    {
                        cboTiposDiametros3.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[11].Value;
                        cboDescripcionDiametros3.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[12].Value != null)
                    {
                        cboTiposDiametros4.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[12].Value;
                        cboDescripcionDiametros4.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[13].Value != null)
                    {
                        cboTiposFormas1.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[13].Value;
                        cboDescripcionFormas1.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[14].Value != null)
                    {
                        cboTiposFormas2.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[14].Value;
                        cboDescripcionFormas2.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[15].Value != null)
                    {
                        cboTiposFormas3.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[15].Value;
                        cboDescripcionFormas3.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[16].Value != null)
                    {
                        cboTiposFormas4.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[16].Value;
                        cboDescripcionFormas4.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[17].Value != null)
                    {
                        cbooTipoEspesores1.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[17].Value;
                        cboDescripcionEspesores1.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[18].Value != null)
                    {
                        cbooTipoEspesores2.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[18].Value;
                        cboDescripcionEspesores2.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[19].Value != null)
                    {
                        cbooTipoEspesores3.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[19].Value;
                        cboDescripcionEspesores3.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[20].Value != null)
                    {
                        cbooTipoEspesores4.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[20].Value;
                        cboDescripcionEspesores4.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[21].Value != null)
                    {
                        cboTiposDiseñosAcabados1.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[21].Value;
                        cboDescripcionDiseñoAcabado1.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[22].Value != null)
                    {
                        cboTiposDiseñosAcabados2.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[22].Value;
                        cboDescripcionDiseñoAcabado2.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[23].Value != null)
                    {
                        cboTiposDiseñosAcabados3.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[23].Value;
                        cboDescripcionDiseñoAcabado3.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[24].Value != null)
                    {
                        cboTiposDiseñosAcabados4.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[24].Value;
                        cboDescripcionDiseñoAcabado4.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[25].Value != null)
                    {
                        cboTiposNTipos1.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[25].Value;
                        cboDescripcionNTipos1.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[26].Value != null)
                    {
                        cboTiposNTipos2.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[26].Value;
                        cboDescripcionNTipos2.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[27].Value != null)
                    {
                        cboTiposNTipos3.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[27].Value;
                        cboDescripcionNTipos3.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[28].Value != null)
                    {
                        cboTiposNTipos4.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[28].Value;
                        cboDescripcionNTipos4.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[29].Value != null)
                    {
                        cboTiposVariosO1.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[29].Value;
                        cboDescripcionVariosO1.SelectedIndex = -1;
                    }

                    if (datalistadoCamposPredeterminadosDetalle.SelectedCells[30].Value != null)
                    {
                        cboTiposVariosO2.SelectedValue = datalistadoCamposPredeterminadosDetalle.SelectedCells[30].Value;
                        cboDescripcionVariosO2.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        //ACCIONES INTERNAS DE MIS COMBOS PRINCIPALES---------------------------------------
        //CARGA DE LAS CUENTAS
        public void CargarCuentas(ComboBox cbo)
        {
            if (cbo.SelectedValue.ToString() != null)
            {
                idmercaderias = cbo.SelectedValue.ToString();
                CargarLineas(idmercaderias);
            }
            GenerarCodigoProducto();
        }

        //CARGA DE LÍNEAS SEGÚN EL TIPO DE CUENTA SELECCIOANDA
        private void cboTipoMercaderia_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarCuentas(cboTipoMercaderia);
        }

        //CARGA DE LÍNEAS SEGÚN EL TIPO DE CUENTA SELECCIOANDA
        public void CargarLineasXCuentaSeleccionada(ComboBox cbo)
        {
            if (cbo.SelectedValue.ToString() != null)
            {
                idlinea = cbo.SelectedValue.ToString();
                CargarModelos(idlinea);
            }
            GenerarCodigoProducto();
        }

        //CARGA DE LÍNEAS SEGÚN EL TIPO DE CUENTA SELECCIOANDA
        private void cboLineas_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarLineasXCuentaSeleccionada(cboLineas);
        }

        //------------------------------------------------------------------------------------------------------------------------------------
        //METODOS PARA LA CARGA DE LOS GRUPOS DE CAMPOS Y SUS DETALLES SEGÚN EL MODELO SELECCIONADO

        //CARGA DE MI GRUPO DE CAMPOS Y LOS CAMPOS CON SUS DETALLES SEGÚN EL MODELO SELECCIONADO
        public void LimpiezaCamposXModeloSeleccionado()
        {
            //LIMPIEZA DE CAMPOS
            espacio1 = "";
            espacio2 = "";
            espacio3 = "";
            espacio4 = "";
            espacio5 = "";
            espacio6 = "";
            espacio7 = "";
            espacio8 = "";
            espacio9 = "";
            espacio10 = "";
            espacio11 = "";
            espacio12 = "";
            espacio13 = "";
            espacio14 = "";
            espacio15 = "";
            espacio16 = "";
            espacio17 = "";
            espacio18 = "";
            espacio19 = "";
            espacio20 = "";
            espacio21 = "";
            espacio22 = "";
            espacio23 = "";
            espacio24 = "";
            espacio25 = "";
            espacio26 = "";
            espacio27 = "";
            espacio28 = "";
            espacio29 = "";
            espacio30 = "";
            espacio31 = " ";
            //
            txtDescripcionCaracteristicas1.Text = "";
            txtDescripcionCaracteristicas2.Text = "";
            txtDescripcionCaracteristicas3.Text = "";
            txtDescripcionCaracteristicas4.Text = "";
            txtDescripcionMedida1.Text = "";
            txtDescripcionMedida2.Text = "";
            txtDescripcionMedida3.Text = "";
            txtDescripcionMedida4.Text = "";
            txtDescripcionDiametros1.Text = "";
            txtDescripcionDiametros2.Text = "";
            txtDescripcionDiametros3.Text = "";
            txtDescripcionDiametros4.Text = "";
            txtDescripcionFormas1.Text = "";
            txtDescripcionFormas2.Text = "";
            txtDescripcionFormas3.Text = "";
            txtDescripcionFormas4.Text = "";
            txtDescripcionEspesores1.Text = "";
            txtDescripcionEspesores2.Text = "";
            txtDescripcionEspesores3.Text = "";
            txtDescripcionEspesores4.Text = "";
            txtDescripcionDiseñoAcabado1.Text = "";
            txtDescripcionDiseñoAcabado2.Text = "";
            txtDescripcionDiseñoAcabado3.Text = "";
            txtDescripcionDiseñoAcabado4.Text = "";
            txtDescripcionNTipos1.Text = "";
            txtDescripcionNTipos2.Text = "";
            txtDescripcionNTipos3.Text = "";
            txtDescripcionNTipos4.Text = "";
            txtDescripcionVariosO1.Text = "";
            txtDescripcionVariosO2.Text = "";
        }

        //CARACTERISTICAS---------------------------------------
        public void CargarGrupoCaracteristicasXModeloSeleccionado(string modelos, string TipCaracteristicas1, string TipCaracteristicas2, string TipCaracteristicas3, string TipCaracteristicas4
            , ComboBox DesCaracteristicas1, ComboBox DesCaracteristicas2, ComboBox DesCaracteristicas3, ComboBox DesCaracteristicas4)
        {
            if (!string.IsNullOrWhiteSpace(TipCaracteristicas1))
            {
                idtipocaracteristica = TipCaracteristicas1;
                idmodelo = modelos;
                CargarDescripcionCaracteristicas(DesCaracteristicas1, idtipocaracteristica, idmodelo, "0");
                DesCaracteristicas1.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(TipCaracteristicas2))
            {
                idtipocaracteristica = TipCaracteristicas2;
                idmodelo = modelos;
                CargarDescripcionCaracteristicas(DesCaracteristicas2, idtipocaracteristica, idmodelo, "0");
                DesCaracteristicas2.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(TipCaracteristicas3))
            {
                idtipocaracteristica = TipCaracteristicas3;
                idmodelo = modelos;
                CargarDescripcionCaracteristicas(DesCaracteristicas3, idtipocaracteristica, idmodelo, "0");
                DesCaracteristicas3.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(TipCaracteristicas4))
            {
                idtipocaracteristica = TipCaracteristicas4;
                idmodelo = modelos;
                CargarDescripcionCaracteristicas(DesCaracteristicas4, idtipocaracteristica, idmodelo, "0");
                DesCaracteristicas4.SelectedIndex = -1;
            }
        }

        //MEDIDAS-----------------------------------------
        public void CargarGrupoMedidasXModeloSeleccionado(string modelos, string TipMedidas1, string TipMedidas2, string TipMedidas3, string TipMedidas4
            , ComboBox Desmedidas1, ComboBox Desmedidas2, ComboBox Desmedidas3, ComboBox Desmedidas4)
        {
            if (!string.IsNullOrWhiteSpace(TipMedidas1))
            {
                idtipomedida = TipMedidas1;
                idmodelo = modelos;
                CargarDescripcionMedidas(Desmedidas1, idtipomedida, idmodelo);
                Desmedidas1.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(TipMedidas2))
            {
                idtipomedida = TipMedidas2;
                idmodelo = modelos;
                CargarDescripcionMedidas(Desmedidas2, idtipomedida, idmodelo);
                Desmedidas2.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(TipMedidas3))
            {
                idtipomedida = TipMedidas3;
                idmodelo = modelos;
                CargarDescripcionMedidas(Desmedidas3, idtipomedida, idmodelo);
                Desmedidas3.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(TipMedidas4))
            {
                idtipomedida = TipMedidas4;
                idmodelo = modelos;
                CargarDescripcionMedidas(Desmedidas4, idtipomedida, idmodelo);
                Desmedidas4.SelectedIndex = -1;
            }
        }

        //DIAMETROS--------------------------------
        public void CargarGrupoDiametrosXModeloSeleccionado(string modelos, string TipDiametros1, string TipDiametros2, string TipDiametros3, string TipDiametros4
            , ComboBox DesDiametros1, ComboBox DesDiametros2, ComboBox DesDiametros3, ComboBox DesDiametros4)
        {
            if (!string.IsNullOrWhiteSpace(TipDiametros1))
            {
                iddiametros = TipDiametros1;
                idmodelo = modelos;
                CargarDescripcionDiametros(DesDiametros1, iddiametros, idmodelo);
                DesDiametros1.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(TipDiametros2))
            {
                iddiametros = TipDiametros2;
                idmodelo = modelos;
                CargarDescripcionDiametros(DesDiametros2, iddiametros, idmodelo);
                DesDiametros2.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(TipDiametros3))
            {
                iddiametros = TipDiametros3;
                idmodelo = Convert.ToString(modelos);
                CargarDescripcionDiametros(DesDiametros3, iddiametros, idmodelo);
                DesDiametros3.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(TipDiametros4))
            {
                iddiametros = TipDiametros4;
                idmodelo = modelos;
                CargarDescripcionDiametros(DesDiametros4, iddiametros, idmodelo);
                DesDiametros4.SelectedIndex = -1;
            }
        }

        //FORMAS--------------------------
        public void CargarGrupoFormasXModeloSeleccionado(string modelos, string TipFormas1, string TipFormas2, string TipFormas3, string TipFormas4
            , ComboBox DesFormas1, ComboBox DesFormas2, ComboBox DesFormas3, ComboBox DesFormas4)
        {
            if (!string.IsNullOrWhiteSpace(TipFormas1))
            {
                idformas = TipFormas1;
                idmodelo = modelos;
                CargarDescripcionFormas(DesFormas1, idformas, idmodelo);
                DesFormas1.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(TipFormas2))
            {
                idformas = TipFormas2;
                idmodelo = modelos;
                CargarDescripcionFormas(DesFormas2, idformas, idmodelo);
                DesFormas2.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(TipFormas3))
            {
                idformas = TipFormas3;
                idmodelo = modelos;
                CargarDescripcionFormas(DesFormas3, idformas, idmodelo);
                DesFormas3.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(TipFormas4))
            {
                idformas = TipFormas4;
                idmodelo = modelos;
                CargarDescripcionFormas(DesFormas4, idformas, idmodelo);
                DesFormas4.SelectedIndex = -1;
            }
        }

        //ESPESORES----------------
        public void CargarGrupoEspesoresXModeloSeleccionado(string modelos, string TipEspesores1, string TipEspesores2, string TipEspesores3, string TipEspesores4
            , ComboBox DesEspesores1, ComboBox DesEspesores2, ComboBox DesEspesores3, ComboBox DesEspesores4)
        {
            if (!string.IsNullOrWhiteSpace(TipEspesores1))
            {
                idespesores = TipEspesores1;
                idmodelo = modelos;
                CargarDescripcionEspesores(DesEspesores1, idespesores, idmodelo);
                DesEspesores1.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(TipEspesores2))
            {
                idespesores = TipEspesores2;
                idmodelo = modelos;
                CargarDescripcionEspesores(DesEspesores2, idespesores, idmodelo);
                DesEspesores2.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(TipEspesores3))
            {
                idespesores = TipEspesores3;
                idmodelo = modelos;
                CargarDescripcionEspesores(DesEspesores3, idespesores, idmodelo);
                DesEspesores3.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(TipEspesores4))
            {
                idespesores = TipEspesores4;
                idmodelo = modelos;
                CargarDescripcionEspesores(DesEspesores4, idespesores, idmodelo);
                DesEspesores4.SelectedIndex = -1;
            }
        }

        //DISEÑO----------------
        public void CargarGrupoDiseñoAcabadoXModeloSeleccionado(string modelos, string TipDiseñoacabado1, string TipDiseñoacabado2, string TipDiseñoacabado3, string TipDiseñoacabado4
            , ComboBox DesDiseñoAcabado1, ComboBox DesDiseñoAcabado2, ComboBox DesDiseñoAcabado3, ComboBox DesDiseñoAcabado4)
        {
            if (!string.IsNullOrWhiteSpace(TipDiseñoacabado1))
            {
                iddiseñoacabado = TipDiseñoacabado1;
                idmodelo = modelos;
                CargarDescripcionDiseñoAcabado(DesDiseñoAcabado1, iddiseñoacabado, idmodelo, "0");
                DesDiseñoAcabado1.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(TipDiseñoacabado2))
            {
                iddiseñoacabado = TipDiseñoacabado2;
                idmodelo = modelos;
                CargarDescripcionDiseñoAcabado(DesDiseñoAcabado2, iddiseñoacabado, idmodelo, "0");
                DesDiseñoAcabado2.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(TipDiseñoacabado3))
            {
                iddiseñoacabado = TipDiseñoacabado3;
                idmodelo = modelos;
                CargarDescripcionDiseñoAcabado(DesDiseñoAcabado3, iddiseñoacabado, idmodelo, "0");
                DesDiseñoAcabado3.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(TipDiseñoacabado4))
            {
                iddiseñoacabado = TipDiseñoacabado4;
                idmodelo = modelos;
                CargarDescripcionDiseñoAcabado(DesDiseñoAcabado4, iddiseñoacabado, idmodelo, "0");
                DesDiseñoAcabado4.SelectedIndex = -1;
            }
        }

        //N Y TIPOS---------------------
        public void CargarGrupoNtiposXModeloSeleccionado(string modelos, string Ntipos1, string Ntipos2, string Ntipos3, string Ntipos4
            , ComboBox DesNtipos1, ComboBox DesNtipos2, ComboBox DesNtipos3, ComboBox DesNtipos4)
        {
            if (!string.IsNullOrWhiteSpace(Ntipos1))
            {
                idntipos = Ntipos1;
                idmodelo = modelos;
                CargarDescripcionNTipos(DesNtipos1, idntipos, idmodelo, "0");
                DesNtipos1.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(Ntipos2))
            {
                idntipos = Ntipos2;
                idmodelo = modelos;
                CargarDescripcionNTipos(DesNtipos2, idntipos, idmodelo, "0");
                DesNtipos2.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(Ntipos3))
            {
                idntipos = Ntipos3;
                idmodelo = modelos;
                CargarDescripcionNTipos(DesNtipos3, idntipos, idmodelo, "0");
                DesNtipos3.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(Ntipos4))
            {
                idntipos = Ntipos4;
                idmodelo = modelos;
                CargarDescripcionNTipos(DesNtipos4, idntipos, idmodelo, "0");
                DesNtipos4.SelectedIndex = -1;
            }
        }

        //VARIOS-----------------------
        public void CargarGrupoVariosXModeloSeleccionado(string modelos, string VariosO1, string VariosO2, ComboBox DesVariosO1, ComboBox DesVariosO2)
        {

            if (!string.IsNullOrWhiteSpace(VariosO1))
            {
                idvarioso = VariosO1;
                idmodelo = modelos;
                CargarDescripcionVariosO(DesVariosO1, idvarioso, idmodelo, "0");
                DesVariosO1.SelectedIndex = -1;
            }

            if (!string.IsNullOrWhiteSpace(VariosO2))
            {
                idvarioso = VariosO2;
                idmodelo = modelos;
                CargarDescripcionVariosO(DesVariosO2, idvarioso, idmodelo, "0");
                DesVariosO2.SelectedIndex = -1;
            }
        }

        //COMBINACION
        public void CargaGrupoCamposXModeloSeleccionado()
        {
            GenerarCodigoProducto();

            //DEFINICION DE NOMBRES Y DE UNOS CAMPOS MÁS SEGUN MODELO
            DefinicionModelosAtributos();
            //-------------------------------------------------------------
            CargarGrupoCamposPredeterminados();
            CargarCamposPredeterminados();
            LimpiezaCamposXModeloSeleccionado();

            //CARGA DE LOS GRUPOS DE CAMPOS SEGÚN EL MODELO SELECCIONADO
            CargarGrupoCaracteristicasXModeloSeleccionado(Convert.ToString(cboModelos.SelectedValue), Convert.ToString(cboTipoCaracteristicas1.SelectedValue), Convert.ToString(cboTipoCaracteristicas2.SelectedValue), Convert.ToString(cboTipoCaracteristicas3.SelectedValue), Convert.ToString(cboTipoCaracteristicas4.SelectedValue), cboDescripcionCaracteristicas1, cboDescripcionCaracteristicas2, cboDescripcionCaracteristicas3, cboDescripcionCaracteristicas4);
            CargarGrupoMedidasXModeloSeleccionado(Convert.ToString(cboModelos.SelectedValue), Convert.ToString(cboTipoMedida1.SelectedValue), Convert.ToString(cboTipoMedida2.SelectedValue), Convert.ToString(cboTipoMedida3.SelectedValue), Convert.ToString(cboTipoMedida4.SelectedValue), cboDescripcionMedida1, cboDescripcionMedida2, cboDescripcionMedida3, cboDescripcionMedida4);
            CargarGrupoDiametrosXModeloSeleccionado(Convert.ToString(cboModelos.SelectedValue), Convert.ToString(cboTiposDiametros1.SelectedValue), Convert.ToString(cboTiposDiametros2.SelectedValue), Convert.ToString(cboTiposDiametros3.SelectedValue), Convert.ToString(cboTiposDiametros4.SelectedValue), cboDescripcionDiametros1, cboDescripcionDiametros2, cboDescripcionDiametros3, cboDescripcionDiametros4);
            CargarGrupoFormasXModeloSeleccionado(Convert.ToString(cboModelos.SelectedValue), Convert.ToString(cboTiposFormas1.SelectedValue), Convert.ToString(cboTiposFormas2.SelectedValue), Convert.ToString(cboTiposFormas3.SelectedValue), Convert.ToString(cboTiposFormas4.SelectedValue), cboDescripcionFormas1, cboDescripcionFormas2, cboDescripcionFormas3, cboDescripcionFormas4);
            CargarGrupoEspesoresXModeloSeleccionado(Convert.ToString(cboModelos.SelectedValue), Convert.ToString(cbooTipoEspesores1.SelectedValue), Convert.ToString(cbooTipoEspesores2.SelectedValue), Convert.ToString(cbooTipoEspesores3.SelectedValue), Convert.ToString(cbooTipoEspesores4.SelectedValue), cboDescripcionEspesores1, cboDescripcionEspesores2, cboDescripcionEspesores3, cboDescripcionEspesores4);
            CargarGrupoDiseñoAcabadoXModeloSeleccionado(Convert.ToString(cboModelos.SelectedValue), Convert.ToString(cboTiposDiseñosAcabados1.SelectedValue), Convert.ToString(cboTiposDiseñosAcabados2.SelectedValue), Convert.ToString(cboTiposDiseñosAcabados3.SelectedValue), Convert.ToString(cboTiposDiseñosAcabados4.SelectedValue), cboDescripcionDiseñoAcabado1, cboDescripcionDiseñoAcabado2, cboDescripcionDiseñoAcabado3, cboDescripcionDiseñoAcabado4);
            CargarGrupoNtiposXModeloSeleccionado(Convert.ToString(cboModelos.SelectedValue), Convert.ToString(cboTiposNTipos1.SelectedValue), Convert.ToString(cboTiposNTipos2.SelectedValue), Convert.ToString(cboTiposNTipos3.SelectedValue), Convert.ToString(cboTiposNTipos4.SelectedValue), cboDescripcionNTipos1, cboDescripcionNTipos2, cboDescripcionNTipos3, cboDescripcionNTipos4);
            CargarGrupoVariosXModeloSeleccionado(Convert.ToString(cboModelos.SelectedValue), Convert.ToString(cboTiposVariosO1.SelectedValue), Convert.ToString(cboTiposVariosO2.SelectedValue), cboDescripcionVariosO1, cboDescripcionVariosO2);

            //DEFINICION DE LA ESTRUCTURA DEL NOMBRE DE MI MODELOI
            //ESTANDARIZACION DE MODELO POR CODIGO
            DefinicionModelosTexto();
        }

        //CARGA DE MI GRUPO DE CAMPOS Y LOS CAMPOS CON SUS DETALLES SEGÚN EL MODELO SELECCIONADO
        private void cboModelos_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargaGrupoCamposXModeloSeleccionado();
        }

        //ACCIONES DE LOS BOTONES DE FUNCIONALIDAD--------------------------------------------------------------
        //CARGAR COMBOS - CARACTERISTICAS DEL PRODUCTO-----------------------------------------------------------------------
        public void CargarTiposCaracteriticas(ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("AgregarProducto_CargarTiposCaracteristicas", con);
                comando.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdTipoCaracteristicas";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR DESCRIPCIÓN DE CARACTERISTICAS
        public void CargarDescripcionCaracteristicas(ComboBox cbo, string idtipocaracteristicas, string idmodelo, string idTipoNN)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("AgregarProducto_CargarDescripcionCaracteristicas", con);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@idtipocaracteristicas", idtipocaracteristicas);
                comando.Parameters.AddWithValue("@idmodelo", idmodelo);
                comando.Parameters.AddWithValue("@idTipoNN", idTipoNN);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdDescripcionCaracteristicas";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR GRUPO DE CAMPOS Y CAMPOS SEGÚN LOS GRUPOS SELECCIOANDO 1
        private void ckCaracteristicas1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckCaracteristicas1.Checked == true)
            {
                flowLayoutPanel.Controls.Add(panelCamposCaracteristicas1);
                CargarTiposCaracteriticas(cboTipoCaracteristicas1);
                CargarTiposCaracteriticas(cboTipoCaracteristicas2);
            }
            else
            {
                flowLayoutPanel.Controls.Remove(panelCamposCaracteristicas1);
                espacio1 = "";
                espacio2 = "";
                espacio3 = "";
                txtDescripcionCaracteristicas1.Text = "";
                txtDescripcionCaracteristicas2.Text = "";
            }
        }

        //CARGAR GRUPO DE CAMPOS Y CAMPOS SEGÚN LOS GRUPOS SELECCIOANDO 2
        private void ckCaracteristicas2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckCaracteristicas2.Checked == true)
            {
                flowLayoutPanel.Controls.Add(panelCamposCaracteristicas2);
                CargarTiposCaracteriticas(cboTipoCaracteristicas3);
                CargarTiposCaracteriticas(cboTipoCaracteristicas4);
            }
            else
            {
                flowLayoutPanel.Controls.Remove(panelCamposCaracteristicas2);
                espacio3 = "";
                espacio4 = "";
                espacio5 = "";
                txtDescripcionCaracteristicas3.Text = "";
                txtDescripcionCaracteristicas4.Text = "";
            }
        }

        //CARGA DEL CAMPO TIPO CARACTERISTICAS 1
        private void cboTipoCaracteristicas1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoCaracteristicas1.SelectedValue != null)
            {
                idtipocaracteristica = cboTipoCaracteristicas1.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionCaracteristicas(cboDescripcionCaracteristicas1, idtipocaracteristica, idmodelo, "0");

                if (cboTipoCaracteristicas1.Text != "NO APLICA")
                {
                    txtDescripcionCaracteristicas1.Text = cboTipoCaracteristicas1.Text + " " + cboDescripcionCaracteristicas1.Text;
                    espacio1 = " ";
                    espacio2 = " ";
                    DefinicionNombreProductoXModelo();
                }
                else
                {
                    txtDescripcionCaracteristicas1.Text = "";
                    espacio1 = "";
                    espacio2 = "";
                    DefinicionNombreProductoXModelo();
                }
            }
        }

        //CARGA DEL CAMPO DESCRIPCION CARACTERISTICAS 1
        private void cboDescripcionCaracteristicas1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoCaracteristicas1.Text != "NO APLICA")
            {
                if (ckHabilitarTextoCaracteristicas1.Checked != false)
                {
                    txtDescripcionCaracteristicas1.Text = cboTipoCaracteristicas1.Text + " " + cboDescripcionCaracteristicas1.Text;
                    espacio1 = " ";
                    espacio2 = " ";
                }
                else
                {
                    txtDescripcionCaracteristicas1.Text = cboDescripcionCaracteristicas1.Text;
                    espacio1 = " ";
                    espacio2 = " ";
                }

                //if (cboDescripcionCaracteristicas1.SelectedValue == null) { idTipoNN = "0"; }
                //else { idTipoNN = cboDescripcionCaracteristicas1.SelectedValue.ToString(); }

                ////PRODUCTOS QUIMICOS - ADHESIVOS
                //if (cboModelos.Text == "ADHESIVOS")
                //{
                //    idtipocaracteristica = cboTipoCaracteristicas2.SelectedValue.ToString();
                //    CargarDescripcionCaracteristicas(cboDescripcionCaracteristicas2, idtipocaracteristica, idmodelo, idTipoNN);
                //}
                ////PRODUCTOS QUIMICOS - COAGULANTES
                //if (cboModelos.Text == "COAGULANTES")
                //{
                //    idvarioso = cboTiposVariosO2.SelectedValue.ToString();
                //    CargarDescripcionVariosO(cboDescripcionVariosO2, idvarioso, idmodelo, idTipoNN);
                //}
                ////PRODUCTOS QUIMICOS - FLOCULANTES
                //if (cboModelos.Text == "FLOCULANTES")
                //{
                //    idtipocaracteristica = cboTipoCaracteristicas2.SelectedValue.ToString();
                //    CargarDescripcionCaracteristicas(cboDescripcionCaracteristicas2, idtipocaracteristica, idmodelo, idTipoNN);
                //}
                ////PRODUCTOS QUIMICOS - POLIURETANO Y COMPONENTES
                //if (cboModelos.Text == "POLIURETANO Y COMPONENTES")
                //{
                //    idtipocaracteristica = cboTipoCaracteristicas2.SelectedValue.ToString();
                //    CargarDescripcionCaracteristicas(cboDescripcionCaracteristicas2, idtipocaracteristica, idmodelo, idTipoNN);
                //    idtipocaracteristica = cboTipoCaracteristicas3.SelectedValue.ToString();
                //    CargarDescripcionCaracteristicas(cboDescripcionCaracteristicas3, idtipocaracteristica, idmodelo, idTipoNN);
                //}
                ////PRODUCTOS QUIMICOS - ANTIESPUMANTE
                //if (cboModelos.Text == "ANTIESPUMANTE")
                //{
                //    idvarioso = cboTiposVariosO2.SelectedValue.ToString();
                //    CargarDescripcionVariosO(cboDescripcionVariosO2, idvarioso, idmodelo, idTipoNN);
                //    idntipos = cboTiposNTipos1.SelectedValue.ToString();
                //    CargarDescripcionNTipos(cboDescripcionNTipos1, idntipos, idmodelo, idTipoNN);
                //}
                ////PRODUCTOS QUIMICOS - SUPRESOR DE POLVO
                //if (cboModelos.Text == "SUPRESOR DE POLVO")
                //{
                //    idvarioso = cboTiposVariosO2.SelectedValue.ToString();
                //    CargarDescripcionVariosO(cboDescripcionVariosO2, idvarioso, idmodelo, idTipoNN);
                //    idntipos = cboTiposNTipos1.SelectedValue.ToString();
                //    CargarDescripcionNTipos(cboDescripcionNTipos1, idntipos, idmodelo, idTipoNN);
                //}
                ////PRODUCTOS QUIMICOS - SECUESTRANTE
                //if (cboModelos.Text == "SECUESTRANTE")
                //{
                //    idvarioso = cboTiposVariosO2.SelectedValue.ToString();
                //    CargarDescripcionVariosO(cboDescripcionVariosO2, idvarioso, idmodelo, idTipoNN);
                //    idntipos = cboTiposNTipos1.SelectedValue.ToString();
                //    CargarDescripcionNTipos(cboDescripcionNTipos1, idntipos, idmodelo, idTipoNN);
                //}
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionCaracteristicas1.Text = "";
                espacio1 = "";
                espacio2 = "";
                DefinicionNombreProductoXModelo();
            }
            //SI EL CAMPO SIGUIENTE ES NO APLICABLE LE QUITA LOS ESPACIOS
            if (cboTipoCaracteristicas2.Text == "NO APLICA")
            {
                txtDescripcionCaracteristicas2.Text = "";

                espacio1 = "";
                espacio2 = "";

                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO CARACTERISTICAS 2
        private void cboTipoCaracteristicas2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoCaracteristicas2.SelectedValue != null)
            {
                idtipocaracteristica = cboTipoCaracteristicas2.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionCaracteristicas(cboDescripcionCaracteristicas2, idtipocaracteristica, idmodelo, "0");

                if (cboTipoCaracteristicas2.Text != "NO APLICA")
                {
                    txtDescripcionCaracteristicas2.Text = cboTipoCaracteristicas2.Text + " " + cboDescripcionCaracteristicas2.Text;
                    espacio2 = " ";
                    espacio3 = " ";
                    DefinicionNombreProductoXModelo();
                }
                else
                {
                    txtDescripcionCaracteristicas2.Text = "";
                    espacio2 = "";
                    espacio3 = "";
                    DefinicionNombreProductoXModelo();
                }
            }
        }

        //CARGA DEL CAMPO DESCRIPCION CARACTERISTICAS 2
        private void cboDescripcionCaracteristicas2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoCaracteristicas2.Text != "NO APLICA")
            {
                if (ckHabilitarTextoCaracteristicas2.Checked != false)
                {
                    txtDescripcionCaracteristicas2.Text = cboTipoCaracteristicas2.Text + " " + cboDescripcionCaracteristicas2.Text;
                    espacio2 = " ";
                    espacio3 = " ";
                }
                else
                {
                    txtDescripcionCaracteristicas2.Text = cboDescripcionCaracteristicas2.Text;
                    espacio2 = " ";
                    espacio3 = " ";
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionCaracteristicas2.Text = "";
                espacio2 = "";
                espacio3 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO CARACTERISTICAS 3
        private void cboTipoCaracteristicas3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoCaracteristicas3.SelectedValue != null)
            {
                idtipocaracteristica = cboTipoCaracteristicas3.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionCaracteristicas(cboDescripcionCaracteristicas3, idtipocaracteristica, idmodelo, "0");

                if (cboTipoCaracteristicas3.Text != "NO APLICA")
                {
                    txtDescripcionCaracteristicas3.Text = cboTipoCaracteristicas3.Text + " " + cboDescripcionCaracteristicas3.Text;
                    espacio3 = " ";
                    espacio4 = " ";
                    DefinicionNombreProductoXModelo();
                }
                else
                {
                    txtDescripcionCaracteristicas3.Text = "";
                    espacio3 = "";
                    espacio4 = "";
                    DefinicionNombreProductoXModelo();
                }
            }
        }

        //CARGA DEL CAMPO DESCRIPCION CARACTERISTICAS 3
        private void cboDescripcionCaracteristicas3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoCaracteristicas3.Text != "NO APLICA")
            {
                if (ckHabilitarTextoCaracteristicas3.Checked != false)
                {
                    txtDescripcionCaracteristicas3.Text = cboTipoCaracteristicas3.Text + " " + cboDescripcionCaracteristicas3.Text;
                    espacio3 = " ";
                    espacio4 = " ";
                }
                else
                {
                    txtDescripcionCaracteristicas3.Text = cboDescripcionCaracteristicas3.Text;
                    espacio3 = " ";
                    espacio4 = " ";
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionCaracteristicas3.Text = "";
                espacio3 = "";
                espacio4 = "";
                DefinicionNombreProductoXModelo();
            }

            //SI EL CAMPO SIGUIENTE ES NO APLICABLE LE QUITA LOS ESPACIOS
            if (cboTipoCaracteristicas4.Text == "NO APLICA")
            {
                txtDescripcionCaracteristicas4.Text = "";
                espacio3 = "";
                espacio4 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO CARACTERISTICAS 4
        private void cboTipoCaracteristicas4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoCaracteristicas4.SelectedValue != null)
            {
                idtipocaracteristica = cboTipoCaracteristicas4.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionCaracteristicas(cboDescripcionCaracteristicas4, idtipocaracteristica, idmodelo, "0");

                if (cboTipoCaracteristicas4.Text != "NO APLICA")
                {
                    txtDescripcionCaracteristicas4.Text = cboTipoCaracteristicas4.Text + " " + cboDescripcionCaracteristicas4.Text;
                    espacio4 = " ";
                    espacio5 = " ";
                    DefinicionNombreProductoXModelo();
                }
                else
                {
                    txtDescripcionCaracteristicas4.Text = "";
                    espacio4 = "";
                    espacio5 = "";
                    DefinicionNombreProductoXModelo();
                }
            }
        }

        //CARGA DEL CAMPO DESCRIPCION CARACTERISTICAS 4
        private void cboDescripcionCaracteristicas4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoCaracteristicas4.Text != "NO APLICA")
            {
                if (ckHabilitarTextoCaracteristicas4.Checked != false)
                {
                    txtDescripcionCaracteristicas4.Text = cboTipoCaracteristicas4.Text + " " + cboDescripcionCaracteristicas4.Text;
                    espacio4 = " ";
                    espacio5 = " ";
                }
                else
                {
                    txtDescripcionCaracteristicas4.Text = cboDescripcionCaracteristicas4.Text;
                    espacio4 = " ";
                    espacio5 = " ";
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionCaracteristicas4.Text = "";
                espacio4 = "";
                espacio5 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGAR COMBOS - MEDIDAS DEL PRODUCTO-----------------------------------------------------------------------
        public void CargarTiposMedidas(ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("AgregarProducto_CargarTiposMedidas", con);
                comando.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdTipoMedidas";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR DESCRIPCIÓN DE MEDIDAS
        public void CargarDescripcionMedidas(ComboBox cbo, string idtipomedida, string idmodelo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("AgregarProducto_CargarDescripcionMedidas", con);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@idtipomedida", idtipomedida);
                comando.Parameters.AddWithValue("@idmodelo", idmodelo);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdDescripcionMedidas";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR GRUPO DE CAMPOS Y CAMPOS SEGÚN LOS GRUPOS SELECCIOANDO 1
        private void ckCamposMedida1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckCamposMedida1.Checked == true)
            {
                flowLayoutPanel.Controls.Add(panelCamposMedidas1);
                CargarTiposMedidas(cboTipoMedida1);
                CargarTiposMedidas(cboTipoMedida2);
            }
            else
            {
                flowLayoutPanel.Controls.Remove(panelCamposMedidas1);
                espacio5 = "";
                espacio6 = "";
                espacio7 = "";
                txtDescripcionMedida1.Text = "";
                txtDescripcionMedida2.Text = "";
            }
        }

        //CARGAR GRUPO DE CAMPOS Y CAMPOS SEGÚN LOS GRUPOS SELECCIOANDO 2
        private void ckCamposMedida2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckCamposMedida2.Checked == true)
            {
                flowLayoutPanel.Controls.Add(panelCamposMedidas2);
                CargarTiposMedidas(cboTipoMedida3);
                CargarTiposMedidas(cboTipoMedida4);
            }
            else
            {
                flowLayoutPanel.Controls.Remove(panelCamposMedidas2);
                espacio7 = "";
                espacio8 = "";
                espacio9 = "";
                txtDescripcionMedida3.Text = "";
                txtDescripcionMedida4.Text = "";
            }
        }

        //CARGA DEL CAMPO TIPO MEDIDAS 1
        private void cboTipoMedida1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoMedida1.SelectedValue != null)
            {
                idtipomedida = cboTipoMedida1.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionMedidas(cboDescripcionMedida1, idtipomedida, idmodelo);
            }

            if (cboTipoMedida1.Text != "NO APLICA")
            {
                txtDescripcionMedida1.Text = cboTipoMedida1.Text + " " + cboDescripcionMedida1.Text;

                espacio5 = " ";
                espacio6 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionMedida1.Text = "";
                espacio5 = "";
                espacio6 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION MEDIDAS 1
        private void cboDescripcionMedida1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoMedida1.Text != "NO APLICA")
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposMedidas WHERE Estado = 1 AND Descripcion = '" + cboTipoMedida1.Text + "'", con);
                da.Fill(dt);
                datalistadoTipoDato.DataSource = dt;
                con.Close();

                string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                if (ckHabilitarTextoMedidas1.Checked != false)
                {
                    if (Magnitud != "" && cboDescripcionMedida1.Text != "")
                    {
                        txtDescripcionMedida1.Text = cboTipoMedida1.Text + " " + cboDescripcionMedida1.Text + Magnitud;
                        espacio5 = " ";
                        espacio6 = " ";
                    }
                    else
                    {
                        txtDescripcionMedida1.Text = cboTipoMedida1.Text + " " + cboDescripcionMedida1.Text;
                        espacio5 = " ";
                        espacio6 = " ";
                    }
                }
                else
                {
                    if (Magnitud != "" && cboDescripcionMedida1.Text != "")
                    {
                        txtDescripcionMedida1.Text = cboDescripcionMedida1.Text + Magnitud;
                        espacio5 = " ";
                        espacio6 = " ";
                    }
                    else
                    {
                        txtDescripcionMedida1.Text = cboDescripcionMedida1.Text;
                        espacio5 = " ";
                        espacio6 = " ";
                    }
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionMedida1.Text = "";
                espacio5 = "";
                espacio6 = "";
                DefinicionNombreProductoXModelo();
            }

            //SI EL CAMPO SIGUIENTE ES NO APLICABLE LE QUITA LOS ESPACIOS
            if (cboTipoMedida2.Text == "NO APLICA")
            {
                txtDescripcionMedida2.Text = "";
                espacio6 = "";
                espacio7 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO MEDIDAS 2
        private void cboTipoMedida2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoMedida2.SelectedValue != null)
            {
                idtipomedida = cboTipoMedida2.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionMedidas(cboDescripcionMedida2, idtipomedida, idmodelo);
            }

            if (cboTipoMedida2.Text != "NO APLICA")
            {
                txtDescripcionMedida2.Text = cboTipoMedida2.Text + " " + cboDescripcionMedida2.Text;
                espacio6 = " ";
                espacio7 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionMedida2.Text = "";
                espacio6 = "";
                espacio7 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION MEDIDAS 2
        private void cboDescripcionMedida2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoMedida2.Text != "NO APLICA")
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposMedidas WHERE Estado = 1 AND Descripcion = '" + cboTipoMedida2.Text + "'", con);
                da.Fill(dt);
                datalistadoTipoDato.DataSource = dt;
                con.Close();

                string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                if (ckHabilitarTextoMedidas2.Checked != false)
                {
                    if (Magnitud != "" && cboDescripcionMedida2.Text != "")
                    {
                        txtDescripcionMedida2.Text = cboTipoMedida2.Text + " " + cboDescripcionMedida2.Text + Magnitud;
                        espacio6 = " ";
                        espacio7 = " ";
                    }
                    else
                    {
                        txtDescripcionMedida2.Text = cboTipoMedida2.Text + " " + cboDescripcionMedida2.Text;
                        espacio6 = " ";
                        espacio7 = " ";
                    }
                }
                else
                {
                    if (Magnitud != "" && cboDescripcionMedida2.Text != "")
                    {
                        txtDescripcionMedida2.Text = cboDescripcionMedida2.Text + Magnitud;
                        espacio6 = " ";
                        espacio7 = " ";
                    }
                    else
                    {
                        txtDescripcionMedida2.Text = cboDescripcionMedida2.Text;
                        espacio6 = " ";
                        espacio7 = " ";
                    }
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionMedida2.Text = "";
                espacio6 = "";
                espacio7 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO MEDIDAS 3
        private void cboTipoMedida3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoMedida3.SelectedValue != null)
            {
                idtipomedida = cboTipoMedida3.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionMedidas(cboDescripcionMedida3, idtipomedida, idmodelo);
            }

            if (cboTipoMedida3.Text != "NO APLICA")
            {
                txtDescripcionMedida3.Text = cboTipoMedida3.Text + " " + cboDescripcionMedida3.Text;
                espacio7 = " ";
                espacio8 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionMedida3.Text = "";
                espacio7 = "";
                espacio8 = "";
            }
        }

        //CARGA DEL CAMPO DESCRIPCION MEDIDAS 3
        private void cboDescripcionMedida3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoMedida3.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposMedidas WHERE Estado = 1 AND Descripcion = '" + cboTipoMedida3.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();

                    string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                    if (ckHabilitarTextoMedidas3.Checked != false)
                    {
                        if (Magnitud != "" && cboDescripcionMedida3.Text != "")
                        {
                            txtDescripcionMedida3.Text = cboTipoMedida3.Text + " " + cboDescripcionMedida3.Text + Magnitud;
                            espacio7 = " ";
                            espacio8 = " ";
                        }
                        else
                        {
                            txtDescripcionMedida3.Text = cboTipoMedida3.Text + " " + cboDescripcionMedida3.Text;
                            espacio7 = " ";
                            espacio8 = " ";
                        }
                    }
                    else
                    {
                        if (Magnitud != "" && cboDescripcionMedida3.Text != "")
                        {
                            txtDescripcionMedida3.Text = cboDescripcionMedida3.Text + Magnitud;
                            espacio7 = " ";
                            espacio8 = " ";
                        }
                        else
                        {
                            txtDescripcionMedida3.Text = cboDescripcionMedida3.Text;
                            espacio7 = " ";
                            espacio8 = " ";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionMedida3.Text = "";
                espacio7 = "";
                espacio8 = "";
                DefinicionNombreProductoXModelo();
            }

            //SI EL CAMPO SIGUIENTE ES NO APLICABLE LE QUITA LOS ESPACIOS
            if (cboTipoMedida4.Text == "NO APLICA")
            {
                txtDescripcionMedida4.Text = "";
                espacio8 = "";
                espacio9 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO MEDIDAS 4
        private void cboTipoMedida4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoMedida4.SelectedValue != null)
            {
                idtipomedida = cboTipoMedida4.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionMedidas(cboDescripcionMedida4, idtipomedida, idmodelo);
            }

            if (cboTipoMedida4.Text != "NO APLICA")
            {
                txtDescripcionMedida4.Text = cboTipoMedida4.Text + " " + cboDescripcionMedida4.Text;
                espacio8 = " ";
                espacio9 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionMedida4.Text = "";
                espacio8 = "";
                espacio9 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION MEDIDAS 4
        private void cboDescripcionMedida4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoMedida4.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposMedidas WHERE Estado = 1 AND Descripcion = '" + cboTipoMedida4.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();

                    string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                    if (ckHabilitarTextoMedidas4.Checked != false)
                    {
                        if (Magnitud != "" && cboDescripcionMedida4.Text != "")
                        {
                            txtDescripcionMedida4.Text = cboTipoMedida4.Text + " " + cboDescripcionMedida4.Text + Magnitud;
                            espacio8 = " ";
                            espacio9 = " ";
                        }
                        else
                        {
                            txtDescripcionMedida4.Text = cboTipoMedida4.Text + " " + cboDescripcionMedida4.Text;
                            espacio8 = " ";
                            espacio9 = " ";
                        }
                    }
                    else
                    {
                        if (Magnitud != "" && cboDescripcionMedida4.Text != "")
                        {
                            txtDescripcionMedida4.Text = cboDescripcionMedida4.Text + Magnitud;
                            espacio8 = " ";
                            espacio9 = " ";
                        }
                        else
                        {
                            txtDescripcionMedida4.Text = cboDescripcionMedida4.Text;
                            espacio8 = " ";
                            espacio9 = " ";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionMedida4.Text = "";
                espacio8 = "";
                espacio9 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGAR TIPOS DE DIAMETROS - DESCRIPCION DE DIAMETROS - SELECCIONA DE VENTANA
        public void CargarTiposDiametros(ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("AgregarProducto_CargarTiposDiametros", con);
                comando.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdTipoDiametros";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR DESCRIPCIÓN DE DIÁMETROS
        public void CargarDescripcionDiametros(ComboBox cbo, string ididametros, string idmodelo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("AgregarProducto_CargarDescripcionDiametros", con);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@idtipodiametros", ididametros);
                comando.Parameters.AddWithValue("@idmodelo", idmodelo);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdDescripcionDiametros";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR GRUPO DE CAMPOS Y CAMPOS SEGÚN LOS GRUPOS SELECCIOANDO 1
        private void ckCamposDiametros1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckCamposDiametros1.Checked == true)
            {
                flowLayoutPanel.Controls.Add(panelCamposDiametros1);
                CargarTiposDiametros(cboTiposDiametros1);
                CargarTiposDiametros(cboTiposDiametros2);
            }
            else
            {
                flowLayoutPanel.Controls.Remove(panelCamposDiametros1);
                espacio9 = "";
                espacio10 = "";
                espacio11 = "";
                txtDescripcionDiametros1.Text = "";
                txtDescripcionDiametros2.Text = "";
            }
        }

        //CARGAR GRUPO DE CAMPOS Y CAMPOS SEGÚN LOS GRUPOS SELECCIOANDO 2
        private void ckCamposDiametros2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckCamposDiametros2.Checked == true)
            {
                flowLayoutPanel.Controls.Add(panelCamposDiametros2);
                CargarTiposDiametros(cboTiposDiametros3);
                CargarTiposDiametros(cboTiposDiametros4);
            }
            else
            {
                flowLayoutPanel.Controls.Remove(panelCamposDiametros2);
                espacio11 = "";
                espacio12 = "";
                espacio13 = "";
                txtDescripcionDiametros3.Text = "";
                txtDescripcionDiametros4.Text = "";
            }
        }

        //CARGA DEL CAMPO TIPO DIÁMETRO 1
        private void cboTiposDiametros1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiametros1.SelectedValue != null)
            {
                iddiametros = cboTiposDiametros1.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionDiametros(cboDescripcionDiametros1, iddiametros, idmodelo);
            }

            if (cboTiposDiametros1.Text != "NO APLICA")
            {
                txtDescripcionDiametros1.Text = cboTiposDiametros1.Text + " " + cboDescripcionDiametros1.Text;
                espacio9 = " ";
                espacio10 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionDiametros1.Text = "";
                espacio9 = "";
                espacio10 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION DIÁMETROS 1
        private void cboDescripcionDiametros1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiametros1.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposDiametros WHERE Estado = 1 AND Descripcion = '" + cboTiposDiametros1.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();

                    string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                    if (ckHabilitarTextoDiametros1.Checked != false)
                    {
                        if (Magnitud != "" && cboDescripcionDiametros1.Text != "")
                        {
                            txtDescripcionDiametros1.Text = cboTiposDiametros1.Text + " " + cboDescripcionDiametros1.Text + Magnitud;
                            espacio9 = " ";
                            espacio10 = " ";
                        }
                        else
                        {
                            txtDescripcionDiametros1.Text = cboTiposDiametros1.Text + " " + cboDescripcionDiametros1.Text;
                            espacio9 = " ";
                            espacio10 = " ";
                        }
                    }
                    else
                    {
                        if (Magnitud != "" && cboDescripcionDiametros1.Text != "")
                        {
                            txtDescripcionDiametros1.Text = " " + cboDescripcionDiametros1.Text + Magnitud;
                            espacio9 = "";
                            espacio10 = "";
                        }
                        else
                        {
                            txtDescripcionDiametros1.Text = " " + cboDescripcionDiametros1.Text;
                            espacio9 = "";
                            espacio10 = "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionDiametros1.Text = "";
                espacio9 = "";
                espacio10 = "";
                DefinicionNombreProductoXModelo();
            }

            //SI EL CAMPO SIGUIENTE ES NO APLICABLE LE QUITA LOS ESPACIOS
            if (cboTiposDiametros2.Text == "NO APLICA")
            {
                txtDescripcionDiametros2.Text = "";
                espacio10 = "";
                espacio11 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO DIÁMETRO 2
        private void cboTiposDiametros2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiametros2.SelectedValue != null)
            {
                iddiametros = cboTiposDiametros2.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionDiametros(cboDescripcionDiametros2, iddiametros, idmodelo);
            }

            if (cboTiposDiametros2.Text != "NO APLICA")
            {
                txtDescripcionDiametros2.Text = cboTiposDiametros2.Text + " " + cboDescripcionDiametros2.Text;
                espacio10 = " ";
                espacio11 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionDiametros2.Text = "";
                espacio10 = "";
                espacio11 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION DIÁMETROS 2
        private void cboDescripcionDiametros2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiametros2.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposDiametros WHERE Estado = 1 AND Descripcion = '" + cboTiposDiametros2.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();

                    string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                    if (ckHabilitarTextoDiametros2.Checked != false)
                    {
                        if (Magnitud != "" && cboDescripcionDiametros2.Text != "")
                        {
                            txtDescripcionDiametros2.Text = cboTiposDiametros2.Text + " " + cboDescripcionDiametros2.Text + Magnitud;
                            espacio10 = " ";
                            espacio11 = " ";
                        }
                        else
                        {
                            txtDescripcionDiametros2.Text = cboTiposDiametros2.Text + " " + cboDescripcionDiametros2.Text;
                            espacio10 = " ";
                            espacio11 = " ";
                        }
                    }
                    else
                    {
                        if (Magnitud != "" && cboDescripcionDiametros2.Text != "")
                        {
                            txtDescripcionDiametros2.Text = cboDescripcionDiametros2.Text + Magnitud;
                            espacio10 = " ";
                            espacio11 = " ";
                        }
                        else
                        {
                            txtDescripcionDiametros2.Text = cboDescripcionDiametros2.Text;
                            espacio10 = " ";
                            espacio11 = " ";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionDiametros2.Text = "";
                espacio10 = "";
                espacio11 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO DIÁMETRO 3
        private void cboTiposDiametros3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiametros3.SelectedValue != null)
            {
                iddiametros = cboTiposDiametros3.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionDiametros(cboDescripcionDiametros3, iddiametros, idmodelo);
            }

            if (cboTiposDiametros3.Text != "NO APLICA")
            {
                txtDescripcionDiametros3.Text = cboTiposDiametros3.Text + " " + cboDescripcionDiametros3.Text;
                espacio11 = " ";
                espacio12 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionDiametros3.Text = "";
                espacio11 = "";
                espacio12 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION DIÁMETROS 3
        private void cboDescripcionDiametros3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiametros3.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposDiametros WHERE Estado = 1 AND Descripcion = '" + cboTiposDiametros3.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();

                    string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                    if (ckHabilitarTextoDiametros3.Checked != false)
                    {
                        if (Magnitud != "" && cboDescripcionDiametros3.Text != "")
                        {
                            txtDescripcionDiametros3.Text = cboTiposDiametros3.Text + " " + cboDescripcionDiametros3.Text + Magnitud;
                            espacio11 = " ";
                            espacio12 = " ";
                        }
                        else
                        {
                            txtDescripcionDiametros3.Text = cboTiposDiametros3.Text + " " + cboDescripcionDiametros3.Text;
                            espacio11 = " ";
                            espacio12 = " ";
                        }
                    }
                    else
                    {
                        if (Magnitud != "" && cboDescripcionDiametros3.Text != "")
                        {
                            txtDescripcionDiametros3.Text = cboDescripcionDiametros3.Text + Magnitud;
                            espacio11 = " ";
                            espacio12 = " ";
                        }
                        else
                        {
                            txtDescripcionDiametros3.Text = cboDescripcionDiametros3.Text;
                            espacio11 = " ";
                            espacio12 = " ";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionDiametros3.Text = "";

                espacio11 = "";
                espacio12 = "";

                DefinicionNombreProductoXModelo();
            }

            //SI EL CAMPO SIGUIENTE ES NO APLICABLE LE QUITA LOS ESPACIOS
            if (cboTiposDiametros4.Text == "NO APLICA")
            {
                txtDescripcionDiametros4.Text = "";

                espacio12 = "";
                espacio13 = "";

                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO DIÁMETRO 4
        private void cboTiposDiametros4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiametros4.SelectedValue != null)
            {
                iddiametros = cboTiposDiametros4.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionDiametros(cboDescripcionDiametros4, iddiametros, idmodelo);
            }

            if (cboTiposDiametros4.Text != "NO APLICA")
            {
                txtDescripcionDiametros4.Text = cboTiposDiametros4.Text + " " + cboDescripcionDiametros4.Text;
                espacio12 = " ";
                espacio13 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionDiametros4.Text = "";
                espacio12 = "";
                espacio13 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION DIÁMETROS 4
        private void cboDescripcionDiametros4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiametros4.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposDiametros WHERE Estado = 1 AND Descripcion = '" + cboTiposDiametros4.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();

                    string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                    if (ckHabilitarTextoDiametros4.Checked != false)
                    {
                        if (Magnitud != "" && cboDescripcionDiametros4.Text != "")
                        {
                            txtDescripcionDiametros4.Text = cboTiposDiametros4.Text + " " + cboDescripcionDiametros4.Text + Magnitud;
                            espacio12 = " ";
                            espacio13 = " ";
                        }
                        else
                        {
                            txtDescripcionDiametros4.Text = cboTiposDiametros4.Text + " " + cboDescripcionDiametros4.Text;
                            espacio12 = " ";
                            espacio13 = " ";
                        }
                    }
                    else
                    {
                        if (Magnitud != "" && cboDescripcionDiametros4.Text != "")
                        {
                            txtDescripcionDiametros4.Text = cboDescripcionDiametros4.Text + Magnitud;
                            espacio12 = " ";
                            espacio13 = " ";
                        }
                        else
                        {
                            txtDescripcionDiametros4.Text = cboDescripcionDiametros4.Text;
                            espacio12 = " ";
                            espacio13 = " ";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionDiametros4.Text = "";
                espacio12 = "";
                espacio13 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGAR TIPOS DE FORMAS - DESCRIPCION DE FORMAS - SELECCIONA DE VENTANA
        public void CargarTiposFormas(ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("AgregarProducto_CargarTiposFormas", con);
                comando.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdTipoFormas";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR DESCRIPCIÓN DE FORMAS
        public void CargarDescripcionFormas(ComboBox cbo, string idformas, string idmodelo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("AgregarProducto_CargarDescripcionFormas", con);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@idtipoformas", idformas);
                comando.Parameters.AddWithValue("@idmodelo", idmodelo);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdDescripcionFormas";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR GRUPO DE CAMPOS Y CAMPOS SEGÚN LOS GRUPOS SELECCIOANDO 1
        private void ckCamposFormas1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckCamposFormas1.Checked == true)
            {
                flowLayoutPanel.Controls.Add(panelCamposFormas1);
                CargarTiposFormas(cboTiposFormas1);
                CargarTiposFormas(cboTiposFormas2);
            }
            else
            {
                flowLayoutPanel.Controls.Remove(panelCamposFormas1);
                espacio13 = "";
                espacio14 = "";
                espacio15 = "";
                txtDescripcionFormas1.Text = "";
                txtDescripcionFormas2.Text = "";
            }
        }

        //CARGAR GRUPO DE CAMPOS Y CAMPOS SEGÚN LOS GRUPOS SELECCIOANDO 2
        private void ckCamposFormas2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckCamposFormas2.Checked == true)
            {
                flowLayoutPanel.Controls.Add(panelCamposFormas2);
                CargarTiposFormas(cboTiposFormas3);
                CargarTiposFormas(cboTiposFormas4);
            }
            else
            {
                flowLayoutPanel.Controls.Remove(panelCamposFormas2);
                espacio15 = "";
                espacio16 = "";
                espacio17 = "";
                txtDescripcionFormas3.Text = "";
                txtDescripcionFormas4.Text = "";
            }
        }

        //CARGA DEL CAMPO TIPO FORMAS 1
        private void cboTiposFormas1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposFormas1.SelectedValue != null)
            {
                idformas = cboTiposFormas1.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionFormas(cboDescripcionFormas1, idformas, idmodelo);
            }

            if (cboTiposFormas1.Text != "NO APLICA")
            {
                txtDescripcionFormas1.Text = cboTiposFormas1.Text + " " + cboDescripcionFormas1.Text;
                espacio13 = " ";
                espacio14 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionFormas1.Text = "";
                espacio13 = "";
                espacio14 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION FORMAS 1
        private void cboDescripcionFormas1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposFormas1.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposFormas WHERE Estado = 1 AND Descripcion = '" + cboTiposFormas1.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();

                    string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                    if (ckHabilitarTextoFormas1.Checked != false)
                    {
                        if (Magnitud != "" && cboDescripcionFormas1.Text != "")
                        {
                            txtDescripcionFormas1.Text = cboTiposFormas1.Text + " " + cboDescripcionFormas1.Text + Magnitud;
                            espacio13 = " ";
                            espacio14 = " ";
                        }
                        else
                        {
                            txtDescripcionFormas1.Text = cboTiposFormas1.Text + " " + cboDescripcionFormas1.Text;
                            espacio13 = " ";
                            espacio14 = " ";
                        }
                    }
                    else
                    {
                        if (Magnitud != "" && cboDescripcionFormas1.Text != "")
                        {
                            txtDescripcionFormas1.Text = cboDescripcionFormas1.Text + Magnitud;
                            espacio13 = " ";
                            espacio14 = " ";
                        }
                        else
                        {
                            txtDescripcionFormas1.Text = cboDescripcionFormas1.Text;
                            espacio13 = " ";
                            espacio14 = " ";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                if (cboDescripcionFormas1.SelectedValue == null) { idTipoNN = "0"; }
                else { idTipoNN = cboDescripcionFormas1.SelectedValue.ToString(); }

                //PANELES POLIURETANO - CIEGO
                if (cboModelos.Text == "CIEGO" || cboModelos.Text == "CONVENCIONAL" || cboModelos.Text == "AUTOLIMPIANTE" || cboModelos.Text == "VIBROHEXAGONAL" || cboModelos.Text == "TEEPEE" || cboModelos.Text == "OBLONGA")
                {
                    iddiseñoacabado = cboTiposDiseñosAcabados2.SelectedValue.ToString();
                    CargarDescripcionDiseñoAcabado(cboDescripcionDiseñoAcabado2, iddiseñoacabado, idmodelo, idTipoNN);
                }

                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionFormas1.Text = "";
                espacio13 = "";
                espacio14 = "";
                DefinicionNombreProductoXModelo();
            }

            //SI EL CAMPO SIGUIENTE ES NO APLICABLE LE QUITA LOS ESPACIOS
            if (cboTiposFormas2.Text == "NO APLICA")
            {
                txtDescripcionFormas2.Text = "";
                espacio14 = "";
                espacio15 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO FORMAS 2
        private void cboTiposFormas2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposFormas2.SelectedValue != null)
            {
                idformas = cboTiposFormas2.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionFormas(cboDescripcionFormas2, idformas, idmodelo);
            }

            if (cboTiposFormas2.Text != "NO APLICA")
            {
                txtDescripcionFormas2.Text = cboTiposFormas2.Text + " " + cboDescripcionFormas2.Text;
                espacio14 = " ";
                espacio15 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionFormas2.Text = "";
                espacio14 = "";
                espacio15 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION FORMAS 2
        private void cboDescripcionFormas2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposFormas2.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposFormas WHERE Estado = 1 AND Descripcion = '" + cboTiposFormas2.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();

                    string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                    if (ckHabilitarTextoFormas2.Checked != false)
                    {
                        if (Magnitud != "" && cboDescripcionFormas2.Text != "")
                        {
                            txtDescripcionFormas2.Text = cboTiposFormas2.Text + " " + cboDescripcionFormas2.Text + Magnitud;
                            espacio14 = " ";
                            espacio15 = " ";
                        }
                        else
                        {
                            txtDescripcionFormas2.Text = cboTiposFormas2.Text + " " + cboDescripcionFormas2.Text;
                            espacio14 = " ";
                            espacio15 = " ";
                        }
                    }
                    else
                    {
                        if (Magnitud != "" && cboDescripcionFormas2.Text != "")
                        {
                            txtDescripcionFormas2.Text = cboDescripcionFormas2.Text + Magnitud;
                            espacio14 = " ";
                            espacio15 = " ";
                        }
                        else
                        {
                            txtDescripcionFormas2.Text = cboDescripcionFormas2.Text;
                            espacio14 = " ";
                            espacio15 = " ";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionFormas2.Text = "";
                espacio14 = "";
                espacio15 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO FORMAS 3
        private void cboTiposFormas3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposFormas3.SelectedValue != null)
            {
                idformas = cboTiposFormas3.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionFormas(cboDescripcionFormas3, idformas, idmodelo);
            }

            if (cboTiposFormas3.Text != "NO APLICA")
            {
                txtDescripcionFormas3.Text = cboTiposFormas3.Text + " " + cboDescripcionFormas3.Text;
                espacio15 = " ";
                espacio16 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionFormas3.Text = "";
                espacio15 = "";
                espacio16 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION FORMAS 3
        private void cboDescripcionFormas3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposFormas3.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposFormas WHERE Estado = 1 AND Descripcion = '" + cboTiposFormas3.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();

                    string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                    if (ckHabilitarTextoFormas3.Checked != false)
                    {
                        if (Magnitud != "" && cboDescripcionFormas3.Text != "")
                        {
                            txtDescripcionFormas3.Text = cboTiposFormas3.Text + " " + cboDescripcionFormas3.Text + Magnitud;
                            espacio15 = " ";
                            espacio16 = " ";
                        }
                        else
                        {
                            txtDescripcionFormas3.Text = cboTiposFormas3.Text + " " + cboDescripcionFormas3.Text;
                            espacio15 = " ";
                            espacio16 = " ";
                        }
                    }
                    else
                    {
                        if (Magnitud != "" && cboDescripcionFormas3.Text != "")
                        {
                            txtDescripcionFormas3.Text = cboDescripcionFormas3.Text + Magnitud;
                            espacio15 = " ";
                            espacio16 = " ";
                        }
                        else
                        {
                            txtDescripcionFormas3.Text = cboDescripcionFormas3.Text;
                            espacio15 = " ";
                            espacio16 = " ";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionFormas3.Text = "";
                espacio15 = "";
                espacio16 = "";
                DefinicionNombreProductoXModelo();
            }

            //SI EL CAMPO SIGUIENTE ES NO APLICABLE LE QUITA LOS ESPACIOS
            if (cboTiposFormas4.Text == "NO APLICA")
            {
                txtDescripcionFormas4.Text = "";
                espacio16 = "";
                espacio17 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO FORMAS 4
        private void cboTiposFormas4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposFormas4.SelectedValue != null)
            {
                idformas = cboTiposFormas4.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionFormas(cboDescripcionFormas4, idformas, idmodelo);
            }

            if (cboTiposFormas4.Text != "NO APLICA")
            {
                txtDescripcionFormas4.Text = cboTiposFormas4.Text + " " + cboDescripcionFormas4.Text;
                espacio16 = " ";
                espacio17 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionFormas4.Text = "";
                espacio16 = "";
                espacio17 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION FORMAS 4
        private void cboDescripcionFormas4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposFormas4.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposFormas WHERE Estado = 1 AND Descripcion = '" + cboTiposFormas4.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();

                    string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                    if (ckHabilitarTextoFormas4.Checked != false)
                    {
                        if (Magnitud != "" && cboDescripcionFormas4.Text != "")
                        {
                            txtDescripcionFormas4.Text = cboTiposFormas4.Text + " " + cboDescripcionFormas4.Text + Magnitud;
                            espacio16 = " ";
                            espacio17 = " ";
                        }
                        else
                        {
                            txtDescripcionFormas4.Text = cboTiposFormas4.Text + " " + cboDescripcionFormas4.Text;
                            espacio16 = " ";
                            espacio17 = " ";
                        }
                    }
                    else
                    {
                        if (Magnitud != "" && cboDescripcionFormas4.Text != "")
                        {
                            txtDescripcionFormas4.Text = cboDescripcionFormas4.Text + Magnitud;
                            espacio16 = " ";
                            espacio17 = " ";
                        }
                        else
                        {
                            txtDescripcionFormas4.Text = cboDescripcionFormas4.Text;
                            espacio16 = " ";
                            espacio17 = " ";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionFormas4.Text = "";
                espacio16 = "";
                espacio17 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGAR TIPOS DE ESPESORES - DESCRIPCION DE ESPESORES - SELECCIONA DE VENTANA
        public void CargarTiposEspesores(ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("AgregarProducto_CargarTiposEspesores", con);
                comando.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdTipoEspesores";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR DESCRIPCIÓN DE ESPESORES
        public void CargarDescripcionEspesores(ComboBox cbo, string idespesores, string idmodelo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("AgregarProducto_CargarDescripcionEspesores", con);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@idtipoespesores", idespesores);
                comando.Parameters.AddWithValue("@idmodelo", idmodelo);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdDescripcionEspesores";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR GRUPO DE CAMPOS Y CAMPOS SEGÚN LOS GRUPOS SELECCIOANDO 1
        private void ckCamposEspesores1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckCamposEspesores1.Checked == true)
            {
                flowLayoutPanel.Controls.Add(panelCamposEspesores1);
                CargarTiposEspesores(cbooTipoEspesores1);
                CargarTiposEspesores(cbooTipoEspesores2);
            }
            else
            {
                flowLayoutPanel.Controls.Remove(panelCamposEspesores1);
                espacio17 = "";
                espacio18 = "";
                espacio19 = "";
                txtDescripcionEspesores1.Text = "";
                txtDescripcionEspesores2.Text = "";
            }
        }

        //CARGAR GRUPO DE CAMPOS Y CAMPOS SEGÚN LOS GRUPOS SELECCIOANDO 2
        private void ckCamposEspesores2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckCamposEspesores2.Checked == true)
            {
                flowLayoutPanel.Controls.Add(panelCamposEspesores2);
                CargarTiposEspesores(cbooTipoEspesores3);
                CargarTiposEspesores(cbooTipoEspesores4);
            }
            else
            {
                flowLayoutPanel.Controls.Remove(panelCamposEspesores2);
                espacio19 = "";
                espacio20 = "";
                espacio21 = "";
                txtDescripcionEspesores3.Text = "";
                txtDescripcionEspesores4.Text = "";
            }
        }

        //CARGA DEL CAMPO TIPO ESPESORES 1
        private void cbooTipoEspesores1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbooTipoEspesores1.SelectedValue != null)
            {
                idespesores = cbooTipoEspesores1.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionEspesores(cboDescripcionEspesores1, idespesores, idmodelo);
            }

            if (cbooTipoEspesores1.Text != "NO APLICA")
            {
                txtDescripcionEspesores1.Text = cbooTipoEspesores1.Text + " " + cboDescripcionEspesores1.Text;
                espacio17 = " ";
                espacio18 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionEspesores1.Text = "";
                espacio17 = "";
                espacio18 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION ESPESORES 1
        private void cboDescripcionEspesores1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbooTipoEspesores1.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposEspesores WHERE Estado = 1 AND Descripcion = '" + cbooTipoEspesores1.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();

                    string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                    if (ckHabilitarTextoEspesores1.Checked != false)
                    {
                        if (Magnitud != "" && cboDescripcionEspesores1.Text != "")
                        {
                            txtDescripcionEspesores1.Text = cbooTipoEspesores1.Text + " " + cboDescripcionEspesores1.Text + Magnitud;
                            espacio17 = " ";
                            espacio18 = " ";
                        }
                        else
                        {
                            txtDescripcionEspesores1.Text = cbooTipoEspesores1.Text + " " + cboDescripcionEspesores1.Text;
                            espacio17 = " ";
                            espacio18 = " ";
                        }
                    }
                    else
                    {
                        if (Magnitud != "" && cboDescripcionEspesores1.Text != "")
                        {
                            txtDescripcionEspesores1.Text = cboDescripcionEspesores1.Text + Magnitud;
                            espacio17 = " ";
                            espacio18 = " ";
                        }
                        else
                        {
                            txtDescripcionEspesores1.Text = cboDescripcionEspesores1.Text;
                            espacio17 = " ";
                            espacio18 = " ";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionEspesores1.Text = "";
                espacio17 = "";
                espacio18 = "";
                DefinicionNombreProductoXModelo();
            }

            //SI EL CAMPO SIGUIENTE ES NO APLICABLE LE QUITA LOS ESPACIOS
            if (cbooTipoEspesores2.Text == "NO APLICA")
            {
                txtDescripcionEspesores2.Text = "";
                espacio18 = "";
                espacio19 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO ESPESORES 2
        private void cbooTipoEspesores2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbooTipoEspesores2.SelectedValue != null)
            {
                idespesores = cbooTipoEspesores2.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionEspesores(cboDescripcionEspesores2, idespesores, idmodelo);
            }

            if (cbooTipoEspesores2.Text != "NO APLICA")
            {
                txtDescripcionEspesores2.Text = cbooTipoEspesores2.Text + " " + cboDescripcionEspesores2.Text;
                espacio18 = " ";
                espacio19 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionEspesores2.Text = "";
                espacio18 = "";
                espacio19 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION ESPESORES 2
        private void cboDescripcionEspesores2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbooTipoEspesores2.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposEspesores WHERE Estado = 1 AND Descripcion = '" + cbooTipoEspesores2.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();

                    string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                    if (ckHabilitarTextoEspesores2.Checked != false)
                    {
                        if (Magnitud != "" && cboDescripcionEspesores2.Text != "")
                        {
                            txtDescripcionEspesores2.Text = cbooTipoEspesores2.Text + " " + cboDescripcionEspesores2.Text + Magnitud;
                            espacio18 = " ";
                            espacio19 = " ";
                        }
                        else
                        {
                            txtDescripcionEspesores2.Text = cbooTipoEspesores2.Text + " " + cboDescripcionEspesores2.Text;
                            espacio18 = " ";
                            espacio19 = " ";
                        }
                    }
                    else
                    {
                        if (Magnitud != "" && cboDescripcionEspesores2.Text != "")
                        {
                            txtDescripcionEspesores2.Text = cboDescripcionEspesores2.Text + Magnitud;
                            espacio18 = " ";
                            espacio19 = " ";
                        }
                        else
                        {
                            txtDescripcionEspesores2.Text = cboDescripcionEspesores2.Text;
                            espacio18 = " ";
                            espacio19 = " ";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionEspesores2.Text = "";
                espacio18 = "";
                espacio19 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO ESPESORES 3
        private void cbooTipoEspesores3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbooTipoEspesores3.SelectedValue != null)
            {
                idespesores = cbooTipoEspesores3.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionEspesores(cboDescripcionEspesores3, idespesores, idmodelo);
            }

            if (cbooTipoEspesores3.Text != "NO APLICA")
            {
                txtDescripcionEspesores3.Text = cbooTipoEspesores3.Text + " " + cboDescripcionEspesores3.Text;
                espacio19 = " ";
                espacio20 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionEspesores3.Text = "";
                espacio19 = "";
                espacio20 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION ESPESORES 3
        private void cboDescripcionEspesores3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbooTipoEspesores3.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposEspesores WHERE Estado = 1 AND Descripcion = '" + cbooTipoEspesores3.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();

                    string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                    if (ckHabilitarTextoEspesores3.Checked != false)
                    {
                        if (Magnitud != "" && cboDescripcionEspesores3.Text != "")
                        {
                            txtDescripcionEspesores3.Text = cbooTipoEspesores3.Text + " " + cboDescripcionEspesores3.Text + Magnitud;
                            espacio19 = " ";
                            espacio20 = " ";
                        }
                        else
                        {
                            txtDescripcionEspesores3.Text = cbooTipoEspesores3.Text + " " + cboDescripcionEspesores3.Text;
                            espacio19 = " ";
                            espacio20 = " ";
                        }
                    }
                    else
                    {
                        if (Magnitud != "" && cboDescripcionEspesores3.Text != "")
                        {
                            txtDescripcionEspesores3.Text = cboDescripcionEspesores3.Text + Magnitud;
                            espacio19 = " ";
                            espacio20 = " ";
                        }
                        else
                        {
                            txtDescripcionEspesores3.Text = cboDescripcionEspesores3.Text;
                            espacio19 = " ";
                            espacio20 = " ";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionEspesores3.Text = "";
                espacio19 = "";
                espacio20 = "";
                DefinicionNombreProductoXModelo();
            }

            //SI EL CAMPO SIGUIENTE ES NO APLICABLE LE QUITA LOS ESPACIOS
            if (cbooTipoEspesores4.Text == "NO APLICA")
            {
                txtDescripcionEspesores4.Text = "";
                espacio20 = "";
                espacio21 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO ESPESORES 4
        private void cbooTipoEspesores4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbooTipoEspesores4.SelectedValue != null)
            {
                idespesores = cbooTipoEspesores4.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionEspesores(cboDescripcionEspesores4, idespesores, idmodelo);
            }

            if (cbooTipoEspesores4.Text != "NO APLICA")
            {
                txtDescripcionEspesores4.Text = cbooTipoEspesores4.Text + " " + cboDescripcionEspesores4.Text;
                espacio20 = " ";
                espacio21 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionEspesores4.Text = "";
                espacio20 = "";
                espacio21 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION ESPESORES 4
        private void cboDescripcionEspesores4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbooTipoEspesores4.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposEspesores WHERE Estado = 1 AND Descripcion = '" + cbooTipoEspesores4.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();

                    string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                    if (ckHabilitarTextoEspesores4.Checked != false)
                    {
                        if (Magnitud != "" && cboDescripcionEspesores4.Text != "")
                        {
                            txtDescripcionEspesores4.Text = cbooTipoEspesores4.Text + " " + cboDescripcionEspesores4.Text + Magnitud;
                            espacio20 = " ";
                            espacio21 = " ";
                        }
                        else
                        {
                            txtDescripcionEspesores4.Text = cbooTipoEspesores4.Text + " " + cboDescripcionEspesores4.Text;
                            espacio20 = " ";
                            espacio21 = " ";
                        }
                    }
                    else
                    {
                        if (Magnitud != "" && cboDescripcionEspesores4.Text != "")
                        {
                            txtDescripcionEspesores4.Text = cboDescripcionEspesores4.Text + Magnitud;
                            espacio20 = " ";
                            espacio21 = " ";
                        }
                        else
                        {
                            txtDescripcionEspesores4.Text = cboDescripcionEspesores4.Text;
                            espacio20 = " ";
                            espacio21 = " ";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionEspesores4.Text = "";
                espacio20 = "";
                espacio21 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGAR TIPOS DE DISEÑO/ACABADO - DESCRIPCION DE DISEÑO/ACABADO - SELECCIONA DE VENTANA
        public void CargarTiposDiseñoAcabado(ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("AgregarProducto_CargarTiposDiseñoAcabado", con);
                comando.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdTipoDiseñoAcabado";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR DESCRIPCIÓN DE DISEÑO Y ACABADOS
        public void CargarDescripcionDiseñoAcabado(ComboBox cbo, string iddiseñoacabado, string idmodelo, string idTipoNN)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("AgregarProducto_CargarDescripcionDiseñoAcabado", con);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@idtipodiseñoacabado", iddiseñoacabado);
                comando.Parameters.AddWithValue("@idmodelo", idmodelo);
                comando.Parameters.AddWithValue("@idTipoNN", idTipoNN);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdDescripcionDiseñoAcabado";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR GRUPO DE CAMPOS Y CAMPOS SEGÚN LOS GRUPOS SELECCIOANDO 1
        private void ckCamposDiseñoAcabado1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckCamposDiseñoAcabado1.Checked == true)
            {
                flowLayoutPanel.Controls.Add(panelCamposDiseñoAcabado1);
                CargarTiposDiseñoAcabado(cboTiposDiseñosAcabados1);
                CargarTiposDiseñoAcabado(cboTiposDiseñosAcabados2);
            }
            else
            {
                flowLayoutPanel.Controls.Remove(panelCamposDiseñoAcabado1);
                espacio21 = "";
                espacio22 = "";
                espacio23 = "";
                txtDescripcionDiseñoAcabado1.Text = "";
                txtDescripcionDiseñoAcabado2.Text = "";
            }
        }

        //CARGAR GRUPO DE CAMPOS Y CAMPOS SEGÚN LOS GRUPOS SELECCIOANDO 2
        private void ckCamposDiseñoAcabado2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckCamposDiseñoAcabado2.Checked == true)
            {
                flowLayoutPanel.Controls.Add(panelCamposDiseñoAcabado2);
                CargarTiposDiseñoAcabado(cboTiposDiseñosAcabados3);
                CargarTiposDiseñoAcabado(cboTiposDiseñosAcabados4);
            }
            else
            {
                flowLayoutPanel.Controls.Remove(panelCamposDiseñoAcabado2);
                espacio23 = "";
                espacio24 = "";
                espacio25 = "";
                txtDescripcionDiseñoAcabado3.Text = "";
                txtDescripcionDiseñoAcabado4.Text = "";
            }
        }

        //CARGA DEL CAMPO TIPO DISEÑO Y ACABADO 1
        private void cboTiposDiseñosAcabados1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiseñosAcabados1.SelectedValue != null)
            {
                iddiseñoacabado = cboTiposDiseñosAcabados1.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionDiseñoAcabado(cboDescripcionDiseñoAcabado1, iddiseñoacabado, idmodelo, "0");
            }

            if (cboTiposDiseñosAcabados1.Text != "NO APLICA")
            {

                txtDescripcionDiseñoAcabado1.Text = cboTiposDiseñosAcabados1.Text + " " + cboDescripcionDiseñoAcabado1.Text;
                espacio21 = " ";
                espacio22 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionDiseñoAcabado1.Text = "";
                espacio21 = "";
                espacio22 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION DISEÑO Y ACABADOS 1
        private void cboDescripcionDiseñoAcabado1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDescripcionDiseñoAcabado1.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposDiseñoAcabado WHERE Estado = 1 AND Descripcion = '" + cboTiposDiseñosAcabados1.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                if (ckHabilitarTextoDiseñoAcabado1.Checked != false)
                {
                    if (Magnitud != "" && cboDescripcionDiseñoAcabado1.Text != "")
                    {
                        txtDescripcionDiseñoAcabado1.Text = cboTiposDiseñosAcabados1.Text + " " + cboDescripcionDiseñoAcabado1.Text + Magnitud;
                        espacio21 = " ";
                        espacio22 = " ";
                    }
                    else
                    {
                        txtDescripcionDiseñoAcabado1.Text = cboTiposDiseñosAcabados1.Text + " " + cboDescripcionDiseñoAcabado1.Text;
                        espacio21 = " ";
                        espacio22 = " ";
                    }
                }
                else
                {
                    if (Magnitud != "" && cboDescripcionDiseñoAcabado1.Text != "")
                    {
                        txtDescripcionDiseñoAcabado1.Text = cboDescripcionDiseñoAcabado1.Text + Magnitud;
                        espacio21 = " ";
                        espacio22 = " ";
                    }
                    else
                    {
                        txtDescripcionDiseñoAcabado1.Text = cboDescripcionDiseñoAcabado1.Text;
                        espacio21 = " ";
                        espacio22 = " ";
                    }
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionDiseñoAcabado1.Text = "";
                espacio21 = "";
                espacio22 = "";
                DefinicionNombreProductoXModelo();
            }

            //SI EL CAMPO SIGUIENTE ES NO APLICABLE LE QUITA LOS ESPACIOS
            if (cboTiposDiseñosAcabados2.Text == "NO APLICA")
            {
                txtDescripcionDiseñoAcabado2.Text = "";
                espacio22 = "";
                espacio23 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO DISEÑO Y ACABADO 2
        private void cboTiposDiseñosAcabados2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiseñosAcabados2.SelectedValue != null)
            {
                iddiseñoacabado = cboTiposDiseñosAcabados2.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionDiseñoAcabado(cboDescripcionDiseñoAcabado2, iddiseñoacabado, idmodelo, "0");
            }

            if (cboTiposDiseñosAcabados2.Text != "NO APLICA")
            {
                txtDescripcionDiseñoAcabado2.Text = cboTiposDiseñosAcabados2.Text + " " + cboDescripcionDiseñoAcabado2.Text;
                espacio22 = " ";
                espacio23 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionDiseñoAcabado2.Text = "";
                espacio22 = "";
                espacio23 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION DISEÑO Y ACABADOS 2
        private void cboDescripcionDiseñoAcabado2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDescripcionDiseñoAcabado2.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposDiseñoAcabado WHERE Estado = 1 AND Descripcion = '" + cboTiposDiseñosAcabados2.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                if (ckHabilitarTextoDiseñoAcabado2.Checked != false)
                {
                    if (Magnitud != "" && cboDescripcionDiseñoAcabado2.Text != "")
                    {
                        txtDescripcionDiseñoAcabado2.Text = cboTiposDiseñosAcabados2.Text + " " + cboDescripcionDiseñoAcabado2.Text + Magnitud;
                        espacio22 = " ";
                        espacio23 = " ";
                    }
                    else
                    {
                        txtDescripcionDiseñoAcabado2.Text = cboTiposDiseñosAcabados2.Text + " " + cboDescripcionDiseñoAcabado2.Text;
                        espacio22 = " ";
                        espacio23 = " ";
                    }
                }
                else
                {
                    if (Magnitud != "" && cboDescripcionDiseñoAcabado2.Text != "")
                    {
                        txtDescripcionDiseñoAcabado2.Text = cboDescripcionDiseñoAcabado2.Text + Magnitud;
                        espacio22 = " ";
                        espacio23 = " ";
                    }
                    else
                    {
                        txtDescripcionDiseñoAcabado2.Text = cboDescripcionDiseñoAcabado2.Text;
                        espacio22 = " ";
                        espacio23 = " ";
                    }
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionDiseñoAcabado2.Text = "";
                espacio22 = "";
                espacio23 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO DISEÑO Y ACABADO 3
        private void cboTiposDiseñosAcabados3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiseñosAcabados3.SelectedValue != null)
            {
                iddiseñoacabado = cboTiposDiseñosAcabados3.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionDiseñoAcabado(cboDescripcionDiseñoAcabado3, iddiseñoacabado, idmodelo, "0");
            }

            if (cboTiposDiseñosAcabados3.Text != "NO APLICA")
            {
                txtDescripcionDiseñoAcabado3.Text = cboTiposDiseñosAcabados3.Text + " " + cboDescripcionDiseñoAcabado3.Text;
                espacio23 = " ";
                espacio24 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionDiseñoAcabado3.Text = "";
                espacio23 = "";
                espacio24 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION DISEÑO Y ACABADOS 3
        private void cboDescripcionDiseñoAcabado3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDescripcionDiseñoAcabado3.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposDiseñoAcabado WHERE Estado = 1 AND Descripcion = '" + cboTiposDiseñosAcabados3.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                if (ckHabilitarTextoDiseñoAcabado3.Checked != false)
                {
                    if (Magnitud != "" && cboDescripcionDiseñoAcabado3.Text != "")
                    {
                        txtDescripcionDiseñoAcabado3.Text = cboTiposDiseñosAcabados3.Text + " " + cboDescripcionDiseñoAcabado3.Text + Magnitud;
                        espacio23 = " ";
                        espacio24 = " ";
                    }
                    else
                    {
                        txtDescripcionDiseñoAcabado3.Text = cboTiposDiseñosAcabados3.Text + " " + cboDescripcionDiseñoAcabado3.Text;
                        espacio23 = " ";
                        espacio24 = " ";
                    }
                }
                else
                {
                    if (Magnitud != "" && cboDescripcionDiseñoAcabado3.Text != "")
                    {
                        txtDescripcionDiseñoAcabado3.Text = cboDescripcionDiseñoAcabado3.Text + Magnitud;
                        espacio23 = " ";
                        espacio24 = " ";
                    }
                    else
                    {
                        txtDescripcionDiseñoAcabado3.Text = cboDescripcionDiseñoAcabado3.Text;
                        espacio23 = " ";
                        espacio24 = " ";
                    }
                }

                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionDiseñoAcabado3.Text = "";
                espacio23 = "";
                espacio24 = "";
                DefinicionNombreProductoXModelo();
            }

            //SI EL CAMPO SIGUIENTE ES NO APLICABLE LE QUITA LOS ESPACIOS
            if (cboTiposDiseñosAcabados4.Text == "NO APLICA")
            {
                txtDescripcionDiseñoAcabado4.Text = "";
                espacio24 = "";
                espacio25 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO DISEÑO Y ACABADO 4
        private void cboTiposDiseñosAcabados4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiseñosAcabados4.SelectedValue != null)
            {
                iddiseñoacabado = cboTiposDiseñosAcabados4.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionDiseñoAcabado(cboDescripcionDiseñoAcabado4, iddiseñoacabado, idmodelo, "0");
            }

            if (cboTiposDiseñosAcabados4.Text != "NO APLICA")
            {
                txtDescripcionDiseñoAcabado4.Text = cboTiposDiseñosAcabados4.Text + " " + cboDescripcionDiseñoAcabado4.Text;
                espacio24 = " ";
                espacio25 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionDiseñoAcabado4.Text = "";
                espacio24 = "";
                espacio25 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION DISEÑO Y ACABADOS 4
        private void cboDescripcionDiseñoAcabado4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDescripcionDiseñoAcabado4.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposDiseñoAcabado WHERE Estado = 1 AND Descripcion = '" + cboTiposDiseñosAcabados4.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                if (ckHabilitarTextoDiseñoAcabado4.Checked != false)
                {
                    if (Magnitud != "" && cboDescripcionDiseñoAcabado4.Text != "")
                    {
                        txtDescripcionDiseñoAcabado4.Text = cboTiposDiseñosAcabados4.Text + " " + cboDescripcionDiseñoAcabado4.Text + Magnitud;
                        espacio24 = " ";
                        espacio25 = " ";
                    }
                    else
                    {
                        txtDescripcionDiseñoAcabado4.Text = cboTiposDiseñosAcabados4.Text + " " + cboDescripcionDiseñoAcabado4.Text;
                        espacio24 = " ";
                        espacio25 = " ";
                    }
                }
                else
                {
                    if (Magnitud != "" && cboDescripcionDiseñoAcabado4.Text != "")
                    {
                        txtDescripcionDiseñoAcabado4.Text = cboDescripcionDiseñoAcabado4.Text + Magnitud;
                        espacio24 = " ";
                        espacio25 = " ";
                    }
                    else
                    {
                        txtDescripcionDiseñoAcabado4.Text = cboDescripcionDiseñoAcabado4.Text + Magnitud;
                        espacio24 = " ";
                        espacio25 = " ";
                    }
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionDiseñoAcabado4.Text = "";
                espacio24 = "";
                espacio25 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGAR TIPOS DE N/TIPOS - DESCRIPCION DE N/TIPOS - SELECCIONA DE VENTANA
        public void CargarTiposNTipos(ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("AgregarProducto_CargarTiposNTipos", con);
                comando.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdTipoNTipos";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR DESCRIPCIÓN DE N TIPOS
        public void CargarDescripcionNTipos(ComboBox cbo, string idntipos, string idmodelo, string idTipoNN)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("AgregarProducto_CargarDescripcionNTipos", con);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@idtiposntipos", idntipos);
                comando.Parameters.AddWithValue("@idmodelo", idmodelo);
                comando.Parameters.AddWithValue("@idTipoNN", idTipoNN);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdDescripcionNTipos";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR GRUPO DE CAMPOS Y CAMPOS SEGÚN LOS GRUPOS SELECCIOANDO 1
        private void ckCamposNTipos1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckCamposNTipos1.Checked == true)
            {
                flowLayoutPanel.Controls.Add(panelCamposNTipos1);
                CargarTiposNTipos(cboTiposNTipos1);
                CargarTiposNTipos(cboTiposNTipos2);
            }
            else
            {
                flowLayoutPanel.Controls.Remove(panelCamposNTipos1);
                espacio25 = "";
                espacio26 = "";
                espacio27 = "";
                txtDescripcionNTipos1.Text = "";
                txtDescripcionNTipos1.Text = "";
            }
        }

        //CARGAR GRUPO DE CAMPOS Y CAMPOS SEGÚN LOS GRUPOS SELECCIOANDO 2
        private void ckCamposNTipos2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckCamposNTipos2.Checked == true)
            {
                flowLayoutPanel.Controls.Add(panelCamposNTipos2);
                CargarTiposNTipos(cboTiposNTipos3);
                CargarTiposNTipos(cboTiposNTipos4);
            }
            else
            {
                flowLayoutPanel.Controls.Remove(panelCamposNTipos2);
                espacio27 = "";
                espacio28 = "";
                espacio29 = "";
                txtDescripcionNTipos3.Text = "";
                txtDescripcionNTipos4.Text = "";
            }
        }

        //CARGA DEL CAMPO TIPO N TIPOS 1
        private void cboTiposNTipos1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposNTipos1.SelectedValue != null)
            {
                idntipos = cboTiposNTipos1.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionNTipos(cboDescripcionNTipos1, idntipos, idmodelo, "0");
            }

            if (cboTiposNTipos1.Text != "NO APLICA")
            {
                txtDescripcionNTipos1.Text = cboTiposNTipos1.Text + " " + cboDescripcionNTipos1.Text;
                espacio25 = " ";
                espacio26 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionNTipos1.Text = "";
                espacio25 = "";
                espacio26 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION N TIPOS 1
        private void cboDescripcionNTipos1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDescripcionNTipos1.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposNTipos WHERE Estado = 1 AND Descripcion = '" + cboTiposNTipos1.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                if (ckHabilitarTextoNTipos1.Checked != false)
                {
                    if (Magnitud != "" && cboDescripcionNTipos1.Text != "")
                    {
                        txtDescripcionNTipos1.Text = cboTiposNTipos1.Text + " " + cboDescripcionNTipos1.Text + Magnitud;
                        espacio25 = " ";
                        espacio26 = " ";
                    }
                    else
                    {
                        txtDescripcionNTipos1.Text = cboTiposNTipos1.Text + " " + cboDescripcionNTipos1.Text;
                        espacio25 = " ";
                        espacio26 = " ";
                    }
                }
                else
                {
                    if (Magnitud != "" && cboDescripcionNTipos1.Text != "")
                    {
                        txtDescripcionNTipos1.Text = cboDescripcionNTipos1.Text + Magnitud;
                        espacio25 = " ";
                        espacio26 = " ";
                    }
                    else
                    {
                        txtDescripcionNTipos1.Text = cboDescripcionNTipos1.Text;
                        espacio25 = " ";
                        espacio26 = " ";
                    }
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionNTipos1.Text = "";
                espacio25 = "";
                espacio26 = "";
                DefinicionNombreProductoXModelo();
            }

            //SI EL CAMPO SIGUIENTE ES NO APLICABLE LE QUITA LOS ESPACIOS
            if (cboTiposNTipos2.Text == "NO APLICA")
            {
                txtDescripcionNTipos2.Text = "";
                espacio26 = "";
                espacio27 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO N TIPOS 2
        private void cboTiposNTipos2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposNTipos2.SelectedValue != null)
            {
                idntipos = cboTiposNTipos2.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionNTipos(cboDescripcionNTipos2, idntipos, idmodelo, "0");
            }

            if (cboTiposNTipos2.Text != "NO APLICA")
            {
                txtDescripcionNTipos2.Text = cboTiposNTipos2.Text + " " + cboDescripcionNTipos2.Text;
                espacio26 = " ";
                espacio27 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionNTipos2.Text = "";
                espacio26 = "";
                espacio27 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION N TIPOS 2
        private void cboDescripcionNTipos2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDescripcionNTipos2.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposNTipos WHERE Estado = 1 AND Descripcion = '" + cboTiposNTipos2.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                if (ckHabilitarTextoNTipos2.Checked != false)
                {
                    if (Magnitud != "" && cboDescripcionNTipos2.Text != "")
                    {
                        txtDescripcionNTipos2.Text = cboTiposNTipos2.Text + " " + cboDescripcionNTipos2.Text + Magnitud;
                        espacio26 = " ";
                        espacio27 = " ";
                    }
                    else
                    {
                        txtDescripcionNTipos2.Text = cboTiposNTipos2.Text + " " + cboDescripcionNTipos2.Text;
                        espacio26 = " ";
                        espacio27 = " ";
                    }
                }
                else
                {
                    if (Magnitud != "" && cboDescripcionNTipos2.Text != "")
                    {
                        txtDescripcionNTipos2.Text = cboDescripcionNTipos2.Text + Magnitud;
                        espacio26 = " ";
                        espacio27 = " ";
                    }
                    else
                    {
                        txtDescripcionNTipos2.Text = cboDescripcionNTipos2.Text;
                        espacio26 = " ";
                        espacio27 = " ";
                    }
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionNTipos2.Text = "";
                espacio26 = "";
                espacio27 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO N TIPOS 3
        private void cboTiposNTipos3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposNTipos3.SelectedValue != null)
            {
                idntipos = cboTiposNTipos3.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionNTipos(cboDescripcionNTipos3, idntipos, idmodelo, "0");
            }

            if (cboTiposNTipos3.Text != "NO APLICA")
            {
                txtDescripcionNTipos3.Text = cboTiposNTipos3.Text + " " + cboDescripcionNTipos3.Text;
                espacio27 = " ";
                espacio28 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionNTipos3.Text = "";
                espacio27 = "";
                espacio28 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION N TIPOS 3
        private void cboDescripcionNTipos3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtDescripcionNTipos3.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposNTipos WHERE Estado = 1 AND Descripcion = '" + cboTiposNTipos3.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                if (ckHabilitarTextoNTipos3.Checked != false)
                {
                    if (Magnitud != "" && cboDescripcionNTipos3.Text != "")
                    {
                        txtDescripcionNTipos3.Text = cboTiposNTipos3.Text + " " + cboDescripcionNTipos3.Text + Magnitud;
                        espacio27 = " ";
                        espacio28 = " ";
                    }
                    else
                    {
                        txtDescripcionNTipos3.Text = cboTiposNTipos3.Text + " " + cboDescripcionNTipos3.Text;
                        espacio27 = " ";
                        espacio28 = " ";
                    }
                }
                else
                {
                    if (Magnitud != "" && cboDescripcionNTipos3.Text != "")
                    {
                        txtDescripcionNTipos3.Text = cboDescripcionNTipos3.Text + Magnitud;
                        espacio27 = " ";
                        espacio28 = " ";
                    }
                    else
                    {
                        txtDescripcionNTipos3.Text = cboDescripcionNTipos3.Text;
                        espacio27 = " ";
                        espacio28 = " ";
                    }
                }

                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionNTipos3.Text = "";
                espacio27 = "";
                espacio28 = "";
                DefinicionNombreProductoXModelo();
            }

            //SI EL CAMPO SIGUIENTE ES NO APLICABLE LE QUITA LOS ESPACIOS
            if (cboTiposNTipos4.Text == "NO APLICA")
            {
                txtDescripcionNTipos4.Text = "";
                espacio28 = "";
                espacio29 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO N TIPOS 4
        private void cboTiposNTipos4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposNTipos4.SelectedValue != null)
            {
                idntipos = cboTiposNTipos4.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionNTipos(cboDescripcionNTipos4, idntipos, idmodelo, "0");
            }

            if (cboTiposNTipos4.Text != "NO APLICA")
            {
                txtDescripcionNTipos4.Text = cboTiposNTipos4.Text + " " + cboDescripcionNTipos4.Text;
                espacio28 = " ";
                espacio29 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionNTipos4.Text = "";
                espacio28 = "";
                espacio29 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION N TIPOS 4
        private void cboDescripcionNTipos4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDescripcionNTipos4.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposNTipos WHERE Estado = 1 AND Descripcion = '" + cboTiposNTipos4.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                if (ckHabilitarTextoNTipos4.Checked != false)
                {
                    if (Magnitud != "" && cboDescripcionNTipos4.Text != "")
                    {
                        txtDescripcionNTipos4.Text = cboTiposNTipos4.Text + " " + cboDescripcionNTipos4.Text + Magnitud;
                        espacio28 = " ";
                        espacio29 = " ";
                    }
                    else
                    {
                        txtDescripcionNTipos4.Text = cboTiposNTipos4.Text + " " + cboDescripcionNTipos4.Text;
                        espacio28 = " ";
                        espacio29 = " ";
                    }
                }
                else
                {
                    if (Magnitud != "" && cboDescripcionNTipos4.Text != "")
                    {
                        txtDescripcionNTipos4.Text = cboDescripcionNTipos4.Text + Magnitud;
                        espacio28 = " ";
                        espacio29 = " ";
                    }
                    else
                    {
                        txtDescripcionNTipos4.Text = cboDescripcionNTipos4.Text;
                        espacio28 = " ";
                        espacio29 = " ";
                    }
                }
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionNTipos4.Text = "";
                espacio28 = "";
                espacio29 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGAR TIPOS DE VARIOS0 - DESCRIPCION DE VARIOS0S - SELECCIONA DE VENTANA
        public void CargarTiposVariosO(ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("AgregarProducto_CargarTiposVariosO", con);
                comando.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdTipoVariosO";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR DESCRIPCIÓN DE VARIOS 0
        public void CargarDescripcionVariosO(ComboBox cbo, string idvarioso, string idmodelo, string idTipoNN)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("AgregarProducto_CargarDescripcionVariosO", con);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@idvarioso", idvarioso);
                comando.Parameters.AddWithValue("@idmodelo", idmodelo);
                comando.Parameters.AddWithValue("@idTipoNN", idTipoNN);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdDescripcionVarios0";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR GRUPO DE CAMPOS Y CAMPOS SEGÚN LOS GRUPOS SELECCIOANDO 1
        private void ckVariosO1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckVariosO1.Checked == true)
            {
                flowLayoutPanel.Controls.Add(panelCamposVariosO1);
                CargarTiposVariosO(cboTiposVariosO1);
            }
            else
            {
                flowLayoutPanel.Controls.Remove(panelCamposVariosO1);
                espacio29 = "";
                espacio30 = "";
                txtDescripcionVariosO1.Text = "";
            }
        }

        //CARGAR GRUPO DE CAMPOS Y CAMPOS SEGÚN LOS GRUPOS SELECCIOANDO 2
        private void ckVariosO2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckVariosO2.Checked == true)
            {
                flowLayoutPanel.Controls.Add(panelCamposVariosO2);
                CargarTiposVariosO(cboTiposVariosO2);
            }
            else
            {
                flowLayoutPanel.Controls.Remove(panelCamposVariosO2);
                espacio30 = "";
                txtDescripcionVariosO2.Text = "";
            }
        }

        //CARGA DEL CAMPO TIPO VARIOS 0 1
        private void cboTiposVariosO1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposVariosO1.SelectedValue != null)
            {
                idvarioso = cboTiposVariosO1.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionVariosO(cboDescripcionVariosO1, idvarioso, idmodelo, "0");
            }

            if (cboTiposVariosO1.Text != "NO APLICA")
            {
                txtDescripcionVariosO1.Text = cboTiposVariosO1.Text + " " + cboDescripcionVariosO1.Text;
                espacio29 = " ";
                espacio30 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionVariosO1.Text = "";
                espacio29 = "";
                espacio30 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION VARIOS0 1
        private void cboDescripcionVariosO1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDescripcionVariosO1.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposVariosO WHERE Estado = 1 AND Descripcion = '" + cboTiposVariosO1.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                if (ckHabilitarTextoVarios01.Checked != false)
                {
                    if (Magnitud != "" && cboDescripcionVariosO1.Text != "")
                    {
                        txtDescripcionVariosO1.Text = cboTiposVariosO1.Text + " " + cboDescripcionVariosO1.Text + Magnitud;
                        espacio29 = " ";
                        espacio30 = " ";
                    }
                    else
                    {
                        txtDescripcionVariosO1.Text = cboTiposVariosO1.Text + " " + cboDescripcionVariosO1.Text;
                        espacio29 = " ";
                        espacio30 = " ";
                    }
                }
                else
                {
                    if (Magnitud != "" && cboDescripcionVariosO1.Text != "")
                    {
                        txtDescripcionVariosO1.Text = cboDescripcionVariosO1.Text + Magnitud;
                        espacio29 = " ";
                        espacio30 = " ";
                    }
                    else
                    {
                        txtDescripcionVariosO1.Text = cboDescripcionVariosO1.Text;
                        espacio29 = " ";
                        espacio30 = " ";
                    }
                }

                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionVariosO1.Text = "";
                espacio29 = "";
                espacio30 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO TIPO VARIOS 0 2
        private void cboTiposVariosO2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposVariosO2.SelectedValue != null)
            {
                idvarioso = cboTiposVariosO2.SelectedValue.ToString();
                idmodelo = cboModelos.SelectedValue.ToString();
                CargarDescripcionVariosO(cboDescripcionVariosO2, idvarioso, idmodelo, "0");
            }

            if (cboTiposVariosO2.Text != "NO APLICA")
            {
                txtDescripcionVariosO2.Text = cboTiposVariosO2.Text + " " + cboDescripcionVariosO2.Text;
                espacio30 = " ";
                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionVariosO2.Text = "";
                espacio30 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGA DEL CAMPO DESCRIPCION VARIOS0 2
        private void cboDescripcionVariosO2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDescripcionVariosO2.Text != "NO APLICA")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    da = new SqlDataAdapter("SELECT Descripcion, Magnitud FROM TiposVariosO WHERE Estado = 1 AND Descripcion = '" + cboTiposVariosO2.Text + "'", con);
                    da.Fill(dt);
                    datalistadoTipoDato.DataSource = dt;
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                string Magnitud = datalistadoTipoDato.SelectedCells[1].Value.ToString();

                if (ckHabilitarTextoVarios02.Checked != false)
                {
                    if (Magnitud != "" && cboDescripcionVariosO2.Text != "")
                    {
                        txtDescripcionVariosO2.Text = cboTiposVariosO2.Text + " " + cboDescripcionVariosO2.Text + Magnitud;
                        espacio30 = " ";
                    }
                    else
                    {
                        txtDescripcionVariosO2.Text = cboTiposVariosO2.Text + " " + cboDescripcionVariosO2.Text;
                        espacio30 = " ";
                    }
                }
                else
                {
                    if (Magnitud != "" && cboDescripcionVariosO2.Text != "")
                    {
                        txtDescripcionVariosO2.Text = cboDescripcionVariosO2.Text + Magnitud;
                        espacio30 = " ";
                    }
                    else
                    {
                        txtDescripcionVariosO2.Text = cboDescripcionVariosO2.Text;
                        espacio30 = " ";
                    }
                }

                DefinicionNombreProductoXModelo();
            }
            else
            {
                txtDescripcionVariosO2.Text = "";
                espacio30 = "";
                DefinicionNombreProductoXModelo();
            }
        }

        //CARGAR GRUPO DE CAMPO GENERAL
        private void ckGenerales_CheckedChanged(object sender, EventArgs e)
        {
            if (ckGenerales.Checked == true)
            {
                flowLayoutPanel.Controls.Add(panelCamposGeneral);
            }
            else
            {
                flowLayoutPanel.Controls.Remove(panelCamposGeneral);
            }
        }

        //CARGA EL CAMPO DESCRIPCION GENERAL
        private void txtDescripcionGeneral_TextChanged(object sender, EventArgs e)
        {
            txtDescripcionGeneradaProducto.Text = txtDescripcionGeneral.Text;
        }

        //CARGAR EL CAMPO DESCIPCION ANOTACIONES
        private void txtAnotaciones_TextChanged(object sender, EventArgs e)
        {
            DefinicionNombreProductoXModelo();
            //txtDescripcionGeneradaProducto.Text = nombreInicial + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 + txtDescripcionCaracteristicas3.Text + espacio4 + txtDescripcionCaracteristicas4.Text + espacio5 + txtDescripcionMedida1.Text + espacio6 + txtDescripcionMedida2.Text + espacio7 + txtDescripcionMedida3.Text + espacio8 + txtDescripcionMedida4.Text + espacio9 + txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11 + txtDescripcionDiametros3.Text + espacio12 + txtDescripcionDiametros4.Text + espacio13 + txtDescripcionFormas1.Text + espacio14 + txtDescripcionFormas2.Text + espacio15 + txtDescripcionFormas3.Text + espacio16 + txtDescripcionFormas4.Text + espacio17 + txtDescripcionEspesores1.Text + espacio18 + txtDescripcionEspesores2.Text + espacio19 + txtDescripcionEspesores3.Text + espacio20 + txtDescripcionEspesores4.Text + espacio21 + txtDescripcionDiseñoAcabado1.Text + espacio22 + txtDescripcionDiseñoAcabado2.Text + espacio23 + txtDescripcionDiseñoAcabado3.Text + espacio24 + txtDescripcionDiseñoAcabado4.Text + espacio25 + txtDescripcionNTipos1.Text + espacio26 + txtDescripcionNTipos2.Text + espacio27 + txtDescripcionNTipos3.Text + espacio28 + txtDescripcionNTipos4.Text + espacio29 + txtDescripcionVariosO1.Text + espacio30 + txtDescripcionVariosO2.Text + espacio31;
        }

        //PARTE GENERAL DEL PLANO-----------------------------------------------------------------
        //CARGA DE PLANO
        public void CargarPlanos(TextBox txt, Button btn)
        {
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Todos los archivos (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txt.Text = openFileDialog1.FileName;
                GeneracionReferenciaPlano(lblModelo, lblCodigoReferenciaPlano);

                btn.Visible = true;
            }
        }

        //BOTON PARA CARGAR MI PLANO, LLAMA A MI FUNCION DE CARGA
        private void btnCargarPdf_Click(object sender, EventArgs e)
        {
            CargarPlanos(txtFile, btnCancelarPlano);
        }

        //CANCELAR PLANO
        private void btnCancelarPlano_Click(object sender, EventArgs e)
        {
            btnCancelarPlano.Visible = false;
            txtFile.Text = "";
            lblCodigoReferenciaPlano.Text = "***";
        }

        //PARTE GENERAL DEL PRODUCTO-------------------------------------------------------------------
        //ACCIONES DE LOS BOTONES PRINCIPALES - CANCELAR - SALIR
        //GUARDAR PRODUCTO
        //FUNCION PARA UGUARDAR PRODUCTO CON PLÑANO
        public void AgregarPlano(string codigo, string idmedida, int idtipomercaderia, int idmodelo, int idlinea, int iddiferencial, string descripciongeneradaproducto, string anotaciones, CheckBox ck
            , string codigoreferenciaplno, string codigoplano, string file, DataGridView DGV)
        {
            //SI NO HAY PLANO AGREGADO
            if (file == "")
            {
                //GUARDAR PRODUCTOS - DATOS PRINCIPALES---------------------------------------------------
                SqlConnection conp = new SqlConnection();
                conp.ConnectionString = Conexion.ConexionMaestra.conexion;
                conp.Open();
                SqlCommand cmdp = new SqlCommand();
                cmdp = new SqlCommand("AgregarProducto_InsertarCamposPrincipales", conp);
                cmdp.CommandType = CommandType.StoredProcedure;

                cmdp.Parameters.AddWithValue("@codom", codigo);
                cmdp.Parameters.AddWithValue("@idmedida", idmedida);
                cmdp.Parameters.AddWithValue("@idtipomercaderia", idtipomercaderia);
                cmdp.Parameters.AddWithValue("@idmodelo", idmodelo);
                cmdp.Parameters.AddWithValue("@idlinea", idlinea);
                cmdp.Parameters.AddWithValue("@iddiferencial", iddiferencial);
                cmdp.Parameters.AddWithValue("@detalle", descripciongeneradaproducto);
                cmdp.Parameters.AddWithValue("@descripcion", anotaciones);

                if (ck.Checked == true) { semirpoducido = 1; } else { semirpoducido = 0; }
                cmdp.Parameters.AddWithValue("@semiproducido", semirpoducido);
                cmdp.Parameters.AddWithValue("@codigogenerado", "");
                cmdp.Parameters.AddWithValue("@rutaImagen", "");

                cmdp.ExecuteNonQuery();
                conp.Close();
            }
            //SI HAN AGREGADO PLANO
            else
            {
                string NombreGenerado = codigo + " - " + descripciongeneradaproducto + " - " + codigoreferenciaplno;

                string RutaOld = file;

                string RutaNew = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Procesos\Productos\Planos\" + NombreGenerado + ".pdf";

                File.Copy(RutaOld, RutaNew);

                //GUARDAR PRODUCTOS - DATOS PRINCIPALES---------------------------------------------------
                SqlConnection conp = new SqlConnection();
                conp.ConnectionString = Conexion.ConexionMaestra.conexion;
                conp.Open();
                SqlCommand cmdp = new SqlCommand();
                cmdp = new SqlCommand("AgregarProducto_InsertarCamposPrincipales_Plano", conp);
                cmdp.CommandType = CommandType.StoredProcedure;

                cmdp.Parameters.AddWithValue("@codom", codigo);
                cmdp.Parameters.AddWithValue("@idmedida", idmedida);
                cmdp.Parameters.AddWithValue("@idtipomercaderia", idtipomercaderia);
                cmdp.Parameters.AddWithValue("@idmodelo", idmodelo);
                cmdp.Parameters.AddWithValue("@idlinea", idlinea);
                cmdp.Parameters.AddWithValue("@iddiferencial", iddiferencial);
                cmdp.Parameters.AddWithValue("@detalle", descripciongeneradaproducto);
                cmdp.Parameters.AddWithValue("@descripcion", anotaciones);

                if (ck.Checked == true) { semirpoducido = 1; } else { semirpoducido = 0; }
                cmdp.Parameters.AddWithValue("@semiproducido", semirpoducido);
                cmdp.Parameters.AddWithValue("@codigogenerado", "");
                cmdp.Parameters.AddWithValue("@rutaImagen", "");

                //PARAMETROS DEL INGRESO DE PLANO-------------------------------
                cmdp.Parameters.AddWithValue("@doc", SqlDbType.VarBinary).Value = System.Data.SqlTypes.SqlBinary.Null;
                cmdp.Parameters.AddWithValue("@namereferences", codigoreferenciaplno);
                cmdp.Parameters.AddWithValue("@name", RutaNew);
                cmdp.Parameters.AddWithValue("@realname", NombreGenerado + ".pdf");

                codigoProducto();
                int codigoproduc = Convert.ToInt32(DGV.SelectedCells[0].Value.ToString());
                cmdp.Parameters.AddWithValue("@idart", codigoproduc + 1);
                codigoPlano();
                codigoplano = DGV.SelectedCells[0].Value.ToString();
                cmdp.Parameters.AddWithValue("@idplano", Convert.ToInt32(codigoplano) + 1);

                cmdp.ExecuteNonQuery();
                conp.Close();
            }
        }

        //FUNCION PARA GUARDAR MIS DATOS ANEXOS AL PRODUCTO
        public void Agregar_DatosAnexos(decimal peso, string ubicacion, decimal minimo, decimal maximo, int idorigen, int idterminoscompra, string contenedor, decimal pesocontenedor
            , string medidas, int idtipoexistencia, string codigounspscs, double porcentajepercepcion, int idbienessujetopercepcion, double porcentajedeatraccion, double porcentajeisc)
        {
            //GUARDAR PRODUCTOS - DATOS ANEXOS---------------------------------------------------------
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("AgregarProducto_InsertarDatosAnexos", con);
            cmd.CommandType = CommandType.StoredProcedure;

            int codigoproducto = Convert.ToInt32(dataListadoCdigoProducto.SelectedCells[0].Value.ToString());
            cmd.Parameters.AddWithValue("@IdArt", codigoproducto);
            cmd.Parameters.AddWithValue("@afectoIGV", afectadoIGV);
            cmd.Parameters.AddWithValue("@controlarStock", controlarstock);
            cmd.Parameters.AddWithValue("@juego", juego);
            cmd.Parameters.AddWithValue("@servicio", servicio);
            cmd.Parameters.AddWithValue("@controlarLotes", controlarlotes);
            cmd.Parameters.AddWithValue("@controlarserie", controlarserie);

            cmd.Parameters.AddWithValue("@peso", peso);
            cmd.Parameters.AddWithValue("@ubicacion", ubicacion);
            cmd.Parameters.AddWithValue("@reposicion", reposicion);
            cmd.Parameters.AddWithValue("@minimo", minimo);
            cmd.Parameters.AddWithValue("@maximo", maximo);

            cmd.Parameters.AddWithValue("@idorigen", idorigen);
            cmd.Parameters.AddWithValue("@idterminoscompra", idterminoscompra);
            cmd.Parameters.AddWithValue("@contenedor", contenedor);
            cmd.Parameters.AddWithValue("@pesocontenedor", pesocontenedor);
            cmd.Parameters.AddWithValue("@medidas", medidas);

            cmd.Parameters.AddWithValue("@idtipoexistencia", idtipoexistencia);
            cmd.Parameters.AddWithValue("@codigounsocs", codigounspscs);
            cmd.Parameters.AddWithValue("@sujetopercepcion", sujetropercepcion);
            cmd.Parameters.AddWithValue("@porcentajepercepcion", porcentajepercepcion);
            cmd.Parameters.AddWithValue("@idbienessujeropercepcion", idbienessujetopercepcion);
            cmd.Parameters.AddWithValue("@sujetodetraccion", sujetodetraccion);
            cmd.Parameters.AddWithValue("@porcentajedetraccion", porcentajedeatraccion);
            cmd.Parameters.AddWithValue("@sujetoISC", sujetoisc);
            cmd.Parameters.AddWithValue("@porcentajeISC", porcentajeisc);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        //FUNCION PARA GUARDAR LOS GRUPOS DE CAMPOS SELECCIONADOS
        public void AgregarGruposCamposSeleccionados(DataGridView DGV, CheckBox CkCarac1, CheckBox CkCarac2, CheckBox CkMedi1, CheckBox CkMedi2, CheckBox CkDia1, CheckBox CkDia2
            , CheckBox CkForm1, CheckBox CkForm2, CheckBox CkEspe1, CheckBox CkEspe2, CheckBox CkDiseAca1, CheckBox CkDiseAca2, CheckBox CkNtip1, CheckBox CkNtip2
            , CheckBox CkVari1, CheckBox CkVari2, CheckBox CkGener)
        {
            //GUARDAR PRODUCTOS - CAMPOS SELECCIONADOS-------------------------------------------------------
            int codigoproducto = Convert.ToInt32(DGV.SelectedCells[0].Value.ToString());

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand("AgregarProducto_InsertarGrupoCamposSeleccionados", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idart", codigoproducto);

            if (CkCarac1.Checked == true)
            {
                campocaracteristicas1 = 1;
            }
            else
            {
                campocaracteristicas1 = 0;
            }
            if (CkCarac2.Checked == true)
            {
                campocaracteristicas2 = 1;
            }
            else
            {
                campocaracteristicas2 = 0;
            }
            cmd.Parameters.AddWithValue("@CampCaracteristicas1", campocaracteristicas1);
            cmd.Parameters.AddWithValue("@CampCaracteristicas2", campocaracteristicas2);

            if (CkMedi1.Checked == true)
            {
                campomedidas1 = 1;
            }
            else
            {
                campomedidas1 = 0;
            }
            if (CkMedi2.Checked == true)
            {
                campomedidas2 = 1;
            }
            else
            {
                campomedidas2 = 0;
            }
            cmd.Parameters.AddWithValue("@CampMedidas1", campomedidas1);
            cmd.Parameters.AddWithValue("@CampMedidas2", campomedidas2);

            if (CkDia1.Checked == true)
            {
                campodiametros1 = 1;
            }
            else
            {
                campodiametros1 = 0;
            }
            if (CkDia2.Checked == true)
            {
                campodiametros2 = 1;
            }
            else
            {
                campodiametros2 = 0;
            }
            cmd.Parameters.AddWithValue("@CampDiametros1", campodiametros1);
            cmd.Parameters.AddWithValue("@CampDiametros2", campodiametros2);

            if (CkForm1.Checked == true)
            {
                campoformas1 = 1;
            }
            else
            {
                campoformas1 = 0;
            }
            if (CkForm2.Checked == true)
            {
                campoformas2 = 1;
            }
            else
            {
                campoformas2 = 0;
            }
            cmd.Parameters.AddWithValue("@CampFormas1", campoformas1);
            cmd.Parameters.AddWithValue("@CampFormas2", campoformas2);

            if (CkEspe1.Checked == true)
            {
                campoespesor1 = 1;
            }
            else
            {
                campoespesor1 = 0;
            }
            if (CkEspe2.Checked == true)
            {
                campoespesor2 = 1;
            }
            else
            {
                campoespesor2 = 0;
            }
            cmd.Parameters.AddWithValue("@CampEspesores1", campoespesor1);
            cmd.Parameters.AddWithValue("@CampEspesores2", campoespesor2);

            if (CkDiseAca1.Checked == true)
            {
                campodiseñoacabado1 = 1;
            }
            else
            {
                campodiseñoacabado1 = 0;
            }
            if (CkDiseAca2.Checked == true)
            {
                campodiseñoacabado2 = 1;
            }
            else
            {
                campodiseñoacabado2 = 0;
            }
            cmd.Parameters.AddWithValue("@CampDiseñoAcabado1", campodiseñoacabado1);
            cmd.Parameters.AddWithValue("@CampDiseñoAcabado2", campodiseñoacabado2);

            if (CkNtip1.Checked == true)
            {
                campontipos1 = 1;
            }
            else
            {
                campontipos1 = 0;
            }
            if (CkNtip2.Checked == true)
            {
                campontipos2 = 1;
            }
            else
            {
                campontipos2 = 0;
            }
            cmd.Parameters.AddWithValue("@CampNTipos1", campontipos1);
            cmd.Parameters.AddWithValue("@CampNTipos2", campontipos2);

            if (CkVari1.Checked == true)
            {
                campovarioso1 = 1;
            }
            else
            {
                campovarioso1 = 0;
            }
            if (CkVari2.Checked == true)
            {
                campovarioso2 = 1;
            }
            else
            {
                campovarioso2 = 0;
            }
            cmd.Parameters.AddWithValue("@CampVarios1", campovarioso1);
            cmd.Parameters.AddWithValue("@CampVarios2", campovarioso2);

            if (CkGener.Checked == true)
            {
                campogeneral = 1;
            }
            else
            {
                campogeneral = 0;
            }
            cmd.Parameters.AddWithValue("@CampGeneral", campogeneral);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        //FIUNCON PARA GUARDAR MIS SELECCION DE CAMPOS DE TIPO Y DESCRIPCION - DETALLES
        public void AgregarGrupoCamposSeleccionadosDetalle(DataGridView DGV, ComboBox TCaracteristicas1, ComboBox TCaracteristicas2, ComboBox TCaracteristicas3
            , ComboBox TCaracteristicas4, ComboBox DesCaracteristicas1, ComboBox DesCaracteristicas2, ComboBox DesCaracteristicas3, ComboBox DesCaracteristicas4
            , ComboBox TMedidas1, ComboBox TMedidas2, ComboBox TMedidas3, ComboBox TMedidas4, ComboBox DesMedidas1, ComboBox DesMedidas2, ComboBox DesMedidas3, ComboBox DesMedidas4
            , ComboBox TDiametros1, ComboBox TDiametros2, ComboBox TDiametros3, ComboBox TDiametros4, ComboBox DesDiametro1, ComboBox DesDiametro2, ComboBox DesDiametro3
            , ComboBox DesDiametro4, ComboBox TFormas1, ComboBox TFormas2, ComboBox TFormas3, ComboBox TFormas4, ComboBox DesFormas1, ComboBox DesFormas2, ComboBox DesFormas3
            , ComboBox DesFormas4, ComboBox TEspesores1, ComboBox TEspesores2, ComboBox TEspesores3, ComboBox TEspesores4, ComboBox DesEspesores1, ComboBox DesEspesores2
            , ComboBox DesEspesores3, ComboBox DesEspesores4, ComboBox TDiseñoAcabado1, ComboBox TDiseñoAcabado2, ComboBox TDiseñoAcabado3, ComboBox TDiseñoAcabado4
            , ComboBox DesDiseñoAcabado1, ComboBox DesDiseñoAcabado2, ComboBox DesDiseñoAcabado3, ComboBox DesDiseñoAcabado4, ComboBox Ntipos1, ComboBox Ntipos2, ComboBox Ntipos3
            , ComboBox Ntipos4, ComboBox DesNtipos1, ComboBox DesNtipos2, ComboBox DesNtipos3, ComboBox DesNtipos4, ComboBox TvariosO1, ComboBox TvariosO2, ComboBox DesVariosO1
            , ComboBox DesVariosO2, CheckBox General, TextBox descripciongenerales)
        {
            //GUARDAR PRODUCTOS - CAMPOS SELECCIONADOS - DETALLES-------------------------------------------------
            int codigoproducto = Convert.ToInt32(DGV.SelectedCells[0].Value.ToString());

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand("AgregarProducto_InsertarCamposSeleccionadosDetalles", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idart", codigoproducto);
            //INGRESO DE TIPOS CARACTERISTICAS 1
            if (TCaracteristicas1.Text == "" || TCaracteristicas1.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipomercaderia1", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipomercaderia1", TCaracteristicas1.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION CARACTERISTICAS 1
            if (DesCaracteristicas1.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionmercaderia1", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionmercaderia1", DesCaracteristicas1.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS CARACTERISTICAS 2
            if (TCaracteristicas2.Text == "" || TCaracteristicas2.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipomercaderia2", 0);

            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipomercaderia2", TCaracteristicas2.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION CARACTERISTICAS 2
            if (DesCaracteristicas2.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionmercaderia2", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionmercaderia2", DesCaracteristicas2.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS CARACTERISTICAS 3
            if (TCaracteristicas3.Text == "" || TCaracteristicas3.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipomercaderia3", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipomercaderia3", TCaracteristicas3.SelectedValue.ToString());

            }
            //INGRESO DE DESCRIPCION CARACTERISTICAS 3
            if (DesCaracteristicas3.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionmercaderia3", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionmercaderia3", DesCaracteristicas3.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS CARACTERISTICAS 4
            if (TCaracteristicas4.Text == "" || TCaracteristicas4.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipomercaderia4", 0);

            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipomercaderia4", TCaracteristicas4.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION CARACTERISTICAS 4
            if (DesCaracteristicas4.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionmercaderia4", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionmercaderia4", DesCaracteristicas4.SelectedValue.ToString());
            }
            //
            //INGRESO DE TIPOS MEDIDAS 1 
            if (TMedidas1.Text == "" || TMedidas1.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipomedida1", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipomedida1", TMedidas1.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION MEDIDAS 1
            if (DesMedidas1.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionmedida1", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionmedida1", DesMedidas1.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS MEDIDAS 2
            if (TMedidas2.Text == "" || TMedidas2.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipomedidaa2", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipomedidaa2", TMedidas2.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION MEDIDAS 2
            if (DesMedidas2.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionmedida2", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionmedida2", DesMedidas2.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS MEDIDAS 3
            if (TMedidas3.Text == "" || TMedidas3.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipomedida3", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipomedida3", TMedidas3.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION MEDIDAS 3
            if (DesMedidas3.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionmedida3", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionmedida3", DesMedidas3.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS MEDIDAS 4
            if (TMedidas4.Text == "" || TMedidas4.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipomedidaa4", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipomedidaa4", TMedidas4.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION MEDIDAS 4
            if (DesMedidas4.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionmedida4", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionmedida4", DesMedidas4.SelectedValue.ToString());
            }
            //
            //INGRESO DE TIPOS DIAMETROS 1
            if (TDiametros1.Text == "" || TDiametros1.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipodiametro1", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipodiametro1", TDiametros1.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION DOAMETROS 1
            if (DesDiametro1.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripciondiametro1", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripciondiametro1", DesDiametro1.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS DIAMETROS 2
            if (TDiametros2.Text == "" || TDiametros2.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipodiametro2", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipodiametro2", TDiametros2.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION DOAMETROS 2
            if (DesDiametro2.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripciondiametro2", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripciondiametro2", DesDiametro2.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS DIAMETROS 3
            if (TDiametros3.Text == "" || TDiametros3.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipodiametro3", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipodiametro3", TDiametros3.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION DOAMETROS 3
            if (DesDiametro3.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripciondiametro3", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripciondiametro3", DesDiametro3.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS DIAMETROS 4
            if (TDiametros4.Text == "" || TDiametros4.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipodiametro4", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipodiametro4", TDiametros4.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION DOAMETROS 4
            if (DesDiametro4.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripciondiametro4", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripciondiametro4", DesDiametro4.SelectedValue.ToString());
            }
            //
            //INGRESO DE TIPOS FORMAS 1
            if (TFormas1.Text == "" || TFormas1.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipoformas1", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipoformas1", TFormas1.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION FORMAS 1
            if (DesFormas1.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionformas1", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionformas1", DesFormas1.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS FORMAS 2
            if (TFormas2.Text == "" || TFormas2.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipoformas2", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipoformas2", TFormas2.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION FORMAS 2
            if (DesFormas2.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionformas2", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionformas2", DesFormas2.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS FORMAS 3
            if (TFormas3.Text == "" || TFormas3.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipoformas3", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipoformas3", TFormas3.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION FORMAS 3
            if (DesFormas3.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionformas3", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionformas3", DesFormas3.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS FORMAS 4
            if (TFormas4.Text == "" || TFormas4.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipoformas4", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipoformas4", TFormas4.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION FORMAS 4
            if (DesFormas4.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionformas4", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionformas4", DesFormas4.SelectedValue.ToString());
            }
            //
            //INGRESO DE TIPOS ESPESORES 1
            if (TEspesores1.Text == "" || TEspesores1.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipoespesores1", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipoespesores1", TEspesores1.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION ESPESORES 1
            if (DesEspesores1.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionespesores1", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionespesores1", DesEspesores1.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS ESPESORES 2
            if (TEspesores2.Text == "" || TEspesores2.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipoespesores2", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipoespesores2", TEspesores2.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION ESPESORES 2
            if (DesEspesores2.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionespesores2", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionespesores2", DesEspesores2.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS ESPESORES 3
            if (TEspesores3.Text == "" || TEspesores3.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipoespesores3", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipoespesores3", TEspesores3.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION ESPESORES 3
            if (DesEspesores3.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionespesores3", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionespesores3", DesEspesores3.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS ESPESORES 4
            if (TEspesores4.Text == "" || TEspesores4.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipoespesores4", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipoespesores4", TEspesores4.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION ESPESORES 4
            if (DesEspesores4.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionespesores4", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionespesores4", DesEspesores4.SelectedValue.ToString());
            }
            //
            //INGRESO DE TIPOS DISEÑO Y ACABADOS 1
            if (TDiseñoAcabado1.Text == "" || TDiseñoAcabado1.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipodiseñoacabado1", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipodiseñoacabado1", TDiseñoAcabado1.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION DISEÑO Y ACABADOS 1
            if (DesDiseñoAcabado1.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripciondiseñoacabado1", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripciondiseñoacabado1", DesDiseñoAcabado1.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS DISEÑO Y ACABADOS 2
            if (TDiseñoAcabado2.Text == "" || TDiseñoAcabado2.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipodiseñoacabado2", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipodiseñoacabado2", TDiseñoAcabado2.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION DISEÑO Y ACABADOS 2
            if (DesDiseñoAcabado2.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripciondiseñoacabado2", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripciondiseñoacabado2", DesDiseñoAcabado2.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS DISEÑO Y ACABADOS 3
            if (TDiseñoAcabado3.Text == "" || TDiseñoAcabado3.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipodiseñoacabado3", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipodiseñoacabado3", TDiseñoAcabado3.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION DISEÑO Y ACABADOS 3
            if (DesDiseñoAcabado3.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripciondiseñoacabado3", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripciondiseñoacabado3", DesDiseñoAcabado3.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS DISEÑO Y ACABADOS 4
            if (TDiseñoAcabado4.Text == "" || TDiseñoAcabado4.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipodiseñoacabado4", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipodiseñoacabado4", TDiseñoAcabado4.SelectedValue.ToString());
            }
            //INGRESO DE DESCRIPCION DISEÑO Y ACABADOS 4
            if (DesDiseñoAcabado4.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripciondiseñoacabado4", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripciondiseñoacabado4", DesDiseñoAcabado4.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS NUMERO Y TIPOS 1
            if (Ntipos1.Text == "" || Ntipos1.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipontipos1", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipontipos1", Ntipos1.SelectedValue.ToString());
            }
            //INGRESO DE DESCRICION NUMEROS Y TIPOS 1
            if (DesNtipos1.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionntipos1", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionntipos1", DesNtipos1.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS NUMERO Y TIPOS 2
            if (Ntipos2.Text == "" || Ntipos2.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipontipos2", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipontipos2", Ntipos2.SelectedValue.ToString());
            }
            //INGRESO DE DESCRICION NUMEROS Y TIPOS 2
            if (DesNtipos2.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionntipos2", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionntipos2", DesNtipos2.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS NUMERO Y TIPOS 3
            if (Ntipos3.Text == "" || Ntipos3.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipontipos3", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipontipos3", Ntipos3.SelectedValue.ToString());
            }
            //INGRESO DE DESCRICION NUMEROS Y TIPOS 3
            if (DesNtipos3.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionntipos3", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionntipos3", DesNtipos3.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS NUMERO Y TIPOS 4
            if (Ntipos4.Text == "" || Ntipos4.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipontipos4", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipontipos4", Ntipos4.SelectedValue.ToString());
            }
            //INGRESO DE DESCRICION NUMEROS Y TIPOS 4
            if (DesNtipos4.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionntipos4", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionntipos4", DesNtipos4.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS VARIOS 1
            if (TvariosO1.Text == "" || TvariosO1.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipovarioso1", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipovarioso1", TvariosO1.SelectedValue.ToString());
            }
            //INGRESO DE DESCRICION VARIOS 1
            if (DesVariosO1.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionvarioso1", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionvarioso1", DesVariosO1.SelectedValue.ToString());
            }
            //INGRESO DE TIPOS VARIOS 2
            if (TvariosO2.Text == "" || TvariosO2.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@idtipovarioso2", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@idtipovarioso2", TvariosO2.SelectedValue.ToString());
            }
            //INGRESO DE DESCRICION VARIOS 2
            if (DesVariosO2.SelectedValue == null)
            {
                cmd.Parameters.AddWithValue("@iddescripcionvarioso2", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@iddescripcionvarioso2", DesVariosO2.SelectedValue.ToString());
            }
            //INGRESO DE CAMPO GENERAL
            if (General.Checked == true)
            {
                cmd.Parameters.AddWithValue("@campogeneral", descripciongenerales.Text);
            }
            else
            {
                cmd.Parameters.AddWithValue("@campogeneral", "");
            }
            cmd.ExecuteNonQuery();
            con.Close();
        }

        //METODO PARA AGREGAR PRODUCTOS
        public void AgregarProductos(string anotaciones, string codigo, string descripciongeneradaproducto)
        {
            ////INGRESAR TABLA PRINCIPAL DE PRODUCTOS
            //SI EL CAMPO ANOTACIONES/CÓDIGO BSS REFERENCIAL NO ESTA VACIO
            if (anotaciones != "")
            {
                //SI EL CAMPO CÓDIGO DEL PRODUCTO GENERÓ ADECUADAMENTE EL CÓDIGO
                if (codigo != "")
                {
                    //CONFIRMACIÓN DE LA ACCIÓN DE GUARDAR CON SUS DETALLES
                    DialogResult boton = MessageBox.Show("Esta por guardar un nuevo producto con el código " + codigo + " y con la siguiente descripción " + descripciongeneradaproducto + ", ¿Realmente desea guardar este producto?.", "Nuevo Producto", MessageBoxButtons.OKCancel);
                    if (boton == DialogResult.OK)
                    {
                        //SISTEMA DE VALIDACION DE CAMPOS VACIOS
                        ValidarCamposVacios();
                        if (EstadoValidacionCampoVacios == 1)
                        {
                            return;
                        }
                        //SISTEMA DE VALIDACIÓN DE PRODUICTOS EXISTENTES
                        ValidacionCampos();
                        //SISTEMA DE BUSQUEDA DE PRODUCTOS EXISTENTES
                        ValidacionCamposBusqueda();
                        //SI EXSISTEN CAMPOS IGUALES
                        if (EstadoCaracteristicas1 == true && EstadoCaracteristicas2 == true && EstadoMedidas1 == true && EstadoMedidas2 == true &&
                            EstadoDiametros1 == true && EstadoDiametros2 == true && EstadoFormas1 == true && EstadoFormas2 == true &&
                            EstadoEspesores1 == true && EstadoEspesores2 == true && EstadoDiseñoAcabados1 == true && EstadoDiseñoAcabados2 == true &&
                            EstadoNTipos1 == true && EstadoNTipos2 == true && EstadoVarios01 == true && EstadoVarios02 == true &&
                            EstadoGeneral == true || EstadoNombreProducto == true)
                        {
                            MessageBox.Show("El producto que intenta ingresar ya existe, por favor revisar los datos seleccionados.", "Validación del Sistema", MessageBoxButtons.OK);
                        }
                        else
                        {
                            try
                            {
                                AgregarPlano(txtCodigo.Text, Convert.ToString(cboTipoMedida.SelectedValue), Convert.ToInt32(cboTipoMercaderia.SelectedValue), Convert.ToInt32(cboModelos.SelectedValue)
                                    , Convert.ToInt32(cboLineas.SelectedValue), Convert.ToInt32(cboDiferencial.SelectedValue), txtDescripcionGeneradaProducto.Text, txtAnotaciones.Text
                                    , ckSemiProducido, lblCodigoReferenciaPlano.Text, lblCodigoPlano.Text, txtFile.Text, dataListadoCdigoPlano);

                                //CAPTURAR EL CÓDIGO DEL PRODUCTO GENERADO PARA ENLAZARLO CON SUS CARACTERISTICAS Y ATRIBUTOS
                                codigoProducto();

                                Agregar_DatosAnexos(Convert.ToDecimal(txtPeso.Text), txtUbicacion.Text, Convert.ToDecimal(txtMinimo.Text), Convert.ToDecimal(txtMaximo.Text)
                                    , Convert.ToInt32(cboOrigen.SelectedValue), Convert.ToInt32(cboTerminosCompra.SelectedValue), txtContenedor.Text, Convert.ToDecimal(txtPesoContenedor.Text)
                                    , txtMedidas.Text, Convert.ToInt32(cboTipoExistencia.SelectedValue), txtCodigoUNSPCS.Text, Convert.ToDouble(txtPorcentajePercepcion.Text)
                                    , Convert.ToInt32(cboBienesSujetoPercepcion.SelectedValue), Convert.ToDouble(txtPorcentajeDetraccion.Text), Convert.ToDouble(txtPorcentajeISC.Text));

                                AgregarGruposCamposSeleccionados(dataListadoCdigoProducto, ckCaracteristicas1, ckCaracteristicas2, ckCamposMedida1, ckCamposMedida2, ckCamposDiametros1, ckCamposDiametros2
                                    , ckCamposFormas1, ckCamposFormas2, ckCamposEspesores1, ckCamposEspesores2, ckCamposDiseñoAcabado1, ckCamposDiseñoAcabado2, ckCamposNTipos1
                                    , ckCamposNTipos2, ckVariosO1, ckVariosO2, ckGenerales);

                                AgregarGrupoCamposSeleccionadosDetalle(dataListadoCdigoProducto, cboTipoCaracteristicas1, cboTipoCaracteristicas2
                                    , cboTipoCaracteristicas3, cboTipoCaracteristicas4, cboDescripcionCaracteristicas1, cboDescripcionCaracteristicas2
                                    , cboDescripcionCaracteristicas3, cboDescripcionCaracteristicas4, cboTipoMedida1, cboTipoMedida2, cboTipoMedida3
                                    , cboTipoMedida4, cboDescripcionMedida1, cboDescripcionMedida2, cboDescripcionMedida3, cboDescripcionMedida4
                                    , cboTiposDiametros1, cboTiposDiametros2, cboTiposDiametros3, cboTiposDiametros4, cboDescripcionDiametros1
                                    , cboDescripcionDiametros2, cboDescripcionDiametros3, cboDescripcionDiametros4, cboTiposFormas1, cboTiposFormas2
                                    , cboTiposFormas3, cboTiposFormas4, cboDescripcionFormas1, cboDescripcionFormas2, cboDescripcionFormas3
                                    , cboDescripcionFormas4, cbooTipoEspesores1, cbooTipoEspesores2, cbooTipoEspesores3, cbooTipoEspesores4
                                    , cboDescripcionEspesores1, cboDescripcionEspesores2, cboDescripcionEspesores3, cboDescripcionEspesores4
                                    , cboTiposDiseñosAcabados1, cboTiposDiseñosAcabados2, cboTiposDiseñosAcabados3, cboTiposDiseñosAcabados4
                                    , cboDescripcionDiseñoAcabado1, cboDescripcionDiseñoAcabado2, cboDescripcionDiseñoAcabado3, cboDescripcionDiseñoAcabado4
                                    , cboTiposNTipos1, cboTiposNTipos2, cboTiposNTipos3, cboTiposNTipos4, cboDescripcionNTipos1, cboDescripcionNTipos2
                                    , cboDescripcionNTipos3, cboDescripcionNTipos4, cboTiposVariosO1, cboTiposVariosO2, cboDescripcionVariosO1, cboDescripcionVariosO2
                                    , ckGenerales, txtDescripcionGeneral);

                                MessageBox.Show("Producto ingresado correctamente.", "Nuevo Producto", MessageBoxButtons.OK);

                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe ingresar un código referencial del BSS para el producto.", "Nuevo Producto", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("Debe ingresar un código para el producto.", "Nuevo Producto", MessageBoxButtons.OK);
                }
            }
        }

        //BOTON GUARDAR PRODUCTO
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            AgregarProductos(txtAnotaciones.Text, txtCodigo.Text, txtDescripcionGeneradaProducto.Text);
        }

        //SALIR DE NUEVO PRODUCTOS
        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        //FIN----------------------------------------------------------------------------------------------------------

        //VALIDACIONES DE LOS DATOS ANEXOS AL PRODUCTO Y GENERACION DE CODIGO PARA LOS PLANOS-----------------------------------
        //ACCIONES DE CAMPOS ANEXOS
        public void GeneracionReferenciaPlano(Label lbl1, Label lbl2)
        {
            string abreviaturaModelo = "";
            abreviaturaModelo = lbl1.Text;

            lbl2.Text = "A-" + abreviaturaModelo + "00";
        }

        //CANCELAR DATOS ANEXOS Y LIMPIAR TODOS LOS CAMPOS
        private void btCancelarDatosAnexos_Click(object sender, EventArgs e)
        {
            //campos de stok y ubicacion
            LimpiarCamposDatosAnexos();
        }

        //METODO PARA LIMPIAR TODOS LOS CAMPOS DE DATOS ANEXOS AL PRODUCTO
        public void LimpiarCamposDatosAnexos()
        {
            afectadoIGV = 0;
            ckAfectoIGV.Checked = false;
            controlarstock = 0;
            ckControlarStock.Checked = false;
            juego = 0;
            ckJuego.Checked = false;

            servicio = 0;
            ckServicio.Checked = false;
            controlarlotes = 0;
            ckControlarLote.Checked = false;
            controlarserie = 0;
            ckControlarSerie.Checked = false;

            txtPeso.Text = "0.00000";
            txtUbicacion.Text = "";
            reposicion = 0;

            txtMaximo.Text = "0.00000";
            txtMinimo.Text = "0.00000";
            ckReposicion.Checked = false;

            //campos de importaciones
            txtContenedor.Text = "";
            txtPesoContenedor.Text = "0.00000";
            txtMedidas.Text = "";

            //campos de sunat
            txtCodigoUNSPCS.Text = "";
            sujetropercepcion = 0;
            ckSujetoPercepcion.Checked = false;

            txtPorcentajePercepcion.Text = "0.00000";
            sujetodetraccion = 0;
            skSujetoDetraccion.Checked = false;
            txtPorcentajeDetraccion.Text = "0.00000";

            sujetoisc = 0;
            ckSujetoISC.Checked = false;
            txtPorcentajeISC.Text = "0.00000";
            panelDatosAnexos.Visible = false;
        }

        //METODO PARA GENERAR EL CÓDIGO DEL PRODUCTO
        public void GenerarCodigoProducto()
        {
            if (nuemroProducto.Length == 1)
            {
                nuemroProducto = "00000" + nuemroProducto;
            }
            else if (nuemroProducto.Length == 2)
            {
                nuemroProducto = "0000" + nuemroProducto;
            }
            else if (nuemroProducto.Length == 3)
            {
                nuemroProducto = "000" + nuemroProducto;
            }
            else if (nuemroProducto.Length == 4)
            {
                nuemroProducto = "00" + nuemroProducto;
            }
            else if (nuemroProducto.Length == 5)
            {
                nuemroProducto = "0" + nuemroProducto;
            }
            txtCodigo.Text = lblTipMercaderia.Text + lblLinea.Text + lblModelo.Text + nuemroProducto;
        }

        //ABRIR DATOS ANEXOS AL PRODUCTO
        private void btnAbrirDatosAnexos_Click(object sender, EventArgs e)
        {
            panelDatosAnexos.Visible = true;
        }

        //ACEPTAR Y GAURDAR LOS DATOS ANEXOS
        private void btnAceptarDatosAnexos_Click(object sender, EventArgs e)
        {
            panelDatosAnexos.Visible = false;
        }

        //DATOS ANEXOS Y ACCIONES DEL PANEL DE DATOS ANEXOS-------------
        //ACCIÓN DE AFECTA IGV
        private void ckAfectoIGV_CheckedChanged(object sender, EventArgs e)
        {
            if (ckAfectoIGV.Checked)
            {
                afectadoIGV = 1;
                lblSiNo1.Text = "Si";
            }
            else
            {
                afectadoIGV = 0;
                lblSiNo1.Text = "No";
            }
        }

        //ACCIÓN DE CONTROL STOCK
        private void ckControlarStock_CheckedChanged(object sender, EventArgs e)
        {
            if (ckControlarStock.Checked)
            {
                controlarstock = 1;
                lblSiNo3.Text = "Si";
            }
            else
            {
                controlarstock = 0;
                lblSiNo3.Text = "No";
            }
        }

        //ACCIÓN DE JUEGO 
        private void ckJuego_CheckedChanged(object sender, EventArgs e)
        {
            if (ckJuego.Checked)
            {
                juego = 1;
                lblSiNo5.Text = "Si";
            }
            else
            {
                juego = 0;
                lblSiNo5.Text = "No";
            }
        }

        //ACCIÓN DE REPOSICIÓN
        private void ckReposicion_CheckedChanged(object sender, EventArgs e)
        {
            if (ckReposicion.Checked)
            {
                reposicion = 1;
                lblSiNo7.Text = "Si";
            }
            else
            {
                reposicion = 0;
                lblSiNo7.Text = "No";
            }
        }

        //ACCIPON DE SERVICIO
        private void ckServicio_CheckedChanged(object sender, EventArgs e)
        {
            if (ckServicio.Checked)
            {
                servicio = 1;
                lblSiNo2.Text = "Si";
            }
            else
            {
                servicio = 0;
                lblSiNo2.Text = "No";
            }
        }

        //ACCIÓN DE CONTROLAR LOTE
        private void ckControlarLote_CheckedChanged(object sender, EventArgs e)
        {
            if (ckControlarLote.Checked)
            {
                controlarlotes = 1;
                lblSiNo4.Text = "Si";
            }
            else
            {
                controlarlotes = 0;
                lblSiNo4.Text = "No";
            }
        }

        //ACCIÓN DE CONTROL DE SERIE
        private void ckControlarSerie_CheckedChanged(object sender, EventArgs e)
        {
            if (ckControlarSerie.Checked)
            {
                controlarserie = 1;
                lblSiNo6.Text = "Si";
            }
            else
            {
                controlarserie = 0;
                lblSiNo6.Text = "No";
            }
        }

        //ACCIÓN DE HABILITAR SUJETO PERCEPCIÓN
        private void ckSujetoPercepcion_CheckedChanged(object sender, EventArgs e)
        {
            if (ckSujetoPercepcion.Checked)
            {
                sujetropercepcion = 1;
                txtPorcentajePercepcion.Visible = true;
                cboBienesSujetoPercepcion.Visible = true;
                lblLeyendaPercepcion.Visible = true;
                lblLeyendaBienesPercepcion.Visible = false;
            }
            else
            {
                sujetropercepcion = 0;
                txtPorcentajePercepcion.Visible = false;
                txtPorcentajePercepcion.Text = "0.00";
                cboBienesSujetoPercepcion.Visible = false;
                lblLeyendaPercepcion.Visible = false;
                lblLeyendaBienesPercepcion.Visible = true;
            }
        }

        //ACCIÓN DE HABILITAR SUJETO ISC
        private void ckSujetoISC_CheckedChanged(object sender, EventArgs e)
        {
            if (ckSujetoISC.Checked)
            {
                sujetoisc = 1;
                txtPorcentajeISC.Visible = true;
                lblLeyendaISC.Visible = true;
            }
            else
            {
                sujetoisc = 0;
                txtPorcentajeISC.Visible = false;
                txtPorcentajeISC.Text = "0.00";
                lblLeyendaISC.Visible = false;
            }
        }

        //ACCIÓN DE HABILITAR SUJETO DETRACCIÓN
        private void skSujetoDetraccion_CheckedChanged(object sender, EventArgs e)
        {
            if (skSujetoDetraccion.Checked)
            {
                sujetodetraccion = 1;
                txtPorcentajeDetraccion.Visible = true;
                lblLeyendaDetraccion.Visible = true;
            }
            else
            {
                sujetodetraccion = 0;
                txtPorcentajeDetraccion.Visible = false;
                txtPorcentajeDetraccion.Text = "0.00";
                lblLeyendaDetraccion.Visible = false;
            }
        }

        //VALIDACIONES ----------------------------------------------------
        //VALIDACIÓN DE SOLO NÚMEROS
        private void txtPorcentajePercepcion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        //VALIDACIÓN DE SOLO NÚMEROS
        private void txtPorcentajeISC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        //VALIDACIÓN DE SOLO NÚMEROS
        private void txtPorcentajeDetraccion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        //VALIDACIÓN DE SOLO NÚMEROS
        private void txtPesoContenedor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        //VALIDACIÓN DE SOLO NÚMEROS
        private void txtPeso_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        //VALIDACIÓN DE SOLO NÚMEROS
        private void txtMaximo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        //VALIDACIÓN DE SOLO NÚMEROS
        private void txtMinimo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        //SISTEMA DE VALIDACIÓN--------------------------------------------------------------------------------------------
        //VALIDAR EXISTENCIA DEL PRODUCTO
        public void ValidacionCampos()
        {
            ValidacionProducto();

            //DECLARACION DE VARIABLES PARA ALMACENAR LOS DATOS INGRESADOS POR EL USUARIO - CARACtERISTICAS 1
            int? codigotipocaracteristicas1 = Convert.ToInt32(cboTipoCaracteristicas1.SelectedValue);
            int? codigodescripcioncaracteristicas1 = Convert.ToInt32(cboDescripcionCaracteristicas1.SelectedValue);
            int? codigotipocaracteristicas2 = Convert.ToInt32(cboTipoCaracteristicas2.SelectedValue);
            int? codigodescripcioncaracteristicas2 = Convert.ToInt32(cboDescripcionCaracteristicas2.SelectedValue);

            //DECLARACION DE VARIABLES PARA ALMACENAR LOS DATOS INGRESADOS POR EL USUARIO - CARACtERISTICAS 2
            int? codigotipocaracteristicas3 = Convert.ToInt32(cboTipoCaracteristicas3.SelectedValue);
            int? codigodescripcioncaracteristicas3 = Convert.ToInt32(cboDescripcionCaracteristicas3.SelectedValue);
            int? codigotipocaracteristicas4 = Convert.ToInt32(cboTipoCaracteristicas4.SelectedValue);
            int? codigodescripcioncaracteristicas4 = Convert.ToInt32(cboDescripcionCaracteristicas4.SelectedValue);

            //DECLARACION DE VARIABLES PARA ALMACENAR LOS DATOS INGRESADOS POR EL USUARIO - MEDIDAS 1
            int? codigotipomedidas1 = Convert.ToInt32(cboTipoMedida1.SelectedValue);
            int? codigodescripcionmedidas1 = Convert.ToInt32(cboDescripcionMedida1.SelectedValue);
            int? codigotipomedidas2 = Convert.ToInt32(cboTipoMedida2.SelectedValue);
            int? codigodescripcionmedidas2 = Convert.ToInt32(cboDescripcionMedida2.SelectedValue);

            //DECLARACION DE VARIABLES PARA ALMACENAR LOS DATOS INGRESADOS POR EL USUARIO - MEDIDAS 2
            int? codigotipomedidas3 = Convert.ToInt32(cboTipoMedida3.SelectedValue);
            int? codigodescripcionmedidas3 = Convert.ToInt32(cboDescripcionMedida3.SelectedValue);
            int? codigotipomedidas4 = Convert.ToInt32(cboTipoMedida4.SelectedValue);
            int? codigodescripcionmedidas4 = Convert.ToInt32(cboDescripcionMedida4.SelectedValue);

            //DECLARACION DE VARIABLES PARA ALMACENAR LOS DATOS INGRESADOS POR EL USUARIO - DIAMETROS 1
            int? codigotipodiametro1 = Convert.ToInt32(cboTiposDiametros1.SelectedValue);
            int? codigodescripciondiametro1 = Convert.ToInt32(cboDescripcionDiametros1.SelectedValue);
            int? codigotipodiametro2 = Convert.ToInt32(cboTiposDiametros2.SelectedValue);
            int? codigodescripciondiametro2 = Convert.ToInt32(cboDescripcionDiametros2.SelectedValue);

            //DECLARACION DE VARIABLES PARA ALMACENAR LOS DATOS INGRESADOS POR EL USUARIO - DIAMETROS 2
            int? codigotipodiametro3 = Convert.ToInt32(cboTiposDiametros3.SelectedValue);
            int? codigodescripciondiametro3 = Convert.ToInt32(cboDescripcionDiametros3.SelectedValue);
            int? codigotipodiametro4 = Convert.ToInt32(cboTiposDiametros4.SelectedValue);
            int? codigodescripciondiametro4 = Convert.ToInt32(cboDescripcionDiametros4.SelectedValue);

            //DECLARACION DE VARIABLES PARA ALMACENAR LOS DATOS INGRESADOS POR EL USUARIO - FORMAS 1
            int? codigotipoformas1 = Convert.ToInt32(cboTiposFormas1.SelectedValue);
            int? codigodescripcionformas1 = Convert.ToInt32(cboDescripcionFormas1.SelectedValue);
            int? codigotipoformas2 = Convert.ToInt32(cboTiposFormas2.SelectedValue);
            int? codigodescripcionformas2 = Convert.ToInt32(cboDescripcionFormas2.SelectedValue);

            //DECLARACION DE VARIABLES PARA ALMACENAR LOS DATOS INGRESADOS POR EL USUARIO - FORMAS 2
            int? codigotipoformas3 = Convert.ToInt32(cboTiposFormas3.SelectedValue);
            int? codigodescripcionformas3 = Convert.ToInt32(cboDescripcionFormas3.SelectedValue);
            int? codigotipoformas4 = Convert.ToInt32(cboTiposFormas4.SelectedValue);
            int? codigodescripcionformas4 = Convert.ToInt32(cboDescripcionFormas4.SelectedValue);

            //DECLARACION DE VARIABLES PARA ALMACENAR LOS DATOS INGRESADOS POR EL USUARIO - ESPESORES 1
            int? codigotipoespesores1 = Convert.ToInt32(cbooTipoEspesores1.SelectedValue);
            int? codigodescripcionespesores1 = Convert.ToInt32(cboDescripcionEspesores1.SelectedValue);
            int? codigotipoespesores2 = Convert.ToInt32(cbooTipoEspesores2.SelectedValue);
            int? codigodescripcionespesores2 = Convert.ToInt32(cboDescripcionEspesores2.SelectedValue);

            //DECLARACION DE VARIABLES PARA ALMACENAR LOS DATOS INGRESADOS POR EL USUARIO - ESPESORES 2
            int? codigotipoespesores3 = Convert.ToInt32(cbooTipoEspesores3.SelectedValue);
            int? codigodescripcionespesores3 = Convert.ToInt32(cboDescripcionEspesores3.SelectedValue);
            int? codigotipoespesores4 = Convert.ToInt32(cbooTipoEspesores4.SelectedValue);
            int? codigodescripcionespesores4 = Convert.ToInt32(cboDescripcionEspesores4.SelectedValue);

            //DECLARACION DE VARIABLES PARA ALMACENAR LOS DATOS INGRESADOS POR EL USUARIO - DISEÑO 1
            int? codigotipodiseñoacabados1 = Convert.ToInt32(cboTiposDiseñosAcabados1.SelectedValue);
            int? codigodescripciondiseñoacabados1 = Convert.ToInt32(cboDescripcionDiseñoAcabado1.SelectedValue);
            int? codigotipodiseñoacabados2 = Convert.ToInt32(cboTiposDiseñosAcabados2.SelectedValue);
            int? codigodescripciondiseñoacabados2 = Convert.ToInt32(cboDescripcionDiseñoAcabado2.SelectedValue);

            //DECLARACION DE VARIABLES PARA ALMACENAR LOS DATOS INGRESADOS POR EL USUARIO - DISEÑO 2
            int? codigotipodiseñoacabados3 = Convert.ToInt32(cboTiposDiseñosAcabados3.SelectedValue);
            int? codigodescripciondiseñoacabados3 = Convert.ToInt32(cboDescripcionDiseñoAcabado3.SelectedValue);
            int? codigotipodiseñoacabados4 = Convert.ToInt32(cboTiposDiseñosAcabados4.SelectedValue);
            int? codigodescripciondiseñoacabados4 = Convert.ToInt32(cboDescripcionDiseñoAcabado4.SelectedValue);

            //DECLARACION DE VARIABLES PARA ALMACENAR LOS DATOS INGRESADOS POR EL USUARIO - N-TIPOS 1
            int? codigotipontipos1 = Convert.ToInt32(cboTiposNTipos1.SelectedValue);
            int? codigodescripcionntipos1 = Convert.ToInt32(cboDescripcionNTipos1.SelectedValue);
            int? codigotipontipos2 = Convert.ToInt32(cboTiposNTipos2.SelectedValue);
            int? codigodescripcionntipos2 = Convert.ToInt32(cboDescripcionNTipos2.SelectedValue);

            //DECLARACION DE VARIABLES PARA ALMACENAR LOS DATOS INGRESADOS POR EL USUARIO - N-TIPOS 2 
            int? codigotipontipos3 = Convert.ToInt32(cboTiposNTipos3.SelectedValue);
            int? codigodescripcionntipos3 = Convert.ToInt32(cboDescripcionNTipos3.SelectedValue);
            int? codigotipontipos4 = Convert.ToInt32(cboTiposNTipos4.SelectedValue);
            int? codigodescripcionntipos4 = Convert.ToInt32(cboDescripcionNTipos4.SelectedValue);

            //DECLARACION DE VARIABLES PARA ALMACENAR LOS DATOS INGRESADOS POR EL USUARIO - VARIOS-01
            int? codigotipovarios1 = Convert.ToInt32(cboTiposVariosO1.SelectedValue);
            int? codigodescripcionvarios01 = Convert.ToInt32(cboDescripcionVariosO1.SelectedValue);

            //DECLARACION DE VARIABLES PARA ALMACENAR LOS DATOS INGRESADOS POR EL USUARIO - VARIOS-02
            int? codigotipovarios2 = Convert.ToInt32(cboTiposVariosO2.SelectedValue);
            int? codigodescripcionvarios02 = Convert.ToInt32(cboDescripcionVariosO2.SelectedValue);

            string campogeneral = txtDescripcionGeneral.Text;

            if (datalistadoValidacionProducto.RowCount == 0)
            {
                EstadoCaracteristicas1 = false;
                EstadoCaracteristicas2 = false;
                EstadoMedidas1 = false;
                EstadoMedidas2 = false;
                EstadoDiametros1 = false;
                EstadoDiametros2 = false;
                EstadoFormas1 = false;
                EstadoFormas2 = false;
                EstadoEspesores1 = false;
                EstadoEspesores2 = false;
                EstadoDiseñoAcabados1 = false;
                EstadoDiseñoAcabados2 = false;
                EstadoNTipos1 = false;
                EstadoNTipos2 = false;
                EstadoVarios01 = false;
                EstadoVarios02 = false;
                EstadoGeneral = false;
            }
            else
            {
                //INICIO DEL SISTEMA DE VALIDACION
                foreach (DataGridViewRow datorecuperado in datalistadoValidacionProducto.Rows)
                {
                    //VALIDACION DE CARACTERISTICAS 1
                    int detalletipocarac1 = Convert.ToInt32(datorecuperado.Cells["IdTipoCaracteristicas1"].Value);
                    int detalledescripcioncarac1 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionCaracteristicas1"].Value);
                    int detalletipocarac2 = Convert.ToInt32(datorecuperado.Cells["IdTipoCaracteristicas2"].Value);
                    int detalledescripcioncarac2 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionCaracteristicas2"].Value);

                    if (detalletipocarac1 == codigotipocaracteristicas1 && detalledescripcioncarac1 == codigodescripcioncaracteristicas1 && detalletipocarac2 == codigotipocaracteristicas2 && detalledescripcioncarac2 == codigodescripcioncaracteristicas2)
                    {
                        EstadoCaracteristicas1 = true;
                    }
                    else
                    {
                        EstadoCaracteristicas1 = false;
                    }

                    //VALIDACION DE CARACTERISTICAS 2
                    int detalletipocarac3 = Convert.ToInt32(datorecuperado.Cells["IdTipoCaracteristicas3"].Value);
                    int detalledescripcioncarac3 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionCaracteristicas3"].Value);
                    int detalletipocarac4 = Convert.ToInt32(datorecuperado.Cells["IdTipoCaracteristicas4"].Value);
                    int detalledescripcioncarac4 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionCaracteristicas4"].Value);

                    if (detalletipocarac3 == codigotipocaracteristicas3 && detalledescripcioncarac3 == codigodescripcioncaracteristicas3 && detalletipocarac4 == codigotipocaracteristicas4 && detalledescripcioncarac4 == codigodescripcioncaracteristicas4)
                    {
                        EstadoCaracteristicas2 = true;
                    }
                    else
                    {
                        EstadoCaracteristicas2 = false;
                    }

                    //VALIDACION DE MEDIDAS 1
                    int delattemedida1 = Convert.ToInt32(datorecuperado.Cells["IdTipoMedidas1"].Value);
                    int detalledescripcionmedida1 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionMedidas1"].Value);
                    int delattemedida2 = Convert.ToInt32(datorecuperado.Cells["IdTipoMedidas2"].Value);
                    int detalledescripcionmedida2 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionMedidas2"].Value);

                    if (delattemedida1 == codigotipomedidas1 && detalledescripcionmedida1 == codigodescripcionmedidas1 && delattemedida2 == codigotipomedidas2 && detalledescripcionmedida2 == codigodescripcionmedidas2)
                    {
                        EstadoMedidas1 = true;
                    }
                    else
                    {
                        EstadoMedidas1 = false;
                    }

                    //VALIDACION DE MEDIDAS 2
                    int detallemedida3 = Convert.ToInt32(datorecuperado.Cells["IdTipoMedidas3"].Value);
                    int detalledescripcionmedida3 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionMedidas3"].Value);
                    int detallemedida4 = Convert.ToInt32(datorecuperado.Cells["IdTipoMedidas4"].Value);
                    int detalledescripcionmedida4 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionMedidas4"].Value);

                    if (detallemedida3 == codigotipomedidas3 && detalledescripcionmedida3 == codigodescripcionmedidas3 && detallemedida4 == codigotipomedidas4 && detalledescripcionmedida4 == codigodescripcionmedidas4)
                    {
                        EstadoMedidas2 = true;
                    }
                    else
                    {
                        EstadoMedidas2 = false;
                    }

                    //VALIDACION DE DIAMETROS 1
                    int detalletipodiametro1 = Convert.ToInt32(datorecuperado.Cells["IdTipoDiametros1"].Value);
                    int detalledescripciondiametro1 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionDiametros1"].Value);
                    int detalletipodiametro2 = Convert.ToInt32(datorecuperado.Cells["IdTipoDiametros2"].Value);
                    int detalledescripciondiametro2 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionDiametros2"].Value);

                    if (detalletipodiametro1 == codigotipodiametro1 && detalledescripciondiametro1 == codigodescripciondiametro1 && detalletipodiametro2 == codigotipodiametro2 && detalledescripciondiametro2 == codigodescripciondiametro2)
                    {
                        EstadoDiametros1 = true;
                    }
                    else
                    {
                        EstadoDiametros1 = false;
                    }

                    //VALIDACION DE DIAMETROS 2
                    int detalletipodiametro3 = Convert.ToInt32(datorecuperado.Cells["IdTipoDiametros3"].Value);
                    int detalledescripciondiametro3 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionDiametros3"].Value);
                    int detalletipodiametro4 = Convert.ToInt32(datorecuperado.Cells["IdTipoDiametros4"].Value);
                    int detalledescripciondiametro4 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionDiametros4"].Value);

                    if (detalletipodiametro3 == codigotipodiametro3 && detalledescripciondiametro3 == codigodescripciondiametro3 && detalletipodiametro4 == codigotipodiametro4 && detalledescripciondiametro4 == codigodescripciondiametro4)
                    {
                        EstadoDiametros2 = true;
                    }
                    else
                    {
                        EstadoDiametros2 = false;
                    }

                    //VALIDACION DE FORMAS 1
                    int detalletipoformas1 = Convert.ToInt32(datorecuperado.Cells["IdTipoFormas1"].Value);
                    int detalledescripcionformas1 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionFormas1"].Value);
                    int detalletipoformas2 = Convert.ToInt32(datorecuperado.Cells["IdTipoFormas2"].Value);
                    int detalledescripcionformas2 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionFormas2"].Value);

                    if (detalletipoformas1 == codigotipoformas1 && detalledescripcionformas1 == codigodescripcionformas1 && detalletipoformas2 == codigotipoformas2 && detalledescripcionformas2 == codigodescripcionformas2)
                    {
                        EstadoFormas1 = true;
                    }
                    else
                    {
                        EstadoFormas1 = false;
                    }

                    //VALIDACION DE FORMAS 2
                    int detalletipoformas3 = Convert.ToInt32(datorecuperado.Cells["IdTipoFormas3"].Value);
                    int detalledescripcionformas3 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionFormas3"].Value);
                    int detalletipoformas4 = Convert.ToInt32(datorecuperado.Cells["IdTipoFormas4"].Value);
                    int detalledescripcionformas4 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionFormas4"].Value);

                    if (detalletipoformas3 == codigotipoformas3 && detalledescripcionformas3 == codigodescripcionformas3 && detalletipoformas4 == codigotipoformas4 && detalledescripcionformas4 == codigodescripcionformas4)
                    {
                        EstadoFormas2 = true;
                    }
                    else
                    {
                        EstadoFormas2 = false;
                    }

                    //VALIDACION DE ESPESORES 1
                    int detalletipoespesores1 = Convert.ToInt32(datorecuperado.Cells["IdTipoEspesores1"].Value);
                    int detalledescripcionespesores1 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionEspesores1"].Value);
                    int detalletipoespesores2 = Convert.ToInt32(datorecuperado.Cells["IdTipoEspesores2"].Value);
                    int detalledescripcionespesores2 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionEspesores2"].Value);

                    if (detalletipoespesores1 == codigotipoespesores1 && detalledescripcionespesores1 == codigodescripcionespesores1 && detalletipoespesores2 == codigotipoespesores2 && detalledescripcionespesores2 == codigodescripcionespesores2)
                    {
                        EstadoEspesores1 = true;
                    }
                    else
                    {
                        EstadoEspesores1 = false;
                    }

                    //VALIDACION DE ESPESORES 2
                    int detalletipoespesores3 = Convert.ToInt32(datorecuperado.Cells["IdTipoEspesores3"].Value);
                    int detalledescripcionespesores3 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionEspesores3"].Value);
                    int detalletipoespesores4 = Convert.ToInt32(datorecuperado.Cells["IdTipoEspesores4"].Value);
                    int detalledescripcionespesores4 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionEspesores4"].Value);

                    if (detalletipoespesores3 == codigotipoespesores3 && detalledescripcionespesores3 == codigodescripcionespesores3 && detalletipoespesores4 == codigotipoespesores4 && detalledescripcionespesores4 == codigodescripcionespesores4)
                    {
                        EstadoEspesores2 = true;
                    }
                    else
                    {
                        EstadoEspesores2 = false;
                    }

                    //VALIDACION DE DISEÑO 1
                    int detalletipodiseño1 = Convert.ToInt32(datorecuperado.Cells["IdTipoDiseñoAcabado1"].Value);
                    int detalledescripciondiseño1 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionDiseñoAcabado1"].Value);
                    int detalletipodiseño2 = Convert.ToInt32(datorecuperado.Cells["IdTipoDiseñoAcabado2"].Value);
                    int detalledescripciondiseño2 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionDiseñoAcabado2"].Value);

                    if (detalletipodiseño1 == codigotipodiseñoacabados1 && detalledescripciondiseño1 == codigodescripciondiseñoacabados1 && detalletipodiseño2 == codigotipodiseñoacabados2 && detalledescripciondiseño2 == codigodescripciondiseñoacabados2)
                    {
                        EstadoDiseñoAcabados1 = true;
                    }
                    else
                    {
                        EstadoDiseñoAcabados1 = false;
                    }

                    //VALIDACION DE DISEÑO 2
                    int detalletipodiseño3 = Convert.ToInt32(datorecuperado.Cells["IdTipoDiseñoAcabado3"].Value);
                    int detalledescripciondiseño3 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionDiseñoAcabado3"].Value);
                    int detalletipodiseño4 = Convert.ToInt32(datorecuperado.Cells["IdTipoDiseñoAcabado4"].Value);
                    int detalledescripciondiseño4 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionDiseñoAcabado4"].Value);

                    if (detalletipodiseño3 == codigotipodiseñoacabados3 && detalledescripciondiseño3 == codigodescripciondiseñoacabados3 && detalletipodiseño4 == codigotipodiseñoacabados4 && detalledescripciondiseño4 == codigodescripciondiseñoacabados4)
                    {
                        EstadoDiseñoAcabados2 = true;
                    }
                    else
                    {
                        EstadoDiseñoAcabados2 = false;
                    }

                    //VALIDACION DE N-TIPOS 1
                    int detalletipontipos1 = Convert.ToInt32(datorecuperado.Cells["IdTipoNTipos1"].Value);
                    int detalledescripcionntipos1 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionNTipos1"].Value);
                    int detalletipontipos2 = Convert.ToInt32(datorecuperado.Cells["IdTipoNTipos2"].Value);
                    int detalledescripcionntipos2 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionNTipos2"].Value);

                    if (detalletipontipos1 == codigotipontipos1 && detalledescripcionntipos1 == codigodescripcionntipos1 && detalletipontipos2 == codigotipontipos2 && detalledescripcionntipos2 == codigodescripcionntipos2)
                    {
                        EstadoNTipos1 = true;
                    }
                    else
                    {
                        EstadoNTipos1 = false;
                    }

                    //VALIDACION DE N-TIPOS 2
                    int detalletipontipos3 = Convert.ToInt32(datorecuperado.Cells["IdTipoNTipos3"].Value);
                    int detalledescripcionntipos3 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionNTipos3"].Value);
                    int detalletipontipos4 = Convert.ToInt32(datorecuperado.Cells["IdTipoNTipos4"].Value);
                    int detalledescripcionntipos4 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionNTipos4"].Value);

                    if (detalletipontipos3 == codigotipontipos3 && detalledescripcionntipos3 == codigodescripcionntipos3 && detalletipontipos4 == codigotipontipos4 && detalledescripcionntipos4 == codigodescripcionntipos4)
                    {
                        EstadoNTipos2 = true;
                    }
                    else
                    {
                        EstadoNTipos2 = false;
                    }

                    //VALIDACION DE VARIOS 01
                    int detalletipovarios01 = Convert.ToInt32(datorecuperado.Cells["IdTipoVarios01"].Value);
                    int detalledescripcionvarios01 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionVarios01"].Value);

                    if (detalletipovarios01 == codigotipovarios1 && detalledescripcionvarios01 == codigodescripcionvarios01)
                    {
                        EstadoVarios01 = true;
                    }
                    else
                    {
                        EstadoVarios01 = false;
                    }

                    //VALIDACION DE VARIOS 02
                    int detalletipovarios02 = Convert.ToInt32(datorecuperado.Cells["IdTipoVarios02"].Value);
                    int detalledescripcionvarios02 = Convert.ToInt32(datorecuperado.Cells["IdDescripcionVarios02"].Value);

                    if (detalletipovarios02 == codigotipovarios2 && detalledescripcionvarios02 == codigodescripcionvarios02)
                    {
                        EstadoVarios02 = true;
                    }
                    else
                    {
                        EstadoVarios02 = false;
                    }

                    //VALIDACION DE GENERAL
                    string detallegeneral = (string)datorecuperado.Cells["CampoGeneral"].Value;
                    if (detallegeneral == campogeneral)
                    {
                        EstadoGeneral = true;
                    }
                    else
                    {
                        EstadoGeneral = false;
                    }

                    if (EstadoCaracteristicas1 == true && EstadoCaracteristicas2 == true && EstadoMedidas1 == true && EstadoMedidas2 == true &&
        EstadoDiametros1 == true && EstadoDiametros2 == true && EstadoFormas1 == true && EstadoFormas2 == true &&
        EstadoEspesores1 == true && EstadoEspesores2 == true && EstadoDiseñoAcabados1 == true && EstadoDiseñoAcabados2 == true &&
        EstadoNTipos1 == true && EstadoNTipos2 == true && EstadoVarios01 == true && EstadoVarios02 == true &&
        EstadoGeneral == true)
                    {
                        return;
                    }
                }
            }
        }

        //VALIDAR TEXTO DE MI PRODUCTO EN MI SERVIDOR
        public void ValidacionCamposBusqueda()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("ValidacíonBusquedaProducto", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@detalle", txtDescripcionGeneradaProducto.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoNombreProducto.DataSource = dt;
            con.Close();

            if (datalistadoNombreProducto.RowCount > 0)
            {
                EstadoNombreProducto = true;
            }
            else
            {
                EstadoNombreProducto = false;
            }
        }

        //VALIDAR CAMPOS VACIOS
        public void ValidarCamposVacios()
        {
            //REINICIO DE LA VARIABLE GLOBAL
            EstadoValidacionCampoVacios = 0;

            //VALIDAR GURPO DE COMBO CARACTERISTICAS 1
            if (ckCaracteristicas1.Checked == true)
            {
                if (cboTipoCaracteristicas1.Text != "NO APLICA" && cboDescripcionCaracteristicas1.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
                if (cboTipoCaracteristicas2.Text != "NO APLICA" && cboDescripcionCaracteristicas2.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
            }
            //VALIDAR GURPO DE COMBO CARACTERISTICAS 2
            if (ckCaracteristicas2.Checked == true)
            {
                if (cboTipoCaracteristicas3.Text != "NO APLICA" && cboDescripcionCaracteristicas3.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
                if (cboTipoCaracteristicas4.Text != "NO APLICA" && cboDescripcionCaracteristicas4.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
            }
            //----------------------------------------------------------------------------------
            //VALIDAR GURPO DE COMBO MEDIDAS 1
            if (ckCamposMedida1.Checked == true)
            {
                if (cboTipoMedida1.Text != "NO APLICA" && cboDescripcionMedida1.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
                if (cboTipoMedida2.Text != "NO APLICA" && cboDescripcionMedida2.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
            }
            //VALIDAR GURPO DE COMBO MEDIDAS 2
            if (ckCamposMedida2.Checked == true)
            {
                if (cboTipoMedida3.Text != "NO APLICA" && cboDescripcionMedida3.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
                if (cboTipoMedida4.Text != "NO APLICA" && cboDescripcionMedida4.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
            }
            //----------------------------------------------------------------------------------
            //VALIDAR GURPO DE COMBO DIAMETROS 1
            if (ckCamposDiametros1.Checked == true)
            {
                if (cboTiposDiametros1.Text != "NO APLICA" && cboDescripcionDiametros1.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
                if (cboTiposDiametros2.Text != "NO APLICA" && cboDescripcionDiametros2.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
            }
            //VALIDAR GURPO DE COMBO DIAMETROS 2
            if (ckCamposDiametros2.Checked == true)
            {
                if (cboTiposDiametros3.Text != "NO APLICA" && cboDescripcionDiametros3.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
                if (cboTiposDiametros4.Text != "NO APLICA" && cboDescripcionDiametros4.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
            }
            //----------------------------------------------------------------------------------
            //VALIDAR GURPO DE COMBO FORMAS 1
            if (ckCamposFormas1.Checked == true)
            {
                if (cboTiposFormas1.Text != "NO APLICA" && cboDescripcionFormas1.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
                if (cboTiposFormas2.Text != "NO APLICA" && cboDescripcionFormas2.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
            }
            //VALIDAR GURPO DE COMBO FORMAS 2
            if (ckCamposFormas2.Checked == true)
            {
                if (cboTiposFormas3.Text != "NO APLICA" && cboDescripcionFormas3.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
                if (cboTiposFormas4.Text != "NO APLICA" && cboDescripcionFormas4.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
            }
            //----------------------------------------------------------------------------------
            //VALIDAR GURPO DE COMBO ESPESORES 1
            if (ckCamposEspesores1.Checked == true)
            {
                if (cbooTipoEspesores1.Text != "NO APLICA" && cboDescripcionEspesores1.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
                if (cbooTipoEspesores2.Text != "NO APLICA" && cboDescripcionEspesores2.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
            }
            //VALIDAR GURPO DE COMBO ESPESORES 2
            if (ckCamposEspesores2.Checked == true)
            {
                if (cbooTipoEspesores3.Text != "NO APLICA" && cboDescripcionEspesores3.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
                if (cbooTipoEspesores4.Text != "NO APLICA" && cboDescripcionEspesores4.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
            }
            //----------------------------------------------------------------------------------
            //VALIDAR GURPO DE COMBO DISEÑO Y ACABADOS 1
            if (ckCamposDiseñoAcabado1.Checked == true)
            {
                if (cboTiposDiseñosAcabados1.Text != "NO APLICA" && cboDescripcionDiseñoAcabado1.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
                if (cboTiposDiseñosAcabados2.Text != "NO APLICA" && cboDescripcionDiseñoAcabado2.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
            }
            //VALIDAR GURPO DE COMBO DISEÑO Y ACABADOS 2
            if (ckCamposDiseñoAcabado2.Checked == true)
            {
                if (cboTiposDiseñosAcabados3.Text != "NO APLICA" && cboDescripcionDiseñoAcabado3.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
                if (cboTiposDiseñosAcabados4.Text != "NO APLICA" && cboDescripcionDiseñoAcabado4.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
            }
            //----------------------------------------------------------------------------------
            //VALIDAR GURPO DE COMBO NUMEROS Y TIPOS 1
            if (ckCamposNTipos1.Checked == true)
            {
                if (cboTiposNTipos1.Text != "NO APLICA" && cboDescripcionNTipos1.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
                if (cboTiposNTipos2.Text != "NO APLICA" && cboDescripcionNTipos2.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
            }
            //VALIDAR GURPO DE COMBO NUMEROS Y TIPOS 2
            if (ckCamposNTipos2.Checked == true)
            {
                if (cboTiposNTipos3.Text != "NO APLICA" && cboDescripcionNTipos3.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
                if (cboTiposNTipos4.Text != "NO APLICA" && cboDescripcionNTipos4.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
            }
            //----------------------------------------------------------------------------------
            //VALIDAR GURPO DE COMBO VARIOS 1 
            if (ckVariosO1.Checked == true)
            {
                if (cboTiposVariosO1.Text != "NO APLICA" && cboDescripcionVariosO1.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
            }
            if (ckVariosO2.Checked == true)
            {
                if (cboTiposVariosO2.Text != "NO APLICA" && cboDescripcionVariosO2.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
            }
        }
        //FIN DEL SISTEMA DE VALIDACION--------------------------------------------------------------------------------------

        //AGREGAR NUEVAS DESCRIPCIONES A MI BD-------------------------------------------------------------
        //DESCRIPCIÓN CARACTERISTICAS - 1
        private void btnAgregarDescripcionCaracteristicas1_Click(object sender, EventArgs e)
        {
            if (cboTipoCaracteristicas1.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - CARACTERISTICAS 1";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTipoCaracteristicas1.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTipoCaracteristicas1.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN CARACTERISTICAS - 2
        private void btnAgregarDescripcionCaracteristicas2_Click(object sender, EventArgs e)
        {
            if (cboTipoCaracteristicas2.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - CARACTERISTICAS 2";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTipoCaracteristicas2.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTipoCaracteristicas2.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN CARACTERISTICAS - 3
        private void btnAgregarDescripcionCaracteristicas3_Click(object sender, EventArgs e)
        {
            if (cboTipoCaracteristicas3.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - CARACTERISTICAS 3";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTipoCaracteristicas3.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTipoCaracteristicas3.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN CARACTERISTICAS - 4
        private void btnAgregarDescripcionCaracteristicas4_Click(object sender, EventArgs e)
        {
            if (cboTipoCaracteristicas4.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - CARACTERISTICAS 4";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTipoCaracteristicas4.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTipoCaracteristicas4.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN MEDIDAS - 1 
        private void btnAgregarDescripcionMedidas1_Click(object sender, EventArgs e)
        {
            if (cboTipoMedida1.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - MEDIDAS 1";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTipoMedida1.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTipoMedida1.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN MEDIDAS - 2
        private void btnAgregarDescripcionMedidas2_Click(object sender, EventArgs e)
        {
            if (cboTipoMedida2.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - MEDIDAS 2";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTipoMedida2.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTipoMedida2.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN MEDIDAS - 3
        private void btnAgregarDescripcionMedidas3_Click(object sender, EventArgs e)
        {
            if (cboTipoMedida3.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - MEDIDAS 3";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTipoMedida3.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTipoMedida3.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN MEDIDAS - 4
        private void btnAgregarDescripcionMedidas4_Click(object sender, EventArgs e)
        {
            if (cboTipoMedida4.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - MEDIDAS 4";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTipoMedida4.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTipoMedida4.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN DIÁMETROS 1
        private void btnAgregarDescripcionDiametros1_Click(object sender, EventArgs e)
        {
            if (cboTiposDiametros1.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - DIAMETROS 1";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTiposDiametros1.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTiposDiametros1.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN DIÁMETROS 2
        private void btnAgregarDescripcionDiametros2_Click(object sender, EventArgs e)
        {
            if (cboTiposDiametros2.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - DIAMETROS 2";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTiposDiametros2.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTiposDiametros2.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN DIÁMETROS 3
        private void btnAgregarDescripcionDiametros3_Click(object sender, EventArgs e)
        {
            if (cboTiposDiametros3.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - DIAMETROS 3";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTiposDiametros3.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTiposDiametros3.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN DIÁMETROS 4
        private void btnAgregarDescripcionDiametros4_Click(object sender, EventArgs e)
        {
            if (cboTiposDiametros4.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - DIAMETROS 4";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTiposDiametros4.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTiposDiametros4.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN FORMAS 1
        private void btnAgregarDescripcionFormas1_Click(object sender, EventArgs e)
        {
            if (cboTiposFormas1.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - FORMAS 1";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTiposFormas1.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTiposFormas1.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN FORMAS 2
        private void btnAgregarDescripcionFormas2_Click(object sender, EventArgs e)
        {
            if (cboTiposFormas2.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - FORMAS 2";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTiposFormas2.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTiposFormas2.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN FORMAS 3
        private void btnAgregarDescripcionFormas3_Click(object sender, EventArgs e)
        {
            if (cboTiposFormas3.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - FORMAS 3";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTiposFormas3.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTiposFormas3.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN FORMAS 4
        private void btnAgregarDescripcionFormas4_Click(object sender, EventArgs e)
        {
            if (cboTiposFormas3.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - FORMAS 3";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTiposFormas3.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTiposFormas3.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN ESPESORES 1
        private void btnAgregarDescripcionEspesores1_Click(object sender, EventArgs e)
        {
            if (cbooTipoEspesores1.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - ESPESORES 1";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cbooTipoEspesores1.SelectedValue.ToString();
                txtTipoOngreso.Text = cbooTipoEspesores1.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN ESPESORES 2
        private void btnAgregarDescripcionEspesores2_Click(object sender, EventArgs e)
        {
            if (cbooTipoEspesores2.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - ESPESORES 2";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cbooTipoEspesores2.SelectedValue.ToString();
                txtTipoOngreso.Text = cbooTipoEspesores2.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN ESPESORES 3
        private void btnAgregarDescripcionEspesores3_Click(object sender, EventArgs e)
        {
            if (cbooTipoEspesores3.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - ESPESORES 3";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cbooTipoEspesores3.SelectedValue.ToString();
                txtTipoOngreso.Text = cbooTipoEspesores3.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN ESPESORES 4
        private void btnAgregarDescripcionEspesores4_Click(object sender, EventArgs e)
        {
            if (cbooTipoEspesores4.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - ESPESORES 4";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cbooTipoEspesores4.SelectedValue.ToString();
                txtTipoOngreso.Text = cbooTipoEspesores4.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN DISEÑO ACABADO 1
        private void btnAgregarDescripcionDiseñoAcabado1_Click(object sender, EventArgs e)
        {
            if (cboTiposDiseñosAcabados1.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - DISEÑO 1";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTiposDiseñosAcabados1.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTiposDiseñosAcabados1.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN DISEÑO ACABADO 2
        private void btnAgregarDescripcionDiseñoAcabado2_Click(object sender, EventArgs e)
        {
            if (cboTiposDiseñosAcabados2.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - DISEÑO 2";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTiposDiseñosAcabados2.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTiposDiseñosAcabados2.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN DISEÑO ACABADO 3
        private void btnAgregarDescripcionDiseñoAcabado3_Click(object sender, EventArgs e)
        {
            if (cboTiposDiseñosAcabados3.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - DISEÑO 3";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTiposDiseñosAcabados3.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTiposDiseñosAcabados3.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN DISEÑO ACABADO 4
        private void btnAgregarDescripcionDiseñoAcabado4_Click(object sender, EventArgs e)
        {
            if (cboTiposDiseñosAcabados4.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - DISEÑO 4";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTiposDiseñosAcabados4.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTiposDiseñosAcabados4.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN NTIPOS 1
        private void btnAgregarDescripcionNTipos1_Click(object sender, EventArgs e)
        {
            if (cboTiposNTipos1.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - NUM. Y TIPOS 1";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTiposNTipos1.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTiposNTipos1.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN NTIPOS 2
        private void btnAgregarDescripcionNTipos2_Click(object sender, EventArgs e)
        {
            if (cboTiposNTipos2.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - NUM. Y TIPOS 2";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTiposNTipos2.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTiposNTipos2.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN NTIPOS 3
        private void btnAgregarDescripcionNTipos3_Click(object sender, EventArgs e)
        {
            if (cboTiposNTipos3.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - NUM. Y TIPOS 3";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTiposNTipos3.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTiposNTipos3.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN NTIPOS 4
        private void btnAgregarDescripcionNTipos4_Click(object sender, EventArgs e)
        {
            if (cboTiposNTipos4.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - NUM. Y TIPOS 4";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTiposNTipos4.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTiposNTipos4.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN VARIOS0 1
        private void btnAgregarDescripcionVarios01_Click(object sender, EventArgs e)
        {
            if (cboTiposVariosO1.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - VARIOS Y 0 1";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTiposVariosO1.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTiposVariosO1.Text;
                txtValorIngreso.Focus();
            }
        }

        //DESCRIPCIÓN VARIOS0 2
        private void btnAgregarDescripcionVarios02_Click(object sender, EventArgs e)
        {
            if (cboTiposVariosO2.Text == "NO APLICA")
            {
                MessageBox.Show("No se puede agregar datos a este campo ya que no aplica para este modelo de producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                panelNuevoValores.Visible = true;
                lblTituloAdaptable.Text = "INGRESO DE NUEVOS DATOS - VARIOS Y 0 2";
                lblCodigoModelo.Text = cboModelos.SelectedValue.ToString();
                txtModeloIngreso.Text = cboModelos.Text;
                lblCodigoTipoIngreso.Text = cboTiposVariosO2.SelectedValue.ToString();
                txtTipoOngreso.Text = cboTiposVariosO2.Text;
                txtValorIngreso.Focus();
            }
        }
        //FIN------------------------------------------------------------------------------------------

        //PARTE GENERAL DEL INGRESO DE NUEVAS DESCIPCIONES-----------------------------------------------
        //GAURDAR NUEVA DESCIPCIÓN
        private void btnIngresarNuevosValores_Click(object sender, EventArgs e)
        {
            IngresarNuevoDato(txtTipoOngreso.Text, lblTituloAdaptable.Text, lblCodigoTipoIngreso.Text, Convert.ToInt32(lblCodigoModelo.Text), txtValorIngreso.Text, cboModelos.Text
                               , cboTipoCaracteristicas1, cboTipoCaracteristicas2, cboTipoCaracteristicas3, cboTipoCaracteristicas4, cboDescripcionCaracteristicas1, cboDescripcionCaracteristicas2
                               , cboDescripcionCaracteristicas3, cboDescripcionCaracteristicas4, cboTipoMedida1, cboTipoMedida2, cboTipoMedida3, cboTipoMedida4, cboDescripcionMedida1, cboDescripcionMedida2
                               , cboDescripcionMedida3, cboDescripcionMedida4, cboTiposDiametros1, cboTiposDiametros2, cboTiposDiametros3, cboTiposDiametros4, cboDescripcionDiametros1, cboDescripcionDiametros2
                               , cboDescripcionDiametros3, cboDescripcionDiametros4, cboTiposFormas1, cboTiposFormas2, cboTiposFormas3, cboTiposFormas4, cboDescripcionFormas1, cboDescripcionFormas2, cboDescripcionFormas3
                               , cboDescripcionFormas4, cbooTipoEspesores1, cbooTipoEspesores2, cbooTipoEspesores3, cbooTipoEspesores4, cboDescripcionEspesores1, cboDescripcionEspesores2, cboDescripcionEspesores3
                               , cboDescripcionEspesores4, cboTiposDiseñosAcabados1, cboTiposDiseñosAcabados2, cboTiposDiseñosAcabados3, cboTiposDiseñosAcabados4, cboDescripcionDiseñoAcabado1, cboDescripcionDiseñoAcabado2
                               , cboDescripcionDiseñoAcabado3, cboDescripcionDiseñoAcabado4, cboTiposNTipos1, cboTiposNTipos2, cboTiposNTipos3, cboTiposNTipos4, cboDescripcionNTipos1, cboDescripcionNTipos2, cboDescripcionNTipos3
                               , cboDescripcionNTipos4, cboTiposVariosO1, cboTiposVariosO2, cboDescripcionVariosO1, cboDescripcionVariosO2, datalistadoCamposPredeterminadosDetalle, panelNuevoValores);
            txtValorIngreso.Text = "";
        }

        //CONFIRMAR INGRESO DEL DATO
        private void txtValorIngreso_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                IngresarNuevoDato(txtTipoOngreso.Text, lblTituloAdaptable.Text, lblCodigoTipoIngreso.Text, Convert.ToInt32(lblCodigoModelo.Text), txtValorIngreso.Text, cboModelos.Text
                    , cboTipoCaracteristicas1, cboTipoCaracteristicas2, cboTipoCaracteristicas3, cboTipoCaracteristicas4, cboDescripcionCaracteristicas1, cboDescripcionCaracteristicas2
                    , cboDescripcionCaracteristicas3, cboDescripcionCaracteristicas4, cboTipoMedida1, cboTipoMedida2, cboTipoMedida3, cboTipoMedida4, cboDescripcionMedida1, cboDescripcionMedida2
                    , cboDescripcionMedida3, cboDescripcionMedida4, cboTiposDiametros1, cboTiposDiametros2, cboTiposDiametros3, cboTiposDiametros4, cboDescripcionDiametros1, cboDescripcionDiametros2
                    , cboDescripcionDiametros3, cboDescripcionDiametros4, cboTiposFormas1, cboTiposFormas2, cboTiposFormas3, cboTiposFormas4, cboDescripcionFormas1, cboDescripcionFormas2, cboDescripcionFormas3
                    , cboDescripcionFormas4, cbooTipoEspesores1, cbooTipoEspesores2, cbooTipoEspesores3, cbooTipoEspesores4, cboDescripcionEspesores1, cboDescripcionEspesores2, cboDescripcionEspesores3
                    , cboDescripcionEspesores4, cboTiposDiseñosAcabados1, cboTiposDiseñosAcabados2, cboTiposDiseñosAcabados3, cboTiposDiseñosAcabados4, cboDescripcionDiseñoAcabado1, cboDescripcionDiseñoAcabado2
                    , cboDescripcionDiseñoAcabado3, cboDescripcionDiseñoAcabado4, cboTiposNTipos1, cboTiposNTipos2, cboTiposNTipos3, cboTiposNTipos4, cboDescripcionNTipos1, cboDescripcionNTipos2, cboDescripcionNTipos3
                    , cboDescripcionNTipos4, cboTiposVariosO1, cboTiposVariosO2, cboDescripcionVariosO1, cboDescripcionVariosO2, datalistadoCamposPredeterminadosDetalle, panelNuevoValores);
            }
        }

        //FUNCION PARA INGRESAR NUEVOS DATOS
        public void IngresarNuevoDato(string tipoOngreso, string tituloadaptable, string codigotipoingreso, int codigomodelo, string valoringreso, string modelos, ComboBox TipCaracteristica1
            , ComboBox TipCaracteristica2, ComboBox TipCaracteristica3, ComboBox TipCaracteristica4, ComboBox DesCaracteristicas1, ComboBox DesCaracteristicas2, ComboBox DesCaracteristicas3
            , ComboBox DesCaracteristicas4, ComboBox TipMedidas1, ComboBox TipMedidas2, ComboBox TipMedidas3, ComboBox TipMedidas4, ComboBox DesMedidas1, ComboBox DesMedidas2, ComboBox DesMedidas3
            , ComboBox DesMedidas4, ComboBox TipDiametros1, ComboBox TipDiametros2, ComboBox TipDiametros3, ComboBox TipDiametros4, ComboBox DesDiametros1, ComboBox DesDiametros2, ComboBox DesDiametros3
            , ComboBox DesDiametros4, ComboBox TipFormas1, ComboBox TipFormas2, ComboBox TipFormas3, ComboBox TipFormas4, ComboBox DesFormas1, ComboBox DesFormas2, ComboBox DesFormas3, ComboBox DesFormas4
            , ComboBox TipEspesores1, ComboBox TipEspesores2, ComboBox TipEspesores3, ComboBox TipEspesores4, ComboBox DesEspesores1, ComboBox DesEspesores2, ComboBox DesEspesores3, ComboBox DesEspesores4
            , ComboBox TipDiseñoAcabado1, ComboBox TipDiseñoAcabado2, ComboBox TipDiseñoAcabado3, ComboBox TipDiseñoAcabado4, ComboBox DesDiseñoAcabado1, ComboBox DesDiseñoAcabado2, ComboBox DesDiseñoAcabado3
            , ComboBox DesDiseñoAcabado4, ComboBox TipNtipos1, ComboBox TipNtipos2, ComboBox TipNtipos3, ComboBox TipNtipos4, ComboBox DesNtipos1, ComboBox DesNtipos2, ComboBox DesNtipos3, ComboBox DesNtipos4
            , ComboBox TipVariosO1, ComboBox TipVariosO2, ComboBox DesVariosO1, ComboBox DesVariosO2, DataGridView DGV, Panel PNuevoValores)
        {
            if (tipoOngreso != "NO APLICA")
            {
                DialogResult boton = MessageBox.Show("Esta por guardar este dato.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        if (tituloadaptable == "INGRESO DE NUEVOS DATOS - CARACTERISTICAS 1" || tituloadaptable == "INGRESO DE NUEVOS DATOS - CARACTERISTICAS 2" || tituloadaptable == "INGRESO DE NUEVOS DATOS - CARACTERISTICAS 3" || tituloadaptable == "INGRESO DE NUEVOS DATOS - CARACTERISTICAS 4")
                        {
                            SqlConnection conp = new SqlConnection();
                            conp.ConnectionString = Conexion.ConexionMaestra.conexion;
                            conp.Open();
                            SqlCommand cmdp = new SqlCommand();
                            cmdp = new SqlCommand("AgregarProducto_InsertarDescripcionCaracteristicas", conp);
                            cmdp.CommandType = CommandType.StoredProcedure;

                            cmdp.Parameters.AddWithValue("@tipo", codigotipoingreso);
                            cmdp.Parameters.AddWithValue("@idmodelo", codigomodelo);
                            cmdp.Parameters.AddWithValue("@descripcion", valoringreso);

                            //PRODUCTOS QUIMICOS - ADHESIVOS
                            if (modelos == "ADHESIVOS" && valoringreso == "COMPONENTES")
                            {
                                cmdp.Parameters.AddWithValue("@idTipoNN", DesCaracteristicas1.SelectedValue);
                                cmdp.Parameters.AddWithValue("@idDescripcionTipoNN", "CAMPO 'COMPONENTES' DEPENDIENTE DEL CAMPO 'SISTEMA'");
                            }
                            //PRODUCTOS QUIMICOS - FLOCULANTES
                            else if (modelos == "FLOCULANTES" && tipoOngreso == "PROVEEDOR")
                            {
                                cmdp.Parameters.AddWithValue("@idTipoNN", DesCaracteristicas1.SelectedValue);
                                cmdp.Parameters.AddWithValue("@idDescripcionTipoNN", "CAMPO 'PROVEEDOR' DEPENDIENTE DEL CAMPO 'ELEMENTO'");
                            }
                            //PRODUCTOS QUIMICOS - POLIURETANO Y COMPONETES
                            else if (modelos == "POLIURETANO Y COMPONENTES" && tipoOngreso == "COMPONENTES" || modelos == "POLIURETANO Y COMPONENTES" && tipoOngreso == "PROVEEDOR")
                            {
                                cmdp.Parameters.AddWithValue("@idTipoNN", DesCaracteristicas1.SelectedValue);
                                cmdp.Parameters.AddWithValue("@idDescripcionTipoNN", "CAMPO 'COMPONENTES' Y CAMPO 'PROVEEDOR' DEPENDIENTE DEL CAMPO 'SISTEMA'");
                            }
                            //SI NO ES UN CAMPO DEPENDEINTE
                            else
                            {
                                cmdp.Parameters.AddWithValue("@idTipoNN", 0);
                                cmdp.Parameters.AddWithValue("@idDescripcionTipoNN", "GENERAL");
                            }

                            cmdp.ExecuteNonQuery();
                            conp.Close();

                            CargarTiposCaracteriticas(TipCaracteristica1);
                            CargarTiposCaracteriticas(TipCaracteristica2);
                            CargarTiposCaracteriticas(TipCaracteristica3);
                            CargarTiposCaracteriticas(TipCaracteristica4);

                            TipCaracteristica1.SelectedValue = DGV.SelectedCells[1].Value;
                            DesCaracteristicas1.SelectedIndex = -1;
                            TipCaracteristica2.SelectedValue = DGV.SelectedCells[2].Value;
                            DesCaracteristicas2.SelectedIndex = -1;
                            TipCaracteristica3.SelectedValue = DGV.SelectedCells[3].Value;
                            DesCaracteristicas3.SelectedIndex = -1;
                            TipCaracteristica4.SelectedValue = DGV.SelectedCells[4].Value;
                            DesCaracteristicas1.SelectedIndex = -1;
                        }
                        else if (tituloadaptable == "INGRESO DE NUEVOS DATOS - MEDIDAS 1" || tituloadaptable == "INGRESO DE NUEVOS DATOS - MEDIDAS 2" || tituloadaptable == "INGRESO DE NUEVOS DATOS - MEDIDAS 3" || tituloadaptable == "INGRESO DE NUEVOS DATOS - MEDIDAS 4")
                        {
                            SqlConnection conp = new SqlConnection();
                            conp.ConnectionString = Conexion.ConexionMaestra.conexion;
                            conp.Open();
                            SqlCommand cmdp = new SqlCommand();
                            cmdp = new SqlCommand("AgregarProducto_InsertarDescripcionMedidas", conp);
                            cmdp.CommandType = CommandType.StoredProcedure;

                            cmdp.Parameters.AddWithValue("@tipo", codigotipoingreso);
                            cmdp.Parameters.AddWithValue("@idmodelo", codigomodelo);
                            cmdp.Parameters.AddWithValue("@descripcion", valoringreso);

                            cmdp.ExecuteNonQuery();
                            conp.Close();

                            CargarTiposMedidas(TipMedidas1);
                            CargarTiposMedidas(TipMedidas2);
                            CargarTiposMedidas(TipMedidas3);
                            CargarTiposMedidas(TipMedidas4);

                            TipMedidas1.SelectedValue = DGV.SelectedCells[5].Value;
                            DesMedidas1.SelectedIndex = -1;
                            TipMedidas2.SelectedValue = DGV.SelectedCells[6].Value;
                            DesMedidas2.SelectedIndex = -1;
                            TipMedidas3.SelectedValue = DGV.SelectedCells[7].Value;
                            DesMedidas3.SelectedIndex = -1;
                            TipMedidas4.SelectedValue = DGV.SelectedCells[8].Value;
                            DesMedidas4.SelectedIndex = -1;
                        }
                        else if (tituloadaptable == "INGRESO DE NUEVOS DATOS - DIAMETROS 1" || tituloadaptable == "INGRESO DE NUEVOS DATOS - DIAMETROS 2" || tituloadaptable == "INGRESO DE NUEVOS DATOS - DIAMETROS 3" || tituloadaptable == "INGRESO DE NUEVOS DATOS - DIAMETROS 4")
                        {
                            SqlConnection conp = new SqlConnection();
                            conp.ConnectionString = Conexion.ConexionMaestra.conexion;
                            conp.Open();
                            SqlCommand cmdp = new SqlCommand();
                            cmdp = new SqlCommand("AgregarProducto_InsertarDescripcionDiametros", conp);
                            cmdp.CommandType = CommandType.StoredProcedure;

                            cmdp.Parameters.AddWithValue("@tipo", codigotipoingreso);
                            cmdp.Parameters.AddWithValue("@idmodelo", codigomodelo);
                            cmdp.Parameters.AddWithValue("@descripcion", valoringreso);

                            cmdp.ExecuteNonQuery();
                            conp.Close();

                            CargarTiposDiametros(TipDiametros1);
                            CargarTiposDiametros(TipDiametros2);
                            CargarTiposDiametros(TipDiametros3);
                            CargarTiposDiametros(TipDiametros4);

                            TipDiametros1.SelectedValue = DGV.SelectedCells[9].Value;
                            DesDiametros1.SelectedIndex = -1;
                            TipDiametros2.SelectedValue = DGV.SelectedCells[10].Value;
                            DesDiametros2.SelectedIndex = -1;
                            TipDiametros3.SelectedValue = DGV.SelectedCells[11].Value;
                            DesDiametros3.SelectedIndex = -1;
                            TipDiametros4.SelectedValue = DGV.SelectedCells[12].Value;
                            DesDiametros4.SelectedIndex = -1;
                        }
                        else if (tituloadaptable == "INGRESO DE NUEVOS DATOS - FORMAS 1" || tituloadaptable == "INGRESO DE NUEVOS DATOS - FORMAS 2" || tituloadaptable == "INGRESO DE NUEVOS DATOS - FORMAS 3" || tituloadaptable == "INGRESO DE NUEVOS DATOS - FORMAS 4")
                        {
                            SqlConnection conp = new SqlConnection();
                            conp.ConnectionString = Conexion.ConexionMaestra.conexion;
                            conp.Open();
                            SqlCommand cmdp = new SqlCommand();
                            cmdp = new SqlCommand("AgregarProducto_InsertarDescripcionFormas", conp);
                            cmdp.CommandType = CommandType.StoredProcedure;

                            cmdp.Parameters.AddWithValue("@tipo", codigotipoingreso);
                            cmdp.Parameters.AddWithValue("@idmodelo", codigomodelo);
                            cmdp.Parameters.AddWithValue("@descripcion", valoringreso);

                            cmdp.ExecuteNonQuery();
                            conp.Close();

                            CargarTiposFormas(TipFormas1);
                            CargarTiposFormas(TipFormas2);
                            CargarTiposFormas(TipFormas3);
                            CargarTiposFormas(TipFormas4);

                            TipFormas1.SelectedValue = DGV.SelectedCells[13].Value;
                            DesFormas1.SelectedIndex = -1;
                            TipFormas2.SelectedValue = DGV.SelectedCells[14].Value;
                            DesFormas2.SelectedIndex = -1;
                            TipFormas3.SelectedValue = DGV.SelectedCells[15].Value;
                            DesFormas3.SelectedIndex = -1;
                            TipFormas4.SelectedValue = DGV.SelectedCells[16].Value;
                            DesFormas4.SelectedIndex = -1;
                        }
                        else if (tituloadaptable == "INGRESO DE NUEVOS DATOS - ESPESORES 1" || tituloadaptable == "INGRESO DE NUEVOS DATOS - ESPESORES 2" || tituloadaptable == "INGRESO DE NUEVOS DATOS - ESPESORES 3" || tituloadaptable == "INGRESO DE NUEVOS DATOS - ESPESORES 4")
                        {
                            SqlConnection conp = new SqlConnection();
                            conp.ConnectionString = Conexion.ConexionMaestra.conexion;
                            conp.Open();
                            SqlCommand cmdp = new SqlCommand();
                            cmdp = new SqlCommand("AgregarProducto_InsertarDescripcionEspesores", conp);
                            cmdp.CommandType = CommandType.StoredProcedure;

                            cmdp.Parameters.AddWithValue("@tipo", codigotipoingreso);
                            cmdp.Parameters.AddWithValue("@idmodelo", codigomodelo);
                            cmdp.Parameters.AddWithValue("@descripcion", valoringreso);

                            cmdp.ExecuteNonQuery();
                            conp.Close();

                            CargarTiposEspesores(TipEspesores1);
                            CargarTiposEspesores(TipEspesores2);
                            CargarTiposEspesores(TipEspesores3);
                            CargarTiposEspesores(TipEspesores4);

                            TipEspesores1.SelectedValue = DGV.SelectedCells[17].Value;
                            DesEspesores1.SelectedIndex = -1;
                            TipEspesores2.SelectedValue = DGV.SelectedCells[18].Value;
                            DesEspesores2.SelectedIndex = -1;
                            TipEspesores3.SelectedValue = DGV.SelectedCells[19].Value;
                            DesEspesores3.SelectedIndex = -1;
                            TipEspesores4.SelectedValue = DGV.SelectedCells[20].Value;
                            DesEspesores4.SelectedIndex = -1;
                        }
                        else if (tituloadaptable == "INGRESO DE NUEVOS DATOS - DISEÑO 1" || tituloadaptable == "INGRESO DE NUEVOS DATOS - DISEÑO 2" || tituloadaptable == "INGRESO DE NUEVOS DATOS - DISEÑO 3" || tituloadaptable == "INGRESO DE NUEVOS DATOS - DISEÑO 4")
                        {
                            SqlConnection conp = new SqlConnection();
                            conp.ConnectionString = Conexion.ConexionMaestra.conexion;
                            conp.Open();
                            SqlCommand cmdp = new SqlCommand();
                            cmdp = new SqlCommand("AgregarProducto_InsertarDescripcionDiseño", conp);
                            cmdp.CommandType = CommandType.StoredProcedure;

                            cmdp.Parameters.AddWithValue("@tipo", codigotipoingreso);
                            cmdp.Parameters.AddWithValue("@idmodelo", codigomodelo);
                            cmdp.Parameters.AddWithValue("@descripcion", valoringreso);

                            //PANELES POLIURETANO - CIEGO
                            if (modelos == "CIEGO" && tipoOngreso == "DUREZA" || modelos == "CONVENCIONAL" && tipoOngreso == "DUREZA" || modelos == "AUTOLIMPIANTE" && tipoOngreso == "DUREZA" || modelos == "VIBROHEXAGONAL" && tipoOngreso == "DUREZA" || modelos == "TEEPEE" && tipoOngreso == "DUREZA" || modelos == "OBLONGA" && tipoOngreso == "DUREZA")
                            {
                                cmdp.Parameters.AddWithValue("@idTipoNN", DesFormas1.SelectedValue);
                                cmdp.Parameters.AddWithValue("@idDescripcionTipoNN", "CAMPO 'DUREZA' DEPENDIENTE DEL CAMPO 'FORMA ESPECÍFICA'");
                                if (DesFormas1.SelectedValue == null) { MessageBox.Show("Debe seleccionar una forma específica para pdoer definir una dureza", "D¿Validación del Sistema"); return; }
                            }
                            //SI NO ES UN CAMPO DEPENDIENTE
                            else
                            {
                                cmdp.Parameters.AddWithValue("@idTipoNN", 0);
                                cmdp.Parameters.AddWithValue("@idDescripcionTipoNN", "GENERAL");
                            }

                            cmdp.ExecuteNonQuery();
                            conp.Close();

                            CargarTiposDiseñoAcabado(TipDiseñoAcabado1);
                            CargarTiposDiseñoAcabado(TipDiseñoAcabado2);
                            CargarTiposDiseñoAcabado(TipDiseñoAcabado3);
                            CargarTiposDiseñoAcabado(TipDiseñoAcabado4);

                            TipDiseñoAcabado1.SelectedValue = DGV.SelectedCells[21].Value;
                            DesDiseñoAcabado1.SelectedIndex = -1;
                            TipDiseñoAcabado2.SelectedValue = DGV.SelectedCells[22].Value;
                            DesDiseñoAcabado2.SelectedIndex = -1;
                            TipDiseñoAcabado3.SelectedValue = DGV.SelectedCells[23].Value;
                            DesDiseñoAcabado3.SelectedIndex = -1;
                            TipDiseñoAcabado4.SelectedValue = DGV.SelectedCells[24].Value;
                            DesDiseñoAcabado4.SelectedIndex = -1;
                        }
                        else if (tituloadaptable == "INGRESO DE NUEVOS DATOS - NUM. Y TIPOS 1" || tituloadaptable == "INGRESO DE NUEVOS DATOS - NUM. Y TIPOS 2" || tituloadaptable == "INGRESO DE NUEVOS DATOS - NUM. Y TIPOS 3" || tituloadaptable == "INGRESO DE NUEVOS DATOS - NUM. Y TIPOS 4")
                        {
                            SqlConnection conp = new SqlConnection();
                            conp.ConnectionString = Conexion.ConexionMaestra.conexion;
                            conp.Open();
                            SqlCommand cmdp = new SqlCommand();
                            cmdp = new SqlCommand("AgregarProducto_InsertarDescripcionNTipos", conp);
                            cmdp.CommandType = CommandType.StoredProcedure;

                            cmdp.Parameters.AddWithValue("@tipo", codigotipoingreso);
                            cmdp.Parameters.AddWithValue("@idmodelo", codigomodelo);
                            cmdp.Parameters.AddWithValue("@descripcion", valoringreso);

                            //PRODUCTOS QUIMICOS - ANTIESPUMANTE
                            if (modelos == "ANTIESPUMANTE" && tipoOngreso == "TIPO DE CARGA")
                            {
                                cmdp.Parameters.AddWithValue("@idTipoNN", DesCaracteristicas1.SelectedValue);
                                cmdp.Parameters.AddWithValue("@idDescripcionTipoNN", "CAMPO 'TIPO DE CARGA' DEPENDIENTE DEL CAMPO 'PROVEEDOR'");
                            }
                            //PRODUCTOS QUIMICOS - SUPRESOR DE POLVO
                            else if (modelos == "SUPRESOR DE POLVO" && tipoOngreso == "TIPO DE CARGA")
                            {
                                cmdp.Parameters.AddWithValue("@idTipoNN", DesCaracteristicas1.SelectedValue);
                                cmdp.Parameters.AddWithValue("@idDescripcionTipoNN", "CAMPO 'TIPO DE CARGA' DEPENDIENTE DEL CAMPO 'PROVEEDOR'");
                            }
                            //PRODUCTOS QUIMICOS - SUPRESOR DE POLVO
                            else if (modelos == "SECUESTRANTE" && tipoOngreso == "TIPO DE CARGA")
                            {
                                cmdp.Parameters.AddWithValue("@idTipoNN", DesCaracteristicas1.SelectedValue);
                                cmdp.Parameters.AddWithValue("@idDescripcionTipoNN", "CAMPO 'TIPO DE CARGA' DEPENDIENTE DEL CAMPO 'PROVEEDOR'");
                            }
                            //SI NO ES UN CAMPO DEPENDEINTE
                            else
                            {
                                cmdp.Parameters.AddWithValue("@idTipoNN", 0);
                                cmdp.Parameters.AddWithValue("@idDescripcionTipoNN", "GENERAL");
                            }

                            cmdp.ExecuteNonQuery();
                            conp.Close();

                            CargarTiposNTipos(TipNtipos1);
                            CargarTiposNTipos(TipNtipos2);
                            CargarTiposNTipos(TipNtipos3);
                            CargarTiposNTipos(TipNtipos4);

                            TipNtipos1.SelectedValue = DGV.SelectedCells[25].Value;
                            DesNtipos1.SelectedIndex = -1;
                            TipNtipos2.SelectedValue = DGV.SelectedCells[26].Value;
                            DesNtipos2.SelectedIndex = -1;
                            TipNtipos3.SelectedValue = DGV.SelectedCells[27].Value;
                            DesNtipos3.SelectedIndex = -1;
                            TipNtipos4.SelectedValue = DGV.SelectedCells[28].Value;
                            DesNtipos4.SelectedIndex = -1;
                        }
                        else if (tituloadaptable == "INGRESO DE NUEVOS DATOS - VARIOS Y 0 1" || tituloadaptable == "INGRESO DE NUEVOS DATOS - VARIOS Y 0 2")
                        {
                            SqlConnection conp = new SqlConnection();
                            conp.ConnectionString = Conexion.ConexionMaestra.conexion;
                            conp.Open();
                            SqlCommand cmdp = new SqlCommand();
                            cmdp = new SqlCommand("AgregarProducto_InsertarDescripcionVariosO", conp);
                            cmdp.CommandType = CommandType.StoredProcedure;

                            cmdp.Parameters.AddWithValue("@tipo", codigotipoingreso);
                            cmdp.Parameters.AddWithValue("@idmodelo", codigomodelo);
                            cmdp.Parameters.AddWithValue("@descripcion", valoringreso);

                            //PRODUCTOS QUIMICOS - COAGULANTES
                            if (modelos == "COAGULANTES" && tipoOngreso == "CODIGO-ARENAS")
                            {
                                cmdp.Parameters.AddWithValue("@idTipoNN", DesCaracteristicas1.SelectedValue);
                                cmdp.Parameters.AddWithValue("@idDescripcionTipoNN", "CAMPO 'CODIGO-ARENAS' DEPENDIENTE DEL CAMPO 'PROVEEDOR'");
                            }
                            //PRODUCTOS QUIMICOS - ANTIESPUMANTE
                            else if (modelos == "ANTIESPUMANTE" && tipoOngreso == "CODIGO-ARENAS")
                            {
                                cmdp.Parameters.AddWithValue("@idTipoNN", DesCaracteristicas1.SelectedValue);
                                cmdp.Parameters.AddWithValue("@idDescripcionTipoNN", "CAMPO 'CODIGO-ARENAS' DEPENDIENTE DEL CAMPO 'PROVEEDOR'");
                            }
                            //PRODUCTOS QUIMICOS - SUPRESOR DE POLVO
                            else if (modelos == "SUPRESOR DE POLVO" && tipoOngreso == "CODIGO-ARENAS")
                            {
                                cmdp.Parameters.AddWithValue("@idTipoNN", DesCaracteristicas1.SelectedValue);
                                cmdp.Parameters.AddWithValue("@idDescripcionTipoNN", "CAMPO 'CODIGO-ARENAS' DEPENDIENTE DEL CAMPO 'PROVEEDOR'");
                            }
                            //PRODUCTOS QUIMICOS - SECUESTRANTE
                            else if (modelos == "SECUESTRANTE" && tipoOngreso == "CODIGO-ARENAS")
                            {
                                cmdp.Parameters.AddWithValue("@idTipoNN", DesCaracteristicas1.SelectedValue);
                                cmdp.Parameters.AddWithValue("@idDescripcionTipoNN", "CAMPO 'CODIGO-ARENAS' DEPENDIENTE DEL CAMPO 'PROVEEDOR'");
                            }
                            //SI NO HAY CAMPOS DEPENDIENTES
                            else
                            {
                                cmdp.Parameters.AddWithValue("@idTipoNN", 0);
                                cmdp.Parameters.AddWithValue("@idDescripcionTipoNN", "GENERAL");
                            }

                            cmdp.ExecuteNonQuery();
                            conp.Close();

                            CargarTiposVariosO(TipVariosO1);
                            CargarTiposVariosO(TipVariosO2);

                            TipVariosO1.SelectedValue = DGV.SelectedCells[29].Value;
                            DesVariosO1.SelectedIndex = -1;
                            TipVariosO2.SelectedValue = DGV.SelectedCells[30].Value;
                            DesVariosO2.SelectedIndex = -1;
                        }

                        MessageBox.Show("Dato ingresado correctamente.", "Validación del Sistema", MessageBoxButtons.OK);
                        valoringreso = "";
                        PNuevoValores.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocurrió un problema: " + ex.Message, "Validación del Sistema");
                    }
                }
            }
            else
            {
                MessageBox.Show("No se puede ingresar a este tipo de dato.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //CANCELAR EL NUEVO INGRESO DE LA DESCIPCIÓN
        private void btnCancelarNuevosValores_Click(object sender, EventArgs e)
        {
            txtValorIngreso.Text = "";
            panelNuevoValores.Visible = false;
        }

        //--------------------------------------------------------------------------------
        //DEFINICION DE MODELOS, CARACTERISTICAS UNICAS POR MODELO
        public void DefinicionModelosAtributos()
        {
            //PRIMERA CUENTA------------------------------------------------------
            //MAQUINARIAS Y EQUIPOS DE EXPLORACION
            if (cboModelos.Text == "MAQUINARIA Y EQUIPOS PARA MODULO CERAMICO")
            {
                nombreInicial = "";
                txtDescripcionGeneradaProducto.Text = "";
                txtDescripcionGeneral.Text = "";
            }
            //SEGUNDA CUENTA------------------------------------------------------
            //ABRASIVOS

            //--------
            //METALES
            else if (cboModelos.Text == "ALUMINIO")
            {
                nombreInicial = "";
                txtDescripcionGeneradaProducto.Text = "";
                txtDescripcionGeneral.Text = "";
            }
            //------
            //ACEROS
            else if (cboModelos.Text == "ALAMBRE")
            {
                nombreInicial = "";
                txtDescripcionGeneradaProducto.Text = "";
                txtDescripcionGeneral.Text = "";
            }
            else if (cboModelos.Text == "CABLE")
            {
                nombreInicial = "CABLE DE ACERO";
                txtDescripcionGeneradaProducto.Text = "CABLE DE ACERO";
                txtDescripcionGeneral.Text = "CABLE DE ACERO";
            }
            //------
            //CERAMICA

            //------
            //ELASTOMEROS

            //------
            //ESTRUCTURAS METALICAS
            else if (cboModelos.Text == "CONO")
            {
                nombreInicial = "CONO";
                txtDescripcionGeneradaProducto.Text = "CONO";
                txtDescripcionGeneral.Text = "CONO";
            }
            else if (cboModelos.Text == "DE TROMMEL")
            {
                nombreInicial = "ESTRUCTURA DE TROMMEL";
                txtDescripcionGeneradaProducto.Text = "ESTRUCTURA DE TROMMEL";
                txtDescripcionGeneral.Text = "ESTRUCTURA DE TROMMEL";
            }
            else if (cboModelos.Text == "PARA MALLA TRENSABLE")
            {
                nombreInicial = "ESTRUCTURA PARA MALLA TRENSABLE";
                txtDescripcionGeneradaProducto.Text = "ESTRUCTURA PARA MALLA TRENSABLE";
                txtDescripcionGeneral.Text = "ESTRUCTURA PARA MALLA TRENSABLE";
            }
            else if (cboModelos.Text == "PLATINA PARA PANEL")
            {
                nombreInicial = "PLATINAS";
                txtDescripcionGeneradaProducto.Text = "PLATINAS";
                txtDescripcionGeneral.Text = "PLATINAS";
            }
            else if (cboModelos.Text == "RESPALDO MODULOS CERAMICOS")
            {
                nombreInicial = "RESPALDO DE MODULO CERAMICO";
                txtDescripcionGeneradaProducto.Text = "RESPALDO DE MODULO CERAMICO";
                txtDescripcionGeneral.Text = "RESPALDO DE MODULO CERAMICO";
            }
            //------
            //PRODUCTOS QUIMICOS
            else if (cboModelos.Text == "GASES")
            {
                nombreInicial = "";
                txtDescripcionGeneradaProducto.Text = "";
                txtDescripcionGeneral.Text = "";
            }
            else if (cboModelos.Text == "POLIURETANO Y COMPONENTES")
            {
                nombreInicial = "";
                txtDescripcionGeneradaProducto.Text = "";
                txtDescripcionGeneral.Text = "";
            }
            else if (cboModelos.Text == "PEGAMENTOS")
            {
                ckHabilitarTextoCaracteristicas2.Enabled = true;
                nombreInicial = "PEGAMENTOS";
                txtDescripcionGeneradaProducto.Text = "PEGAMENTOS";
                txtDescripcionGeneral.Text = "PEGAMENTOS";
            }
            //------
            //SOLDADIRA
            else if (cboModelos.Text == "MIG/MAG")
            {
                nombreInicial = "SOLDADURA POR MIG/MAG";
                txtDescripcionGeneradaProducto.Text = "SOLDADURA POR MIG/MAG";
                txtDescripcionGeneral.Text = "SOLDADURA POR MIG/MAG";
            }
            else if (cboModelos.Text == "ARCO ELECTRICO")
            {
                nombreInicial = "SOLDADURA POR ARCO ELECTRICO";
                txtDescripcionGeneradaProducto.Text = "OLDADURA POR ARCO ELECTRICO";
                txtDescripcionGeneral.Text = "OLDADURA POR ARCO ELECTRICO";
            }
            //------
            //NAILON
            else if (cboModelos.Text == "BARRA")
            {
                nombreInicial = "BARRA DE NAILON";
                txtDescripcionGeneradaProducto.Text = "BARRA DE NAILON";
                txtDescripcionGeneral.Text = "BARRA DE NAILON";
            }
            else if (cboModelos.Text == "CINTILLOS")
            {
                nombreInicial = "CINTILLOS DE NAILON";
                txtDescripcionGeneradaProducto.Text = "CINTILLOS DE NAILON";
                txtDescripcionGeneral.Text = "CINTILLOS DE NAILON";
            }
            //-----
            //MALLAS DE ACERO Y MALLAS DE ACERO TEMOFUNDIDO
            else if (cboModelos.Text == "MALLAS DE ACERO")
            {
                ckHabilitarTextoNTipos4.Enabled = true;
                nombreInicial = cboModelos.Text;
                txtDescripcionGeneradaProducto.Text = cboModelos.Text;
                txtDescripcionGeneral.Text = cboModelos.Text;
            }
            else if (cboModelos.Text == "MALLAS TERMOFUNDIDO")
            {
                ckHabilitarTextoNTipos4.Enabled = true;
                nombreInicial = "MALLAS DE ACERO TERMOFUNDIDO";
                txtDescripcionGeneradaProducto.Text = "MALLAS DE ACERO TERMOFUNDIDO";
                txtDescripcionGeneral.Text = "MALLAS DE ACERO TERMOFUNDIDO";
            }
            //-----
            //MODULO CERAMICO
            else if (cboModelos.Text == "FUNDIDO" || cboModelos.Text == "PEGADO")
            {
                nombreInicial = cboLineas.Text + ' ' + cboModelos.Text;
                txtDescripcionGeneradaProducto.Text = cboLineas.Text + ' ' + cboModelos.Text;
                txtDescripcionGeneral.Text = cboLineas.Text + ' ' + cboModelos.Text;
            }
            //-----
            //PANELES DE POLIURETANO
            else if (cboLineas.Text == "PANELES DE POLIURETANO")
            {
                nombreInicial = "PANEL " + cboModelos.Text;
                txtDescripcionGeneradaProducto.Text = "PANEL " + cboModelos.Text;
                txtDescripcionGeneral.Text = "PANEL " + cboModelos.Text;

                ckHabilitarTextoMedidas1.Enabled = true;
                ckHabilitarTextoDiseñoAcabado3.Enabled = true;
            }
            //-----
            //PIEZAS DE POLIURETANO
            else if (cboModelos.Text == "PIN")
            {
                ckHabilitarTextoEspesores1.Enabled = true;
            }
            else if (cboModelos.Text == "RIEL" && cboLineas.Text == "PIEZAS DE POLIURETANO")
            {
                nombreInicial = cboModelos.Text + " CON POLIURETANO";
                txtDescripcionGeneradaProducto.Text = cboModelos.Text + " CON POLIURETANO";
                txtDescripcionGeneral.Text = cboModelos.Text + " CON POLIURETANO";
            }
            else if (cboModelos.Text == "COLA DE PATO")
            {
                nombreInicial = "TOBERA DE POLIURETANO";
                txtDescripcionGeneradaProducto.Text = cboModelos.Text + "TOBERA DE POLIURETANO";
                txtDescripcionGeneral.Text = cboModelos.Text + "TOBERA DE POLIURETANO";
            }
            //------
            //MALLAS TERMOINYECTADAS
            else if (cboModelos.Text == "MALLAS TERMOINYECTADAS")
            {
                nombreInicial = "MALLAS DE ACERO TERMOINYECTADO";
                txtDescripcionGeneradaProducto.Text = "MALLAS DE ACERO TERMOINYECTADO";
                txtDescripcionGeneral.Text = "MALLAS DE ACERO TERMOINYECTADO";
                ckHabilitarTextoNTipos4.Enabled = true;
            }
            //------
            //MALLAS TRENSABLES
            else if (cboModelos.Text == " ")
            {
                nombreInicial = "MALLA TRENSABLE";
                txtDescripcionGeneradaProducto.Text = "MALLA TRENSABLE";
                txtDescripcionGeneral.Text = "MALLA TRENSABLE";
            }
            //PIEZAS METALICAS
            else if (cboModelos.Text == "RESPALDO PARA PIEZAS")
            {
                nombreInicial = "";
                txtDescripcionGeneradaProducto.Text = "";
                txtDescripcionGeneral.Text = "";
            }
            //-----
            //SI NO CUENTA
            else
            {
                nombreInicial = cboModelos.Text;
                txtDescripcionGeneradaProducto.Text = cboModelos.Text;
                txtDescripcionGeneral.Text = cboModelos.Text;

                ckHabilitarTextoNTipos4.Enabled = false;
                ckHabilitarTextoMedidas1.Enabled = false;
                ckHabilitarTextoDiseñoAcabado3.Enabled = false;
                ckHabilitarTextoEspesores1.Enabled = false;
            }
        }

        //DEFINICION DE NOMBRE DEL PRODUCTO POR MODELO
        public void DefinicionNombreProductoXModelo()
        {
            //PRIMERA CUENTA------------------------------------------------------
            //MAQUINARIAS Y EQUIPOS DE EXPLORACION
            if (cboModelos.Text == "MAQUINARIA Y EQUIPOS PARA MODULO CERAMICO")
            {
                DescripicionProducto = nombreInicial + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 + " " + txtDescripcionCaracteristicas3.Text + espacio4 +
                                                      " " + txtDescripcionNTipos1.Text + espacio26 + txtDescripcionNTipos2.Text + espacio27 + txtDescripcionNTipos3.Text + espacio28 + txtDescripcionNTipos4.Text + espacio29;
            }
            //SEGUNDA CUENTA------------------------------------------------------
            //ABRASIVOS
            else if (cboModelos.Text == "ARENA")
            {
                DescripicionProducto = nombreInicial + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + "DE " + txtDescripcionCaracteristicas2.Text;
            }
            else if (cboModelos.Text == "CEPILLO CIRCULAR")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 + " " +
                txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11;
            }
            else if (cboModelos.Text == "COPA TRENZADA")
            {

            }
            else if (cboModelos.Text == "DISCO DE CORTE DE ACERO")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                txtDescripcionDiametros1.Text + espacio10 + " X " + txtDescripcionEspesores1.Text + espacio18 + txtDescripcionEspesores2.Text + espacio19 + " X " + txtDescripcionDiametros2.Text + espacio11;
            }
            else if (cboModelos.Text == "DISO DE CORTE DE CERAMICO")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                txtDescripcionDiametros1.Text + espacio10 + " X " + txtDescripcionEspesores1.Text + espacio18 + txtDescripcionEspesores2.Text + espacio19 + " X " + txtDescripcionDiametros2.Text + espacio11;
            }
            else if (cboModelos.Text == "DISCO DE DESBASTE")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                txtDescripcionDiametros1.Text + espacio10 + " X " + txtDescripcionEspesores1.Text + espacio18 + txtDescripcionEspesores2.Text + espacio19 + " X " + txtDescripcionDiametros2.Text + espacio11;
            }
            else if (cboModelos.Text == "GRANALLA")
            {
                DescripicionProducto = nombreInicial + espacio1 + " DE " + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                    " " + txtDescripcionFormas1.Text + espacio14 + txtDescripcionFormas2.Text + espacio15 +
                    " " + txtDescripcionMedida1.Text + espacio6 + txtDescripcionMedida2.Text + espacio7;
            }
            else if (cboModelos.Text == "LIJA")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                   " " + txtDescripcionNTipos1.Text + espacio26 + txtDescripcionNTipos2.Text + espacio27;
            }
            else if (cboModelos.Text == "RUEDA DE DESBASTE")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                txtDescripcionDiametros1.Text + espacio10 + " X " + txtDescripcionEspesores1.Text + espacio18 + txtDescripcionEspesores2.Text + espacio19 + " X " + txtDescripcionDiametros2.Text + espacio11;
            }
            //--------
            //METALES
            else if (cboModelos.Text == "ALUMINIO")
            {

            }
            //------
            //ACEROS
            else if (cboModelos.Text == "ALAMBRE")
            {
                DescripicionProducto = nombreInicial + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 +
                    txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11 +
                  " " + txtDescripcionCaracteristicas2.Text + espacio3;
            }
            else if (cboModelos.Text == "ANGULO")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                    " " + txtDescripcionEspesores1.Text + espacio18 + txtDescripcionEspesores2.Text + espacio19 +
                    " " + txtDescripcionMedida1.Text + espacio6 + txtDescripcionMedida2.Text + espacio7;
            }
            else if (cboModelos.Text == "ANILLO")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                    " " + txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11 +
                    txtDescripcionEspesores1.Text + espacio18 + txtDescripcionEspesores2.Text + espacio19;
            }
            else if (cboModelos.Text == "ARANDELA" && cboLineas.Text == "ACERO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionFormas1.Text + espacio14 + txtDescripcionFormas2.Text + espacio15 +
                    " " + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                    " " + txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11;
            }
            else if (cboModelos.Text == "BARRA RECTANGULAR")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + txtDescripcionMedida1.Text + espacio6 + txtDescripcionMedida2.Text + espacio7 +
                txtDescripcionEspesores1.Text + espacio18 + txtDescripcionEspesores2.Text + espacio19;
            }
            else if (cboModelos.Text == "BARRA REDONDA")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + txtDescripcionMedida1.Text + espacio6 + txtDescripcionMedida2.Text + espacio7 +
                " " + txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11;
            }
            else if (cboModelos.Text == "BARRA REDONDA PERFORADA")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + txtDescripcionMedida1.Text + espacio6 + txtDescripcionMedida2.Text + espacio7 +
                " " + txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11;
            }
            else if (cboModelos.Text == "BARRA CUADRADA")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + txtDescripcionMedida1.Text + espacio6 + txtDescripcionMedida2.Text + espacio7 +
                txtDescripcionEspesores1.Text + espacio18 + txtDescripcionEspesores2.Text + espacio19;
            }
            else if (cboModelos.Text == "BRIDA")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11 +
                txtDescripcionEspesores1.Text + espacio18 + txtDescripcionEspesores2.Text + espacio19;
            }
            else if (cboModelos.Text == "CABLE")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + "CON " + txtDescripcionDiseñoAcabado1.Text + espacio22 + txtDescripcionDiseñoAcabado2.Text + espacio23 +
                " " + txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11;
            }
            else if (cboModelos.Text == "CANAL U")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + "CON " + txtDescripcionNTipos1.Text + espacio26 + txtDescripcionNTipos2.Text + espacio27 +
                " " + txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11;
            }
            else if (cboModelos.Text == "CHAPAS")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + txtDescripcionMedida2.Text + espacio6 + txtDescripcionMedida1.Text + espacio7 +
                txtDescripcionEspesores1.Text + espacio18 + txtDescripcionEspesores2.Text + espacio19;
            }
            else if (cboModelos.Text == "ESTOBOL")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + txtDescripcionMedida1.Text + espacio6 + txtDescripcionMedida2.Text + espacio7 +
                " " + txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11;
            }
            else if (cboModelos.Text == "ESTROBO")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + txtDescripcionMedida1.Text + espacio6 + txtDescripcionMedida2.Text + espacio7 +
                " " + txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11;
            }
            else if (cboModelos.Text == "RUEDA PARA CABLE")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11;
            }
            else if (cboModelos.Text == "NIPPLE")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11 +
                " " + txtDescripcionMedida1.Text + espacio6 + txtDescripcionMedida2.Text + espacio7;
            }
            else if (cboModelos.Text == "PERNO" && cboLineas.Text == "ACERO")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + txtDescripcionNTipos2.Text + espacio27 +
                txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11 +
                " " + txtDescripcionMedida1.Text + espacio6 + txtDescripcionMedida2.Text + espacio7 +
                " " + txtDescripcionNTipos1.Text + espacio26;
            }
            else if (cboModelos.Text == "PLANCHA" && cboLineas.Text == "ACERO")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + txtDescripcionMedida2.Text + espacio6 + txtDescripcionMedida1.Text + espacio7 +
                txtDescripcionEspesores1.Text + espacio18 + txtDescripcionEspesores2.Text + espacio19;
            }
            else if (cboModelos.Text == "PLATINA")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + txtDescripcionMedida1.Text + espacio6 + txtDescripcionMedida2.Text + espacio7 +
                txtDescripcionEspesores1.Text + espacio18 + txtDescripcionEspesores2.Text + espacio19;
            }
            else if (cboModelos.Text == "REMACHE")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + txtDescripcionMedida1.Text + espacio6 + txtDescripcionMedida2.Text + espacio7 +
                " " + txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11;
            }
            else if (cboModelos.Text == "TEE")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + txtDescripcionMedida2.Text + espacio6 + txtDescripcionMedida1.Text + espacio7 +
                txtDescripcionEspesores1.Text + espacio18 + txtDescripcionEspesores2.Text + espacio19;
            }
            else if (cboModelos.Text == "TORNILLO")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + txtDescripcionMedida1.Text + espacio6 + txtDescripcionMedida2.Text + espacio7 +
                " " + txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11 +
                " " + txtDescripcionVariosO1.Text + espacio30 + txtDescripcionVariosO2.Text + espacio31;
            }
            else if (cboModelos.Text == "TUBO CUADRADO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionDiseñoAcabado1.Text + espacio22 + txtDescripcionDiseñoAcabado2.Text + espacio23 +
                " " + txtDescripcionMedida2.Text + espacio6 + txtDescripcionMedida1.Text + espacio7 +
                txtDescripcionEspesores1.Text + espacio18 + txtDescripcionEspesores2.Text + espacio19 +
                " " + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3;
            }
            else if (cboModelos.Text == "TUBO REDONDO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionDiseñoAcabado1.Text + espacio22 + txtDescripcionDiseñoAcabado2.Text + espacio23 +
                " " + txtDescripcionVariosO1.Text + espacio30 +
                txtDescripcionEspesores1.Text + espacio18 + txtDescripcionEspesores2.Text + espacio19 +
                " " + txtDescripcionMedida1.Text + espacio6 + txtDescripcionMedida2.Text + espacio7 +
                " " + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3;
            }
            else if (cboModelos.Text == "TUBO RECTANGULAR")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionDiseñoAcabado1.Text + espacio22 + txtDescripcionDiseñoAcabado2.Text + espacio23 +
                " " + txtDescripcionMedida2.Text + espacio6 + txtDescripcionMedida1.Text + espacio7 +
                txtDescripcionEspesores1.Text + espacio18 + txtDescripcionEspesores2.Text + espacio19 +
                " " + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3;
            }
            else if (cboModelos.Text == "TUERCA" && cboLineas.Text == "ACERO")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + txtDescripcionNTipos1.Text + espacio26 +
                txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11 +
                " " + txtDescripcionNTipos2.Text + espacio27;
            }
            else if (cboModelos.Text == "VARILLA LISA")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + txtDescripcionMedida1.Text + espacio6 + txtDescripcionMedida2.Text + espacio7 +
                " " + txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11;
            }
            else if (cboModelos.Text == "VARILLA ROSCADA")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + txtDescripcionMedida1.Text + espacio6 + txtDescripcionMedida2.Text + espacio7 +
                " " + txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11;
            }
            else if (cboModelos.Text == "RODAJE")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text;
            }
            else if (cboModelos.Text == "BOCINA")
            {
                DescripicionProducto = nombreInicial + " " + espacio1 + txtDescripcionCaracteristicas1.Text + espacio2 + txtDescripcionCaracteristicas2.Text + espacio3 +
                " " + txtDescripcionDiametros1.Text + espacio10 + txtDescripcionDiametros2.Text + espacio11 +
                " " + txtDescripcionMedida1.Text + espacio6 + txtDescripcionMedida2.Text + espacio7;
            }
            //-------
            //CERAMICA
            else if (cboModelos.Text == "CERAMICA")
            {
                string union1 = " & ";
                string union2 = " & ";
                string union3 = " & ";

                if (txtDescripcionMedida4.Text == "") { union1 = " "; }
                if (txtDescripcionMedida3.Text == "") { union2 = " "; }
                if (txtDescripcionEspesores2.Text == "") { union3 = " "; }

                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas3.Text + " " + txtDescripcionCaracteristicas2.Text +
                " " + txtDescripcionDiseñoAcabado1.Text +
                " " + txtDescripcionCaracteristicas1.Text + " " + txtDescripcionCaracteristicas4.Text + " DE ALUMINA " +
                " " + txtDescripcionMedida1.Text + union1 + txtDescripcionMedida4.Text + " " + txtDescripcionMedida2.Text + union2 + txtDescripcionMedida3.Text +
                " " + txtDescripcionEspesores1.Text + union3 + txtDescripcionEspesores2.Text +
                " " + txtDescripcionDiseñoAcabado2.Text;
            }
            //------
            //ELASTOMEROS
            else if (cboModelos.Text == "FRISA ESPONJOSA")
            {
                DescripicionProducto = nombreInicial + " DE " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionEspesores1.Text +
                " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "ORING")
            {
                DescripicionProducto = nombreInicial + " DE " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionDiametros1.Text +
                " " + txtDescripcionEspesores1.Text;
            }
            else if (cboModelos.Text == "PERFIL")
            {
                DescripicionProducto = nombreInicial + " DE " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionFormas1.Text +
                " " + txtDescripcionDiseñoAcabado2.Text + " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "PLANCHA" && cboLineas.Text == "ELASTOMEROS")
            {
                DescripicionProducto = nombreInicial + " DE " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionDiseñoAcabado1.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionEspesores1.Text;
            }
            else if (cboModelos.Text == "CUERDA")
            {
                DescripicionProducto = nombreInicial + " DE " + txtDescripcionCaracteristicas1.Text + " " + txtDescripcionCaracteristicas2.Text +
                " " + txtDescripcionDiametros1.Text;
            }
            else if (cboModelos.Text == "CODO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text + " DE" +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionDiametros1.Text +
                " " + txtDescripcionVariosO1.Text;
            }
            else if (cboModelos.Text == "CONO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionDiametros1.Text + " " + txtDescripcionDiametros2.Text +
                " " + txtDescripcionEspesores1.Text;
            }
            else if (cboModelos.Text == "DE TROMMEL")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionDiametros1.Text + " " + txtDescripcionDiametros2.Text;
            }
            else if (cboModelos.Text == "PARA MALLA TRENSABLE")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionNTipos3.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionNTipos2.Text +
                " " + txtDescripcionNTipos1.Text;
            }
            else if (cboModelos.Text == "PLATINA PARA PANEL")
            {
                DescripicionProducto = nombreInicial +
                " " + txtDescripcionFormas1.Text + " PARA PANEL" +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionMedida2.Text +
                " " + txtDescripcionEspesores1.Text;
            }
            else if (cboModelos.Text == "REJILLAS PARA PANEL")
            {
                DescripicionProducto = nombreInicial + " CON" +
                " " + txtDescripcionNTipos1.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionMedida2.Text +
                " " + txtDescripcionMedida3.Text +
                " " + txtDescripcionFormas1.Text +
                " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "RESPALDO MODULOS CERAMICOS")
            {
                string union1 = " & ";
                string union2 = " & ";

                if (txtDescripcionMedida3.Text == "") { union1 = " "; }
                if (txtDescripcionMedida4.Text == "") { union2 = " "; }

                DescripicionProducto = nombreInicial + " " + txtDescripcionFormas2.Text +
                " " + txtDescripcionMedida1.Text + union1 + txtDescripcionMedida3.Text + " " + txtDescripcionMedida2.Text + union2 + txtDescripcionMedida4.Text +
                " " + txtDescripcionEspesores1.Text + " CON" +
                " " + txtDescripcionFormas1.Text +
                " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "TUBO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionFormas1.Text + " " + txtDescripcionFormas2.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionVariosO1.Text + " " + txtDescripcionVariosO2.Text;
            }
            else if (cboModelos.Text == "REDUCTOR")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionFormas1.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionVariosO1.Text + " " + txtDescripcionVariosO2.Text;
            }
            //-------
            //PRODUCTOS QUIMICOS
            else if (cboModelos.Text == "ADHESIVOS")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text + " " + txtDescripcionCaracteristicas2.Text +
                " " + txtDescripcionMedida1.Text;
            }
            else if (cboModelos.Text == "COAGULANTES")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionVariosO2.Text +
                " " + txtDescripcionNTipos1.Text +
                " " + txtDescripcionVariosO1.Text +
                " " + txtDescripcionMedida1.Text;
            }
            else if (cboModelos.Text == "DESMOLDANTES")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text + " " + txtDescripcionCaracteristicas2.Text;
            }
            else if (cboModelos.Text == "DISOLVENTES")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text;
            }
            else if (cboModelos.Text == "FLOCULANTES")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text + " " + txtDescripcionCaracteristicas2.Text +
                " " + txtDescripcionNTipos1.Text +
                " " + txtDescripcionVariosO1.Text +
                " " + txtDescripcionMedida1.Text;
            }
            else if (cboModelos.Text == "GASES")
            {
                DescripicionProducto = nombreInicial + txtDescripcionCaracteristicas1.Text;
            }
            else if (cboModelos.Text == "MASILLAS")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text;
            }
            else if (cboModelos.Text == "PASTA")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "PEGAMENTOS")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text + " " + txtDescripcionCaracteristicas2.Text +
                " " + txtDescripcionMedida1.Text;
            }
            else if (cboModelos.Text == "PINTURA")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "POLIURETANO Y COMPONENTES")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text + " " + txtDescripcionCaracteristicas2.Text + " " + txtDescripcionCaracteristicas3.Text;
            }
            else if (cboModelos.Text == "PRESERVANTE")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text;
            }
            else if (cboModelos.Text == "PIGMENTO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "ANTIESPUMANTE")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionVariosO2.Text +
                " " + txtDescripcionNTipos1.Text +
                " " + txtDescripcionVariosO1.Text +
                " " + txtDescripcionMedida1.Text;
            }
            else if (cboModelos.Text == "SUPRESOR DE POLVO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionVariosO2.Text +
                " " + txtDescripcionNTipos1.Text +
                " " + txtDescripcionVariosO1.Text +
                " " + txtDescripcionMedida1.Text;
            }
            else if (cboModelos.Text == "SECUESTRANTE")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionVariosO2.Text +
                " " + txtDescripcionNTipos1.Text +
                " " + txtDescripcionVariosO1.Text +
                " " + txtDescripcionMedida1.Text;
            }
            //-------
            //SOLDADURA
            else if (cboModelos.Text == "MIG/MAG")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionNTipos1.Text +
                " CON " + txtDescripcionDiseñoAcabado1.Text +
                " DE " + txtDescripcionMedida1.Text;
            }
            else if (cboModelos.Text == "ARCO ELECTRICO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionNTipos1.Text +
                " CON " + txtDescripcionDiseñoAcabado1.Text +
                " DE " + txtDescripcionMedida1.Text;
            }
            //-------
            //NAILON
            else if (cboModelos.Text == "BARRA")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionDiametros1.Text +
                " X " + txtDescripcionMedida1.Text;
            }
            else if (cboModelos.Text == "CINTILLOS")
            {
                DescripicionProducto = nombreInicial + " DE " + txtDescripcionMedida1.Text + " X " + txtDescripcionMedida2.Text;
            }
            else if (cboModelos.Text == "PERNO" && cboLineas.Text == "NAILON")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionVariosO1.Text +
                " " + txtDescripcionDiametros1.Text +
                " " + txtDescripcionMedida1.Text;
            }
            else if (cboModelos.Text == "TUERCA" && cboLineas.Text == "NAILON")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionFormas1.Text +
                " " + txtDescripcionDiametros1.Text;
            }
            //-------
            //MALLAS DE ACERO
            else if (cboModelos.Text == "MALLAS DE ACERO")
            {
                if (cboDescripcionNTipos3.Text == "")
                {
                    DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text +
                    " " + txtDescripcionNTipos1.Text + " " + txtDescripcionNTipos2.Text + " " + txtDescripcionNTipos4.Text +
                    " " + txtDescripcionDiseñoAcabado1.Text + " " + txtDescripcionNTipos3.Text +
                    " " + txtDescripcionVariosO1.Text + " " + txtDescripcionVariosO2.Text;
                }
                else
                {
                    DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text +
                    " " + txtDescripcionNTipos1.Text + " " + txtDescripcionNTipos2.Text + " " + txtDescripcionNTipos4.Text +
                    " " + txtDescripcionDiseñoAcabado1.Text + " X " + txtDescripcionNTipos3.Text +
                    " " + txtDescripcionVariosO1.Text + " " + txtDescripcionVariosO2.Text;
                }

            }
            //-------
            //MALLAS DE ACERO TERMOFUNDIDO
            else if (cboModelos.Text == "MALLAS TERMOFUNDIDO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionNTipos1.Text + " " + txtDescripcionNTipos2.Text + " " + txtDescripcionNTipos4.Text +
                " " + txtDescripcionDiseñoAcabado1.Text +
                " " + txtDescripcionDiametros1.Text +
                " " + txtDescripcionNTipos3.Text +
                " " + txtDescripcionVariosO1.Text +
                " " + txtDescripcionDiseñoAcabado2.Text;
            }
            //-------
            //MODULO CERAMICO
            else if (cboModelos.Text == "FUNDIDO" || cboModelos.Text == "PEGADO")
            {
                string union1 = " & ";
                string union2 = " & ";

                if (txtDescripcionMedida2.Text == "") { union1 = " "; }
                if (txtDescripcionMedida4.Text == "") { union2 = " "; }

                DescripicionProducto = nombreInicial + " " + txtDescripcionDiseñoAcabado1.Text +
                " " + txtDescripcionFormas1.Text +
                " " + txtDescripcionMedida1.Text + union1 + txtDescripcionMedida2.Text + " " + txtDescripcionMedida3.Text + union2 + txtDescripcionMedida4.Text +
                " " + txtDescripcionEspesores1.Text +
                " " + txtDescripcionCaracteristicas1.Text;
            }
            //-------
            //PANELES POLIURETANO
            if (cboModelos.Text == "CIEGO" || cboModelos.Text == "CONVENCIONAL" || cboModelos.Text == "AUTOLIMPIANTE" || cboModelos.Text == "VIBROHEXAGONAL" || cboModelos.Text == "TEEPEE" || cboModelos.Text == "OBLONGA")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionFormas1.Text +
                " " + txtDescripcionDiseñoAcabado2.Text + " " + txtDescripcionDiseñoAcabado1.Text +
                " " + txtDescripcionNTipos1.Text +
                " " + txtDescripcionMedida1.Text + " " + txtDescripcionMedida2.Text +
                " " + txtDescripcionDiseñoAcabado3.Text +
                " " + txtDescripcionEspesores1.Text +
                " " + txtDescripcionFormas2.Text;
            }
            //-------
            //PIEZAS DE POLIURETANO
            else if (cboModelos.Text == "ALETA")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionMedida2.Text + " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionEspesores1.Text +
                " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "APEX")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionFormas1.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionVariosO2.Text + " " + txtDescripcionVariosO1.Text +
                " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "BARRAS")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionDiseñoAcabado1.Text +
                " " + txtDescripcionMedida1.Text + " " + txtDescripcionMedida2.Text +
                " " + txtDescripcionEspesores1.Text +
                " " + txtDescripcionFormas1.Text +
                " " + txtDescripcionDiseñoAcabado2.Text;
            }
            else if (cboModelos.Text == "BUJE")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionDiametros1.Text + " " + txtDescripcionDiametros2.Text +
                " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "CUÑA")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionDiseñoAcabado1.Text +
                " " + txtDescripcionMedida1.Text + " " + txtDescripcionMedida2.Text +
                " " + txtDescripcionEspesores1.Text + " " + txtDescripcionEspesores1.Text +
                " " + txtDescripcionDiseñoAcabado2.Text;
            }
            else if (cboModelos.Text == "NOCKING BAR")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionMedida1.Text + " " + txtDescripcionMedida2.Text + " " + txtDescripcionMedida3.Text +
                " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "PIN")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionFormas1.Text +
                " " + txtDescripcionDiametros1.Text +
                " " + txtDescripcionEspesores1.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "PROTECTOR DE RIEL")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionMedida1.Text + " " + txtDescripcionMedida2.Text + " " + txtDescripcionMedida3.Text +
                " " + txtDescripcionEspesores1.Text +
                " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "RETENEDOR DE CARGA")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionMedida1.Text + " " + txtDescripcionMedida2.Text +
                " " + txtDescripcionEspesores1.Text +
                " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "RIEL" && cboLineas.Text == "PIEZAS DE POLIURETANO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionMedida1.Text + " " + txtDescripcionMedida2.Text +
                " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "SLEEVE")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionDiametros1.Text +
                " " + txtDescripcionEspesores1.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionDiseñoAcabado1.Text + " " + txtDescripcionDiseñoAcabado2.Text;
            }
            else if (cboModelos.Text == "SPALTO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionEspesores1.Text +
                " ABERTURA " + txtDescripcionNTipos1.Text +
                " X " + txtDescripcionDiseñoAcabado1.Text +
                " " + txtDescripcionVariosO1.Text +
                " " + txtDescripcionDiseñoAcabado2.Text;
            }
            else if (cboModelos.Text == "TAPON")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionDiametros1.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "COLA DE PATO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionMedida1.Text + " " + txtDescripcionMedida2.Text +
                " " + txtDescripcionDiametros1.Text +
                " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "VORTEX")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionDiametros1.Text +
                " " + txtDescripcionDiametros2.Text +
                " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "ZAPATA")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionMedida1.Text + " " + txtDescripcionMedida2.Text +
                " " + txtDescripcionEspesores1.Text +
                " " + txtDescripcionDiametros1.Text +
                " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "LINER")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionMedida1.Text + " " + txtDescripcionMedida2.Text +
                " " + txtDescripcionEspesores1.Text +
                " " + txtDescripcionDiametros1.Text +
                " " + txtDescripcionDiseñoAcabado1.Text;
            }
            //-------
            //MALLAS DE ACERO TERMOINYECTADAS
            else if (cboModelos.Text == "MALLAS TERMOINYECTADAS")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionNTipos1.Text + " " + txtDescripcionNTipos2.Text + " " + txtDescripcionNTipos4.Text +
                " " + txtDescripcionDiseñoAcabado1.Text +
                " " + txtDescripcionDiametros1.Text +
                " " + txtDescripcionNTipos3.Text +
                " " + txtDescripcionDiseñoAcabado3.Text +
                " " + txtDescripcionDiseñoAcabado2.Text;
            }
            //-------
            //REVESTIMIENTO CERAMICO
            else if (cboModelos.Text == "REVESTIMIENTO / APEX")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text + " " + txtDescripcionCaracteristicas2.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionVariosO1.Text + " " + txtDescripcionVariosO2.Text;
            }
            else if (cboModelos.Text == "REVESTIMIENTO / CODO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionVariosO1.Text + " " + txtDescripcionVariosO2.Text;
            }
            else if (cboModelos.Text == "REVESTIMIENTO / CONO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionVariosO1.Text + " " + txtDescripcionVariosO2.Text;
            }
            else if (cboModelos.Text == "REVESTIMIENTO / SPLASH O MANGA")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionVariosO1.Text + " " + txtDescripcionVariosO2.Text;
            }
            else if (cboModelos.Text == "VORTEX" && cboLineas.Text == "REVESTIMEINTO CERAMICO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text + " " + txtDescripcionCaracteristicas2.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionVariosO1.Text + " " + txtDescripcionVariosO2.Text;
            }
            else if (cboModelos.Text == "TUBO" && cboLineas.Text == "REVESTIMEINTO CERAMICO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionFormas2.Text + " " + txtDescripcionFormas1.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionVariosO1.Text + " " + txtDescripcionVariosO2.Text;
            }
            else if (cboModelos.Text == "REDUCTOR" && cboLineas.Text == "REVESTIMEINTO CERAMICO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionFormas1.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionVariosO1.Text + " " + txtDescripcionVariosO2.Text;
            }
            else if (cboModelos.Text == "SPOOL" && cboLineas.Text == "REVESTIMEINTO CERAMICO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionFormas1.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionVariosO1.Text + " " + txtDescripcionVariosO2.Text;
            }
            else if (cboModelos.Text == "ANILLO" && cboLineas.Text == "REVESTIMEINTO CERAMICO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionFormas1.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionVariosO1.Text + " " + txtDescripcionVariosO2.Text;
            }
            else if (cboModelos.Text == "BUSHING" && cboLineas.Text == "REVESTIMEINTO CERAMICO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionFormas1.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionVariosO1.Text + " " + txtDescripcionVariosO2.Text;
            }
            //-------
            //MALLAS TRENSABLES
            else if (cboModelos.Text == " ")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionDiseñoAcabado3.Text +
                " " + txtDescripcionMedida1.Text + " " + txtDescripcionMedida2.Text +
                " " + txtDescripcionEspesores1.Text +
                " " + txtDescripcionDiseñoAcabado2.Text + " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "MALLAS TRENSABLES / HIBRIDO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionDiseñoAcabado2.Text +
                " " + txtDescripcionMedida1.Text + " " + txtDescripcionMedida2.Text +
                " " + txtDescripcionEspesores1.Text +
                " " + txtDescripcionDiseñoAcabado3.Text + " Y " + txtDescripcionDiseñoAcabado4.Text + " " + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "MALLAS TRENSABLES / CIEGO")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionDiseñoAcabado2.Text +
                " " + txtDescripcionMedida1.Text + " " + txtDescripcionMedida2.Text +
                " " + txtDescripcionEspesores1.Text +
                " " + txtDescripcionDiseñoAcabado3.Text + " Y " + txtDescripcionDiseñoAcabado4.Text + " " + txtDescripcionDiseñoAcabado1.Text;
            }
            //-------
            //PIEZAS METALICAS
            else if (cboModelos.Text == "RIEL" && cboLineas.Text == "PIEZAS METALICAS")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionFormas1.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionEspesores1.Text;
            }
            else if (cboModelos.Text == "CLAMP")
            {
                string union1 = " CON ";

                if (txtDescripcionDiseñoAcabado1.Text == "") { union1 = " "; }

                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionMedida1.Text +
                " " + txtDescripcionEspesores1.Text +
                union1 + txtDescripcionDiseñoAcabado1.Text;
            }
            else if (cboModelos.Text == "ACCESORIO DE TRANSICION")
            {
                string union1 = " CON ";

                if (txtDescripcionFormas1.Text == "") { union1 = " "; }

                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionMedida1.Text +
                union1 + txtDescripcionFormas1.Text;
            }
            else if (cboModelos.Text == "RESPALDO PARA PIEZAS")
            {
                DescripicionProducto = nombreInicial + txtDescripcionFormas1.Text +
                " " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionMedida1.Text + " " + txtDescripcionMedida2.Text +
                " " + txtDescripcionEspesores1.Text;
            }
            else if (cboModelos.Text == "ARANDELA" && cboLineas.Text == "PIEZAS METALICAS")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionFormas1.Text +
                " " + txtDescripcionVariosO1.Text + " " + txtDescripcionVariosO2.Text +
                " " + txtDescripcionEspesores1.Text;
            }
            else if (cboModelos.Text == "CHUTE" && cboLineas.Text == "PIEZAS METALICAS")
            {
                DescripicionProducto = nombreInicial + " " + txtDescripcionCaracteristicas1.Text +
                " " + txtDescripcionMedida1.Text + " " + txtDescripcionMedida2.Text + " " + txtDescripcionMedida3.Text +
                " " + txtDescripcionEspesores1.Text;
            }

            LimpiarEspaciosDescripcionProducto();
        }

        //FUNCION QUE LIMPIA MIS ESPACIOS EN BLANCO
        private void LimpiarEspaciosDescripcionProducto()
        {
            DescripicionProducto = DescripicionProducto.Replace("   ", " ").Replace("  ", " ").Replace("  ", " ").Replace("    ", " ");
            txtDescripcionGeneradaProducto.Text = DescripicionProducto;
        }

        //DEFINICION DE MODELOS
        //--------------------------------------------------------------------------------
        public void DefinicionModelosTexto()
        {
            //PRIMERA CUENTA------------------------------------------------------
            //MAQUINARIAS Y EQUIPOS DE EXPLORACION
            if (cboModelos.Text == "MAQUINARIA Y EQUIPOS PARA MODULO CERAMICO")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoCaracteristicas3.Checked = true;
                ckHabilitarTextoCaracteristicas4.Checked = false;
                ckHabilitarTextoNTipos1.Checked = true;
                ckHabilitarTextoNTipos2.Checked = false;
            }
            //SEGUNDA CUENTA------------------------------------------------------
            //ABRASIVOS
            else if (cboModelos.Text == "ARENA")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
            }
            else if (cboModelos.Text == "CEPILLO CIRCULAR")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
            }
            else if (cboModelos.Text == "COPA TRENZADA")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
            }
            else if (cboModelos.Text == "DISCO DE CORTE DE ACERO")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = false;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = false;
                ckHabilitarTextoEspesores2.Checked = false;
            }
            else if (cboModelos.Text == "DISO DE CORTE DE CERAMICO")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = false;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = false;
                ckHabilitarTextoEspesores2.Checked = false;
            }
            else if (cboModelos.Text == "DISCO DE DESBASTE")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = false;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = false;
                ckHabilitarTextoEspesores2.Checked = false;
            }
            else if (cboModelos.Text == "GRANALLA")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = false;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
            }
            else if (cboModelos.Text == "LIJA")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoNTipos1.Checked = true;
                ckHabilitarTextoNTipos2.Checked = false;
            }
            else if (cboModelos.Text == "RUEDA DE DESBASTE")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = false;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = false;
                ckHabilitarTextoEspesores2.Checked = false;
            }
            //--------
            //METALES
            else if (cboModelos.Text == "ALUMINIO")
            {
                //ckHabilitarTextoCaracteristicas1.Checked = false;
                //ckHabilitarTextoCaracteristicas2.Checked = false;
                //ckHabilitarTextoMedidas1.Checked = true;
                //ckHabilitarTextoMedidas2.Checked = false;
                //ckHabilitarTextoDiametros1.Checked = true;
                //ckHabilitarTextoDiametros2.Checked = false;
            }
            //------
            //ACEROS
            else if (cboModelos.Text == "ALAMBRE")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoCaracteristicas3.Checked = false;
                ckHabilitarTextoCaracteristicas4.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
            }
            else if (cboModelos.Text == "ANGULO")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
            }
            else if (cboModelos.Text == "ANILLO")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = true;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
            }
            else if (cboModelos.Text == "ARANDELA" && cboLineas.Text == "ACERO")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
            }
            else if (cboModelos.Text == "BARRA RECTANGULAR")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
            }
            else if (cboModelos.Text == "BARRA REDONDA")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
            }
            else if (cboModelos.Text == "BARRA REDONDA PERFORADA")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = true;
            }
            else if (cboModelos.Text == "BARRA CUADRADA")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
            }
            else if (cboModelos.Text == "BRIDA")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = true;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
            }
            else if (cboModelos.Text == "CABLE")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "CANAL U")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoNTipos1.Checked = false;
                ckHabilitarTextoNTipos2.Checked = false;
            }
            else if (cboModelos.Text == "CHAPAS")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
            }
            else if (cboModelos.Text == "ESTOBOL")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
            }
            else if (cboModelos.Text == "ESTROBO")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
            }
            else if (cboModelos.Text == "RUEDA PARA CABLE")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = false;
                ckHabilitarTextoDiametros2.Checked = false;
            }
            else if (cboModelos.Text == "NIPPLE")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
            }
            else if (cboModelos.Text == "PERNO" && cboLineas.Text == "ACERO")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoNTipos1.Checked = true;
                ckHabilitarTextoNTipos2.Checked = true;
            }
            else if (cboModelos.Text == "PLANCHA" && cboLineas.Text == "ACERO")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
            }
            else if (cboModelos.Text == "PLATINA")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
            }
            else if (cboModelos.Text == "REMACHE")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
            }
            else if (cboModelos.Text == "TEE")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
            }
            else if (cboModelos.Text == "TORNILLO")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
            }
            else if (cboModelos.Text == "TUBO CUADRADO")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "TUBO REDONDO")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = false;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
                ckHabilitarTextoVarios01.Checked = false;
            }
            else if (cboModelos.Text == "TUBO RECTANGULAR")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "TUERCA" && cboLineas.Text == "ACERO")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoNTipos1.Checked = false;
                ckHabilitarTextoNTipos2.Checked = true;
            }
            else if (cboModelos.Text == "VARILLA LISA")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
            }
            else if (cboModelos.Text == "VARILLA ROSCADA")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
            }
            else if (cboModelos.Text == "RODAJE")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
            }
            else if (cboModelos.Text == "BOCINA")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
            }
            //-------
            //CERAMICA
            else if (cboModelos.Text == "CERAMICA")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoCaracteristicas3.Checked = false;
                ckHabilitarTextoCaracteristicas4.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoMedidas3.Checked = false;
                ckHabilitarTextoMedidas4.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            //------
            //ELASTOMEROS
            else if (cboModelos.Text == "FRISA ESPONJOSA")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = true;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "ORING")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = true;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "PERFIL")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoFormas1.Checked = true;
                ckHabilitarTextoFormas2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = true;
                ckHabilitarTextoDiseñoAcabado2.Checked = true;
            }
            else if (cboModelos.Text == "PLANCHA" && cboLineas.Text == "ELASTOMEROS")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "CUERDA")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
            }
            //--------
            //ESTRUCTURAS METALICAS
            else if (cboModelos.Text == "CODO")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = false;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoVarios01.Checked = false;
            }
            else if (cboModelos.Text == "CONO")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = true;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
            }
            else if (cboModelos.Text == "DE TROMMEL")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = true;
            }
            else if (cboModelos.Text == "PARA MALLA TRENSABLE")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoNTipos1.Checked = false;
                ckHabilitarTextoNTipos2.Checked = true;
                ckHabilitarTextoNTipos3.Checked = true;
                ckHabilitarTextoNTipos4.Checked = false;
            }
            else if (cboModelos.Text == "PLATINA PARA PANEL")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
            }
            else if (cboModelos.Text == "REJILLAS PARA PANEL")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoMedidas3.Checked = true;
                ckHabilitarTextoMedidas4.Checked = false;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
                ckHabilitarTextoNTipos1.Checked = false;
                ckHabilitarTextoNTipos2.Checked = false;
            }
            else if (cboModelos.Text == "RESPALDO MODULOS CERAMICOS")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoMedidas3.Checked = false;
                ckHabilitarTextoMedidas4.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "TUBO")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
                ckHabilitarTextoVarios02.Checked = true;
            }
            else if (cboModelos.Text == "REDUCTOR")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
                ckHabilitarTextoVarios02.Checked = true;
            }
            //-------
            //PRODUCTOS QUIMICOS
            else if (cboModelos.Text == "ADHESIVOS")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
            }
            else if (cboModelos.Text == "COAGULANTES")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoNTipos1.Checked = false;
                ckHabilitarTextoNTipos2.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
                ckHabilitarTextoVarios02.Checked = false;
            }
            else if (cboModelos.Text == "DESMOLANTES")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
            }
            else if (cboModelos.Text == "FLOCULANTES")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoNTipos1.Checked = false;
                ckHabilitarTextoNTipos2.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
            }
            else if (cboModelos.Text == "GASES")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
            }
            else if (cboModelos.Text == "MASILLAS")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
            }
            else if (cboModelos.Text == "PASTA")
            {
                ckHabilitarTextoDiseñoAcabado1.Checked = true;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "PEGAMENTOS")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = true;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
            }
            else if (cboModelos.Text == "PINTURA")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = true;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "POLIURETANO Y COMPONENTES")
            {
                ckHabilitarTextoCaracteristicas1.Checked = true;
                ckHabilitarTextoCaracteristicas2.Checked = true;
                ckHabilitarTextoCaracteristicas3.Checked = false;
                ckHabilitarTextoCaracteristicas4.Checked = false;
            }
            else if (cboModelos.Text == "PRESERVANTE")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
            }
            else if (cboModelos.Text == "PIGMENTO")
            {
                ckHabilitarTextoDiseñoAcabado1.Checked = true;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "ANTIESPUMANTE")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoNTipos1.Checked = false;
                ckHabilitarTextoNTipos2.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
                ckHabilitarTextoVarios02.Checked = false;
            }
            else if (cboModelos.Text == "SUPRESOR DE POLVO")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoNTipos1.Checked = false;
                ckHabilitarTextoNTipos2.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
                ckHabilitarTextoVarios02.Checked = false;
            }
            else if (cboModelos.Text == "SECUESTRANTE")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoNTipos1.Checked = false;
                ckHabilitarTextoNTipos2.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
                ckHabilitarTextoVarios02.Checked = false;
            }
            //--------
            //SOLDADURA
            else if (cboModelos.Text == "MIG/MAG")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
                ckHabilitarTextoNTipos1.Checked = false;
                ckHabilitarTextoNTipos2.Checked = false;
            }
            else if (cboModelos.Text == "ARCO ELECTRICO")
            {
                ckHabilitarTextoMedidas1.Checked = false;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
                ckHabilitarTextoNTipos1.Checked = false;
                ckHabilitarTextoNTipos2.Checked = false;
            }
            //--------
            //NAILON
            else if (cboModelos.Text == "BARRA")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
            }
            else if (cboModelos.Text == "CINTILLOS")
            {
                ckHabilitarTextoMedidas1.Checked = false;
                ckHabilitarTextoMedidas2.Checked = false;
            }
            else if (cboModelos.Text == "PERNO" && cboLineas.Text == "NAILON")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoVarios01.Checked = false;
            }
            else if (cboModelos.Text == "TUERCA" && cboLineas.Text == "NAILON")
            {
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
            }
            //-------
            //MALAS DE ACERO
            else if (cboModelos.Text == "MALLAS DE ACERO")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = true;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
                ckHabilitarTextoNTipos1.Checked = true;
                ckHabilitarTextoNTipos2.Checked = true;
                ckHabilitarTextoNTipos3.Checked = false;
                ckHabilitarTextoNTipos4.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
                ckHabilitarTextoVarios02.Checked = false;
            }
            //-------
            //MALAS DE ACERO TERMOFUNDIDOS
            else if (cboModelos.Text == "MALLAS TERMOFUNDIDO")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = true;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
                ckHabilitarTextoNTipos1.Checked = true;
                ckHabilitarTextoNTipos2.Checked = true;
                ckHabilitarTextoNTipos3.Checked = false;
                ckHabilitarTextoNTipos4.Checked = false;
                ckHabilitarTextoVarios01.Checked = false;
            }
            //-------
            //MODULO CERAMICO
            else if (cboModelos.Text == "FUNDIDO" || cboModelos.Text == "PEGADO")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoMedidas3.Checked = true;
                ckHabilitarTextoMedidas4.Checked = false;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            //-------
            //PANELES POLIURETANO
            else if (cboModelos.Text == "CIEGO" || cboModelos.Text == "CONVENCIONAL" || cboModelos.Text == "AUTOLIMPIANTE" || cboModelos.Text == "VIBROHEXAGONAL" || cboModelos.Text == "TEE PEE" || cboModelos.Text == "OBLONGA")
            {
                ckHabilitarTextoMedidas1.Checked = false;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
                ckHabilitarTextoDiseñoAcabado3.Checked = false;
                ckHabilitarTextoDiseñoAcabado4.Checked = false;
                ckHabilitarTextoNTipos1.Checked = false;
                ckHabilitarTextoNTipos2.Checked = false;
            }
            //------
            //PIEZAS DE POLIURETANO
            else if (cboModelos.Text == "ALETA")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "APEX")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
                ckHabilitarTextoVarios02.Checked = true;
            }
            else if (cboModelos.Text == "BARRAS")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoFormas1.Checked = true;
                ckHabilitarTextoFormas2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "BUJE")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = true;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "CUÑA")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = true;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "NOCKING BAR")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoMedidas3.Checked = true;
                ckHabilitarTextoMedidas4.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "PIN")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "PROTECTOR DE RIEL")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoMedidas3.Checked = true;
                ckHabilitarTextoMedidas4.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "RETENEDOR DE CARGA")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "RIEL" && cboLineas.Text == "PIEZAS DE POLIURETANO")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "SLEEVE")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "SPALTO")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
                ckHabilitarTextoNTipos1.Checked = false;
                ckHabilitarTextoNTipos2.Checked = false;
                ckHabilitarTextoVarios01.Checked = false;
            }
            else if (cboModelos.Text == "TAPON")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "COLA DE PATO")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "VORTEX")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = true;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "ZAPATA")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "LINER")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            //-------
            //MALLAS DE ACERO TERMOINYECTADAS
            else if (cboModelos.Text == "MALLAS TERMOINYECTADAS")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoDiametros1.Checked = true;
                ckHabilitarTextoDiametros2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = true;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
                ckHabilitarTextoDiseñoAcabado3.Checked = false;
                ckHabilitarTextoDiseñoAcabado4.Checked = false;
                ckHabilitarTextoNTipos1.Checked = true;
                ckHabilitarTextoNTipos2.Checked = true;
                ckHabilitarTextoNTipos3.Checked = true;
                ckHabilitarTextoNTipos4.Checked = false;
            }
            //--------
            //REVESTIMIENTO CERAMICO
            else if (cboModelos.Text == "REVESTIMIENTO / APEX")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
                ckHabilitarTextoVarios02.Checked = true;
            }
            else if (cboModelos.Text == "REVESTIMIENTO / CODO")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
                ckHabilitarTextoVarios02.Checked = true;
            }
            else if (cboModelos.Text == "REVESTIMIENTO / CONO")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
                ckHabilitarTextoVarios02.Checked = true;
            }
            else if (cboModelos.Text == "REVESTIMIENTO / SPLASH O MANGA")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
                ckHabilitarTextoVarios02.Checked = true;
            }
            else if (cboModelos.Text == "VORTEX")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoVarios01.Checked = true;
                ckHabilitarTextoVarios02.Checked = true;
            }
            else if (cboModelos.Text == "TUBO")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
                ckHabilitarTextoVarios02.Checked = true;
            }
            else if (cboModelos.Text == "REDUCTOR")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
                ckHabilitarTextoVarios02.Checked = true;
            }
            else if (cboModelos.Text == "SPOOL")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
                ckHabilitarTextoVarios02.Checked = true;
            }
            else if (cboModelos.Text == "ANILLO")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
                ckHabilitarTextoVarios02.Checked = true;
            }
            else if (cboModelos.Text == "BUSHING")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
                ckHabilitarTextoVarios02.Checked = true;
            }
            //--------
            //MALLAS TRENSABLES
            else if (cboModelos.Text == " ")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = true;
                ckHabilitarTextoDiseñoAcabado2.Checked = true;
                ckHabilitarTextoDiseñoAcabado3.Checked = false;
                ckHabilitarTextoDiseñoAcabado4.Checked = false;
            }
            else if (cboModelos.Text == "MALLAS TRENSABLES / HIBRIDO")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
                ckHabilitarTextoDiseñoAcabado3.Checked = true;
                ckHabilitarTextoDiseñoAcabado4.Checked = false;
            }
            else if (cboModelos.Text == "MALLAS TRENSABLES / CIEGO")
            {
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
                ckHabilitarTextoDiseñoAcabado3.Checked = true;
                ckHabilitarTextoDiseñoAcabado4.Checked = false;
            }
            //--------
            //PIEZAS METALICAS
            else if (cboModelos.Text == "RIEL" && cboLineas.Text == "PIEZAS METALICAS")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoFormas1.Checked = true;
                ckHabilitarTextoFormas2.Checked = false;
            }
            else if (cboModelos.Text == "CLAMP")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoDiseñoAcabado1.Checked = false;
                ckHabilitarTextoDiseñoAcabado2.Checked = false;
            }
            else if (cboModelos.Text == "ACCESORIO DE TRANSICION")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = false;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
            }
            else if (cboModelos.Text == "RESPALDO PARA PIEZAS")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
            }
            else if (cboModelos.Text == "ARANDELA" && cboLineas.Text == "PIEZAS METALICAS")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
                ckHabilitarTextoFormas1.Checked = false;
                ckHabilitarTextoFormas2.Checked = false;
                ckHabilitarTextoVarios01.Checked = true;
                ckHabilitarTextoVarios02.Checked = true;
            }
            else if (cboModelos.Text == "CHUTE")
            {
                ckHabilitarTextoCaracteristicas1.Checked = false;
                ckHabilitarTextoCaracteristicas2.Checked = false;
                ckHabilitarTextoMedidas1.Checked = true;
                ckHabilitarTextoMedidas2.Checked = true;
                ckHabilitarTextoMedidas3.Checked = true;
                ckHabilitarTextoMedidas4.Checked = false;
                ckHabilitarTextoEspesores1.Checked = true;
                ckHabilitarTextoEspesores2.Checked = false;
            }
            //--------
        }
    }
}

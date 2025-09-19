using iTextSharp.text.pdf.codec;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Procesos.Productos
{
    public partial class ListadoProductos : Form
    {
        ////VARIABLES PARA LA CARGA DEL PRODUCTO SELECCIONADO
        int idart = 0;
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

        ////VARIABLES DE BUSQUEDA
        string idlinea;
        string idbusquedamercaderia;
        string idbusquedalinea;

        //VARIABLLES PARA HABILITAR E INABILITAR LOS GRUPOS DE CAMPOS
        int CampCaracteristicas1 = 0;
        int CampCaracteristicas2 = 0;

        int CampMedidas1 = 0;
        int CampMedidas2 = 0;

        int CampDiametros1 = 0;
        int CampDiametros2 = 0;

        int CampFormas1 = 0;
        int CampFormas2 = 0;

        int CampEspesores1 = 0;
        int CampEspesores2 = 0;

        int CampDiseñoAcabado1 = 0;
        int CampDiseñoAcabado2 = 0;

        int CampNTipos1 = 0;
        int CampNTipos2 = 0;

        int CampVarios1 = 0;
        int CampVarios2 = 0;

        int CampGeneral = 0;

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

        int EstadoValidacionCampoVacios = 0;

        //CONSTRUCTOR DEL MANTENIMIENTO - MANTENIEMINTO DE LISTADO DE PRODUCTOS
        public ListadoProductos()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI MANTENIMIENTOS DE LISTADO DE PRODUCTOS
        private void ListadoProductos_Load(object sender, EventArgs e)
        {
            CargarTipoMedida();
            CargarTipoMercaderia();
            CargarLineas();
            CargarBusquedasTipoMercaderia();
            CargarDiferencial();
            MostrarProductos();

            CargarOrigen();
            CargarTerminosCompra();
            TipoExistencia();
            BienesSujetoPercepcion();

            cboTipoMedida.SelectedIndex = -1;
            cboTipoMercaderia.SelectedIndex = -1;
            cboDiferencial.SelectedIndex = -1;
            cboLineas.SelectedIndex = -1;
            cboModelos.SelectedIndex = -1;

            alternarColorFilas(datalistado);

            cboBusquedaProducto.SelectedIndex = 1;
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
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR COMBOS - PRINCIPALES-----------------------------------------------------------------------------------
        //CARGA DE CEUNTAS O TIPO DE MERCADERIA
        public void CargarTipoMercaderia()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdTipoMercaderias,Desciripcion FROM TIPOMERCADERIAS WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboTipoMercaderia.DisplayMember = "Desciripcion";
                cboTipoMercaderia.ValueMember = "IdTipoMercaderias";
                cboTipoMercaderia.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGA DE TIPO DE MEDIDAS
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
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGA DE LAS LINEAS
        public void CargarLineas()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdLinea,Descripcion FROM LINEAS WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboLineas.DisplayMember = "Descripcion";
                cboLineas.ValueMember = "IdLinea";
                cboLineas.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGA DE LA BÚSQUEDA DE CUENTAS O TIPO MERCADERIA
        public void CargarBusquedasTipoMercaderia()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdTipoMercaderias,Desciripcion FROM TIPOMERCADERIAS WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboFiltroTipoMercaderiaProducto.DisplayMember = "Desciripcion";
                cboFiltroTipoMercaderiaProducto.ValueMember = "IdTipoMercaderias";
                cboFiltroTipoMercaderiaProducto.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGA DE LA BÚSQUEDA DE LÍNEAS SEGÚN LA CUENTA
        public void CargarBusquedasLinea(string idmercaderia)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdLinea,Descripcion FROM LINEAS WHERE Estado = 1 AND IdTipMer = @idmercaderia", con);
                comando.Parameters.AddWithValue("@idmercaderia", idmercaderia);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboFiltroLineaProducto.DisplayMember = "Descripcion";
                cboFiltroLineaProducto.ValueMember = "IdLinea";
                cboFiltroLineaProducto.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR EL DIFERENCIAL
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
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
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
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR LOS MODELOS SEGÚN LA LÍNEA
        public void CargarModelos(string idlinea)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdModelo,Descripcion, Abreviatura FROM MODELOS WHERE Estado = 1 AND IdLinea = @idlinea", con);
                comando.Parameters.AddWithValue("@idlinea", idlinea);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboModelos.DisplayMember = "Descripcion";
                cboModelos.ValueMember = "IdModelo";
                DataRow row = dt.Rows[0];
                lblAbreviaturaModelo.Text = System.Convert.ToString(row["Abreviatura"]);
                cboModelos.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR ABREVIATURA SEGÚN EL REGIOSTRO SELECCIOANDO
        public void AbreviaturaSegunRegistroSeleccioando(string id)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdModelo,Descripcion, Abreviatura FROM MODELOS WHERE Estado = 1 AND IdModelo = @id", con);
                comando.Parameters.AddWithValue("@id", id);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lblAbreviaturaModelo.Text = System.Convert.ToString(row["Abreviatura"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR BUSCADOR DE MODELOS SEGÚN LA LÍNEA SELECCIOANDA
        public void CargarBusquedasModelos(string idlinea)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdModelo,Descripcion FROM MODELOS WHERE Estado = 1 AND IdLinea = @idlinea", con);
                comando.Parameters.AddWithValue("@idlinea", idlinea);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboFiltroModeloProducto.DisplayMember = "Descripcion";
                cboFiltroModeloProducto.ValueMember = "IdModelo";
                cboFiltroModeloProducto.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //COMBOS DE DATOS ANEXOS------------------------------------------------------------
        //CARGAR COMOBO DE ORIGEN
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
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR COMBO DE TERMINOS DE COMPRA
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
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR COMBO DE TIPO DE EXISTENCIA
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
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR COMBO DE BIENES SUJETO PERCEPCIÓN
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
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGA DE CAMPOS SEGUN LA SELECCION DEL PRODUCTO
        //CARGAR TIPOS DE CARACTERISTICAS - DESCRIPCION DE CARACTERISTICAS - SELECCIONA DE VENTANA
        public void CargarTiposCaracteriticas(ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdTipoCaracteristicas,Descripcion FROM TiposCaracteristicas WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdTipoCaracteristicas";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        public void CargarDescripcionCaracteristicas(ComboBox cbo, string idtipocaracteristicas, string idmodelo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdDescripcionCaracteristicas,Descripcion FROM DescripcionCaracteristicas WHERE Estado = 1 AND IdTipoCaracteristicas = @idtipocaracteristicas AND IdModelo = @idmodelo", con);
                comando.Parameters.AddWithValue("@idtipocaracteristicas", idtipocaracteristicas);
                comando.Parameters.AddWithValue("@idmodelo", idmodelo);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdDescripcionCaracteristicas";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR TIPOS DE MEDIDAS - DESCRIPCION DE MEDIDAS - SELECCIONA DE VENTANA
        public void CargarTiposMedidas(ComboBox cbo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdTipoMedidas,Descripcion FROM TiposMedidas WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdTipoMedidas";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        public void CargarDescripcionMedidas(ComboBox cbo, string idtipomedida, string idmodelo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdDescripcionMedidas,Descripcion FROM DescripcionMedidas WHERE Estado = 1 AND IdTipoMedidas = @idtipomedida AND IdModelo = @idmodelo", con);
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
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
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
                SqlCommand comando = new SqlCommand("SELECT IdTipoDiametros,Descripcion FROM TiposDiametros WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdTipoDiametros";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }

        }

        public void CargarDescripcionDiametros(ComboBox cbo, string ididametros, string idmodelo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdDescripcionDiametros,Descripcion FROM DescripcionDiametros WHERE Estado = 1 AND IdTipoDiametros = @idtipodiametros AND IdModelo = @idmodelo", con);
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
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
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
                SqlCommand comando = new SqlCommand("SELECT IdTipoFormas,Descripcion FROM TiposFormas WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdTipoFormas";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        public void CargarDescripcionFormas(ComboBox cbo, string idformas, string idmodelo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdDescripcionFormas,Descripcion FROM DescripcionFormas WHERE Estado = 1 AND IdTipoFormas = @idtipoformas AND IdModelo = @idmodelo", con);
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
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
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
                SqlCommand comando = new SqlCommand("SELECT IdTipoEspesores,Descripcion FROM TiposEspesores WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdTipoEspesores";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        public void CargarDescripcionEspesores(ComboBox cbo, string idespesores, string idmodelo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdDescripcionEspesores,Descripcion FROM DescripcionEspesores WHERE Estado = 1 AND IdTipoEspesores = @idtipoespesores AND IdModelo = @idmodelo", con);
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
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
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
                SqlCommand comando = new SqlCommand("SELECT IdTipoDiseñoAcabado,Descripcion FROM TiposDiseñoAcabado WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdTipoDiseñoAcabado";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        public void CargarDescripcionDiseñoAcabado(ComboBox cbo, string iddiseñoacabado, string idmodelo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdDescripcionDiseñoAcabado,Descripcion FROM DescripcionDiseñoAcabado WHERE Estado = 1 AND IdTipoDiseñoAcabado = @idtipodiseñoacabado AND IdModelo = @idmodelo", con);
                comando.Parameters.AddWithValue("@idtipodiseñoacabado", iddiseñoacabado);
                comando.Parameters.AddWithValue("@idmodelo", idmodelo);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdDescripcionDiseñoAcabado";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
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
                SqlCommand comando = new SqlCommand("SELECT IdTipoNTipos,Descripcion FROM TiposNTipos WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdTipoNTipos";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        public void CargarDescripcionNTipos(ComboBox cbo, string idntipos, string idmodelo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdDescripcionNTipos,Descripcion FROM DescripcionNTipos WHERE Estado = 1 AND IdTipoNTipos = @idtiposntipos AND IdModelo = @idmodelo", con);
                comando.Parameters.AddWithValue("@idtiposntipos", idntipos);
                comando.Parameters.AddWithValue("@idmodelo", idmodelo);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdDescripcionNTipos";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
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
                SqlCommand comando = new SqlCommand("SELECT IdTipoVariosO,Descripcion FROM TiposVariosO WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdTipoVariosO";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        public void CargarDescripcionVariosO(ComboBox cbo, string idvarioso, string idmodelo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdDescripcionVarios0,Descripcion FROM DescripcionVarios0 WHERE Estado = 1 AND IdTipoVarios0 = @idvarioso AND IdModelo = @idmodelo", con);
                comando.Parameters.AddWithValue("@idvarioso", idvarioso);
                comando.Parameters.AddWithValue("@idmodelo", idmodelo);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.ValueMember = "IdDescripcionVarios0";
                cbo.DisplayMember = "Descripcion";
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGA DE LISTADOS SEGUN EL PRODUCTO SELECCIONADO PARA VISUALIZAR----------------
        //CARGAR LISTADO CARACTERISTICAS 1
        public void CargarListadoCaracteristicas1()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarListadoCaracteristicas1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idart", idart);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoCaracteristicas1.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR LISTADO CARACTERISTICAS 2
        public void CargarListadoCaracteristicas2()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarListadoCaracteristicas2", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idart", idart);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoCaracteristicas2.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR LISTADO MEDIDAS 1
        public void CargarListadoMedidas1()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarListadoMedidas1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idart", idart);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoMedidas1.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR LISTADO MEDIDAS 2
        public void CargarListadoMedidas2()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarListadoMedidas2", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idart", idart);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoMedidas2.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR LISTADO DIÁMETROS 1
        public void CargarListadoDiametros1()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarListadoDiametros1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idart", idart);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoDiametros1.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR LISTADO DIÁMETROS 2
        public void CargarListadoDiametros2()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarListadoDiametros2", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idart", idart);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoDiametros2.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR LISTADO FORMAS 1
        public void CargarListadoFormas1()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarListadoFormas1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idart", idart);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoFormas1.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR LISTADO FORMAS 2
        public void CargarListadoFormas2()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarListadoFormas2", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idart", idart);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoFormas2.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR LISTADO ESPESORES 1
        public void CargarListadoEspesores1()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarListadoEspesores1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idart", idart);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoEspesores1.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR LISTADO ESPESORES 2
        public void CargarListadoEspesores2()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarListadoEspesores2", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idart", idart);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoEspesores2.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR LISTADO DISEÑO ACABADO 1
        public void CargarListadoDiseñoAcabado1()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarListadoDiseñoAcabado1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idart", idart);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoDiseñoAcabado1.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR LISTADO DISEÑO ACABADO 2
        public void CargarListadoDiseñoAcabado2()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarListadoDiseñoAcabado2", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idart", idart);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoDiseñoAcabado2.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR LISTADO NTIPOS 1
        public void CargarListadoNTipos1()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarListadoNTipos1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idart", idart);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                DatalistadoNTipos1.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR LISTADO NTIPOS 2
        public void CargarListadoNTipos2()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarListadoNTipos2", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idart", idart);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                DatalistadoNTipos2.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR LISTADO VARIOS0 1
        public void CargarListadoVariosO1()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarListadoVariosO1", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idart", idart);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoVarios01.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR LISTADO VARIOS0 2
        public void CargarListadoVariosO2()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarListadoVariosO2", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idart", idart);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoVarios02.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR LISTADO GENERAL
        public void CargarListadoGenerales()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarListadoGenerales", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idart", idart);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoGeneral.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGAR EL ÚLTIMO CÓDIGO DE PLANO INGRESADO
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
                datalistadoPlano.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }
        //FIN----------------------------------------------------------------------------------------------------------------------

        //METODOS Y ACCIONES DEL FORMULATIO------------------------------------------------------
        //MOSTRAR TODOS LOS PRODUCTOS INGRESADOS AL SISTEMA
        public void MostrarProductos()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand("ListadoProductos_MostrarProductos", con);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistado.DataSource = dt;
                con.Close();
                datalistado.Columns[0].Visible = false;
                datalistado.Columns[1].Width = 100;
                datalistado.Columns[2].Width = 100;
                datalistado.Columns[3].Width = 915;
                datalistado.Columns[4].Visible = false;
                datalistado.Columns[5].Width = 90;
                datalistado.Columns[6].Visible = false;
                datalistado.Columns[7].Width = 135;
                datalistado.Columns[8].Visible = false;
                datalistado.Columns[9].Visible = false;
                datalistado.Columns[10].Visible = false;
                datalistado.Columns[11].Visible = false;
                datalistado.Columns[12].Visible = false;
                datalistado.Columns[13].Width = 30;
                datalistado.Columns[14].Visible = false;
                datalistado.Columns[15].Visible = false;
                datalistado.Columns[16].Visible = false;
                datalistado.Columns[17].Visible = false;
                datalistado.Columns[18].Visible = false;
                datalistado.Columns[19].Visible = false;
                datalistado.Columns[20].Visible = false;
                datalistado.Columns[21].Visible = false;
                datalistado.Columns[22].Visible = false;
                datalistado.Columns[23].Visible = false;
                datalistado.Columns[24].Visible = false;
                datalistado.Columns[25].Visible = false;
                datalistado.Columns[26].Visible = false;
                datalistado.Columns[27].Visible = false;
                datalistado.Columns[28].Visible = false;
                datalistado.Columns[29].Visible = false;
                datalistado.Columns[30].Visible = false;
                datalistado.Columns[31].Visible = false;
                datalistado.Columns[32].Visible = false;
                datalistado.Columns[33].Visible = false;
                datalistado.Columns[34].Visible = false;
                datalistado.Columns[35].Visible = false;
                datalistado.Columns[36].Visible = false;
                datalistado.Columns[37].Visible = false;
                datalistado.Columns[38].Visible = false;
                datalistado.Columns[39].Visible = false;
                datalistado.Columns[40].Visible = false;
                datalistado.Columns[41].Visible = false;
                datalistado.Columns[42].Visible = false;
                datalistado.Columns[43].Visible = false;
                datalistado.Columns[44].Visible = false;
                datalistado.Columns[45].Visible = false;
                datalistado.Columns[46].Visible = false;
                datalistado.Columns[47].Visible = false;
                datalistado.Columns[48].Visible = false;
                datalistado.Columns[49].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //---------------------------------------------
        //SELECCION DE UN PRODUCTO
        private void datalistado_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            idart = Convert.ToInt32(datalistado.SelectedCells[0].Value.ToString());
            MostrarSegunCamposSeleccionados(idart);

            if (datalistadoCamposSeleccionados.RowCount > 0)
            {
                try
                {
                    //CAMPOS PRINCIPALES
                    txtDetalleProducto.Text = datalistado.SelectedCells[3].Value.ToString();
                    lblCodigoProducto.Text = datalistado.SelectedCells[1].Value.ToString();
                    cboTipoMedida.SelectedValue = datalistado.SelectedCells[4].Value.ToString();
                    cboTipoMercaderia.SelectedValue = datalistado.SelectedCells[6].Value.ToString();
                    cboLineas.SelectedValue = datalistado.SelectedCells[8].Value.ToString();
                    cboModelos.SelectedValue = datalistado.SelectedCells[10].Value.ToString();
                    cboDiferencial.SelectedValue = datalistado.SelectedCells[12].Value.ToString();

                    //CAMPOS QUE SE GUARDAN - GRUPOS DE CAMPOS CARACTERISTICAS 1
                    CampCaracteristicas1 = Convert.ToInt32(datalistadoCamposSeleccionados.SelectedCells[1].Value.ToString());
                    if (CampCaracteristicas1 == 1)
                    {
                        ckCamposCaracteristicas1.Checked = true;
                    }
                    else
                    {
                        ckCamposCaracteristicas1.Checked = false;
                    }

                    //CAMPOS QUE SE GUARDAN - GRUPOS DE CAMPOS CARACTERISTICAS 2
                    CampCaracteristicas2 = Convert.ToInt32(datalistadoCamposSeleccionados.SelectedCells[2].Value.ToString());
                    if (CampCaracteristicas2 == 1)
                    {
                        ckCamposCaracteristicas2.Checked = true;
                    }
                    else
                    {
                        ckCamposCaracteristicas2.Checked = false;
                    }

                    //CAMPOS QUE SE GUARDAN - GRUPOS DE CAMPOS MEDIDAS 1
                    CampMedidas1 = Convert.ToInt32(datalistadoCamposSeleccionados.SelectedCells[3].Value.ToString());
                    if (CampMedidas1 == 1)
                    {
                        ckCamposMedida1.Checked = true;
                    }
                    else
                    {
                        ckCamposMedida1.Checked = false;
                    }

                    //CAMPOS QUE SE GUARDAN - GRUPOS DE CAMPOS MEDIDAS 2
                    CampMedidas2 = Convert.ToInt32(datalistadoCamposSeleccionados.SelectedCells[4].Value.ToString());
                    if (CampMedidas2 == 1)
                    {
                        ckCamposMedida2.Checked = true;
                    }
                    else
                    {
                        ckCamposMedida2.Checked = false;
                    }

                    //CAMPOS QUE SE GUARDAN - GRUPOS DE CAMPOS DIAMETROS 1
                    CampDiametros1 = Convert.ToInt32(datalistadoCamposSeleccionados.SelectedCells[5].Value.ToString());
                    if (CampDiametros1 == 1)
                    {
                        ckCamposDiametros1.Checked = true;
                    }
                    else
                    {
                        ckCamposDiametros1.Checked = false;
                    }

                    //CAMPOS QUE SE GUARDAN - GRUPOS DE CAMPOS DIAMETROS 2
                    CampDiametros2 = Convert.ToInt32(datalistadoCamposSeleccionados.SelectedCells[6].Value.ToString());
                    if (CampDiametros2 == 1)
                    {
                        ckCamposDiametros2.Checked = true;
                    }
                    else
                    {
                        ckCamposDiametros2.Checked = false;
                    }

                    //CAMPOS QUE SE GUARDAN - GRUPOS DE CAMPOS FORMAS 1
                    CampFormas1 = Convert.ToInt32(datalistadoCamposSeleccionados.SelectedCells[7].Value.ToString());
                    if (CampFormas1 == 1)
                    {
                        ckCamposFormas1.Checked = true;
                    }
                    else
                    {
                        ckCamposFormas1.Checked = false;
                    }

                    //CAMPOS QUE SE GUARDAN - GRUPOS DE CAMPOS FORMAS 2
                    CampFormas2 = Convert.ToInt32(datalistadoCamposSeleccionados.SelectedCells[8].Value.ToString());
                    if (CampFormas2 == 1)
                    {
                        ckCamposFormas2.Checked = true;
                    }
                    else
                    {
                        ckCamposFormas2.Checked = false;
                    }

                    //CAMPOS QUE SE GUARDAN - GRUPOS DE CAMPOS ESPESORES 1
                    CampEspesores1 = Convert.ToInt32(datalistadoCamposSeleccionados.SelectedCells[9].Value.ToString());
                    if (CampEspesores1 == 1)
                    {
                        ckCamposEspesores1.Checked = true;
                    }
                    else
                    {
                        ckCamposEspesores1.Checked = false;
                    }

                    //CAMPOS QUE SE GUARDAN - GRUPOS DE CAMPOS ESPESORES 2
                    CampEspesores2 = Convert.ToInt32(datalistadoCamposSeleccionados.SelectedCells[10].Value.ToString());
                    if (CampEspesores2 == 1)
                    {
                        ckCamposEspesores2.Checked = true;
                    }
                    else
                    {
                        ckCamposEspesores2.Checked = false;
                    }

                    //CAMPOS QUE SE GUARDAN - GRUPOS DE CAMPOS DISEÑO ACABADO 1
                    CampDiseñoAcabado1 = Convert.ToInt32(datalistadoCamposSeleccionados.SelectedCells[11].Value.ToString());
                    if (CampDiseñoAcabado1 == 1)
                    {
                        ckCamposDiseñoAcabado1.Checked = true;
                    }
                    else
                    {
                        ckCamposDiseñoAcabado1.Checked = false;
                    }

                    //CAMPOS QUE SE GUARDAN - GRUPOS DE CAMPOS DISEÑO ACABADO 1
                    CampDiseñoAcabado2 = Convert.ToInt32(datalistadoCamposSeleccionados.SelectedCells[12].Value.ToString());
                    if (CampDiseñoAcabado2 == 1)
                    {
                        ckCamposDiseñoAcabado2.Checked = true;
                    }
                    else
                    {
                        ckCamposDiseñoAcabado2.Checked = false;
                    }

                    //CAMPOS QUE SE GUARDAN - GRUPOS DE CAMPOS N Y TIPOS 1
                    CampNTipos1 = Convert.ToInt32(datalistadoCamposSeleccionados.SelectedCells[13].Value.ToString());
                    if (CampNTipos1 == 1)
                    {
                        ckCamposNTipos1.Checked = true;
                    }
                    else
                    {
                        ckCamposNTipos1.Checked = false;
                    }

                    //CAMPOS QUE SE GUARDAN - GRUPOS DE CAMPOS N Y TIPOS 2
                    CampNTipos2 = Convert.ToInt32(datalistadoCamposSeleccionados.SelectedCells[14].Value.ToString());
                    if (CampNTipos2 == 1)
                    {
                        ckCamposNTipos2.Checked = true;
                    }
                    else
                    {
                        ckCamposNTipos2.Checked = false;
                    }

                    //CAMPOS QUE SE GUARDAN - GRUPOS DE CAMPOS VARIOS 0 1
                    CampVarios1 = Convert.ToInt32(datalistadoCamposSeleccionados.SelectedCells[15].Value.ToString());
                    if (CampVarios1 == 1)
                    {
                        ckVariosO1.Checked = true;
                    }
                    else
                    {
                        ckVariosO1.Checked = false;
                    }

                    //CAMPOS QUE SE GUARDAN - GRUPOS DE CAMPOS VARIOS 0 2
                    CampVarios2 = Convert.ToInt32(datalistadoCamposSeleccionados.SelectedCells[16].Value.ToString());
                    if (CampVarios2 == 1)
                    {
                        ckVariosO2.Checked = true;
                    }
                    else
                    {
                        ckVariosO2.Checked = false;
                    }

                    //CAMPOS QUE SE GUARDAN - CAMPO GENERAL
                    CampGeneral = Convert.ToInt32(datalistadoCamposSeleccionados.SelectedCells[17].Value.ToString());
                    if (CampGeneral == 1)
                    {
                        ckGeneral.Checked = true;
                    }
                    else
                    {
                        ckGeneral.Checked = false;
                    }

                    //CAMPOS - DATOS ANEXOS
                    afectadoIGV = Convert.ToInt32(datalistado.SelectedCells[14].Value.ToString());
                    if (afectadoIGV == 1) { ckAfectoIGV.Checked = true; } else { ckAfectoIGV.Checked = false; }

                    controlarstock = Convert.ToInt32(datalistado.SelectedCells[15].Value.ToString());
                    if (controlarstock == 1) { ckControlarStock.Checked = true; } else { ckControlarStock.Checked = false; }

                    juego = Convert.ToInt32(datalistado.SelectedCells[16].Value.ToString());
                    if (juego == 1) { ckJuego.Checked = true; } else { ckJuego.Checked = false; }

                    servicio = Convert.ToInt32(datalistado.SelectedCells[17].Value.ToString());
                    if (servicio == 1) { ckServicio.Checked = true; } else { ckServicio.Checked = false; }

                    controlarlotes = Convert.ToInt32(datalistado.SelectedCells[18].Value.ToString());
                    if (controlarlotes == 1) { ckControlarLote.Checked = true; } else { ckControlarLote.Checked = false; }

                    controlarserie = Convert.ToInt32(datalistado.SelectedCells[19].Value.ToString());
                    if (controlarserie == 1) { ckControlarSerie.Checked = true; } else { ckControlarSerie.Checked = false; }

                    txtPeso.Text = datalistado.SelectedCells[20].Value.ToString();

                    txtUbicacion.Text = datalistado.SelectedCells[21].Value.ToString();

                    reposicion = Convert.ToInt32(datalistado.SelectedCells[22].Value.ToString());
                    if (reposicion == 1) { ckReposicion.Checked = true; } else { ckReposicion.Checked = false; }

                    txtMinimo.Text = datalistado.SelectedCells[23].Value.ToString();
                    txtMaximo.Text = datalistado.SelectedCells[24].Value.ToString();

                    cboTipoExistencia.SelectedValue = datalistado.SelectedCells[25].Value.ToString();

                    txtCodigoUNSPCS.Text = datalistado.SelectedCells[28].Value.ToString();

                    sujetropercepcion = Convert.ToInt32(datalistado.SelectedCells[29].Value.ToString());
                    if (sujetropercepcion == 1) { ckSujetoPercepcion.Checked = true; } else { ckSujetoPercepcion.Checked = false; }

                    txtPorcentajePercepcion.Text = datalistado.SelectedCells[30].Value.ToString();

                    sujetodetraccion = Convert.ToInt32(datalistado.SelectedCells[31].Value.ToString());
                    if (sujetodetraccion == 1) { skSujetoDetraccion.Checked = true; } else { skSujetoDetraccion.Checked = false; }

                    txtPorcentajeDetraccion.Text = datalistado.SelectedCells[32].Value.ToString();

                    sujetoisc = Convert.ToInt32(datalistado.SelectedCells[33].Value.ToString());
                    if (sujetoisc == 1) { ckSujetoISC.Checked = true; } else { ckSujetoISC.Checked = false; }

                    txtPorcentajeISC.Text = datalistado.SelectedCells[34].Value.ToString();

                    cboBienesSujetoPercepcion.SelectedValue = datalistado.SelectedCells[35].Value.ToString();

                    cboOrigen.SelectedValue = datalistado.SelectedCells[38].Value.ToString();

                    txtContenedor.Text = datalistado.SelectedCells[41].Value.ToString();

                    txtPesoContenedor.Text = datalistado.SelectedCells[42].Value.ToString();

                    txtMedidas.Text = datalistado.SelectedCells[43].Value.ToString();

                    cboTerminosCompra.SelectedValue = datalistado.SelectedCells[44].Value.ToString();

                    int semiproducido = Convert.ToInt32(datalistado.SelectedCells[47].Value.ToString());
                    if (semiproducido == 1)
                    {
                        ckSemiProducido.Checked = true;
                    }
                    else
                    {
                        ckSemiProducido.Checked = false;
                    }

                    txtAnotaciones.Text = datalistado.SelectedCells[48].Value.ToString();

                    string rutaImagen = datalistado.SelectedCells[49].Value.ToString();

                    if (rutaImagen == "")
                    {
                        imgProductoSeleccioandoSinImagen.SizeMode = PictureBoxSizeMode.Zoom;
                        imgProductoSeleccioandoSinImagen.BackgroundImage = Image.FromFile(@"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Procesos\Predeterminado\caja2.png");
                        imgProductoSeleccioandoSinImagen.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    else
                    {
                        imgProductoSeleccioandoSinImagen.SizeMode = PictureBoxSizeMode.Zoom;
                        imgProductoSeleccioandoSinImagen.BackgroundImage = Image.FromFile(rutaImagen);

                    }

                    //TRAER PLANOS SEGUN EL PRODUCTO SELECCIONADO
                    MostrarSegunId(idart);
                    AbreviaturaSegunRegistroSeleccioando(cboModelos.SelectedValue.ToString());
                    GeneracionReferenciaPlano();

                    if (datalistadopdf.Rows.Count > 0)
                    {
                        btnEliminarPlano.Visible = true;
                        lblEliminarPlano.Visible = true;
                    }
                    else
                    {
                        btnEliminarPlano.Visible = false;
                        lblEliminarPlano.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " " + "Ocurrió un error inesperado al momento de cargar los datos, por favor comunicar al administrador.", "Validación del Sistema", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Ocurrió un error inesperado al momento de cargar los datos, por favor comunicar al administrador.", "Validación del Sistema", MessageBoxButtons.OK);
            }

            txtFile.Text = "";

            btnConfirmarPlano.Visible = false;
            btnCancelarPlano.Visible = false;

            btnAgregarPlano.Visible = true;
            btnAbrirPdf.Visible = true;
        }

        //METODO QUE REALIZARA LA VISUALIZACION DE LA IMAGEN DEL PRODUCTO
        public void VisualizarImagenProducto(DataGridView DGV, string rutaimagen)
        {
            try
            {
                if (DGV.CurrentRow != null)
                {
                    string ruta = DGV.SelectedCells[49].Value.ToString();
                    string ruta2 = rutaimagen;

                    if (ruta == "" && ruta2 == "")
                    {
                        MessageBox.Show("No se encontró una imagen referente al producto.", "Abrir Imagen");
                    }
                    else if (ruta != "")
                    {
                        Process.Start(ruta);
                    }
                    else if (ruta2 != "")
                    {
                        Process.Start(ruta2);
                    }
                    else
                    {
                        MessageBox.Show("Error al monento de intentar cargar la imagen.", "Abrir Imagen");
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione un producto para poder ver la imagen.", "Abrir Imagen");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //VISUALIZACION DE LA IMAGEN DEL PRODUCTO
        private void btnVisualizarImagenProducto_Click(object sender, EventArgs e)
        {
            VisualizarImagenProducto(datalistado, txtRutaImagen.Text);
        }

        //BOTON PARA CARGAR UNA IMAGEN AL PRODUCTO
        private void btnCargarImagenProducto_Click(object sender, EventArgs e)
        {
            if (idart != 0)
            {
                openFileDialog2.InitialDirectory = "c:\\";
                openFileDialog2.Filter = "Todos los archivos (*.*)|*.*";
                openFileDialog2.FilterIndex = 1;
                openFileDialog2.RestoreDirectory = true;

                if (openFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        txtRutaImagen.Text = openFileDialog2.FileName;

                        btnCargarImagenProducto.Visible = false;
                        lblCargarImagen.Visible = false;
                        btnConfirmarImagenProducto.Visible = true;
                        btnCancelarImagenProducto.Visible = true;

                        imgProductoSeleccioandoSinImagen.BackgroundImage = Image.FromFile(txtRutaImagen.Text);
                        imgProductoSeleccioandoSinImagen.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe elegir un producto para poder agregar una imagen.", "Agregar Nueva Imagen", MessageBoxButtons.OK);
            }
        }

        //BOTON PARA GUARDAR LA IMAGEN ADJUNTADA A MI PRODUCTO SELECCIONADO
        public void AgregarImagenProducto(string codigoproducto, string rutaimagen)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea asignar esta imagen a este producto.", "Validación del Sistema", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                try
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("ListadoProductos_InsertarImagen", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    string fechaHora = " Hora " + Convert.ToString(DateTime.Now.Hour) + " Minuto " + Convert.ToString(DateTime.Now.Minute);
                    string nombreGenerado = "IMAGEN REFERENCIAL N - " + codigoproducto + fechaHora;
                    string rutaOld = rutaimagen;
                    string RutaNew = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Procesos\Productos\Imagenes\" + nombreGenerado + ".jpg";

                    File.Copy(rutaOld, RutaNew);
                    cmd.Parameters.AddWithValue("@iamgenProducto", RutaNew);
                    cmd.Parameters.AddWithValue("@codigo", codigoproducto);

                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Se ingresó la imagen de manera correcta.", "Validación del Sistema");
                    btnCargarImagenProducto.Visible = true;
                    lblCargarImagen.Visible = true;
                    btnConfirmarImagenProducto.Visible = false;
                    btnCancelarImagenProducto.Visible = false;
                    MostrarProductos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error en el servidor");
                }
            }
        }

        //BOTON PARA GUARDAR LA IMAGEN ADJUNTADA A MI PRODUCTO SELECCIONADO
        private void btnConfirmarImagenProducto_Click(object sender, EventArgs e)
        {
            AgregarImagenProducto(lblCodigoProducto.Text, txtRutaImagen.Text);
        }

        //BOTON PARA CANCELAR LA CARGA DE UNA IMAGEN
        private void btnCancelarImagenProducto_Click(object sender, EventArgs e)
        {
            imgProductoSeleccioandoSinImagen.BackgroundImage = Image.FromFile(@"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Procesos\Predeterminado\caja.png");
            txtRutaImagen.Text = "";
            txtRutaImagen.Text = "";
            imgProductoSeleccioandoSinImagen.SizeMode = PictureBoxSizeMode.Zoom;
            btnConfirmarImagenProducto.Visible = false;
            btnCancelarImagenProducto.Visible = false;
            btnCargarImagenProducto.Visible = true;
            lblCargarImagen.Visible = true;
        }

        //ACCIONES DE CARGA INTERNA---------------------------------------------------------------------
        //CARGA Y ALMACENAMIENTO DE LA LÍNEA SELECCIOANDA
        private void cboLineas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLineas.SelectedValue != null)
            {
                idlinea = cboLineas.SelectedValue.ToString();
                CargarModelos(idlinea);
            }
        }

        //CARGA DE CAMPOS AL SELECCIONAR EL PRODUCTO-------------------------------------------
        //CARGAR CARACTERISTICAS---------------------------------
        //TIPO CARACTERISTICAS 1
        private void cboTipoCaracteristicas1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoCaracteristicas1.SelectedValue != null)
            {
                idtipocaracteristica = cboTipoCaracteristicas1.SelectedValue.ToString();
                CargarDescripcionCaracteristicas(cboDescripcionCaracteristicas1, idtipocaracteristica, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO CARACTERISTICAS 2
        private void cboTipoCaracteristicas2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoCaracteristicas2.SelectedValue != null)
            {
                idtipocaracteristica = cboTipoCaracteristicas2.SelectedValue.ToString();
                CargarDescripcionCaracteristicas(cboDescripcionCaracteristicas2, idtipocaracteristica, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO CARACTERISTICAS 3
        private void cboTipoCaracteristicas3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoCaracteristicas3.SelectedValue != null)
            {
                idtipocaracteristica = cboTipoCaracteristicas3.SelectedValue.ToString();
                CargarDescripcionCaracteristicas(cboDescripcionCaracteristicas3, idtipocaracteristica, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO CARACTERISTICAS 4
        private void cboTipoCaracteristicas4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoCaracteristicas4.SelectedValue != null)
            {
                idtipocaracteristica = cboTipoCaracteristicas4.SelectedValue.ToString();
                CargarDescripcionCaracteristicas(cboDescripcionCaracteristicas4, idtipocaracteristica, cboModelos.SelectedValue.ToString());
            }
        }

        //CARGAR MEDIDAS---------------------------------
        //TIPO MEDIDA 1
        private void cboTipoMedidas1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoMedidas1.SelectedValue != null)
            {
                idtipomedida = cboTipoMedidas1.SelectedValue.ToString();
                CargarDescripcionMedidas(cboDescripcionMedidas1, idtipomedida, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO MEDIDA 2
        private void cboTipoMedidas2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoMedidas2.SelectedValue != null)
            {
                idtipomedida = cboTipoMedidas2.SelectedValue.ToString();
                CargarDescripcionMedidas(cboDescripcionMedidas2, idtipomedida, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO MEDIDA 3
        private void cboTipoMedidas3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoMedidas3.SelectedValue != null)
            {
                idtipomedida = cboTipoMedidas3.SelectedValue.ToString();
                CargarDescripcionMedidas(cboDescripcionMedidas3, idtipomedida, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO MEDIDA 4
        private void cboTipoMedidas4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoMedidas4.SelectedValue != null)
            {
                idtipomedida = cboTipoMedidas4.SelectedValue.ToString();
                CargarDescripcionMedidas(cboDescripcionMedidas4, idtipomedida, cboModelos.SelectedValue.ToString());
            }
        }

        //CARGAR DIÁMETROS---------------------------------
        //TIPO DIAMETRO 1
        private void cboTiposDiametros1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiametros1.SelectedValue != null)
            {
                iddiametros = cboTiposDiametros1.SelectedValue.ToString();
                CargarDescripcionDiametros(cboDescripcionDiametros1, iddiametros, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO DIAMETRO 2
        private void cboTiposDiametros2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiametros2.SelectedValue != null)
            {
                iddiametros = cboTiposDiametros2.SelectedValue.ToString();
                CargarDescripcionDiametros(cboDescripcionDiametros2, iddiametros, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO DIAMETRO 3
        private void cboTiposDiametros3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiametros3.SelectedValue != null)
            {
                iddiametros = cboTiposDiametros3.SelectedValue.ToString();
                CargarDescripcionDiametros(cboDescripcionDiametros3, iddiametros, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO DIAMETRO 4
        private void cboTiposDiametros4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiametros4.SelectedValue != null)
            {
                iddiametros = cboTiposDiametros4.SelectedValue.ToString();
                CargarDescripcionDiametros(cboDescripcionDiametros4, iddiametros, cboModelos.SelectedValue.ToString());
            }
        }

        //CARGAR FORMAS---------------------------------
        //TIPO FORMAS 1
        private void cboTiposFormas1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposFormas1.SelectedValue != null)
            {
                idformas = cboTiposFormas1.SelectedValue.ToString();
                CargarDescripcionFormas(cboDescripcionFormas1, idformas, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO FORMAS 2
        private void cboTiposFormas2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposFormas2.SelectedValue != null)
            {
                idformas = cboTiposFormas2.SelectedValue.ToString();
                CargarDescripcionFormas(cboDescripcionFormas2, idformas, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO FORMAS 3
        private void cboTiposFormas3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposFormas3.SelectedValue != null)
            {
                idformas = cboTiposFormas3.SelectedValue.ToString();
                CargarDescripcionFormas(cboDescripcionFormas3, idformas, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO FORMAS 4
        private void cboTiposFormas4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposFormas4.SelectedValue != null)
            {
                idformas = cboTiposFormas4.SelectedValue.ToString();
                CargarDescripcionFormas(cboDescripcionFormas4, idformas, cboModelos.SelectedValue.ToString());
            }
        }

        //CARGAR ESPESORES---------------------------------
        //TIPO ESPESORES 1
        private void cbooTipoEspesores1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbooTipoEspesores1.SelectedValue != null)
            {
                idespesores = cbooTipoEspesores1.SelectedValue.ToString();
                CargarDescripcionEspesores(cboDescripcionEspesores1, idespesores, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO ESPESORES 2
        private void cbooTipoEspesores2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbooTipoEspesores2.SelectedValue != null)
            {
                idespesores = cbooTipoEspesores2.SelectedValue.ToString();
                CargarDescripcionEspesores(cboDescripcionEspesores2, idespesores, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO ESPESORES 3
        private void cbooTipoEspesores3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbooTipoEspesores3.SelectedValue != null)
            {
                idespesores = cbooTipoEspesores3.SelectedValue.ToString();
                CargarDescripcionEspesores(cboDescripcionEspesores3, idespesores, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO ESPESORES 4
        private void cbooTipoEspesores4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbooTipoEspesores4.SelectedValue != null)
            {
                idespesores = cbooTipoEspesores4.SelectedValue.ToString();
                CargarDescripcionEspesores(cboDescripcionEspesores4, idespesores, cboModelos.SelectedValue.ToString());
            }
        }

        //CARGAR DISEÑO Y ACABADOS---------------------------------
        //TIPO DISEÑO Y ACABADOS 1
        private void cboTiposDiseñoAcabado1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiseñoAcabado1.SelectedValue != null)
            {
                iddiseñoacabado = cboTiposDiseñoAcabado1.SelectedValue.ToString();
                CargarDescripcionDiseñoAcabado(cboDescripcionDiseñoAcabado1, iddiseñoacabado, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO DISEÑO Y ACABADOS 2
        private void cboTiposDiseñoAcabado2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiseñoAcabado2.SelectedValue != null)
            {
                iddiseñoacabado = cboTiposDiseñoAcabado2.SelectedValue.ToString();
                CargarDescripcionDiseñoAcabado(cboDescripcionDiseñoAcabado2, iddiseñoacabado, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO DISEÑO Y ACABADOS 3
        private void cboTiposDiseñoAcabado3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiseñoAcabado3.SelectedValue != null)
            {
                iddiseñoacabado = cboTiposDiseñoAcabado3.SelectedValue.ToString();
                CargarDescripcionDiseñoAcabado(cboDescripcionDiseñoAcabado3, iddiseñoacabado, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO DISEÑO Y ACABADOS 4
        private void cboTiposDiseñoAcabado4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposDiseñoAcabado4.SelectedValue != null)
            {
                iddiseñoacabado = cboTiposDiseñoAcabado4.SelectedValue.ToString();
                CargarDescripcionDiseñoAcabado(cboDescripcionDiseñoAcabado4, iddiseñoacabado, cboModelos.SelectedValue.ToString());
            }
        }

        //CARGAR TIPOS Y NTIPOS---------------------------------
        //TIPO TIPOS Y NTIPOS 1
        private void cboTiposNTipos1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposNTipos1.SelectedValue != null)
            {
                idntipos = cboTiposNTipos1.SelectedValue.ToString();
                CargarDescripcionNTipos(cboDescripcionNTipos1, idntipos, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO TIPOS Y NTIPOS 2
        private void cboTiposNTipos2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposNTipos2.SelectedValue != null)
            {
                idntipos = cboTiposNTipos2.SelectedValue.ToString();
                CargarDescripcionNTipos(cboDescripcionNTipos2, idntipos, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO TIPOS Y NTIPOS 3
        private void cboTiposNTipos3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposNTipos3.SelectedValue != null)
            {
                idntipos = cboTiposNTipos3.SelectedValue.ToString();
                CargarDescripcionNTipos(cboDescripcionNTipos3, idntipos, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO TIPOS Y NTIPOS 4
        private void cboTiposNTipos4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposNTipos4.SelectedValue != null)
            {
                idntipos = cboTiposNTipos4.SelectedValue.ToString();
                CargarDescripcionNTipos(cboDescripcionNTipos4, idntipos, cboModelos.SelectedValue.ToString());
            }
        }

        //CARGAR VARIOS0---------------------------------
        //TIPO VARIOS0 1
        private void cboTiposVariosO1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposVariosO1.SelectedValue != null)
            {
                idvarioso = cboTiposVariosO1.SelectedValue.ToString();
                CargarDescripcionVariosO(cboDescripcionVariosO1, idvarioso, cboModelos.SelectedValue.ToString());
            }
        }

        //TIPO VARIOS0 2
        private void cboTiposVariosO2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTiposVariosO2.SelectedValue != null)
            {
                idvarioso = cboTiposVariosO2.SelectedValue.ToString();
                CargarDescripcionVariosO(cboDescripcionVariosO2, idvarioso, cboModelos.SelectedValue.ToString());
            }
        }
        //--------------------------------------------------------------------------------------

        //CARGA DE PLANOS
        public void MostrarSegunId(int id)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarPlanoPorId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idart", id);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadopdf.DataSource = dt;
            con.Close();
            datalistadopdf.Columns[0].Visible = false;
            datalistadopdf.Columns[1].Width = 70;
            datalistadopdf.Columns[2].Width = 350;
        }

        //CAGA DE CAMPOS SELECCIOANDOS
        public void MostrarSegunCamposSeleccionados(int id)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("ListadoProductos_MostrarCamposSeleccionados", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idart", id);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoCamposSeleccionados.DataSource = dt;
            con.Close();
        }
        //---------------------------------------------------------------------------------------

        //ACCIONES DE LOS BOTONES DE FUNCIONALIDAD------------------------------------------------------
        //BOTÓN PARA AGREGAR NUEVO PRODUCTO
        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            Productos.AgregarProducto Agregar = new Productos.AgregarProducto();
            Agregar.Show();
        }

        //BOTÓN PARA REFRESCAR EL LISTADO DE PRODUCTOS
        private void btnCargarProductos_Click(object sender, EventArgs e)
        {
            MostrarProductos();
        }

        //BOTÓN PARA EDITAR EL PRODUCTO SELECCIOANDO
        private void btnEditarProducto_Click(object sender, EventArgs e)
        {
            if (idart != 0)
            {
                //BLOQUEAR EL CAMBIO DE REGISTRO
                datalistado.Enabled = false;
                //CARACTERISTICAS - TIPOS Y DESCRIPCION - 1 Y 2
                if (ckCamposCaracteristicas1.Checked == true)
                {
                    flowLayoutPanel.Controls.Add(panelCamposCaracteristicas1);
                    CargarTiposCaracteriticas(cboTipoCaracteristicas1);
                    CargarTiposCaracteriticas(cboTipoCaracteristicas2);
                    CargarListadoCaracteristicas1();
                    cboTipoCaracteristicas1.SelectedValue = datalistadoCaracteristicas1.SelectedCells[1].Value.ToString();
                    cboDescripcionCaracteristicas1.SelectedValue = datalistadoCaracteristicas1.SelectedCells[2].Value.ToString();
                    cboTipoCaracteristicas2.SelectedValue = datalistadoCaracteristicas1.SelectedCells[3].Value.ToString();
                    cboDescripcionCaracteristicas2.SelectedValue = datalistadoCaracteristicas1.SelectedCells[4].Value.ToString();
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposCaracteristicas1);
                }

                //CARACTERISTICAS - TIPOS Y DESCRIPCION - 3 Y 4
                if (ckCamposCaracteristicas2.Checked == true)
                {
                    flowLayoutPanel.Controls.Add(panelCamposCaracteristicas2);
                    CargarTiposCaracteriticas(cboTipoCaracteristicas3);
                    CargarTiposCaracteriticas(cboTipoCaracteristicas4);
                    CargarListadoCaracteristicas2();
                    cboTipoCaracteristicas3.SelectedValue = datalistadoCaracteristicas2.SelectedCells[1].Value.ToString();
                    cboDescripcionCaracteristicas3.SelectedValue = datalistadoCaracteristicas2.SelectedCells[2].Value.ToString();
                    cboTipoCaracteristicas4.SelectedValue = datalistadoCaracteristicas2.SelectedCells[3].Value.ToString();
                    cboDescripcionCaracteristicas4.SelectedValue = datalistadoCaracteristicas2.SelectedCells[4].Value.ToString();
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposCaracteristicas2);
                }

                //MEDIDAS - TIPOS Y DESCRIPCION - 1 Y 2
                if (ckCamposMedida1.Checked == true)
                {
                    flowLayoutPanel.Controls.Add(panelCamposMedidas1);
                    CargarTiposMedidas(cboTipoMedidas1);
                    CargarTiposMedidas(cboTipoMedidas2);
                    CargarListadoMedidas1();
                    cboTipoMedidas1.SelectedValue = datalistadoMedidas1.SelectedCells[1].Value.ToString();
                    cboDescripcionMedidas1.SelectedValue = datalistadoMedidas1.SelectedCells[2].Value.ToString();
                    cboTipoMedidas2.SelectedValue = datalistadoMedidas1.SelectedCells[3].Value.ToString();
                    cboDescripcionMedidas2.SelectedValue = datalistadoMedidas1.SelectedCells[4].Value.ToString();
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposMedidas1);
                }

                //MEDIDAS - TIPOS Y DESCRIPCION - 3 Y 4
                if (ckCamposMedida2.Checked == true)
                {
                    flowLayoutPanel.Controls.Add(panelCamposMedidas2);
                    CargarTiposMedidas(cboTipoMedidas3);
                    CargarTiposMedidas(cboTipoMedidas4);
                    CargarListadoMedidas2();
                    cboTipoMedidas3.SelectedValue = datalistadoMedidas2.SelectedCells[1].Value.ToString();
                    cboDescripcionMedidas3.SelectedValue = datalistadoMedidas2.SelectedCells[2].Value.ToString();
                    cboTipoMedidas4.SelectedValue = datalistadoMedidas2.SelectedCells[3].Value.ToString();
                    cboDescripcionMedidas4.SelectedValue = datalistadoMedidas2.SelectedCells[4].Value.ToString();
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposMedidas2);
                }

                //DIAMETROS - TIPOS Y DESCRIPCION - 1 Y 2
                if (ckCamposDiametros1.Checked == true)
                {
                    flowLayoutPanel.Controls.Add(panelCamposDiametros1);
                    CargarTiposDiametros(cboTiposDiametros1);
                    CargarTiposDiametros(cboTiposDiametros2);
                    CargarListadoDiametros1();
                    cboTiposDiametros1.SelectedValue = datalistadoDiametros1.SelectedCells[1].Value.ToString();
                    cboDescripcionDiametros1.SelectedValue = datalistadoDiametros1.SelectedCells[2].Value.ToString();
                    cboTiposDiametros2.SelectedValue = datalistadoDiametros1.SelectedCells[3].Value.ToString();
                    cboDescripcionDiametros2.SelectedValue = datalistadoDiametros1.SelectedCells[4].Value.ToString();
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposDiametros1);
                }

                //DIAMETROS - TIPOS Y DESCRIPCION - 3 Y 4
                if (ckCamposDiametros2.Checked == true)
                {
                    flowLayoutPanel.Controls.Add(panelCamposDiametros2);
                    CargarTiposDiametros(cboTiposDiametros3);
                    CargarTiposDiametros(cboTiposDiametros4);
                    CargarListadoDiametros2();
                    cboTiposDiametros3.SelectedValue = datalistadoDiametros2.SelectedCells[1].Value.ToString();
                    cboDescripcionDiametros3.SelectedValue = datalistadoDiametros2.SelectedCells[2].Value.ToString();
                    cboTiposDiametros4.SelectedValue = datalistadoDiametros2.SelectedCells[3].Value.ToString();
                    cboDescripcionDiametros4.SelectedValue = datalistadoDiametros2.SelectedCells[4].Value.ToString();
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposDiametros2);
                }

                //FORMAS - TIPOS Y DESCRIPCION - 1 Y 2
                if (ckCamposFormas1.Checked == true)
                {
                    flowLayoutPanel.Controls.Add(panelCamposFormas1);
                    CargarTiposFormas(cboTiposFormas1);
                    CargarTiposFormas(cboTiposFormas2);
                    CargarListadoFormas1();
                    cboTiposFormas1.SelectedValue = datalistadoFormas1.SelectedCells[1].Value.ToString();
                    cboDescripcionFormas1.SelectedValue = datalistadoFormas1.SelectedCells[2].Value.ToString();
                    cboTiposFormas2.SelectedValue = datalistadoFormas1.SelectedCells[3].Value.ToString();
                    cboDescripcionFormas2.SelectedValue = datalistadoFormas1.SelectedCells[4].Value.ToString();
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposFormas1);
                }

                //FORMAS - TIPOS Y DESCRIPCION - 3 Y 4
                if (ckCamposFormas2.Checked == true)
                {
                    flowLayoutPanel.Controls.Add(panelCamposFormas2);
                    CargarTiposFormas(cboTiposFormas3);
                    CargarTiposFormas(cboTiposFormas4);
                    CargarListadoFormas2();
                    cboTiposFormas3.SelectedValue = datalistadoFormas2.SelectedCells[1].Value.ToString();
                    cboDescripcionFormas3.SelectedValue = datalistadoFormas2.SelectedCells[2].Value.ToString();
                    cboTiposFormas4.SelectedValue = datalistadoFormas2.SelectedCells[3].Value.ToString();
                    cboDescripcionFormas4.SelectedValue = datalistadoFormas2.SelectedCells[4].Value.ToString();
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposFormas2);
                }

                //ESPESORES - TIPOS Y DESCRIPCION - 1 Y 2
                if (ckCamposEspesores1.Checked == true)
                {
                    flowLayoutPanel.Controls.Add(panelCamposEspesores1);
                    CargarTiposEspesores(cbooTipoEspesores1);
                    CargarTiposEspesores(cbooTipoEspesores2);
                    CargarListadoEspesores1();
                    cbooTipoEspesores1.SelectedValue = datalistadoEspesores1.SelectedCells[1].Value.ToString();
                    cboDescripcionEspesores1.SelectedValue = datalistadoEspesores1.SelectedCells[2].Value.ToString();
                    cbooTipoEspesores2.SelectedValue = datalistadoEspesores1.SelectedCells[3].Value.ToString();
                    cboDescripcionEspesores2.SelectedValue = datalistadoEspesores1.SelectedCells[4].Value.ToString();
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposEspesores1);
                }

                //ESPESORES - TIPOS Y DESCRIPCION - 3 Y 4
                if (ckCamposEspesores2.Checked == true)
                {
                    flowLayoutPanel.Controls.Add(panelCamposEspesores2);
                    CargarTiposEspesores(cbooTipoEspesores3);
                    CargarTiposEspesores(cbooTipoEspesores4);
                    CargarListadoEspesores2();
                    cbooTipoEspesores3.SelectedValue = datalistadoEspesores2.SelectedCells[1].Value.ToString();
                    cboDescripcionEspesores3.SelectedValue = datalistadoEspesores2.SelectedCells[2].Value.ToString();
                    cbooTipoEspesores4.SelectedValue = datalistadoEspesores2.SelectedCells[3].Value.ToString();
                    cboDescripcionEspesores4.SelectedValue = datalistadoEspesores2.SelectedCells[4].Value.ToString();
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposEspesores2);
                }

                //DISEÑO Y ACABADP - TIPOS Y DESCRIPCION - 1 Y 2
                if (ckCamposDiseñoAcabado1.Checked == true)
                {
                    flowLayoutPanel.Controls.Add(panelCamposDiseñoAcabado1);
                    CargarTiposDiseñoAcabado(cboTiposDiseñoAcabado1);
                    CargarTiposDiseñoAcabado(cboTiposDiseñoAcabado2);
                    CargarListadoDiseñoAcabado1();
                    cboTiposDiseñoAcabado1.SelectedValue = datalistadoDiseñoAcabado1.SelectedCells[1].Value.ToString();
                    cboDescripcionDiseñoAcabado1.SelectedValue = datalistadoDiseñoAcabado1.SelectedCells[2].Value.ToString();
                    cboTiposDiseñoAcabado2.SelectedValue = datalistadoDiseñoAcabado1.SelectedCells[3].Value.ToString();
                    cboDescripcionDiseñoAcabado2.SelectedValue = datalistadoDiseñoAcabado1.SelectedCells[4].Value.ToString();
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposDiseñoAcabado1);
                }

                //DISEÑO Y ACABADP - TIPOS Y DESCRIPCION - 3 Y 4
                if (ckCamposDiseñoAcabado2.Checked == true)
                {
                    flowLayoutPanel.Controls.Add(panelCamposDiseñoAcabado2);
                    CargarTiposDiseñoAcabado(cboTiposDiseñoAcabado3);
                    CargarTiposDiseñoAcabado(cboTiposDiseñoAcabado4);
                    CargarListadoDiseñoAcabado2();
                    cboTiposDiseñoAcabado3.SelectedValue = datalistadoDiseñoAcabado2.SelectedCells[1].Value.ToString();
                    cboDescripcionDiseñoAcabado3.SelectedValue = datalistadoDiseñoAcabado2.SelectedCells[2].Value.ToString();
                    cboTiposDiseñoAcabado4.SelectedValue = datalistadoDiseñoAcabado2.SelectedCells[3].Value.ToString();
                    cboDescripcionDiseñoAcabado4.SelectedValue = datalistadoDiseñoAcabado2.SelectedCells[4].Value.ToString();
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposDiseñoAcabado2);
                }

                //N Y TIPOS - TIPOS Y DESCRIPCION - 1 Y 2
                if (ckCamposNTipos1.Checked == true)
                {
                    flowLayoutPanel.Controls.Add(panelCamposNTipos1);
                    CargarTiposNTipos(cboTiposNTipos1);
                    CargarTiposNTipos(cboTiposNTipos2);
                    CargarListadoNTipos1();
                    cboTiposNTipos1.SelectedValue = DatalistadoNTipos1.SelectedCells[1].Value.ToString();
                    cboDescripcionNTipos1.SelectedValue = DatalistadoNTipos1.SelectedCells[2].Value.ToString();
                    cboTiposNTipos2.SelectedValue = DatalistadoNTipos1.SelectedCells[3].Value.ToString();
                    cboDescripcionNTipos2.SelectedValue = DatalistadoNTipos1.SelectedCells[4].Value.ToString();
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposNTipos1);
                }

                //N Y TIPOS - TIPOS Y DESCRIPCION - 3 Y 4
                if (ckCamposNTipos2.Checked == true)
                {
                    flowLayoutPanel.Controls.Add(panelCamposNTipos2);
                    CargarTiposNTipos(cboTiposNTipos3);
                    CargarTiposNTipos(cboTiposNTipos4);
                    CargarListadoNTipos2();
                    cboTiposNTipos3.SelectedValue = DatalistadoNTipos2.SelectedCells[1].Value.ToString();
                    cboDescripcionNTipos3.SelectedValue = DatalistadoNTipos2.SelectedCells[2].Value.ToString();
                    cboTiposNTipos4.SelectedValue = DatalistadoNTipos2.SelectedCells[3].Value.ToString();
                    cboDescripcionNTipos4.SelectedValue = DatalistadoNTipos2.SelectedCells[4].Value.ToString();
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposNTipos2);
                }

                //VARIOS Y 0 - TIPOS Y DESCRIPCION - 1
                if (ckVariosO1.Checked == true)
                {
                    flowLayoutPanel.Controls.Add(panelCamposVariosO1);
                    CargarTiposVariosO(cboTiposVariosO1);
                    CargarListadoVariosO1();
                    cboTiposVariosO1.SelectedValue = datalistadoVarios01.SelectedCells[1].Value.ToString();
                    cboDescripcionVariosO1.SelectedValue = datalistadoVarios01.SelectedCells[2].Value.ToString();
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposVariosO1);
                }

                //VARIOS Y 0 - TIPOS Y DESCRIPCION - 2
                if (ckVariosO2.Checked == true)
                {
                    flowLayoutPanel.Controls.Add(panelCamposVariosO2);
                    CargarTiposVariosO(cboTiposVariosO2);
                    CargarListadoVariosO2();
                    cboTiposVariosO2.SelectedValue = datalistadoVarios02.SelectedCells[1].Value.ToString();
                    cboDescripcionVariosO2.SelectedValue = datalistadoVarios02.SelectedCells[2].Value.ToString();
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposVariosO2);
                }

                //GENERAL
                if (ckGeneral.Checked == true)
                {
                    flowLayoutPanel.Controls.Add(panelCamposGeneral);
                    CargarListadoGenerales();
                    txtDescripcionGeneral.Text = datalistadoGeneral.SelectedCells[1].Value.ToString();
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposGeneral);
                }

                txtDescripcionGeneradaProducto.Text = txtDetalleProducto.Text;
                txtCodigoBSSEdicion.Text = txtAnotaciones.Text;
                ckSemiProducidoEdicion.Checked = Convert.ToBoolean(datalistado.SelectedCells[47].Value);
                panelCamposProducto.Visible = true;
            }
            else
            {
                MessageBox.Show("Debe seleccionar un producto para poder editar sus campos.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //PROCESO, METODOS Y FUNCIOANES ASOCIADAS AL PLANO, CRUD DEL PLANO---------
        public void VisualizarPlano(DataGridView DGV)
        {
            try
            {
                if (DGV.CurrentRow != null)
                {
                    string ruta = DGV.SelectedCells[2].Value.ToString();
                    if (ruta == "")
                    {
                        MessageBox.Show("Seleccione un plano para continuar.", "Abrir Plano");
                    }
                    else
                    {
                        Process.Start(ruta);
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione un plano para poder abrirlo.", "Abrir Plano");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //ABRIR PLANO GUARDARO O SELECCIOANDO
        private void btnAbrirPdf_Click(object sender, EventArgs e)
        {
            VisualizarPlano(datalistadopdf);
        }

        //AGREGAR UN PLANO NUEVO
        private void btnAgregarPlano_Click(object sender, EventArgs e)
        {
            if (idart != 0)
            {
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "Todos los archivos (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtFile.Text = openFileDialog1.FileName;
                    GeneracionReferenciaPlano();
                    btnAgregarPlano.Visible = false;
                    lblCargarPlano.Visible = false;
                    btnConfirmarPlano.Visible = true;
                    btnCancelarPlano.Visible = true;
                }
            }
            else
            {
                MessageBox.Show("Debe elegir un producto para poder agregar un nuevo plano.", "Agregar Nuevo Plano", MessageBoxButtons.OK);
            }
        }

        //METODO QUE GUARDA EL PLANO EN LA BASE DE DATOS RELACIONADO AL PRODUCTO    
        public void AgregarPlano(string codigoproducto, string detalleproducto, string codigoreferencialplano, string file, string codigoplano, DataGridView DGV,
            Button btn1, Button btn2, Button btn3, Button btn4)
        {
            try
            {
                string NombreGenerado = codigoproducto + " - " + detalleproducto + " - " + codigoreferencialplano;

                string RutaOld = file;

                string RutaNew = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Procesos\Productos\Planos\" + NombreGenerado + ".pdf";

                File.Copy(RutaOld, RutaNew);

                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("ListadoProductos_InsertarPlanoDiferente", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@doc", SqlDbType.VarBinary).Value = System.Data.SqlTypes.SqlBinary.Null;
                cmd.Parameters.AddWithValue("@namereferences", codigoreferencialplano);
                cmd.Parameters.AddWithValue("@name", RutaNew);
                cmd.Parameters.AddWithValue("@realname", NombreGenerado + ".pdf");

                cmd.Parameters.AddWithValue("@idart", Convert.ToInt32(idart));
                codigoPlano();
                codigoplano = DGV.SelectedCells[0].Value.ToString();
                cmd.Parameters.AddWithValue("@idplano", Convert.ToInt32(codigoplano) + 1);

                cmd.ExecuteNonQuery();
                con.Close();

                MostrarSegunId(idart);

                MessageBox.Show("Registro ingresado exitosamente.", "Nuevo plano", MessageBoxButtons.OK);
                file = "";

                btn1.Visible = false;
                btn2.Visible = false;

                btn3.Visible = true;
                lblCargarPlano.Visible = true;
                txtFile.Text = "";
                btn4.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //GAURDAR EL LANO NUEVO EN LA BASE DE DATOS ASOCIADO AL PRODUCTO
        private void btnConfirmarPlano_Click(object sender, EventArgs e)
        {
            AgregarPlano(lblCodigoProducto.Text, txtDetalleProducto.Text, lblCodigoReferenciaPlano.Text, txtFile.Text, lblCodigoPlano.Text, datalistadoPlano
            , btnConfirmarPlano, btnCancelarPlano, btnAgregarPlano, btnAbrirPdf);
        }

        //CANCELAR LA ACCIÓN DE AGREGAR UN NUEVO PLANO
        private void btnCancelarPlano_Click(object sender, EventArgs e)
        {
            txtFile.Text = "";
            lblCodigoReferenciaPlano.Text = "";

            btnCancelarPlano.Visible = false;
            btnConfirmarPlano.Visible = false;
            txtFile.Text = "";
            btnAgregarPlano.Visible = true;
            lblCargarPlano.Visible = true;
            btnAbrirPdf.Visible = true;
            btnEliminarPlano.Visible = true;
        }

        //MÉTODO PARA ELIMINAR EL PLANO SELECIOANDO DE UN PRODUCTO
        public void EliminarPlano(DataGridView DGV)
        {
            try
            {
                string rutaplano = "";
                int codigoplano = 0;

                if (DGV.CurrentRow != null)
                {
                    rutaplano = DGV.Rows[DGV.CurrentRow.Index].Cells[2].Value.ToString();
                    codigoplano = int.Parse(DGV.Rows[DGV.CurrentRow.Index].Cells[0].Value.ToString());

                    if (codigoplano != 0)
                    {
                        DialogResult boton = MessageBox.Show("Realmente desea eliminar.", "Eliminar Plano", MessageBoxButtons.OKCancel);
                        if (boton == DialogResult.OK)
                        {
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("ListadoProductos_EliminarPlano", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idplano", codigoplano);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            //VERIFICAR SI EXISTE LA CARPETA
                            string carpetaDestino = @"C:\Planos\Planos Eliminados";
                            // Verifica si la carpeta existe, si no, la crea
                            if (!Directory.Exists(carpetaDestino))
                            {
                                Directory.CreateDirectory(carpetaDestino);
                            }
                            File.Move(rutaplano, @"C:\Planos\Planos Eliminados\PlanoEliminado - " + codigoplano + ".pdf");

                            MostrarSegunId(idart);
                            MessageBox.Show("Eliminación correcta, operación hecha satisfactoriamente.", "Eliminación Nueva", MessageBoxButtons.OK);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar.", "Eliminación de un plano", MessageBoxButtons.OKCancel);
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione un plano para poder eliminar.", "Eliminación de Plano", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //MÉTODO PARA ELIMINAR EL PLANO SELECIOANDO DE UN PRODUCTO
        private void btnEliminarPlano_Click(object sender, EventArgs e)
        {
            EliminarPlano(datalistadopdf);
        }

        //PROCESO, METODOS Y FUNCIOANES ASOCIADAS AL PRODUCTO, CRUD DEL PRODUCTO---------
        //ELIMINAR PRODUCTO GUARDARO O SELECCIOANDO
        public void EliminarProducto()
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea eliminar este producto?.", "Eliminación de un Producto", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                try
                {
                    if (idart != 0)
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("ListadoProductos_EliminarProducto", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idart", idart);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MostrarProductos();
                        MessageBox.Show("Eliminación correcta, operación hecha satisfactoriamente.", "Eliminación Producto", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("Debe seleccionar un producto para poder eliminarlo.", "Validación del Sistema", MessageBoxButtons.OK);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //ELIMINAR PRODUCTO GUARDARO O SELECCIOANDO
        private void btnEliminarProducto_Click(object sender, EventArgs e)
        {
            EliminarProducto();
        }

        //HABILITAR LA VENTANA DE EDICION
        public void EditarProducto(CheckBox ck, Panel pa, DataGridView DGV)
        {
            if(txtCodigoBSSEdicion.Text == "")
            {
                MessageBox.Show("Debe ingresar un código BSS para poder editar el producto.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    DialogResult boton = MessageBox.Show("¿Realmente desea editar este producto?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                    if (boton == DialogResult.OK)
                    {
                        if (idart != 0)
                        {
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("ListadoProductos_Editar", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idArt", idart);
                            cmd.Parameters.AddWithValue("@estadoSemiProducido", ck.Checked);
                            cmd.Parameters.AddWithValue("@dcodigBSS", txtCodigoBSSEdicion.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MostrarProductos();
                            MessageBox.Show("Edición correcta, operación hecha satisfactoriamente.", "Validación del Sistema", MessageBoxButtons.OK);
                            pa.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("Debe seleccionar un producto para poder editarlo.", "Validación del Sistema", MessageBoxButtons.OK);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            DGV.Enabled = true;
        }

        //HABILITAR LA VENTANA DE EDICION
        private void btnEditarProductoCampos_Click(object sender, EventArgs e)
        {
            EditarProducto(ckSemiProducidoEdicion, panelCamposProducto, datalistado);
        }

        //SALIR DE LOS DETALLES Y CARACTERISTICAS DE MI PRODUCTO
        private void btnSalirCamposProducto_Click(object sender, EventArgs e)
        {
            datalistado.Enabled = true;
            panelCamposProducto.Visible = false;
        }
        //-------------------------------------------------------------------------------------------

        //BUSQUEDAS DEL PRODUCTO, FILTROS POR LINEA, MODELO, CODIGO O DESCRIPCION--------------
        //BÚSQUEDA DE LÍNEAS SEGÚN LA CUENTA SELECCIAONDA
        private void cboFiltroTipoMercaderiaProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFiltroTipoMercaderiaProducto.SelectedValue != null)
            {
                idbusquedamercaderia = cboFiltroTipoMercaderiaProducto.SelectedValue.ToString();
                CargarBusquedasLinea(idbusquedamercaderia);
            }
        }

        //BÚSQUEDA DE MODELOS SEGÚN LA LÍNEA SELECCIOANDA
        private void cboFiltroLineaProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFiltroLineaProducto.SelectedValue != null)
            {
                idbusquedalinea = cboFiltroLineaProducto.SelectedValue.ToString();
                CargarBusquedasModelos(idbusquedalinea);
            }
        }

        //BÚSQUEDA DE PRODUCTOS SEGÚN EL MODELO SELECCIONADO
        private void cboFiltroModeloProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("ListadoProductos_BuscarPorModelo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idmodelo", cboFiltroModeloProducto.SelectedValue.ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistado.DataSource = dt;
                con.Close();
                ReirdenarListadoProductos(datalistado);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //METODO QUE FILTRA LOS PRODUCTOS SEGÚN EL TIPO DE BÚSQUEDA SELECCIONADA
        private void FiltrarProductos(ComboBox cbo, string busqueda)
        {
            try
            {
                if (cbo.Text == "CÓDIGO")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("ListadoProductos_BuscarPorCodcom", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codcom", busqueda);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistado.DataSource = dt;
                    con.Close();
                }
                else if (cbo.Text == "DESCRIPCIÓN")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("ListadoProductos_BuscarPorDescripcion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@descripcion", busqueda);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistado.DataSource = dt;
                    con.Close();
                }
                else
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("ListadoProductos_BuscarPorCodigoBss", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@descripcion", busqueda);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistado.DataSource = dt;
                    con.Close();
                }
                ReirdenarListadoProductos(datalistado);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //REORDENAR MIS LSITADO DE PRODUCTOS
        public void ReirdenarListadoProductos(DataGridView DGV)
        {
            DGV.Columns[0].Visible = false;
            DGV.Columns[1].Width = 100;
            DGV.Columns[2].Width = 100;
            DGV.Columns[3].Width = 915;
            DGV.Columns[4].Visible = false;
            DGV.Columns[5].Width = 90;
            DGV.Columns[6].Visible = false;
            DGV.Columns[7].Width = 135;
            DGV.Columns[8].Visible = false;
            DGV.Columns[9].Visible = false;
            DGV.Columns[10].Visible = false;
            DGV.Columns[11].Visible = false;
            DGV.Columns[12].Visible = false;
            DGV.Columns[13].Width = 30;
            DGV.Columns[14].Visible = false;
            DGV.Columns[15].Visible = false;
            DGV.Columns[16].Visible = false;
            DGV.Columns[17].Visible = false;
            DGV.Columns[18].Visible = false;
            DGV.Columns[19].Visible = false;
            DGV.Columns[20].Visible = false;
            DGV.Columns[21].Visible = false;
            DGV.Columns[22].Visible = false;
            DGV.Columns[23].Visible = false;
            DGV.Columns[24].Visible = false;
            DGV.Columns[25].Visible = false;
            DGV.Columns[26].Visible = false;
            DGV.Columns[27].Visible = false;
            DGV.Columns[28].Visible = false;
            DGV.Columns[29].Visible = false;
            DGV.Columns[30].Visible = false;
            DGV.Columns[31].Visible = false;
            DGV.Columns[32].Visible = false;
            DGV.Columns[33].Visible = false;
            DGV.Columns[34].Visible = false;
            DGV.Columns[35].Visible = false;
            DGV.Columns[36].Visible = false;
            DGV.Columns[37].Visible = false;
            DGV.Columns[38].Visible = false;
            DGV.Columns[39].Visible = false;
            DGV.Columns[40].Visible = false;
            DGV.Columns[41].Visible = false;
            DGV.Columns[42].Visible = false;
            DGV.Columns[43].Visible = false;
            DGV.Columns[44].Visible = false;
            DGV.Columns[45].Visible = false;
            DGV.Columns[46].Visible = false;
            DGV.Columns[47].Visible = false;
            DGV.Columns[48].Visible = false;
            DGV.Columns[49].Visible = false;
        }

        //METODO QUE FILTRA LOS PRODUCTOS SEGÚN EL TIPO DE BÚSQUEDA SELECCIONADA
        private void txtBusquedaProducto_TextChanged(object sender, EventArgs e)
        {
            FiltrarProductos(cboBusquedaProducto, txtBusquedaProducto.Text);
        }
        //----------------------------------------------------------------------------------

        //VALIDACIONES DE LOS DATOS ANEXOS AL PRODUCTO Y GENERACION DE CODIGO PARA LOS PLANOS-----------------------------------
        public void GeneracionReferenciaPlano()
        {
            string abreviaturaModelo = "";
            abreviaturaModelo = lblAbreviaturaModelo.Text;

            if (datalistadopdf.Rows.Count == 0)
            {
                lblCodigoReferenciaPlano.Text = "A-" + abreviaturaModelo + "00";
            }
            else if (datalistadopdf.Rows.Count == 1)
            {
                lblCodigoReferenciaPlano.Text = "B-" + abreviaturaModelo + "00";
            }
            else if (datalistadopdf.Rows.Count == 2)
            {
                lblCodigoReferenciaPlano.Text = "C-" + abreviaturaModelo + "00";
            }
            else if (datalistadopdf.Rows.Count == 3)
            {
                lblCodigoReferenciaPlano.Text = "D-" + abreviaturaModelo + "00";
            }
            else if (datalistadopdf.Rows.Count == 4)
            {
                lblCodigoReferenciaPlano.Text = "E-" + abreviaturaModelo + "00";
            }
            else if (datalistadopdf.Rows.Count == 5)
            {
                lblCodigoReferenciaPlano.Text = "F-" + abreviaturaModelo + "00";
            }
            else if (datalistadopdf.Rows.Count == 6)
            {
                lblCodigoReferenciaPlano.Text = "G-" + abreviaturaModelo + "00";
            }
            else if (datalistadopdf.Rows.Count == 7)
            {
                lblCodigoReferenciaPlano.Text = "H-" + abreviaturaModelo + "00";
            }
            else if (datalistadopdf.Rows.Count == 8)
            {
                lblCodigoReferenciaPlano.Text = "I-" + abreviaturaModelo + "00";
            }
            else if (datalistadopdf.Rows.Count == 9)
            {
                lblCodigoReferenciaPlano.Text = "J-" + abreviaturaModelo + "00";
            }
        }

        //ACCIONES DE CAMPOS ANEXOS-----------------------------------------------------------
        //CANCELAR LOS DATOS ANEXOS
        private void btCancelarDatosAnexos_Click(object sender, EventArgs e)
        {
            //campos de stok y ubicacion
            LimpiarCamposDatosAnexos();
        }

        //METODO PARA LIMPIAR LOS DATOS ANEXOS
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
            int? codigotipomedidas1 = Convert.ToInt32(cboTipoMedidas1.SelectedValue);
            int? codigodescripcionmedidas1 = Convert.ToInt32(cboDescripcionMedidas1.SelectedValue);
            int? codigotipomedidas2 = Convert.ToInt32(cboTipoMedidas2.SelectedValue);
            int? codigodescripcionmedidas2 = Convert.ToInt32(cboDescripcionMedidas2.SelectedValue);

            //DECLARACION DE VARIABLES PARA ALMACENAR LOS DATOS INGRESADOS POR EL USUARIO - MEDIDAS 2
            int? codigotipomedidas3 = Convert.ToInt32(cboTipoMedidas3.SelectedValue);
            int? codigodescripcionmedidas3 = Convert.ToInt32(cboDescripcionMedidas3.SelectedValue);
            int? codigotipomedidas4 = Convert.ToInt32(cboTipoMedidas4.SelectedValue);
            int? codigodescripcionmedidas4 = Convert.ToInt32(cboDescripcionMedidas4.SelectedValue);

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
            int? codigotipodiseñoacabados1 = Convert.ToInt32(cboTiposDiseñoAcabado1.SelectedValue);
            int? codigodescripciondiseñoacabados1 = Convert.ToInt32(cboDescripcionDiseñoAcabado1.SelectedValue);
            int? codigotipodiseñoacabados2 = Convert.ToInt32(cboTiposDiseñoAcabado2.SelectedValue);
            int? codigodescripciondiseñoacabados2 = Convert.ToInt32(cboDescripcionDiseñoAcabado2.SelectedValue);

            //DECLARACION DE VARIABLES PARA ALMACENAR LOS DATOS INGRESADOS POR EL USUARIO - DISEÑO 2
            int? codigotipodiseñoacabados3 = Convert.ToInt32(cboTiposDiseñoAcabado3.SelectedValue);
            int? codigodescripciondiseñoacabados3 = Convert.ToInt32(cboDescripcionDiseñoAcabado3.SelectedValue);
            int? codigotipodiseñoacabados4 = Convert.ToInt32(cboTiposDiseñoAcabado4.SelectedValue);
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

            string campogeneral = "";
            if (ckGeneral.Checked == true)
            {
                campogeneral = txtDescripcionGeneral.Text;
            }
            else
            {
                campogeneral = "";
            }

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

        //VALIDAR CAMPOS VACIOS
        public void ValidarCamposVacios()
        {
            //REINICIO DE LA VARIABLE GLOBAL
            EstadoValidacionCampoVacios = 0;

            //VALIDAR GURPO DE COMBO CARACTERISTICAS 1
            if (ckCamposCaracteristicas1.Checked == true)
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
            if (ckCamposCaracteristicas2.Checked == true)
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
                if (cboTipoMedidas1.Text != "NO APLICA" && cboDescripcionMedidas1.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
                if (cboTipoMedidas2.Text != "NO APLICA" && cboDescripcionMedidas2.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
            }
            //VALIDAR GURPO DE COMBO MEDIDAS 2
            if (ckCamposMedida2.Checked == true)
            {
                if (cboTipoMedidas3.Text != "NO APLICA" && cboDescripcionMedidas3.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
                if (cboTipoMedidas4.Text != "NO APLICA" && cboDescripcionMedidas4.SelectedItem == null)
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
                if (cboTiposDiseñoAcabado1.Text != "NO APLICA" && cboDescripcionDiseñoAcabado1.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
                if (cboTiposDiseñoAcabado2.Text != "NO APLICA" && cboDescripcionDiseñoAcabado2.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
            }
            //VALIDAR GURPO DE COMBO DISEÑO Y ACABADOS 2
            if (ckCamposDiseñoAcabado2.Checked == true)
            {
                if (cboTiposDiseñoAcabado3.Text != "NO APLICA" && cboDescripcionDiseñoAcabado3.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar todos los campos correspondientes, Si existe un campo que lleva un dato en blanco, seleccionarlo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    EstadoValidacionCampoVacios = 1;
                }
                if (cboTiposDiseñoAcabado4.Text != "NO APLICA" && cboDescripcionDiseñoAcabado4.SelectedItem == null)
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

        //ABRIR LOS DATOS ANEXOS AL PRODUTO
        private void btnDatosAnexos_Click(object sender, EventArgs e)
        {
            panelDatosAnexos.Visible = true;
        }

        //ACEPTAR LOS DATOS ANEXOS Y GUARDAR LA SELECCION Y LLENADO
        private void btnAceptarDatosAnexos_Click(object sender, EventArgs e)
        {
            panelDatosAnexos.Visible = false;
        }

        //VALIDACIONES DE LOS DATOS ANEXOS - CAMPOS ------------------------------------------
        //ACCIÓN DEL CAMPO AFECTO IGV
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

        //ACCIÓN DEL CAMPO CONTROL STOCK
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

        //ACCIÓN DEL CAMPO JUEGO
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

        //ACCIÓN DEL CAMPO REPOSICIÓN
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

        //ACCIÓN DEL CAMPO SERVICIO
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

        //ACCIÓN DEL CAMPO DE CONTROLAR LOTE
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

        //ACCIÓN DEL CAMPO DE CONTROLOAR SERIE
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

        //ACCIÓN DEL CAMPO SUJETO PERCEPCIÓN
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

        //ACCIÓN DEL CAMPO SUJETO ISC
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

        //ACCIÓN DEL CAMPO SUJETO DETRACCIÓN
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

        //VALIDACION DE INGRESO DE NUMEROS
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

        //VALIDACION DE INGRESO DE NUMEROS
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

        //VALIDACION DE INGRESO DE NUMEROSS
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

        //VALIDACION DE INGRESO DE NUMEROS
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

        //VALIDACION DE INGRESO DE NUMEROS
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

        //VALIDACION DE INGRESO DE NUMEROS
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

        //VALIDACION DE INGRESO DE NUMEROS
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

        //FUNCION PARA GENERAR MI QR
        public void GenerarCodigoQR(string codigoproducto, string detalleproducto, Panel codigoqr)
        {
            try
            {
                if (lblCodigoProducto.Text != "*")
                {
                    panelCodigoQr.Visible = true;

                    Zen.Barcode.CodeQrBarcodeDraw mGenerarQr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                    ptQR.Image = mGenerarQr.Draw(txtDetalleProducto.Text, 250);
                }
                else
                {
                    MessageBox.Show("Seleccione un producto para poder generar el QR de este.", "Validación del Sistema");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //FUNCION PARA GENERAR MI QR
        private void btnVerQr_Click(object sender, EventArgs e)
        {
            GenerarCodigoQR(lblCodigoProducto.Text, txtDetalleProducto.Text, panelCodigoQr);
        }

        //BOTON PARA CERRAR MI QR GENERADO
        private void btnOcultarGenradorQR_Click(object sender, EventArgs e)
        {
            panelCodigoQr.Visible = false;
        }

        //COPIAR CODIGO DE MI PRODUCTO EN PORTAPATELES
        public void CopiarCodigoProducto(string codigoproducto, Label notificacion)
        {
            try
            {
                Clipboard.Clear();
                Clipboard.SetText(lblCodigoProducto.Text);

                lblNotificacionCopiaPortapapeles.Visible = true;
                this.timer.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //COPIAR CODIGO DE MI PRODUCTO EN PORTAPATELES
        private void btnCopiarCodigoProducto_Click(object sender, EventArgs e)
        {
            CopiarCodigoProducto(lblCodigoProducto.Text, lblNotificacionCopiaPortapapeles);
        }

        //BOTON PARA VALIDAR Y ACCIONR MI IMPRESION
        private void btnImprimirQR_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(ImprimirPagina);
            PrintDialog printDlg = new PrintDialog
            {
                Document = pd
            };

            if (printDlg.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
            }
        }

        //FUNCION QUE REALIZA MI TIMER AL MOMENTO DE ACABAR
        private void timer_Tick(object sender, EventArgs e)
        {
            lblNotificacionCopiaPortapapeles.Visible = false;
            this.timer.Enabled = false;
        }

        //FUNCION PARA MANDAR A IMPRIMIR MI QR
        private void ImprimirPagina(object sender, PrintPageEventArgs e)
        {
            if (ptQR.Image != null)
            {
                // Dibuja la imagen en la página impresa
                e.Graphics.DrawImage(ptQR.Image, new Point(100, 100));
            }
        }

        //LIMPIAR MI BUSQUEDA DE PRODUCTOS
        private void cboBusquedaProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusquedaProducto.Text = "";
        }

        //VISUALIZAR TEST WEB
        private void btnVisualizarWeb_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("http://localhost:5026/Home/Index");
        }
    }
}

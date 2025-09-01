using ArenasProyect3.Modulos.ManGeneral;
using ArenasProyect3.Properties;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using FlashControlV71;
using Org.BouncyCastle.Asn1.Mozilla;
using Org.BouncyCastle.Crypto.Parameters;
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
using System.Web.UI;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Mantenimientos
{
    public partial class Clientes : Form
    {
        //VARIABLES CREADAS PARA CLIENTES
        string codigo1;
        string codigo2;
        string codigo3;
        string codigo4;
        string codigo5;

        int idclienteseleccionado = 0;
        bool EstadoDni = false;
        bool EstadoRuc = false;
        bool EstadoOtro = false;

        string Manual = ManGeneral.Manual.manualAreaComercial;

        //CONSTRUCTOR DEL MANTENIMIENTO - CLIENTES
        public Clientes()
        {
            InitializeComponent();
        }

        //EVENTO DE INICIO Y DE CARGA DE CLIENTES
        private void Clientes_Load(object sender, EventArgs e)
        {
            //PRIMERA CARGA DEL FORMULACIO
            Mostrar(datalistado);
            cboTipoBusqueda.SelectedIndex = 0;
        }

        //CARGA DE LOS CAMBOS-------------------------------------------------------------------
        //CARGAR TIPOD DE CLIENTES
        public void CargarTipoCliente()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdTipoClientes, Descripcion FROM TipoClientes WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboTipoClientes.DisplayMember = "Descripcion";
            cboTipoClientes.ValueMember = "IdTipoClientes";
            cboTipoClientes.DataSource = dt;
        }

        //CARGAR TIPO DE DOCUMENTOS
        public void CargarTipoDocumentos()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdTipoDocumento, Descripcion FROM TipoDocumentos WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboTipoDocumento.DisplayMember = "Descripcion";
            cboTipoDocumento.ValueMember = "IdTipoDocumento";
            cboTipoDocumento.DataSource = dt;
        }

        //CARGAR TIPO DE GRUPOS
        public void CargarTipoGrupo()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdTipoGrupo, Descripcion FROM TipoGrupo WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboGrupo.DisplayMember = "Descripcion";
            cboGrupo.ValueMember = "IdTipoGrupo";
            cboGrupo.DataSource = dt;
        }

        //CARGAR TIPÓ DE MONEDA
        public void CargarTipoMoneda()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdTipoMonedas, Descripcion FROM TipoMonedas WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboMoneda.DisplayMember = "Descripcion";
            cboMoneda.ValueMember = "IdTipoMonedas";
            cboMoneda.DataSource = dt;
        }

        //CARGAR TIPO DE RETENCION
        public void CargarTipoRetencion()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdTipoRetencion, Descripcion FROM TipoRetencion WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboRetencion.DisplayMember = "Descripcion";
            cboRetencion.ValueMember = "IdTipoRetencion";
            cboRetencion.DataSource = dt;
        }

        //CARGAR TIPO DE CONDICION
        public void CargarCondicion()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("Select IdCondicionPago, Descripcion from CondicionPago WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboCondicionCondicion.DisplayMember = "Descripcion";
            cboCondicionCondicion.ValueMember = "IdCondicionPago";
            cboCondicionCondicion.DataSource = dt;
        }

        //CARGAR TIPO DE FORMA
        public void CargarForma()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdFormaPago, Descripcion FROM FormaPago WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboFormaCondicion.DisplayMember = "Descripcion";
            cboFormaCondicion.ValueMember = "IdFormaPago";
            cboFormaCondicion.DataSource = dt;
        }

        //SE UTILIZA PARA EL CLIENTE Y SUCURSAL Y UNIDAD - PAIS
        public void CargarPais(ComboBox cbo)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT CodigoPais, Descripcion FROM UbicacionPais WHERE Estado = 1 ORDER BY Descripcion", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cbo.DisplayMember = "Descripcion";
            cbo.ValueMember = "CodigoPais";
            cbo.DataSource = dt;
        }

        //SE UTILIZA PARA EL CLIENTE Y SUCURSAL Y UNIDAD - PROVINCIA
        public void CargarDepartamento(ComboBox cbo, string idpais)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT CodigoDepartamento, Descripcion FROM UbicacionDepartamento WHERE CodigoPais = @idpais", con);
            comando.Parameters.AddWithValue("@idpais", idpais);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cbo.ValueMember = "CodigoDepartamento";
            cbo.DisplayMember = "Descripcion";
            cbo.DataSource = dt;
        }

        //SE UTILIZA PARA EL CLIENTE Y SUCURSAL - PROVINCIA
        public void CargarProvincia(ComboBox cbo, string iddepartamento)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT CodigoProvincia, Descripcion FROM  UbicacionProvincia WHERE CodigoDepartamento= @iddepartamento", con);
            comando.Parameters.AddWithValue("@iddepartamento", iddepartamento);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cbo.ValueMember = "CodigoProvincia";
            cbo.DisplayMember = "Descripcion";
            cbo.DataSource = dt;
        }

        //SE UTILIZA PARA EL CLIENTE Y SUCURSAL - DISTRITO
        public void CargarDistrito(ComboBox cbo, string idprovincia)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT CodigoDistrito,Descripcion FROM  UbicacionDistrito WHERE CodigoProvincia = @idprovincia", con);
            comando.Parameters.AddWithValue("@idprovincia", idprovincia);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cbo.ValueMember = "CodigoDistrito";
            cbo.DisplayMember = "Descripcion";
            cbo.DataSource = dt;
        }

        //ACCIONES DE LOS COMBOS AL SELECCIONAR - UBICACION DE CLIENTES Y OTROS MANTENIMIENTOS
        private void cboPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPais.SelectedValue.ToString() != null)
            {
                string idpais = cboPais.SelectedValue.ToString();
                CargarDepartamento(cboDepartamento, idpais);
            }
        }

        //CARGAR PROVINCIAS DE ACUERDO AL DEPARTAMENTO ESCOFIGO
        private void cboDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDepartamento.SelectedValue.ToString() != null)
            {
                string iddepartamento = cboDepartamento.SelectedValue.ToString();
                CargarProvincia(cboProvincia, iddepartamento);
            }
        }

        //CARGAR LOS DISTRITOS DE ACUERDO A LA PROVINCIA ESCOGIDA
        private void cboProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProvincia.SelectedValue.ToString() != null)
            {
                string idprovincia = cboProvincia.SelectedValue.ToString();
                CargarDistrito(cboDistrito, idprovincia);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------

        //VIZUALIZAR DATOS--------------------------------------------------------------------
      
        public void Mostrar(DataGridView DGV)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand("Clientes_Mostrar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                DGV.DataSource = dt;
                con.Close();

                OcultarColumnas_Listado(DGV);
                alternarColorFilas(DGV);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void OcultarColumnas_Listado(DataGridView DGV)
        {
            DGV.Columns[0].Width = 145;
            DGV.Columns[1].Width = 150;
            DGV.Columns[2].Width = 420;
            DGV.Columns[3].Width = 140;
            DGV.Columns[4].Width = 162;

            DGV.Columns[5].Visible = false;
            DGV.Columns[6].Visible = false;
            DGV.Columns[7].Visible = false;
            DGV.Columns[8].Visible = false;
            DGV.Columns[9].Visible = false;
            DGV.Columns[10].Visible = false;
            DGV.Columns[11].Visible = false;
            DGV.Columns[12].Visible = false;
            DGV.Columns[13].Visible = false;
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
        }

        //VIZUALIZAR DATOS EXCEL--------------------------------------------------------------------
        public void MostrarExcel()
        {
            datalistadoExcel.Rows.Clear();

            foreach (DataGridViewRow dgv in datalistado.Rows)
            {
                string tipocliente = dgv.Cells[1].Value.ToString();
                string documento = dgv.Cells[1].Value.ToString();
                string cliente = dgv.Cells[1].Value.ToString();
                string telefono = dgv.Cells[1].Value.ToString();

                datalistadoExcel.Rows.Add(new[] { tipocliente, documento, cliente, telefono });
            }
        }

        //VALIDADORES DE EXISTENCIA-----------------------------------------------------------
        //VALIDAR DNI
        public void ValidarDni()
        {
            foreach (DataGridViewRow datorecuperado in datalistado.Rows)
            {
                string dni = Convert.ToString(datorecuperado.Cells["DNI / RUC / OTRO"].Value);
                if (dni == txtDni.Text)
                {
                    EstadoDni = true;
                    return;
                }
            }
            return;
        }

        //VALIDAR RUC
        public void ValidarRuc()
        {
            foreach (DataGridViewRow datorecuperado in datalistado.Rows)
            {
                string ruc = Convert.ToString(datorecuperado.Cells["DNI / RUC / OTRO"].Value);
                if (ruc == txtRuc.Text)
                {
                    EstadoRuc = true;
                    return;
                }
            }
            return;
        }

        //VALIDAR OTROS DOCUMENTOS
        public void ValidarOtro()
        {
            foreach (DataGridViewRow datorecuperado in datalistado.Rows)
            {
                string otro = Convert.ToString(datorecuperado.Cells["DNI / RUC / OTRO"].Value);
                if (otro == txtOtroDocumento.Text)
                {
                    EstadoOtro = true;
                    return;
                }
            }
            return;
        }

        //COLOREAR MI LISTADO
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
                MessageBox.Show(ex.Message);
            }

            //deshabilitar el click y  reordenamiento por columnas
            foreach (DataGridViewColumn column in datalistado.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //------------------------------------------------------------------------------
        //SELECCION DE UN REGISTRO O CLIENTE
        private void datalistado_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            CargarTipoMoneda();
            CargarTipoRetencion();
            CargarPais(cboPais);
            CargarTipoGrupo();
            CargarTipoCliente();
            CargarTipoDocumentos();
            idclienteseleccionado = Convert.ToInt32(datalistado.SelectedCells[30].Value.ToString());
            btnAgregar2.Visible = false;
            lblGuardar.Visible = false;
            btnEditar.Visible = true;
            lblEditar.Visible = true;
            panelAgregarCliente.Visible = true;
            lblTituloPanel.Text = "Editar Cliente";
            btnlUnidad.Visible = true;
            btnContacto.Visible = true;
            btnCondicion.Visible = true;
            btnSucursal.Visible = true;

            //Datos Personales/ Juridico/ Codigos
            txtCodigoClientes.Text = datalistado.SelectedCells[29].Value.ToString();
            cboTipoClientes.SelectedValue = datalistado.SelectedCells[14].Value.ToString();
            txtNombreClientes.Text = datalistado.SelectedCells[9].Value.ToString();

            //Datos Personales
            txtPrimerNombre.Text = datalistado.SelectedCells[10].Value.ToString();
            txtSegundoNombre.Text = datalistado.SelectedCells[11].Value.ToString();
            txtApellidoPaterno.Text = datalistado.SelectedCells[12].Value.ToString();
            txtApellidoMaterno.Text = datalistado.SelectedCells[13].Value.ToString();

            //Datos COntacto
            txtTelefono.Text = datalistado.SelectedCells[3].Value.ToString();
            txtTelefonoFijo.Text = datalistado.SelectedCells[15].Value.ToString();
            txtCorreo1.Text = datalistado.SelectedCells[4].Value.ToString();
            txtCorreo2.Text = datalistado.SelectedCells[16].Value.ToString();

            //Datos Comercio
            cboGrupo.SelectedValue = datalistado.SelectedCells[17].Value.ToString();
            cboMoneda.SelectedValue = datalistado.SelectedCells[18].Value.ToString();
            cboRetencion.SelectedValue = datalistado.SelectedCells[19].Value.ToString();
            cboTipoDocumento.SelectedValue = datalistado.SelectedCells[20].Value.ToString();

            //Datos personales 2
            txtDni.Text = datalistado.SelectedCells[6].Value.ToString();
            txtRuc.Text = datalistado.SelectedCells[7].Value.ToString();
            txtOtroDocumento.Text = datalistado.SelectedCells[8].Value.ToString();

            txtDireccion.Text = datalistado.SelectedCells[21].Value.ToString();
            txtReferencia.Text = datalistado.SelectedCells[22].Value.ToString();

            //Datos Ubicacion
            cboPais.SelectedValue = datalistado.SelectedCells[23].Value.ToString();
            cboDepartamento.SelectedValue = datalistado.SelectedCells[24].Value.ToString();
            cboProvincia.SelectedValue = datalistado.SelectedCells[25].Value.ToString();
            cboDistrito.SelectedValue = datalistado.SelectedCells[26].Value.ToString();

            //Datos Cantidades
            txtSoles.Text = datalistado.SelectedCells[27].Value.ToString();
            txtDolares.Text = datalistado.SelectedCells[28].Value.ToString();
        }

        //BOTON PARA AGREGAR UN NUEVO VLIENTE
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            panelAgregarCliente.Visible = true;
            LimpiarCamposNuevoCliente();
        }

        //FUNCION PARA LIMPIAR TODOS LOS CAMPOS DEL NUEVO CLIENTE
        public void LimpiarCamposNuevoCliente()
        {
            CargarTipoMoneda();
            CargarTipoRetencion();
            CargarPais(cboPais);
            CargarTipoGrupo();
            CargarTipoCliente();
            CargarTipoDocumentos();
            btnEditar.Visible = false;
            lblEditar.Visible = false;
            btnAgregar2.Visible = true;
            lblGuardar.Visible = true;
            lblTituloPanel.Text = "Nuevo Cliente";
            btnlUnidad.Visible = false;
            btnContacto.Visible = false;
            btnCondicion.Visible = false;
            btnSucursal.Visible = false;

            //Limpiesa de campos
            txtCodigoClientes.Text = "";
            cboTipoClientes.SelectedValue = 1;
            txtNombreClientes.Text = "";
            txtPrimerNombre.Text = "";
            txtSegundoNombre.Text = "";
            txtApellidoPaterno.Text = "";
            txtApellidoMaterno.Text = "";
            txtTelefono.Text = "";
            txtTelefonoFijo.Text = "";
            txtCorreo1.Text = "";
            txtCorreo2.Text = "";
            cboGrupo.SelectedValue = 1;
            cboMoneda.SelectedValue = 1;
            cboRetencion.SelectedValue = 1;
            cboTipoDocumento.SelectedValue = 1;
            txtDni.Text = "";
            txtRuc.Text = "";
            txtOtroDocumento.Text = "";
            txtDireccion.Text = "";
            txtReferencia.Text = "";
            txtSoles.Text = "0.00";
            txtDolares.Text = "0.00";
        }

        //BOTON PARA REGRESAR Y SALIR DEL NUEVO CLIENTE
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            panelAgregarCliente.Visible = false;
            LimpiarCamposNuevoCliente();
        }

        //JUEGO DE COMBOS Y SUS TIPOS DE CLIENTES
        private void cboTipoClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoClientes.Text == "PERSONA JURÍDICA")
            {
                txtNombreClientes.ReadOnly = false;

                txtPrimerNombre.ReadOnly = true;
                txtSegundoNombre.ReadOnly = true;
                txtApellidoPaterno.ReadOnly = true;
                txtApellidoMaterno.ReadOnly = true;
            }
            else if (cboTipoClientes.Text == "PERSONA NATURAL")
            {
                txtNombreClientes.ReadOnly = true;

                txtPrimerNombre.ReadOnly = false;
                txtSegundoNombre.ReadOnly = false;
                txtApellidoPaterno.ReadOnly = false;
                txtApellidoMaterno.ReadOnly = false;
            }
            else if (cboTipoClientes.Text == "SUJETO NO DOMICILIADO")
            {
                txtNombreClientes.ReadOnly = false;

                txtPrimerNombre.ReadOnly = true;
                txtSegundoNombre.ReadOnly = true;
                txtApellidoPaterno.ReadOnly = true;
                txtApellidoMaterno.ReadOnly = true;
            }
            else if (cboTipoClientes.Text == "ADQUIRIENTE TICKET")
            {
                txtNombreClientes.ReadOnly = false;

                txtPrimerNombre.ReadOnly = true;
                txtSegundoNombre.ReadOnly = true;
                txtApellidoPaterno.ReadOnly = true;
                txtApellidoMaterno.ReadOnly = true;
            }
        }

        //JUEGO DE COMBOS DE TIPO DE DOCUEMNTOS
        private void cboTipoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoDocumento.Text == "OTROS TIPOS DE DOCUMENTOS")
            {
                txtDni.ReadOnly = true;
                txtRuc.ReadOnly = true;
                txtOtroDocumento.ReadOnly = false;
                cboGrupo.SelectedIndex = 1;
                txtDni.Text = "";
                txtRuc.Text = "";
                txtOtroDocumento.Text = "";
            }
            else if (cboTipoDocumento.Text == "DOCUMENTO NACIONAL DE IDENTIDAD (D.N.I)")
            {
                txtDni.ReadOnly = false;
                txtRuc.ReadOnly = true;
                txtOtroDocumento.ReadOnly = true;
                cboGrupo.SelectedIndex = 0;
                txtDni.Text = "";
                txtRuc.Text = "";
                txtOtroDocumento.Text = "";
            }
            else if (cboTipoDocumento.Text == "CARNET DE EXTRANJERIA")
            {
                txtDni.ReadOnly = true;
                txtRuc.ReadOnly = true;
                txtOtroDocumento.ReadOnly = false;
                cboGrupo.SelectedIndex = 1;
                txtDni.Text = "";
                txtRuc.Text = "";
                txtOtroDocumento.Text = "";
            }
            else if (cboTipoDocumento.Text == "REGISTRO UNICO DE CONTRIBUYENTE (R.U.C)")
            {
                txtDni.ReadOnly = true;
                txtRuc.ReadOnly = false;
                txtOtroDocumento.ReadOnly = true;
                txtDni.Text = "";
                txtRuc.Text = "";
                txtOtroDocumento.Text = "";
            }
            else if (cboTipoDocumento.Text == "PASAPORTE")
            {
                txtDni.ReadOnly = true;
                txtRuc.ReadOnly = true;
                txtOtroDocumento.ReadOnly = false;
                cboGrupo.SelectedIndex = 1;
                txtDni.Text = "";
                txtRuc.Text = "";
                txtOtroDocumento.Text = "";
            }
            else
            {
                txtDni.ReadOnly = true;
                txtRuc.ReadOnly = true;
                txtOtroDocumento.ReadOnly = false;
                cboGrupo.SelectedIndex = 1;
                txtDni.Text = "";
                txtRuc.Text = "";
                txtOtroDocumento.Text = "";
            }
        }

        //GENERACION DEL CODIGO SEGUN TIPO DE CLIENTE
        //CLIENTE POR DNI
        private void txtDni_TextChanged(object sender, EventArgs e)
        {
            if (cboTipoDocumento.Text == "DOCUMENTO NACIONAL DE IDENTIDAD (D.N.I)")
            {
                codigo2 = "CDNI" + txtDni.Text;
                txtCodigoClientes.Text = codigo2;
            }
        }

        //CLIENTE POR RUC
        private void txtRuc_TextChanged(object sender, EventArgs e)
        {
            if (cboTipoDocumento.Text == "REGISTRO UNICO DE CONTRIBUYENTE (R.U.C)")
            {
                codigo4 = "CRUC" + txtRuc.Text;
                txtCodigoClientes.Text = codigo4;
            }
        }

        //CLIENTE POR DOCUMENTOS OTROS
        private void txtOtroDocumento_TextChanged(object sender, EventArgs e)
        {
            if (cboTipoDocumento.Text == "OTROS TIPOS DE DOCUMENTOS")
            {
                codigo1 = "COTD" + txtOtroDocumento.Text;
                txtCodigoClientes.Text = codigo1;
            }
            else if (cboTipoDocumento.Text == "CARNET DE EXTRANJERIA")
            {
                codigo3 = "CCDE" + txtOtroDocumento.Text;
                txtCodigoClientes.Text = codigo3;
            }
            else if (cboTipoDocumento.Text == "PASAPORTE")
            {
                codigo5 = "CPAS" + txtOtroDocumento.Text;
                txtCodigoClientes.Text = codigo5;
            }
        }
       

        //METODO PARA EL INGRESO DE UN NUEVO CLIENTE
        public void AgregarCliente(string tipCliente,int idtipocliente,string nomClientes,string primNombre,string segNombre,string apePaterno,string apeMaterno,string tipDocumento,string idtipodocumento
            , string otrDocumento,string dni,string ruc,string tefono,string tefonofijo,string direccion,string distrito,string correo1,string correo2
            ,string grupo,string moneda,string retencion,string referencia,string pais,string departamento,string provincia,decimal soles,decimal dolares
            ,string codiClientes,Panel pagreCliente,DataGridView DGV)
        {
            ValidarDni();
            ValidarRuc();
            ValidarOtro();

            
            if (tipCliente == "PERSONA JURÍDICA" && nomClientes == "" || tipCliente == "SUJETO NO DOMICILIADO" && nomClientes == "" 
                || tipCliente == "ADQUIRIENTE TICKET" && nomClientes == "")
            {
                MessageBox.Show("Debe ingresar un nombre válido.", "Registro de Cliente", MessageBoxButtons.OK);
            }
            else if (tipCliente == "PERSONA NATURAL" && primNombre == "" || tipCliente == "PERSONA NATURAL" && apePaterno == "" || tipCliente == "PERSONA NATURAL" && apeMaterno == "")
            {
                MessageBox.Show("Debe ingresar un nombre o apellidos válido.", "Registro de Cliente", MessageBoxButtons.OK);
            }
            else if (tipDocumento == "OTROS TIPOS DE DOCUMENTOS" && otrDocumento == "" || tipDocumento == "DOCUMENTO NACIONAL DE IDENTIDAD (D.N.I)" && dni == "" || tipDocumento == "CARNET DE EXTRANJERIA" && otrDocumento == "" || tipDocumento == "REGISTRO UNICO DE CONTRIBUYENTE (R.U.C)" && ruc == "" || tipDocumento == "PASAPORTE" && otrDocumento == "" || tipDocumento == "DOCUMENTO NACIONAL DE IDENTIDAD (D.N.I)" && dni.Length != 8 || tipDocumento == "REGISTRO UNICO DE CONTRIBUYENTE (R.U.C)" && ruc.Length != 11 || tipDocumento == "OTROS TIPOS DE DOCUMENTOS" && otrDocumento.Length == 0 || tipDocumento == "CARNET DE EXTRANJERIA" && otrDocumento.Length != 11 || tipDocumento == "PASAPORTE" && otrDocumento.Length != 12)
            {
                MessageBox.Show("Debe ingresar un número de documento válido.", "Registro de Cliente", MessageBoxButtons.OK);
            }
            else if (EstadoDni == true || EstadoRuc == true || EstadoOtro == true)
            {
                MessageBox.Show("El documento ingresado ya se encuentra registrado en el sistema.", "Registro de Cliente", MessageBoxButtons.OK);
                EstadoDni = false;
                EstadoRuc = false;
                EstadoOtro = false;
            }
            else
            {
                if (tefono == "" && tefonofijo == "" || tefono.Length != 9)
                {
                    MessageBox.Show("Debe ingresar un número de teléfono movil o fijo válido.", "Registro de Cliente", MessageBoxButtons.OK);
                }
                else
                {
                    if (direccion == "" || Convert.ToString(distrito) == "")
                    {
                        MessageBox.Show("Debe ingresar una dirección o seleccionar un distrito.", "Registro de Cliente", MessageBoxButtons.OK);
                    }
                    else
                    {
                        try
                        {
                            DialogResult boton = MessageBox.Show("¿Realmente desea guardar a este cliente?.", "Registro de Cliente", MessageBoxButtons.OKCancel);
                            if (boton == DialogResult.OK)
                            {
                                SqlConnection con = new SqlConnection();
                                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                                con.Open();
                                SqlCommand cmd = new SqlCommand();
                                cmd = new SqlCommand("Clientes_Insertar", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@idtipocliente", idtipocliente);
                                cmd.Parameters.AddWithValue("@nombrecliente", nomClientes);
                                cmd.Parameters.AddWithValue("@primernombre", primNombre);
                                cmd.Parameters.AddWithValue("@segundonombre", segNombre);
                                cmd.Parameters.AddWithValue("@apellidopaterno", apePaterno);
                                cmd.Parameters.AddWithValue("@apellidomaterno", apeMaterno);

                                if (tefono == "")
                                {
                                    cmd.Parameters.AddWithValue("@telefono", DBNull.Value);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@telefono", Convert.ToInt32(tefono));
                                }

                                if (tefonofijo == "")
                                {
                                    cmd.Parameters.AddWithValue("@telefonofijo", DBNull.Value);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@telefonofijo", tefonofijo);
                                }

                                cmd.Parameters.AddWithValue("@correo1", correo1);
                                cmd.Parameters.AddWithValue("@correo2", correo2);
                                cmd.Parameters.AddWithValue("@idgrupo", grupo);
                                cmd.Parameters.AddWithValue("@idtipomoneda", moneda);
                                cmd.Parameters.AddWithValue("@idreferencia", retencion);
                                cmd.Parameters.AddWithValue("@idtipodocuemnto", idtipodocumento);

                                if (dni == "")
                                {
                                    cmd.Parameters.AddWithValue("@dni", "");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@dni", dni);
                                }
                                if (ruc == "")
                                {
                                    cmd.Parameters.AddWithValue("@ruc", "");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ruc", ruc);
                                }
                                if (otrDocumento == "")
                                {
                                    cmd.Parameters.AddWithValue("@otros", "");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@otros", otrDocumento);
                                }

                                cmd.Parameters.AddWithValue("@direccion", direccion);
                                cmd.Parameters.AddWithValue("@referencia", referencia);
                                cmd.Parameters.AddWithValue("@idpais", pais);
                                cmd.Parameters.AddWithValue("@iddepartamento", departamento);
                                cmd.Parameters.AddWithValue("@idprovincia", provincia);
                                cmd.Parameters.AddWithValue("@iddistrito", distrito);
                                cmd.Parameters.AddWithValue("@lsoles", soles);
                                cmd.Parameters.AddWithValue("@ldoalres", dolares);
                                cmd.Parameters.AddWithValue("@ubigeo", distrito);
                                cmd.Parameters.AddWithValue("@codigo", codiClientes);
                                cmd.ExecuteNonQuery();
                                con.Close();

                                MessageBox.Show("Cliente guardado exitosamente.", "Registro de Cliente", MessageBoxButtons.OK);
                                pagreCliente.Visible = false;
                                Mostrar(DGV);
                                LimpiarCamposNuevoCliente();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }
        private void btnAgregar2_Click(object sender, EventArgs e)
        {
            AgregarCliente(cboTipoClientes.Text, Convert.ToInt32(cboTipoClientes.SelectedValue), txtNombreClientes.Text, txtPrimerNombre.Text, txtSegundoNombre.Text, txtApellidoPaterno.Text, txtApellidoMaterno.Text, cboTipoDocumento.Text, Convert.ToString(cboTipoDocumento.SelectedValue)
                , txtOtroDocumento.Text, txtDni.Text, txtRuc.Text, txtTelefono.Text, txtTelefonoFijo.Text, txtDireccion.Text, Convert.ToString(cboDistrito.SelectedValue), txtCorreo1.Text, txtCorreo2.Text, Convert.ToString(cboGrupo.SelectedValue)
                , Convert.ToString(cboMoneda.SelectedValue), Convert.ToString(cboRetencion.SelectedValue), txtReferencia.Text, Convert.ToString(cboPais.SelectedValue), Convert.ToString(cboDepartamento.SelectedValue)
                , Convert.ToString(cboProvincia.SelectedValue), Convert.ToDecimal(txtSoles.Text), Convert.ToDecimal(txtDolares.Text), txtCodigoClientes.Text, panelAgregarCliente, datalistado);
        }

        //METODO PARA EDITAR AL CLIENTE SELECCIONADO
        public void EditarCliente(string tipCliente, int idtipocliente, string nomClientes, string primNombre, string segNombre, string apePaterno, string apeMaterno, string tipDocumento, string idtipodocumento
            , string otrDocumento, string dni, string ruc, string tefono, string tefonofijo, string direccion, string distrito, string correo1, string correo2
            , string grupo, string moneda, string retencion, string referencia, string pais, string departamento, string provincia, decimal soles, decimal dolares
            , string codiClientes, Panel pagreCliente, DataGridView DGV)
        {
            if (tipCliente == "PERSONA JURÍDICA" && nomClientes == "" || tipCliente == "SUJETO NO DOMICILIADO" && nomClientes == "" || tipCliente == "ADQUIRIENTE TICKET" && nomClientes == "")
            {
                MessageBox.Show("Debe ingresar un nombre válido.", "Registro de Cliente", MessageBoxButtons.OK);
            }
            else if (tipCliente == "PERSONA NATURAL" && primNombre == "" || tipCliente == "PERSONA NATURAL" && apePaterno == "" || tipCliente == "PERSONA NATURAL" && apeMaterno == "")
            {
                MessageBox.Show("Debe ingresar un nombre o apellidos válidos.", "Registro de Cliente", MessageBoxButtons.OK);
            }
            else if (tipDocumento == "OTROS TIPOS DE DOCUMENTOS" && otrDocumento == "" || tipDocumento == "DOCUMENTO NACIONAL DE IDENTIDAD (D.N.I)" && dni == "" || tipDocumento == "CARNET DE EXTRANJERIA" && otrDocumento== "" || tipDocumento== "REGISTRO UNICO DE CONTRIBUYENTE (R.U.C)" && ruc == "" || tipDocumento == "PASAPORTE" && otrDocumento == "" || tipDocumento == "DOCUMENTO NACIONAL DE IDENTIDAD (D.N.I)" && dni.Length != 8 || tipDocumento == "REGISTRO UNICO DE CONTRIBUYENTE (R.U.C)" && ruc.Length != 11 || tipDocumento == "OTROS TIPOS DE DOCUMENTOS" && otrDocumento.Length == 0 || tipDocumento == "CARNET DE EXTRANJERIA" && otrDocumento.Length != 15 || tipDocumento == "PASAPORTE" && otrDocumento.Length != 15)
            {
                MessageBox.Show("Debe ingresar un número de documento válido.", "Registro de Cliente", MessageBoxButtons.OK);
            }
            else
            {
                if (tefono == "" && tefonofijo == "" || tefono.Length != 9 && tefono != "")
                {
                    MessageBox.Show("Debe ingresar un número de teléfono movil o fijo válido.", "Registro de Cliente", MessageBoxButtons.OK);
                }
                else
                {
                    if (direccion == "" || Convert.ToString(distrito) == "")
                    {
                        MessageBox.Show("Debe ingresar una dirección o seleccionar un distrito.", "Registro de Cliente", MessageBoxButtons.OK);
                    }
                    else
                    {
                        try
                        {
                            DialogResult boton = MessageBox.Show("¿Realmente desea editar a este cliente?.", "Registro de Cliente", MessageBoxButtons.OKCancel);
                            if (boton == DialogResult.OK)
                            {
                                SqlConnection con = new SqlConnection();
                                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                                con.Open();
                                SqlCommand cmd = new SqlCommand();
                                cmd = new SqlCommand("Clientes_Editar", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@idcliente", idclienteseleccionado);
                                cmd.Parameters.AddWithValue("@idtipocliente", idtipocliente);
                                cmd.Parameters.AddWithValue("@nombrecliente", nomClientes);
                                cmd.Parameters.AddWithValue("@primernombre", primNombre);
                                cmd.Parameters.AddWithValue("@segundonombre", segNombre);
                                cmd.Parameters.AddWithValue("@apellidopaterno", apePaterno);
                                cmd.Parameters.AddWithValue("@apellidomaterno", apeMaterno);

                                if (tefono == "")
                                {
                                    cmd.Parameters.AddWithValue("@telefono", DBNull.Value);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@telefono", Convert.ToInt32(tefono));
                                }

                                if (tefonofijo == "")
                                {
                                    cmd.Parameters.AddWithValue("@telefonofijo", DBNull.Value);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@telefonofijo", tefonofijo);
                                }

                                cmd.Parameters.AddWithValue("@correo1", correo1);
                                cmd.Parameters.AddWithValue("@correo2", correo2);
                                cmd.Parameters.AddWithValue("@idgrupo", Convert.ToInt32(grupo));
                                cmd.Parameters.AddWithValue("@idtipomoneda", moneda);
                                cmd.Parameters.AddWithValue("@idreferencia", retencion);
                                cmd.Parameters.AddWithValue("@idtipodocuemnto", idtipodocumento);

                                if (dni == "")
                                {
                                    cmd.Parameters.AddWithValue("@dni", "");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@dni", dni);
                                }
                                if (ruc == "")
                                {
                                    cmd.Parameters.AddWithValue("@ruc", "");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ruc", ruc);
                                }
                                if (otrDocumento == "")
                                {
                                    cmd.Parameters.AddWithValue("@otros", "");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@otros", otrDocumento);
                                }

                                cmd.Parameters.AddWithValue("@direccion", direccion);
                                cmd.Parameters.AddWithValue("@referencia", referencia);
                                cmd.Parameters.AddWithValue("@idpais", pais);
                                cmd.Parameters.AddWithValue("@iddepartamento", departamento);
                                cmd.Parameters.AddWithValue("@idprovincia", provincia);
                                cmd.Parameters.AddWithValue("@iddistrito", distrito);
                                cmd.Parameters.AddWithValue("@lsoles", soles);
                                cmd.Parameters.AddWithValue("@ldoalres", dolares);
                                cmd.Parameters.AddWithValue("@ubigeo", distrito);
                                cmd.Parameters.AddWithValue("@codigo", codiClientes);
                                cmd.ExecuteNonQuery();
                                con.Close();

                                MessageBox.Show("Cliente editado exitosamente.", "Registro de Cliente", MessageBoxButtons.OK);
                                pagreCliente.Visible = false;
                                Mostrar(DGV);
                                LimpiarCamposNuevoCliente();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {


            EditarCliente(cboTipoClientes.Text, Convert.ToInt32(cboTipoClientes.SelectedValue), txtNombreClientes.Text, txtPrimerNombre.Text, txtSegundoNombre.Text, txtApellidoPaterno.Text, txtApellidoMaterno.Text, cboTipoDocumento.Text, Convert.ToString(cboTipoDocumento.SelectedValue)
                , txtOtroDocumento.Text, txtDni.Text, txtRuc.Text, txtTelefono.Text, txtTelefonoFijo.Text, txtDireccion.Text, Convert.ToString(cboDistrito.SelectedValue), txtCorreo1.Text, txtCorreo2.Text, Convert.ToString(cboGrupo.SelectedValue)
                , Convert.ToString(cboMoneda.SelectedValue), Convert.ToString(cboRetencion.SelectedValue), txtReferencia.Text, Convert.ToString(cboPais.SelectedValue), Convert.ToString(cboDepartamento.SelectedValue)
                , Convert.ToString(cboProvincia.SelectedValue), Convert.ToDecimal(txtSoles.Text), Convert.ToDecimal(txtDolares.Text), txtCodigoClientes.Text, panelAgregarCliente, datalistado);
        }


        //PANELES Y VENTANAS ANEXAS AL CLIENTE----------------------------------------------------
        //----------------------------------------------------------------------------------------
        //CARGA DE LISTADO DE CAMPOS CARGADOS AL CLIENTE---------------------------------------
        //MOSTARA UNIDADES DEL CLIENTE SELECCIOANDO
     
        public void MostrarUnidad(int idcliente,DataGridView DGV)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Clientes_MostrarUnidad", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idcliente", idcliente);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            DGV.DataSource = dt;
            con.Close();
            Unidad_Reordenamiento_Columnas(DGV);
        }
        //REORDENAMIENTO DE COLUMNAS DE UNIDADES
        public void Unidad_Reordenamiento_Columnas(DataGridView DGV)
        {
            DGV.Columns[6].Visible = false;
            DGV.Columns[0].Width = 260;
            DGV.Columns[1].Width = 190;
            DGV.Columns[2].Width = 150;
            DGV.Columns[3].Width = 150;
            DGV.Columns[4].Width = 100;
            DGV.Columns[5].Width = 103;

            //deshabilitar el click y  reordenamiento por columnas
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //MOSTRAR CONTACTOS DEL CLIENTE SELECCIOANDO
        public void MostrarContacto(int idcliente,DataGridView DGV)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Clientes_MostrarContacto", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idcliente", idcliente);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            DGV.DataSource = dt;
            con.Close();
            Contacto_Reordenamiento_Columnas(DGV);
        }
        //REORDENAMIENTO DE COLUMNAS DE CONTACTO
        public void Contacto_Reordenamiento_Columnas(DataGridView DGV)
        {
            DGV.Columns[7].Visible = false;
            DGV.Columns[0].Width = 220;
            DGV.Columns[1].Width = 85;
            DGV.Columns[2].Width = 85;
            DGV.Columns[3].Width = 180;
            DGV.Columns[4].Width = 172;
            DGV.Columns[5].Width = 105;
            DGV.Columns[6].Width = 105;
            //deshabilitar el click y  reordenamiento por columnas
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //MOSTRAR CONDICIONES DEL CLIENTE SELECCIOAND
        public void MostrarCondicion(int idcliente,DataGridView DGV)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Clientes_MostrarCondicion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idcliente", idcliente);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            DGV.DataSource = dt;
            con.Close();
            Condicion_Reordenamiento_Columnas(DGV);
        }
        //REORDENAMIENTO DE COLUMNAS DE CONDICION
        public void Condicion_Reordenamiento_Columnas(DataGridView DGV)
        {
            DGV.Columns[3].Visible = false;
            DGV.Columns[0].Width = 430;
            DGV.Columns[1].Width = 290;
            DGV.Columns[2].Width = 233;
            //deshabilitar el click y  reordenamiento por columnas
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //MOSTRAR SUCURSALES DEL CLEINTE SELECCIOANDO
        public void MostrarSucursal(int idcliente,DataGridView DGV)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("Clientes_MostrarSucursal", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idcliente", idcliente);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            DGV.DataSource = dt;
            con.Close();

            Sucursal_Reordenamiento_Columnas(DGV);
        }
        //REORDENAMIENTO DE COLUMNAS DE SUCURSAL
        public void Sucursal_Reordenamiento_Columnas(DataGridView DGV)
        {
            DGV.Columns[8].Visible = false;
            DGV.Columns[0].Width = 250;
            DGV.Columns[1].Width = 120;
            DGV.Columns[2].Width = 150;
            DGV.Columns[3].Width = 90;
            DGV.Columns[4].Width = 100;
            DGV.Columns[5].Width = 140;
            DGV.Columns[6].Width = 140;
            DGV.Columns[7].Width = 140;

            //deshabilitar el click y  reordenamiento por columnas
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //CARGA DE COMBOS GENERAL------------------------------------------------------------------
        //CARGAR RESPONSABLES
        public void CargarResponsable()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdUsuarios, Nombres + ' ' + Apellidos AS NOMBRE FROM Usuarios WHERE Estado = 'Activo' AND HabilitadoRequerimientoVenta = 1 ORDER BY Nombres + ' ' + Apellidos", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboResponsable.DisplayMember = "NOMBRE";
            cboResponsable.ValueMember = "IdUsuarios";
            cboResponsable.DataSource = dt;
        }

        //CARGAR ZONAS
        public void CargarZona()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdZona, Descripcion FROM Zona WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboZona.DisplayMember = "Descripcion";
            cboZona.ValueMember = "IdZona";
            cboZona.DataSource = dt;
        }

        //CARGAR AREAS
        public void CargarArea()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdArea, Descripcion FROM Area WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboAreaContacto.DisplayMember = "Descripcion";
            cboAreaContacto.ValueMember = "IdArea";
            cboAreaContacto.DataSource = dt;
        }

        //CARGAR CARGOS
        public void CargarCargo()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdCargo, Descripcion FROM Cargo WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboCargoContacto.DisplayMember = "Descripcion";
            cboCargoContacto.ValueMember = "IdCargo";
            cboCargoContacto.DataSource = dt;
        }

        //CARGAR UNIDADES DE DATOS ANEZOS
        public void CargarUnidadDatosAnexos(int idcliente)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("Select IdDatosAnexosClienteUnidad, Descripcion from DatosAnexosCliente_Unidad where Estado = 1 and IdCLiente = @idcliente", con);
            comando.Parameters.AddWithValue("@idcliente", idcliente);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboUnidadContacto.DisplayMember = "Descripcion";
            cboUnidadContacto.ValueMember = "IdDatosAnexosClienteUnidad";
            cboUnidadContacto.DataSource = dt;
        }

        //ACCIONES UNIDAD------------------------------------------------------------------------
        //ENTRAR A UNIDADES DEL CLIENTE
        private void lblUnidad_Click(object sender, EventArgs e)
        {
            MostrarUnidad(idclienteseleccionado,datalistadounidad);
            CargarResponsable();
            CargarZona();
            CargarPais(cboPaisUnidad);
            lblCodigoUnida.Text = "0";
            txtCodigoClienteUnidad.Text = txtCodigoClientes.Text;

            if (txtNombreClientes.Text == "")
            {
                txtNombreClienteUnidad.Text = txtPrimerNombre.Text + " " + txtSegundoNombre.Text + " " + txtApellidoPaterno.Text + " " + txtApellidoMaterno.Text;
            }
            else
            {
                txtNombreClienteUnidad.Text = txtNombreClientes.Text;
            }

            panelUnidad.Visible = true;
            panelCondicion.Visible = false;
            panelContacto.Visible = false;
            panelSucursal.Visible = false;
        }

        //CARGAR DEPARTAMENTO DE ACUERDO AL PAIS SELECCIAONDO
        private void cboPaisUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPaisUnidad.SelectedValue.ToString() != null)
            {
                string idpais = cboPaisUnidad.SelectedValue.ToString();
                CargarDepartamento(cboDepartamentoUnidad, idpais);
            }
        }

        //SELECCIOANR UN REGISTRO Y CAPTURAR SU CODIGO
        private void datalistadounidad_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadounidad.RowCount == 0)
            {
                MessageBox.Show("No hay registros para poder visualizar.", "Validación del Sistema");
            }
            else
            {
                lblCodigoUnida.Text = datalistadounidad.SelectedCells[6].Value.ToString();
            }
        }

        //METODO QUE GUARDA LA UNIDAD AL CLIENTE
        public void AgregarUnidad(string nombreunidad,int idresponsable,int idzona,string idpais,string iddepartamento,decimal longitud,decimal latitud,TextBox limpiarlatitud
            ,TextBox limpiarlongitud, TextBox limpiarnombreunidad)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea ingresar esta unidad?.", "Nueva Unidad", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (Convert.ToString(latitud) == "" || Convert.ToString(longitud) == "" || string.IsNullOrWhiteSpace(nombreunidad))
                {
                    MessageBox.Show("Debe ingresar los datos correspondientes.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Clientes_Insertar_Unidad", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idcliente", idclienteseleccionado);
                        cmd.Parameters.AddWithValue("@descipcion", nombreunidad);
                        cmd.Parameters.AddWithValue("@idresponsable", idresponsable);
                        cmd.Parameters.AddWithValue("@idzona", idzona);
                        cmd.Parameters.AddWithValue("@idpais", idpais);
                        cmd.Parameters.AddWithValue("@iddepartamento", iddepartamento);
                        cmd.Parameters.AddWithValue("@longitud", longitud);
                        cmd.Parameters.AddWithValue("@latitud",latitud);

                        cmd.ExecuteNonQuery();
                        con.Close();

                        MostrarUnidad(idclienteseleccionado,datalistadounidad);
                        MessageBox.Show("Registro ingresado exitosamente.", "Nueva Unidad", MessageBoxButtons.OK);

                        limpiarlatitud.Text = "";
                        limpiarlongitud.Text = "";
                        limpiarnombreunidad.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void btnGuardarUnidad_Click(object sender, EventArgs e)
        {
            AgregarUnidad(txtNombreUnidad.Text, Convert.ToInt32(cboResponsable.SelectedValue), Convert.ToInt32(cboZona.SelectedValue), cboPaisUnidad.SelectedValue.ToString()
                ,cboDepartamentoUnidad.SelectedValue.ToString(), Convert.ToDecimal(txtLongitud.Text), Convert.ToDecimal(txtLatitud.Text),txtLatitud,txtLongitud,txtNombreUnidad);
        }

        //METODO QUE ELIMINA LA UNIDAD REGISTRADA DEL CLIENTE
        public void EliminarUnidad(int codigounidad,Label limpiarcodigounidad)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea eliminar?.", "Eliminar Unidad", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (Convert.ToString(codigounidad) != "0")
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Clientes_Eliminar_Unidad", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", codigounidad);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Eliminación correcta, operación hecha satisfactoriamente.", "Eliminación de una Unidad", MessageBoxButtons.OK);
                        limpiarcodigounidad.Text = "0";

                        MostrarUnidad(idclienteseleccionado,datalistadounidad);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un registro para poder eliminarlo.", "Eliminación de una Unidad", MessageBoxButtons.OK);
                }
            }
        }
        private void btnEiminarUnidad_Click(object sender, EventArgs e)
        {
            EliminarUnidad(Convert.ToInt32(lblCodigoUnida.Text),lblCodigoUnida);
        }

        //VOLVER O SALIR DE UNDIAD
        private void btnCerrarUnidad_Click(object sender, EventArgs e)
        {
            panelUnidad.Visible = false;
            txtLongitud.Text = "";
            txtLatitud.Text = "";
            txtNombreUnidad.Text = "";
        }

        //ACCIONES CONTACTO------------------------------------------------------------------------
        //ENTRAR A CONTACTO DEL CLIENTE
        private void lblContacto_Click(object sender, EventArgs e)
        {
            MostrarContacto(idclienteseleccionado,datalistadocontacto);
            CargarUnidadDatosAnexos(idclienteseleccionado);
            CargarCargo();
            CargarArea();
            lblCodigoContacto.Text = "0";
            txtCodigoClienteContacto.Text = txtCodigoClientes.Text;

            if (txtNombreClientes.Text == "")
            {
                txtNombreClienteContacto.Text = txtPrimerNombre.Text + " " + txtSegundoNombre.Text + " " + txtApellidoPaterno.Text + " " + txtApellidoMaterno.Text;
            }
            else
            {
                txtNombreClienteContacto.Text = txtNombreClientes.Text;
            }

            panelUnidad.Visible = false;
            panelCondicion.Visible = false;
            panelContacto.Visible = true;
            panelSucursal.Visible = false;
        }

        //SELECCIOANR UN REGISTRO Y CAPTURAR SU CODIGO
        private void datalistadocontacto_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadocontacto.RowCount == 0)
            {
                MessageBox.Show("No hay registros para poder visualizar.", "Validación del Sistema");
            }
            else
            {
                lblCodigoContacto.Text = datalistadocontacto.SelectedCells[7].Value.ToString();
            }
        }

        //METODO QUE GUARDARA EL CONTACTO AL CLIENTE
        public void AgregarContacto(string nombrecontacto,string telefonocontacto,string anexocontacto,string correocontacto,int idunidad,int idarea,int idcargo,TextBox limpiarnombrecontacto
            ,TextBox limpiartelefonocontacto,TextBox limpiaranexocontacto,TextBox limpiarcorreocontacto,ComboBox cbounidadcontacto)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea ingresar este contacto?.", "Nuevo Contacto", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(nombrecontacto) || string.IsNullOrWhiteSpace(correocontacto) || string.IsNullOrWhiteSpace(anexocontacto) || cbounidadcontacto.SelectedValue == null || cbounidadcontacto.Text == "")
                {
                    MessageBox.Show("Debe ingresar o seleccionar los datos correspondientes.", "Registro", MessageBoxButtons.OK);
                }
                else
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Clientes_Insertar_Contacto", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idcliente", idclienteseleccionado);
                        cmd.Parameters.AddWithValue("@descipcion", nombrecontacto);
                        cmd.Parameters.AddWithValue("@telefono", telefonocontacto);
                        cmd.Parameters.AddWithValue("@anexo", anexocontacto);
                        cmd.Parameters.AddWithValue("@correo", correocontacto);
                        cmd.Parameters.AddWithValue("@idunidad", idunidad);
                        cmd.Parameters.AddWithValue("@idarea", idarea);
                        cmd.Parameters.AddWithValue("@idcargo", idcargo);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MostrarContacto(idclienteseleccionado,datalistadocontacto);
                        MessageBox.Show("Registro ingresado exitosamente.", "Nuevo Contacto", MessageBoxButtons.OK);

                        limpiarnombrecontacto.Text = "";
                        limpiartelefonocontacto.Text = "";
                        limpiaranexocontacto.Text = "";
                        limpiarcorreocontacto.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void btnGuardarContacto_Click(object sender, EventArgs e)
        {
            AgregarContacto(txtNombreContacto.Text,txtTelefonoContacto.Text,txtAnexoContacto.Text,txtCorreoContacto.Text,Convert.ToInt32(cboUnidadContacto.SelectedValue)
                ,Convert.ToInt32(cboAreaContacto.SelectedValue),Convert.ToInt32(cboCargoContacto.SelectedValue),txtNombreContacto,txtTelefonoContacto,txtAnexoContacto
                ,txtCorreoContacto,cboUnidadContacto);
        }

        //METODO DE ELIMINACIÓN DE CONTACTO REGISTRADO DEL CLIENTE
        public void EliminarContacto(int codigocontacto,Label limpiarcodigocontacto)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea eliminar?.", "Eliminar Contacto", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (Convert.ToString(codigocontacto) != "0")
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Clientes_Eliminar_Contacto", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", codigocontacto);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MostrarContacto(idclienteseleccionado,datalistadocontacto);
                        MessageBox.Show("Eliminación correcta, operación hecha satisfactoriamente.", "Eliminación Contacto", MessageBoxButtons.OK);
                        limpiarcodigocontacto.Text = "0";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un registro para poder eliminarlo.", "Eliminación de un Contacto", MessageBoxButtons.OK);
                }
            }
        }
        private void btnEliminarContactos_Click(object sender, EventArgs e)
        {
            EliminarContacto(Convert.ToInt32(lblCodigoContacto.Text),lblCodigoContacto);
        }

        //VOLVER O SALIR DE CONTACTO
        private void btnRegresarContacto_Click(object sender, EventArgs e)
        {
            panelContacto.Visible = false;
            txtNombreContacto.Text = "";
            txtTelefonoContacto.Text = "";
            txtCorreoContacto.Text = "";
            txtAnexoContacto.Text = "";
        }

        //ACCIONES CONDICION------------------------------------------------------------------------
        //ENTRAR A CONDICIONES DEL CLIENTE
        private void lblCondicion_Click(object sender, EventArgs e)
        {
            MostrarCondicion(idclienteseleccionado,datalistadoCondicion);
            CargarCondicion();
            CargarForma();
            lblCodigoCOndicion.Text = "0";

            if (txtNombreClientes.Text == "")
            {
                txtNombreCLienteCondicion.Text = txtPrimerNombre.Text + " " + txtSegundoNombre.Text + " " + txtApellidoPaterno.Text + " " + txtApellidoMaterno.Text;
            }
            else
            {
                txtNombreCLienteCondicion.Text = txtNombreClientes.Text;
            }

            panelUnidad.Visible = false;
            panelCondicion.Visible = true;
            panelContacto.Visible = false;
            panelSucursal.Visible = false;
        }

        //SELECCIOANR UN REGISTRO Y CAPTURAR SU CODIGO
        private void datalistadoCondicion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoCondicion.RowCount == 0)
            {
                MessageBox.Show("No hay registros para poder visualizar.", "Validación del Sistema");
            }
            else
            {
                lblCodigoCOndicion.Text = datalistadoCondicion.SelectedCells[3].Value.ToString();
            }
        }

        //ACCION DE GUARDAR CONDICION PARA MI CLIENTE
        public void AgregarCondicion(int idcondicion,int idforma)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea ingresar esta condición?.", "Nueva Condición", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                try
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Clientes_Insertar_Condicion", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@idcliente", idclienteseleccionado);
                    cmd.Parameters.AddWithValue("@idcondicion", idcondicion);
                    cmd.Parameters.AddWithValue("@idforma", idforma);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MostrarCondicion(idclienteseleccionado,datalistadoCondicion);
                    MessageBox.Show("Registro ingresado exitosamente.", "Nueva Condición", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void btnGuardarCondicion_Click(object sender, EventArgs e)
        {
            AgregarCondicion(Convert.ToInt32(cboCondicionCondicion.SelectedValue),Convert.ToInt32(cboFormaCondicion.SelectedValue));
        }

        //METODO QUE ELIMINA LA CONDICION REGISTRARA PARA EL CLIENTE
        public void EliminarCondicion(int codigocondicion,Label limpiarcodigocondicion)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea eliminar?.", "Eliminar Condición", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (Convert.ToString(codigocondicion) != "0")
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Clientes_Eliminar_Condicion", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", codigocondicion);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MostrarCondicion(idclienteseleccionado,datalistadoCondicion);
                        MessageBox.Show("Eliminación correcta, operación hecha satisfactoriamente.", "Eliminación de una Condición", MessageBoxButtons.OK);
                        limpiarcodigocondicion.Text = "0";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un registro para poder eliminarlo.", "Eliminación de una Condición", MessageBoxButtons.OK);
                }
            }
        }
        private void btnEliminarCondicion_Click(object sender, EventArgs e)
        {
            EliminarCondicion(Convert.ToInt32(lblCodigoCOndicion.Text),lblCodigoCOndicion);
        }

        //VOLVER O SALIR DE CONDICION
        private void btnRetrocederCondicion_Click(object sender, EventArgs e)
        {
            panelCondicion.Visible = false;
        }

        //ACCIONES SUCURSAL------------------------------------------------------------------------
        //ENTRAR A SUCURSAL DEL CLIENTE
        private void lblSucursal_Click(object sender, EventArgs e)
        {
            MostrarSucursal(idclienteseleccionado, datalistadosucursal);
            CargarPais(cboPaisSucursal);
            lblCodigoSucursal.Text = "0";
            txtCodigoClienteSucursal.Text = txtCodigoClientes.Text;

            if (txtNombreClientes.Text == "")
            {
                txtNombreClienteSucursal.Text = txtPrimerNombre.Text + " " + txtSegundoNombre.Text + " " + txtApellidoPaterno.Text + " " + txtApellidoMaterno.Text;
            }
            else
            {
                txtNombreClienteSucursal.Text = txtNombreClientes.Text;
            }

            panelUnidad.Visible = false;
            panelCondicion.Visible = false;
            panelContacto.Visible = false;
            panelSucursal.Visible = true;
        }

        //CARGAR DEPARTAMENTO DE ACUERDO AL PAIS SELECCIAONDO
        private void cboPaisSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPaisSucursal.SelectedValue.ToString() != null)
            {
                string idpais = cboPaisSucursal.SelectedValue.ToString();
                CargarDepartamento(cboDepartamentoSucursal, idpais);
            }
        }

        //CARGAR PROVINCIAS DE ACUERDO AL DEPARTAMENTO SELECCIAONDO
        private void cboDepartamentoSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDepartamentoSucursal.SelectedValue.ToString() != null)
            {
                string iddepartamento = cboDepartamentoSucursal.SelectedValue.ToString();
                CargarProvincia(cboProvinciaSucursal, iddepartamento);
            }
        }

        //CARGAR SITRITOS DE ACUERDO A LAS PRONVICIAS SELECCIOANDAS
        private void cboProvinciaSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProvinciaSucursal.SelectedValue.ToString() != null)
            {
                string idprovincia = cboProvinciaSucursal.SelectedValue.ToString();
                CargarDistrito(cboDistritoSucursal, idprovincia);
            }
        }

        //SELECCIOANR UN REGISTRO Y CAPTURAR SU CODIGO
        private void datalistadosucursal_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadosucursal.RowCount == 0)
            {
                MessageBox.Show("No hay registros para poder visualizar.", "Validación del Sistema");
            }
            else
            {
                lblCodigoSucursal.Text = datalistadosucursal.SelectedCells[8].Value.ToString();
            }
        }

        //METODO QUE GUARDA LA SUCURSAL AL CLIENTE
        public void AgregarSucursal(string nombresucursal,string direccionsucurusal,string telefonosucursal,string codigopais,string codigodepartamento,string codigoprovincia,string codigodistrito
            ,TextBox limpiarnombresucursal, TextBox limpiardireccionsucursal, TextBox limpiartelefonosucursal)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea ingresar esta sucursal?.", "Registro de Sucursal", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(nombresucursal) || string.IsNullOrWhiteSpace(telefonosucursal) || string.IsNullOrWhiteSpace(direccionsucurusal))
                {
                    MessageBox.Show("Debe ingresar datos válidos para poder hacer el registro.", "Registro de Sucursal", MessageBoxButtons.OK);
                }
                else
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Clientes_Insertar_Sucursal", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idcliente", idclienteseleccionado);
                        cmd.Parameters.AddWithValue("@nombre", nombresucursal);
                        cmd.Parameters.AddWithValue("@direccion", direccionsucurusal);
                        cmd.Parameters.AddWithValue("@telefono", telefonosucursal);
                        cmd.Parameters.AddWithValue("@codigopais", codigopais);
                        cmd.Parameters.AddWithValue("@codigodepartamento", codigodepartamento);
                        cmd.Parameters.AddWithValue("@codigoprovincia", codigoprovincia);
                        cmd.Parameters.AddWithValue("@codigodistrito", codigodistrito);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MostrarSucursal(idclienteseleccionado, datalistadosucursal);
                        MessageBox.Show("Registro ingresado exitosamente.", "Nuevo Sucursal", MessageBoxButtons.OK);

                        limpiarnombresucursal.Text = "";
                        limpiardireccionsucursal.Text = "";
                        limpiartelefonosucursal.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void btnGuardarSucursal_Click(object sender, EventArgs e)
        {
            AgregarSucursal(txtNombreSucursal.Text,txtDireccionSucursal.Text,txtTelefonoSucursal.Text,cboPaisSucursal.SelectedValue.ToString(),cboDepartamentoSucursal.SelectedValue.ToString()
                ,cboProvinciaSucursal.SelectedValue.ToString(),cboDistritoSucursal.SelectedValue.ToString(),txtNombreSucursal,txtDireccionSucursal,txtTelefonoSucursal);
        }

        //METODO PARA ELIMINAR LA SUCURSAL REGISTRADA DEL CLIENTE
        public void EliminarSucursal(int codigosucursal,Label limpiarcodigosucursal)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea eliminar?.", "Eliminar Sucursal", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                try
                {
                    if (Convert.ToString(codigosucursal) != "0")
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Clientes_Eliminar_Sucursal", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", codigosucursal);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MostrarSucursal(idclienteseleccionado, datalistadosucursal);
                        MessageBox.Show("Eliminación correcta, operación hecha satisfactoriamente.", "Eliminación nueva", MessageBoxButtons.OK);
                        limpiarcodigosucursal.Text = "0";
                    }
                    else
                    {
                        MessageBox.Show("Debe seleccionar un registro para poder eliminarlo.", "Eliminación de una Sucursal", MessageBoxButtons.OK);
                    }
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void btnEliminarSucursal_Click(object sender, EventArgs e)
        {
            EliminarSucursal(Convert.ToInt32(lblCodigoSucursal.Text), lblCodigoSucursal);
        }

        //VOLVER O SALIR DE SUCURSAL
        private void btnRegresarSucursal_Click(object sender, EventArgs e)
        {
            panelSucursal.Visible = false;
            txtNombreSucursal.Text = "";
            txtDireccionSucursal.Text = "";
            txtTelefonoSucursal.Text = "";
        }

        //BUSQEUDAS DE CLIENTES Y VALIDACIONES -------------------------------------------
        private void cboTipoBusqueda_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCliente.Text = "";
        }

        //FILTROS DE BUSQUEDA PARA LA BUSQUEDA DE CLIENTES
        public void FiltrarClientes(ComboBox cboseleccionbusqueda, string cliente, DataGridView DGV)
        {
            if (cboseleccionbusqueda.Text == "NOMBRES")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Clientes_BuscarPorNombre", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", cliente);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();  
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (cboseleccionbusqueda.Text == "DOCUMENTO")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Clientes_BuscarPorDocumento", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@documento", cliente);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    DGV.DataSource = dt;
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            Filtro_Configurar_Columnas(cboseleccionbusqueda, DGV);
        }
        public void Filtro_Configurar_Columnas(ComboBox cbo,DataGridView DGV)
        {
            if(cbo.Text == "NOMBRES")
            {
                DGV.Columns[0].Width = 145;
                DGV.Columns[1].Width = 150;
                DGV.Columns[2].Width = 420;
                DGV.Columns[3].Width = 140;
                DGV.Columns[4].Width = 162;

                DGV.Columns[5].Visible = false;
                DGV.Columns[6].Visible = false;
                DGV.Columns[7].Visible = false;
                DGV.Columns[8].Visible = false;
                DGV.Columns[9].Visible = false;
                DGV.Columns[10].Visible = false;
                DGV.Columns[11].Visible = false;
                DGV.Columns[12].Visible = false;
                DGV.Columns[13].Visible = false;
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
            }
            else
            {
                DGV.Columns[0].Width = 145;
                DGV.Columns[1].Width = 150;
                DGV.Columns[2].Width = 420;
                DGV.Columns[3].Width = 140;
                DGV.Columns[4].Width = 162;

                DGV.Columns[5].Visible = false;
                DGV.Columns[6].Visible = false;
                DGV.Columns[7].Visible = false;
                DGV.Columns[8].Visible = false;
                DGV.Columns[9].Visible = false;
                DGV.Columns[10].Visible = false;
                DGV.Columns[11].Visible = false;
                DGV.Columns[12].Visible = false;
                DGV.Columns[13].Visible = false;
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
            }
        }

        private void txtCliente_TextChanged(object sender, EventArgs e)
        {
            FiltrarClientes(cboTipoBusqueda, txtCliente.Text, datalistado);
        }

        //VALIDACIONBES DE INRGESO DE CARACATERES-------------------------------------------------------------
        //VALIDAR TELEFONO
        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //VALIDAR DNI
        private void txtDni_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //VALIDAR RUC
        private void txtRuc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //VALIDAR INGRESO DE LONGITUD
        private void txtLongitud_KeyPress(object sender, KeyPressEventArgs e)
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

            if (e.KeyChar == '-')
            {
                e.Handled = false;
            }
        }

        //VALIDAR INGRESO DE LATITUD
        private void txtLatitud_KeyPress(object sender, KeyPressEventArgs e)
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

            if (e.KeyChar == '-')
            {
                e.Handled = false;
            }
        }

        //VALIDAR INGRESO DE CARACTERES EN TELEFONO
        private void txtTelefonoContacto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //VALIDAR INGRESO DE CARACTERES EN TELEFONO
        private void txtTelefonoSucursal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //BOTON PARA EXPORTAR MI LISTADO DE CLIENTES
        private void btnExportarListadoClientes_Click(object sender, EventArgs e)
        {
            MostrarExcel();

            SLDocument sl = new SLDocument();
            SLStyle style = new SLStyle();
            SLStyle styleC = new SLStyle();

            //COLUMNAS
            sl.SetColumnWidth(1, 20);
            sl.SetColumnWidth(2, 20);
            sl.SetColumnWidth(3, 70);
            sl.SetColumnWidth(4, 15);

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
            foreach (DataGridViewColumn column in datalistadoExcel.Columns)
            {
                sl.SetCellValue(1, ic, column.HeaderText.ToString());
                sl.SetCellStyle(1, ic, style);
                ic++;
            }

            int ir = 2;
            foreach (DataGridViewRow row in datalistadoExcel.Rows)
            {
                sl.SetCellValue(ir, 1, row.Cells[0].Value.ToString());
                sl.SetCellValue(ir, 2, row.Cells[1].Value.ToString());
                sl.SetCellValue(ir, 3, row.Cells[2].Value.ToString());
                sl.SetCellValue(ir, 4, row.Cells[3].Value.ToString());
                sl.SetCellStyle(ir, 1, styleC);
                sl.SetCellStyle(ir, 2, styleC);
                sl.SetCellStyle(ir, 3, styleC);
                sl.SetCellStyle(ir, 4, styleC);
                ir++;
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            sl.SaveAs(desktopPath + @"\Reporte de Clientes.xlsx");
            MessageBox.Show("Se exportó los datos a un archivo de Microsoft Excel en la ubicación siguiente: " + desktopPath, "Validación del Sistema", MessageBoxButtons.OK);
        }

        //BOTON PARA ABIRIR MI MANUAL DE USUARIO
        private void btnInfo_Click(object sender, EventArgs e)
        {
            Process.Start(Manual);
        }

        //BOTON PARA ABIRIR MI MANUAL DE USUARIO
        private void btnInfoPrincipal_Click(object sender, EventArgs e)
        {
            Process.Start(Manual);
        }

        private void txtDolares_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
           
        }

        private void txtSoles_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}

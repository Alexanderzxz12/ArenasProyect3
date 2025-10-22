using SpreadsheetLight;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
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
using System.Diagnostics;
using ArenasProyect3.Modulos.ManGeneral;

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
            Mostrar();
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
            SqlCommand comando = new SqlCommand("SELECT CodigoProvincia, Descripcion FROM  UbicacionProvincia WHERE CodigoDepartamento= @iddepartamento AND Estado=1", con);
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
        public void Mostrar()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            da = new SqlDataAdapter("SELECT TC.Descripcion AS [TIPO CLIENTE], C.Dni + C.Ruc + C.OtroDocumento as [DNI / RUC / OTRO], NombreCliente + C.PrimerNombre + ' ' + C.ApellidoPaterno + ' ' + C.ApellidoMaterno AS[RAZÓN SOCIAL / NOMBRES Y APELLIDOS], COALESCE(CONVERT(VARCHAR, C.TelefonoCelular), C.TelefonoFijo) AS[TELÉFONO / TELÉDONO FIJO], C.Correo1 AS[CORREO], C.Correo2, C.Dni, C.Ruc, C.OtroDocumento, C.NombreCliente, C.PrimerNombre, C.SegundoNombre, C.ApellidoPaterno, C.ApellidoMaterno, C.IdTipoCliente, C.TelefonoFijo, C.Correo2, C.IdGrupo, C.IdTipoMoneda, C.IdRetencion, C.IdTipoDocumento, C.Direccion, C.Referencia, P.CodigoPais, D.CodigoDepartamento, PR.CodigoProvincia, DI.CodigoDistrito, C.Lsoles, C.Ldolares, C.Codigo, C.IdCliente FROM Clientes C INNER JOIN TipoClientes TC ON C.IdTipoCliente = TC.IdTipoClientes INNER JOIN UbicacionPais P ON C.CodigoPais = P.CodigoPais INNER JOIN UbicacionDepartamento D ON C.CodigoDepartamento = D.CodigoDepartamento INNER JOIN UbicacionProvincia PR ON C.CodigoProvincia = PR.CodigoProvincia INNER JOIN UbicacionDistrito DI ON C.CodigoDistrito = DI.CodigoDistrito WHERE C.Estado = 1 ORDER BY NombreCliente + C.PrimerNombre + ' ' + C.ApellidoPaterno + ' ' + C.ApellidoMaterno", con);
            da.Fill(dt);
            datalistado.DataSource = dt;
            con.Close();

            datalistado.Columns[0].Width = 145;
            datalistado.Columns[1].Width = 150;
            datalistado.Columns[2].Width = 420;
            datalistado.Columns[3].Width = 140;
            datalistado.Columns[4].Width = 162;

            datalistado.Columns[5].Visible = false;
            datalistado.Columns[6].Visible = false;
            datalistado.Columns[7].Visible = false;
            datalistado.Columns[8].Visible = false;
            datalistado.Columns[9].Visible = false;
            datalistado.Columns[10].Visible = false;
            datalistado.Columns[11].Visible = false;
            datalistado.Columns[12].Visible = false;
            datalistado.Columns[13].Visible = false;
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

            alternarColorFilas(datalistado);
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

        //NBOTON PAR AINGRESAR UN NUEVO CLIENTE
        private void btnAgregar2_Click(object sender, EventArgs e)
        {
            ValidarDni();
            ValidarRuc();
            ValidarOtro();

            if (cboTipoClientes.Text == "PERSONA JURÍDICA" && txtNombreClientes.Text == "" || cboTipoClientes.Text == "SUJETO NO DOMICILIADO" && txtNombreClientes.Text == "" || cboTipoClientes.Text == "ADQUIRIENTE TICKET" && txtNombreClientes.Text == "")
            {
                MessageBox.Show("Debe ingresar un nombre válido.", "Registro de Cliente", MessageBoxButtons.OK);
            }
            else if (cboTipoClientes.Text == "PERSONA NATURAL" && txtPrimerNombre.Text == "" || cboTipoClientes.Text == "PERSONA NATURAL" && txtApellidoPaterno.Text == "" || cboTipoClientes.Text == "PERSONA NATURAL" && txtApellidoMaterno.Text == "")
            {
                MessageBox.Show("Debe ingresar un nombre o apellidos válido.", "Registro de Cliente", MessageBoxButtons.OK);
            }
            else if (cboTipoDocumento.Text == "OTROS TIPOS DE DOCUMENTOS" && txtOtroDocumento.Text == "" || cboTipoDocumento.Text == "DOCUMENTO NACIONAL DE IDENTIDAD (D.N.I)" && txtDni.Text == "" || cboTipoDocumento.Text == "CARNET DE EXTRANJERIA" && txtOtroDocumento.Text == "" || cboTipoDocumento.Text == "REGISTRO UNICO DE CONTRIBUYENTE (R.U.C)" && txtRuc.Text == "" || cboTipoDocumento.Text == "PASAPORTE" && txtOtroDocumento.Text == "" || cboTipoDocumento.Text == "DOCUMENTO NACIONAL DE IDENTIDAD (D.N.I)" && txtDni.Text.Length != 8 || cboTipoDocumento.Text == "REGISTRO UNICO DE CONTRIBUYENTE (R.U.C)" && txtRuc.Text.Length != 11 || cboTipoDocumento.Text == "OTROS TIPOS DE DOCUMENTOS" && txtOtroDocumento.Text.Length == 0 || cboTipoDocumento.Text == "CARNET DE EXTRANJERIA" && txtOtroDocumento.Text.Length != 11 || cboTipoDocumento.Text == "PASAPORTE" && txtOtroDocumento.Text.Length != 12)
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
                if (txtTelefono.Text == "" && txtTelefonoFijo.Text == "" || txtTelefono.TextLength != 9)
                {
                    MessageBox.Show("Debe ingresar un número de teléfono movil o fijo válido.", "Registro de Cliente", MessageBoxButtons.OK);
                }
                else
                {
                    if (txtDireccion.Text == "" || cboDistrito.Text == "")
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
                                cmd = new SqlCommand("InsertarClientes", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@idtipocliente", cboTipoClientes.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@nombrecliente", txtNombreClientes.Text);
                                cmd.Parameters.AddWithValue("@primernombre", txtPrimerNombre.Text);
                                cmd.Parameters.AddWithValue("@segundonombre", txtSegundoNombre.Text);
                                cmd.Parameters.AddWithValue("@apellidopaterno", txtApellidoPaterno.Text);
                                cmd.Parameters.AddWithValue("@apellidomaterno", txtApellidoMaterno.Text);

                                if (txtTelefono.Text == "")
                                {
                                    cmd.Parameters.AddWithValue("@telefono", DBNull.Value);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@telefono", Convert.ToInt32(txtTelefono.Text));
                                }

                                if (txtTelefonoFijo.Text == "")
                                {
                                    cmd.Parameters.AddWithValue("@telefonofijo", DBNull.Value);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@telefonofijo", txtTelefonoFijo.Text);
                                }

                                cmd.Parameters.AddWithValue("@correo1", txtCorreo1.Text);
                                cmd.Parameters.AddWithValue("@correo2", txtCorreo2.Text);
                                cmd.Parameters.AddWithValue("@idgrupo", cboGrupo.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@idtipomoneda", cboMoneda.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@idreferencia", cboRetencion.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@idtipodocuemnto", cboTipoDocumento.SelectedValue.ToString());

                                if (txtDni.Text == "")
                                {
                                    cmd.Parameters.AddWithValue("@dni", "");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@dni", txtDni.Text);
                                }
                                if (txtRuc.Text == "")
                                {
                                    cmd.Parameters.AddWithValue("@ruc", "");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ruc", txtRuc.Text);
                                }
                                if (txtOtroDocumento.Text == "")
                                {
                                    cmd.Parameters.AddWithValue("@otros", "");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@otros", txtOtroDocumento.Text);
                                }

                                cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                                cmd.Parameters.AddWithValue("@referencia", txtReferencia.Text);
                                cmd.Parameters.AddWithValue("@idpais", cboPais.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@iddepartamento", cboDepartamento.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@idprovincia", cboProvincia.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@iddistrito", cboDistrito.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@lsoles", Convert.ToDecimal(txtSoles.Text));
                                cmd.Parameters.AddWithValue("@ldoalres", Convert.ToDecimal(txtDolares.Text));
                                cmd.Parameters.AddWithValue("@ubigeo", cboDistrito.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@codigo", txtCodigoClientes.Text);
                                cmd.ExecuteNonQuery();
                                con.Close();

                                MessageBox.Show("Cliente guardado exitosamente.", "Registro de Cliente", MessageBoxButtons.OK);
                                panelAgregarCliente.Visible = false;
                                Mostrar();
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

        //BOTON PARA EDITAR A UN CLIENTE SELECCIONADO
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (cboTipoClientes.Text == "PERSONA JURÍDICA" && txtNombreClientes.Text == "" || cboTipoClientes.Text == "SUJETO NO DOMICILIADO" && txtNombreClientes.Text == "" || cboTipoClientes.Text == "ADQUIRIENTE TICKET" && txtNombreClientes.Text == "")
            {
                MessageBox.Show("Debe ingresar un nombre válido.", "Registro de Cliente", MessageBoxButtons.OK);
            }
            else if (cboTipoClientes.Text == "PERSONA NATURAL" && txtPrimerNombre.Text == "" || cboTipoClientes.Text == "PERSONA NATURAL" && txtApellidoPaterno.Text == "" || cboTipoClientes.Text == "PERSONA NATURAL" && txtApellidoMaterno.Text == "")
            {
                MessageBox.Show("Debe ingresar un nombre o apellidos válidos.", "Registro de Cliente", MessageBoxButtons.OK);
            }
            else if (cboTipoDocumento.Text == "OTROS TIPOS DE DOCUMENTOS" && txtOtroDocumento.Text == "" || cboTipoDocumento.Text == "DOCUMENTO NACIONAL DE IDENTIDAD (D.N.I)" && txtDni.Text == "" || cboTipoDocumento.Text == "CARNET DE EXTRANJERIA" && txtOtroDocumento.Text == "" || cboTipoDocumento.Text == "REGISTRO UNICO DE CONTRIBUYENTE (R.U.C)" && txtRuc.Text == "" || cboTipoDocumento.Text == "PASAPORTE" && txtOtroDocumento.Text == "" || cboTipoDocumento.Text == "DOCUMENTO NACIONAL DE IDENTIDAD (D.N.I)" && txtDni.Text.Length != 8 || cboTipoDocumento.Text == "REGISTRO UNICO DE CONTRIBUYENTE (R.U.C)" && txtRuc.Text.Length != 11 || cboTipoDocumento.Text == "OTROS TIPOS DE DOCUMENTOS" && txtOtroDocumento.Text.Length == 0 || cboTipoDocumento.Text == "CARNET DE EXTRANJERIA" && txtOtroDocumento.Text.Length != 15 || cboTipoDocumento.Text == "PASAPORTE" && txtOtroDocumento.Text.Length != 15)
            {
                MessageBox.Show("Debe ingresar un número de documento válido.", "Registro de Cliente", MessageBoxButtons.OK);
            }
            else
            {
                if (txtTelefono.Text == "" && txtTelefonoFijo.Text == "" || txtTelefono.TextLength != 9 && txtTelefono.Text != "")
                {
                    MessageBox.Show("Debe ingresar un número de teléfono movil o fijo válido.", "Registro de Cliente", MessageBoxButtons.OK);
                }
                else
                {
                    if (txtDireccion.Text == "" || cboDistrito.Text == "")
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
                                cmd = new SqlCommand("EditarClientes", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@idcliente", idclienteseleccionado);
                                cmd.Parameters.AddWithValue("@idtipocliente", cboTipoClientes.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@nombrecliente", txtNombreClientes.Text);
                                cmd.Parameters.AddWithValue("@primernombre", txtPrimerNombre.Text);
                                cmd.Parameters.AddWithValue("@segundonombre", txtSegundoNombre.Text);
                                cmd.Parameters.AddWithValue("@apellidopaterno", txtApellidoPaterno.Text);
                                cmd.Parameters.AddWithValue("@apellidomaterno", txtApellidoMaterno.Text);

                                if (txtTelefono.Text == "")
                                {
                                    cmd.Parameters.AddWithValue("@telefono", DBNull.Value);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@telefono", Convert.ToInt32(txtTelefono.Text));
                                }

                                if (txtTelefonoFijo.Text == "")
                                {
                                    cmd.Parameters.AddWithValue("@telefonofijo", DBNull.Value);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@telefonofijo", txtTelefonoFijo.Text);
                                }

                                cmd.Parameters.AddWithValue("@correo1", txtCorreo1.Text);
                                cmd.Parameters.AddWithValue("@correo2", txtCorreo2.Text);
                                cmd.Parameters.AddWithValue("@idgrupo", cboGrupo.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@idtipomoneda", cboMoneda.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@idreferencia", cboRetencion.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@idtipodocuemnto", cboTipoDocumento.SelectedValue.ToString());

                                if (txtDni.Text == "")
                                {
                                    cmd.Parameters.AddWithValue("@dni", "");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@dni", txtDni.Text);
                                }
                                if (txtRuc.Text == "")
                                {
                                    cmd.Parameters.AddWithValue("@ruc", "");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ruc", txtRuc.Text);
                                }
                                if (txtOtroDocumento.Text == "")
                                {
                                    cmd.Parameters.AddWithValue("@otros", "");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@otros", txtOtroDocumento.Text);
                                }

                                cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                                cmd.Parameters.AddWithValue("@referencia", txtReferencia.Text);
                                cmd.Parameters.AddWithValue("@idpais", cboPais.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@iddepartamento", cboDepartamento.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@idprovincia", cboProvincia.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@iddistrito", cboDistrito.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@lsoles", Convert.ToDecimal(txtSoles.Text));
                                cmd.Parameters.AddWithValue("@ldoalres", Convert.ToDecimal(txtDolares.Text));
                                cmd.Parameters.AddWithValue("@ubigeo", cboDistrito.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@codigo", txtCodigoClientes.Text);
                                cmd.ExecuteNonQuery();
                                con.Close();

                                MessageBox.Show("Cliente editado exitosamente.", "Registro de Cliente", MessageBoxButtons.OK);
                                panelAgregarCliente.Visible = false;
                                Mostrar();
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

        //PANELES Y VENTANAS ANEXAS AL CLIENTE----------------------------------------------------
        //----------------------------------------------------------------------------------------
        //CARGA DE LISTADO DE CAMPOS CARGADOS AL CLIENTE---------------------------------------
        //MOSTARA UNIDADES DEL CLIENTE SELECCIOANDO
        public void MostrarUnidad(int idcliente)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarClienteUnidad", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idcliente", idcliente);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadounidad.DataSource = dt;
            con.Close();

            datalistadounidad.Columns[6].Visible = false;

            datalistadounidad.Columns[0].Width = 260;
            datalistadounidad.Columns[1].Width = 190;
            datalistadounidad.Columns[2].Width = 150;
            datalistadounidad.Columns[3].Width = 150;
            datalistadounidad.Columns[4].Width = 100;
            datalistadounidad.Columns[5].Width = 103;

            //deshabilitar el click y  reordenamiento por columnas
            foreach (DataGridViewColumn column in datalistadounidad.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //MOSTRAR CONTACTOS DEL CLIENTE SELECCIOANDO
        public void MostrarContacto(int idcliente)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarClienteContacto", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idcliente", idcliente);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadocontacto.DataSource = dt;
            con.Close();

            datalistadocontacto.Columns[7].Visible = false;

            datalistadocontacto.Columns[0].Width = 220;
            datalistadocontacto.Columns[1].Width = 85;
            datalistadocontacto.Columns[2].Width = 85;
            datalistadocontacto.Columns[3].Width = 180;
            datalistadocontacto.Columns[4].Width = 172;
            datalistadocontacto.Columns[5].Width = 105;
            datalistadocontacto.Columns[6].Width = 105;

            //deshabilitar el click y  reordenamiento por columnas
            foreach (DataGridViewColumn column in datalistadocontacto.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //MOSTRAR CONDICIONES DEL CLIENTE SELECCIOAND
        public void MostrarCondicion(int idcliente)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarClienteCondicion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idcliente", idcliente);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoCondicion.DataSource = dt;
            con.Close();

            datalistadoCondicion.Columns[3].Visible = false;

            datalistadoCondicion.Columns[0].Width = 430;
            datalistadoCondicion.Columns[1].Width = 290;
            datalistadoCondicion.Columns[2].Width = 233;

            //deshabilitar el click y  reordenamiento por columnas
            foreach (DataGridViewColumn column in datalistadoCondicion.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //MOSTRAR SUCURSALES DEL CLEINTE SELECCIOANDO
        public void MostrarSucursal(int idcliente)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarClienteSucursal", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idcliente", idcliente);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadosucursal.DataSource = dt;
            con.Close();

            datalistadosucursal.Columns[8].Visible = false;

            datalistadosucursal.Columns[0].Width = 250;
            datalistadosucursal.Columns[1].Width = 120;
            datalistadosucursal.Columns[2].Width = 150;
            datalistadosucursal.Columns[3].Width = 90;
            datalistadosucursal.Columns[4].Width = 100;
            datalistadosucursal.Columns[5].Width = 140;
            datalistadosucursal.Columns[6].Width = 140;
            datalistadosucursal.Columns[7].Width = 140;

            //deshabilitar el click y  reordenamiento por columnas
            foreach (DataGridViewColumn column in datalistadosucursal.Columns)
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
            MostrarUnidad(idclienteseleccionado);
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

        //ACCION DE GUARDAR LA UNIDAD PARA MI CLIENTE
        private void btnGuardarUnidad_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea ingresar esta unidad?.", "Nueva Unidad", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (txtLatitud.Text == "" || txtLongitud.Text == "" || txtNombreUnidad.Text == "")
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
                        cmd = new SqlCommand("InsertarClientes_Unidad", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idcliente", idclienteseleccionado);
                        cmd.Parameters.AddWithValue("@descipcion", txtNombreUnidad.Text);
                        cmd.Parameters.AddWithValue("@idresponsable", cboResponsable.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@idzona", cboZona.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@idpais", cboPaisUnidad.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@iddepartamento", cboDepartamentoUnidad.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@longitud", Convert.ToDecimal(txtLongitud.Text));
                        cmd.Parameters.AddWithValue("@latitud", Convert.ToDecimal(txtLatitud.Text));

                        cmd.ExecuteNonQuery();
                        con.Close();

                        MostrarUnidad(idclienteseleccionado);
                        MessageBox.Show("Registro ingresado exitosamente.", "Nueva Unidad", MessageBoxButtons.OK);

                        txtLatitud.Text = "";
                        txtLongitud.Text = "";
                        txtNombreUnidad.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //ACCION DE ELIMINAR UNA UNIDAD REGISTRADA DE MI CLEINTE
        private void btnEiminarUnidad_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea eliminar?.", "Eliminar Unidad", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (lblCodigoUnida.Text != "0")
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("EliminarCliente_Unidad", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt32(lblCodigoUnida.Text));
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Eliminación correcta, operación hecha satisfactoriamente.", "Eliminación de una Unidad", MessageBoxButtons.OK);
                        lblCodigoUnida.Text = "0";

                        MostrarUnidad(idclienteseleccionado);
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
            MostrarContacto(idclienteseleccionado);
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

        //ACCION DE GUARDAR CONTACTO PARA MI CLIENTE
        private void btnGuardarContacto_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea ingresar este contacto?.", "Nuevo Contacto", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (txtNombreContacto.Text == "" || txtTelefonoContacto.Text == "" || txtCorreoContacto.Text == "" || cboUnidadContacto.SelectedValue == null || cboUnidadContacto.Text == "")
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
                        cmd = new SqlCommand("InsertarClientes_Contacto", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idcliente", idclienteseleccionado);
                        cmd.Parameters.AddWithValue("@descipcion", txtNombreContacto.Text);
                        cmd.Parameters.AddWithValue("@telefono", txtTelefonoContacto.Text);
                        cmd.Parameters.AddWithValue("@anexo", txtAnexoContacto.Text);
                        cmd.Parameters.AddWithValue("@correo", txtCorreoContacto.Text);
                        cmd.Parameters.AddWithValue("@idunidad", cboUnidadContacto.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@idarea", cboAreaContacto.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@idcargo", cboCargoContacto.SelectedValue.ToString());
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MostrarContacto(idclienteseleccionado);
                        MessageBox.Show("Registro ingresado exitosamente.", "Nuevo Contacto", MessageBoxButtons.OK);

                        txtNombreContacto.Text = "";
                        txtTelefonoContacto.Text = "";
                        txtAnexoContacto.Text = "";
                        txtCorreoContacto.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //ACCION DE ELIMINAR UNA CONDTACTO REGISTRADA DE MI CLEINTE
        private void btnEliminarContactos_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea eliminar?.", "Eliminar Contacto", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (lblCodigoContacto.Text != "0")
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("EliminarCliente_Contacto", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt32(lblCodigoContacto.Text));
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MostrarContacto(idclienteseleccionado);
                        MessageBox.Show("Eliminación correcta, operación hecha satisfactoriamente.", "Eliminación Contacto", MessageBoxButtons.OK);
                        lblCodigoContacto.Text = "0";
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
            MostrarCondicion(idclienteseleccionado);
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
        private void btnGuardarCondicion_Click(object sender, EventArgs e)
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
                    cmd = new SqlCommand("InsertarClientes_Condicion", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@idcliente", idclienteseleccionado);
                    cmd.Parameters.AddWithValue("@idcondicion", cboCondicionCondicion.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@idforma", cboFormaCondicion.SelectedValue.ToString());
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MostrarCondicion(idclienteseleccionado);
                    MessageBox.Show("Registro ingresado exitosamente.", "Nueva Condición", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //ACCION DE ELIMINAR CONIDCION REGISTRADA DE MI CLEINTE
        private void btnEliminarCondicion_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea eliminar?.", "Eliminar Condición", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (lblCodigoCOndicion.Text != "0")
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("EliminarCliente_Condicion", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt32(lblCodigoCOndicion.Text));
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MostrarCondicion(idclienteseleccionado);
                        MessageBox.Show("Eliminación correcta, operación hecha satisfactoriamente.", "Eliminación de una Condición", MessageBoxButtons.OK);
                        lblCodigoCOndicion.Text = "0";
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

        //VOLVER O SALIR DE CONDICION
        private void btnRetrocederCondicion_Click(object sender, EventArgs e)
        {
            panelCondicion.Visible = false;
        }

        //ACCIONES SUCURSAL------------------------------------------------------------------------
        //ENTRAR A SUCURSAL DEL CLIENTE
        private void lblSucursal_Click(object sender, EventArgs e)
        {
            MostrarSucursal(idclienteseleccionado);
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

        //ACCION DE GUARDAR SUCURSAL PARA MI CLIENTE
        private void btnGuardarSucursal_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea ingresar esta sucursal?.", "Registro de Sucursal", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (txtNombreSucursal.Text == "" || txtTelefonoSucursal.Text == "" || txtDireccionSucursal.Text == "")
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
                        cmd = new SqlCommand("InsertarClientes_Sucursal", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idcliente", idclienteseleccionado);
                        cmd.Parameters.AddWithValue("@nombre", txtNombreSucursal.Text);
                        cmd.Parameters.AddWithValue("@direccion", txtDireccionSucursal.Text);
                        cmd.Parameters.AddWithValue("@telefono", txtTelefonoSucursal.Text);
                        cmd.Parameters.AddWithValue("@codigopais", cboPaisSucursal.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@codigodepartamento", cboDepartamentoSucursal.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@codigoprovincia", cboProvinciaSucursal.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@codigodistrito", cboDistritoSucursal.SelectedValue.ToString());
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MostrarSucursal(idclienteseleccionado);
                        MessageBox.Show("Registro ingresado exitosamente.", "Nuevo Sucursal", MessageBoxButtons.OK);

                        txtNombreSucursal.Text = "";
                        txtDireccionSucursal.Text = "";
                        txtTelefonoSucursal.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //ACCION DE ELIMINAR SUCRUSAL REGISTRADA DE MI CLEINTE
        private void btnEliminarSucursal_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea eliminar?.", "Eliminar Sucursal", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (lblCodigoSucursal.Text != "0")
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("EliminarCliente_Sucursal", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(lblCodigoSucursal.Text));
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MostrarSucursal(idclienteseleccionado);
                    MessageBox.Show("Eliminación correcta, operación hecha satisfactoriamente.", "Eliminación nueva", MessageBoxButtons.OK);
                    lblCodigoSucursal.Text = "0";
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un registro para poder eliminarlo.", "Eliminación de una Sucursal", MessageBoxButtons.OK);
                }
            }
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

        //BUSCAR CLEITNE POR NOMBRE O DOCUEMTO
        private void txtCliente_TextChanged(object sender, EventArgs e)
        {
            if (cboTipoBusqueda.Text == "NOMBRES")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("BuscarClientePorNombre", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", txtCliente.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistado.DataSource = dt;
                    con.Close();

                    datalistado.Columns[0].Width = 145;
                    datalistado.Columns[1].Width = 150;
                    datalistado.Columns[2].Width = 420;
                    datalistado.Columns[3].Width = 140;
                    datalistado.Columns[4].Width = 162;

                    datalistado.Columns[5].Visible = false;
                    datalistado.Columns[6].Visible = false;
                    datalistado.Columns[7].Visible = false;
                    datalistado.Columns[8].Visible = false;
                    datalistado.Columns[9].Visible = false;
                    datalistado.Columns[10].Visible = false;
                    datalistado.Columns[11].Visible = false;
                    datalistado.Columns[12].Visible = false;
                    datalistado.Columns[13].Visible = false;
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (cboTipoBusqueda.Text == "DOCUMENTO")
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("BuscarClientePorDocumento", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@documento", txtCliente.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistado.DataSource = dt;
                    con.Close();

                    datalistado.Columns[0].Width = 145;
                    datalistado.Columns[1].Width = 150;
                    datalistado.Columns[2].Width = 420;
                    datalistado.Columns[3].Width = 140;
                    datalistado.Columns[4].Width = 162;

                    datalistado.Columns[5].Visible = false;
                    datalistado.Columns[6].Visible = false;
                    datalistado.Columns[7].Visible = false;
                    datalistado.Columns[8].Visible = false;
                    datalistado.Columns[9].Visible = false;
                    datalistado.Columns[10].Visible = false;
                    datalistado.Columns[11].Visible = false;
                    datalistado.Columns[12].Visible = false;
                    datalistado.Columns[13].Visible = false;
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
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

        private void txtSoles_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 1. Permite la tecla de control (como Backspace)
            if (char.IsControl(e.KeyChar))
            {
                return; // Permite el carácter y sale
            }

            // 2. Permite dígitos del 0 al 9
            if (char.IsDigit(e.KeyChar))
            {
                return; // Permite el dígito y sale
            }

            // 3. Permite el punto decimal '.'
            // (Asegúrate de que solo se pueda ingresar uno y que el control lo tenga como separador decimal)
            if (e.KeyChar == '.')
            {
                // El TextBox actual
                TextBox textBox = (TextBox)sender;

                // Si el texto ya contiene un punto, o si es la primera posición y no es un punto...
                if (textBox.Text.Contains("."))
                {
                    e.Handled = true; // Ignora el punto si ya existe uno
                    return;
                }
                return; // Permite el primer punto y sale
            }

            // Si no es un dígito, control, o el primer punto, se ignora
            e.Handled = true;
        }

        //private void txtSoles_TextChanged(object sender, EventArgs e)
        //{

        //}

        private void txtSoles_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (char.IsControl(e.KeyChar))
            {
                return; 
            }

            // 2. PERMITIR Dígitos (0-9)
            if (char.IsDigit(e.KeyChar))
            {
                return; 
            }

            
            if (e.KeyChar == '.')
            {
                
                if (textBox.Text.Contains("."))
                {
                    e.Handled = true; 
                    return;
                }
                return; 
            }

            
            e.Handled = true; 
        }

        private void txtDolares_KeyPress(object sender, KeyPressEventArgs e)
        {
    
            TextBox textBox = (TextBox)sender;

           
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

           
            if (char.IsDigit(e.KeyChar))
            {
                return; 
            }

            
            if (e.KeyChar == '.')
            {
               
                if (textBox.Text.Contains("."))
                {
                    e.Handled = true; 
                    return;
                }
                return; 
            }

          
            e.Handled = true; 
        }
    }
}

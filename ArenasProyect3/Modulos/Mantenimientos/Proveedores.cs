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

namespace ArenasProyect3.Modulos.Mantenimientos
{
    public partial class Proveedores : Form
    {
        //VARIABLES CREADAS PARA PROVEEDORES
        //GENERACIÓN DE CÓDIGO
        string codigo1;
        string codigo2;
        string codigo3;
        string codigo4;
        string codigo5;

        int idproveedorseleccionado = 0;
        bool EstadoDni = false;
        bool EstadoRuc = false;
        bool EstadoOtro = false;

        //CONSTRUCTOR DE MI FORMULARIO
        public Proveedores()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI FORMULARIO
        private void Proveedores_Load(object sender, EventArgs e)
        {
            //PRIMERA CARGA DEL FORMULACIO
            Mostrar();
            cboTipoBusqueda.SelectedIndex = 0;
        }

        //CARGA DE LOS CAMBOS------------------------------------------------------
        public void CargarTipoCliente()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdTipoClientes, Descripcion FROM TipoClientes WHERE Estado = 1 ORDER BY Descripcion", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboTipoProveedor.DisplayMember = "Descripcion";
            cboTipoProveedor.ValueMember = "IdTipoClientes";
            cboTipoProveedor.DataSource = dt;
        }

        //FUNCION PARA CARGAR EL TIPO DE DOCUMENTOS
        public void CargarTipoDocumentos()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdTipoDocumento, Descripcion FROM TipoDocumentos WHERE Estado = 1 ORDER BY Descripcion", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboTipoDocumento.DisplayMember = "Descripcion";
            cboTipoDocumento.ValueMember = "IdTipoDocumento";
            cboTipoDocumento.DataSource = dt;
        }

        //FUNCION PARA CARGAR EL TIPO DE PROVEEDOR
        public void CargarTipoProveedor()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdTipoProveedor, Descripcion FROM TipoProveedor WHERE Estado = 1 ORDER BY Descripcion", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboProveedor.DisplayMember = "Descripcion";
            cboProveedor.ValueMember = "IdTipoProveedor";
            cboProveedor.DataSource = dt;
        }

        //FUNCION PARA CARGAR EL TIPO DE PROCEDENCIA
        public void CargarTipoProcedencia()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdTipoGrupo, Descripcion FROM TipoGrupo WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboProcedencia.DisplayMember = "Descripcion";
            cboProcedencia.ValueMember = "IdTipoGrupo";
            cboProcedencia.DataSource = dt;
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
            SqlCommand comando = new SqlCommand("SELECT CodigoDepartamento, Descripcion FROM UbicacionDepartamento WHERE CodigoPais = @idpais ORDER BY Descripcion", con);
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
            SqlCommand comando = new SqlCommand("SELECT CodigoProvincia, Descripcion FROM  UbicacionProvincia WHERE CodigoDepartamento= @iddepartamento ORDER BY Descripcion", con);
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
            SqlCommand comando = new SqlCommand("SELECT CodigoDistrito,Descripcion FROM  UbicacionDistrito WHERE CodigoProvincia = @idprovincia ORDER BY Descripcion", con);
            comando.Parameters.AddWithValue("@idprovincia", idprovincia);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cbo.ValueMember = "CodigoDistrito";
            cbo.DisplayMember = "Descripcion";
            cbo.DataSource = dt;
        }

        //ACCIONES DE LOS COMBOS AL SELECCIONAR - UBICACION DE CLIENTES Y OTROS MANTENIMIENTOS---
        private void cboPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPais.SelectedValue.ToString() != null)
            {
                string idpais = cboPais.SelectedValue.ToString();
                CargarDepartamento(cboDepartamento, idpais);
            }
        }

        //ACCIONES DE LOS COMBOS AL SELECCIONAR 
        private void cboDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDepartamento.SelectedValue.ToString() != null)
            {
                string iddepartamento = cboDepartamento.SelectedValue.ToString();
                CargarProvincia(cboProvincia, iddepartamento);
            }
        }

        //ACCIONES DE LOS COMBOS AL SELECCIONAR 
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
            da = new SqlDataAdapter("SELECT TC.Descripcion AS [TIPO], P.Ruc + '' + P.DNI + '' + P.OTROS AS [NÚMERO DE DOCUMENTO], NombreProveedor + P.PrimerNombre + ' ' + P.ApellidoPaterno + ' ' + P.ApellidoMaterno AS [NOMBRE / NOMBRES Y APELLIDOS], P.Telefono AS [TELÉFONO] ,P.Correo AS [CORREO],P.IdTipoCLiente,P.NombreProveedor,P.PrimerNombre,P.SegundoNombre,P.ApellidoPaterno,P.ApellidoMaterno,P.Telefono,P.Correo,P.PaginaWeb,P.Direccion,UP.CodigoPais,UD.CodigoDepartamento,PR.CodigoProvincia,DI.CodigoDistrito,P.IdProcedencia,P.IdTipoDocumento,P.RUC,P.DNI,P.OTROS,P.Detraccion,P.Declarante,P.Percepcion,P.Retencion,P.Lsoles,P.Ldolares,P.Codigo,P.IdProveedor FROM Proveedores P INNER JOIN TipoClientes TC ON P.IdTipoCliente = TC.IdTipoClientes INNER JOIN UbicacionPais UP ON P.CodigoPais = UP.CodigoPais INNER JOIN UbicacionDepartamento UD ON P.CodigoDepartamento = UD.CodigoDepartamento INNER JOIN UbicacionProvincia PR ON P.CodigoProvincia = PR.CodigoProvincia INNER JOIN UbicacionDistrito DI ON P.CodigoDistrito = DI.CodigoDistrito WHERE P.Estado = 1", con);
            da.Fill(dt);
            datalistado.DataSource = dt;
            con.Close();

            datalistado.Columns[0].Width = 150;
            datalistado.Columns[1].Width = 150;
            datalistado.Columns[2].Width = 420;
            datalistado.Columns[3].Width = 150;
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
            datalistado.Columns[31].Visible = false;

            alternarColorFilas(datalistado);
        }

        //VALIDADORES DE EXISTENCIA-----------------------------------------------------------
        //VALIDAR DNI
        public void ValidarDni()
        {
            foreach (DataGridViewRow datorecuperado in datalistado.Rows)
            {
                string dni = Convert.ToString(datorecuperado.Cells["NÚMERO DE DOCUMENTO"].Value);
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
                string ruc = Convert.ToString(datorecuperado.Cells["NÚMERO DE DOCUMENTO"].Value);
                if (ruc == txtRuc.Text)
                {
                    EstadoRuc = true;
                    return;
                }
            }
            return;
        }

        //VALIDAR OTRO DOCUMENTO
        public void ValidarOtro()
        {
            foreach (DataGridViewRow datorecuperado in datalistado.Rows)
            {
                string otro = Convert.ToString(datorecuperado.Cells["NÚMERO DE DOCUMENTO"].Value);
                if (otro == txtOtroDocumento.Text)
                {
                    EstadoOtro = true;
                    return;
                }
            }
            return;
        }

        //FUNCION PARA COLORER MI LSITADO
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
                MessageBox.Show(ex.Message);
            }
        }
        //------------------------------------------------------------------------------

        //SELECCION DE UN REGISTRO O CLIENTE
        private void datalistado_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            CargarTipoProveedor();
            CargarPais(cboPais);
            CargarTipoCliente();
            CargarTipoProcedencia();
            CargarTipoDocumentos();
            idproveedorseleccionado = Convert.ToInt32(datalistado.SelectedCells[31].Value.ToString());
            btnAgregar2.Visible = false;
            btnEditar.Visible = true;
            panelAgregarProveedor.Visible = true;
            lblTituloPanel.Text = "Editar Proveedor";
            lblCuentaProducto.Visible = true;
            lblContacto.Visible = true;
            lblCuentasBancarias.Visible = true;
            lblSucursal.Visible = true;

            //Datos Personales/ Juridico/ Codigos
            txtCodigoProveedores.Text = datalistado.SelectedCells[30].Value.ToString();
            cboTipoProveedor.SelectedValue = datalistado.SelectedCells[5].Value.ToString();
            txtNombreProveddor.Text = datalistado.SelectedCells[6].Value.ToString();

            //Datos Personales
            txtPrimerNombre.Text = datalistado.SelectedCells[7].Value.ToString();
            txtSegundoNombre.Text = datalistado.SelectedCells[8].Value.ToString();
            txtApellidoPaterno.Text = datalistado.SelectedCells[9].Value.ToString();
            txtApellidoMaterno.Text = datalistado.SelectedCells[10].Value.ToString();

            //Datos Contacto
            txtTelefono.Text = datalistado.SelectedCells[3].Value.ToString();
            txtCorreo.Text = datalistado.SelectedCells[4].Value.ToString();
            txtPaginaWEB.Text = datalistado.SelectedCells[13].Value.ToString();

            //Datos Comercio
            cboTipoDocumento.SelectedValue = datalistado.SelectedCells[20].Value.ToString();

            //Datos personales 2
            txtRuc.Text = datalistado.SelectedCells[21].Value.ToString();
            txtDni.Text = datalistado.SelectedCells[22].Value.ToString();
            txtOtroDocumento.Text = datalistado.SelectedCells[23].Value.ToString();
            txtDireccion.Text = datalistado.SelectedCells[14].Value.ToString();

            //Datos Ubicacion
            cboPais.SelectedValue = datalistado.SelectedCells[15].Value.ToString();
            cboDepartamento.SelectedValue = datalistado.SelectedCells[16].Value.ToString();
            cboProvincia.SelectedValue = datalistado.SelectedCells[17].Value.ToString();
            cboDistrito.SelectedValue = datalistado.SelectedCells[18].Value.ToString();

            //Datos Cantidades
            txtSoles.Text = datalistado.SelectedCells[28].Value.ToString();
            txtDolares.Text = datalistado.SelectedCells[29].Value.ToString();

            //Datos ChekBox
            int detracion = Convert.ToInt32(datalistado.SelectedCells[24].Value.ToString());
            int declarante = Convert.ToInt32(datalistado.SelectedCells[25].Value.ToString());
            int percepcion = Convert.ToInt32(datalistado.SelectedCells[26].Value.ToString());
            int retencion = Convert.ToInt32(datalistado.SelectedCells[27].Value.ToString());

            if (detracion == 1)
            {
                ckDetraccion.Checked = true;
            }
            else
            {
                ckDetraccion.Checked = false;
            }

            if (declarante == 1)
            {
                ckDeclarante.Checked = true;
            }
            else
            {
                ckDeclarante.Checked = false;
            }

            if (percepcion == 1)
            {
                ckPercepcion.Checked = true;
            }
            else
            {
                ckPercepcion.Checked = false;
            }

            if (retencion == 1)
            {
                ckRetencion.Checked = true;
            }
            else
            {
                ckRetencion.Checked = false;
            }
        }

        //FUNCION PARA AGREGAR UN NUEVO PROVEEDOR
        private void btnAgregarProveedor_Click(object sender, EventArgs e)
        {
            panelAgregarProveedor.Visible = true;
            LimpiarCamposNuevoProveedor();
        }

        //FUNCION PARA LIMPIAR MI NUEVVO PROVEEDOR
        public void LimpiarCamposNuevoProveedor()
        {
            CargarTipoDocumentos();
            CargarTipoProveedor();
            CargarPais(cboPais);
            CargarTipoCliente();
            CargarTipoProcedencia();
            btnEditar.Visible = false;
            btnAgregar2.Visible = true;
            lblTituloPanel.Text = "Nuevo Proveedor";
            lblCuentaProducto.Visible = false;
            lblContacto.Visible = false;
            lblCuentasBancarias.Visible = false;
            lblSucursal.Visible = false;

            //Limpiesa de campos
            txtCodigoProveedores.Text = "";
            cboTipoProveedor.SelectedValue = 1;
            txtNombreProveddor.Text = "";
            txtPrimerNombre.Text = "";
            txtSegundoNombre.Text = "";
            txtApellidoPaterno.Text = "";
            txtApellidoMaterno.Text = "";
            txtTelefono.Text = "";
            txtCorreo.Text = "";
            txtPaginaWEB.Text = "";
            txtDireccion.Text = "";
            txtRuc.Text = "";
            txtOtroDocumento.Text = "";
            txtDni.Text = "";

            ckDetraccion.Checked = false;
            ckDeclarante.Checked = false;
            ckPercepcion.Checked = false;
            ckRetencion.Checked = false;

            txtSoles.Text = "0.00";
            txtDolares.Text = "0.00";
        }

        //FUNCION PARA REGRESAR DE MI NUEVO PROVEEDOR
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            panelAgregarProveedor.Visible = false;
            LimpiarCamposNuevoProveedor();
        }

        //JUEGO DE COMBOS-------
        //AL SELECICOINAR UN TIPO DE PROVEEDOR
        private void cboTipoProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoProveedor.Text == "PERSONA JURÍDICA")
            {
                txtNombreProveddor.ReadOnly = false;

                txtPrimerNombre.ReadOnly = true;
                txtSegundoNombre.ReadOnly = true;
                txtApellidoPaterno.ReadOnly = true;
                txtApellidoMaterno.ReadOnly = true;
            }
            else if (cboTipoProveedor.Text == "PERSONA NATURAL")
            {
                txtNombreProveddor.ReadOnly = true;

                txtPrimerNombre.ReadOnly = false;
                txtSegundoNombre.ReadOnly = false;
                txtApellidoPaterno.ReadOnly = false;
                txtApellidoMaterno.ReadOnly = false;
            }
            else if (cboTipoProveedor.Text == "SUJETO NO DOMICILIADO")
            {
                txtNombreProveddor.ReadOnly = false;

                txtPrimerNombre.ReadOnly = true;
                txtSegundoNombre.ReadOnly = true;
                txtApellidoPaterno.ReadOnly = true;
                txtApellidoMaterno.ReadOnly = true;
            }
            else if (cboTipoProveedor.Text == "ADQUIRIENTE TICKET")
            {
                txtNombreProveddor.ReadOnly = false;

                txtPrimerNombre.ReadOnly = true;
                txtSegundoNombre.ReadOnly = true;
                txtApellidoPaterno.ReadOnly = true;
                txtApellidoMaterno.ReadOnly = true;
            }
        }

        //AL SELECCIONAR UN TIPO DE DOCUEMNTO PARA MI RPOVEEDOR
        private void cboTipoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoDocumento.Text == "OTROS TIPOS DE DOCUMENTOS")
            {
                txtDni.ReadOnly = true;
                txtRuc.ReadOnly = true;
                txtOtroDocumento.ReadOnly = false;
                //cboProcedencia.SelectedIndex = 1;
                //txtDni.Text = "";
                //txtRuc.Text = "";
                //txtOtroDocumento.Text = "";
            }
            else if (cboTipoDocumento.Text == "DOCUMENTO NACIONAL DE IDENTIDAD (D.N.I)")
            {
                txtDni.ReadOnly = false;
                txtRuc.ReadOnly = true;
                txtOtroDocumento.ReadOnly = true;
                //cboProcedencia.SelectedIndex = 0;
                //txtDni.Text = "";
                //txtRuc.Text = "";
                //txtOtroDocumento.Text = "";
            }
            else if (cboTipoDocumento.Text == "CARNET DE EXTRANJERIA")
            {
                txtDni.ReadOnly = true;
                txtRuc.ReadOnly = true;
                txtOtroDocumento.ReadOnly = false;
                //cboProcedencia.SelectedIndex = 1;
                //txtDni.Text = "";
                //txtRuc.Text = "";
                //txtOtroDocumento.Text = "";
            }
            else if (cboTipoDocumento.Text == "REGISTRO UNICO DE CONTRIBUYENTE (R.U.C)")
            {
                txtDni.ReadOnly = true;
                txtRuc.ReadOnly = false;
                txtOtroDocumento.ReadOnly = true;
                //cboProcedencia.SelectedIndex = 0;
                //txtDni.Text = "";
                //txtRuc.Text = "";
                //txtOtroDocumento.Text = "";
            }
            else if (cboTipoDocumento.Text == "PASAPORTE")
            {
                txtDni.ReadOnly = true;
                txtRuc.ReadOnly = true;
                txtOtroDocumento.ReadOnly = false;
                //cboProcedencia.SelectedIndex = 1;
                //txtDni.Text = "";
                //txtRuc.Text = "";
                //txtOtroDocumento.Text = "";
            }
            else
            {
                txtDni.ReadOnly = true;
                txtRuc.ReadOnly = true;
                txtOtroDocumento.ReadOnly = false;
                //cboProcedencia.SelectedIndex = 1;
                //txtDni.Text = "";
                //txtRuc.Text = "";
                //txtOtroDocumento.Text = "";
            }
        }

        //FUNCION DE GENERAR AL MOMENTO DE INGRESAR
        private void txtDni_TextChanged(object sender, EventArgs e)
        {
            if (cboTipoDocumento.Text == "DOCUMENTO NACIONAL DE IDENTIDAD (D.N.I)")
            {
                codigo2 = "CDNI" + txtDni.Text;
                txtCodigoProveedores.Text = codigo2;
            }
        }

        //FUNCION DE GENERAR AL MOMENTO DE INGRESAR
        private void txtRuc_TextChanged(object sender, EventArgs e)
        {
            if (cboTipoDocumento.Text == "REGISTRO UNICO DE CONTRIBUYENTE (R.U.C)")
            {
                codigo4 = "CRUC" + txtRuc.Text;
                txtCodigoProveedores.Text = codigo4;
            }
        }

        //FUNCION DE GENERAR AL MOMENTO DE INGRESAR
        private void txtOtroDocumento_TextChanged(object sender, EventArgs e)
        {
            if (cboTipoDocumento.Text == "OTROS TIPOS DE DOCUMENTOS")
            {
                codigo1 = "COTD" + txtOtroDocumento.Text;
                txtCodigoProveedores.Text = codigo1;
            }
            else if (cboTipoDocumento.Text == "CARNET DE EXTRANJERIA")
            {
                codigo3 = "CCDE" + txtOtroDocumento.Text;
                txtCodigoProveedores.Text = codigo3;
            }
            else if (cboTipoDocumento.Text == "PASAPORTE")
            {
                codigo5 = "CPAS" + txtOtroDocumento.Text;
                txtCodigoProveedores.Text = codigo5;
            }
        }

        //FUNCION PARA AGREGAR UN NUEVO PROVEEDOR
        private void btnAgregar2_Click(object sender, EventArgs e)
        {
            ValidarDni();
            ValidarRuc();
            ValidarOtro();

            if (cboTipoProveedor.Text == "PERSONA JURÍDICA" && txtNombreProveddor.Text == "" || cboTipoProveedor.Text == "SUJETO NO DOMICILIADO" && txtNombreProveddor.Text == "" || cboTipoProveedor.Text == "ADQUIRIENTE TICKET" && txtNombreProveddor.Text == "")
            {
                MessageBox.Show("Debe ingresar un nombre válido.", "Registro de Proveedores", MessageBoxButtons.OK);
            }
            else if (cboTipoProveedor.Text == "PERSONA NATURAL" && txtPrimerNombre.Text == "" || cboTipoProveedor.Text == "PERSONA NATURAL" && txtApellidoPaterno.Text == "" || cboTipoProveedor.Text == "PERSONA NATURAL" && txtApellidoMaterno.Text == "")
            {
                MessageBox.Show("Debe ingresar un nombre o apellidos válido.", "Registro de Proveedores", MessageBoxButtons.OK);
            }
            else if (cboTipoDocumento.Text == "OTROS TIPOS DE DOCUMENTOS" && txtOtroDocumento.Text == "" || cboTipoDocumento.Text == "DOCUMENTO NACIONAL DE IDENTIDAD (D.N.I)" && txtDni.Text == "" || cboTipoDocumento.Text == "CARNET DE EXTRANJERIA" && txtOtroDocumento.Text == "" || cboTipoDocumento.Text == "REGISTRO UNICO DE CONTRIBUYENTE (R.U.C)" && txtRuc.Text == "" || cboTipoDocumento.Text == "PASAPORTE" && txtOtroDocumento.Text == "" || cboTipoDocumento.Text == "DOCUMENTO NACIONAL DE IDENTIDAD (D.N.I)" && txtDni.Text.Length != 8 || cboTipoDocumento.Text == "REGISTRO UNICO DE CONTRIBUYENTE (R.U.C)" && txtRuc.Text.Length != 11 || cboTipoDocumento.Text == "OTROS TIPOS DE DOCUMENTOS" && txtOtroDocumento.Text.Length == 0 || cboTipoDocumento.Text == "CARNET DE EXTRANJERIA" && txtOtroDocumento.Text.Length != 11 || cboTipoDocumento.Text == "PASAPORTE" && txtOtroDocumento.Text.Length != 12)
            {
                MessageBox.Show("Debe ingresar un número de documento válido.", "Registro de Proveedores", MessageBoxButtons.OK);
            }
            else if (EstadoDni == true || EstadoRuc == true || EstadoOtro == true)
            {
                MessageBox.Show("El documento ingresado ya se encuentra registrado en el sistema.", "Registro de Proveedores", MessageBoxButtons.OK);
                EstadoDni = false;
                EstadoRuc = false;
                EstadoOtro = false;
            }
            else
            {
                if (txtTelefono.Text == "" && txtTelefonoFijo.Text == "" || txtTelefono.TextLength != 9)
                {
                    MessageBox.Show("Debe ingresar un número de teléfono movil o fijo válido.", "Registro de Proveedores", MessageBoxButtons.OK);
                }
                else
                {
                    try
                    {
                        DialogResult boton = MessageBox.Show("¿Realmente desea guardar a este Proveedor?.", "Registro de Proveedores", MessageBoxButtons.OKCancel);
                        if (boton == DialogResult.OK)
                        {
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("InsertarProveedor", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@codigo", txtCodigoProveedores.Text);
                            cmd.Parameters.AddWithValue("@idTipoProveedor", cboTipoProveedor.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@nombreProveedor", txtNombreProveddor.Text);
                            cmd.Parameters.AddWithValue("@primerNombre", txtPrimerNombre.Text);
                            cmd.Parameters.AddWithValue("@segundoNombre", txtSegundoNombre.Text);
                            cmd.Parameters.AddWithValue("@apellidoPaterno", txtApellidoPaterno.Text);
                            cmd.Parameters.AddWithValue("@apellidoMaterno", txtApellidoMaterno.Text);

                            if (txtTelefono.Text == "")
                            {
                                cmd.Parameters.AddWithValue("@telefono", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@telefono", Convert.ToInt32(txtTelefono.Text));
                            }


                            cmd.Parameters.AddWithValue("@correo", txtCorreo.Text);
                            cmd.Parameters.AddWithValue("@paginaWeb", txtPaginaWEB.Text);
                            cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text);

                            cmd.Parameters.AddWithValue("@codigoPais", cboPais.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@codigoDepartamento", cboDepartamento.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@codigoProvincia", cboProvincia.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@codigoDistrito", cboDistrito.SelectedValue.ToString());

                            cmd.Parameters.AddWithValue("@idProcedencia", cboProcedencia.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@idTipoDocumento", cboTipoDocumento.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@ruc", txtRuc.Text);
                            cmd.Parameters.AddWithValue("@dni", txtDni.Text);
                            cmd.Parameters.AddWithValue("@otros", txtOtroDocumento.Text);

                            //------------------------------
                            if (ckDetraccion.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@detraccion", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@detraccion", 0);
                            }
                            if (ckDeclarante.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@declarante", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@declarante", 0);
                            }
                            if (ckPercepcion.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@percepcion", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@percepcion", 0);
                            }
                            if (ckRetencion.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@retencion", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@retencion", 0);
                            }

                            cmd.Parameters.AddWithValue("@lsoles", Convert.ToDecimal(txtSoles.Text));
                            cmd.Parameters.AddWithValue("@ldolares", Convert.ToDecimal(txtDolares.Text));
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Proveedor gaurdado exitosamente.", "Registro de Proveedores", MessageBoxButtons.OK);
                            panelAgregarProveedor.Visible = false;
                            Mostrar();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //EDITAR A UN PROVEEDOR
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (cboTipoProveedor.Text == "PERSONA JURÍDICA" && txtNombreProveddor.Text == "" || cboTipoProveedor.Text == "SUJETO NO DOMICILIADO" && txtNombreProveddor.Text == "" || cboTipoProveedor.Text == "ADQUIRIENTE TICKET" && txtNombreProveddor.Text == "")
            {
                MessageBox.Show("Debe ingresar un nombre válido.", "Registro de Proveedores", MessageBoxButtons.OK);
            }
            else if (cboTipoProveedor.Text == "PERSONA NATURAL" && txtPrimerNombre.Text == "" || cboTipoProveedor.Text == "PERSONA NATURAL" && txtApellidoPaterno.Text == "" || cboTipoProveedor.Text == "PERSONA NATURAL" && txtApellidoMaterno.Text == "")
            {
                MessageBox.Show("Debe ingresar un nombre o apellidos válidos.", "Registro de Proveedores", MessageBoxButtons.OK);
            }
            else if (cboTipoDocumento.Text == "OTROS TIPOS DE DOCUMENTOS" && txtOtroDocumento.Text == "" || cboTipoDocumento.Text == "DOCUMENTO NACIONAL DE IDENTIDAD (D.N.I)" && txtDni.Text == "" || cboTipoDocumento.Text == "CARNET DE EXTRANJERIA" && txtOtroDocumento.Text == "" || cboTipoDocumento.Text == "REGISTRO UNICO DE CONTRIBUYENTE (R.U.C)" && txtRuc.Text == "" || cboTipoDocumento.Text == "PASAPORTE" && txtOtroDocumento.Text == "" || cboTipoDocumento.Text == "DOCUMENTO NACIONAL DE IDENTIDAD (D.N.I)" && txtDni.Text.Length != 8 || cboTipoDocumento.Text == "REGISTRO UNICO DE CONTRIBUYENTE (R.U.C)" && txtRuc.Text.Length != 11 || cboTipoDocumento.Text == "OTROS TIPOS DE DOCUMENTOS" && txtOtroDocumento.Text.Length == 0 || cboTipoDocumento.Text == "CARNET DE EXTRANJERIA" && txtOtroDocumento.Text.Length != 15 || cboTipoDocumento.Text == "PASAPORTE" && txtOtroDocumento.Text.Length != 15)
            {
                MessageBox.Show("Debe ingresar un número de documento válido.", "Registro de Proveedores", MessageBoxButtons.OK);
            }
            else
            {
                if (txtTelefono.Text == "" && txtTelefonoFijo.Text == "" || txtTelefono.TextLength != 9 && txtTelefono.Text != "")
                {
                    MessageBox.Show("Debe ingresar un número de teléfono movil o fijo válido.", "Registro de Proveedores", MessageBoxButtons.OK);
                }
                else
                {
                    try
                    {
                        DialogResult boton = MessageBox.Show("¿Realmente desea editar a este proveedor?.", "Editar Proveedores", MessageBoxButtons.OKCancel);
                        if (boton == DialogResult.OK)
                        {
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("EditarProvveedor", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@odproveedor", idproveedorseleccionado);
                            cmd.Parameters.AddWithValue("@idTipoProveedor", cboTipoProveedor.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@nombreProveedor", txtNombreProveddor.Text);
                            cmd.Parameters.AddWithValue("@primerNombre", txtPrimerNombre.Text);
                            cmd.Parameters.AddWithValue("@segundoNombre", txtSegundoNombre.Text);
                            cmd.Parameters.AddWithValue("@apellidoPaterno", txtApellidoPaterno.Text);
                            cmd.Parameters.AddWithValue("@apellidoMaterno", txtApellidoMaterno.Text);

                            if (txtTelefono.Text == "")
                            {
                                cmd.Parameters.AddWithValue("@telefono", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@telefono", Convert.ToInt32(txtTelefono.Text));
                            }

                            cmd.Parameters.AddWithValue("@correo", txtCorreo.Text);
                            cmd.Parameters.AddWithValue("@paginaWeb", txtPaginaWEB.Text);
                            cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text);

                            cmd.Parameters.AddWithValue("@codigoPais", cboPais.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@codigoDepartamento", cboDepartamento.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@codigoProvincia", cboProvincia.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@codigoDistrito", cboDistrito.SelectedValue.ToString());

                            cmd.Parameters.AddWithValue("@idProcedencia", cboProcedencia.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@idTipoDocumento", cboTipoDocumento.SelectedValue.ToString());

                            //------------------------------
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

                            //------------------------------
                            if (ckDetraccion.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@detraccion", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@detraccion", 0);
                            }
                            if (ckDeclarante.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@declarante", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@declarante", 0);
                            }
                            if (ckPercepcion.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@percepcion", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@percepcion", 0);
                            }
                            if (ckRetencion.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@retencion", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@retencion", 0);
                            }

                            cmd.Parameters.AddWithValue("@lsoles", Convert.ToDecimal(txtSoles.Text));
                            cmd.Parameters.AddWithValue("@ldolares", Convert.ToDecimal(txtDolares.Text));
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Proveedor editado exitosamente.", "Registro de Proveedores", MessageBoxButtons.OK);
                            panelAgregarProveedor.Visible = false;
                            Mostrar();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //PANELES Y VENTANAS ANEXAS AL PROVEEDOR----------------------------------------------------
        //----------------------------------------------------------------------------------------
        //CARGA DE LISTADO DE CAMPOS CARGADOS AL PROVEEDOR---------------------------------------
        public void MostrarCuentaProducto(int idproveedor)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarProveedorCuentaProducto", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idProveedor", idproveedor);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoCuentaProducto.DataSource = dt;
            con.Close();
            datalistadoCuentaProducto.Columns[0].Width = 90;
            datalistadoCuentaProducto.Columns[1].Width = 220;
            datalistadoCuentaProducto.Columns[2].Width = 260;
            datalistadoCuentaProducto.Columns[3].Width = 380;
        }

        public void MostrarContacto(int idproveedor)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarProveedorContacto", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idproveedor", idproveedor);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoContacto.DataSource = dt;
            con.Close();
            datalistadoContacto.Columns[0].Width = 90;
            datalistadoContacto.Columns[1].Width = 210;
            datalistadoContacto.Columns[2].Width = 210;
            datalistadoContacto.Columns[3].Width = 210;
            datalistadoContacto.Columns[4].Width = 210;
        }

        public void MostrarCuentaBancaria(int idproveedor)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarProveedorCuentaBancaria", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idproveedor", idproveedor);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoCuentaBancarias.DataSource = dt;
            con.Close();
            datalistadoCuentaBancarias.Columns[0].Width = 90;
            datalistadoCuentaBancarias.Columns[1].Width = 160;
            datalistadoCuentaBancarias.Columns[2].Width = 160;
            datalistadoCuentaBancarias.Columns[3].Width = 150;
            datalistadoCuentaBancarias.Columns[4].Width = 130;
            datalistadoCuentaBancarias.Columns[5].Width = 130;
            datalistadoCuentaBancarias.Columns[6].Width = 130;
        }

        public void MostrarSucursal(int idproveedor)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarProveedorSucursal", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idproveedor", idproveedor);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoSucursal.DataSource = dt;
            con.Close();
            datalistadoSucursal.Columns[0].Width = 90;
            datalistadoSucursal.Columns[1].Width = 420;
            datalistadoSucursal.Columns[2].Width = 420;
        }

        //CARGA DE COMBOS GENERAL------------------------------------------------------------------
        //CUENTA DE PRODUCTO
        public void CargarCuenta()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdTipoMercaderias, Desciripcion FROM TIPOMERCADERIAS WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboCuentaProducctoCuenta.DisplayMember = "Desciripcion";
            cboCuentaProducctoCuenta.ValueMember = "IdTipoMercaderias";
            cboCuentaProducctoCuenta.DataSource = dt;
        }

        //CARGA DE LÍNEAS
        public void CargarLinea(int valor)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdLinea, Descripcion FROM LINEAS WHERE Estado = 1 AND IdTipMer = @id", con);
            comando.Parameters.AddWithValue("@id", valor);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboCuentaProductoLinea.DisplayMember = "Descripcion";
            cboCuentaProductoLinea.ValueMember = "IdLinea";
            cboCuentaProductoLinea.DataSource = dt;
        }

        //CARGA DE MODELOS
        public void CargarModelo(int valor)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdModelo, Descripcion FROM MODELOS WHERE Estado = 1 AND IdLinea = @id", con);
            comando.Parameters.AddWithValue("@id", valor);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboCuentaProductoModelo.DisplayMember = "Descripcion";
            cboCuentaProductoModelo.ValueMember = "IdModelo";
            cboCuentaProductoModelo.DataSource = dt;
        }

        //CUENTAS BANCARIAS
        public void CargarBanco()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdBanco, Descripcion FROM Bancos WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboBancoCuentasBancarias.DisplayMember = "Descripcion";
            cboBancoCuentasBancarias.ValueMember = "IdBanco";
            cboBancoCuentasBancarias.DataSource = dt;
        }

        //CARGA DE MONEDAS
        public void CargarMoneda()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdTipoMonedas, Descripcion FROM TipoMonedas WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboMonedaCuentasBancarias.DisplayMember = "Descripcion";
            cboMonedaCuentasBancarias.ValueMember = "IdTipoMonedas";
            cboMonedaCuentasBancarias.DataSource = dt;
        }

        //ACCIONES DE CUENTA PRODUCTO------------------------------------------------------------------------
        private void lblCuentaProducto_Click(object sender, EventArgs e)
        {
            MostrarCuentaProducto(idproveedorseleccionado);
            CargarCuenta();
            txtCodigoClienteCuentaProducto.Text = txtCodigoProveedores.Text;

            if (txtNombreProveddor.Text == "")
            {
                txtNombreProveedorCuentaProducto.Text = txtPrimerNombre.Text + " " + txtSegundoNombre.Text + " " + txtApellidoPaterno.Text + " " + txtApellidoMaterno.Text;
            }
            else
            {
                txtNombreProveedorCuentaProducto.Text = txtNombreProveddor.Text;
            }

            panelCuentaProducto.Visible = true;
            PanelCOntacto.Visible = false;
            panelCuentasBancarias.Visible = false;
            panelSucursal.Visible = false;
        }

        //CARGAR MIS CUENTAS ARA MIS PROVEEDORES
        private void cboCuentaProducctoCuenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarLinea(Convert.ToInt32(cboCuentaProducctoCuenta.SelectedValue.ToString()));
        }

        //CARGAR MIS LÍONEAS PARA MIS PROVBEEDORES
        private void cboCuentaProductoLinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarModelo(Convert.ToInt32(cboCuentaProductoLinea.SelectedValue.ToString()));
        }

        //SELECCIONAR EL ID DE MI REGISTRO
        private void datalistadoCuentaProducto_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lblCodigoCuentaProducto.Text = datalistadoCuentaProducto.SelectedCells[0].Value.ToString();
        }

        //GUARDAR UNA CUENTA PARA MI PROVEEDOR
        private void btnGuardarCuentaProducto_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea ingresar una cuenta para el proveedor?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                try
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("InsertarProveedor_CuentasProducto", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@idProveedor", idproveedorseleccionado);
                    cmd.Parameters.AddWithValue("@idCuenta", cboCuentaProducctoCuenta.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@idLinea", cboCuentaProductoLinea.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@idMdelo", cboCuentaProductoModelo.SelectedValue.ToString());
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MostrarCuentaProducto(idproveedorseleccionado);
                    MessageBox.Show("Registro ingresado exitosamente.", "Nueva cuenta del producto", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //ELIMINAR UNA CUENTA DE PRODUCTOS
        private void btnEliminarCuentaProducto_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea eliminar?.", "Eliminar Cuenta del Producto", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (lblCodigoCuentaProducto.Text != "0")
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("EliminarProveedor_CuentasProducto", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt32(lblCodigoCuentaProducto.Text));
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Eliminación correcta, operación hecha satisfactoriamente.", "Eliminación de una Cuenta del Producto", MessageBoxButtons.OK);
                        lblCodigoCuentaProducto.Text = "0";

                        MostrarCuentaProducto(idproveedorseleccionado);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un registro para poder eliminarlo.", "Eliminación de una Cuenta del Producto", MessageBoxButtons.OK);
                }
            }
        }

        //FUNCION PARA SALIR DE MI PANEL
        private void btnRegresarCuentaProducto_Click(object sender, EventArgs e)
        {
            panelCuentaProducto.Visible = false;
        }

        //ACCIONES DE CONTACTO------------------------------------------------------------------------
        private void lblContacto_Click(object sender, EventArgs e)
        {
            MostrarContacto(idproveedorseleccionado);
            txtCodigoCLienteContacto.Text = txtCodigoProveedores.Text;

            if (txtNombreProveddor.Text == "")
            {
                txtNombreProveedorContacto.Text = txtPrimerNombre.Text + " " + txtSegundoNombre.Text + " " + txtApellidoPaterno.Text + " " + txtApellidoMaterno.Text;
            }
            else
            {
                txtNombreProveedorContacto.Text = txtNombreProveddor.Text;
            }

            panelCuentaProducto.Visible = false;
            PanelCOntacto.Visible = true;
            panelCuentasBancarias.Visible = false;
            panelSucursal.Visible = false;
        }

        //SELECCIONAR EL ID DE MI REGISTRO
        private void datalistadoContacto_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lblCodigoContacto.Text = datalistadoContacto.SelectedCells[0].Value.ToString();
        }

        //GUARDAR UNA CONTACTO PARA MI PROVEEDOR
        private void btnGuardarContacto_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea ingresar este contacto?.", "Registrar un nuevo contacto", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (txtNombreContacto.Text == "" || txtTelefonoContacto.Text == "" || txtCorreoContacto.Text == "" || txtDireccionContacto.Text == "")
                {
                    MessageBox.Show("Debe ingresar los datos correspondientes.", "Registrar un nuevo contacto", MessageBoxButtons.OK);
                }
                else
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("InsertarProveedor_Contacto", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idProveedor", idproveedorseleccionado);
                        cmd.Parameters.AddWithValue("@nombre", txtNombreContacto.Text);
                        cmd.Parameters.AddWithValue("@telefono", txtTelefonoContacto.Text);
                        cmd.Parameters.AddWithValue("@direccion", txtDireccionContacto.Text);
                        cmd.Parameters.AddWithValue("@correo", txtCorreoContacto.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MostrarContacto(idproveedorseleccionado);
                        MessageBox.Show("Registro ingresado exitosamente", "Nuevo Contacto", MessageBoxButtons.OK);

                        txtNombreContacto.Text = "";
                        txtTelefonoContacto.Text = "";
                        txtDireccionContacto.Text = "";
                        txtCorreoContacto.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //ELIMINAR CONTACTO DE PROVEEDORES
        private void btnEliminarContacto_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea eliminar este contacto?.", "Eliminar Contacto", MessageBoxButtons.OKCancel);
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
                        cmd = new SqlCommand("EliminarProveedor_Contacto", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt32(lblCodigoContacto.Text));
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MostrarContacto(idproveedorseleccionado);
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
                    MessageBox.Show("Debe seleccionar un registo para poder eliminarlo.", "Eliminación de un Contacto", MessageBoxButtons.OK);
                }
            }
        }

        //FUNCION PARA SALIR DE MI PANEL
        private void btnRegresarContacto_Click(object sender, EventArgs e)
        {
            PanelCOntacto.Visible = false;
            txtNombreContacto.Text = "";
            txtTelefonoContacto.Text = "";
            txtCorreoContacto.Text = "";
            txtDireccionContacto.Text = "";
        }

        //ACCIONES CUENTA BACARIAS------------------------------------------------------------------------
        private void lblCuentasBancarias_Click(object sender, EventArgs e)
        {
            MostrarCuentaBancaria(idproveedorseleccionado);
            txtCodigoProveedorCuetnasBancarias.Text = txtCodigoProveedores.Text;
            CargarBanco();
            CargarMoneda();

            if (txtNombreProveddor.Text == "")
            {
                txtNombreProveedorCuentaBancarias.Text = txtPrimerNombre.Text + " " + txtSegundoNombre.Text + " " + txtApellidoPaterno.Text + " " + txtApellidoMaterno.Text;
            }
            else
            {
                txtNombreProveedorCuentaBancarias.Text = txtNombreProveddor.Text;
            }

            panelCuentaProducto.Visible = false;
            PanelCOntacto.Visible = false;
            panelCuentasBancarias.Visible = true;
            panelSucursal.Visible = false;
        }

        //SELECCIONAR EL ID DE MI REGISTRO
        private void datalistadoCuentaBancarias_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lblCodigoCuentaBancaria.Text = datalistadoCuentaBancarias.SelectedCells[0].Value.ToString();
        }

        //GUARDAR UNA CUENTA BANCARIA
        private void btnGuardarCuetnasBancarias_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea ingresar esta cuenta bancaria?.", "Regístro cuenta bancaria", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (txtTipoBanco.Text == "" || txtDireccionCuentasBancarias.Text == "" || txtNumeroCuentaCUentasBancarias.Text == "" || txtCCICuentasBancarias.Text == "")
                {
                    MessageBox.Show("Debe ingresar los datos correspondientes.", "Regístro", MessageBoxButtons.OK);
                }
                else
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("InsertarProveedor_CuentasBancarias", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idproveedor", idproveedorseleccionado);
                        cmd.Parameters.AddWithValue("@tipoBnco", txtTipoBanco.Text);
                        cmd.Parameters.AddWithValue("@idbanco", cboBancoCuentasBancarias.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@idmoneda", cboMonedaCuentasBancarias.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@direccion", txtDireccionCuentasBancarias.Text);
                        cmd.Parameters.AddWithValue("@numerocuentabancaria", txtNumeroCuentaCUentasBancarias.Text);
                        cmd.Parameters.AddWithValue("@cci", txtCCICuentasBancarias.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MostrarCuentaBancaria(idproveedorseleccionado);
                        MessageBox.Show("Registro ingresado exitosamente.", "Nueva cuenta bancaria", MessageBoxButtons.OK);

                        txtDireccionCuentasBancarias.Text = "";
                        txtNumeroCuentaCUentasBancarias.Text = "";
                        txtCCICuentasBancarias.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //ELIMINAR CUENTA BANCARIA DE PROVEEDORES
        private void btnEliminarCuentasBancarias_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea eliminar?.", "Eliminar Cuenta Bancaria", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (lblCodigoCuentaBancaria.Text != "0")
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("EliminarProveedor_CuentasBancarias", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt32(lblCodigoCuentaBancaria.Text));
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MostrarCuentaBancaria(idproveedorseleccionado);
                        MessageBox.Show("Eliminación correcta, operación hecha satisfactoriamente.", "Eliminación Cuenta bancararia", MessageBoxButtons.OK);
                        lblCodigoCuentaBancaria.Text = "0";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Debe Seleccionar un registor para poder eliminarlo.", "Eliminación de una cuenta bamcaria", MessageBoxButtons.OK);
                }
            }
        }

        //FUNCION PARA SALIR DE MI PANEL
        private void btnRegresarCuentasBancarias_Click(object sender, EventArgs e)
        {
            panelCuentasBancarias.Visible = false;
            txtDireccionCuentasBancarias.Text = "";
            txtNumeroCuentaCUentasBancarias.Text = "";
            txtCCICuentasBancarias.Text = "";
        }

        //ACCIONES SUCURSAL------------------------------------------------------------------------
        private void lblSucursal_Click(object sender, EventArgs e)
        {
            MostrarSucursal(idproveedorseleccionado);
            txtCodigoProveedorSucursal.Text = txtCodigoProveedores.Text;
            cboLugarEntrega.SelectedIndex = 0;

            if (txtNombreProveddor.Text == "")
            {
                txtNombreProveedorSucursal.Text = txtPrimerNombre.Text + " " + txtSegundoNombre.Text + " " + txtApellidoPaterno.Text + " " + txtApellidoMaterno.Text;
            }
            else
            {
                txtNombreProveedorSucursal.Text = txtNombreProveddor.Text;
            }

            panelCuentaProducto.Visible = false;
            PanelCOntacto.Visible = false;
            panelCuentasBancarias.Visible = false;
            panelSucursal.Visible = true;
        }

        //SELECCIONAR EL ID DE MI REGISTRO
        private void datalistadoSucursal_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lblCodigoSucursal.Text = datalistadoSucursal.SelectedCells[0].Value.ToString();
        }

        //GUARDAR UNA SUCURSAL
        private void btnGuardarSucursal_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea ingresar una sucursal?.", "Registro de Sucursal", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (txtNombreSucursal.Text == "")
                {
                    MessageBox.Show("Debe ingresar datos válido para poder hacer el registro.", "Registro de Sucursal", MessageBoxButtons.OK);
                }
                else
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("InsertarProveedor_Sucursal", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idproveedor", idproveedorseleccionado);
                        cmd.Parameters.AddWithValue("@nomrbre", txtNombreSucursal.Text);
                        cmd.Parameters.AddWithValue("@lugarEntrega", cboLugarEntrega.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MostrarSucursal(idproveedorseleccionado);
                        MessageBox.Show("Registro ingresado exitosamente.", "Nueva Sucursal", MessageBoxButtons.OK);

                        txtNombreSucursal.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //ELIMINAR SUCURSAL
        private void btnEliminarSucursal_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea eliminar?", "Eliminar Sucursal", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (lblCodigoSucursal.Text != "0")
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("EliminarProveedor_Sucursal", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(lblCodigoSucursal.Text));
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MostrarSucursal(idproveedorseleccionado);
                    MessageBox.Show("Eliminacion correcta, operacion hecha satisfactoriamente.", "Eliminacion nueva", MessageBoxButtons.OK);
                    lblCodigoSucursal.Text = "0";
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un registo para poder eliminarlo.", "Eliminación de una Sucursal", MessageBoxButtons.OK);
                }
            }
        }

        //FUNCION PARA SALIR DE MI PANEL
        private void btnRegresarSucursal_Click(object sender, EventArgs e)
        {
            panelSucursal.Visible = false;
            txtNombreSucursal.Text = "";
        }

        //BUSQEUDAS DE PROVEEDORES Y VALIDACIONES -------------------------------------------
        private void cboTipoBusqueda_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtProveedor.Text = "";
        }

        //FUNCION PARA BUSCAR TODOS MIS PROVEEDORES
        private void txtProveedor_TextChanged(object sender, EventArgs e)
        {
            if (cboTipoBusqueda.Text == "NOMBRES")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarProveedorPorNombre", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre", txtProveedor.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistado.DataSource = dt;
                con.Close();

            }
            else if (cboTipoBusqueda.Text == "DOCUMENTO")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarProveedorPorDocumento", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@numero", txtProveedor.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistado.DataSource = dt;
                con.Close();
            }
        }

        //VALIDAR EL INGHRESO DE SOLO NÚMEROS
        private void txtCCICuentasBancarias_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //VALIDAR EL INGHRESO DE SOLO NÚMEROS
        private void txtNumeroCuentaCUentasBancarias_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //VALIDAR EL INGHRESO DE SOLO NÚMEROS
        private void txtTelefonoContacto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //VALIDAR EL INGHRESO DE SOLO NÚMEROS
        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //VALIDAR EL INGHRESO DE SOLO NÚMEROS
        private void txtSoles_KeyPress(object sender, KeyPressEventArgs e)
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

        //VALIDAR EL INGHRESO DE SOLO NÚMEROS
        private void txtDolares_KeyPress(object sender, KeyPressEventArgs e)
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

        //VALIDAREL INGRESO DE SOLO NÚMEROS
        private void txtDni_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //VALIDAREL INGRESO DE SOLO NÚMEROS
        private void txtRuc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //VALIDAREL INGRESO DE SOLO NÚMEROS
        private void txtOtroDocumento_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //CAMBIAR EL ESTADO DE MI DETRACCION
        private void ckDetraccion_CheckedChanged(object sender, EventArgs e)
        {
            if (ckDetraccion.Checked == true)
            {
                lblLeyendaDetraccion.Text = "SI";
            }
            else
            {
                lblLeyendaDetraccion.Text = "NO";
            }
        }

        //CAMBIAR EL ESTADO DE MI DECLARANTE
        private void ckDeclarante_CheckedChanged(object sender, EventArgs e)
        {
            if (ckDeclarante.Checked == true)
            {
                lblLeyendaDeclarante.Text = "SI";
            }
            else
            {
                lblLeyendaDeclarante.Text = "NO";
            }
        }

        //CAMBIAR EL ESTADO DE MI PERCEPCION
        private void ckPercepcion_CheckedChanged(object sender, EventArgs e)
        {
            if (ckPercepcion.Checked == true)
            {
                lblLeyendaPercepcion.Text = "SI";
            }
            else
            {
                lblLeyendaPercepcion.Text = "NO";
            }
        }

        //CAMBIAR EL ESTADO DE MI RETENCION
        private void ckRetencion_CheckedChanged(object sender, EventArgs e)
        {
            if (ckRetencion.Checked == true)
            {
                lblLeyendaRetencion.Text = "SI";
            }
            else
            {
                lblLeyendaRetencion.Text = "NO";
            }
        }
    }
}

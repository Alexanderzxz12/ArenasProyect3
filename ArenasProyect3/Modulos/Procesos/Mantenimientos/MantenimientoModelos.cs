using DocumentFormat.OpenXml.Office2013.Drawing.Chart;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Procesos.Mantenimientos
{
    public partial class MantenimientoModelos : Form
    {
        //VARIABLES DE VALIDACIÓN PARA EL INGRESO Y EDICIÓN DE DATOS
        bool repetidoDescripcion;
        bool repetidoAbreviatura;
        bool habilitarValidaciones;

        //CONSTRUCTOR DEL MANTENIMIENTO - MANTENIEMINTO DE LINEAS
        public MantenimientoModelos()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI MANTENIMIENTOS DE MODELOS
        private void MantenimientoModelos_Load(object sender, EventArgs e)
        {
            CargarTipoLinea();
            ColorDescripcion();
            alternarColorFilas(datalistadoLineas);

            cboBusquedaModelo.SelectedIndex = 1;
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

        //CARGA DE DATOS - TIPO DE LINEA
        public void CargarTipoLinea()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdLinea,L.Descripcion, IdTipMer, TM.Desciripcion FROM LINEAS L INNER JOIN TIPOMERCADERIAS TM ON TM.IdTipoMercaderias = L.IdTipMer WHERE L.Estado = 1 ORDER BY L.Descripcion", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboTipoLinea.DisplayMember = "Descripcion";
                cboTipoLinea.ValueMember = "IdLinea";
                DataRow row = dt.Rows[0];
                txtCodigoLinea.Text = System.Convert.ToString(row["Desciripcion"]);
                cboTipoLinea.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //EVENTO DE CAMBIO DE DATO EN EL COMBO DE MIS LINEAS
        private void cboTipoLinea_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdLinea,Descripcion, IdTipMer, TM.Desciripcion FROM LINEAS L INNER JOIN TIPOMERCADERIAS TM ON TM.IdTipoMercaderias = L.IdTipMer WHERE L.Estado = 1 AND IdLinea = @idlinea ORDER BY L.Descripcion", con);
                comando.Parameters.AddWithValue("@idlinea", System.Convert.ToString(cboTipoLinea.SelectedValue));
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtCodigoLinea.Text = System.Convert.ToString(row["Desciripcion"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //BÚSQUEDA DE LINEAS SEGÚN EL TIPO DE MERCADERIA SELECIONARA - EVENTO SELECCIÓN
        private void cboTipoLinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mostrar(Convert.ToInt32(cboTipoLinea.SelectedValue));
        }

        //MOSTRAR TODAS MIS LÍNEAS SUGUN EL TIPO DE CUENTA SELECCIOANDO - METODO
        public void Mostrar(int idlinea)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Modelos_MostrarSegunLinea", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@linea", idlinea);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoLineas.DataSource = dt;
                con.Close();
                OrdenarColumnasModelo(datalistadoLineas);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //EVENTO DE DOBLE CLICK PARA EN MI LISTADO DE LINEAS
        private void datalistadoLineas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoLineas.RowCount != 0)
            {
                lblCodigo.Text = datalistadoLineas.SelectedCells[1].Value.ToString();
                txtAbreviatura.Text = datalistadoLineas.SelectedCells[2].Value.ToString();
                txtDescripcion.Text = datalistadoLineas.SelectedCells[3].Value.ToString();
                cboTipoLinea.SelectedValue = datalistadoLineas.SelectedCells[4].Value.ToString();

                string estadoAtirubutos = datalistadoLineas.SelectedCells[6].Value.ToString();
                if (estadoAtirubutos == "NO DEFINIDO")
                {
                    cboEstadoAtributo.Text = "MODELO POR DEFINIR";
                }
                else
                {
                    cboEstadoAtributo.Text = "MODELO YA DEFINIDO";
                }

                string estado = datalistadoLineas.SelectedCells[0].Value.ToString();
                if (estado == "ACTIVO")
                {
                    cboEstado.Text = "ACTIVO";
                }
                else
                {
                    cboEstado.Text = "INACTIVO";
                }

                txtDescripcion.Enabled = false;
                txtAbreviatura.Enabled = false;

                btnEditar.Visible = true;
                btnEditar2.Visible = false;

                btnGuardar.Visible = true;
                btnGuardar2.Visible = false;

                Cancelar.Visible = false;
                lblCancelar.Visible = false;
            }   
        }

        //VALIDACIÓN EL SISTEMA PARA PODER AVERIGUAR SI YA EXISTE OTRO REGISTRO CON LA MISMA DESCRIPCION
        public void ColorDescripcion()
        {
            foreach (DataGridViewRow datorecuperado in datalistadoLineas.Rows)
            {
                string valor = Convert.ToString(datorecuperado.Cells["NOMBRE"].Value);
                valor = valor.Trim();

                if (valor == txtDescripcion.Text)
                {
                    txtDescripcion.ForeColor = Color.Red;
                    repetidoDescripcion = true;
                    return;
                }
                else
                {
                    txtDescripcion.ForeColor = Color.Green;
                    repetidoDescripcion = false;
                }
            }
            txtDescripcion.ForeColor = Color.Green;
            repetidoDescripcion = false;
        }

        //HABILITAR GUARDADO DE UNA NUEVA MDOELO
        public void ColorAbreviatura()
        {
            foreach (DataGridViewRow datorecuperado in datalistadoLineas.Rows)
            {
                string valor = Convert.ToString(datorecuperado.Cells["ABREVIATURA"].Value);
                valor = valor.Trim();

                if (valor == txtAbreviatura.Text)
                {
                    txtAbreviatura.ForeColor = Color.Red;
                    repetidoAbreviatura = true;
                    return;
                }
                else
                {
                    txtAbreviatura.ForeColor = Color.Green;
                    repetidoAbreviatura = false;
                }
            }
        }

        //TRAER EL ÚLTIMO REGISTRO INGRESADO PARA GENERARLE LOS ATRIBUTOS AL MODELO
        public void CargarModeloRecienIngresado()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT IdModelo FROM MODELOS WHERE Estado = 1 AND IdModelo = (SELECT MAX(IdModelo) FROM MODELOS)", con);
                da.Fill(dt);
                datalistadoModeloRecienIngresado.DataSource = dt;
                con.Close();

                lblCodigo.Text = datalistadoModeloRecienIngresado.SelectedCells[0].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //ACCIONES Y FUNCONES DEL SISITEMA------------------------------------------------------
        //HABILITAR EL INGRESO DE DATOS
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            txtDescripcion.Enabled = true;
            txtAbreviatura.Enabled = true;

            btnGuardar.Visible = false;
            btnGuardar2.Visible = true;

            lblCancelar.Visible = true;
            Cancelar.Visible = true;
            btnEditar.Enabled = true;

            cboEstado.Text = "ACTIVO";
            txtDescripcion.Text = "";
            txtAbreviatura.Text = "";

            lblCodigo.Text = "N";

            cboEstadoAtributo.Text = "MODELO NO DEFINIDO";
        }

        //METODO ENCARGADO DE AGREGAR UN NUEVO MODELO A MI BASE DE DATOS
        public void AgregarModelos(string descripcion, string abreavitura, int codigolinea)
        {
            if (repetidoDescripcion == true)
            {
                MessageBox.Show("No se puede ingresar dos registros iguales.", "Validación del Sistema", MessageBoxButtons.OK);
                txtDescripcion.Focus();
            }
            else
            {
                if (txtDescripcion.Text == "" || txtAbreviatura.Text == "" ||
                    ckCaracteristicas1.Checked == false && ckCaracteristicas2.Checked == false && ckCamposMedida1.Checked == false && ckCamposMedida2.Checked == false
                && ckCamposDiametros1.Checked == false && ckCamposDiametros2.Checked == false && ckCamposFormas1.Checked == false && ckCamposFormas2.Checked == false
                && ckCamposEspesores1.Checked == false && ckCamposEspesores2.Checked == false && ckCamposDiseñoAcabado1.Checked == false && ckCamposDiseñoAcabado2.Checked == false
                && ckCamposNTipos1.Checked == false && ckCamposNTipos2.Checked == false && ckVariosO1.Checked == false && ckVariosO2.Checked == false && ckGenerales.Checked == false)
                {
                    MessageBox.Show("Debe ingresar todos los campos necesarios incluyendo los atributos del modelo para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
                    txtDescripcion.Focus();
                }
                else
                {
                    DialogResult boton = MessageBox.Show("¿Esta seguro que desea guardar este modelo?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                    if (boton == DialogResult.OK)
                    {
                        try
                        {
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("Modelos_Insertar", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@descripcion", descripcion);
                            cmd.Parameters.AddWithValue("@abreviatura", abreavitura);
                            cmd.Parameters.AddWithValue("@codigolinea", codigolinea);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            Mostrar(codigolinea);

                            MessageBox.Show("Se ingresó el nuevo registro correctamente.", "Registro Nuevo", MessageBoxButtons.OK);
                            ColorDescripcion();

                            txtDescripcion.Enabled = false;
                            txtAbreviatura.Enabled = false;

                            btnEditar.Visible = true;
                            btnEditar2.Visible = false;

                            btnGuardar.Visible = true;
                            btnGuardar2.Visible = false;

                            cboEstado.SelectedIndex = -1;
                            Cancelar.Visible = false;
                            lblCancelar.Visible = false;

                            cboEstadoAtributo.Text = "***";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        CargarModeloRecienIngresado();

                        try
                        {
                            //INGRESAMOS LOS CAMPOS GENERALES
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("Modelos_InsertarAtributosXModelo", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idmodelo", lblCodigo.Text);

                            cmd.Parameters.AddWithValue("@campcaracteristicas1", ckCaracteristicas1.Checked ? 1 : 0);
                            cmd.Parameters.AddWithValue("@campcaracteristicas2", ckCaracteristicas2.Checked ? 1 : 0);

                            cmd.Parameters.AddWithValue("@campmedidas1", ckCamposMedida1.Checked ? 1 : 0);
                            cmd.Parameters.AddWithValue("@campmedidas2", ckCamposMedida2.Checked ? 1 : 0);

                            cmd.Parameters.AddWithValue("@campdiametro1", ckCamposDiametros1.Checked ? 1 : 0);
                            cmd.Parameters.AddWithValue("@campdiametro2", ckCamposDiametros2.Checked ? 1 : 0);

                            cmd.Parameters.AddWithValue("@campformas1", ckCamposFormas1.Checked ? 1 : 0);
                            cmd.Parameters.AddWithValue("@campformas2", ckCamposFormas2.Checked ? 1 : 0);

                            cmd.Parameters.AddWithValue("@campespesores1", ckCamposEspesores1.Checked ? 1 : 0);
                            cmd.Parameters.AddWithValue("@campespesores2", ckCamposEspesores2.Checked ? 1 : 0);

                            cmd.Parameters.AddWithValue("@campdiseñoacabados1", ckCamposDiseñoAcabado1.Checked ? 1 : 0);
                            cmd.Parameters.AddWithValue("@campdiseñoacabados2", ckCamposDiseñoAcabado2.Checked ? 1 : 0);
  
                            cmd.Parameters.AddWithValue("@campntipos1", ckCamposNTipos1.Checked ? 1 : 0);
                            cmd.Parameters.AddWithValue("@campntipos2", ckCamposNTipos2.Checked ? 1 : 0);

                            cmd.Parameters.AddWithValue("@campvarios1", ckVariosO1.Checked ? 1 : 0);
                            cmd.Parameters.AddWithValue("@campvarios2", ckVariosO2.Checked ? 1 : 0);

                            cmd.Parameters.AddWithValue("@campgenerales", ckGenerales.Checked ? 1 : 0);

                            cmd.ExecuteNonQuery();
                            con.Close();

                            //INGRESAMOS DETALLES
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("Modelos_InsertarAtributosXModeloDetalle", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idmodelo", lblCodigo.Text);

                            cmd.Parameters.AddWithValue("@idtipomercaderia1", (object)cboTipoCaracteristicas1.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipomercaderia2", (object)cboTipoCaracteristicas2.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipomercaderia3", (object)cboTipoCaracteristicas3.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipomercaderia4", (object)cboTipoCaracteristicas4.SelectedValue ?? DBNull.Value);
                            
                            cmd.Parameters.AddWithValue("@idtipomedida1", (object)cboTipoMedida1.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipomedida2", (object)cboTipoMedida2.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipomedida3", (object)cboTipoMedida3.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipomedida4", (object)cboTipoMedida4.SelectedValue ?? DBNull.Value);
                            
                            cmd.Parameters.AddWithValue("@idtipodiametro1", (object)cboTiposDiametros1.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipodiametro2", (object)cboTiposDiametros2.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipodiametro3", (object)cboTiposDiametros3.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipodiametro4", (object)cboTiposDiametros4.SelectedValue ?? DBNull.Value);
                           
                            cmd.Parameters.AddWithValue("@idtipoformas1", (object)cboTiposFormas1.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipoformas2", (object)cboTiposFormas2.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipoformas3", (object)cboTiposFormas3.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipoformas4", (object)cboTiposFormas4.SelectedValue ?? DBNull.Value);
                            
                            cmd.Parameters.AddWithValue("@idtipoespesores1", (object)cbooTipoEspesores1.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipoespesores2", (object)cbooTipoEspesores2.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipoespesores3", (object)cbooTipoEspesores3.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipoespesores4", (object)cbooTipoEspesores4.SelectedValue ?? DBNull.Value);
                            
                            cmd.Parameters.AddWithValue("@idtipodiametroacabados1", (object)cboTiposDiseñosAcabados1.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipodiametroacabados2", (object)cboTiposDiseñosAcabados2.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipodiametroacabados3", (object)cboTiposDiseñosAcabados3.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipodiametroacabados4", (object)cboTiposDiseñosAcabados4.SelectedValue ?? DBNull.Value);
                            
                            cmd.Parameters.AddWithValue("@idtipontipos1", (object)cboTiposNTipos1.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipontipos2", (object)cboTiposNTipos2.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipontipos3", (object)cboTiposNTipos3.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtipontipos4", (object)cboTiposNTipos4.SelectedValue ?? DBNull.Value);
                            
                            cmd.Parameters.AddWithValue("@idtpovarios1", (object)cboTiposVariosO1.SelectedValue ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@idtpovarios2", (object)cboTiposVariosO2.SelectedValue ?? DBNull.Value);
                           
                            cmd.Parameters.AddWithValue("@campogeneral", DBNull.Value);

                            cmd.ExecuteNonQuery();
                            con.Close();

                            Mostrar(codigolinea);
                            cboEstadoAtributo.SelectedText = "MODELO YA DEFINIDO";

                            MessageBox.Show("Se ingresó el nuevo registro correctamente.", "Registro Nuevo", MessageBoxButtons.OK);
                            Limpiar();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        //GUARDAR UNA NUEVO MDOELO EN MI BASE DE DATOS CON SUS ATRIBUTOS Y DETALLE DE ESTOS
        private void btnGuardar2_Click(object sender, EventArgs e)
        {
            AgregarModelos(txtDescripcion.Text, txtAbreviatura.Text, Convert.ToInt32(cboTipoLinea.SelectedValue));
        }

        //HABILITAR EDICIÓN PARA MODIFICAR UNA MODELO YA INGRESADA
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (lblCodigo.Text == "N" || lblCodigo.Text == "")
            {
                MessageBox.Show("Debe seleccionar un registro para poder editar.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                txtDescripcion.Enabled = true;
                txtAbreviatura.Enabled = true;
                cboTipoLinea.Enabled = false;

                btnEditar.Visible = false;
                btnEditar2.Visible = true;

                Cancelar.Visible = true;
                lblCancelar.Visible = true;
                btnGuardar.Enabled = true;
            }
        }

        //METODO ENCARGADO DE LA EDICIÓN DE MODELOS
        public void EditarModelos(string descripcion, string abreavitura, string codigolinea, int codigo)
        {
            if (descripcion == "" || abreavitura == "" || Convert.ToString(codigo) == "N")
            {
                MessageBox.Show("Los campos no pueden estar vacios.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                DialogResult boton = MessageBox.Show("¿Esta seguro que desea editar este modelo?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Modelos_Editar", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@codigo", codigo);
                        cmd.Parameters.AddWithValue("@descripcion", descripcion);
                        cmd.Parameters.AddWithValue("@abreviatura", abreavitura);
                        cmd.Parameters.AddWithValue("@codigolinea", codigolinea);
                        cmd.Parameters.AddWithValue("@estado", cboEstado.Text == "ACTIVO" ? 1 : 0);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        int linea = Convert.ToInt32(codigolinea);
                        Mostrar(linea);

                        MessageBox.Show("Se editó correctamente el registro.", "Edición", MessageBoxButtons.OK);
                        ColorDescripcion();

                        txtDescripcion.Enabled = false;
                        txtAbreviatura.Enabled = false;

                        btnEditar.Visible = true;
                        btnEditar2.Visible = false;

                        btnGuardar.Visible = true;
                        btnGuardar2.Visible = false;

                        cboEstado.SelectedIndex = -1;
                        Cancelar.Visible = false;
                        lblCancelar.Visible = false;
                        cboTipoLinea.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //EDITAR UN MODELO DE MI BASE DE DATOS
        private void btnEditar2_Click(object sender, EventArgs e)
        {
            if (lblCodigo.Text == "N" || lblCodigo.Text == "")
            {
                MessageBox.Show("Debe seleccionar un registro para poder editar.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                EditarModelos(txtDescripcion.Text, txtAbreviatura.Text, cboTipoLinea.SelectedValue.ToString(), Convert.ToInt32(lblCodigo.Text));
            }
        }

        //CACELAR ACCIÓN DE GUARDADO O EDITADO
        private void Cancelar_Click(object sender, EventArgs e)
        {
            txtDescripcion.Enabled = false;
            txtAbreviatura.Enabled = false;

            btnEditar.Visible = true;
            btnEditar2.Visible = false;

            btnGuardar.Visible = true;
            btnGuardar2.Visible = false;

            Cancelar.Visible = false;
            lblCancelar.Visible = false;

            cboTipoLinea.Enabled = true;
            cboEstado.SelectedIndex = -1;
            txtDescripcion.Text = "";
            txtAbreviatura.Text = "";

            cboEstadoAtributo.Text = "***";
            lblCodigo.Text = "N";
        }

        //VALIDACIONES DE INGRESO DE DATOS Y EXISTENCOA DE ESTOS-----------------------------
        //VALIDAR LA DIGITACIÓN DE UN MODELO
        private void txtDescripcion_TextChanged(object sender, EventArgs e)
        {
            ColorDescripcion();
        }

        //VALIDAR LA DIGITACIÓN DE UNA ABREVIATURA
        private void txtAbreviatura_TextChanged(object sender, EventArgs e)
        {
            ColorAbreviatura();
        }

        //LLAMADO DE UN METODO PARA EXPORTAR A EXCEL EL LISTADO DE MODELOS
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            ExportarDatos(datalistadoLineas);
        }

        //METODO PARA EXPORTAR LAS CUENTAS A EXCEL
        public void ExportarDatos(DataGridView datalistado)
        {
            Microsoft.Office.Interop.Excel.Application exportarexcel = new Microsoft.Office.Interop.Excel.Application();

            exportarexcel.Application.Workbooks.Add(true);

            int indicecolumna = 0;
            foreach (DataGridViewColumn columna in datalistado.Columns)
            {
                indicecolumna++;

                exportarexcel.Cells[1, indicecolumna] = columna.Name;
            }

            int indicefila = 0;
            foreach (DataGridViewRow fila in datalistado.Rows)
            {
                indicefila++;
                indicecolumna = 0;
                foreach (DataGridViewColumn columna in datalistado.Columns)
                {
                    indicecolumna++;
                    exportarexcel.Cells[indicefila + 1, indicecolumna] = fila.Cells[columna.Name].Value;
                }
            }
            exportarexcel.Visible = true;
        }

        //CONFIJURACION DE ATRIBUTOS---------------------------------------------------------------
        private void CargarAtributos_Click(object sender, EventArgs e)
        {
            if (cboEstadoAtributo.Text == "MODELO YA DEFINIDO" || cboEstadoAtributo.Text == "***")
            {
                MessageBox.Show("Este modelo ya ha sido definido.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                if (txtCodigoLinea.Text == "PRODUCTO TERMINADO")
                {
                    ckGenerales.Visible = false;
                }
                else
                {
                    ckGenerales.Visible = true;
                }

                panelDefinicionAtributos.Visible = true;
            }
        }

        //CARGAS DE LOS COMBOS CON METODODS Y EJECUCIÓN DE ESTOS AL MOSTRARLOS---------------
        //CARGAS DE CARACTERISTUCAS-------------------------
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
                MessageBox.Show(ex.Message);
            }
        }

        //SELECCION DEL GRUPO DE CAMPOS CARACTERISTICAS 1
        private void ckCaracteristicas1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckGenerales.Checked == false)
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
                }
            }
            else
            {
                MessageBox.Show("El modelo solo puede tener dos definiciones, campo libre(general) o campos especificos.", "Validación del Sistema", MessageBoxButtons.OK);
                ckCaracteristicas1.Checked = false;
            }
        }

        //SELECCION DEL GRUPO DE CAMPOS CARACTERISTICAS 2
        private void ckCaracteristicas2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckGenerales.Checked == false)
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
                }
            }
            else
            {
                MessageBox.Show("El modelo solo puede tener dos definiciones, campo libre(general) o campos especificos.", "Validación del Sistema", MessageBoxButtons.OK);
                ckCaracteristicas2.Checked = false;
            }
        }

        //CARGAS DE MEDIDAS--------------------------
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
                MessageBox.Show(ex.Message);
            }
        }

        //SELECCION DEL GRUPO DE CAMPOS MEDIDAS 1
        private void ckCamposMedida1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckGenerales.Checked == false)
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
                }
            }
            else
            {
                MessageBox.Show("El modelo solo puede tener dos definiciones, campo libre(general) o campos especificos.", "Validación del Sistema", MessageBoxButtons.OK);
                ckCamposMedida1.Checked = false;
            }
        }

        //SELECCION DEL GRUPO DE CAMPOS MEDIDAS 2
        private void ckCamposMedida2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckGenerales.Checked == false)
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
                }
            }
            else
            {
                MessageBox.Show("El modelo solo puede tener dos definiciones, campo libre(general) o campos especificos.", "Validación del Sistema", MessageBoxButtons.OK);
                ckCamposMedida2.Checked = false;
            }
        }

        //CARGAS DE DIAMETRO--------------------------
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
                MessageBox.Show(ex.Message);
            }
        }

        //SELECCION DEL GRUPO DE CAMPOS DIAMETRO 1
        private void ckCamposDiametros1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckGenerales.Checked == false)
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
                }
            }
            else
            {
                MessageBox.Show("El modelo solo puede tener dos definiciones, campo libre(general) o campos especificos.", "Validación del Sistema", MessageBoxButtons.OK);
                ckCamposDiametros1.Checked = false;
            }
        }

        //SELECCION DEL GRUPO DE CAMPOS DIAMETRO 2
        private void ckCamposDiametros2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckGenerales.Checked == false)
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
                }
            }
            else
            {
                MessageBox.Show("El modelo solo puede tener dos definiciones, campo libre(general) o campos especificos.", "Validación del Sistema", MessageBoxButtons.OK);
                ckCamposDiametros2.Checked = false;
            }
        }

        //CARGAS DE FORMAS--------------------------
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
                MessageBox.Show(ex.Message);
            }
        }

        //SELECCION DEL GRUPO DE CAMPOS FORMAS 1
        private void ckCamposFormas1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckGenerales.Checked == false)
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
                }
            }
            else
            {
                MessageBox.Show("El modelo solo puede tener dos definiciones, campo libre(general) o campos especificos.", "Validación del Sistema", MessageBoxButtons.OK);
                ckCamposFormas1.Checked = false;
            }
        }

        //SELECCION DEL GRUPO DE CAMPOS FORMAS 2
        private void ckCamposFormas2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckGenerales.Checked == false)
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
                }
            }
            else
            {
                MessageBox.Show("El modelo solo puede tener dos definiciones, campo libre(general) o campos especificos.", "Validación del Sistema", MessageBoxButtons.OK);
                ckCamposFormas2.Checked = false;
            }
        }

        //CARGAS DE ESPESORES--------------------------
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
                MessageBox.Show(ex.Message);
            }
        }

        //SELECCION DEL GRUPO DE CAMPOS ESPESORES 1
        private void ckCamposEspesores1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckGenerales.Checked == false)
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
                }
            }
            else
            {
                MessageBox.Show("El modelo solo puede tener dos definiciones, campo libre(general) o campos especificos.", "Validación del Sistema", MessageBoxButtons.OK);
                ckCamposEspesores1.Checked = false;
            }
        }

        //SELECCION DEL GRUPO DE CAMPOS ESPESORES 2
        private void ckCamposEspesores2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckGenerales.Checked == false)
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
                }
            }
            else
            {
                MessageBox.Show("El modelo solo puede tener dos definiciones, campo libre(general) o campos especificos.", "Validación del Sistema", MessageBoxButtons.OK);
                ckCamposEspesores2.Checked = false;
            }
        }

        //CARGAS DE DISEÑO Y ACABADOS--------------------------
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
                MessageBox.Show(ex.Message);
            }
        }

        //SELECCION DEL GRUPO DE CAMPOS DISEÑO ACABADO 1
        private void ckCamposDiseñoAcabado1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckGenerales.Checked == false)
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
                }
            }
            else
            {
                MessageBox.Show("El modelo solo puede tener dos definiciones, campo libre general o campos especificos.", "Validación del Sistema", MessageBoxButtons.OK);
                ckCamposDiseñoAcabado1.Checked = false;
            }
        }

        //SELECCION DEL GRUPO DE CAMPOS DISEÑO ACABADO 2
        private void ckCamposDiseñoAcabado2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckGenerales.Checked == false)
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
                }
            }
            else
            {
                MessageBox.Show("El modelo solo puede tener dos definiciones, campo libre(general) o campos especificos.", "Validación del Sistema", MessageBoxButtons.OK);
                ckCamposDiseñoAcabado2.Checked = false;
            }
        }

        //CARGAS DE TIPOS U NÚMERO DE TIPOS--------------------------
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
                MessageBox.Show(ex.Message);
            }
        }

        //SELECCION DEL GRUPO DE CAMPOS TIPOS Y NÚMERO 1
        private void ckCamposNTipos1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckGenerales.Checked == false)
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
                }
            }
            else
            {
                MessageBox.Show("El modelo solo puede tener dos definiciones, campo libre(general) o campos especificos.", "Validación del Sistema", MessageBoxButtons.OK);
                ckCamposNTipos1.Checked = false;
            }
        }

        //SELECCION DEL GRUPO DE CAMPOS TIPOS Y NÚMERO 2
        private void ckCamposNTipos2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckGenerales.Checked == false)
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
                }
            }
            else
            {
                MessageBox.Show("El modelo solo puede tener dos definiciones, campo libre(general) o campos especificos.", "Validación del Sistema", MessageBoxButtons.OK);
                ckCamposNTipos2.Checked = false;
            }
        }

        //CARGAS DE VARIOS Y 0--------------------------
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
                MessageBox.Show(ex.Message);
            }
        }

        //SELECCION DEL GRUPO DE CAMPOS VARIOS Y 0 1
        private void ckVariosO1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckGenerales.Checked == false)
            {
                if (ckVariosO1.Checked == true)
                {
                    flowLayoutPanel.Controls.Add(panelCamposVariosO1);
                    CargarTiposVariosO(cboTiposVariosO1);
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposVariosO1);
                }
            }
            else
            {
                MessageBox.Show("El modelo solo puede tener dos definiciones, campo libre(general) o campos especificos.", "Validación del Sistema", MessageBoxButtons.OK);
                ckVariosO1.Checked = false;
            }
        }

        //SELECCION DEL GRUPO DE CAMPOS VARIOS Y 0 2
        private void ckVariosO2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckGenerales.Checked == false)
            {
                if (ckVariosO2.Checked == true)
                {
                    flowLayoutPanel.Controls.Add(panelCamposVariosO2);
                    CargarTiposVariosO(cboTiposVariosO2);
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposVariosO2);
                }
            }
            else
            {
                MessageBox.Show("El modelo solo puede tener dos definiciones, campo libre(general) o campos especificos.", "Validación del Sistema", MessageBoxButtons.OK);
                ckVariosO2.Checked = false;
            }
        }

        //SELECCIONAR EL CAMPOS GENERAL
        private void ckGenerales_CheckedChanged(object sender, EventArgs e)
        {
            if (ckCaracteristicas1.Checked == false || ckCaracteristicas2.Checked == false || ckCamposMedida1.Checked == false || ckCamposMedida2.Checked == false
    || ckCamposDiametros1.Checked == false || ckCamposDiametros2.Checked == false || ckCamposFormas1.Checked == false || ckCamposFormas2.Checked == false
    || ckCamposEspesores1.Checked == false || ckCamposEspesores2.Checked == false || ckCamposDiseñoAcabado1.Checked == false || ckCamposDiseñoAcabado2.Checked == false
    || ckCamposNTipos1.Checked == false || ckCamposNTipos2.Checked == false || ckVariosO1.Checked == false || ckVariosO2.Checked == false)
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
            else
            {
                MessageBox.Show("El modelo solo puede tener dos definiciones, campo libre(general) o campos especificos.", "Validación del Sistema", MessageBoxButtons.OK);
                ckGenerales.Checked = false;
            }
        }

        //ACCION DE GAURDAR LOS ATRIBUTOS ESCOGIDOS Y DEFINIDOS DE MI MODELO
        private void btnGuardarAtributos_Click(object sender, EventArgs e)
        {
            ValidacionGruposCampos();
        }

        //LIMPIAR Y BORRAR LAS SELECCIONAR ECHAS Y LOS CAMPOS ESCOGIDOS DE LA DEFINICIÓN DE ATRIBUTOS
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        //FUNCION PARA LIMPIAR
        public void Limpiar()
        {
            ckCaracteristicas1.Checked = false;
            ckCaracteristicas2.Checked = false;
            ckCamposMedida1.Checked = false;
            ckCamposMedida2.Checked = false;
            ckCamposDiametros1.Checked = false;
            ckCamposDiametros2.Checked = false;
            ckCamposFormas1.Checked = false;
            ckCamposFormas2.Checked = false;
            ckCamposEspesores1.Checked = false;
            ckCamposEspesores2.Checked = false;
            ckCamposDiseñoAcabado1.Checked = false;
            ckCamposDiseñoAcabado2.Checked = false;
            ckCamposNTipos1.Checked = false;
            ckCamposNTipos2.Checked = false;
            ckVariosO1.Checked = false;
            ckVariosO2.Checked = false;
            ckGenerales.Checked = false;
        }

        //CERRAR Y SALIR DEL PANEL DE DEFINICIO DE MODELO
        private void btnSalir_Click(object sender, EventArgs e)
        {
            panelDefinicionAtributos.Visible = false;
        }

        //FILTRAR MDOELOS POR DIFERENTES CRITERIOS
        public void FiltrarModelos(TextBox busquedamodelo, DataGridView dgv, ComboBox cbo)
        {
            try
            {
                if (busquedamodelo.Text == "")
                {
                    Mostrar(Convert.ToInt32(cboTipoLinea.SelectedValue));
                }
                else
                {
                    if (cbo.Text == "DESCRIPCIÓN")
                    {
                        DataTable dt = new DataTable();
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Modelos_BusquedaPorDescripcion", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@descripcion", busquedamodelo.Text);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        dgv.DataSource = dt;
                        con.Close();
                        OrdenarColumnasModelo(dgv);
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Modelos_BusquedaPorAbreviatura", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@abreviatura", busquedamodelo.Text);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        dgv.DataSource = dt;
                        con.Close();
                        OrdenarColumnasModelo(dgv);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //BUSQUEDA DE MODELO------------------------------------------------------------
        private void txtBusquedaModelo_TextChanged(object sender, EventArgs e)
        {
            FiltrarModelos(txtBusquedaModelo, datalistadoLineas, cboBusquedaModelo);
        }

        //FUNCION PARA ORDENAR MIS COLUMNAS DE MI BUSQUEDAS
        public void OrdenarColumnasModelo(DataGridView DGV)
        {
            DGV.Columns[0].Width = 75;
            DGV.Columns[1].Width = 75;
            DGV.Columns[2].Width = 100;
            DGV.Columns[3].Width = 230;
            DGV.Columns[4].Visible = false;
            DGV.Columns[5].Width = 218;
        }

        //METODO ENCARGADO DE VALIDAR QUE LOS ATRIBUTOS NO SE (REPITAN, TENGAN ESPACIO EN BLANCO O ESTE EN NO APLICA) ESTE SERA INVOCADO EN EL BOTON DE GUARDAR ATRIBUTOS
        private void ValidacionGruposCampos()
        {
            //CARACTERISTICAS
            string cbcaracteristicas1 = cboTipoCaracteristicas1.Text;
            string cbcaracteristicas2 = cboTipoCaracteristicas2.Text;
            string cbcaracteristicas3 = cboTipoCaracteristicas3.Text;
            string cbcaracteristicas4 = cboTipoCaracteristicas4.Text;

            if (ckCaracteristicas1.Checked == true)
            {
                if (cbcaracteristicas1 == "NO APLICA")
                {
                    MessageBox.Show("Debe seleccionar un atributo valido en los primeros campos de los grupos de Caracteristicas.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbcaracteristicas1 == cbcaracteristicas2 || cbcaracteristicas1 == cbcaracteristicas3 || cbcaracteristicas1 == cbcaracteristicas4
               || cbcaracteristicas2 == cbcaracteristicas1 || cbcaracteristicas2 == cbcaracteristicas3 || cbcaracteristicas2 == cbcaracteristicas4)
                {
                    MessageBox.Show("Los atributos no se pueden repetir.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbcaracteristicas1 == "" || cbcaracteristicas2 == "")
                {
                    MessageBox.Show("Los campos no pueden estar vacios.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
            }

            if (ckCaracteristicas2.Checked == true)
            {
                if (cbcaracteristicas3 == "NO APLICA")
                {
                    MessageBox.Show("Debe seleccionar un atributo valido en los primeros campos de los grupos de Caracteristicas.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbcaracteristicas3 == cbcaracteristicas1 || cbcaracteristicas3 == cbcaracteristicas2 || cbcaracteristicas3 == cbcaracteristicas4
               || cbcaracteristicas4 == cbcaracteristicas1 || cbcaracteristicas4 == cbcaracteristicas2 || cbcaracteristicas4 == cbcaracteristicas3)
                {
                    MessageBox.Show("Los atributos no se pueden repetir.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbcaracteristicas3 == "" || cbcaracteristicas4 == "")
                {
                    MessageBox.Show("Los campos no pueden estar vacios.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
            }

            //MEDIDAS
            string cbmedidas1 = cboTipoMedida1.Text;
            string cbmedidas2 = cboTipoMedida2.Text;
            string cbmedidas3 = cboTipoMedida3.Text;
            string cbmedidas4 = cboTipoMedida4.Text;

            if (ckCamposMedida1.Checked == true || ckCamposMedida2.Checked == true)
            {
                if (cbmedidas1 == "NO APLICA")
                {
                    MessageBox.Show("Debe seleccionar un atributo valido en los primeros campos de los grupos de Medidas.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbmedidas1 == cbmedidas2 || cbmedidas1 == cbmedidas3 || cbmedidas1 == cbmedidas4
               || cbmedidas2 == cbmedidas1 || cbmedidas2 == cbmedidas3 || cbmedidas2 == cbmedidas4)
                {
                    MessageBox.Show("Los atributos no se pueden repetir.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbmedidas1 == "" || cbmedidas2 == "")
                {
                    MessageBox.Show("Los campos no pueden estar vacios.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

            }
            if (ckCamposMedida2.Checked == true)
            {
                if (cbmedidas3 == "NO APLICA")
                {
                    MessageBox.Show("Debe seleccionar un atributo valido en los primeros campos de los grupos de Medidas.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbmedidas3 == cbmedidas1 || cbmedidas3 == cbmedidas2 || cbmedidas3 == cbmedidas4
               || cbmedidas4 == cbmedidas1 || cbmedidas4 == cbmedidas2 || cbmedidas4 == cbmedidas3)
                {
                    MessageBox.Show("Los atributos no se pueden repetir.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbmedidas3 == "" || cbmedidas4 == "")
                {
                    MessageBox.Show("Los campos no pueden estar vacios.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
            }

            //DIAMETRO
            string cbdiametros1 = cboTiposDiametros1.Text;
            string cbdiametros2 = cboTiposDiametros2.Text;
            string cbdiametros3 = cboTiposDiametros3.Text;
            string cbdiametros4 = cboTiposDiametros4.Text;

            if (ckCamposDiametros1.Checked == true)
            {
                if (cbdiametros1 == "NO APLICA")
                {
                    MessageBox.Show("Debe seleccionar un atributo valido en los primeros campos de los grupos de Diametros.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbdiametros1 == cbdiametros2 || cbdiametros1 == cbdiametros3 || cbdiametros1 == cbdiametros4
               || cbdiametros2 == cbdiametros1 || cbdiametros2 == cbdiametros3 || cbdiametros2 == cbdiametros4)
                {
                    MessageBox.Show("Los atributos no se pueden repetir.", "Validacion del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbmedidas1 == "" || cbdiametros2 == "")
                {
                    MessageBox.Show("Los campos no pueden estar vacios.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
            }

            if (ckCamposDiametros2.Checked == true)
            {
                if (cbdiametros3 == "NO APLICA")
                {
                    MessageBox.Show("Debe seleccionar un atributo valido en los primeros campos de los grupos de Diametros.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbdiametros3 == cbdiametros1 || cbdiametros3 == cbdiametros2 || cbdiametros3 == cbdiametros4
               || cbdiametros4 == cbdiametros1 || cbdiametros4 == cbdiametros2 || cbdiametros4 == cbdiametros3)
                {
                    MessageBox.Show("Los atributos no se pueden repetir.", "Validacion del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbdiametros3 == "" || cbdiametros4 == "")
                {
                    MessageBox.Show("Los campos no pueden estar vacios.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
            }

            //FORMA
            string cbformas1 = cboTiposFormas1.Text;
            string cbformas2 = cboTiposFormas2.Text;
            string cbformas3 = cboTiposFormas3.Text;
            string cbformas4 = cboTiposFormas4.Text;

            if (ckCamposFormas1.Checked == true)
            {
                if (cbformas1 == "NO APLICA")
                {
                    MessageBox.Show("Debe seleccionar un atributo valido en los primeros campos de los grupos de Formas", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbformas1 == cbformas2 || cbformas1 == cbformas3 || cbformas1 == cbformas4
               || cbformas2 == cbformas1 || cbformas2 == cbformas3 || cbformas2 == cbformas4)
                {
                    MessageBox.Show("Los atributos no se pueden repetir.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbformas1 == "" || cbformas2 == "")
                {
                    MessageBox.Show("Los campos no pueden estar vacios.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }             
            }

            if (ckCamposFormas2.Checked == true)
            {
                if (cbformas3 == "NO APLICA")
                {
                    MessageBox.Show("Debe seleccionar un atributo valido en los primeros campos de los grupos de Formas", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbformas3 == cbformas1 || cbformas3 == cbformas2 || cbformas3 == cbformas4
               || cbformas4 == cbformas1 || cbformas4 == cbformas2 || cbformas4 == cbformas3)
                {
                    MessageBox.Show("Los atributos no se pueden repetir.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbformas3 == "" || cbformas4 == "")
                {
                    MessageBox.Show("Los campos no pueden estar vacios.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }          
            }

            //ESPESORES
            string cbespesores1 = cbooTipoEspesores1.Text;
            string cbespesores2 = cbooTipoEspesores2.Text;
            string cbespesores3 = cbooTipoEspesores3.Text;
            string cbespesores4 = cbooTipoEspesores4.Text;

            if (ckCamposEspesores1.Checked == true)
            {
                if (cbespesores1 == "NO APLICA")
                {
                    MessageBox.Show("Debe seleccionar un atributo valido en los primeros campos de los grupos de Espesores", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbespesores1 == cbespesores2 || cbespesores1 == cbespesores3 || cbespesores1 == cbespesores4
               || cbespesores2 == cbespesores1 || cbespesores2 == cbespesores3 || cbespesores2 == cbespesores4)
                {
                    MessageBox.Show("Los atributos no se pueden repetir.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbespesores1 == "" || cbespesores2 == "")
                {
                    MessageBox.Show("Los campos no pueden estar vacios.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }        
            }

            if (ckCamposEspesores2.Checked == true)
            {
                if (cbespesores3 == "NO APLICA")
                {
                    MessageBox.Show("Debe seleccionar un atributo valido en los primeros campos de los grupos de Espesores", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbespesores3 == cbespesores1 || cbespesores3 == cbespesores2 || cbespesores3 == cbespesores4
               || cbespesores4 == cbespesores1 || cbespesores4 == cbespesores2 || cbespesores4 == cbespesores3)
                {
                    MessageBox.Show("Los atributos no se pueden repetir.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbespesores3 == "" || cbespesores4 == "")
                {
                    MessageBox.Show("Los campos no pueden estar vacios.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }       
            }

            //DISEÑO Y ACABADO
            string cbdiseñoacabado1 = cboTiposDiseñosAcabados1.Text;
            string cbdiseñoacabado2 = cboTiposDiseñosAcabados2.Text;
            string cbdiseñoacabado3 = cboTiposDiseñosAcabados3.Text;
            string cbdiseñoacabado4 = cboTiposDiseñosAcabados4.Text;

            if (ckCamposDiseñoAcabado1.Checked == true)
            {
                if (cbdiseñoacabado1 == "NO APLICA")
                {
                    MessageBox.Show("Debe seleccionar un atributo valido en los primeros campos de los grupos de Diseño Acabado.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbdiseñoacabado1 == cbdiseñoacabado2 || cbdiseñoacabado1 == cbdiseñoacabado3 || cbdiseñoacabado1 == cbdiseñoacabado4
               || cbdiseñoacabado2 == cbdiseñoacabado1 || cbdiseñoacabado2 == cbdiseñoacabado3 || cbdiseñoacabado2 == cbdiseñoacabado4)
                {
                    MessageBox.Show("Los atributos no se pueden repetir.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbdiseñoacabado1 == "" || cbdiseñoacabado2 == "")
                {
                    MessageBox.Show("Los campos no pueden estar vacios.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }       
            }

            if (ckCamposDiseñoAcabado2.Checked == true)
            {
                if (cbdiseñoacabado3 == "NO APLICA")
                {
                    MessageBox.Show("Debe seleccionar un atributo valido en los primeros campos de los grupos de Diseño Acabado.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbdiseñoacabado3 == cbdiseñoacabado1 || cbdiseñoacabado3 == cbdiseñoacabado2 || cbdiseñoacabado3 == cbdiseñoacabado4
               || cbdiseñoacabado4 == cbdiseñoacabado1 || cbdiseñoacabado4 == cbdiseñoacabado2 || cbdiseñoacabado4 == cbdiseñoacabado3)
                {
                    MessageBox.Show("Los atributos no se pueden repetir.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbdiseñoacabado3 == "" || cbdiseñoacabado4 == "")
                {
                    MessageBox.Show("Los campos no pueden estar vacios.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }          
            }

            //N TIPOS
            string cbNtipos1 = cboTiposNTipos1.Text;
            string cbNtipos2 = cboTiposNTipos2.Text;
            string cbNtipos3 = cboTiposNTipos3.Text;
            string cbNtipos4 = cboTiposNTipos4.Text;

            if (ckCamposNTipos1.Checked == true)
            {
                if (cbNtipos1 == "NO APLICA")
                {
                    MessageBox.Show("Debe seleccionar un atributo valido en los primeros campos de los grupos de Tipos.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbNtipos1 == cbNtipos2 || cbNtipos1 == cbNtipos3 || cbNtipos1 == cbNtipos4
               || cbNtipos2 == cbNtipos1 || cbNtipos2 == cbNtipos3 || cbNtipos2 == cbNtipos4)
                {
                    MessageBox.Show("Los atributos no se pueden repetir.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbNtipos1 == "" || cbNtipos2 == "")
                {
                    MessageBox.Show("Los campos no pueden estar vacios.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }            
            }
            if (ckCamposNTipos2.Checked == true)
            {

                if (cbNtipos3 == "NO APLICA")
                {
                    MessageBox.Show("Debe seleccionar un atributo valido en los primeros campos de los grupos de Tipos.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbNtipos3 == cbNtipos1 || cbNtipos3 == cbNtipos2 || cbNtipos3 == cbNtipos4
               || cbNtipos4 == cbNtipos1 || cbNtipos4 == cbNtipos2 || cbNtipos4 == cbNtipos3)
                {
                    MessageBox.Show("Los atributos no se pueden repetir.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbNtipos3 == "" || cbNtipos4 == "")
                {
                    MessageBox.Show("Los campos no pueden estar vacios.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
            }

            //VARIOS
            string cbVariosO1 = cboTiposVariosO1.Text;
            string cbVariosO2 = cboTiposVariosO2.Text;

            if (ckVariosO1.Checked == true)
            {
                if (cbVariosO1 == "NO APLICA")
                {
                    MessageBox.Show("Debe seleccionar un atributo valido en el primer campo del grupo de Varios.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbVariosO1 == "")
                {
                    MessageBox.Show("Los campos no pueden estar vacios", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                } 
            }

            if (ckVariosO2.Checked == true)
            {
                if (cbVariosO2 == "")
                {
                    MessageBox.Show("Los campos no pueden estar vacios", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbVariosO1 == cbVariosO2)
                {
                    MessageBox.Show("Los atributos no se pueden repetir.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
            }
            panelDefinicionAtributos.Visible = false;
        }


        //VALIDACIONES TIEMPO REAL PARA LA DEFINICIÓN DE ATRIBUTOS
        private void ValidacionesCaracteristicas_TiempoReal(CheckBox ckCaracteri1,CheckBox ckCaracteri2, ComboBox cbcaracteristicas1, ComboBox cbcaracteristicas2, ComboBox cbcaracteristicas3 , ComboBox cbcaracteristicas4)
        {
            if (ckCaracteri1.Checked == true)
            {
                if (cbcaracteristicas2.Text == "NO APLICA")
                {
                    ckCaracteri2.Checked = false;
                    ckCaracteri2.Enabled = false;
                    MessageBox.Show("AVISO: Si selecciona el atributo " + cbcaracteristicas2.Text + " no podra o se deshabilitara la opcion de utilizar el segundo grupo de Caracteristicas.", "Validación Del Sistema", MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    ckCaracteri2.Enabled = true;
                }
            }
            if (ckCaracteri1.Checked == true)
            {
                if (cbcaracteristicas1.Text == "NO APLICA")
                {
                    MessageBox.Show("Defina atributos validos y seleccione los atributos ordenadamente.", "Validación del Sistema", MessageBoxButtons.OK);
                    ckCaracteri2.Enabled = false;
                    ckCaracteri2.Checked = false;
                    cbcaracteristicas2.SelectedIndex = 0;
                    cbcaracteristicas1.Focus();
                }
            }
            if (ckCaracteri2.Checked == true)
            {
                if (cbcaracteristicas3.Text == "NO APLICA")
                {
                    MessageBox.Show("Defina atributos validos y seleccione los atributos ordenadamente.", "Validación del Sistema", MessageBoxButtons.OK);
                    cbcaracteristicas4.SelectedIndex = 0;
                    cbcaracteristicas3.Focus();
                }
            }
        }

        public void ValidacionesMedidas_TiempoReal(CheckBox ckMedi1, CheckBox ckMedi2, ComboBox cbmedidas1, ComboBox cbmedidas2, ComboBox cbmedidas3, ComboBox cbmedidas4)
        {

          if(ckMedi1.Checked == true)
            {
                if(cbmedidas2.Text == "NO APLICA")
                {
                    ckMedi2.Checked = false;
                    ckMedi2.Enabled = false;
                    MessageBox.Show("AVISO: Si selecciona el atributo " + cbmedidas2.Text + " no podra o se deshabilitara la opcion de utilizar el segundo grupo de Medidas.", "Validación Del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    ckMedi2.Enabled = true;
                }
            }
            if (ckMedi1.Checked == true)
            {
                if (cbmedidas1.Text == "NO APLICA")
                {
                    MessageBox.Show("Defina atributos validos y seleccione los atributos ordenadamente.", "Validación del Sistema", MessageBoxButtons.OK);
                    ckMedi2.Enabled = false;
                    ckMedi2.Checked = false;
                    cbmedidas2.SelectedIndex = 0;
                    cbmedidas1.Focus();
                }
            }
            if (ckMedi2.Checked == true)
            {
                if (cbmedidas3.Text == "NO APLICA")
                {
                    MessageBox.Show("Defina atributos validos y seleccione los atributos ordenadamente.", "Validación del Sistema", MessageBoxButtons.OK);
                    cbmedidas4.SelectedIndex = 0;
                    cbmedidas3.Focus();
                }
            }
        }
    
        public void ValidacionesDiametros_TiempoReal(CheckBox ckDiametr1, CheckBox ckDiametr2, ComboBox cbdiametros1, ComboBox cbdiametros2, ComboBox cbdiametros3, ComboBox cbdiametros4)
        {
            if (ckDiametr1.Checked == true)
            {
                if (cbdiametros2.Text == "NO APLICA")
                {
                    ckDiametr2.Checked = false;
                    ckDiametr2.Enabled = false;
                    MessageBox.Show("AVISO: Si selecciona el atributo " + cbdiametros2.Text + " no podra o se deshabilitara la opcion de utilizar el segundo grupo de Diametros.", "Validación Del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    ckDiametr2.Enabled = true;
                }
            }
            if (ckDiametr1.Checked == true)
            {
                if (cbdiametros1.Text == "NO APLICA")
                {
                    MessageBox.Show("Defina atributos validos y seleccione los atributos ordenadamente.", "Validación del Sistema", MessageBoxButtons.OK);
                    ckDiametr2.Enabled = false;
                    ckDiametr2.Checked = false;
                    cbdiametros2.SelectedIndex = 0;
                    ckDiametr1.Focus();
                }
            }
            if (ckDiametr2.Checked == true)
            {
                if (cbdiametros3.Text == "NO APLICA")
                {
                    MessageBox.Show("Defina atributos validos y seleccione los atributos ordenadamente.", "Validación del Sistema", MessageBoxButtons.OK);
                    cbdiametros4.SelectedIndex = 0;
                    cbdiametros3.Focus();
                }
            }
        }

        public void ValidacionesFormas_TiempoReal(CheckBox ckForm1, CheckBox ckForm2, ComboBox cbformas1, ComboBox cbformas2, ComboBox cbformas3, ComboBox cbformas4)
        {
            if (ckForm1.Checked == true)
            {
                if (cbformas2.Text == "NO APLICA")
                {
                    ckForm2.Checked = false;
                    ckForm2.Enabled = false;
                    MessageBox.Show("AVISO: Si selecciona el atributo " + cbformas2.Text + " no podra o se deshabilitara la opcion de utilizar el segundo grupo de Formas.", "Validación Del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    ckForm2.Enabled = true;
                }
            }
            if (ckForm1.Checked == true)
            {
                if (cbformas1.Text == "NO APLICA")
                {
                    MessageBox.Show("Defina atributos validos y seleccione los atributos ordenadamente.", "Validación del Sistema", MessageBoxButtons.OK);
                    ckForm2.Enabled = false;
                    ckForm2.Checked = false;
                    cbformas2.SelectedIndex = 0;
                    cbformas1.Focus();
                }
            }
            if (ckForm2.Checked == true)
            {
                if (cbformas3.Text == "NO APLICA")
                {
                    MessageBox.Show("Defina atributos validos y seleccione los atributos ordenadamente.", "Validación del Sistema", MessageBoxButtons.OK);
                    cbformas4.SelectedIndex = 0;
                    cbformas3.Focus();
                }
            }
        }

        public void ValidacionesEspesores_TiempoReal(CheckBox ckEspe1, CheckBox ckEspe2, ComboBox cbespesores1, ComboBox cbespesores2, ComboBox cbespesores3, ComboBox cbespesores4)
        {
            if (ckEspe1.Checked == true)
            {
                if (cbespesores2.Text == "NO APLICA")
                {
                    ckEspe2.Checked = false;
                    ckEspe2.Enabled = false;
                    MessageBox.Show("AVISO: Si selecciona el atributo " + cbespesores2.Text + " no podra o se deshabilitara la opcion de utilizar el segundo grupo de Espesores.", "Validación Del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    ckEspe2.Enabled = true;
                }
            }
            if (ckEspe1.Checked == true)
            {
                if (cbespesores1.Text == "NO APLICA")
                {
                    MessageBox.Show("Defina atributos validos y seleccione los atributos ordenadamente.", "Validación del Sistema", MessageBoxButtons.OK);
                    ckEspe2.Enabled = false;
                    ckEspe2.Checked = false;
                    cbespesores2.SelectedIndex = 0;
                    cbespesores1.Focus();
                }
            }
            if (ckEspe2.Checked == true)
            {
                if (cbespesores3.Text == "NO APLICA")
                {
                    MessageBox.Show("Defina atributos validos y seleccione los atributos ordenadamente.", "Validación del Sistema", MessageBoxButtons.OK);
                    cbespesores4.SelectedIndex = 0;
                    cbespesores3.Focus();
                }
            }
        }

        public void ValidacionesDiseñoAcabado_TiempoReal(CheckBox ckDiseAca1, CheckBox ckDiseAca2, ComboBox cbdiseñoacabado1, ComboBox cbdiseñoacabado2, ComboBox cbdiseñoacabado3, ComboBox cbdiseñoacabado4)
        {
            if (ckDiseAca1.Checked == true)
            {
                if (cbdiseñoacabado2.Text == "NO APLICA")
                {
                    ckDiseAca2.Checked = false;
                    ckDiseAca2.Enabled = false;
                    MessageBox.Show("AVISO: Si selecciona el atributo " + cbdiseñoacabado2.Text + " no podra o se deshabilitara la opcion de utilizar el segundo grupo de Diseño Acabado.", "Validación Del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    ckDiseAca2.Enabled = true;
                }
            }
            if (ckDiseAca1.Checked == true)
            {
                if (cbdiseñoacabado1.Text == "NO APLICA")
                {
                    MessageBox.Show("Defina atributos validos y seleccione los atributos ordenadamente.", "Validación del Sistema", MessageBoxButtons.OK);
                    ckDiseAca2.Enabled = false;
                    ckDiseAca2.Checked = false;
                    cbdiseñoacabado2.SelectedIndex = 0;
                    cbdiseñoacabado1.Focus();
                }
            }
            if (ckDiseAca2.Checked == true)
            {
                if (cbdiseñoacabado3.Text == "NO APLICA")
                {
                    MessageBox.Show("Defina atributos validos y seleccione los atributos ordenadamente.", "Validación del Sistema", MessageBoxButtons.OK);
                    cbdiseñoacabado4.SelectedIndex = 0;
                    cbdiseñoacabado3.Focus();
                }
            }
        }

        public void ValidacionesNTipos_TiempoReal(CheckBox ckNtip1, CheckBox ckNtip2, ComboBox cbNtipos1, ComboBox cbNtipos2, ComboBox cbNtipos3, ComboBox cbNtipos4)
        {
            if (ckNtip1.Checked == true)
            {
                if (cbNtipos2.Text == "NO APLICA")
                {
                    ckNtip2.Checked = false;
                    ckNtip2.Enabled = false;
                    MessageBox.Show("AVISO: Si selecciona el atributo " + cbNtipos2.Text + " no podra o se deshabilitara la opcion de utilizar el segundo grupo de N y Tipos.", "Validación Del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    ckNtip2.Enabled = true;
                }
            }
            if (ckNtip1.Checked == true)
            {
                if (cbNtipos1.Text == "NO APLICA")
                {
                    MessageBox.Show("Defina atributos validos y seleccione los atributos ordenadamente.", "Validación del Sistema", MessageBoxButtons.OK);
                    ckNtip2.Enabled = false;
                    ckNtip2.Checked = false;
                    cbNtipos2.SelectedIndex = 0;
                    cbNtipos1.Focus();
                }
            }
            if (ckNtip2.Checked == true)
            {
                if (cbNtipos3.Text == "NO APLICA")
                {
                    MessageBox.Show("Defina atributos validos y seleccione los atributos ordenadamente.", "Validación del Sistema", MessageBoxButtons.OK);
                    cbNtipos4.SelectedIndex = 0;
                    cbNtipos3.Focus();
                }
            }
        }

        public void ValidacionesVarios_TiempoReal(CheckBox ckVari1, CheckBox ckVarios2, ComboBox cbVariosO1, ComboBox cbVariosO2)
        {
           
            if (ckVari1.Checked == true)
            {
                if (cbVariosO1.Text == "NO APLICA")
                {
                    ckVarios2.Checked = false;
                    ckVarios2.Enabled = false;
                    MessageBox.Show("AVISO: Si selecciona el atributo " + cbVariosO1.Text + " no podra o se deshabilitara la opcion de utilizar el segundo grupo de Varios.", "Validación Del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    ckVarios2.Enabled = true;
                }
            }
        }

        //EVENTOS DE CIERRE DE COMBOS PARA VALIDACIONES EN TIEMPO REAL
        private void cboTipoCaracteristicas2_DropDownClosed(object sender, EventArgs e)
        {
            ValidacionesCaracteristicas_TiempoReal(ckCaracteristicas1,ckCaracteristicas2,cboTipoCaracteristicas1,cboTipoCaracteristicas2,cboTipoCaracteristicas3,cboTipoCaracteristicas4);
        }

        private void cboTipoCaracteristicas4_DropDownClosed(object sender, EventArgs e)
        {
            ValidacionesCaracteristicas_TiempoReal(ckCaracteristicas1, ckCaracteristicas2, cboTipoCaracteristicas1, cboTipoCaracteristicas2, cboTipoCaracteristicas3, cboTipoCaracteristicas4);
        }

        private void cboTipoMedida2_DropDownClosed(object sender, EventArgs e)
        {
            ValidacionesMedidas_TiempoReal(ckCamposMedida1, ckCamposMedida2, cboTipoMedida1, cboTipoMedida2, cboTipoMedida3, cboTipoMedida4);
        }

        private void cboTipoMedida4_DropDownClosed(object sender, EventArgs e)
        {
            ValidacionesMedidas_TiempoReal(ckCamposMedida1, ckCamposMedida2, cboTipoMedida1, cboTipoMedida2, cboTipoMedida3, cboTipoMedida4);
        }

        private void cboTiposDiametros2_DropDownClosed(object sender, EventArgs e)
        {
            ValidacionesDiametros_TiempoReal(ckCamposDiametros1, ckCamposDiametros2, cboTiposDiametros1, cboTiposDiametros2, cboTiposDiametros3, cboTiposDiametros4);
        }

        private void cboTiposDiametros4_DropDownClosed(object sender, EventArgs e)
        {
            ValidacionesDiametros_TiempoReal(ckCamposDiametros1, ckCamposDiametros2, cboTiposDiametros1, cboTiposDiametros2, cboTiposDiametros3, cboTiposDiametros4);
        }

        private void cboTiposFormas2_DropDownClosed(object sender, EventArgs e)
        {
            ValidacionesFormas_TiempoReal(ckCamposFormas1,ckCamposFormas2, cboTiposFormas1, cboTiposFormas2, cboTiposFormas3, cboTiposFormas4);
        }

        private void cboTiposFormas4_DropDownClosed(object sender, EventArgs e)
        {
            ValidacionesFormas_TiempoReal(ckCamposFormas1, ckCamposFormas2, cboTiposFormas1, cboTiposFormas2, cboTiposFormas3, cboTiposFormas4);
        }

        private void cbooTipoEspesores2_DropDownClosed(object sender, EventArgs e)
        {
            ValidacionesEspesores_TiempoReal(ckCamposEspesores1, ckCamposEspesores2, cbooTipoEspesores1, cbooTipoEspesores2, cbooTipoEspesores3, cbooTipoEspesores4);
        }

        private void cbooTipoEspesores4_DropDownClosed(object sender, EventArgs e)
        {
            ValidacionesEspesores_TiempoReal(ckCamposEspesores1, ckCamposEspesores2, cbooTipoEspesores1, cbooTipoEspesores2, cbooTipoEspesores3, cbooTipoEspesores4);
        }

        private void cboTiposDiseñosAcabados2_DropDownClosed(object sender, EventArgs e)
        {
            ValidacionesDiseñoAcabado_TiempoReal(ckCamposDiseñoAcabado1, ckCamposDiseñoAcabado2, cboTiposDiseñosAcabados1, cboTiposDiseñosAcabados2, cboTiposDiseñosAcabados3, cboTiposDiseñosAcabados4);
        }

        private void cboTiposDiseñosAcabados4_DropDownClosed(object sender, EventArgs e)
        {
            ValidacionesDiseñoAcabado_TiempoReal(ckCamposDiseñoAcabado1, ckCamposDiseñoAcabado2, cboTiposDiseñosAcabados1, cboTiposDiseñosAcabados2, cboTiposDiseñosAcabados3, cboTiposDiseñosAcabados4);
        }

        private void cboTiposNTipos2_DropDownClosed(object sender, EventArgs e)
        {
            ValidacionesNTipos_TiempoReal(ckCamposNTipos1, ckCamposNTipos2, cboTiposNTipos1, cboTiposNTipos2, cboTiposNTipos3, cboTiposNTipos4);
        }

        private void cboTiposNTipos4_DropDownClosed(object sender, EventArgs e)
        {
            ValidacionesNTipos_TiempoReal(ckCamposNTipos1, ckCamposNTipos2, cboTiposNTipos1, cboTiposNTipos2, cboTiposNTipos3, cboTiposNTipos4);
        }

        private void cboTiposVariosO1_DropDownClosed(object sender, EventArgs e)
        {
            ValidacionesVarios_TiempoReal(ckVariosO1, ckVariosO2, cboTiposVariosO1, cboTiposVariosO2);
        }

        //LIMPIAR LA BUSQUEDA DE MODELO
        private void cboBusquedaModelo_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusquedaModelo.Text = "";
        }
    }
}


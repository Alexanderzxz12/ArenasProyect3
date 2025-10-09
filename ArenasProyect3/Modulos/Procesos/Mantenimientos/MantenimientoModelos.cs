using DocumentFormat.OpenXml.Office2013.Drawing.Chart;
using FlashControlV71;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Management;
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
                lblCodigoLinea.Text = System.Convert.ToString(row["Desciripcion"]);
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
                    lblCodigoLinea.Text = System.Convert.ToString(row["Desciripcion"]);
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
                datalistadoLineas.Columns[0].Width = 80;
                datalistadoLineas.Columns[1].Width = 80;
                datalistadoLineas.Columns[2].Width = 100;
                datalistadoLineas.Columns[3].Width = 220;
                datalistadoLineas.Columns[4].Visible = false;
                datalistadoLineas.Columns[5].Width = 218;
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

                if (datalistadoLineas.SelectedCells[6].Value.ToString() == "NO DEFINIDO")
                {
                    lblEstadoAtributo.Text = "MODELO NO DEFINIDO";
                    lblEstadoAtributo.ForeColor = Color.Red;
                }
                else
                {
                    lblEstadoAtributo.Text = "MODELO YA DEFINIDO";
                    lblEstadoAtributo.ForeColor = Color.Green;
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
                da = new SqlDataAdapter("SELECT IdModelo FROM MODELOS WHERE Estado = 1 AND IdModelo = (SELECT MAX(IdModelo) FROM MODELOS)\r\n", con);
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

            lblEstadoAtributo.Text = "MODELO NO DEFINIDO";
        }

        //METODO ENCARGADO DE AGREGAR UN NUEVO MODELO A MI BASE DE DATOS
        public void AgregarModelos(string descripcion, string abreavitura, int codigolinea)
        {
            if (repetidoDescripcion == true)
            {
                MessageBox.Show("No se puede ingresar dos registros iguales.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                if (txtDescripcion.Text == "" || txtAbreviatura.Text == "" ||
                    ckCaracteristicas1.Checked == false && ckCaracteristicas2.Checked == false && ckCamposMedida1.Checked == false && ckCamposMedida2.Checked == false
                && ckCamposDiametros1.Checked == false && ckCamposDiametros2.Checked == false && ckCamposFormas1.Checked == false && ckCamposFormas2.Checked == false
                && ckCamposEspesores1.Checked == false && ckCamposEspesores2.Checked == false && ckCamposDiseñoAcabado1.Checked == false && ckCamposDiseñoAcabado2.Checked == false
                && ckCamposNTipos1.Checked == false && ckCamposNTipos2.Checked == false && ckVariosO1.Checked == false && ckVariosO2.Checked == false && ckGenerales.Checked == false)
                {
                    MessageBox.Show("Debe ingresar todos los campos necesarios para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
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

                            lblEstadoAtributo.Text = "***";

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

                            if (ckCaracteristicas1.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@campcaracteristicas1", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@campcaracteristicas1", 0);
                            }

                            if (ckCaracteristicas2.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@campcaracteristicas2", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@campcaracteristicas2", 0);
                            }

                            if (ckCamposMedida1.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@campmedidas1", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@campmedidas1", 0);
                            }

                            if (ckCamposMedida2.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@campmedidas2", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@campmedidas2", 0);
                            }

                            if (ckCamposDiametros1.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@campdiametro1", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@campdiametro1", 0);
                            }

                            if (ckCamposDiametros2.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@campdiametro2", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@campdiametro2", 0);
                            }

                            if (ckCamposFormas1.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@campformas1", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@campformas1", 0);
                            }

                            if (ckCamposFormas2.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@campformas2", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@campformas2", 0);
                            }

                            if (ckCamposEspesores1.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@campespesores1", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@campespesores1", 0);
                            }

                            if (ckCamposEspesores2.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@campespesores2", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@campespesores2", 0);
                            }

                            if (ckCamposDiseñoAcabado1.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@campdiseñoacabados1", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@campdiseñoacabados1", 0);
                            }

                            if (ckCamposDiseñoAcabado2.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@campdiseñoacabados2", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@campdiseñoacabados2", 0);
                            }

                            if (ckCamposNTipos1.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@campntipos1", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@campntipos1", 0);
                            }

                            if (ckCamposNTipos2.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@campntipos2", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@campntipos2", 0);
                            }

                            if (ckVariosO1.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@campvarios1", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@campvarios1", 0);
                            }

                            if (ckVariosO2.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@campvarios2", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@campvarios2", 0);
                            }

                            if (ckGenerales.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@campgenerales", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@campgenerales", 0);
                            }
                            cmd.ExecuteNonQuery();
                            con.Close();

                            //INGRESAMOS DETALLES
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("Modelos_InsertarAtributosXModeloDetalle", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idmodelo", lblCodigo.Text);

                            if (cboTipoCaracteristicas1.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipomercaderia1", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipomercaderia1", cboTipoCaracteristicas1.SelectedValue.ToString());
                            }

                            if (cboTipoCaracteristicas2.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipomercaderia2", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipomercaderia2", cboTipoCaracteristicas2.SelectedValue.ToString());
                            }

                            if (cboTipoCaracteristicas3.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipomercaderia3", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipomercaderia3", cboTipoCaracteristicas3.SelectedValue.ToString());
                            }

                            if (cboTipoCaracteristicas4.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipomercaderia4", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipomercaderia4", cboTipoCaracteristicas4.SelectedValue.ToString());
                            }

                            if (cboTipoMedida1.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipomedida1", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipomedida1", cboTipoMedida1.SelectedValue.ToString());
                            }

                            if (cboTipoMedida2.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipomedida2", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipomedida2", cboTipoMedida2.SelectedValue.ToString());
                            }

                            if (cboTipoMedida3.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipomedida3", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipomedida3", cboTipoMedida3.SelectedValue.ToString());
                            }

                            if (cboTipoMedida4.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipomedida4", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipomedida4", cboTipoMedida4.SelectedValue.ToString());
                            }

                            if (cboTiposDiametros1.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipodiametro1", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipodiametro1", cboTiposDiametros1.SelectedValue.ToString());
                            }

                            if (cboTiposDiametros2.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipodiametro2", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipodiametro2", cboTiposDiametros2.SelectedValue.ToString());
                            }

                            if (cboTiposDiametros3.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipodiametro3", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipodiametro3", cboTiposDiametros3.SelectedValue.ToString());
                            }

                            if (cboTiposDiametros4.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipodiametro4", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipodiametro4", cboTiposDiametros4.SelectedValue.ToString());
                            }

                            if (cboTiposFormas1.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipoformas1", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipoformas1", cboTiposFormas1.SelectedValue.ToString());
                            }

                            if (cboTiposFormas2.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipoformas2", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipoformas2", cboTiposFormas2.SelectedValue.ToString());
                            }

                            if (cboTiposFormas3.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipoformas3", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipoformas3", cboTiposFormas3.SelectedValue.ToString());
                            }

                            if (cboTiposFormas4.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipoformas4", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipoformas4", cboTiposFormas4.SelectedValue.ToString());
                            }

                            if (cbooTipoEspesores1.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipoespesores1", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipoespesores1", cbooTipoEspesores1.SelectedValue.ToString());
                            }

                            if (cbooTipoEspesores2.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipoespesores2", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipoespesores2", cbooTipoEspesores2.SelectedValue.ToString());
                            }

                            if (cbooTipoEspesores3.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipoespesores3", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipoespesores3", cbooTipoEspesores3.SelectedValue.ToString());
                            }

                            if (cbooTipoEspesores4.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipoespesores4", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipoespesores4", cbooTipoEspesores4.SelectedValue.ToString());
                            }

                            if (cboTiposDiseñosAcabados1.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipodiametroacabados1", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipodiametroacabados1", cboTiposDiseñosAcabados1.SelectedValue.ToString());
                            }

                            if (cboTiposDiseñosAcabados2.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipodiametroacabados2", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipodiametroacabados2", cboTiposDiseñosAcabados2.SelectedValue.ToString());
                            }

                            if (cboTiposDiseñosAcabados3.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipodiametroacabados3", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipodiametroacabados3", cboTiposDiseñosAcabados3.SelectedValue.ToString());
                            }

                            if (cboTiposDiseñosAcabados4.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipodiametroacabados4", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipodiametroacabados4", cboTiposDiseñosAcabados4.SelectedValue.ToString());
                            }

                            if (cboTiposNTipos1.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipontipos1", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipontipos1", cboTiposNTipos1.SelectedValue.ToString());
                            }

                            if (cboTiposNTipos2.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipontipos2", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipontipos2", cboTiposNTipos2.SelectedValue.ToString());
                            }

                            if (cboTiposNTipos3.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipontipos3", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipontipos3", cboTiposNTipos3.SelectedValue.ToString());
                            }

                            if (cboTiposNTipos4.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtipontipos4", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtipontipos4", cboTiposNTipos4.SelectedValue.ToString());
                            }

                            if (cboTiposVariosO1.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtpovarios1", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtpovarios1", cboTiposVariosO1.SelectedValue.ToString());
                            }

                            if (cboTiposVariosO2.SelectedValue == null)
                            {
                                cmd.Parameters.AddWithValue("@idtpovarios2", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@idtpovarios2", cboTiposVariosO2.SelectedValue.ToString());
                            }

                            if (ckGenerales.Checked == false)
                            {
                                cmd.Parameters.AddWithValue("@campogeneral", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@campogeneral", DBNull.Value);
                            }
                            cmd.ExecuteNonQuery();
                            con.Close();

                            Mostrar(codigolinea);
                            lblEstadoAtributo.Text = "MODELO YA DEFINIDO";

                            AgregarCk_NombreTipos(Convert.ToInt32(lblCodigo.Text),ckCaracteristicaAtributo1, ckCaracteristicaAtributo2, ckCaracteristicaAtributo3, ckCaracteristicaAtributo4
                                ,ckMedidasAtributos1, ckMedidasAtributos2, ckMedidasAtributos3, ckMedidasAtributos4,ckDiametroAtributos1, ckDiametroAtributos2, ckDiametroAtributos3, ckDiametroAtributos4
                                ,ckFormasAtributos1, ckFormasAtributos2, ckFormasAtributos3, ckFormasAtributos4,ckEspesoresAtributos1, ckEspesoresAtributos2, ckEspesoresAtributos3, ckEspesoresAtributos4
                                ,ckDiseñoAcabadoAtributos1, ckDiseñoAcabadoAtributos2, ckDiseñoAcabadoAtributos3, ckDiseñoAcabadoAtributos4,ckTiposNTiposAtributos1, ckTiposNTiposAtributos2, ckTiposNTiposAtributos3
                                , ckTiposNTiposAtributos4,ckVariosAtributos1, ckVariosAtributos2);

                            MessageBox.Show("Se ingresó el nuevo registro correctamente.", "Registro Nuevo", MessageBoxButtons.OK);
                            Limpiar();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        return;
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

                        if (cboEstado.Text == "ACTIVO")
                        {
                            cmd.Parameters.AddWithValue("@estado", 1);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@estado", 0);
                        }

                        cmd.ExecuteNonQuery();
                        con.Close();

                        int linea = Convert.ToInt32(codigolinea);
                        Mostrar(linea);

                        MessageBox.Show("Se editó correctamente el registro.", "Edición", MessageBoxButtons.OK);
                        ColorDescripcion();

                        txtDescripcion.Enabled = true;
                        txtAbreviatura.Enabled = false;

                        btnEditar.Visible = true;
                        btnEditar2.Visible = false;

                        btnGuardar.Visible = true;
                        btnGuardar2.Visible = false;

                        cboEstado.SelectedIndex = -1;
                        Cancelar.Visible = false;
                        lblCancelar.Visible = false;
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

            cboEstado.SelectedIndex = -1;
            txtDescripcion.Text = "";
            txtAbreviatura.Text = "";

            lblEstadoAtributo.Text = "***";
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
            if (lblEstadoAtributo.Text == "MODELO YA DEFINIDO" || lblEstadoAtributo.Text == "***")
            {
                MessageBox.Show("Este modelo ya ha sido definido.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                if (lblCodigoLinea.Text == "PRODUCTO TERMINADO")
                {
                    ckGenerales.Visible = false;
                }
                else
                {
                    ckGenerales.Visible = true;
                }

                panelDefinicionAtributos.Visible = true;

                BloquearCombos_AsingacionAtributos(ckCaracteristicas1, ckCaracteristicas2,ckCamposMedida1, ckCamposMedida2,ckCamposDiametros1, ckCamposDiametros2,ckCamposFormas1, ckCamposFormas2
                    , ckCamposEspesores1, ckCamposEspesores2,ckCamposDiseñoAcabado1, ckCamposDiseñoAcabado2,ckCamposNTipos1, ckCamposNTipos2,ckVariosO1, ckVariosO2,ckCaracteristicaAtributo1
                    , ckCaracteristicaAtributo2, ckCaracteristicaAtributo3, ckCaracteristicaAtributo4,ckMedidasAtributos1, ckMedidasAtributos2, ckMedidasAtributos3, ckMedidasAtributos4
                    , ckDiametroAtributos1, ckDiametroAtributos2, ckDiametroAtributos3, ckDiametroAtributos4,ckFormasAtributos1, ckFormasAtributos2, ckFormasAtributos3, ckFormasAtributos4
                    ,ckEspesoresAtributos1, ckEspesoresAtributos2, ckEspesoresAtributos3, ckEspesoresAtributos4,ckDiseñoAcabadoAtributos1, ckDiseñoAcabadoAtributos2, ckDiseñoAcabadoAtributos3
                    , ckDiseñoAcabadoAtributos4,ckTiposNTiposAtributos1, ckTiposNTiposAtributos2, ckTiposNTiposAtributos3, ckTiposNTiposAtributos4,ckVariosAtributos1, ckVariosAtributos2
                    , cboTipoCaracteristicas1.Text, cboTipoCaracteristicas2.Text, cboTipoCaracteristicas3.Text, cboTipoCaracteristicas4.Text,cboTipoMedida1.Text, cboTipoMedida2.Text
                    , cboTipoMedida3.Text, cboTipoMedida4.Text,cboTiposDiametros1.Text, cboTiposDiametros2.Text, cboTiposDiametros3.Text, cboTiposDiametros4.Text,cboTiposFormas1.Text
                    , cboTiposFormas2.Text, cboTiposFormas3.Text, cboTiposFormas4.Text,cbooTipoEspesores1.Text, cbooTipoEspesores2.Text, cbooTipoEspesores3.Text, cbooTipoEspesores4.Text
                    ,cboTiposDiseñosAcabados1.Text, cboTiposDiseñosAcabados2.Text, cboTiposDiseñosAcabados3.Text, cboTiposDiseñosAcabados4.Text,cboTiposNTipos1.Text, cboTiposNTipos2.Text
                    , cboTiposNTipos3.Text, cboTiposNTipos4.Text,cboTiposVariosO1.Text, cboTiposVariosO2.Text, flowLayoutPanel);
            }
        }

        public void BloquearCombos_AsingacionAtributos(CheckBox caracteGrup1, CheckBox caracteGrup2, CheckBox medidaGrup1, CheckBox medidaGrup2, CheckBox diameGrup1, CheckBox diameGrup2
            , CheckBox formaGrup1, CheckBox formaGrup2, CheckBox espesoGrup1, CheckBox espesoGrup2, CheckBox diseñoacaGrup1, CheckBox diseñoacaGrup2, CheckBox ntiposGrup1, CheckBox ntiposGrup2
            , CheckBox variosGrup1, CheckBox variosGrup2,CheckBox caracAtribu1, CheckBox caracAtribu2, CheckBox caracAtribu3, CheckBox caracAtribu4, CheckBox medidaAtribu1, CheckBox medidaAtribu2
            , CheckBox medidaAtribu3, CheckBox medidaAtribu4, CheckBox diameAtribu1, CheckBox diameAtribu2, CheckBox diameAtribu3, CheckBox diameAtribu4, CheckBox formaAtribu1, CheckBox formaAtribu2
            , CheckBox formaAtribu3, CheckBox formaAtribu4, CheckBox espesoAtribu1, CheckBox espesoAtribu2, CheckBox espesoAtribu3, CheckBox espesoAtribu4, CheckBox diseñoacabaAtribu1, CheckBox diseñoacabaAtribu2
            , CheckBox diseñoacabaAtribu3, CheckBox diseñoacabaAtribu4, CheckBox ntiposAtribu1, CheckBox ntiposAtribu2, CheckBox ntiposAtribu3, CheckBox ntiposAtribu4, CheckBox variosAtribu1
            , CheckBox variosAtribu2, string caracteTipo1, string caracteTipo2, string caracteTipo3, string caracteTipo4, string medidaTipo1, string medidaTipo2, string medidaTipo3, string medidaTipo4
            , string diameTipo1, string diameTipo2, string diameTipo3, string diameTipo4, string formaTipo1, string formaTipo2, string formaTipo3, string formaTipo4, string espesoTipo1, string espesoTipo2
            , string espesoTipo3, string espesoTipo4, string diseñoacabaTipo1, string diseñoacabaTipo2, string diseñoacabaTipo3, string diseñoacabaTipo4, string ntiposTipo1, string ntiposTipo2
            , string ntiposTipo3, string ntiposTipo4, string variosTipo1, string variosTipo2,FlowLayoutPanel definiciAtributos)
        {
            //bloqueo checkbox de los segundos grupos en la asignación de atributos para el modelo
            caracteGrup2.Enabled = false;
            medidaGrup2.Enabled = false;
            diameGrup2.Enabled = false;
            formaGrup2.Enabled = false;
            espesoGrup2.Enabled = false;
            diseñoacaGrup2.Enabled = false;
            ntiposGrup2.Enabled = false;
            variosGrup2.Enabled = false;

            //bloqueo de los checkbox de los atributos 
            caracAtribu1.Enabled = false;
            caracAtribu2.Enabled = false;
            caracAtribu3.Enabled = false;
            caracAtribu4.Enabled = false;

            medidaAtribu1.Enabled = false;
            medidaAtribu2.Enabled = false;
            medidaAtribu3.Enabled = false;
            medidaAtribu4.Enabled = false;

            diameAtribu1.Enabled = false;
            diameAtribu2.Enabled = false;
            diameAtribu3.Enabled = false;
            diameAtribu4.Enabled = false;

            formaAtribu1.Enabled = false;
            formaAtribu2.Enabled = false;
            formaAtribu3.Enabled = false;
            formaAtribu4.Enabled = false;

            espesoAtribu1.Enabled = false;
            espesoAtribu2.Enabled = false;
            espesoAtribu3.Enabled = false;
            espesoAtribu4.Enabled = false;

            diseñoacabaAtribu1.Enabled = false;
            diseñoacabaAtribu2.Enabled = false;
            diseñoacabaAtribu3.Enabled = false;
            diseñoacabaAtribu4.Enabled = false;

            ntiposAtribu1.Enabled = false;
            ntiposAtribu2.Enabled = false;
            ntiposAtribu3.Enabled = false;
            ntiposAtribu4.Enabled = false;

            variosAtribu1.Enabled = false;
            variosAtribu2.Enabled = false;

            if (definiciAtributos.Controls.Count > 0)
            {
                ////// CARACTERISTICAS
                if (caracteGrup1.Checked && (caracteTipo2 != "NO APLICA" && caracteTipo1 != "NO APLICA"))
                {
                    caracteGrup2.Enabled = true;

                    if(caracteTipo1 != "NO APLICA")
                    {
                        caracAtribu1.Enabled = true;
                    }
                    else
                    {
                        caracAtribu1.Enabled = false;
                    }

                    if(caracteTipo2 != "NO APLICA")
                    {
                        caracAtribu2.Enabled = true;
                    }
                    else
                    {
                        caracAtribu2.Enabled = false;
                    }
                }
                else
                {
                    caracteGrup2.Enabled = false;
                }

                if (caracteGrup2.Checked)
                {
                    if(caracteTipo3 != "NO APLICA")
                    {
                        caracAtribu3.Enabled = true;
                    }
                    else
                    {
                        caracAtribu3.Enabled = false;
                    }

                    if (caracteTipo4 != "NO APLICA")
                    {
                        caracAtribu4.Enabled = true;
                    }
                    else
                    {
                        caracAtribu4.Enabled = false;
                    }
                }


                /////// MEDIDAS
                if (medidaGrup1.Checked && (medidaTipo2 != "NO APLICA" && medidaTipo1 != "NO APLICA"))
                {
                    medidaGrup2.Enabled = true;

                    if(medidaTipo1 != "NO APLICA")
                    {
                        medidaAtribu1.Enabled = true;
                    }
                    else
                    {
                        medidaAtribu1.Enabled = false;
                    }

                    if(medidaTipo2 != "NO APLICA")
                    {
                        medidaAtribu2.Enabled = true;
                    }
                    else
                    {
                        medidaAtribu2.Enabled = false;
                    }
                }
                else
                {
                    medidaGrup2.Enabled = false;
                }

                if (medidaGrup2.Checked)
                {
                    if(medidaTipo3 != "NO APLICA")
                    {
                        medidaAtribu3.Enabled = true;
                    }
                    else
                    {
                        medidaAtribu3.Enabled = false;
                    }

                    if(medidaTipo4 != "NO APLICA")
                    {
                        medidaAtribu4.Enabled = true;
                    }
                    else
                    {
                        medidaAtribu4.Enabled = false;
                    }
                }

                ////// DIAMETROS
                if (diameGrup1.Checked && (diameTipo2 != "NO APLICA" && diameTipo1 != "NO APLICA"))
                {
                    diameGrup2.Enabled = true;

                    if(diameTipo1 != "NO APLICA")
                    {
                        diameAtribu1.Enabled = true;
                    }
                    else
                    {
                        diameAtribu1.Enabled = false;
                    }

                    if(diameTipo2 != "NO APLICA")
                    {
                        diameAtribu2.Enabled = true;
                    }
                    else
                    {
                        diameAtribu2.Enabled = false;
                    }
                }
                else
                {
                    diameGrup2.Enabled = false;
                }

                if (diameGrup2.Checked)
                {
                    if(diameTipo3 != "NO APLICA")
                    {
                        diameAtribu3.Enabled = true;
                    }
                    else
                    {
                        diameAtribu3.Enabled = false;
                    }

                    if(diameTipo4 != "NO APLICA")
                    {
                        diameAtribu4.Enabled = true;
                    }
                    else
                    {
                        diameAtribu4.Enabled = false;
                    }
                }

                ////// FORMAS
                if (formaGrup1.Checked && (formaTipo2 != "NO APLICA" && formaTipo1 != "NO APLICA"))
                {
                    formaGrup2.Enabled = true;
                    
                    if(formaTipo1 != "NO APLICA")
                    {
                        formaAtribu1.Enabled = true;
                    }
                    else
                    {
                        formaAtribu1.Enabled = false;
                    }

                    if(formaTipo2 != "NO APLICA")
                    {
                        formaAtribu2.Enabled = true;
                    }
                    else
                    {
                        formaAtribu2.Enabled = false;
                    }
                }
                else
                {
                    formaGrup2.Enabled = false;
                }

                if (formaGrup2.Checked)
                {
                    if(formaTipo3 != "NO APLICA")
                    {
                        formaAtribu3.Enabled = true;
                    }
                    else
                    {
                        formaAtribu3.Enabled = false;
                    }

                    if (formaTipo4 != "NO APLICA")
                    {
                        formaAtribu4.Enabled = true;
                    }
                    else
                    {
                        formaAtribu4.Enabled = false;
                    }
                }

                ////// ESPESORES
                if (espesoGrup1.Checked && (espesoTipo2 != "NO APLICA" && espesoTipo1 != "NO APLICA"))
                {
                    espesoGrup2.Enabled = true;

                    if(espesoTipo1 != "NO APLICA")
                    {
                        espesoAtribu1.Enabled = true;
                    }
                    else
                    {
                        espesoAtribu1.Enabled = false;
                    }

                    if(espesoTipo2 != "NO APLICA")
                    {
                        espesoAtribu2.Enabled = true;
                    }
                    else
                    {
                        espesoAtribu2.Enabled = false;
                    }
                }
                else
                {
                    espesoGrup2.Enabled = false;
                }

                if (espesoGrup2.Checked)
                {
                    if(espesoTipo3 != "NO APLICA")
                    {
                        espesoAtribu3.Enabled = true;
                    }
                    else
                    {
                        espesoAtribu3.Enabled = false;
                    }

                    if(espesoTipo4 != "NO APLICA")
                    {
                        espesoAtribu4.Enabled = true;
                    }
                    else
                    {
                        espesoAtribu4.Enabled = false;
                    }
                }

                ////// DISEÑO Y ACABADO
                if (diseñoacaGrup1.Checked && (diseñoacabaTipo2 != "NO APLICA" && diseñoacabaTipo1 != "NO APLICA"))
                {
                    diseñoacaGrup2.Enabled = true;

                    if(diseñoacabaTipo1 != "NO APLICA")
                    {
                        diseñoacabaAtribu1.Enabled = true;
                    }
                    else
                    {
                        diseñoacabaAtribu1.Enabled = false;
                    }

                    if(diseñoacabaTipo2 != "NO APLICA")
                    {
                        diseñoacabaAtribu2.Enabled = true;
                    }
                    else
                    {
                        diseñoacabaAtribu2.Enabled = false;
                    }
                }
                else
                {
                    diseñoacaGrup2.Enabled = false;
                }

                if (diseñoacaGrup2.Checked)
                {
                    if(diseñoacabaTipo3 != "NO APLICA")
                    {
                        diseñoacabaAtribu3.Enabled = true;
                    }
                    else
                    {
                        diseñoacabaAtribu3.Enabled = false;
                    }

                    if (diseñoacabaTipo4 != "NO APLICA")
                    {
                        diseñoacabaAtribu4.Enabled = true;
                    }
                    else
                    {
                        diseñoacabaAtribu4.Enabled = false;
                    }
                }

                ////// N TIPOS
                if (ntiposGrup1.Checked && (ntiposTipo2 != "NO APLICA" && ntiposTipo1 != "NO APLICA"))
                {
                    ntiposGrup2.Enabled = true;

                    if(ntiposTipo1 != "NO APLICA")
                    {
                        ntiposAtribu1.Enabled = true;
                    }
                    else
                    {
                        ntiposAtribu1.Enabled = false;
                    }

                    if(ntiposTipo2 != "NO APLICA")
                    {
                        ntiposAtribu2.Enabled = true;
                    }
                    else
                    {
                        ntiposAtribu2.Enabled = false;
                    }
                }
                else
                {
                    ntiposGrup2.Enabled = false;
                }

                if (ntiposGrup2.Checked)
                {
                    if(ntiposTipo3 != "NO APLICA")
                    {
                        ntiposAtribu3.Enabled = true;
                    }
                    else
                    {
                        ntiposAtribu3.Enabled = false;
                    }

                    if(ntiposTipo4 != "NO APLICA")
                    {
                        ntiposAtribu4.Enabled = true;
                    }
                    else
                    {
                        ntiposAtribu4.Enabled = false;
                    }
                }

                ////// VARIOS
                if (variosGrup1.Checked && variosTipo1 != "NO APLICA" )
                {
                    variosGrup2.Enabled = true;

                    if(variosTipo1 != "NO APLICA")
                    {
                        variosAtribu1.Enabled = true;
                    }
                    else
                    {
                        variosAtribu1.Enabled = false;
                    }                  
                }
                else
                {
                    variosGrup2.Enabled = false;
                }

                if (variosGrup2.Checked)
                {
                    if(variosTipo2 != "NO APLICA")
                    {
                        variosAtribu2.Enabled = true;
                    }
                    else
                    {
                        variosAtribu2.Enabled = false;
                    }
                }
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
                    habilitarValidaciones = false;
                    flowLayoutPanel.Controls.Add(panelCamposCaracteristicas1);
                    CargarTiposCaracteriticas(cboTipoCaracteristicas1);
                    CargarTiposCaracteriticas(cboTipoCaracteristicas2);
                    habilitarValidaciones = true;
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposCaracteristicas1);
                    ckCaracteristicaAtributo1.Checked = false;
                    ckCaracteristicaAtributo1.Enabled = false;
                    ckCaracteristicaAtributo2.Checked = false;
                    ckCaracteristicaAtributo2.Enabled = false;

                    ckCaracteristicas2.Enabled = false;
                    ckCaracteristicas2.Checked = false;


                }
            }
            else
            {
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
                    habilitarValidaciones = false;

                    flowLayoutPanel.Controls.Add(panelCamposCaracteristicas2);
                    CargarTiposCaracteriticas(cboTipoCaracteristicas3);
                    CargarTiposCaracteriticas(cboTipoCaracteristicas4);

                    habilitarValidaciones = true;
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposCaracteristicas2);
                    cboTipoCaracteristicas3.DataSource = null;
                    cboTipoCaracteristicas4.DataSource = null;

                    ckCaracteristicaAtributo3.Enabled = false;
                    ckCaracteristicaAtributo3.Checked = false;
                    ckCaracteristicaAtributo4.Enabled = false;
                    ckCaracteristicaAtributo4.Checked = false;


                }
            }
            else
            {
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
                    habilitarValidaciones = false;

                    flowLayoutPanel.Controls.Add(panelCamposMedidas1);
                    CargarTiposMedidas(cboTipoMedida1);
                    CargarTiposMedidas(cboTipoMedida2);

                    habilitarValidaciones = true;
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposMedidas1);
                    ckMedidasAtributos1.Checked = false;
                    ckMedidasAtributos1.Enabled = false;
                    ckMedidasAtributos2.Checked = false;
                    ckMedidasAtributos2.Enabled = false;

                    ckCamposMedida2.Enabled = false;
                    ckCamposMedida2.Checked = false;
                }
            }
            else
            {
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
                    habilitarValidaciones = false;

                    flowLayoutPanel.Controls.Add(panelCamposMedidas2);
                    CargarTiposMedidas(cboTipoMedida3);
                    CargarTiposMedidas(cboTipoMedida4);

                    habilitarValidaciones = true;
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposMedidas2);
                    cboTipoMedida3.DataSource = null;
                    cboTipoMedida4.DataSource = null;

                    ckMedidasAtributos3.Enabled = false;
                    ckMedidasAtributos3.Checked = false;
                    ckMedidasAtributos4.Enabled = false;
                    ckMedidasAtributos4.Checked = false;
                }
            }
            else
            {
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
                    habilitarValidaciones = false;

                    flowLayoutPanel.Controls.Add(panelCamposDiametros1);
                    CargarTiposDiametros(cboTiposDiametros1);
                    CargarTiposDiametros(cboTiposDiametros2);

                    habilitarValidaciones = true;
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposDiametros1);
                    ckDiametroAtributos1.Checked = false;
                    ckDiametroAtributos1.Enabled = false;
                    ckDiametroAtributos2.Checked = false;
                    ckDiametroAtributos2.Enabled = false;

                    ckCamposDiametros2.Enabled = false;
                    ckCamposDiametros2.Checked = false;
                }
            }
            else
            {
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
                    habilitarValidaciones = false;

                    flowLayoutPanel.Controls.Add(panelCamposDiametros2);
                    CargarTiposDiametros(cboTiposDiametros3);
                    CargarTiposDiametros(cboTiposDiametros4);

                    habilitarValidaciones = true;
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposDiametros2);
                    cboTiposDiametros3.DataSource = null;
                    cboTiposDiametros4.DataSource = null;

                    ckDiametroAtributos3.Checked = false;
                    ckDiametroAtributos3.Enabled = false;
                    ckDiametroAtributos4.Checked = false;
                    ckDiametroAtributos4.Enabled = false;
                }
            }
            else
            {
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
                    habilitarValidaciones = false;

                    flowLayoutPanel.Controls.Add(panelCamposFormas1);
                    CargarTiposFormas(cboTiposFormas1);
                    CargarTiposFormas(cboTiposFormas2);

                    habilitarValidaciones = true;
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposFormas1);
                    ckFormasAtributos1.Checked = false;
                    ckFormasAtributos1.Enabled = false;
                    ckFormasAtributos2.Checked = false;
                    ckFormasAtributos2.Enabled = false;

                    ckCamposFormas2.Enabled = false;
                    ckCamposFormas2.Checked = false;

                }
            }
            else
            {
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
                    habilitarValidaciones = false;

                    flowLayoutPanel.Controls.Add(panelCamposFormas2);
                    CargarTiposFormas(cboTiposFormas3);
                    CargarTiposFormas(cboTiposFormas4);

                    habilitarValidaciones = true;
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposFormas2);
                    cboTiposFormas3.DataSource = null;
                    cboTiposFormas4.DataSource = null;

                    ckFormasAtributos3.Enabled = false;
                    ckFormasAtributos3.Checked = false;
                    ckFormasAtributos4.Enabled = false;
                    ckFormasAtributos4.Checked = false;
                }
            }
            else
            {
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
                    habilitarValidaciones = false;

                    flowLayoutPanel.Controls.Add(panelCamposEspesores1);
                    CargarTiposEspesores(cbooTipoEspesores1);
                    CargarTiposEspesores(cbooTipoEspesores2);

                    habilitarValidaciones = true;
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposEspesores1);
                    ckEspesoresAtributos1.Checked = false;
                    ckEspesoresAtributos1.Enabled = false;
                    ckEspesoresAtributos2.Checked = false;
                    ckEspesoresAtributos2.Enabled= false;

                    ckCamposEspesores2.Enabled = false;
                    ckCamposEspesores2.Checked = false;

                }
            }
            else
            {
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
                    habilitarValidaciones = false;

                    flowLayoutPanel.Controls.Add(panelCamposEspesores2);
                    CargarTiposEspesores(cbooTipoEspesores3);
                    CargarTiposEspesores(cbooTipoEspesores4);

                    habilitarValidaciones = true;
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposEspesores2);
                    cbooTipoEspesores3.DataSource = null;
                    cbooTipoEspesores4.DataSource = null;

                    ckEspesoresAtributos3.Checked = false;
                    ckEspesoresAtributos3.Enabled = false;
                    ckEspesoresAtributos4.Checked = false;
                    ckEspesoresAtributos4.Enabled = false;
                }
            }
            else
            {
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
                    habilitarValidaciones = false;

                    flowLayoutPanel.Controls.Add(panelCamposDiseñoAcabado1);
                    CargarTiposDiseñoAcabado(cboTiposDiseñosAcabados1);
                    CargarTiposDiseñoAcabado(cboTiposDiseñosAcabados2);

                    habilitarValidaciones = true;
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposDiseñoAcabado1);
                    ckDiseñoAcabadoAtributos1.Checked = false;
                    ckDiseñoAcabadoAtributos1.Enabled = false;
                    ckDiseñoAcabadoAtributos2.Checked = false;
                    ckDiseñoAcabadoAtributos2.Enabled = false;

                    ckCamposDiseñoAcabado2.Enabled = false;
                    ckCamposDiseñoAcabado2.Checked = false;
                }
            }
            else
            {
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
                    habilitarValidaciones = false;

                    flowLayoutPanel.Controls.Add(panelCamposDiseñoAcabado2);
                    CargarTiposDiseñoAcabado(cboTiposDiseñosAcabados3);
                    CargarTiposDiseñoAcabado(cboTiposDiseñosAcabados4);

                    habilitarValidaciones = true;
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposDiseñoAcabado2);
                    cboTiposDiseñosAcabados3.DataSource = null;
                    cboTiposDiseñosAcabados4.DataSource = null;

                    ckDiseñoAcabadoAtributos3.Checked = false;
                    ckDiseñoAcabadoAtributos3.Enabled = false;
                    ckDiseñoAcabadoAtributos4.Checked = false;
                    ckDiseñoAcabadoAtributos4.Enabled = false;

                }
            }
            else
            {
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
                    habilitarValidaciones = false;

                    flowLayoutPanel.Controls.Add(panelCamposNTipos1);
                    CargarTiposNTipos(cboTiposNTipos1);
                    CargarTiposNTipos(cboTiposNTipos2);

                    habilitarValidaciones = true;
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposNTipos1);
                    ckTiposNTiposAtributos1.Checked = false;
                    ckTiposNTiposAtributos1.Enabled = false;
                    ckTiposNTiposAtributos2.Checked = false;
                    ckTiposNTiposAtributos2.Enabled = false;

                    ckCamposNTipos2.Enabled = false;
                    ckCamposNTipos2.Checked = false;

                }
            }
            else
            {
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
                    habilitarValidaciones = false;

                    flowLayoutPanel.Controls.Add(panelCamposNTipos2);
                    CargarTiposNTipos(cboTiposNTipos3);
                    CargarTiposNTipos(cboTiposNTipos4);

                    habilitarValidaciones = true;
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposNTipos2);
                    cboTiposNTipos3.DataSource = null;
                    cboTiposNTipos4.DataSource = null;

                    ckTiposNTiposAtributos3.Checked = false;
                    ckTiposNTiposAtributos3.Enabled = false;
                    ckTiposNTiposAtributos4.Checked = false;
                    ckTiposNTiposAtributos4.Enabled = false;
                }
            }
            else
            {
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
                    habilitarValidaciones = false;
                    flowLayoutPanel.Controls.Add(panelCamposVariosO1);
                    CargarTiposVariosO(cboTiposVariosO1);

                    habilitarValidaciones = true;
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposVariosO1);
                    ckVariosAtributos1.Checked = false;
                    ckVariosAtributos1.Enabled = false;

                    ckVariosO2.Enabled = false;
                    ckVariosO2.Checked = false;
                }
            }
            else
            {
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
                    habilitarValidaciones = false;

                    flowLayoutPanel.Controls.Add(panelCamposVariosO2);
                    CargarTiposVariosO(cboTiposVariosO2);

                    habilitarValidaciones = true;
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposVariosO2);
                    cboTiposVariosO2.DataSource = null;

                    ckVariosAtributos2.Checked = false;
                    ckVariosAtributos2.Enabled = false;
                }
            }
            else
            {
                ckVariosO2.Checked = false;
            }
        }

        //SELECCIONAR EL CAMPOS GENERAL
        private void ckGenerales_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox[] desacitvarcheks = {ckCaracteristicas1,ckCamposMedida1,ckCamposDiametros1,ckCamposFormas1,ckCamposEspesores1,ckCamposDiseñoAcabado1,ckCamposNTipos1
                ,ckVariosO1};

            if (ckCaracteristicas1.Checked == true || ckCaracteristicas2.Checked == true || ckCamposMedida1.Checked == true || ckCamposMedida2.Checked == true
    || ckCamposDiametros1.Checked == true || ckCamposDiametros2.Checked == true || ckCamposFormas1.Checked == true || ckCamposFormas2.Checked == true
    || ckCamposEspesores1.Checked == true || ckCamposEspesores2.Checked == true || ckCamposDiseñoAcabado1.Checked == true || ckCamposDiseñoAcabado2.Checked == true
    || ckCamposNTipos1.Checked == true || ckCamposNTipos2.Checked == true || ckVariosO1.Checked == true || ckVariosO2.Checked == true)
            {

                foreach(CheckBox ck in desacitvarcheks)
                {
                    ck.Enabled = false;
                    ck.Checked = false;
                }
                
                flowLayoutPanel.Controls.Clear();
                MessageBox.Show("El modelo solo puede tener dos definiciones, campo libre(general) o campos especificos.", "Validación del Sistema", MessageBoxButtons.OK);
                flowLayoutPanel.Controls.Add(panelCamposGeneral);
            }
            else
            {
                if (ckGenerales.Checked == true)
                {
                    foreach (CheckBox ck in desacitvarcheks)
                    {
                        ck.Enabled = false;
                    }

                    flowLayoutPanel.Controls.Add(panelCamposGeneral);
                }
                else
                {
                    flowLayoutPanel.Controls.Remove(panelCamposGeneral);

                    foreach (CheckBox ck in desacitvarcheks)
                    {
                        ck.Enabled = true;
                    }
                }
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


            ckCaracteristicaAtributo1.Checked = false;
            ckCaracteristicaAtributo2.Checked = false;
            ckCaracteristicaAtributo3.Checked = false;
            ckCaracteristicaAtributo4.Checked = false;
            ckMedidasAtributos1.Checked = false;
            ckMedidasAtributos2.Checked = false;
            ckMedidasAtributos3.Checked = false;
            ckMedidasAtributos4.Checked = false;
            ckDiametroAtributos1.Checked = false;
            ckDiametroAtributos2.Checked = false;
            ckDiametroAtributos3.Checked = false;
            ckDiametroAtributos4.Checked = false;
            ckFormasAtributos1.Checked = false;
            ckFormasAtributos2.Checked = false;
            ckFormasAtributos3.Checked = false;
            ckFormasAtributos4.Checked = false;
            ckEspesoresAtributos1.Checked = false;
            ckEspesoresAtributos2.Checked = false;
            ckEspesoresAtributos3.Checked = false;
            ckEspesoresAtributos4.Checked = false;
            ckDiseñoAcabadoAtributos1.Checked = false;
            ckDiseñoAcabadoAtributos2.Checked = false;
            ckDiseñoAcabadoAtributos3.Checked = false;
            ckDiseñoAcabadoAtributos4.Checked = false;
            ckTiposNTiposAtributos1.Checked = false;
            ckTiposNTiposAtributos2.Checked = false;
            ckTiposNTiposAtributos3.Checked = false;
            ckTiposNTiposAtributos4.Checked = false;
            ckVariosAtributos1.Checked = false;
            ckVariosAtributos2.Checked = false;
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
            DGV.Columns[0].Width = 80;
            DGV.Columns[1].Width = 80;
            DGV.Columns[2].Width = 100;
            DGV.Columns[3].Width = 220;
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
                    MessageBox.Show("Debe seleccionar un atributo valido en Caracteristicas.", "Validación del Sistema", MessageBoxButtons.OK);
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
                    MessageBox.Show("Debe seleccionar un atributo valido en Caracteristicas.", "Validación del Sistema", MessageBoxButtons.OK);
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
                    MessageBox.Show("Debe seleccionar un atributo valido en Medidas.", "Validación del Sistema", MessageBoxButtons.OK);
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
                    MessageBox.Show("Debe seleccionar un atributo valido en Medidas.", "Validación del Sistema", MessageBoxButtons.OK);
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
                    MessageBox.Show("Debe seleccionar un atributo valido en Diametros.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbdiametros1 == cbdiametros2 || cbdiametros1 == cbdiametros3 || cbdiametros1 == cbdiametros4
               || cbdiametros2 == cbdiametros1 || cbdiametros2 == cbdiametros3 || cbdiametros2 == cbdiametros4)
                {
                    MessageBox.Show("Los atributos no se pueden repetir.", "Validacion del Sistema", MessageBoxButtons.OK);
                    return;
                }

                if (cbdiametros1 == "" || cbdiametros2 == "")
                {
                    MessageBox.Show("Los campos no pueden estar vacios.", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
            }

            if (ckCamposDiametros2.Checked == true)
            {
                if (cbdiametros3 == "NO APLICA")
                {
                    MessageBox.Show("Debe seleccionar un atributo valido en Diametros.", "Validación del Sistema", MessageBoxButtons.OK);
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
                    MessageBox.Show("Debe seleccionar un atributo valido en Formas", "Validación del Sistema", MessageBoxButtons.OK);
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
                    MessageBox.Show("Debe seleccionar un atributo valido en Formas", "Validación del Sistema", MessageBoxButtons.OK);
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
                    MessageBox.Show("Debe seleccionar un atributo valido en Espesores", "Validación del Sistema", MessageBoxButtons.OK);
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
                    MessageBox.Show("Debe seleccionar un atributo valido en Espesores", "Validación del Sistema", MessageBoxButtons.OK);
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
                    MessageBox.Show("Debe seleccionar un atributo valido en Diseño Acabado.", "Validación del Sistema", MessageBoxButtons.OK);
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
                    MessageBox.Show("Debe seleccionar un atributo valido en Diseño Acabado.", "Validación del Sistema", MessageBoxButtons.OK);
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
                    MessageBox.Show("Debe seleccionar un atributo valido en N. y Tipos.", "Validación del Sistema", MessageBoxButtons.OK);
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
                    MessageBox.Show("Debe seleccionar un atributo valido en N. y Tipos.", "Validación del Sistema", MessageBoxButtons.OK);
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
                    MessageBox.Show("Debe seleccionar un atributo valido en Varios.", "Validación del Sistema", MessageBoxButtons.OK);
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


        //////////////////////////////////////////////////////////
        ////METODO PARA LAS VALIDACIONES TIEMPO REAL PARA CARACTERISTICAS
        
        public void ValidacionesCaracteristicas_TiempoReal(CheckBox ckGrupoCampCaracter1, CheckBox ckGrupoCampCaracter2, string Tipcaracteristica1, string Tipcaracteristica2, string Tipcaracteristica3, string Tipcaracteristica4
            , CheckBox ckcaractectributos1, CheckBox ckcaractectributos2, CheckBox ckcaracteatributos3, CheckBox ckcaracteatributos4)
        {
            if (habilitarValidaciones != false)
            {
                if (Tipcaracteristica1 == "NO APLICA")
                {
                    MessageBox.Show("Seleccione un Atributo Valido.", "Validación del Sistema", MessageBoxButtons.OK);

                    ckGrupoCampCaracter2.Enabled = false;
                    ckGrupoCampCaracter2.Checked = false;

                    ckcaractectributos1.Enabled = false;
                    ckcaractectributos1.Checked = false;

                    return;
                }
                else
                {
                    ckcaractectributos1.Enabled = true;
                }


                if (Tipcaracteristica2 == "NO APLICA")
                {
                    ckGrupoCampCaracter2.Enabled = false;
                    ckGrupoCampCaracter2.Checked = false;

                    ckcaractectributos2.Enabled = false;
                    ckcaractectributos2.Checked = false;
                }
                else
                {
                    ckGrupoCampCaracter2.Enabled = true;
                    ckcaractectributos2.Enabled = true;
                }


                if (ckGrupoCampCaracter2.Checked == true)
                {
                    if (Tipcaracteristica3 == "NO APLICA")
                    {
                        MessageBox.Show("Seleccione un atributo valido.", "Validación del Sistema", MessageBoxButtons.OK);
                        ckcaracteatributos3.Enabled = false;
                        ckcaracteatributos3.Checked = false;

                    }
                    else
                    {
                        ckcaracteatributos3.Enabled = true;
                    }

                    if (Tipcaracteristica4 == "NO APLICA")
                    {
                        ckcaracteatributos4.Enabled = false;
                        ckcaracteatributos4.Checked = false;
                    }
                    else
                    {
                        ckcaracteatributos4.Enabled = true;
                    }
                }
            }
        }
           
        
        //////////////////////////////////////////////////////////
        ////METODO PARA LAS VALIDACIONES TIEMPO REAL PARA MEDIDAS

        public void ValidacionesMedidas_TiempoReal(CheckBox ckGrupoCampMedid1, CheckBox ckGrupoCampMedid2, string Tipmedidas1, string Tipmedidas2, string Tipmedidas3, string Tipmedidas4
            ,CheckBox ckmedidatributos1, CheckBox ckmedidatributos2, CheckBox ckmedidatributos3, CheckBox ckmedidatributos4)
        {
            if(habilitarValidaciones != false)
            {
                if (ckGrupoCampMedid1.Checked == true)
                {
                    if (Tipmedidas1 == "NO APLICA")
                    {
                        MessageBox.Show("Seleccione un Atributo Valido.", "Validación del Sistema", MessageBoxButtons.OK);

                        ckGrupoCampMedid2.Enabled = false;
                        ckGrupoCampMedid2.Checked = false;

                        ckmedidatributos1.Enabled = false;
                        ckmedidatributos1.Checked = false;

                        return;
                    }
                    else
                    {
                        ckmedidatributos1.Enabled = true;
                    }

                    if(Tipmedidas2 == "NO APLICA")
                    {
                        ckGrupoCampMedid2.Enabled = false;
                        ckGrupoCampMedid2.Checked = false;

                        ckmedidatributos2.Enabled = false;
                        ckmedidatributos2.Checked = false;
                    }
                    else
                    {
                        ckGrupoCampMedid2.Enabled = true;
                        ckmedidatributos2.Enabled = true;
                    }
                }
                if (ckGrupoCampMedid2.Checked == true)
                {
                    if (Tipmedidas3 == "NO APLICA")
                    {
                        MessageBox.Show("Seleccione un Atributo Valido.", "Validación del Sistema", MessageBoxButtons.OK);

                        ckmedidatributos3.Enabled = false;
                        ckmedidatributos3.Checked = false;

                    }
                    else
                    {
                        ckmedidatributos3.Enabled = true;
                    }

                    if(Tipmedidas4 == "NO APLICA")
                    {
                        ckmedidatributos4.Enabled = false;
                        ckmedidatributos4.Checked = false;
                    }
                    else
                    {
                        ckmedidatributos4.Enabled = true;
                    }
                }
            } 
        }

        //////////////////////////////////////////////////////////
        ////METODO PARA LAS VALIDACIONES TIEMPO REAL PARA DIAMETROS

        public void ValidacionesDiametros_TiempoReal(CheckBox ckGrupoCampDiame1, CheckBox ckGrupoCampDiame2, string Tipdiame1, string Tipdiame2, string Tipdiame3, string Tipdiame4,CheckBox ckDiamtributos1
            , CheckBox ckDiamtributos2, CheckBox ckDiamtributos3, CheckBox ckDiamtributos4)
        {
            if (habilitarValidaciones != false)
            {
                if (ckGrupoCampDiame1.Checked == true)
                {
                    if (Tipdiame1 == "NO APLICA")
                    {
                        MessageBox.Show("Seleccione un Atributo Valido.", "Validación del Sistema", MessageBoxButtons.OK);

                        ckGrupoCampDiame2.Enabled = false;
                        ckGrupoCampDiame2.Checked = false;

                        ckDiamtributos1.Enabled = false;
                        ckDiamtributos1.Checked = false;

                        return;
                    }
                    else
                    {
                        ckGrupoCampDiame2.Enabled = true;
                        ckDiamtributos1.Enabled = true;
                    }

                    if (Tipdiame2 == "NO APLICA")
                    {
                        ckGrupoCampDiame2.Enabled = false;
                        ckGrupoCampDiame2.Checked = false;

                        ckDiamtributos2.Enabled = false;
                        ckDiamtributos2.Checked = false;
                    }
                    else
                    {
                        ckGrupoCampDiame2.Enabled = true;
                        ckDiamtributos2.Enabled = true;
                    }
                }

                if (ckGrupoCampDiame2.Checked == true)
                {
                    if (Tipdiame3 == "NO APLICA")
                    {
                        MessageBox.Show("Seleccione un Atributo Valido.", "Validación del Sistema", MessageBoxButtons.OK);

                        ckDiamtributos3.Enabled = false;
                        ckDiamtributos3.Checked = false;

                    }
                    else
                    {
                        ckDiamtributos3.Enabled = true;
                    }

                    if (Tipdiame4 == "NO APLICA")
                    {
                        ckDiamtributos4.Enabled = false;
                        ckDiamtributos4.Checked = false;
                    }
                    else
                    {
                        ckDiamtributos4.Enabled = true;
                    }
                }
            }
        }

        //////////////////////////////////////////////////////////
        ////METODO PARA LAS VALIDACIONES TIEMPO REAL PARA FORMAS

        public void ValidacionesFormas_TiempoReal(CheckBox ckGrupoCampForm1, CheckBox ckGrupoCampForm2, string Tipforma1, string Tipforma2, string Tipforma3, string Tipforma4,CheckBox ckFormtributos1
            , CheckBox ckFormtributos2, CheckBox ckFormtributos3, CheckBox ckFormtributos4)
        {
            if (habilitarValidaciones != false)
            {
                if (ckGrupoCampForm1.Checked == true)
                {
                    if (Tipforma1 == "NO APLICA")
                    {
                        MessageBox.Show("Seleccione un Atributo Valido.", "Validación del Sistema", MessageBoxButtons.OK);

                        ckGrupoCampForm2.Enabled = false;
                        ckGrupoCampForm2.Checked = false;

                        ckFormtributos1.Enabled = false;
                        ckFormtributos1.Checked = false;

                        return;
                    }
                    else
                    {
                        ckGrupoCampForm2.Enabled = true;
                        ckFormtributos1.Enabled = true;
                    }

                    if (Tipforma2 == "NO APLICA")
                    {
                        ckGrupoCampForm2.Enabled = false;
                        ckGrupoCampForm2.Checked = false;

                        ckFormtributos2.Enabled = false;
                        ckFormtributos2.Checked = false;
                    }
                    else
                    {
                        ckGrupoCampForm2.Enabled = true;
                        ckFormtributos2.Enabled = true;
                    }
                }

                if (ckGrupoCampForm2.Checked == true)
                {
                    if (Tipforma3 == "NO APLICA")
                    {
                        MessageBox.Show("Seleccione un Atributo Valido.", "Validación del Sistema", MessageBoxButtons.OK);

                        ckFormtributos3.Enabled = false;
                        ckFormtributos3.Checked = false;
                    }
                    else
                    {
                        ckFormtributos3.Enabled = true;
                    }

                    if (Tipforma4 == "NO APLICA")
                    {
                        ckFormtributos4.Enabled = false;
                        ckFormtributos4.Checked = false;
                    }
                    else
                    {
                        ckFormtributos4.Enabled = true;
                    }
                }
            }
        }
       
        //////////////////////////////////////////////////////////
        ////METODO PARA LAS VALIDACIONES TIEMPO REAL PARA ESPESORES

        public void ValidacionesEspesores_TiempoReal(CheckBox ckGrupoCampEspe1, CheckBox ckGrupoCampEspe2, string Tipespe1, string Tipespe2, string Tipespe3, string Tipespe4,CheckBox ckEsptributos1
            , CheckBox ckEsptributos2, CheckBox ckEsptributos3, CheckBox ckEsptributos4)
        {
            if (habilitarValidaciones != false)
            {
                if (ckGrupoCampEspe1.Checked == true)
                {
                    if (Tipespe1 == "NO APLICA")
                    {
                        MessageBox.Show("Seleccione un Atributo Valido.", "Validación del Sistema", MessageBoxButtons.OK);

                        ckGrupoCampEspe2.Enabled = false;
                        ckGrupoCampEspe2.Checked = false;

                        ckEsptributos1.Enabled = false;
                        ckEsptributos1.Checked = false;

                        return;

                    }
                    else
                    {
                        ckGrupoCampEspe2.Enabled = true;
                        ckEsptributos1.Enabled = true;
                    }

                    if (Tipespe2 == "NO APLICA")
                    {
                        ckGrupoCampEspe2.Enabled = false;
                        ckGrupoCampEspe2.Checked = false;


                        ckEsptributos2.Enabled = false;
                        ckEsptributos2.Checked = false;
                    }
                    else
                    {
                        ckGrupoCampEspe2.Enabled = true;
                        ckEsptributos2.Enabled = true;
                    }
                }

                if (ckGrupoCampEspe2.Checked == true)
                {
                    if (Tipespe3 == "NO APLICA")
                    {
                        MessageBox.Show("Seleccione un Atributo Valido.", "Validación del Sistema", MessageBoxButtons.OK);

                        ckEsptributos3.Enabled = false;
                        ckEsptributos3.Checked = false;
                    }
                    else
                    {
                        ckEsptributos3.Enabled = true;
                    }

                    if (Tipespe4 == "NO APLICA")
                    {
                        ckEsptributos4.Enabled = false;
                        ckEsptributos4.Checked = false;
                    }
                    else
                    {
                        ckEsptributos4.Enabled = true;
                    }
                }
            }
        }

        //////////////////////////////////////////////////////////
        ////METODO PARA LAS VALIDACIONES TIEMPO REAL PARA DISEÑO ACABADO

        public void ValidacionesDiseñoAcabado_TiempoReal(CheckBox ckGrupoCampDise1, CheckBox ckGrupoCampDise2, string Tipdiseacab1, string Tipdiseacab2, string Tipdiseacab3, string Tipdiseacab4
            ,CheckBox ckDiseAcaAtributos1, CheckBox ckDiseAcaAtributos2, CheckBox ckDiseAcaAtributos3, CheckBox ckDiseAcaAtributos4)
        {
            if (habilitarValidaciones != false)
            {
                if (ckGrupoCampDise1.Checked == true)
                {
                    if (Tipdiseacab1 == "NO APLICA")
                    {
                        MessageBox.Show("Seleccione un Atributo Valido.", "Validación del Sistema", MessageBoxButtons.OK);

                        ckGrupoCampDise2.Enabled = false;
                        ckGrupoCampDise2.Checked = false;

                        ckDiseAcaAtributos1.Enabled = false;
                        ckDiseAcaAtributos1.Checked = false;

                        return;
                    }
                    else
                    {
                        ckGrupoCampDise2.Enabled = true;
                        ckDiseAcaAtributos1.Enabled = true;
                    }

                    if (Tipdiseacab2 == "NO APLICA")
                    {
                        ckGrupoCampDise2.Enabled = false;
                        ckGrupoCampDise2.Checked = false;

                        ckDiseAcaAtributos2.Enabled = false;
                        ckDiseAcaAtributos2.Checked = false;
                    }
                    else
                    {
                        ckGrupoCampDise2.Enabled = true;
                        ckDiseAcaAtributos2.Enabled = true;
                    }
                }
                if (ckGrupoCampDise2.Checked == true)
                {
                    if (Tipdiseacab3 == "NO APLICA")
                    {
                        MessageBox.Show("Seleccione un Atributo Valido.", "Validación del Sistema", MessageBoxButtons.OK);

                        ckDiseAcaAtributos3.Enabled = false;
                        ckDiseAcaAtributos3.Checked = false;
                    }
                    else
                    {
                        ckDiseAcaAtributos3.Enabled = true;
                    }

                    if (Tipdiseacab4 == "NO APLICA")
                    {
                        ckDiseAcaAtributos4.Enabled = false;
                        ckDiseAcaAtributos4.Checked = false;
                    }
                    else
                    {
                        ckDiseAcaAtributos4.Enabled = true;
                    }
                }
            }
        }

        //////////////////////////////////////////////////////////
        ////METODO PARA LAS VALIDACIONES TIEMPO REAL PARA NTIPOS

        public void ValidacionesNTipos_TiempoReal(CheckBox ckGrupoCampNtip1, CheckBox ckGrupoCampNtip2, string Tipntipo1, string Tipntipo2, string Tipntipo3, string Tipntipo4,CheckBox ckNtipAtributos1
            , CheckBox ckNtipAtributos2, CheckBox ckNtipAtributos3, CheckBox ckNtipAtributos4)
        {
           if(habilitarValidaciones != false)
            {
                if (ckGrupoCampNtip1.Checked == true)
                {
                    if (Tipntipo1 == "NO APLICA")
                    {
                        MessageBox.Show("Seleccione un Atributo Valido.", "Validación del Sistema", MessageBoxButtons.OK);

                        ckGrupoCampNtip2.Enabled = false;
                        ckGrupoCampNtip2.Checked = false;

                        ckNtipAtributos1.Enabled = false;
                        ckNtipAtributos1.Checked = false;

                        return;
                    }
                    else
                    {
                        ckGrupoCampNtip2.Enabled = true;
                        ckNtipAtributos1.Enabled = true;
                    }

                    if(Tipntipo2 == "NO APLICA")
                    {
                        ckGrupoCampNtip2.Enabled = false;
                        ckGrupoCampNtip2.Checked = false;

                        ckNtipAtributos2.Enabled = false;
                        ckNtipAtributos2.Checked = false;
                    }
                    else
                    {
                        ckGrupoCampNtip2.Enabled = true;
                        ckNtipAtributos2.Enabled = true;
                    }
                }

                if (ckGrupoCampNtip2.Checked == true)
                {
                    if (Tipntipo3 == "NO APLICA")
                    {
                        MessageBox.Show("Seleccione un Atributo Valido.", "Validación del Sistema", MessageBoxButtons.OK);

                        ckNtipAtributos3.Enabled = false;
                        ckNtipAtributos3.Checked = false;
                    }
                    else
                    {
                        ckNtipAtributos3.Enabled = true;
                    }

                    if(Tipntipo4 == "NO APLICA")
                    {
                        ckNtipAtributos4.Enabled = false;
                        ckNtipAtributos4.Checked = false;
                    }
                    else
                    {
                        ckNtipAtributos4.Enabled = true;
                    }
                }
            }
        }

        //////////////////////////////////////////////////////////
        ////METODO PARA LAS VALIDACIONES TIEMPO REAL PARA VARIOS

        public void ValidacionesVarios_TiempoReal(CheckBox ckGrupoCampVari1, CheckBox ckGrupoCampVari2, string TipVario1, string TipVario2,CheckBox ckVarioAtributos1, CheckBox ckVarioAtributos2)
        {
            if (habilitarValidaciones != false)
            {
                if (ckGrupoCampVari1.Checked == true)
                {
                    if (TipVario1 == "NO APLICA")
                    {
                        
                        MessageBox.Show("Seleccione un Atributo Valido.", "Validación Del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ckGrupoCampVari2.Checked = false;
                        ckGrupoCampVari2.Enabled = false;

                        ckVarioAtributos1.Enabled = false;
                        ckVarioAtributos1.Checked = false;

                        return;
                    }
                    else
                    {
                        ckGrupoCampVari2.Enabled = true;
                        ckVarioAtributos1.Enabled = true;
                    }
                }

                if(ckGrupoCampVari2.Checked == true)
                {
                    if(TipVario2 == "NO APLICA")
                    {
                        ckVarioAtributos2.Enabled = false;
                        ckVarioAtributos2.Enabled = false;
                    }
                    else
                    {
                        ckVarioAtributos2.Enabled = true;
                    }
                }
            }
        }

        ////////////////////////////////////
        //EVENTOS PARA LAS VALIDACIONES EN TIEMPO REAL DE CARACTERISTICAS
        private void cboTipoCaracteristicas1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesCaracteristicas_TiempoReal(ckCaracteristicas1, ckCaracteristicas2, cboTipoCaracteristicas1.Text, cboTipoCaracteristicas2.Text, cboTipoCaracteristicas3.Text, cboTipoCaracteristicas4.Text
               , ckCaracteristicaAtributo1, ckCaracteristicaAtributo2, ckCaracteristicaAtributo3, ckCaracteristicaAtributo4);
        }
        private void cboTipoCaracteristicas2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesCaracteristicas_TiempoReal(ckCaracteristicas1, ckCaracteristicas2, cboTipoCaracteristicas1.Text, cboTipoCaracteristicas2.Text, cboTipoCaracteristicas3.Text, cboTipoCaracteristicas4.Text
               , ckCaracteristicaAtributo1, ckCaracteristicaAtributo2, ckCaracteristicaAtributo3, ckCaracteristicaAtributo4);
        }

        private void cboTipoCaracteristicas3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesCaracteristicas_TiempoReal(ckCaracteristicas1, ckCaracteristicas2, cboTipoCaracteristicas1.Text, cboTipoCaracteristicas2.Text, cboTipoCaracteristicas3.Text, cboTipoCaracteristicas4.Text
               , ckCaracteristicaAtributo1, ckCaracteristicaAtributo2, ckCaracteristicaAtributo3, ckCaracteristicaAtributo4);
        }

        private void cboTipoCaracteristicas4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesCaracteristicas_TiempoReal(ckCaracteristicas1, ckCaracteristicas2, cboTipoCaracteristicas1.Text, cboTipoCaracteristicas2.Text, cboTipoCaracteristicas3.Text, cboTipoCaracteristicas4.Text
               , ckCaracteristicaAtributo1, ckCaracteristicaAtributo2, ckCaracteristicaAtributo3, ckCaracteristicaAtributo4);
        }

        ////////////////////////////////////
        //EVENTOS PARA LAS VALIDACIONES EN TIEMPO REAL DE MEDIDAS
        private void cboTipoMedida1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesMedidas_TiempoReal(ckCamposMedida1, ckCamposMedida2, cboTipoMedida1.Text, cboTipoMedida2.Text, cboTipoMedida3.Text, cboTipoMedida4.Text,ckMedidasAtributos1, ckMedidasAtributos2, ckMedidasAtributos3
                , ckMedidasAtributos4);

        }
        private void cboTipoMedida2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesMedidas_TiempoReal(ckCamposMedida1, ckCamposMedida2, cboTipoMedida1.Text, cboTipoMedida2.Text, cboTipoMedida3.Text, cboTipoMedida4.Text, ckMedidasAtributos1, ckMedidasAtributos2, ckMedidasAtributos3
                , ckMedidasAtributos4);
        }

        private void cboTipoMedida3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesMedidas_TiempoReal(ckCamposMedida1, ckCamposMedida2, cboTipoMedida1.Text, cboTipoMedida2.Text, cboTipoMedida3.Text, cboTipoMedida4.Text, ckMedidasAtributos1, ckMedidasAtributos2, ckMedidasAtributos3
                , ckMedidasAtributos4);
        }

        private void cboTipoMedida4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesMedidas_TiempoReal(ckCamposMedida1, ckCamposMedida2, cboTipoMedida1.Text, cboTipoMedida2.Text, cboTipoMedida3.Text, cboTipoMedida4.Text, ckMedidasAtributos1, ckMedidasAtributos2, ckMedidasAtributos3
               , ckMedidasAtributos4);
        }

        ////////////////////////////////////
        //EVENTOS PARA LAS VALIDACIONES EN TIEMPO REAL DE DIAMETROS
        private void cboTiposDiametros1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesDiametros_TiempoReal(ckCamposDiametros1, ckCamposDiametros2, cboTiposDiametros1.Text, cboTiposDiametros2.Text, cboTiposDiametros3.Text, cboTiposDiametros4.Text, ckDiametroAtributos1, ckDiametroAtributos2
              , ckDiametroAtributos3, ckDiametroAtributos4);
        }

        private void cboTiposDiametros2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesDiametros_TiempoReal(ckCamposDiametros1, ckCamposDiametros2, cboTiposDiametros1.Text, cboTiposDiametros2.Text, cboTiposDiametros3.Text, cboTiposDiametros4.Text,ckDiametroAtributos1, ckDiametroAtributos2
                , ckDiametroAtributos3, ckDiametroAtributos4);
        }


        private void cboTiposDiametros3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesDiametros_TiempoReal(ckCamposDiametros1, ckCamposDiametros2, cboTiposDiametros1.Text, cboTiposDiametros2.Text, cboTiposDiametros3.Text, cboTiposDiametros4.Text, ckDiametroAtributos1, ckDiametroAtributos2
              , ckDiametroAtributos3, ckDiametroAtributos4);
        }

        private void cboTiposDiametros4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesDiametros_TiempoReal(ckCamposDiametros1, ckCamposDiametros2, cboTiposDiametros1.Text, cboTiposDiametros2.Text, cboTiposDiametros3.Text, cboTiposDiametros4.Text, ckDiametroAtributos1, ckDiametroAtributos2
                          , ckDiametroAtributos3, ckDiametroAtributos4);
        }


        ////////////////////////////////////
        //EVENTOS PARA LAS VALIDACIONES EN TIEMPO REAL DE FORMAS

        private void cboTiposFormas1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesFormas_TiempoReal(ckCamposFormas1, ckCamposFormas2, cboTiposFormas1.Text, cboTiposFormas2.Text, cboTiposFormas3.Text, cboTiposFormas4.Text, ckFormasAtributos1, ckFormasAtributos2, ckFormasAtributos3
               , ckFormasAtributos4);
        }

        private void cboTiposFormas2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesFormas_TiempoReal(ckCamposFormas1, ckCamposFormas2, cboTiposFormas1.Text, cboTiposFormas2.Text, cboTiposFormas3.Text, cboTiposFormas4.Text,ckFormasAtributos1, ckFormasAtributos2, ckFormasAtributos3
                , ckFormasAtributos4);
        }

        private void cboTiposFormas3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesFormas_TiempoReal(ckCamposFormas1, ckCamposFormas2, cboTiposFormas1.Text, cboTiposFormas2.Text, cboTiposFormas3.Text, cboTiposFormas4.Text, ckFormasAtributos1, ckFormasAtributos2, ckFormasAtributos3
               , ckFormasAtributos4);
        }

        private void cboTiposFormas4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesFormas_TiempoReal(ckCamposFormas1, ckCamposFormas2, cboTiposFormas1.Text, cboTiposFormas2.Text, cboTiposFormas3.Text, cboTiposFormas4.Text, ckFormasAtributos1, ckFormasAtributos2, ckFormasAtributos3
                            , ckFormasAtributos4);
        }

        ////////////////////////////////////
        //EVENTOS PARA LAS VALIDACIONES EN TIEMPO REAL DE ESPESORES
        private void cbooTipoEspesores1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesEspesores_TiempoReal(ckCamposEspesores1, ckCamposEspesores2, cbooTipoEspesores1.Text, cbooTipoEspesores2.Text, cbooTipoEspesores3.Text, cbooTipoEspesores4.Text, ckEspesoresAtributos1, ckEspesoresAtributos2
                , ckEspesoresAtributos3, ckEspesoresAtributos4);
        }

        private void cbooTipoEspesores2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesEspesores_TiempoReal(ckCamposEspesores1, ckCamposEspesores2, cbooTipoEspesores1.Text, cbooTipoEspesores2.Text, cbooTipoEspesores3.Text, cbooTipoEspesores4.Text,ckEspesoresAtributos1, ckEspesoresAtributos2
                , ckEspesoresAtributos3, ckEspesoresAtributos4);
        }

        private void cbooTipoEspesores3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesEspesores_TiempoReal(ckCamposEspesores1, ckCamposEspesores2, cbooTipoEspesores1.Text, cbooTipoEspesores2.Text, cbooTipoEspesores3.Text, cbooTipoEspesores4.Text, ckEspesoresAtributos1, ckEspesoresAtributos2
                , ckEspesoresAtributos3, ckEspesoresAtributos4);
        }

        private void cbooTipoEspesores4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesEspesores_TiempoReal(ckCamposEspesores1, ckCamposEspesores2, cbooTipoEspesores1.Text, cbooTipoEspesores2.Text, cbooTipoEspesores3.Text, cbooTipoEspesores4.Text, ckEspesoresAtributos1, ckEspesoresAtributos2
                           , ckEspesoresAtributos3, ckEspesoresAtributos4);
        }

        ////////////////////////////////////
        //EVENTOS PARA LAS VALIDACIONES EN TIEMPO REAL DE DISEÑO ACABADO
        private void cboTiposDiseñosAcabados1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesDiseñoAcabado_TiempoReal(ckCamposDiseñoAcabado1, ckCamposDiseñoAcabado2, cboTiposDiseñosAcabados1.Text, cboTiposDiseñosAcabados2.Text, cboTiposDiseñosAcabados3.Text, cboTiposDiseñosAcabados4.Text
                , ckDiseñoAcabadoAtributos1, ckDiseñoAcabadoAtributos2, ckDiseñoAcabadoAtributos3, ckDiseñoAcabadoAtributos4);
        }

        private void cboTiposDiseñosAcabados2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesDiseñoAcabado_TiempoReal(ckCamposDiseñoAcabado1, ckCamposDiseñoAcabado2, cboTiposDiseñosAcabados1.Text, cboTiposDiseñosAcabados2.Text, cboTiposDiseñosAcabados3.Text, cboTiposDiseñosAcabados4.Text
               , ckDiseñoAcabadoAtributos1, ckDiseñoAcabadoAtributos2, ckDiseñoAcabadoAtributos3, ckDiseñoAcabadoAtributos4);
        }

        private void cboTiposDiseñosAcabados3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesDiseñoAcabado_TiempoReal(ckCamposDiseñoAcabado1, ckCamposDiseñoAcabado2, cboTiposDiseñosAcabados1.Text, cboTiposDiseñosAcabados2.Text, cboTiposDiseñosAcabados3.Text, cboTiposDiseñosAcabados4.Text
              , ckDiseñoAcabadoAtributos1, ckDiseñoAcabadoAtributos2, ckDiseñoAcabadoAtributos3, ckDiseñoAcabadoAtributos4);
        }

        private void cboTiposDiseñosAcabados4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesDiseñoAcabado_TiempoReal(ckCamposDiseñoAcabado1, ckCamposDiseñoAcabado2, cboTiposDiseñosAcabados1.Text, cboTiposDiseñosAcabados2.Text, cboTiposDiseñosAcabados3.Text, cboTiposDiseñosAcabados4.Text
               , ckDiseñoAcabadoAtributos1, ckDiseñoAcabadoAtributos2, ckDiseñoAcabadoAtributos3, ckDiseñoAcabadoAtributos4);
        }

        ////////////////////////////////////
        //EVENTOS PARA LAS VALIDACIONES EN TIEMPO REAL DE N TIPOS

        private void cboTiposNTipos1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesNTipos_TiempoReal(ckCamposNTipos1, ckCamposNTipos2, cboTiposNTipos1.Text, cboTiposNTipos2.Text, cboTiposNTipos3.Text, cboTiposNTipos4.Text, ckTiposNTiposAtributos1, ckTiposNTiposAtributos2, ckTiposNTiposAtributos3
               , ckTiposNTiposAtributos4);
        }

        private void cboTiposNTipos2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesNTipos_TiempoReal(ckCamposNTipos1, ckCamposNTipos2, cboTiposNTipos1.Text, cboTiposNTipos2.Text, cboTiposNTipos3.Text, cboTiposNTipos4.Text,ckTiposNTiposAtributos1, ckTiposNTiposAtributos2, ckTiposNTiposAtributos3
                , ckTiposNTiposAtributos4);
        }

        private void cboTiposNTipos3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesNTipos_TiempoReal(ckCamposNTipos1, ckCamposNTipos2, cboTiposNTipos1.Text, cboTiposNTipos2.Text, cboTiposNTipos3.Text, cboTiposNTipos4.Text, ckTiposNTiposAtributos1, ckTiposNTiposAtributos2, ckTiposNTiposAtributos3
               , ckTiposNTiposAtributos4);
        }

        private void cboTiposNTipos4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesNTipos_TiempoReal(ckCamposNTipos1, ckCamposNTipos2, cboTiposNTipos1.Text, cboTiposNTipos2.Text, cboTiposNTipos3.Text, cboTiposNTipos4.Text, ckTiposNTiposAtributos1, ckTiposNTiposAtributos2, ckTiposNTiposAtributos3
                           , ckTiposNTiposAtributos4);
        }

        ////////////////////////////////////
        //EVENTOS PARA LAS VALIDACIONES EN TIEMPO REAL DE VARIOS

        private void cboTiposVariosO1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesVarios_TiempoReal(ckVariosO1, ckVariosO2, cboTiposVariosO1.Text, cboTiposVariosO2.Text,ckVariosAtributos1,ckVariosAtributos2);
        }

        private void cboTiposVariosO2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidacionesVarios_TiempoReal(ckVariosO1, ckVariosO2, cboTiposVariosO1.Text, cboTiposVariosO2.Text, ckVariosAtributos1, ckVariosAtributos2);
        }

        //////////////////////////////////////////---------------------------------------------------------------------------
        ////------------------------------------------------
        ///IMPLEMENTACIÓN PARA LA ESTRUCTURA DEL NOMBRE DEL PRODUCTO

        //METODO PARA GUARDAR LOS NOMBRES DE LOS TIPOS QUE ESTEN MARCADOS CON LOS CHECKBOX    
        private void AgregarCk_NombreTipos(int idmodelo, CheckBox ckcaracteristica1, CheckBox ckcaracteristica2, CheckBox ckcaracteristica3, CheckBox ckcaracteristica4, CheckBox ckmedidas1
            , CheckBox ckmedidas2, CheckBox ckmedidas3, CheckBox ckmedidas4, CheckBox ckdiametros1, CheckBox ckdiametros2, CheckBox ckdiametros3, CheckBox ckdiametros4, CheckBox ckformas1
            , CheckBox ckformas2, CheckBox ckformas3, CheckBox ckformas4, CheckBox ckespesores1, CheckBox ckespesores2, CheckBox ckespesores3, CheckBox ckespesores4, CheckBox ckdiseñoacab1, CheckBox ckdiseñoacab2
            , CheckBox ckdiseñoacab3, CheckBox ckdiseñoacab4, CheckBox ckntipos1, CheckBox ckntipos2, CheckBox ckntipos3, CheckBox ckntipos4, CheckBox ckvarios1, CheckBox ckvarios2)
        {
            try
            {
                //INGRESO DE CK EN 0
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Modelos_InsertarXCkAtributos", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idmodelo", idmodelo);

                //CARACTERISTICAS
                if (ckcaracteristica1.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipocaracteristica1", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipocaracteristica1", 0);

                }

                if (ckcaracteristica2.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipocaracteristica2", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipocaracteristica2", 0);
                }

                if (ckcaracteristica3.Checked == true)
                {


                    cmd.Parameters.AddWithValue("@cktipocaracteristica3", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipocaracteristica3", 0);
                }

                if (ckcaracteristica4.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipocaracteristica4", 1);
                }

                else
                {
                    cmd.Parameters.AddWithValue("@cktipocaracteristica4", 0);
                }
                //MEDIDAS
                if (ckmedidas1.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipomedidas1", 1);

                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipomedidas1", 0);
                }

                if (ckmedidas2.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipomedidas2", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipomedidas2", 0);
                }
                if (ckmedidas3.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipomedidas3", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipomedidas3", 0);
                }

                if (ckmedidas4.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipomedidas4", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipomedidas4", 0);
                }
                //DIAMETRO
                if (ckdiametros1.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipodiametro1", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipodiametro1", 0);
                }

                if (ckdiametros2.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipodiametro2", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipodiametro2", 0);
                }
                if (ckdiametros3.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipodiametro3", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipodiametro3", 0);
                }

                if (ckdiametros4.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipodiametro4", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipodiametro4", 0);
                }

                //FORMA
                if (ckformas1.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipoforma1", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipoforma1", 0);
                }

                if (ckformas2.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipoforma2", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipoforma2", 0);
                }

                if (ckformas3.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipoforma3", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipoforma3", 0);
                }

                if (ckformas4.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipoforma4", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipoforma4", 0);
                }
                //ESPESORES
                if (ckespesores1.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipoespesores1", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipoespesores1", 0);
                }

                if (ckespesores2.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipoespesores2", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipoespesores2", 0);
                }

                if (ckespesores3.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipoespesores3", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipoespesores3", 0);
                }

                if (ckespesores4.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipoespesores4", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipoespesores4", 0);
                }
                //DISEÑO ACABADO
                if (ckdiseñoacab1.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipodiseñoacabado1", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipodiseñoacabado1", 0);
                }

                if (ckdiseñoacab2.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipodiseñoacabado2", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipodiseñoacabado2", 0);
                }

                if (ckdiseñoacab3.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipodiseñoacabado3", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipodiseñoacabado3", 0);
                }

                if (ckdiseñoacab4.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipodiseñoacabado4", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipodiseñoacabado4", 0);
                }
                //TIPOS N TIPOS
                if (ckntipos1.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipontipos1", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipontipos1", 0);
                }

                if (ckntipos2.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipontipos2", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipontipos2", 0);
                }

                if (ckntipos3.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipontipos3", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipontipos3", 0);
                }

                if (ckntipos4.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipontipos4", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipontipos4", 0);
                }
                //VARIOS
                if (ckvarios1.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipovariosO1", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipovariosO1", 0);
                }

                if (ckvarios2.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipovariosO2", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipovariosO2", 0);
                }
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //METODO PARA CARGAR LOS GRUPOS DE CAMPOS PREDETERMINADOS SEGUN EL MODELO SELECCIONADO
        public void CargarGrupoCamposPredeterminados(int idmodelo,DataGridView DGVGrupoCampos,FlowLayoutPanel agregargruposcampos,CheckBox ckcampcaracteristica1, CheckBox ckcampcaracteristica2
            , CheckBox ckcampmedidas1, CheckBox ckcampmedidas2,CheckBox ckcampdiametros1, CheckBox ckcampdiametros2 
            , CheckBox ckcampformas1, CheckBox ckcampformas2, CheckBox ckcampespesores1, CheckBox ckcampespesores2,CheckBox ckcampdiseñoaca1
            , CheckBox ckcampdiseñoaca2,CheckBox ckcampntipos1, CheckBox ckcampntipos2,CheckBox ckcampvarios1, CheckBox ckcampvarios2
            , CheckBox ckcampgenerales,Panel caracteristica1, Panel caracteristica2, Panel medidas1, Panel medidas2, Panel diametros1, Panel diametros2,Panel formas1, Panel formas2,Panel espesores1
            , Panel espesores2, Panel diseñoacabado1, Panel diseñoacabado2, Panel ntipos1, Panel ntipos2, Panel varios1, Panel varios2, Panel generales)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Modelos_CargarGrupoCamposPredeterminados", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idmodelo", idmodelo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                DGVGrupoCampos.DataSource = dt;
                con.Close();


                //CARACTERISTICAS - 1
                int CampCaracteristicas1 = Convert.ToInt32(DGVGrupoCampos.SelectedCells[1].Value.ToString());
                if (CampCaracteristicas1 == 1)
                {
                    agregargruposcampos.Controls.Add(caracteristica1);
                    CargarTiposCaracteriticas(cboTipoCaracteristicas1);
                    CargarTiposCaracteriticas(cboTipoCaracteristicas2);

                }
                else
                {
                    agregargruposcampos.Controls.Remove(caracteristica1);

                }

                //CARACTERISTICAS - 2
                int CampCaracteristicas2 = Convert.ToInt32(DGVGrupoCampos.SelectedCells[2].Value.ToString());
                if (CampCaracteristicas2 == 1)
                {
                    agregargruposcampos.Controls.Add(caracteristica2);
                    CargarTiposCaracteriticas(cboTipoCaracteristicas3);
                    CargarTiposCaracteriticas(cboTipoCaracteristicas4);
                }
                else
                {
                    agregargruposcampos.Controls.Remove(caracteristica2);

                }

                //MEDIDAS - 1
                int CampMedidas1 = Convert.ToInt32(DGVGrupoCampos.SelectedCells[3].Value.ToString());
                if (CampMedidas1 == 1)
                {
                    agregargruposcampos.Controls.Add(medidas1);
                    CargarTiposMedidas(cboTipoMedida1);
                    CargarTiposMedidas(cboTipoMedida2);
                }
                else
                {
                    agregargruposcampos.Controls.Remove(medidas1);

                }

                //MEDIDAS - 2
                int CampMedidas2 = Convert.ToInt32(DGVGrupoCampos.SelectedCells[4].Value.ToString());
                if (CampMedidas2 == 1)
                {
                    agregargruposcampos.Controls.Add(medidas2);
                    CargarTiposMedidas(cboTipoMedida3);
                    CargarTiposMedidas(cboTipoMedida4);
                }
                else
                {
                    agregargruposcampos.Controls.Remove(medidas2);

                }

                //DIAMETROS - 1
                int CampDiametros1 = Convert.ToInt32(DGVGrupoCampos.SelectedCells[5].Value.ToString());
                if (CampDiametros1 == 1)
                {
                    agregargruposcampos.Controls.Add(diametros1);
                    CargarTiposDiametros(cboTiposDiametros1);
                    CargarTiposDiametros(cboTiposDiametros2);

                }
                else
                {
                    agregargruposcampos.Controls.Remove(diametros1);

                }

                //DIAMETROS - 2
                int CampDiametros2 = Convert.ToInt32(DGVGrupoCampos.SelectedCells[6].Value.ToString());
                if (CampDiametros2 == 1)
                {
                    agregargruposcampos.Controls.Add(diametros2);
                    CargarTiposDiametros(cboTiposDiametros3);
                    CargarTiposDiametros(cboTiposDiametros4);
                }
                else
                {
                    agregargruposcampos.Controls.Remove(diametros2);

                }

                //FORMAS - 1
                int CampFormas1 = Convert.ToInt32(DGVGrupoCampos.SelectedCells[7].Value.ToString());
                if (CampFormas1 == 1)
                {
                    agregargruposcampos.Controls.Add(formas1);
                    CargarTiposFormas(cboTiposFormas1);
                    CargarTiposFormas(cboTiposFormas2);

                }
                else
                {
                    agregargruposcampos.Controls.Remove(formas1);

                }

                //FORMAS - 2
                int CampFormas2 = Convert.ToInt32(DGVGrupoCampos.SelectedCells[8].Value.ToString());
                if (CampFormas2 == 1)
                {
                    agregargruposcampos.Controls.Add(formas2);
                    CargarTiposFormas(cboTiposFormas3);
                    CargarTiposFormas(cboTiposFormas4);
                }
                else
                {
                    agregargruposcampos.Controls.Remove(formas2);

                }

                //ESPESORES - 1
                int CampEspesores1 = Convert.ToInt32(DGVGrupoCampos.SelectedCells[9].Value.ToString());
                if (CampEspesores1 == 1)
                {
                    agregargruposcampos.Controls.Add(espesores1);
                    CargarTiposEspesores(cbooTipoEspesores1);
                    CargarTiposEspesores(cbooTipoEspesores2);

                }
                else
                {
                    agregargruposcampos.Controls.Remove(espesores1);

                }

                //ESPESORES - 2
                int CampEspesores2 = Convert.ToInt32(DGVGrupoCampos.SelectedCells[10].Value.ToString());
                if (CampEspesores2 == 1)
                {
                    agregargruposcampos.Controls.Add(espesores2);
                    CargarTiposEspesores(cbooTipoEspesores3);
                    CargarTiposEspesores(cbooTipoEspesores4);

                }
                else
                {
                    agregargruposcampos.Controls.Remove(espesores2);

                }

                //DISEÑO Y ACABADOS - 1
                int CampDiseñoAcabado1 = Convert.ToInt32(DGVGrupoCampos.SelectedCells[11].Value.ToString());
                if (CampDiseñoAcabado1 == 1)
                {
                    agregargruposcampos.Controls.Add(diseñoacabado1);
                    CargarTiposDiseñoAcabado(cboTiposDiseñosAcabados1);
                    CargarTiposDiseñoAcabado(cboTiposDiseñosAcabados2);

                }
                else
                {
                    agregargruposcampos.Controls.Remove(diseñoacabado1);

                }

                //DISEÑO Y ACABADOS - 2
                int CampDiseñoAcabado2 = Convert.ToInt32(DGVGrupoCampos.SelectedCells[12].Value.ToString());
                if (CampDiseñoAcabado2 == 1)
                {
                    agregargruposcampos.Controls.Add(diseñoacabado2);
                    CargarTiposDiseñoAcabado(cboTiposDiseñosAcabados3);
                    CargarTiposDiseñoAcabado(cboTiposDiseñosAcabados4);

                }
                else
                {
                    agregargruposcampos.Controls.Remove(diseñoacabado2);

                }

                //NUMEROS Y TIPOS - 1
                int CampNTipos1 = Convert.ToInt32(DGVGrupoCampos.SelectedCells[13].Value.ToString());
                if (CampNTipos1 == 1)
                {
                    agregargruposcampos.Controls.Add(ntipos1);
                    CargarTiposNTipos(cboTiposNTipos1);
                    CargarTiposNTipos(cboTiposNTipos2);

                }
                else
                {
                    agregargruposcampos.Controls.Remove(ntipos1);

                }

                //NUMEROS Y TIPOS - 2
                int CampNTipos2 = Convert.ToInt32(DGVGrupoCampos.SelectedCells[14].Value.ToString());
                if (CampNTipos2 == 1)
                {
                    agregargruposcampos.Controls.Add(ntipos2);
                    CargarTiposNTipos(cboTiposNTipos3);
                    CargarTiposNTipos(cboTiposNTipos4);

                }
                else
                {
                    agregargruposcampos.Controls.Remove(ntipos2);

                }

                //VARIOS - 1
                int CampVarios1 = Convert.ToInt32(DGVGrupoCampos.SelectedCells[15].Value.ToString());
                if (CampVarios1 == 1)
                {
                    agregargruposcampos.Controls.Add(varios1);
                    CargarTiposVariosO(cboTiposVariosO1);

                }
                else
                {
                    agregargruposcampos.Controls.Remove(varios1);

                }

                //VARIOS - 2
                int CampVarios2 = Convert.ToInt32(DGVGrupoCampos.SelectedCells[16].Value.ToString());
                if (CampVarios2 == 1)
                {
                    agregargruposcampos.Controls.Add(varios2);
                    CargarTiposVariosO(cboTiposVariosO2);

                }
                else
                {
                    agregargruposcampos.Controls.Remove(varios2);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR LOS TIPOS DE CADA GRUPO DE CAMPOS PREDETERMINADOS SEGUN EL MODELO SELECCIONADO
        public void CargarCamposPredeterminadosDetalle(int idmodelo,DataGridView DGVGrupoCamposDetalle,ComboBox Tipocaracteristica1, ComboBox Tipocaracteristica2, ComboBox Tipocaracteristica3, ComboBox Tipocaracteristica4
            , ComboBox Tipomedidas1, ComboBox Tipomedidas2, ComboBox Tipomedidas3, ComboBox Tipomedidas4,ComboBox Tipodiametros1, ComboBox Tipodiametros2, ComboBox Tipodiametros3, ComboBox Tipodiametros4
            , ComboBox Tipoformas1, ComboBox Tipoformas2, ComboBox Tipoformas3, ComboBox Tipoformas4,ComboBox Tipoespesores1, ComboBox Tipoespesores2, ComboBox Tipoespesores3, ComboBox Tipoespesores4
            , ComboBox Tipodiseñoaca1, ComboBox Tipodiseñoaca2, ComboBox Tipodiseñoaca3, ComboBox Tipodiseñoaca4,ComboBox Tipontipos1, ComboBox Tipontipos2, ComboBox Tipontipos3, ComboBox Tipontipos4
            , ComboBox Tipovarios1, ComboBox Tipovarios2)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Modelos_CargarCamposPredetermiandosDetalle", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idmodelo", idmodelo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                DGVGrupoCamposDetalle.DataSource = dt;
                con.Close();

                if (DGVGrupoCamposDetalle.RowCount == 0)
                {
                    MessageBox.Show("El modelo elegido no tiene detalles definidos, por favor defina los campos.", "Validación del Sistema", MessageBoxButtons.OK);
                    flowLayoutPanel.Controls.Clear();
                }
                else
                {
                    if (DGVGrupoCamposDetalle.SelectedCells[1].Value != null)
                    {
                        Tipocaracteristica1.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[1].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[2].Value != null)
                    {
                        Tipocaracteristica2.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[2].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[3].Value != null)
                    {
                        Tipocaracteristica3.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[3].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[4].Value != null)
                    {
                        Tipocaracteristica4.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[4].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[5].Value != null)
                    {
                        Tipomedidas1.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[5].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[6].Value != null)
                    {
                        Tipomedidas2.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[6].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[7].Value != null)
                    {
                        Tipomedidas3.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[7].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[8].Value != null)
                    {
                        Tipomedidas4.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[8].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[9].Value != null)
                    {
                        Tipodiametros1.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[9].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[10].Value != null)
                    {
                        Tipodiametros2.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[10].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[11].Value != null)
                    {
                        Tipodiametros3.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[11].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[12].Value != null)
                    {
                        Tipodiametros4.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[12].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[13].Value != null)
                    {
                        Tipoformas1.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[13].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[14].Value != null)
                    {
                        Tipoformas2.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[14].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[15].Value != null)
                    {
                        Tipoformas3.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[15].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[16].Value != null)
                    {
                        Tipoformas4.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[16].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[17].Value != null)
                    {
                        Tipoespesores1.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[17].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[18].Value != null)
                    {
                        Tipoespesores2.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[18].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[19].Value != null)
                    {
                        Tipoespesores3.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[19].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[20].Value != null)
                    {
                        Tipoespesores4.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[20].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[21].Value != null)
                    {
                        Tipodiseñoaca1.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[21].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[22].Value != null)
                    {
                        Tipodiseñoaca2.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[22].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[23].Value != null)
                    {
                        Tipodiseñoaca3.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[23].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[24].Value != null)
                    {
                        Tipodiseñoaca4.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[24].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[25].Value != null)
                    {
                        Tipontipos1.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[25].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[26].Value != null)
                    {
                        Tipontipos2.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[26].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[27].Value != null)
                    {
                        Tipontipos3.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[27].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[28].Value != null)
                    {
                        Tipontipos4.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[28].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[29].Value != null)
                    {
                        Tipovarios1.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[29].Value;
                    }

                    if (DGVGrupoCamposDetalle.SelectedCells[30].Value != null)
                    {
                        Tipovarios2.SelectedValue = DGVGrupoCamposDetalle.SelectedCells[30].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //METODO PARA CARGAR LOS CHECKBOX QUE SE SELECCIONARON PARA LA VISUALIZACIÓN DEL TIPO AL MOMENTO DE DEFINIR EL NOMBRE
        public void CargarCksAtributos(int idmodelo,DataGridView DGVCkAtributos,CheckBox ckcaracteristica1, CheckBox ckcaracteristica2, CheckBox ckcaracteristica3, CheckBox ckcaracteristica4
            , CheckBox ckmedidas1, CheckBox ckmedidas2, CheckBox ckmedidas3, CheckBox ckmedidas4,CheckBox ckdiametros1, CheckBox ckdiametros2, CheckBox ckdiametros3, CheckBox ckdiametros4
            , CheckBox ckformas1, CheckBox ckformas2, CheckBox ckformas3, CheckBox ckformas4, CheckBox ckespesores1, CheckBox ckespesores2, CheckBox ckespesores3, CheckBox ckespesores4
            , CheckBox ckdiseñoaca1, CheckBox ckdiseñoaca2, CheckBox ckdiseñoaca3, CheckBox ckdiseñoaca4, CheckBox ckntipos1, CheckBox ckntipos2, CheckBox ckntipos3, CheckBox ckntipos4
            , CheckBox ckvarios1, CheckBox ckvarios2)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Modelos_CargarCkAtributos", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idmodelo", idmodelo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    CheckBox[] listacks = {ckcaracteristica1, ckcaracteristica2, ckcaracteristica3, ckcaracteristica4, ckmedidas1, ckmedidas2, ckmedidas3, ckmedidas4, ckdiametros1, ckdiametros2, ckdiametros3
                    ,ckdiametros4,ckformas1,ckformas2,ckformas3,ckformas4,ckespesores1,ckespesores2,ckespesores3,ckespesores4,ckdiseñoaca1,ckdiseñoaca2,ckdiseñoaca3,ckdiseñoaca4,ckntipos1,ckntipos2,ckntipos3
                    ,ckntipos4,ckvarios1,ckvarios2};
                    
                    foreach(CheckBox cks in listacks)
                    {
                        cks.Checked = false;
                    }               
                    return;
                }
                DGVCkAtributos.DataSource = dt;
                con.Close();

       
                //ESTADO CHECKBOX CARACTERISTICA 1
                int ckestadocaracteristica1 = Convert.ToInt32(DGVCkAtributos.SelectedCells[1].Value.ToString());
                if (ckestadocaracteristica1 == 1)
                {
                    ckcaracteristica1.Checked = true;
                }
                else
                {
                    ckcaracteristica1.Checked = false;
                }

                //ESTADO CHECKBOX CARACTERISTICA 2
                int ckestadocaracteristica2 = Convert.ToInt32(DGVCkAtributos.SelectedCells[2].Value.ToString());
                if (ckestadocaracteristica2 == 1)
                {
                    ckcaracteristica2.Checked = true;

                }
                else
                {
                    ckcaracteristica2.Checked = false;
                }


                //ESTADO CHECKBOX CARACTERISTICA 3
                int ckestadocaracteristica3 = Convert.ToInt32(DGVCkAtributos.SelectedCells[3].Value.ToString());
                if (ckestadocaracteristica3 == 1)
                {
                    ckcaracteristica3.Checked = true;

                }
                else
                {
                    ckcaracteristica3.Checked = false;
                }

                //ESTADO CHECKBOX CARACTERISTICA 4
                int ckestadocaracteristica4 = Convert.ToInt32(DGVCkAtributos.SelectedCells[4].Value.ToString());
                if (ckestadocaracteristica4 == 1)
                {
                    ckcaracteristica4.Checked = true;

                }
                else
                {
                    ckcaracteristica4.Checked = false;
                }

                //ESTADO CHECKBOX MEDIDAS 1
                int ckestadomedidas1 = Convert.ToInt32(DGVCkAtributos.SelectedCells[5].Value.ToString());
                if (ckestadomedidas1 == 1)
                {
                    ckmedidas1.Checked = true;

                }
                else
                {
                    ckmedidas1.Checked = false;
                }

                //ESTADO CHECKBOX MEDIDAS 2
                int ckestadomedidas2 = Convert.ToInt32(DGVCkAtributos.SelectedCells[6].Value.ToString());
                if (ckestadomedidas2 == 1)
                {
                    ckmedidas2.Checked = true;
                }
                else
                {
                    ckmedidas2.Checked = false;
                }

                //ESTADO CHECKBOX MEDIDAS 3
                int ckestadomedidas3 = Convert.ToInt32(DGVCkAtributos.SelectedCells[7].Value.ToString());
                if (ckestadomedidas3 == 1)
                {
                    ckmedidas3.Checked = true;
                }
                else
                {
                    ckmedidas3.Checked = false;
                }

                //ESTADO CHECKBOX MEDIDAS 4
                int ckestadomedidas4 = Convert.ToInt32(DGVCkAtributos.SelectedCells[8].Value.ToString());
                if (ckestadomedidas4 == 1)
                {
                    ckmedidas4.Checked = true;
                }
                else
                {
                    ckmedidas4.Checked = false;
                }

                //ESTADO CHECKBOX DIAMETRO 1
                int ckestadodiametro1 = Convert.ToInt32(DGVCkAtributos.SelectedCells[9].Value.ToString());
                if (ckestadodiametro1 == 1)
                {
                    ckdiametros1.Checked = true;

                }
                else
                {
                    ckdiametros1.Checked = false;
                }

                //ESTADO CHECKBOX DIAMETRO 2
                int ckestadodiametro2 = Convert.ToInt32(DGVCkAtributos.SelectedCells[10].Value.ToString());
                if (ckestadodiametro2 == 1)
                {
                    ckdiametros2.Checked = true;
                }
                else
                {
                    ckdiametros2.Checked = false;
                }

                //ESTADO CHECKBOX DIAMETRO 3
                int ckestadodiametro3 = Convert.ToInt32(DGVCkAtributos.SelectedCells[11].Value.ToString());
                if (ckestadodiametro3 == 1)
                {
                    ckdiametros3.Checked = true;
                }
                else
                {
                    ckdiametros3.Checked = false;
                }

                //ESTADO CHECKBOX DIAMETRO 4
                int ckestadodiametro4 = Convert.ToInt32(DGVCkAtributos.SelectedCells[12].Value.ToString());
                if (ckestadodiametro4 == 1)
                {
                    ckdiametros4.Checked = true;
                }
                else
                {
                    ckdiametros4.Checked = false;
                }

                //ESTADO CHECKBOX FORMA 1
                int ckestadoforma1 = Convert.ToInt32(DGVCkAtributos.SelectedCells[13].Value.ToString());
                if (ckestadoforma1 == 1)
                {
                    ckformas1.Checked = true;

                }
                else
                {
                    ckformas1.Checked = false;
                }

                //ESTADO CHECKBOX FORMA 2
                int ckestadoforma2 = Convert.ToInt32(DGVCkAtributos.SelectedCells[14].Value.ToString());
                if (ckestadoforma2 == 1)
                {
                    ckformas2.Checked = true;
                }
                else
                {
                    ckformas2.Checked = false;
                }

                //ESTADO CHECKBOX FORMA 3
                int ckestadoforma3 = Convert.ToInt32(DGVCkAtributos.SelectedCells[15].Value.ToString());
                if (ckestadoforma3 == 1)
                {
                    ckformas3.Checked = true;
                }
                else
                {
                    ckformas3.Checked = false;
                }

                //ESTADO CHECKBOX FORMA 4
                int ckestadoforma4 = Convert.ToInt32(DGVCkAtributos.SelectedCells[16].Value.ToString());
                if (ckestadoforma4 == 1)
                {
                    ckformas4.Checked = true;
                }
                else
                {
                    ckformas4.Checked = false;
                }

                //ESTADO CHECKBOX ESPESORES 1
                int ckestadoespesores1 = Convert.ToInt32(DGVCkAtributos.SelectedCells[17].Value.ToString());
                if (ckestadoespesores1 == 1)
                {
                    ckespesores1.Checked = true;

                }
                else
                {
                    ckespesores1.Checked = false;
                }

                //ESTADO CHECKBOX ESPESORES 2
                int ckestadoespesores2 = Convert.ToInt32(DGVCkAtributos.SelectedCells[18].Value.ToString());
                if (ckestadoespesores2 == 1)
                {
                    ckespesores2.Checked = true;
                }
                else
                {
                    ckespesores2.Checked = false;
                }

                //ESTADO CHECKBOX ESPESORES 3
                int ckestadoespesores3 = Convert.ToInt32(DGVCkAtributos.SelectedCells[19].Value.ToString());
                if (ckestadoespesores3 == 1)
                {
                    ckespesores3.Checked = true;
                }
                else
                {
                    ckespesores3.Checked = false;
                }

                //ESTADO CHECKBOX ESPESORES 4
                int ckestadoespesores4 = Convert.ToInt32(DGVCkAtributos.SelectedCells[20].Value.ToString());
                if (ckestadoespesores4 == 1)
                {
                    ckespesores4.Checked = true;
                }
                else
                {
                    ckespesores4.Checked = false;
                }

                //ESTADO CHECKBOX DISEÑO ACABADO 1
                int ckestadodiseñoacabado1 = Convert.ToInt32(DGVCkAtributos.SelectedCells[21].Value.ToString());
                if (ckestadodiseñoacabado1 == 1)
                {
                    ckdiseñoaca1.Checked = true;

                }
                else
                {
                    ckdiseñoaca1.Checked = false;
                }

                //ESTADO CHECKBOX DISEÑO ACABADO 2
                int ckestadodiseñoacabado2 = Convert.ToInt32(DGVCkAtributos.SelectedCells[22].Value.ToString());
                if (ckestadodiseñoacabado2 == 1)
                {
                    ckdiseñoaca2.Checked = true;
                }
                else
                {
                    ckdiseñoaca2.Checked = false;
                }

                //ESTADO CHECKBOX DISEÑO ACABADO 3
                int ckestadodiseñoacabado3 = Convert.ToInt32(DGVCkAtributos.SelectedCells[23].Value.ToString());
                if (ckestadodiseñoacabado3 == 1)
                {
                    ckdiseñoaca3.Checked = true;
                }
                else
                {
                    ckdiseñoaca3.Checked = false;
                }

                //ESTADO CHECKBOX DISEÑO ACABADO 4
                int ckestadodiseñoacabado4 = Convert.ToInt32(DGVCkAtributos.SelectedCells[24].Value.ToString());
                if (ckestadodiseñoacabado4 == 1)
                {
                    ckdiseñoaca4.Checked = true;
                }
                else
                {
                    ckdiseñoaca4.Checked = false;
                }

                //ESTADO CHECKBOX TIPO N TIPOS 1
                int ckestadotipontipos1 = Convert.ToInt32(DGVCkAtributos.SelectedCells[25].Value.ToString());
                if (ckestadotipontipos1 == 1)
                {
                    ckntipos1.Checked = true;

                }
                else
                {
                    ckntipos1.Checked = false;
                }

                //ESTADO CHECKBOX TIPO N TIPOS 2
                int ckestadotipontipos2 = Convert.ToInt32(DGVCkAtributos.SelectedCells[26].Value.ToString());
                if (ckestadotipontipos2 == 1)
                {
                    ckntipos2.Checked = true;
                }
                else
                {
                    ckntipos2.Checked = false;
                }

                //ESTADO CHECKBOX TIPO N TIPOS 3
                int ckestadotipontipos3 = Convert.ToInt32(DGVCkAtributos.SelectedCells[27].Value.ToString());
                if (ckestadotipontipos3 == 1)
                {
                    ckntipos3.Checked = true;
                }
                else
                {
                    ckntipos3.Checked = false;
                }

                //ESTADO CHECKBOX TIPO N TIPOS 4
                int ckestadotipontipos4 = Convert.ToInt32(DGVCkAtributos.SelectedCells[28].Value.ToString());
                if (ckestadotipontipos4 == 1)
                {
                    ckntipos4.Checked = true;
                }
                else
                {
                    ckntipos4.Checked = false;
                }

                //ESTADO CHECKBOX TIPO VARIOS 1
                int ckestadotipovarios1 = Convert.ToInt32(DGVCkAtributos.SelectedCells[29].Value.ToString());
                if (ckestadotipovarios1 == 1)
                {
                    ckvarios1.Checked = true;

                }
                else
                {
                    ckvarios1.Checked = false;
                }

                //ESTADO CHECKBOX TIPO VARIOS 2
                int ckestadotipovarios2 = Convert.ToInt32(DGVCkAtributos.SelectedCells[30].Value.ToString());
                if (ckestadotipovarios2 == 1)
                {
                    ckvarios2.Checked = true;
                }
                else
                {
                    ckvarios2.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //METODO PARA VALIDAR SI EL MODELO YA TIENE POSICIONES ASIGNADAS
        public void ValidarModeloConPosiciones(int idmodelo,DataGridView DGVModeloConPosicion,Panel posicionnombre)
        {
            habilitarValidaciones = false;
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM ModeloXPosicion WHERE IdModelo = @idmodelo AND Estado = 1", con);
                cmd.Parameters.AddWithValue("idmodelo", idmodelo);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                DGVModeloConPosicion.DataSource = dt;
                con.Close();

                if (DGVModeloConPosicion.Rows.Count > 0)
                {
                    MessageBox.Show("El Modelo seleccionado ya tiene posiciones registradas para la definición del nombre del producto.", "Validación del Sistema", MessageBoxButtons.OK);
                    posicionnombre.Visible = false;
                    return;
                }
                else
                {
                    CargarGrupoCamposPredeterminados(idmodelo, datalistadoCargarGrupoCampos, floGrupoCampos, ckCaracteristicas1, ckCaracteristicas2, ckCamposMedida1, ckCamposMedida2, ckCamposDiametros1, ckCamposDiametros2
                        , ckCamposFormas1, ckCamposFormas2, ckCamposEspesores1, ckCamposEspesores2, ckCamposDiseñoAcabado1, ckCamposDiseñoAcabado2, ckCamposNTipos1, ckCamposNTipos2, ckVariosO1, ckVariosO2, ckGenerales, panelCamposCaracteristicas1
                        , panelCamposCaracteristicas2, panelCamposMedidas1, panelCamposMedidas2, panelCamposDiametros1, panelCamposDiametros2, panelCamposFormas1, panelCamposFormas2, panelCamposEspesores1, panelCamposEspesores2
                        , panelCamposDiseñoAcabado1, panelCamposDiseñoAcabado2, panelCamposNTipos1, panelCamposNTipos2, panelCamposVariosO1, panelCamposVariosO2, panelCamposGeneral);

                    int CampGenerales = Convert.ToInt32(datalistadoCargarGrupoCampos.SelectedCells[17].Value.ToString());
                    if (CampGenerales == 1)
                    {
                        MessageBox.Show("No es posible definir posiciones cuando el modelo utiliza el atributo de campo general", "Restricción del Sistema", MessageBoxButtons.OK);
                        return;
                    }

                    CargarCamposPredeterminadosDetalle(idmodelo, datalistadoCargarGrupoCamposDetalle, cboTipoCaracteristicas1, cboTipoCaracteristicas2, cboTipoCaracteristicas3, cboTipoCaracteristicas4
                        , cboTipoMedida1, cboTipoMedida2, cboTipoMedida3, cboTipoMedida4, cboTiposDiametros1, cboTiposDiametros2, cboTiposDiametros3, cboTiposDiametros4, cboTiposFormas1, cboTiposFormas2
                        , cboTiposFormas3, cboTiposFormas4, cbooTipoEspesores1, cbooTipoEspesores2, cbooTipoEspesores3, cbooTipoEspesores4, cboTiposDiseñosAcabados1, cboTiposDiseñosAcabados2, cboTiposDiseñosAcabados3
                        , cboTiposDiseñosAcabados4, cboTiposNTipos1, cboTiposNTipos2, cboTiposNTipos3, cboTiposNTipos4, cboTiposVariosO1, cboTiposVariosO2);


                    CargarCksAtributos(idmodelo, datalistadockatributos, ckCaracteristicaAtributo1, ckCaracteristicaAtributo2, ckCaracteristicaAtributo3, ckCaracteristicaAtributo4, ckMedidasAtributos1, ckMedidasAtributos2
                        , ckMedidasAtributos3, ckMedidasAtributos4, ckDiametroAtributos1, ckDiametroAtributos2, ckDiametroAtributos3, ckDiametroAtributos4, ckFormasAtributos1, ckFormasAtributos2, ckFormasAtributos3
                        , ckFormasAtributos4, ckEspesoresAtributos1, ckEspesoresAtributos2, ckEspesoresAtributos3, ckEspesoresAtributos4, ckDiseñoAcabadoAtributos1, ckDiseñoAcabadoAtributos2, ckDiseñoAcabadoAtributos3
                        , ckDiseñoAcabadoAtributos4, ckTiposNTiposAtributos1, ckTiposNTiposAtributos2, ckTiposNTiposAtributos3, ckTiposNTiposAtributos4, ckVariosAtributos1, ckVariosAtributos2);

                    posicionnombre.Visible = true;
                   
                    BloqueoControles_PosicionNombre(new ComboBox[]
                    {cboTipoCaracteristicas1,cboTipoCaracteristicas2,cboTipoCaracteristicas3,cboTipoCaracteristicas4
                    ,cboTipoMedida1,cboTipoMedida2,cboTipoMedida3,cboTipoMedida4
                    ,cboTiposDiametros1,cboTiposDiametros2,cboTiposDiametros3,cboTiposDiametros4
                    ,cboTiposFormas1,cboTiposFormas2,cboTiposFormas3,cboTiposFormas4
                    ,cbooTipoEspesores1,cbooTipoEspesores2,cbooTipoEspesores3,cbooTipoEspesores4
                    ,cboTiposDiseñosAcabados1,cboTiposDiseñosAcabados2,cboTiposDiseñosAcabados3,cboTiposDiseñosAcabados4
                    ,cboTiposNTipos1,cboTiposNTipos2,cboTiposNTipos3,cboTiposNTipos4
                    ,cboTiposVariosO1,cboTiposVariosO2}
                    , new CheckBox[]
                    {ckCaracteristicaAtributo1,ckCaracteristicaAtributo2,ckCaracteristicaAtributo3,ckCaracteristicaAtributo4
                    ,ckMedidasAtributos1,ckMedidasAtributos2,ckMedidasAtributos3,ckMedidasAtributos4
                    ,ckDiametroAtributos1,ckDiametroAtributos2,ckDiametroAtributos3,ckDiametroAtributos4
                    ,ckFormasAtributos1,ckFormasAtributos2,ckFormasAtributos3,ckFormasAtributos4
                    ,ckEspesoresAtributos1,ckEspesoresAtributos2,ckEspesoresAtributos3,ckEspesoresAtributos4
                    ,ckDiseñoAcabadoAtributos1,ckDiseñoAcabadoAtributos2,ckDiseñoAcabadoAtributos3,ckDiseñoAcabadoAtributos4
                    ,ckTiposNTiposAtributos1,ckTiposNTiposAtributos2,ckTiposNTiposAtributos3,ckTiposNTiposAtributos4
                    ,ckVariosAtributos1,ckVariosAtributos2});
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //METODO QUE GUARDA LAS POSICIONES PARA EL NOMBRE DEL PRODUCTO
        public void Agregar_PosicionesNombre(int idmodelo,FlowLayoutPanel grupocampos)
        {
            try
            {
                DialogResult boton = MessageBox.Show("¿Desea guardar las posiciones definidas para el nombre del producto?", "Validación del Sistema", MessageBoxButtons.OKCancel);

                if (boton == DialogResult.OK)
                {

                    int Totalposiciones = 16;

                    string[] arr_posicionNombre = new string[Totalposiciones];

                    for (int i = 0; i < Totalposiciones; i++)
                    {
                        if (i < grupocampos.Controls.Count)
                        {
                            Control panelGrupo = grupocampos.Controls[i];
                            arr_posicionNombre[i] = panelGrupo.Name;
                        }
                        else
                        {
                            arr_posicionNombre[i] = "";
                        }
                    }

                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Modelos_InsertarXPosicion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdModelo", idmodelo);

                    for (int i = 0; i < Totalposiciones; i++)
                    {
                        cmd.Parameters.AddWithValue($"@posicion{i}", arr_posicionNombre[i]);
                    }
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Se registraron correctamente las posiciones definidas para el nombre del Producto.", "Registro Nuevo", MessageBoxButtons.OK);

                    floGrupoCampos.Controls.Clear();
                    floPosicionGrupoCampos.Controls.Clear();
                    habilitarValidaciones = true;
                 
                    LimpiarControles_PosiciNombre(new CheckBox[] 
                    {ckCaracteristicaAtributo1,ckCaracteristicaAtributo2,ckCaracteristicaAtributo3,ckCaracteristicaAtributo4,
                    ckMedidasAtributos1,ckMedidasAtributos2,ckMedidasAtributos3,ckMedidasAtributos4,
                    ckDiametroAtributos1,ckDiametroAtributos2,ckDiametroAtributos3,ckDiametroAtributos4,
                    ckFormasAtributos1,ckFormasAtributos2,ckFormasAtributos3,ckFormasAtributos4,
                    ckEspesoresAtributos1,ckEspesoresAtributos2,ckEspesoresAtributos3,ckEspesoresAtributos4,
                    ckDiseñoAcabadoAtributos1,ckDiseñoAcabadoAtributos2,ckDiseñoAcabadoAtributos3,ckDiseñoAcabadoAtributos4,
                    ckTiposNTiposAtributos1,ckTiposNTiposAtributos2,ckTiposNTiposAtributos3,ckTiposNTiposAtributos4,
                    ckVariosAtributos1,ckVariosAtributos2});

                  
                    HabilitarControles_PosicionNombre(new ComboBox[] 
                    {cboTipoCaracteristicas1,cboTipoCaracteristicas2,cboTipoCaracteristicas3,cboTipoCaracteristicas4
                    ,cboTipoMedida1,cboTipoMedida2,cboTipoMedida3,cboTipoMedida4
                    ,cboTiposDiametros1,cboTiposDiametros2,cboTiposDiametros3,cboTiposDiametros4
                    ,cboTiposFormas1,cboTiposFormas2,cboTiposFormas3,cboTiposFormas4
                    ,cbooTipoEspesores1,cbooTipoEspesores2,cbooTipoEspesores3,cbooTipoEspesores4
                    ,cboTiposDiseñosAcabados1,cboTiposDiseñosAcabados2,cboTiposDiseñosAcabados3,cboTiposDiseñosAcabados4
                    ,cboTiposNTipos1,cboTiposNTipos2,cboTiposNTipos3,cboTiposNTipos4
                    ,cboTiposVariosO1,cboTiposVariosO2}
                    , new CheckBox[] 
                    {ckCaracteristicaAtributo1,ckCaracteristicaAtributo2,ckCaracteristicaAtributo3,ckCaracteristicaAtributo4
                    ,ckMedidasAtributos1,ckMedidasAtributos2,ckMedidasAtributos3,ckMedidasAtributos4
                    ,ckDiametroAtributos1,ckDiametroAtributos2,ckDiametroAtributos3,ckDiametroAtributos4
                    ,ckFormasAtributos1,ckFormasAtributos2,ckFormasAtributos3,ckFormasAtributos4
                    ,ckEspesoresAtributos1,ckEspesoresAtributos2,ckEspesoresAtributos3,ckEspesoresAtributos4
                    ,ckDiseñoAcabadoAtributos1,ckDiseñoAcabadoAtributos2,ckDiseñoAcabadoAtributos3,ckDiseñoAcabadoAtributos4
                    ,ckTiposNTiposAtributos1,ckTiposNTiposAtributos2,ckTiposNTiposAtributos3,ckTiposNTiposAtributos4
                    ,ckVariosAtributos1,ckVariosAtributos2});
                    panelPosicionNombre.Visible = false;
                }
                else
                {
                    panelPosicionNombre.Visible = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //////-----------------------
        ///METODOS DE LIMPIEZA
       
        //METODO PARA BLOQUEAR LOS CONTROLES AL MOMENTO DE DEFINIR POSICIONES PARA EL NOMBRE DEL PRODUCTO
        public void BloqueoControles_PosicionNombre(ComboBox[] listaCombos, CheckBox[] listacks)
        {
            foreach (ComboBox cbos in listaCombos)
            {
                cbos.Enabled = false;
            }

            foreach (CheckBox cks in listacks)
            {
                cks.Enabled = false;
            }
         
        }

        //DESBLOQUEO DE LOS CONTROLES AL MOMENTO DE SALIR DE LA DEFINICIÓN DE POSICIONES
        public void HabilitarControles_PosicionNombre(ComboBox[] listaCombos, CheckBox[] listacks)         
        {
            foreach(ComboBox cbos in listaCombos)
            {
                cbos.Enabled = true;
            }

            foreach(CheckBox cks in listacks)
            {
                cks.Enabled = true;
            }
        }

        public void LimpiarControles_PosiciNombre(CheckBox[] listacks)
        {
            foreach(CheckBox cks in listacks)
            {
                cks.Checked = false;
            }          
        }

        //EVENTO QUE ABRE EL PANEL PARA ASIGNAR LAS POSICIONES Y DEFINIR EL NOMBRE DEL PRODUCTO
        private void btnAgregarPosicionNombre_Click(object sender, EventArgs e)
        {
            if (lblCodigo.Text == "N")
            {
                MessageBox.Show("La definición de posiciones no está disponible hasta que el modelo haya sido creado.", "Restricción del Sistema", MessageBoxButtons.OK);
                return;  
            }
            else
            {
                ValidarModeloConPosiciones(Convert.ToInt32(lblCodigo.Text), datalistadoModeloConPosicion, panelPosicionNombre);
            }
        }

        private void btnCerrarPosicionNombre_Click(object sender, EventArgs e)
        {
            floGrupoCampos.Controls.Clear();
            floPosicionGrupoCampos.Controls.Clear();
            habilitarValidaciones = true;

            HabilitarControles_PosicionNombre(new ComboBox[]
                    {cboTipoCaracteristicas1,cboTipoCaracteristicas2,cboTipoCaracteristicas3,cboTipoCaracteristicas4
                    ,cboTipoMedida1,cboTipoMedida2,cboTipoMedida3,cboTipoMedida4
                    ,cboTiposDiametros1,cboTiposDiametros2,cboTiposDiametros3,cboTiposDiametros4
                    ,cboTiposFormas1,cboTiposFormas2,cboTiposFormas3,cboTiposFormas4
                    ,cbooTipoEspesores1,cbooTipoEspesores2,cbooTipoEspesores3,cbooTipoEspesores4
                    ,cboTiposDiseñosAcabados1,cboTiposDiseñosAcabados2,cboTiposDiseñosAcabados3,cboTiposDiseñosAcabados4
                    ,cboTiposNTipos1,cboTiposNTipos2,cboTiposNTipos3,cboTiposNTipos4
                    ,cboTiposVariosO1,cboTiposVariosO2}
                    , new CheckBox[]
                    {ckCaracteristicaAtributo1,ckCaracteristicaAtributo2,ckCaracteristicaAtributo3,ckCaracteristicaAtributo4
                    ,ckMedidasAtributos1,ckMedidasAtributos2,ckMedidasAtributos3,ckMedidasAtributos4
                    ,ckDiametroAtributos1,ckDiametroAtributos2,ckDiametroAtributos3,ckDiametroAtributos4
                    ,ckFormasAtributos1,ckFormasAtributos2,ckFormasAtributos3,ckFormasAtributos4
                    ,ckEspesoresAtributos1,ckEspesoresAtributos2,ckEspesoresAtributos3,ckEspesoresAtributos4
                    ,ckDiseñoAcabadoAtributos1,ckDiseñoAcabadoAtributos2,ckDiseñoAcabadoAtributos3,ckDiseñoAcabadoAtributos4
                    ,ckTiposNTiposAtributos1,ckTiposNTiposAtributos2,ckTiposNTiposAtributos3,ckTiposNTiposAtributos4
                    ,ckVariosAtributos1,ckVariosAtributos2});

            panelPosicionNombre.Visible = false;
            
        }

        //AVISO : ANTES DE REALIZAR ESTO SE DEBE ACTIVAR EN EL DISEÑADOR EN EL FLOWLAYOUT SU PROPIEDAD ALLOWDROP EN TRUE 
        //PARA QUE RECIBA OBJETOS EN ESTE CASO PANELES

        //EFECTO VISUAL DE MOVIMIENTO
        private void floGrupoCampos_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Panel)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        //REALIZAR LA ACCION DE MOVER EL PANEL
        private void floGrupoCampos_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Panel)))
            {
                Panel posicionomnbre = (Panel)e.Data.GetData(typeof(Panel));
                FlowLayoutPanel origen = posicionomnbre.Parent as FlowLayoutPanel;
                FlowLayoutPanel destino = sender as FlowLayoutPanel;

                if(origen != null && destino != null)
                {
                    origen.Controls.Remove(posicionomnbre);
                    destino.Controls.Add(posicionomnbre);
                }
            }
        }

        private void floPosicionGrupoCampos_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Panel)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void floPosicionGrupoCampos_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Panel)))
            {
                Panel posicionomnbre = (Panel)e.Data.GetData(typeof(Panel));
                FlowLayoutPanel origen = posicionomnbre.Parent as FlowLayoutPanel;
                FlowLayoutPanel destino = sender as FlowLayoutPanel;

                if (origen != null && destino != null)
                {
                    origen.Controls.Remove(posicionomnbre);
                    destino.Controls.Add(posicionomnbre);
                }
            }
        }

        //EVENTOS PARA INICIAR EL ARRASTRE A LOS PANALES
        private void panelCamposCaracteristicas1_MouseDown(object sender, MouseEventArgs e)
        {
           if(sender is Panel caracterisita1)
            {
                DoDragDrop(caracterisita1, DragDropEffects.Move);
            }
        }

        private void panelCamposCaracteristicas2_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Panel caracterisita2)
            {
                DoDragDrop(caracterisita2, DragDropEffects.Move);
            }
        }

        private void panelCamposMedidas1_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Panel medidas1)
            {
                DoDragDrop(medidas1, DragDropEffects.Move);
            }
        }

        private void panelCamposMedidas2_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Panel medidas2)
            {
                DoDragDrop(medidas2, DragDropEffects.Move);
            }
        }

        private void panelCamposDiametros1_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Panel diametros1)
            {
                DoDragDrop(diametros1, DragDropEffects.Move);
            }
        }

        private void panelCamposDiametros2_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Panel diametros2)
            {
                DoDragDrop(diametros2, DragDropEffects.Move);
            }
        }

        private void panelCamposFormas1_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Panel formas1)
            {
                DoDragDrop(formas1, DragDropEffects.Move);
            }
        }

        private void panelCamposFormas2_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Panel formas2)
            {
                DoDragDrop(formas2, DragDropEffects.Move);
            }
        }

        private void panelCamposEspesores1_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Panel espesores1)
            {
                DoDragDrop(espesores1, DragDropEffects.Move);
            }
        }

        private void panelCamposEspesores2_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Panel espesores2)
            {
                DoDragDrop(espesores2, DragDropEffects.Move);
            }
        }

        private void panelCamposDiseñoAcabado1_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Panel diseñoacabado1)
            {
                DoDragDrop(diseñoacabado1, DragDropEffects.Move);
            }
        }

        private void panelCamposDiseñoAcabado2_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Panel diseñoacabado2)
            {
                DoDragDrop(diseñoacabado2, DragDropEffects.Move);
            }
        }

        private void panelCamposNTipos1_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Panel ntipos1)
            {
                DoDragDrop(ntipos1, DragDropEffects.Move);
            }
        }

        private void panelCamposNTipos2_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Panel ntipos2)
            {
                DoDragDrop(ntipos2, DragDropEffects.Move);
            }
        }

        private void panelCamposVariosO1_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Panel varios1)
            {
                DoDragDrop(varios1, DragDropEffects.Move);
            }
        }

        private void panelCamposVariosO2_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Panel varios2)
            {
                DoDragDrop(varios2, DragDropEffects.Move);
            }
        }

        private void btnGuardarPosicionNombre_Click(object sender, EventArgs e)
        {
            if(floGrupoCampos.Controls.Count > 0)
            {
                MessageBox.Show("Arrastre los grupos restantes para guardar la posición para el nombre del producto.","Configuración de Posiciones", MessageBoxButtons.OK);
                return;
            }
            else
            {
                Agregar_PosicionesNombre(Convert.ToInt32(lblCodigo.Text), floPosicionGrupoCampos);
               
            }
        }

       
    }
}









            


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

namespace ArenasProyect3.Modulos.Procesos.Mantenimientos
{
    public partial class MantenimientoModelos : Form
    {
        //VARIABLES DE VALIDACIÓN PARA EL INGRESO Y EDICIÓN DE DATOS
        bool repetidoDescripcion;
        bool repetidoAbreviatura;

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
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarModeloSegunLinea", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@linea", cboTipoLinea.SelectedValue.ToString());
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
                cmd = new SqlCommand("MostrarModeloSegunLinea", con);
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

            Cancelar.Visible = true;
            btnEditar.Enabled = true;

            cboEstado.Text = "ACTIVO";
            txtDescripcion.Text = "";
            txtAbreviatura.Text = "";

            lblCodigo.Text = "N";

            lblEstadoAtributo.Text = "MODELO NO DEFINIDO";
        }

        //GUARDAR UNA NUEVO MDOELO EN MI BASE DE DATOS CON SUS ATRIBUTOS Y DETALLE DE ESTOS
        private void btnGuardar2_Click(object sender, EventArgs e)
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
                    MessageBox.Show("Debe ingresar todos los campos necesarios para pode continuar.", "Validación del Sistema", MessageBoxButtons.OK);
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
                            cmd = new SqlCommand("InsertarModelos", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text);
                            cmd.Parameters.AddWithValue("@abreviatura", txtAbreviatura.Text);
                            cmd.Parameters.AddWithValue("@codigolinea", cboTipoLinea.SelectedValue.ToString());
                            cmd.ExecuteNonQuery();
                            con.Close();

                            int linea = Convert.ToInt32(cboTipoLinea.SelectedValue.ToString());
                            Mostrar(linea);

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
                            cmd = new SqlCommand("InsertarAtributosXModelo", con);
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
                            cmd = new SqlCommand("InsertarAtributosXModeloDetalle", con);
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

                            //EDITAMOS EL ESTADO DE MI MODELO
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("EditarEstadoAtributoModelo", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idmodelo", lblCodigo.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            int linea = Convert.ToInt32(cboTipoLinea.SelectedValue.ToString());
                            Mostrar(linea);

                            lblEstadoAtributo.Text = "MODELO YA DEFINIDO";

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

        //EDITAR UN MODELO DE MI BASE DE DATOS
        private void btnEditar2_Click(object sender, EventArgs e)
        {
            if (txtDescripcion.Text != "" || txtAbreviatura.Text != "" || lblCodigo.Text != "N")
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
                        cmd = new SqlCommand("EditarModelo", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@codigo", Convert.ToInt32(lblCodigo.Text));
                        cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text);
                        cmd.Parameters.AddWithValue("@abreviatura", txtAbreviatura.Text);
                        cmd.Parameters.AddWithValue("@codigolinea", cboTipoLinea.SelectedValue.ToString());

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

                        int linea = Convert.ToInt32(cboTipoLinea.SelectedValue.ToString());
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
            else
            {
                MessageBox.Show("Los campos no pueden estar vacios.", "Validación del Sistema", MessageBoxButtons.OK);
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
            panelDefinicionAtributos.Visible = false;
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

        //BUSQUEDA DE MODELO------------------------------------------------------------
        private void txtBusquedaModelo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboBusquedaModelo.Text == "DESCRIPCIÓN")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("BusquedaModeloPorDescripcion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@descripcion", txtBusquedaModelo.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoLineas.DataSource = dt;
                    con.Close();
                    OrdenarColumnasModelo(datalistadoLineas);
                }
                else
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("BusquedaModeloPorAbreviatura", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@abreviatura", txtBusquedaModelo.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoLineas.DataSource = dt;
                    con.Close();
                    OrdenarColumnasModelo(datalistadoLineas);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
    }
}

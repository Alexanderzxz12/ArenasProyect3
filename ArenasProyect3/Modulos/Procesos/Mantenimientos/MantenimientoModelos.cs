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


                            //GUARDADO DE LOS NOMBRES DE LOS TIPOS A TRAVES DE LOS CHECKBOX
                            Modelos_InsertarCkNombreTipos(Convert.ToInt32(lblCodigo.Text),ckCaracteristicaAtributo1, ckCaracteristicaAtributo2,ckCaracteristicaAtributo3
                                ,ckCaracteristicaAtributo4,ckMedidasAtributos1, ckMedidasAtributos2, ckMedidasAtributos3, ckMedidasAtributos4,ckDiametroAtributos1
                                , ckDiametroAtributos2, ckDiametroAtributos3, ckDiametroAtributos4, ckFormasAtributos1, ckFormasAtributos2
                                , ckFormasAtributos3, ckFormasAtributos4,ckEspesoresAtributos1, ckEspesoresAtributos2, ckEspesoresAtributos3, ckEspesoresAtributos4
                                ,ckDiseñoAcabadoAtributos1, ckDiseñoAcabadoAtributos2, ckDiseñoAcabadoAtributos3, ckDiseñoAcabadoAtributos4,ckTiposNTiposAtributos1
                                , ckTiposNTiposAtributos2, ckTiposNTiposAtributos3, ckTiposNTiposAtributos4,ckVariosAtributos1,ckVariosAtributos2);

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

        ///////////////////////////////
        ////IMPLEMENTACIÓN DE LA DEFINICIÓN DE NOMBRE PARA LOS ATRIBUTOS
        ///

        private void Modelos_InsertarCkNombreTipos(int idmodelo,CheckBox ckcaracteristica1, CheckBox ckcaracteristica2, CheckBox ckcaracteristica3, CheckBox ckcaracteristica4, CheckBox ckmedidas1, CheckBox ckmedidas2
            , CheckBox ckmedidas3, CheckBox ckmedidas4, CheckBox ckdiametros1, CheckBox ckdiametros2, CheckBox ckdiametros3, CheckBox ckdiametros4, CheckBox ckformas1, CheckBox ckformas2, CheckBox ckformas3
            , CheckBox ckformas4, CheckBox ckespesores1, CheckBox ckespesores2, CheckBox ckespesores3, CheckBox ckespesores4, CheckBox ckdiseñoacabado1, CheckBox ckdiseñoacabado2, CheckBox ckdiseñoacabado3
            , CheckBox ckdiseñoacabado4, CheckBox ckntipos1, CheckBox ckntipos2, CheckBox ckntipos3, CheckBox ckntipos4, CheckBox ckvarios1, CheckBox ckvarios2)
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
                if (ckdiseñoacabado1.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipodiseñoacabado1", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipodiseñoacabado1", 0);
                }

                if (ckdiseñoacabado2.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipodiseñoacabado2", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipodiseñoacabado2", 0);
                }

                if (ckdiseñoacabado3.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@cktipodiseñoacabado3", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@cktipodiseñoacabado3", 0);
                }

                if (ckdiseñoacabado4.Checked == true)
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

        public void CargarGrupoCamposPredeterminados()
        {
            string asignarnombre = lblCodigo.Text;
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Modelos_CargarGrupoCamposPredeterminados", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idmodelo", Convert.ToInt32(asignarnombre));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoCargarGrupoCampos.DataSource = dt;
                con.Close();


                //CARACTERISTICAS - 1
                int CampCaracteristicas1 = Convert.ToInt32(datalistadoCargarGrupoCampos.SelectedCells[1].Value.ToString());
                if (CampCaracteristicas1 == 1)
                {
                    ckCaracteristicas1.Checked = true;
                    flowlayoutpanelasignarnombre.Controls.Add(panelCamposCaracteristicas1);
                }
                else
                {
                    ckCaracteristicas1.Checked = false;
                    flowlayoutpanelasignarnombre.Controls.Remove(panelCamposCaracteristicas1);

                }

                //CARACTERISTICAS - 2
                int CampCaracteristicas2 = Convert.ToInt32(datalistadoCargarGrupoCampos.SelectedCells[2].Value.ToString());
                if (CampCaracteristicas2 == 1)
                {
                    ckCaracteristicas2.Checked = true;
                    flowlayoutpanelasignarnombre.Controls.Add(panelCamposCaracteristicas2);

                }
                else
                {
                    ckCaracteristicas2.Checked = false;
                    flowlayoutpanelasignarnombre.Controls.Remove(panelCamposCaracteristicas2);

                }

                //MEDIDAS - 1
                int CampMedidas1 = Convert.ToInt32(datalistadoCargarGrupoCampos.SelectedCells[3].Value.ToString());
                if (CampMedidas1 == 1)
                {
                    ckCamposMedida1.Checked = true;
                    flowlayoutpanelasignarnombre.Controls.Add(panelCamposMedidas1);

                }
                else
                {
                    ckCamposMedida1.Checked = false;
                    flowlayoutpanelasignarnombre.Controls.Remove(panelCamposMedidas1);

                }

                //MEDIDAS - 2
                int CampMedidas2 = Convert.ToInt32(datalistadoCargarGrupoCampos.SelectedCells[4].Value.ToString());
                if (CampMedidas2 == 1)
                {
                    ckCamposMedida2.Checked = true;
                    flowlayoutpanelasignarnombre.Controls.Add(panelCamposMedidas2);

                }
                else
                {
                    ckCamposMedida2.Checked = false;
                    flowlayoutpanelasignarnombre.Controls.Remove(panelCamposMedidas2);

                }

                //DIAMETROS - 1
                int CampDiametros1 = Convert.ToInt32(datalistadoCargarGrupoCampos.SelectedCells[5].Value.ToString());
                if (CampDiametros1 == 1)
                {
                    ckCamposDiametros1.Checked = true;
                    flowlayoutpanelasignarnombre.Controls.Add(panelCamposDiametros1);

                }
                else
                {
                    ckCamposDiametros1.Checked = false;
                    flowlayoutpanelasignarnombre.Controls.Remove(panelCamposDiametros1);

                }

                //DIAMETROS - 2
                int CampDiametros2 = Convert.ToInt32(datalistadoCargarGrupoCampos.SelectedCells[6].Value.ToString());
                if (CampDiametros2 == 1)
                {
                    ckCamposDiametros2.Checked = true;
                    flowlayoutpanelasignarnombre.Controls.Add(panelCamposDiametros2);

                }
                else
                {
                    ckCamposDiametros2.Checked = false;
                    flowlayoutpanelasignarnombre.Controls.Remove(panelCamposDiametros2);

                }

                //FORMAS - 1
                int CampFormas1 = Convert.ToInt32(datalistadoCargarGrupoCampos.SelectedCells[7].Value.ToString());
                if (CampFormas1 == 1)
                {
                    ckCamposFormas1.Checked = true;
                    flowlayoutpanelasignarnombre.Controls.Add(panelCamposFormas1);
                }
                else
                {
                    ckCamposFormas1.Checked = false;
                    flowlayoutpanelasignarnombre.Controls.Remove(panelCamposFormas1);

                }

                //FORMAS - 2
                int CampFormas2 = Convert.ToInt32(datalistadoCargarGrupoCampos.SelectedCells[8].Value.ToString());
                if (CampFormas2 == 1)
                {
                    ckCamposFormas2.Checked = true;
                    flowlayoutpanelasignarnombre.Controls.Add(panelCamposFormas2);

                }
                else
                {
                    ckCamposFormas2.Checked = false;
                    flowlayoutpanelasignarnombre.Controls.Remove(panelCamposFormas2);

                }

                //ESPESORES - 1
                int CampEspesores1 = Convert.ToInt32(datalistadoCargarGrupoCampos.SelectedCells[9].Value.ToString());
                if (CampEspesores1 == 1)
                {
                    ckCamposEspesores1.Checked = true;
                    flowlayoutpanelasignarnombre.Controls.Add(panelCamposEspesores1);

                }
                else
                {
                    ckCamposEspesores1.Checked = false;
                    flowlayoutpanelasignarnombre.Controls.Remove(panelCamposEspesores1);

                }

                //ESPESORES - 2
                int CampEspesores2 = Convert.ToInt32(datalistadoCargarGrupoCampos.SelectedCells[10].Value.ToString());
                if (CampEspesores2 == 1)
                {
                    ckCamposEspesores2.Checked = true;
                    flowlayoutpanelasignarnombre.Controls.Add(panelCamposEspesores2);

                }
                else
                {
                    ckCamposEspesores2.Checked = false;
                    flowlayoutpanelasignarnombre.Controls.Remove(panelCamposEspesores2);

                }

                //DISEÑO Y ACABADOS - 1
                int CampDiseñoAcabado1 = Convert.ToInt32(datalistadoCargarGrupoCampos.SelectedCells[11].Value.ToString());
                if (CampDiseñoAcabado1 == 1)
                {
                    ckCamposDiseñoAcabado1.Checked = true;
                    flowlayoutpanelasignarnombre.Controls.Add(panelCamposDiseñoAcabado1);

                }
                else
                {
                    ckCamposDiseñoAcabado1.Checked = false;
                    flowlayoutpanelasignarnombre.Controls.Remove(panelCamposDiseñoAcabado1);

                }

                //DISEÑO Y ACABADOS - 2
                int CampDiseñoAcabado2 = Convert.ToInt32(datalistadoCargarGrupoCampos.SelectedCells[12].Value.ToString());
                if (CampDiseñoAcabado2 == 1)
                {
                    ckCamposDiseñoAcabado2.Checked = true;
                    flowlayoutpanelasignarnombre.Controls.Add(panelCamposDiseñoAcabado2);

                }
                else
                {
                    ckCamposDiseñoAcabado2.Checked = false;
                    flowlayoutpanelasignarnombre.Controls.Remove(panelCamposDiseñoAcabado2);

                }

                //NUMEROS Y TIPOS - 1
                int CampNTipos1 = Convert.ToInt32(datalistadoCargarGrupoCampos.SelectedCells[13].Value.ToString());
                if (CampNTipos1 == 1)
                {
                    ckCamposNTipos1.Checked = true;
                    flowlayoutpanelasignarnombre.Controls.Add(panelCamposNTipos1);

                }
                else
                {
                    ckCamposNTipos1.Checked = false;
                    flowlayoutpanelasignarnombre.Controls.Remove(panelCamposNTipos1);

                }

                //NUMEROS Y TIPOS - 2
                int CampNTipos2 = Convert.ToInt32(datalistadoCargarGrupoCampos.SelectedCells[14].Value.ToString());
                if (CampNTipos2 == 1)
                {
                    ckCamposNTipos2.Checked = true;
                    flowlayoutpanelasignarnombre.Controls.Add(panelCamposNTipos2);

                }
                else
                {
                    ckCamposNTipos2.Checked = false;
                    flowlayoutpanelasignarnombre.Controls.Remove(panelCamposNTipos2);

                }

                //VARIOS - 1
                int CampVarios1 = Convert.ToInt32(datalistadoCargarGrupoCampos.SelectedCells[15].Value.ToString());
                if (CampVarios1 == 1)
                {
                    ckVariosO1.Checked = true;
                    flowlayoutpanelasignarnombre.Controls.Add(panelCamposVariosO1);

                }
                else
                {
                    ckVariosO1.Checked = false;
                    flowlayoutpanelasignarnombre.Controls.Remove(panelCamposVariosO1);

                }

                //VARIOS - 2
                int CampVarios2 = Convert.ToInt32(datalistadoCargarGrupoCampos.SelectedCells[16].Value.ToString());
                if (CampVarios2 == 1)
                {
                    ckVariosO2.Checked = true;
                    flowlayoutpanelasignarnombre.Controls.Add(panelCamposVariosO2);

                }
                else
                {
                    ckVariosO2.Checked = false;
                    flowlayoutpanelasignarnombre.Controls.Remove(panelCamposVariosO2);

                }

                //GENERALES
                int CampGenerales = Convert.ToInt32(datalistadoCargarGrupoCampos.SelectedCells[17].Value.ToString());
                if (CampGenerales == 1)
                {
                    ckGenerales.Checked = true;
                    flowlayoutpanelasignarnombre.Controls.Add(panelCamposGeneral);

                }
                else
                {
                    ckGenerales.Checked = false;
                    flowlayoutpanelasignarnombre.Controls.Remove(panelCamposGeneral);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void CargarCamposPredeterminados()
        {
            string asignarnombre = lblCodigo.Text;
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarCamposPredeterminados_AsignarNombre_Modelo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idmodelo", Convert.ToInt32(asignarnombre));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoCamposAsignarNombre.DataSource = dt;
                con.Close();

                if (datalistadoCamposAsignarNombre.RowCount == 0)
                {
                    MessageBox.Show("El modelo elegido no tiene detalles definidos, por favor defina los campos.", "Validación del Sistema", MessageBoxButtons.OK);
                    flowLayoutPanel.Controls.Clear();
                }
                else
                {
                    if (datalistadoCamposAsignarNombre.SelectedCells[1].Value != null)
                    {
                        cboTipoCaracteristicas1.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[1].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[2].Value != null)
                    {
                        cboTipoCaracteristicas2.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[2].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[3].Value != null)
                    {
                        cboTipoCaracteristicas3.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[3].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[4].Value != null)
                    {
                        cboTipoCaracteristicas4.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[4].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[5].Value != null)
                    {
                        cboTipoMedida1.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[5].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[6].Value != null)
                    {
                        cboTipoMedida2.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[6].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[7].Value != null)
                    {
                        cboTipoMedida3.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[7].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[8].Value != null)
                    {
                        cboTipoMedida4.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[8].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[9].Value != null)
                    {
                        cboTiposDiametros1.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[9].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[10].Value != null)
                    {
                        cboTiposDiametros2.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[10].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[11].Value != null)
                    {
                        cboTiposDiametros3.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[11].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[12].Value != null)
                    {
                        cboTiposDiametros4.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[12].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[13].Value != null)
                    {
                        cboTiposFormas1.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[13].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[14].Value != null)
                    {
                        cboTiposFormas2.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[14].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[15].Value != null)
                    {
                        cboTiposFormas3.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[15].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[16].Value != null)
                    {
                        cboTiposFormas4.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[16].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[17].Value != null)
                    {
                        cbooTipoEspesores1.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[17].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[18].Value != null)
                    {
                        cbooTipoEspesores2.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[18].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[19].Value != null)
                    {
                        cbooTipoEspesores3.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[19].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[20].Value != null)
                    {
                        cbooTipoEspesores4.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[20].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[21].Value != null)
                    {
                        cboTiposDiseñosAcabados1.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[21].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[22].Value != null)
                    {
                        cboTiposDiseñosAcabados2.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[22].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[23].Value != null)
                    {
                        cboTiposDiseñosAcabados3.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[23].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[24].Value != null)
                    {
                        cboTiposDiseñosAcabados4.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[24].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[25].Value != null)
                    {
                        cboTiposNTipos1.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[25].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[26].Value != null)
                    {
                        cboTiposNTipos2.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[26].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[27].Value != null)
                    {
                        cboTiposNTipos3.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[27].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[28].Value != null)
                    {
                        cboTiposNTipos4.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[28].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[29].Value != null)
                    {
                        cboTiposVariosO1.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[29].Value;
                    }

                    if (datalistadoCamposAsignarNombre.SelectedCells[30].Value != null)
                    {
                        cboTiposVariosO2.SelectedValue = datalistadoCamposAsignarNombre.SelectedCells[30].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CargarEstados_AtributosCks()
        {
            string asignarnombre = lblCodigo.Text;
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CargarCkAtributos", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idmodelo", Convert.ToInt32(asignarnombre));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadockatributos.DataSource = dt;
                con.Close();

                //ESTADO CHECKBOX CARACTERISTICA 1
                int ckestadocaracteristica1 = Convert.ToInt32(datalistadockatributos.SelectedCells[1].Value.ToString());
                if (ckestadocaracteristica1 == 1)
                {
                    ckCaracteristicaAtributo1.Checked = true;
                }
                else
                {
                    ckCaracteristicaAtributo1.Checked = false;
                }

                //ESTADO CHECKBOX CARACTERISTICA 2
                int ckestadocaracteristica2 = Convert.ToInt32(datalistadockatributos.SelectedCells[2].Value.ToString());
                if (ckestadocaracteristica2 == 1)
                {
                    ckCaracteristicaAtributo2.Checked = true;

                }
                else
                {
                    ckCaracteristicaAtributo2.Checked = false;
                }


                //ESTADO CHECKBOX CARACTERISTICA 3
                int ckestadocaracteristica3 = Convert.ToInt32(datalistadockatributos.SelectedCells[3].Value.ToString());
                if (ckestadocaracteristica3 == 1)
                {
                    ckCaracteristicaAtributo3.Checked = true;

                }
                else
                {
                    ckCaracteristicaAtributo3.Checked = false;
                }

                //ESTADO CHECKBOX CARACTERISTICA 4
                int ckestadocaracteristica4 = Convert.ToInt32(datalistadockatributos.SelectedCells[4].Value.ToString());
                if (ckestadocaracteristica4 == 1)
                {
                    ckCaracteristicaAtributo4.Checked = true;

                }
                else
                {
                    ckCaracteristicaAtributo4.Checked = false;
                }

                //ESTADO CHECKBOX MEDIDAS 1
                int ckestadomedidas1 = Convert.ToInt32(datalistadockatributos.SelectedCells[5].Value.ToString());
                if (ckestadomedidas1 == 1)
                {
                    ckMedidasAtributos1.Checked = true;

                }
                else
                {
                    ckMedidasAtributos1.Checked = false;
                }

                //ESTADO CHECKBOX MEDIDAS 2
                int ckestadomedidas2 = Convert.ToInt32(datalistadockatributos.SelectedCells[6].Value.ToString());
                if (ckestadomedidas2 == 1)
                {
                    ckMedidasAtributos2.Checked = true;
                }
                else
                {
                    ckMedidasAtributos2.Checked = false;
                }

                //ESTADO CHECKBOX MEDIDAS 3
                int ckestadomedidas3 = Convert.ToInt32(datalistadockatributos.SelectedCells[7].Value.ToString());
                if (ckestadomedidas3 == 1)
                {
                    ckMedidasAtributos3.Checked = true;
                }
                else
                {
                    ckMedidasAtributos3.Checked = false;
                }

                //ESTADO CHECKBOX MEDIDAS 4
                int ckestadomedidas4 = Convert.ToInt32(datalistadockatributos.SelectedCells[8].Value.ToString());
                if (ckestadomedidas4 == 1)
                {
                    ckMedidasAtributos4.Checked = true;
                }
                else
                {
                    ckMedidasAtributos4.Checked = false;
                }

                //ESTADO CHECKBOX DIAMETRO 1
                int ckestadodiametro1 = Convert.ToInt32(datalistadockatributos.SelectedCells[9].Value.ToString());
                if (ckestadodiametro1 == 1)
                {
                    ckDiametroAtributos1.Checked = true;

                }
                else
                {
                    ckDiametroAtributos1.Checked = false;
                }

                //ESTADO CHECKBOX DIAMETRO 2
                int ckestadodiametro2 = Convert.ToInt32(datalistadockatributos.SelectedCells[10].Value.ToString());
                if (ckestadodiametro2 == 1)
                {
                    ckDiametroAtributos2.Checked = true;
                }
                else
                {
                    ckDiametroAtributos2.Checked = false;
                }

                //ESTADO CHECKBOX DIAMETRO 3
                int ckestadodiametro3 = Convert.ToInt32(datalistadockatributos.SelectedCells[11].Value.ToString());
                if (ckestadodiametro3 == 1)
                {
                    ckDiametroAtributos3.Checked = true;
                }
                else
                {
                    ckDiametroAtributos3.Checked = false;
                }

                //ESTADO CHECKBOX DIAMETRO 4
                int ckestadodiametro4 = Convert.ToInt32(datalistadockatributos.SelectedCells[12].Value.ToString());
                if (ckestadodiametro4 == 1)
                {
                    ckDiametroAtributos4.Checked = true;
                }
                else
                {
                    ckDiametroAtributos4.Checked = false;
                }

                //ESTADO CHECKBOX FORMA 1
                int ckestadoforma1 = Convert.ToInt32(datalistadockatributos.SelectedCells[13].Value.ToString());
                if (ckestadoforma1 == 1)
                {
                    ckFormasAtributos1.Checked = true;

                }
                else
                {
                    ckFormasAtributos1.Checked = false;
                }

                //ESTADO CHECKBOX FORMA 2
                int ckestadoforma2 = Convert.ToInt32(datalistadockatributos.SelectedCells[14].Value.ToString());
                if (ckestadoforma2 == 1)
                {
                    ckFormasAtributos2.Checked = true;
                }
                else
                {
                    ckFormasAtributos2.Checked = false;
                }

                //ESTADO CHECKBOX FORMA 3
                int ckestadoforma3 = Convert.ToInt32(datalistadockatributos.SelectedCells[15].Value.ToString());
                if (ckestadoforma3 == 1)
                {
                    ckFormasAtributos3.Checked = true;
                }
                else
                {
                    ckFormasAtributos3.Checked = false;
                }

                //ESTADO CHECKBOX FORMA 4
                int ckestadoforma4 = Convert.ToInt32(datalistadockatributos.SelectedCells[16].Value.ToString());
                if (ckestadoforma4 == 1)
                {
                    ckFormasAtributos4.Checked = true;
                }
                else
                {
                    ckFormasAtributos4.Checked = false;
                }

                //ESTADO CHECKBOX ESPESORES 1
                int ckestadoespesores1 = Convert.ToInt32(datalistadockatributos.SelectedCells[17].Value.ToString());
                if (ckestadoespesores1 == 1)
                {
                    ckEspesoresAtributos1.Checked = true;

                }
                else
                {
                    ckEspesoresAtributos1.Checked = false;
                }

                //ESTADO CHECKBOX ESPESORES 2
                int ckestadoespesores2 = Convert.ToInt32(datalistadockatributos.SelectedCells[18].Value.ToString());
                if (ckestadoespesores2 == 1)
                {
                    ckEspesoresAtributos2.Checked = true;
                }
                else
                {
                    ckEspesoresAtributos2.Checked = false;
                }

                //ESTADO CHECKBOX ESPESORES 3
                int ckestadoespesores3 = Convert.ToInt32(datalistadockatributos.SelectedCells[19].Value.ToString());
                if (ckestadoespesores3 == 1)
                {
                    ckEspesoresAtributos3.Checked = true;
                }
                else
                {
                    ckEspesoresAtributos3.Checked = false;
                }

                //ESTADO CHECKBOX ESPESORES 4
                int ckestadoespesores4 = Convert.ToInt32(datalistadockatributos.SelectedCells[20].Value.ToString());
                if (ckestadoespesores4 == 1)
                {
                    ckEspesoresAtributos4.Checked = true;
                }
                else
                {
                    ckEspesoresAtributos4.Checked = false;
                }

                //ESTADO CHECKBOX DISEÑO ACABADO 1
                int ckestadodiseñoacabado1 = Convert.ToInt32(datalistadockatributos.SelectedCells[21].Value.ToString());
                if (ckestadodiseñoacabado1 == 1)
                {
                    ckDiseñoAcabadoAtributos1.Checked = true;

                }
                else
                {
                    ckDiseñoAcabadoAtributos1.Checked = false;
                }

                //ESTADO CHECKBOX DISEÑO ACABADO 2
                int ckestadodiseñoacabado2 = Convert.ToInt32(datalistadockatributos.SelectedCells[22].Value.ToString());
                if (ckestadodiseñoacabado2 == 1)
                {
                    ckDiseñoAcabadoAtributos2.Checked = true;
                }
                else
                {
                    ckDiseñoAcabadoAtributos2.Checked = false;
                }

                //ESTADO CHECKBOX DISEÑO ACABADO 3
                int ckestadodiseñoacabado3 = Convert.ToInt32(datalistadockatributos.SelectedCells[23].Value.ToString());
                if (ckestadodiseñoacabado3 == 1)
                {
                    ckDiseñoAcabadoAtributos3.Checked = true;
                }
                else
                {
                    ckDiseñoAcabadoAtributos3.Checked = false;
                }

                //ESTADO CHECKBOX DISEÑO ACABADO 4
                int ckestadodiseñoacabado4 = Convert.ToInt32(datalistadockatributos.SelectedCells[24].Value.ToString());
                if (ckestadodiseñoacabado4 == 1)
                {
                    ckDiseñoAcabadoAtributos4.Checked = true;
                }
                else
                {
                    ckDiseñoAcabadoAtributos4.Checked = false;
                }

                //ESTADO CHECKBOX TIPO N TIPOS 1
                int ckestadotipontipos1 = Convert.ToInt32(datalistadockatributos.SelectedCells[25].Value.ToString());
                if (ckestadotipontipos1 == 1)
                {
                    ckTiposNTiposAtributos1.Checked = true;

                }
                else
                {
                    ckTiposNTiposAtributos1.Checked = false;
                }

                //ESTADO CHECKBOX TIPO N TIPOS 2
                int ckestadotipontipos2 = Convert.ToInt32(datalistadockatributos.SelectedCells[26].Value.ToString());
                if (ckestadotipontipos2 == 1)
                {
                    ckTiposNTiposAtributos2.Checked = true;
                }
                else
                {
                    ckTiposNTiposAtributos2.Checked = false;
                }

                //ESTADO CHECKBOX TIPO N TIPOS 3
                int ckestadotipontipos3 = Convert.ToInt32(datalistadockatributos.SelectedCells[27].Value.ToString());
                if (ckestadotipontipos3 == 1)
                {
                    ckTiposNTiposAtributos3.Checked = true;
                }
                else
                {
                    ckTiposNTiposAtributos3.Checked = false;
                }

                //ESTADO CHECKBOX TIPO N TIPOS 4
                int ckestadotipontipos4 = Convert.ToInt32(datalistadockatributos.SelectedCells[28].Value.ToString());
                if (ckestadotipontipos4 == 1)
                {
                    ckTiposNTiposAtributos4.Checked = true;
                }
                else
                {
                    ckTiposNTiposAtributos4.Checked = false;
                }

                //ESTADO CHECKBOX TIPO VARIOS 1
                int ckestadotipovarios1 = Convert.ToInt32(datalistadockatributos.SelectedCells[29].Value.ToString());
                if (ckestadotipovarios1 == 1)
                {
                    ckVariosAtributos1.Checked = true;

                }
                else
                {
                    ckVariosAtributos1.Checked = false;
                }

                //ESTADO CHECKBOX TIPO VARIOS 2
                int ckestadotipovarios2 = Convert.ToInt32(datalistadockatributos.SelectedCells[30].Value.ToString());
                if (ckestadotipovarios2 == 1)
                {
                    ckVariosAtributos2.Checked = true;
                }
                else
                {
                    ckVariosAtributos2.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDefiniciónNombre_Click(object sender, EventArgs e)
        {

        }
    }
}


        
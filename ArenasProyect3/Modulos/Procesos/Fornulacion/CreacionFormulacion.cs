using DocumentFormat.OpenXml.Wordprocessing;
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
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Procesos.Fornulacion
{
    public partial class CreacionFormulacion : Form
    {
        //VARIABLES GENERALES
        int idartproducto = 0;
        int idartsemiproducido = 0;
        string textocodigoformulacion;
        private Cursor curAnterior = null;

        //Validacion del correlativo existencia
        bool ValidacionCorrelativoProducto = false;
        bool ValidacionCorrelativoSemiProducido = false;

        //CONSTRUCTOR DEL MANTENIMIENTO - MANTENIEMINTO DE FORMULAIONES
        public CreacionFormulacion()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI MANTENIMIENTOS DE FORMULAION
        private void CreacionFormulacion_Load(object sender, EventArgs e)
        {
            //CARGAR DEFINCIONES DE FORMUALCION
            BusquedaDatosPrincipales(cboDefinicionFormulacion, lblIdLinea);
        }

        //METODO PARA PINTAR DE COLORES LAS FILAS DE MI LSITADO
        public void alternarColorFilas(DataGridView dgv)
        {
            try
            {
                {
                    var withBlock = dgv;
                    withBlock.RowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(215, 227, 252);
                    withBlock.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //BUSQUEDA DE FORMULACION Y CARGA DE MI COMBO
        public void BusquedaDatosPrincipales(ComboBox cbo, Label lbl)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("CreacionFormulacion_MostrarTipos", con);
                comando.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.DisplayMember = "LINEA";
                cbo.ValueMember = "ID";
                DataRow row = dt.Rows[0];
                txtTipoFormulacion.Text = System.Convert.ToString(row["TIPO"]);
                lbl.Text = System.Convert.ToString(row["IdLinea"]);
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //EVENTO DE CAMBIO DE DATO EN EL COMBO DE MIS DEFINICIONES CARGADAS
        private void cboDefinicionFormulacion_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT DF.IdDefinicionFormulaciones AS [ID], L.IdLinea ,L.Descripcion AS [LINEA], DF.IdTipo, TF.Descripcion AS [TIPO] FROM DefinicionFormulaciones DF INNER JOIN LINEAS L ON L.IdLinea = DF.IdLinea INNER JOIN TipoFormulacion TF ON TF.IdTipoFormulacion = DF.IdTipo WHERE DF.Estado = 1 AND DF.IdDefinicionFormulaciones = @id ORDER BY L.IdLinea", con);
                comando.Parameters.AddWithValue("@id", cboDefinicionFormulacion.SelectedValue.ToString());
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtTipoFormulacion.Text = System.Convert.ToString(row["TIPO"]);
                    lblIdLinea.Text = System.Convert.ToString(row["IdLinea"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //RECURSOS PARA GUARDAR LA FORMULACION Y LA ACCION DE GUARDAR FORMULACION
        public void CrearCodigoFormulacion()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT IdFormulacion FROM Formulacion WHERE IdFormulacion =(SELECT max(IdFormulacion) FROM Formulacion)", con);
                da.Fill(dt);
                datalistadocodigoformulacion.DataSource = dt;
                con.Close();

                int lblultimaformulacion;
                textocodigoformulacion = "";

                if (datalistadocodigoformulacion.RowCount == 0)
                {
                    lblultimaformulacion = 0;
                }
                else
                {
                    lblultimaformulacion = Convert.ToInt32(datalistadocodigoformulacion.SelectedCells[0].Value.ToString());
                }

                if (Convert.ToString(lblultimaformulacion).Length == 5)
                {
                    lblultimaformulacion = lblultimaformulacion + 1;
                    textocodigoformulacion = "FM" + lblultimaformulacion++;
                }
                else if (Convert.ToString(lblultimaformulacion).Length == 4)
                {
                    lblultimaformulacion = lblultimaformulacion + 1;
                    textocodigoformulacion = "FM0" + lblultimaformulacion++;
                }
                else if (Convert.ToString(lblultimaformulacion).Length == 3)
                {
                    lblultimaformulacion = lblultimaformulacion + 1;
                    textocodigoformulacion = "FM00" + lblultimaformulacion++;
                }
                else if (Convert.ToString(lblultimaformulacion).Length == 2)
                {
                    lblultimaformulacion = lblultimaformulacion + 1;
                    textocodigoformulacion = "FM000" + lblultimaformulacion++;
                }
                else if (Convert.ToString(lblultimaformulacion).Length == 1)
                {
                    lblultimaformulacion = lblultimaformulacion + 1;
                    textocodigoformulacion = "FM0000" + lblultimaformulacion;
                }
                else if (Convert.ToString(lblultimaformulacion).Length == 0)
                {
                    textocodigoformulacion = "FM0000" + lblultimaformulacion;
                }

                lblCodigoFormulacion.Text = textocodigoformulacion;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //ESOCGER FORMUALCION Y CONTINUAR CON LA CARGA------------------------------------------------------------
        private void btnContinuar_Click(object sender, EventArgs e)
        {
            panelSeleccionDefinicion.Visible = false;

            //VISUALIZACION GENERAL DE CAMPOS
            lblTituloAdaptable.Text = cboDefinicionFormulacion.Text;
            lblTituloAdaptable.Visible = true;
            lineaPrincipal.Visible = true;
            lblCodigoFormulacion.Visible = true;
            txtCif.Visible = true;
            lblCIF.Visible = true;
            cboBusquedaFormulacion.Visible = true;
            cboBusquedaFormulacion.SelectedIndex = 0;
            txtFormulaciones.Visible = true;
            datalistadoFormulaciones.Visible = true;
            btnAnular.Visible = true;
            gbPlanosTecnicosSeguridad.Visible = true;
            btnAgregar.Visible = true;
            btnEditar.Visible = true;
            lblGuardar.Visible = true;
            lblEditar.Visible = true;
            lblEliminar.Visible = true;

            //CAPTURA DE DATOS
            lblTipoFormulacion.Text = txtTipoFormulacion.Text;
            lblIdDefinicion.Text = cboDefinicionFormulacion.SelectedValue.ToString();

            if (txtTipoFormulacion.Text == "CON SEMIPRODUCIDO")
            {
                //VISUALIZACION ESPECIFICA
                //VISUALIZACION DEL PRODUCTO
                lblProducto.Visible = true;
                txtProducto.Visible = true;
                btnAgregarProducto.Visible = true;
                lblCodigoProducto.Visible = true;

                //VISUALIZACION DEL SEMIPRODUCIDO
                lblSemiProducido.Visible = true;
                txtSemiProducido.Visible = true;
                btnAgregarSemiProducido.Visible = true;
                lblCodigoSemiProducido.Visible = true;
                lblHabilitacionSemi.Visible = false;
                imgOcultoSemiProducido.Visible = false;
                gbPlanosProducto.Visible = true;
                gbPlanosSemiProducido.Visible = true;
                lblPlanoSemiproducido.Visible = false;
                imgOcultoPlanoSemiProducido.Visible = false;
            }
            else
            {
                //VISUALIZACION ESPECIFICA
                //VISUALIZACION DEL PRODUCTO
                lblProducto.Visible = true;
                txtProducto.Visible = true;
                btnAgregarProducto.Visible = true;
                lblCodigoProducto.Visible = true;

                //VISUALIZACION DEL SEMIPRODUCIDO
                lblSemiProducido.Visible = false;
                txtSemiProducido.Visible = false;
                btnAgregarSemiProducido.Visible = false;
                lblCodigoSemiProducido.Visible = false;
                lblHabilitacionSemi.Visible = true;
                imgOcultoSemiProducido.Visible = true;
                gbPlanosProducto.Visible = true;
                gbPlanosSemiProducido.Visible = false;
                lblPlanoSemiproducido.Visible = true;
                imgOcultoPlanoSemiProducido.Visible = true;
            }

            MostrarFormulaciones(cboDefinicionFormulacion, datalistadoFormulaciones);
            alternarColorFilas(datalistadoFormulaciones);
        }

        //-----------------------------------------------------------------------FORMULACION--------------------------------------------------------------
        //LISTAR FORMULACIONES INGRESADAS
        public void MostrarFormulaciones(ComboBox cbo, DataGridView dgv)
        {
            try
            {
                if (cbo.Text.Contains("CON SEMIPRODUCIDO"))
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("CreacionFormulacion_MostrarConSemiProducido", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dgv.DataSource = dt;
                    con.Close();
                }
                else
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("CreacionFormulacion_MostrarSinSemiProducido", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dgv.DataSource = dt;
                    con.Close();
                }

                AjustesColunmasMostrarFormulaicones(dgv);
                alternarColorFilas(dgv);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //REORDENAMIENTO DE MI VISTA DE FORMULACIONES
        public void AjustesColunmasMostrarFormulaicones(DataGridView dgv)
        {
            if (cboDefinicionFormulacion.Text.Contains("CON SEMIPRODUCIDO"))
            {
                dgv.Columns[1].Visible = false;
                dgv.Columns[3].Visible = false;
                dgv.Columns[7].Visible = false;
                dgv.Columns[11].Visible = false;
                dgv.Columns[12].Visible = false;
                dgv.Columns[13].Visible = false;
                dgv.Columns[14].Visible = false;
                dgv.Columns[15].Visible = false;
                dgv.Columns[16].Visible = false;
                dgv.Columns[17].Visible = false;
                dgv.Columns[18].Visible = false;

                dgv.Columns[2].Width = 80;
                dgv.Columns[4].Width = 120;
                dgv.Columns[5].Width = 400;
                dgv.Columns[6].Width = 95;

                dgv.Columns[8].Width = 120;
                dgv.Columns[9].Width = 400;
                dgv.Columns[10].Width = 95;
            }
            else
            {
                dgv.Columns[1].Visible = false;
                dgv.Columns[3].Visible = false;
                dgv.Columns[7].Visible = false;
                dgv.Columns[8].Visible = false;
                dgv.Columns[9].Visible = false;
                dgv.Columns[10].Visible = false;
                dgv.Columns[11].Visible = false;

                dgv.Columns[2].Width = 80;
                dgv.Columns[4].Width = 120;
                dgv.Columns[5].Width = 695;
                dgv.Columns[6].Width = 150;
            }
            alternarColorFilas(dgv);
        }

        //CREACION DE UNA FORMULACION-------------------------------------------------------------------------
        //PRODUCTO---------------------------------------------------------------------------
        //BUSCAR LOS PLANOS ASOCIADOS AL PRODUCTO SELECCIOANDO
        public void MostrarPlanosSegunIdProducto(int id, DataGridView dgv)
        {
            try
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
                dgv.DataSource = dt;
                con.Close();
                dgv.Columns[0].Width = 55;
                dgv.Columns[1].Width = 90;
                dgv.Columns[2].Width = 325;
                alternarColorFilas(dgv);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //VISUALOIZAR PALNOS SELECCIOANDOS
        public void VisualizarPlanosSeleccionados(DataGridView dgv, TextBox txt)
        {
            try
            {
                if (dgv != null)
                {
                    if (dgv.CurrentRow != null)
                    {
                        string ruta = dgv.SelectedCells[2].Value.ToString();
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
                else if (txt != null)
                {
                    if (txt.Text != "")
                    {
                        string ruta = txt.Text;
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
                        MessageBox.Show("No hay un plano para abrir.", "Abrir Plano");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //FUNCION PARA CARGAR DOCUMENTOS
        public void CargarDocuemntos(TextBox txt)
        {
            openFileDialog2.InitialDirectory = "c:\\";
            openFileDialog2.Filter = "Todos los archivos (*.*)|*.*";
            openFileDialog2.FilterIndex = 1;
            openFileDialog2.RestoreDirectory = true;
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                txt.Text = openFileDialog2.FileName;
            }
        }

        //AGREGAR PRODUCTO
        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            cboBusquedaProductos.SelectedIndex = 0;
            panelBusquedaProducto.Visible = true;
            CargarProductos();
        }

        //VALIDACION PARA EL PRODUCTO
        public void ColorDescripcionProducto()
        {
            foreach (DataGridViewRow datorecuperado in datalistadoFormulaciones.Rows)
            {
                String codigoDatoRecuperado = datorecuperado.Index.ToString();
                string detalle = Convert.ToString(datorecuperado.Cells["DESCRIPCIÓN P."].Value);
                if (detalle == txtProducto.Text)
                {
                    txtProducto.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                else
                {
                    txtProducto.ForeColor = System.Drawing.Color.Green;
                }
            }
            return;
        }

        //METODO PARA CARGAR LOS PRODUCTOS TERMINADOS
        public void CargarProductos()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT Codcom AS [CÓDIGO], IdArt AS [C. ART], Descripcion AS [CÓDIGO BSS],Detalle AS [DESCRIPCIÓN] \r\nFROM PRODUCTOS WHERE Estado = 1", con);
                da.Fill(dt);
                datalistadoproductos.DataSource = dt;
                con.Close();
                datalistadoproductos.Columns[0].Width = 100;
                datalistadoproductos.Columns[2].Width = 90;
                datalistadoproductos.Columns[3].Width = 605;
                datalistadoproductos.Columns[1].Visible = false;
                alternarColorFilas(datalistadoproductos);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //SELECCIONAR LE PRODUCTO Y LELVARLO A MI FORMUALCION
        public void SeleccionarProducto(Label codigo, DataGridView dgv, TextBox producto, Panel panelproductos, TextBox codigoplano, TextBox rutaplano)
        {
            try
            {
                codigo.Text = dgv.SelectedCells[0].Value.ToString();
                idartproducto = Convert.ToInt32(dgv.SelectedCells[1].Value.ToString());
                producto.Text = dgv.SelectedCells[3].Value.ToString();
                panelproductos.Visible = false;
                codigoplano.Text = "";
                rutaplano.Text = "";
                MostrarPlanosSegunIdProducto(idartproducto, datalistadopdfProducto);
                ColorDescripcionProducto();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //SELECCIONAR LE PRODUCTO Y LELVARLO A MI FORMUALCION
        private void datalistadoproductos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoproductos.RowCount != 0)
            {
                SeleccionarProducto(lblCodigoProducto, datalistadoproductos, txtProducto, panelBusquedaProducto, txtCodigoPlanoProducto, txtRutaPlanoProducto);
            }
        }

        //SELECCIONAR UN PLANO DE PRODUCTO
        public void SeleccionarPlanoProducto(Label codigoPlano, TextBox codigoPlanoText, TextBox rutaPlanoText, DataGridView dgv)
        {
            try
            {
                codigoPlano.Text = dgv.SelectedCells[0].Value.ToString();
                codigoPlanoText.Text = dgv.SelectedCells[1].Value.ToString();
                rutaPlanoText.Text = dgv.SelectedCells[2].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //SELECCIONAR UN PLANO DE PRODUCTO
        private void datalistadopdfProducto_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadopdfProducto.RowCount != 0)
            {
                SeleccionarPlanoProducto(lblCodigoPlanoProducto, txtCodigoPlanoProducto, txtRutaPlanoProducto, datalistadopdfProducto);
            }
        }

        //VISUALIZAR PLANO
        private void btnVisualizarPlanoProducto_Click(object sender, EventArgs e)
        {
            VisualizarPlanosSeleccionados(datalistadopdfProducto, null);
        }

        //LIMPIAR PLANO SELECCIONADO
        public void LimpiarPlanoProducto(TextBox codigoplano, TextBox rutaplano, Label codigoplanoprod)
        {
            codigoplano.Text = "";
            rutaplano.Text = "";
            codigoplanoprod.Text = "*";
        }

        //LIMPIAR PLANO SELECCIONADO
        private void btnLimpiarPlanoProducto_Click(object sender, EventArgs e)
        {
            LimpiarPlanoProducto(txtCodigoPlanoProducto, txtRutaPlanoProducto, lblCodigoPlanoProducto);
        }

        //SALIR DE LA BUSQUEDA DE MI PRODUCTO
        private void btnSalirBusquedaProducto_Click(object sender, EventArgs e)
        {
            cboBusquedaProductos.SelectedIndex = 0;
            txtBusquedaProducto.Text = "";
            panelBusquedaProducto.Visible = false;
        }

        //SALIR DE LA BUSQUEDA DE MI PRODUCTO
        private void lblSalirBusquedaProducto_Click(object sender, EventArgs e)
        {
            cboBusquedaProductos.SelectedIndex = 0;
            txtBusquedaProducto.Text = "";
            panelBusquedaProducto.Visible = false;
        }

        //SEMIPRODUCIDO------------------------------------------------------------------------
        //AGREGAR PRODUCTOAGREGAR SEMIPRODUCIDO
        private void btnAgregarSemiProducido_Click(object sender, EventArgs e)
        {
            cboBusquedaSemiProducido.SelectedIndex = 0;
            panelBusquedaSemiProducido.Visible = true;
            CargarSemiProducido();
        }

        public void ColorDescripcionSemiProducido()
        {
            foreach (DataGridViewRow datorecuperado in datalistadoFormulaciones.Rows)
            {
                String codigoDatoRecuperado = datorecuperado.Index.ToString();
                string detalle = Convert.ToString(datorecuperado.Cells["DESCRIPCIÓN S."].Value);
                if (detalle == txtSemiProducido.Text)
                {
                    txtSemiProducido.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                else
                {
                    txtSemiProducido.ForeColor = System.Drawing.Color.Green;
                }
            }
            return;
        }

        //METODO PARA CARGAR LOS PRODUCTOS SEMIPRODUCIDO
        public void CargarSemiProducido()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT Codcom AS [CÓDIGO], IdArt AS [C. ART], Descripcion AS [CÓDIGO BSS],Detalle AS [DESCRIPCIÓN] FROM PRODUCTOS WHERE Estado = 1", con);
                da.Fill(dt);
                datalistadoSemiProducido.DataSource = dt;
                con.Close();
                datalistadoSemiProducido.Columns[0].Width = 100;
                datalistadoSemiProducido.Columns[2].Width = 90;
                datalistadoSemiProducido.Columns[3].Width = 605;
                datalistadoSemiProducido.Columns[1].Visible = false;
                alternarColorFilas(datalistadoSemiProducido);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //SELECCIONAR EL SEMIPRODUCIDO LELVARLO A MI FORMUALCION
        public void SeleccionarSemiProducido(object sender, DataGridViewCellEventArgs e, Label codigo, TextBox semiproducdo, Panel panel, TextBox codigoplanosemi, TextBox rutaplanosemi, DataGridView dgv)
        {
            try
            {
                codigo.Text = dgv.SelectedCells[0].Value.ToString();
                idartsemiproducido = Convert.ToInt32(dgv.SelectedCells[1].Value.ToString());
                semiproducdo.Text = dgv.SelectedCells[3].Value.ToString();
                panel.Visible = false;
                codigoplanosemi.Text = "";
                rutaplanosemi.Text = "";
                MostrarPlanosSegunIdProducto(idartsemiproducido, datalistadopdfSemiProducido);
                ColorDescripcionSemiProducido();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //SELECCIONAR EL SEMIPRODUCIDO LELVARLO A MI FORMUALCION
        private void datalistadoSemiProducido_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoSemiProducido.RowCount != 0)
            {
                SeleccionarSemiProducido(sender, e, lblCodigoSemiProducido, txtSemiProducido, panelBusquedaSemiProducido, txtCodigoPlanoSemiProducido, txtRutaPlanoSemiProducido, datalistadoSemiProducido);
            }
        }

        //SELECCIONAR UN PLANO DEL SEMIPRODUCIDO
        public void SeleccionarplanoSemiProducido(object sender, DataGridViewCellEventArgs e, Label codigoplansem, TextBox codigoplanosemi, TextBox rutaplanosemipr)
        {
            try
            {
                lblCodigoPlanoSemiProducido.Text = datalistadopdfSemiProducido.SelectedCells[0].Value.ToString();
                txtCodigoPlanoSemiProducido.Text = datalistadopdfSemiProducido.SelectedCells[1].Value.ToString();
                txtRutaPlanoSemiProducido.Text = datalistadopdfSemiProducido.SelectedCells[2].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //SELECCIONAR UN PLANO DEL SEMIPRODUCIDO
        private void datalistadopdfSemiProducido_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadopdfSemiProducido.RowCount != 0)
            {
                SeleccionarplanoSemiProducido(sender, e, lblCodigoPlanoSemiProducido, txtCodigoPlanoSemiProducido, txtRutaPlanoSemiProducido);
            }
        }

        //VISUALIZAR PLANO
        private void btnAbrirPdfSemiProducido_Click(object sender, EventArgs e)
        {
            VisualizarPlanosSeleccionados(datalistadopdfSemiProducido, null);
        }

        //LIMPIAR PLANO SELECCIONADO
        public void LimpiarPlanoSemiProducido(TextBox codigoplanosemi, TextBox rutaplanosemi, Label codigoplanoprod)
        {
            codigoplanosemi.Text = "";
            rutaplanosemi.Text = "";
            codigoplanoprod.Text = "*";
        }

        //LIMPIAR PLANO SELECCIONADO
        private void btnLimpiarPlanoSemiPorducido_Click(object sender, EventArgs e)
        {
            LimpiarPlanoSemiProducido(txtCodigoPlanoSemiProducido, txtRutaPlanoSemiProducido, lblCodigoPlanoSemiProducido);
        }

        //BOTON PARA SALIR D ELA BUSQUEDA DE MI SEMI-PRODUCIDO
        private void btnBusquedaSemiProducido_Click(object sender, EventArgs e)
        {
            cboBusquedaSemiProducido.SelectedIndex = 0;
            txtBusquedaSemiProducido.Text = "";
            panelBusquedaSemiProducido.Visible = false;
        }

        //LBL PARA SALIR D ELA BUSQUEDA DE MI SEMI-PRODUCIDO
        private void lblBusquedaSemiProducido_Click(object sender, EventArgs e)
        {
            cboBusquedaSemiProducido.SelectedIndex = 0;
            txtBusquedaSemiProducido.Text = "";
            panelBusquedaSemiProducido.Visible = false;
        }

        //---------------------------------------------------------------------------------------------------
        //PLANOS DE SEGURIDAD Y TECNICOS
        private void btnCargarPdfHojaTecnica_Click(object sender, EventArgs e)
        {
            CargarDocuemntos(txtFileHojaTecnica);
        }

        //PLANOS DE SEGURIDAD Y TECNICOS
        private void btnCargarPdfHojaSeguridad_Click(object sender, EventArgs e)
        {
            CargarDocuemntos(txtFileHojaSeguridad);
        }

        //VISUALIZAR PLANO TECNICO ADJUNTADO
        private void btnAbrirPdfPlanoTecnico_Click(object sender, EventArgs e)
        {
            VisualizarPlanosSeleccionados(null, txtFileHojaTecnica);
        }

        //VISUALIZAR PLANO DE SEGURIDAD ADJUNTADO
        private void btnAbrirPdfPlanoSeguridad_Click(object sender, EventArgs e)
        {
            VisualizarPlanosSeleccionados(null, txtFileHojaSeguridad);
        }

        //ACCIONES DE LA FORMUALCION---------------------------------------------------------------------------
        //AGREGAR FORMULACION
        public void AgregarFormulacion(ComboBox cbodefinicionformulacion, TextBox producto, TextBox semiproducido, TextBox hojatecnica, TextBox hojaseguridad, decimal Cif, Label codigoplanoproducto, Label codigoplanosemiproducido, int definicion)
        {
            if (cbodefinicionformulacion.Text.Contains("CON SEMIPRODUCIDO"))
            {
                if (producto.Text == "" || semiproducido.Text == "")
                {
                    MessageBox.Show("Debe seleccionar todos los datos necesarios (producto, semiproducido, plano del producto y plano del semiproducido), solo se selecciona el semiproducido si aplica.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("CreacionFormulacion_Insertar", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        CrearCodigoFormulacion();
                        cmd.Parameters.AddWithValue("@codigoFormulacion", textocodigoformulacion);
                        cmd.Parameters.AddWithValue("@idProducto", idartproducto);
                        cmd.Parameters.AddWithValue("@idsemiproducido", idartsemiproducido);

                        //plano hoja tecnica
                        if (hojatecnica.Text == "")
                        {
                            cmd.Parameters.AddWithValue("@rutaPlanoTecnico", DBNull.Value);
                        }
                        else
                        {
                            string RutaNew = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Procesos\PlanosTecnicos\" + textocodigoformulacion + " - " + "Plano Tecnico" + ".pdf";
                            string RutaOld = hojatecnica.Text;
                            File.Copy(RutaOld, RutaNew);
                            cmd.Parameters.AddWithValue("@rutaPlanoTecnico", RutaNew);
                        }

                        //plano hoja de seguridad
                        if (hojaseguridad.Text == "")
                        {
                            cmd.Parameters.AddWithValue("@rutaPlanoSeguridad", DBNull.Value);
                        }
                        else
                        {
                            string RutaNew = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Procesos\PlanosSeguridad\" + textocodigoformulacion + " - " + "Plano Seguridad" + ".pdf";
                            string RutaOld = hojaseguridad.Text;
                            File.Copy(RutaOld, RutaNew);
                            cmd.Parameters.AddWithValue("@rutaPlanoSeguridad", RutaNew);
                        }

                        cmd.Parameters.AddWithValue("@cif", Cif);

                        if (codigoplanoproducto.Text == "*")
                        {
                            cmd.Parameters.AddWithValue("@idPlanoProducto", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@idPlanoProducto", codigoplanoproducto.Text);
                        }

                        if (codigoplanosemiproducido.Text == "*")
                        {
                            cmd.Parameters.AddWithValue("@idPlanoSemiproducido", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@idPlanoSemiproducido", codigoplanosemiproducido.Text);
                        }

                        cmd.Parameters.AddWithValue("@idDefinicionFormulacion", definicion);

                        cmd.ExecuteNonQuery();
                        con.Close();
                        MostrarFormulaciones(cbodefinicionformulacion, datalistadoFormulaciones);
                        MessageBox.Show("Registro ingresado exitosamente.", "Nueva Formulación", MessageBoxButtons.OK);

                        txtProducto.Text = "";
                        codigoplanoproducto.Text = "*********";

                        txtSemiProducido.Text = "";
                        codigoplanosemiproducido.Text = "*********";

                        txtCodigoPlanoProducto.Text = "";
                        txtRutaPlanoProducto.Text = "";
                        codigoplanoproducto.Text = "*";

                        txtCodigoPlanoSemiProducido.Text = "";
                        txtRutaPlanoSemiProducido.Text = "";
                        codigoplanosemiproducido.Text = "*";

                        txtFileHojaSeguridad.Text = "";
                        txtFileHojaTecnica.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + " " + "Debe seleccionar todos los datos necesarios (producto, semiproducido, plano del producto y plano del semiproducido), solo se selecciona el semiproducido si aplica.", "Validación del Sistema", MessageBoxButtons.OK);

                    }
                }
            }
            else
            {
                if (txtProducto.Text == "")
                {
                    MessageBox.Show("Debe seleccionar todos los datos necesarios (producto y plano del producto), solo se selecciona el semiproducido si aplica.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("CreacionFormulacion_Insertar", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        CrearCodigoFormulacion();
                        cmd.Parameters.AddWithValue("@codigoFormulacion", textocodigoformulacion);
                        cmd.Parameters.AddWithValue("@idProducto", idartproducto);
                        cmd.Parameters.AddWithValue("@idsemiproducido", DBNull.Value);

                        //plano hoja tecnica
                        if (hojatecnica.Text == "")
                        {
                            cmd.Parameters.AddWithValue("@rutaPlanoTecnico", DBNull.Value);
                        }
                        else
                        {
                            string RutaNew = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Procesos\PlanosTecnicos\" + textocodigoformulacion + " - " + "Plano Tecnico" + ".pdf";
                            string RutaOld = hojatecnica.Text;
                            File.Copy(RutaOld, RutaNew);
                            cmd.Parameters.AddWithValue("@rutaPlanoTecnico", RutaNew);
                        }

                        //plano hoja de seguridad
                        if (hojaseguridad.Text == "")
                        {
                            cmd.Parameters.AddWithValue("@rutaPlanoSeguridad", DBNull.Value);
                        }
                        else
                        {
                            string RutaNew = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Procesos\PlanosSeguridad\" + textocodigoformulacion + " - " + "Plano Seguridad" + ".pdf";
                            string RutaOld = hojaseguridad.Text;
                            File.Copy(RutaOld, RutaNew);
                            cmd.Parameters.AddWithValue("@rutaPlanoSeguridad", RutaNew);
                        }

                        cmd.Parameters.AddWithValue("@cif", Cif);

                        if (codigoplanoproducto.Text == "*")
                        {
                            cmd.Parameters.AddWithValue("@idPlanoProducto", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@idPlanoProducto", codigoplanoproducto.Text);
                        }

                        if (codigoplanosemiproducido.Text == "*")
                        {
                            cmd.Parameters.AddWithValue("@idPlanoSemiproducido", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@idPlanoSemiproducido", codigoplanosemiproducido.Text);
                        }

                        cmd.Parameters.AddWithValue("@idDefinicionFormulacion", definicion);

                        cmd.ExecuteNonQuery();
                        con.Close();
                        MostrarFormulaciones(cbodefinicionformulacion, datalistadoFormulaciones);
                        MessageBox.Show("Registro ingresado exitosamente.", "Nueva Formulación", MessageBoxButtons.OK);

                        producto.Text = "";
                        codigoplanoproducto.Text = "*********";

                        semiproducido.Text = "";
                        codigoplanosemiproducido.Text = "*********";

                        txtCodigoPlanoProducto.Text = "";
                        txtRutaPlanoProducto.Text = "";
                        codigoplanoproducto.Text = "*";

                        txtCodigoPlanoSemiProducido.Text = "";
                        txtRutaPlanoSemiProducido.Text = "";
                        codigoplanosemiproducido.Text = "*";

                        hojaseguridad.Text = "";
                        hojatecnica.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + " " + "Debe seleccionar todos los datos necesarios (producto, semiproducido, plano del producto y plano del semiproducido), solo se selecciona el semiproducido si aplica.", "Validación del Sistema", MessageBoxButtons.OK);
                    }
                }
            }
        }

        //EVENTO DE GUARDAR DEL BOTON DE AGREGAR
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AgregarFormulacion(cboDefinicionFormulacion, txtProducto, txtSemiProducido, txtFileHojaTecnica, txtFileHojaSeguridad, Convert.ToDecimal(txtCif.Text), lblCodigoPlanoProducto, lblCodigoPlanoSemiProducido, Convert.ToInt32(lblIdDefinicion.Text));
        }

        //EDITAR LA FORMULACION
        public void EditarFormulacion(DataGridView dgv, string codigoplanoproducto, string codigoplanosemiproducido)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea editar esta formulación?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                try
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("CreacionFormulacion_Editar", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idFormulacion", Convert.ToInt32(dgv.SelectedCells[1].Value.ToString()));

                    if (codigoplanoproducto == "*")
                    {
                        cmd.Parameters.AddWithValue("@idPlanoProducto", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@idPlanoProducto", Convert.ToString(codigoplanoproducto));
                    }

                    if (codigoplanosemiproducido == "*")
                    {
                        cmd.Parameters.AddWithValue("@idPlanoSemiProducido", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@idPlanoSemiProducido", Convert.ToString(codigoplanosemiproducido));
                    }

                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Se editó correctamente la formulación seleccionada.", "Validación del Sistema", MessageBoxButtons.OK);
                    MostrarFormulaciones(cboDefinicionFormulacion, datalistadoFormulaciones);

                    txtProducto.Text = "";
                    lblCodigoProducto.Text = "*********";

                    txtSemiProducido.Text = "";
                    lblCodigoSemiProducido.Text = "*********";

                    txtCodigoPlanoProducto.Text = "";
                    txtRutaPlanoProducto.Text = "";
                    lblCodigoPlanoProducto.Text = "*";

                    txtCodigoPlanoSemiProducido.Text = "";
                    txtRutaPlanoSemiProducido.Text = "";
                    lblCodigoPlanoSemiProducido.Text = "*";

                    txtFileHojaSeguridad.Text = "";
                    txtFileHojaTecnica.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //EDITAR LA FORMULACION
        private void btnEditar_Click(object sender, EventArgs e)
        {
            EditarFormulacion(datalistadoFormulaciones, Convert.ToString(lblCodigoPlanoProducto.Text), Convert.ToString(lblCodigoPlanoSemiProducido.Text));
        }

        //ANULAR FORMULACION
        public void AnularFormulacion(DataGridView dgv, ComboBox cbo)
        {
            if (dgv.RowCount != 0)
            {
                DialogResult boton = MessageBox.Show("¿Realmente desea anular esta formulación?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("CreacionoFormulacion_Anular", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idFormulacion", Convert.ToInt32(dgv.SelectedCells[1].Value.ToString()));

                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Se eliminó correctamente la formulación seleccionada.", "Validación del Sistema", MessageBoxButtons.OK);
                        MostrarFormulaciones(cbo, dgv);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una formulación para poder anularla.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //ANULAR FORMULACION
        private void btnAnular_Click(object sender, EventArgs e)
        {
            AnularFormulacion(datalistadoFormulaciones, cboDefinicionFormulacion);
        }

        //COMPARAR FORMULACION Y COPAIR ATRIBUTOS
        private void btnCompararMateriales_Click(object sender, EventArgs e)
        {
            panelBusquedaCopiaFormulaciones.Visible = true;
            cboBusquedaCopiaFormulacion.SelectedIndex = 0;
        }
        //-----------------------------------------------------------------------------------------------------

        //ACCIONES DE MI FORMULACION INGRESADA--------------------------------------
        //PASAR POR ENCIMA DE MI ICONO
        private void datalistadoFormulaciones_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoFormulaciones.Columns[e.ColumnIndex].Name == "SELECCIONAR")
            {
                this.datalistadoFormulaciones.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoFormulaciones.Cursor = curAnterior;
            }
        }

        //SELECCIONAR UN FOMRULARIO Y CARGAR SUS DATOS
        private void datalistadoFormulaciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoFormulaciones.RowCount != 0)
            {
                DataGridViewColumn currentColumn = datalistadoFormulaciones.Columns[e.ColumnIndex];

                if (txtTipoFormulacion.Text == "CON SEMIPRODUCIDO")
                {
                    //SI SE PRECIONA SOBRE LA COLUMNA CON EL NOMBRE SELECCIOANDO
                    if (currentColumn.Name == "SELECCIONAR")
                    {
                        panelActividades.Visible = true;
                        panelVisibleActividades.Visible = false;
                        panelVisibleMateriales.Visible = false;
                        textocodigoformulacion = datalistadoFormulaciones.SelectedCells[2].Value.ToString();
                        lblCodigoFormulacionVision.Text = datalistadoFormulaciones.SelectedCells[2].Value.ToString();
                        lblCodigoProductoActividades.Text = datalistadoFormulaciones.SelectedCells[4].Value.ToString();
                        txtProductoActividades.Text = datalistadoFormulaciones.SelectedCells[5].Value.ToString();
                        lblMedidaProductoActividades.Text = datalistadoFormulaciones.SelectedCells[6].Value.ToString();
                        lblCodigoSemiProducidoActividades.Text = datalistadoFormulaciones.SelectedCells[8].Value.ToString();
                        txtSemiProducidoActividades.Text = datalistadoFormulaciones.SelectedCells[9].Value.ToString();
                        lblMedidaSemiProducidoActividades.Text = datalistadoFormulaciones.SelectedCells[10].Value.ToString();
                        txtDetallesPlanoRutaProducto.Text = datalistadoFormulaciones.SelectedCells[15].Value.ToString();
                        txtDetallesPlanoRutaSemiProducido.Text = datalistadoFormulaciones.SelectedCells[18].Value.ToString();

                        MostrarDetalleFormulacionesProducto(datalistadoactividadesproducto, textocodigoformulacion);
                        MostrarMaterialFormulacionesProducto(datalistadomaterialproducto, textocodigoformulacion);
                        datalistadoactividadesproducto.Size = new Size(1108, 126);
                        datalistadomaterialproducto.Size = new Size(628, 126);

                        MostrarDetalleFormulacionesSemiProducido(datalistadoactividadsemiproducido, textocodigoformulacion);
                        MostrarMaterialFormulacionesSemiProducido(datalistadomaterialsemiproducido, textocodigoformulacion);
                        datalistadoactividadsemiproducido.Size = new Size(1108, 126);
                        datalistadomaterialsemiproducido.Size = new Size(628, 126);
                    }
                }
                else
                {
                    if (currentColumn.Name == "SELECCIONAR")
                    {
                        panelActividades.Visible = true;
                        panelVisibleActividades.Visible = true;
                        panelVisibleMateriales.Visible = true;
                        textocodigoformulacion = datalistadoFormulaciones.SelectedCells[2].Value.ToString();
                        lblCodigoFormulacionVision.Text = datalistadoFormulaciones.SelectedCells[2].Value.ToString();
                        lblCodigoProductoActividades.Text = datalistadoFormulaciones.SelectedCells[4].Value.ToString();
                        txtProductoActividades.Text = datalistadoFormulaciones.SelectedCells[5].Value.ToString();
                        lblMedidaProductoActividades.Text = datalistadoFormulaciones.SelectedCells[6].Value.ToString();
                        txtDetallesPlanoRutaProducto.Text = datalistadoFormulaciones.SelectedCells[11].Value.ToString();

                        MostrarDetalleFormulacionesProducto(datalistadoactividadesproducto, textocodigoformulacion);
                        MostrarMaterialFormulacionesProducto(datalistadomaterialproducto, textocodigoformulacion);
                        datalistadoactividadesproducto.Size = new Size(1108, 170);
                        datalistadomaterialproducto.Size = new Size(628, 170);
                        datalistadoactividadsemiproducido.Size = new Size(1108, 126);
                        datalistadomaterialsemiproducido.Size = new Size(628, 126);
                    }
                }
            }
        }

        //SELECCIONAR UNA FOMRULACION CON DOBLE CLICK
        private void datalistadoFormulaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoFormulaciones.RowCount != 0)
            {
                if (txtTipoFormulacion.Text == "CON SEMIPRODUCIDO")
                {
                    lblCodigoPlanoProducto.Text = "*";
                    lblCodigoPlanoSemiProducido.Text = "*";
                    textocodigoformulacion = datalistadoFormulaciones.SelectedCells[2].Value.ToString();
                    lblCodigoFormulacion.Text = datalistadoFormulaciones.SelectedCells[2].Value.ToString();
                    idartproducto = Convert.ToInt32(datalistadoFormulaciones.SelectedCells[3].Value.ToString());
                    lblCodigoProducto.Text = datalistadoFormulaciones.SelectedCells[4].Value.ToString();
                    txtProducto.Text = datalistadoFormulaciones.SelectedCells[5].Value.ToString();
                    idartsemiproducido = Convert.ToInt32(datalistadoFormulaciones.SelectedCells[7].Value.ToString());
                    lblCodigoSemiProducido.Text = datalistadoFormulaciones.SelectedCells[8].Value.ToString();
                    txtSemiProducido.Text = datalistadoFormulaciones.SelectedCells[9].Value.ToString();

                    txtCodigoPlanoProducto.Text = datalistadoFormulaciones.SelectedCells[14].Value.ToString();
                    txtRutaPlanoProducto.Text = datalistadoFormulaciones.SelectedCells[15].Value.ToString();
                    txtCodigoPlanoSemiProducido.Text = datalistadoFormulaciones.SelectedCells[17].Value.ToString();
                    txtRutaPlanoSemiProducido.Text = datalistadoFormulaciones.SelectedCells[18].Value.ToString();

                    txtFileHojaTecnica.Text = datalistadoFormulaciones.SelectedCells[11].Value.ToString();
                    txtFileHojaSeguridad.Text = datalistadoFormulaciones.SelectedCells[12].Value.ToString();

                    MostrarPlanosSegunIdProducto(idartproducto, datalistadopdfProducto);
                    alternarColorFilas(datalistadopdfProducto);
                    MostrarPlanosSegunIdProducto(idartsemiproducido, datalistadopdfSemiProducido);
                    alternarColorFilas(datalistadopdfSemiProducido);

                    ColorDescripcionProducto();
                    ColorDescripcionSemiProducido();
                }
                else
                {
                    lblCodigoPlanoProducto.Text = "*";
                    lblCodigoPlanoSemiProducido.Text = "*";
                    textocodigoformulacion = datalistadoFormulaciones.SelectedCells[2].Value.ToString();
                    lblCodigoFormulacion.Text = datalistadoFormulaciones.SelectedCells[2].Value.ToString();
                    idartproducto = Convert.ToInt32(datalistadoFormulaciones.SelectedCells[3].Value.ToString());
                    lblCodigoProducto.Text = datalistadoFormulaciones.SelectedCells[4].Value.ToString();
                    txtProducto.Text = datalistadoFormulaciones.SelectedCells[5].Value.ToString();

                    txtCodigoPlanoProducto.Text = datalistadoFormulaciones.SelectedCells[10].Value.ToString();
                    txtRutaPlanoProducto.Text = datalistadoFormulaciones.SelectedCells[11].Value.ToString();

                    txtFileHojaTecnica.Text = datalistadoFormulaciones.SelectedCells[7].Value.ToString();
                    txtFileHojaSeguridad.Text = datalistadoFormulaciones.SelectedCells[8].Value.ToString();

                    MostrarPlanosSegunIdProducto(idartproducto, datalistadopdfProducto);
                    alternarColorFilas(datalistadopdfProducto);

                    ColorDescripcionProducto();
                }
            }
        }

        //LIMPIAR BUSQUEDA PARA REALIZARLO POR OTRO CRITERIO
        private void cboBusquedaFormulacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFormulaciones.Text = "";
        }

        //fFUNCION PARA BUSCAR POR CRITERIOS
        public void FiltrarFormulaciones(ComboBox cbo, DataGridView dgv, string busquedaformulaciones)
        {
            try
            {
                if (busquedaformulaciones == "")
                {
                    MostrarFormulaciones(cboDefinicionFormulacion, datalistadoFormulaciones);
                }
                else
                {
                    if (cboDefinicionFormulacion.Text.Contains("CON SEMIPRODUCIDO"))
                    {
                        if (cbo.Text == "DESCRIPCIÓN")
                        {
                            DataTable dt = new DataTable();
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("CreacionFormulacion_BuscarFormulacionSemiPorDescripcion", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@descripcion", busquedaformulaciones);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                            dgv.DataSource = dt;
                            con.Close();
                            ReordenarSemiProducida(dgv);
                            alternarColorFilas(dgv);
                        }
                        else if (cbo.Text == "CÓDIGO FORM.")
                        {
                            DataTable dt = new DataTable();
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("CreacionFormulacion_BuscarFormulacionSemiPorCodigo", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@codigo", busquedaformulaciones);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                            dgv.DataSource = dt;
                            con.Close();
                            ReordenarSemiProducida(dgv);
                            alternarColorFilas(dgv);
                        }
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
                            cmd = new SqlCommand("CreacionFormulacion_BuscarFormulacionPorDescripcion", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@descripcion", busquedaformulaciones);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                            datalistadoFormulaciones.DataSource = dt;
                            ReordenarProducto(datalistadoFormulaciones);
                            con.Close();
                        }
                        else if (cbo.Text == "CÓDIGO FORM.")
                        {
                            DataTable dt = new DataTable();
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("CreacionFormulacion_BuscarFormulacionPorCodigo", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@codigo", busquedaformulaciones);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                            datalistadoFormulaciones.DataSource = dt;
                            ReordenarProducto(datalistadoFormulaciones);
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //REORDENAR MI LISTADO DE FORMULACIONES SEMIPRODUCIDO
        public void ReordenarSemiProducida(DataGridView dgv)
        {
            dgv.Columns[1].Visible = false;
            dgv.Columns[3].Visible = false;
            dgv.Columns[7].Visible = false;
            dgv.Columns[11].Visible = false;
            dgv.Columns[12].Visible = false;
            dgv.Columns[13].Visible = false;
            dgv.Columns[14].Visible = false;
            dgv.Columns[15].Visible = false;
            dgv.Columns[16].Visible = false;
            dgv.Columns[17].Visible = false;
            dgv.Columns[18].Visible = false;

            dgv.Columns[2].Width = 80;
            dgv.Columns[4].Width = 120;
            dgv.Columns[5].Width = 400;
            dgv.Columns[6].Width = 95;

            dgv.Columns[8].Width = 120;
            dgv.Columns[9].Width = 400;
            dgv.Columns[10].Width = 95;
        }

        //REORDENAR MI LISTADO DE FORMULACIONES PRODUCTO
        public void ReordenarProducto(DataGridView dgv)
        {
            dgv.Columns[1].Visible = false;
            dgv.Columns[3].Visible = false;
            dgv.Columns[7].Visible = false;
            dgv.Columns[8].Visible = false;
        }

        //fFUNCION PARA BUSCAR POR CRITERIOS
        private void txtFormulaciones_TextChanged(object sender, EventArgs e)
        {
            FiltrarFormulaciones(cboBusquedaFormulacion, datalistadoFormulaciones, txtFormulaciones.Text);
        }

        //LIMPIAR BUSQUEDA PARA REALIZARLO POR OTRO CRITERIO
        private void cboBusquedaProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusquedaProducto.Text = "";
        }

        //BÚSQUEDA DE PRODUCTO
        private void txtBusquedaProducto_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboBusquedaProductos.Text == "DESCRIPCIÓN")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("CreacionFormulacion_BuscarProductoPorDescripcion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@descripcion", txtBusquedaProducto.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoproductos.DataSource = dt;
                    con.Close();
                    ReordenarProductoBusqueda(datalistadoproductos);
                    alternarColorFilas(datalistadoproductos);
                }
                else if (cboBusquedaProductos.Text == "CÓDIGO")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("CreacionFormulacion_BuscarProductoPorCodigo", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codigo", txtBusquedaProducto.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoproductos.DataSource = dt;
                    con.Close();
                    ReordenarProductoBusqueda(datalistadoproductos);
                    alternarColorFilas(datalistadoproductos);
                }
                else if (cboBusquedaProductos.Text == "CÓDIGO BSS")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("CreacionFormulacion_BuscarProductoPorCodigoBSS", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codigoBSS", txtBusquedaProducto.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoproductos.DataSource = dt;
                    con.Close();
                    ReordenarProductoBusqueda(datalistadoproductos);
                    alternarColorFilas(datalistadoproductos);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }  
        }

        //REORDENAR PRODUCTO
        public void ReordenarProductoBusqueda(DataGridView DRV)
        {
            DRV.Columns[0].Width = 100;
            DRV.Columns[2].Width = 90;
            DRV.Columns[3].Width = 605;
            DRV.Columns[1].Visible = false;
        }

        //LIMPIAR BUSQUEDA PARA REALIZARLO POR OTRO CRITERIO
        private void cboBusquedaSemiProducido_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusquedaSemiProducido.Text = "";
        }

        //BÚSQUEDA DE SEMIPRODUCIDOS
        private void txtBusquedaSemiProducido_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboBusquedaSemiProducido.Text == "DESCRIPCIÓN")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("CreacionFormulacion_BuscarProductoPorDescripcion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@descripcion", txtBusquedaSemiProducido.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoSemiProducido.DataSource = dt;
                    con.Close();
                    ReordenarProductoBusqueda(datalistadoSemiProducido);
                    alternarColorFilas(datalistadoSemiProducido);
                }
                else if (cboBusquedaSemiProducido.Text == "CÓDIGO")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("CreacionFormulacion_BuscarProductoPorCodigo", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codigo", txtBusquedaSemiProducido.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoSemiProducido.DataSource = dt;
                    con.Close();
                    ReordenarProductoBusqueda(datalistadoSemiProducido);
                    alternarColorFilas(datalistadoSemiProducido);
                }
                else if (cboBusquedaSemiProducido.Text == "CÓDIGO BSS")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("CreacionFormulacion_BuscarProductoPorCodigoBSS", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codigoBSS", txtBusquedaSemiProducido.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoSemiProducido.DataSource = dt;
                    con.Close();
                    ReordenarProductoBusqueda(datalistadoSemiProducido);
                    alternarColorFilas(datalistadoSemiProducido);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //------------------------------------------------------------------------------------------

        //FORMULACION - ACCIONES - PROCESOS - PROCEDIMEINTOS -----------------------------------
        //ACCIONES DE LOS DETALLES DE MI FORMULACION------------------------------------------------
        //CARGA DE RECUROS - ACTIVIDADES PRODUCTO DETALLE
        public void MostrarDetalleFormulacionesProducto(DataGridView dgv, string idformulacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CreacionFormulacion_MostrarActividadProductos", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idformulacion", idformulacion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dgv.DataSource = dt;
                con.Close();
                ReordenarActividades(dgv);
                alternarColorFilas(dgv);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGA DE RECUROS - ACTIVIDADES SEMIPRODUCIDO DETALLE
        public void MostrarDetalleFormulacionesSemiProducido(DataGridView dgv, string idformulacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CreacionFormulacion_MostrarActividadSemiProducido", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idformulacion", idformulacion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dgv.DataSource = dt;
                con.Close();
                ReordenarActividades(dgv);
                alternarColorFilas(dgv);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //REORDENAR LISTADO DE ACTIVIDADES
        public void ReordenarActividades(DataGridView dgv)
        {
            dgv.Columns[0].Visible = false;
            dgv.Columns[1].Visible = false;
            dgv.Columns[2].Visible = false;
            dgv.Columns[3].Visible = false;
            dgv.Columns[4].Visible = false;
            dgv.Columns[5].Visible = false;
            dgv.Columns[7].Visible = false;
            dgv.Columns[9].Visible = false;
            dgv.Columns[19].Visible = false;

            dgv.Columns[6].Width = 250;
            dgv.Columns[8].Width = 300;
            dgv.Columns[10].Width = 80;
            dgv.Columns[11].Width = 65;
            dgv.Columns[12].Width = 65;
            dgv.Columns[13].Width = 90;
            dgv.Columns[14].Width = 65;
            dgv.Columns[15].Width = 65;
            dgv.Columns[16].Width = 70;
            dgv.Columns[17].Width = 70;
            dgv.Columns[18].Width = 65;
            dgv.Columns[20].Width = 65;
        }

        //MATERIALES----------------
        //BUSQUEDA DE MATERIALES DEL PRODUTO
        public void MostrarMaterialFormulacionesProducto(DataGridView DGV, string idformulacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CreacionFormulacion_MostrarMaterialProducto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idformulacion", idformulacion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                DGV.DataSource = dt;
                con.Close();
                ReordenarMateirales(DGV);
                alternarColorFilas(DGV);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //BUSQUEDA DE MATERIALES DEL SEMIPRODUCIDO
        public void MostrarMaterialFormulacionesSemiProducido(DataGridView DGV, string idformulacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarFormulacionMaterialSemiProducido", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idformulacion", idformulacion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                DGV.DataSource = dt;
                con.Close();
                ReordenarMateirales(DGV);
                alternarColorFilas(DGV);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //REORDENAR LISTADO DE MATERIALES
        public void ReordenarMateirales(DataGridView dgv)
        {
            dgv.Columns[0].Visible = false;
            dgv.Columns[1].Visible = false;
            dgv.Columns[2].Visible = false;

            dgv.Columns[3].Width = 90;
            dgv.Columns[4].Width = 70;
            dgv.Columns[5].Width = 280;
            dgv.Columns[6].Width = 110;
            dgv.Columns[7].Width = 75;
            dgv.Columns[8].Width = 75;
            dgv.Columns[9].Width = 100;
        }

        //INGRESO, EDICION, ELIMINACION CON SUS RESPESCTIVOS BOTONES PRODUCTOS---------------------------
        //ACCIONES DE LA FORMULACION------------------------------------------------------------
        //CARGAR LÍNEAS
        public void CargarLineas()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdLinea, Descripcion FROM LINEAS WHERE Estado = 1 AND IdLinea = " + lblIdLinea.Text, con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboLinea.ValueMember = "IdLinea";
                cboLinea.DisplayMember = "Descripcion";
                cboLinea.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR OPERACIONES
        public void CargarOperacion(string idlinea)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdOperacion, O.Descripcion FROM LineaXOperacion LO INNER JOIN OPERACIONES O ON O.IdOperaciones = LO.IdOperacion WHERE LO.Estado = 1 AND IdLinea = @idlinea", con);
                comando.Parameters.AddWithValue("@idlinea", idlinea);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboOperacion.ValueMember = "IdOperacion";
                cboOperacion.DisplayMember = "Descripcion";
                cboOperacion.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR MAQUINARIA
        public void CargarMaquinaria(string idlinea, string idoperacion)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdMaquinaria, M.Descripcion FROM LineaXOperacionXMaquinaria LOM INNER JOIN MAQUINARIAS M ON M.IdMaquinarias = LOM.IdMaquinaria WHERE LOM.Estado = 1 AND LOM.IdLinea = @idlinea AND LOM.IdOperacion = @idoperacion", con);
                comando.Parameters.AddWithValue("@idlinea", idlinea);
                comando.Parameters.AddWithValue("@idoperacion", idoperacion);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboMaquinaria.ValueMember = "IdMaquinaria";
                cboMaquinaria.DisplayMember = "Descripcion";
                cboMaquinaria.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR LINEA- OPERACIÓN Y MAQUINARIA VALIDACIÓN 
        public void CargarLOMValidacion(string idlinea, string idoperacion, string idmaquinaria)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CreacionFormulacion_MostrarLOMValidacion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idlinea", idlinea);
                cmd.Parameters.AddWithValue("@idoperacion", idoperacion);
                cmd.Parameters.AddWithValue("@idmaquinaria", idmaquinaria);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoLOM.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //EVENTO DE SELECCIÓN DE LA LÍNEA DESEADA Y CARGA DE OPERACIONES SEGÚN LA LÍNEA SELECCIOANDA
        private void cboLinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLinea.SelectedValue.ToString() != null)
            {
                string idlinea = cboLinea.SelectedValue.ToString();
                CargarOperacion(idlinea);
            }
        }

        //EVENTO DE SELECCIÓN DE LA OPERACIÓN DESEADA Y CARGA DE MAQUINARIAS SEGÚN LA OPERACIÓN SELECCIOANDA
        private void cboOperacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboOperacion.SelectedValue.ToString() != null)
            {
                string idlinea = cboLinea.SelectedValue.ToString();
                string idoperacion = cboOperacion.SelectedValue.ToString();
                CargarMaquinaria(idlinea, idoperacion);
            }
        }

        //EVENTO DE SELECCIÓN DE LA MAQUIANRIA DESEADA Y VALIDACIÓN DE LA EXISTENCIA DE ESTA EN EL LISTADO
        private void cboMaquinaria_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idlinea = cboLinea.SelectedValue.ToString();
            string idoperacion = cboOperacion.SelectedValue.ToString();
            string idmaquinaria = cboMaquinaria.SelectedValue.ToString();
            CargarLOMValidacion(idlinea, idoperacion, idmaquinaria);
        }

        //CARGA DEL COORRELATIVO
        public void CargarCorrelativo()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("Select IdCorrelativo, Descripcion from Correlativo where Estado = 1 and Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboCorrelativo.ValueMember = "IdCorrelativo";
                cboCorrelativo.DisplayMember = "Descripcion";
                cboCorrelativo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGA DEL TIPO DE OPERACIÓN
        public void CargarTipoOperacion()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("Select IdTipoOperacion, Descripcion from TipoOperacion where Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboTipoOperacion.ValueMember = "IdTipoOperacion";
                cboTipoOperacion.DisplayMember = "Descripcion";
                cboTipoOperacion.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //ABRIR EL PANEL APRA AGREGAR UNA NUEVA ACTIVIDADA A MI PRODUCOT
        private void btnAgregarActividadProducto_Click(object sender, EventArgs e)
        {
            panelActividadProducto.Visible = true;
            CargarLineas();
            CargarCorrelativo();
            CargarTipoOperacion();
            txtCodigoPanel.Text = lblCodigoProductoActividades.Text;
            MostrarDetalleFormulacionesProducto(datalistadoactividadproductosseleccionar, lblCodigoFormulacionVision.Text);
            alternarColorFilas(datalistadoactividadproductosseleccionar);
            AumentarPosicionCorrelativoProducto(datalistadoactividadproductosseleccionar, cboCorrelativo);
        }

        //VALIDAR LA POCISIÓN Y EL VALOR DEL CORRELATIVO
        public void ValidarPosicionCorrelativoProducto()
        {
            foreach (DataGridViewRow datorecuperado in datalistadoactividadproductosseleccionar.Rows)
            {
                string correlativo = Convert.ToString(datorecuperado.Cells["CORRELATIVO"].Value);
                if (correlativo == cboCorrelativo.Text)
                {
                    ValidacionCorrelativoProducto = true;
                    return;
                }
                else
                {
                    ValidacionCorrelativoProducto = false;
                }
            }
            return;
        }

        //AUTOINCREMENTAR LA POSICION
        public void AumentarPosicionCorrelativoProducto(DataGridView DGV, ComboBox CBO)
        {
            foreach (DataGridViewRow datorecuperado in DGV.Rows)
            {
                int correlativo = DGV.Rows.Count;

                if (correlativo == 0)
                {
                    CBO.SelectedIndex = 0;
                }
                else if (correlativo == 1)
                {
                    CBO.SelectedIndex = 1;

                }
                else if (correlativo == 2)
                {
                    CBO.SelectedIndex = 2;
                }
                else if (correlativo == 3)
                {
                    CBO.SelectedIndex = 3;
                }
                else if (correlativo == 4)
                {
                    CBO.SelectedIndex = 4;
                }
                else if (correlativo == 5)
                {
                    CBO.SelectedIndex = 5;
                }
                else if (correlativo == 6)
                {
                    CBO.SelectedIndex = 6;
                }
                else if (correlativo == 7)
                {
                    CBO.SelectedIndex = 7;
                }
                else if (correlativo == 8)
                {
                    CBO.SelectedIndex = 8;
                }
                else if (correlativo == 9)
                {
                    CBO.SelectedIndex = 9;
                }
                else if (correlativo == 10)
                {
                    CBO.SelectedIndex = 10;
                }
                else if (correlativo == 11)
                {
                    CBO.SelectedIndex = 11;
                }
                else if (correlativo == 12)
                {
                    CBO.SelectedIndex = 12;
                }
                else if (correlativo == 13)
                {
                    CBO.SelectedIndex = 13;
                }
                else if (correlativo == 14)
                {
                    CBO.SelectedIndex = 14;
                }
                else if (correlativo == 15)
                {
                    CBO.SelectedIndex = 15;
                }
                else if (correlativo == 16)
                {
                    CBO.SelectedIndex = 16;
                }
                else if (correlativo == 17)
                {
                    CBO.SelectedIndex = 17;
                }
                else if (correlativo == 18)
                {
                    CBO.SelectedIndex = 18;
                }
                else if (correlativo == 19)
                {
                    CBO.SelectedIndex = 19;
                }
            }
        }

        //METODO PARA INGRESAR UNA NUEVA ACTIVIDAD A MI PRODUCTO
        public void AgregarActividadProducto(string codigoformulacion, DataGridView dgv, int idcorrelativo, int Tcosto, decimal Tsetup, decimal Toperacion, int Tpor, int Thoras
            , int personal, decimal Cpersonal, int idtipo, string cbomaquin, string cboopera)
        {
            if (cbomaquin == "" || cboopera == "" || Convert.ToString(Tcosto) == "" || Convert.ToString(Tsetup) == "" || Convert.ToString(Toperacion) == ""
                || Convert.ToString(Tpor) == "" || Convert.ToString(personal) == "" || Convert.ToString(Cpersonal) == "")
            {
                MessageBox.Show("Debe llenar todos los campos para continuar.", "REGISTRO", MessageBoxButtons.OKCancel);
            }
            else
            {
                try
                {
                    if (dgv.SelectedRows.Count != 1)
                    {
                        MessageBox.Show("Se encontraron 2 o más registros repetidos, por favor verificar las líneas por operación por maquinaria ingresados.", "Error Inesperado", MessageBoxButtons.OK);
                    }
                    else
                    {
                        ValidarPosicionCorrelativoProducto();

                        if (ValidacionCorrelativoProducto == false)
                        {
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("CreacionFormulacion_InsertarActividadProducto", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@codigoformulacion", codigoformulacion);
                            cmd.Parameters.AddWithValue("@codigoLOM", Convert.ToInt32(dgv.SelectedCells[0].Value.ToString()));
                            cmd.Parameters.AddWithValue("@idcorrelativo", idcorrelativo);
                            cmd.Parameters.AddWithValue("@tcosto", Tcosto);
                            cmd.Parameters.AddWithValue("@tsetup", Tsetup);
                            cmd.Parameters.AddWithValue("@toperacion", Toperacion);
                            cmd.Parameters.AddWithValue("@tpor", Tpor);
                            cmd.Parameters.AddWithValue("@thoras", Thoras);
                            cmd.Parameters.AddWithValue("@personal", personal);
                            cmd.Parameters.AddWithValue("@cpersonal", Cpersonal);
                            decimal ctotalsuma = Cpersonal + Convert.ToDecimal(personal);
                            cmd.Parameters.AddWithValue("@ctotal", ctotalsuma);
                            cmd.Parameters.AddWithValue("@idtipo", idtipo);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            MostrarDetalleFormulacionesProducto(datalistadoactividadproductosseleccionar, lblCodigoFormulacionVision.Text);

                            cboCorrelativo.SelectedIndex = 0;
                            txtTcosto.Text = "0";
                            txtTpor.Text = "1";
                            txtTsetup.Text = "0";
                            txtPersonal.Text = "1";
                            txtToperacion.Text = "0";
                            cboTipoOperacion.SelectedIndex = 0;
                            txtCpersonal.Text = "0";
                            AumentarPosicionCorrelativoProducto(datalistadoactividadproductosseleccionar, cboCorrelativo);

                            MessageBox.Show("Registro ingresado exitosamente.", "Nueva Actividad", MessageBoxButtons.OK);
                        }
                        else
                        {
                            MessageBox.Show("No se pueden guardar una actividad con correlativos iguales.", "Validación del Sistema", MessageBoxButtons.OK);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //METODO PARA INGRESAR UNA NUEVA ACTIVIDAD A MI PRODUCTO
        private void btnConfirmarActividadProducto_Click(object sender, EventArgs e)
        {
            AgregarActividadProducto(lblCodigoFormulacionVision.Text, datalistadoLOM, Convert.ToInt32(cboCorrelativo.SelectedValue), Convert.ToInt32(txtTcosto.Text), Convert.ToDecimal(txtTsetup.Text),
            Convert.ToDecimal(txtToperacion.Text), Convert.ToInt32(txtTpor.Text), Convert.ToInt32(txtHoras.Text), Convert.ToInt32(txtPersonal.Text), Convert.ToDecimal(txtCpersonal.Text)
            , Convert.ToInt32(cboTipoOperacion.SelectedValue), cboMaquinaria.Text, cboOperacion.Text);
        }

        //METODO PARA ELIMINAR UNA ACTIVIDAD A MI PRODUCTO
        public void EliminarActividadProducto(DataGridView DGV, string codigoformulacion)
        {
            if (DGV.CurrentRow != null)
            {
                DialogResult boton = MessageBox.Show("¿Realmente desea eliminar esta actividad?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("CreacionFormulacion_EliminarActividadProducto", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idActividadProducto", Convert.ToInt32(DGV.SelectedCells[0].Value.ToString()));
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Se eliminó correctamente.", "Validación del Sistema", MessageBoxButtons.OK);
                        MostrarDetalleFormulacionesProducto(DGV, codigoformulacion);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una actividad para poder borrarla.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //METODO PARA ELIMINAR UNA ACTIVIDAD A MI PRODUCTO
        private void btnEliminarActividadProducto_Click(object sender, EventArgs e)
        {
            EliminarActividadProducto(datalistadoactividadesproducto, lblCodigoFormulacionVision.Text);
        }

        //CERRAR LA VENANA DE ACTIVIDADES Y LIMPIAR LOS CAMPOS
        private void btnRegresarActividadProducto_Click(object sender, EventArgs e)
        {
            panelActividadProducto.Visible = false;
            cboCorrelativo.SelectedIndex = 1;
            txtTcosto.Text = "0";
            txtTpor.Text = "1";
            txtTsetup.Text = "0";
            txtPersonal.Text = "1";
            txtToperacion.Text = "0";
            cboTipoOperacion.SelectedIndex = 1;
            txtCpersonal.Text = "0";
            MostrarDetalleFormulacionesProducto(datalistadoactividadesproducto, lblCodigoFormulacionVision.Text);
        }

        //INGRESO, EDICIÓN, ELIMINACIÓN CON SUS RESPESCTIVOS BOTONES SEMIPRDOFUCIDO---------------------------
        //CARGAR LÍNEAS SEMIPRODUCIDO
        public void CargarLineasS()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdLinea, Descripcion FROM LINEAS WHERE Estado = 1 AND IdLinea = " + lblIdLinea.Text, con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboLineaS.ValueMember = "IdLinea";
                cboLineaS.DisplayMember = "Descripcion";
                cboLineaS.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR MODELOS SEMIPRODUCIDO
        public void CargarModeloS()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("  SELECT M.IdModelo, M.Descripcion FROM MODELOS M INNER JOIN PRODUCTOS P ON P.IDMODELO = M.IDMODELO WHERE M.Estado = 1 AND P.Codcom = '" + lblCodigoSemiProducidoActividades.Text + "'", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboModeloS.ValueMember = "IdModelo";
                cboModeloS.DisplayMember = "Descripcion";
                cboModeloS.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR OPERACIONES SEMIPRODUCIDO
        public void CargarOperacionesS(string idmodelo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT O.IdOperaciones, O.Descripcion FROM ModeloxOperacion MOM INNER JOIN Operaciones O ON O.IdOperaciones = MOM.IdOperacion WHERE MOM.Estado = 1 AND IdModelo = @idmodelo", con);
                comando.Parameters.AddWithValue("@idmodelo", idmodelo);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboOperacionS.ValueMember = "O.IdOperaciones";
                cboOperacionS.DisplayMember = "Descripcion";
                cboOperacionS.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR MAQUINARIAS SEMIPRODUCIDO
        public void CargarMaquinariaS(string idmodelo, string idoperacion)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdMaquinaria, M.Descripcion FROM ModeloXOperacionXMaquinaria MOM INNER JOIN MAQUINARIAS M ON M.IdMaquinarias = MOM.IdMaquinaria WHERE MOM.Estado = 1 AND MOM.IdModelo = @idmodelo AND MOM.IdOperacion = @idoperacion", con);
                comando.Parameters.AddWithValue("@idmodelo", idmodelo);
                comando.Parameters.AddWithValue("@idoperacion", idoperacion);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboMaquinariaS.ValueMember = "IdMaquinaria";
                cboMaquinariaS.DisplayMember = "Descripcion";
                cboMaquinariaS.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR LINEA - MODELO - OPERACIÓN Y MAQUINARIA VALIDACIÓN 
        public void CargarMOMValidacion(string idmodelo, string idoperacion, string idmaquinaria)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CreacionFormulacion_MostrarMOMValidacion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idmodelo", idmodelo);
                cmd.Parameters.AddWithValue("@idoperacion", idoperacion);
                cmd.Parameters.AddWithValue("@idmaquinaria", idmaquinaria);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoMOM.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //EVENTO DE SELECCIÓN DEL MODELO DESEADA Y CARGA DE OPERACIONES SEGÚN EL MODELO SELECCIOANDA
        private void cboModeloS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboModeloS.SelectedValue.ToString() != null)
            {
                string idmodeloS = cboModeloS.SelectedValue.ToString();
                CargarOperacionesS(idmodeloS);
            }
        }

        //EVENTO DE SELECCIÓN DE LA OPERACIÓN DESEADA Y CARGA DE MAQUINARIAS SEGÚN LA OPERACIÓN SELECCIOANDA
        private void cboOperacionS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboOperacionS.SelectedValue.ToString() != null)
            {
                string idmodeloS = cboModeloS.SelectedValue.ToString();
                string idoperacionS = cboOperacionS.SelectedValue.ToString();
                CargarMaquinariaS(idmodeloS, idoperacionS);
            }
        }

        //EVENTO DE SELECCIÓN DE LA MAQUIANRIA DESEADA Y VALIDACIÓN DE LA EXISTENCIA DE ESTA EN EL LISTADO
        private void cboMaquinariaS_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idmodeloS = cboModeloS.SelectedValue.ToString();
            string idoperacionS = cboOperacionS.SelectedValue.ToString();
            string idmaquinariaS = cboMaquinariaS.SelectedValue.ToString();
            CargarMOMValidacion(idmodeloS, idoperacionS, idmaquinariaS);
        }

        //CARGA DEL COORRELATIVO
        public void CargarCorrelativoS()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdCorrelativo, Descripcion FROM Correlativo WHERE Estado = 1 and Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboCorrelativoS.ValueMember = "IdCorrelativo";
                cboCorrelativoS.DisplayMember = "Descripcion";
                cboCorrelativoS.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR TIPO DE OPERACIÓN
        public void CargarTipoOperacionS()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdTipoOperacion, Descripcion FROM TipoOperacion WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboTipoOperacionS.ValueMember = "IdTipoOperacion";
                cboTipoOperacionS.DisplayMember = "Descripcion";
                cboTipoOperacionS.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //ABRIR EL PANEL PARA AGREGAR UNA NUEVA ACTIVIDADA A MI SEMIPRODUCIDO
        private void btnAgregarActividadSemiProducido_Click(object sender, EventArgs e)
        {
            panelActividadSemiProducido.Visible = true;
            CargarLineasS();
            CargarModeloS();
            CargarCorrelativoS();
            CargarTipoOperacionS();
            lblIdFormulacionS.Text = lblCodigoFormulacionVision.Text;
            MostrarDetalleFormulacionesSemiProducido(datalistadoactividadsemiproducidoseleccionar, lblIdFormulacionS.Text);
            alternarColorFilas(datalistadoactividadsemiproducidoseleccionar);
            AumentarPosicionCorrelativoProducto(datalistadoactividadsemiproducidoseleccionar, cboCorrelativoS);
        }

        //VALIDAR LA POCISIÓN Y EL VALOR DEL CORRELATIVO
        public void ValidarPosicionCorrelativoSemiProducido()
        {
            foreach (DataGridViewRow datorecuperado in datalistadoactividadsemiproducidoseleccionar.Rows)
            {
                string correlativo = Convert.ToString(datorecuperado.Cells["CORRELATIVO"].Value);
                if (correlativo == cboCorrelativoS.Text)
                {
                    ValidacionCorrelativoSemiProducido = true;
                    return;
                }
                else
                {
                    ValidacionCorrelativoSemiProducido = false;
                }
            }
            return;
        }

        //METODO PARA INGRESAR UNA NUEVA ACTIVIDAD A MI SEMIPRODUCIDO
        public void AgregarActividadSemiProducido(string codigoformulacion, DataGridView dgv, int idcorrelativo, int Tcosto, decimal Tsetup, decimal Toperacion, int Tpor, int Thoras, int personal, decimal Cpersonal,
            int idtipo, string cbomaqui, string cboope)
        {
            if (cbomaqui == "" || cboope == "" || Convert.ToString(Tcosto) == "" || Convert.ToString(Tsetup) == "" || Convert.ToString(Toperacion) == "" || Convert.ToString(Tpor) == "" || Convert.ToString(personal) == "" || Convert.ToString(Cpersonal) == "")
            {
                MessageBox.Show("Debe llenar todos los campos para continuar.", "REGISTRO", MessageBoxButtons.OKCancel);
            }
            else
            {
                try
                {
                    if (datalistadoMOM.SelectedRows.Count != 1)
                    {
                        MessageBox.Show("Se encontraron 2 o más registros repetidos, por favor verificar los modelos por operación por maquinaria ingresados.", "Error Inesperado", MessageBoxButtons.OK);
                    }
                    else
                    {
                        ValidarPosicionCorrelativoSemiProducido();
                        if (ValidacionCorrelativoSemiProducido == false)
                        {
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("CreacionFormulacion_InsertarActividadSemiProducido", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@codigoformulacion", lblIdFormulacionS.Text);
                            cmd.Parameters.AddWithValue("@codigoMOM", Convert.ToInt32(datalistadoMOM.SelectedCells[0].Value.ToString()));
                            cmd.Parameters.AddWithValue("@idcorrelativo", cboCorrelativoS.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@tcosto", Convert.ToInt32(txtTcostoS.Text));
                            cmd.Parameters.AddWithValue("@tsetup", Convert.ToDecimal(txtTsetupS.Text));
                            cmd.Parameters.AddWithValue("@toperacion", Convert.ToDecimal(txtToperacionS.Text));
                            cmd.Parameters.AddWithValue("@tpor", Convert.ToInt32(txtTporS.Text));
                            cmd.Parameters.AddWithValue("@thoras", Convert.ToInt32(txtHorasS.Text));
                            cmd.Parameters.AddWithValue("@personal", Convert.ToInt32(txtPersonalS.Text));
                            cmd.Parameters.AddWithValue("@cpersonal", Convert.ToDecimal(txtCpersonalS.Text));
                            decimal ctotalsuma = Convert.ToDecimal(txtCpersonalS.Text) + Convert.ToDecimal(txtPersonalS.Text);
                            cmd.Parameters.AddWithValue("@ctotal", ctotalsuma);
                            cmd.Parameters.AddWithValue("@idtipo", cboTipoOperacionS.SelectedValue.ToString());

                            cmd.ExecuteNonQuery();
                            con.Close();
                            MostrarDetalleFormulacionesSemiProducido(datalistadoactividadsemiproducidoseleccionar, lblIdFormulacionS.Text);

                            cboCorrelativoS.SelectedIndex = 0;
                            txtTcostoS.Text = "0";
                            txtTporS.Text = "1";
                            txtTsetupS.Text = "0";
                            txtPersonalS.Text = "1";
                            txtToperacionS.Text = "0";
                            cboTipoOperacionS.SelectedIndex = 0;
                            txtCpersonalS.Text = "0";
                            AumentarPosicionCorrelativoProducto(datalistadoactividadsemiproducidoseleccionar, cboCorrelativoS);

                            MessageBox.Show("Registro ingresado exitosamente.", "Nueva Actividad", MessageBoxButtons.OK);
                        }
                        else
                        {
                            MessageBox.Show("No se pueden guardar una actividad con correlativos iguales.", "Validación del Sistema", MessageBoxButtons.OK);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //METODO PARA INGRESAR UNA NUEVA ACTIVIDAD A MI PRODUCTO
        private void btnConfirmarActividadSemiProducido_Click(object sender, EventArgs e)
        {
            AgregarActividadSemiProducido(lblIdFormulacionS.Text, datalistadoMOM, Convert.ToInt32(cboCorrelativoS.SelectedValue), Convert.ToInt32(txtTcostoS.Text)
            , Convert.ToDecimal(txtTsetupS.Text), Convert.ToDecimal(txtToperacionS.Text), Convert.ToInt32(txtTporS.Text), Convert.ToInt32(txtHorasS.Text), Convert.ToInt32(txtPersonalS.Text)
            , Convert.ToInt32(txtCpersonalS.Text), Convert.ToInt32(cboTipoOperacionS.SelectedValue), cboMaquinariaS.Text, cboOperacionS.Text);
        }

        //METODO PARA ELIMINAR UNA ACTIVIDAD A MI SEMI-PRODUCIDO
        public void EliminarActividadSemiProducido(DataGridView dgv, string codigoformulacion)
        {
            if (dgv.CurrentRow != null)
            {
                DialogResult boton = MessageBox.Show("¿Realmente desea eliminar esta actividad?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("CreacionFormulacion_EliminarActividadSemiProducto", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idActividadSemiProducto", Convert.ToInt32(dgv.SelectedCells[0].Value.ToString()));

                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Se eliminó correctamente.", "Validación del Sistema", MessageBoxButtons.OK);
                        MostrarDetalleFormulacionesSemiProducido(dgv, codigoformulacion);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una actividad para poder borrarla.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //METODO PARA ELIMINAR UNA ACTIVIDAD A MI SEMI-PRODUCIDO
        private void btnEliminarActividadSemiProducido_Click(object sender, EventArgs e)
        {
            EliminarActividadSemiProducido(datalistadoactividadsemiproducido, lblCodigoFormulacionVision.Text);
        }

        //CERRAR Y LIMPOAR EL PANEL DE ACTIVIADES DE SEMIPRODUCIDO
        private void btnRegresarActividadSemiProducido_Click(object sender, EventArgs e)
        {
            panelActividadSemiProducido.Visible = false;
            cboCorrelativoS.SelectedIndex = 1;
            txtTcostoS.Text = "0";
            txtTporS.Text = "1";
            txtTsetupS.Text = "0";
            txtPersonalS.Text = "1";
            txtToperacionS.Text = "0";
            cboTipoOperacionS.SelectedIndex = 1;
            txtCpersonalS.Text = "0";
            MostrarDetalleFormulacionesSemiProducido(datalistadoactividadsemiproducido, lblIdFormulacionS.Text);
        }

        //---------MATERIAS PRIMAS CARGA DE PRODUCTOS---------------------------------------------------
        //CARGA DE MATERIAS PRIMAS PRODUCTO-----------------------------
        public void CargarProductosMateriasPrimas(DataGridView dgv)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT Codcom AS [CÓDIGO], IdArt AS [C. ART], P.Descripcion AS [CÓDIGO BSS], Detalle AS [DESCRIPCIÓN] , M.Descripcion AS [MEDIDA] FROM PRODUCTOS P INNER JOIN MEDIDA M ON M.IdMedida = P.IdMedida WHERE P.Estado = 1 AND IdTipoMercaderias IN (16,15)", con);
                da.Fill(dt);
                dgv.DataSource = dt;
                con.Close();
                dgv.Columns[0].Width = 100;
                dgv.Columns[2].Width = 90;
                dgv.Columns[3].Width = 445;
                dgv.Columns[4].Width = 160;
                dgv.Columns[1].Visible = false;
                alternarColorFilas(dgv);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //SELECCIOANR LA ACTIVIDAD Y ABRIRA LOS AMTERIALES ASIGNADOS A ESTA
        private void datalistadoactividadesproducto_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoactividadesproducto.RowCount != 0)
            {
                panelMaterialproducto.Visible = true;
                lblMaterialFormulacion.Text = lblCodigoFormulacionVision.Text;
                lblMaterialCodigoOperacion.Text = datalistadoactividadesproducto.SelectedCells[0].Value.ToString();
                txtMaterialProducto.Text = lblCodigoProductoActividades.Text;
                txtMaterialOperacion.Text = datalistadoactividadesproducto.SelectedCells[6].Value.ToString();
                txtMaterialCorrelativo.Text = datalistadoactividadesproducto.SelectedCells[10].Value.ToString();
                CargarProductosMateriasPrimas(datalistadomaterialseleccionarseleccionar);
                lblTituloMateriales.Text = "Materia Prima X Producto";
                alternarColorFilas(datalistadomaterialseleccionarseleccionar);
            }
        }

        //SELECCIOANR EL AMTERIAL REQUERIDO PARA PODER INGRESARLO
        private void datalistadomaterialseleccionarseleccionar_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadomaterialseleccionarseleccionar.RowCount != 0)
            {
                txtMaterialNombre.Text = datalistadomaterialseleccionarseleccionar.SelectedCells[3].Value.ToString();
                txtMaterialUnidad.Text = datalistadomaterialseleccionarseleccionar.SelectedCells[4].Value.ToString();
                lblMaterialCodigo.Text = datalistadomaterialseleccionarseleccionar.SelectedCells[1].Value.ToString();
            }
        }

        //METODO QUE DEFINE LOS MATERIALES PARA MI PRODUCTO O SEMIPRODUCIDO
        public void AgregarMaterial(string materialformulacion, int materialCodigoOperacion, int materialcodigo, decimal materialcantidad, int materialcorrelativo, string titulomateriales
            , int materialcantidadtotal, string materialnombre)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CreacionFormulacion_InsertarMaterialProducto", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@codigoformulacion", materialformulacion);
                cmd.Parameters.AddWithValue("@idactividadproducto", materialCodigoOperacion);
                cmd.Parameters.AddWithValue("@idart", materialcodigo);
                cmd.Parameters.AddWithValue("@cantidad", materialcantidad);
                cmd.Parameters.AddWithValue("@posicion", materialcorrelativo);
                if (titulomateriales == "Materia Prima X Producto")
                {
                    cmd.Parameters.AddWithValue("@tipomaterial", "MATERIAL PRODUCTO");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@tipomaterial", "MATERIAL SEMIPRODUCIDO");
                }

                cmd.Parameters.AddWithValue("@cantidadtotal", materialcantidadtotal);

                cmd.ExecuteNonQuery();
                con.Close();

                txtMaterialCantidad.Text = "";
                txtMaterialBusqueda.Text = "";
                //txtMaterialCantidadTotal.Text = "";
                txtMaterialNombre.Text = "";
                txtMaterialUnidad.Text = "";
                lblMaterialCodigo.Text = "**************";

                MessageBox.Show("Registro ingresado exitosamente.", "Nuevo Material", MessageBoxButtons.OK);
                panelMaterialproducto.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            MostrarMaterialFormulacionesProducto(datalistadomaterialproducto, textocodigoformulacion);
            MostrarMaterialFormulacionesSemiProducido(datalistadomaterialsemiproducido, textocodigoformulacion);
        }

        //EVENTO DE CONFIRMACION DE GUARDAR UN MATERIAL
        private void btnConfirmarMaterial_Click(object sender, EventArgs e)
        {
            if (txtMaterialCantidad.Text == "" || txtMaterialCantidadTotal.Text == "" || txtMaterialNombre.Text == "")
            {
                MessageBox.Show("Debe Seleccionar un producto o llenar todos los datos necesarios", "Valiración del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                AgregarMaterial(lblMaterialFormulacion.Text, Convert.ToInt32(lblMaterialCodigoOperacion.Text), Convert.ToInt32(lblMaterialCodigo.Text)
               , Convert.ToDecimal(txtMaterialCantidad.Text), Convert.ToInt32(txtMaterialCorrelativo.Text), lblTituloMateriales.Text, Convert.ToInt32(txtMaterialCantidadTotal.Text)
               , txtMaterialNombre.Text);
            }
        }

        //BOTON PARA ELIMINAR EL MATERIAL ASIGNADOA  MI ACTIVIDAD
        private void btnEliminarMaterialProducto_Click(object sender, EventArgs e)
        {
            EliminarMaterialActividad(datalistadomaterialproducto);
        }

        //BOTON PARA ELIMINAR EL MATERIAL ASIGNADOA  MI ACTIVIDAD
        private void btnEliminarMaterialSemiProducido_Click(object sender, EventArgs e)
        {
            EliminarMaterialActividad(datalistadomaterialsemiproducido);
        }

        //BOTON PARA INTERCAMBIAR UN NUEVO PRODUCTO POR EL YA INGRESADO
        private void btnRecuperarMaterialProducto_Click(object sender, EventArgs e)
        {
            IntercambiarMaterialActividad(datalistadomaterialproducto);
        }

        //BOTON PARA INTERCAMBIAR UN NUEVO PRODUCTO POR EL YA INGRESADO
        private void btnRecuperarMaterialSemiProducido_Click(object sender, EventArgs e)
        {
            IntercambiarMaterialActividad(datalistadomaterialsemiproducido);
        }

        //FUNCION PARA INTECAMBIAR MATERIALES DE MIS ACTIVIDADES
        public void IntercambiarMaterialActividad(DataGridView DGV)
        {
            if (txtCodigoBusquedaMaterial.Text == "")
            {
                MessageBox.Show("Debe seleccionar un material válido para poder realizar el intercambio.", "Validación del Sistema");
            }
            else
            {
                DialogResult boton = MessageBox.Show("¿Realmente desea intercambiar este producto?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("CreacionFormulacion_CambioMaterial", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArt", Convert.ToInt32(datalistadobusquedamaterial.SelectedCells[1].Value.ToString()));
                        cmd.Parameters.AddWithValue("@idMaterial", Convert.ToInt32(DGV.SelectedCells[0].Value.ToString()));

                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Se editó el material correctamente.", "Validación del Sistema", MessageBoxButtons.OK);
                        MostrarMaterialFormulacionesProducto(datalistadomaterialproducto, textocodigoformulacion);
                        MostrarMaterialFormulacionesSemiProducido(datalistadomaterialsemiproducido, textocodigoformulacion);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //FUNCION PARA ELIMINAR MATERIALES DE MIS ACTIVIDADES
        public void EliminarMaterialActividad(DataGridView DGV)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea eliminar este material?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                try
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("CreacionFormulacion_EliminarMaterialActividad", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idActividadMaterial", Convert.ToInt32(DGV.SelectedCells[0].Value.ToString()));

                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Se eliminó correctamente.", "Validación del Sistema", MessageBoxButtons.OK);
                    MostrarMaterialFormulacionesProducto(datalistadomaterialproducto, textocodigoformulacion);
                    MostrarMaterialFormulacionesSemiProducido(datalistadomaterialsemiproducido, textocodigoformulacion);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //CERRAR MATERIALES DE ACTIVIDADES DEL PRODUCTO
        private void btnRegresarMaterial_Click(object sender, EventArgs e)
        {
            panelMaterialproducto.Visible = false;
            MostrarMaterialFormulacionesProducto(datalistadomaterialproducto, textocodigoformulacion);
            txtMaterialBusqueda.Text = "";
            txtMaterialCantidad.Text = "";
            txtMaterialNombre.Text = "";
            txtMaterialUnidad.Text = "";
            //txtMaterialCantidadTotal.Text = "";
        }

        //CARGA DE MATERIAS PRIMAS DEL SEMIPRODUCID-------------------------------------------
        //SELECCIOANR LA ACTIVIDAD Y ABRIRA LOS AMTERIALES ASIGNADOS A ESTA
        private void datalistadoactividadsemiproducido_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoactividadsemiproducido.RowCount != 0)
            {
                panelMaterialproducto.Visible = true;
                lblMaterialFormulacion.Text = lblCodigoFormulacionVision.Text;
                lblMaterialCodigoOperacion.Text = datalistadoactividadsemiproducido.SelectedCells[0].Value.ToString();
                txtMaterialProducto.Text = lblCodigoSemiProducidoActividades.Text;
                txtMaterialOperacion.Text = datalistadoactividadsemiproducido.SelectedCells[6].Value.ToString();
                txtMaterialCorrelativo.Text = datalistadoactividadsemiproducido.SelectedCells[10].Value.ToString();
                CargarProductosMateriasPrimas(datalistadomaterialseleccionarseleccionar);
                lblTituloMateriales.Text = "Materia Prima X Semiproducido";
                alternarColorFilas(datalistadomaterialseleccionarseleccionar);
            }
        }
        //-----------------------------------------------------------------------

        //ACCIONES DEL PANEL - PARTE GENERAL-------------------------------------
        //SALIR DE LOS DETALLES DE MI FORMULACIÓN
        //CERRAR LOS DETALLES DE MI FORMULACION
        private void brnCerrarActividades_Click(object sender, EventArgs e)
        {
            panelActividades.Visible = false;
            txtBusquedaMateriales.Text = "";
            datalistadobusquedamaterial.DataSource = null;
            txtBusquedaMaterial.Text = "";
            txtCodigoBusquedaMaterial.Text = "";
            panelBusquedaCopiaFormulaciones.Visible = false;
            txtBusquedaCopiaFormulacion.Text = "";
            datalistadoBusquedaCopiaFormulaciones.DataSource = null;
        }

        //VISUALIZAR EL PLANO DEL PRODUCTO - DETALLES
        private void btnVistaPlanoProducto_Click(object sender, EventArgs e)
        {
            VisualizarPlanosSeleccionados(null, txtDetallesPlanoRutaProducto);
        }

        //VISUALIZAR EL PLANO DEL SEMIPRODUVIDO - DETALLES
        private void btnVistaPlanoSemiProducida_Click(object sender, EventArgs e)
        {
            VisualizarPlanosSeleccionados(null, txtDetallesPlanoRutaSemiProducido);
        }

        //BUSCAR MATERIA PRIMA PARA INTERCAMBIAR - LIBRE----------------------------------
        private void datalistadobusquedamaterial_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadobusquedamaterial.RowCount != 0)
            {
                txtCodigoBusquedaMaterial.Text = datalistadobusquedamaterial.SelectedCells[0].Value.ToString();
                txtBusquedaMaterial.Text = datalistadobusquedamaterial.SelectedCells[2].Value.ToString();
            }
        }

        //VALIDACIONES DIVERSAS Y BÚSQUEDAS----------------------------------------------------------------------
        private void txtTcosto_KeyPress(object sender, KeyPressEventArgs e)
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

        //VALIDACIONES
        private void txtTpor_KeyPress(object sender, KeyPressEventArgs e)
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

        //VALIDACIONES
        private void txtTsetup_KeyPress(object sender, KeyPressEventArgs e)
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

        //VALIDACIONES
        private void txtHoras_KeyPress(object sender, KeyPressEventArgs e)
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

        //VALIDACIONES
        private void txtPersonal_KeyPress(object sender, KeyPressEventArgs e)
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

        //VALIDACIONES
        private void txtCpersonal_KeyPress(object sender, KeyPressEventArgs e)
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

        //VALIDACIONES
        private void txtToperacion_KeyPress(object sender, KeyPressEventArgs e)
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

        //BUSCAR MATEIRALES POR DESCRIPCION
        //METODO DE BUSQUEDA PARA EL INTERCAMBIO DE MATERIALES  
        public void BusquedaMateriales_Intercambiar(string busquedamateriales, DataGridView DGV)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CreacionFormulacion_BuscarPorDetallesMaterial", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@detalle", busquedamateriales);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                DGV.DataSource = dt;
                con.Close();
                DGV.Columns[1].Visible = false;
                DGV.Columns[3].Visible = false;

                DGV.Columns[0].Width = 90;
                DGV.Columns[2].Width = 300;
                DGV.Columns[4].Width = 150;
                alternarColorFilas(DGV);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //BUSCAR MATEIRALES POR DESCRIPCION
        private void txtBusquedaMateriales_TextChanged(object sender, EventArgs e)
        {
            BusquedaMateriales_Intercambiar(txtBusquedaMateriales.Text, datalistadobusquedamaterial);
        }

        //BUSCAR MATEIRALES POR DESCRIPCION
        //METODO DE BUSQUEDA PARA EL INGRESO DE MATERIALES
        public void BusquedaMateriales(string busquedamateriales, DataGridView DGV)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("CreacionFormulacion_BuscarPorDetallesMaterialF", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@detalle", busquedamateriales);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                DGV.DataSource = dt;
                con.Close();
                DGV.Columns[0].Width = 100;
                DGV.Columns[1].Width = 90;
                DGV.Columns[2].Width = 445;
                DGV.Columns[3].Width = 160;
                alternarColorFilas(DGV);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //BUSCAR MATEIRALES POR DESCRIPCION
        private void txtMaterialBusqueda_TextChanged(object sender, EventArgs e)
        {
            BusquedaMateriales(txtMaterialBusqueda.Text, datalistadomaterialseleccionarseleccionar);
        }

        //-------------------------------------------------------------------------------------------------------------
        //-----------------------------------------BUSQUEDA COPIA FUNCIONES---------------------------------------
        //COMBO DE OPCIONES DE BÚSQUEDA
        private void cboBusquedaCopiaFormulacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusquedaCopiaFormulacion.Text = "";
        }

        //BUSQUEDA SENSITIVA EN TEIMPÓ REAL
        private void txtBusquedaCopiaFormulacion_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboBusquedaCopiaFormulacion.Text == "CÓDIGO FORMULACIÓN")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("CreacionFormulacion_BuscarCopiarCodigo", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@descripcion", txtBusquedaCopiaFormulacion.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoBusquedaCopiaFormulaciones.DataSource = dt;
                    con.Close();
                    RedimensionarBusquedaCopiaFormulaciones(datalistadoBusquedaCopiaFormulaciones);
                    alternarColorFilas(datalistadoBusquedaCopiaFormulaciones);
                }
                else if (cboBusquedaCopiaFormulacion.Text == "CÓDIGO PRODUCTO")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("CreacionFormulacion_BuscarCopiarCodigoProducto", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@descripcion", txtBusquedaCopiaFormulacion.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoBusquedaCopiaFormulaciones.DataSource = dt;
                    con.Close();
                    RedimensionarBusquedaCopiaFormulaciones(datalistadoBusquedaCopiaFormulaciones);
                    alternarColorFilas(datalistadoBusquedaCopiaFormulaciones);
                }
                else if (cboBusquedaCopiaFormulacion.Text == "DESCRIPCIÓN PRODUCTO")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("CreacionFormulacion_BuscarCopiarDescripcionProducto", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@descripcion", txtBusquedaCopiaFormulacion.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoBusquedaCopiaFormulaciones.DataSource = dt;
                    con.Close();
                    RedimensionarBusquedaCopiaFormulaciones(datalistadoBusquedaCopiaFormulaciones);
                    alternarColorFilas(datalistadoBusquedaCopiaFormulaciones);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //REDIMENSIONAR MI BUSQUEDA DE COPIA DE FOMRULACIONES
        public void RedimensionarBusquedaCopiaFormulaciones(DataGridView DGV)
        {
            DGV.Columns[0].Visible = false;
            DGV.Columns[1].Width = 120;
            DGV.Columns[2].Width = 130;
            DGV.Columns[3].Width = 115;
            DGV.Columns[4].Width = 500;
            DGV.Columns[5].Width = 150;
        }

        //SELECCIONAR UNA FORMULACION DE MI COPIA
        private void datalistadoBusquedaCopiaFormulaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoBusquedaCopiaFormulaciones.RowCount != 0)
            {
                MostrarDetalleFormulacionesProducto(datalistadoItemsActividadesProductoBusquedaCopiaFormulacion, datalistadoBusquedaCopiaFormulaciones.SelectedCells[2].Value.ToString());
                MostrarMaterialFormulacionesProducto(datalistadoItemsMaterialProducidoBusquedaCopiaFormulacion, datalistadoBusquedaCopiaFormulaciones.SelectedCells[2].Value.ToString());

                MostrarDetalleFormulacionesSemiProducido(datalistadoItemsActividadesSemiProducidoBusquedaCopiaFormulacion, datalistadoBusquedaCopiaFormulaciones.SelectedCells[2].Value.ToString());
                MostrarMaterialFormulacionesSemiProducido(datalistadoItemsMaterialSemiProducidoBusquedaCopiaFormulacion, datalistadoBusquedaCopiaFormulaciones.SelectedCells[2].Value.ToString());

                lblTipoFormulacionCopia.Text = datalistadoBusquedaCopiaFormulaciones.SelectedCells[1].Value.ToString();
            }
        }

        //REGRESAR DE BUSQEUDA Y COPIA DE FOMRULACIONES
        private void btnRegresarBusquedaCopiaFormulacion_Click(object sender, EventArgs e)
        {
            txtBusquedaCopiaFormulacion.Text = "";
            datalistadoBusquedaCopiaFormulaciones.DataSource = null;
            datalistadoItemsActividadesProductoBusquedaCopiaFormulacion.DataSource = null;
            datalistadoItemsActividadesSemiProducidoBusquedaCopiaFormulacion.DataSource = null;
            datalistadoItemsMaterialProducidoBusquedaCopiaFormulacion.DataSource = null;
            datalistadoItemsMaterialSemiProducidoBusquedaCopiaFormulacion.DataSource = null;
            panelBusquedaCopiaFormulaciones.Visible = false;
        }

        //FUNCION PARA COPIAR LOS DATOS DE LA FORMULACION A OTRA
        public void CopiarBusquedaFormulacion(DataGridView DGV1, DataGridView DGV2, DataGridView DGV3, DataGridView DGV4, DataGridView DGV5, DataGridView DGV6
            , DataGridView DGV7, DataGridView DGV8, string tipoformulacionCopia, string tipoformulacion, string codigoformulacionVision)
        {
            if (datalistadoBusquedaCopiaFormulaciones.CurrentRow == null)
            {
                MessageBox.Show("Debe seleccionar una formulación para copiar los datos.", "Validación del Sistema", MessageBoxButtons.OK);
                return;
            }
            else
            {
                if (tipoformulacionCopia == tipoformulacion)
                {
                    if (DGV5.RowCount == 0 && DGV6.RowCount == 0)
                    {
                        //INGRESAR ACTIVIDADES DEL PRODUCTO
                        foreach (DataGridViewRow row in DGV1.Rows)
                        {
                            try
                            {
                                SqlConnection con = new SqlConnection();
                                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                                con.Open();
                                SqlCommand cmd = new SqlCommand();
                                cmd = new SqlCommand("CreacionFormulacion_InsertarActividadProducto", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@codigoformulacion", codigoformulacionVision);
                                cmd.Parameters.AddWithValue("@codigoLOM", Convert.ToInt32(row.Cells[2].Value.ToString()));
                                cmd.Parameters.AddWithValue("@idcorrelativo", row.Cells[9].Value.ToString());
                                cmd.Parameters.AddWithValue("@tcosto", Convert.ToInt32(row.Cells[11].Value.ToString()));
                                cmd.Parameters.AddWithValue("@tsetup", Convert.ToDecimal(row.Cells[12].Value.ToString()));
                                cmd.Parameters.AddWithValue("@toperacion", Convert.ToDecimal(row.Cells[13].Value.ToString()));
                                cmd.Parameters.AddWithValue("@tpor", Convert.ToInt32(row.Cells[14].Value.ToString()));
                                cmd.Parameters.AddWithValue("@thoras", Convert.ToInt32(row.Cells[15].Value.ToString()));
                                cmd.Parameters.AddWithValue("@personal", Convert.ToDecimal(row.Cells[17].Value.ToString()));
                                cmd.Parameters.AddWithValue("@cpersonal", Convert.ToDecimal(row.Cells[16].Value.ToString()));
                                cmd.Parameters.AddWithValue("@ctotal", Convert.ToDecimal(row.Cells[18].Value.ToString()));
                                cmd.Parameters.AddWithValue("@idtipo", Convert.ToInt32(row.Cells[19].Value.ToString()));
                                cmd.ExecuteNonQuery();
                                con.Close();
                                MostrarDetalleFormulacionesProducto(datalistadoactividadesproducto, codigoformulacionVision);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error inesperado, " + ex.Message);
                            }
                        }

                        //INGRESAR ACTIVIDADES DEL SEMIPRODUCIDO
                        foreach (DataGridViewRow row in DGV2.Rows)
                        {
                            try
                            {
                                SqlConnection con = new SqlConnection();
                                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                                con.Open();
                                SqlCommand cmd = new SqlCommand();
                                cmd = new SqlCommand("CreacionFormulacion_InsertarActividadSemiProducido", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@codigoformulacion", codigoformulacionVision);
                                cmd.Parameters.AddWithValue("@codigoMOM", Convert.ToInt32(row.Cells[2].Value.ToString()));
                                cmd.Parameters.AddWithValue("@idcorrelativo", row.Cells[9].Value.ToString());
                                cmd.Parameters.AddWithValue("@tcosto", Convert.ToInt32(row.Cells[11].Value.ToString()));
                                cmd.Parameters.AddWithValue("@tsetup", Convert.ToDecimal(row.Cells[12].Value.ToString()));
                                cmd.Parameters.AddWithValue("@toperacion", Convert.ToDecimal(row.Cells[13].Value.ToString()));
                                cmd.Parameters.AddWithValue("@tpor", Convert.ToInt32(row.Cells[14].Value.ToString()));
                                cmd.Parameters.AddWithValue("@thoras", Convert.ToInt32(row.Cells[15].Value.ToString()));
                                cmd.Parameters.AddWithValue("@personal", Convert.ToDecimal(row.Cells[17].Value.ToString()));
                                cmd.Parameters.AddWithValue("@cpersonal", Convert.ToDecimal(row.Cells[16].Value.ToString()));
                                cmd.Parameters.AddWithValue("@ctotal", Convert.ToDecimal(row.Cells[18].Value.ToString()));
                                cmd.Parameters.AddWithValue("@idtipo", Convert.ToInt32(row.Cells[19].Value.ToString()));

                                cmd.ExecuteNonQuery();
                                con.Close();
                                MostrarDetalleFormulacionesSemiProducido(datalistadoactividadsemiproducido, codigoformulacionVision);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error inesperado, " + ex.Message);
                            }
                        }

                        //INGRESAR MATERIALES DE MI PRODUCTO
                        foreach (DataGridViewRow row in DGV3.Rows)
                        {
                            try
                            {
                                SqlConnection con = new SqlConnection();
                                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                                con.Open();
                                SqlCommand cmd = new SqlCommand();
                                cmd = new SqlCommand("CreacionFormulacion_InsertarMaterialProducto", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@codigoformulacion", codigoformulacionVision);
                                cmd.Parameters.AddWithValue("@idactividadproducto", Convert.ToInt32(row.Cells[1].Value.ToString()));
                                cmd.Parameters.AddWithValue("@idart", Convert.ToInt32(row.Cells[4].Value.ToString()));
                                cmd.Parameters.AddWithValue("@cantidad", Convert.ToDecimal(row.Cells[8].Value.ToString()));
                                cmd.Parameters.AddWithValue("@posicion", Convert.ToInt32(row.Cells[7].Value.ToString()));
                                cmd.Parameters.AddWithValue("@tipomaterial", "MATERIAL PRODUCTO");
                                cmd.Parameters.AddWithValue("@cantidadtotal", Convert.ToInt32("5"));
                                cmd.ExecuteNonQuery();
                                con.Close();
                                MostrarMaterialFormulacionesProducto(datalistadomaterialproducto, textocodigoformulacion);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error inesperado, " + ex.Message);
                            }
                        }

                        if (datalistadoBusquedaCopiaFormulaciones.SelectedCells[1].Value.ToString() == "CON SEMIPRODUCIDO")
                        {
                            //INGRESAR MATERIALES DE MI SEMIPRODUCIO
                            foreach (DataGridViewRow row in DGV4.Rows)
                            {
                                try
                                {
                                    SqlConnection con = new SqlConnection();
                                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                                    con.Open();
                                    SqlCommand cmd = new SqlCommand();
                                    cmd = new SqlCommand("CreacionFormulacion_InsertarMaterialProducto", con);
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.AddWithValue("@codigoformulacion", codigoformulacionVision);
                                    cmd.Parameters.AddWithValue("@idactividadproducto", Convert.ToInt32(row.Cells[1].Value.ToString()));
                                    cmd.Parameters.AddWithValue("@idart", Convert.ToInt32(row.Cells[4].Value.ToString()));
                                    cmd.Parameters.AddWithValue("@cantidad", Convert.ToDecimal(row.Cells[8].Value.ToString()));
                                    cmd.Parameters.AddWithValue("@posicion", Convert.ToInt32(row.Cells[7].Value.ToString()));
                                    cmd.Parameters.AddWithValue("@tipomaterial", "MATERIAL SEMIPRODUCIDO");
                                    cmd.Parameters.AddWithValue("@cantidadtotal", Convert.ToInt32("5"));
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                    MostrarMaterialFormulacionesSemiProducido(datalistadomaterialsemiproducido, textocodigoformulacion);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error inesperado, " + ex.Message);
                                }
                            }
                        }
                        else
                        {
                            //datalistadoItemsMaterialSemiProducidoBusquedaCopiaFormulacion.DataSource = DBNull.Value;
                        }

                        MessageBox.Show("Se copió la formulación exitosamente.", "Validación del Sistema");
                        panelBusquedaCopiaFormulaciones.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("La formulación en donde intenta copiar los datos ya tienen actividades o materiales ingresados, solo se puede copiar a formulaciones vaciias.", "Validación del Sistema", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("No se puede copiar la formulación porque son formulaciones de tipo diferente, SIN SEMIPRODUCIDO <> CON SEMIPRODUCIDO.", "Validación del Sistema", MessageBoxButtons.OK);
                }
            }
        }

        //FUNCION PARA COPIAR LOS DATOS DE LA FORMULACION A OTRA
        private void btnCopiarBusquedaCopiaFormulacion_Click(object sender, EventArgs e)
        {
            CopiarBusquedaFormulacion(datalistadoItemsActividadesProductoBusquedaCopiaFormulacion, datalistadoItemsActividadesSemiProducidoBusquedaCopiaFormulacion
            , datalistadoItemsMaterialProducidoBusquedaCopiaFormulacion, datalistadoItemsMaterialSemiProducidoBusquedaCopiaFormulacion, datalistadoactividadesproducto
            , datalistadoactividadsemiproducido, datalistadomaterialproducto, datalistadomaterialsemiproducido, lblTipoFormulacionCopia.Text, txtTipoFormulacion.Text
            , lblCodigoFormulacionVision.Text);
        }
    }
}

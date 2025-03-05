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
            BusquedaDatosPrincipales();
        }

        //METODO PARA PINTAR DE COLORES LAS FILAS DE MI LSITADO
        public void alternarColorFilas(DataGridView dgv)
        {
            try
            {
                {
                    var withBlock = dgv;
                    withBlock.RowsDefaultCellStyle.BackColor = Color.FromArgb(215, 227, 252);
                    withBlock.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //BUSQUEDA DE FORMULACION Y CARGA DE MI COMBO
        public void BusquedaDatosPrincipales()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT DF.IdDefinicionFormulaciones AS [ID], L.IdLinea ,L.Descripcion AS [LINEA], DF.IdTipo, TF.Descripcion AS [TIPO] FROM DefinicionFormulaciones DF INNER JOIN LINEAS L ON L.IdLinea = DF.IdLinea INNER JOIN TipoFormulacion TF ON TF.IdTipoFormulacion = DF.IdTipo WHERE DF.Estado = 1 ORDER BY L.IdLinea", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboDefinicionFormulacion.DisplayMember = "LINEA";
            cboDefinicionFormulacion.ValueMember = "ID";
            DataRow row = dt.Rows[0];
            txtTipoFormulacion.Text = System.Convert.ToString(row["TIPO"]);
            lblIdLinea.Text = System.Convert.ToString(row["IdLinea"]);
            cboDefinicionFormulacion.DataSource = dt;
        }

        //EVENTO DE CAMBIO DE DATO EN EL COMBO DE MIS DEFINICIONES CARGADAS
        private void cboDefinicionFormulacion_SelectionChangeCommitted(object sender, EventArgs e)
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

        //RECURSOS PARA GUARDAR LA FORMULACION Y LA ACCION DE GUARDAR FORMULACION
        public void CrearCodigoFormulacion()
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

            MostrarFormulaciones();
            alternarColorFilas(datalistadoFormulaciones);
        }

        //-----------------------------------------------------------------------FORMULACION--------------------------------------------------------------
        //LISTAR FORMULACIONES INGRESADAS
        public void MostrarFormulaciones()
        {
            if (txtTipoFormulacion.Text == "CON SEMIPRODUCIDO")
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter(" SELECT F.IdFormulacion, F.CodigoFormulacion as [COD. F.], P.IdArt, P.Codcom as [COD. P.], P.Detalle as [DESCRIPCIÓN P.], M.DESCRIPCION as [MEDIDA P.], P2.IdArt, P2.Codcom as [COD.SP.], P2.Detalle as [DESCRIPCIÓN S.], M2.DESCRIPCION as [MEDIDA S.], NamePlanoTecnico, NamePlanoSeguridad, PP.IdPlano, PP.NameReferences, PP.Name, PP2.IdPlano, PP2.NameReferences, PP2.Name FROM Formulacion F INNER JOIN PRODUCTOS P ON P.IdArt = F.IdProducto INNER JOIN PRODUCTOS P2 ON P2.IdArt = F.IdSemiProducido INNER JOIN MEDIDA M ON P.IdMedida = M.IdMedida INNER JOIN MEDIDA M2 ON P2.IdMedida = M2.IdMedida INNER JOIN PlanoProducto PP ON PP.IdPlano = F.IdPlanoProducto INNER JOIN PlanoProducto PP2 ON PP2.IdPlano = F.IdPlanoSemiproducido INNER JOIN DefinicionFormulaciones DF ON DF.IdDefinicionFormulaciones = F.IdDefinicionFormulacion WHERE F.Estado = 1 AND DF.IdTipo = 2 UNION ALL SELECT F.IdFormulacion, F.CodigoFormulacion as [COD.F.], P.IdArt, P.Codcom as [COD.P.], P.Detalle as [DESCRIPCIÓN P.], M.DESCRIPCION as [MEDIDA P.], P2.IdArt, P2.Codcom as [COD.SP.], P2.Detalle as [DESCRIPCIÓN S.], M2.DESCRIPCION as [MEDIDA S.], NamePlanoTecnico, NamePlanoSeguridad, NULL, NULL, NULL, NULL, NULL, NULL FROM Formulacion F INNER JOIN PRODUCTOS P ON P.IdArt = F.IdProducto INNER JOIN PRODUCTOS P2 ON P2.IdArt = F.IdSemiProducido INNER JOIN MEDIDA M ON P.IdMedida = M.IdMedida INNER JOIN MEDIDA M2 ON P2.IdMedida = M2.IdMedida INNER JOIN DefinicionFormulaciones DF ON DF.IdDefinicionFormulaciones = F.IdDefinicionFormulacion WHERE F.Estado = 1 AND DF.IdTipo = 2 AND F.IdPlanoProducto IS NULL", con);
                da.Fill(dt);
                datalistadoFormulaciones.DataSource = dt;
                con.Close();
            }
            else
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                da = new SqlDataAdapter("SELECT F.IdFormulacion, F.CodigoFormulacion as [COD. F.], P.IdArt, P.Codcom as [COD. P.], P.Detalle as [DESCRIPCIÓN P.], M.DESCRIPCION as [MEDIDA P.], NamePlanoTecnico, NamePlanoSeguridad, PP.IdPlano, PP.NameReferences, PP.Name FROM Formulacion F INNER JOIN PRODUCTOS P ON P.IdArt = F.IdProducto INNER JOIN MEDIDA M ON P.IdMedida = M.IdMedida INNER JOIN PlanoProducto PP ON PP.IdPlano = F.IdPlanoProducto INNER JOIN DefinicionFormulaciones DF ON DF.IdDefinicionFormulaciones = F.IdDefinicionFormulacion WHERE F.Estado = 1 AND DF.IdTipo = 1 UNION SELECT F.IdFormulacion, F.CodigoFormulacion as [COD.F.], P.IdArt, P.Codcom as [COD.P.], P.Detalle as [DESCRIPCIÓN P.], M.DESCRIPCION as [MEDIDA P.], NamePlanoTecnico, NamePlanoSeguridad, NULL, NULL, NULL FROM Formulacion F INNER JOIN PRODUCTOS P ON P.IdArt = F.IdProducto INNER JOIN MEDIDA M ON P.IdMedida = M.IdMedida INNER JOIN DefinicionFormulaciones DF ON DF.IdDefinicionFormulaciones = F.IdDefinicionFormulacion WHERE F.Estado = 1 AND DF.IdTipo = 1 AND F.IdPlanoProducto IS NULL", con);
                da.Fill(dt);
                datalistadoFormulaciones.DataSource = dt;
                con.Close();
            }
            AjustesColunmasMostrarFormulaicones(datalistadoFormulaciones);
            alternarColorFilas(datalistadoFormulaciones);
        }

        public void AjustesColunmasMostrarFormulaicones(DataGridView dgv)
        {
            if (txtTipoFormulacion.Text == "CON SEMIPRODUCIDO")
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

                dgv.Columns[2].Width = 50;
                dgv.Columns[4].Width = 90;
                dgv.Columns[5].Width = 320;
                dgv.Columns[6].Width = 90;

                dgv.Columns[8].Width = 90;
                dgv.Columns[9].Width = 320;
                dgv.Columns[10].Width = 90;

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

                dgv.Columns[2].Width = 100;
                dgv.Columns[4].Width = 150;
                dgv.Columns[5].Width = 650;
                dgv.Columns[6].Width = 150;
            }
            alternarColorFilas(dgv);
        }

        //CREACION DE UNA FORMULACION-------------------------------------------------------------------------
        //PRODUCTO---------------------------------------------------------------------------
        //BUSCAR LOS PLANOS ASOCIADOS AL PRODUCTO SELECCIOANDO
        public void MostrarPlanosSegunIdProducto(int id, DataGridView dgv)
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
            dgv.Columns[0].Width = 60;
            dgv.Columns[1].Width = 100;
            dgv.Columns[2].Width = 345;
            alternarColorFilas(dgv);
        }

        //VISUALOIZAR PALNOS SELECCIOANDOS
        public void VisualizarPlanosSeleccionados(DataGridView dgv, TextBox txt)
        {
            if (dgv != null)
            {
                if (dgv.CurrentRow != null)
                {
                    string ruta = dgv.SelectedCells[2].Value.ToString();
                    if (ruta == "")
                    {
                        MessageBox.Show("Seleccione un plano para continuar", "Abrir Plano");
                    }
                    else
                    {
                        Process.Start(ruta);
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione un plano para poder abrirlo", "Abrir Plano");
                }
            }
            else if (txt != null)
            {
                if (txt.Text != "")
                {
                    string ruta = txt.Text;
                    if (ruta == "")
                    {
                        MessageBox.Show("Seleccione un plano para continuar", "Abrir Plano");
                    }
                    else
                    {
                        Process.Start(ruta);
                    }
                }
                else
                {
                    MessageBox.Show("No hay un plano para abrir", "Abrir Plano");
                }
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
                    txtProducto.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    txtProducto.ForeColor = Color.Green;
                }
            }
            return;
        }

        //METODO PARA CARGAR LOS PRODUCTOS TERMINADOS
        public void CargarProductos()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            da = new SqlDataAdapter("SELECT Codcom AS [CÓDIGO], IdArt AS [C. ART],Detalle AS [DESCRIPCIÓN] FROM PRODUCTOS WHERE Estado = 1", con);
            da.Fill(dt);
            datalistadoproductos.DataSource = dt;
            con.Close();
            datalistadoproductos.Columns[0].Width = 110;
            datalistadoproductos.Columns[1].Width = 80;
            datalistadoproductos.Columns[2].Width = 620;
            alternarColorFilas(datalistadoproductos);
        }

        //SELECCIONAR LE PRODUCTO Y LELVARLO A MI FORMUALCION
        private void datalistadoproductos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            lblCodigoProducto.Text = datalistadoproductos.SelectedCells[0].Value.ToString();
            idartproducto = Convert.ToInt32(datalistadoproductos.SelectedCells[1].Value.ToString());
            txtProducto.Text = datalistadoproductos.SelectedCells[2].Value.ToString();
            panelBusquedaProducto.Visible = false;
            txtCodigoPlanoProducto.Text = "";
            txtRutaPlanoProducto.Text = "";
            MostrarPlanosSegunIdProducto(idartproducto, datalistadopdfProducto);
            ColorDescripcionProducto();
        }

        //SELECCIONAR UN PLANO DE PRODUCTO
        private void datalistadopdfProducto_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            lblCodigoPlanoProducto.Text = datalistadopdfProducto.SelectedCells[0].Value.ToString();
            txtCodigoPlanoProducto.Text = datalistadopdfProducto.SelectedCells[1].Value.ToString();
            txtRutaPlanoProducto.Text = datalistadopdfProducto.SelectedCells[2].Value.ToString();
        }

        //VISUALIZAR PLANO
        private void btnVisualizarPlanoProducto_Click(object sender, EventArgs e)
        {
            VisualizarPlanosSeleccionados(datalistadopdfProducto, null);
        }

        //LIMPIAR PLANO SELECCIONADO
        private void btnLimpiarPlanoProducto_Click(object sender, EventArgs e)
        {
            txtCodigoPlanoProducto.Text = "";
            txtRutaPlanoProducto.Text = "";
            lblCodigoPlanoProducto.Text = "*";
        }

        //SALIR DE LA BUSQUEDA DE MI PRODUCTO
        private void btnSalirBusquedaProducto_Click(object sender, EventArgs e)
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
                    txtSemiProducido.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    txtSemiProducido.ForeColor = Color.Green;
                }
            }
            return;
        }

        //METODO PARA CARGAR LOS PRODUCTOS SEMIPRODUCIDO
        public void CargarSemiProducido()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            da = new SqlDataAdapter("SELECT Codcom AS [CÓDIGO], IdArt AS [C. ART],Detalle AS [DESCRIPCIÓN] FROM PRODUCTOS WHERE Estado = 1 AND SemiProducido = 1", con);
            da.Fill(dt);
            datalistadoSemiProducido.DataSource = dt;
            con.Close();
            datalistadoSemiProducido.Columns[0].Width = 110;
            datalistadoSemiProducido.Columns[1].Width = 80;
            datalistadoSemiProducido.Columns[2].Width = 620;
            alternarColorFilas(datalistadoSemiProducido);
        }

        //SELECCIONAR EL SEMIPRODUCIDO LELVARLO A MI FORMUALCION
        private void datalistadoSemiProducido_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            lblCodigoSemiProducido.Text = datalistadoSemiProducido.SelectedCells[0].Value.ToString();
            idartsemiproducido = Convert.ToInt32(datalistadoSemiProducido.SelectedCells[1].Value.ToString());
            txtSemiProducido.Text = datalistadoSemiProducido.SelectedCells[2].Value.ToString();
            panelBusquedaSemiProducido.Visible = false;
            txtCodigoPlanoSemiProducido.Text = "";
            txtRutaPlanoSemiProducido.Text = "";
            MostrarPlanosSegunIdProducto(idartsemiproducido, datalistadopdfSemiProducido);
            ColorDescripcionSemiProducido();
        }

        //SELECCIONAR UN PLANO DEL SEMIPRODUCIDO
        private void datalistadopdfSemiProducido_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            lblCodigoPlanoSemiProducido.Text = datalistadopdfSemiProducido.SelectedCells[0].Value.ToString();
            txtCodigoPlanoSemiProducido.Text = datalistadopdfSemiProducido.SelectedCells[1].Value.ToString();
            txtRutaPlanoSemiProducido.Text = datalistadopdfSemiProducido.SelectedCells[2].Value.ToString();
        }

        //VISUALIZAR PLANO
        private void btnAbrirPdfSemiProducido_Click(object sender, EventArgs e)
        {
            VisualizarPlanosSeleccionados(datalistadopdfSemiProducido, null);
        }

        //LIMPIAR PLANO SELECCIONADO
        private void btnLimpiarPlanoSemiPorducido_Click(object sender, EventArgs e)
        {
            txtCodigoPlanoSemiProducido.Text = "";
            txtRutaPlanoSemiProducido.Text = "";
            lblCodigoPlanoSemiProducido.Text = "*";
        }

        //BOTON PARA SALIR D ELA BUSQUEDA DE MI SEMI-PRODUCIDO
        private void btnBusquedaSemiProducido_Click(object sender, EventArgs e)
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
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (txtTipoFormulacion.Text == "CON SEMIPRODUCIDO")
            {
                if (txtProducto.Text == "" || txtSemiProducido.Text == "")
                {
                    MessageBox.Show("Debe seleccionar todos los datos necesarios (producto, semiproducido, plano del producto y plano del semiproducido), solo se selecciona el semiproducido si aplica", "Validación de Sistema");
                }
                else
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("InsertarFormulacion", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        CrearCodigoFormulacion();
                        cmd.Parameters.AddWithValue("@codigoFormulacion", textocodigoformulacion);
                        cmd.Parameters.AddWithValue("@idProducto", idartproducto);
                        cmd.Parameters.AddWithValue("@idsemiproducido", idartsemiproducido);

                        //plano hoja tecnica
                        if (txtFileHojaTecnica.Text == "")
                        {
                            cmd.Parameters.AddWithValue("@rutaPlanoTecnico", DBNull.Value);
                        }
                        else
                        {
                            string RutaNew = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Procesos\PlanosTecnicos\" + textocodigoformulacion + " - " + "Plano Tecnico" + ".pdf";
                            string RutaOld = txtFileHojaTecnica.Text;
                            File.Copy(RutaOld, RutaNew);
                            cmd.Parameters.AddWithValue("@rutaPlanoTecnico", RutaNew);
                        }

                        //plano hoja de seguridad
                        if (txtFileHojaSeguridad.Text == "")
                        {
                            cmd.Parameters.AddWithValue("@rutaPlanoSeguridad", DBNull.Value);
                        }
                        else
                        {
                            string RutaNew = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Procesos\PlanosSeguridad\" + textocodigoformulacion + " - " + "Plano Seguridad" + ".pdf";
                            string RutaOld = txtFileHojaSeguridad.Text;
                            File.Copy(RutaOld, RutaNew);
                            cmd.Parameters.AddWithValue("@rutaPlanoSeguridad", RutaNew);
                        }

                        cmd.Parameters.AddWithValue("@cif", Convert.ToDecimal(txtCif.Text));

                        if (lblCodigoPlanoProducto.Text == "*")
                        {
                            cmd.Parameters.AddWithValue("@idPlanoProducto", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@idPlanoProducto", lblCodigoPlanoProducto.Text);
                        }

                        if (lblCodigoPlanoSemiProducido.Text == "*")
                        {
                            cmd.Parameters.AddWithValue("@idPlanoSemiproducido", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@idPlanoSemiproducido", lblCodigoPlanoSemiProducido.Text);
                        }

                        cmd.Parameters.AddWithValue("@idDefinicionFormulacion", Convert.ToInt32(lblIdDefinicion.Text));

                        cmd.ExecuteNonQuery();
                        con.Close();
                        MostrarFormulaciones();
                        MessageBox.Show("Registro ingresado exitosamente", "Nueva Formulación", MessageBoxButtons.OK);

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
                        MessageBox.Show(ex.Message + " " + "Debe seleccionar todos los datos necesarios (producto, semiproducido, plano del producto y plano del semiproducido), solo se selecciona el semiproducido si aplica.", "Validación de Sistema");

                    }
                }
            }
            else
            {
                if (txtProducto.Text == "")
                {
                    MessageBox.Show("Debe seleccionar todos los datos necesarios (producto y plano del producto), solo se selecciona el semiproducido si aplica", "Validación de Sistema");
                }
                else
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("InsertarFormulacion", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        CrearCodigoFormulacion();
                        cmd.Parameters.AddWithValue("@codigoFormulacion", textocodigoformulacion);
                        cmd.Parameters.AddWithValue("@idProducto", idartproducto);
                        cmd.Parameters.AddWithValue("@idsemiproducido", DBNull.Value);

                        //plano hoja tecnica
                        if (txtFileHojaTecnica.Text == "")
                        {
                            cmd.Parameters.AddWithValue("@rutaPlanoTecnico", DBNull.Value);
                        }
                        else
                        {
                            string RutaNew = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Procesos\PlanosTecnicos\" + textocodigoformulacion + " - " + "Plano Tecnico" + ".pdf";
                            string RutaOld = txtFileHojaTecnica.Text;
                            File.Copy(RutaOld, RutaNew);
                            cmd.Parameters.AddWithValue("@rutaPlanoTecnico", RutaNew);
                        }

                        //plano hoja de seguridad
                        if (txtFileHojaSeguridad.Text == "")
                        {
                            cmd.Parameters.AddWithValue("@rutaPlanoSeguridad", DBNull.Value);
                        }
                        else
                        {
                            string RutaNew = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Procesos\PlanosSeguridad\" + textocodigoformulacion + " - " + "Plano Seguridad" + ".pdf";
                            string RutaOld = txtFileHojaSeguridad.Text;
                            File.Copy(RutaOld, RutaNew);
                            cmd.Parameters.AddWithValue("@rutaPlanoSeguridad", RutaNew);
                        }

                        cmd.Parameters.AddWithValue("@cif", Convert.ToDecimal(txtCif.Text));

                        if (lblCodigoPlanoProducto.Text == "*")
                        {
                            cmd.Parameters.AddWithValue("@idPlanoProducto", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@idPlanoProducto", lblCodigoPlanoProducto.Text);
                        }

                        if (lblCodigoPlanoSemiProducido.Text == "*")
                        {
                            cmd.Parameters.AddWithValue("@idPlanoSemiproducido", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@idPlanoSemiproducido", lblCodigoPlanoSemiProducido.Text);
                        }

                        cmd.Parameters.AddWithValue("@idDefinicionFormulacion", Convert.ToInt32(lblIdDefinicion.Text));

                        cmd.ExecuteNonQuery();
                        con.Close();
                        MostrarFormulaciones();
                        MessageBox.Show("Registro ingresado exitosamente", "Nueva Formulación", MessageBoxButtons.OK);

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
                        MessageBox.Show(ex.Message + " " + "Debe seleccionar todos los datos necesarios (producto, semiproducido, plano del producto y plano del semiproducido), solo se selecciona el semiproducido si aplica.", "Validación de Sistema");
                    }
                }
            }
        }

        //EDITAR LA FORMULACION
        private void btnEditar_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea editar esta formulación?.", "Validación de Sistema", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                try
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("EditarFormulacion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idFormulacion", Convert.ToInt32(datalistadoFormulaciones.SelectedCells[1].Value.ToString()));

                    if (lblCodigoPlanoProducto.Text == "*")
                    {
                        cmd.Parameters.AddWithValue("@idPlanoProducto", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@idPlanoProducto", Convert.ToInt32(lblCodigoPlanoProducto.Text));
                    }

                    if (lblCodigoPlanoSemiProducido.Text == "*")
                    {
                        cmd.Parameters.AddWithValue("@idPlanoSemiProducido", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@idPlanoSemiProducido", Convert.ToInt32(lblCodigoPlanoSemiProducido.Text));
                    }

                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Se editó correctamente la formulación seleccionada", "Validación del Sistema", MessageBoxButtons.OK);
                    MostrarFormulaciones();

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

        //ANULAR FORMULACION
        private void btnAnular_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea anular esta formulación?.", "Validación de Sistema", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                try
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("AnularFormulacion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idFormulacion", Convert.ToInt32(datalistadoFormulaciones.SelectedCells[1].Value.ToString()));

                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Se eliminó correctamente la formulación seleccionada", "Validación del Sistema", MessageBoxButtons.OK);
                    MostrarFormulaciones();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
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

                    MostrarDetalleFormulacionesSemiProducido(datalistadoactividadsemiproducido, textocodigoformulacion);
                    MostrarMaterialFormulacionesSemiProducido(datalistadomaterialsemiproducido, textocodigoformulacion);
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
                }
            }
        }

        //SELECCIONAR UNA FOMRULACION CON DOBLE CLICK
        private void datalistadoFormulaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (txtTipoFormulacion.Text == "CON SEMIPRODUCIDO")
            {
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

        //LIMPIAR BUSQUEDA PARA REALIZARLO POR OTRO CRITERIO
        private void cboBusquedaFormulacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFormulaciones.Text = "";
        }

        //fFUNCION PARA BUSCAR POR CRITERIOS
        private void txtFormulaciones_TextChanged(object sender, EventArgs e)
        {
            if (cboBusquedaFormulacion.Text == "DESCRIPCIÓN")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarFormulacionPorDescripcion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@descripcion", txtFormulaciones.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoFormulaciones.DataSource = dt;
                con.Close();
                datalistadoFormulaciones.Columns[1].Visible = false;
                datalistadoFormulaciones.Columns[3].Visible = false;
                datalistadoFormulaciones.Columns[7].Visible = false;
                datalistadoFormulaciones.Columns[11].Visible = false;
                datalistadoFormulaciones.Columns[12].Visible = false;
                datalistadoFormulaciones.Columns[13].Visible = false;
                datalistadoFormulaciones.Columns[14].Visible = false;
                datalistadoFormulaciones.Columns[15].Visible = false;
                datalistadoFormulaciones.Columns[16].Visible = false;
                datalistadoFormulaciones.Columns[17].Visible = false;
                datalistadoFormulaciones.Columns[18].Visible = false;

                datalistadoFormulaciones.Columns[2].Width = 70;
                datalistadoFormulaciones.Columns[4].Width = 101;
                datalistadoFormulaciones.Columns[5].Width = 320;
                datalistadoFormulaciones.Columns[6].Width = 90;

                datalistadoFormulaciones.Columns[8].Width = 101;
                datalistadoFormulaciones.Columns[9].Width = 320;
                datalistadoFormulaciones.Columns[10].Width = 90;
                alternarColorFilas(datalistadoFormulaciones);
            }
            else if (cboBusquedaFormulacion.Text == "CÓDIGO")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarFormulacionPorCodigo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigo", txtFormulaciones.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoFormulaciones.DataSource = dt;
                con.Close();
                datalistadoFormulaciones.Columns[1].Visible = false;
                datalistadoFormulaciones.Columns[3].Visible = false;
                datalistadoFormulaciones.Columns[7].Visible = false;
                datalistadoFormulaciones.Columns[11].Visible = false;
                datalistadoFormulaciones.Columns[12].Visible = false;
                datalistadoFormulaciones.Columns[13].Visible = false;
                datalistadoFormulaciones.Columns[14].Visible = false;
                datalistadoFormulaciones.Columns[15].Visible = false;
                datalistadoFormulaciones.Columns[16].Visible = false;
                datalistadoFormulaciones.Columns[17].Visible = false;
                datalistadoFormulaciones.Columns[18].Visible = false;

                datalistadoFormulaciones.Columns[2].Width = 70;
                datalistadoFormulaciones.Columns[4].Width = 101;
                datalistadoFormulaciones.Columns[5].Width = 320;
                datalistadoFormulaciones.Columns[6].Width = 90;

                datalistadoFormulaciones.Columns[8].Width = 101;
                datalistadoFormulaciones.Columns[9].Width = 320;
                datalistadoFormulaciones.Columns[10].Width = 90;
                alternarColorFilas(datalistadoFormulaciones);
            }
        }

        //LIMPIAR BUSQUEDA PARA REALIZARLO POR OTRO CRITERIO
        private void cboBusquedaProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusquedaProducto.Text = "";
        }

        //BÚSQUEDA DE PRODUCTO
        private void txtBusquedaProducto_TextChanged(object sender, EventArgs e)
        {
            if (cboBusquedaProductos.Text == "DESCRIPCIÓN")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarProductoPorDescripcionFormulacion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@descripcion", txtBusquedaProducto.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoproductos.DataSource = dt;
                con.Close();
                datalistadoproductos.Columns[0].Width = 110;
                datalistadoproductos.Columns[1].Width = 90;
                datalistadoproductos.Columns[2].Width = 675;
                alternarColorFilas(datalistadoproductos);
            }
            else if (cboBusquedaProductos.Text == "CÓDIGO")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarProductoPorCodigoFormulacion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigo", txtBusquedaProducto.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoproductos.DataSource = dt;
                con.Close();
                datalistadoproductos.Columns[0].Width = 110;
                datalistadoproductos.Columns[1].Width = 90;
                datalistadoproductos.Columns[2].Width = 675;
                alternarColorFilas(datalistadoproductos);
            }
        }

        //LIMPIAR BUSQUEDA PARA REALIZARLO POR OTRO CRITERIO
        private void cboBusquedaSemiProducido_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusquedaSemiProducido.Text = "";
        }

        //BÚSQUEDA DE SEMIPRODUCIDOS
        private void txtBusquedaSemiProducido_TextChanged(object sender, EventArgs e)
        {
            if (cboBusquedaSemiProducido.Text == "DESCRIPCIÓN")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarProductoPorDescripcionFormulacion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@descripcion", txtBusquedaSemiProducido.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoSemiProducido.DataSource = dt;
                con.Close();
                datalistadoSemiProducido.Columns[0].Width = 110;
                datalistadoSemiProducido.Columns[1].Width = 90;
                datalistadoSemiProducido.Columns[2].Width = 675;
                alternarColorFilas(datalistadoSemiProducido);
            }
            else if (cboBusquedaSemiProducido.Text == "CÓDIGO")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarProductoPorCodigoFormulacion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigo", txtBusquedaSemiProducido.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoSemiProducido.DataSource = dt;
                con.Close();
                datalistadoSemiProducido.Columns[0].Width = 110;
                datalistadoSemiProducido.Columns[1].Width = 90;
                datalistadoSemiProducido.Columns[2].Width = 675;
                alternarColorFilas(datalistadoSemiProducido);
            }
        }
        //------------------------------------------------------------------------------------------

        //FORMULACION - ACCIONES - PROCESOS - PROCEDIMEINTOS -----------------------------------
        //ACCIONES DE LOS DETALLES DE MI FORMULACION------------------------------------------------
        //CARGA DE RECUROS - ACTIVIDADES PRODUCTO DETALLE
        public void MostrarDetalleFormulacionesProducto(DataGridView dgv, string idformulacion)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarFormulacionActividadProductos", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idformulacion", idformulacion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dgv.DataSource = dt;
            con.Close();
            dgv.Columns[0].Visible = false;
            dgv.Columns[1].Visible = false;
            dgv.Columns[2].Visible = false;
            dgv.Columns[3].Visible = false;
            dgv.Columns[4].Visible = false;
            dgv.Columns[5].Visible = false;
            dgv.Columns[7].Visible = false;
            dgv.Columns[9].Visible = false;
            dgv.Columns[19].Visible = false;

            dgv.Columns[6].Width = 200;
            dgv.Columns[8].Width = 250;
            dgv.Columns[10].Width = 100;
            dgv.Columns[11].Width = 65;
            dgv.Columns[12].Width = 65;
            dgv.Columns[13].Width = 90;
            dgv.Columns[14].Width = 65;
            dgv.Columns[15].Width = 65;
            dgv.Columns[16].Width = 85;
            dgv.Columns[17].Width = 85;
            dgv.Columns[18].Width = 65;
            dgv.Columns[20].Width = 65;
            alternarColorFilas(dgv);
        }

        //CARGA DE RECUROS - ACTIVIDADES SEMIPRODUCIDO DETALLE
        public void MostrarDetalleFormulacionesSemiProducido(DataGridView dgv, string idformulacion)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarFormulacionActividadSemiProducido", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idformulacion", idformulacion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dgv.DataSource = dt;
            con.Close();
            dgv.Columns[0].Visible = false;
            dgv.Columns[1].Visible = false;
            dgv.Columns[2].Visible = false;
            dgv.Columns[3].Visible = false;
            dgv.Columns[4].Visible = false;
            dgv.Columns[5].Visible = false;
            dgv.Columns[7].Visible = false;
            dgv.Columns[9].Visible = false;
            dgv.Columns[19].Visible = false;

            dgv.Columns[6].Width = 200;
            dgv.Columns[8].Width = 250;
            dgv.Columns[10].Width = 100;
            dgv.Columns[11].Width = 65;
            dgv.Columns[12].Width = 65;
            dgv.Columns[13].Width = 90;
            dgv.Columns[14].Width = 65;
            dgv.Columns[15].Width = 65;
            dgv.Columns[16].Width = 85;
            dgv.Columns[17].Width = 85;
            dgv.Columns[18].Width = 65;
            dgv.Columns[20].Width = 65;
            alternarColorFilas(dgv);
        }

        //MATERIALES----------------
        //BUSQUEDA DE MATERIALES DEL PRODUTO
        public void MostrarMaterialFormulacionesProducto(DataGridView DGV, string idformulacion)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarFormulacionMaterialProducto", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idformulacion", idformulacion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            DGV.DataSource = dt;
            con.Close();
            DGV.Columns[0].Visible = false;
            DGV.Columns[1].Visible = false;
            DGV.Columns[2].Visible = false;

            DGV.Columns[3].Width = 90;
            DGV.Columns[4].Width = 60;
            DGV.Columns[5].Width = 280;
            DGV.Columns[6].Width = 100;
            DGV.Columns[7].Width = 75;
            DGV.Columns[8].Width = 75;
            DGV.Columns[9].Width = 100;
            alternarColorFilas(DGV);
        }

        //BUSQUEDA DE MATERIALES DEL SEMIPRODUCIDO
        public void MostrarMaterialFormulacionesSemiProducido(DataGridView DGV, string idformulacion)
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
            DGV.Columns[0].Visible = false;
            DGV.Columns[1].Visible = false;
            DGV.Columns[2].Visible = false;

            DGV.Columns[3].Width = 90;
            DGV.Columns[4].Width = 60;
            DGV.Columns[5].Width = 280;
            DGV.Columns[6].Width = 100;
            DGV.Columns[7].Width = 75;
            DGV.Columns[8].Width = 75;
            DGV.Columns[9].Width = 100;
            alternarColorFilas(DGV);
        }

        //INGRESO, EDICION, ELIMINACION CON SUS RESPESCTIVOS BOTONES PRODUCTOS---------------------------
        //ACCIONES DE LA FORMULACION------------------------------------------------------------
        //CARGAR LÍNEAS
        public void CargarLineas()
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

        //CARGAR OPERACIONES
        public void CargarOperacion(string idlinea)
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

        //CARGAR MAQUINARIA
        public void CargarMaquinaria(string idlinea, string idoperacion)
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

        //CARGAR LINEA- OPERACIÓN Y MAQUINARIA VALIDACIÓN 
        public void CargarLOMValidacion(string idlinea, string idoperacion, string idmaquinaria)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostarLOMValidacion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idlinea", idlinea);
            cmd.Parameters.AddWithValue("@idoperacion", idoperacion);
            cmd.Parameters.AddWithValue("@idmaquinaria", idmaquinaria);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoLOM.DataSource = dt;
            con.Close();
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

        //CARGA DEL TIPO DE OPERACIÓN
        public void CargarTipoOperacion()
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
        private void btnConfirmarActividadProducto_Click(object sender, EventArgs e)
        {
            if (cboMaquinaria.Text == "" || cboOperacion.Text == "" || txtTcosto.Text == "" || txtTsetup.Text == "" || txtToperacion.Text == "" || txtTpor.Text == "" || txtPersonal.Text == "" || txtCpersonal.Text == "")
            {
                MessageBox.Show("Debe llenar todos los campos para continuar.", "REGISTRO", MessageBoxButtons.OKCancel);
            }
            else
            {
                try
                {
                    if (datalistadoLOM.SelectedRows.Count != 1)
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
                            cmd = new SqlCommand("InsertarFormulacionActividadProducto", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@codigoformulacion", lblCodigoFormulacionVision.Text);
                            cmd.Parameters.AddWithValue("@codigoLOM", Convert.ToInt32(datalistadoLOM.SelectedCells[0].Value.ToString()));
                            cmd.Parameters.AddWithValue("@idcorrelativo", cboCorrelativo.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@tcosto", Convert.ToInt32(txtTcosto.Text));
                            cmd.Parameters.AddWithValue("@tsetup", Convert.ToDecimal(txtTsetup.Text));
                            cmd.Parameters.AddWithValue("@toperacion", Convert.ToDecimal(txtToperacion.Text));
                            cmd.Parameters.AddWithValue("@tpor", Convert.ToInt32(txtTpor.Text));
                            cmd.Parameters.AddWithValue("@thoras", Convert.ToInt32(txtHoras.Text));
                            cmd.Parameters.AddWithValue("@personal", Convert.ToInt32(txtPersonal.Text));
                            cmd.Parameters.AddWithValue("@cpersonal", Convert.ToDecimal(txtCpersonal.Text));
                            decimal ctotalsuma = Convert.ToDecimal(txtCpersonal.Text) + Convert.ToDecimal(txtPersonal.Text);
                            cmd.Parameters.AddWithValue("@ctotal", ctotalsuma);
                            cmd.Parameters.AddWithValue("@idtipo", cboTipoOperacion.SelectedValue.ToString());

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

        //METODO PARA ELIMINAR UNA ACTIVIDAD A MI PRODUCTO
        private void btnEliminarActividadProducto_Click(object sender, EventArgs e)
        {
            if (datalistadoactividadesproducto.CurrentRow != null)
            {
                DialogResult boton = MessageBox.Show("¿Realmente desea eliminar esta actividad?.", "Validación de Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("CambiarEstadoActividadProducto", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idActividadProducto", Convert.ToInt32(datalistadoactividadesproducto.SelectedCells[0].Value.ToString()));
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Se eliminó correctamente", "Validación del Sistema", MessageBoxButtons.OK);
                        MostrarDetalleFormulacionesProducto(datalistadoactividadesproducto, lblCodigoFormulacionVision.Text);
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

        //CARGAR MODELOS SEMIPRODUCIDO
        public void CargarModeloS()
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

        //CARGAR OPERACIONES SEMIPRODUCIDO
        public void CargarOperacionesS(string idmodelo)
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

        //CARGAR MAQUINARIAS SEMIPRODUCIDO
        public void CargarMaquinariaS(string idmodelo, string idoperacion)
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

        //CARGAR LINEA - MODELO - OPERACIÓN Y MAQUINARIA VALIDACIÓN 
        public void CargarMOMValidacion(string idmodelo, string idoperacion, string idmaquinaria)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostarMOMValidacion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idmodelo", idmodelo);
            cmd.Parameters.AddWithValue("@idoperacion", idoperacion);
            cmd.Parameters.AddWithValue("@idmaquinaria", idmaquinaria);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoMOM.DataSource = dt;
            con.Close();
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

        //CARGAR TIPO DE OPERACIÓN
        public void CargarTipoOperacionS()
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

        //METODO PARA INGRESAR UNA NUEVA ACTIVIDAD A MI PRODUCTO
        private void btnConfirmarActividadSemiProducido_Click(object sender, EventArgs e)
        {
            if (cboMaquinariaS.Text == "" || cboOperacionS.Text == "" || txtTcostoS.Text == "" || txtTsetupS.Text == "" || txtToperacionS.Text == "" || txtTporS.Text == "" || txtPersonalS.Text == "" || txtCpersonalS.Text == "")
            {
                MessageBox.Show("Debe llenar todos los campos para continuar", "REGISTRO", MessageBoxButtons.OKCancel);
            }
            else
            {
                try
                {
                    if (datalistadoMOM.SelectedRows.Count != 1)
                    {
                        MessageBox.Show("Se encontraron 2 o más registros repetidos, por favor verificar los modelos por operación por maquinaria ingresados", "Error Inesperado", MessageBoxButtons.OK);
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
                            cmd = new SqlCommand("InsertarFormulacionActividadSemiProducido", con);
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

                            MessageBox.Show("Registro ingresado exitosamente", "Nueva Actividad", MessageBoxButtons.OK);
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

        //METODO PARA ELIMINAR UNA ACTIVIDAD A MI SEMI-PRODUCIDO
        private void btnEliminarActividadSemiProducido_Click(object sender, EventArgs e)
        {
            if (datalistadoactividadsemiproducido.CurrentRow != null)
            {
                DialogResult boton = MessageBox.Show("¿Realmente desea eliminar esta actividad?.", "Validación de Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("CambiarEstadoActividadSemiProducto", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idActividadSemiProducto", Convert.ToInt32(datalistadoactividadsemiproducido.SelectedCells[0].Value.ToString()));

                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Se eliminó correctamente.", "Validación del Sistema", MessageBoxButtons.OK);
                        MostrarDetalleFormulacionesSemiProducido(datalistadoactividadsemiproducido, lblCodigoFormulacionVision.Text);
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
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            da = new SqlDataAdapter("SELECT Codcom AS [CÓDIGO], IdArt AS [C. ART], Detalle AS [DESCRIPCIÓN] , M.Descripcion AS [MEDIDA] FROM PRODUCTOS P INNER JOIN MEDIDA M ON M.IdMedida = P.IdMedida WHERE P.Estado = 1 AND IdTipoMercaderias IN (16,15)", con);
            da.Fill(dt);
            dgv.DataSource = dt;
            con.Close();
            dgv.Columns[0].Width = 100;
            dgv.Columns[1].Width = 90;
            dgv.Columns[2].Width = 445;
            dgv.Columns[3].Width = 160;
            alternarColorFilas(dgv);
        }

        //SELECCIOANR LA ACTIVIDAD Y ABRIRA LOS AMTERIALES ASIGNADOS A ESTA
        private void datalistadoactividadesproducto_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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

        //SELECCIOANR EL AMTERIAL REQUERIDO PARA PODER INGRESARLO
        private void datalistadomaterialseleccionarseleccionar_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaterialNombre.Text = datalistadomaterialseleccionarseleccionar.SelectedCells[2].Value.ToString();
            txtMaterialUnidad.Text = datalistadomaterialseleccionarseleccionar.SelectedCells[0].Value.ToString();
            lblMaterialCodigo.Text = datalistadomaterialseleccionarseleccionar.SelectedCells[1].Value.ToString();
        }

        private void btnConfirmarMaterial_Click(object sender, EventArgs e)
        {
            if (txtMaterialCantidad.Text == "" || txtMaterialCantidadTotal.Text == "" || txtMaterialNombre.Text == "")
            {
                MessageBox.Show("Debe Seleccionar un producto o llenar todos los datos necesarios", "Valiración del Sistema", MessageBoxButtons.OK);
            }
            else
            {

                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("InsertarFormulacionMaterialProducto", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@codigoformulacion", lblMaterialFormulacion.Text);
                cmd.Parameters.AddWithValue("@idactividadproducto", Convert.ToInt32(lblMaterialCodigoOperacion.Text));
                cmd.Parameters.AddWithValue("@idart", Convert.ToInt32(lblMaterialCodigo.Text));
                cmd.Parameters.AddWithValue("@cantidad", Convert.ToDecimal(txtMaterialCantidad.Text));
                cmd.Parameters.AddWithValue("@posicion", Convert.ToInt32(txtMaterialCorrelativo.Text));
                if (lblTituloMateriales.Text == "Materia Prima X Producto")
                {
                    cmd.Parameters.AddWithValue("@tipomaterial", "MATERIAL PRODUCTO");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@tipomaterial", "MATERIAL SEMIPRODUCIDO");
                }

                cmd.Parameters.AddWithValue("@cantidadtotal", Convert.ToInt32(txtMaterialCantidadTotal.Text));

                cmd.ExecuteNonQuery();
                con.Close();

                txtMaterialCantidad.Text = "";
                txtMaterialBusqueda.Text = "";
                //txtMaterialCantidadTotal.Text = "";
                txtMaterialNombre.Text = "";
                txtMaterialUnidad.Text = "";
                lblMaterialCodigo.Text = "**************";

                MessageBox.Show("Registro ingresado exitosamente", "Nuevo Material", MessageBoxButtons.OK);

                panelMaterialproducto.Visible = false;
            }

            MostrarMaterialFormulacionesProducto(datalistadomaterialproducto, textocodigoformulacion);
            MostrarMaterialFormulacionesSemiProducido(datalistadomaterialsemiproducido, textocodigoformulacion);
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
                MessageBox.Show("Debe seleccionar un material válido para poder realizar el intercambio", "Validación del Sistema");
            }
            else
            {
                DialogResult boton = MessageBox.Show("¿Realmente desea intercambiar este producto?.", "Validación de Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("CambioMaterialFormulacion", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idArt", Convert.ToInt32(datalistadobusquedamaterial.SelectedCells[1].Value.ToString()));
                        cmd.Parameters.AddWithValue("@idMaterial", Convert.ToInt32(DGV.SelectedCells[0].Value.ToString()));

                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Se editó el material correctamente", "Validación del Sistema", MessageBoxButtons.OK);
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
            DialogResult boton = MessageBox.Show("¿Realmente desea eliminar este material?.", "Validación de Sistema", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                try
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("CambiarEstadoMaterialActividad", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idActividadMaterial", Convert.ToInt32(DGV.SelectedCells[0].Value.ToString()));

                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Se eliminó correctamente", "Validación del Sistema", MessageBoxButtons.OK);
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
            //txtMaterialCantidadTotal.Text = "";
        }

        //CARGA DE MATERIAS PRIMAS DEL SEMIPRODUCID-------------------------------------------
        //SELECCIOANR LA ACTIVIDAD Y ABRIRA LOS AMTERIALES ASIGNADOS A ESTA
        private void datalistadoactividadsemiproducido_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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
            txtCodigoBusquedaMaterial.Text = datalistadobusquedamaterial.SelectedCells[0].Value.ToString();
            txtBusquedaMaterial.Text = datalistadobusquedamaterial.SelectedCells[2].Value.ToString();
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
        private void txtBusquedaMateriales_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarPorDetallesMaterialformulacion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@detalle", txtBusquedaMateriales.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadobusquedamaterial.DataSource = dt;
            con.Close();
            datalistadobusquedamaterial.Columns[1].Visible = false;
            datalistadobusquedamaterial.Columns[3].Visible = false;

            datalistadobusquedamaterial.Columns[0].Width = 90;
            datalistadobusquedamaterial.Columns[2].Width = 300;
            datalistadobusquedamaterial.Columns[4].Width = 150;
            alternarColorFilas(datalistadobusquedamaterial);
        }

        //BUSCAR MATEIRALES POR DESCRIPCION
        private void txtMaterialBusqueda_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarPorDetallesMaterialformulacionF", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@detalle", txtMaterialBusqueda.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadomaterialseleccionarseleccionar.DataSource = dt;
            con.Close();
            datalistadomaterialseleccionarseleccionar.Columns[0].Width = 100;
            datalistadomaterialseleccionarseleccionar.Columns[1].Width = 90;
            datalistadomaterialseleccionarseleccionar.Columns[2].Width = 445;
            datalistadomaterialseleccionarseleccionar.Columns[3].Width = 160;
            alternarColorFilas(datalistadomaterialseleccionarseleccionar);
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
            if (cboBusquedaCopiaFormulacion.Text == "CÓDIGO FORMULACIÓN")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("BuscarCopiaCodigoFormulacion", con);
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
                cmd = new SqlCommand("BuscarCopiaCodigoProductoFormulacion", con);
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
                cmd = new SqlCommand("BuscarCopiaDescripcionProductoFormulacion", con);
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

        //REDIMENSIONAR MI BUSQUEDA DE COPIA DE FOMRULACIONES
        public void RedimensionarBusquedaCopiaFormulaciones(DataGridView DGV)
        {
            DGV.Columns[0].Visible = false;
            DGV.Columns[1].Width = 150;
            DGV.Columns[2].Width = 140;
            DGV.Columns[3].Width = 120;
            DGV.Columns[4].Width = 500;
            DGV.Columns[5].Width = 150;
        }

        //SELECCIONAR UNA FORMULACION DE MI COPIA
        private void datalistadoBusquedaCopiaFormulaciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MostrarDetalleFormulacionesProducto(datalistadoItemsActividadesProductoBusquedaCopiaFormulacion, datalistadoBusquedaCopiaFormulaciones.SelectedCells[2].Value.ToString());
            MostrarMaterialFormulacionesProducto(datalistadoItemsMaterialProducidoBusquedaCopiaFormulacion, datalistadoBusquedaCopiaFormulaciones.SelectedCells[2].Value.ToString());

            MostrarDetalleFormulacionesSemiProducido(datalistadoItemsActividadesSemiProducidoBusquedaCopiaFormulacion, datalistadoBusquedaCopiaFormulaciones.SelectedCells[2].Value.ToString());
            MostrarMaterialFormulacionesSemiProducido(datalistadoItemsMaterialSemiProducidoBusquedaCopiaFormulacion, datalistadoBusquedaCopiaFormulaciones.SelectedCells[2].Value.ToString());

            lblTipoFormulacionCopia.Text = datalistadoBusquedaCopiaFormulaciones.SelectedCells[1].Value.ToString();
        }

        //REGRESAR DE BUSQEUDA Y COPIA DE FOMRULACIONES
        private void btnRegresarBusquedaCopiaFormulacion_Click(object sender, EventArgs e)
        {
            panelBusquedaCopiaFormulaciones.Visible = false;
        }

        //FUNCION PARA COPIAR LOS DATOS DE LA FORMULACION A OTRA
        private void btnCopiarBusquedaCopiaFormulacion_Click(object sender, EventArgs e)
        {
            if (lblTipoFormulacionCopia.Text == txtTipoFormulacion.Text)
            {
                if (datalistadoactividadesproducto.RowCount == 0 && datalistadoactividadsemiproducido.RowCount == 0)
                {
                    //INGRESAR ACTIVIDADES DEL PRODUCTO
                    foreach (DataGridViewRow row in datalistadoItemsActividadesProductoBusquedaCopiaFormulacion.Rows)
                    {
                        try
                        {

                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("InsertarFormulacionActividadProducto", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@codigoformulacion", lblCodigoFormulacionVision.Text);
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
                            MostrarDetalleFormulacionesProducto(datalistadoactividadesproducto, lblCodigoFormulacionVision.Text);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error inesperado, " + ex.Message);
                        }
                    }

                    //INGRESAR ACTIVIDADES DEL SEMIPRODUCIDO
                    foreach (DataGridViewRow row in datalistadoItemsActividadesSemiProducidoBusquedaCopiaFormulacion.Rows)
                    {
                        try
                        {
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("InsertarFormulacionActividadSemiProducido", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@codigoformulacion", lblCodigoFormulacionVision.Text);
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
                            MostrarDetalleFormulacionesSemiProducido(datalistadoactividadsemiproducido, lblCodigoFormulacionVision.Text);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error inesperado, " + ex.Message);
                        }
                    }

                    //INGRESAR MATERIALES DE MI PRODUCTO
                    foreach (DataGridViewRow row in datalistadoItemsMaterialProducidoBusquedaCopiaFormulacion.Rows)
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("InsertarFormulacionMaterialProducto", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@codigoformulacion", lblCodigoFormulacionVision.Text);
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

                    //INGRESAR MATERIALES DE MI SEMIPRODUCIO
                    foreach (DataGridViewRow row in datalistadoItemsMaterialSemiProducidoBusquedaCopiaFormulacion.Rows)
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("InsertarFormulacionMaterialProducto", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@codigoformulacion", lblCodigoFormulacionVision.Text);
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
                MessageBox.Show("No se puede copiar la formulación porque son formulaciones diferentes, SIN SEMIPRODUCIDO <> CON SEMIPRODUCIDO.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }
    }
}

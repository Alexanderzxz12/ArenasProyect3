using ArenasProyect3.Modulos.Mantenimientos;
using ArenasProyect3.Modulos.Resourses;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text.pdf.codec;
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

namespace ArenasProyect3.Modulos.Calidad.Revision
{
    public partial class ListadoOrdenProduccion : Form
    {
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        private Cursor curAnterior = null;
        //int totalCantidades = 0;
        bool estadoSNG = false;
        bool estadoSNGCulminada = false;
        bool estadoDesaprobado = false;
        DataGridView dgvActivo = null;

        public ListadoOrdenProduccion()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI FORMULARIO
        private void ListadoOrdenProduccion_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            cboBusqeuda.SelectedIndex = 0;
            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            VerificarDGVActivo();
        }

        //FUNCION PARA VERIFICAR SI HAY UNA CANTIDAD 
        public void MostrarCantidadesSegunOP(int idOrdenProduccion)
        {
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Calidad_MostrarCantidades", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idOrdenProduccion", idOrdenProduccion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoHistorial.DataSource = dt;
                con.Close();
                //REORDENAMIENTO DE COLUMNAS
                datalistadoHistorial.Columns[2].Width = 120;
                datalistadoHistorial.Columns[3].Width = 70;
                datalistadoHistorial.Columns[4].Width = 70;
                datalistadoHistorial.Columns[5].Width = 120;
                //COLUMNAS NO VISIBLES
                datalistadoHistorial.Columns[1].Visible = false;
                datalistadoHistorial.Columns[6].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR TIPOS DE HALLAZGO
        public void CargarTipoHallazgo()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdTipoHallazgo, Nombre FROM TipoHallazgo WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                System.Data.DataTable dt = new System.Data.DataTable();
                data.Fill(dt);
                cboTipoHallazgo.ValueMember = "IdTipoHallazgo";
                cboTipoHallazgo.DisplayMember = "Nombre";
                cboTipoHallazgo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema." + ex.Message, "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO DE CANTIDADES
        public void ColoresListadoCantidades(DataGridView dgv)
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= dgv.RowCount - 1; i++)
                {
                    string estadoRevision = dgv.Rows[i].Cells[5].Value.ToString();

                    if (estadoRevision == "APROBADO" || estadoRevision == "SNC CULMINADA")
                    {
                        dgv.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                    }
                    else if (estadoRevision == "DESAPROBADO")
                    {
                        dgv.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    else if (estadoRevision == "SNC GENERADA")
                    {
                        dgv.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.DarkOrange;
                    }
                    else
                    {
                        dgv.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO OPs
        public void ColoresListadoOPCalidad(DataGridView DGV)
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= DGV.RowCount - 1; i++)
                {
                    string estadoCalidad = DGV.Rows[i].Cells[17].Value.ToString();

                    if (estadoCalidad == "REVISIÓN PARCIAL")
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (estadoCalidad == "CULMINADA" || estadoCalidad == "CULMINADA - SNC")
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.DarkGreen;
                    }
                    else if (estadoCalidad == "ANULADO" || estadoCalidad == "NO DEFINIDO")
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //LISTADO DE OP Y SELECCION DE PDF Y ESTADO DE OP---------------------
        //MOSTRAR OP AL INCIO 
        //FUNCION PARA VISUALIZAR MIS RESULTADOS
        public void MostrarOrdenProduccion(DateTime fechaInicio, DateTime fechaTermino, string cliente = null, string codigoOP = null, string descripcion = null)
        {
            using (SqlConnection con = new SqlConnection(Conexion.ConexionMaestra.conexion))
            using (SqlCommand cmd = new SqlCommand("OP_Mostrar", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                cmd.Parameters.AddWithValue("@cliente", (object)cliente ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@codigoOP", (object)codigoOP ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@descripcion", (object)descripcion ?? DBNull.Value);
                try
                {
                    con.Open();
                    System.Data.DataTable dt = new System.Data.DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    datalistadoTodasOP.DataSource = dt;
                    DataRow[] rowsCulminado = dt.Select("ESTADO = 'CULMINADO'");
                    // Si hay filas, crea un nuevo DataTable, si no, usa una copia vacía del esquema.
                    System.Data.DataTable dtCulminado = rowsCulminado.Any() ? rowsCulminado.CopyToDataTable() : dt.Clone();
                    datalistadoEnProcesoOP.DataSource = dtCulminado; // Asumiendo este es el nombre de tu DataGrid

                    RedimensionarListadoOrdenProduccion(datalistadoTodasOP);
                    RedimensionarListadoOrdenProduccion(datalistadoEnProcesoOP);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //FUNCION PARA REDIMENSIONAR MIS LISTADOS
        public void RedimensionarListadoOrdenProduccion(DataGridView DGV)
        {
            ////REDIEMNSION DE PEDIDOS
            DGV.Columns[2].Width = 95;
            DGV.Columns[3].Width = 80;
            DGV.Columns[4].Width = 80;
            DGV.Columns[5].Width = 250;
            DGV.Columns[6].Width = 130;
            DGV.Columns[7].Width = 40;
            DGV.Columns[8].Width = 400;
            DGV.Columns[9].Width = 60;
            DGV.Columns[10].Width = 89;
            DGV.Columns[11].Width = 75;
            DGV.Columns[14].Width = 75;
            DGV.Columns[15].Width = 75;
            DGV.Columns[16].Width = 110;
            DGV.Columns[17].Width = 110;
            ////SE HACE NO VISIBLE LAS COLUMNAS QUE NO LES INTERESA AL USUARIO
            DGV.Columns[1].Visible = false;
            DGV.Columns[12].Visible = false;
            DGV.Columns[13].Visible = false;
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

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN
        private void datalistadoTodasOP_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            ModificarCursor(datalistadoTodasOP, "detalles", e);
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN - HISTORIAL
        private void datalistadoHistorial_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            ModificarCursor(datalistadoHistorial, "columDesc", e);
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN
        private void datalistadoEnProcesoOP_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            ModificarCursor(datalistadoEnProcesoOP, "detallesCulminadas", e);
        }

        //BOTONES DE ACCIONES DE BUSQUEDA
        //MOSTRAR OP SEGUN LAS FECHAS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarOrdenProduccion(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OP SEGUN LAS FECHAS
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarOrdenProduccion(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OP SEGUN LAS FECHAS
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarOrdenProduccion(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OPRDENES PRODUCCION DEPENDIENTO LA OPCIÓN ESCOGIDA
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            string cliente = null;
            string codigoOP = null;
            string descripcion = null;
            string textoBusqueda = txtBusqueda.Text;

            if (cboBusqeuda.Text == "CÓDIGO OT")
            {
                codigoOP = textoBusqueda;
            }
            else if (cboBusqeuda.Text == "CLIENTE")
            {
                cliente = textoBusqueda;
            }
            else if (cboBusqeuda.Text == "DESCRIPCIÓN PRODUCTO")
            {
                descripcion = textoBusqueda;
            }

            MostrarOrdenProduccion(
                DesdeFecha.Value,
                HastaFecha.Value,
                cliente,
                codigoOP,
                descripcion
            );
        }

        //LIMPIEZA DE BUSQUEDA
        private void cboBusqeuda_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";
        }

        //CARGAR MI PLANO DE PRODUCTO ASIGANDO A LA OP
        private void btnPlano_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(datalistadoTodasOP.SelectedCells[20].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Documento no encontrado, hubo un error al momento de cargar el archivo.", ex.Message);
            }
        }

        //ENTRARA A MIS DETALLES DE MI REVISION DE OP
        private void datalistadoTodasOP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoTodasOP.RowCount != 0)
            {
                DataGridViewColumn currentColumnT = datalistadoTodasOP.Columns[e.ColumnIndex];

                if (currentColumnT.Name == "detalles")
                {
                    MostrarCantidadesEntregadasOP();
                }
            }
        }

        //ENTRARA A MIS DETALLES DE MI REVISION DE OP
        private void datalistadoEnProcesoOP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoEnProcesoOP.RowCount != 0)
            {
                DataGridViewColumn currentColumnT = datalistadoEnProcesoOP.Columns[e.ColumnIndex];

                if (currentColumnT.Name == "detallesCulminadas")
                {
                    MostrarCantidadesEntregadasOP();
                }
            }
        }

        //ENTRARA A MIS DETALLES DE MI REVISION DE OP
        private void datalistadoTodasOP_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoTodasOP.RowCount != 0)
            {
                MostrarCantidadesEntregadasOP();
            }
        }

        //ENTRARA A MIS DETALLES DE MI REVISION DE OP
        private void datalistadoEnProcesoOP_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoEnProcesoOP.RowCount != 0)
            {
                MostrarCantidadesEntregadasOP();
            }
        }

        //fFUNCION PARA EJECUTAR EL MOSTRAR CANTIDADES
        public void MostrarCantidadesEntregadasOP()
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (dgvActivo.CurrentRow != null)
            {
                panelControlCalidad.Visible = true;
                btnVisualizar.Visible = false;
                lblLeyendaVisualizar.Visible = false;
                btnGenerarCSM.Visible = false;
                lblGenerarCSM.Visible = false;

                lblIdOP.Text = dgvActivo.SelectedCells[1].Value.ToString();
                txtCodigoOP.Text = dgvActivo.SelectedCells[2].Value.ToString();
                txtDescripcionProducto.Text = dgvActivo.SelectedCells[8].Value.ToString();
                txtCodigoFormulacion.Text = dgvActivo.SelectedCells[30].Value.ToString();
                txtCantidadTotalOP.Text = dgvActivo.SelectedCells[9].Value.ToString();
                txtCantidadEntregada.Text = dgvActivo.SelectedCells[14].Value.ToString();
                MostrarCantidadesSegunOP(Convert.ToInt32(lblIdOP.Text));
                lblCantidadRealizada.Text = dgvActivo.SelectedCells[15].Value.ToString();
                txtCantidadRestante.Text = Convert.ToString(Convert.ToInt32(txtCantidadEntregada.Text) - Convert.ToInt32(lblCantidadRealizada.Text));
                txtPesoTeorico.Text = "0.00";
                txtPesoReal.Text = "0.00";
                txtObservaciones.Text = "";
                btnGenerarCSM.Visible = false;
                lblGenerarCSM.Visible = false;
                CargarTipoHallazgo();
            }
        }

        //GENERACION DE REPORTES
        private void btnGenerarOrdenProduccionPDF_Click(object sender, EventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (dgvActivo.CurrentRow != null)
            {
                string codigoOrdenProduccion = dgvActivo.Rows[dgvActivo.CurrentRow.Index].Cells[1].Value.ToString();
                Visualizadores.VisualizarOrdenProduccion frm = new Visualizadores.VisualizarOrdenProduccion();
                frm.lblCodigo.Text = codigoOrdenProduccion;
                frm.Show();
            }
            else
            {
                MessageBox.Show("Debe seleccionar una OP para poder generar el PDF.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //CARGAR MI OC TRAIDO DESDE MI PEDIDO
        private void btnOC_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(datalistadoTodasOP.SelectedCells[19].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Documento no encontrado, hubo un error al momento de cargar el archivo.", ex.Message);
            }
        }


        //APROBAR LAS CANTIDADES INGRESADAS
        private void btnAprobar_Click(object sender, EventArgs e)
        {
            GuardarRevisionCalidad("aprobar", Convert.ToInt32(lblIdOP.Text), Convert.ToInt32(txtCantidadInspeccionar.Text), Convert.ToDecimal(txtPesoTeorico.Text), Convert.ToDecimal(txtPesoReal.Text), txtObservaciones.Text, 2, Convert.ToInt32(cboTipoHallazgo.SelectedIndex.ToString()));
        }

        //DESAPROBAR LAS CANTIDADES INGRESADAS
        private void btnDesaprobar_Click(object sender, EventArgs e)
        {
            GuardarRevisionCalidad("desaprobar", Convert.ToInt32(lblIdOP.Text), Convert.ToInt32(txtCantidadInspeccionar.Text), Convert.ToDecimal(txtPesoTeorico.Text), Convert.ToDecimal(txtPesoReal.Text), txtObservaciones.Text, 0, Convert.ToInt32(cboTipoHallazgo.SelectedIndex.ToString()));
        }

        //FUNCION PARA GUARDAR UNA REVISION
        public void GuardarRevisionCalidad(string tipo, int idOP, int cantidad, decimal pesoTeorico, decimal pesoReal, string observaciones, int estadoAD, int tipoHallazgp)
        {
            VerificarDGVActivo();
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (dgvActivo.CurrentRow != null)
            {
                if (txtCantidadInspeccionar.Text == "" || txtCantidadInspeccionar.Text == "0" || txtObservaciones.Text == "" && cboTipoHallazgo.Text != "TODO CONFORME")
                {
                    MessageBox.Show("Debe ingresar una cantidad u obserbación válida para poder aprobar o desaprobar.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else if (Convert.ToInt32(txtCantidadInspeccionar.Text) > Convert.ToInt32(txtCantidadRestante.Text))
                {
                    MessageBox.Show("No se puede revisar más de la cantidad restante.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else if (cboTipoHallazgo.Text != "TODO CONFORME" && txtObservaciones.Text == "" && tipo == "desaprobar")
                {
                    MessageBox.Show("No se puede revisar hasta que se llenen todos los campos.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    DialogResult boton = MessageBox.Show("¿Realmente desea " + tipo + " esta cantidad?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                    if (boton == DialogResult.OK)
                    {
                        try
                        {
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("Calidad_IngresarRegistroCantidad", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idOrdenProduccion", idOP);
                            cmd.Parameters.AddWithValue("@cantidad", cantidad);
                            cmd.Parameters.AddWithValue("@fechaRegistro", DateTime.Now);
                            cmd.Parameters.AddWithValue("@pesoTeorico", pesoTeorico);
                            cmd.Parameters.AddWithValue("@pesoReal", pesoTeorico);
                            cmd.Parameters.AddWithValue("@observaciones", observaciones);
                            cmd.Parameters.AddWithValue("@estadoAD", estadoAD);
                            cmd.Parameters.AddWithValue("@tipoHallazgo", tipoHallazgp);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Cantidad revisada correctamente.", "Validación del Sistema", MessageBoxButtons.OK);
                            txtCantidadRestante.Text = Convert.ToString(Convert.ToInt16(txtCantidadRestante.Text) - Convert.ToInt16(txtCantidadInspeccionar.Text));
                            LimpiarCantidades();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        //LIMPIAR MIS DATOS DE MI INGRESO DE CANTIDADES
        public void LimpiarCantidades()
        {
            txtCantidadInspeccionar.Text = "";
            txtPesoReal.Text = "0.00";
            txtObservaciones.Text = "";
            MostrarCantidadesSegunOP(Convert.ToInt16(lblIdOP.Text));
        }

        //VERIFICAR SI MI DATAGRIDVIEW ESTA SNC CULMINADA
        public void VerificarSNGCulminada_Desaprobado_SNC()
        {
            estadoSNG = false;
            foreach (DataGridViewRow fila in datalistadoHistorial.Rows)
            {
                // Evita procesar la fila nueva que aparece al final
                if (!fila.IsNewRow)
                {
                    var valorCelda = fila.Cells[5].Value?.ToString(); // Columna 4 = índice 3

                    if (valorCelda == "SNC GENERADA")
                    {
                        estadoSNG = true;
                    }
                }
            }
            estadoSNGCulminada = false;
            foreach (DataGridViewRow fila in datalistadoHistorial.Rows)
            {
                // Evita procesar la fila nueva que aparece al final
                if (!fila.IsNewRow)
                {
                    var valorCelda = fila.Cells[5].Value?.ToString(); // Columna 4 = índice 3

                    if (valorCelda == "SNC CULMINADA")
                    {
                        estadoSNGCulminada = true;
                    }
                }
            }
            estadoDesaprobado = false;
            foreach (DataGridViewRow fila in datalistadoHistorial.Rows)
            {
                // Evita procesar la fila nueva que aparece al final
                if (!fila.IsNewRow)
                {
                    var valorCelda = fila.Cells[5].Value?.ToString(); // Columna 4 = índice 3

                    if (valorCelda == "DESAPROBADO")
                    {
                        estadoDesaprobado = true;
                    }
                }
            }
        }

        //CAMBIO DE CANTUIDADES DEPENDIENDO LA INGRESADA
        private void txtCantidadInspeccionar_TextChanged(object sender, EventArgs e)
        {
            if (txtCantidadInspeccionar.Text == "") { txtCantidadInspeccionar.Text = "0"; }
        }

        //CERRAR MI PANEL DE CONTROL DE CALIDAD
        private void btnRegresarControl_Click(object sender, EventArgs e)
        {
            VerificarDGVActivo();
            PanelRevisioncantidades();
        }

        //CERRAR DETALLES CANTIDADES
        private void btnCerrarDetallesOPCantidades_Click(object sender, EventArgs e)
        {
            PanelRevisioncantidades();
        }

        //SALIR DE LA REVISION DE CANTIDADES
        public void PanelRevisioncantidades()
        {
            if (estadoDesaprobado == false)
            {
                panelControlCalidad.Visible = false;
                ValidarEstadoOP();
                LimpiarCantidades();
                MostrarOrdenProduccion(DesdeFecha.Value, HastaFecha.Value);
            }
            else
            {
                MessageBox.Show("No se puede salir del control de calidad si hay una cantidad desaprobada, debe generar la SNG correspondiente.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //FUNCION PARA VALIDAR MIS ANTIDADES Y LOS ESTADOS DE ESTOS
        public void ValidarEstadoOP()
        {
            VerificarSNGCulminada_Desaprobado_SNC();

            if (datalistadoHistorial.RowCount == 0)
            {
                CambiarEstadoCalidad(Convert.ToInt32(lblIdOP.Text), 1);
            }
            else
            {
                //SI COMPLETE TODOS LAS CANTIDADES PERO DENTRO NO HAY NINGUNA DESAPROBADA PERO HAY UN SNG GENERADA O UN SNC CULMINADA
                if (txtCantidadInspeccionar.Text == txtCantidadRestante.Text && estadoSNG == true && txtCantidadEntregada.Text == txtCantidadTotalOP.Text || txtCantidadInspeccionar.Text == txtCantidadRestante.Text && estadoSNGCulminada == true && txtCantidadEntregada.Text == txtCantidadTotalOP.Text || Convert.ToInt16(txtCantidadInspeccionar.Text) >= Convert.ToInt16(txtCantidadRestante.Text) && estadoSNGCulminada == true && txtCantidadEntregada.Text == txtCantidadTotalOP.Text)
                {
                    CambiarEstadoCalidad(Convert.ToInt32(lblIdOP.Text), 4);
                }
                //SI COMPLETE TODOS LAS CANTIDADES PERO DENTRO NO HAY NINGUNA DESAPROBADA Y NINGUN SNG GENERADA
                else if (txtCantidadInspeccionar.Text == txtCantidadRestante.Text && estadoSNG == false && txtCantidadEntregada.Text == txtCantidadTotalOP.Text)
                {
                    CambiarEstadoCalidad(Convert.ToInt32(lblIdOP.Text), 3);
                }

                //SI SE INGRESA PARCIALMENTE LAS CANTIDADES PERO NO HAY UN DESAPROBADO Y NO HAY SNG
                else if (txtCantidadRestante.Text != txtCantidadEntregada.Text && estadoSNG == false && estadoDesaprobado == false || txtCantidadRestante.Text == txtCantidadEntregada.Text && txtCantidadInspeccionar.Text != txtCantidadEntregada.Text && estadoSNG == false && estadoDesaprobado == false)
                {
                    CambiarEstadoCalidad(Convert.ToInt32(lblIdOP.Text), 2);
                }
                //SI SE INGRESA PARCIALMENTE LAS CANTIDADES CON UNA SNG GENERADA
                else if (txtCantidadRestante.Text != txtCantidadEntregada.Text && estadoSNG == true || txtCantidadRestante.Text == txtCantidadEntregada.Text && txtCantidadInspeccionar.Text != txtCantidadEntregada.Text && estadoSNG == true)
                {
                    CambiarEstadoCalidad(Convert.ToInt32(lblIdOP.Text), 2);
                }
                else
                {
                    CambiarEstadoCalidad(Convert.ToInt32(lblIdOP.Text), 1);
                }
            }
        }

        //CAMBIAR EL ESTADO DE MI OP A FINALIZADA
        public void CambiarEstadoCalidad(int idOP, int estadoCalidad)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                SqlCommand cmd = new SqlCommand();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                cmd = new SqlCommand("Calidad_EstadoCalidad", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idOP", idOP);
                cmd.Parameters.AddWithValue("@estadoCalidad", estadoCalidad);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //SELECCIONAR MI REGISTRO SI ESTA DESAPROBADO
        private void datalistadoHistorial_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoHistorial.CurrentRow != null)
            {
                MostrarSNC(datalistadoHistorial);

                //ABRIR PANEL DE OBSERVACIONES
                if (datalistadoHistorial.RowCount != 0)
                {
                    DataGridViewColumn currentColumnT = datalistadoHistorial.Columns[e.ColumnIndex];

                    if (currentColumnT.Name == "columDesc")
                    {
                        panelDetallesObservacion.Visible = true;
                        txtDetallesObservacion.Text = datalistadoHistorial.SelectedCells[6].Value.ToString();
                    }
                }
            }
            else
            {
                MessageBox.Show("Deben haber registros cargados.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //CARGAR SI SE MUESTRA O NO LOS BOTONES
        public void MostrarSNC(DataGridView DGV)
        {
            //EVALUAR SI ESTA DESAPROBADO O TIENE SNC
            if (DGV.SelectedCells[5].Value.ToString() == "DESAPROBADO")
            {
                btnGenerarCSM.Visible = true;
                lblGenerarCSM.Visible = true;
            }
            else
            {
                btnGenerarCSM.Visible = false;
                lblGenerarCSM.Visible = false;
            }

            if (DGV.SelectedCells[5].Value.ToString() == "SNC CULMINADA")
            {
                btnVisualizar.Visible = true;
                lblLeyendaVisualizar.Visible = true;
            }
            else
            {
                btnVisualizar.Visible = false;
                lblLeyendaVisualizar.Visible = false;
            }
        }

        //VISUALIZAR DOCUMETNOS DEL CONTROL DE CALIDAD
        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoHistorial.CurrentRow != null)
            {
                //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO DESAPROBADO
                string codigoDetalleCantidadCalidad = datalistadoHistorial.Rows[datalistadoHistorial.CurrentRow.Index].Cells[1].Value.ToString();
                Visualizadores.VisualizarSNC frm = new Visualizadores.VisualizarSNC();
                frm.lblCodigo.Text = codigoDetalleCantidadCalidad;
                //CARGAR VENTANA
                frm.Show();
            }
        }

        //CERRAR MI PANEL DE OBSERVACIONES
        private void btnCerarDetallesObservacion_Click(object sender, EventArgs e)
        {
            panelDetallesObservacion.Visible = false;
        }

        //GENERA UNA SALIDA NO CONFORME
        private void btnGenerarCSM_Click(object sender, EventArgs e)
        {
            panelSNC.Visible = true;
            txtReponsableRegistro.Text = Program.UnoNombreUnoApellidoUsuario;
            txtOrdenProduccionSNC.Text = txtCodigoOP.Text;
            LimpiarSNC();
        }

        //BOTON PARA GUARDAR MI SNC
        private void btnGuardarSNC_Click(object sender, EventArgs e)
        {
            //SI LOS CAMPOS ESTAN VACIOS
            if (txtReponsableRegistro.Text == "" || txtOrdenProduccionSNC.Text == "0" || txtDescripcionSNC.Text == "")
            {
                MessageBox.Show("Debe ingresar todos los campos para poder registrar la SNC.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                DialogResult boton = MessageBox.Show("¿Realmente desea generar esta SNC?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        SqlCommand cmd = new SqlCommand();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        cmd = new SqlCommand("Calidad_IngresarSNC", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idDetalleCantidadCalidad", Convert.ToInt32(datalistadoHistorial.SelectedCells[1].Value.ToString()));
                        cmd.Parameters.AddWithValue("@idUsuarioResponsable", Program.IdUsuario);
                        cmd.Parameters.AddWithValue("@fechaHallazgo", dtpFechaHallazgo.Value);
                        cmd.Parameters.AddWithValue("@IdOp", Convert.ToInt32(lblIdOP.Text));
                        cmd.Parameters.AddWithValue("@descripcionSNC", txtDescripcionSNC.Text);

                        //PRIMERA IMAGEN
                        if (txtImagen1.Text != "")
                        {
                            string nombreGenerado1 = "IMAGEN 1 OP " + txtOrdenProduccionSNC.Text + " - " + DateTime.Now.ToString("ddMMyyyyHHmmss");
                            string rutaOld1 = txtImagen1.Text;
                            string RutaNew1 = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Calidad\ImagenesSNC\" + nombreGenerado1 + ".jpg";
                            File.Copy(rutaOld1, RutaNew1);
                            cmd.Parameters.AddWithValue("@imagen1", RutaNew1);
                        }
                        else{cmd.Parameters.AddWithValue("@imagen1", "");}

                        //SEGUNDA IMAGEN
                        if (txtImagen2.Text != "")
                        {
                            string nombreGenerado2 = "IMAGEN 2 OP " + txtOrdenProduccionSNC.Text + " - " + DateTime.Now.ToString("ddMMyyyyHHmmss");
                            string rutaOld2 = txtImagen2.Text;
                            string RutaNew2 = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Calidad\ImagenesSNC\" + nombreGenerado2 + ".jpg";
                            File.Copy(rutaOld2, RutaNew2);
                            cmd.Parameters.AddWithValue("@imagen2", RutaNew2);
                        }
                        else{cmd.Parameters.AddWithValue("@imagen2", "");}

                        //TERCERA IMAGEN
                        if (txtImagen3.Text != "")
                        {
                            string nombreGenerado3 = "IMAGEN 3 OP " + txtOrdenProduccionSNC.Text + " - " + DateTime.Now.ToString("ddMMyyyyHHmmss");
                            string rutaOld3 = txtImagen3.Text;
                            string RutaNew3 = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Calidad\ImagenesSNC\" + nombreGenerado3 + ".jpg";
                            File.Copy(rutaOld3, RutaNew3);
                            cmd.Parameters.AddWithValue("@imagen3", RutaNew3);
                        }
                        else{cmd.Parameters.AddWithValue("@imagen3", "");}

                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Salida No Conforme registrada correctamente.", "Validación del Sistema", MessageBoxButtons.OK);
                        MostrarCantidadesSegunOP(Convert.ToInt16(lblIdOP.Text));
                        panelSNC.Visible = false;
                        LimpiarSNC();
                        MostrarSNC(datalistadoHistorial);
                        //FUNCION PARAENVIAR EL CORREO RESPECTIVO
                        //ClassResourses.Enviar("arenassoft@arenassrl.com.pe", "CORREO AUTOMATIZADO - CREACIÓN DE UNA SNC", "Correo de creación de una salida no conforme a la OP número " + txtOrdenProduccionSNC.Text + " por parte del usuario " + Program.UnoNombreUnoApellidoUsuario + " el la fecha siguiente: " + DateTime.Now + ". Por favor no responder.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //FUNCION PARA LIMPIAR MI SNC
        public void LimpiarSNC()
        {
            txtDescripcionSNC.Text = "";
        }

        //BOTON PARA SALIR DE MI SNC
        private void btnCerrarSNC_Click(object sender, EventArgs e)
        {
            panelSNC.Visible = false;
            LimpiarSNC();
        }

        //SECCION DE CARGA PARA MIS IMAGENES DE LA SNC
        private void btnCargar1_Click(object sender, EventArgs e)
        {
            ConfiguracionCarga(openFileDialog1, txtImagen1);
        }

        //LAMADO DE CARGA
        private void btnCargar2_Click(object sender, EventArgs e)
        {
            ConfiguracionCarga(openFileDialog2, txtImagen2);
        }

        //LAMADO DE CARGA
        private void btnCargar3_Click(object sender, EventArgs e)
        {
            ConfiguracionCarga(openFileDialog3, txtImagen3);
        }

        //FUNCION PARA OPERAR CON MI DLG
        public void ConfiguracionCarga(OpenFileDialog dlg, TextBox txtCarga)
        {
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Todos los archivos (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtCarga.Text = dlg.FileName;
            }
        }

        //CAJAS DE TEXTO PARA LIMPIAR MI IMAGEN
        private void btnLimpiar1_Click(object sender, EventArgs e)
        {
            txtImagen1.Text = "";
        }

        //CAJAS DE TEXTO PARA LIMPIAR MI IMAGEN
        private void btnLimpiar2_Click(object sender, EventArgs e)
        {
            txtImagen2.Text = "";
        }

        //CAJAS DE TEXTO PARA LIMPIAR MI IMAGEN
        private void btnLimpiar3_Click(object sender, EventArgs e)
        {
            txtImagen3.Text = "";
        }

        //EVENTO PARA COLOREAR MIS LSITADOS
        private void datalistadoEnProcesoOP_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            ColoresListadoOPCalidad(datalistadoEnProcesoOP);
        }

        //EVENTO PARA COLOREAR MIS LSITADOS
        private void datalistadoTodasOP_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            ColoresListadoOPCalidad(datalistadoTodasOP);
        }

        //VALIDAR QUE SOLO INGRESE NÚMEROS ENTEROS
        private void datalistadoHistorial_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            ColoresListadoCantidades(datalistadoHistorial);
        }

        //VALIDAR QUE SOLO INGRESE NÚMEROS ENTEROS
        private void txtCantidadInspeccionar_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo dígitos y teclas de control como Backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Bloquea el carácter
            }
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN - HISTORIAL
        public void ModificarCursor(DataGridView dgv, string nomColum, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (dgv.Columns[e.ColumnIndex].Name == nomColum)
            {
                dgv.Cursor = Cursors.Hand;
            }
            else
            {
                dgv.Cursor = curAnterior;
            }
        }

        //VERIFICAR EN QUE LSITADO ESTOY
        public void VerificarDGVActivo()
        {
            if (TabControl.SelectedTab.Text == "OP Culminadas")
            {
                dgvActivo = datalistadoEnProcesoOP;
            }
            else if (TabControl.SelectedTab.Text == "Todas las OP")
            {
                dgvActivo = datalistadoTodasOP;
            }
        }

        //CARGAR DATOS CUANDO SE ACMBIA
        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarDGVActivo();
        }
        //-----------------------------------------------------------------------------------------------------------

        //VIZUALIZAR DATOS EXCEL--------------------------------------------------------------------
    }
}
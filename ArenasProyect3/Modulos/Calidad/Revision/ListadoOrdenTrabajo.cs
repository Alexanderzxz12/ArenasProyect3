using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Calidad.Revision
{
    public partial class ListadoOrdenTrabajo : Form
    {
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        private Cursor curAnterior = null;
        //int totalCantidades = 0;
        bool estadoSNG = false;
        bool estadoSNGCulminada = false;
        bool estadoDesaprobado = false;
        DataGridView dgvActivo = null;

        public ListadoOrdenTrabajo()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI FORMULARIO
        private void ListadoOrdenTrabajo_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            cboBusqeuda.SelectedIndex = 0;
            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            datalistadoTodasOT.DataSource = null;
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO Y VER SI ESTAN VENCIDOS


        //FUNCION PARA VERIFICAR SI HAY UNA CANTIDAD 
        public void MostrarCantidadesSegunOT(int idOrdenTrabajo)
        {
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Calidad_MostrarCantidadesOT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idOrdenTrabajo", idOrdenTrabajo);
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
                ColoresListadoCantidades();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO DE CANTIDADES
        public void ColoresListadoCantidades()
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= datalistadoHistorial.RowCount - 1; i++)
                {
                    string estadoRevision = datalistadoHistorial.Rows[i].Cells[5].Value.ToString();

                    if (estadoRevision == "APROBADO" || estadoRevision == "SNC CULMINADA")
                    {
                        datalistadoHistorial.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                    }
                    else if (estadoRevision == "DESAPROBADO")
                    {
                        datalistadoHistorial.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    else if (estadoRevision == "SNC GENERADA")
                    {
                        datalistadoHistorial.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.DarkOrange;
                    }
                    else
                    {
                        datalistadoHistorial.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //EWJECUTAR EL PINTADO DE CLOLORES
        private void datalistadoHistorial_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            ColoresListadoCantidades();
        }

        //LISTADO DE OP Y SELECCION DE PDF Y ESTADO DE OT---------------------
        //MOSTRAR OP AL INCIO 
        public void MostrarOrdenTrabajoPorCriterios(
            DateTime fechaInicio,
            DateTime fechaTermino,
            string cliente = null,
            string codigoOT = null,
            string descripcion = null)
        {
            // Usamos 'using' para asegurar que la conexión se cierre y se libere la memoria correctamente
            using (SqlConnection con = new SqlConnection(Conexion.ConexionMaestra.conexion))
            using (SqlCommand cmd = new SqlCommand("OT_MostrarOrdenServicio", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                cmd.Parameters.AddWithValue("@cliente", (object)cliente ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@codigoOT", (object)codigoOT ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@descripcion", (object)descripcion ?? DBNull.Value);

                try
                {
                    con.Open();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    datalistadoTodasOT.DataSource = dt;
                    DataRow[] rowsCulminado = dt.Select("ESTADO = 'CULMINADO'");
                    // Si hay filas, crea un nuevo DataTable, si no, usa una copia vacía del esquema.
                    DataTable dtCulminado = rowsCulminado.Any() ? rowsCulminado.CopyToDataTable() : dt.Clone();
                    datalistadoEnProcesoOT.DataSource = dtCulminado; // Asumiendo este es el nombre de tu DataGrid

                    RedimensionarListadoOrdenProduccion(datalistadoTodasOT);
                    RedimensionarListadoOrdenProduccion(datalistadoEnProcesoOT);
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
            DGV.Columns[6].Width = 400;
            DGV.Columns[7].Width = 60;
            DGV.Columns[8].Width = 90;
            DGV.Columns[9].Width = 85;
            DGV.Columns[12].Width = 75;
            DGV.Columns[13].Width = 75;
            DGV.Columns[14].Width = 110;
            DGV.Columns[16].Width = 110;
            ////SE HACE NO VISIBLE LAS COLUMNAS QUE NO LES INTERESA AL USUARIO
            DGV.Columns[1].Visible = false;
            DGV.Columns[10].Visible = false;
            DGV.Columns[11].Visible = false;
            DGV.Columns[15].Visible = false;
            DGV.Columns[17].Visible = false;
            DGV.Columns[18].Visible = false;
            DGV.Columns[19].Visible = false;
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO OPs
        public void ColoresListadoOPCalidad(DataGridView DGV)
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= DGV.RowCount - 1; i++)
                {
                    string estadoCalidad = DGV.Rows[i].Cells[16].Value.ToString();

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

        //BOTONES DE ACCIONES DE BUSQUEDA
        //MOSTRAR OP SEGUN LAS FECHAS
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarOrdenTrabajoPorCriterios(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OP SEGUN LAS FECHAS
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarOrdenTrabajoPorCriterios(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OP SEGUN LAS FECHAS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarOrdenTrabajoPorCriterios(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OP SEGUN CRITERIO DE BUSQUEDA
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            string cliente = null;
            string codigoOT = null;
            string descripcion = null;
            string textoBusqueda = txtBusqueda.Text;

            if (cboBusqeuda.Text == "CÓDIGO OT")
            {
                codigoOT = textoBusqueda;
            }
            else if (cboBusqeuda.Text == "CLIENTE")
            {
                cliente = textoBusqueda;
            }
            else if (cboBusqeuda.Text == "DESCRIPCIÓN PRODUCTO")
            {
                descripcion = textoBusqueda;
            }

            MostrarOrdenTrabajoPorCriterios(
                DesdeFecha.Value,
                HastaFecha.Value,
                cliente,
                codigoOT,
                descripcion
            );
        }

        //LIMPIEZA DE BUSQUEDA
        private void cboBusqeuda_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";
        }

        //PINTAR MIS LSITADOS
        private void datalistadoEnProcesoOT_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            ColoresListadoOPCalidad(datalistadoEnProcesoOT);
        }

        //PINTAR MIS LSITADOS
        private void datalistadoTodasOT_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            ColoresListadoOPCalidad(datalistadoTodasOT);
        }

        //CAMIBAIR EL CURSOR DE MI LSITADO
        private void datalistadoTodasOT_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoTodasOT.Columns[e.ColumnIndex].Name == "detalles") { this.datalistadoTodasOT.Cursor = Cursors.Hand; }
            else { this.datalistadoTodasOT.Cursor = curAnterior; }
        }

        //CAMIBAIR EL CURSOR DE MI LSITADO
        private void datalistadoEnProcesoOT_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoEnProcesoOT.Columns[e.ColumnIndex].Name == "columnaDetallesCul") { this.datalistadoEnProcesoOT.Cursor = Cursors.Hand; }
            else { this.datalistadoEnProcesoOT.Cursor = curAnterior; }
        }

        //CAMIBAIR EL CURSOR DE MI LSITADO DE REVIIOSNES
        private void datalistadoHistorial_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoHistorial.Columns[e.ColumnIndex].Name == "columDesc") { this.datalistadoHistorial.Cursor = Cursors.Hand; }
            else { this.datalistadoHistorial.Cursor = curAnterior; }
        }

        //ENTRARA A MIS DETALLES DE MI REVISION DE OT
        private void datalistadoEnProcesoOT_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoEnProcesoOT.RowCount != 0)
            {
                DataGridViewColumn currentColumnT = datalistadoEnProcesoOT.Columns[e.ColumnIndex];

                if (currentColumnT.Name == "columnaDetallesCul")
                {
                    VerificarDGVActivo();
                    MostrarCantidadesEntregadasOT();
                }
            }
        }

        //ENTRARA A MIS DETALLES DE MI REVISION DE OT
        private void datalistadoTodasOT_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoTodasOT.RowCount != 0)
            {
                DataGridViewColumn currentColumnT = datalistadoTodasOT.Columns[e.ColumnIndex];

                if (currentColumnT.Name == "detalles")
                {
                    VerificarDGVActivo();
                    MostrarCantidadesEntregadasOT();
                }
            }
        }

        //ENTRARA A MIS DETALLES DE MI REVISION DE OT
        private void datalistadoEnProcesoOT_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoEnProcesoOT.RowCount != 0)
            {
                VerificarDGVActivo();
                MostrarCantidadesEntregadasOT();
            }
        }

        //ENTRARA A MIS DETALLES DE MI REVISION DE OT
        private void datalistadoTodasOT_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoTodasOT.RowCount != 0)
            {
                VerificarDGVActivo();
                MostrarCantidadesEntregadasOT();
            }
        }

        //fFUNCION PARA EJECUTAR EL MOSTRAR CANTIDADES
        public void MostrarCantidadesEntregadasOT()
        {
            panelControlCalidad.Visible = true;
            btnVisualizar.Visible = false;
            lblLeyendaVisualizar.Visible = false;
            btnGenerarCSM.Visible = false;
            lblGenerarCSM.Visible = false;

            lblIdOT.Text = dgvActivo.SelectedCells[1].Value.ToString();
            txtCodigoOT.Text = dgvActivo.SelectedCells[2].Value.ToString();
            txtDescripcionProducto.Text = dgvActivo.SelectedCells[6].Value.ToString();
            txtCantidadTotalOT.Text = dgvActivo.SelectedCells[7].Value.ToString();
            txtCantidadEntregada.Text = dgvActivo.SelectedCells[12].Value.ToString();
            MostrarCantidadesSegunOT(Convert.ToInt32(lblIdOT.Text));
            lblCantidadRealizada.Text = dgvActivo.SelectedCells[13].Value.ToString();
            txtCantidadRestante.Text = Convert.ToString(Convert.ToInt32(txtCantidadEntregada.Text) - Convert.ToInt32(lblCantidadRealizada.Text));
            txtPesoTeorico.Text = "0.00";
            txtPesoReal.Text = "0.00";
            txtObservaciones.Text = "";
            btnGenerarCSM.Visible = false;
            lblGenerarCSM.Visible = false;
            CargarTipoHallazgo();
        }

        //VERIFICAR EN QUE LSITADO ESTOY
        public void VerificarDGVActivo()
        {
            if (TabControl.SelectedTab.Text == "OT Culminadas")
            {
                dgvActivo = datalistadoEnProcesoOT;
            }
            else if (TabControl.SelectedTab.Text == "Todas las OT")
            {
                dgvActivo = datalistadoTodasOT;
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

        //SALIR DE LA REVISION DE CANTIDADES
        public void PanelRevisioncantidades()
        {
            if (estadoDesaprobado == false)
            {
                panelControlCalidad.Visible = false;
                ValidarEstadoOT();
                LimpiarCantidades();
                MostrarOrdenTrabajoPorCriterios(DesdeFecha.Value, HastaFecha.Value);
            }
            else
            {
                MessageBox.Show("No se puede salir del control de calidad si hay una cantidad desaprobada, debe generar la SNG correspondiente.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //FUNCION PARA VALIDAR MIS ANTIDADES Y LOS ESTADOS DE ESTOS
        public void ValidarEstadoOT()
        {
            VerificarSNGCulminada_Desaprobado_SNC();

            if (datalistadoHistorial.RowCount == 0)
            {
                CambiarEstadoCalidad(Convert.ToInt32(lblIdOT.Text), 1);
            }
            else
            {
                //SI COMPLETE TODOS LAS CANTIDADES PERO DENTRO NO HAY NINGUNA DESAPROBADA PERO HAY UN SNG GENERADA O UN SNC CULMINADA
                if (txtCantidadInspeccionar.Text == txtCantidadRestante.Text && estadoSNG == true && txtCantidadEntregada.Text == txtCantidadTotalOT.Text || txtCantidadInspeccionar.Text == txtCantidadRestante.Text && estadoSNGCulminada == true && txtCantidadEntregada.Text == txtCantidadTotalOT.Text || Convert.ToInt16(txtCantidadInspeccionar.Text) >= Convert.ToInt16(txtCantidadRestante.Text) && estadoSNGCulminada == true && txtCantidadEntregada.Text == txtCantidadTotalOT.Text)
                {
                    CambiarEstadoCalidad(Convert.ToInt32(lblIdOT.Text), 4);
                }
                //SI COMPLETE TODOS LAS CANTIDADES PERO DENTRO NO HAY NINGUNA DESAPROBADA Y NINGUN SNG GENERADA
                else if (txtCantidadInspeccionar.Text == txtCantidadRestante.Text && estadoSNG == false && txtCantidadEntregada.Text == txtCantidadTotalOT.Text)
                {
                    CambiarEstadoCalidad(Convert.ToInt32(lblIdOT.Text), 3);
                }

                //SI SE INGRESA PARCIALMENTE LAS CANTIDADES PERO NO HAY UN DESAPROBADO Y NO HAY SNG
                else if (txtCantidadRestante.Text != txtCantidadEntregada.Text && estadoSNG == false && estadoDesaprobado == false || txtCantidadRestante.Text == txtCantidadEntregada.Text && txtCantidadInspeccionar.Text != txtCantidadEntregada.Text && estadoSNG == false && estadoDesaprobado == false)
                {
                    CambiarEstadoCalidad(Convert.ToInt32(lblIdOT.Text), 2);
                }
                //SI SE INGRESA PARCIALMENTE LAS CANTIDADES CON UNA SNG GENERADA
                else if (txtCantidadRestante.Text != txtCantidadEntregada.Text && estadoSNG == true || txtCantidadRestante.Text == txtCantidadEntregada.Text && txtCantidadInspeccionar.Text != txtCantidadEntregada.Text && estadoSNG == true)
                {
                    CambiarEstadoCalidad(Convert.ToInt32(lblIdOT.Text), 2);
                }
                else
                {
                    CambiarEstadoCalidad(Convert.ToInt32(lblIdOT.Text), 1);
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
                cmd = new SqlCommand("Calidad_EstadoCalidadOT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idOT", idOP);
                cmd.Parameters.AddWithValue("@estadoCalidad", estadoCalidad);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        //LIMPIAR MIS DATOS DE MI INGRESO DE CANTIDADES
        public void LimpiarCantidades()
        {
            txtCantidadInspeccionar.Text = "";
            txtPesoReal.Text = "0.00";
            txtObservaciones.Text = "";
            MostrarCantidadesSegunOT(Convert.ToInt16(lblIdOT.Text));
        }

        //APROBAR LAS CANTIDADES INGRESADAS
        private void btnAprobar_Click(object sender, EventArgs e)
        {
            GuardarRevisionCalidad("aprobar", Convert.ToInt32(lblIdOT.Text), Convert.ToInt32(txtCantidadInspeccionar.Text), Convert.ToDecimal(txtPesoTeorico.Text), Convert.ToDecimal(txtPesoReal.Text), txtObservaciones.Text, 2, Convert.ToInt32(cboTipoHallazgo.SelectedIndex.ToString()));
        }

        //DESAPROBAR LAS CANTIDADES INGRESADAS
        private void btnDesaprobar_Click(object sender, EventArgs e)
        {
            GuardarRevisionCalidad("desaprobar", Convert.ToInt32(lblIdOT.Text), Convert.ToInt32(txtCantidadInspeccionar.Text), Convert.ToDecimal(txtPesoTeorico.Text), Convert.ToDecimal(txtPesoReal.Text), txtObservaciones.Text, 0, Convert.ToInt32(cboTipoHallazgo.SelectedIndex.ToString()));
        }

        //FUNCION PARA GUARDAR UNA REVISION
        public void GuardarRevisionCalidad(string tipo, int idOP, int cantidad, decimal pesoTeorico, decimal pesoReal, string observaciones, int estadoAD, int tipoHallazgp)
        {
            VerificarDGVActivo();
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (dgvActivo.CurrentRow != null)
            {
                if (txtCantidadInspeccionar.Text == "" || txtCantidadInspeccionar.Text == "0" || txtObservaciones.Text == "" && tipo == "desaprobar")
                {
                    MessageBox.Show("Debe ingresar una cantidad u obserbación válida para poder aprobar o desaprobar.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else if (Convert.ToInt32(txtCantidadInspeccionar.Text) > Convert.ToInt32(txtCantidadRestante.Text))
                {
                    MessageBox.Show("No se puede revisar más de la cantidad restante.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else if (cboTipoHallazgo.Text == "TODO CONFORME" && txtObservaciones.Text == "" && tipo == "desaprobar")
                {
                    MessageBox.Show("No se puede revisar hasta que se llenen todos los campos de manera correcta.", "Validación del Sistema", MessageBoxButtons.OK);
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
                            cmd = new SqlCommand("Calidad_IngresarRegistroCantidadOT", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idOrdenServicio", idOP);
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

        //SELECCIONAR SI ESTA APROBADO O DESAPROBADO O SNC
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

        //VALIDAR QUE SOLO INGRESE NÚMEROS ENTEROS
        private void txtCantidadInspeccionar_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo dígitos y teclas de control como Backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Bloquea el carácter
            }
        }

        //CERRAR MI PANEL DE OBSERVACIONES
        private void btnCerarDetallesObservacion_Click(object sender, EventArgs e)
        {
            panelDetallesObservacion.Visible = false;
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

        //GENERA UNA SALIDA NO CONFORME
        private void btnGenerarCSM_Click(object sender, EventArgs e)
        {
            panelSNC.Visible = true;
            txtReponsableRegistro.Text = Program.UnoNombreUnoApellidoUsuario;
            txtOrdenTrabajoSNC.Text = txtCodigoOT.Text;
            LimpiarSNC();
        }

        //BOTON PARA GUARDAR MI SNC
        private void btnGuardarSNC_Click(object sender, EventArgs e)
        {
            //SI LOS CAMPOS ESTAN VACIOS
            if (txtReponsableRegistro.Text == "" || txtOrdenTrabajoSNC.Text == "0" || txtDescripcionSNC.Text == "")
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
                        cmd = new SqlCommand("Calidad_IngresarSNCOT", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idDetalleCantidadCalidad", Convert.ToInt32(datalistadoHistorial.SelectedCells[1].Value.ToString()));
                        cmd.Parameters.AddWithValue("@idUsuarioResponsable", Program.IdUsuario);
                        cmd.Parameters.AddWithValue("@fechaHallazgo", dtpFechaHallazgo.Value);
                        cmd.Parameters.AddWithValue("@IdOt", Convert.ToInt32(lblIdOT.Text));
                        cmd.Parameters.AddWithValue("@descripcionSNC", txtDescripcionSNC.Text);

                        //PRIMERA IMAGEN
                        if (txtImagen1.Text != "")
                        {
                            string nombreGenerado1 = "IMAGEN 1 OT " + txtOrdenTrabajoSNC.Text + " - " + DateTime.Now.ToString("ddMMyyyyHHmmss");
                            string rutaOld1 = txtImagen1.Text;
                            string RutaNew1 = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Calidad\ImagenesSNC\" + nombreGenerado1 + ".jpg";
                            File.Copy(rutaOld1, RutaNew1);
                            cmd.Parameters.AddWithValue("@imagen1", RutaNew1);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@imagen1", "");
                        }

                        //SEGUNDA IMAGEN
                        if (txtImagen2.Text != "")
                        {
                            string nombreGenerado2 = "IMAGEN 2 OP " + txtOrdenTrabajoSNC.Text + " - " + DateTime.Now.ToString("ddMMyyyyHHmmss");
                            string rutaOld2 = txtImagen2.Text;
                            string RutaNew2 = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Calidad\ImagenesSNC\" + nombreGenerado2 + ".jpg";
                            File.Copy(rutaOld2, RutaNew2);
                            cmd.Parameters.AddWithValue("@imagen2", RutaNew2);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@imagen2", "");
                        }

                        //TERCERA IMAGEN
                        if (txtImagen3.Text != "")
                        {
                            string nombreGenerado3 = "IMAGEN 3 OP " + txtOrdenTrabajoSNC.Text + " - " + DateTime.Now.ToString("ddMMyyyyHHmmss");
                            string rutaOld3 = txtImagen3.Text;
                            string RutaNew3 = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Calidad\ImagenesSNC\" + nombreGenerado3 + ".jpg";
                            File.Copy(rutaOld3, RutaNew3);
                            cmd.Parameters.AddWithValue("@imagen3", RutaNew3);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@imagen3", "");
                        }

                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Salida No Conforme registrada correctamente.", "Validación del Sistema", MessageBoxButtons.OK);
                        MostrarCantidadesSegunOT(Convert.ToInt16(lblIdOT.Text));
                        panelSNC.Visible = false;
                        LimpiarSNC();
                        MostrarSNC(datalistadoHistorial);

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
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Todos los archivos (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtImagen1.Text = openFileDialog1.FileName;
            }
        }

        private void btnCargar2_Click(object sender, EventArgs e)
        {
            openFileDialog2.InitialDirectory = "c:\\";
            openFileDialog2.Filter = "Todos los archivos (*.*)|*.*";
            openFileDialog2.FilterIndex = 1;
            openFileDialog2.RestoreDirectory = true;

            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                txtImagen2.Text = openFileDialog2.FileName;
            }
        }

        private void btnCargar3_Click(object sender, EventArgs e)
        {
            openFileDialog3.InitialDirectory = "c:\\";
            openFileDialog3.Filter = "Todos los archivos (*.*)|*.*";
            openFileDialog3.FilterIndex = 1;
            openFileDialog3.RestoreDirectory = true;

            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                txtImagen3.Text = openFileDialog3.FileName;
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

        //VISUALIZAR PLANO
        private void btnPlano_Click(object sender, EventArgs e)
        {

        }

        //VISUALIZAR OC
        private void btnOC_Click(object sender, EventArgs e)
        {

        }

        //VISUALIZAR ORDEN DE TRABAJO
        private void btnGenerarOrdenTrabajo_Click(object sender, EventArgs e)
        {

        }
    }
}

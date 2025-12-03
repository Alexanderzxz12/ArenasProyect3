using ArenasProyect3.Modulos.Resourses;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DocumentFormat.OpenXml.Spreadsheet;
using SpreadsheetLight;
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

namespace ArenasProyect3.Modulos.Produccion.ConsultasOT
{
    public partial class ListadoOT : Form
    {
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        private Cursor curAnterior = null;
        int totalCantidades = 0;
        DataGridView dgvActivo = null;
        bool hayCheckActivo = false;

        //CONMSTRUCTOR DE MI FORMULARIO
        public ListadoOT()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI FORMULARIO
        private void ListadoOT_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            cboBusqeuda.SelectedIndex = 0;
            VerificarDGVActivo();
        }

        //FUNCION PARA VERIFICAR SI HAY UNA CANTIDAD 
        public void MostrarCantidadesSegunOT(int idOrdenServicio)
        {
            totalCantidades = 0;
            datalistadoCantidades.Columns.Clear();
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("OT_MostrarCantidades", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idOrdenServicio", idOrdenServicio);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoCantidades.DataSource = dt;
                con.Close();
                datalistadoCantidades.Columns[0].Width = 40;
                datalistadoCantidades.Columns[1].Width = 120;
                datalistadoCantidades.Columns[2].Width = 100;
                alternarColorFilas(datalistadoCantidades);

                //CONTAR CUANTAS CANTIDADES HAY
                foreach (DataGridViewRow row in datalistadoCantidades.Rows)
                {
                    if (row.Cells[3].Value.ToString() == "ENTREGADO")
                    {
                        totalCantidades = totalCantidades + Convert.ToInt32(row.Cells[1].Value.ToString());
                    }
                }
                //DESCONTAR CUANTAS CANTIDADES HAY
                foreach (DataGridViewRow row in datalistadoCantidades.Rows)
                {
                    if (row.Cells[3].Value.ToString() == "DESAPROBADO")
                    {
                        totalCantidades = totalCantidades - Convert.ToInt32(row.Cells[1].Value.ToString());
                    }
                }

                //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
                foreach (DataGridViewColumn column in datalistadoCantidades.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //FUNCION PARA COLOCAR IMAGENES A MIS REGISTROS DE CANTIDADES
        public void ColocarImagenesListadoCaantidadaesOP()
        {
            try
            {
                DataGridViewImageColumn colEstado = new DataGridViewImageColumn();
                colEstado.Name = "imgEstado";
                colEstado.HeaderText = "Estado IMG";
                colEstado.ImageLayout = DataGridViewImageCellLayout.Zoom; // Ajusta la imagen
                colEstado.Width = 60;
                datalistadoCantidades.Columns.Insert(0, colEstado);

                Image imgAprobado = Image.FromFile(@"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Producción\Imagenes\flechaCorrecta.png");     // Imagen para aprobado
                Image imgDesaprobado = Image.FromFile(@"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Producción\Imagenes\flechaIncorrecta.png"); // Imagen para desaprobado

                foreach (DataGridViewRow row in datalistadoCantidades.Rows)
                {
                    if (row.Cells[4].Value != null)
                    {
                        string estado = row.Cells[4].Value.ToString();

                        if (estado == "ENTREGADO")
                        {
                            row.Cells["imgEstado"].Value = imgAprobado;
                        }
                        else if (estado == "DESAPROBADO")
                        {
                            row.Cells["imgEstado"].Value = imgDesaprobado;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de carga de datos. " + ex.Message, "Validación del sistema", MessageBoxButtons.OK);
            }
        }

        //FUNCION PARA VERIFICAR SI HAY UNA CANTIDAD EN CALIDAD
        public void MostrarCantidadesSegunOPCalidad(int idOrdenServicio)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("OT_MostrarCantidadesCalidad", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idOrdenServicio", idOrdenServicio);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoHistorial.DataSource = dt;
                con.Close();
                //REORDENAMIENTO DE COLUMNAS
                datalistadoHistorial.Columns[2].Width = 120;
                datalistadoHistorial.Columns[3].Width = 90;
                datalistadoHistorial.Columns[4].Width = 80;
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

        //FUNCION PARA MOSTRAR TODOS LOS DATOS DE MI SNC
        public void MostrarSNCCalidad(int idDetalleCantidadCalidad)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("OT_MostrarSNC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idDetalleCantidadCalidadOT", idDetalleCantidadCalidad);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoSNCDatos.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //FUNCION PARA MOSTRAR MI JEFATURA PRODUCCION
        public void MostrarJefaturaTra()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Reporte_MostrarJefeAreaIngenieria", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoDatosJefatura.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la jefatura " + ex.Message);
            }
        }

        //funcion para poder extraer l a ultima rq
        public void MostrarUltimoRQ()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            da = new SqlDataAdapter("SELECT IdRequerimientoSimple FROM RequerimientoSimple WHERE IdRequerimientoSimple = (SELECT MAX(IdRequerimientoSimple) FROM RequerimientoSimple)", con);
            da.Fill(dt);
            datalistadoRQ.DataSource = dt;
            con.Close();
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
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO
        public void ColoresListado(DataGridView dgv)
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= dgv.RowCount - 1; i++)
                {
                    string estadoPro = dgv.Rows[i].Cells[14].Value.ToString();

                    if (estadoPro == "FUERA DE FECHA")
                    {
                        dgv.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Fuchsia;
                    }
                    else if (estadoPro == "LÍMITE")
                    {
                        dgv.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Orange;
                    }
                    else if (estadoPro == "PENDIENTE")
                    {
                        dgv.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else if (estadoPro == "CULMINADO")
                    {
                        dgv.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.DarkGreen;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
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
                    string estadoHistoria = dgv.Rows[i].Cells[5].Value.ToString();

                    if (estadoHistoria == "APROBADO" || estadoHistoria == "SNC CULMINADA")
                    {
                        dgv.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                    }
                    else if (estadoHistoria == "DESAPROBADO")
                    {
                        dgv.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    else if (estadoHistoria == "SNC GENERADA")
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
        public void ColoresListadoOTCalidad(DataGridView DGV)
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

        //LISTADO DE OT Y SELECCION DE PDF Y ESTADO DE OT---------------------
        //MOSTRAR OT AL INCIO 
        //FUNCION PARA VISUALIZAR MIS RESULTADOS
        public void MostrarOrdenTrabajo(DateTime fechaInicio, DateTime fechaTermino, string cliente = null, string codigoOT = null, string descripcion = null)
        {
            using (SqlConnection con = new SqlConnection(Conexion.ConexionMaestra.conexion))
            using (SqlCommand cmd = new SqlCommand("OT_Mostrar", con))
            {
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
                        DataRow[] rowsEnProceso = dt.Select("ESTADO IN ('PENDIENTE', 'FUERA DE FECHA', 'LÍMITE')");
                        // Si hay filas, crea un nuevo DataTable, si no, usa una copia vacía del esquema.
                        DataTable dtEnProceso = rowsEnProceso.Any() ? rowsEnProceso.CopyToDataTable() : dt.Clone();
                        datalistadoEnProc.DataSource = dtEnProceso; // Asumiendo este es el nombre de tu DataGrid

                        // --- 2.3. OT Observadas (Asumimos que el estado "Observadas" es FUERA DE FECHA o LÍMITE) ---
                        DataRow[] rowsObservadas = dt.Select("[INDICADOR SNC] = 1");
                        DataTable dtObservadas = rowsObservadas.Any() ? rowsObservadas.CopyToDataTable() : dt.Clone();
                        datalistadoObs.DataSource = dtObservadas; // Asumiendo este es el nombre de tu DataGrid

                        RedimensionarListadoGeneral(datalistadoTodasOT);
                        RedimensionarListadoGeneral(datalistadoEnProc);
                        RedimensionarListadoGeneralCalidad(datalistadoObs);
                    }
                    catch (Exception ex)
                    {
                        // Manejar el error, por ejemplo, mostrando un mensaje
                        MessageBox.Show("Error al cargar las órdenes de trabajo: " + ex.Message);
                    }
                }
            }
        }

        //FUNCION PARA REDIMENSIONAR MIS LISTADOS
        public void RedimensionarListadoGeneral(DataGridView DGV)
        {
            //REDIEMNSION DE PEDIDOS
            DGV.Columns[2].Width = 95;
            DGV.Columns[3].Width = 80;
            DGV.Columns[4].Width = 80;
            DGV.Columns[5].Width = 250;
            DGV.Columns[6].Width = 465;
            DGV.Columns[7].Width = 60;
            DGV.Columns[8].Width = 90;
            DGV.Columns[9].Width = 93;
            DGV.Columns[12].Width = 75;
            DGV.Columns[14].Width = 110;
            //SE HACE NO VISIBLE LAS COLUMNAS QUE NO LES INTERESA AL USUARIO
            DGV.Columns[1].Visible = false;
            DGV.Columns[10].Visible = false;
            DGV.Columns[11].Visible = false;
            DGV.Columns[13].Visible = false;
            DGV.Columns[15].Visible = false;
            DGV.Columns[16].Visible = false;
            DGV.Columns[17].Visible = false;
            DGV.Columns[18].Visible = false;
            DGV.Columns[19].Visible = false;
            DGV.Columns[20].Visible = false;
        }

        //FUNCION PARA REDIMENSIONAR MIS LISTADOS
        public void RedimensionarListadoGeneralCalidad(DataGridView DGV)
        {
            //REDIEMNSION DE PEDIDOS
            DGV.Columns[2].Width = 95;
            DGV.Columns[3].Width = 80;
            DGV.Columns[4].Width = 80;
            DGV.Columns[5].Width = 250;
            DGV.Columns[6].Width = 465;
            DGV.Columns[7].Width = 60;
            DGV.Columns[8].Width = 90;
            DGV.Columns[9].Width = 93;
            DGV.Columns[12].Width = 75;
            DGV.Columns[13].Width = 75;
            DGV.Columns[14].Width = 110;
            DGV.Columns[16].Width = 110;
            //SE HACE NO VISIBLE LAS COLUMNAS QUE NO LES INTERESA AL USUARIO
            DGV.Columns[1].Visible = false;
            DGV.Columns[10].Visible = false;
            DGV.Columns[11].Visible = false;
            DGV.Columns[15].Visible = false;
            DGV.Columns[17].Visible = false;
            DGV.Columns[18].Visible = false;
            DGV.Columns[19].Visible = false;
            DGV.Columns[20].Visible = false;
        }

        //BUSQUEDA DE MATERIALES REORDENAMIENTO
        public void ReordenarBusquedaMateriales(DataGridView DGV)
        {
            //REDIEMNSION DE PEDIDOS
            DGV.Columns[2].Width = 90;
            DGV.Columns[3].Width = 330;
            DGV.Columns[4].Width = 60;
            ////SE HACE NO VISIBLE LAS COLUMNAS QUE NO LES INTERESA AL USUARIO
            DGV.Columns[1].Visible = false;
            ////SE BLOQUEA MI LISTADO
            DGV.Columns[2].ReadOnly = true;
            DGV.Columns[3].ReadOnly = true;
            DGV.Columns[4].ReadOnly = true;
        }

        //CAMBIAR EL CURSOR A MI LSITADOS
        private void datalistadoEnProc_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            ModificarCursor(datalistadoEnProc, "detalles", e);
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN
        private void datalistadoTodasOT_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            ModificarCursor(datalistadoTodasOT, "detallesTodos", e);
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN
        private void datalistadoObs_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            ModificarCursor(datalistadoObs, "columSelectObs", e);
        }

        //MOSTRAR OT POR FECHA
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarOrdenTrabajo(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OT POR FECHA
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarOrdenTrabajo(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OT POR FECHA
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarOrdenTrabajo(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OPRDENES TRABAJO DEPENDIENTO LA OPCIÓN ESCOGIDA
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
            MostrarOrdenTrabajo(
                DesdeFecha.Value,
                HastaFecha.Value,
                cliente,
                codigoOT,
                descripcion
            );
        }

        //LIMPIAR EL CAMBIO DE BUSQUEDA
        private void cboBusqeuda_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";
        }

        //VISUALIZAR EL PLANO DEL PRODUCTO
        private void btnVisualizarPDFPorducto_Click(object sender, EventArgs e)
        {
            EjecutarDocumento(dgvActivo.SelectedCells[15].Value.ToString());
        }

        //EJECUTAR DOCUMENTOS
        public void EjecutarDocumento(string link)
        {
            try
            {
                Process.Start(link);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Documento no encontrado, hubo un error al momento de cargar el archivo.", ex.Message, MessageBoxButtons.OK);
            }
        }

        //CARGAR MIS DETALLES DE INGRESO DE CANTIDADES
        private void datalistadoEnProc_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AbrirDetalles(datalistadoEnProc);
        }

        //CARGAR MIS DETALLES DE INGRESO DE CANTIDADES
        private void datalistadoTodasOT_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AbrirDetalles(datalistadoTodasOT);
        }

        //FUNCION PARA ABIRI MIS DETALLES D EIGRESO DE CANTUIDADES DEPENDIENDO EL ISTADO
        public void AbrirDetalles(DataGridView DGV)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (DGV.CurrentRow != null)
            {
                int count = 0;
                foreach (DataGridViewRow row in DGV.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value))
                    {
                        count++;
                    }
                }

                if (count == 0) { count = 1; }

                txtOtsSeleccionadas.Text = Convert.ToString(count);
                //CARGA DE DAOTS
                txtCodigoOT.Text = DGV.SelectedCells[2].Value.ToString();
                int IdOrdenTrabajo = Convert.ToInt32(DGV.SelectedCells[1].Value.ToString());
                txtDescripcionSub_Producto.Text = DGV.SelectedCells[6].Value.ToString();
                txtCantidadTotalOT.Text = DGV.SelectedCells[7].Value.ToString();
                txtCantidadRequerida.Text = DGV.SelectedCells[7].Value.ToString();
                dtpFechaRealizada.Value = DateTime.Now;
                txtCantidadRealizada.Text = "";
                txtCantidadRestante.Text = "";
                MostrarCantidadesSegunOT(IdOrdenTrabajo);
                lblCantidadTotalInghresada.Text = Convert.ToString(totalCantidades);
                txtCantidadRestante.Text = Convert.ToString(Convert.ToInt32(txtCantidadRequerida.Text) - Convert.ToInt32(lblCantidadTotalInghresada.Text));

                DGV.Enabled = false;
                panelIngresoCantidades.Visible = true;

                if (count != 1)
                {
                    btnGenerarGuardarCantidades.Visible = true;
                    lblGenerarGuardarCantidades.Visible = true;
                    btnGuardarCantidad.Visible = false;
                    lblGuardarCantidad.Visible = false;
                    txtCantidadRealizada.ReadOnly = true;
                    txtCantidadRealizada.Text = "Gen. Automática";
                    lblIdOT.Text = "Varios";
                    txtCantidadRestante.Text = "0";
                }
                else
                {
                    btnGuardarCantidad.Visible = true;
                    lblGuardarCantidad.Visible = true;
                    btnGenerarGuardarCantidades.Visible = false;
                    lblGenerarGuardarCantidades.Visible = false;
                    txtCantidadRealizada.ReadOnly = false;
                    lblIdOT.Text = DGV.SelectedCells[1].Value.ToString();
                }
                ColocarImagenesListadoCaantidadaesOP();
            }
            else
            {
                MessageBox.Show("Debe seleccionar una OP para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //ABIRIR MI PANEL PARA INGRESAR OBSERVACIONES
        private void datalistadoObs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoObs.RowCount != 0)
            {
                btnVisualizarSNC.Visible = false;
                panelControlCalidad.Visible = true;

                lblIdOT.Text = datalistadoObs.SelectedCells[1].Value.ToString();
                txtCoidgoOTCalidad.Text = datalistadoObs.SelectedCells[2].Value.ToString();
                txtDescripcionProductoCalidad.Text = datalistadoObs.SelectedCells[6].Value.ToString();
                txtArea.Text = datalistadoObs.SelectedCells[18].Value.ToString();
                txtIdArea.Text = datalistadoObs.SelectedCells[17].Value.ToString();
                lblIdOT.Text = datalistadoObs.SelectedCells[1].Value.ToString();
                MostrarCantidadesSegunOPCalidad(Convert.ToInt32(lblIdOT.Text));
                btnGenerarCSM.Visible = false;
                lblGenerarCSM.Visible = false;
                datalistadoObs.Enabled = false;
            }
        }

        //GENERACION DE REPORTES
        private void btnVsualizarPDFPT_Click(object sender, EventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (dgvActivo.CurrentRow != null)
            {
                string codigoOrdenTrabajo = dgvActivo.Rows[dgvActivo.CurrentRow.Index].Cells[1].Value.ToString();
                Visualizadores.VisualizarOrdenTrabajo frm = new Visualizadores.VisualizarOrdenTrabajo();
                frm.lblCodigo.Text = codigoOrdenTrabajo;
                frm.Show();
            }
            else
            {
                MessageBox.Show("Debe seleccionar una OT para poder generar el PDF.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //EVENTO PARA GUARDAR MI S CANTIDADES INGRESADAS
        private void btnGuardarCantidad_Click(object sender, EventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoEnProc.CurrentRow != null)
            {
                if (txtCantidadRealizada.Text == "" || txtCantidadRealizada.Text == "0")
                {
                    MessageBox.Show("Debe ingresar una cantidad válida para poder registrar.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else if (txtCantidadRequerida.Text == lblCantidadTotalInghresada.Text)
                {
                    MessageBox.Show("La orden de producción ya culminó.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else if (Convert.ToInt32(txtCantidadRestante.Text) < Convert.ToInt32(txtCantidadRealizada.Text))
                {
                    MessageBox.Show("No se puede ingresar una cantidad mayor a la restante.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    DialogResult boton = MessageBox.Show("¿Realmente desea ingresar esta cantidad?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                    if (boton == DialogResult.OK)
                    {
                        try
                        {
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("OT_IngresarRegistroCantidad", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idOrdenServicio", lblIdOT.Text);
                            cmd.Parameters.AddWithValue("@cantidad", txtCantidadRealizada.Text);
                            cmd.Parameters.AddWithValue("@fechaRegistro", Convert.ToDateTime(dtpFechaRealizada.Value));
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Cantidd ingresada correctamente.", "Validación del Sistema", MessageBoxButtons.OK);
                            MostrarOrdenTrabajo(DesdeFecha.Value, HastaFecha.Value);
                            LimpiarCantidades();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una OT para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //EVENTO PARA GUARDAR VARIAS CANTIDADES INGRESADAS
        private void btnGenerarGuardarCantidades_Click(object sender, EventArgs e)
        {
            //List<int> idOTSeleccionada = new List<int>();
            //List<int> CantidadTotalOTSeleccionada = new List<int>();

            //foreach (DataGridViewRow row in datalistadoEnProc.Rows)
            //{
            //    DataGridViewCheckBoxCell checkBox = row.Cells[0] as DataGridViewCheckBoxCell;

            //    if (checkBox != null && Convert.ToBoolean(checkBox.Value) == true)
            //    {
            //        try
            //        {
            //            int idOt = Convert.ToInt32(row.Cells[1].Value.ToString());
            //            int cantidadEsperada = Convert.ToInt32(row.Cells[7].Value.ToString());
            //            int cantidadHecha = Convert.ToInt32(row.Cells[10].Value.ToString());
            //            int TotalCantidad = cantidadEsperada - cantidadHecha;

            //            if (TotalCantidad != 0)
            //            {
            //                SqlConnection con = new SqlConnection();
            //                SqlCommand cmd = new SqlCommand();
            //                con.ConnectionString = Conexion.ConexionMaestra.conexion;
            //                con.Open();
            //                cmd = new SqlCommand("OT_IngresarRegistroCantidad", con);
            //                cmd.CommandType = CommandType.StoredProcedure;
            //                cmd.Parameters.AddWithValue("@idOrdenServicio", idOt);
            //                cmd.Parameters.AddWithValue("@cantidad", TotalCantidad);
            //                cmd.Parameters.AddWithValue("@fechaRegistro", Convert.ToDateTime(dtpFechaRealizada.Value));
            //                cmd.ExecuteNonQuery();
            //                con.Close();
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.Message);
            //        }
            //    }
            //}

            //MessageBox.Show("Operación terminada.", "Validación del Sistema", MessageBoxButtons.OK);
            //MostrarOrdenTrabajo(DesdeFecha.Value, HastaFecha.Value);
            //LimpiarCantidades();
        }

        //EVENTO PARA RETROCEDER O SALIR DE MI VENTANA DE INGRESO DE CANTIDADES
        private void btnSalirCantidad_Click(object sender, EventArgs e)
        {
            LimpiarCantidades();
        }

        //EVENTO PARA RETROCEDER O SALIR DE MI VENTANA DE INGRESO DE CANTIDADES
        private void btnCerrarDetallesOPCantidades_Click(object sender, EventArgs e)
        {
            LimpiarCantidades();
        }

        //FUNCION PARA LIMPIAR LAS CANTIDADES
        public void LimpiarCantidades()
        {
            dgvActivo.Enabled = true;
            panelIngresoCantidades.Visible = false;
            txtOtsSeleccionadas.Text = "";
            txtCantidadRealizada.Text = "";
            txtCantidadRestante.Text = "";
        }

        //SALIR DE DETALLES DE REVISION
        private void btnRegresarControl_Click(object sender, EventArgs e)
        {
            panelControlCalidad.Visible = false;
            datalistadoObs.Enabled = true;
            lblGenerarCSM.Visible = false;
            btnGenerarCSM.Visible = false;
            btnVisualizarSNC.Visible = false;
            lblLeyendaVisualizar.Visible = false;
        }

        //ABIRR MI COMETARIO DE LA SNC
        private void datalistadoHistorial_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lblGenerarCSM.Visible = false;
            btnGenerarCSM.Visible = false;
            btnVisualizarSNC.Visible = false;
            lblLeyendaVisualizar.Visible = false;
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoHistorial.CurrentRow != null)
            {
                if (datalistadoHistorial.SelectedCells[5].Value.ToString() == "SNC GENERADA")
                {
                    lblGenerarCSM.Visible = true;
                    btnGenerarCSM.Visible = true;
                }
                else
                {
                    lblGenerarCSM.Visible = false;
                    btnGenerarCSM.Visible = false;
                }
                if (datalistadoHistorial.SelectedCells[5].Value.ToString() == "SNC CULMINADA")
                {
                    btnVisualizarSNC.Visible = true;
                    lblLeyendaVisualizar.Visible = true;

                }
                else
                {
                    btnVisualizarSNC.Visible = false;
                    lblLeyendaVisualizar.Visible = false;
                }
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

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN - HISTORIAL
        private void datalistadoHistorial_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            ModificarCursor(datalistadoHistorial, "columDesc", e);
        }
        
        //SALIR DEL COMETARIO DE CALIDAD
        private void btnCerarDetallesObservacion_Click(object sender, EventArgs e)
        {
            panelDetallesObservacion.Visible = false;
        }

        //VISUALIZAR MI PANEL DE SNC
        private void btnVisualizarSNC_Click(object sender, EventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoHistorial.CurrentRow != null)
            {
                //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO DESAPROBADO
                string codigoDetalleCantidadCalidad = datalistadoHistorial.Rows[datalistadoHistorial.CurrentRow.Index].Cells[1].Value.ToString();
                Visualizadores.VisualizarSNCOT frm = new Visualizadores.VisualizarSNCOT();
                frm.lblCodigo.Text = codigoDetalleCantidadCalidad;
                //CARGAR VENTANA
                frm.Show();
            }
        }

        //GENERAR CSM POR PARTE DE INGENIERIA
        private void btnGenerarCSM_Click(object sender, EventArgs e)
        {
            panelRevisionOT.Visible = true;
            panelControlCalidad.Visible = false;

            MostrarSNCCalidad(Convert.ToInt32(datalistadoHistorial.SelectedCells[1].Value.ToString()));
            txtReponsableRegistro.Text = datalistadoSNCDatos.SelectedCells[0].Value.ToString();
            txtAutoriza.Text = Program.NombreUsuarioCompleto;
            dtpFechaHallazgo.Value = Convert.ToDateTime(datalistadoSNCDatos.SelectedCells[1].Value.ToString());
            txtOrdenTrabajoSNC.Text = txtCoidgoOTCalidad.Text;
            txtDescripcionSNC.Text = datalistadoSNCDatos.SelectedCells[2].Value.ToString();
            lblImagen1.Text = datalistadoSNCDatos.SelectedCells[5].Value.ToString();
            lblImagen2.Text = datalistadoSNCDatos.SelectedCells[6].Value.ToString();
            lblImagen3.Text = datalistadoSNCDatos.SelectedCells[7].Value.ToString();
            lblIdSNC.Text = datalistadoSNCDatos.SelectedCells[8].Value.ToString();
            txtCantidadObservada.Text = datalistadoHistorial.SelectedCells[3].Value.ToString();
            txtAreaSNC.Text = txtArea.Text;
            txtIdAreaSNC.Text = txtIdArea.Text;
            txtIdOTSNC.Text = lblIdOT.Text;
        }

        //CARGA DEL FACTOR DE FALLO
        private void txtAreaSNC_TextChanged(object sender, EventArgs e)
        {
            MostrarFactorFallo();
        }

        //FUNCION PARA DEFINIR EL FACTOR DE FALLO
        public void MostrarFactorFallo()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdTipoHallazgo, Nombre FROM [TipoHallazgo] WHERE Estado = 1 AND IdArea = 13", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboFactorFallo.ValueMember = "IdTipoHallazgo";
                cboFactorFallo.DisplayMember = "Nombre";
                cboFactorFallo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error del sistema. " + ex.Message, "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //ADJUNTAR IMAGEN
        private void btnAgregarArhcivo_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Todos los archivos (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    txtRutaArchivo.Text = openFileDialog1.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //LIMPIAR MI RUTA DE ARCHVO
        private void btnLimpiarArchivo_Click(object sender, EventArgs e)
        {
            txtRutaArchivo.Text = "";
        }

        //LIMPIAR CUANDO SE CAMBIA DE TIPO DE ARCHIVO
        private void cboTipoArchivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtRutaArchivo.Text = "";
        }

        //CERRAR MI SNC
        private void btnCerrarSNC_Click(object sender, EventArgs e)
        {
            panelRevisionOT.Visible = false;
            panelControlCalidad.Visible = true;
            datalistadoObs.Enabled = false;
            LimpairCampos();
        }

        //CERRAR MI PANEL DE DETALLES DE EVALUACION
        private void lblCerrarDetallesEvaluacion_Click(object sender, EventArgs e)
        {
            panelControlCalidad.Visible = false;
            datalistadoObs.Enabled = true;
            lblGenerarCSM.Visible = false;
            btnGenerarCSM.Visible = false;
            btnVisualizarSNC.Visible = false;
            lblLeyendaVisualizar.Visible = false;
        }

        //GUARDAR MI SNV
        private void btnGuardarSNC_Click(object sender, EventArgs e)
        {
            if (txtOrdenTrabajoSNC.Text == "" || txtDescripcionSNC.Text == "" || txtCausaSNC.Text == "" || txtAccionesTomadas.Text == "" || txtOportunidadMejora.Text == "" || ckLiberacion.Checked == true && txtRutaArchivo.Text == "")
            {
                MessageBox.Show("Debe completar todos los campos obligatorios para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                DialogResult boton = MessageBox.Show("¿Realmente desea completar esta SNC?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        SqlCommand cmd = new SqlCommand();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        cmd = new SqlCommand("OT_IngresarSNC", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idSNC", Convert.ToInt16(lblIdSNC.Text));
                        cmd.Parameters.AddWithValue("@idDetalleCantidadCalidadOT", Convert.ToInt16(datalistadoHistorial.SelectedCells[1].Value.ToString()));
                        cmd.Parameters.AddWithValue("@descripcionAcciones", txtAccionesTomadas.Text);
                        cmd.Parameters.AddWithValue("@idAutoriza", Program.IdUsuario);
                        cmd.Parameters.AddWithValue("@inicio", dtpInicio.Value);
                        cmd.Parameters.AddWithValue("@finaliza", dtpFinal.Value);
                        //------------------------------
                        cmd.Parameters.AddWithValue("@liberacion", ckLiberacion.Checked ? 1 : 0);
                        //------------------------------
                        cmd.Parameters.AddWithValue("@correcion", ckCorrecion.Checked ? 1 : 0);
                        //------------------------------
                        cmd.Parameters.AddWithValue("@reproceso", ckReproceso.Checked ? 1 : 0);
                        //------------------------------
                        cmd.Parameters.AddWithValue("@reclasificacion", ckReclasificacion.Checked ? 1 : 0);
                        //------------------------------
                        cmd.Parameters.AddWithValue("@recuperacion", ckRecuperacion.Checked ? 1 : 0);
                        //------------------------------
                        cmd.Parameters.AddWithValue("@destruccion", ckReposicion.Checked ? 1 : 0);
                        //------------------------------
                        cmd.Parameters.AddWithValue("@otros", ckOtros.Checked ? 1 : 0);
                        cmd.Parameters.AddWithValue("@descripcionOtros", txtDescripcionOtros.Text);
                        cmd.Parameters.AddWithValue("@fechaRegistroProduccion", DateTime.Now);
                        cmd.Parameters.AddWithValue("@causaConformidad", txtCausaSNC.Text);
                        cmd.Parameters.AddWithValue("@oprtunidadMejora", txtOportunidadMejora.Text);

                        if (txtRutaArchivo.Text == "")
                        {
                            cmd.Parameters.AddWithValue("@rutaLiberacionEvi", "");
                        }
                        else
                        {
                            string NombreGenerado = "OT " + txtOrdenTrabajoSNC.Text + " SNC " + datalistadoHistorial.SelectedCells[1].Value.ToString();
                            string RutaOld = txtRutaArchivo.Text;
                            string RutaNew = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Calidad\SNC Evidencia\" + NombreGenerado + ".pdf";
                            File.Copy(RutaOld, RutaNew, true);
                            cmd.Parameters.AddWithValue("@rutaLiberacionEvi", RutaNew);
                        }

                        cmd.ExecuteNonQuery();
                        con.Close();

                        if (ckReproceso.Checked == true)
                        {
                            CambiarEstadoCalidad(txtOrdenTrabajoSNC.Text, 2);
                        }

                        MessageBox.Show("Salida No Conforme registrada correctamente.", "Validación del Sistema", MessageBoxButtons.OK);
                        panelRevisionOT.Visible = false;
                        dgvActivo.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            if (ckReproceso.Checked == true)
            {
                ValidarRequeSheck();
                // Ahora decides qué hacer
                if (hayCheckActivo)
                {
                    GenerarRQAdicional();
                }
            }

            if (ckReposicion.Checked == true)
            {
                GenerarOTAdicional();
            }
            LimpairCampos();
        }

        //VALIDAR SI EN MI RQ HAY UN ELEMENTO SELECCIONADO
        public void ValidarRequeSheck()
        {
            hayCheckActivo = false;
            foreach (DataGridViewRow fila in datalistadoMaterialesFormulacion.Rows)
            {
                // Evitar filas nuevas (cuando AllowUserToAddRows = true)
                if (fila.IsNewRow) continue;
                // Verificamos el valor del checkbox
                bool valorCheck = Convert.ToBoolean(fila.Cells["columSeleccionar"].Value);

                if (valorCheck)
                {
                    hayCheckActivo = true;
                    break; // ya encontramos uno, no hace falta seguir
                }
            }
        }

        //FUNCION PARA GENERAR UNA OP ADICIONAL POR REQPOSISCION
        public void GenerarOTAdicional()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                SqlCommand cmd = new SqlCommand();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                cmd = new SqlCommand("OT_InsertarReposicion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Codigo_OT_Original", txtOrdenTrabajoSNC.Text);
                cmd.Parameters.AddWithValue("@Nueva_Fecha_Entrega", dtpFinal.Value);
                cmd.Parameters.AddWithValue("@idCantCal", Convert.ToInt16(datalistadoHistorial.SelectedCells[1].Value.ToString()));
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Se generó la OP por reposición automaticamnete: OT " + txtOrdenTrabajoSNC.Text, "Validación del Sistema", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //FUNCION PARA GENERAR UN RQ PARA PRODUCCION ADICIONAL
        public void GenerarRQAdicional()
        {
            MostrarJefaturaTra();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            SqlCommand cmd = new SqlCommand();
            con.Open();
            cmd = new SqlCommand("OP_InsertarRequerimientoAdicional", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //INGRESAR LOS DATOS GENERALES DE MI REQUERIMIENTO
            cmd.Parameters.AddWithValue("@fechaRequerida", DateTime.Now);
            cmd.Parameters.AddWithValue("@fechaSolicitada", DateTime.Now);
            cmd.Parameters.AddWithValue("@desJefatura", datalistadoDatosJefatura.SelectedCells[0].Value.ToString());
            cmd.Parameters.AddWithValue("@idSolicitante", datalistadoDatosJefatura.SelectedCells[3].Value.ToString());
            cmd.Parameters.AddWithValue("@idCentroCostos", 8);
            cmd.Parameters.AddWithValue("@observaciones", "REQUERIMIENTO ADICIONAL PARA ORDEN DE TRABAJO");
            cmd.Parameters.AddWithValue("@idSede", 1);
            cmd.Parameters.AddWithValue("@idLocal", 1);
            cmd.Parameters.AddWithValue("@idArea", Convert.ToInt16(txtIdAreaSNC.Text));
            cmd.Parameters.AddWithValue("@idipo", 6);
            cmd.Parameters.AddWithValue("@estadoLogistica", 1);
            cmd.Parameters.AddWithValue("@mensajeAnulacion", "");
            cmd.Parameters.AddWithValue("@idJefatura", datalistadoDatosJefatura.SelectedCells[3].Value.ToString());
            cmd.Parameters.AddWithValue("@aliasCargaJefatura", datalistadoDatosJefatura.SelectedCells[4].Value.ToString());

            int cantidadItems = 0;
            foreach (DataGridViewRow row in datalistadoMaterialesFormulacion.Rows)
            {
                bool estado = Convert.ToBoolean(row.Cells["columSeleccionar"].Value);
                if (estado == true)
                {
                    cantidadItems = cantidadItems + 1;
                }
            }

            cmd.Parameters.AddWithValue("@cantidadItems", cantidadItems);
            cmd.Parameters.AddWithValue("@idPrioridad", 1);
            cmd.Parameters.AddWithValue("@idOP", 0);
            cmd.Parameters.AddWithValue("@idOT", txtIdOTSNC.Text);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Se generó el requerimiento para la OT " + txtCodigoOTReque.Text + ".", "Validación del Sistema", MessageBoxButtons.OK);

            //VARIABLE PARA CONTAR LA CANTIDAD DE ITEMS QUE HAY
            int contador = 1;
            //INGRESO DE LOS DETALLES DEL REQUERIMIENTO SIMPLE CON UN FOREACH
            foreach (DataGridViewRow row in datalistadoMaterialesFormulacion.Rows)
            {
                bool estado = Convert.ToBoolean(row.Cells["columSeleccionar"].Value);
                if (estado == true)
                {
                    decimal cantidad = Convert.ToDecimal(row.Cells["cantidadSolicitada"].Value);

                    //PROCEDIMIENTO ALMACENADO PARA GUARDAR LOS PRODUCTOS
                    con.Open();
                    cmd = new SqlCommand("OP_InsertarRequerimientoSimpleDetalleProductos", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@item", contador);
                    cmd.Parameters.AddWithValue("@idArt", Convert.ToString(row.Cells[5].Value));
                    //SACAR LA CANTIDAD REAL POR UNO
                    cantidad = cantidad / Convert.ToDecimal(txtCantidadObserbadaReque.Text);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@stock", Convert.ToString(row.Cells[4].Value));
                    cmd.Parameters.AddWithValue("@cantidadTotal", Convert.ToString(row.Cells[3].Value));
                    cmd.ExecuteNonQuery();
                    con.Close();
                    //AUMENTAR
                    contador++;
                }
            }

            MostrarUltimoRQ();
            VisualizarRQGenerado();
        }

        //EVENTO PARA VISUALIZAR EL RQ
        public void VisualizarRQGenerado()
        {
            string codigoReporte = datalistadoRQ.Rows[datalistadoRQ.CurrentRow.Index].Cells[0].Value.ToString();
            Visualizadores.VisualizarRequerimientoSimple frm = new Visualizadores.VisualizarRequerimientoSimple();
            frm.lblCodigo.Text = codigoReporte;
            frm.Show();
        }

        //ACTIALIZAR MI DATAGRIDVIEW NI BIEN MODIFIQUE UN DATO
        private void datalistadoMaterialesFormulacion_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (datalistadoMaterialesFormulacion.IsCurrentCellDirty &&
            datalistadoMaterialesFormulacion.CurrentCell is DataGridViewCheckBoxCell)
            {
                datalistadoMaterialesFormulacion.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        //CERRAR MI PANEL DE GENERACION DE REQUERIMIENTO
        private void btnCerrarGeneracionReque_Click(object sender, EventArgs e)
        {
            panelGeneracionRequeRepro.Visible = false;
            panelRevisionOT.Visible = true;
        }

        //CAMBIAR EL ESTADO DE MI OP A FINALIZADA
        public void CambiarEstadoCalidad(string codigoOP, int estadoCalidad)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                SqlCommand cmd = new SqlCommand();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                cmd = new SqlCommand("OT_EstadoCalidad", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigoOT", codigoOP);
                cmd.Parameters.AddWithValue("@estadoCalidad", estadoCalidad);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //SELECCION DE UN TIPO DE ACCION - LIBERACION
        private void ckLiberacion_CheckedChanged(object sender, EventArgs e)
        {
            cboTipoArchivo.SelectedIndex = 0;
            txtRutaArchivo.Text = "";

            if (ckLiberacion.Checked == true)
            {
                panelCargaImagen.Visible = true;
                ckReproceso.Checked = false;
                ckReposicion.Checked = false;
            }
            else
            {
                panelCargaImagen.Visible = false;
            }
        }

        //SELECCION DE UN TIPO DE ACCION - REPROCESO
        private void ckReproceso_CheckedChanged(object sender, EventArgs e)
        {
            if (ckReproceso.Checked)
            {
                btnInfoMaterialesRepro.Visible = true;
                ckReposicion.Checked = false;
                ckLiberacion.Checked = false;
            }
            else
            {
                btnInfoMaterialesRepro.Visible = false;
            }
        }

        //SELECCION DE UN TIPO DE ACCION - REPROSISION
        private void ckReposicion_CheckedChanged(object sender, EventArgs e)
        {
            if (ckReproceso.Checked)
            {
                ckLiberacion.Checked = false;
                ckReproceso.Checked = false;
            }
        }

        //ABIRIR PABNEL DE MATERIALES
        private void btnInfoMaterialesRepro_Click(object sender, EventArgs e)
        {
            cboBusquedaMaterial.SelectedIndex = 0;
            bool algunaMarcada = false;

            foreach (DataGridViewRow row in datalistadoMaterialesFormulacion.Rows)
            {
                bool estadoCheck = Convert.ToBoolean(row.Cells["columSeleccionar"].Value);

                if (estadoCheck == true)
                {
                    algunaMarcada = true;
                    break; // Ya encontramos una, no hace falta seguir
                }
            }

            if (algunaMarcada)
            {
                panelGeneracionRequeRepro.Visible = true;
                panelRevisionOT.Visible = false;
            }
            else
            {
                panelGeneracionRequeRepro.Visible = true;
                panelRevisionOT.Visible = false;
                CargarDatos();
            }
        }

        //CARGAR DATOS DE MI GENERACION DE REQUERIMIENTO
        public void CargarDatos()
        {
            txtResponsableRegistroIngenieria.Text = Program.NombreUsuarioCompleto;
            txtCodigoOTReque.Text = txtOrdenTrabajoSNC.Text;
            txtCantidadObserbadaReque.Text = txtCantidadObservada.Text;
            txtCodigoFormulacionReque.Text = datalistadoObs.SelectedCells[20].Value.ToString();
            txtIdOPSNCRQ.Text = txtIdOTSNC.Text;
            BuscarMaterialesFormulacion(txtCodigoFormulacionReque.Text);
        }

        //BUSCAR DETALLES Y MATERIALES DE MI FORMULACION
        public void BuscarMaterialesFormulacion(string codigoFormulacion)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("OT_BuscarMaterialesFormulacion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigoFormulacion", codigoFormulacion);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalsitadoMaterialesPre.DataSource = dt;
            con.Close();

            datalistadoMaterialesFormulacion.Rows.Clear();

            foreach (DataGridViewRow dgv in datalsitadoMaterialesPre.Rows)
            {
                string codigoBSS = dgv.Cells[2].Value.ToString();
                string desProducto = dgv.Cells[4].Value.ToString();

                decimal cantidadObserbada = Convert.ToDecimal(txtCantidadObserbadaReque.Text);
                decimal cantidad = Convert.ToDecimal(dgv.Cells[5].Value.ToString());
                decimal CantidadTotal = cantidadObserbada * cantidad;

                string Stock = dgv.Cells[8].Value.ToString();
                string idArt = dgv.Cells[1].Value.ToString();

                datalistadoMaterialesFormulacion.Rows.Add(new[] { null, codigoBSS, desProducto, Convert.ToString(CantidadTotal), Stock, idArt });
            }
        }

        //CERRAR EL PANEL DE GENERAR SNC
        private void lblCerrarSNC_Click(object sender, EventArgs e)
        {
            panelRevisionOT.Visible = false;
            panelControlCalidad.Visible = true;
            datalistadoObs.Enabled = false;
            LimpairCampos();
        }

        //FUNCION PARA LIMPIAR CAMPOS
        public void LimpairCampos()
        {
            txtCausaSNC.Text = "";
            txtAccionesTomadas.Text = "";
            lblImagen1.Text = "***";
            lblImagen2.Text = "***";
            lblImagen3.Text = "***";
            txtDescripcionOtros.Text = "";
            txtOportunidadMejora.Text = "";
            ckLiberacion.Checked = false;
            ckReproceso.Checked = false;
            ckRecuperacion.Checked = false;
            ckOtros.Checked = false;
            ckCorrecion.Checked = false;
            ckReclasificacion.Checked = false;
            ckReposicion.Checked = false;
        }

        //EVENETOS Y RECURSOS VARIOS--------------------------------------------------------------------------------
        //COLOREAR MI LSITADO DE OT EN PROCESO
        private void datalistadoEnProc_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (datalistadoEnProc.RowCount != 0)
            {
                ColoresListado(datalistadoEnProc);
            }
        }

        //COLOREAR MI LIESTADO DE OT TODAS
        private void datalistadoTodasOT_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (datalistadoTodasOT.RowCount != 0)
            {
                ColoresListado(datalistadoTodasOT);
            }
        }

        //COLOREAR MI LIESTADO DE OT TODAS
        private void datalistadoObs_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (datalistadoObs.RowCount != 0)
            {
                ColoresListadoOTCalidad(datalistadoObs);
            }
        }

        //COLOREAR MI LIESTADO DE OT TODAS
        private void datalistadoHistorial_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (datalistadoHistorial.RowCount != 0)
            {
                ColoresListadoCantidades(datalistadoHistorial);
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
            if (TabControl.SelectedTab.Text == "OT en proceso")
            {
                dgvActivo = datalistadoEnProc;
            }
            else if (TabControl.SelectedTab.Text == "Todas las OT")
            {
                dgvActivo = datalistadoTodasOT;
            }
            else if (TabControl.SelectedTab.Text == "OT observadas")
            {
                dgvActivo = datalistadoObs;
            }
        }

        //SELECCIONAR UN LISTADO
        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarDGVActivo();
        }

        //-----------------------------------------------------------------------------------------------------------
        //PARTE DE BUSQUEDA DE NUEVOS MATERIALES
        //TIPO DE BUSQUEDA DE MATERIALES
        private void cboBusquedaMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusquedaMaterial.Text = "";
        }

        //CAJA DE BUSQUEDA DE MATERIALES
        private void txtBusquedaMaterial_TextChanged(object sender, EventArgs e)
        {
            string codigoBss = null;
            string codigo = null;
            string descripcion = null;
            string textoBusqueda = txtBusquedaMaterial.Text;

            if (cboBusquedaMaterial.Text == "CÓDIGO BSS")
            {
                codigoBss = textoBusqueda;
            }
            else if (cboBusquedaMaterial.Text == "CÓDIGO")
            {
                codigo = textoBusqueda;
            }
            else if (cboBusquedaMaterial.Text == "DESCRIPCIÓN")
            {
                descripcion = textoBusqueda;
            }
            BuscarMateriales(
                codigoBss,
                codigo,
                descripcion
            );
        }

        //FUNCIO PARA BUSCAR MATERIALES
        public void BuscarMateriales(string codigoBss = null, string codigo = null, string descripcion = null)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("OP_BuscarProductos", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigoBSS", codigoBss);
                cmd.Parameters.AddWithValue("@codigo", codigo);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoMasMateriales.DataSource = dt;
                con.Close();
                ReordenarBusquedaMateriales(datalistadoMasMateriales);
            }
            catch (Exception ex)
            {
                // Manejar el error, por ejemplo, mostrando un mensaje
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
        }

        //SELECCIONAR UN PRODCITG
        private void datalistadoMasMateriales_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //////////////////////////--------------------------------------------------------------------------
            //CODIGO IMPLEMENTADO 
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            else
            {
                DataGridViewColumn currentColumn = datalistadoMasMateriales.Columns[e.ColumnIndex];

                //SI SE PRESIONA SOBRE LA COLUMNA QUE CONTIENE LA FLECHA PARA COLOCAR LOS CLIENTES
                if (currentColumn.Name == "ckSeleccionarMaterial")
                {
                    //RECOPILACIÓN DE DATOS Y ALMACENAMIENTO
                    string codigoBSS = datalistadoMasMateriales.SelectedCells[2].Value.ToString();
                    string desProducto = datalistadoMasMateriales.SelectedCells[3].Value.ToString();
                    string stock = datalistadoMasMateriales.SelectedCells[4].Value.ToString();
                    string idArt = datalistadoMasMateriales.SelectedCells[1].Value.ToString();

                    datalistadoMaterialesFormulacion.Rows.Add(new[] { null, codigoBSS, desProducto, "0", stock, idArt });

                    //CODIGO IMPLEMENTADO 
                    datalistadoMasMateriales.Rows.RemoveAt(e.RowIndex);
                }
            }
        }
        //-----------------------------------------------------------------------------------------------------------
        private void btnImagen1_Click(object sender, EventArgs e)
        {
            CargarImagenes(lblImagen1);
        }

        private void btnImagen2_Click(object sender, EventArgs e)
        {
            CargarImagenes(lblImagen2);
        }

        private void btnImagen3_Click(object sender, EventArgs e)
        {
            CargarImagenes(lblImagen3);
        }

        //FUNCION PARA LIMPIAR DIDNAMICAMENTE MIS CAJAS DE IMAGENES
        public void CargarImagenes(Label lblImagen)
        {
            try
            {
                if (lblImagen.Text == "***" || lblImagen.Text == "")
                {
                    MessageBox.Show("No hay ninguna imagen para mostrar.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    Process.Start(lblImagen.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de carga." + ex, "Validación del Sistema", MessageBoxButtons.OK);
            }
        }
        //-----------------------------------------------------------------------------------------------------------

        //VIZUALIZAR DATOS EXCEL--------------------------------------------------------------------
        public void MostrarExcel()
        {
            datalistadoExcel.Rows.Clear();

            foreach (DataGridViewRow dgv in dgvActivo.Rows)
            {
                string numeroOT = dgv.Cells[2].Value.ToString();
                string fechaInicio = dgv.Cells[3].Value.ToString();
                string fechaFinal = dgv.Cells[4].Value.ToString();
                string cliente = dgv.Cells[5].Value.ToString();
                string descripcionDescripcion = dgv.Cells[6].Value.ToString();
                string cantidad = dgv.Cells[7].Value.ToString();
                string color = dgv.Cells[8].Value.ToString();
                string numeroOrdenProduccion = dgv.Cells[9].Value.ToString();
                string cantidadRealizada = dgv.Cells[12].Value.ToString();
                string estado = dgv.Cells[14].Value.ToString();

                datalistadoExcel.Rows.Add(new[] { numeroOT, fechaInicio, fechaFinal, cliente, descripcionDescripcion, cantidad, color, numeroOrdenProduccion, cantidadRealizada, estado });
            }
        }

        //BOTON PARA EXPORTAR MIS DATOS
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            MostrarExcel();

            SLDocument sl = new SLDocument();
            SLStyle style = new SLStyle();
            SLStyle styleC = new SLStyle();

            //COLUMNAS
            sl.SetColumnWidth(1, 15);
            sl.SetColumnWidth(2, 20);
            sl.SetColumnWidth(3, 20);
            sl.SetColumnWidth(4, 40);
            sl.SetColumnWidth(5, 60);
            sl.SetColumnWidth(6, 15);
            sl.SetColumnWidth(7, 20);
            sl.SetColumnWidth(8, 20);
            sl.SetColumnWidth(9, 15);
            sl.SetColumnWidth(10, 25);

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
                sl.SetCellValue(ir, 5, row.Cells[4].Value.ToString());
                sl.SetCellValue(ir, 6, row.Cells[5].Value.ToString());
                sl.SetCellValue(ir, 7, row.Cells[6].Value.ToString());
                sl.SetCellValue(ir, 8, row.Cells[7].Value.ToString());
                sl.SetCellValue(ir, 9, row.Cells[8].Value.ToString());
                sl.SetCellValue(ir, 10, row.Cells[9].Value.ToString());
                sl.SetCellStyle(ir, 1, styleC);
                sl.SetCellStyle(ir, 2, styleC);
                sl.SetCellStyle(ir, 3, styleC);
                sl.SetCellStyle(ir, 4, styleC);
                sl.SetCellStyle(ir, 5, styleC);
                sl.SetCellStyle(ir, 6, styleC);
                sl.SetCellStyle(ir, 7, styleC);
                sl.SetCellStyle(ir, 8, styleC);
                sl.SetCellStyle(ir, 9, styleC);
                sl.SetCellStyle(ir, 10, styleC);
                ir++;
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            sl.SaveAs(desktopPath + @"\Reporte de ordenes de trabajo.xlsx");
            MessageBox.Show("Se exportó los datos a un archivo de Microsoft Excel en la siguiente ubicación: " + desktopPath, "Validación del Sistema", MessageBoxButtons.OK);
        }

        //FUNCION PARA GUARDAR 
        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear una instancia del reporte
                ReportDocument crystalReport = new ReportDocument();

                // Ruta del reporte .rpt
                //string rutaBase = Application.StartupPath;
                string rutaBase = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Recursos y Programas\";
                string rutaReporte = "";

                rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeOrdenTrabajo.rpt");

                crystalReport.Load(rutaReporte);

                // Configurar la conexión a la base de datos
                ConnectionInfo connectionInfo = new ConnectionInfo
                {
                    ServerName = "192.168.1.154,1433", // Ejemplo: "localhost" o "192.168.1.100"
                    DatabaseName = "BD_VENTAS_2", // Nombre de la base de datos
                    UserID = "sa", // Usuario de la base de datos
                    Password = "Arenas.2020!" // Contraseña del usuario
                };

                // Aplicar la conexión a cada tabla del reporte
                foreach (CrystalDecisions.CrystalReports.Engine.Table table in crystalReport.Database.Tables)
                {
                    TableLogOnInfo logOnInfo = table.LogOnInfo;
                    logOnInfo.ConnectionInfo = connectionInfo;
                    table.ApplyLogOnInfo(logOnInfo);
                }

                // **Enviar parámetro al reporte**
                // Cambia "NombreParametro" por el nombre exacto del parámetro en tu reporte
                int idOrdenServicio = Convert.ToInt32(dgvActivo.SelectedCells[1].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string codigoOrdenServicio = dgvActivo.SelectedCells[2].Value.ToString(); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string cliente = dgvActivo.SelectedCells[6].Value.ToString(); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string unidad = dgvActivo.SelectedCells[7].Value.ToString(); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                crystalReport.SetParameterValue("@idOrdenServicio", idOrdenServicio);

                // Ruta de salida en el escritorio
                string rutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string rutaSalida = System.IO.Path.Combine(rutaEscritorio, "OT número " + codigoOrdenServicio + " - " + cliente + " - " + unidad + ".pdf");

                // Exportar a PDF
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaSalida);

                MessageBox.Show($"Reporte exportado correctamente a: {rutaSalida}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al exportar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

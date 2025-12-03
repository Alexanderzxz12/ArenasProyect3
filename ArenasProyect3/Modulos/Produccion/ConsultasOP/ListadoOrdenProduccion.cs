using ArenasProyect3.Modulos.Resourses;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text.pdf.codec.wmf;
using Org.BouncyCastle.Asn1.Mozilla;
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
using HorizontalAlignmentValues = DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues;

namespace ArenasProyect3.Modulos.Produccion.ConsultasOP
{
    public partial class ListadoOrdenProduccion : Form
    {
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        private Cursor curAnterior = null;
        int totalCantidades = 0;
        DataGridView dgvActivo = null;
        bool hayCheckActivo = false;

        //CONMSTRUCTOR DE MI FORMULARIO
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

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            cboBusqeuda.SelectedIndex = 0;
            VerificarDGVActivo();
        }

        //FUNCION PARA VERIFICAR SI HAY UNA CANTIDAD 
        public void MostrarCantidadesSegunOP(int idOrdenProduccion)
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
                cmd = new SqlCommand("OP_MostrarCantidades", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idOrdenProduccion", idOrdenProduccion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoCantidades.DataSource = dt;
                con.Close();
                datalistadoCantidades.Columns[0].Width = 40;
                datalistadoCantidades.Columns[1].Width = 120;
                datalistadoCantidades.Columns[2].Width = 100;

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
        public void MostrarCantidadesSegunOPCalidad(int idOrdenProduccion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("OP_MostrarCantidadesCalidad", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idOrdenProduccion", idOrdenProduccion);
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
                cmd = new SqlCommand("OP_MostrarSNC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idDetalleCantidadCalidad", idDetalleCantidadCalidad);
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
        public void MostrarJefaturaPro()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Reporte_MostrarJefeAreaProduccion", con);
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

        //FUNCION PARA MOSTRAR MI OT
        public void MostrarOTpRODUCCION(string CodigoOP)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("OP_MostrarOTOP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigoOP", CodigoOP);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoOrdenServicioOP.DataSource = dt;
                con.Close();
                txtOrdenTrabajoSNC.Text = datalistadoOrdenServicioOP.SelectedCells[0].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la jefatura " + ex.Message);
            }
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
        public void ColoresListado(DataGridView DGV)
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= DGV.RowCount - 1; i++)
                {
                    string estadoPro = DGV.Rows[i].Cells[16].Value.ToString();

                    if (estadoPro == "FUERA DE FECHA")
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Fuchsia;
                    }
                    else if (estadoPro == "LÍMITE")
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Orange;
                    }
                    else if (estadoPro == "PENDIENTE")
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else if (estadoPro == "CULMINADO")
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.DarkGreen;
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
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                        datalistadoTodasOP.DataSource = dt;
                        DataRow[] rowsEnProceso = dt.Select("ESTADO IN ('PENDIENTE', 'FUERA DE FECHA', 'LÍMITE')");
                        // Si hay filas, crea un nuevo DataTable, si no, usa una copia vacía del esquema.
                        DataTable dtEnProceso = rowsEnProceso.Any() ? rowsEnProceso.CopyToDataTable() : dt.Clone();
                        datalistadoEnProcesoOP.DataSource = dtEnProceso; // Asumiendo este es el nombre de tu DataGrid

                        // --- 2.3. OT Observadas (Asumimos que el estado "Observadas" es FUERA DE FECHA o LÍMITE) ---
                        DataRow[] rowsObservadas = dt.Select("[INDICADOR SNC] = 1");
                        DataTable dtObservadas = rowsObservadas.Any() ? rowsObservadas.CopyToDataTable() : dt.Clone();
                        datalistadoObservadas.DataSource = dtObservadas; // Asumiendo este es el nombre de tu DataGrid

                        RedimensionarListadoGeneralPedido(datalistadoTodasOP);
                        RedimensionarListadoGeneralPedido(datalistadoEnProcesoOP);
                        RedimensionarListadoOPCalidad(datalistadoObservadas);
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
        public void RedimensionarListadoGeneralPedido(DataGridView DGV)
        {
            //REDIEMNSION DE PEDIDOS
            DGV.Columns[2].Width = 95;
            DGV.Columns[3].Width = 80;
            DGV.Columns[4].Width = 80;
            DGV.Columns[5].Width = 250;
            DGV.Columns[6].Width = 130;
            DGV.Columns[7].Width = 40;
            DGV.Columns[8].Width = 370;
            DGV.Columns[9].Width = 60;
            DGV.Columns[10].Width = 89;
            DGV.Columns[11].Width = 79;
            DGV.Columns[14].Width = 75;
            DGV.Columns[16].Width = 110;
            DGV.Columns[18].Width = 55;
            ////SE HACE NO VISIBLE LAS COLUMNAS QUE NO LES INTERESA AL USUARIO
            DGV.Columns[1].Visible = false;
            DGV.Columns[12].Visible = false;
            DGV.Columns[13].Visible = false;
            DGV.Columns[15].Visible = false;
            DGV.Columns[17].Visible = false;
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
            ////SE BLOQUEA MI LISTADO
            DGV.Columns[2].ReadOnly = true;
            DGV.Columns[3].ReadOnly = true;
            DGV.Columns[4].ReadOnly = true;
            DGV.Columns[5].ReadOnly = true;
            DGV.Columns[6].ReadOnly = true;
            DGV.Columns[7].ReadOnly = true;
            DGV.Columns[8].ReadOnly = true;
            DGV.Columns[9].ReadOnly = true;
            DGV.Columns[10].ReadOnly = true;
            DGV.Columns[11].ReadOnly = true;
            DGV.Columns[14].ReadOnly = true;
            DGV.Columns[16].ReadOnly = true;
            DGV.Columns[18].ReadOnly = true;
        }

        //FUNCION PARA REDIMENSIONAR MIS LISTADOS
        public void RedimensionarListadoOPCalidad(DataGridView DGV)
        {
            //REDIEMNSION DE PEDIDOS
            DGV.Columns[2].Width = 95;
            DGV.Columns[3].Width = 80;
            DGV.Columns[4].Width = 80;
            DGV.Columns[5].Width = 250;
            DGV.Columns[6].Width = 130;
            DGV.Columns[7].Width = 40;
            DGV.Columns[8].Width = 370;
            DGV.Columns[9].Width = 60;
            DGV.Columns[10].Width = 89;
            DGV.Columns[11].Width = 79;
            DGV.Columns[14].Width = 75;
            DGV.Columns[16].Width = 110;
            DGV.Columns[17].Width = 110;
            ////SE HACE NO VISIBLE LAS COLUMNAS QUE NO LES INTERESA AL USUARIO
            DGV.Columns[1].Visible = false;
            DGV.Columns[12].Visible = false;
            DGV.Columns[13].Visible = false;
            DGV.Columns[15].Visible = false;
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
            ////SE BLOQUEA MI LISTADO
            DGV.Columns[2].ReadOnly = true;
            DGV.Columns[3].ReadOnly = true;
            DGV.Columns[4].ReadOnly = true;
            DGV.Columns[5].ReadOnly = true;
            DGV.Columns[6].ReadOnly = true;
            DGV.Columns[7].ReadOnly = true;
            DGV.Columns[8].ReadOnly = true;
            DGV.Columns[9].ReadOnly = true;
            DGV.Columns[10].ReadOnly = true;
            DGV.Columns[11].ReadOnly = true;
            DGV.Columns[14].ReadOnly = true;
            DGV.Columns[16].ReadOnly = true;
            DGV.Columns[17].ReadOnly = true;
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

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN
        private void datalistadoTodasOP_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            ModificarCursor(datalistadoTodasOP, "detallesTodos", e);
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN
        private void datalistadoEnProcesoOP_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            ModificarCursor(datalistadoEnProcesoOP, "detalles", e);
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN
        private void datalistadoObservadas_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            ModificarCursor(datalistadoObservadas, "detallesObs", e);
        }

        //MOSTRAR OP SEGUN LAS FECHAS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarOrdenProduccion(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OP SEGUN LAS FECHAS
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarOrdenProduccion(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OP SEGUN LAS FECHAS
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarOrdenProduccion(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OPRDENES PRODUCCION DEPENDIENTO LA OPCIÓN ESCOGIDA
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            string cliente = null;
            string codigoOT = null;
            string descripcion = null;
            string textoBusqueda = txtBusqueda.Text;

            if (cboBusqeuda.Text == "CÓDIGO OP")
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
            MostrarOrdenProduccion(
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

        //CARGAR MI PLANO DE PRODUCTO ASIGANDO A LA OP
        private void btnPlano_Click(object sender, EventArgs e)
        {
            EjecutarDocumento(dgvActivo.SelectedCells[20].Value.ToString());
        }

        //CARGAR MI OC TRAIDO DESDE MI PEDIDO
        private void btnOC_Click(object sender, EventArgs e)
        {
            EjecutarDocumento(dgvActivo.SelectedCells[19].Value.ToString());
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

        //EVENTO PARA ABRIR EL INGRESO DE CANTIDADES
        private void datalistadoTodasOP_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AbrirDetalles(datalistadoTodasOP);
        }

        //EVENTO PARA ABRIR EL INGRESO DE CANTIDADES
        private void datalistadoEnProcesoOP_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AbrirDetalles(datalistadoEnProcesoOP);
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

                txtOpsSeleccionadas.Text = Convert.ToString(count);
                //CARGA DE DAOTS
                txtCodigoOP.Text = DGV.SelectedCells[2].Value.ToString();
                int IdOrdenProduccion = Convert.ToInt32(DGV.SelectedCells[1].Value.ToString());
                txtDescripcionProducto.Text = DGV.SelectedCells[8].Value.ToString();
                txtCantidadTotalOP.Text = DGV.SelectedCells[9].Value.ToString();
                txtCantidadRequerida.Text = DGV.SelectedCells[9].Value.ToString();
                dtpFechaRealizada.Value = DateTime.Now;
                txtCantidadRealizada.Text = "";
                txtCantidadRestante.Text = "";
                MostrarCantidadesSegunOP(IdOrdenProduccion);
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
                    lblIdOP.Text = "Varios";
                    txtCantidadRestante.Text = "0";
                }
                else
                {
                    btnGuardarCantidad.Visible = true;
                    lblGuardarCantidad.Visible = true;
                    btnGenerarGuardarCantidades.Visible = false;
                    lblGenerarGuardarCantidades.Visible = false;
                    txtCantidadRealizada.ReadOnly = false;
                    lblIdOP.Text = DGV.SelectedCells[1].Value.ToString();
                }
                ColocarImagenesListadoCaantidadaesOP();
            }
            else
            {
                MessageBox.Show("Debe seleccionar una OP para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //EVENTO PARA ABRIR EL INGRESO DE LA SNC
        private void datalistadoObservadas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoObservadas.RowCount != 0)
            {
                btnVisualizarSNC.Visible = false;
                panelControlCalidad.Visible = true;

                lblIdOP.Text = datalistadoObservadas.SelectedCells[1].Value.ToString();
                txtCoidgoOPCalidad.Text = datalistadoObservadas.SelectedCells[2].Value.ToString();
                txtDescripcionProductoCalidad.Text = datalistadoObservadas.SelectedCells[8].Value.ToString();
                txtIdArea.Text = datalistadoObservadas.SelectedCells[28].Value.ToString();
                txtArea.Text = datalistadoObservadas.SelectedCells[29].Value.ToString();
                txtIdOP.Text = datalistadoObservadas.SelectedCells[1].Value.ToString();
                MostrarCantidadesSegunOPCalidad(Convert.ToInt32(lblIdOP.Text));
                btnGenerarCSM.Visible = false;
                lblGenerarCSM.Visible = false;
                datalistadoObservadas.Enabled = false;
            }
        }

        //GENERACION DE REPORTES
        private void btnGenerarOrdenProduccionPDF_Click(object sender, EventArgs e)
        {
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

        //EVENTO PARA GUARDAR MI S CANTIDADES INGRESADAS
        private async void btnGuardarCantidad_Click(object sender, EventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoEnProcesoOP.CurrentRow != null)
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
                            using (SqlConnection con = new SqlConnection(Conexion.ConexionMaestra.conexion))
                            using (SqlCommand cmd = new SqlCommand("OP_IngresarRegistroCantidad", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idOrdenProduccion", lblIdOP.Text);
                                cmd.Parameters.AddWithValue("@cantidad", txtCantidadRealizada.Text);
                                cmd.Parameters.AddWithValue("@fechaRegistro", Convert.ToDateTime(dtpFechaRealizada.Value));
                                await con.OpenAsync();
                                await cmd.ExecuteNonQueryAsync();
                            }

                            MessageBox.Show("Cantidd ingresada correctamente.", "Validación del Sistema", MessageBoxButtons.OK);
                            MostrarOrdenProduccion(DesdeFecha.Value, HastaFecha.Value);
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
                MessageBox.Show("Debe seleccionar una OP para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //EVENTO PARA GUARDAR VARIAS CANTIDADES INGRESADAS
        private void btnGenerarGuardarCantidades_Click(object sender, EventArgs e)
        {
            //List<int> idOPSeleccionada = new List<int>();
            //List<int> CantidadTotalOPSeleccionada = new List<int>();

            //foreach (DataGridViewRow row in datalistadoEnProcesoOP.Rows)
            //{
            //    DataGridViewCheckBoxCell checkBox = row.Cells[0] as DataGridViewCheckBoxCell;

            //    if (checkBox != null && Convert.ToBoolean(checkBox.Value) == true)
            //    {
            //        try
            //        {
            //            int idOp = Convert.ToInt32(row.Cells[1].Value.ToString());
            //            int cantidadEsperada = Convert.ToInt32(row.Cells[9].Value.ToString());
            //            int cantidadHecha = Convert.ToInt32(row.Cells[12].Value.ToString());
            //            int TotalCantidad = cantidadEsperada - cantidadHecha;

            //            if (TotalCantidad != 0)
            //            {
            //                SqlConnection con = new SqlConnection();
            //                SqlCommand cmd = new SqlCommand();
            //                con.ConnectionString = Conexion.ConexionMaestra.conexion;
            //                con.Open();
            //                cmd = new SqlCommand("OP_IngresarRegistroCantidad", con);
            //                cmd.CommandType = CommandType.StoredProcedure;
            //                cmd.Parameters.AddWithValue("@idOrdenProduccion", idOp);
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
            //MostrarOrdenProduccion(DesdeFecha.Value, HastaFecha.Value);
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
            txtOpsSeleccionadas.Text = "";
            txtCantidadRealizada.Text = "";
            txtCantidadRestante.Text = "";
        }

        //SALIR DEL CONTROL DE CALIDAD
        private void btnRegresarControl_Click(object sender, EventArgs e)
        {
            panelControlCalidad.Visible = false;
            datalistadoObservadas.Enabled = true;
            lblGenerarCSM.Visible = false;
            btnGenerarCSM.Visible = false;
            btnVisualizarSNC.Visible = false;
            lblLeyendaVisualizar.Visible = false;
        }

        //ANULACION DE MI OP - PEDIDO - COTIZACION---------------------------------------------------------------
        private void btnAnularOP_Click(object sender, EventArgs e)
        {
            LimpiarAnulacionPedido();
            panleAnulacion.Visible = true;
            datalistadoEnProcesoOP.Enabled = false;
        }

        //FUNCION PARA PROCEDER A ANULAR MI PEDIDO, COTIZACION Y PRODICCION
        private void btnProcederAnulacion_Click(object sender, EventArgs e)
        {

        }

        //BOTON PARA RETROCEDER DE LA ANULACION
        private void btnRetrocederAnulacion_Click(object sender, EventArgs e)
        {
            LimpiarAnulacionPedido();
            panleAnulacion.Visible = false;
            datalistadoEnProcesoOP.Enabled = true;
        }

        //FUNCION PARA LIMPIAR MIS CONTROLES ORIETADO A ANULACION DE PEDIDO
        public void LimpiarAnulacionPedido()
        {
            //datalistadoBuscarOPxPedidoAnulacion.Rows.Clear();
            //txtJustificacionAnulacion.Text = "";
        }
        //----------------------------------------------------------------------------------------------------------

        //VISUALIZAR IMAGEN 1
        private void btnImagen1_Click(object sender, EventArgs e)
        {
            CargarImagenes(lblImagen1);
        }

        //VISUALIZAR IMAGEN 2
        private void btnImagen2_Click(object sender, EventArgs e)
        {
            CargarImagenes(lblImagen2);
        }

        //VISUALIZAR IMAGEN 3
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

        //VISUALIZAR EL COMENTARIO HECHO POR CALIDAD Y LOS COLORES
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
                Visualizadores.VisualizarSNC frm = new Visualizadores.VisualizarSNC();
                frm.lblCodigo.Text = codigoDetalleCantidadCalidad;
                //CARGAR VENTANA
                frm.Show();
            }
        }

        //GENERAR CSM POR PARTE DE PRODUCCION
        private void btnGenerarCSM_Click(object sender, EventArgs e)
        {
            panelRevisionOP.Visible = true;
            panelControlCalidad.Visible = false;

            MostrarSNCCalidad(Convert.ToInt32(datalistadoHistorial.SelectedCells[1].Value.ToString()));
            txtReponsableRegistro.Text = datalistadoSNCDatos.SelectedCells[0].Value.ToString();
            txtAutoriza.Text = Program.NombreUsuarioCompleto;
            dtpFechaHallazgo.Value = Convert.ToDateTime(datalistadoSNCDatos.SelectedCells[1].Value.ToString());
            txtOrdenProduccionSNC.Text = txtCoidgoOPCalidad.Text;
            txtDescripcionSNC.Text = datalistadoSNCDatos.SelectedCells[2].Value.ToString();
            lblImagen1.Text = datalistadoSNCDatos.SelectedCells[5].Value.ToString();
            lblImagen2.Text = datalistadoSNCDatos.SelectedCells[6].Value.ToString();
            lblImagen3.Text = datalistadoSNCDatos.SelectedCells[7].Value.ToString();
            lblIdSNC.Text = datalistadoSNCDatos.SelectedCells[8].Value.ToString();
            txtCantidadObservada.Text = datalistadoHistorial.SelectedCells[3].Value.ToString();
            txtAreaSNC.Text = txtArea.Text;
            txtIdAreaSNC.Text = txtIdArea.Text;
            txtIdOPSNC.Text = txtIdOP.Text;
        }

        //CARGA DEL FACTOR DE FALLO
        private void txtAreaSNC_TextChanged(object sender, EventArgs e)
        {
            CargarTipoFallo(txtAreaSNC.Text);
        }

        //CARGAR TIPO DE FALLOR DEPENDIENDO EL AREA
        public void CargarTipoFallo(string are)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdTipoHallazgo, Nombre, AG.Descripcion AS [AREA] FROM TipoHallazgo TH INNER JOIN AreaGeneral AG ON AG.IdArea = TH.IdArea WHERE th.Estado = 1 AND AG.Descripcion = @area", con);
                comando.Parameters.AddWithValue("@area", are);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboFactorFallo.ValueMember = "IdTipoHallazgo";
                cboFactorFallo.DisplayMember = "Nombre";
                cboFactorFallo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
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

        //CERRAR EL PANEL DE GENERAR SNC
        private void btnCerrarSNC_Click(object sender, EventArgs e)
        {
            panelRevisionOP.Visible = false;
            panelControlCalidad.Visible = true;
            datalistadoObservadas.Enabled = false;
            LimpairCampos();
        }

        //CERRAR MI PANEL DE DETALLES DE EVALUACION
        private void lblCerrarDetallesEvaluacion_Click(object sender, EventArgs e)
        {
            panelControlCalidad.Visible = false;
            datalistadoObservadas.Enabled = true;
            lblGenerarCSM.Visible = false;
            btnGenerarCSM.Visible = false;
            btnVisualizarSNC.Visible = false;
            lblLeyendaVisualizar.Visible = false;
        }

        //GUARDAR LA SNC POR PARTE DEL PRODUCCION
        private void btnGuardarSNC_Click(object sender, EventArgs e)
        {
            if (txtOrdenProduccionSNC.Text == "" || txtDescripcionSNC.Text == "" || txtCausaSNC.Text == "" || txtAccionesTomadas.Text == "" || txtOportunidadMejora.Text == "" || ckLiberacion.Checked == true && txtRutaArchivo.Text == "")
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
                        cmd = new SqlCommand("OP_IngresarSNC", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idSNC", Convert.ToInt16(lblIdSNC.Text));
                        cmd.Parameters.AddWithValue("@idDetalleCantidadCalidad", Convert.ToInt16(datalistadoHistorial.SelectedCells[1].Value.ToString()));
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
                            string NombreGenerado = "OP " + txtOrdenProduccionSNC.Text + " SNC " + datalistadoHistorial.SelectedCells[1].Value.ToString();
                            string RutaOld = txtRutaArchivo.Text;
                            string RutaNew = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Calidad\SNC Evidencia\" + NombreGenerado + ".pdf";
                            File.Copy(RutaOld, RutaNew, true);
                            cmd.Parameters.AddWithValue("@rutaLiberacionEvi", RutaNew);
                        }

                        cmd.ExecuteNonQuery();
                        con.Close();

                        if (ckReproceso.Checked == true)
                        {
                            CambiarEstadoCalidad(txtOrdenProduccionSNC.Text, 2);
                        }

                        MessageBox.Show("Salida No Conforme registrada correctamente.", "Validación del Sistema", MessageBoxButtons.OK);
                        panelRevisionOP.Visible = false;
                        datalistadoObservadas.Enabled = true;
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
                GenerarOPAdicional();
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
        public void GenerarOPAdicional()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                SqlCommand cmd = new SqlCommand();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                cmd = new SqlCommand("OP_InsertarReposicion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Codigo_OP_Original", txtOrdenProduccionSNC.Text);
                cmd.Parameters.AddWithValue("@Nueva_Fecha_Entrega", dtpFinal.Value);
                cmd.Parameters.AddWithValue("@idCantCal", Convert.ToInt16(datalistadoHistorial.SelectedCells[1].Value.ToString()));
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Se generó la OP por reposición automaticamnete: OP " + txtOrdenProduccionSNC.Text, "Validación del Sistema", MessageBoxButtons.OK);

                MostrarOTpRODUCCION(txtOrdenProduccionSNC.Text);

                SqlConnection con2 = new SqlConnection();
                SqlCommand cmd2 = new SqlCommand();
                con2.ConnectionString = Conexion.ConexionMaestra.conexion;
                con2.Open();
                cmd2 = new SqlCommand("OT_InsertarReposicion", con2);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@Codigo_OT_Original", txtOrdenTrabajoSNC.Text);
                cmd2.Parameters.AddWithValue("@Nueva_Fecha_Entrega", dtpFinal.Value);
                cmd2.Parameters.AddWithValue("@idCantCal", DBNull.Value);
                cmd2.Parameters.AddWithValue("@cantidad", Convert.ToInt32(txtCantidadObservada.Text));
                cmd2.ExecuteNonQuery();
                con2.Close();
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
            MostrarJefaturaPro();
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
            cmd.Parameters.AddWithValue("@observaciones", "REQUERIMIENTO ADICIONAL PARA ORDEN DE PRODUCCION");
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
            cmd.Parameters.AddWithValue("@idOP", txtIdOPSNCRQ.Text);
            cmd.Parameters.AddWithValue("@idOT", 0);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Se generó el requerimiento para la OP " + txtCodigoOPReque.Text + ".", "Validación del Sistema", MessageBoxButtons.OK);

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
                cmd = new SqlCommand("OP_EstadoCalidad", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigoOP", codigoOP);
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
            }
            else
            {
                panelGeneracionRequeRepro.Visible = true;
                CargarDatos();
            }
        }

        //CARGAR DATOS DE MI GENERACION DE REQUERIMIENTO
        public void CargarDatos()
        {
            txtResponsableRegistroProduccion.Text = Program.NombreUsuarioCompleto;
            txtCodigoOPReque.Text = txtOrdenProduccionSNC.Text;
            txtCantidadObserbadaReque.Text = txtCantidadObservada.Text;
            txtCodigoFormulacionReque.Text = datalistadoObservadas.SelectedCells[30].Value.ToString();
            txtIdOPSNCRQ.Text = txtIdOPSNC.Text;
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
            cmd = new SqlCommand("OP_BuscarMaterialesFormulacion", con);
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
            panelRevisionOP.Visible = false;
            panelControlCalidad.Visible = true;
            datalistadoObservadas.Enabled = false;
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

        //----------------------------------------------------------------------------------------------------------
        //MODIFICAR MI FECHA DE ENTREGA DE MI ORDEN DE PRODUCCION
        private void btnModificarFecha_Click(object sender, EventArgs e)
        {
            if (dgvActivo.CurrentRow != null)
            {
                dgvActivo.Enabled = false;
                panelModiFechaEntrega.Visible = true;
                int IdOP = Convert.ToInt32(dgvActivo.SelectedCells[1].Value);
                txtModiCodigoOP.Text = dgvActivo.SelectedCells[2].Value.ToString();
                dtpModiFechaOP.Value = Convert.ToDateTime(dgvActivo.SelectedCells[3].Value);
                dtpModiFechaEntrega.Value = Convert.ToDateTime(dgvActivo.SelectedCells[4].Value);
                txtModiObservacionModiFecha.Text = "";
            }
        }

        //CONFIRMAR MI MODIFICACION DE FECHAS
        private void btnModiConfirmar_Click(object sender, EventArgs e)
        {
            if (dgvActivo.CurrentRow != null)
            {
                int idOrdenProduccion = Convert.ToInt32(dgvActivo.SelectedCells[1].Value.ToString());

                DialogResult boton = MessageBox.Show("¿Realmente desea modificar la fecha de entrega?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        SqlCommand cmd = new SqlCommand();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        cmd = new SqlCommand("OP_ModificarFecha", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idOrdenProduccion", idOrdenProduccion);
                        cmd.Parameters.AddWithValue("@fechaEntrega", dtpModiFechaEntrega.Value);
                        cmd.Parameters.AddWithValue("@observacion", txtModiObservacionModiFecha.Text);
                        cmd.ExecuteScalar();
                        con.Close();

                        MessageBox.Show("Se modificó la fecha de la orden de producción.", "Validación del Sistema", MessageBoxButtons.OK);
                        MostrarOrdenProduccion(DesdeFecha.Value, HastaFecha.Value);
                        dgvActivo.Enabled = true;
                        panelModiFechaEntrega.Visible = false;
                        txtModiObservacionModiFecha.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una orden de producción para poder editarla.", "Validación del Sistema",MessageBoxButtons.OK);
            }
        }

        //FUNCION PARA RETROCEDER MI MODIFICACION DE FECHA
        private void btnModiRetroceder_Click(object sender, EventArgs e)
        {
            txtModiObservacionModiFecha.Text = "";
            dgvActivo.Enabled = true;
            panelModiFechaEntrega.Visible = false;
        }
        //-----------------------------------------------------------------------------------------------------------

        //EVENETOS Y RECURSOS VARIOS--------------------------------------------------------------------------------
        //COLOREAR LISTADO
        private void datalistadoEnProcesoOP_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (datalistadoEnProcesoOP.RowCount != 0)
            {
                ColoresListado(datalistadoEnProcesoOP);
            }
        }

        //COLOREAR MI LIESTADO DE OT TODAS
        private void datalistadoTodasOP_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (datalistadoTodasOP.RowCount != 0)
            {
                ColoresListado(datalistadoTodasOP);
            }
        }

        //COLOREAR MI LIESTADO DE OT TODAS
        private void datalistadoObservadas_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (datalistadoObservadas.RowCount != 0)
            {
                ColoresListadoOPCalidad(datalistadoObservadas);
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
            if (TabControl.SelectedTab.Text == "OP en Proceso")
            {
                dgvActivo = datalistadoEnProcesoOP;
            }
            else if (TabControl.SelectedTab.Text == "Todas las OP")
            {
                dgvActivo = datalistadoTodasOP;
            }
            else if (TabControl.SelectedTab.Text == "OP Observadas")
            {
                dgvActivo = datalistadoObservadas;
            }
        }

        //CAMBIAR MI LISTADO
        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarDGVActivo();
            MostrarOrdenProduccion(DesdeFecha.Value, HastaFecha.Value);
        }

        //EVENTO PARA VALIDAR EL INGRESO DE NUMEROS Y SIGNOS
        private void txtCantidadRealizada_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, puntos, comas y teclas de control (como retroceso)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }
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

        //VIZUALIZAR DATOS EXCEL--------------------------------------------------------------------
        public void MostrarExcel()
        {
            datalistadoExcel.Rows.Clear();

            foreach (DataGridViewRow dgv in datalistadoEnProcesoOP.Rows)
            {
                string numeroOP = dgv.Cells[2].Value.ToString();
                DateTime fechaInicio = Convert.ToDateTime(dgv.Cells[3].Value.ToString()).Date;
                DateTime fechaFinal = Convert.ToDateTime(dgv.Cells[4].Value.ToString()).Date;
                string cliente = dgv.Cells[5].Value.ToString();
                string unidad = dgv.Cells[6].Value.ToString();
                string item = dgv.Cells[7].Value.ToString();
                string descripcionDescripcion = dgv.Cells[8].Value.ToString();
                string cantidad = dgv.Cells[9].Value.ToString();
                string color = dgv.Cells[10].Value.ToString();
                string numeroPedido = dgv.Cells[11].Value.ToString();
                string cantidadRealizada = dgv.Cells[14].Value.ToString();
                string estado = dgv.Cells[16].Value.ToString();
                string estadoOC = dgv.Cells[18].Value.ToString();
                //COLUMNAS EXTRAS DE MI REPORTE
                string fechaCulminacionV = dgv.Cells[25].Value.ToString();
                string fechaCulminacion;
                int diferenciasDias = 0;

                if (fechaCulminacionV == "SIN REGISTRO")
                {
                    fechaCulminacion = "SIN FECHA REGISTRADA";
                    diferenciasDias = 0;
                }
                else
                {
                    DateTime fechaCulminacionO = Convert.ToDateTime(fechaCulminacionV).Date;
                    diferenciasDias = (fechaCulminacionO - fechaFinal).Days;
                    fechaCulminacion = Convert.ToString(fechaCulminacionO);
                }

                string area = dgv.Cells[29].Value.ToString();

                datalistadoExcel.Rows.Add(new[] { numeroOP, Convert.ToString(fechaInicio), Convert.ToString(fechaFinal), Convert.ToString(fechaCulminacion), Convert.ToString(diferenciasDias), area, cliente, unidad, item, descripcionDescripcion, cantidad, color, numeroPedido, estado, cantidadRealizada, estado, estadoOC });
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
            sl.SetColumnWidth(4, 23);
            sl.SetColumnWidth(5, 17);
            sl.SetColumnWidth(6, 30);
            sl.SetColumnWidth(7, 50);
            sl.SetColumnWidth(8, 35);
            sl.SetColumnWidth(9, 10);
            sl.SetColumnWidth(10, 50);
            sl.SetColumnWidth(11, 15);
            sl.SetColumnWidth(12, 15);
            sl.SetColumnWidth(13, 15);
            sl.SetColumnWidth(14, 20);
            sl.SetColumnWidth(15, 20);
            sl.SetColumnWidth(16, 15);

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
                sl.SetCellValue(ir, 11, row.Cells[10].Value.ToString());
                sl.SetCellValue(ir, 12, row.Cells[11].Value.ToString());
                sl.SetCellValue(ir, 13, row.Cells[12].Value.ToString());
                sl.SetCellValue(ir, 14, row.Cells[13].Value.ToString());
                sl.SetCellValue(ir, 15, row.Cells[14].Value.ToString());
                sl.SetCellValue(ir, 16, row.Cells[15].Value.ToString());
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
                sl.SetCellStyle(ir, 11, styleC);
                sl.SetCellStyle(ir, 12, styleC);
                sl.SetCellStyle(ir, 13, styleC);
                sl.SetCellStyle(ir, 14, styleC);
                sl.SetCellStyle(ir, 15, styleC);
                sl.SetCellStyle(ir, 16, styleC);
                ir++;
            }

            string desde = DesdeFecha.Value.ToShortDateString();
            string desdeFormateada = desde.Replace("/", "-");
            string hasta = HastaFecha.Value.ToShortDateString();
            string hastaFormateada = hasta.Replace("/", "-");

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            sl.SaveAs(desktopPath + @"\Reporte de ordenes de producción del " + desdeFormateada + " al " + hastaFormateada + ".xlsx");
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

                rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeOrdenProduccion.rpt");

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
                int idOrdenProduccion = Convert.ToInt32(datalistadoEnProcesoOP.SelectedCells[1].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string codigoOrdenProduccion = datalistadoEnProcesoOP.SelectedCells[2].Value.ToString(); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string cliente = datalistadoEnProcesoOP.SelectedCells[5].Value.ToString(); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                string unidad = datalistadoEnProcesoOP.SelectedCells[6].Value.ToString(); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                crystalReport.SetParameterValue("@idOrdenProduccion", idOrdenProduccion);

                // Ruta de salida en el escritorio
                string rutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string rutaSalida = System.IO.Path.Combine(rutaEscritorio, "OP número " + codigoOrdenProduccion + " - " + cliente + " - " + unidad + ".pdf");

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
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
            datalistadoTodasOP.DataSource = null;
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO Y VER SI ESTAN VENCIDOS
        public void CargarColoresListadoOPGeneral(DataGridView DGV)
        {
            try
            {
                //VARIABLE DE FECHA
                var DateAndTime = DateTime.Now;
                //RECORRER MI LISTADO PARA VALIDAR MIS OPs, SI ESTAN VENCIDAS O NO
                foreach (DataGridViewRow datorecuperado in DGV.Rows)
                {
                    //RECUERAR LA FECHA Y EL CÓDIGO DE MI OP
                    DateTime fechaEntrega = Convert.ToDateTime(datorecuperado.Cells["FECHA DE ENTREGA"].Value);
                    int codigoOP = Convert.ToInt32(datorecuperado.Cells["ID"].Value);
                    string estadoOP = Convert.ToString(datorecuperado.Cells["ESTADO OP"].Value);

                    int cantidadEsperada = Convert.ToInt32(datorecuperado.Cells["CANTIDAD"].Value);
                    int cantidadRealizada = Convert.ToInt32(datorecuperado.Cells["CANTIDAD REALIZADA"].Value);

                    if (estadoOP != "ANULADO")
                    {
                        //SI LA FECHA DE VALIDEZ ES MAYOR A LA FECHA ACTUAL CONSULTADA
                        if (fechaEntrega == DateAndTime.Date)
                        {
                            //CAMBIAR EL ESTADO DE MI COTIZACIÓN
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("OP_CambiarEstado", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idOP", codigoOP);
                            cmd.Parameters.AddWithValue("@estadoOP", 2);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        else if (fechaEntrega < DateAndTime.Date)
                        {
                            //CAMBIAR EL ESTADO DE MI COTIZACIÓN
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("OP_CambiarEstado", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idOP", codigoOP);
                            cmd.Parameters.AddWithValue("@estadoOP", 3);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        else if (fechaEntrega > DateAndTime)
                        {
                            //CAMBIAR EL ESTADO DE MI COTIZACIÓN
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("OP_CambiarEstado", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idOP", codigoOP);
                            cmd.Parameters.AddWithValue("@estadoOP", 1);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }

                        if (cantidadEsperada == cantidadRealizada)
                        {
                            //CAMBIAR EL ESTADO DE MI OP
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("OP_CambiarEstado", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idOP", codigoOP);
                            cmd.Parameters.AddWithValue("@estadoOP", 4);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
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
                    string estadoOP = datalistadoHistorial.Rows[i].Cells[5].Value.ToString();

                    if (estadoOP == "APROBADO" || estadoOP == "SNC CULMINADA")
                    {
                        datalistadoHistorial.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                    }
                    else if (estadoOP == "DESAPROBADO")
                    {
                        datalistadoHistorial.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    else if (estadoOP == "SNC GENERADA")
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

        //LISTADO DE OP Y SELECCION DE PDF Y ESTADO DE OP---------------------
        //MOSTRAR OP AL INCIO 
        public void MostrarOrdenProduccionPorCriterios(DateTime fechaInicio, DateTime fechaTermino, string valorBusqueda)
        {
            try
            {
                if (cboBusqeuda.Text == "CÓDIGO OP")
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Calidad_MostrarPorCodigo", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                    cmd.Parameters.AddWithValue("@codigoOP", valorBusqueda);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoTodasOP.DataSource = dt;
                    con.Close();

                    System.Data.DataTable dt2 = new System.Data.DataTable();
                    SqlConnection con2 = new SqlConnection();
                    con2.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con2.Open();
                    SqlCommand cmd2 = new SqlCommand();
                    cmd2 = new SqlCommand("Calidad_MostrarPorCodigoCulminado", con2);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd2.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                    cmd2.Parameters.AddWithValue("@codigoOP", valorBusqueda);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    da2.Fill(dt2);
                    datalistadoEnProcesoOP.DataSource = dt2;
                    con2.Close();
                }
                else if (cboBusqeuda.Text == "CLIENTE")
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Calidad_MostrarPorCliente", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                    cmd.Parameters.AddWithValue("@cliente", valorBusqueda);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoTodasOP.DataSource = dt;
                    con.Close();

                    System.Data.DataTable dt2 = new System.Data.DataTable();
                    SqlConnection con2 = new SqlConnection();
                    con2.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con2.Open();
                    SqlCommand cmd2 = new SqlCommand();
                    cmd2 = new SqlCommand("Calidad_MostrarPorClienteCulminado", con2);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd2.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                    cmd2.Parameters.AddWithValue("@cliente", valorBusqueda);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    da2.Fill(dt2);
                    datalistadoEnProcesoOP.DataSource = dt2;
                    con2.Close();
                }
                else if (cboBusqeuda.Text == "DESCRIPCIÓN PRODUCTO")
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Calidad_MostrarPorDescripcion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                    cmd.Parameters.AddWithValue("@descripcion", valorBusqueda);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    datalistadoTodasOP.DataSource = dt;
                    con.Close();

                    System.Data.DataTable dt2 = new System.Data.DataTable();
                    SqlConnection con2 = new SqlConnection();
                    con2.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con2.Open();
                    SqlCommand cmd2 = new SqlCommand();
                    cmd2 = new SqlCommand("Calidad_MostrarPorDescripcionCulminado", con2);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd2.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                    cmd2.Parameters.AddWithValue("@descripcion", valorBusqueda);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    da2.Fill(dt2);
                    datalistadoEnProcesoOP.DataSource = dt2;
                    con2.Close();
                }
                RedimensionarListadoGeneralPedido(datalistadoTodasOP);
                RedimensionarListadoGeneralPedido(datalistadoEnProcesoOP);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            DGV.Columns[7].Width = 35;
            DGV.Columns[8].Width = 350;
            DGV.Columns[9].Width = 60;
            DGV.Columns[10].Width = 85;
            DGV.Columns[11].Width = 75;
            DGV.Columns[12].Width = 75;
            DGV.Columns[13].Width = 75;
            DGV.Columns[14].Width = 110;
            DGV.Columns[15].Width = 110;
            DGV.Columns[16].Width = 60;
            //SE HACE NO VISIBLE LAS COLUMNAS QUE NO LES INTERESA AL USUARIO
            DGV.Columns[1].Visible = false;
            DGV.Columns[17].Visible = false;
            DGV.Columns[18].Visible = false;
            DGV.Columns[19].Visible = false;
            DGV.Columns[20].Visible = false;
            DGV.Columns[21].Visible = false;
            DGV.Columns[22].Visible = false;
            DGV.Columns[23].Visible = false;
            DGV.Columns[24].Visible = false;
            DGV.Columns[25].Visible = false;
            //SE BLOQUEA MI LISTADO
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
            DGV.Columns[12].ReadOnly = true;
            DGV.Columns[13].ReadOnly = true;
            DGV.Columns[14].ReadOnly = true;
            CargarColoresListadoOPGeneral(DGV);
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN
        private void datalistadoTodasOP_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoTodasOP.Columns[e.ColumnIndex].Name == "detalles") { this.datalistadoTodasOP.Cursor = Cursors.Hand; }
            else { this.datalistadoTodasOP.Cursor = curAnterior; }
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN - HISTORIAL
        private void datalistadoHistorial_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoHistorial.Columns[e.ColumnIndex].Name == "columDesc") { this.datalistadoHistorial.Cursor = Cursors.Hand; }
            else { this.datalistadoHistorial.Cursor = curAnterior; }
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN
        private void datalistadoEnProcesoOP_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoEnProcesoOP.Columns[e.ColumnIndex].Name == "detallesCulminadas") { this.datalistadoEnProcesoOP.Cursor = Cursors.Hand; }
            else { this.datalistadoEnProcesoOP.Cursor = curAnterior; }
        }

        //ENTRARA A MIS DETALLES DE MI REVISION DE OP
        private void datalistadoTodasOP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoTodasOP.RowCount != 0)
            {
                DataGridViewColumn currentColumnT = datalistadoTodasOP.Columns[e.ColumnIndex];

                if (currentColumnT.Name == "detalles")
                {
                    VerificarDGVActivo();
                    MostrarCantidadesEntregadasOP();
                }
            }
        }

        private void datalistadoEnProcesoOP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoEnProcesoOP.RowCount != 0)
            {
                DataGridViewColumn currentColumnT = datalistadoEnProcesoOP.Columns[e.ColumnIndex];

                if (currentColumnT.Name == "detallesCulminadas")
                {
                    VerificarDGVActivo();
                    MostrarCantidadesEntregadasOP();
                }
            }
        }

        //ENTRARA A MIS DETALLES DE MI REVISION DE OP
        private void datalistadoTodasOP_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datalistadoTodasOP.RowCount != 0)
            {
                VerificarDGVActivo();
                MostrarCantidadesEntregadasOP();
            }
        }

        private void datalistadoEnProcesoOP_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            VerificarDGVActivo();
            MostrarCantidadesEntregadasOP();
        }

        //fFUNCION PARA EJECUTAR EL MOSTRAR CANTIDADES
        public void MostrarCantidadesEntregadasOP()
        {
            panelControlCalidad.Visible = true;
            btnVisualizar.Visible = false;
            lblLeyendaVisualizar.Visible = false;
            btnGenerarCSM.Visible = false;
            lblGenerarCSM.Visible = false;

            lblIdOP.Text = dgvActivo.SelectedCells[1].Value.ToString();
            txtCodigoOP.Text = dgvActivo.SelectedCells[2].Value.ToString();
            txtDescripcionProducto.Text = dgvActivo.SelectedCells[8].Value.ToString();
            txtCantidadTotalOP.Text = dgvActivo.SelectedCells[9].Value.ToString();
            txtCantidadEntregada.Text = dgvActivo.SelectedCells[12].Value.ToString();
            MostrarCantidadesSegunOP(Convert.ToInt32(lblIdOP.Text));
            lblCantidadRealizada.Text = dgvActivo.SelectedCells[13].Value.ToString();
            txtCantidadRestante.Text = Convert.ToString(Convert.ToInt32(txtCantidadEntregada.Text) - Convert.ToInt32(lblCantidadRealizada.Text));
            txtPesoTeorico.Text = "0.00";
            txtPesoReal.Text = "0.00";
            txtObservaciones.Text = "";
            btnGenerarCSM.Visible = false;
            lblGenerarCSM.Visible = false;
            CargarTipoHallazgo();
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
        private void btnCerrarDetallesOPCantidades_Click(object sender, EventArgs e)
        {
            PanelRevisioncantidades();
        }

        //CERRAR MI PANEL DE CONTROL DE CALIDAD
        private void btnRegresarControl_Click(object sender, EventArgs e)
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
                MostrarOrdenProduccionPorCriterios(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
                MostrarOrdenProduccionPorCriterios(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
            }
            else
            {
                MessageBox.Show("No se puede salir del control de calidad si hay una cantidad desaprobada, debe generar la SNG correspondiente.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //FUNCION PARA VALIDAR MIS ANTIDADES Y LOS ESTADOS DE ESTOS
        public void ValidarEstadoOP()
        {
            VerificarDesaprobados();
            VerificarSNG();
            VerificarSNGCulminada();

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

        //MOSTRAR OP SEGUN LAS FECHAS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarOrdenProduccionPorCriterios(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
            MostrarOrdenProduccionPorCriterios(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
        }

        //MOSTRAR OP SEGUN LAS FECHAS
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarOrdenProduccionPorCriterios(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
            MostrarOrdenProduccionPorCriterios(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
        }

        //MOSTRAR OP SEGUN LAS FECHAS
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarOrdenProduccionPorCriterios(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
            MostrarOrdenProduccionPorCriterios(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
        }

        //CARGAR DATOS CUANDO SE ACMBIA
        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostrarOrdenProduccionPorCriterios(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
            MostrarOrdenProduccionPorCriterios(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
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

        //MOSTRAR OPRDENES PRODUCCION DEPENDIENTO LA OPCIÓN ESCOGIDA
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            if (cboBusqeuda.Text == "CÓDIGO OP")
            {
                MostrarOrdenProduccionPorCriterios(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
                MostrarOrdenProduccionPorCriterios(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
            }
            else if (cboBusqeuda.Text == "CLIENTE")
            {
                MostrarOrdenProduccionPorCriterios(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
                MostrarOrdenProduccionPorCriterios(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
            }
            else if (cboBusqeuda.Text == "DESCRIPCIÓN PRODUCTO")
            {
                MostrarOrdenProduccionPorCriterios(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
                MostrarOrdenProduccionPorCriterios(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
            }
        }

        //GENERACION DE REPORTES
        private void btnGenerarOrdenProduccionPDF_Click(object sender, EventArgs e)
        {
            VerificarDGVActivo();
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

        //CARGAR MI PLANO DE PRODUCTO ASIGANDO A LA OP
        private void btnPlano_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(datalistadoTodasOP.SelectedCells[17].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Documento no encontrado, hubo un error al momento de cargar el archivo.", ex.Message);
            }
        }

        //CARGAR MI OC TRAIDO DESDE MI PEDIDO
        private void btnOC_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(datalistadoTodasOP.SelectedCells[16].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Documento no encontrado, hubo un error al momento de cargar el archivo.", ex.Message);
            }
        }

        //VERIFICAR SI MI DATAGRIDVIEW ESTA DESAPROBVADO
        public void VerificarSNG()
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
        }

        //VERIFICAR SI MI DATAGRIDVIEW ESTA SNC CULMINADA
        public void VerificarSNGCulminada()
        {
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
        }

        //VERIFICAR SI MI DATAGRIDVIEW ESTA DESAPROBVADO
        public void VerificarDesaprobados()
        {
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

        //GENERA UNA SALIDA NO CONFORME
        private void btnGenerarCSM_Click(object sender, EventArgs e)
        {
            panelSNC.Visible = true;
            txtReponsableRegistro.Text = Program.UnoNombreUnoApellidoUsuario;
            txtOrdenProduccionSNC.Text = txtCodigoOP.Text;
            LimpiarSNC();
        }

        //FUNCION PARA LIMPIAR MI SNC
        public void LimpiarSNC()
        {
            txtDescripcionSNC.Text = "";
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
                        else
                        {
                            cmd.Parameters.AddWithValue("@imagen1", "");
                        }

                        //SEGUNDA IMAGEN
                        if (txtImagen2.Text != "")
                        {
                            string nombreGenerado2 = "IMAGEN 2 OP " + txtOrdenProduccionSNC.Text + " - " + DateTime.Now.ToString("ddMMyyyyHHmmss");
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
                            string nombreGenerado3 = "IMAGEN 3 OP " + txtOrdenProduccionSNC.Text + " - " + DateTime.Now.ToString("ddMMyyyyHHmmss");
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
                        MostrarCantidadesSegunOP(Convert.ToInt16(lblIdOP.Text));
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

        //BOTON PARA SALIR DE MI SNC
        private void btnCerrarSNC_Click(object sender, EventArgs e)
        {
            panelSNC.Visible = false;
            LimpiarSNC();
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

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO OPs
        public void ColoresListadoOPCalidad(DataGridView DGV)
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= DGV.RowCount - 1; i++)
                {
                    if (DGV.Rows[i].Cells[15].Value.ToString() == "REVISIÓN PARCIAL")
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (DGV.Rows[i].Cells[15].Value.ToString() == "CULMINADA" || DGV.Rows[i].Cells[15].Value.ToString() == "CULMINADA - SNC")
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.DarkGreen;
                    }
                    else if (DGV.Rows[i].Cells[15].Value.ToString() == "ANULADO" || DGV.Rows[i].Cells[15].Value.ToString() == "NO DEFINIDO")
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

        //CERRAR MI PANEL DE OBSERVACIONES
        private void btnCerarDetallesObservacion_Click(object sender, EventArgs e)
        {
            panelDetallesObservacion.Visible = false;
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

        private void btnLimpiar2_Click(object sender, EventArgs e)
        {
            txtImagen2.Text = "";
        }

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
    }
}
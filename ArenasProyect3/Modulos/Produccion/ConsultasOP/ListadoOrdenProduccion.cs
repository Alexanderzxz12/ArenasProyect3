using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
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

        //CONMSTRUCTOR DE MI FORMULARIO
        public ListadoOrdenProduccion()
        {
            InitializeComponent();
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
                    string estadoOP = Convert.ToString(datorecuperado.Cells["ESTADO"].Value);

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

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO
        public void ColoresListado(DataGridView DGV)
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= DGV.RowCount - 1; i++)
                {
                    if (DGV.Rows[i].Cells[15].Value.ToString() == "FUERA DE FECHA")
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Fuchsia;
                    }
                    else if (DGV.Rows[i].Cells[15].Value.ToString() == "LÍMITE")
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Orange;
                    }
                    else if (DGV.Rows[i].Cells[15].Value.ToString() == "PENDIENTE")
                    {
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else if (DGV.Rows[i].Cells[15].Value.ToString() == "CULMINADO")
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
                string estado = dgv.Cells[15].Value.ToString();
                string estadoOC = dgv.Cells[16].Value.ToString();
                //COLUMNAS EXTRAS DE MI REPORTE
                string fechaCulminacionV = dgv.Cells[23].Value.ToString();
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

                string area = dgv.Cells[25].Value.ToString();

                datalistadoExcel.Rows.Add(new[] { numeroOP, Convert.ToString(fechaInicio), Convert.ToString(fechaFinal), Convert.ToString(fechaCulminacion), Convert.ToString(diferenciasDias), area, cliente, unidad, item, descripcionDescripcion, cantidad, color, numeroPedido, estado, cantidadRealizada, estado, estadoOC });
            }
        }

        //PRIMERA CARGA DE MI FORMULARIO
        private void ListadoOrdenProduccion_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            datalistadoEnProcesoOP.DataSource = null;
            cboBusqeuda.SelectedIndex = 0;

            //PREFILES Y PERSIMOS---------------------------------------------------------------
            if (Program.RangoEfecto != 1)
            {
                //btnAnularPedido.Visible = false;
                //lblAnularPedido.Visible = false;
            }
            //---------------------------------------------------------------------------------
        }

        //FUNCION PARA VERIFICAR SI HAY UNA CANTIDAD 
        public void MostrarCantidadesSegunOP(int idOrdenProduccion)
        {
            totalCantidades = 0;
            datalistadoCantidades.Columns.Clear();

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
        }

        //FUNCION PARA COLOCAR IMAGENES A MIS REGISTROS DE CANTIDADES
        public void ColocarImagenesListadoCaantidadaesOP()
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
                ColoresListadoCantidades();

                //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
                foreach (DataGridViewColumn column in datalistadoHistorial.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
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

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO DE CANTIDADES
        public void ColoresListadoCantidades()
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= datalistadoHistorial.RowCount - 1; i++)
                {
                    if (datalistadoHistorial.Rows[i].Cells[5].Value.ToString() == "APROBADO" || datalistadoHistorial.Rows[i].Cells[5].Value.ToString() == "SNC CULMINADA")
                    {
                        datalistadoHistorial.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Green;
                    }
                    else if (datalistadoHistorial.Rows[i].Cells[5].Value.ToString() == "DESAPROBADO")
                    {
                        datalistadoHistorial.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    else if (datalistadoHistorial.Rows[i].Cells[5].Value.ToString() == "SNC GENERADA")
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

            //deshabilitar el click y  reordenamiento por columnas
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //LISTADO DE OP Y SELECCION DE PDF Y ESTADO DE OP---------------------
        //MOSTRAR OP AL INCIO 
        public void MostrarOrdenProduccionPorFecha(DateTime fechaInicio, DateTime fechaTermino)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("OP_MostrarPorFecha", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoTodasOP.DataSource = dt;
            con.Close();

            DataTable dt2 = new DataTable();
            SqlConnection con2 = new SqlConnection();
            con2.ConnectionString = Conexion.ConexionMaestra.conexion;
            con2.Open();
            SqlCommand cmd2 = new SqlCommand();
            cmd2 = new SqlCommand("OP_MostrarPorFecha_EnProceso", con2);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd2.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt2);
            datalistadoEnProcesoOP.DataSource = dt2;
            con2.Close();

            DataTable dt3 = new DataTable();
            SqlConnection con3 = new SqlConnection();
            con3.ConnectionString = Conexion.ConexionMaestra.conexion;
            con3.Open();
            SqlCommand cmd3 = new SqlCommand();
            cmd3 = new SqlCommand("OP_MostrarPorFecha_Observadas", con3);
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd3.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
            da3.Fill(dt3);
            datalistadoObservadas.DataSource = dt3;
            con3.Close();

            RedimensionarListadoGeneralPedido(datalistadoTodasOP);
            RedimensionarListadoGeneralPedido(datalistadoEnProcesoOP);
            RedimensionarListadoOPCalidad(datalistadoObservadas);
        }

        //MOSTRAR OP POR CLIENTE
        public void MostrarOrdenProduccionPorCliente(DateTime fechaInicio, DateTime fechaTermino, string cliente)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("OP_MostrarPorCliente", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            cmd.Parameters.AddWithValue("@cliente", cliente);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoTodasOP.DataSource = dt;
            con.Close();

            DataTable dt2 = new DataTable();
            SqlConnection con2 = new SqlConnection();
            con2.ConnectionString = Conexion.ConexionMaestra.conexion;
            con2.Open();
            SqlCommand cmd2 = new SqlCommand();
            cmd2 = new SqlCommand("OP_MostrarPorCliente_EnProceso", con2);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd2.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            cmd2.Parameters.AddWithValue("@cliente", cliente);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt2);
            datalistadoEnProcesoOP.DataSource = dt2;
            con2.Close();

            DataTable dt3 = new DataTable();
            SqlConnection con3 = new SqlConnection();
            con3.ConnectionString = Conexion.ConexionMaestra.conexion;
            con3.Open();
            SqlCommand cmd3 = new SqlCommand();
            cmd3 = new SqlCommand("OP_MostrarPorCliente_Observadas", con3);
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd3.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            cmd3.Parameters.AddWithValue("@cliente", cliente);
            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
            da3.Fill(dt3);
            datalistadoObservadas.DataSource = dt3;
            con3.Close();

            RedimensionarListadoGeneralPedido(datalistadoTodasOP);
            RedimensionarListadoGeneralPedido(datalistadoEnProcesoOP);
            RedimensionarListadoOPCalidad(datalistadoObservadas);
        }

        //MOSTRAR OP POR CODIGO OP
        public void MostrarOrdenProduccionPorCodigoOP(DateTime fechaInicio, DateTime fechaTermino, string codigoOP)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("OP_MostrarPorCodigo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            cmd.Parameters.AddWithValue("@codigoOP", codigoOP);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoTodasOP.DataSource = dt;
            con.Close();

            DataTable dt2 = new DataTable();
            SqlConnection con2 = new SqlConnection();
            con2.ConnectionString = Conexion.ConexionMaestra.conexion;
            con2.Open();
            SqlCommand cmd2 = new SqlCommand();
            cmd2 = new SqlCommand("OP_MostrarPorCodigo_EnProceso", con2);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd2.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            cmd2.Parameters.AddWithValue("@codigoOP", codigoOP);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt2);
            datalistadoEnProcesoOP.DataSource = dt2;
            con2.Close();

            DataTable dt3 = new DataTable();
            SqlConnection con3 = new SqlConnection();
            con3.ConnectionString = Conexion.ConexionMaestra.conexion;
            con3.Open();
            SqlCommand cmd3 = new SqlCommand();
            cmd3 = new SqlCommand("OP_MostrarPorCodigo_Observada", con3);
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd3.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            cmd3.Parameters.AddWithValue("@codigoOP", codigoOP);
            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
            da3.Fill(dt3);
            datalistadoObservadas.DataSource = dt3;
            con3.Close();

            RedimensionarListadoGeneralPedido(datalistadoTodasOP);
            RedimensionarListadoGeneralPedido(datalistadoEnProcesoOP);
            RedimensionarListadoOPCalidad(datalistadoObservadas);
        }

        //MOSTRAR OP POR CODIGO OP
        public void MostrarOrdenProduccionPorDescripcion(DateTime fechaInicio, DateTime fechaTermino, string descripcipon)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("OP_MostrarPorDescripcion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            cmd.Parameters.AddWithValue("@descripcion", descripcipon);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoTodasOP.DataSource = dt;
            con.Close();

            DataTable dt2 = new DataTable();
            SqlConnection con2 = new SqlConnection();
            con2.ConnectionString = Conexion.ConexionMaestra.conexion;
            con2.Open();
            SqlCommand cmd2 = new SqlCommand();
            cmd2 = new SqlCommand("OP_MostrarPorDescripcion_EnProceso", con2);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd2.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            cmd.Parameters.AddWithValue("@descripcion", descripcipon);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt2);
            datalistadoEnProcesoOP.DataSource = dt2;
            con2.Close();

            DataTable dt3 = new DataTable();
            SqlConnection con3 = new SqlConnection();
            con3.ConnectionString = Conexion.ConexionMaestra.conexion;
            con3.Open();
            SqlCommand cmd3 = new SqlCommand();
            cmd3 = new SqlCommand("OP_MostrarPorDescripcion_Observada", con3);
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd3.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            cmd.Parameters.AddWithValue("@descripcion", descripcipon);
            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
            da3.Fill(dt3);
            datalistadoObservadas.DataSource = dt3;
            con3.Close();

            RedimensionarListadoGeneralPedido(datalistadoTodasOP);
            RedimensionarListadoGeneralPedido(datalistadoEnProcesoOP);
            RedimensionarListadoOPCalidad(datalistadoObservadas);
        }

        //FUNCION PARA REDIMENSIONAR MIS LISTADOS
        public void RedimensionarListadoGeneralPedido(DataGridView DGV)
        {
            //REDIEMNSION DE PEDIDOS
            DGV.Columns[2].Width = 95;
            DGV.Columns[3].Width = 80;
            DGV.Columns[4].Width = 80;
            DGV.Columns[5].Width = 300;
            DGV.Columns[6].Width = 130;
            DGV.Columns[7].Width = 40;
            DGV.Columns[8].Width = 300;
            DGV.Columns[9].Width = 60;
            DGV.Columns[10].Width = 85;
            DGV.Columns[11].Width = 75;
            DGV.Columns[14].Width = 75;
            DGV.Columns[15].Width = 110;
            DGV.Columns[16].Width = 65;
            //SE HACE NO VISIBLE LAS COLUMNAS QUE NO LES INTERESA AL USUARIO
            DGV.Columns[1].Visible = false;
            DGV.Columns[12].Visible = false;
            DGV.Columns[13].Visible = false;
            DGV.Columns[17].Visible = false;
            DGV.Columns[18].Visible = false;
            DGV.Columns[19].Visible = false;
            DGV.Columns[20].Visible = false;
            DGV.Columns[21].Visible = false;
            DGV.Columns[22].Visible = false;
            DGV.Columns[23].Visible = false;
            DGV.Columns[24].Visible = false;
            DGV.Columns[25].Visible = false;
            DGV.Columns[26].Visible = false;
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
            ColoresListado(DGV);

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //FUNCION PARA REDIMENSIONAR MIS LISTADOS
        public void RedimensionarListadoOPCalidad(DataGridView DGV)
        {
            //REDIEMNSION DE PEDIDOS
            DGV.Columns[2].Width = 95;
            DGV.Columns[3].Width = 80;
            DGV.Columns[4].Width = 80;
            DGV.Columns[5].Width = 300;
            DGV.Columns[6].Width = 130;
            DGV.Columns[7].Width = 40;
            DGV.Columns[8].Width = 300;
            DGV.Columns[9].Width = 60;
            DGV.Columns[10].Width = 85;
            DGV.Columns[11].Width = 75;
            DGV.Columns[14].Width = 75;
            DGV.Columns[15].Width = 110;
            DGV.Columns[16].Width = 110;
            DGV.Columns[17].Width = 65;
            //SE HACE NO VISIBLE LAS COLUMNAS QUE NO LES INTERESA AL USUARIO
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
            ColoresListadoOPCalidad();

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO OPs
        public void ColoresListadoOPCalidad()
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= datalistadoObservadas.RowCount - 1; i++)
                {
                    if (datalistadoObservadas.Rows[i].Cells[16].Value.ToString() == "REVISIÓN PARCIAL")
                    {
                        datalistadoObservadas.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                    }
                    else if (datalistadoObservadas.Rows[i].Cells[16].Value.ToString() == "CULMINADA" || datalistadoObservadas.Rows[i].Cells[16].Value.ToString() == "CULMINADA - SNC")
                    {
                        datalistadoObservadas.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.DarkGreen;
                    }
                    else if (datalistadoObservadas.Rows[i].Cells[16].Value.ToString() == "ANULADO" || datalistadoObservadas.Rows[i].Cells[16].Value.ToString() == "NO DEFINIDO")
                    {
                        datalistadoObservadas.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        datalistadoObservadas.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN
        private void datalistadoTodasOP_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (datalistadoTodasOP.Columns[e.ColumnIndex].Name == "detalles")
            {
                datalistadoTodasOP.Cursor = Cursors.Hand;
            }
            else
            {
                datalistadoTodasOP.Cursor = curAnterior;
            }
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN
        private void datalistadoEnProcesoOP_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (datalistadoEnProcesoOP.Columns[e.ColumnIndex].Name == "detalles")
            {
                datalistadoEnProcesoOP.Cursor = Cursors.Hand;
            }
            else
            {
                datalistadoEnProcesoOP.Cursor = curAnterior;
            }
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN
        private void datalistadoObservadas_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (datalistadoObservadas.Columns[e.ColumnIndex].Name == "detalles")
            {
                datalistadoObservadas.Cursor = Cursors.Hand;
            }
            else
            {
                datalistadoObservadas.Cursor = curAnterior;
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
                txtArea.Text = datalistadoObservadas.SelectedCells[26].Value.ToString();
                MostrarCantidadesSegunOPCalidad(Convert.ToInt32(lblIdOP.Text));
                btnGenerarCSM.Visible = false;
                lblGenerarCSM.Visible = false;
                datalistadoObservadas.Enabled = false;
            }
        }

        //VISUALIZAR EL COMENTARIO HECHO POR CALIDAD Y LOS COLORES
        private void datalistadoHistorial_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lblGenerarCSM.Visible = false;
            btnGenerarCSM.Visible = false;
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
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoHistorial.Columns[e.ColumnIndex].Name == "columDesc")
            {
                this.datalistadoHistorial.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoHistorial.Cursor = curAnterior;
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
        }

        //VISUALIZAR IMAGEN 1
        private void btnImagen1_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblImagen1.Text == "***" || lblImagen1.Text == "")
                {
                    MessageBox.Show("No hay ninguna imagen para mostrar.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    Process.Start(lblImagen1.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de carga." + ex, "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //VISUALIZAR IMAGEN 2
        private void btnImagen2_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblImagen2.Text == "***" || lblImagen2.Text == "")
                {
                    MessageBox.Show("No hay ninguna imagen para mostrar.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    Process.Start(lblImagen2.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de carga." + ex, "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //VISUALIZAR IMAGEN 3
        private void btnImagen3_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblImagen3.Text == "***" || lblImagen3.Text == "")
                {
                    MessageBox.Show("No hay ninguna imagen para mostrar.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    Process.Start(lblImagen3.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de carga." + ex, "Validación del Sistema", MessageBoxButtons.OK);
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
            }
            else
            {
                btnInfoMaterialesRepro.Visible = false;
            }
        }

        //ABIRIR PABNEL DE MATERIALES
        private void btnInfoMaterialesRepro_Click(object sender, EventArgs e)
        {
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
            txtCodigoFormulacionReque.Text = datalistadoObservadas.SelectedCells[27].Value.ToString();
            BuscarMaterialesFormulacion(txtCodigoFormulacionReque.Text);
        }

        //LIMPIAR MI RUTA DE ARCHVO
        private void btnLimpiarArchivo_Click(object sender, EventArgs e)
        {
            txtRutaArchivo.Text = "";
        }

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
                        cmd.Parameters.AddWithValue("@correcion", ckLiberacion.Checked ? 1 : 0);
                        //------------------------------
                        cmd.Parameters.AddWithValue("@reproceso", ckLiberacion.Checked ? 1 : 0);
                        //------------------------------
                        cmd.Parameters.AddWithValue("@reclasificacion", ckLiberacion.Checked ? 1 : 0);
                        //------------------------------
                        cmd.Parameters.AddWithValue("@recuperacion", ckLiberacion.Checked ? 1 : 0);
                        //------------------------------
                        cmd.Parameters.AddWithValue("@destruccion", ckLiberacion.Checked ? 1 : 0);
                        //------------------------------
                        cmd.Parameters.AddWithValue("@otros", ckLiberacion.Checked ? 1 : 0);

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
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            if (ckReproceso.Checked == true)
            {

            }

            if (ckReposicion.Checked == true)
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            LimpairCampos();
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

        //CHECKBOX OTROS
        private void ckOtros_CheckedChanged(object sender, EventArgs e)
        {
            if (ckOtros.Checked == true)
            {
                txtDescripcionOtros.ReadOnly = false;
                txtDescripcionOtros.Text = "";
            }
            else
            {
                txtDescripcionOtros.ReadOnly = true;
                txtDescripcionOtros.Text = "";
            }
        }

        //SALIR DEL COMETARIO DE CALIDAD
        private void btnCerarDetallesObservacion_Click(object sender, EventArgs e)
        {
            panelDetallesObservacion.Visible = false;
        }

        //SALIR DEL CONTROL DE CALIDAD
        private void btnRegresarControl_Click(object sender, EventArgs e)
        {
            panelControlCalidad.Visible = false;
            datalistadoObservadas.Enabled = true;
        }

        //CERRAR EL PANEL DE GENERAR SNC
        private void lblCerrarSNC_Click(object sender, EventArgs e)
        {
            panelRevisionOP.Visible = false;
            panelControlCalidad.Visible = true;
            datalistadoObservadas.Enabled = false;
            LimpairCampos();
        }

        //CERRAR EL PANEL DE GENERAR SNC
        private void btnCerrarSNC_Click(object sender, EventArgs e)
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
                txtDescripcionProducto.Text = DGV.SelectedCells[5].Value.ToString();
                txtCantidadTotalOP.Text = DGV.SelectedCells[9].Value.ToString();
                txtCantidadRequerida.Text = DGV.SelectedCells[9].Value.ToString();
                dtpFechaRealizada.Value = DateTime.Now;
                txtCantidadRealizada.Text = "";
                txtCantidadRestante.Text = "";
                MostrarCantidadesSegunOP(IdOrdenProduccion);
                lblCantidadTotalInghresada.Text = Convert.ToString(totalCantidades);
                txtCantidadRestante.Text = Convert.ToString(Convert.ToInt32(txtCantidadRequerida.Text) - Convert.ToInt32(lblCantidadTotalInghresada.Text));

                //if (txtCantidadRestante.Text == "0")
                //{
                //    DGV.Enabled = true;
                //    panelIngresoCantidades.Visible = false;
                //    MessageBox.Show("Esta OP ya culminó satisfactoriamente.", "Validación del Sistema", MessageBoxButtons.OK);
                //}
                //else
                //{
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
                //}

                ColocarImagenesListadoCaantidadaesOP();
            }
            else
            {
                MessageBox.Show("Debe seleccionar una OP para poder continuar.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //MOSTRAR OP SEGUN LAS FECHAS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
            MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OP SEGUN LAS FECHAS
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
            MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OP SEGUN LAS FECHAS
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
            MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OPRDENES PRODUCCION DEPENDIENTO LA OPCIÓN ESCOGIDA
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            if (cboBusqeuda.Text == "CÓDIGO OP")
            {
                MostrarOrdenProduccionPorCodigoOP(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
                MostrarOrdenProduccionPorCodigoOP(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
            }
            else if (cboBusqeuda.Text == "CLIENTE")
            {
                MostrarOrdenProduccionPorCliente(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
                MostrarOrdenProduccionPorCliente(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
            }
            else if (cboBusqeuda.Text == "DESCRIPCIÓN PRODUCTO")
            {
                MostrarOrdenProduccionPorDescripcion(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
                MostrarOrdenProduccionPorDescripcion(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
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
                Process.Start(datalistadoEnProcesoOP.SelectedCells[16].Value.ToString());
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
                Process.Start(datalistadoEnProcesoOP.SelectedCells[15].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Documento no encontrado, hubo un error al momento de cargar el archivo.", ex.Message);
            }
        }

        //EVENTO PARA GUARDAR MI S CANTIDADES INGRESADAS
        private void btnGuardarCantidad_Click(object sender, EventArgs e)
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
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("OP_IngresarRegistroCantidad", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idOrdenProduccion", lblIdOP.Text);
                            cmd.Parameters.AddWithValue("@cantidad", txtCantidadRealizada.Text);
                            cmd.Parameters.AddWithValue("@fechaRegistro", Convert.ToDateTime(dtpFechaRealizada.Value));
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Cantidd ingresada correctamente.", "Validación del Sistema", MessageBoxButtons.OK);
                            MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
                            MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
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
                MessageBox.Show("Debe seleccionar una OP para poder continuar.", "Validación del Sistema");
            }
        }

        //EVENTO PARA GUARDAR VARIAS CANTIDADES INGRESADAS
        private void btnGenerarGuardarCantidades_Click(object sender, EventArgs e)
        {
            List<int> idOPSeleccionada = new List<int>();
            List<int> CantidadTotalOPSeleccionada = new List<int>();

            foreach (DataGridViewRow row in datalistadoEnProcesoOP.Rows)
            {
                DataGridViewCheckBoxCell checkBox = row.Cells[0] as DataGridViewCheckBoxCell;

                if (checkBox != null && Convert.ToBoolean(checkBox.Value) == true)
                {
                    try
                    {
                        int idOp = Convert.ToInt32(row.Cells[1].Value.ToString());
                        int cantidadEsperada = Convert.ToInt32(row.Cells[9].Value.ToString());
                        int cantidadHecha = Convert.ToInt32(row.Cells[12].Value.ToString());
                        int TotalCantidad = cantidadEsperada - cantidadHecha;

                        if (TotalCantidad != 0)
                        {
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("OP_IngresarRegistroCantidad", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idOrdenProduccion", idOp);
                            cmd.Parameters.AddWithValue("@cantidad", TotalCantidad);
                            cmd.Parameters.AddWithValue("@fechaRegistro", Convert.ToDateTime(dtpFechaRealizada.Value));
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            MessageBox.Show("Operación terminada.", "Validación del Sistema");
            MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
            LimpiarCantidades();
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
            datalistadoEnProcesoOP.Enabled = true;
            datalistadoTodasOP.Enabled = true;
            datalistadoObservadas.Enabled = true;
            panelIngresoCantidades.Visible = false;
            txtOpsSeleccionadas.Text = "";
            txtCantidadRealizada.Text = "";
            txtCantidadRestante.Text = "";
        }

        //ANULACION DE MI OP - PEDIDO - COTIZACION
        private void btnAnularOP_Click(object sender, EventArgs e)
        {
            LimpiarAnulacionPedido();
            panleAnulacion.Visible = true;
            datalistadoEnProcesoOP.Enabled = false;
        }

        //FUNCION PARA PROCEDER A ANULAR MI PEDIDO, COTIZACION Y PRODICCION
        private void btnProcederAnulacion_Click(object sender, EventArgs e)
        {
            //if (datalistadoTodasPedido.CurrentRow != null)
            //{
            //    int idOrdenProduccion = Convert.ToInt32(datalistadoTodasPedido.SelectedCells[1].Value.ToString());
            //    int idPedido = Convert.ToInt32(datalistadoTodasPedido.SelectedCells[1].Value.ToString());
            //    string idCotizacion = datalistadoTodasPedido.SelectedCells[13].Value.ToString();

            //    VerificarOPxPedidoAnulacion(idPedido);

            //    if (datalistadoBuscarOPxPedidoAnulacion.RowCount > 0)
            //    {
            //        ordenProduccion = datalistadoBuscarOPxPedidoAnulacion.RowCount;
            //    }

            //    DialogResult boton = MessageBox.Show("¿Realmente desea anular esta orden de producción?. Se anulará la cotización y pedido asociada ha esta orden de producción.", "Validación del Sistema", MessageBoxButtons.OKCancel);
            //    if (boton == DialogResult.OK)
            //    {
            //        if (ordenProduccion > 0)
            //        {
            //            MessageBox.Show("El pedido que desea anular ya tiene una orden de producción generada.", "Validación del Sistema", MessageBoxButtons.OK);
            //        }
            //        else
            //        {
            //            try
            //            {
            //                SqlConnection con = new SqlConnection();
            //                SqlCommand cmd = new SqlCommand();
            //                con.ConnectionString = Conexion.ConexionMaestra.conexion;
            //                con.Open();
            //                cmd = new SqlCommand("AnularPedido", con);
            //                cmd.CommandType = CommandType.StoredProcedure;
            //                cmd.Parameters.AddWithValue("@idOrdenProduccion", idPedido);
            //                cmd.Parameters.AddWithValue("@idPedido", idPedido);
            //                cmd.Parameters.AddWithValue("@idCotizacion", idCotizacion);
            //                cmd.Parameters.AddWithValue("@mensajeAnulado", txtJustificacionAnulacion.Text);
            //                cmd.ExecuteNonQuery();
            //                con.Close();

            //                MessageBox.Show("Cotización, pedido y orden de producción asociado a esta, anuladas exitosamente.", "Validación del Sistema");
            //                MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
            //                LimpiarAnulacionPedido();
            //                panleAnulacion.Visible = false;
            //                datalistadoTodasOP.Enabled = true;
            //            }
            //            catch (Exception ex)
            //            {
            //                MessageBox.Show(ex.Message);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Debe seleccionar una orden de producción para poder anularlo.", "Validación del Sistema");
            //}
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
        //MODIFICAR MI FECHA DE ENTREGA DE MI ORDEN DE PRODUCCION
        private void btnModificarFecha_Click(object sender, EventArgs e)
        {
            if (datalistadoEnProcesoOP.CurrentRow != null)
            {
                datalistadoEnProcesoOP.Enabled = false;
                panelModiFechaEntrega.Visible = true;
                int IdOP = Convert.ToInt32(datalistadoEnProcesoOP.SelectedCells[1].Value);
                txtModiCodigoOP.Text = datalistadoEnProcesoOP.SelectedCells[2].Value.ToString();
                dtpModiFechaOP.Value = Convert.ToDateTime(datalistadoEnProcesoOP.SelectedCells[3].Value);
                dtpModiFechaEntrega.Value = Convert.ToDateTime(datalistadoEnProcesoOP.SelectedCells[4].Value);
                txtModiObservacionModiFecha.Text = "";
            }
        }

        //CONFIRMAR MI MODIFICACION DE FECHAS
        private void btnModiConfirmar_Click(object sender, EventArgs e)
        {
            if (datalistadoEnProcesoOP.CurrentRow != null)
            {
                int idOrdenProduccion = Convert.ToInt32(datalistadoEnProcesoOP.SelectedCells[1].Value.ToString());

                DialogResult boton = MessageBox.Show("¿Realmente desea modificar esta fech" +
                    "+36" +
                    "+a de orden de producción?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
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
                        string mensaje = cmd.ExecuteScalar()?.ToString();
                        cmd.ExecuteScalar();
                        con.Close();

                        if (mensaje == "")
                        {
                            MessageBox.Show("Fecha de entrega de mi orden de producción modificada exitosamente.", "Validación del Sistema");
                        }
                        else
                        {
                            MessageBox.Show(mensaje, "Validación del Sistema");
                        }

                        MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
                        MostrarOrdenProduccionPorFecha(DesdeFecha.Value, HastaFecha.Value);
                        datalistadoEnProcesoOP.Enabled = true;
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
                MessageBox.Show("Debe seleccionar una orden de producción para poder editarla.", "Validación del Sistema");
            }
        }

        //FUNCION PARA RETROCEDER MI MODIFICACION DE FECHA
        private void btnModiRetroceder_Click(object sender, EventArgs e)
        {
            txtModiObservacionModiFecha.Text = "";
            datalistadoEnProcesoOP.Enabled = true;
            panelModiFechaEntrega.Visible = false;
        }

        //-----------------------------------------------------------------------------------------------------------

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

        //EVENTO PARA VALIDAR EL INGRESO DE NUMEROS Y SIGNOS
        private void txtCantidadRealizada_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, puntos, comas y teclas de control (como retroceso)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }
        }

        //CAMBIAR MI LISTADO
        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            ColoresListado(datalistadoEnProcesoOP);
            ColoresListado(datalistadoTodasOP);
            RedimensionarListadoOPCalidad(datalistadoObservadas);
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

        //CARGA DEL FACTOR DE FALLO
        private void txtAreaSNC_TextChanged(object sender, EventArgs e)
        {
            cboFactorFallo.Items.Clear();

            if (txtAreaSNC.Text == "MODULOS CERAMICOS")
            {
                cboFactorFallo.Items.AddRange(new string[] { "MEDIDAS", "COLOR", "DUREZA", "DISTANCIA ENTRE PINES", "POSICIÓN DE ABERTURA" });
            }
            else if (txtAreaSNC.Text == "MALLAS DE ACERO")
            {
                cboFactorFallo.Items.AddRange(new string[] { "MEDIDAS", "COLOR", "TIPO DE PLIEGUE", "TIPO DE ALAMBRE", "POSICIÓN" });
            }
            else if (txtAreaSNC.Text == "PANELES Y PIEZAS DE POLIURETANO")
            {
                cboFactorFallo.Items.AddRange(new string[] { "MEDIDAS", "COLOR", "TIPO DE PLIEGUE", "TIPO DE ALAMBRE", "POSICIÓN" });
            }
            cboFactorFallo.SelectedIndex = 0;
        }

        //LIMPIAR CUANDO SE CAMBIA DE TIPO DE ARCHIVO
        private void cboTipoArchivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtRutaArchivo.Text = "";
        }

        //CERRAR MI PANEL DE GENERACION DE REQUERIMIENTO
        private void btnCerrarGeneracionReque_Click(object sender, EventArgs e)
        {
            panelGeneracionRequeRepro.Visible = false;
        }

        //CERRAR MI PANEL DE DETALLES DE EVALUACION
        private void lblCerrarDetallesEvaluacion_Click(object sender, EventArgs e)
        {
            panelControlCalidad.Visible = false;
            datalistadoObservadas.Enabled = true;
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
    }
}
using ArenasProyect3.Modulos.ManGeneral;
using ArenasProyect3.Modulos.Resourses;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DocumentFormat.OpenXml;
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

namespace ArenasProyect3.Modulos.Comercial.RequerimientosVentas
{
    public partial class ListadoActas : Form
    {
        //VARIABLES GLOBALES PARA MIS ACTAS DE VISITA
        private Cursor curAnterior = null;
        string ruta = ManGeneral.Manual.manualAreaComercial;

        //CONSTRUCTOR DEL MANTENIMIENTO - ACTAS DE VISITA
        public ListadoActas()
        {
            InitializeComponent();
        }

        //INICIO Y CARGA INICIAL DE ACTAS DE VISITA - CONSTRUCTOR--------------------------------------------------------------------------------------
        private void ListadoActas_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            datalistadoTodasActas.DataSource = null;

            //PREFILES Y PERSIMOS---------------------------------------------------------------
            if (Program.RangoEfecto != 1)
            {
                btnAprobarActa.Visible = false;
                btnDesaprobarActa.Visible = false;
                lblAproarActa.Visible = false;
                lblDesaprobarActa.Visible = false;
            }
            //---------------------------------------------------------------------------------
        }

        //VIZUALIZAR DATOS EXCEL--------------------------------------------------------------------
        public void MostrarExcel()
        {
            datalistadoExcel.Rows.Clear();

            foreach (DataGridViewRow dgv in datalistadoTodasActas.Rows)
            {
                string numeroActa = dgv.Cells[1].Value.ToString();
                string numeroLiqui = dgv.Cells[2].Value.ToString();
                string validado = dgv.Cells[4].Value.ToString();
                string fechaInicio = dgv.Cells[5].Value.ToString();
                string fechaTermino = dgv.Cells[6].Value.ToString();
                string cliente = dgv.Cells[8].Value.ToString();
                string unidad = dgv.Cells[10].Value.ToString();
                string responsable = dgv.Cells[12].Value.ToString();
                string estado = dgv.Cells[13].Value.ToString();

                datalistadoExcel.Rows.Add(new[] { numeroActa, numeroLiqui, validado, fechaInicio, fechaTermino, cliente, unidad, responsable, estado });
            }
        }

        //LISTADO DE ACTAS Y SELECCION DE PDF Y ESTADO DE ACTAS---------------------
        //MOSTRAR ACTAS AL INCIO 
        public void MostrarActasPorFecha(DateTime fechaInicio, DateTime fechaTermino)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarActasPorFecha_Jefatura", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoTodasActas.DataSource = dt;
                con.Close();
                RedimensionarListadoGeneralActas(datalistadoTodasActas);
            }
            catch(Exception ex)
            {                
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 6, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //MOSTRAR ACTAS POR RESPONSABLE
        public void MostrarActasResponsable(string resopnsable, DateTime fechaInicio, DateTime fechaTermino)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarActasPorResponsable", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@responsable", resopnsable);
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoTodasActas.DataSource = dt;
                con.Close();
                RedimensionarListadoGeneralActas(datalistadoTodasActas);
            }
            catch(Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 6, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //MOSTRAR ACTAS POR CLIENTE
        public void MostrarActasCliente(string cliente, DateTime fechaInicio, DateTime fechaTermino)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarActasPorCliente", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cliente", cliente);
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoTodasActas.DataSource = dt;
                con.Close();
                RedimensionarListadoGeneralActas(datalistadoTodasActas);
            }
            catch(Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 6, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //FUNCION PARA REDIMENSIONAR MIS LISTADOS
        public void RedimensionarListadoGeneralActas(DataGridView DGV)
        {
            //NO MOSTRAR LAS COLUMNAS QUE NO SEAN DE REELEVANCIA PARA EL USUARIO
            DGV.Columns[3].Visible = false;
            DGV.Columns[7].Visible = false;
            DGV.Columns[9].Visible = false;
            DGV.Columns[11].Visible = false;
            DGV.Columns[14].Visible = false;
            DGV.Columns[15].Visible = false;
            //BLOQUEAR LAS COLUMNAS Y HACERLAS DE SOLO LECTURA
            DGV.Columns[1].ReadOnly = true;
            DGV.Columns[2].ReadOnly = true;
            DGV.Columns[5].ReadOnly = true;
            DGV.Columns[6].ReadOnly = true;
            DGV.Columns[8].ReadOnly = true;
            DGV.Columns[10].ReadOnly = true;
            DGV.Columns[12].ReadOnly = true;
            DGV.Columns[13].ReadOnly = true;
            //REDIMENSIONAR LAS COLUMNAS SEGUN EL TEMAÑO REQUERIDO
            DGV.Columns[1].Width = 55;
            DGV.Columns[2].Width = 55;
            DGV.Columns[4].Width = 65;
            DGV.Columns[5].Width = 90;
            DGV.Columns[6].Width = 90;
            DGV.Columns[8].Width = 350;
            DGV.Columns[10].Width = 150;
            DGV.Columns[12].Width = 198;
            DGV.Columns[13].Width = 90;
            //CARGAR EL MÉTODO QUE COLOREA LAS FILAS
            ColoresListado(DGV);

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //COLOREAR REGISTROS
        public void ColoresListado(DataGridView DGV)
        {
            try
            {
                for (var i = 0; i <= DGV.RowCount - 1; i++)
                {
                    if (DGV.Rows[i].Cells[13].Value.ToString() == "PENDIENTE")
                    {
                        //PENDIENTE
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                    else if (DGV.Rows[i].Cells[13].Value.ToString() == "APROBADO")
                    {
                        //APROBADO
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                    }
                    else if (DGV.Rows[i].Cells[13].Value.ToString() == "ANULADO")
                    {
                        //DESAPROBADO
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        //CULMINADO
                        DGV.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                    }
                }
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 6, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN DE GENERACIÓN DEL PDF
        private void datalistadoTodasActas_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoTodasActas.Columns[e.ColumnIndex].Name == "btnGenerarPdf")
            {
                this.datalistadoTodasActas.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoTodasActas.Cursor = curAnterior;
            }
        }

        //MOSTRAR ACTAS DE VISITA POR RESPONSABLE
        private void txtBusquedaResponsable_TextChanged(object sender, EventArgs e)
        {
            MostrarActasResponsable(txtBusquedaResponsable.Text, DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR ACTAS DE VISITA POR CLIENTES
        private void txtBusquedaCLiente_TextChanged(object sender, EventArgs e)
        {
            MostrarActasCliente(txtBusquedaCLiente.Text, DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR ACTAS DE VISITA POR FECHAS
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarActasPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR ACTAS DE VISITA POR FECHAS
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarActasPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR ACTAS DE VISITA POR FECHAS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarActasPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //GENERACION DE REPORTES
        private void btnVerActa_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasActas.CurrentRow != null)
            {
                try
                {
                    string codigoActaReporte = "";

                    if (datalistadoTodasActas.SelectedCells[13].Value.ToString() == "PENDIENTE" || datalistadoTodasActas.SelectedCells[13].Value.ToString() == "CULMINADO")
                    {
                        codigoActaReporte = datalistadoTodasActas.Rows[datalistadoTodasActas.CurrentRow.Index].Cells[1].Value.ToString();
                        Visualizadores.VisualizarActa frm = new Visualizadores.VisualizarActa();
                        frm.lblCodigo.Text = codigoActaReporte;

                        frm.Show();
                    }
                    else if (datalistadoTodasActas.SelectedCells[13].Value.ToString() == "APROBADO")
                    {
                        codigoActaReporte = datalistadoTodasActas.Rows[datalistadoTodasActas.CurrentRow.Index].Cells[1].Value.ToString();
                        Visualizadores.VisualizarActaAprobada frm = new Visualizadores.VisualizarActaAprobada();
                        frm.lblCodigo.Text = codigoActaReporte;

                        frm.Show();
                    }
                    else
                    {
                        codigoActaReporte = datalistadoTodasActas.Rows[datalistadoTodasActas.CurrentRow.Index].Cells[1].Value.ToString();
                        Visualizadores.VisualizarActaDesaprobada frm = new Visualizadores.VisualizarActaDesaprobada();
                        frm.lblCodigo.Text = codigoActaReporte;

                        frm.Show();
                    }

                    //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                    ClassResourses.RegistrarAuditora(6, this.Name, 6, Program.IdUsuario, "Visualización del acta de viaje PDF", Convert.ToInt32(codigoActaReporte));
                }
                catch(Exception ex)
                {
                    //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                    ClassResourses.RegistrarAuditora(13, this.Name, 6, Program.IdUsuario, ex.Message, 0);
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un acta para poder generar el PDF.", "Validación del Sistema",MessageBoxButtons.OK);
            }
        }

        //ACCION PARA APROBAR UN ACTA
        private void btnAprobarActa_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasActas.CurrentRow != null)
            {
                if (Convert.ToBoolean(datalistadoTodasActas.SelectedCells[4].Value.ToString()) == true)
                {
                    DialogResult boton = MessageBox.Show("¿Realmente desea aprobar esta Acta?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                    if (boton == DialogResult.OK)
                    {
                        int idActa = Convert.ToInt32(datalistadoTodasActas.SelectedCells[1].Value.ToString());
                        int idLiquidacion = Convert.ToInt32(datalistadoTodasActas.SelectedCells[14].Value.ToString());
                        int idRequerimiento = Convert.ToInt32(datalistadoTodasActas.SelectedCells[15].Value.ToString());

                        string estadoActa = datalistadoTodasActas.SelectedCells[13].Value.ToString();

                        if (estadoActa == "APROBADO" || estadoActa == "ANULADO")
                        {
                            MessageBox.Show("Esta acta ya está aprobado o anulada.", "Validación del Sistema",MessageBoxButtons.OK);
                        }
                        else
                        {
                            if (estadoActa == "CULMINADO")
                            {
                                try
                                {
                                    SqlConnection con = new SqlConnection();
                                    SqlCommand cmd = new SqlCommand();
                                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                                    con.Open();
                                    cmd = new SqlCommand("CambioEstadoActa_Comercial", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@idActa", idActa);
                                    cmd.Parameters.AddWithValue("@idDetalleClienteLiquidacion", Convert.ToInt32(datalistadoTodasActas.SelectedCells[2].Value.ToString()));
                                    cmd.Parameters.AddWithValue("@estado", 2);
                                    cmd.Parameters.AddWithValue("@estado2", 2);
                                    cmd.ExecuteNonQuery();
                                    con.Close();

                                    MessageBox.Show("Acta aprobado exitosamente.", "Validación del Sistema", MessageBoxButtons.OK);
                                    MostrarActasPorFecha(DesdeFecha.Value, HastaFecha.Value);

                                    //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                                    ClassResourses.RegistrarAuditora(3, this.Name, 6, Program.IdUsuario, "Aprobación de acta de viaje.", Convert.ToInt32(idActa));
                                }
                                catch (Exception ex)
                                {
                                    //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                                    ClassResourses.RegistrarAuditora(13, this.Name, 6, Program.IdUsuario, ex.Message, 0);
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Esta acta aún no se ha culminado.", "Validación del Sistema",MessageBoxButtons.OK);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Esta acta aún no esta validada ni culminada.", "Validación del Sistema", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una acta para poder aprobarla.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //ACCION PARA DESAPROBAR 
        private void btnDesaprobarActa_Click(object sender, EventArgs e)
        {
            panleAnulacion.Visible = true;
        }

        //GENERAR EL PDF DE MI ACTA
        private void datalistadoTodasActas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoTodasActas.Columns[e.ColumnIndex];

            if (currentColumn.Name == "btnGenerarPdf")
            {
                if (datalistadoTodasActas.CurrentRow != null)
                {
                    try
                    {
                        string codigoActaReporte = "";

                        if (datalistadoTodasActas.SelectedCells[13].Value.ToString() == "PENDIENTE")
                        {
                            codigoActaReporte = datalistadoTodasActas.Rows[datalistadoTodasActas.CurrentRow.Index].Cells[1].Value.ToString();
                            Visualizadores.VisualizarActa frm = new Visualizadores.VisualizarActa();
                            frm.lblCodigo.Text = codigoActaReporte;
                            frm.Show();
                        }
                        else if (datalistadoTodasActas.SelectedCells[13].Value.ToString() == "APROBADO")
                        {
                            codigoActaReporte = datalistadoTodasActas.Rows[datalistadoTodasActas.CurrentRow.Index].Cells[1].Value.ToString();
                            Visualizadores.VisualizarActaAprobada frm = new Visualizadores.VisualizarActaAprobada();
                            frm.lblCodigo.Text = codigoActaReporte;
                            frm.Show();
                        }
                        else
                        {
                            codigoActaReporte = datalistadoTodasActas.Rows[datalistadoTodasActas.CurrentRow.Index].Cells[1].Value.ToString();
                            Visualizadores.VisualizarActaDesaprobada frm = new Visualizadores.VisualizarActaDesaprobada();
                            frm.lblCodigo.Text = codigoActaReporte;
                            frm.Show();
                        }

                        //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                        ClassResourses.RegistrarAuditora(6, this.Name, 6, Program.IdUsuario, "Visualización del acta de viaje PDF", Convert.ToInt32(codigoActaReporte));
                    }
                    catch(Exception ex)
                    {
                        //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                        ClassResourses.RegistrarAuditora(13, this.Name, 6, Program.IdUsuario, ex.Message, 0);
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar una acta para poder generar el PDF.", "Validación del Sistema",MessageBoxButtons.OK);
                }
            }
        }

        //FUNCION PARA PROCEDER A ANULAR MI ACTA, LIQUIDACION Y REQUERIMITNO
        private void btnProcederAnulacion_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasActas.CurrentRow != null)
            {
                int idActa = Convert.ToInt32(datalistadoTodasActas.SelectedCells[1].Value.ToString());
                int idLiquidacion = Convert.ToInt32(datalistadoTodasActas.SelectedCells[14].Value.ToString());
                int idRequerimiento = Convert.ToInt32(datalistadoTodasActas.SelectedCells[15].Value.ToString());
                string estadoActa = datalistadoTodasActas.SelectedCells[13].Value.ToString();

                DialogResult boton = MessageBox.Show("¿Realmente desea anular esta acta?. Se anulará el requerimeinto así como la liquidación asociada ha esta acta.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    if(estadoActa == "APROBADO" || estadoActa == "ANULADO")
                    {
                        MessageBox.Show("No se puede anular esta acta ya que se encuentra aprobada o anulada.", "Validación del Sistema",MessageBoxButtons.OK);
                    }
                    else
                    {
                        //////////////////////////--------------------------------------------------------------------------
                        //CODIGO IMPLEMENTADO 
                        if (!string.IsNullOrWhiteSpace(txtJustificacionAnulacion.Text) /*!= ""*/)
                        {
                            //////////////////////////--------------------------------------------------------------------------
                            try
                            {
                                SqlConnection con = new SqlConnection();
                                SqlCommand cmd = new SqlCommand();
                                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                                con.Open();
                                cmd = new SqlCommand("DesaprobarActa", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@idacta", idActa);
                                cmd.Parameters.AddWithValue("@idliquidacion", idLiquidacion);
                                cmd.Parameters.AddWithValue("@idrequerimiento", idRequerimiento);
                                cmd.Parameters.AddWithValue("@mensajeAnulado", txtJustificacionAnulacion.Text);
                                cmd.ExecuteNonQuery();
                                con.Close();

                                MessageBox.Show("Acta, liquidación y requerimiento asociado a esta, anuladas exitosamente.", "Validación del Sistema", MessageBoxButtons.OK);
                                MostrarActasPorFecha(DesdeFecha.Value, HastaFecha.Value);

                                panleAnulacion.Visible = false;
                                txtJustificacionAnulacion.Text = "";

                                //ClassResourses.Enviar("ynunahuanca@arenassrl.com.pe", "CORREO AUTOMATIZADO - ANULACIÓN DEL ACTA N°. " + idActa, "Correo de verificación de anulación de una acta por parte del usuario '" + Program.UnoNombreUnoApellidoUsuario + "' el la fecha siguiente: " + DateTime.Now + ". Por favor no responder.");
                                //ClassResourses.Enviar("jhoalexxxcc@gmail.com", "CORREO AUTOMATIZADO - ANULACIÓN DEL ACTA N°. " + idActa, "Correo de verificación de anulación de una acta por parte del usuario '" + Program.UnoNombreUnoApellidoUsuario + "' el la fecha siguiente: " + DateTime.Now + ". Por favor no responder.");

                                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                                ClassResourses.RegistrarAuditora(2, this.Name, 6, Program.IdUsuario, "Anular acta de viaje.", Convert.ToInt32(idActa));
                            }
                            catch (Exception ex)
                            {
                                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                                ClassResourses.RegistrarAuditora(13, this.Name, 6, Program.IdUsuario, ex.Message, 0);
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Debe ingresar una justificación para poder anular el acta.", "Validación del Sistema", MessageBoxButtons.OK);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un acta para poder anularla.", "Validación del Sistema", MessageBoxButtons.OK);
            }

            txtJustificacionAnulacion.Text = "";
            panleAnulacion.Visible = false;
        }

        //BOTON PARA RETROCEDER DE LA ANULACION
        private void btnRetrocederAnulacion_Click(object sender, EventArgs e)
        {
            panleAnulacion.Visible = false;
            txtJustificacionAnulacion.Text = "";
        }

        //FUNCIO PARA EXPORTAR TODOS LOS DATOS POR EXCEL
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                MostrarExcel();

                SLDocument sl = new SLDocument();
                SLStyle style = new SLStyle();
                SLStyle styleC = new SLStyle();

                //COLUMNAS
                sl.SetColumnWidth(1, 15);
                sl.SetColumnWidth(2, 15);
                sl.SetColumnWidth(3, 20);
                sl.SetColumnWidth(4, 20);
                sl.SetColumnWidth(5, 20);
                sl.SetColumnWidth(6, 50);
                sl.SetColumnWidth(7, 35);
                sl.SetColumnWidth(8, 40);
                sl.SetColumnWidth(9, 30);

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
                sl.SaveAs(desktopPath + @"\Reporte de actas.xlsx");
                MessageBox.Show("Se exportó los datos a un archivo de Microsoft Excel en la siguiente ubicación: " + desktopPath, "Validación del Sistema", MessageBoxButtons.OK);

                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(5, this.Name, 6, Program.IdUsuario, "Exportar listado de liquidaciones de ventas EXCEL", 0);
            }
            catch(Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 6, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //FUNCION PARA ABRIR EL MANUAL DE USUARIO
        private void btnInfo_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //FUNCION PARA ABRIR EL MANUAL DE USUARIO
        private void btnInfoDetalles_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //EXPORTAR DOCUMENTO SELECCIOANDO
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

                if (datalistadoTodasActas.SelectedCells[13].Value.ToString() == "PENDIENTE" || datalistadoTodasActas.SelectedCells[13].Value.ToString() == "CULMINADO")
                {
                    rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeActa.rpt");
                }
                else if (datalistadoTodasActas.SelectedCells[13].Value.ToString() == "APROBADO")
                {
                    rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeActaAprobada.rpt");
                }
                else
                {
                    rutaReporte = Path.Combine(rutaBase, "Reportes", "InformeActaDesaprobada.rpt");
                }

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
                int idActa = Convert.ToInt32(datalistadoTodasActas.SelectedCells[1].Value.ToString()); // Valor del parámetro (puedes obtenerlo de un TextBox, ComboBox, etc.)
                crystalReport.SetParameterValue("@idActa", idActa);

                // Ruta de salida en el escritorio
                string rutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string rutaSalida = System.IO.Path.Combine(rutaEscritorio, "Acta número " + idActa + ".pdf");

                // Exportar a PDF
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, rutaSalida);

                MessageBox.Show($"Reporte exportado correctamente a: {rutaSalida}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClassResourses.RegistrarAuditora(5, this.Name, 6, Program.IdUsuario, "Exportar Acta PDF", idActa);
            }
            catch (Exception ex)
            {
                ClassResourses.RegistrarAuditora(13, this.Name, 6, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show($"Ocurrió un error al exportar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
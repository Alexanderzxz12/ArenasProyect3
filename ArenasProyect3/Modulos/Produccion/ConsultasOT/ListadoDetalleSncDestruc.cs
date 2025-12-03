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

namespace ArenasProyect3.Modulos.Produccion.ConsultasOT
{
    public partial class ListadoDetalleSncDestruc : Form
    {
        //VARIABLES GLOBALES
        DataGridView dgvActivo = null;

        //CONMSTRUCTOR DE MI FORMULARIO
        public ListadoDetalleSncDestruc()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI FORMULARIO
        private void ListadoDetalleSncDestruc_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            // 1. Configuración de Controles
            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            VerificarDGVActivo();
        }

        //LISTADO DE OP Y SELECCION DE PDF Y ESTADO DE OP---------------------
        //MOSTRAR OP AL INCIO 
        //FUNCION PARA VISUALIZAR MIS RESULTADOS
        public void MostrarOrdenTrabajo(DateTime fechaInicio, DateTime fechaTermino, string codigo = null)
        {
            using (SqlConnection con = new SqlConnection(Conexion.ConexionMaestra.conexion))
            using (SqlCommand cmd = new SqlCommand("OT_MostrarSNCList", con))
            {
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                    cmd.Parameters.AddWithValue("@codigoOt", (object)codigo ?? DBNull.Value);
                    try
                    {
                        con.Open();
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                        datalsitadoTodasSNC.DataSource = dt;
                        DataRow[] rowsDes = dt.Select("[TIPO SNC] IN ('REPOSICIÓN')");
                        // Si hay filas, crea un nuevo DataTable, si no, usa una copia vacía del esquema.
                        DataTable dtDes = rowsDes.Any() ? rowsDes.CopyToDataTable() : dt.Clone();
                        datalistadoPorDestrucccion.DataSource = dtDes; // Asumiendo este es el nombre de tu DataGrid

                        RedimensionarListadoSNCDes(datalistadoPorDestrucccion);
                        RedimensionarListadoSNC(datalsitadoTodasSNC);
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
        public void RedimensionarListadoSNC(DataGridView DGV)
        {
            DGV.Columns[1].Width = 50;
            DGV.Columns[2].Width = 190;
            DGV.Columns[3].Width = 75;
            DGV.Columns[5].Width = 95;
            DGV.Columns[6].Width = 535;
            DGV.Columns[8].Width = 70;
            DGV.Columns[9].Width = 90;
            //COLUMNAS NO VISIBLES PARA EL USUARIO
            DGV.Columns[4].Visible = false;
            DGV.Columns[7].Visible = false;
            DGV.Columns[10].Visible = false;
            DGV.Columns[11].Visible = false;
            DGV.Columns[12].Visible = false;
            BloquearEdicion(DGV);
        }

        //FUNCION PARA REDIMENSIONAR MIS LISTADOS
        public void RedimensionarListadoSNCDes(DataGridView DGV)
        {
            DGV.Columns[1].Width = 50;
            DGV.Columns[2].Width = 190;
            DGV.Columns[3].Width = 75;
            DGV.Columns[5].Width = 95;
            DGV.Columns[6].Width = 400;
            DGV.Columns[8].Width = 70;
            DGV.Columns[9].Width = 90;
            DGV.Columns[10].Width = 135;
            //COLUMNAS NO VISIBLES PARA EL USUARIO
            DGV.Columns[4].Visible = false;
            DGV.Columns[7].Visible = false;
            DGV.Columns[11].Visible = false;
            DGV.Columns[12].Visible = false;
            BloquearEdicion(DGV);
        }

        //FUNCION PARA BLOQUERA MI LISTADO DE SNC
        public void BloquearEdicion(DataGridView DGV)
        {
            ////SE BLOQUEA MI LISTADO
            DGV.Columns[1].ReadOnly = true;
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
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO
        public void CargarColores(DataGridView dgv)
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= dgv.RowCount - 1; i++)
                {
                    string estado = dgv.Rows[i].Cells[10].Value.ToString();

                    if (estado == "DESTRUIDO")
                    {
                        dgv.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                    }
                    else if (estado == "PENDIENTE DESTRUCCIÓN")
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

        //MOSTRAR OT POR FECHA
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarOrdenTrabajo(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OT POR FECHA
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            MostrarOrdenTrabajo(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
        }

        //MOSTRAR OT POR FECHA
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarOrdenTrabajo(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR OT POR FECHA
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarOrdenTrabajo(DesdeFecha.Value, HastaFecha.Value);
        }

        //VISUALISAR MI PDF DE LA SNC
        private void btnPdfSNC_Click(object sender, EventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (dgvActivo.CurrentRow != null)
            {
                //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO DESAPROBADO
                string codigoDetalleCantidadCalidad = dgvActivo.Rows[dgvActivo.CurrentRow.Index].Cells[11].Value.ToString();
                Visualizadores.VisualizarSNCOT frm = new Visualizadores.VisualizarSNCOT();
                frm.lblCodigo.Text = codigoDetalleCantidadCalidad;
                //CARGAR VENTANA
                frm.Show();
            }
        }

        //FUNCION PARA DESTRUIR
        private void btnDestruir_Click(object sender, EventArgs e)
        {
            ActualizarSNCSeleccionado();
        }

        //ACTUALIZAR EL ESTADO DE LA SNC
        public void ActualizarSNCSeleccionado()
        {
            // Validar la selección de fila
            if (dgvActivo.SelectedRows.Count == 0 || dgvActivo.Name == "datalsitadoTodasSNC")
            {
                MessageBox.Show("No existe un registro para destruir o se encuentra en un listado diferente al de destrucción.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener el estado actual y el ID
            string estadoActual = dgvActivo.Rows[dgvActivo.CurrentRow.Index].Cells[10].Value.ToString();
            int idDetalleCantidadCalidad;

            try
            {
                idDetalleCantidadCalidad = Convert.ToInt32(dgvActivo.Rows[dgvActivo.CurrentRow.Index].Cells[11].Value);
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo obtener el ID de la fila seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Validación de estado
            if (estadoActual == "DESTRUIDO")
            {
                MessageBox.Show("Este registro ya se encuentra en estado 'DESTRUIDO'. No se requiere actualización.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Detiene la ejecución
            }

            //SOLICITAR CONFIRMACIÓN
            DialogResult resultado = MessageBox.Show(
                "¿Está seguro de marcar este registro como DESTRUIDO?",
                "Confirmar Destrucción",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            //Proceder solo si el usuario confirma
            if (resultado == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(Conexion.ConexionMaestra.conexion))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("OT_ActualizarEstadoDestruido", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@IdDetalleCantidadCalidadOT", idDetalleCantidadCalidad);
                            cmd.Parameters.AddWithValue("@NuevoEstado", 4); // Estado fijo 'DESTRUIDO'
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("El registro ha sido marcado como DESTRUIDO.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar en la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MostrarOrdenTrabajo(DesdeFecha.Value, HastaFecha.Value);
            }
        }
        //-----------------------------------------------------------------------------------------------------------

        //EVENETOS Y RECURSOS VARIOS--------------------------------------------------------------------------------
        //COLOREAR LISTADO
        private void datalistadoTodasSNC_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            CargarColores(datalsitadoTodasSNC);
        }

        //COLOREAR MI LSITADO
        private void datalistadoPorDestrucccion_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            CargarColores(datalistadoPorDestrucccion);
        }

        //VERIFICAR EN QUE LSITADO ESTOY
        public void VerificarDGVActivo()
        {
            if (TabControl.SelectedTab.Text == "SNC para destrucción")
            {
                dgvActivo = datalistadoPorDestrucccion;
            }
            else if (TabControl.SelectedTab.Text == "Todas las SNC")
            {
                dgvActivo = datalsitadoTodasSNC;
            }
        }

        //CAMBIAR MI LISTADO
        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarDGVActivo();
        }
    }
}

using DocumentFormat.OpenXml.Wordprocessing;
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

namespace ArenasProyect3.Modulos.Produccion.ConsultasOP
{
    public partial class ListadoDetalleSncDestruc : Form
    {
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
        }

        //FUNCION PARA VISUALIZAR MIS RESULTADOS
        public void MostrarListaSncDestruc(DateTime fechaInicio, DateTime fechaTermino, string codigoOp)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(Conexion.ConexionMaestra.conexion))
            {
                using (SqlCommand cmd = new SqlCommand("OP_MostrarSncDestruccion", con)) // Asume el nombre del SP que discutimos
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);                 
                    cmd.Parameters.AddWithValue("@codigoOp", codigoOp ?? string.Empty);
                    try
                    {
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores básico (puedes adaptarlo a tu sistema de logging)
                        System.Diagnostics.Debug.WriteLine("Error al ejecutar el procedimiento almacenado: " + ex.Message);
                        return; 
                    }
                }
            } 
            datalistadoTodasSNC.DataSource = dt;
            RedimensionarListadoSNC(datalistadoTodasSNC);
        }

        //FUNCION PARA REDIMENSIONAR MIS LISTADOS
        public void RedimensionarListadoSNC(DataGridView DGV)
        {
            DGV.Columns[0].Width = 55;
            DGV.Columns[1].Width = 190;
            DGV.Columns[2].Width = 90;
            DGV.Columns[4].Width = 100;
            DGV.Columns[5].Width = 390;
            DGV.Columns[7].Width = 80;
            DGV.Columns[8].Width = 150;
            DGV.Columns[11].Width = 80;
            DGV.Columns[3].Visible = false;
            DGV.Columns[6].Visible = false;
            DGV.Columns[9].Visible = false;
            DGV.Columns[10].Visible = false;
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO
        public void CargarColores(DataGridView dgv)
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= dgv.RowCount - 1; i++)
                {
                    string estado = dgv.Rows[i].Cells[8].Value.ToString();

                    if (estado == "DESTRUIDO")
                    {
                        dgv.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    else if (estado == "PENDIENTE DESTRUCCIÓN")
                    {
                        dgv.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
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
            MostrarListaSncDestruc(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
        }

        //MOSTRAR OT POR FECHA Y CODIGO
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            MostrarListaSncDestruc(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
        }

        //MOSTRAR OT POR FECHA Y CODIGO
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarListaSncDestruc(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
        }

        //MOSTRAR OT POR FECHA Y CODIGO
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarListaSncDestruc(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
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
            if (datalistadoTodasSNC.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione una fila para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener el estado actual y el ID
            string estadoActual = datalistadoTodasSNC.SelectedRows[0].Cells["ESTADO"].Value.ToString();
            int idDetalleCantidadCalidad;

            try
            {
                idDetalleCantidadCalidad = Convert.ToInt32(datalistadoTodasSNC.SelectedRows[0].Cells["IdDetalleCantidadCalidad"].Value);
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
                        using (SqlCommand cmd = new SqlCommand("OP_ActualizarEstadoDestruido", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@IdDetalleCantidadCalidad", idDetalleCantidadCalidad);
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
                MostrarListaSncDestruc(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
            }
        }

        //COLOREAR MI LSITADO
        private void datalistadoTodasSNC_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            CargarColores(datalistadoTodasSNC);
        }

        //VISUALISAR MI PDF DE LA SNC
        private void btnPdfSNC_Click(object sender, EventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoTodasSNC.CurrentRow != null)
            {
                //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO DESAPROBADO
                string codigoDetalleCantidadCalidad = datalistadoTodasSNC.Rows[datalistadoTodasSNC.CurrentRow.Index].Cells[10].Value.ToString();
                Visualizadores.VisualizarSNC frm = new Visualizadores.VisualizarSNC();
                frm.lblCodigo.Text = codigoDetalleCantidadCalidad;
                //CARGAR VENTANA
                frm.Show();
            }
        }
    }
}

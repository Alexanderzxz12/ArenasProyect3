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
        private Cursor curAnterior = null;
        public ListadoDetalleSncDestruc()
        {
            InitializeComponent();
        }

        private void ListadoDetalleSncDestruc_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            // 1. Configuración de Controles
            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            datalistadoTodasSNC.DataSource = null; // Esto se mantendrá así si 'datalistadoTodasSNC' es el control final
            
            MostrarListaSncDestruc(oPrimerDiaDelMes, oUltimoDiaDelMes, string.Empty);
            
            // PREFILES Y PERSIMOS---------------------------------------------------------------
            if (Program.RangoEfecto != 1)
            {
                //btnAnularPedido.Visible = false;
                //lblAnularPedido.Visible = false;
            }
            //---------------------------------------------------------------------------------
        }

        public void MostrarListaSncDestruc(DateTime fechaInicio, DateTime fechaTermino, string codigoOp)
        {

            DataTable dt = new DataTable();
    
            using (SqlConnection con = new SqlConnection(Conexion.ConexionMaestra.conexion))
            {
                using (SqlCommand cmd = new SqlCommand("OP_MostrarSncDestruccion", con)) // Asume el nombre del SP que discutimos
                {
                    // Configuración del Comando
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);                 
                    cmd.Parameters.AddWithValue("@codigoOp", codigoOp ?? string.Empty);

                    // Ejecución y Llenado del DataTable
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

            // Asignación y Redimensionamiento
            datalistadoTodasSNC.DataSource = dt;
            RedimensionarListadoSNC(datalistadoTodasSNC);
        }

        public void RedimensionarListadoSNC(DataGridView DGV)
        {

            DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            if (DGV.Columns.Count > 5)
            {
                DGV.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DGV.Columns[5].FillWeight = 150;
            }

            if (DGV.Columns.Count > 6)
            {
    
                DGV.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                DGV.Columns[6].FillWeight = 150;
            }
            DGV.Columns[9].Visible = false;
            CargarColores();



        }

        public void CargarColores()
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= datalistadoTodasSNC.RowCount - 1; i++)
                {
                    if (datalistadoTodasSNC.Rows[i].Cells[8].Value.ToString() == "DESTRUIDO")
                    {
                        datalistadoTodasSNC.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    else if (datalistadoTodasSNC.Rows[i].Cells[8].Value.ToString() == "PENDIENTE DESTRUCCIÓN")
                    {
                        datalistadoTodasSNC.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            //Obtener los valores de los controles
            DateTime fechaInicio = DesdeFecha.Value;
            DateTime fechaTermino = HastaFecha.Value;

         
            string codigoOp = string.IsNullOrWhiteSpace(txtBusqueda.Text) ? string.Empty : txtBusqueda.Text.Trim();

           
            MostrarListaSncDestruc(fechaInicio, fechaTermino, codigoOp);
        }

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

                //Recargar los datos con los filtros actuales
                DateTime fechaInicio = DesdeFecha.Value;
                DateTime fechaTermino = HastaFecha.Value;
                string codigoOp = string.IsNullOrWhiteSpace(txtBusqueda.Text) ? string.Empty : txtBusqueda.Text.Trim();

                MostrarListaSncDestruc(fechaInicio, fechaTermino, codigoOp);
            }
        }


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ActualizarSNCSeleccionado();
        }



        private void datalistadoTodasSNC_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            CargarColores();
        }

        private void btnPlano_Click(object sender, EventArgs e)
        {
            //SI NO HAY NINGUN REGISTRO SELECCIONADO
            if (datalistadoTodasSNC.CurrentRow != null)
            {
                //SE CARGA EL VISUALIZADOR DEL REQUERIMIENTO DESAPROBADO
                string codigoDetalleCantidadCalidad = datalistadoTodasSNC.Rows[datalistadoTodasSNC.CurrentRow.Index].Cells[0].Value.ToString();
                Visualizadores.VisualizarSNC frm = new Visualizadores.VisualizarSNC();
                frm.lblCodigo.Text = codigoDetalleCantidadCalidad;
                //CARGAR VENTANA
                frm.Show();
            }
        }

        private void txtBusqueda_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 1. Permite el control (ej. Backspace, Delete)
            // El carácter de control se usa para teclas como Borrar/Retroceso.
            if (char.IsControl(e.KeyChar))
            {
                e.Handled = false; // Permite el carácter
                return;
            }

            // 2. Permite dígitos (0-9)
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = false; // Permite el dígito
                return;
            }

            // 3. Ignora cualquier otra cosa (texto, símbolos, etc.)
            e.Handled = true; // Ignora el carácter (no se muestra en el TextBox)
        }
    }
}

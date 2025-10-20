using ArenasProyect3.Modulos.Resourses;
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

namespace ArenasProyect3.Modulos.Comercial.Auditora
{
    public partial class Auditora : Form
    {
        //CONSTRUCTOR DEL MANTENIMIENTO - MANTENIMIENTO AUDITOR
        public Auditora()
        {
            InitializeComponent();
        }

        //INICIO Y CARGA INICIAL DE AUDITORA - CONSTRUCTOR--------------------------------------------------------------------------------------
        private void Auditora_Load(object sender, EventArgs e)
        {
            //AJUSTAR FECHAS AL INICIO DEL MES Y FINAL DEL MES
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);
            //ASIGNARLE LAS VARIABLES YA CARGADAS A MIS DateTimerPicker
            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;

            CargarResponsables();
            CargarProcesos();
        }

        //METODO PARA PINTAR DE COLORES LAS FILAS DE MI LSITADO
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
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
                ClassResourses.RegistrarAuditora(13, this.Name, 2, Program.IdUsuario = 0, ex.Message, 0);
            }
        }

        //CARGA DE COMBOS ----------------------------------------------------------------------------
        //CARGAR RESPONSABLES
        public void CargarResponsables()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdUsuarios, Nombres + ' ' + Apellidos AS [NOMBRES] FROM Usuarios WHERE Estado = 'Activo' ORDER BY Nombres", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);

                // Crear fila adicional
                DataRow filaInicial = dt.NewRow();
                filaInicial["NOMBRES"] = "Seleccionar usuario";
                filaInicial["IdUsuarios"] = DBNull.Value;

                // Insertar en la primera posición
                dt.Rows.InsertAt(filaInicial, 0);

                cboUsuarios.DisplayMember = "NOMBRES";
                cboUsuarios.ValueMember = "IdUsuarios";
                cboUsuarios.DataSource = dt;
            }
            catch(Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 1, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show(ex.Message);
            }
        }

        //CARGAR PROCESOS
        public void CargarProcesos()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdProceso, Nombre FROM ProcesoSistema WHERE Estado = 1 ORDER BY Nombre", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);

                // Crear fila adicional
                DataRow filaInicial = dt.NewRow();
                filaInicial["Nombre"] = "Seleccionar usuario";
                filaInicial["IdProceso"] = DBNull.Value;

                // Insertar en la primera posición
                dt.Rows.InsertAt(filaInicial, 0);

                cboProceso.DisplayMember = "Nombre";
                cboProceso.ValueMember = "IdProceso";
                cboProceso.DataSource = dt;
            }
            catch (Exception ex)
            {
                //INGRESO DE AUDITORA | ACCION - MANTENIMIENTO - PROCESO - IDUSUARIO - DESCRIPCION - IDGENERAL
                ClassResourses.RegistrarAuditora(13, this.Name, 1, Program.IdUsuario, ex.Message, 0);
                MessageBox.Show(ex.Message);
            }
        }

        //LISTADO DE ACCIONES Y SELECCIÓN DE PDF Y ESTADO---------------------------------------------------------------
        //MOSTRAR ACCIONES POR FECHA
        public void MostrarAcciones(DateTime fechaInicio, DateTime fechaTermino, int? idUsuario, int? idProceso)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarAcciones_Comercial", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
            cmd.Parameters.AddWithValue("@idProceso", idProceso);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoAcciones.DataSource = dt;
            con.Close();
            //SE REDIMENSIONA EL TAMAÑO DE CADA COLUMNA DE MI LISTADO DE REQUERIMIENTOS
            datalistadoAcciones.Columns[0].Width = 50;
            datalistadoAcciones.Columns[1].Width = 110;
            datalistadoAcciones.Columns[2].Width = 230;
            datalistadoAcciones.Columns[3].Width = 120;
            datalistadoAcciones.Columns[4].Width = 120;
            datalistadoAcciones.Columns[5].Width = 160;
            datalistadoAcciones.Columns[6].Width = 230;
            datalistadoAcciones.Columns[7].Width = 90;
            datalistadoAcciones.Columns[8].Width = 100;
            datalistadoAcciones.Columns[9].Width = 120;

            foreach (DataGridViewColumn column in datalistadoAcciones.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            alternarColorFilas(datalistadoAcciones);
        }

        //BÚSQUEDA DE ACCIONES POR FECHAS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            int? idUsuario = null;
            int? idProceso = null;

            if (cboUsuarios.Text != "Seleccionar usuario")
            {
                idUsuario = Convert.ToInt32(cboUsuarios.SelectedValue.ToString());
            }

            if (cboProceso.Text != "Seleccionar usuario")
            {
                idProceso = Convert.ToInt32(cboProceso.SelectedValue.ToString());
            }

            MostrarAcciones(DesdeFecha.Value, HastaFecha.Value, idUsuario, idProceso);
        }
    }
}

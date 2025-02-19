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
            cboCodigoDocumento.SelectedIndex = 0;
        }

        //CARGA DE COMBOS ----------------------------------------------------------------------------
        //CARGAR RESPONSABLES
        public void CargarResponsables()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdUsuarios, Nombres + ' ' + Apellidos AS [NOMBRES] FROM Usuarios WHERE Estado = 'Activo' AND HabilitadoRequerimientoVenta = 1 ORDER BY Nombres", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboUsuarios.DisplayMember = "NOMBRES";
            cboUsuarios.ValueMember = "IdUsuarios";
            cboUsuarios.DataSource = dt;
        }

        //LISTADO DE ACCIONES Y SELECCIÓN DE PDF Y ESTADO---------------------------------------------------------------
        //MOSTRAR ACCIONES POR FECHA
        public void MostrarRequerimientos(DateTime fechaInicio, DateTime fechaTermino, int idUsuario)
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
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoAcciones.DataSource = dt;
            con.Close();
            //SE REDIMENSIONA EL TAMAÑO DE CADA COLUMNA DE MI LISTADO DE REQUERIMIENTOS
            datalistadoAcciones.Columns[0].Width = 90;
            datalistadoAcciones.Columns[1].Width = 150;
            datalistadoAcciones.Columns[2].Width = 280;
            datalistadoAcciones.Columns[4].Width = 380;
            datalistadoAcciones.Columns[6].Width = 230;
            //SE QUITA LAS COLUMNAS QUE NO SON RELEVANTES PARA EL USUARIO
            datalistadoAcciones.Columns[3].Visible = false;
            datalistadoAcciones.Columns[5].Visible = false;
            datalistadoAcciones.Columns[7].Visible = false;
            datalistadoAcciones.Columns[8].Visible = false;
            datalistadoAcciones.Columns[9].Visible = false;
            datalistadoAcciones.Columns[10].Visible = false;
            datalistadoAcciones.Columns[11].Visible = false;
            datalistadoAcciones.Columns[12].Visible = false;
            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in datalistadoAcciones.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //BÚSQUEDA DE ACCIONES POR FECHAS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarRequerimientos(DesdeFecha.Value, HastaFecha.Value, Convert.ToInt32(cboUsuarios.SelectedValue.ToString()));
        }

        //DETALLES DEL REGISTRO
        private void datalistadoAcciones_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

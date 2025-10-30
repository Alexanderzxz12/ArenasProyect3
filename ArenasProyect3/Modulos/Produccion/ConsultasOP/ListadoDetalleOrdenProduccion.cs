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
    public partial class ListadoDetalleOrdenProduccion : Form
    {
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        private Cursor curAnterior = null;

        //CONMSTRUCTOR DE MI FORMULARIO
        public ListadoDetalleOrdenProduccion()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI FORMULARIO
        private void ListadoDetalleOrdenProduccion_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            datalistadoTodas.DataSource = null;
            cboBusqeuda.SelectedIndex = 0;

            //PREFILES Y PERSIMOS---------------------------------------------------------------
            if (Program.RangoEfecto != 1)
            {
                //btnAnularPedido.Visible = false;
                //lblAnularPedido.Visible = false;
            }
            //---------------------------------------------------------------------------------
        }

        //LISTADO DE PEDIDO POR OPP---------------------
        //MOSTRAR OP AL INCIO 
        public void MostrarDetallePedidoXOPPorFecha(DateTime fechaInicio, DateTime fechaTermino)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("OP_MostrarDetallePedidoXOPorFecha", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoTodas.DataSource = dt;
            con.Close();
            RedimensionarListadoGeneralPedido(datalistadoTodas);
        }

        //MOSTRAR OP AL INCIO 
        public void MostrarDetallePedidoXOPPorCodigoPedido(DateTime fechaInicio, DateTime fechaTermino, string codigoPedido)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("OP_MostrarDetallePedidoXOPorCodigoPedido", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoTodas.DataSource = dt;
            con.Close();
            RedimensionarListadoGeneralPedido(datalistadoTodas);
        }

        //FUNCION PARA REDIMENSIONAR MIS LISTADOS
        public void RedimensionarListadoGeneralPedido(DataGridView DGV)
        {
            //REDIEMNSION DE PEDIDOS
            DGV.Columns[0].Width = 100;
            DGV.Columns[1].Width = 100;
            DGV.Columns[2].Width = 100;
            DGV.Columns[3].Width = 70;
            DGV.Columns[4].Width = 555;
            DGV.Columns[5].Width = 100;
            DGV.Columns[6].Width = 120;
            //SE HACE NO VISIBLE LAS COLUMNAS QUE NO LES INTERESA AL USUARIO
            DGV.Columns[7].Visible = false;

            CargarColores();
        }

        //FUNCIÓN PARA COLOREAR MIS REGISTROS EN MI LISTADO
        public void CargarColores()
        {
            try
            {
                //RECORRIDO DE MI LISTADO
                for (var i = 0; i <= datalistadoTodas.RowCount - 1; i++)
                {
                    if (datalistadoTodas.Rows[i].Cells[6].Value.ToString() == "FALTA GENERAR")
                    {
                        datalistadoTodas.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    }
                    else if (datalistadoTodas.Rows[i].Cells[6].Value.ToString() == "GENERADO")
                    {
                        datalistadoTodas.Rows[i].DefaultCellStyle.ForeColor = System.Drawing.Color.ForestGreen;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //MOSTRAR PEDIDOS SEGUN LAS FECHAS
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarDetallePedidoXOPPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR PEDIDOS SEGUN LAS FECHAS
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarDetallePedidoXOPPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR PEDIDOS SEGUN LAS FECHAS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarDetallePedidoXOPPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //FUNCION PARA REORDENAR Y APLICAR LOS COLRORES
        private void datalistadoTodas_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            CargarColores();
        }

        //MOSTRAR SEGUN EL CODIGO DE PEDIDO
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            MostrarDetallePedidoXOPPorCodigoPedido(DesdeFecha.Value, HastaFecha.Value, txtBusqueda.Text);
        }

        private void datalistadoTodas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArenasProyect3.Modulos.ManGeneral;
using Newtonsoft.Json.Linq;

namespace ArenasProyect3.Modulos.Logistica.Almacen
{
    public partial class TipoCambio : Form
    {
        //CONSTRUCTOR DE MI FORMULARIO
        public TipoCambio()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI FORMULARIO
        private void TipoCambio_Load(object sender, EventArgs e)
        {
            Mostrar();
        }

        //VIZUALIZAR DATOS--------------------------------------------------------------------
        public void Mostrar()
        {

            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarTipoCambio", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoTipoCambio.DataSource = dt;
            con.Close();

            datalistadoTipoCambio.Columns[0].Visible = false;

            datalistadoTipoCambio.Columns[1].Width = 100;
            datalistadoTipoCambio.Columns[2].Width = 100;
            datalistadoTipoCambio.Columns[3].Width = 100;

            alternarColorFilas(datalistadoTipoCambio);
        }

        //MÉTODO PARA PINTAR EL LISTADO Y PARA QUE SE VEA MÁS BONITO
        public void alternarColorFilas(DataGridView dgv)
        {
            try
            {
                {
                    var withBlock = dgv;
                    withBlock.RowsDefaultCellStyle.BackColor = Color.LightBlue;
                    withBlock.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //SELECCIONAR UN IGRESO AUTOMÁTICO
        private void rbIngresoAutomatica_CheckedChanged(object sender, EventArgs e)
        {
            panelIngresoAutomatico.Visible = true;
            panelIngresoManual.Visible = false;
            btnGenerarCambio.Visible = false;
            lblGenerarCambio.Visible = false;
        }

        //SELECCIONAR UN IGRESO MANUAL
        private void rbIngresoManual_CheckedChanged(object sender, EventArgs e)
        {
            panelIngresoAutomatico.Visible = false;
            panelIngresoManual.Visible = true;
            btnGenerarCambio.Visible = false;
            lblGenerarCambio.Visible = false;
        }

        //SELECCIONAR UN IGRESO MANUAL MÁS GENERACION
        private void rbGeneracionAutomatica_CheckedChanged(object sender, EventArgs e)
        {
            panelIngresoManual.Visible = true;
            btnGenerarCambio.Visible = true;
            lblGenerarCambio.Visible = true;
        }

        //EVENTO DE GUARDADO DE UN NUEVO TIPO DE CAMBIO
        private void btnIngresarCambio_Click(object sender, EventArgs e)
        {
            if (txtTipoCambio.Text == "" || txtTipoVenta.Text == "")
            {
                MessageBox.Show("Debe ingresar los datos respectivos para poder guardar un tipo de cambio.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    DialogResult boton = MessageBox.Show("¿Realmente desea guardar este tipo de cambio?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                    if (boton == DialogResult.OK)
                    {
                        //LLAMANDO AL PROCEDIMIENTO ALMACENADO
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("InsertarTipoCambio", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        //INGRESO DE LOS PARÁMETROS DEL PROCEDIMEINTO ALMACENADO
                        cmd.Parameters.AddWithValue("@fechaingreso", datatimeFechaIngreso.Value);
                        cmd.Parameters.AddWithValue("@tipocambio", txtTipoCambio.Text);
                        cmd.Parameters.AddWithValue("@tipoventa", txtTipoVenta.Text);
                        cmd.Parameters.AddWithValue("@maquina", Environment.MachineName);
                        cmd.Parameters.AddWithValue("@idusuario", Program.IdUsuario);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Se ingresó el nuevo registro exitosamente.", "Validación del Sistema", MessageBoxButtons.OK);

                        txtTipoCambio.Text = "";
                        txtTipoVenta.Text = "";
                        lblCodigoManual.Text = "***";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //EVENTO QUE REFRESCA EL LISTADO DE TIPOS DE CAMBIO
        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            //HABILITAR Y DESHABILITAR MIS BOTONES DE GUARDADO Y EDICIÓN 
            datatimeFechaIngreso.Value = DateTime.Now;
            lblCodigoManual.Text = "***";
            txtTipoCambio.Text = "";
            txtTipoVenta.Text = "";

            Mostrar();
        }

        //EVENTO PARA ABRIR LA PÁGINA DE SUNAT DONDE SE ENCUENTRAN LOS TIPOS DE CAMBIO
        private void bntSunat_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://e-consulta.sunat.gob.pe/cl-at-ittipcam/tcS01Alias");
        }

        //EVENTO PARA VALIDAR LO QUE SE INGRESA EN EL CAMPO DE TEXTO - SOL NÚMEROS CON DECMALES
        private void txtTipoCambio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        //EVENTO PARA VALIDAR LO QUE SE INGRESA EN EL CAMPO DE TEXTO - SOL NÚMEROS CON DECMALES
        private void txtTipoVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }



        private void btnGenerarCambio_Click(object sender, EventArgs e)
        {

        }





        //ACCION PARA MOSTRAR EL TIPO DE CAMBIO
        private async void btnCambio_Click(object sender, EventArgs e)
        {
            string fecha = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            var service = new TipoCambioServices();

            try
            {
                JObject tipoCambio = await service.ObtenerTipoCambioAsync(fecha);
                lblCompra.Text = tipoCambio["compra"].ToString();
                lblVenta.Text = tipoCambio["venta"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);

            }
        }
    }
}

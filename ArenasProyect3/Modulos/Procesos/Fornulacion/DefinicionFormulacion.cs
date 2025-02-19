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

namespace ArenasProyect3.Modulos.Procesos.Fornulacion
{
    public partial class DefinicionFormulacion : Form
    {
        //CONSTRUCTOR DEL MANTENIMIENTO - MANTENIEMINTO DE DEFINICION DE FORMULACION
        public DefinicionFormulacion()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI MANTENIMIENTOS DE DEFINICION
        private void DefinicionFormulacion_Load(object sender, EventArgs e)
        {
            cboBusqueda.SelectedIndex = 0;
            CargarLineas(cboLinea);
            CargarTipoFormulacion(cboTipo);
            MostrarTodos();
            alternarColorFilas(datalistadoDefinicionFormulacion);
            cboEstado.SelectedIndex = 1;
        }

        //METODO PARA PINTAR DE COLORES LAS FILAS DE MI LSITADO
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
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGA DE DATOS - TIPO DE FORMULACION
        public void CargarTipoFormulacion(ComboBox cbo)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdTipoFormulacion, Descripcion FROM TipoFormulacion WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cbo.DisplayMember = "Descripcion";
            cbo.ValueMember = "IdTipoFormulacion";
            cbo.DataSource = dt;
        }

        //CARGA DE DATOS - CATEGORIA DE FORMULACION
        public void CargarLineas(ComboBox cbo)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdLinea, Descripcion FROM  LINEAS WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cbo.DisplayMember = "Descripcion";
            cbo.ValueMember = "IdLinea";
            cbo.DataSource = dt;
        }

        //METODO PARA LISTAR TODAS MIS DEFINICIONES
        public void MostrarTodos()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            da = new SqlDataAdapter("SELECT Case When DF.Estado = 1 Then 'ACTIVO' Else 'INCATIVO' End As [ESTADO], DF.IdDefinicionFormulaciones AS [ID], L.IdLinea,L.Descripcion [LINEA], DF.IdTipo, TF.Descripcion AS[TIPO] FROM DefinicionFormulaciones DF INNER JOIN LINEAS L ON L.IdLinea = DF.IdLinea INNER JOIN TipoFormulacion TF ON TF.IdTipoFormulacion = DF.IdTipo", con);
            da.Fill(dt);
            datalistadoDefinicionFormulacion.DataSource = dt;
            con.Close();
            datalistadoDefinicionFormulacion.Columns[2].Visible = false;
            datalistadoDefinicionFormulacion.Columns[4].Visible = false;
            //
            datalistadoDefinicionFormulacion.Columns[0].Width = 90;
            datalistadoDefinicionFormulacion.Columns[1].Width = 60;
            datalistadoDefinicionFormulacion.Columns[3].Width = 240;
            datalistadoDefinicionFormulacion.Columns[5].Width = 240;
        }

        //EVENTO DE DOBLE CLICK PARA EN MI LISTADO DE DEFINICIONES
        private void datalistadoDefinicionFormulacion_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            lblCodigo.Text = datalistadoDefinicionFormulacion.SelectedCells[1].Value.ToString();
            cboLinea.SelectedValue = datalistadoDefinicionFormulacion.SelectedCells[2].Value.ToString();
            cboTipo.SelectedValue = datalistadoDefinicionFormulacion.SelectedCells[4].Value.ToString();
            string estado = datalistadoDefinicionFormulacion.SelectedCells[0].Value.ToString();

            if (estado == "ACTIVO")
            {
                cboEstado.Text = "ACTIVO";
            }
            else
            {
                cboEstado.Text = "INACTIVO";
            }
        }

        //HABILITAR GUARDADO DE UNA NUEVA DEFINICION
        private void btnGuardar2_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("Realmente desea guardar esta definición de formulación.", "Validación de Sistema", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("InsertarDefinicionFormulacion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idLinea", cboLinea.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@idTipo", cboTipo.SelectedValue.ToString());
                if (cboEstado.Text == "ACTIVO")
                {
                    cmd.Parameters.AddWithValue("@estado", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@estado", 0);
                }

                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Se ingreso el nuevo registro correctamente.", "Registro Nuevo", MessageBoxButtons.OK);
                MostrarTodos();

                cboEstado.SelectedIndex = -1;
            }
        }

        //EDITAR UNA CUENTA DE MI BASE DE DATO
        private void btnEditar2_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("Realmente desea editar el estado de esta definición de una formulación.", "Validación de Sistema", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (lblCodigo.Text == "N")
                {
                    MessageBox.Show("Debe seleccionar un registro para poder cambiar el estado.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("EditarDefinicionFormulacion", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@codigo", Convert.ToInt32(lblCodigo.Text));

                        if (cboEstado.Text == "ACTIVO")
                        {
                            cmd.Parameters.AddWithValue("@estado", 1);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@estado", 0);
                        }

                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Se edito correctamente el registro.", "Edición", MessageBoxButtons.OK);
                        MostrarTodos();

                        cboEstado.SelectedIndex = 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //VALIDACION Y BUSQUEDA DE DEFINICIONES SEGUN CRITERIOS-----------
        private void cboBusqueda_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusqueda.Visible = true;
            txtBusqueda.Text = "";
        }

        //BUSQUEDA--------------------------------------------------------------------------------
        //REALIZAR LA BUSQUEDA POR TEXTO
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarDefiniciónFormulacionPorCodigo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigo", txtBusqueda.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoDefinicionFormulacion.DataSource = dt;
            con.Close();
            datalistadoDefinicionFormulacion.Columns[2].Visible = false;
            datalistadoDefinicionFormulacion.Columns[4].Visible = false;
            //
            datalistadoDefinicionFormulacion.Columns[0].Width = 90;
            datalistadoDefinicionFormulacion.Columns[1].Width = 60;
            datalistadoDefinicionFormulacion.Columns[3].Width = 240;
            datalistadoDefinicionFormulacion.Columns[5].Width = 240;
        }

        //VALIDACIONES Y EXPRTACION DE DATOS-----------------------------
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            ExportarDatos(datalistadoDefinicionFormulacion);
        }

        //METODO PARA EXPORTAR LAS CUENTAS A EXCEL
        public void ExportarDatos(DataGridView datalistado)
        {
            Microsoft.Office.Interop.Excel.Application exportarexcel = new Microsoft.Office.Interop.Excel.Application();

            exportarexcel.Application.Workbooks.Add(true);

            int indicecolumna = 0;
            foreach (DataGridViewColumn columna in datalistado.Columns)
            {
                indicecolumna++;

                exportarexcel.Cells[1, indicecolumna] = columna.Name;
            }

            int indicefila = 0;
            foreach (DataGridViewRow fila in datalistado.Rows)
            {
                indicefila++;
                indicecolumna = 0;
                foreach (DataGridViewColumn columna in datalistado.Columns)
                {
                    indicecolumna++;
                    exportarexcel.Cells[indicefila + 1, indicecolumna] = fila.Cells[columna.Name].Value;
                }
            }
            exportarexcel.Visible = true;
        }
    }
}

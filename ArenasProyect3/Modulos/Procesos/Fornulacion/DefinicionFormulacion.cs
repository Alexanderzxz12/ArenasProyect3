using DocumentFormat.OpenXml.InkML;
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
        //VARIABLE GLOBAL PARA VALIDAR SI SE REPITE UNA FORMULACION CON EL MISMO TIPO
        bool repetidalinea;

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
            lineasrepetidas();
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

        //FUNCION PARA VERIFICAR LÍNEAS REPETIDAS
        public void lineasrepetidas()
        {
            repetidalinea = false;

            foreach (DataGridViewRow dgv in datalistadoDefinicionFormulacion.Rows)
            {
                int valor1 = Convert.ToInt32(dgv.Cells["IdLinea"].Value);
                int valor2 = Convert.ToInt32(dgv.Cells["IdTipo"].Value);

                if (valor1 == Convert.ToInt32(cboLinea.SelectedValue) && valor2 == Convert.ToInt32(cboTipo.SelectedValue))
                {
                    repetidalinea = true;
                    return;
                }
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
            try
            {

                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand("DefinicionFormulacion_Mostrar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

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

        //METODO PARA GUARDAR EN LA BASE DE DATOS UNA NUEVA DEFINICION DE FORMULACION
        public void AgregarDefinicionFormulacion(int idlinea, int idtipo, ComboBox cbo)
        {
            try
            {
                if (repetidalinea == true)
                {
                    MessageBox.Show("No se puede ingresar dos registros iguales.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    DialogResult boton = MessageBox.Show("Realmente desea guardar esta definición de formulación.", "Validación de Sistema", MessageBoxButtons.OKCancel);
                    if (boton == DialogResult.OK)
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("DefinicionFormulacion_Insertar", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idLinea", idlinea);
                        cmd.Parameters.AddWithValue("@idTipo", idtipo);
                        if (cbo.Text == "ACTIVO")
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

                        cbo.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //EVENTO DE BOTON PARA EJECUTAR LA FUNCION DE GUARDAR FORMULACION
        private void btnGuardar2_Click(object sender, EventArgs e)
        {
            AgregarDefinicionFormulacion(Convert.ToInt32(cboLinea.SelectedValue), Convert.ToInt32(cboTipo.SelectedValue), cboEstado);
        }

        //EDITAR UNA CUENTA DE MI BASE DE DATO
        public void EditarDefinicionFormulacion(int codigo, ComboBox cbo)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea editar el estado de esta definición de una formulación?.", "Validación de Sistema", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (Convert.ToString(codigo) == "N")
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
                        cmd = new SqlCommand("DefinicionFormulacion_Editar", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@codigo", codigo);

                        if (cbo.Text == "ACTIVO")
                        {
                            cmd.Parameters.AddWithValue("@estado", 1);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@estado", 0);
                        }

                        cmd.ExecuteNonQuery();
                        con.Close();
                        MostrarTodos();
                        MessageBox.Show("Se editó correctamente el registro.", "Edición", MessageBoxButtons.OK);
                        lineasrepetidas();

                        cbo.SelectedIndex = 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //EVENTO DE BOTON PARA EJECUTAR LA FUNCION DE EDITAR
        private void btnEditar2_Click(object sender, EventArgs e)
        {
            EditarDefinicionFormulacion(Convert.ToInt32(lblCodigo.Text), cboEstado);
        }

        //VALIDACION Y BUSQUEDA DE DEFINICIONES SEGUN CRITERIOS-----------
        private void cboBusqueda_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusqueda.Visible = true;
            txtBusqueda.Text = "";
        }

        //BUSQUEDA--------------------------------------------------------------------------------
        //METODO PARA REALIZAR LA BUSQUEDA POR TEXTO
        public void FiltrarDefinicionFormulacion(string busqueda, DataGridView dgvlistadodefin)
        {
            try
            {

                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("DefinicionFormulacion_BuscarPorCodigo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigo", busqueda);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dgvlistadodefin.DataSource = dt;
                con.Close();
                dgvlistadodefin.Columns[2].Visible = false;
                dgvlistadodefin.Columns[4].Visible = false;
                //
                dgvlistadodefin.Columns[0].Width = 90;
                dgvlistadodefin.Columns[1].Width = 60;
                dgvlistadodefin.Columns[3].Width = 240;
                dgvlistadodefin.Columns[5].Width = 240;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //EVENTO DE CAJA DE TEXTO QUE EJECITA MI BUSCAR DEFINICIONES DE FORMUYLACION
        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            FiltrarDefinicionFormulacion(txtBusqueda.Text, datalistadoDefinicionFormulacion);
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

        //EVENTO PARA VALIDAR SI EXISTE UNO IGUAL
        private void cboLinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            lineasrepetidas();
        }

        //EVENTO PARA VALIDAR SI EXISTE UNO IGUAL
        private void cboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lineasrepetidas();
        }

        //EVENTO PARA VALIDAR SI EXISTE UNO IGUAL
        private void cboEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            lineasrepetidas();
        }

        //VALIDAR QUE SOLO SE INGRESE NÚMEROS
        private void txtBusqueda_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }

            if (char.IsDigit(e.KeyChar))
            {
                int digitoscontados = txtBusqueda.Text.Count(char.IsDigit);
                if (digitoscontados >= 6)
                {
                    e.Handled = true;
                }
            }
        }
    }
}

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

namespace ArenasProyect3.Modulos.Procesos.Mantenimientos
{
    public partial class MantenimeintoMaquinarias : Form
    {
        //CREACIÓN DE VARIABLES PARA VALIDAR EL INGRESO DE MAQUINARIAS
        bool repetidoDescripcion;

        //CONSTRUCTOR DEL MANTENIMIENTO - MANTENIEMINTO DE MAQUINAS
        public MantenimeintoMaquinarias()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI MANTENIMIENTOS DE MAQUINARIAS
        private void MantenimeintoMaquinarias_Load(object sender, EventArgs e)
        {
            Mostrar();
            //ColorDescripcion();
            alternarColorFilas(datalistado);

            cboBusquedaMaquinara.SelectedIndex = 0;
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

        //METODO PARA VISUALIZAR LOS DATOS, LISTADO DE DATOS EN MI GRILLA
        public void Mostrar()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand("Maquinarias_Mostrar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistado.DataSource = dt;
                con.Close();
                datalistado.Columns[0].Width = 110;
                datalistado.Columns[1].Width = 140;
                datalistado.Columns[2].Width = 609;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //EVENTO DE DOBLE CLICK PARA PODER VISUALIZAR LOS DATOS DE UN REGISTRO
        private void datalistado_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            lblCodigo.Text = datalistado.SelectedCells[1].Value.ToString();
            txtDescripcion.Text = datalistado.SelectedCells[2].Value.ToString();
            string estado = datalistado.SelectedCells[0].Value.ToString();

            if (estado == "ACTIVO") { cboEstado.Text = "ACTIVO"; } else { cboEstado.Text = "INACTIVO"; }
            txtDescripcion.Enabled = false;

            btnEditar.Visible = true;
            btnEditar2.Visible = false;

            btnGuardar.Visible = true;
            btnGuardar2.Visible = false;

            Cancelar.Visible = false;
            lblCancelar.Visible = false;
        }

        //VALIDACIÓN DE EXISTENCIA DE LA MAQUINARIA A INGRESAR
        public void ColorDescripcion()
        {
            foreach (DataGridViewRow datorecuperado in datalistado.Rows)
            {
                string valor = Convert.ToString(datorecuperado.Cells["NOMBRE"].Value);
                if (valor == txtDescripcion.Text)
                {
                    txtDescripcion.ForeColor = Color.Red;
                    repetidoDescripcion = true;
                    return;
                }
                else
                {
                    txtDescripcion.ForeColor = Color.Green;
                    repetidoDescripcion = false;
                }
            }
        }

        //ACCIONES Y PROCESOS DEL MANTENIMIENTO*--------------------------------------
        //HABILITAR EL GUARDAR DE MI MANTENIMIENTO
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            txtDescripcion.Enabled = true;

            btnGuardar.Visible = false;
            btnGuardar2.Visible = true;

            Cancelar.Visible = true;
            lblCancelar.Visible = true;
            btnEditar.Enabled = true;

            cboEstado.Text = "ACTIVO";
            txtDescripcion.Text = "";

            lblCodigo.Text = "N";
        }

        //ACCION DE GAURDAR EN MI BASE DE DATOS LA NUEVA MAQUINARIA
        public void AgregarMaquinarias(string descripcion,string estado)
        {
            if (repetidoDescripcion == true)
            {
                MessageBox.Show("No se puede ingresar dos registros iguales.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                DialogResult boton = MessageBox.Show("¿Esta seguro que desea guardar esta maquinaria?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    if (descripcion != "")
                    {
                        try
                        {
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("Maquinarias_Insertar", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@descripcion", descripcion);

                            if (estado == "ACTIVO")
                            {
                                cmd.Parameters.AddWithValue("@estado", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@estado", 0);
                            }
                            cmd.ExecuteNonQuery();
                            con.Close();
                            Mostrar();
                            MessageBox.Show("Se ingresó el nuevo registro correctamente.", "Registro Nuevo", MessageBoxButtons.OK);
                            ColorDescripcion();

                            txtDescripcion.Enabled = false;

                            btnEditar.Visible = true;
                            btnEditar2.Visible = false;

                            btnGuardar.Visible = true;
                            btnGuardar2.Visible = false;

                            cboEstado.SelectedIndex = -1;
                            Cancelar.Visible = false;
                            lblCancelar.Visible = false;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Hubo un error inesperado, " + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe ingresar todos los campos necesarios.", "Validación del Sistema", MessageBoxButtons.OK);
                        txtDescripcion.Focus();
                    }
                }
            }
        }
        private void btnGuardar2_Click(object sender, EventArgs e)
        {
            AgregarMaquinarias(txtDescripcion.Text,cboEstado.Text);
        }

        //HABILITAR EL EDITADO DE MI MANTENIMIENTO
        private void btnEditar_Click(object sender, EventArgs e)
        {
            txtDescripcion.Enabled = true;

            btnEditar.Visible = false;
            btnEditar2.Visible = true;

            Cancelar.Visible = true;
            btnGuardar.Enabled = true;
        }

        //ACCION DE EDITADO EN MI BASE DE DATOS DE UNA MAQUINARIA
        public void EditarMaquinarias(int codigo, string descripcion, string estado)
        {
            if (descripcion != "" || Convert.ToString(codigo) != "N")
            {
                DialogResult boton = MessageBox.Show("¿Esta seguro que desea editar esta maquinaria?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Maquinarias_Editar", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@codigo", codigo);
                        cmd.Parameters.AddWithValue("@descripcion", descripcion);

                        if (estado == "ACTIVO")
                        {
                            cmd.Parameters.AddWithValue("@estado", 1);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@estado", 0);
                        }

                        cmd.ExecuteNonQuery();
                        con.Close();
                        Mostrar();
                        MessageBox.Show("Se editó correctamente el registro.", "Validación del Sistema", MessageBoxButtons.OK);
                        ColorDescripcion();

                        txtDescripcion.Enabled = false;

                        btnEditar.Visible = true;
                        btnEditar2.Visible = false;

                        btnGuardar.Visible = true;
                        btnGuardar2.Visible = false;

                        cboEstado.SelectedIndex = -1;
                        Cancelar.Visible = false;
                        lblCancelar.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Los campos no pueden estar vacios.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }
        private void btnEditar2_Click(object sender, EventArgs e)
        {
            EditarMaquinarias(Convert.ToInt32(lblCodigo.Text),txtDescripcion.Text,cboEstado.Text);
        }

        //ACCIÓN DE CANCELAR LA OPERACIÓN 
        private void Cancelar_Click(object sender, EventArgs e)
        {
            txtDescripcion.Enabled = false;

            btnEditar.Visible = true;
            btnEditar2.Visible = false;

            btnGuardar.Visible = true;
            btnGuardar2.Visible = false;

            Cancelar.Visible = false;
            lblCancelar.Visible = false;

            cboEstado.SelectedIndex = -1;
            lblCodigo.Text = "N";
            txtDescripcion.Text = "";
        }

        //VALIDACIONES Y BÚSQUEDAS DE MI MANTENIMIENTO MAQUINARIAS-------------------------
        //VALIDACIÓN DE LA NUEVA MAQUINARIA A INGRESAR
        private void txtDescripcion_TextChanged(object sender, EventArgs e)
        {
            ColorDescripcion();
        }

        //METODO PARA EXPORTAR A EXCEL MI LISTADO DE MAQUINARIA
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            ExportarDatos(datalistado);
        }

        //METODO PARA EXPORTAR A EXCEL
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

        //BÚSQUEDA DE OPERACIONES POR DESCRIPCIÓN - SENSITICO
        public void FiltrarMaquinarias(string cbobusquedamaquinarias, string busquedamaquinarias,DataGridView dgvlistado)
        {
            try
            {
                if (cbobusquedamaquinarias == "DESCRIPCIÓN")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Maquinarias_BuscarSegunDescripcion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@descripcion", busquedamaquinarias);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dgvlistado.DataSource = dt;
                    con.Close();
                    dgvlistado.Columns[0].Width = 110;
                    dgvlistado.Columns[1].Width = 140;
                    dgvlistado.Columns[2].Width = 609;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }
        private void txtBusquedaMaquinarias_TextChanged(object sender, EventArgs e)
        {
            FiltrarMaquinarias(cboBusquedaMaquinara.Text,txtBusquedaMaquinarias.Text,datalistado);
        }

        private void cboBusquedaMaquinara_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusquedaMaquinarias.Text = "";
        }
    }
}

using iTextSharp.text.pdf.codec.wmf;
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
    public partial class MantenimientoOperaciones : Form
    {
        //CREACIÓN DE VARIABLES PARA VALIDAR EL INGRESO DE OPERACIONES
        bool repetidoDescripcion;

        //CONSTRUCTOR DEL MANTENIMIENTO - MANTENIEMINTO DE OPERACIONES
        public MantenimientoOperaciones()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI MANTENIMIENTOS DE OPERACIONES
        private void MantenimientoOperaciones_Load(object sender, EventArgs e)
        {
            Mostrar();
            ColorDescripcion();
            alternarColorFilas(datalistado);

            cboBusquedaOperaciones.SelectedIndex = 0;
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
                SqlCommand cmd = new SqlCommand("Operaciones_Mostrar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistado.DataSource = dt;
                con.Close();
                datalistado.Columns[0].Width = 120;
                datalistado.Columns[1].Width = 150;
                datalistado.Columns[2].Width = 422;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //ACCION DE DOBLE CLICK PARA PODER TRAER LOS DATOS DEL REGISTRO SELECIOANDO
        private void datalistado_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            lblCodigo.Text = datalistado.SelectedCells[1].Value.ToString();
            txtDescripcion.Text = datalistado.SelectedCells[2].Value.ToString();
            string estado = datalistado.SelectedCells[0].Value.ToString();

            if (estado == "ACTIVO")
            {
                cboEstado.Text = "ACTIVO";
            }
            else
            {
                cboEstado.Text = "INACTIVO";
            }

            txtDescripcion.Enabled = false;

            btnEditar.Visible = true;
            btnEditar2.Visible = false;

            btnGuardar.Visible = true;
            btnGuardar2.Visible = false;

            Cancelar.Visible = false;
        }

        //METODO PARA LA VALIDACIÓN DE LA EXISTENCIA DE UNA OPERACIÓN
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
            btnEditar.Enabled = true;

            cboEstado.Text = "ACTIVO";
            txtDescripcion.Text = "";

            lblCodigo.Text = "N";
        }

        //ACCION DE GAURDAR EN MI BASE DE DATOS LA NUEVA OPERACIÓN
        public void AgregarOperacion(string descripcion, string estado)
        {
            if (repetidoDescripcion == true)
            {
                MessageBox.Show("No se puede ingresar dos registros iguales.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                DialogResult boton = MessageBox.Show("¿Esta seguro que desea guardar esta operación?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
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
                            cmd = new SqlCommand("Operaciones_Insertar", con);
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

                            //BLOQUEO DEL TEXTBOX DESCRIPCIÓN
                            txtDescripcion.Enabled = false;

                            btnEditar.Visible = true;
                            btnEditar2.Visible = false;

                            btnGuardar.Visible = true;
                            btnGuardar2.Visible = false;

                            cboEstado.SelectedIndex = -1;
                            Cancelar.Visible = false;
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
            AgregarOperacion(txtDescripcion.Text,cboEstado.Text);
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

        //METODO DE EDICION EN MI BASE DE DATOS PARA UNA OPERACION
        public void EditarOperaciones(int codigo, string descripcion, string estado)
        {
            if (descripcion != "" || Convert.ToString(codigo) != "N")
            {
                DialogResult boton = MessageBox.Show("¿Esta seguro que desea editar esta operación?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Operaciones_Editar", con);
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
                        MessageBox.Show("Se editó correctamente el registro.", "Edición", MessageBoxButtons.OK);
                        ColorDescripcion();

                        txtDescripcion.Enabled = false;

                        btnEditar.Visible = true;
                        btnEditar2.Visible = false;

                        btnGuardar.Visible = true;
                        btnGuardar2.Visible = false;

                        cboEstado.SelectedIndex = -1;
                        Cancelar.Visible = false;
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
            EditarOperaciones(Convert.ToInt32(lblCodigo.Text),txtDescripcion.Text,cboEstado.Text);
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

            cboEstado.SelectedIndex = -1;
            lblCodigo.Text = "N";
            txtDescripcion.Text = "";
        }

        //VALIDACIONES Y BÚSQUEDAS DE MI MANTENIMIENTO OPERACIONES-------------------------
        //VALIDACIÓN DE LA NUEVA OPERACIÓN A INGRESAR
        private void txtDescripcion_TextChanged(object sender, EventArgs e)
        {
            ColorDescripcion();
        }

        //METODO PARA EXPORTAR A EXCEL MI LISTADO DE OPERACIONES
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
        public void FiltrarOperaciones(string cbobusquedaoperaciones, string busquedaoperaciones,DataGridView dgv)
        {
            try
            {
                if(busquedaoperaciones == "")
                {
                    Mostrar();
                }
                else
                {
                    if (cbobusquedaoperaciones == "DESCRIPCIÓN")
                    {
                        DataTable dt = new DataTable();
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Operaciones_BuscarSegunDescripcion", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@descripcion", busquedaoperaciones);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        dgv.DataSource = dt;
                        con.Close();
                        dgv.Columns[0].Width = 120;
                        dgv.Columns[1].Width = 150;
                        dgv.Columns[2].Width = 422;
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Operaciones_BuscarPorCodigo", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idoperacion", busquedaoperaciones);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        dgv.DataSource = dt;
                        con.Close();
                        dgv.Columns[0].Width = 120;
                        dgv.Columns[1].Width = 150;
                        dgv.Columns[2].Width = 422;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }
        private void txtBusquedaOperaciones_TextChanged(object sender, EventArgs e)
        {
            FiltrarOperaciones(cboBusquedaOperaciones.Text, txtBusquedaOperaciones.Text,datalistado);
        }

        private void cboBusquedaOperaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusquedaOperaciones.Text = "";
        }

        private void txtBusquedaOperaciones_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cboBusquedaOperaciones.Text == "CODIGO")
            {
                //VERIFICA EL INGRESO DE CONTROLES O NÚMEROS EN EL CAMPO DE BÚSQUEDA
                if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
                {
                    e.Handled = false;  //SI LA TECLA PRESIONADA ES UN CONTROL O UN NÚMERO, SE PERMITE
                }
                else
                {
                    e.Handled = true;  //SINO SE BLOQUEA Y NO SE INGRESA
                }

                //VERIFICA CUANTOS NUMEROS SE HA INGRESADO EN EL CAMPO DE BÚSQUEDA
                if (char.IsDigit(e.KeyChar))
                {
                    int digitoscontados = txtBusquedaOperaciones.Text.Count(char.IsDigit);
                    if (digitoscontados >= 6)
                    {
                        e.Handled = true;  //SI HAY MAS DE 3 DIGITOS, SE BLOQUEA
                    }
                }
            }
        }
    }
}

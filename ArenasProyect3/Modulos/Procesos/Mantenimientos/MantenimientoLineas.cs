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
    public partial class MantenimientoLineas : Form
    {
        //VARIABLES DE VALIDACIÓN PARA EL INGRESO Y EDICIÓN DE DATOS
        bool repetidoDescripcion;
        bool repetidoAbreviatura;

        //CONSTRUCTOR DEL MANTENIMIENTO - MANTENIEMINTO DE LINEAS
        public MantenimientoLineas()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI MANTENIMIENTOS DE LINEAS
        private void MantenimientoLineas_Load(object sender, EventArgs e)
        {
            CargarTipoMercaderia();
            ColorDescripcion();
            alternarColorFilas(datalistadoLineas);
            cboBusquedaLinea.SelectedIndex = 1;
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

        //CARGA DE DATOS - TIPO DE CUENTAS O TIPO DE MERCADERIAS
        public void CargarTipoMercaderia()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdTipoMercaderias,Desciripcion FROM TIPOMERCADERIAS WHERE Estado = 1", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboTipoMercaderia.DisplayMember = "Desciripcion";
                cboTipoMercaderia.ValueMember = "IdTipoMercaderias";
                cboTipoMercaderia.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //BÚSQUEDA DE LINEAS SEGÚN EL TIPO DE MERCADERIA SELECIONARA - EVENTO SELECCIÓN
        private void cboTipoMercaderia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Lineas_MostrarSegunCuenta", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cuenta", cboTipoMercaderia.SelectedValue.ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoLineas.DataSource = dt;
                con.Close();
                ReordenarFilas(datalistadoLineas);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //MOSTRAR TODAS MIS LÍNEAS SUGUN EL TIPO DE CUENTA SELECCIOANDO - METODO
        public void Mostrar(int cuenta)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Lineas_MostrarSegunCuenta", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cuenta", cuenta);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoLineas.DataSource = dt;
                con.Close();
                ReordenarFilas(datalistadoLineas);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //FUNCION PARA ORDENAR LAS FILAS
        public void ReordenarFilas(DataGridView DGV)
        {
            DGV.Columns[0].Width = 75;
            DGV.Columns[1].Width = 75;
            DGV.Columns[2].Width = 90;
            DGV.Columns[3].Width = 250;
            DGV.Columns[4].Visible = false;
            DGV.Columns[5].Width = 240;
        }

        //EVENTO DE DOBLE CLICK PARA EN MI LISTADO DE LINEAS
        private void datalistadoLineas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            lblCodigo.Text = datalistadoLineas.SelectedCells[1].Value.ToString();
            txtAbreviatura.Text = datalistadoLineas.SelectedCells[2].Value.ToString();
            txtDescripcion.Text = datalistadoLineas.SelectedCells[3].Value.ToString();
            cboTipoMercaderia.SelectedValue = datalistadoLineas.SelectedCells[4].Value.ToString();
            string estado = datalistadoLineas.SelectedCells[0].Value.ToString();

            if (estado == "ACTIVO")
            {
                cboEstado.Text = "ACTIVO";
            }
            else
            {
                cboEstado.Text = "INACTIVO";
            }

            txtDescripcion.Enabled = false;
            txtAbreviatura.Enabled = false;

            btnEditarF.Visible = true;
            btnEditar2F.Visible = false;

            btnGuardarF.Visible = true;
            btnGuardar2F.Visible = false;

            CancelarF.Visible = false;
            lblCancelar.Visible = false;
        }

        //VALIDACIÓN EL SISTEMA PARA PODER AVERIGUAR SI YA EXISTE OTRO REGISTRO CON LA MISMA DESCRIPCION
        public void ColorDescripcion()
        {
            foreach (DataGridViewRow datorecuperado in datalistadoLineas.Rows)
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

        //VALIDACIÓN EL SISTEMA PARA PODER AVERIGUAR SI YA EXISTE OTRO REGISTRO CON LA MISMA ABREVIATURA
        public void ColorAbreviatura()
        {
            foreach (DataGridViewRow datorecuperado in datalistadoLineas.Rows)
            {
                string valor = Convert.ToString(datorecuperado.Cells["ABREVIATURA"].Value);
                valor = valor.Trim();

                if (valor == txtAbreviatura.Text)
                {
                    txtAbreviatura.ForeColor = Color.Red;
                    repetidoAbreviatura = true;
                    return;
                }
                else
                {
                    txtAbreviatura.ForeColor = Color.Green;
                    repetidoAbreviatura = false;
                }
            }
        }

        //HABILITAR GUARDADO DE UNA NUEVA LÍNEA
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            txtDescripcion.Enabled = true;
            txtAbreviatura.Enabled = true;

            btnGuardarF.Visible = false;
            btnGuardar2F.Visible = true;

            CancelarF.Visible = true;
            lblCancelar.Visible = true;
            btnEditarF.Enabled = true;

            cboEstado.Text = "ACTIVO";
            txtDescripcion.Text = "";
            txtAbreviatura.Text = "";

            lblCodigo.Text = "N";
        }

        public void AgregarLineas(string descripcion, string abreviatura, string codigotipomercaderia)
        {
            if (repetidoDescripcion == true)
            {
                MessageBox.Show("No se puede ingresar dos registros iguales.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                DialogResult boton = MessageBox.Show("¿Esta seguro que desea guardar esta línea?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    if (descripcion != "" || abreviatura != "")
                    {
                        try
                        {
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("Lineas_Insertar", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@descripcion", descripcion);
                            cmd.Parameters.AddWithValue("@abreviatura", abreviatura);
                            cmd.Parameters.AddWithValue("@codigotipomercaderia", codigotipomercaderia);
                            if(cboEstado.Text == "ACTIVO")
                            {
                                cmd.Parameters.AddWithValue("@estado", 1);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@estado", 0);
                            }

                            cmd.ExecuteNonQuery();
                            con.Close();

                            int cuenta = Convert.ToInt32(codigotipomercaderia);
                            Mostrar(cuenta);
                            MessageBox.Show("Se ingresÓ el nuevo registro correctamente.", "Registro Nuevo", MessageBoxButtons.OK);
                            CamposBloqueados();
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
        //GUARDAR UNA NUEVA LÍNEA EN MI BASE DE DATOS
        private void btnGuardar2_Click(object sender, EventArgs e)
        {
            AgregarLineas(txtDescripcion.Text, txtAbreviatura.Text, cboTipoMercaderia.SelectedValue.ToString());
        }

        //HABILITAR EDICIÓN PARA MODIFICAR UNA LÍNEA YA INGRESADA
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (lblCodigo.Text == "N" || lblCodigo.Text == "")
            {
                MessageBox.Show("Debe seleccionar un registro para poder editar.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                txtDescripcion.Enabled = true;
                txtAbreviatura.Enabled = true;

                btnEditarF.Visible = false;
                btnEditar2F.Visible = true;

                CancelarF.Visible = true;
                lblCancelar.Visible = true;
                btnGuardarF.Enabled = true;
            }
        }
        public void EditarLineas(string codigo,string descripcion, string abreviatura, string idtipomercaderia)
        {
            if (descripcion != "" || abreviatura != "" || codigo != "N")
            {
                DialogResult boton = MessageBox.Show("¿Esta seguro que desea editar esta línea?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("Lineas_Editar", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@codigo", Convert.ToInt32(codigo));
                        cmd.Parameters.AddWithValue("@descripcion", descripcion);
                        cmd.Parameters.AddWithValue("@abreviatura", abreviatura);
                        cmd.Parameters.AddWithValue("@idtipomercaderia", idtipomercaderia);

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

                        int cuenta = Convert.ToInt32(idtipomercaderia);
                        Mostrar(cuenta);

                        MessageBox.Show("Se editó correctamente el registro.", "Edición", MessageBoxButtons.OK);

                        CamposBloqueados();
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
        //EDITAR UNA CUENTA DE MI BASE DE DATOS
        private void btnEditar2_Click(object sender, EventArgs e)
        {
            EditarLineas(lblCodigo.Text,txtDescripcion.Text,txtAbreviatura.Text,cboTipoMercaderia.SelectedValue.ToString());
        }

        //CACELAR ACCIÓN DE GUARDADO O EDITADO
        private void Cancelar_Click(object sender, EventArgs e)
        {
            CamposBloqueados();
            cboEstado.SelectedIndex = -1;
            lblCodigo.Text = "N";
            txtDescripcion.Text = "";
            txtAbreviatura.Text = "";
        }

        //FUNCION PARA BLOQUEAR LOS CAMPOS
        public void CamposBloqueados()
        {
            txtDescripcion.Enabled = false;
            txtAbreviatura.Enabled = false;

            btnEditarF.Visible = true;
            btnEditar2F.Visible = false;

            btnGuardarF.Visible = true;
            btnGuardar2F.Visible = false;

            CancelarF.Visible = false;
            lblCancelar.Visible = false;
        }

        //VALIDACIONES DE INGRESO DE DATOS Y EXISTENCOA DE ESTOS-----------------------------
        //VALIDAR LA DIGITACIÓN DE UNA CUENTA
        private void txtDescripcion_TextChanged(object sender, EventArgs e)
        {
            ColorDescripcion();
        }

        //VALIDAR LA DIGITACIÓN DE UNA ABREVIATURA
        private void txtAbreviatura_TextChanged(object sender, EventArgs e)
        {
            ColorAbreviatura();
        }

        //LLAMADO DE UN METODO PARA EXPORTAR A EXCEL EL LISTADO DE CUENTAS
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            ExportarDatos(datalistadoLineas);
        }
        public void FiltrarLineas(string busquedalinea,DataGridView dgv, ComboBox cbo)
        {
            try
            {
                if (cbo.Text == "DESCRIPCIÓN")
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Lineas_BusquedaPorDescripcion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@descripcion", busquedalinea);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dgv.DataSource = dt;
                    con.Close();
                    ReordenarFilas(dgv);
                }
                else
                {
                    DataTable dt = new DataTable();
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("Lineas_BusquedaPorAbreviatura", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@abreviatura", busquedalinea);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dgv.DataSource = dt;
                    con.Close();
                    ReordenarFilas(dgv);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //BÚSQUEDA DE LINEAS SEGUN LA DESCIPCIÓN O LA ABREVIATURA
        private void txtBusquedaLinea_TextChanged(object sender, EventArgs e)
        {
            FiltrarLineas(txtBusquedaLinea.Text,datalistadoLineas,cboBusquedaLinea);
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

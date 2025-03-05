﻿using System;
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
    public partial class MantenimientoCuentas : Form
    {
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        bool repetidoDescripcion;
        bool repetidoAbreviatura;

        //CONSTRUCTOR DEL MANTENIMIENTO - MANTENIEMINTO DE CUENTAS
        public MantenimientoCuentas()
        {
            InitializeComponent();
        }

        //INICIO Y CARGA INICIAL DEL REQUERIMEINTO - CONSTRUCTOR--------------------------------------------------------------------------------------
        private void MantenimientoCuentas_Load(object sender, EventArgs e)
        {
            Mostrar();
            ColorDescripcion();
            alternarColorFilas(datalistadoTipomer);
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

        //METODO PARA LISTAR TODAS MIS CUENTAS GUARDADAS EN EL SISTEMA
        public void Mostrar()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            da = new SqlDataAdapter("SELECT Case When Estado = 1 Then 'ACTIVO' Else 'INCATIVO' End As ESTADO, IdTipoMercaderias AS [CÓDIGO], Desciripcion AS [NOMBRE], Abreviatura AS [ABREVIATURA], CodSunet AS [C. SUNAT] FROM TIPOMERCADERIAS", con);
            da.Fill(dt);
            datalistadoTipomer.DataSource = dt;
            con.Close();
            datalistadoTipomer.Columns[0].Width = 90;
            datalistadoTipomer.Columns[1].Width = 90;
            datalistadoTipomer.Columns[2].Width = 325;
            datalistadoTipomer.Columns[3].Width = 90;
            datalistadoTipomer.Columns[4].Width = 90;
        }

        //DOBLE CLICK EN EL REGISTRO PARA MOSTRAR DETALLES DE MI CUENTA
        private void datalistadoTipomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            lblCodigo.Text = datalistadoTipomer.SelectedCells[1].Value.ToString();
            txtDescripcion.Text = datalistadoTipomer.SelectedCells[2].Value.ToString();
            txtAbreviatura.Text = datalistadoTipomer.SelectedCells[3].Value.ToString();
            txtCodSunat.Text = datalistadoTipomer.SelectedCells[4].Value.ToString();
            string estado = datalistadoTipomer.SelectedCells[0].Value.ToString();

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
            txtCodSunat.Enabled = false;

            btnEditarF.Visible = true;
            btnEditar2F.Visible = false;

            btnGuardar.Visible = true;
            btnGuardar2F.Visible = false;

            CancelarF.Visible = false;
            lblCancelar.Visible = false;
        }

        //VALIDACIÓND EL SISTEMA PARA PODER AVERIGUAR SI YA EXISTE OTRO REGISTRO CON LA MISMA DESCRIPCION
        public void ColorDescripcion()
        {
            foreach (DataGridViewRow datorecuperado in datalistadoTipomer.Rows)
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

        //VALIDACIÓND EL SISTEMA PARA PODER AVERIGUAR SI YA EXISTE OTRO REGISTRO CON LA MISMA ABREVIATURA
        public void ColorAbreviatura()
        {
            foreach (DataGridViewRow datorecuperado in datalistadoTipomer.Rows)
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

        //HABILITAR GUARDADO DE UNA NUEVA CUENTA
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            txtDescripcion.Enabled = true;
            txtAbreviatura.Enabled = true;
            txtCodSunat.Enabled = true;

            btnGuardar.Visible = false;
            btnGuardar2F.Visible = true;

            CancelarF.Visible = true;
            lblCancelar.Visible = true;
            btnEditarF.Enabled = true;

            cboEstado.Text = "ACTIVO";
            txtDescripcion.Text = "";
            txtAbreviatura.Text = "";
            txtCodSunat.Text = "";

            lblCodigo.Text = "N";
        }

        //GUARDAR UNA NUEVA CUENTA EN MI BASE DE DATOS
        private void btnGuardar2_Click(object sender, EventArgs e)
        {
            if (repetidoDescripcion == true)
            {
                MessageBox.Show("No se puede ingresar dos registros iguales.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                if (txtDescripcion.Text != "" || txtAbreviatura.Text != "" || txtCodSunat.Text != "")
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("InsertarCuentas", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text);
                    cmd.Parameters.AddWithValue("@abreviatura", txtAbreviatura.Text);
                    cmd.Parameters.AddWithValue("@codigosunat", txtCodSunat.Text);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Mostrar();
                    MessageBox.Show("Se ingresó el nuevo registro correctamente.", "Registro nuevo", MessageBoxButtons.OK);
                    ColorDescripcion();

                    txtDescripcion.Enabled = false;
                    txtAbreviatura.Enabled = false;
                    txtCodSunat.Enabled = false;

                    btnEditarF.Visible = true;
                    btnEditar2F.Visible = false;

                    btnGuardar.Visible = true;
                    btnGuardar2F.Visible = false;

                    cboEstado.SelectedIndex = -1;
                    CancelarF.Visible = false;
                    lblCancelar.Visible = false;
                }
                else
                {
                    MessageBox.Show("Debe ingresar todos los campos necesarios.", "Validación del Sistema", MessageBoxButtons.OK);
                    txtDescripcion.Focus();
                }
            }
        }

        //HABILITAR EDICIÓN PARA MODIFICAR UNA CUENTA YA INGRESADA
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
                txtCodSunat.Enabled = true;

                btnEditarF.Visible = false;
                btnEditar2F.Visible = true;

                CancelarF.Visible = true;
                lblCancelar.Visible = true;
                btnGuardar.Enabled = true;
            }
        }

        //EDITAR UNA CUENTA DE MI BASE DE DATOS
        private void btnEditar2_Click(object sender, EventArgs e)
        {
            if (txtDescripcion.Text != "" || txtAbreviatura.Text != "" || txtCodSunat.Text != "" || lblCodigo.Text != "N")
            {
                try
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("EditarCuenta", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@codigo", Convert.ToInt32(lblCodigo.Text));
                    cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text);
                    cmd.Parameters.AddWithValue("@abreviatura", txtAbreviatura.Text);
                    cmd.Parameters.AddWithValue("@codigosunat", txtCodSunat.Text);

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
                    Mostrar();
                    MessageBox.Show("Se editó correctamente el registro.", "Nueva Edición", MessageBoxButtons.OK);
                    ColorDescripcion();

                    txtDescripcion.Enabled = false;
                    txtAbreviatura.Enabled = false;
                    txtCodSunat.Enabled = false;

                    btnEditarF.Visible = true;
                    btnEditar2F.Visible = false;

                    btnGuardar.Visible = true;
                    btnGuardar2F.Visible = false;

                    cboEstado.SelectedIndex = -1;
                    CancelarF.Visible = false;
                    lblCancelar.Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Los campos no pueden estar vacios.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //CACELAR ACCIÓN DE GUARDADO O EDITADO
        private void Cancelar_Click(object sender, EventArgs e)
        {
            txtDescripcion.Enabled = false;
            txtAbreviatura.Enabled = false;
            txtCodSunat.Enabled = false;

            btnEditarF.Visible = true;
            btnEditar2F.Visible = false;

            btnGuardar.Visible = true;
            btnGuardar2F.Visible = false;

            CancelarF.Visible = false;
            lblCancelar.Visible = false;

            cboEstado.SelectedIndex = -1;
            lblCodigo.Text = "N";
            txtDescripcion.Text = "";
            txtAbreviatura.Text = "";
            txtCodSunat.Text = "";
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
            ExportarDatos(datalistadoTipomer);
        }

        //SOLO INGRESO DE NÚMEROS Y DECIMALES A MI CAMPO CÓDIGO SUNAT
        private void txtCodSunat_KeyPress(object sender, KeyPressEventArgs e)
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

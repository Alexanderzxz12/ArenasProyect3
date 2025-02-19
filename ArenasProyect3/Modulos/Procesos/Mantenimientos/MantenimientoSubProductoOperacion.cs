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
    public partial class MantenimientoSubProductoOperacion : Form
    {
        //VARIABLES PARA DEFINIR DATOS Y PARA VALIDAR REPETICIONES
        string idmodelo1;
        string idmodelo2;
        string idoperacion2;
        int idmodeloxoperacion1;
        int idmodeloxoperacionxmequinaria;
        bool DetalleRepetido = false;
        bool DetalleRepetido2 = false;

        //CONSTRUCTOR DEL MANTENIMIENTO - MANTENIEMINTO DE MODELO POR OPERACION
        public MantenimientoSubProductoOperacion()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI MANTENIMIENTOS DE MODELOS POR OPERACIONES
        private void MantenimientoSubProductoOperacion_Load(object sender, EventArgs e)
        {
            CargarModelo1();
            CargarOperacion1();
            CargarModelo2();
            CargarMaquinarias();

            alternarColorFilas(datalistadoModeloXOperacion);
            alternarColorFilas(datalistadoModeloXOperacionXMaquinaria);
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

        //CARGA DE CAMPOS---------------------------------------------------------------------
        //PRIMERA PARTE-------------
        //CARGA DE MODELO N1
        public void CargarModelo1()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdModelo, Descripcion FROM MODELOS WHERE Estado = 1 ORDER BY Descripcion", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboModelo1.ValueMember = "IdModelo";
            cboModelo1.DisplayMember = "Descripcion";
            cboModelo1.DataSource = dt;
        }

        //CATGA DE OPERACIONES N1
        public void CargarOperacion1()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdOperaciones, Descripcion FROM Operaciones where Estado = 1 ORDER BY Descripcion", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboOperacion1.ValueMember = "IdOperaciones";
            cboOperacion1.DisplayMember = "Descripcion";
            cboOperacion1.DataSource = dt;
        }

        //SEGUNDA PARTE-------------
        //CARGA DE MODELO N2
        public void CargarModelo2()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdModelo, Descripcion FROM MODELOS WHERE Estado = 1 ORDER BY Descripcion", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboModelo2.ValueMember = "IdModelo";
            cboModelo2.DisplayMember = "Descripcion";
            cboModelo2.DataSource = dt;
        }

        //CATGA DE OPERACIONES N2
        public void CargarOperacion2(string idmodelo)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT MxO.IdModeloXOperacion,O.IdOperaciones, O.Descripcion FROM ModeloXOperacion MxO INNER JOIN Operaciones O ON O.IdOperaciones = MxO.IdOperacion where MxO.Estado = 1 AND IdModelo = @idmodelo ORDER BY Descripcion", con);
            comando.Parameters.AddWithValue("@idmodelo", idmodelo);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboOperacion2.ValueMember = "O.IdOperaciones";
            cboOperacion2.DisplayMember = "Descripcion";
            cboOperacion2.DataSource = dt;
        }

        //CARGA DE MAQUINARIAS N2
        public void CargarMaquinarias()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdMaquinarias,Descripcion from Maquinarias where Estado = 1 ORDER BY Descripcion", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboMaquinaria2.ValueMember = "IdMaquinarias";
            cboMaquinaria2.DisplayMember = "Descripcion";
            cboMaquinaria2.DataSource = dt;
        }

        //VALIDACIONES-------------------------------------------------------------------
        //VALIDACIÓN POR SI EXISTE MODELO POR OPERACIÓN PARA EL PRIMERO
        public void ValidarExisitencia1()
        {
            foreach (DataGridViewRow datorecuperado in datalistadoModeloXOperacion.Rows)
            {
                string modelo = Convert.ToString(datorecuperado.Cells["MODELO"].Value);
                string operacion = Convert.ToString(datorecuperado.Cells["OPERACIÓN"].Value);
                if (modelo == cboModelo1.Text)
                {
                    if (operacion == cboOperacion1.Text)
                    {
                        DetalleRepetido = true;
                        return;
                    }
                    else
                    {
                        DetalleRepetido = false;
                    }
                }
            }
        }

        //VALIDACIÓN POR SI EXISTE MODELO POR OPERACIÓN PARA EL SEGUNDO CON MAQUINARIA
        public void ValidarExisitencia2()
        {
            foreach (DataGridViewRow datorecuperado in datalistadoModeloXOperacionXMaquinaria.Rows)
            {
                string modelo = Convert.ToString(datorecuperado.Cells["MODELO"].Value);
                string operacion = Convert.ToString(datorecuperado.Cells["OPERACIÓN"].Value);
                string maquinaria = Convert.ToString(datorecuperado.Cells["MAQUINARIA"].Value);
                if (modelo == cboModelo2.Text)
                {
                    if (operacion == cboOperacion2.Text)
                    {
                        if (maquinaria == cboMaquinaria2.Text)
                        {
                            DetalleRepetido2 = true;
                            return;
                        }
                        else
                        {
                            DetalleRepetido2 = false;
                        }
                    }
                }
            }
            DetalleRepetido2 = false;
        }

        //MOSTRAR RESULTADOS EN LAS GRILLAS------------------------------------------------
        //MOSTRAR MODELO POR OPERACIÓN SEGUN EL MODELO SELECCIOANDO
        public void Mostrar1(string idmodelo)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT MO.IdModeloxOperacion, M.IdModelo,O.IdOperaciones,M.Descripcion AS MODELO,O.Descripcion AS OPERACIÓN FROM ModeloxOperacion MO INNER JOIN Modelos M ON M.IdModelo = MO.IdModelo INNER JOIN Operaciones O ON O.IdOperaciones = MO.IdOperacion WHERE MO.Estado = 1 AND MO.IdModelo = @idmodelo", con);
            comando.Parameters.AddWithValue("@idmodelo", idmodelo);
            da = new SqlDataAdapter(comando);
            da.Fill(dt);
            datalistadoModeloXOperacion.DataSource = dt;
            con.Close();
            datalistadoModeloXOperacion.Columns[0].Visible = false;
            datalistadoModeloXOperacion.Columns[1].Visible = false;
            datalistadoModeloXOperacion.Columns[2].Visible = false;
            datalistadoModeloXOperacion.Columns[3].Width = 150;
            datalistadoModeloXOperacion.Columns[4].Width = 330;
        }

        //MOSTRAR MODELO POR OPERACIÓN POR MAQUINARIA SEGÚN EL MODELO Y LA OPERACIÓN SELECCIONADA
        public void Mostrar2(string idmodelo, string idoperacion)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT MOM.IdModeloXOperacionXMaquinaria,MOM.IdModelo,M.Descripcion AS MODELO,MOM.IdOperacion,O.Descripcion AS OPERACIÓN,MOM.IdMaquinaria,MA.Descripcion AS MAQUINARIA FROM ModeloxOperacionxMaquinaria MOM INNER JOIN Modelos M ON M.IdModelo = MOM.IdModelo INNER JOIN Operaciones O ON O.IdOperaciones = MOM.IdOperacion INNER JOIN Maquinarias MA ON MA.IdMaquinarias = MOM.IdMaquinaria WHERE MOM.IdModelo = @idmodelo AND MOM.IdOperacion = @idoperacion AND MOM.Estado = 1", con);
            comando.Parameters.AddWithValue("@idmodelo", idmodelo);
            comando.Parameters.AddWithValue("@idoperacion", idoperacion);
            da = new SqlDataAdapter(comando);
            da.Fill(dt);
            datalistadoModeloXOperacionXMaquinaria.DataSource = dt;
            con.Close();
            datalistadoModeloXOperacionXMaquinaria.Columns[0].Visible = false;
            datalistadoModeloXOperacionXMaquinaria.Columns[1].Visible = false;
            datalistadoModeloXOperacionXMaquinaria.Columns[3].Visible = false;
            datalistadoModeloXOperacionXMaquinaria.Columns[5].Visible = false;
            datalistadoModeloXOperacionXMaquinaria.Columns[2].Width = 120;
            datalistadoModeloXOperacionXMaquinaria.Columns[4].Width = 250;
            datalistadoModeloXOperacionXMaquinaria.Columns[6].Width = 260;
        }

        //VISUALIZACION DEL REGISTRO SELECCIONADO----------------------------------------------
        //ACCIÓN DE DOBLE CLICK PARA LA GRILLA DE MODELO X OPERACION
        private void datalistadoModeloXOperacion_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            idmodeloxoperacion1 = Convert.ToInt32(datalistadoModeloXOperacion.SelectedCells[0].Value.ToString());
            cboModelo1.SelectedValue = datalistadoModeloXOperacion.SelectedCells[1].Value.ToString();
            cboOperacion1.SelectedValue = datalistadoModeloXOperacion.SelectedCells[2].Value.ToString();
        }

        //ACCIÓN DE DOBLE CLICK PARA LA GRILLA DE MODELO X OPERACION X MAQUINARIA
        private void datalistadoModeloXOperacionXMaquinaria_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            idmodeloxoperacionxmequinaria = Convert.ToInt32(datalistadoModeloXOperacionXMaquinaria.SelectedCells[0].Value.ToString());
            cboModelo2.SelectedValue = datalistadoModeloXOperacionXMaquinaria.SelectedCells[1].Value.ToString();
            cboOperacion2.SelectedValue = datalistadoModeloXOperacionXMaquinaria.SelectedCells[3].Value.ToString();
            cboMaquinaria2.SelectedValue = datalistadoModeloXOperacionXMaquinaria.SelectedCells[5].Value.ToString();
        }

        //SELECCION DE UN MODELO----------------
        private void cboModelo1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboModelo1.SelectedValue.ToString() != null)
            {
                idmodelo1 = cboModelo1.SelectedValue.ToString();
                Mostrar1(idmodelo1);
            }
        }

        //ACCIONES DE CRUD PRIMERA PARTE----------------------------------------------------------
        //METODO PARA GAURDAR MODELO X OPERACIÓN
        private void btnGuardar1_Click(object sender, EventArgs e)
        {
            ValidarExisitencia1();

            if (DetalleRepetido == false)
            {
                try
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = Conexion.ConexionMaestra.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("InsertarModeloxOperacion", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@idmodelo", Convert.ToInt32(cboModelo1.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@idoperacion", Convert.ToInt32(cboOperacion1.SelectedValue.ToString()));

                    cmd.ExecuteNonQuery();
                    con.Close();
                    Mostrar1(idmodelo1);
                    CargarOperacion2(idmodelo1);
                    MessageBox.Show("Registro ingresado exitosamente", "Validación del Sistema", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("El registro que intenta insertar ya se encuentra en el sistema", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //METODO PARA ELIMINAR MODELO X OPERACIÓN
        private void btnEliminar1_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("Realmente desea eliminar", "Validación del Sistema", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (datalistadoModeloXOperacion.CurrentRow != null)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("EliminarModeloxOperacion", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", idmodeloxoperacion1);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        Mostrar1(idmodelo1);
                        MessageBox.Show("Eliminación correcta, operación echa satisfactoriamente", "Validación del Sistema", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un registro para poder eliminar", "Validación del Sistema", MessageBoxButtons.OK);
                }
            }
        }

        //MOSTREO DE DATOS CON FILTROS---------------------------------------------------------
        //GUARDAR MODELO SELECCIONADA Y GUARDARLA PARA MOSRARLA
        private void cboModelo2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboModelo2.SelectedValue.ToString() != null)
            {
                idmodelo2 = cboModelo2.SelectedValue.ToString();
                CargarOperacion2(idmodelo2);
            }
        }

        //METODO PARA MOSTRAR REGISTROS SEGÍN MODELO Y OPERACIÓN
        private void cboOperacion2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboOperacion2.SelectedValue.ToString() != null)
            {
                idoperacion2 = cboOperacion2.SelectedValue.ToString();
                Mostrar2(idmodelo2, idoperacion2);
            }
        }

        //ACCIONES DE CRUD SEGUNDA PARTE----------------------------------------------------------
        //METODO PARA GAURDAR MODELO X OPERACIÓN X MAQUINARIA
        private void btnGuardar2_Click(object sender, EventArgs e)
        {
            ValidarExisitencia2();

            if (DetalleRepetido2 == false)
            {
                try
                {
                    if (cboOperacion2.SelectedValue == null || cboOperacion2.Text == "")
                    {
                        MessageBox.Show("No se puede ingresar sin escoger una operación", "Validación del Sistema", MessageBoxButtons.OK);
                    }
                    else
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("InsertarModeloxOperacionxMaquinaria", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idmodelo", Convert.ToInt32(cboModelo2.SelectedValue.ToString()));
                        cmd.Parameters.AddWithValue("@idoperacion", Convert.ToInt32(cboOperacion2.SelectedValue.ToString()));
                        cmd.Parameters.AddWithValue("@idmaquinaria", Convert.ToInt32(cboMaquinaria2.SelectedValue.ToString()));

                        cmd.ExecuteNonQuery();
                        con.Close();
                        Mostrar2(idmodelo2, idoperacion2);
                        MessageBox.Show("Registro ingresado exitosamente", "Validación del Sistema", MessageBoxButtons.OK);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("El registro que intenta insertar ya se encuentra en el sistema", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //METODO PARA ELIMINAR MODELO X OPERACIÓN
        private void btnEliminar2_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("Realmente desea eliminar", "Validación del Sistema", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (datalistadoModeloXOperacionXMaquinaria.CurrentRow != null)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("EliminarModeloxOperacionxMaquinaria", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", idmodeloxoperacionxmequinaria);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        Mostrar2(idmodelo2, idoperacion2);
                        MessageBox.Show("Eliminación correcta, operación echa satisfactoriamente", "Validación del Sistema", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un registro para poder eliminar", "Validación del Sistema", MessageBoxButtons.OK);
                }
            }
        }
    }
}

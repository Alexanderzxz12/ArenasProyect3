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
using Zen.Barcode;

namespace ArenasProyect3.Modulos.Procesos.Mantenimientos
{
    public partial class MantenimientoProductoOperacion : Form
    {
        //VARIABLES PARA DEFINIR DATOS Y PARA VALIDAR REPETICIONES
        string idlinea1;
        string idlinea2;
        string idoperacion2;
        int idlineaxoperacion1;
        int idlineaxoperacionxmequinaria;
        bool DetalleRepetido = false;
        bool DetalleRepetido2 = false;

        //CONSTRUCTOR DEL MANTENIMIENTO - MANTENIEMINTO DE PRODUCTO POR OPERACION
        public MantenimientoProductoOperacion()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI MANTENIMIENTOS DE LINEA POR OPERACIONES
        private void MantenimientoProductoOperacion_Load(object sender, EventArgs e)
        {
            CargarLineas1();
            CargarOperacion1();
            CargarLineas2();
            CargarMaquinarias();
            alternarColorFilas(datalistadoLineaXOperacion);
            alternarColorFilas(datalistadoLineaXOperacionXMaquinaria);
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
        //CARGA DE LINEA N1
        public void CargarLineas1()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdLinea, Descripcion FROM LINEAS WHERE Estado = 1 ORDER BY Descripcion", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboLinea1.ValueMember = "IdLinea";
                cboLinea1.DisplayMember = "Descripcion";
                cboLinea1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CATGA DE OPERACIONES N1
        public void CargarOperacion1()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //SEGUNDA PARTE-------------
        //CARGA DE LINEA N2
        public void CargarLineas2()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT IdLinea, Descripcion FROM LINEAS WHERE Estado = 1 ORDER BY Descripcion", con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboLinea2.ValueMember = "IdLinea";
                cboLinea2.DisplayMember = "Descripcion";
                cboLinea2.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CATGA DE OPERACIONES N2
        public void CargarOperacion2(string idlinea)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT LxO.IdLineaXOperacion,O.IdOperaciones, O.Descripcion FROM LineaXOperacion LxO INNER JOIN Operaciones O ON O.IdOperaciones = LxO.IdOperacion where LxO.Estado = 1 AND IdLinea = @idlinea ORDER BY Descripcion", con);
                comando.Parameters.AddWithValue("@idlinea", idlinea);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboOperacion2.ValueMember = "O.IdOperaciones";
                cboOperacion2.DisplayMember = "Descripcion";
                cboOperacion2.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //CARGA DE MAQUINARIAS N2
        public void CargarMaquinarias()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //VALIDACIONES-------------------------------------------------------------------
        //VALIDACIÓN POR SI EXISTE LA LINEA POR OPERACIÓN PARA EL PRIMERO
        public void ValidarExisitencia1()
        {
            foreach (DataGridViewRow datorecuperado in datalistadoLineaXOperacion.Rows)
            {
                string linea = Convert.ToString(datorecuperado.Cells["LINEA"].Value);
                string operacion = Convert.ToString(datorecuperado.Cells["OPERACIÓN"].Value);
                if (linea == cboLinea1.Text)
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

        //VALIDACIÓN POR SI EXISTE LA LINEA POR OPERACIÓN PARA EL SEGUNDO CON MAQUINARIA
        public void ValidarExisitencia2()
        {
            foreach (DataGridViewRow datorecuperado in datalistadoLineaXOperacionXMaquinaria.Rows)
            {
                string linea = Convert.ToString(datorecuperado.Cells["LINEA"].Value);
                string operacion = Convert.ToString(datorecuperado.Cells["OPERACIÓN"].Value);
                string maquinaria = Convert.ToString(datorecuperado.Cells["MAQUINARIA"].Value);
                if (linea == cboLinea2.Text)
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
        }
        //----------------------------------------------------------------------------------------------------------------

        //MOSTRAR RESULTADOS EN LAS GRILLAS------------------------------------------------
        //MOSTRAR LINEA POR OPERACIÓN SEGUN LA LINEA SELECCIOANDA
        public void Mostrar1(string idlinea)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("LineaXOperacion_Mostrar", con);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@idlinea", idlinea);
                da = new SqlDataAdapter(comando);
                da.Fill(dt);
                datalistadoLineaXOperacion.DataSource = dt;
                con.Close();
                datalistadoLineaXOperacion.Columns[0].Visible = false;
                datalistadoLineaXOperacion.Columns[1].Visible = false;
                datalistadoLineaXOperacion.Columns[2].Visible = false;
                datalistadoLineaXOperacion.Columns[3].Width = 220;
                datalistadoLineaXOperacion.Columns[4].Width = 350;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //MOSTRAR LINEA POR OPERACIÓN POR MAQUINARIA SEGÚN LA LÍNEA Y LA OPERACIÓN SELECCIONADA
        public void Mostrar2(string idlinea, string idoperacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("LineaXOperacionXMaquinaria_Mostrar", con);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@idlinea", idlinea);
                comando.Parameters.AddWithValue("@idoperacion", idoperacion);
                da = new SqlDataAdapter(comando);
                da.Fill(dt);
                datalistadoLineaXOperacionXMaquinaria.DataSource = dt;
                con.Close();
                datalistadoLineaXOperacionXMaquinaria.Columns[0].Visible = false;
                datalistadoLineaXOperacionXMaquinaria.Columns[1].Visible = false;
                datalistadoLineaXOperacionXMaquinaria.Columns[3].Visible = false;
                datalistadoLineaXOperacionXMaquinaria.Columns[5].Visible = false;
                datalistadoLineaXOperacionXMaquinaria.Columns[2].Width = 140;
                datalistadoLineaXOperacionXMaquinaria.Columns[4].Width = 200;
                datalistadoLineaXOperacionXMaquinaria.Columns[6].Width = 290;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hubo un error inesperado, " + ex.Message);
            }
        }

        //VISUALIZACION DEL REGISTRO SELECCIONADO----------------------------------------------
        //ACCIÓN DE DOBLE CLICK PARA LA GRILLA DE LINEA X OPERACION
        private void datalistadoLineaXOperacion_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            idlineaxoperacion1 = Convert.ToInt32(datalistadoLineaXOperacion.SelectedCells[0].Value.ToString());
            cboLinea1.SelectedValue = datalistadoLineaXOperacion.SelectedCells[1].Value.ToString();
            cboOperacion1.SelectedValue = datalistadoLineaXOperacion.SelectedCells[2].Value.ToString();
        }

        //ACCIÓN DE DOBLE CLICK PARA LA GRILLA DE LINEA X MQUINARIA
        private void datalistadoLineaXOperacionXMaquinaria_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            idlineaxoperacionxmequinaria = Convert.ToInt32(datalistadoLineaXOperacionXMaquinaria.SelectedCells[0].Value.ToString());
            cboLinea2.SelectedValue = datalistadoLineaXOperacionXMaquinaria.SelectedCells[1].Value.ToString();
            cboOperacion2.SelectedValue = datalistadoLineaXOperacionXMaquinaria.SelectedCells[3].Value.ToString();
            cboMaquinaria2.SelectedValue = datalistadoLineaXOperacionXMaquinaria.SelectedCells[5].Value.ToString();
        }

        //MOSTREO DE DATOS CON FILTROS---------------------------------------------------------
        //SELECCION DE UNA LINEA Y CARGA DE DATOS SEGÚN LA LÍNEA SELECCIOANDA
        private void cboLinea1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLinea1.SelectedValue.ToString() != null)
            {
                idlinea1 = cboLinea1.SelectedValue.ToString();
                Mostrar1(idlinea1);
            }
        }

        //ACCIONES DE CRUD PRIMERA PARTE----------------------------------------------------------
        //METODO PARA GAURDAR LINEA X OPERACIÓN
        public void AgregarLineaXOperacion(int linea1, int operacion1)
        {
            ValidarExisitencia1();

            if (DetalleRepetido == false)
            {
                DialogResult boton = MessageBox.Show("¿Esta seguro que desea guardar este producto por operación?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("LineaxOperacion_Insertar", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idlinea", linea1);
                        cmd.Parameters.AddWithValue("@idoperacion", operacion1);

                        cmd.ExecuteNonQuery();
                        con.Close();
                        Mostrar1(idlinea1);
                        CargarOperacion2(idlinea2);
                        MessageBox.Show("Registro ingresado exitosamente.", "Validación del Sistema", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("El registro que intenta insertar ya se encuentra en el sistema.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        private void btnGuardar1_Click(object sender, EventArgs e)
        {
            AgregarLineaXOperacion(Convert.ToInt32(cboLinea1.SelectedValue), Convert.ToInt32(cboOperacion1.SelectedValue));
        }

        //METODO PARA ELIMINAR LINEA X OPERACIÓN
        public void EliminarLineaXOperacion(int id,DataGridView dgv)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea eliminar este registro?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (dgv.CurrentRow != null)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("LineaXOperacion_Eliminar", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        Mostrar1(idlinea1);
                        //RECARGAR DE LAS OPERACIONES PARA ACTUALIZAR LA ELIMINACIÓN DE LINEA X OPERACION
                        CargarOperacion2(idlinea2);
                        MessageBox.Show("Eliminación correcta, operación hecha satisfactoriamente.", "Validación del Sistema", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un registro para poder eliminar.", "Validación del Sistema", MessageBoxButtons.OK);
                }
            }
        }
        private void btnEliminar1_Click(object sender, EventArgs e)
        {
            EliminarLineaXOperacion(idlineaxoperacion1,datalistadoLineaXOperacion);
        }

        //MOSTREO DE DATOS CON FILTROS---------------------------------------------------------
        //GUARDAR LA LÍNEA SELECCIONADA Y GUARDARLA PARA MOSRARLA
        private void cboLinea2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLinea2.SelectedValue.ToString() != null)
            {
                idlinea2 = cboLinea2.SelectedValue.ToString();
                CargarOperacion2(idlinea2);
            }
        }

        //METODO PARA MOSTRAR REGISTROS SEGÍN LÍNEA Y PERACIÓN
        private void cboOperacion2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboOperacion2.SelectedValue.ToString() != null)
            {
                idoperacion2 = cboOperacion2.SelectedValue.ToString();
                Mostrar2(idlinea2, idoperacion2);
            }
        }
    
        //ACCIONES DE CRUD SEGUNDA PARTE----------------------------------------------------------
        //METODO PARA GAURDAR LINEA X OPERACIÓN X MAQUINARIA
        public void AgregarLineaXOperacionXMaquinaria(int linea2, int operacion2, int maquinaria)
        {
            ValidarExisitencia2();

            if (DetalleRepetido2 == false)
            {

                if (operacion2 == 0 || Convert.ToString(operacion2) == "")
                {
                    MessageBox.Show("No se puede ingresar sin escoger una operación.", "Validación del Sistema", MessageBoxButtons.OK);
                }
                else
                {
                    DialogResult boton = MessageBox.Show("¿Realmente desea guardar este producto por maquinaria?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                    if (boton == DialogResult.OK)
                    {
                        try
                        {
                            SqlConnection con = new SqlConnection();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd = new SqlCommand("LineaXOperacionXMaquinaria_Insertar", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@idlinea", linea2);
                            cmd.Parameters.AddWithValue("@idoperacion", operacion2);
                            cmd.Parameters.AddWithValue("@idmaquinaria", maquinaria);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            Mostrar2(idlinea2, idoperacion2);
                            MessageBox.Show("Registro ingresado exitosamente.", "Validación del Sistema", MessageBoxButtons.OK);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("El registro que intenta insertar ya se encuentra en el sistema.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        private void btnGuardar2_Click(object sender, EventArgs e)
        {
           AgregarLineaXOperacionXMaquinaria(Convert.ToInt32(cboLinea2.SelectedValue), Convert.ToInt32(cboOperacion2.SelectedValue), Convert.ToInt32(cboMaquinaria2.SelectedValue));
        }

        //METODO PARA ELIMINAR LINEA X OPERACIÓN
        public void EliminarLineaXOperacionxMaquinaria(int id,DataGridView dgv)
        {
            DialogResult boton = MessageBox.Show("¿Realmente desea eliminar este registro?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
            if (boton == DialogResult.OK)
            {
                if (dgv.CurrentRow != null)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("LineaXOperacionXMaquinaria_Eliminar", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        Mostrar2(idlinea2, idoperacion2);
                        MessageBox.Show("Eliminación correcta, operación hecha satisfactoriamente.", "Validación del Sistema", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un registro para poder eliminar.", "Validación del Sistema", MessageBoxButtons.OK);
                }
            }
        }
        private void btnEliminar2_Click(object sender, EventArgs e)
        {
            EliminarLineaXOperacionxMaquinaria(idlineaxoperacionxmequinaria,datalistadoLineaXOperacionXMaquinaria);
        }
    }
}

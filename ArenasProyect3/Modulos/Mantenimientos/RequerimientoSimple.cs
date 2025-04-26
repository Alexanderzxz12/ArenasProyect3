using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArenasProyect3.Modulos.ManGeneral;

namespace ArenasProyect3.Modulos.Mantenimientos
{
    public partial class RequerimientoSimple : Form
    {
        //VARIABLES GLOBALES DE MI REQUERIMIENTO SIMPLE
        string codigoRequerimientoSimple = "";
        string cantidadRequerimiento = "0000000";
        string cantidadRequerimiento2 = "";
        int IdUsuario = 0;
        string area = "";
        string ruta = ManGeneral.Manual.manualRequerimientosSimples;

        //CONSTRUCTOR DE MI MANTENIMIENTO
        public RequerimientoSimple()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI MANTENIMEINTO
        private void RequerimientoSimple_Load(object sender, EventArgs e)
        {
            //CARGA DE COMBOS Y DATOS DEL USUARIO
            CargarTipoRequerimiento();
            CargarLocal();
            CargarSede();
            CargarPrioridad();
            DatosUsuario();

            //SELECCIÓN AUTOMÁTICA DE LA JEFATURA INMEDIATA
            if (area == "Comercial")
            {
                DatosJefaturas(1);
            }
            else if (area == "Procesos")
            {
                DatosJefaturas(5);
            }
            else if (area == "Contabilidad")
            {
                DatosJefaturas(8);
            }
            else if (area == "Logística")
            {
                DatosJefaturas(11);
            }
            else if (area == "Ingienería")
            {
                DatosJefaturas(14);
            }

            CargarCentroCostos();

            //DEFINICIÓND DE SOLO LECTURA DE MI LISTADO DE PRODUCTOS
            datalistadoProductosRequerimiento.Columns[1].ReadOnly = true;
            datalistadoProductosRequerimiento.Columns[2].ReadOnly = true;
            datalistadoProductosRequerimiento.Columns[3].ReadOnly = true;

            //AUMENTAR 5 DIAS A MI FECHA REQUERIDA
            DateTime fechaActual = DateTime.Now;
            fechaActual = fechaActual.AddDays(2);
            dateTimeFechaRequerida.Value = fechaActual;

            //VISUALIZAR EL VALIDADO AUTO
            if(txtJefatura.Text == txtSolicitante.Text)
            {
                ckAprobacionAuto.Visible = true;
            }
        }

        //COLOREAR MI LISTADO
        public void alternarColorFilas(DataGridView dgv)
        {
            try
            {
                {
                    var withBlock = dgv;
                    withBlock.RowsDefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
                    withBlock.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //------------------------------CARGA DE COMBOS-----------------------------------
        //CARGAR TIPO DE REQUERIMIENTO
        public void CargarTipoRequerimiento()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdTipoRequerimiento, Descripcion FROM TipoRequerimientoGeneral WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboTipoRequerimiento.DisplayMember = "Descripcion";
            cboTipoRequerimiento.ValueMember = "IdTipoRequerimiento";
            cboTipoRequerimiento.DataSource = dt;
        }

        //CARGAR SEDE
        public void CargarSede()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdSede, Descripcion FROM Sede WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboSede.DisplayMember = "Descripcion";
            cboSede.ValueMember = "IdSede";
            cboSede.DataSource = dt;
        }

        //CARGAR LOCAL
        public void CargarLocal()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdLocal, Descripcion FROM Local WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboLocal.DisplayMember = "Descripcion";
            cboLocal.ValueMember = "IdLocal";
            cboLocal.DataSource = dt;
        }

        //CARGAR PRIORIDAD
        public void CargarPrioridad()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdPrioridad, Descripcion FROM Prioridades WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboPrioridad.DisplayMember = "Descripcion";
            cboPrioridad.ValueMember = "IdPrioridad";
            cboPrioridad.DataSource = dt;
        }

        //CARGAR CENTRO DE COSTOS
        public void CargarCentroCostos()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdCentroCostos, Descripcion FROM CentroCostos WHERE Estado = 1 AND IdCentroCostos = @idcentrocostos", con);
            comando.Parameters.AddWithValue("@idcentrocostos", datalistadoBusquedaJefatura.SelectedCells[10].Value.ToString());
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboCentroCostos.ValueMember = "IdCentroCostos";
            cboCentroCostos.DisplayMember = "Descripcion";
            cboCentroCostos.DataSource = dt;
        }

        //CARGAR EL LAS AREAS SEGÚN EL CENTRO DE COSTOS
        public void CargarAreaSegunCentroCostos(string idCentroCostos)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdArea,Descripcion  FROM AreaGeneral WHERE Estado = 1 AND CentroCostos = @idCentroCsotos", con);
            comando.Parameters.AddWithValue("@idCentroCsotos", idCentroCostos);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboAreaGeneral.ValueMember = "IdArea";
            cboAreaGeneral.DisplayMember = "Descripcion";
            cboAreaGeneral.DataSource = dt;
        }

        //CONTAR LA CANTIDAD DE REQUERIMIENTOS QUE HAY EN MI TABLA
        public void ConteoRequerimientosSimples()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            da = new SqlDataAdapter("SELECT IdRequerimientoSimple FROM RequerimientoSimple WHERE IdRequerimientoSimple = (SELECT MAX(IdRequerimientoSimple) FROM RequerimientoSimple)", con);
            da.Fill(dt);
            datalistadoCargarCantidadRequerimeintoSimple.DataSource = dt;
            con.Close();

            if (datalistadoCargarCantidadRequerimeintoSimple.RowCount > 0)
            {
                cantidadRequerimiento = datalistadoCargarCantidadRequerimeintoSimple.SelectedCells[0].Value.ToString();

                if (cantidadRequerimiento.Length == 1)
                {
                    cantidadRequerimiento2 = "000000" + cantidadRequerimiento;
                }
                else if (cantidadRequerimiento.Length == 2)
                {
                    cantidadRequerimiento2 = "00000" + cantidadRequerimiento;
                }
                else if (cantidadRequerimiento.Length == 3)
                {
                    cantidadRequerimiento2 = "0000" + cantidadRequerimiento;
                }
                else if (cantidadRequerimiento.Length == 4)
                {
                    cantidadRequerimiento2 = "000" + cantidadRequerimiento;
                }
                else if (cantidadRequerimiento.Length == 5)
                {
                    cantidadRequerimiento2 = "00" + cantidadRequerimiento;
                }
                else if (cantidadRequerimiento.Length == 6)
                {
                    cantidadRequerimiento2 = "0" + cantidadRequerimiento;
                }
                else if (cantidadRequerimiento.Length == 7)
                {
                    cantidadRequerimiento2 = cantidadRequerimiento;
                }
            }
            else
            {
                cantidadRequerimiento2 = cantidadRequerimiento;
            }
        }

        //CARGAR Y GENERAR EL CÓDIGO DEL REQUERIMIENTO SIMPLE
        public void GenerarCodigoRequerimientoSimple()
        {
            ConteoRequerimientosSimples();

            DateTime date = DateTime.Now;

            codigoRequerimientoSimple = Convert.ToString(date.Year) + cantidadRequerimiento2;
        }

        //LISTAR TODOS LOS PRODUTOS PARA SELECCIONAR EN MI REQUERIMIENTO
        public void MostrarProductosRequerimientoGeneral()
        {
            //PROCEDIMIENTO ALMACENADO PARA LISTAR LOS PRODUCTOS
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("ListarProductosRequerimientoGeneral_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoBusquedaProducto.DataSource = dt;
            con.Close();
            Rediemnsion(datalistadoBusquedaProducto);
            alternarColorFilas(datalistadoBusquedaProducto);
        }

        //FUNCION DE REDIEMNSION
        public void Rediemnsion(DataGridView DGV)
        {
            //SE REDIMENSIONA EL TAMAÑO DE CADA COLUMNA DE MI LISTADO DE PRODUCTOS
            DGV.Columns[1].Width = 100;
            DGV.Columns[2].Width = 80;
            DGV.Columns[3].Width = 520;
            DGV.Columns[4].Width = 150;
            DGV.Columns[9].Width = 88;
            DGV.Columns[10].Width = 80;
            DGV.Columns[11].Width = 87;
            //SE QUITA LAS COLUMNAS QUE NO SON RELEVANTES PARA EL USUARIO
            DGV.Columns[5].Visible = false;
            DGV.Columns[6].Visible = false;
            DGV.Columns[7].Visible = false;
            DGV.Columns[8].Visible = false;
            DGV.Columns[12].Visible = false;

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            alternarColorFilas(DGV);
        }

        //---------------------------------------------------------------------------------
        //-------------------------------------------ACCIONES--------------------------------
        //REQUERIMEINTO SEGÚN EL TIPO SELECCIONADO
        private void cboTipoRequerimiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipoRequerimiento.Text == "SIN ORDEN DE PRODUCCIÓN")
            {
                btnBuscarOP.Visible = false;
                txtCodigoOrdenProduccion.Visible = false;
                btnBuscarOT.Visible = false;
                txtCodigoOrdenTrabajo.Visible = false;
                datalistadoProductosRequerimiento.Visible = true;
                datalistadoProductosRequerimientoOP.Visible = false;
            }
            else if (cboTipoRequerimiento.Text == "CON ORDEN DE PRODUCCIÓN")
            {
                btnBuscarOP.Visible = true;
                txtCodigoOrdenProduccion.Visible = true;
                btnBuscarOT.Visible = false;
                txtCodigoOrdenTrabajo.Visible = false;
                datalistadoProductosRequerimiento.Visible = false;
                datalistadoProductosRequerimientoOP.Visible = true;
            }
            else if (cboTipoRequerimiento.Text == "CON ORDEN DE TRABAJO")
            {
                btnBuscarOP.Visible = true;
                txtCodigoOrdenProduccion.Visible = true;
                btnBuscarOT.Visible = true;
                txtCodigoOrdenTrabajo.Visible = true;
                datalistadoProductosRequerimiento.Visible = false;
                datalistadoProductosRequerimientoOP.Visible = true;
            }
            else if (cboTipoRequerimiento.Text == "REPOSICIÓN DE STOCK")
            {
                btnBuscarOP.Visible = false;
                txtCodigoOrdenProduccion.Visible = false;
                btnBuscarOT.Visible = false;
                txtCodigoOrdenTrabajo.Visible = false;
                datalistadoProductosRequerimiento.Visible = true;
                datalistadoProductosRequerimientoOP.Visible = false;
            }
            else if (cboTipoRequerimiento.Text == "SERVICIOS POR TERCEROS")
            {
                btnBuscarOP.Visible = false;
                txtCodigoOrdenProduccion.Visible = false;
                btnBuscarOT.Visible = false;
                txtCodigoOrdenTrabajo.Visible = false;
                datalistadoProductosRequerimiento.Visible = true;
                datalistadoProductosRequerimientoOP.Visible = false;
            }
        }

        //ACCIÓN ENLAZADA DE LA CARGA DEL CENTRO DE COSTOS
        private void cboCentroCostos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCentroCostos.SelectedValue.ToString() != null)
            {
                string idCentroCostos = cboCentroCostos.SelectedValue.ToString();
                CargarAreaSegunCentroCostos(idCentroCostos);
            }
        }

        //AÑADIR PRODUCTOS A MI DETALLE
        private void btnAgregarProductos_Click(object sender, EventArgs e)
        {
            panelBuscarProductos.Visible = true;
            //HACER QUE EL COMBO TENGA UN DATO SELECCIONADO POR DEFAULT
            cboTipoBusquedaProducto.SelectedIndex = 0;
            MostrarProductosRequerimientoGeneral();
            //DEFINIR LAS COLUMNAS DE MI LISTADO COMO SOLO LECTURA
            datalistadoBusquedaProducto.Columns[1].ReadOnly = true;
            datalistadoBusquedaProducto.Columns[2].ReadOnly = true;
            datalistadoBusquedaProducto.Columns[3].ReadOnly = true;
            datalistadoBusquedaProducto.Columns[4].ReadOnly = true;
            datalistadoBusquedaProducto.Columns[9].ReadOnly = true;
            datalistadoBusquedaProducto.Columns[10].ReadOnly = true;
            datalistadoBusquedaProducto.Columns[11].ReadOnly = true;
        }

        //CAMBIAR EL CRITERIO DE BÚSQUEDA
        private void cboTipoBusquedaProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusquedaProducto.Text = "";
        }

        //ACCIÓN DE SELECCIOANR UNA FILA Y LLEVARLA A MI OTRO LISTADO
        private void datalistadoBusquedaProducto_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoBusquedaProducto.Columns[e.ColumnIndex];

            //SI SE PRECIONA SOBRE LA COLUMNA CON ESE NOMBRE
            if (currentColumn.Name == "ckSeleccionarProducto")
            {
                //SE CAPTURA LAS VARIABLES 
                string id = datalistadoBusquedaProducto.SelectedCells[12].Value.ToString();
                string codigo = datalistadoBusquedaProducto.SelectedCells[1].Value.ToString();
                string producto = datalistadoBusquedaProducto.SelectedCells[3].Value.ToString();
                string tipoMedida = datalistadoBusquedaProducto.SelectedCells[4].Value.ToString();
                string stock = datalistadoBusquedaProducto.SelectedCells[9].Value.ToString();

                //SE AGREGA A LA NUEVA LISTA
                datalistadoSeleccionBusquedaProducto.Rows.Add(new[] { id, codigo, producto, tipoMedida, stock });
                alternarColorFilas(datalistadoSeleccionBusquedaProducto);
                //SE BORRA EL REGISTRO SELECCIONADO
                datalistadoBusquedaProducto.Rows.Remove(datalistadoBusquedaProducto.CurrentRow);
            }
        }

        //RECARGAR EL LISTADO DE PRODUCTOS Y LIMPIAR LA BARRA DE BÚSQUEDA
        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            //HACER EL LLAMADO AL MÉTODO DE LISTAD DE NUEVO
            MostrarProductosRequerimientoGeneral();
            //LIMPIAR LA BARRA DE BÚSQUEDA Y REINICIAR EL CBO
            txtBusquedaProducto.Text = "";
            cboTipoBusquedaProducto.SelectedIndex = 0;
        }

        //BORRAR DE MI LISTA SELECCIONADA UN PRODUCTO ANTES SELECCIONADO
        private void btnRegresarBusqeudaProductos_Click(object sender, EventArgs e)
        {
            panelBuscarProductos.Visible = false;
            txtBusquedaProducto.Text = "";
            cboTipoBusquedaProducto.SelectedIndex = 1;
            datalistadoSeleccionBusquedaProducto.Rows.Clear();
        }

        //BORRAR DE MI LISTA SELECCIONADA UN PRODUCTO ANTES SELECCIONADO
        private void btnBorrarBusquedaProductps_Click(object sender, EventArgs e)
        {
            //SI EN EL LISTADO DE CLIENTES NO HAY REGIUSTROS
            if (datalistadoSeleccionBusquedaProducto.Rows.Count > 0)
            {
                //MENSAJE DE CONFIRMACIÓN DE ELIMINACIÓN DE PRODUCTOS
                DialogResult resul = MessageBox.Show("¿Seguro que desea borrar este producto?.", "Validación del Sistema", MessageBoxButtons.YesNo);
                if (resul == DialogResult.Yes)
                {
                    //BORRAR EL REGISTRO SELECCIONADO
                    datalistadoSeleccionBusquedaProducto.Rows.Remove(datalistadoSeleccionBusquedaProducto.CurrentRow);
                }
            }
            else
            {
                MessageBox.Show("No hay productos agregados para poder borrarlos.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //LLEVAR LOS PRODUCTOS A MI OTRO FORMULARIO
        private void btnConfirmarBusquedaProductos_Click(object sender, EventArgs e)
        {
            //SE USA EL FOREACH PARA RECORRER TODAS LAS FILAS SELECCIOANDAS
            foreach (DataGridViewRow row in datalistadoSeleccionBusquedaProducto.Rows)
            {
                //SE CAPTURA LAS VARIABLES 
                string id = Convert.ToString(row.Cells[0].Value);
                string codigo = Convert.ToString(row.Cells[1].Value);
                string producto = Convert.ToString(row.Cells[2].Value);
                string tipoMedida = Convert.ToString(row.Cells[3].Value);
                string stock = Convert.ToString(row.Cells[4].Value);

                //SE AGREGA A LA NUEVA LISTA
                datalistadoProductosRequerimiento.Rows.Add(new[] { id, codigo, producto, tipoMedida, null, stock });
            }

            //LIMPIAR Y REINICIAR LA BÚSQUEDA DE PRODUCTOS
            panelBuscarProductos.Visible = false;
            panelBuscarProductos.Visible = false;
            txtBusquedaProducto.Text = "";
            cboTipoBusquedaProducto.SelectedIndex = 1;
            datalistadoSeleccionBusquedaProducto.Rows.Clear();
            alternarColorFilas(datalistadoProductosRequerimiento);
        }

        //VALIDACIÓN DEL LISTADO DE PRESUPUESTO
        private void datalistadoProductosRequerimiento_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //VARIABLES PARA ALMACENAR LOS DATOS
            decimal a;
            //ALMACENAMIENTO DE DATOS PARA LA VALIDACIÓN
            //RECORRIDO DE MI LISTADO PARA ALMACENAR LAS DIFERENTES COLUMNAS
            DataGridViewRow row = (DataGridViewRow)datalistadoProductosRequerimiento.Rows[e.RowIndex];
            //ALMACENAMIENTOS DE COLUMNAS
            a = Convert.ToDecimal(row.Cells[4].Value);

            //VALIDACIÓN DE VIÁTICOS
            if (row.Cells[4].Value == DBNull.Value)
            {
                //REINICIO DE CAMPO
                a = Convert.ToDecimal("1.000");
            }
            else
            {
                //CAPTURA DEL VALOR
                a = Convert.ToDecimal(row.Cells[4].Value);
            }

            row.Cells[4].Value = String.Format("{0:#,0.000}", a);
        }

        //BORRAR UN PRODICTO DE MI LISTADO DE MI REQUERIMIENTO
        private void btnBorrarProducto_Click(object sender, EventArgs e)
        {
            //SI EN EL LISTADO DE CLIENTES NO HAY REGIUSTROS
            if (datalistadoProductosRequerimiento.Rows.Count > 0)
            {
                //MENSAJE DE CONFIRMACIÓN DE ELIMINACIÓN DE PRODUCTOS
                DialogResult resul = MessageBox.Show("¿Seguro que desea borrar este producto?.", "Validación del Sistema", MessageBoxButtons.YesNo);
                if (resul == DialogResult.Yes)
                {
                    //BORRAR EL REGISTRO SELECCIONADO
                    datalistadoProductosRequerimiento.Rows.Remove(datalistadoProductosRequerimiento.CurrentRow);
                }
            }
            else
            {
                MessageBox.Show("No hay productos agregados para poder borrarlos.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //ACCIÓN DE GUARDAR EL REQUERIMIENTO SIMPLE
        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (datalistadoProductosRequerimiento.RowCount == 0)
            {
                MessageBox.Show("Debe seleccionar productos para poder proceder a guardar este requerimiento.", "Validación del Sistema", MessageBoxButtons.OK);
            }
            else
            {
                DialogResult boton = MessageBox.Show("¿Realmente desea guardar este requerimiento?", "Validación del Sistema", MessageBoxButtons.OKCancel);
                if (boton == DialogResult.OK)
                {
                    try
                    {
                        SqlConnection con = new SqlConnection();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand("InsertarRequerimientoSimple", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        //INGRESO - PARTE GENERAL DEL REQUERIMIENTO SIMPLE
                        GenerarCodigoRequerimientoSimple();
                        cmd.Parameters.AddWithValue("@codigoRequerimeintoSimple", codigoRequerimientoSimple);
                        cmd.Parameters.AddWithValue("@fechaRequerida", dateTimeFechaRequerida.Value);
                        cmd.Parameters.AddWithValue("@fechaSolicitada", dateTimeFechaSolicitada.Value);
                        cmd.Parameters.AddWithValue("@desJefatura", txtJefatura.Text);
                        cmd.Parameters.AddWithValue("@idSolicitante", IdUsuario);
                        cmd.Parameters.AddWithValue("@idCentroCostos", cboCentroCostos.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@observaciones", txtObservaciones.Text);
                        cmd.Parameters.AddWithValue("@idSede", cboSede.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@idLocal", cboLocal.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@idArea", cboAreaGeneral.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@idipo", cboTipoRequerimiento.SelectedValue.ToString());

                        if (ckAprobacionAuto.Visible == true && ckAprobacionAuto.Checked == true)
                        {
                            cmd.Parameters.AddWithValue("@estadoLogistica", 2);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@estadoLogistica", 1);
                        }

                        cmd.Parameters.AddWithValue("@mensajeAnulacion", "");
                        cmd.Parameters.AddWithValue("@idJefatura", DBNull.Value);
                        cmd.Parameters.AddWithValue("@aliasCargaJefatura", "");
                        cmd.Parameters.AddWithValue("@cantidadItems", datalistadoProductosRequerimiento.RowCount);
                        cmd.Parameters.AddWithValue("@idPrioridad", cboPrioridad.SelectedValue.ToString());
                        cmd.ExecuteNonQuery();
                        con.Close();

                        //VARIABLE PARA CONTAR LA CANTIDAD DE ITEMS QUE HAY
                        int contador = 1;
                        //INGRESO DE LOS DETALLES DEL REQUERIMIENTO SIMPLE CON UN FOREACH
                        foreach (DataGridViewRow row in datalistadoProductosRequerimiento.Rows)
                        {
                            decimal cantidad = Convert.ToDecimal(row.Cells["CANTIDAD"].Value);

                            //PROCEDIMIENTO ALMACENADO PARA GUARDAR LOS PRODUCTOS
                            con.Open();
                            cmd = new SqlCommand("InsertarRequerimientoSimple_DetalleProductos", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@item", contador);
                            cmd.Parameters.AddWithValue("@idArt", Convert.ToString(row.Cells[0].Value));
                            //SI NO HAN PUESTO UN VALOR AL PRODUCTO
                            if (cantidad == 0)
                            {
                                cmd.Parameters.AddWithValue("@cantidad", 1.000);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@cantidad", cantidad);
                            }

                            cmd.Parameters.AddWithValue("@stock", Convert.ToString(row.Cells[5].Value));
                            cmd.Parameters.AddWithValue("@cantidadTotal", 0.000);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            //AUMENTAR
                            contador++;
                        }

                        MessageBox.Show("Se ingresó el requerimiento correctamente.", "Validación del Sistema");

                        datalistadoProductosRequerimiento.Rows.Clear();
                        txtObservaciones.Text = "";
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //ACCIÓN DE CERRAR EL MANTENIMIENTO DE NUEVO REQUERIMIENTO SIMPLE
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //CARGA DE METODOS - GENERAL----------------------------------------------------------------------------------
        //CARGA DE DATOS DEL USUARIO QUE INICIO SESIÓN
        //BUSQUEDA DE USUARIO
        public void DatosUsuario()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarUsuarioPorCodigo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idusuario", Program.IdUsuario);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoBusquedaUusario.DataSource = dt;
            con.Close();

            IdUsuario = Convert.ToInt32(datalistadoBusquedaUusario.SelectedCells[0].Value.ToString());
            txtSolicitante.Text = datalistadoBusquedaUusario.SelectedCells[1].Value.ToString() + " " + datalistadoBusquedaUusario.SelectedCells[2].Value.ToString();
            area = datalistadoBusquedaUusario.SelectedCells[7].Value.ToString();
        }

        //BUSQUEDA DE JEFATURAS
        public void DatosJefaturas(int idusuario)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarJefaturas", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idRol", idusuario);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoBusquedaJefatura.DataSource = dt;
            con.Close();

            txtJefatura.Text = datalistadoBusquedaJefatura.SelectedCells[1].Value.ToString() + " " + datalistadoBusquedaJefatura.SelectedCells[2].Value.ToString();
        }

        //BÚSQUEDA-------------------------------------------------------------------------
        //BUSCAR PRODUCTO POR CÓDIGO Y DESCIPCIÓ
        private void txtBusquedaProducto_TextChanged(object sender, EventArgs e)
        {
            if (cboTipoBusquedaProducto.Text == "CÓDIGO")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("ListarProductosRequerimientoGeneral_PorCodigo_SP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codigo", txtBusquedaProducto.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoBusquedaProducto.DataSource = dt;
                con.Close();
                Rediemnsion(datalistadoBusquedaProducto);
            }
            else if (cboTipoBusquedaProducto.Text == "DESCRIPCIÓN")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("ListarProductosRequerimientoGeneral_PorDescripcion_SP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@descripcion", txtBusquedaProducto.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoBusquedaProducto.DataSource = dt;
                con.Close();
                Rediemnsion(datalistadoBusquedaProducto);
            }
            else if (cboTipoBusquedaProducto.Text == "CÓDIGO BSS")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("[ListarProductosRequerimientoGeneral_PorCodigoBSS_SP]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@descripcion", txtBusquedaProducto.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoBusquedaProducto.DataSource = dt;
                con.Close();
                Rediemnsion(datalistadoBusquedaProducto);
            }
        }

        //BOTONES RANDOMS
        private void btnInformacionFechas_Click(object sender, EventArgs e)
        {
            if (panelInformacionFecha.Visible == true)
            {
                panelInformacionFecha.Visible = false;
            }
            else
            {
                panelInformacionFecha.Visible = true;
            }
        }

        //ABRIR EL MANUAL DE USUARIO
        private void btnInfoBusquedaProductos_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }
    }
}

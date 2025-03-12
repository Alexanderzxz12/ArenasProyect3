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

namespace ArenasProyect3.Modulos.Logistica.Almacen
{
    public partial class NotaSalida : Form
    {
        //VARIABLES GLOBALES
        DataView dv;
        DataSet ds = new DataSet();
        string codigoSalidaAlmacen = "";
        bool estadoRequerimeintoAtendidoTotal = true;
        bool ValidarCantidadesExactas = true;

        //CONSTRUCTOR DE MI FORMULARIO
        public NotaSalida()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI FORMULARIO
        private void NotaSalida_Load(object sender, EventArgs e)
        {
            //
        }

        //CARGA DE COMBOS Y DATOS-------------------------------------------------------------------
        //LISTADO DE COMBO DE REQUERIMEINTOS SIMPLES
        public void CargarRequerimientosSimples()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT RS.IdRequerimientoSimple, RS.CodigoRequerimientoSimple, RS.FechaRequerida, RS.FechaSolicitada, RS.IdSolicitante, USU.Nombres + ' ' + USU.ApellidoParterno + ' ' + USU.ApellidoMaterno AS [SOLICITANTE] FROM RequerimientoSimple RS INNER JOIN Usuarios USU ON USU.IdUsuarios = RS.IdSolicitante WHERE RS.Estado = 1 AND RS.IdTipo = 1 AND EstadoAtendido = 0", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboComboReqeurimientoSimple.DisplayMember = "CodigoRequerimientoSimple";
            cboComboReqeurimientoSimple.ValueMember = "IdRequerimientoSimple";
            DataRow row = dt.Rows[0];
            DataTimeFechaReqeurmientoRequerida.Text = System.Convert.ToString(row["FechaRequerida"]);
            DateTimeFechaRequerimientoSolicitada.Text = System.Convert.ToString(row["FechaSolicitada"]);
            txtSolicitante.Text = System.Convert.ToString(row["SOLICITANTE"]);
            lblIdSolicitante.Text = System.Convert.ToString(row["IdSolicitante"]);
            cboComboReqeurimientoSimple.DataSource = dt;
        }

        //LISTADO DE COMBO DE ALMACENES
        public void CargarAlmacenes()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdTipoAlmacenEntrada, Descripcion FROM TipoAlmacenEntradaSalidaAlmacen WHERE Estado = 1", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboAlmacenes.DisplayMember = "Descripcion";
            cboAlmacenes.ValueMember = "IdTipoAlmacenEntrada";
            cboAlmacenes.DataSource = dt;
        }

        //LISTADO DE COMBO DE TIPOS DE MOVIMIENTOS
        public void CargarTiposMovimientos()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdTipoMovimientoEntradaAlmacen, Descripcion FROM TipoMovimientosEntradaSalidaAlmacen WHERE Estado = 1 AND EntradaSalida = 'SALIDA'", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboTipoMovimeintos.DisplayMember = "Descripcion";
            cboTipoMovimeintos.ValueMember = "IdTipoMovimientoEntradaAlmacen";
            cboTipoMovimeintos.DataSource = dt;
        }

        //REFRESCAR EL COMBO DE REQUERMIENTOS SIMPLES
        private void cboComboReqeurimientoSimple_SelectionChangeCommitted(object sender, EventArgs e)
        {
            txtBusquedaReque.Text = "";
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT RS.IdRequerimientoSimple, RS.CodigoRequerimientoSimple, RS.FechaRequerida, RS.FechaSolicitada, RS.IdSolicitante, USU.Nombres + ' ' + USU.ApellidoParterno + ' ' + USU.ApellidoMaterno AS [SOLICITANTE] FROM RequerimientoSimple RS INNER JOIN Usuarios USU ON USU.IdUsuarios = RS.IdSolicitante WHERE RS.Estado = 1 AND IdRequerimientoSimple = @id", con);
            comando.Parameters.AddWithValue("@id", System.Convert.ToString(cboComboReqeurimientoSimple.SelectedValue));
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                DataTimeFechaReqeurmientoRequerida.Text = System.Convert.ToString(row["FechaRequerida"]);
                DateTimeFechaRequerimientoSolicitada.Text = System.Convert.ToString(row["FechaSolicitada"]);
                txtSolicitante.Text = System.Convert.ToString(row["SOLICITANTE"]);
                lblIdSolicitante.Text = System.Convert.ToString(row["IdSolicitante"]);
            }
        }

        //MOSTRAR EL STOCK DE CADA ITEM DE MI REQUERIMEINTO SIMPLE
        public void MostrarItemsSegunRequerimientoStockAlmacen(string codigo)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("ListarProductosRequerimientoGeneral_PorCodigo_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigo", codigo);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoDetalleRequerimeintoSimpleStockAlmacen.DataSource = dt;
            con.Close();
        }

        //VER DETALLES (ITEMS) DE MI REQUERIMIENTO SIMPLE
        public void MostrarItemsSegunRequerimiento(int idRequerimiento)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarItemsSegunRequerimientoSimple_NotaSalida", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idRequerimiento", idRequerimiento);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoDetallesReqeuermientoSimple.DataSource = dt;
            con.Close();

            foreach (DataGridViewRow row in datalistadoDetallesReqeuermientoSimple.Rows)
            {
                string valorListado1_IdDetalleRequerimeinto = row.Cells[0].Value.ToString();
                string valorListado4_IdArt = row.Cells[1].Value.ToString();
                string valorListado5_Codigo = row.Cells[2].Value.ToString();
                string valorListado6_Descripcion = row.Cells[3].Value.ToString();
                string valorListado7_TipoMedida = row.Cells[4].Value.ToString();
                string valorListado8_CantidadRequerida = row.Cells[5].Value.ToString();
                string valorListado_CantidadRetirada = row.Cells[6].Value.ToString();
                string valorListado_CantidadARetirar = Convert.ToString(Convert.ToDecimal(valorListado8_CantidadRequerida) - Convert.ToDecimal(valorListado_CantidadRetirada));

                MostrarItemsSegunRequerimientoStockAlmacen(valorListado5_Codigo);

                string stockAlcualAlmacen = datalistadoDetalleRequerimeintoSimpleStockAlmacen.SelectedCells[8].Value.ToString();

                datalistadoProductosRequerimiento.Rows.Add(new[] { null, valorListado1_IdDetalleRequerimeinto, null, null, valorListado4_IdArt, valorListado5_Codigo, valorListado6_Descripcion, valorListado7_TipoMedida, valorListado8_CantidadRequerida, valorListado_CantidadRetirada, stockAlcualAlmacen, valorListado_CantidadARetirar });
            }

            //DEFINICIÓND DE SOLO LECTURA DE MI LISTADO DE PRODUCTOS
            datalistadoProductosRequerimiento.Columns[5].ReadOnly = true;
            datalistadoProductosRequerimiento.Columns[6].ReadOnly = true;
            datalistadoProductosRequerimiento.Columns[7].ReadOnly = true;
            datalistadoProductosRequerimiento.Columns[8].ReadOnly = true;
            datalistadoProductosRequerimiento.Columns[9].ReadOnly = true;
            datalistadoProductosRequerimiento.Columns[10].ReadOnly = true;

        }

        //CAPTURAR ERRORES INESPERADOS GENERADOS EN EL LISTADO
        private void datalistadoProductosRequerimiento_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        //FIN DEL EDIT AL MOMENTO DE INGRESAR VALORES
        private void datalistadoProductosRequerimiento_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //VARIABLES PARA ALMACENAR LOS DATOS
            decimal cantidadRequerida;
            decimal cantidadRetirada;
            decimal cantidadRetiradamenosRequerida;
            decimal cantidadARetirar;
            //ALMACENAMIENTO DE DATOS PARA LA VALIDACIÓN
            //RECORRIDO DE MI LISTADO PARA ALMACENAR LAS DIFERENTES COLUMNAS
            DataGridViewRow row = (DataGridViewRow)datalistadoProductosRequerimiento.Rows[e.RowIndex];
            //ALMACENAMIENTOS DE COLUMNAS
            cantidadRequerida = Convert.ToDecimal(row.Cells[8].Value);
            cantidadRetirada = Convert.ToDecimal(row.Cells[9].Value);
            cantidadARetirar = Convert.ToDecimal(row.Cells[11].Value);

            //VALIDACIÓN DE CANTIDAD A RETIRAR
            if (row.Cells[11].Value == DBNull.Value)
            {
                //REINICIO DE CAMPO
                cantidadARetirar = Convert.ToDecimal("0.00");
            }
            else
            {
                cantidadRetiradamenosRequerida = cantidadRequerida - cantidadRetirada;

                if (cantidadARetirar > cantidadRequerida || cantidadARetirar > cantidadRetiradamenosRequerida)
                {
                    MessageBox.Show("No se puede retirar más de la cantidad indicada en el requerimiento, por favor ingrese la cantidad requerida.", "Validación del Sistema");
                    cantidadARetirar = Convert.ToDecimal("0.00");
                }
                else
                {
                    cantidadARetirar = Convert.ToDecimal(row.Cells[11].Value);
                }
            }

            row.Cells[11].Value = String.Format("{0:#,0.000}", cantidadARetirar);
        }

        //CARGAR COMBO DENTRO DE MI LISTADO
        public void CargarComboData()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            da = new SqlDataAdapter("SELECT * FROM TipoMovimientosEntradaSalidaAlmacen WHERE EntradaSalida = 'SALIDA'", con);
            da.Fill(ds, "tipoMovimeintoEntradaSalidaAlmacen");

            DataGridViewComboBoxColumn dgvCombo = datalistadoProductosRequerimiento.Columns["columTipoMovimeinto"] as DataGridViewComboBoxColumn;
            {
                var withBlock = dgvCombo;
                withBlock.Width = 220;
                withBlock.DataSource = ds.Tables["tipoMovimeintoEntradaSalidaAlmacen"];
                withBlock.DisplayMember = "Descripcion";
                //withBlock.DataPropertyName = "IdBonificacion";
                withBlock.ValueMember = "IdTipoMovimientoEntradaAlmacen";
                withBlock.DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Calibri", 8),
                };
            }

            // Establecer valores predeterminados en las columnas ComboBox
            foreach (DataGridViewRow row in datalistadoProductosRequerimiento.Rows)
            {
                if (!row.IsNewRow) // Ignorar la fila nueva (si aplica)
                {
                    // Establecer el primer valor de la columna 'bonificacion'
                    if (ds.Tables["tipoMovimeintoEntradaSalidaAlmacen"].Rows.Count > 0)
                    {
                        row.Cells["columTipoMovimeinto"].Value = ds.Tables["tipoMovimeintoEntradaSalidaAlmacen"].Rows[1]["IdTipoMovimientoEntradaAlmacen"];
                    }
                }
            }
        }

        //FUNCION PARA GENERAR UN CODIGO A MI NOTA DE SALIDA
        public void CodigoNotaSalida()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            da = new SqlDataAdapter("SELECT IdSalidaAlmacen FROM Kardex_SalidaAlmacen WHERE IdSalidaAlmacen = (SELECT MAX(IdSalidaAlmacen) FROM Kardex_SalidaAlmacen)", con);
            da.Fill(dt);
            datalistadoCodigoNotaSalida.DataSource = dt;
            con.Close();

            if (datalistadoCodigoNotaSalida.Rows.Count != 0)
            {
                int numeroSalidaAlmacen = Convert.ToInt32(datalistadoCodigoNotaSalida.SelectedCells[0].Value.ToString());
                int numeroSalidaAlmacen2 = 0;
                string numeroSalidaAlmacen3 = "";
                numeroSalidaAlmacen2 = numeroSalidaAlmacen;
                numeroSalidaAlmacen2 = numeroSalidaAlmacen2 + 1;
                numeroSalidaAlmacen3 = Convert.ToString(numeroSalidaAlmacen2);

                if (numeroSalidaAlmacen3.Length == 1)
                {
                    codigoSalidaAlmacen = "NS" + DateTime.Now.Year + "0000" + numeroSalidaAlmacen3;
                }
                else if (numeroSalidaAlmacen3.Length == 2)
                {
                    codigoSalidaAlmacen = "NS" + DateTime.Now.Year + "000" + numeroSalidaAlmacen3;
                }
                else if (numeroSalidaAlmacen3.Length == 3)
                {
                    codigoSalidaAlmacen = "NS" + DateTime.Now.Year + "00" + numeroSalidaAlmacen3;
                }
                else if (numeroSalidaAlmacen3.Length == 4)
                {
                    codigoSalidaAlmacen = "NS" + DateTime.Now.Year + "0" + numeroSalidaAlmacen3;
                }
                else if (numeroSalidaAlmacen3.Length == 5)
                {
                    codigoSalidaAlmacen = "NS" + DateTime.Now.Year + numeroSalidaAlmacen3;
                }
            }
            else
            {
                MessageBox.Show("Se debe inicializar la tabla NOTA DE SALIDA.", "Validación del Sistema", MessageBoxButtons.OK);
            }
        }

        //FUNCION PARA GENERAR UN CODIGO A MI NOTA DE SALIDA
        public void CargarDetalleProductoSalida(string codigoRequerimeinto, string codigoProducto)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarDetallesProductoSalida", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigoRequerimiento", codigoRequerimeinto);
            cmd.Parameters.AddWithValue("@codigoproducto", codigoProducto);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoDetalleProductoSalida.DataSource = dt;
            con.Close();
        }

        //ACCIONES SEGÚN EL TIPO DE BÚSQUEDA SELECCIONADA--------------------------------------------------
        //BÚSQUEDA DE REQUERIMEINTO SIMPLE
        private void rbRequerimiento_CheckedChanged(object sender, EventArgs e)
        {
            CargarRequerimientosSimples();
            CargarAlmacenes();
            CargarTiposMovimientos();
            cboTipoMovimeintos.SelectedIndex = 1;
        }

        //BUSQUEDA DE REQUERIMIENTO
        private void txtBusquedaReque_TextChanged(object sender, EventArgs e)
        {
            if (rbRequerimiento.Checked == false && rbRequerimientoOT.Checked == false && rbRequerimientoOP.Checked == false && rbPedidoGenerado.Checked == false)
            {
                MessageBox.Show("Debe seleccionar un criterio de búsqueda.", "Validación del Sistema", MessageBoxButtons.OK);
                txtBusquedaReque.Text = "";
            }
            else
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT RS.IdRequerimientoSimple, RS.CodigoRequerimientoSimple, RS.FechaRequerida, RS.FechaSolicitada, RS.IdSolicitante, USU.Nombres + ' ' + USU.ApellidoParterno + ' ' + USU.ApellidoMaterno AS[SOLICITANTE] FROM RequerimientoSimple RS INNER JOIN Usuarios USU ON USU.IdUsuarios = RS.IdSolicitante WHERE RS.Estado = 1 AND RS.IdTipo = 1 AND EstadoAtendido = 0 AND RS.CodigoRequerimientoSimple LIKE '%' + @codigo + '%'", con);
                comando.Parameters.AddWithValue("@codigo", txtBusquedaReque.Text);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    DataTimeFechaReqeurmientoRequerida.Text = System.Convert.ToString(row["FechaRequerida"]);
                    DateTimeFechaRequerimientoSolicitada.Text = System.Convert.ToString(row["FechaSolicitada"]);
                    txtSolicitante.Text = System.Convert.ToString(row["SOLICITANTE"]);
                    lblIdSolicitante.Text = System.Convert.ToString(row["IdSolicitante"]);
                    cboComboReqeurimientoSimple.DataSource = dt;
                }
                else
                {
                    // Maneja el caso donde no hay registros
                    MessageBox.Show("No se encontraron registros.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBusquedaReque.Text = "";
                }

                cboComboReqeurimientoSimple.DisplayMember = "CodigoRequerimientoSimple";
                cboComboReqeurimientoSimple.ValueMember = "IdRequerimientoSimple";
            }
        }

        //BÚSQUEDA DE REQUERIMIENTO SIMPLE - DETALLES
        private void cboComboReqeurimientoSimple_SelectedIndexChanged(object sender, EventArgs e)
        {
            datalistadoProductosRequerimiento.Rows.Clear();

            //SI SE PRECIONA SOBRE LA COLUMNA CON EL NOMBRE SELECCIOANDO
            if (cboComboReqeurimientoSimple.Text != "")
            {
                //CAPTURAR EL CÓDIFO DE MI REQUERIMIENTO SIMPLE
                int idRequerimiento = Convert.ToInt32(cboComboReqeurimientoSimple.SelectedValue.ToString());
                MostrarItemsSegunRequerimiento(idRequerimiento);
                estadoRequerimeintoAtendidoTotal = true;
                CargarComboData();
            }
        }

        //BÚSQUEDA DE REQUERIMEINTO POR ORDEN DE TRABAJO
        private void rbPedidoGenerado_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        //BÚSQUEDA DE REQUERIMIENTO POR ORDEN DE PRODUCCION
        private void rbRequerimientoOP_CheckedChanged(object sender, EventArgs e)
        {

        }

        //BÚSQUEDA DE REQUERIMIENTO POR PEDIDO GENERADO
        private void rbRequerimientoOT_CheckedChanged(object sender, EventArgs e)
        {

        }

        //ELIMINAR ITEM DE LA NOTA DE SALIDA
        private void btnEliminarItemNotaSalida_Click(object sender, EventArgs e)
        {
            DialogResult resul = MessageBox.Show("Seguro que desea eliminar este registro?.", "Eliminar Item", MessageBoxButtons.YesNo);
            if ((resul == DialogResult.Yes))
            {
                for (int Row = datalistadoProductosRequerimiento.Rows.Count - 1; Row >= 0; Row += -1)
                {
                    if (Convert.ToBoolean(datalistadoProductosRequerimiento.Rows[Row].Cells[0].Value))
                    {
                        datalistadoProductosRequerimiento.Rows.RemoveAt(Row);
                        estadoRequerimeintoAtendidoTotal = false;
                    }
                }
            }
        }

        //FILTRAR QUE SOLO SE INGRESEN NÚMEROS
        private void txtBusquedaReque_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //GUARDAR NOTA DE SALIDA - REQUERIMEINTO SIMPLE
        private void btnGuardarRequeSimple_Click(object sender, EventArgs e)
        {
            ValidarCantidadesExactas = true;

            if (rbRequerimiento.Checked == true)
            {
                if (DataTimeFechaReqeurmientoRequerida.Text != "" && txtSolicitante.Text != "")
                {
                    if (datalistadoProductosRequerimiento.RowCount > 0)
                    {
                        //VALIDACIÓN DE CANTIDADES - SI LA CANTIDAD DESEADA SE ENCUENTRA EN EL ALMACÉN
                        foreach (DataGridViewRow row in datalistadoProductosRequerimiento.Rows)
                        {
                            decimal cantidadRequerida = Convert.ToDecimal(row.Cells[8].Value.ToString());
                            decimal cantidadRetirada = Convert.ToDecimal(row.Cells[9].Value.ToString());
                            decimal cantidadAlmacen = Convert.ToDecimal(row.Cells[10].Value.ToString());
                            decimal cantidadReitrar = Convert.ToDecimal(row.Cells[11].Value.ToString());
                            decimal cantidadRetiradaMenosRequerida = cantidadRequerida - cantidadRetirada;

                            if (cantidadReitrar > cantidadAlmacen)
                            {
                                MessageBox.Show("No hay la cantidad deseada en los almacenes, por favor contactarse con el área de logística para coordinar el stock.", "Validación de Sistema");
                                return;
                            }
                            else if (cantidadRetiradaMenosRequerida > cantidadReitrar)
                            {
                                ValidarCantidadesExactas = false;
                            }
                        }

                        try
                        {
                            //CONFIRMACIÓN PARA PODER GUARDAR LA NOTA DE SALIDA
                            DialogResult boton = MessageBox.Show("¿Realmente desea guardar esta nota de salida?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                            if (boton == DialogResult.OK)
                            {
                                //PROCEDIMEINTO ALMACENADO PARA HACER LA ACCIÓN DE GUARDAR
                                SqlConnection con = new SqlConnection();
                                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                                con.Open();
                                SqlCommand cmd = new SqlCommand();
                                cmd = new SqlCommand("InsertarNotaSalida", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                CodigoNotaSalida();
                                //INGRESO DEL ENCABEZADO DEL REQUERIMIENTO
                                cmd.Parameters.AddWithValue("@codigfoSalidaAlmacen", codigoSalidaAlmacen);
                                cmd.Parameters.AddWithValue("@fechaSalida", DateTime.Now);
                                cmd.Parameters.AddWithValue("@idTipoSalida", 1); //1 ES REQUERIMIENTO SIMPLE
                                cmd.Parameters.AddWithValue("@numeroOrden", "001");
                                cmd.Parameters.AddWithValue("@fechaOrden", DateTime.Now);
                                cmd.Parameters.AddWithValue("@numeroRequerimeinto", cboComboReqeurimientoSimple.Text);
                                cmd.Parameters.AddWithValue("@fechaRequerimiento", Convert.ToDateTime(DataTimeFechaReqeurmientoRequerida.Text));
                                cmd.Parameters.AddWithValue("@idTipoMovimiento", cboTipoMovimeintos.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@idTipoAlmacen", cboAlmacenes.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@idUsuario", Convert.ToInt32(lblIdSolicitante.Text));
                                cmd.Parameters.AddWithValue("@observaciones", txtObservaciones.Text);

                                cmd.Parameters.AddWithValue("@codigoRequerimeinto", cboComboReqeurimientoSimple.Text);

                                if (estadoRequerimeintoAtendidoTotal == true && ValidarCantidadesExactas == true)
                                {
                                    cmd.Parameters.AddWithValue("@estadoReque", 6);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@estadoReque", 5);
                                }

                                cmd.ExecuteNonQuery();
                                con.Close();

                                //INGRESO DE LOS DETALLES DE LA NOTA DE SALIDA
                                foreach (DataGridViewRow row in datalistadoProductosRequerimiento.Rows)
                                {
                                    //PROCEDIMIENTO ALMACENADO PARA GUARDAR LOS DETALLES DE LA NOTA DE SALIDA
                                    con.Open();
                                    cmd = new SqlCommand("InsertarNotaSalida_Detalles", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@idart", Convert.ToInt32(row.Cells[4].Value));
                                    cmd.Parameters.AddWithValue("@cantidad", Convert.ToDecimal(row.Cells[11].Value));
                                    cmd.Parameters.AddWithValue("@precioUnitarioDolares", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@precioTotalDolares", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@precioUnitarioSoles", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@precioTotalSoles", DBNull.Value);

                                    string str = Convert.ToString(row.Cells["columTipoMovimeinto"].Value);
                                    if (str == "") { str = "42"; }
                                    cmd.Parameters.AddWithValue("@idTipoMovimiento", str);
                                    //EDIDION DEL REQUERMIENTO  SUS DETALLES
                                    cmd.Parameters.AddWithValue("@codigoDetalleRequerimientoSimple", Convert.ToInt32(row.Cells[1].Value));

                                    decimal cantidadRetirar = Convert.ToDecimal(row.Cells[11].Value);
                                    decimal cantidadRetirada = Convert.ToDecimal(row.Cells[9].Value);
                                    decimal cantidadRequerida = Convert.ToDecimal(row.Cells[8].Value);
                                    decimal cantidadRequeridaMenosCantidadRetirada = cantidadRequerida - cantidadRetirada;
                                    if (cantidadRetirar == cantidadRequeridaMenosCantidadRetirada)
                                    {
                                        cmd.Parameters.AddWithValue("@estadoAtendido", 2);
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@estadoAtendido", 1);
                                    }

                                    decimal sumaTotal = 0;
                                    CargarDetalleProductoSalida(cboComboReqeurimientoSimple.Text, Convert.ToString(row.Cells[4].Value));
                                    foreach (DataGridViewRow row2 in datalistadoDetalleProductoSalida.Rows)
                                    {
                                        sumaTotal = sumaTotal + Convert.ToDecimal(row2.Cells[0].Value);
                                    }

                                    sumaTotal = sumaTotal + Convert.ToDecimal(row.Cells[11].Value);
                                    cmd.Parameters.AddWithValue("@cantidadRetirada", sumaTotal);

                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }

                                MessageBox.Show("Se ingresó correctamente la nota de salida.", "Validación del Sistema");
                                datalistadoProductosRequerimiento.Rows.Clear();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se cargaron los detalles del requerimiento correctamente, por favor comunicarse con el área de sistemas.", "Validación del Sistema");
                    }
                }
                else
                {
                    MessageBox.Show("No se cargaron los datos correctamente, por favor comunicarse con el área de sistemas.", "Validación del Sistema");
                }
            }
            else if (rbRequerimientoOT.Checked == true)
            {

            }
            else if (rbRequerimientoOP.Checked == true)
            {

            }
            else if (rbPedidoGenerado.Checked == true)
            {

            }
            else
            {
                MessageBox.Show("Error inesperado. por favor comunicarse con el área de sistemas.", "Validación del Sistema");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArenasProyect3.Modulos.ManGeneral;

namespace ArenasProyect3.Modulos.Comercial.RequerimientosVentas
{
    public partial class ActasVisita : Form
    {
        //VARIABLES GLOBALES PARA MIS ACTAS DE VISITA
        int idActa = 0;
        string reconocerImagen1 = "";
        string reconocerImagen2 = "";
        string reconocerImagen3 = "";
        private Cursor curAnterior = null;
        string ruta = ManGeneral.Manual.manualViajes;

        //CONSTRUCTOR DEL MANTENIMIENTO - ACTAS DE VISITA
        public ActasVisita()
        {
            InitializeComponent();
        }

        //INICIO Y CARGA INICIAL DE ACTAS DE VISITA - CONSTRUCTOR--------------------------------------------------------------------------------------
        private void ActasVisita_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);
            DateTime oUltimoDiaDelMes = oPrimerDiaDelMes.AddMonths(1).AddDays(-1);

            DesdeFecha.Value = oPrimerDiaDelMes;
            HastaFecha.Value = oUltimoDiaDelMes;
            datalistadoTodasActas.DataSource = null;

            //PREFILES Y PERSIMOS---------------------------------------------------------------
            if (Program.RangoEfecto != 1)
            {

            }
            //---------------------------------------------------------------------------------
        }

        //CARGA DE COMBOS PARA LINEA Y OTROS----------------------------
        //cargar linea de trabajo
        public void CargarLineaTrabajo()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand("ListarComboLineaTrabajo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter data = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cboLineaTrabajo.DisplayMember = "Descripcion";
                cboLineaTrabajo.ValueMember = "IdLinea";
                cboLineaTrabajo.DataSource = dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar procedimiento almacenado " + ex.Message);
            }
        }

        //CARGAR TIPO DE CUENTA
        public void CargarTipoCuenta(ComboBox cbo)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdTipoCuenta, Descripcion FROM TipoCuenta WHERE Estado = 1 AND IdTipoCuenta = 4 ORDER BY Descripcion", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cbo.DisplayMember = "Descripcion";
            cbo.ValueMember = "IdTipoCuenta";
            cbo.DataSource = dt;
        }

        //CARGAR RESPONSABLE
        public void CargarResponsables(ComboBox cbo)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdUsuarios, Nombres + ' ' + Apellidos AS [NOMBRES] FROM Usuarios WHERE Estado = 'Activo' AND HabilitadoRequerimientoVenta = 1 ORDER BY Nombres", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cbo.DisplayMember = "NOMBRES";
            cbo.ValueMember = "IdUsuarios";
            cbo.DataSource = dt;
        }

        //CARGAR EQUIPO DE AREAS
        public void CargarEquiposAreas()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT IdEquipoArea, DescripcionEquipoArea FROM EquipoArea EA WHERE EA.Estado = 1 AND EA.IdEquipoArea = 4", con);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            cboEquipoArea.DisplayMember = "DescripcionEquipoArea";
            cboEquipoArea.ValueMember = "IdEquipoArea";
            cboEquipoArea.DataSource = dt;
        }

        //CARGAR CODIGOS DE LINEAS DE TRABAJO
        public void CargarCodigoLineaTrabajo()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            da = new SqlDataAdapter("SELECT IdLineaTrabajo FROM LineaTrabajo WHERE IdLineaTrabajo = (SELECT MAX(IdLineaTrabajo) FROM LineaTrabajo)", con);
            da.Fill(dt);
            datalistadoCoidgoLineaTrabajo.DataSource = dt;
            con.Close();
        }

        //CARGAR ANTEDECENTES DE MI LINEA DE TRABAJO
        public void CargarAntecedentesLineaTrabajo(int idcliente, int idunidad)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT TOP 1 IdLineaTrabajo,C.NombreCliente + ' ' + PrimerNombre + ' ' + SegundoNombre + ' ' + ApellidoPaterno + ' ' + ApellidoMaterno ,LT.IdCliente ,LT.AntecedentesDescripcion FROM LineaTrabajo LT  INNER JOIN Clientes C ON C.IdCliente = LT.IdCliente  INNER JOIN Acta AC ON AC.IdActa = LT.IdActa  WHERE LT.IdCliente = @idcliente AND LT.IdUnidad =  @idunidad AND LT.Estado = 1 AND AC.EstadoActa = 2 ORDER BY IdLineaTrabajo DESC", con);
            comando.Parameters.AddWithValue("@idcliente", idcliente);
            comando.Parameters.AddWithValue("@idunidad", idunidad);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            datalistadoAntecedentesLineaTrabajo.DataSource = dt;
            con.Close();
        }
        
        //CARGAR ANTECEDENTES POR LINEA DE TRABAJO EDICION
        public void CargarAntecedentesLineaTrabajoEdicion(int idcliente, int idunidad)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand comando = new SqlCommand("SELECT TOP 1 IdLineaTrabajo,C.NombreCliente + ' ' + PrimerNombre + ' ' + SegundoNombre + ' ' + ApellidoPaterno + ' ' + ApellidoMaterno ,LT.IdCliente ,LT.AntecedentesDescripcion FROM LineaTrabajo LT INNER JOIN Clientes C ON C.IdCliente = LT.IdCliente INNER JOIN Acta AC ON AC.IdActa = LT.IdActa WHERE LT.IdCliente = @idcliente AND LT.IdUnidad =  @idunidad AND LT.Estado = 1 ORDER BY IdLineaTrabajo DESC", con);
            comando.Parameters.AddWithValue("@idcliente", idcliente);
            comando.Parameters.AddWithValue("@idunidad", idunidad);
            SqlDataAdapter data = new SqlDataAdapter(comando);
            DataTable dt = new DataTable();
            data.Fill(dt);
            datalistadoAntecedentesLineaTrabajoEdicion.DataSource = dt;
            con.Close();
        }

        //CARGA Y BUSQUEDA DE DATOS
        public void BuscarActaGeneral(int codigoActa)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarActaPorCodigo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@codigo", codigoActa);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoBusquedaActaGeneral.DataSource = dt;
            con.Close();
        }

        //CARGA CONTACTOS DEL CLIENTE
        public void CargarContactoSegunCLiente(ComboBox cbo, int idClinete, Label lblTelefono, Label lblCargo, Label lblCorreo)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT DACC.IdDatosAnexosClienteContacto, DACC.Descripcion, DACC.Telefono, C.Descripcion AS CARGO, DACC.Correo FROM DatosAnexosCliente_Contacto DACC INNER JOIN Cargo C on C.IdCargo = DACC.IdCargo WHERE IdCliente = @idcliente ORDER BY  DACC.Descripcion", con);
                comando.Parameters.AddWithValue("@idcliente", idClinete);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);
                cbo.DisplayMember = "Descripcion";
                cbo.ValueMember = "IdDatosAnexosClienteContacto";
                DataRow row = dt.Rows[0];
                lblTelefono.Text = System.Convert.ToString(row["Telefono"]);
                lblCargo.Text = System.Convert.ToString(row["Descripcion"]);
                lblCorreo.Text = System.Convert.ToString(row["Correo"]);
                cbo.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de carga de datos, no se tiene un contacto registrado para este cliente, " + ex.Message, "Validación del sistema");
            }
        }
        //-----------------------------------------------------------------------------

        //LISTADO DE LIQUIDACIONES Y SELECCION DE PDF Y ESTADO DE ACTAS---------------------
        //MOSTRAR REQUERIMIENTOS AL INCIO 
        public void MostrarActasPorFecha(DateTime fechaInicio, DateTime fechaTermino)
        {
            if (lblCarga.Text == "0")
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("MostrarActasPorFecha_Jefatura", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                datalistadoTodasActas.DataSource = dt;
                con.Close();
                RediemndionarListado(datalistadoTodasActas);
            }
        }

        //MOSTRAR ACTAS POR RESPONSABLE
        public void MostrarActasResponsable(string resopnsable, DateTime fechaInicio, DateTime fechaTermino)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("MostrarActasPorResponsable", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@responsable", resopnsable);
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaTermino", fechaTermino);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoTodasActas.DataSource = dt;
            con.Close();
            RediemndionarListado(datalistadoTodasActas);
        }

        //FINCION PARA REDIMENCIONAR MI LISTADO
        public void RediemndionarListado(DataGridView DGV)
        {
            //NO MOSTRAR LAS COLUMNAS QUE NO SEAN DE REELEVANCIA PARA EL USUARIO
            DGV.Columns[3].Visible = false;
            DGV.Columns[7].Visible = false;
            DGV.Columns[9].Visible = false;
            DGV.Columns[11].Visible = false;
            DGV.Columns[14].Visible = false;
            DGV.Columns[15].Visible = false;
            //BLOQUEAR LAS COLUMNAS Y HACERLAS DE SOLO LECTURA
            DGV.Columns[1].ReadOnly = true;
            DGV.Columns[2].ReadOnly = true;
            DGV.Columns[5].ReadOnly = true;
            DGV.Columns[6].ReadOnly = true;
            DGV.Columns[8].ReadOnly = true;
            DGV.Columns[10].ReadOnly = true;
            DGV.Columns[12].ReadOnly = true;
            DGV.Columns[13].ReadOnly = true;
            //REDIMENSIONAR LAS COLUMNAS SEGUN EL TEMAÑO REQUERIDO
            DGV.Columns[1].Width = 55;
            DGV.Columns[2].Width = 55;
            DGV.Columns[4].Width = 90;
            DGV.Columns[5].Width = 90;
            DGV.Columns[6].Width = 90;
            DGV.Columns[8].Width = 350;
            DGV.Columns[10].Width = 150;
            DGV.Columns[12].Width = 198;
            DGV.Columns[13].Width = 90;
            //CARGAR EL MÉTODO QUE COLOREA LAS FILAS
            ColoresListado();

            //DESHABILITAR EL CLICK Y REORDENAMIENTO POR COLUMNAS
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //COLOREAR REGISTROS
        public void ColoresListado()
        {
            try
            {
                for (var i = 0; i <= datalistadoTodasActas.RowCount - 1; i++)
                {
                    if (datalistadoTodasActas.Rows[i].Cells[13].Value.ToString() == "PENDIENTE")
                    {
                        //PENDIENTE
                        datalistadoTodasActas.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else if (datalistadoTodasActas.Rows[i].Cells[13].Value.ToString() == "APROBADO")
                    {
                        //APROBADO
                        datalistadoTodasActas.Rows[i].DefaultCellStyle.ForeColor = Color.ForestGreen;
                    }
                    else if (datalistadoTodasActas.Rows[i].Cells[13].Value.ToString() == "ANULADO")
                    {
                        //DESAPROBADO
                        datalistadoTodasActas.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    }
                    else
                    {
                        //CULMINADO
                        datalistadoTodasActas.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la operación por: " + ex.Message);
            }
        }

        //EVENTO PARA PODER CAMBIAR EL CURSOR AL PASAR POR EL BOTÓN DE GENERACIÓN DEL PDF
        private void datalistadoTodasActas_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //SI SE PASA SOBRE UNA COLUMNA DE MI LISTADO CON EL SIGUIENTE NOMBRA
            if (this.datalistadoTodasActas.Columns[e.ColumnIndex].Name == "btnGenerarActa")
            {
                this.datalistadoTodasActas.Cursor = Cursors.Hand;
            }
            else
            {
                this.datalistadoTodasActas.Cursor = curAnterior;
            }
        }

        //MOSTRAR ACTAS DE VISITA POR RESPONSABLE
        private void txtBusquedaResponsable_TextChanged(object sender, EventArgs e)
        {
            MostrarActasResponsable(txtBusquedaResponsable.Text, DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR ACTAS DE VISITA POR FECHAS
        private void DesdeFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarActasPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR ACTAS DE VISITA POR FECHAS
        private void HastaFecha_ValueChanged(object sender, EventArgs e)
        {
            MostrarActasPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //MOSTRAR ACTAS
        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            MostrarActasPorFecha(DesdeFecha.Value, HastaFecha.Value);
        }

        //SELECCION DE LA ACTA Y CARGA DE SUS LINEA DE TRABAJO---------------------------
        //PROCESO PARA PRESIONAR Y CARGAR TODO LOS DATOS PARA EL INGRESO
        public void BuscarLineaTrabajoActa(int idActa)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarLineaTrabajoActa", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idActa", idActa);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoLineasTrabajo.DataSource = dt;
            con.Close();
            //OCULTAR LAS COLUMNAS QUE NO SON REELEVANTES PARA EL USUARIO
            datalistadoLineasTrabajo.Columns[0].Visible = false;
            datalistadoLineasTrabajo.Columns[5].Visible = false;
            datalistadoLineasTrabajo.Columns[6].Visible = false;
            datalistadoLineasTrabajo.Columns[7].Visible = false;
            datalistadoLineasTrabajo.Columns[8].Visible = false;
            datalistadoLineasTrabajo.Columns[9].Visible = false;
            datalistadoLineasTrabajo.Columns[10].Visible = false;
            datalistadoLineasTrabajo.Columns[11].Visible = false;
            datalistadoLineasTrabajo.Columns[12].Visible = false;
            datalistadoLineasTrabajo.Columns[13].Visible = false;
            datalistadoLineasTrabajo.Columns[14].Visible = false;
            datalistadoLineasTrabajo.Columns[15].Visible = false;
            //REDIMENSIONAR EL TAMAÑO DE MIS COLUMAS DE MI LÍNEA DE TRABAJO
            datalistadoLineasTrabajo.Columns[1].Width = 150;
            datalistadoLineasTrabajo.Columns[2].Width = 185;
            datalistadoLineasTrabajo.Columns[3].Width = 160;
            datalistadoLineasTrabajo.Columns[4].Width = 350;
        }

        //SELECCIONAR UNA ACTA Y VER SUS DETALLES
        private void datalistadoTodasActas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            BuscarLineaTrabajoActa(Convert.ToInt32(datalistadoTodasActas.SelectedCells[1].Value.ToString()));
            DatetimeFechaInicioActa.Value = Convert.ToDateTime(datalistadoTodasActas.SelectedCells[5].Value.ToString());
            DatetimeFechaTerminoActa.Value = Convert.ToDateTime(datalistadoTodasActas.SelectedCells[6].Value.ToString());
            txtCLienteActa.Text = datalistadoTodasActas.SelectedCells[8].Value.ToString();
            txtUnidadActa.Text = datalistadoTodasActas.SelectedCells[10].Value.ToString();
            txtResponsableActa.Text = datalistadoTodasActas.SelectedCells[12].Value.ToString();
            panelNuevaActa.Visible = true;
        }

        //ACCIONES DE ACTAS - NUEVA LINEA DE TRABAJO------------------------------------------------------------
        private void btnGuardarLineaTrabajo_Click(object sender, EventArgs e)
        {
            string estadoActa = datalistadoTodasActas.SelectedCells[13].Value.ToString();

            if (estadoActa == "CULMINADO" || estadoActa == "APROBADO" || estadoActa == "ANULADO")
            {
                MessageBox.Show("No se puede continuar ya que el acta culminó, ya se generó o esta anulada.", "Validación del Sistema", MessageBoxButtons.OKCancel);
            }
            else
            {
                if (datalistadoLineasTrabajo.RowCount >= 1)
                {
                    MessageBox.Show("Ya se ingresó un detalle de acta.", "Validación del Sistema");
                }
                else
                {
                    CargarLineaTrabajo();

                    int inLinea = Convert.ToInt32(cboLineaTrabajo.SelectedValue.ToString());
                    int idCliente = Convert.ToInt32(datalistadoTodasActas.SelectedCells[7].Value.ToString());
                    int idUnidad = Convert.ToInt32(datalistadoTodasActas.SelectedCells[9].Value.ToString());

                    CargarTipoCuenta(cboTipoCuenta);
                    CargarResponsables(cboCargarResponsables);
                    CargarEquiposAreas();
                    CargarAntecedentesLineaTrabajo(idCliente, idUnidad);

                    foreach (DataGridViewRow row in datalistadoAntecedentesLineaTrabajo.Rows)
                    {
                        String anteedente = Convert.ToString(row.Cells[3].Value);
                        txtAntecedentes.AppendText(anteedente + "\r\n");
                        txtAntecedentes.AppendText("\r\n");
                    }

                    panelNuevaLineaTrabajo.Visible = true;
                    lblLineaTrabajo.Text = "NUEVA LINEA DE TRABAJO";
                }
            }
        }

        //RECURSOS DE LA NUEVA LINEA DE TRABAJO
        private void btnCargarImagen1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Todos los archivos (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtImagen1.Text = openFileDialog1.FileName;
                reconocerImagen1 = "1";
            }
        }

        //RECURSOS DE LA NUEVA LINEA DE TRABAJO
        private void btnCargarImagen2_Click(object sender, EventArgs e)
        {
            openFileDialog2.InitialDirectory = "c:\\";
            openFileDialog2.Filter = "Todos los archivos (*.*)|*.*";
            openFileDialog2.FilterIndex = 1;
            openFileDialog2.RestoreDirectory = true;

            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                txtImagen2.Text = openFileDialog2.FileName;
                reconocerImagen2 = "2";
            }
        }

        //RECURSOS DE LA NUEVA LINEA DE TRABAJO
        private void btnCargarImagen3_Click(object sender, EventArgs e)
        {
            openFileDialog3.InitialDirectory = "c:\\";
            openFileDialog3.Filter = "Todos los archivos (*.*)|*.*";
            openFileDialog3.FilterIndex = 1;
            openFileDialog3.RestoreDirectory = true;

            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                txtImagen3.Text = openFileDialog3.FileName;
                reconocerImagen3 = "3";
            }
        }
        //------------------------------------------------------------------------------

        //CONFIRMAR Y GUARDAR UNA NUEVA LINEA DE TRABAJO
        private void btnGuardarNuevaLineaTrabajo_Click(object sender, EventArgs e)
        {
            if (lblLineaTrabajo.Text == "NUEVA LINEA DE TRABAJO")
            {
                if (cboEquipoArea.Text == "")
                {
                    MessageBox.Show("Debe seleccionar un equipo o área para poder continuar.", "Validación del Sistema");
                }
                else
                {
                    if (txtAntecedentes.Text == "" || txtDesarrollo.Text == "" || txtResultado.Text == "" || txtAcciones.Text == "")
                    {
                        MessageBox.Show("Debe llenar todos los campos para poder continuar.", "Validación del Sistema");
                    }
                    else
                    {
                        DialogResult boton = MessageBox.Show("¿Realmente desea guardar este detalle de acta?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                        if (boton == DialogResult.OK)
                        {
                            try
                            {
                                SqlConnection con = new SqlConnection();
                                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                                con.Open();
                                SqlCommand cmd = new SqlCommand();
                                cmd = new SqlCommand("InsertarLineaTrabajoActa", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                string coidgoActa = datalistadoTodasActas.SelectedCells[1].Value.ToString();
                                int idCliente = Convert.ToInt32(datalistadoTodasActas.SelectedCells[7].Value.ToString());
                                int idUnidad = Convert.ToInt32(datalistadoTodasActas.SelectedCells[9].Value.ToString());

                                //INGRESO
                                cmd.Parameters.AddWithValue("@idLinea", cboLineaTrabajo.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@idEquipoArea", cboEquipoArea.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@idTipoCuenta", cboTipoCuenta.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@idActa", Convert.ToInt32(coidgoActa));
                                cmd.Parameters.AddWithValue("@idCliente", idCliente);
                                cmd.Parameters.AddWithValue("@idUnidad", idUnidad);
                                cmd.Parameters.AddWithValue("@antecedentesDescripcion", txtAntecedentes.Text);
                                cmd.Parameters.AddWithValue("@desarrolloDescripcion", txtDesarrollo.Text);
                                cmd.Parameters.AddWithValue("@resultadoDescripcion", txtResultado.Text);

                                CargarCodigoLineaTrabajo();

                                int codigoLineaTrabajo = 0;

                                if (datalistadoCoidgoLineaTrabajo.RowCount > 0)
                                {
                                    codigoLineaTrabajo = Convert.ToInt32(datalistadoCoidgoLineaTrabajo.SelectedCells[0].Value.ToString()) + 1;
                                }
                                else
                                {
                                    codigoLineaTrabajo = 1;

                                }
                                //PRIMERA IMAGEN
                                if (txtImagen1.Text != "")
                                {
                                    string nombreGenerado1 = "LINEA DE TRABAJO N " + Convert.ToString(codigoLineaTrabajo) + " - IMAGEN " + reconocerImagen1 + " - " + "ACTA N " + coidgoActa;
                                    string rutaOld1 = txtImagen1.Text;

                                    string RutaNew1 = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Comercial\LineaTrabajoImagenes\" + nombreGenerado1 + ".jpg";

                                    File.Copy(rutaOld1, RutaNew1);
                                    cmd.Parameters.AddWithValue("@imagen1", RutaNew1);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@imagen1", "");
                                }

                                //SEGUNDA IMAGEN
                                if (txtImagen2.Text != "")
                                {
                                    string nombreGenerado2 = "LINEA DE TRABAJO N " + Convert.ToString(codigoLineaTrabajo) + " - IMAGEN " + reconocerImagen2 + " - " + "ACTA N " + coidgoActa;
                                    string rutaOld2 = txtImagen2.Text;

                                    string RutaNew2 = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Comercial\LineaTrabajoImagenes\" + nombreGenerado2 + ".jpg";

                                    File.Copy(rutaOld2, RutaNew2);
                                    cmd.Parameters.AddWithValue("@imagen2", RutaNew2);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@imagen2", "");
                                }

                                //TERCERA IMAGEN
                                if (txtImagen3.Text != "")
                                {
                                    string nombreGenerado3 = "LINEA DE TRABAJO N " + Convert.ToString(codigoLineaTrabajo) + " - IMAGEN " + reconocerImagen3 + " - " + "ACTA N " + coidgoActa;
                                    string rutaOld3 = txtImagen3.Text;

                                    string RutaNew3 = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Comercial\LineaTrabajoImagenes\" + nombreGenerado3 + ".jpg";

                                    File.Copy(rutaOld3, RutaNew3);
                                    cmd.Parameters.AddWithValue("@imagen3", RutaNew3);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@imagen3", "");
                                }

                                cmd.Parameters.AddWithValue("@accionesDescripcion", txtAcciones.Text);
                                cmd.Parameters.AddWithValue("@fechaAcciones", datetimePlazoMaximo.Value);
                                cmd.Parameters.AddWithValue("@idresponsabel", Convert.ToInt32(datalistadoTodasActas.SelectedCells[11].Value.ToString()));
                                cmd.Parameters.AddWithValue("@gastoLinea", 0.00);
                                cmd.ExecuteNonQuery();
                                con.Close();

                                MessageBox.Show("Se ingresó el detalle del acta correctamente.", "Validación del Sistema");

                                //INGRESO DE LA TABLA AUDITORA
                                con.Open();
                                cmd = new SqlCommand("InsertarDatosTablaAuditora_Comercial", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@idUsuario", Program.IdUsuario);
                                cmd.Parameters.AddWithValue("@mantenimiento", "Área comercial - Menú Actas - Actas de Visita");
                                cmd.Parameters.AddWithValue("@accion", "Nueva línea de trabajo");
                                cmd.Parameters.AddWithValue("@descripcion", "Línea de trabajo creada por el usuario " + Program.UnoNombreUnoApellidoUsuario + " en la fecha " + DateTime.Now);
                                cmd.Parameters.AddWithValue("@maquina", Environment.MachineName);
                                cmd.Parameters.AddWithValue("@fechaAccion", DateTime.Now);
                                cmd.Parameters.AddWithValue("@nameUsuarioSesion", Environment.UserName);
                                cmd.Parameters.AddWithValue("@codigoRequerimiento", Convert.ToInt32(datalistadoTodasActas.SelectedCells[15].Value.ToString()));
                                cmd.Parameters.AddWithValue("@codigoLiquidacion", Convert.ToInt32(datalistadoTodasActas.SelectedCells[14].Value.ToString()));
                                cmd.Parameters.AddWithValue("@codigoActa", coidgoActa);
                                cmd.Parameters.AddWithValue("@codigoLineaTrabajo", codigoLineaTrabajo);
                                cmd.ExecuteNonQuery();
                                con.Close();

                                panelNuevaLineaTrabajo.Visible = false;

                                BuscarLineaTrabajoActa(Convert.ToInt32(datalistadoTodasActas.SelectedCells[1].Value.ToString()));

                                txtAntecedentes.Text = "";
                                txtDesarrollo.Text = "";
                                txtResultado.Text = "";
                                txtAcciones.Text = "";
                                txtImagen1.Text = "";
                                txtImagen2.Text = "";
                                txtImagen3.Text = "";
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error en el servidor");
                            }
                        }
                    }
                }

            }
            else
            {
                if (cboEquipoArea.Text == "")
                {
                    MessageBox.Show("Debe seleccionar un equipo o área para poder continuar.", "Validación del Sistema");
                }
                else
                {
                    if (txtAntecedentes.Text == "" || txtDesarrollo.Text == "" || txtResultado.Text == "" || txtAcciones.Text == "")
                    {
                        MessageBox.Show("Debe llenar todos los campos para poder continuar.", "Validación del Sistema");
                    }
                    else
                    {
                        DialogResult boton = MessageBox.Show("¿Realmente desea guardar este detalle de acta?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                        if (boton == DialogResult.OK)
                        {
                            try
                            {
                                SqlConnection con = new SqlConnection();
                                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                                con.Open();
                                SqlCommand cmd = new SqlCommand();
                                cmd = new SqlCommand("EditarLineaTrabajoActa", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                string coidgoActa = datalistadoTodasActas.SelectedCells[1].Value.ToString();
                                int idCliente = Convert.ToInt32(datalistadoTodasActas.SelectedCells[7].Value.ToString());
                                int idUnidad = Convert.ToInt32(datalistadoTodasActas.SelectedCells[9].Value.ToString());
                                int idLineaTrabajo = Convert.ToInt32(datalistadoLineasTrabajo.SelectedCells[0].Value.ToString());

                                //INGRESO
                                cmd.Parameters.AddWithValue("@idLineaTrabajo", idLineaTrabajo);
                                cmd.Parameters.AddWithValue("@idLinea", cboLineaTrabajo.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@idEquipoArea", cboEquipoArea.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@idTipoCuenta", cboTipoCuenta.SelectedValue.ToString());
                                cmd.Parameters.AddWithValue("@idActa", Convert.ToInt32(coidgoActa));
                                cmd.Parameters.AddWithValue("@idCliente", idCliente);
                                cmd.Parameters.AddWithValue("@idUnidad", idUnidad);
                                cmd.Parameters.AddWithValue("@antecedentesDescripcion", txtAntecedentes.Text);
                                cmd.Parameters.AddWithValue("@desarrolloDescripcion", txtDesarrollo.Text);
                                cmd.Parameters.AddWithValue("@resultadoDescripcion", txtResultado.Text);

                                string rutaImageSistema1 = datalistadoLineasTrabajo.SelectedCells[13].Value.ToString();
                                //PRIMERA IMAGEN
                                if (txtImagen1.Text == rutaImageSistema1)
                                {
                                    cmd.Parameters.AddWithValue("@imagen1", txtImagen1.Text);
                                }
                                else
                                {
                                    if (txtImagen1.Text != "")
                                    {
                                        if (txtImagen1.Text != "" && rutaImageSistema1 != "")
                                        {
                                            File.Delete(rutaImageSistema1);

                                            string nombreGenerado1 = "LINEA DE TRABAJO N " + Convert.ToString(idLineaTrabajo) + " - IMAGEN " + reconocerImagen1 + " - " + "ACTA N " + coidgoActa;
                                            string rutaOld1 = txtImagen1.Text;

                                            string RutaNew1 = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Comercial\LineaTrabajoImagenes\" + nombreGenerado1 + ".jpg";

                                            File.Copy(rutaOld1, RutaNew1);
                                            cmd.Parameters.AddWithValue("@imagen1", RutaNew1);
                                        }
                                        else
                                        {
                                            string nombreGenerado1 = "LINEA DE TRABAJO N " + Convert.ToString(idLineaTrabajo) + " - IMAGEN " + reconocerImagen1 + " - " + "ACTA N " + coidgoActa;
                                            string rutaOld1 = txtImagen1.Text;

                                            string RutaNew1 = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Comercial\LineaTrabajoImagenes\" + nombreGenerado1 + ".jpg";

                                            File.Copy(rutaOld1, RutaNew1);
                                            cmd.Parameters.AddWithValue("@imagen1", RutaNew1);
                                        }

                                    }
                                    else
                                    {
                                        File.Delete(rutaImageSistema1);
                                        cmd.Parameters.AddWithValue("@imagen1", "");
                                    }
                                }

                                string rutaImageSistema2 = datalistadoLineasTrabajo.SelectedCells[14].Value.ToString();
                                //SEGUNDA IMAGEN
                                if (txtImagen2.Text == rutaImageSistema2)
                                {
                                    cmd.Parameters.AddWithValue("@imagen2", txtImagen2.Text);
                                }
                                else
                                {
                                    if (txtImagen2.Text != "")
                                    {
                                        if (txtImagen2.Text != "" && rutaImageSistema2 != "")
                                        {
                                            File.Delete(rutaImageSistema2);

                                            string nombreGenerado2 = "LINEA DE TRABAJO N " + Convert.ToString(idLineaTrabajo) + " - IMAGEN " + reconocerImagen2 + " - " + "ACTA N " + coidgoActa;
                                            string rutaOld2 = txtImagen2.Text;

                                            string RutaNew2 = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Comercial\LineaTrabajoImagenes\" + nombreGenerado2 + ".jpg";

                                            File.Copy(rutaOld2, RutaNew2);
                                            cmd.Parameters.AddWithValue("@imagen2", RutaNew2);
                                        }
                                        else
                                        {
                                            string nombreGenerado2 = "LINEA DE TRABAJO N " + Convert.ToString(idLineaTrabajo) + " - IMAGEN " + reconocerImagen2 + " - " + "ACTA N " + coidgoActa;
                                            string rutaOld2 = txtImagen2.Text;

                                            string RutaNew2 = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Comercial\LineaTrabajoImagenes\" + nombreGenerado2 + ".jpg";

                                            File.Copy(rutaOld2, RutaNew2);
                                            cmd.Parameters.AddWithValue("@imagen2", RutaNew2);
                                        }
                                    }
                                    else
                                    {
                                        File.Delete(rutaImageSistema2);
                                        cmd.Parameters.AddWithValue("@imagen2", "");
                                    }
                                }

                                string rutaImageSistema3 = datalistadoLineasTrabajo.SelectedCells[15].Value.ToString();
                                //TERCERA IMAGEN
                                if (txtImagen3.Text == rutaImageSistema3)
                                {
                                    cmd.Parameters.AddWithValue("@imagen3", txtImagen3.Text);
                                }
                                else
                                {
                                    if (txtImagen3.Text != "")
                                    {
                                        if (txtImagen3.Text != "" && rutaImageSistema2 != "")
                                        {
                                            File.Delete(rutaImageSistema3);

                                            string nombreGenerado3 = "LINEA DE TRABAJO N " + Convert.ToString(idLineaTrabajo) + " - IMAGEN" + reconocerImagen3 + " - " + "ACTA N " + coidgoActa;
                                            string rutaOld3 = txtImagen3.Text;

                                            string RutaNew3 = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Comercial\LineaTrabajoImagenes\" + nombreGenerado3 + ".jpg";

                                            File.Copy(rutaOld3, RutaNew3);
                                            cmd.Parameters.AddWithValue("@imagen3", RutaNew3);
                                        }
                                        else
                                        {
                                            string nombreGenerado3 = "LINEA DE TRABAJO N " + Convert.ToString(idLineaTrabajo) + " - IMAGEN" + reconocerImagen3 + " - " + "ACTA N " + coidgoActa;
                                            string rutaOld3 = txtImagen3.Text;

                                            string RutaNew3 = @"\\192.168.1.150\arenas1976\ARENASSOFT\RECURSOS\Areas\Comercial\LineaTrabajoImagenes\" + nombreGenerado3 + ".jpg";

                                            File.Copy(rutaOld3, RutaNew3);
                                            cmd.Parameters.AddWithValue("@imagen3", RutaNew3);
                                        }
                                    }
                                    else
                                    {
                                        File.Delete(rutaImageSistema3);
                                        cmd.Parameters.AddWithValue("@imagen3", "");
                                    }
                                }

                                cmd.Parameters.AddWithValue("@accionesDescripcion", txtAcciones.Text);
                                cmd.Parameters.AddWithValue("@fechaAcciones", datetimePlazoMaximo.Value);
                                cmd.Parameters.AddWithValue("@idresponsabel", Convert.ToInt32(datalistadoTodasActas.SelectedCells[11].Value.ToString()));
                                cmd.Parameters.AddWithValue("@gastoLinea", 0.00);
                                cmd.ExecuteNonQuery();
                                con.Close();

                                MessageBox.Show("Se editó el detalle del acta correctamente.", "Validación del Sistema");

                                //INGRESO DE LA TABLA AUDITORA
                                con.Open();
                                cmd = new SqlCommand("InsertarDatosTablaAuditora_Comercial", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@idUsuario", Program.IdUsuario);
                                cmd.Parameters.AddWithValue("@mantenimiento", "Área comercial - Menú Actas - Actas de Visita");
                                cmd.Parameters.AddWithValue("@accion", "Editar línea de trabajo número " + idLineaTrabajo);
                                cmd.Parameters.AddWithValue("@descripcion", "Línea de trabajo editada por el usuario " + Program.UnoNombreUnoApellidoUsuario + " en la fecha " + DateTime.Now);
                                cmd.Parameters.AddWithValue("@maquina", Environment.MachineName);
                                cmd.Parameters.AddWithValue("@fechaAccion", DateTime.Now);
                                cmd.Parameters.AddWithValue("@nameUsuarioSesion", Environment.UserName);
                                cmd.Parameters.AddWithValue("@codigoRequerimiento", Convert.ToInt32(datalistadoTodasActas.SelectedCells[15].Value.ToString()));
                                cmd.Parameters.AddWithValue("@codigoLiquidacion", Convert.ToInt32(datalistadoTodasActas.SelectedCells[14].Value.ToString()));
                                cmd.Parameters.AddWithValue("@codigoActa", coidgoActa);
                                cmd.Parameters.AddWithValue("@codigoLineaTrabajo", idLineaTrabajo);
                                cmd.ExecuteNonQuery();
                                con.Close();

                                panelNuevaLineaTrabajo.Visible = false;

                                BuscarLineaTrabajoActa(Convert.ToInt32(datalistadoTodasActas.SelectedCells[1].Value.ToString()));

                                txtAntecedentes.Text = "";
                                txtDesarrollo.Text = "";
                                txtResultado.Text = "";
                                txtAcciones.Text = "";
                                txtImagen1.Text = "";
                                txtImagen2.Text = "";
                                txtImagen3.Text = "";
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error en el servidor.");
                            }
                        }
                    }
                }
            }
        }

        //REGRESAR DE UNA NUEVA LINEA DE TRABAJO
        private void btnRegresarNuevaLineaTranajo_Click(object sender, EventArgs e)
        {
            panelNuevaLineaTrabajo.Visible = false;

            txtAntecedentes.Text = "";
            txtDesarrollo.Text = "";
            txtResultado.Text = "";
            txtAcciones.Text = "";
            txtImagen1.Text = "";
            txtImagen2.Text = "";
            txtImagen3.Text = "";
        }

        //LINEA DE RABAJO-----------------------
        private void btnEditarLineaTrabajo_Click(object sender, EventArgs e)
        {
            if (datalistadoLineasTrabajo.CurrentRow != null)
            {
                string estadoActa = datalistadoTodasActas.SelectedCells[13].Value.ToString();

                if (estadoActa == "CULMINADO" || estadoActa == "APROBADO" || estadoActa == "ANULADO")
                {
                    MessageBox.Show("No se puede continuar ya que el acta culminó o ya se generó.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                }
                else
                {
                    CargarLineaTrabajo();

                    int inLinea = Convert.ToInt32(cboLineaTrabajo.SelectedValue.ToString());
                    int idCliente = Convert.ToInt32(datalistadoTodasActas.SelectedCells[7].Value.ToString());
                    int idUnidad = Convert.ToInt32(datalistadoTodasActas.SelectedCells[9].Value.ToString());

                    CargarTipoCuenta(cboTipoCuenta);
                    CargarResponsables(cboCargarResponsables);
                    CargarEquiposAreas();
                    CargarAntecedentesLineaTrabajoEdicion(idCliente, idUnidad);

                    foreach (DataGridViewRow row in datalistadoAntecedentesLineaTrabajoEdicion.Rows)
                    {
                        String anteedente = Convert.ToString(row.Cells[3].Value);
                        txtAntecedentes.AppendText(anteedente + "\r\n");
                        txtAntecedentes.AppendText("\r\n");
                    }
                    panelNuevaLineaTrabajo.Visible = true;

                    //REGARGA DE CAMPOS
                    cboLineaTrabajo.SelectedValue = Convert.ToInt32(datalistadoLineasTrabajo.SelectedCells[5].Value.ToString());
                    cboEquipoArea.SelectedValue = Convert.ToInt32(datalistadoLineasTrabajo.SelectedCells[6].Value.ToString());
                    cboTipoCuenta.SelectedValue = Convert.ToInt32(datalistadoLineasTrabajo.SelectedCells[7].Value.ToString());
                    cboCargarResponsables.SelectedValue = Convert.ToInt32(datalistadoLineasTrabajo.SelectedCells[8].Value.ToString());
                    datetimePlazoMaximo.Value = Convert.ToDateTime(datalistadoLineasTrabajo.SelectedCells[9].Value.ToString());
                    //txtAntecedentes.Text = datalistadoLineasTrabajo.SelectedCells[4].Value.ToString();
                    txtDesarrollo.Text = datalistadoLineasTrabajo.SelectedCells[10].Value.ToString();
                    txtResultado.Text = datalistadoLineasTrabajo.SelectedCells[11].Value.ToString();
                    txtAcciones.Text = datalistadoLineasTrabajo.SelectedCells[12].Value.ToString();
                    txtImagen1.Text = datalistadoLineasTrabajo.SelectedCells[13].Value.ToString();
                    txtImagen2.Text = datalistadoLineasTrabajo.SelectedCells[14].Value.ToString();
                    txtImagen3.Text = datalistadoLineasTrabajo.SelectedCells[15].Value.ToString();
                    lblLineaTrabajo.Text = "EDITAR LINEA DE TRABAJO";
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una línea de trabajo para poder ser editada.", "Validación del Sistema");
            }
        }

        //ELIMINAR UNA LINEA DE TRABAJO
        private void btnEliminarLineaTrabajo_Click(object sender, EventArgs e)
        {
            string estadoActa = datalistadoTodasActas.SelectedCells[13].Value.ToString();

            if (datalistadoLineasTrabajo.CurrentRow != null)
            {
                if (estadoActa == "CULMINADO" || estadoActa == "APROBADO" || estadoActa == "ANULADO")
                {
                    MessageBox.Show("No se puede continuar ya que el acta culminó, ya se generó o está anulada.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                }
                else
                {
                    int codigo = Convert.ToInt32(datalistadoLineasTrabajo.SelectedCells[0].Value.ToString());

                    DialogResult boton = MessageBox.Show("¿Realmente desea eliminar esta línea de trabajo?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                    if (boton == DialogResult.OK)
                    {
                        SqlConnection con = new SqlConnection();
                        SqlCommand cmd = new SqlCommand();
                        con.ConnectionString = Conexion.ConexionMaestra.conexion;
                        con.Open();
                        cmd = new SqlCommand("EliminarLineaTrabajo", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", codigo);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Se eliminó esta línea de trabajo correctamente.", "Validación del Sistema");

                        //INGRESO DE LA TABLA AUDITORA
                        con.Open();
                        cmd = new SqlCommand("InsertarDatosTablaAuditora_Comercial", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idUsuario", Program.IdUsuario);
                        cmd.Parameters.AddWithValue("@mantenimiento", "Área comercial - Menú Actas - Actas de Visita");
                        cmd.Parameters.AddWithValue("@accion", "Eliminar línea de trabajo número " + codigo);
                        cmd.Parameters.AddWithValue("@descripcion", "Línea de trabajo eliminada por el usuario " + Program.UnoNombreUnoApellidoUsuario + " en la fecha " + DateTime.Now);
                        cmd.Parameters.AddWithValue("@maquina", Environment.MachineName);
                        cmd.Parameters.AddWithValue("@fechaAccion", DateTime.Now);
                        cmd.Parameters.AddWithValue("@nameUsuarioSesion", Environment.UserName);
                        cmd.Parameters.AddWithValue("@codigoRequerimiento", Convert.ToInt32(datalistadoTodasActas.SelectedCells[15].Value.ToString()));
                        cmd.Parameters.AddWithValue("@codigoLiquidacion", Convert.ToInt32(datalistadoTodasActas.SelectedCells[14].Value.ToString()));
                        cmd.Parameters.AddWithValue("@codigoActa", Convert.ToInt32(datalistadoTodasActas.SelectedCells[1].Value.ToString()));
                        cmd.Parameters.AddWithValue("@codigoLineaTrabajo", codigo);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        BuscarLineaTrabajoActa(Convert.ToInt32(datalistadoTodasActas.SelectedCells[1].Value.ToString()));
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un registro para poder eliminar.", "Validación del Sistema");
            }
        }

        //CULIMAR ACTA-------------------------------------------------------------------------
        private void datalistadoTodasActas_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            txtEstadoValidado.Text = "ESTADO_VALIDADO";
        }

        //BOTON PARA CULMINAR EL ACTA
        private void btnCulminarActa_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasActas.CurrentRow != null)
            {
                string estadoActa = datalistadoTodasActas.SelectedCells[13].Value.ToString();

                if (txtEstadoValidado.Text == "false" || estadoActa == "APROBADO" || estadoActa == "ANULADA")
                {
                    MessageBox.Show("No se puede continuar, ya que no se marcó el check de validado para continuar o ya ha sido aprobada.", "Validación del Sistema");
                }
                else
                {
                    BuscarLineaTrabajoActa(Convert.ToInt32(datalistadoTodasActas.SelectedCells[1].Value.ToString()));

                    if (datalistadoLineasTrabajo.RowCount == 0)
                    {
                        MessageBox.Show("El acta que intenta culminar no tiene ningún detalle registrado, por favor añade una línea de trabajo válida.", "Validación del Sistema");
                        datalistadoTodasActas.SelectedCells[4].Value = false;
                    }
                    else
                    {
                        DialogResult boton = MessageBox.Show("¿Realmente desea validar está acta?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                        if (boton == DialogResult.OK)
                        {
                            SqlConnection con = new SqlConnection();
                            SqlCommand cmd = new SqlCommand();
                            con.ConnectionString = Conexion.ConexionMaestra.conexion;
                            con.Open();
                            cmd = new SqlCommand("CambiarEstadoActa", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idActa", Convert.ToInt32(datalistadoTodasActas.SelectedCells[1].Value.ToString()));
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Se validó el acta y sus detalles correctamente.", "Validación del Sistema");

                            //INGRESO DE LA TABLA AUDITORA
                            con.Open();
                            cmd = new SqlCommand("InsertarDatosTablaAuditora_Comercial", con);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@idUsuario", Program.IdUsuario);
                            cmd.Parameters.AddWithValue("@mantenimiento", "Área comercial - Menú Actas - Actas de Visita");
                            cmd.Parameters.AddWithValue("@accion", "Culminar acta número " + Convert.ToInt32(datalistadoTodasActas.SelectedCells[1].Value.ToString()));
                            cmd.Parameters.AddWithValue("@descripcion", "Acta culminada por el usuario " + Program.UnoNombreUnoApellidoUsuario + " en la fecha " + DateTime.Now);
                            cmd.Parameters.AddWithValue("@maquina", Environment.MachineName);
                            cmd.Parameters.AddWithValue("@fechaAccion", DateTime.Now);
                            cmd.Parameters.AddWithValue("@nameUsuarioSesion", Environment.UserName);
                            cmd.Parameters.AddWithValue("@codigoRequerimiento", Convert.ToInt32(datalistadoTodasActas.SelectedCells[15].Value.ToString()));
                            cmd.Parameters.AddWithValue("@codigoLiquidacion", Convert.ToInt32(datalistadoTodasActas.SelectedCells[14].Value.ToString()));
                            cmd.Parameters.AddWithValue("@codigoActa", Convert.ToInt32(datalistadoTodasActas.SelectedCells[1].Value.ToString()));
                            cmd.Parameters.AddWithValue("@codigoLineaTrabajo", DBNull.Value);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MostrarActasPorFecha(DesdeFecha.Value, HastaFecha.Value);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar una acta para poder continuar.", "Validación del Sistema");

            }
        }

        //EDITAR ACTA - ABRE LA VENTANA DE DETALLES DEL ACTA, LINEA DE TRABAJOS
        private void btnModificarActa_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasActas.CurrentRow != null)
            {
                idActa = Convert.ToInt32(datalistadoTodasActas.SelectedCells[1].Value.ToString());

                if (datalistadoTodasActas.SelectedCells[13].Value.ToString() == "APROBADO" || datalistadoTodasActas.SelectedCells[13].Value.ToString() == "ANULADO")
                {
                    MessageBox.Show("Esta acta ya ha sido aprobada, culminada o anulada.", "Validación del Sistema");
                }
                else
                {
                    BuscarActaGeneral(idActa);

                    int idCLiente = Convert.ToInt32(datalistadoBusquedaActaGeneral.SelectedCells[30].Value.ToString());
                    datatimeFechaInicioNuevaActa.Value = Convert.ToDateTime(datalistadoBusquedaActaGeneral.SelectedCells[1].Value.ToString());
                    datetimeFechaTerminoNuevaActa.Value = Convert.ToDateTime(datalistadoBusquedaActaGeneral.SelectedCells[2].Value.ToString());
                    txtClienteNuevaActa.Text = datalistadoBusquedaActaGeneral.SelectedCells[23].Value.ToString();
                    txtUnidadNuevaActa.Text = datalistadoBusquedaActaGeneral.SelectedCells[24].Value.ToString();
                    int tipoCliente = Convert.ToInt32(datalistadoBusquedaActaGeneral.SelectedCells[3].Value.ToString());

                    if (tipoCliente == 1)
                    {
                        rbTipoClienteActualNuevaActa.Checked = true;
                        rbTipoClienteFuturoNuevaActa.Checked = false;
                    }
                    else
                    {
                        rbTipoClienteActualNuevaActa.Checked = false;
                        rbTipoClienteFuturoNuevaActa.Checked = true;
                    }

                    int frecuenciaVolumen1 = Convert.ToInt32(datalistadoBusquedaActaGeneral.SelectedCells[5].Value.ToString());
                    int frecuenciaVolumen2 = Convert.ToInt32(datalistadoBusquedaActaGeneral.SelectedCells[6].Value.ToString());
                    int frecuenciaVolumen3 = Convert.ToInt32(datalistadoBusquedaActaGeneral.SelectedCells[7].Value.ToString());

                    if (frecuenciaVolumen1 == 1)
                    {
                        rbFrecuenciaAltaNuevaActa.Checked = true;
                        rbFrecuenciaMediaNuevaActa.Checked = false;
                        rbFrecuenduaBajaNuevaActa.Checked = false;
                    }
                    else if (frecuenciaVolumen2 == 1)
                    {
                        rbFrecuenciaAltaNuevaActa.Checked = false;
                        rbFrecuenciaMediaNuevaActa.Checked = true;
                        rbFrecuenduaBajaNuevaActa.Checked = false;
                    }
                    else if (frecuenciaVolumen3 == 1)
                    {
                        rbFrecuenciaAltaNuevaActa.Checked = false;
                        rbFrecuenciaMediaNuevaActa.Checked = false;
                        rbFrecuenduaBajaNuevaActa.Checked = true;
                    }

                    txtAsistentes1NuevaActa.Text = datalistadoTodasActas.SelectedCells[12].Value.ToString();

                    int presente = Convert.ToInt32(datalistadoBusquedaActaGeneral.SelectedCells[29].Value.ToString());

                    if (presente == 1)
                    {
                        ckPresenteAsistente1.Checked = true;
                    }
                    else
                    {
                        ckPresenteAsistente1.Checked = false;
                    }

                    txtAsistentes2NuevaActa.Text = datalistadoBusquedaActaGeneral.SelectedCells[9].Value.ToString();
                    txtAsistentes3NuevaActa.Text = datalistadoBusquedaActaGeneral.SelectedCells[10].Value.ToString();

                    if (txtAsistentes2NuevaActa.Text == "")
                    {
                        CargarResponsables(txtAsistentes2NuevaActa);
                        txtAsistentes2NuevaActa.SelectedIndex = -1;
                    }
                    if (txtAsistentes3NuevaActa.Text == "")
                    {
                        CargarResponsables(txtAsistentes3NuevaActa);
                        txtAsistentes3NuevaActa.SelectedIndex = -1;
                    }

                    //CONTACTO 1
                    txtContactoCliente1NuevaActa.Text = datalistadoBusquedaActaGeneral.SelectedCells[11].Value.ToString();
                    lblContactoCorreo1.Text = datalistadoBusquedaActaGeneral.SelectedCells[12].Value.ToString();
                    lblClienteCargo1.Text = datalistadoBusquedaActaGeneral.SelectedCells[13].Value.ToString();
                    lblContactoTelefono1.Text = datalistadoBusquedaActaGeneral.SelectedCells[14].Value.ToString();
                    //CONTACTO 2
                    txtContactoCliente2NuevaActa.Text = datalistadoBusquedaActaGeneral.SelectedCells[15].Value.ToString();
                    lblContactoCorreo2.Text = datalistadoBusquedaActaGeneral.SelectedCells[16].Value.ToString();
                    lblClienteCargo2.Text = datalistadoBusquedaActaGeneral.SelectedCells[17].Value.ToString();
                    lblContactoTelefono2.Text = datalistadoBusquedaActaGeneral.SelectedCells[18].Value.ToString();
                    //CONTACTO 3
                    txtContactoCliente3NuevaActa.Text = datalistadoBusquedaActaGeneral.SelectedCells[19].Value.ToString();
                    lblContactoCorreo3.Text = datalistadoBusquedaActaGeneral.SelectedCells[20].Value.ToString();
                    lblClienteCargo3.Text = datalistadoBusquedaActaGeneral.SelectedCells[21].Value.ToString();
                    lblContactoTelefono3.Text = datalistadoBusquedaActaGeneral.SelectedCells[22].Value.ToString();

                    if (txtContactoCliente1NuevaActa.Text == "")
                    {
                        CargarContactoSegunCLiente(txtContactoCliente1NuevaActa, idCLiente, lblContactoTelefono1, lblClienteCargo1, lblContactoCorreo1);
                        txtContactoCliente1NuevaActa.SelectedIndex = -1;
                        lblContactoCorreo1.Text = "***";
                        lblClienteCargo1.Text = "***";
                        lblContactoTelefono1.Text = "***";
                    }
                    if (txtContactoCliente2NuevaActa.Text == "")
                    {
                        CargarContactoSegunCLiente(txtContactoCliente2NuevaActa, idCLiente, lblContactoTelefono2, lblClienteCargo2, lblContactoCorreo2);
                        txtContactoCliente2NuevaActa.SelectedIndex = -1;
                        lblContactoCorreo2.Text = "***";
                        lblClienteCargo2.Text = "***";
                        lblContactoTelefono2.Text = "***";
                    }
                    if (txtContactoCliente3NuevaActa.Text == "")
                    {
                        CargarContactoSegunCLiente(txtContactoCliente3NuevaActa, idCLiente, lblContactoTelefono3, lblClienteCargo3, lblContactoCorreo3);
                        txtContactoCliente3NuevaActa.SelectedIndex = -1;
                        lblContactoCorreo3.Text = "***";
                        lblClienteCargo3.Text = "***";
                        lblContactoTelefono3.Text = "***";
                    }

                    int objetivoViaje1 = Convert.ToInt32(datalistadoBusquedaActaGeneral.SelectedCells[25].Value.ToString());
                    int objetivoViaje2 = Convert.ToInt32(datalistadoBusquedaActaGeneral.SelectedCells[26].Value.ToString());
                    int objetivoViaje3 = Convert.ToInt32(datalistadoBusquedaActaGeneral.SelectedCells[27].Value.ToString());
                    int objetivoViaje4 = Convert.ToInt32(datalistadoBusquedaActaGeneral.SelectedCells[28].Value.ToString());

                    if (objetivoViaje1 == 1)
                    {
                        ckSostenimientoNuevaActa.Checked = true;
                    }
                    else
                    {
                        ckSostenimientoNuevaActa.Checked = false;
                    }

                    if (objetivoViaje2 == 1)
                    {
                        ckCaptacionNuevaActa.Checked = true;
                    }
                    else
                    {
                        ckCaptacionNuevaActa.Checked = false;
                    }

                    if (objetivoViaje3 == 1)
                    {
                        ckRecuperacionNuevaActa.Checked = true;
                    }
                    else
                    {
                        ckRecuperacionNuevaActa.Checked = false;
                    }

                    if (objetivoViaje4 == 1)
                    {
                        ckReclamoNuevaActa.Checked = true;
                    }
                    else
                    {
                        ckReclamoNuevaActa.Checked = false;
                    }

                    panelModificarActa.Visible = true;
                    datalistadoTodasActas.Enabled = false;

                    panelModificarActa.Visible = true;
                    datalistadoTodasActas.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un acta para poder editarla.", "Validación del Sistema");
            }
        }

        //BOTON PARA REGRESAR DE LA EDICION DE ACTA
        private void btnRegresarEdicionActa_Click(object sender, EventArgs e)
        {
            panelModificarActa.Visible = false;
            datalistadoTodasActas.Enabled = true;
        }

        //BOTON PARA CARGAR Y LIMPIAR LOS ASISTENTES
        private void btnCargarDatosAsistente2EdicionActa_Click(object sender, EventArgs e)
        {
            CargarResponsables(txtAsistentes2NuevaActa);
            txtAsistentes2NuevaActa.SelectedIndex = -1;
        }

        //BOTON PARA CARGAR Y LIMPIAR LOS ASISTENTES
        private void btnCargarDatosAsistente3EdicionActa_Click(object sender, EventArgs e)
        {
            CargarResponsables(txtAsistentes3NuevaActa);
            txtAsistentes3NuevaActa.SelectedIndex = -1;
        }

        //BOTON PARA RECARGAR A LOS CLIEENTES 1
        private void btnCargarDatosClietne1EdicionActa_Click(object sender, EventArgs e)
        {
            int idCLiente = Convert.ToInt32(datalistadoBusquedaActaGeneral.SelectedCells[30].Value.ToString());
            CargarContactoSegunCLiente(txtContactoCliente1NuevaActa, idCLiente, lblContactoTelefono1, lblClienteCargo1, lblContactoCorreo1);
            txtContactoCliente1NuevaActa.SelectedIndex = -1;
        }

        //BOTON PARA RECARGAR A LOS CLIEENTES 2
        private void btnCargarDatosClietne2EdicionActa_Click(object sender, EventArgs e)
        {
            int idCLiente = Convert.ToInt32(datalistadoBusquedaActaGeneral.SelectedCells[30].Value.ToString());
            CargarContactoSegunCLiente(txtContactoCliente2NuevaActa, idCLiente, lblContactoTelefono2, lblClienteCargo2, lblContactoCorreo2);
            txtContactoCliente2NuevaActa.SelectedIndex = -1;
        }

        //BOTON PARA RECARGAR A LOS CLIEENTES 3
        private void btnCargarDatosClietne3EdicionActa_Click(object sender, EventArgs e)
        {
            int idCLiente = Convert.ToInt32(datalistadoBusquedaActaGeneral.SelectedCells[30].Value.ToString());
            CargarContactoSegunCLiente(txtContactoCliente3NuevaActa, idCLiente, lblContactoTelefono3, lblClienteCargo3, lblContactoCorreo3);
            txtContactoCliente3NuevaActa.SelectedIndex = -1;
        }

        //BOTON PARA RECARGAR LOS DATOS DE CONTACTO DEL CLIENTE
        private void txtContactoCliente1NuevaActa_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT DACC.IdDatosAnexosClienteContacto, DACC.Descripcion, DACC.Telefono, C.Descripcion AS CARGO, DACC.Correo FROM DatosAnexosCliente_Contacto DACC INNER JOIN Cargo C on C.IdCargo = DACC.IdCargo WHERE IdDatosAnexosClienteContacto = @id ORDER BY  DACC.Descripcion", con);
                comando.Parameters.AddWithValue("@id", System.Convert.ToString(txtContactoCliente1NuevaActa.SelectedValue));
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lblContactoTelefono1.Text = System.Convert.ToString(row["Telefono"]);
                    lblClienteCargo1.Text = System.Convert.ToString(row["CARGO"]);
                    lblContactoCorreo1.Text = System.Convert.ToString(row["Correo"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de carga de datos, no se tiene un contacto registrado para este cliente, " + ex.Message, "Validación del Sistema");
            }
        }

        //BOTON PARA RECARGAR LOS DATOS DE CONTACTO DEL CLIENTE
        private void txtContactoCliente2NuevaActa_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT DACC.IdDatosAnexosClienteContacto, DACC.Descripcion, DACC.Telefono, C.Descripcion AS CARGO, DACC.Correo FROM DatosAnexosCliente_Contacto DACC INNER JOIN Cargo C on C.IdCargo = DACC.IdCargo WHERE IdDatosAnexosClienteContacto = @id ORDER BY  DACC.Descripcion", con);
                comando.Parameters.AddWithValue("@id", System.Convert.ToString(txtContactoCliente2NuevaActa.SelectedValue));
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lblContactoTelefono2.Text = System.Convert.ToString(row["Telefono"]);
                    lblClienteCargo2.Text = System.Convert.ToString(row["CARGO"]);
                    lblContactoCorreo2.Text = System.Convert.ToString(row["Correo"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de carga de datos, no se tiene un contacto registrado para este cliente, " + ex.Message, "Validación del Sistema");
            }
        }

        //BOTON PARA RECARGAR LOS DATOS DE CONTACTO DEL CLIENTE
        private void txtContactoCliente3NuevaActa_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand comando = new SqlCommand("SELECT DACC.IdDatosAnexosClienteContacto, DACC.Descripcion, DACC.Telefono, C.Descripcion AS CARGO, DACC.Correo FROM DatosAnexosCliente_Contacto DACC INNER JOIN Cargo C on C.IdCargo = DACC.IdCargo WHERE IdDatosAnexosClienteContacto = @id ORDER BY  DACC.Descripcion", con);
                comando.Parameters.AddWithValue("@id", System.Convert.ToString(txtContactoCliente3NuevaActa.SelectedValue));
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable dt = new DataTable();
                data.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lblContactoTelefono3.Text = System.Convert.ToString(row["Telefono"]);
                    lblClienteCargo3.Text = System.Convert.ToString(row["CARGO"]);
                    lblContactoCorreo3.Text = System.Convert.ToString(row["Correo"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de carga de datos, no se tiene un contacto registrado para este cliente, " + ex.Message, "Validación del Sistema");
            }
        }

        //ABRIR EL PANEL - VISUALIZADOR DE DETALLES 1
        private void btnVerDetalles1_Click(object sender, EventArgs e)
        {
            if (panelDetallesContacto1.Visible == true)
            {
                panelDetallesContacto1.Visible = false;
            }
            else
            {
                panelDetallesContacto1.Visible = true;
            }
        }

        //ABRIR EL PANEL - VISUALIZADOR DE DETALLES 2
        private void btnVerDetalles2_Click(object sender, EventArgs e)
        {
            if (panelDetallesContacto2.Visible == true)
            {
                panelDetallesContacto2.Visible = false;
            }
            else
            {
                panelDetallesContacto2.Visible = true;
            }
        }

        //ABRIR EL PANEL - VISUALIZADOR DE DETALLES 3
        private void btnVerDetalles3_Click(object sender, EventArgs e)
        {
            if (panelDetallesContacto3.Visible == true)
            {
                panelDetallesContacto3.Visible = false;
            }
            else
            {
                panelDetallesContacto3.Visible = true;
            }
        }

        //GUARDAR UNA NUEV ACTA
        private void btnGuardarNuevaActa_Click(object sender, EventArgs e)
        {
            if (rbTipoClienteActualNuevaActa.Checked == false && rbTipoClienteFuturoNuevaActa.Checked == false)
            {
                MessageBox.Show("Debe seleccionar un tipo de cliente.", "Validación del Sistema");
            }
            else
            {
                if (rbFrecuenciaAltaNuevaActa.Checked == false && rbFrecuenciaMediaNuevaActa.Checked == false && rbFrecuenduaBajaNuevaActa.Checked == false)
                {
                    MessageBox.Show("Debe seleccionar una frecuencia y volúmen de compra.", "Validación del Sistema");
                }
                else
                {
                    if (txtContactoCliente1NuevaActa.Text == "")
                    {
                        MessageBox.Show("Debe seleccionar al menos un contacto del cliente.", "Validación del Sistema");
                    }
                    else
                    {
                        try
                        {
                            DialogResult boton = MessageBox.Show("¿Realmente desea editar esta acta?.", "Validación del Sistema", MessageBoxButtons.OKCancel);
                            if (boton == DialogResult.OK)
                            {
                                SqlConnection con = new SqlConnection();
                                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                                con.Open();
                                SqlCommand cmd = new SqlCommand();
                                cmd = new SqlCommand("ModificarActa", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@idActa", datalistadoTodasActas.SelectedCells[1].Value.ToString());
                                cmd.Parameters.AddWithValue("@fechaInicio", datatimeFechaInicioNuevaActa.Value);
                                cmd.Parameters.AddWithValue("@fechaTermino", datetimeFechaTerminoNuevaActa.Value);

                                if (rbTipoClienteActualNuevaActa.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@ckActual", 1);
                                    cmd.Parameters.AddWithValue("@ckFuturoPotencial", 0);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ckActual", 0);
                                    cmd.Parameters.AddWithValue("@ckFuturoPotencial", 1);
                                }

                                if (rbFrecuenciaAltaNuevaActa.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@ckAlto", 1);
                                    cmd.Parameters.AddWithValue("@ckMedia", 0);
                                    cmd.Parameters.AddWithValue("@ckBaja", 0);
                                }
                                else if (rbFrecuenciaMediaNuevaActa.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@ckAlto", 0);
                                    cmd.Parameters.AddWithValue("@ckMedia", 1);
                                    cmd.Parameters.AddWithValue("@ckBaja", 0);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ckAlto", 0);
                                    cmd.Parameters.AddWithValue("@ckMedia", 0);
                                    cmd.Parameters.AddWithValue("@ckBaja", 1);
                                }

                                cmd.Parameters.AddWithValue("@asistente1", txtAsistentes1NuevaActa.Text);
                                cmd.Parameters.AddWithValue("@asistente2", txtAsistentes2NuevaActa.Text);
                                cmd.Parameters.AddWithValue("@asistente3", txtAsistentes3NuevaActa.Text);

                                if (txtContactoCliente1NuevaActa.Text == "")
                                {
                                    cmd.Parameters.AddWithValue("@ContactoCliente1", "");
                                    cmd.Parameters.AddWithValue("@correocliente1", "");
                                    cmd.Parameters.AddWithValue("@cargocliente1", "");
                                    cmd.Parameters.AddWithValue("@telefonocliente1", "");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ContactoCliente1", txtContactoCliente1NuevaActa.Text);
                                    cmd.Parameters.AddWithValue("@correocliente1", lblContactoCorreo1.Text);
                                    cmd.Parameters.AddWithValue("@cargocliente1", lblClienteCargo1.Text);
                                    cmd.Parameters.AddWithValue("@telefonocliente1", lblContactoTelefono1.Text);
                                }

                                if (txtContactoCliente2NuevaActa.Text == "")
                                {
                                    cmd.Parameters.AddWithValue("@ContactoCliente2", "");
                                    cmd.Parameters.AddWithValue("@correocliente2", "");
                                    cmd.Parameters.AddWithValue("@cargocliente2", "");
                                    cmd.Parameters.AddWithValue("@telefonocliente2", "");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ContactoCliente2", txtContactoCliente2NuevaActa.Text);
                                    cmd.Parameters.AddWithValue("@correocliente2", lblContactoCorreo2.Text);
                                    cmd.Parameters.AddWithValue("@cargocliente2", lblClienteCargo2.Text);
                                    cmd.Parameters.AddWithValue("@telefonocliente2", lblContactoTelefono2.Text);
                                }

                                if (txtContactoCliente3NuevaActa.Text == "")
                                {
                                    cmd.Parameters.AddWithValue("@ContactoCliente3", "");
                                    cmd.Parameters.AddWithValue("@correocliente3", "");
                                    cmd.Parameters.AddWithValue("@cargocliente3", "");
                                    cmd.Parameters.AddWithValue("@telefonocliente3", "");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ContactoCliente3", txtContactoCliente3NuevaActa.Text);
                                    cmd.Parameters.AddWithValue("@correocliente3", lblContactoCorreo3.Text);
                                    cmd.Parameters.AddWithValue("@cargocliente3", lblClienteCargo3.Text);
                                    cmd.Parameters.AddWithValue("@telefonocliente3", lblContactoTelefono3.Text);
                                }

                                if (ckSostenimientoNuevaActa.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@ckSostenimiento", 1);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ckSostenimiento", 0);
                                }

                                if (ckCaptacionNuevaActa.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@ckCapacitacion", 1);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ckCapacitacion", 0);
                                }

                                if (ckRecuperacionNuevaActa.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@ckRecuperacion", 1);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ckRecuperacion", 0);
                                }

                                if (ckReclamoNuevaActa.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@ckReclamo", 1);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@ckReclamo", 0);
                                }

                                cmd.Parameters.AddWithValue("@fechaActa", datetimeActa.Value);

                                if (ckPresenteAsistente1.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@presenciaAsistente1Encargado", 1);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@presenciaAsistente1Encargado", 0);
                                }

                                cmd.ExecuteNonQuery();
                                con.Close();

                                MessageBox.Show("Se editó el acta correctamente en el sistema.", "Validación del Sistema");

                                //INGRESO DE LA TABLA AUDITORA
                                con.Open();
                                cmd = new SqlCommand("InsertarDatosTablaAuditora_Comercial", con);
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@idUsuario", Program.IdUsuario);
                                cmd.Parameters.AddWithValue("@mantenimiento", "Área comercial - Menú Actas - Actas de Visita");
                                cmd.Parameters.AddWithValue("@accion", "Modificación de acta número " + datalistadoTodasActas.SelectedCells[1].Value.ToString());
                                cmd.Parameters.AddWithValue("@descripcion", "Acta editada por el usuario " + Program.UnoNombreUnoApellidoUsuario + " en la fecha " + DateTime.Now);
                                cmd.Parameters.AddWithValue("@maquina", Environment.MachineName);
                                cmd.Parameters.AddWithValue("@fechaAccion", DateTime.Now);
                                cmd.Parameters.AddWithValue("@nameUsuarioSesion", Environment.UserName);
                                cmd.Parameters.AddWithValue("@codigoRequerimiento", Convert.ToInt32(datalistadoTodasActas.SelectedCells[15].Value.ToString()));
                                cmd.Parameters.AddWithValue("@codigoLiquidacion", Convert.ToInt32(datalistadoTodasActas.SelectedCells[14].Value.ToString()));
                                cmd.Parameters.AddWithValue("@codigoActa", datalistadoTodasActas.SelectedCells[1].Value.ToString());
                                cmd.Parameters.AddWithValue("@codigoLineaTrabajo", DBNull.Value);
                                cmd.ExecuteNonQuery();
                                con.Close();

                                panelModificarActa.Visible = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error en el servidor.");
                        }
                    }
                }
            }
        }

        //BOTON PARA VISUALIZAR LAS ACTAS
        private void btnVisualizarActa_Click(object sender, EventArgs e)
        {
            if (datalistadoTodasActas.CurrentRow != null)
            {
                if (datalistadoTodasActas.SelectedCells[13].Value.ToString() == "PENDIENTE")
                {
                    string codigoActaReporte = datalistadoTodasActas.Rows[datalistadoTodasActas.CurrentRow.Index].Cells[1].Value.ToString();
                    Visualizadores.VisualizarActa frm = new Visualizadores.VisualizarActa();
                    frm.lblCodigo.Text = codigoActaReporte;

                    frm.Show();
                }
                else if (datalistadoTodasActas.SelectedCells[13].Value.ToString() == "APROBADO")
                {
                    string codigoCotizacionReporte = datalistadoTodasActas.Rows[datalistadoTodasActas.CurrentRow.Index].Cells[1].Value.ToString();
                    Visualizadores.VisualizarActaAprobada frm = new Visualizadores.VisualizarActaAprobada();
                    frm.lblCodigo.Text = codigoCotizacionReporte;

                    frm.Show();
                }
                else if (datalistadoTodasActas.SelectedCells[13].Value.ToString() == "CULMINADO")
                {
                    string codigoCotizacionReporte = datalistadoTodasActas.Rows[datalistadoTodasActas.CurrentRow.Index].Cells[1].Value.ToString();
                    Visualizadores.VisualizarActa frm = new Visualizadores.VisualizarActa();
                    frm.lblCodigo.Text = codigoCotizacionReporte;

                    frm.Show();
                }
                else
                {
                    string codigoActaReporte = datalistadoTodasActas.Rows[datalistadoTodasActas.CurrentRow.Index].Cells[1].Value.ToString();
                    Visualizadores.VisualizarActaDesaprobada frm = new Visualizadores.VisualizarActaDesaprobada();
                    frm.lblCodigo.Text = codigoActaReporte;

                    frm.Show();
                }

            }
            else
            {
                MessageBox.Show("Debe seleccionar un acta para poder generar el PDF.", "Validación del Sistema");
            }
        }

        //VISUALIZAR EL REPORTE EN PDF DE MI ACTA
        private void datalistadoTodasActas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn currentColumn = datalistadoTodasActas.Columns[e.ColumnIndex];

            if (currentColumn.Name == "btnGenerarActa")
            {
                if (datalistadoTodasActas.CurrentRow != null)
                {
                    if (datalistadoTodasActas.SelectedCells[13].Value.ToString() == "PENDIENTE")
                    {
                        string codigoActaReporte = datalistadoTodasActas.Rows[datalistadoTodasActas.CurrentRow.Index].Cells[1].Value.ToString();
                        Visualizadores.VisualizarActa frm = new Visualizadores.VisualizarActa();
                        frm.lblCodigo.Text = codigoActaReporte;

                        frm.Show();
                    }
                    else if (datalistadoTodasActas.SelectedCells[13].Value.ToString() == "APROBADO")
                    {
                        string codigoCotizacionReporte = datalistadoTodasActas.Rows[datalistadoTodasActas.CurrentRow.Index].Cells[1].Value.ToString();
                        Visualizadores.VisualizarActaAprobada frm = new Visualizadores.VisualizarActaAprobada();
                        frm.lblCodigo.Text = codigoCotizacionReporte;

                        frm.Show();
                    }
                    else if (datalistadoTodasActas.SelectedCells[13].Value.ToString() == "CULMINADO")
                    {
                        string codigoCotizacionReporte = datalistadoTodasActas.Rows[datalistadoTodasActas.CurrentRow.Index].Cells[1].Value.ToString();
                        Visualizadores.VisualizarActa frm = new Visualizadores.VisualizarActa();
                        frm.lblCodigo.Text = codigoCotizacionReporte;

                        frm.Show();
                    }
                    else
                    {
                        string codigoActaReporte = datalistadoTodasActas.Rows[datalistadoTodasActas.CurrentRow.Index].Cells[1].Value.ToString();
                        Visualizadores.VisualizarActaDesaprobada frm = new Visualizadores.VisualizarActaDesaprobada();
                        frm.lblCodigo.Text = codigoActaReporte;

                        frm.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar una acta para poder generar el PDF.", "Validación del Sistema");
                }
            }

            if (currentColumn.Name == "VALIDAR")
            {
                if (txtEstadoValidado.Text == "true")
                {
                    txtEstadoValidado.Text = "false";
                }
                else
                {
                    txtEstadoValidado.Text = "true";
                }
            }
        }
        //--------------------------------------------------------------------------------------

        //SALIR DE LINEA DE TRABAJO
        private void btnRegresarLineaTrabajo_Click(object sender, EventArgs e)
        {
            panelNuevaActa.Visible = false;
        }

        //BOTON PARA ABRIR LA IMAGEN ADJUNTADA
        private void btnVisualizarImagen1_Click(object sender, EventArgs e)
        {
            if (txtImagen1.Text != "")
            {
                string ruta = txtImagen1.Text;

                Process.Start(ruta);
            }
            else
            {
                MessageBox.Show("No hay imagen cargada.", "Abrir Imagen");
            }
        }

        //BOTON PARA ABRIR LA IMAGEN ADJUNTADA
        private void btnVisualizarImagen2_Click(object sender, EventArgs e)
        {
            if (txtImagen2.Text != "")
            {
                string ruta = txtImagen2.Text;

                Process.Start(ruta);
            }
            else
            {
                MessageBox.Show("No hay imagen cargada.", "Abrir Imagen");
            }
        }

        //BOTON PARA ABRIR LA IMAGEN ADJUNTADA
        private void btnVisualizarImagen3_Click(object sender, EventArgs e)
        {
            if (txtImagen3.Text != "")
            {
                string ruta = txtImagen3.Text;

                Process.Start(ruta);
            }
            else
            {
                MessageBox.Show("No hay imagen cargada.", "Abrir Imagen");
            }
        }

        //LIMPIEZA DE CAMPOS -------------------------------------------------------------------------
        //LIMPIEZA DE CAMPO IMAGEN 1
        private void btnBorrarImagen1_Click(object sender, EventArgs e)
        {
            txtImagen1.Text = "";
        }

        //LIMPIEZA DE CAMPO IMAGEN 2
        private void btnBorrarImagen2_Click(object sender, EventArgs e)
        {
            txtImagen2.Text = "";
        }

        //LIMPIEZA DE CAMPO IMAGEN 3
        private void btnBorrarImagen3_Click(object sender, EventArgs e)
        {
            txtImagen3.Text = "";
        }

        //BOTON PARA ABRIR EL MAUAL DE SUAURIO
        private void btnInfo_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //BOTON PARA ABRIR EL MAUAL DE SUAURIO
        private void btnInfoDetalleActa_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //BOTON PARA ABRIR EL MAUAL DE SUAURIO
        private void btnInfoDetalle_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //BOTON PARA ABRIR EL MAUAL DE SUAURIO
        private void btnInfoActa_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }
    }
}

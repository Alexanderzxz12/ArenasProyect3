using DocumentFormat.OpenXml.InkML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Resourses
{
    public class CreacionNombreProducto
    {
        //METODO QUE CARGA LOS GRUPOS QUE TIENEN INGRESADO UNA DEFINICIÓN DE POSICION (ESTO SE DEFINE EN MODELOS)
        public static void CargarGrupoCamposXPosicion(int idmodelo,DataGridView DGVGrupoCamposPos)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection();
                con.ConnectionString = Conexion.ConexionMaestra.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand("AgregarProducto_CargarGrupoCamposXPosicion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idmodelo", idmodelo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("El modelo seleccionado no tiene posiciones definidas", "Validación del Sistema", MessageBoxButtons.OK);
                    return;
                }
                DGVGrupoCamposPos.DataSource = dt;
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
          
        }

        //ORDENAMIENTO DE LOS TXTS TAGEADOS(PERTENECEN) AL PANEL QUE CORRESPONDEN
        public static void ReordenarTxT_GrupoCamposXPosicion(DataGridView DGVGrupoCamposPos,FlowLayoutPanel contenedortxt,TextBox[] listatxt)
        {
            try
            {
                if(DGVGrupoCamposPos.Rows.Count != 0)
                {
                    contenedortxt.Controls.Clear();

                    DataGridViewRow filas = DGVGrupoCamposPos.Rows[0];

                    for (int i = 0; i <= 15; i++)
                    {
                        string nombreposicion = "Posicion" + i;

                        object nombregrupo = filas.Cells[nombreposicion].Value;

                        string nombrepanel = nombregrupo.ToString();

                        foreach (TextBox txt in listatxt)
                        {
                            if (txt.Tag != null && txt.Tag.ToString() == nombrepanel)
                            {
                                contenedortxt.Controls.Add(txt);
                            }
                        }
                    }
                }
                else
                {
                    return;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message);
            }
        }

        //CONSTRUCCIÓN DEL NOMBRE DEL PRODUCTO POR MEDIO DEL ORDEN DE LOS TXTS ALMACENADOS EN EL FLOWLAYOUTPANEL
        public static void ConstruirNombreProducto(ComboBox modeloSeleccionado,FlowLayoutPanel contenedortxt,TextBox nombreProductoDefinido)
        {
            try
            {
                if (modeloSeleccionado.SelectedValue == null)
                {
                    return;
                }
                else
                {
                    StringBuilder nombreproducto = new StringBuilder();

                    foreach (TextBox txt in contenedortxt.Controls.OfType<TextBox>())
                    {
                        if (!string.IsNullOrWhiteSpace(txt.Text))
                        {
                            nombreproducto.Append(txt.Text + " ");
                        }
                    }
                    nombreProductoDefinido.Text = modeloSeleccionado.Text + " " + nombreproducto.ToString().Trim();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        //METODO DE SUSCRIPCION DE EVENTOS AL CONTROL TXT QUE ALMACENARA EL NOMBRE DEL PRODUCTO GENERADO (txtDescripcionGeneradaProducto)
        public static void SucribirEventosTextChanged(FlowLayoutPanel contenedortxt,EventHandler eventos)
        {
            foreach(TextBox txt in contenedortxt.Controls.OfType<TextBox>())
            {
                txt.TextChanged -= eventos;
                txt.TextChanged += eventos;
            }
        }
    }
}

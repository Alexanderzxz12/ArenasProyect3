using ArenasProyect3.Modulos.Resourses;
using ArenasProyect3.Reportes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3.Visualizadores
{
    public partial class VisualizarRequerimientoAprobado : Form
    {
   
        //CONSTRUCTOR DEL MANTENIMIENTO - VISUALIZAR REQUERIMEINTO
        public VisualizarRequerimientoAprobado(int codigo)
        {
            InitializeComponent();

            ClassResourses.CargarReportes(CrvVisualizarRequerimientoAprobado, new InformeRequerimientoVentaAprobado(), "@idRequerimiento", codigo);       
        }

        //INICIO Y CARGA INICIAL DEL FORMULARIO
        private void VisualizarRequerimientoAprobado_Load(object sender, EventArgs e)
        {
            //int codigo = Convert.ToInt32(lblCodigo.Text);

            //InformeRequerimientoVentaAprobado reporteD = new InformeRequerimientoVentaAprobado();

            ////LLAMADO A LA CLASE INVOCANDO AL METODO QUE APLICA LA CONEXION A LOS REPORTES
            //ClassResourses.AplicarConexionReportes(reporteD);

            ////reporteD.DataSourceConnections[0].SetLogon("sa", "Arenas.2020!");
            //reporteD.SetParameterValue("@idRequerimiento", _codigo);
            //CrvVisualizarRequerimientoAprobado.ReportSource = reporteD;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArenasProyect3.Modulos.Resourses;
using ArenasProyect3.Reportes;

namespace ArenasProyect3.Visualizadores
{
    public partial class VisualizarRequerimientoDesaprobado : Form
    {

        public VisualizarRequerimientoDesaprobado(int codigo)
        {
            InitializeComponent();         
            ClassResourses.CargarReportes(CrvVisualizarRequerimientoDesaprobado, new InformeRequerimientoVentaAnulada(), "@idRequerimiento",codigo);
        }

        private void VisualizarRequerimientoDesaprobado_Load(object sender, EventArgs e)
        {
            //int codigo = Convert.ToInt32(lblCodigo.Text);

            //InformeRequerimientoVentaAnulada reporteD = new InformeRequerimientoVentaAnulada();

            ////LLAMADO A LA CLASE INVOCANDO AL METODO QUE APLICA LA CONEXION A LOS REPORTES
            //ClassResourses.AplicarConexionReportes(reporteD);

            ////reporteD.DataSourceConnections[0].SetLogon("sa", "Arenas.2020!");
            //reporteD.SetParameterValue("@idRequerimiento", _codigo);
            //CrvVisualizarRequerimientoDesaprobado.ReportSource = reporteD;
        }
    }
}

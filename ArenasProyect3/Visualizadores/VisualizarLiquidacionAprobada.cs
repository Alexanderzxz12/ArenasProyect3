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
    public partial class VisualizarLiquidacionAprobada : Form
    {
        public VisualizarLiquidacionAprobada(int codigo)
        {
            InitializeComponent();         
            ClassResourses.CargarReportes(CrvVisualizarLiquidacionVentaAprobada,new InformeLiquidacionVentaAprobada(), "@idLiquidacion",codigo);
        }
        
        private void VisualizarLiquidacionAprobada_Load(object sender, EventArgs e)
        {
            //CargarReporte();
            //int codigo = Convert.ToInt32(lblCodigo.Text);

            //InformeLiquidacionVentaAprobada reporteD = new InformeLiquidacionVentaAprobada();

            ////LLAMADO A LA CLASE INVOCANDO AL METODO QUE APLICA LA CONEXION A LOS REPORTES
            //ClassResourses.AplicarConexionReportes(reporteD);

            ////reporteD.DataSourceConnections[0].SetLogon("sa", "Arenas.2020!");

            //reporteD.SetParameterValue("@idLiquidacion", _codigo);
            //CrvVisualizarLiquidacionVentaAprobada.ReportSource = reporteD;
        }
    }
}

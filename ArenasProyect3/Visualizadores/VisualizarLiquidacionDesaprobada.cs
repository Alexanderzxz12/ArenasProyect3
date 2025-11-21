using ArenasProyect3.Modulos.Resourses;
using ArenasProyect3.Reportes;
using CrystalDecisions.Windows.Forms;
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
    public partial class VisualizarLiquidacionDesaprobada : Form
    {
        public VisualizarLiquidacionDesaprobada(int codigo)
        {
            InitializeComponent();       
            ClassResourses.CargarReportes(CrvVisualizarLiquidacionVentaDesaprobada,new InformeLiquidacionVentaAnulada(), "@idLiquidacion",codigo);
        }     

        private void VisualizarLiquidacionDesaprobada_Load(object sender, EventArgs e)
        {
            //int codigo = Convert.ToInt32(lblCodigo.Text);

            //InformeLiquidacionVentaAnulada reporteD = new InformeLiquidacionVentaAnulada();

            ////LLAMADO A LA CLASE INVOCANDO AL METODO QUE APLICA LA CONEXION A LOS REPORTES
            //ClassResourses.AplicarConexionReportes(reporteD);

            ////reporteD.DataSourceConnections[0].SetLogon("sa", "Arenas.2020!");

            //reporteD.SetParameterValue("@idLiquidacion", _codigo);
            //CrvVisualizarLiquidacionVentaDesaprobada.ReportSource = reporteD;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArenasProyect3.Reportes;

namespace ArenasProyect3.Visualizadores
{
    public partial class VisualizarLiquidacionAprobada : Form
    {
        public VisualizarLiquidacionAprobada()
        {
            InitializeComponent();
        }

        private void VisualizarLiquidacionAprobada_Load(object sender, EventArgs e)
        {
            int codigo = Convert.ToInt32(lblCodigo.Text);

            InformeLiquidacionVentaAprobada reporteD = new InformeLiquidacionVentaAprobada();
            reporteD.DataSourceConnections[0].SetLogon("sa", "Arenas.2020!");
            reporteD.SetParameterValue("@idLiquidacion", codigo);
            CrvVisualizarLiquidacionVentaAprobada.ReportSource = reporteD;
        }
    }
}

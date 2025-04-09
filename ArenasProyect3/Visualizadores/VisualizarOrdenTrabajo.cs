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
    public partial class VisualizarOrdenTrabajo : Form
    {
        public VisualizarOrdenTrabajo()
        {
            InitializeComponent();
        }

        private void VisualizarOrdenTrabajo_Load(object sender, EventArgs e)
        {
            int codigo = Convert.ToInt32(lblCodigo.Text);

            InformeOrdenTrabajo reporteD = new InformeOrdenTrabajo();
            reporteD.DataSourceConnections[0].SetLogon("sa", "Arenas.2020!");
            reporteD.SetParameterValue("@idOrdenServicio", codigo);
            CrvVisualizarOrdenProduccion.ReportSource = reporteD;
        }
    }
}

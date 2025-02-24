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
    public partial class VisualizarOrdenProduccion : Form
    {
        public VisualizarOrdenProduccion()
        {
            InitializeComponent();
        }

        private void VisualizarOrdenProduccion_Load(object sender, EventArgs e)
        {
            int codigo = Convert.ToInt32(lblCodigo.Text);

            InformeOrdenProduccion reporteD = new InformeOrdenProduccion();
            reporteD.DataSourceConnections[0].SetLogon("sa", "Arenas.2020!");
            reporteD.SetParameterValue("@idOrdenProduccion", codigo);
            CrvVisualizarOrdenProduccion.ReportSource = reporteD;
        }
    }
}

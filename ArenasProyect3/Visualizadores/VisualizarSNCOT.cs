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
    public partial class VisualizarSNCOT : Form
    {
        public VisualizarSNCOT()
        {
            InitializeComponent();
        }

        private void VisualizarSNCOT_Load(object sender, EventArgs e)
        {
            int codigo = Convert.ToInt32(lblCodigo.Text);

            InformeSNCOT reporteD = new InformeSNCOT();
            reporteD.DataSourceConnections[0].SetLogon("sa", "Arenas.2020!");
            reporteD.SetParameterValue("@idDetalleCantidadCalidadOT", codigo);
            CrvVisualizarActaVisita.ReportSource = reporteD;
        }
    }
}

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
    public partial class VisualizarSNC : Form
    {
        public VisualizarSNC()
        {
            InitializeComponent();
        }

        private void VisualizarSNC_Load(object sender, EventArgs e)
        {
            int codigo = Convert.ToInt32(lblCodigo.Text);

            InformeSNC reporteD = new InformeSNC();
            reporteD.DataSourceConnections[0].SetLogon("sa", "Arenas.2020!");
            reporteD.SetParameterValue("@idDetalleCantidadCalidad", codigo);
            CrvVisualizarActaVisita.ReportSource = reporteD;
        }
    }
}

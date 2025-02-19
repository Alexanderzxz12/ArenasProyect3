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
    public partial class VisualizarActaDesaprobada : Form
    {
        public VisualizarActaDesaprobada()
        {
            InitializeComponent();
        }

        private void VisualizarActaDesaprobada_Load(object sender, EventArgs e)
        {
            int codigo = Convert.ToInt32(lblCodigo.Text);

            InformeActaDesaprobada reporteD = new InformeActaDesaprobada();
            reporteD.DataSourceConnections[0].SetLogon("sa", "Arenas.2020!");
            reporteD.SetParameterValue("@idActa", codigo);
            CrvVisualizarActaVisitaDesaprobada.ReportSource = reporteD;
        }
    }
}

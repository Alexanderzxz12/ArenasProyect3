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
    public partial class VisualizarRequerimientoDesaprobado : Form
    {
        public VisualizarRequerimientoDesaprobado()
        {
            InitializeComponent();
        }

        private void VisualizarRequerimientoDesaprobado_Load(object sender, EventArgs e)
        {
            int codigo = Convert.ToInt32(lblCodigo.Text);

            InformeRequerimientoVentaAnulada reporteD = new InformeRequerimientoVentaAnulada();
            reporteD.DataSourceConnections[0].SetLogon("sa", "Arenas.2020!");
            reporteD.SetParameterValue("@idRequerimiento", codigo);
            CrvVisualizarRequerimientoDesaprobado.ReportSource = reporteD;
        }
    }
}

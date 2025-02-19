using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArenasProyect3.Reportes;
using CrystalDecisions.Shared;

namespace ArenasProyect3.Visualizadores
{
    public partial class VisualizarRequerimientoVenta : Form
    {

        public VisualizarRequerimientoVenta()
        {
            InitializeComponent();
        }

        private void VisualizarRequerimientoVenta_Load(object sender, EventArgs e)
        {
            int codigo = Convert.ToInt32(lblCodigo.Text);

            InformeRequerimientoVenta reporteD = new InformeRequerimientoVenta();
            reporteD.DataSourceConnections[0].SetLogon("sa", "Arenas.2020!");
            reporteD.SetParameterValue("@idRequerimiento", codigo);
            CrvVisualizarRequerimientoVenta.ReportSource = reporteD;
        }
    }
}

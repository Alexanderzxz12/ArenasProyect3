using ArenasProyect3.Reportes;
using CrystalDecisions.Shared;
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
    public partial class VisualizarCotizacionVenta : Form
    {
        public VisualizarCotizacionVenta()
        {
            InitializeComponent();
        }

        private void VisualizarCotizacionVenta_Load(object sender, EventArgs e)
        {
            int codigo = Convert.ToInt32(lblCodigo.Text);

            InformeCotizacionVenta reporteD = new InformeCotizacionVenta();
            reporteD.DataSourceConnections[0].SetLogon("sa", "Arenas.2020!");
            reporteD.SetParameterValue("@idCotizacion", codigo);
            CrvVisualizarActaVisita.ReportSource = reporteD;

            string rutaReporte = @"C:\ArenasSoftBrochure\Cotizacion.pdf";
            reporteD.ExportToDisk(ExportFormatType.PortableDocFormat, rutaReporte);
        }
    }
}

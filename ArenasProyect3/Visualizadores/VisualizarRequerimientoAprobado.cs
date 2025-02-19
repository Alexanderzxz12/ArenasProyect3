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
    public partial class VisualizarRequerimientoAprobado : Form
    {
        //CONSTRUCTOR DEL MANTENIMIENTO - VISUALIZAR REQUERIMEINTO
        public VisualizarRequerimientoAprobado()
        {
            InitializeComponent();
        }

        //INICIO Y CARGA INICIAL DEL FORMULARIO
        private void VisualizarRequerimientoAprobado_Load(object sender, EventArgs e)
        {
            int codigo = Convert.ToInt32(lblCodigo.Text);

            InformeRequerimientoVentaAprobado reporteD = new InformeRequerimientoVentaAprobado();
            reporteD.DataSourceConnections[0].SetLogon("sa", "Arenas.2020!");
            reporteD.SetParameterValue("@idRequerimiento", codigo);
            CrvVisualizarRequerimientoAprobado.ReportSource = reporteD;
        }
    }
}

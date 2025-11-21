using ArenasProyect3.Modulos.Resourses;
using ArenasProyect3.Reportes;
using CrystalDecisions.Shared;
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

namespace ArenasProyect3.Visualizadores
{
    public partial class VisualizarRequerimientoVenta : Form
    {

        public VisualizarRequerimientoVenta(int codigo)
        {
            InitializeComponent();
        
            ClassResourses.CargarReportes(CrvVisualizarRequerimientoVenta,new InformeRequerimientoVenta(), "@idRequerimiento",codigo);
        }  

        private void VisualizarRequerimientoVenta_Load(object sender, EventArgs e)
        {
            //int codigo = Convert.ToInt32(lblCodigo.Text);

            //InformeRequerimientoVenta reporteD = new InformeRequerimientoVenta();

            ////LLAMADO A LA CLASE INVOCANDO AL METODO QUE APLICA LA CONEXION A LOS REPORTES
            //ClassResourses.AplicarConexionReportes(reporteD);

            ////reporteD.DataSourceConnections[0].SetLogon("sa", "Arenas.2020!");
            //reporteD.SetParameterValue("@idRequerimiento", _codigo);
            //CrvVisualizarRequerimientoVenta.ReportSource = reporteD;
        }
    }
}

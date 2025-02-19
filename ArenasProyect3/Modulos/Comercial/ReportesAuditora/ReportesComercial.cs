using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Comercial
{
    public partial class ReportesComercial : Form
    {
        //CONSTRUCTOR DEL MANTENIMIENTO - MANTENIEMINTO
        public ReportesComercial()
        {
            InitializeComponent();
        }

        //PRIMERA CARGA DE MI MANTENIMIENTOS
        private void ReportesComercial_Load(object sender, EventArgs e)
        {

        }

        //REPORTES DE REQEURIEMITNOS---------------------------------------------------------------
        //HABILITAR REQUERIEMINTOS
        private void btnReportesRequerimeinto_Click(object sender, EventArgs e)
        {
            panelReportesRequerimiento.Visible = true;
        }


    }
}

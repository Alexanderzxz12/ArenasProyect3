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
    public partial class MenuReportes : Form
    {
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO

        //CONSTRUCTOR DEL MANTENIMIENTO - MENU
        public MenuReportes()
        {
            InitializeComponent();
        }

        //FUNCION PARA ABRIR FORMULARIOS
        public void AbrirMantenimiento(object frmMantenimientos)
        {
            Form frm = frmMantenimientos as Form;
            frm.TopLevel = false;
            frm.Dock = DockStyle.Fill;
            this.panelMantenimientos.Controls.Add(frm);
            this.panelMantenimientos.Tag = frm;
            frm.Show();
        }

        //EVENTO DE INICIO Y DE CARGA DEL MENÚ 
        private void MenuReportes_Load(object sender, EventArgs e)
        {
            //
        }

        //ABRIR REPORTES DEL ÁREA COMERCIAL
        private void btnReportes_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ReportesComercial());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ReportesComercial());
            }
        }

        //ABRIR EL MANTENIMINETO DE AUDITOR
        private void btnAuditor_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new Auditora.Auditora());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new Auditora.Auditora());
            }

        }
    }
}

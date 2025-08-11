using ArenasProyect3.Modulos.ManGeneral;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Mantenimientos
{
    public partial class MenuRequerimientoSimple : Form
    {
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        string ruta = ManGeneral.Manual.manualAreaLogistica;

        //CONSTRUCTOR DEL MANTENIMIENTO - MENU DE REQUERIMIENTOS SIMPLE
        public MenuRequerimientoSimple()
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
        private void MenuRequerimientoSimple_Load(object sender, EventArgs e)
        {
            // 
        }

        //ABRIR NNUEVO REQUERIMEINTO
        private void btnRequerimientossSimple_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new RequerimientoSimple());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new RequerimientoSimple());
            }
        }

        //ABRIR REQUERIMIENTOS SIMPLES
        private void btnListadoRequerimientoSimple_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ListadoRequerimientoSimple());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ListadoRequerimientoSimple());
            }
        }

        //ABRIR VALIDACION POR JEFATURA
        private void btnValidacionJeftura_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ValidacionRequerimientoSimple());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ValidacionRequerimientoSimple());
            }
        }

        //ABRIR MANUAL DE USUSARIO
        private void btnManualUsuario_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }
    }
}

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

namespace ArenasProyect3.Modulos.Calidad.Revision
{
    public partial class MenuConsultasOP : Form
    {
        //VARIABLES GLOBALES
        string ruta = Manual.manualAreaProduccion;

        //CONSTRUCTOR DEL MANTENIMIENTO - MENU CONSULTAS DE OP
        public MenuConsultasOP()
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
        private void MenuConsultasOP_Load(object sender, EventArgs e)
        {
            //
        }

        //ABRIR LISTADO DE ORDENES DE PRODUCCION
        private void btnListadoOP_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ListadoOrdenProduccion());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ListadoOrdenProduccion());
            }
        }

        //ABIRIR EL MANUAL DE USUARIO
        private void btnManualUsuario_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }
    }
}

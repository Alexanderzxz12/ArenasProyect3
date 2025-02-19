using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Procesos.Fornulacion
{
    public partial class MenuFormulacion : Form
    {
        //CONSTRUCTOR DEL MANTENIMIENTO - MENU DE ACTIVIDADES PRINCIPALES
        public MenuFormulacion()
        {
            InitializeComponent();
        }

        //FUNCION PARA ABRIR FORMULARIOS
        public void AbrirMantenimiento(object frmMantenimientos)
        {
            Form frm = frmMantenimientos as Form;
            frm.TopLevel = false;
            frm.Dock = DockStyle.Fill;
            this.panelMantenimientosAPrincipales.Controls.Add(frm);
            this.panelMantenimientosAPrincipales.Tag = frm;
            frm.Show();
        }

        //EVENTO DE INICIO Y DE CARGA DEL MENÚ 
        private void MenuFormulacion_Load(object sender, EventArgs e)
        {
            //
        }

        //ABRIR MANTENIMIENTO DE DEFINICIONES
        private void btnDefinicionFormulacion_Click(object sender, EventArgs e)
        {
            if (panelMantenimientosAPrincipales.Controls.Count == 1)
            {
                panelMantenimientosAPrincipales.Controls.Clear();
                AbrirMantenimiento(new DefinicionFormulacion());
            }
            else
            {
                panelMantenimientosAPrincipales.Controls.Clear();
                AbrirMantenimiento(new DefinicionFormulacion());
            }
        }

        //ABRIR MANTENIMIENTO DE FORMULACIONES
        private void btnCreacionFormulacion_Click(object sender, EventArgs e)
        {
            if (panelMantenimientosAPrincipales.Controls.Count == 1)
            {
                panelMantenimientosAPrincipales.Controls.Clear();
                AbrirMantenimiento(new CreacionFormulacion());
            }
            else
            {
                panelMantenimientosAPrincipales.Controls.Clear();
                AbrirMantenimiento(new CreacionFormulacion());
            }
        }
    }
}

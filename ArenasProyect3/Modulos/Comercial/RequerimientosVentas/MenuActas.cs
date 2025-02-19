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
using ArenasProyect3.Modulos.ManGeneral;

namespace ArenasProyect3.Modulos.Comercial.RequerimientosVentas
{
    public partial class MenuActas : Form
    {
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        string ruta = Manual.manualComercial;

        //CONSTRUCTOR DEL MANTENIMIENTO - MENU DE REQUERIMIENTOS Y LIQUIDACIONES
        public MenuActas()
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
        private void MenuActas_Load(object sender, EventArgs e)
        {
            //
        }

        //ABRIR ACTAS DE VISITA
        private void btnActasVisitas_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ActasVisita());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ActasVisita());
            }
        }

        //ABRIR LSITADO DE ACTAS DE VISITA
        private void btnListadoActas_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ListadoActas());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ListadoActas());
            }
        }

        //ABIRIR EL MANUAL DE USUARIO
        private void btnManualUsuario_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }
    }
}

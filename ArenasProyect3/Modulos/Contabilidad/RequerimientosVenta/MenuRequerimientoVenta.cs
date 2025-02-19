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

namespace ArenasProyect3.Modulos.Contabilidad.RequerimientosVenta
{
    public partial class MenuRequerimientoVenta : Form
    {
        //VARIABLES GLOBALES PARA MIS ACTAS DE VISITA
        string ruta = Manual.manualComercial;

        //CONSTRUCTOR DEL MANTENIMIENTO
        public MenuRequerimientoVenta()
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
        private void MenuRequerimientoVenta_Load(object sender, EventArgs e)
        {
            //
        }

        //ABRIR REQUERIMIENTOS DE VENTAS
        private void btnRequerimientoss_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new RequerimientoVenta());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new RequerimientoVenta());
            }
        }

        //BOTON PARA ABRORO EL MANUAL DE USUARIO DEL SISTEMA
        private void btnManualUsuario_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }
    }
}

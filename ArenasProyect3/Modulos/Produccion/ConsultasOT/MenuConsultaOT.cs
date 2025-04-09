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

namespace ArenasProyect3.Modulos.Produccion.ConsultasOT
{
    public partial class MenuConsultaOT : Form
    {
        //VARIABLES GLOBALES
        string ruta = Manual.manualProduccion;

        //CONSTRUCTOR DEL MANTENIMIENTO - MENU CONSULTAS DE OT
        public MenuConsultaOT()
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
        private void MenuConsultaOT_Load(object sender, EventArgs e)
        {
            //
        }

        //ABRIR NUEVA OT
        private void btnNuevaOrdenTrabajo_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new NuevaOT());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new NuevaOT());
            }
        }

        //ABRIR LISTADO DE ORDENES DE TRABAJO
        private void btnListarOrdenTrabajo_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ListadoOT());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ListadoOT());
            }
        }

        //LISTADO DE DETALLES DE OT
        private void btnListadoDetalleOT_Click(object sender, EventArgs e)
        {
            //if (panelMantenimientos.Controls.Count == 1)
            //{
            //    panelMantenimientos.Controls.Clear();
            //    AbrirMantenimiento(new ListadoOT());
            //}
            //else
            //{
            //    panelMantenimientos.Controls.Clear();
            //    AbrirMantenimiento(new ListadoOT());
            //}
        }

        //ABIRIR EL MANUAL DE USUARIO
        private void btnManualUsuario_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }
    }
}

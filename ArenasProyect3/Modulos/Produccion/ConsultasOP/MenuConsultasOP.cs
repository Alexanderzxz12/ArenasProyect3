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

namespace ArenasProyect3.Modulos.Produccion.ConsultasOP
{
    public partial class MenuConsultasOP : Form
    {
        //VARIABLES GLOBALES
        string ruta = Manual.manualProduccion;

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

        //ABRIR LISTADO DE PEDIDOS
        private void btnListadoPedidois_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ListadoPedidos());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ListadoPedidos());
            }
        }

        //ABRIR LISTADO DE ORDENES DE PRODUCCION
        private void btnListadoOP_Click(object sender, EventArgs e)
        {

        }

        //ABRIR LISTADO DETALLES DE LAS ORDENES DE PRODUCCION
        private void btnListadoDetalleOP_Click(object sender, EventArgs e)
        {

        }

        //ABIRIR EL MANUAL DE USUARIO
        private void btnManualUsuario_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }
    }
}

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

namespace ArenasProyect3.Modulos.Comercial.Ventas
{
    public partial class MenuVentas : Form
    {
        //VARIABLES GLOBALES
        string ruta = ManGeneral.Manual.manualVentas;

        //CONSTRUCTOR DEL MANTENIMIENTO - MENU DE COTIZACIONES Y PEDIDOS
        public MenuVentas()
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
        private void MenuVentas_Load(object sender, EventArgs e)
        {
            //
        }

        //ABRIR COTIZACIONES
        private void btnCotizaciones_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new Cotizacion());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new Cotizacion());
            }
        }

        //ABRIR PEDIDOS
        private void btnPedidos_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new Pedido());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new Pedido());
            }
        }

        //ABIRIR EL MANUAL DE USUARIO
        private void btnManualUsuario_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }
    }
}

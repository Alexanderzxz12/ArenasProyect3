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

namespace ArenasProyect3.Modulos.Logistica.Compras
{
    public partial class MenuAccionesCompra : Form
    {
        //VARIABLES GLOBALES
        string ruta = ManGeneral.Manual.manualAreaLogistica;

        //CONSTRUCTOR DE MI FORM
        public MenuAccionesCompra()
        {
            InitializeComponent();
        }

        //FUNCION PARA ABRIR MI FORMNULARIO SECUNDARIO EN MI FORMULARIO PRONCIPAL
        public void AbrirMantenimiento(object frmMantenimientos)
        {
            Form frm = frmMantenimientos as Form;
            frm.TopLevel = false;
            frm.Dock = DockStyle.Fill;
            this.panelMantenimientos.Controls.Add(frm);
            this.panelMantenimientos.Tag = frm;
            frm.Show();
        }

        //PRIMERA CARGA DE MI FORMULARIO
        private void MenuAccionesCompra_Load(object sender, EventArgs e)
        {
            //
        }

        //INGRESO A MI LISTADO DE REQUERIMIENTOS SIMPLES
        private void btnListadoRequerimientoSimple_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new Compras.ListadoRequerimientosSimples());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new Compras.ListadoRequerimientosSimples());
            }
        }

        //INGRESO A MI AMNTENIMIENTO PROCEEDORES
        private void btnListadoProveedores_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new Mantenimientos.Proveedores());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new Mantenimientos.Proveedores());
            }
        }

        //ABRIR MI MANUAL DEL ÁREA
        private void btnManualUsuario_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }

        //INGRESO A MI MANTENUMIENTO DE LISTADO DE ORDENES DE COMPRA
        private void btnListadoOrdenCompra_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new Compras.ListadoOrdenesCompra());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new Compras.ListadoOrdenesCompra());
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using ArenasProyect3.Modulos.ManGeneral;

namespace ArenasProyect3.Modulos.Comercial.RequerimientosVentas
{
    public partial class MenuRequerimientoLiquidacion : Form
    {
        //VARIABLES GLOBALES PARA EL MANTENIMIENTO
        string ruta = ManGeneral.Manual.manualViajes;

        //CONSTRUCTOR DEL MANTENIMIENTO - MENU DE REQUERIMIENTOS Y LIQUIDACIONES
        public MenuRequerimientoLiquidacion()
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
        private void MenuRequerimientoLiquidacion_Load(object sender, EventArgs e)
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

        //ABRIR LIQUIDACION DE VENTAS
        private void btnLiquidaciones_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new LiquidacionVenta());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new LiquidacionVenta());
            }
        }

        //ABIRIR EL MANUAL DE USUARIO
        private void btnManualUsuario_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }
    }
}

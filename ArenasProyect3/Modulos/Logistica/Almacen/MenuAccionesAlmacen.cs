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

namespace ArenasProyect3.Modulos.Logistica.Almacen
{
    public partial class MenuAccionesAlmacen : Form
    {
        //VARIABLES GLOBALES
        string ruta = Manual.manualLogistica;

        //CONSTRUCTOR DE MI FORM
        public MenuAccionesAlmacen()
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
        private void MenuAccionesAlmacen_Load(object sender, EventArgs e)
        {
            //
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

        //INGRESO A MI MANTENIMIENTO TIPO DE CAMBIO
        private void btnTipoCambio_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new TipoCambio());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new TipoCambio());
            }
        }

        //INGRESO A MI MANTENIMIENTO NOTA DE INGRESO
        private void btnNotaIngreso_Click(object sender, EventArgs e)
        {
            //if (panelMantenimientosCarga.Controls.Count == 1)
            //{
            //    panelMantenimientosCarga.Controls.Clear();
            //    AbrirMantenimiento(new NotaIngreso());
            //}
            //else
            //{
            //    panelMantenimientosCarga.Controls.Clear();
            //    AbrirMantenimiento(new NotaIngreso());
            //}
        }

        //INGRESO A MI MANTENIMIENTO LISTADO DE NOTAS DE INGRESO
        private void btnListadoNotaIngreso_Click(object sender, EventArgs e)
        {
            //if (panelMantenimientosCarga.Controls.Count == 1)
            //{
            //    panelMantenimientosCarga.Controls.Clear();
            //    AbrirMantenimiento(new NotaIngreso());
            //}
            //else
            //{
            //    panelMantenimientosCarga.Controls.Clear();
            //    AbrirMantenimiento(new NotaIngreso());
            //}
        }

        //INGRESO A MI MANTENIMIENTO NOTA DE SALIDA
        private void btnNotaSalida_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new NotaSalida());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new NotaSalida());
            }
        }

        //INGRESO A MI MANTENIMIENTO LISTADO NOTA DE SALIDA
        private void btnListadoNotaSalida_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ListadoNotaSalida());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ListadoNotaSalida());
            }
        }

        //INGRESO A MI MANTENUMIENTO DE KARDEX
        private void btnListadoKardex_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Este submenú aún no se encuentra disponible", "Validación del Sistema");
        }

        //ABRIR MI MANUAL DEL ÁREA
        private void btnManualUsuario_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }
    }
}

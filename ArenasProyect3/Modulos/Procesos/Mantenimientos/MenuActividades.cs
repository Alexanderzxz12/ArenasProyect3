using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Procesos.Mantenimientos
{
    public partial class MenuActividades : Form
    {
        //CONSTRUCTOR DEL MANTENIMIENTO - MENU DE ACTIVIDADES PRINCIPALES
        public MenuActividades()
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
        private void MenuActividades_Load(object sender, EventArgs e)
        {
            //
        }

        //ABRIR MANTENIMIENTO DE CUENTAS
        private void btnCuenta_Click(object sender, EventArgs e)
        {
            if (panelMantenimientosAPrincipales.Controls.Count == 1)
            {
                panelMantenimientosAPrincipales.Controls.Clear();
                AbrirMantenimiento(new MantenimientoCuentas());
            }
            else
            {
                panelMantenimientosAPrincipales.Controls.Clear();
                AbrirMantenimiento(new MantenimientoCuentas());
            }
        }

        //ABRIR MANTENIMEINTO DE LINEAS
        private void btnLinea_Click(object sender, EventArgs e)
        {
            if (panelMantenimientosAPrincipales.Controls.Count == 1)
            {
                panelMantenimientosAPrincipales.Controls.Clear();
                AbrirMantenimiento(new MantenimientoLineas());
            }
            else
            {
                panelMantenimientosAPrincipales.Controls.Clear();
                AbrirMantenimiento(new MantenimientoLineas());
            }
        }

        //ABRIR MANTEINMIENTO DE MODELOS
        private void btnModelo_Click(object sender, EventArgs e)
        {
            if (panelMantenimientosAPrincipales.Controls.Count == 1)
            {
                panelMantenimientosAPrincipales.Controls.Clear();
                AbrirMantenimiento(new MantenimientoModelos());
            }
            else
            {
                panelMantenimientosAPrincipales.Controls.Clear();
                AbrirMantenimiento(new MantenimientoModelos());
            }
        }

        //ABRIR MANTENIMEINTO DE PRODUCTO POR OPERACION
        private void btnProductoPorOperacion_Click(object sender, EventArgs e)
        {
            if (panelMantenimientosAPrincipales.Controls.Count == 1)
            {
                panelMantenimientosAPrincipales.Controls.Clear();
                AbrirMantenimiento(new MantenimientoProductoOperacion());
            }
            else
            {
                panelMantenimientosAPrincipales.Controls.Clear();
                AbrirMantenimiento(new MantenimientoProductoOperacion());
            }
        }

        //ABRIR MANTENIMEINTO DE SUBPRODUCTO POR OPERACION
        private void btnSubproductoPorOperacion_Click(object sender, EventArgs e)
        {
            if (panelMantenimientosAPrincipales.Controls.Count == 1)
            {
                panelMantenimientosAPrincipales.Controls.Clear();
                AbrirMantenimiento(new MantenimientoSubProductoOperacion());
            }
            else
            {
                panelMantenimientosAPrincipales.Controls.Clear();
                AbrirMantenimiento(new MantenimientoSubProductoOperacion());
            }
        }

        //ABRIR MANTENIMIENTO DE OPERACIONES
        private void btnOperaciones_Click(object sender, EventArgs e)
        {
            if (panelMantenimientosAPrincipales.Controls.Count == 1)
            {
                panelMantenimientosAPrincipales.Controls.Clear();
                AbrirMantenimiento(new MantenimientoOperaciones());
            }
            else
            {
                panelMantenimientosAPrincipales.Controls.Clear();
                AbrirMantenimiento(new MantenimientoOperaciones());
            }
        }

        //ABRIRI MANTENIMEINTO DE MAQUINARIAS
        private void btnMaquinarias_Click(object sender, EventArgs e)
        {
            if (panelMantenimientosAPrincipales.Controls.Count == 1)
            {
                panelMantenimientosAPrincipales.Controls.Clear();
                AbrirMantenimiento(new MantenimeintoMaquinarias());
            }
            else
            {
                panelMantenimientosAPrincipales.Controls.Clear();
                AbrirMantenimiento(new MantenimeintoMaquinarias());
            }
        }
    }
}

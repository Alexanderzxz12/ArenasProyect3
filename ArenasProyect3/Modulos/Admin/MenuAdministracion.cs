using ArenasProyect3.Modulos.Comercial.Ventas;
using ArenasProyect3.Modulos.UsuariosPermisos;
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

namespace ArenasProyect3.Modulos.Admin
{
    public partial class MenuAdministracion : Form
    {
        //VARIABLES GLOBALES
        string ruta = ManGeneral.Manual.manualAreaAdmin;

        //CONSTRUCTOR DEL MANTENIMIENTO - MENU DE COTIZACIONES Y PEDIDOS
        public MenuAdministracion()
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
        private void MenuAdministracion_Load(object sender, EventArgs e)
        {
            //
        }

        //ABRIR MANTENIMEINTO DE USUARIOS
        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new Usuarios());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new Usuarios());
            }
        }

        //ABRIR ESTADO SISTEMA
        private void btnEstadoSistema_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ManEstadoSistema());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ManEstadoSistema());
            }
        }

        //ABRIR ESTADO LICENCIAS
        private void btnEstadoLicencias_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ManLicencias());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new ManLicencias());
            }
        }

        //ABRIR ESTADO NOVEDADES
        private void btnEstadoNovedades_Click(object sender, EventArgs e)
        {
            if (panelMantenimientos.Controls.Count == 1)
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new MnEstadoNovedades());
            }
            else
            {
                panelMantenimientos.Controls.Clear();
                AbrirMantenimiento(new MnEstadoNovedades());
            }
        }

        //ABIRIR EL MANUAL DE USUARIO
        private void btnManualUsuario_Click(object sender, EventArgs e)
        {
            Process.Start(ruta);
        }
    }
}

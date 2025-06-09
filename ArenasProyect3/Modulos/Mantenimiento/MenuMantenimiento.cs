using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Mantenimiento
{
    public partial class MenuMantenimiento : Form
    {
        //VARIABLES GENERALES
        string maquina = Environment.MachineName;

        //CONSTRUCTOR DEL MANTENIMIENTO - MENU MANTENIMIENTO
        public MenuMantenimiento()
        {
            InitializeComponent();
        }

        //CÓDIGO PARA PODER MOSTRAR LA HORA EN VIVO
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblHoraVivo.Text = DateTime.Now.ToString("H:mm:ss tt");
            lblFechaVivo.Text = DateTime.Now.ToLongDateString();
        }

        //Drag Form - LIBRERIA PARA PODER MOVER EL FORMULARIO PRINCIPAL
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int LParam);

        //EVENTO PARA TRAER LAS LIBRERIAS PARA PODER MOVER
        private void panelPrincipal_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        //CERRAR EL MENÚ PRINCIPAÑ
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        //MINIMIZAR EL MENÚ PRINCIPAL
        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        //ACHICAR PANTALLA
        private void btnMinimizarTamañoEspecifico_Click(object sender, EventArgs e)
        {
            if (this.Size == new Size(1337, 720))
            {
                this.FormBorderStyle = FormBorderStyle.None; // Opcional: quitar bordes
                this.Bounds = Screen.PrimaryScreen.WorkingArea; // Ajustar al área disponible
            }
            else
            {
                this.WindowState = FormWindowState.Normal;  // Restaurar a tamaño normal
                this.Size = new Size(1337, 720);  // Definir un tamaño más pequeño
            }
        }

        //EVENTO DE INICIO Y DE CARGA DEL MENÚ PRINCIPAL
        private void MenuMantenimiento_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None; // Opcional: quitar bordes
            this.Bounds = Screen.PrimaryScreen.WorkingArea; // Ajustar al área disponible
        }

        private void panel41_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel43_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel45_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnClientes_Click(object sender, EventArgs e)
        {

        }
    }
}

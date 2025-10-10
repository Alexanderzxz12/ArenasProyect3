using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Mantenimiento
{
    public partial class MenuMantenimiento : Form
    {
        //CONSTRUCTOR DEL MANTENIMIENTO - MENU MANTENIMIENTO
        public MenuMantenimiento()
        {
            InitializeComponent();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
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

        //EVENTO DE INICIO Y DE CARGA DEL MENÚ PRINCIPAL
        private void MenuMantenimiento_Load(object sender, EventArgs e)
        {
            //FUNCION PARA CARGAR DATOS DEL USUARIO
            DatosUsuario();
            this.Resize += new EventHandler(MenuMantenimiento_Resize);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                Color.Black, 2, ButtonBorderStyle.Solid,
                Color.Black, 2, ButtonBorderStyle.Solid,
                Color.Black, 2, ButtonBorderStyle.Solid,
                Color.Black, 2, ButtonBorderStyle.Solid);
        }

        //QUITAR EL BORDE PARA DIBUJARA DE NUEVO
        private void MenuMantenimiento_Resize(object sender, EventArgs e)
        {
            this.Invalidate(); // Fuerza el repintado del formulario
        }

        //FUNCION PARA ARRASTRAR EL FORMULARIO
        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTCLIENT = 1;
            const int HTLEFT = 10;
            const int HTRIGHT = 11;
            const int HTTOP = 12;
            const int HTTOPLEFT = 13;
            const int HTTOPRIGHT = 14;
            const int HTBOTTOM = 15;
            const int HTBOTTOMLEFT = 16;
            const int HTBOTTOMRIGHT = 17;

            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST)
            {
                Point cursor = PointToClient(Cursor.Position);
                int grip = 10; // Tamaño del área sensible al borde

                if (cursor.X <= grip && cursor.Y <= grip)
                    m.Result = (IntPtr)HTTOPLEFT;
                else if (cursor.X >= Width - grip && cursor.Y <= grip)
                    m.Result = (IntPtr)HTTOPRIGHT;
                else if (cursor.X <= grip && cursor.Y >= Height - grip)
                    m.Result = (IntPtr)HTBOTTOMLEFT;
                else if (cursor.X >= Width - grip && cursor.Y >= Height - grip)
                    m.Result = (IntPtr)HTBOTTOMRIGHT;
                else if (cursor.X <= grip)
                    m.Result = (IntPtr)HTLEFT;
                else if (cursor.X >= Width - grip)
                    m.Result = (IntPtr)HTRIGHT;
                else if (cursor.Y <= grip)
                    m.Result = (IntPtr)HTTOP;
                else if (cursor.Y >= Height - grip)
                    m.Result = (IntPtr)HTBOTTOM;
                else
                    m.Result = (IntPtr)HTCLIENT;
            }
        }

        //MINIMIZAR EL MENÚ PRINCIPAL
        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        //CERRAR EL MENÚ PRINCIPAÑ
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private Rectangle tamañoOriginal;

        //MINIMIZAR Y MAXIMINZAR
        private void btnMaximinarMinimizar_Click(object sender, EventArgs e)
        {
            if (this.Bounds == Screen.GetWorkingArea(this))
            {
                this.Bounds = tamañoOriginal;
            }
            else
            {
                tamañoOriginal = this.Bounds;
                this.Bounds = Screen.GetWorkingArea(this);
            }
        }

        //BUSQUEDA DE USUARIO
        public void DatosUsuario()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Conexion.ConexionMaestra.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("BuscarUsuarioPorCodigo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idusuario", Program.IdUsuario);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            datalistadoBusquedaUusario.DataSource = dt;
            con.Close();

            imgUsuario.BackgroundImage = null;
            byte[] b = (Byte[])datalistadoBusquedaUusario.SelectedCells[5].Value;
            MemoryStream ms = new MemoryStream(b);
            imgUsuario.Image = Image.FromStream(ms);

            lblusuarioActual.Text = datalistadoBusquedaUusario.SelectedCells[1].Value.ToString() + " " + datalistadoBusquedaUusario.SelectedCells[2].Value.ToString();
            Program.NombreUsuarioCompleto = datalistadoBusquedaUusario.SelectedCells[1].Value.ToString() + " " + datalistadoBusquedaUusario.SelectedCells[2].Value.ToString();
        }


    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Modulos.Login());           
        }
        //Modulos.Admin.MnEstadoNovedades

        //VARIABLES DE ACCESO AL SISTEMA
        public static int IdUsuario;
        public static string AreaUsuario;
        public static int RangoEfecto;
        public static string NombreUsuario;
        public static string UnoNombreUnoApellidoUsuario;
        public static string NombreUsuarioCompleto;
        public static string Alias;

        //VARIABLES PARA PRODUCTOS GENERALES
        //public static string idlinea;
        //public static string idmodelo;


    }
}

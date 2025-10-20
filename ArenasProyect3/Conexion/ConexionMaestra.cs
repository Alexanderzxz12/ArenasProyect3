using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenasProyect3.Conexion
{
    public class ConexionMaestra
    {
        //public static string conexion = @"Data source = LAPTOP-LVHSRB96\SQLEXPRESS; Initial Catalog=BD_VENTAS_2;Integrated Security=true";
        //public static string conexion = @"Server = tcp:LAPTOP-JCUADROS\SQLEXPRESS,49500;DataBase=BD_VENTAS_2;User = sa;Password=12345";
        //connectionString="Data Source=192.168.1.203\SA,1433;Initial Catalog=dbarenasprod;User ID=sa;Password=Arenas2019"
        public static string conexionSoft = @"Data Source=192.168.1.203\SA,1433;Initial Catalog=dbarenasprod;User ID=sa;Password=Arenas2019";
        public static string conexion = @"Server = tcp:192.168.1.154,1433;DataBase=BD_VENTAS_2;User = sa;Password=Arenas.2020!";
        //public static string conexion = @"Server = AHUAMAN-PC;DataBase=BD_VENTAS_2;User = sa;Password=123456";
    }

}
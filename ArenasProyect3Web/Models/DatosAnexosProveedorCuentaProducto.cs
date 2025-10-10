using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DatosAnexosProveedorCuentaProducto
    {
        public int IdDatosAnexosProveedorCuentaProducto { get; set; }
        public int? IdProveedor { get; set; }
        public int? IdCuenta { get; set; }
        public int? IdLinea { get; set; }
        public int? IdModelo { get; set; }
        public int? Estado { get; set; }
    }
}

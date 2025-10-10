using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DatosAnexosProveedorSucursal
    {
        public int IdDatosAnexosProveedorSucursal { get; set; }
        public string? NombreSucursal { get; set; }
        public string? LugarEntrega { get; set; }
        public int? Estado { get; set; }
        public int? IdProveedor { get; set; }
    }
}

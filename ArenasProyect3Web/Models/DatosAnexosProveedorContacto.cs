using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DatosAnexosProveedorContacto
    {
        public int IdDatosAnexosProveedorContacto { get; set; }
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public int? Estado { get; set; }
        public int? IdProveedor { get; set; }
    }
}

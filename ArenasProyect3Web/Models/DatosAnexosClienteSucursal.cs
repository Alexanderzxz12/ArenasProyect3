using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DatosAnexosClienteSucursal
    {
        public int IdDatosAnexosClienteSucursal { get; set; }
        public int? IdCliente { get; set; }
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? CodigoPais { get; set; }
        public string? CodigoDepartamento { get; set; }
        public string? CodigoProvincia { get; set; }
        public string? CodigoDistrito { get; set; }
        public int? Estado { get; set; }

        public virtual Cliente? IdClienteNavigation { get; set; }
    }
}

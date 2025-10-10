using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DetalleVendedorRequerimientoVentum
    {
        public int IdDetalleVendedorRequerimientoVenta { get; set; }
        public int? IdRequerimientoVenta { get; set; }
        public int? IdVendedor { get; set; }
        public int? Estado { get; set; }
    }
}

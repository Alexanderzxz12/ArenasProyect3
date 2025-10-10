using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DetalleClienteRequerimientoVentum
    {
        public int IdDetalleClienteRequerimientoVenta { get; set; }
        public int? IdRequerimientoVenta { get; set; }
        public int? IdCliente { get; set; }
        public int? IdUnidad { get; set; }
        public string? CodigoDepartamento { get; set; }
        public int? Estado { get; set; }
    }
}

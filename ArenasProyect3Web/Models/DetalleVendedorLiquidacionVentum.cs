using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DetalleVendedorLiquidacionVentum
    {
        public int IdDetalleVnededorLiquidacionVenta { get; set; }
        public int? IdLiquidacion { get; set; }
        public bool? Asistencia { get; set; }
        public int? IdVendedor { get; set; }
        public int? Estado { get; set; }
    }
}

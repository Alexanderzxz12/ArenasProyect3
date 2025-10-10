using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DetalleLiquidacionVentum
    {
        public int IdDetalleLiquidacionVenta { get; set; }
        public int? IdLiquidacion { get; set; }
        public DateTime? FechaLiquidacion { get; set; }
        public string? Conbustible { get; set; }
        public string? Hospedaje { get; set; }
        public string? Viatico { get; set; }
        public string? Peaje { get; set; }
        public string? Movilidad { get; set; }
        public string? Otros { get; set; }
        public string? Subtotal { get; set; }
        public int? Estado { get; set; }
    }
}

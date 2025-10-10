using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DetalleClienteLiquidacionVentum
    {
        public int IdDetalleClienteLiquidacionVenta { get; set; }
        public int? IdLiquidacion { get; set; }
        public bool? Asistencia { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaTermino { get; set; }
        public int? IdCliente { get; set; }
        public int? IdUnidad { get; set; }
        public string? CodigoDepartamento { get; set; }
        public int? Estado { get; set; }
        public int? EstadoActas { get; set; }
        public int? IdActa { get; set; }
    }
}

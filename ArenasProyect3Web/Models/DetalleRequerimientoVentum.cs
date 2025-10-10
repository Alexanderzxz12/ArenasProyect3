using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DetalleRequerimientoVentum
    {
        public int IdDetalleRequerimientoVenta { get; set; }
        public int? IdRequerimientoVenta { get; set; }
        public DateTime? FechaRequerimeinto { get; set; }
        public string? Combustible { get; set; }
        public string? Hospedaje { get; set; }
        public string? Viatico { get; set; }
        public string? Peaje { get; set; }
        public string? Movilidad { get; set; }
        public string? Otros { get; set; }
        public string? SubTotal { get; set; }
        public int? Estado { get; set; }

        public virtual RequerimientoVentum? IdRequerimientoVentaNavigation { get; set; }
    }
}

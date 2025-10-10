using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DetalleCantidadesCalidad
    {
        public int IdDetalleCantidadCalidad { get; set; }
        public int? IdOrdenProduccion { get; set; }
        public int? Cantidad { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? Estado { get; set; }
        public decimal? PesoTeorico { get; set; }
        public decimal? PesoReal { get; set; }
        public string? Obserbaciones { get; set; }
        public int? EstadoAd { get; set; }
    }
}

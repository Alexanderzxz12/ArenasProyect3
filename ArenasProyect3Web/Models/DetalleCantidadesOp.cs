using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DetalleCantidadesOp
    {
        public int IdDetalleCantidadOrdenProduccion { get; set; }
        public int? IdOrdenProduccion { get; set; }
        public int? Cantidad { get; set; }
        public int? Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DetalleCantidadesOt
    {
        public int IdDetalleCantidadOrdenServicio { get; set; }
        public int? IdOrdenServicio { get; set; }
        public int? Cantidad { get; set; }
        public int? Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}

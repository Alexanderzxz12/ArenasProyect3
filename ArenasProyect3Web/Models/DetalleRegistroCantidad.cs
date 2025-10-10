using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DetalleRegistroCantidad
    {
        public int IdRegistroCantidades { get; set; }
        public int? IdOrdenProduccion { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? Cantidad { get; set; }
        public int? Estado { get; set; }
    }
}

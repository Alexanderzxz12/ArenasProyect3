using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Bonificacion
    {
        public Bonificacion()
        {
            DetalleCotizacions = new HashSet<DetalleCotizacion>();
        }

        public int IdBonificacion { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<DetalleCotizacion> DetalleCotizacions { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Almacen
    {
        public Almacen()
        {
            Cotizacions = new HashSet<Cotizacion>();
        }

        public int IdAlmacen { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
        public string? Concepto { get; set; }

        public virtual ICollection<Cotizacion> Cotizacions { get; set; }
    }
}

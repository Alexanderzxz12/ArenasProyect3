using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class CondicionPago
    {
        public CondicionPago()
        {
            Cotizacions = new HashSet<Cotizacion>();
            DatosAnexosClienteCindicions = new HashSet<DatosAnexosClienteCindicion>();
        }

        public int IdCondicionPago { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<Cotizacion> Cotizacions { get; set; }
        public virtual ICollection<DatosAnexosClienteCindicion> DatosAnexosClienteCindicions { get; set; }
    }
}

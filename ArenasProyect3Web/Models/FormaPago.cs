using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class FormaPago
    {
        public FormaPago()
        {
            Cotizacions = new HashSet<Cotizacion>();
            DatosAnexosClienteCindicions = new HashSet<DatosAnexosClienteCindicion>();
        }

        public int IdFormaPago { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<Cotizacion> Cotizacions { get; set; }
        public virtual ICollection<DatosAnexosClienteCindicion> DatosAnexosClienteCindicions { get; set; }
    }
}

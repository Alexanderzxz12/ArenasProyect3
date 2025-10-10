using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Zona
    {
        public Zona()
        {
            DatosAnexosClienteUnidads = new HashSet<DatosAnexosClienteUnidad>();
        }

        public int IdZona { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<DatosAnexosClienteUnidad> DatosAnexosClienteUnidads { get; set; }
    }
}

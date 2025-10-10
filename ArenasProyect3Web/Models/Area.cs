using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Area
    {
        public Area()
        {
            DatosAnexosClienteContactos = new HashSet<DatosAnexosClienteContacto>();
        }

        public int IdArea { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<DatosAnexosClienteContacto> DatosAnexosClienteContactos { get; set; }
    }
}

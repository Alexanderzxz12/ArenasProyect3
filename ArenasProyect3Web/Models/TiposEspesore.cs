using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class TiposEspesore
    {
        public TiposEspesore()
        {
            DescripcionEspesores = new HashSet<DescripcionEspesore>();
        }

        public int IdTipoEspesores { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
        public string? Magnitud { get; set; }

        public virtual ICollection<DescripcionEspesore> DescripcionEspesores { get; set; }
    }
}

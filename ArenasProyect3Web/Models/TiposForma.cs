using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class TiposForma
    {
        public TiposForma()
        {
            DescripcionFormas = new HashSet<DescripcionForma>();
        }

        public int IdTipoFormas { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
        public string? Magnitud { get; set; }

        public virtual ICollection<DescripcionForma> DescripcionFormas { get; set; }
    }
}

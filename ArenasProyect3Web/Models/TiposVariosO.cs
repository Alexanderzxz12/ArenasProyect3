using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class TiposVariosO
    {
        public TiposVariosO()
        {
            DescripcionVarios0s = new HashSet<DescripcionVarios0>();
        }

        public int IdTipoVariosO { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
        public string? Magnitud { get; set; }

        public virtual ICollection<DescripcionVarios0> DescripcionVarios0s { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class TiposNtipo
    {
        public TiposNtipo()
        {
            DescripcionNtipos = new HashSet<DescripcionNtipo>();
        }

        public int IdTipoNtipos { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
        public string? Magnitud { get; set; }

        public virtual ICollection<DescripcionNtipo> DescripcionNtipos { get; set; }
    }
}

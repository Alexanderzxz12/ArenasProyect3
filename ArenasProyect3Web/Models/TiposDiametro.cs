using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class TiposDiametro
    {
        public TiposDiametro()
        {
            DescripcionDiametros = new HashSet<DescripcionDiametro>();
        }

        public int IdTipoDiametros { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
        public string? Magnitud { get; set; }

        public virtual ICollection<DescripcionDiametro> DescripcionDiametros { get; set; }
    }
}

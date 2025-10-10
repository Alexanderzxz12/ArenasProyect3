using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Operacione
    {
        public Operacione()
        {
            LineaXoperacionXmaquinaria = new HashSet<LineaXoperacionXmaquinarium>();
            LineaXoperacions = new HashSet<LineaXoperacion>();
        }

        public int IdOperaciones { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<LineaXoperacionXmaquinarium> LineaXoperacionXmaquinaria { get; set; }
        public virtual ICollection<LineaXoperacion> LineaXoperacions { get; set; }
    }
}

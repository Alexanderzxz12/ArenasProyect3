using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Maquinaria
    {
        public Maquinaria()
        {
            LineaXoperacionXmaquinaria = new HashSet<LineaXoperacionXmaquinarium>();
        }

        public int IdMaquinarias { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<LineaXoperacionXmaquinarium> LineaXoperacionXmaquinaria { get; set; }
    }
}

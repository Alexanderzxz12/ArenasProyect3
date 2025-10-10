using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class LineaXoperacion
    {
        public int IdLineaXoperacion { get; set; }
        public int? IdLinea { get; set; }
        public int? IdOperacion { get; set; }
        public int? Estado { get; set; }

        public virtual Linea? IdLineaNavigation { get; set; }
        public virtual Operacione? IdOperacionNavigation { get; set; }
    }
}

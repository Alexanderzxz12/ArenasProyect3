using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class PlanoXproducto
    {
        public int IdPlanoXproducto { get; set; }
        public int? IdArt { get; set; }
        public int? IdPlano { get; set; }
        public int? Estado { get; set; }

        public virtual Producto? IdArtNavigation { get; set; }
        public virtual PlanoProducto? IdPlanoNavigation { get; set; }
    }
}

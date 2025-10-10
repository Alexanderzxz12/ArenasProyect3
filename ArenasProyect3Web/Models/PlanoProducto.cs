using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class PlanoProducto
    {
        public PlanoProducto()
        {
            PlanoXproductos = new HashSet<PlanoXproducto>();
        }

        public int IdPlano { get; set; }
        public byte[]? Doc { get; set; }
        public string? NameReferences { get; set; }
        public string? Name { get; set; }
        public string? RealDoc { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<PlanoXproducto> PlanoXproductos { get; set; }
    }
}

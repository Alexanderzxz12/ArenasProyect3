using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class LineaXoperacionXmaquinarium
    {
        public LineaXoperacionXmaquinarium()
        {
            FormulacionActividadesProductos = new HashSet<FormulacionActividadesProducto>();
        }

        public int IdLineaXoperacioXmaquinaria { get; set; }
        public int? IdLinea { get; set; }
        public int? IdOperacion { get; set; }
        public int? IdMaquinaria { get; set; }
        public int? Estado { get; set; }

        public virtual Linea? IdLineaNavigation { get; set; }
        public virtual Maquinaria? IdMaquinariaNavigation { get; set; }
        public virtual Operacione? IdOperacionNavigation { get; set; }
        public virtual ICollection<FormulacionActividadesProducto> FormulacionActividadesProductos { get; set; }
    }
}

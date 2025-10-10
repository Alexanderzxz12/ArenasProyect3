using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Correlativo
    {
        public Correlativo()
        {
            FormulacionActividadesProductos = new HashSet<FormulacionActividadesProducto>();
        }

        public int IdCorrelativo { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<FormulacionActividadesProducto> FormulacionActividadesProductos { get; set; }
    }
}

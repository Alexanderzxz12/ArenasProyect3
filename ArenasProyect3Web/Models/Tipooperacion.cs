using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Tipooperacion
    {
        public Tipooperacion()
        {
            FormulacionActividadesProductos = new HashSet<FormulacionActividadesProducto>();
        }

        public int IdTipoOperacion { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<FormulacionActividadesProducto> FormulacionActividadesProductos { get; set; }
    }
}

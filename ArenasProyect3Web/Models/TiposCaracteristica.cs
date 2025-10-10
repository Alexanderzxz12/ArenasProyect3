using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class TiposCaracteristica
    {
        public TiposCaracteristica()
        {
            DescripcionCaracteristicas = new HashSet<DescripcionCaracteristica>();
        }

        public int IdTipoCaracteristicas { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<DescripcionCaracteristica> DescripcionCaracteristicas { get; set; }
    }
}

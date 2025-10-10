using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class TiposMedida
    {
        public TiposMedida()
        {
            DescripcionMedida = new HashSet<DescripcionMedida>();
        }

        public int IdTipoMedidas { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
        public string? Magnitud { get; set; }

        public virtual ICollection<DescripcionMedida> DescripcionMedida { get; set; }
    }
}

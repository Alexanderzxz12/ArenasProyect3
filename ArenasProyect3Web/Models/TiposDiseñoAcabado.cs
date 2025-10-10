using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class TiposDiseñoAcabado
    {
        public TiposDiseñoAcabado()
        {
            DescripcionDiseñoAcabados = new HashSet<DescripcionDiseñoAcabado>();
        }

        public int IdTipoDiseñoAcabado { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
        public string? Magnitud { get; set; }

        public virtual ICollection<DescripcionDiseñoAcabado> DescripcionDiseñoAcabados { get; set; }
    }
}

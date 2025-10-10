using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DescripcionMedida
    {
        public int IdDescripcionMedidas { get; set; }
        public int? IdTipoMedidas { get; set; }
        public int? IdModelo { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual Modelo? IdModeloNavigation { get; set; }
        public virtual TiposMedida? IdTipoMedidasNavigation { get; set; }
    }
}

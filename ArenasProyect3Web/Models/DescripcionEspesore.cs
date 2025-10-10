using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DescripcionEspesore
    {
        public int IdDescripcionEspesores { get; set; }
        public int? IdTipoEspesores { get; set; }
        public int? IdModelo { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual Modelo? IdModeloNavigation { get; set; }
        public virtual TiposEspesore? IdTipoEspesoresNavigation { get; set; }
    }
}

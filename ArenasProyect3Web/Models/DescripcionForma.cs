using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DescripcionForma
    {
        public int IdDescripcionFormas { get; set; }
        public int? IdTipoFormas { get; set; }
        public int? IdModelo { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual Modelo? IdModeloNavigation { get; set; }
        public virtual TiposForma? IdTipoFormasNavigation { get; set; }
    }
}

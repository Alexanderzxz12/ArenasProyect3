using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DescripcionDiametro
    {
        public int IdDescripcionDiametros { get; set; }
        public int? IdTipoDiametros { get; set; }
        public int? IdModelo { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual Modelo? IdModeloNavigation { get; set; }
        public virtual TiposDiametro? IdTipoDiametrosNavigation { get; set; }
    }
}

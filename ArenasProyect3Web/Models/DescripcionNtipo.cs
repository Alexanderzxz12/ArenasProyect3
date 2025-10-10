using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DescripcionNtipo
    {
        public int IdDescripcionNtipos { get; set; }
        public int? IdTipoNtipos { get; set; }
        public int? IdModelo { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
        public int? IdTipoNn { get; set; }
        public string? DescripcionTipoNn { get; set; }

        public virtual Modelo? IdModeloNavigation { get; set; }
        public virtual TiposNtipo? IdTipoNtiposNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DescripcionDiseñoAcabado
    {
        public int IdDescripcionDiseñoAcabado { get; set; }
        public int? IdTipoDiseñoAcabado { get; set; }
        public int? IdModelo { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
        public int? IdTipoNn { get; set; }
        public string? DescripcionTipoNn { get; set; }

        public virtual Modelo? IdModeloNavigation { get; set; }
        public virtual TiposDiseñoAcabado? IdTipoDiseñoAcabadoNavigation { get; set; }
    }
}

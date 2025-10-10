using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DescripcionCaracteristica
    {
        public int IdDescripcionCaracteristicas { get; set; }
        public int? IdTipoCaracteristicas { get; set; }
        public int? IdModelo { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
        public int? IdTipoNn { get; set; }
        public string? DescripcionTipoNn { get; set; }

        public virtual Modelo? IdModeloNavigation { get; set; }
        public virtual TiposCaracteristica? IdTipoCaracteristicasNavigation { get; set; }
    }
}

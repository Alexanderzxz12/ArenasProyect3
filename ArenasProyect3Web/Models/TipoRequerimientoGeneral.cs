using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class TipoRequerimientoGeneral
    {
        public int IdTipoRequerimiento { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class TipoNotaIngreso
    {
        public int IdTipoNotaIngreso { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
    }
}

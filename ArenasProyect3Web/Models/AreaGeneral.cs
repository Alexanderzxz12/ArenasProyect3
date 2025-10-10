using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class AreaGeneral
    {
        public int IdArea { get; set; }
        public string? Descripcion { get; set; }
        public int? CentroCostos { get; set; }
        public int? Estado { get; set; }
    }
}

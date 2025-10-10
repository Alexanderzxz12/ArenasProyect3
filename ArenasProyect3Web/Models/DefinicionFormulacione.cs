using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DefinicionFormulacione
    {
        public int IdDefinicionFormulaciones { get; set; }
        public string? CodigoDefinicion { get; set; }
        public int? IdTipo { get; set; }
        public int? Estado { get; set; }
        public int? IdLinea { get; set; }
    }
}

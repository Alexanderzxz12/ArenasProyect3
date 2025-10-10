using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Brochure
    {
        public int IdBrochures { get; set; }
        public string? Nombre { get; set; }
        public string? Ruta { get; set; }
        public int? IdLinea { get; set; }
        public int? Estado { get; set; }
    }
}

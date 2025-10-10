using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Responsable
    {
        public int IdResponsable { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
        public int? Documento { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class UbicacionPai
    {
        public int IdPais { get; set; }
        public string? CodigoPais { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
    }
}

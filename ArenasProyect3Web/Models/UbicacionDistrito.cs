using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class UbicacionDistrito
    {
        public int IdDistrito { get; set; }
        public string? CodigoProvincia { get; set; }
        public string? CodigoDistrito { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
    }
}

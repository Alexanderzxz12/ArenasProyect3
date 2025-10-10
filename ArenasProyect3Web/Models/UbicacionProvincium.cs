using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class UbicacionProvincium
    {
        public int IdProvincia { get; set; }
        public string? CodigoDepartamento { get; set; }
        public string? CodigoProvincia { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
    }
}

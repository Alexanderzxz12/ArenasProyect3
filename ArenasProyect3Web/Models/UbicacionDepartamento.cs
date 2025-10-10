using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class UbicacionDepartamento
    {
        public int IdDepartamento { get; set; }
        public string? CodigoPais { get; set; }
        public string? CodigoDepartamento { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
    }
}

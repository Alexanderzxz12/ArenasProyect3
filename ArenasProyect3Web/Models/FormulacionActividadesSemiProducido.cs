using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class FormulacionActividadesSemiProducido
    {
        public int IdActividadFormulacionSemiProducido { get; set; }
        public string? CodigoFormulacion { get; set; }
        public int? IdMom { get; set; }
        public int? IdCorrelativo { get; set; }
        public int? Tcosto { get; set; }
        public decimal? Tsetup { get; set; }
        public decimal? Toperacion { get; set; }
        public int? Tpor { get; set; }
        public int? Thoras { get; set; }
        public int? Personal { get; set; }
        public decimal? Cpersonal { get; set; }
        public decimal? Ctotal { get; set; }
        public int? IdTipoOperacion { get; set; }
        public int? Estado { get; set; }
    }
}

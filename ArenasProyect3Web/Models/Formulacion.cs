using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Formulacion
    {
        public int IdFormulacion { get; set; }
        public string? CodigoFormulacion { get; set; }
        public int? IdProducto { get; set; }
        public int? IdSemiProducido { get; set; }
        public string? PlanoTecnico { get; set; }
        public string? NamePlanoTecnico { get; set; }
        public string? PlanoSeguridad { get; set; }
        public string? NamePlanoSeguridad { get; set; }
        public decimal? Cif { get; set; }
        public int? IdPlanoProducto { get; set; }
        public int? IdPlanoSemiproducido { get; set; }
        public int? Visible { get; set; }
        public int? Estado { get; set; }
        public int? IdDefinicionFormulacion { get; set; }
        public int? RelacionProXsemi { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}

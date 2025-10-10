using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class FormulacionMateriale
    {
        public int IdMaterialOperacion { get; set; }
        public string? CodigoFormulacion { get; set; }
        public int? IdActividadOperacion { get; set; }
        public int? IdArt { get; set; }
        public decimal? Cantidad { get; set; }
        public int? Posicion { get; set; }
        public string? TipoMaterial { get; set; }
        public decimal? CantidadProducto { get; set; }
        public int? Estado { get; set; }
    }
}

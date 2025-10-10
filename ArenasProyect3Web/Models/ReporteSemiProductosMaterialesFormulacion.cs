using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class ReporteSemiProductosMaterialesFormulacion
    {
        public int Idmaterialactividad { get; set; }
        public int? Idproduc { get; set; }
        public string? CodBss { get; set; }
        public string? CodSistema { get; set; }
        public string? DescripciónProducto { get; set; }
        public decimal? Cantidad { get; set; }
        public string? Medida { get; set; }
        public string? Codformulacion { get; set; }
        public int Idofrmualcion { get; set; }
    }
}

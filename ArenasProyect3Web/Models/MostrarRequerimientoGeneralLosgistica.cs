using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class MostrarRequerimientoGeneralLosgistica
    {
        public int CódigoRequerimeinto { get; set; }
        public string? Código { get; set; }
        public string? Producto { get; set; }
        public string? TipoDeMedida { get; set; }
        public int? Proceso { get; set; }
        public int? CódigoMercaderiaCuenta { get; set; }
        public int? CódigoLínea { get; set; }
        public int? CódigoModelo { get; set; }
        public decimal Stock { get; set; }
        public decimal? CantidadMinima { get; set; }
        public int? VCritico { get; set; }
        public int CódigoInterno { get; set; }
    }
}

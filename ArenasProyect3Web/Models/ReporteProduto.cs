using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class ReporteProduto
    {
        public string? Código { get; set; }
        public int CódigoInterno { get; set; }
        public int? CódigoMercaderiaCuenta { get; set; }
        public int? CódigoLínea { get; set; }
        public int? CódigoModelo { get; set; }
        public string? Descripción { get; set; }
        public string? Medida { get; set; }
        public string? Modelo { get; set; }
        public string? Línea { get; set; }
        public int? Proceso { get; set; }
        public decimal? CantidadMinima { get; set; }
        public int? VCritico { get; set; }
        public string? CódigoBss { get; set; }
        public int? Estado { get; set; }
    }
}

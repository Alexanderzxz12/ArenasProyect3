using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class MostrarRequerimientoItemsGeneralLosgistica
    {
        public int Código { get; set; }
        public int? IdAtrt { get; set; }
        public string? CDelProducto { get; set; }
        public string? Producto { get; set; }
        public string? TipoDeMedida { get; set; }
        public decimal? CantidadTotal { get; set; }
        public decimal? CantidadRetirada { get; set; }
        public decimal Stock { get; set; }
        public int? IdRequerimientoSimple { get; set; }
        public string EstadoAtendido { get; set; } = null!;
    }
}

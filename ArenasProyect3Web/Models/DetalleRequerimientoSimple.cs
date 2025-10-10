using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DetalleRequerimientoSimple
    {
        public int IdDetalleRequerimientoSimple { get; set; }
        public int? IdRequerimientoSimple { get; set; }
        public int? Item { get; set; }
        public int? IdAtrt { get; set; }
        public decimal? Cantidad { get; set; }
        public decimal? Stock { get; set; }
        public int? Estado { get; set; }
        public int? EstadoAtendido { get; set; }
        public decimal? CantidadRetirada { get; set; }
        public decimal? CantidadTotal { get; set; }
    }
}

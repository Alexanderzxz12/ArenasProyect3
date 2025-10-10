using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class KardexEntradaAlmacenDetalle
    {
        public int IdDetalleEntradaAlmacen { get; set; }
        public int? IdEntradaAlmacen { get; set; }
        public int? IdArt { get; set; }
        public decimal? Cantidad { get; set; }
        public decimal? PrecioUnitarioDolares { get; set; }
        public decimal? PrecioTotalDolares { get; set; }
        public decimal? PrecioUnitarioSoles { get; set; }
        public decimal? PrecioTotalSoles { get; set; }
        public int? IdTipoMovimiento { get; set; }
        public int? Estado { get; set; }
    }
}

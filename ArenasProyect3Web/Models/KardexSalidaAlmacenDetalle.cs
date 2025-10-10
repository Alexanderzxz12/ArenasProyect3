using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class KardexSalidaAlmacenDetalle
    {
        public int IdDetalleSalidaAlmacen { get; set; }
        public int? IdSalidaAlmacen { get; set; }
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

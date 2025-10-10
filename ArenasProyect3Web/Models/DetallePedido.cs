using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DetallePedido
    {
        public int IdDetallePedido { get; set; }
        public int? IdPedido { get; set; }
        public string? CodigoProducto { get; set; }
        public string? DescripcionProducto { get; set; }
        public int? Cantidad { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public decimal? Descuento { get; set; }
        public decimal? Total { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string? CodigoFormulacion { get; set; }
        public int? Item { get; set; }
        public int? Estado { get; set; }
        public int? IdDetalleCotizacion { get; set; }
        public int? EstadoPedido { get; set; }
        public int? IdArt { get; set; }
    }
}

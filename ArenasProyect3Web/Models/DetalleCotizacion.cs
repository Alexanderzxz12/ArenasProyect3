using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DetalleCotizacion
    {
        public int IdDetalleCotizacion { get; set; }
        public int? IdCotizacion { get; set; }
        public string? CodigoProducto { get; set; }
        public string? CodigoFormulacion { get; set; }
        public int? Cantidad { get; set; }
        public decimal? PrecioUnidad { get; set; }
        public decimal? Descuento { get; set; }
        public decimal? Total { get; set; }
        public int? IdBonificacion { get; set; }
        public string? Ta { get; set; }
        public string? CodigoCliente { get; set; }
        public string? DescripcionCliente { get; set; }
        public int? Estado { get; set; }
        public bool? EstadoCoti { get; set; }
        public int? Item { get; set; }
        public int? IdPedido { get; set; }
        public int? ItemPedido { get; set; }
        public int? IdArt { get; set; }

        public virtual Producto? IdArtNavigation { get; set; }
        public virtual Bonificacion? IdBonificacionNavigation { get; set; }
        public virtual Cotizacion? IdCotizacionNavigation { get; set; }
    }
}

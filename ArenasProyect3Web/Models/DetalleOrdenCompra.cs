using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DetalleOrdenCompra
    {
        public int IdDetalleOrdenCompra { get; set; }
        public bool? Atendido { get; set; }
        public int? Item { get; set; }
        public int? IdArt { get; set; }
        public string? Cantidad { get; set; }
        public string? Precio { get; set; }
        public string? Descuento { get; set; }
        public string? Total { get; set; }
        public string? FechaEstimada { get; set; }
        public string? DescripcionProductoProveedor { get; set; }
        public int? Estado { get; set; }
        public int? IdOrdenCompra { get; set; }
        public string? FechaEntregaReal { get; set; }
        public int? IngresoAlmacen { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class OrdenProduccion
    {
        public int IdOrdenProduccion { get; set; }
        public string? CodigoOrdenProduccion { get; set; }
        public DateTime? FechaIncial { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public int? IdCliente { get; set; }
        public int? IdUnidad { get; set; }
        public int? IdVendedor { get; set; }
        public string? LugarEntrega { get; set; }
        public DateTime? FechaProduccion { get; set; }
        public int? IdArt { get; set; }
        public string? CodigoProducto { get; set; }
        public string? DescripcionProducto { get; set; }
        public string? PlanoProducto { get; set; }
        public string? Color { get; set; }
        public string? CodigoBss { get; set; }
        public string? CodigoSis { get; set; }
        public string? CodigoCliente { get; set; }
        public int? IdArtSemi { get; set; }
        public string? CodigoProductoSemi { get; set; }
        public string? DescripcionProductoSemi { get; set; }
        public string? PlanoProductoSemi { get; set; }
        public string? ColorSemi { get; set; }
        public string? CodigoBssSemi { get; set; }
        public string? CodigoSisSemi { get; set; }
        public string? CodigoClienteSemi { get; set; }
        public int? IdPedido { get; set; }
        public int? Items { get; set; }
        public int? Cantidad { get; set; }
        public int? EstadoOp { get; set; }
        public int? Estado { get; set; }
        public int? TotalItems { get; set; }
        public bool? EstadoOc { get; set; }
        public string? Observaciones { get; set; }
        public DateTime? FechaEntregaRepro1 { get; set; }
        public DateTime? FechaEntregaRepro2 { get; set; }
        public DateTime? FechaEntregaRepro3 { get; set; }
        public int? IdDetallePedido { get; set; }
        public int? EstadoCalidad { get; set; }
    }
}

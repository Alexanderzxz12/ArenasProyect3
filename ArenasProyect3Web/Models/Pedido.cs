using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Pedido
    {
        public int IdPedido { get; set; }
        public string? CodigoPedido { get; set; }
        public DateTime? FechaEmision { get; set; }
        public int? IdCliente { get; set; }
        public string? Direccion { get; set; }
        public string? LugarEntrega { get; set; }
        public int? IdUnidad { get; set; }
        public int? IdResponsable { get; set; }
        public int? IdContacto { get; set; }
        public int? IdCondicionPago { get; set; }
        public int? IdFormaPago { get; set; }
        public int? IdMoneda { get; set; }
        public int? IdAlmacen { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public decimal? Peso { get; set; }
        public string? OrdenCompra { get; set; }
        public string? RutaOrdenCompraPdf { get; set; }
        public string? Observaciones { get; set; }
        public string? DetallePedido { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? Descuento { get; set; }
        public decimal? Inafecta { get; set; }
        public decimal? Exonerado { get; set; }
        public decimal? Igv { get; set; }
        public decimal? TotalDescuento { get; set; }
        public decimal? Total { get; set; }
        public int? IdCotizacion { get; set; }
        public int? Estado { get; set; }
        public int? CantidadItems { get; set; }
        public string? MensajeAnulacion { get; set; }
        public int? EstadoPedido { get; set; }
        public int? EstadoDetalles { get; set; }
    }
}

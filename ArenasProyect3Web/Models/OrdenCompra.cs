using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class OrdenCompra
    {
        public int IdOrdenCompra { get; set; }
        public string? CodigoOrdenCompra { get; set; }
        public int? IdRequerimiento1 { get; set; }
        public int? IdRequerimeinto2 { get; set; }
        public int? IdRequerimeinto3 { get; set; }
        public int? IdProveedor { get; set; }
        public int? IdContactoProveedor { get; set; }
        public int? IdBancoProveedor { get; set; }
        public int? IdTipoOrdenCompra { get; set; }
        public int? IdFormaPago { get; set; }
        public int? IdCentroCostos { get; set; }
        public int? IdTipoMoneda { get; set; }
        public int? IdLugarEntrega { get; set; }
        public DateTime? FechaOrdenCompra { get; set; }
        public DateTime? FechaEstimada { get; set; }
        public string? CodigoCotizacion { get; set; }
        public string? FileCotizacion { get; set; }
        public string? Autorizado { get; set; }
        public string? Generado { get; set; }
        public string? Observaciones { get; set; }
        public string? SubTotal { get; set; }
        public string? Descuento { get; set; }
        public string? Flete { get; set; }
        public string? Igv { get; set; }
        public string? Total { get; set; }
        public int? Estado { get; set; }
        public int? EstadoItems { get; set; }
        public int? EstadoOc { get; set; }
        public string? MensajeAnulacion { get; set; }
        public DateTime? FechaRequerimientoMasAntiguo { get; set; }
        public DateTime? FechaRequerimeintoMasProximo { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Cotizacion
    {
        public Cotizacion()
        {
            DetalleCotizacions = new HashSet<DetalleCotizacion>();
        }

        public int IdCotizacion { get; set; }
        public DateTime? FechaEmision { get; set; }
        public DateTime? FechaValidez { get; set; }
        public int? IdCliente { get; set; }
        public int? IdComercial { get; set; }
        public int? IdModeda { get; set; }
        public string? Referencia { get; set; }
        public int? IdAlmacen { get; set; }
        public string? LugarEntrega { get; set; }
        public string? Grarantia { get; set; }
        public string? TiempoEntrega { get; set; }
        public string? Observaciones { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? Descuento { get; set; }
        public decimal? Inafecta { get; set; }
        public decimal? Exonerado { get; set; }
        public decimal? Igv { get; set; }
        public decimal? TotalDescuento { get; set; }
        public decimal? Total { get; set; }
        public int? IdUnidad { get; set; }
        public int? IdResponsable { get; set; }
        public int? IdContacto { get; set; }
        public int? IdFormaPago { get; set; }
        public int? IdCondicionPago { get; set; }
        public int? EstadoCoti { get; set; }
        public int? EstadoModi { get; set; }
        public int? Estado { get; set; }
        public string? CodigoCotizacion { get; set; }
        public string? MensajeAnulacion { get; set; }
        public int? IdBrochure { get; set; }
        public string? RutaBrochureFinal { get; set; }

        public virtual Almacen? IdAlmacenNavigation { get; set; }
        public virtual Cliente? IdClienteNavigation { get; set; }
        public virtual CondicionPago? IdCondicionPagoNavigation { get; set; }
        public virtual DatosAnexosClienteContacto? IdContactoNavigation { get; set; }
        public virtual FormaPago? IdFormaPagoNavigation { get; set; }
        public virtual TipoMoneda? IdModedaNavigation { get; set; }
        public virtual Usuario? IdResponsableNavigation { get; set; }
        public virtual DatosAnexosClienteUnidad? IdUnidadNavigation { get; set; }
        public virtual ICollection<DetalleCotizacion> DetalleCotizacions { get; set; }
    }
}

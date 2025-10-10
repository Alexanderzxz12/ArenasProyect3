using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Producto
    {
        public Producto()
        {
            DatosAnexosProductoImportacions = new HashSet<DatosAnexosProductoImportacion>();
            DatosAnexosProductoStockUbicacions = new HashSet<DatosAnexosProductoStockUbicacion>();
            DatosAnexosProductoSunats = new HashSet<DatosAnexosProductoSunat>();
            DetalleCotizacions = new HashSet<DetalleCotizacion>();
            PlanoXproductos = new HashSet<PlanoXproducto>();
            ProductoXcamposSeleccionadosDetalles = new HashSet<ProductoXcamposSeleccionadosDetalle>();
            ProductosXcamposSeleccionados = new HashSet<ProductosXcamposSeleccionado>();
        }

        public string? Codcom { get; set; }
        public int IdArt { get; set; }
        public string? IdMedida { get; set; }
        public int? IdTipoMercaderias { get; set; }
        public int? IdModelo { get; set; }
        public int? IdLinea { get; set; }
        public int? IdDiferencial { get; set; }
        public string? Detalle { get; set; }
        public string? Descripcion { get; set; }
        public int? Elevado { get; set; }
        public int? SemiProducido { get; set; }
        public string? CodigoGenerado { get; set; }
        public string? Tipo { get; set; }
        public int? Estado { get; set; }
        public int? Proceso { get; set; }
        public decimal? CantidadMinima { get; set; }
        public int? VCritico { get; set; }
        public string? RutaImagen { get; set; }
        public DateTime? FechaIngreso { get; set; }

        public virtual Diferencial? IdDiferencialNavigation { get; set; }
        public virtual Linea? IdLineaNavigation { get; set; }
        public virtual Medidum? IdMedidaNavigation { get; set; }
        public virtual Modelo? IdModeloNavigation { get; set; }
        public virtual Tipomercaderia? IdTipoMercaderiasNavigation { get; set; }
        public virtual ICollection<DatosAnexosProductoImportacion> DatosAnexosProductoImportacions { get; set; }
        public virtual ICollection<DatosAnexosProductoStockUbicacion> DatosAnexosProductoStockUbicacions { get; set; }
        public virtual ICollection<DatosAnexosProductoSunat> DatosAnexosProductoSunats { get; set; }
        public virtual ICollection<DetalleCotizacion> DetalleCotizacions { get; set; }
        public virtual ICollection<PlanoXproducto> PlanoXproductos { get; set; }
        public virtual ICollection<ProductoXcamposSeleccionadosDetalle> ProductoXcamposSeleccionadosDetalles { get; set; }
        public virtual ICollection<ProductosXcamposSeleccionado> ProductosXcamposSeleccionados { get; set; }
    }
}

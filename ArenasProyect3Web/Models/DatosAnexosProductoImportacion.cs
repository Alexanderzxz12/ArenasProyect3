using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DatosAnexosProductoImportacion
    {
        public int IdImportacion { get; set; }
        public int? IdArt { get; set; }
        public int? IdOrigen { get; set; }
        public int? IdTerminosCompra { get; set; }
        public string? Contenedor { get; set; }
        public decimal? PesoContenedor { get; set; }
        public string? Medidas { get; set; }
        public int? Estado { get; set; }

        public virtual Producto? IdArtNavigation { get; set; }
        public virtual DatosAnexosOrigen? IdOrigenNavigation { get; set; }
        public virtual DatosAnexosTerminosCompra? IdTerminosCompraNavigation { get; set; }
    }
}

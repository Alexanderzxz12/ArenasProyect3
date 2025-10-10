using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DatosAnexosProductoStockUbicacion
    {
        public int IdStockUbicacion { get; set; }
        public int? IdArt { get; set; }
        public int? AfectoIgv { get; set; }
        public int? ControlStock { get; set; }
        public int? Juego { get; set; }
        public int? Servicio { get; set; }
        public int? ControlarLotes { get; set; }
        public int? ControlarSerie { get; set; }
        public decimal? Peso { get; set; }
        public string? Ubicacion { get; set; }
        public int? Reposicion { get; set; }
        public decimal? Minimo { get; set; }
        public decimal? Maximo { get; set; }
        public int? Estado { get; set; }

        public virtual Producto? IdArtNavigation { get; set; }
    }
}

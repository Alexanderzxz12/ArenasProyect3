using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Kardex
    {
        public DateTime? Fecha { get; set; }
        public string? Guía { get; set; }
        public string? Código { get; set; }
        public string? Producto { get; set; }
        public decimal? Entradas { get; set; }
        public decimal? PrecioUnitEntradaDólares { get; set; }
        public decimal? CTotalEntradaDólares { get; set; }
        public decimal? PrecioUnitEntradaSoles { get; set; }
        public decimal? CTotalEntradaSoles { get; set; }
        public decimal? Salida { get; set; }
        public decimal? PrecioUnitSalidaDólares { get; set; }
        public decimal? CTotalSalidaDólares { get; set; }
        public decimal? PrecioUnitSalidaSoles { get; set; }
        public decimal? CTotalSalidaSoles { get; set; }
        public decimal? AlmacenGeneral { get; set; }
        public decimal? PStockDólares { get; set; }
        public decimal? CTotalDólares { get; set; }
        public decimal? PStockSoles { get; set; }
        public decimal? CTotalSoles { get; set; }
        public string? Almacen { get; set; }
    }
}

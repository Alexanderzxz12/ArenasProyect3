using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class ReporteOpCalidad
    {
        public int Id { get; set; }
        public string? NOp { get; set; }
        public DateTime? FechaDeInicio { get; set; }
        public DateTime? FechaDeEntrega { get; set; }
        public string? Cliente { get; set; }
        public string? Unidad { get; set; }
        public int? Item { get; set; }
        public string? DescripciónDelProducto { get; set; }
        public int? Cantidad { get; set; }
        public string? Color { get; set; }
        public string? NPedido { get; set; }
        public int CantidadRealizada { get; set; }
        public int CantidadInspeccionada { get; set; }
        public string EstadoOp { get; set; } = null!;
        public string EstadoCalidad { get; set; } = null!;
        public bool? EstadoDeOc { get; set; }
        public string? Oc { get; set; }
        public string? Pl { get; set; }
        public string? PrimerNombre { get; set; }
        public string? SegundoNombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public DateTime? FechaProduccion { get; set; }
    }
}

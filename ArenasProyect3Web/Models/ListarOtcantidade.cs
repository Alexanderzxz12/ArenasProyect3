using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class ListarOtcantidade
    {
        public int Id { get; set; }
        public string? NOt { get; set; }
        public DateTime? FechaDeInicio { get; set; }
        public DateTime? FechaDeEntrega { get; set; }
        public string? Cliente { get; set; }
        public string? DescripciónDelSubProducto { get; set; }
        public int? Cantidad { get; set; }
        public string? Color { get; set; }
        public int? NOp { get; set; }
        public int CantidadRealizada { get; set; }
        public string Estado { get; set; } = null!;
    }
}

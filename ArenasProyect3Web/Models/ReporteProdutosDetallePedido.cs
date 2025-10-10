using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class ReporteProdutosDetallePedido
    {
        public int Id { get; set; }
        public int? Idpedido { get; set; }
        public int Idart { get; set; }
        public int? Item { get; set; }
        public string? DescripciónProducto { get; set; }
        public string? CPedido { get; set; }
        public string? MProducto { get; set; }
        public int? CantPedido { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string? CodigoProducto { get; set; }
        public string? Descripcion { get; set; }
        public string? CodigoCliente { get; set; }
        public string? CodigoForm { get; set; }
        public string? PlProducto { get; set; }
        public string? PlSemiProducido { get; set; }
        public int? CantidadTotalItems { get; set; }
        public int? NumeroItem { get; set; }
    }
}

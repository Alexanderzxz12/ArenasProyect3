using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class MostrarOrdenCompraItemsGeneralLogistica
    {
        public int Código { get; set; }
        public int? IdArt { get; set; }
        public string? CDelProducto { get; set; }
        public string? Producto { get; set; }
        public string? TipoDeMedida { get; set; }
        public string? CantidadTotal { get; set; }
        public decimal Stock { get; set; }
        public int? IdOrdenCompra { get; set; }
        public string Estado { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class EquipoArea
    {
        public int IdEquipoArea { get; set; }
        public string? DescripcionEquipoArea { get; set; }
        public string? Consumo { get; set; }
        public string? MontoPromedioVenta { get; set; }
        public int? IdLinea { get; set; }
        public int? IdCliente { get; set; }
        public int? IdUnidad { get; set; }
        public string? Precio { get; set; }
        public string? MontoPromedioAnual { get; set; }
        public int? Estado { get; set; }
    }
}

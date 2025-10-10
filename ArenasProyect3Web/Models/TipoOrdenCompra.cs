using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class TipoOrdenCompra
    {
        public int IdTipoOrdenCompra { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
    }
}

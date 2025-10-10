using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class TipoProveedor
    {
        public int IdTipoProveedor { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
    }
}

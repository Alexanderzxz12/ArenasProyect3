using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class TipoAccion
    {
        public int IdTipoAccion { get; set; }
        public string? Accion { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
    }
}

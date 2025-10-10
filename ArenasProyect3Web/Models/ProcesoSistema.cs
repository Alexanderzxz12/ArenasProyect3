using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class ProcesoSistema
    {
        public int IdProceso { get; set; }
        public int? IdArea { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
    }
}

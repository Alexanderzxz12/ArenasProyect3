using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class PausaActiva
    {
        public int IdPausaActiva { get; set; }
        public string? Descripcion { get; set; }
        public int? TimeInterval { get; set; }
        public int? TimeSuspension { get; set; }
        public int? Estado { get; set; }
    }
}

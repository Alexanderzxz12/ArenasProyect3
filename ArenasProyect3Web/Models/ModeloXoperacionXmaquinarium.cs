using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class ModeloXoperacionXmaquinarium
    {
        public int IdModeloXoperacionXmaquinaria { get; set; }
        public int? IdModelo { get; set; }
        public int? IdOperacion { get; set; }
        public int? IdMaquinaria { get; set; }
        public int? Estado { get; set; }
    }
}

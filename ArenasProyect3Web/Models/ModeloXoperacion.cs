using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class ModeloXoperacion
    {
        public int IdModeloXoperacion { get; set; }
        public int? IdModelo { get; set; }
        public int? IdOperacion { get; set; }
        public int? Estado { get; set; }
    }
}

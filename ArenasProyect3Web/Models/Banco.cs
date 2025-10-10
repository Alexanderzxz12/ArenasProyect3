using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Banco
    {
        public int IdBanco { get; set; }
        public string? Descripcion { get; set; }
        public string? Anotacion { get; set; }
        public int? Estado { get; set; }
    }
}

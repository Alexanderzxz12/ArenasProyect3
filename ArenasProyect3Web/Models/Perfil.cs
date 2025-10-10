using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Perfil
    {
        public int IdPerfil { get; set; }
        public string? Perfil1 { get; set; }
        public string? Descripcion { get; set; }
        public string? Area { get; set; }
        public int? Estado { get; set; }
        public string? Alias { get; set; }
    }
}

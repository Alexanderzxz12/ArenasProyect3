using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class EstadoSistema
    {
        public int IdEstadoSistema { get; set; }
        public string? Descripcion { get; set; }
        public string? EstadoSistema1 { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string? Maquina { get; set; }
        public string? UsuarioDispositivo { get; set; }
        public string? UsuarioSistema { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class SistemaMensajerium
    {
        public int IdSistemaNotificaciones { get; set; }
        public int? IdUsuarioGenera { get; set; }
        public int? IdAreaAlcance { get; set; }
        public string? TituloMensaje { get; set; }
        public string? AsuntoMensaje { get; set; }
        public string? MensajeMen { get; set; }
        public DateTime? FechaGeneracion { get; set; }
        public string? ImagenAdjunta { get; set; }
        public string? ArchivoAdjunto { get; set; }
        public int? Estado { get; set; }
    }
}

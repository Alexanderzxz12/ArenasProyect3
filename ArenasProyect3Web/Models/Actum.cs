using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Actum
    {
        public int IdActa { get; set; }
        public int? IdClienteDetalleLiquidacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaTermino { get; set; }
        public int? CkActual { get; set; }
        public int? CkFuturoPotencial { get; set; }
        public int? CkAlto { get; set; }
        public int? CkMedia { get; set; }
        public int? CkBaja { get; set; }
        public string? Asistente1 { get; set; }
        public string? Asistente2 { get; set; }
        public string? Asistente3 { get; set; }
        public int? IdCliente { get; set; }
        public string? ContactoCliente1 { get; set; }
        public string? CorreoCliente1 { get; set; }
        public string? CargoCliente1 { get; set; }
        public string? TelefonoCliente1 { get; set; }
        public string? ContactoCliente2 { get; set; }
        public string? CorreoCliente2 { get; set; }
        public string? CargoCliente2 { get; set; }
        public string? TelefonoCliente2 { get; set; }
        public string? ContactoCliente3 { get; set; }
        public string? CorreoCliente3 { get; set; }
        public string? CargoCliente3 { get; set; }
        public string? TelefonoCliente3 { get; set; }
        public int? IdUnidad { get; set; }
        public int? CkSosteniemintoConclusion { get; set; }
        public int? CkCapacitacion { get; set; }
        public int? CkRecuperacion { get; set; }
        public int? CkReclamo { get; set; }
        public int? Estado { get; set; }
        public bool? Validado { get; set; }
        public int? EstadoActa { get; set; }
        public DateTime? FechaActa { get; set; }
        public int? PresenciaAsistente { get; set; }
        public int? IdResponsable { get; set; }
        public int? IdJefatura { get; set; }
    }
}

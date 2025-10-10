using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class AuditoraGeneral
    {
        public int IdAuditoraGeneral { get; set; }
        public int? IdUsuario { get; set; }
        public string? Mantenimiento { get; set; }
        public int? IdAccion { get; set; }
        public string? Descripcion { get; set; }
        public string? Maquina { get; set; }
        public DateTime? FechaAccion { get; set; }
        public string? NombreUsuarioSesion { get; set; }
        public int? IdGeneral { get; set; }
        public int? IdProceso { get; set; }
        public int? Estado { get; set; }
    }
}

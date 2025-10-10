using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Auditora
    {
        public int IdAuditora { get; set; }
        public int? IdUsuario { get; set; }
        public string? Mantenimiento { get; set; }
        public string? Accion { get; set; }
        public string? Descripcion { get; set; }
        public string? Maquina { get; set; }
        public DateTime? FechaAccion { get; set; }
        public string? NombreUsuarioSesion { get; set; }
        public int? Estado { get; set; }
        public int? CodigoRequerimeintoAsociado { get; set; }
        public int? CodigoLiquidacionAsociado { get; set; }
        public int? CodigoActaAsociado { get; set; }
        public int? CodigoLineaTrabajoAsociado { get; set; }
    }
}

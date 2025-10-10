using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class RequerimientoSimple
    {
        public int IdRequerimientoSimple { get; set; }
        public string? CodigoRequerimientoSimple { get; set; }
        public DateTime? FechaRequerida { get; set; }
        public DateTime? FechaSolicitada { get; set; }
        public string? DesJefatura { get; set; }
        public int? IdSolicitante { get; set; }
        public int? IdCentroCostos { get; set; }
        public string? Obervaciones { get; set; }
        public int? IdSede { get; set; }
        public int? IdLocal { get; set; }
        public int? IdArea { get; set; }
        public int? IdTipo { get; set; }
        public int? EstadoLogistica { get; set; }
        public int? Estado { get; set; }
        public string? MensajeAnulacion { get; set; }
        public int? IdJefatura { get; set; }
        public string? AliasCargoJefatura { get; set; }
        public int? CantidadItems { get; set; }
        public int? IdPrioridad { get; set; }
        public bool? EstadoAtendido { get; set; }
        public bool? EstadoOc { get; set; }
        public int? IdOp { get; set; }
        public int? IdOt { get; set; }
    }
}

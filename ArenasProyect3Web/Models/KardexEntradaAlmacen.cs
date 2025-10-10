using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class KardexEntradaAlmacen
    {
        public int IdEntradaAlmacen { get; set; }
        public string? CodigoEntradaAlmacen { get; set; }
        public DateTime? FechaEntrada { get; set; }
        public int? IdTipoEntrada { get; set; }
        public string? NumeroOrden { get; set; }
        public DateTime? FechaOrden { get; set; }
        public int? IdTipoDoc { get; set; }
        public string? NumeroDoc { get; set; }
        public string? NumeroGuia { get; set; }
        public DateTime? FechaGuia { get; set; }
        public int? IdTipoMovimiento { get; set; }
        public int? IdTipoAlmacen { get; set; }
        public int? IdTipoMon { get; set; }
        public int? IdProveedor { get; set; }
        public string? Observaciones { get; set; }
        public string? PdfdocuemntoAdjunto { get; set; }
        public int? Estado { get; set; }
        public int? EstadoEntrada { get; set; }
    }
}

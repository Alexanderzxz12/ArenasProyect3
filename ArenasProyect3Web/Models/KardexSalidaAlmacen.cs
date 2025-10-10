using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class KardexSalidaAlmacen
    {
        public int IdSalidaAlmacen { get; set; }
        public string? CodigoSalidaAlmacen { get; set; }
        public DateTime? FechaSalida { get; set; }
        public int? IdTipoSalida { get; set; }
        public string? NumeroOrden { get; set; }
        public DateTime? FechaOrden { get; set; }
        public string? NumeroRequerimiento { get; set; }
        public DateTime? FechaRequerimiento { get; set; }
        public int? IdTipoMovimiento { get; set; }
        public int? IdTipoAlmacen { get; set; }
        public int? IdUsuario { get; set; }
        public string? Observaciones { get; set; }
        public string? CentroCostos { get; set; }
        public int? Estado { get; set; }
        public int? EstadoNs { get; set; }
    }
}

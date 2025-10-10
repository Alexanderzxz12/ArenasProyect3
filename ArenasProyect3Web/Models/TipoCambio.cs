using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class TipoCambio
    {
        public int IdTipoCambio { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public string? TipoCompra { get; set; }
        public string? TipoVenta { get; set; }
        public string? Maquina { get; set; }
        public int? IdUsuario { get; set; }
    }
}

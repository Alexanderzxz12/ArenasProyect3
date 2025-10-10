using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class TipoMovimientosEntradaSalidaAlmacen
    {
        public int IdTipoMovimientoEntradaAlmacen { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
        public string? EntradaSalida { get; set; }
    }
}

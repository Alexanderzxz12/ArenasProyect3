using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class TipoMoneda
    {
        public TipoMoneda()
        {
            Clientes = new HashSet<Cliente>();
            Cotizacions = new HashSet<Cotizacion>();
            LiquidacionVenta = new HashSet<LiquidacionVentum>();
            RequerimientoVenta = new HashSet<RequerimientoVentum>();
        }

        public int IdTipoMonedas { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
        public string? Abreviatura { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
        public virtual ICollection<Cotizacion> Cotizacions { get; set; }
        public virtual ICollection<LiquidacionVentum> LiquidacionVenta { get; set; }
        public virtual ICollection<RequerimientoVentum> RequerimientoVenta { get; set; }
    }
}

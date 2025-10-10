using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Vehiculo
    {
        public Vehiculo()
        {
            LiquidacionVenta = new HashSet<LiquidacionVentum>();
            RequerimientoVenta = new HashSet<RequerimientoVentum>();
        }

        public int IdVehiculo { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }
        public int? EstadoRequerimiento { get; set; }
        public int? EstadoLiquidacion { get; set; }

        public virtual ICollection<LiquidacionVentum> LiquidacionVenta { get; set; }
        public virtual ICollection<RequerimientoVentum> RequerimientoVenta { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class RequerimientoVentum
    {
        public RequerimientoVentum()
        {
            DetalleRequerimientoVenta = new HashSet<DetalleRequerimientoVentum>();
            LiquidacionVenta = new HashSet<LiquidacionVentum>();
        }

        public int IdRequerimientoVenta { get; set; }
        public DateTime? FechaRequerimiento { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaTermino { get; set; }
        public int? Nacional { get; set; }
        public int? Extranjero { get; set; }
        public string? MotivoVisita { get; set; }
        public int? IdVendedor { get; set; }
        public int? IdVehiculo { get; set; }
        public string? ItinerarioViaje { get; set; }
        public int? EstadoJefatura { get; set; }
        public int? EstadoContabilidad { get; set; }
        public string? Total { get; set; }
        public int? Estado { get; set; }
        public bool? EstadoLiquidacion { get; set; }
        public int? EstadoAtrasado { get; set; }
        public string? MensajeAtrasado { get; set; }
        public int? IdTipoMoneda { get; set; }
        public int? IdJefatura { get; set; }
        public string? AliasCargoComercial { get; set; }
        public string? AliasCargoJefatura { get; set; }
        public int? EstadoLiquidacionFueraFecha { get; set; }
        public int? EstadoHabilitadoJefatura { get; set; }
        public string? MensajeFueraFecha { get; set; }
        public string? MensajeAnulado { get; set; }

        public virtual Usuario? IdJefaturaNavigation { get; set; }
        public virtual TipoMoneda? IdTipoMonedaNavigation { get; set; }
        public virtual Vehiculo? IdVehiculoNavigation { get; set; }
        public virtual Usuario? IdVendedorNavigation { get; set; }
        public virtual ICollection<DetalleRequerimientoVentum> DetalleRequerimientoVenta { get; set; }
        public virtual ICollection<LiquidacionVentum> LiquidacionVenta { get; set; }
    }
}

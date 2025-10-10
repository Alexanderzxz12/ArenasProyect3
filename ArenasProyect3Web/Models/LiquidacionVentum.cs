using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class LiquidacionVentum
    {
        public int IdLiquidacion { get; set; }
        public DateTime? FechaLiquidacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaTermino { get; set; }
        public int? Nacional { get; set; }
        public int? Extranjero { get; set; }
        public string? MotivoVisita { get; set; }
        public int? IdVendedor { get; set; }
        public int? IdVehiculo { get; set; }
        public string? ItinerarioViaje { get; set; }
        public string? Total { get; set; }
        public string? Adelanto { get; set; }
        public string? Saldo { get; set; }
        public int? EstadoContabilidad { get; set; }
        public bool? EstadoActas { get; set; }
        public int? Estado { get; set; }
        public int? IdRequerimeinto { get; set; }
        public int? IdTipoMoneda { get; set; }
        public int? IdJefatura { get; set; }
        public int? EstadoComercial { get; set; }

        public virtual Usuario? IdJefaturaNavigation { get; set; }
        public virtual RequerimientoVentum? IdRequerimeintoNavigation { get; set; }
        public virtual TipoMoneda? IdTipoMonedaNavigation { get; set; }
        public virtual Vehiculo? IdVehiculoNavigation { get; set; }
        public virtual Usuario? IdVendedorNavigation { get; set; }
    }
}

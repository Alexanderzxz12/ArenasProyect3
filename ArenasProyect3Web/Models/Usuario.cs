using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Cotizacions = new HashSet<Cotizacion>();
            DatosAnexosClienteUnidads = new HashSet<DatosAnexosClienteUnidad>();
            LiquidacionVentumIdJefaturaNavigations = new HashSet<LiquidacionVentum>();
            LiquidacionVentumIdVendedorNavigations = new HashSet<LiquidacionVentum>();
            RequerimientoVentumIdJefaturaNavigations = new HashSet<RequerimientoVentum>();
            RequerimientoVentumIdVendedorNavigations = new HashSet<RequerimientoVentum>();
        }

        public int IdUsuarios { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public byte[]? Icono { get; set; }
        public string? NombreIcono { get; set; }
        public string? Area { get; set; }
        public string? Estado { get; set; }
        public int? Rol { get; set; }
        public int? HabilitadoRequerimientoVenta { get; set; }
        public string? Documento { get; set; }
        public string? RutaFirma { get; set; }
        public int? IdArea { get; set; }
        public string? PrimerNombre { get; set; }
        public string? SegundoNombre { get; set; }
        public string? ApellidoParterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public int? VisibleUsuario { get; set; }
        public int? HabilitadoCotizacion { get; set; }

        public virtual ICollection<Cotizacion> Cotizacions { get; set; }
        public virtual ICollection<DatosAnexosClienteUnidad> DatosAnexosClienteUnidads { get; set; }
        public virtual ICollection<LiquidacionVentum> LiquidacionVentumIdJefaturaNavigations { get; set; }
        public virtual ICollection<LiquidacionVentum> LiquidacionVentumIdVendedorNavigations { get; set; }
        public virtual ICollection<RequerimientoVentum> RequerimientoVentumIdJefaturaNavigations { get; set; }
        public virtual ICollection<RequerimientoVentum> RequerimientoVentumIdVendedorNavigations { get; set; }
    }
}

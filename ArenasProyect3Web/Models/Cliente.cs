using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Cotizacions = new HashSet<Cotizacion>();
            DatosAnexosClienteCindicions = new HashSet<DatosAnexosClienteCindicion>();
            DatosAnexosClienteContactos = new HashSet<DatosAnexosClienteContacto>();
            DatosAnexosClienteSucursals = new HashSet<DatosAnexosClienteSucursal>();
        }

        public int IdCliente { get; set; }
        public string? Codigo { get; set; }
        public int? IdTipoCliente { get; set; }
        public string? NombreCliente { get; set; }
        public string? PrimerNombre { get; set; }
        public string? SegundoNombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public int? TelefonoCelular { get; set; }
        public string? TelefonoFijo { get; set; }
        public string? Correo1 { get; set; }
        public string? Correo2 { get; set; }
        public int? IdGrupo { get; set; }
        public int? IdTipoMoneda { get; set; }
        public int? IdRetencion { get; set; }
        public int? IdTipoDocumento { get; set; }
        public string? Dni { get; set; }
        public string? Ruc { get; set; }
        public string? OtroDocumento { get; set; }
        public string? Direccion { get; set; }
        public string? Referencia { get; set; }
        public string? CodigoPais { get; set; }
        public string? CodigoDepartamento { get; set; }
        public string? CodigoProvincia { get; set; }
        public string? CodigoDistrito { get; set; }
        public decimal? Lsoles { get; set; }
        public decimal? Ldolares { get; set; }
        public string? Ubigeo { get; set; }
        public int? Estado { get; set; }

        public virtual TipoGrupo? IdGrupoNavigation { get; set; }
        public virtual TipoRetencion? IdRetencionNavigation { get; set; }
        public virtual TipoCliente? IdTipoClienteNavigation { get; set; }
        public virtual TipoDocumento? IdTipoDocumentoNavigation { get; set; }
        public virtual TipoMoneda? IdTipoMonedaNavigation { get; set; }
        public virtual ICollection<Cotizacion> Cotizacions { get; set; }
        public virtual ICollection<DatosAnexosClienteCindicion> DatosAnexosClienteCindicions { get; set; }
        public virtual ICollection<DatosAnexosClienteContacto> DatosAnexosClienteContactos { get; set; }
        public virtual ICollection<DatosAnexosClienteSucursal> DatosAnexosClienteSucursals { get; set; }
    }
}

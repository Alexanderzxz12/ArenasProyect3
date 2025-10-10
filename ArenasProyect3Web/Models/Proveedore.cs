using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Proveedore
    {
        public int IdProveedor { get; set; }
        public string? Codigo { get; set; }
        public int? IdTipoCliente { get; set; }
        public string? NombreProveedor { get; set; }
        public string? PrimerNombre { get; set; }
        public string? SegundoNombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public int? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? PaginaWeb { get; set; }
        public string? Direccion { get; set; }
        public string? CodigoPais { get; set; }
        public string? CodigoDepartamento { get; set; }
        public string? CodigoProvincia { get; set; }
        public string? CodigoDistrito { get; set; }
        public int? IdProcedencia { get; set; }
        public int? IdTipoDocumento { get; set; }
        public string? Ruc { get; set; }
        public int? Detraccion { get; set; }
        public int? Declarante { get; set; }
        public int? Percepcion { get; set; }
        public int? Retencion { get; set; }
        public decimal? Lsoles { get; set; }
        public decimal? Ldolares { get; set; }
        public int? Estado { get; set; }
        public string? Dni { get; set; }
        public string? Otros { get; set; }
    }
}

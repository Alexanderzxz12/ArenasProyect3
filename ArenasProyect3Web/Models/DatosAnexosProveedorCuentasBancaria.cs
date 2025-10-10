using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DatosAnexosProveedorCuentasBancaria
    {
        public int IdDatosAnexosProveedorCuentaBancaria { get; set; }
        public int? IdProveedor { get; set; }
        public string? TipoBanco { get; set; }
        public int? IdBanco { get; set; }
        public int? IdMoneda { get; set; }
        public string? Direccion { get; set; }
        public string? NumeroCuenta { get; set; }
        public string? Cci { get; set; }
        public int? Estado { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class LineaTrabajo
    {
        public int IdLineaTrabajo { get; set; }
        public int? IdLinea { get; set; }
        public int? IdEquipoArea { get; set; }
        public int? IdTipoCuenta { get; set; }
        public int? IdActa { get; set; }
        public int? IdCliente { get; set; }
        public int? IdUnidad { get; set; }
        public string? AntecedentesDescripcion { get; set; }
        public string? DesarrolloDescripcion { get; set; }
        public string? ResultadoDescripcion { get; set; }
        public string? AccionesDescripcion { get; set; }
        public DateTime? FechaAcciones { get; set; }
        public int? IdResponsable { get; set; }
        public decimal? GastoLinea { get; set; }
        public int? Estado { get; set; }
        public string? Imagen1 { get; set; }
        public string? Imagen2 { get; set; }
        public string? Imagen3 { get; set; }
        public int? HabilitadoHistorial { get; set; }
    }
}

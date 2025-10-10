using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class SalidaNoConforme
    {
        public int IdSnc { get; set; }
        public int? IdDetalleCantidadCalidad { get; set; }
        public int? IdUsuarioResponsable { get; set; }
        public DateTime? FechaHallazgo { get; set; }
        public int? IdOp { get; set; }
        public string? DescripcionSnc { get; set; }
        public string? DescripcionAccionesTomadas { get; set; }
        public int? IdUsuarioAutorizacion { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Finaliza { get; set; }
        public string? Imagen1 { get; set; }
        public string? Imagen2 { get; set; }
        public string? Imagen3 { get; set; }
        public int? CkLiberacion { get; set; }
        public int? CkCorrecion { get; set; }
        public int? CkReproceso { get; set; }
        public int? CkReclasificacion { get; set; }
        public int? SkRecuperacion { get; set; }
        public int? SkDestruccion { get; set; }
        public int? SkOtros { get; set; }
        public string? DescripcionOtros { get; set; }
        public int? Estado { get; set; }
        public DateTime? FechaRegistroPro { get; set; }
        public string? CausaConformidad { get; set; }
        public string? OportunidadMejora { get; set; }
    }
}

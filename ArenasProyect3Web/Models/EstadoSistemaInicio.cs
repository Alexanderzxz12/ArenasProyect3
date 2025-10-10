using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class EstadoSistemaInicio
    {
        public int IdEstadoSistemaInicio { get; set; }
        public string? Descripcion { get; set; }
        public string? VersionSsitema { get; set; }
        public string? FechaInstalacionSsitema { get; set; }
        public string? NuevasFuncionesNovedades { get; set; }
        public DateTime? FechaAparicion { get; set; }
        public int? Estado { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class TablaLicencia
    {
        public int IdLicencia { get; set; }
        public string? Titulo { get; set; }
        public string? Maquina { get; set; }
        public string? Placa { get; set; }
        public string? Usuario { get; set; }
        public string? PersonalAsignado { get; set; }
        public string? Anotaciones { get; set; }
        public int? Estado { get; set; }
        public string? NumeroIdentificador { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}

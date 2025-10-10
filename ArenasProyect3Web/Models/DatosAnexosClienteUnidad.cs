using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DatosAnexosClienteUnidad
    {
        public DatosAnexosClienteUnidad()
        {
            Cotizacions = new HashSet<Cotizacion>();
            DatosAnexosClienteContactos = new HashSet<DatosAnexosClienteContacto>();
        }

        public int IdDatosAnexosClienteUnidad { get; set; }
        public int? IdCliente { get; set; }
        public string? Descripcion { get; set; }
        public int? IdResponsable { get; set; }
        public int? IdZona { get; set; }
        public string? CodigoPais { get; set; }
        public string? CodigoDepartamento { get; set; }
        public decimal? Longitud { get; set; }
        public decimal? Latitud { get; set; }
        public int? Estado { get; set; }

        public virtual Usuario? IdResponsableNavigation { get; set; }
        public virtual Zona? IdZonaNavigation { get; set; }
        public virtual ICollection<Cotizacion> Cotizacions { get; set; }
        public virtual ICollection<DatosAnexosClienteContacto> DatosAnexosClienteContactos { get; set; }
    }
}

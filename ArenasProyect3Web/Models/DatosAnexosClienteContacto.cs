using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DatosAnexosClienteContacto
    {
        public DatosAnexosClienteContacto()
        {
            Cotizacions = new HashSet<Cotizacion>();
        }

        public int IdDatosAnexosClienteContacto { get; set; }
        public int? IdCliente { get; set; }
        public string? Descripcion { get; set; }
        public string? Telefono { get; set; }
        public string? Anexo { get; set; }
        public string? Correo { get; set; }
        public int? IdUnidadCliente { get; set; }
        public int? IdArea { get; set; }
        public int? IdCargo { get; set; }
        public int? Estado { get; set; }

        public virtual Area? IdAreaNavigation { get; set; }
        public virtual Cargo? IdCargoNavigation { get; set; }
        public virtual Cliente? IdClienteNavigation { get; set; }
        public virtual DatosAnexosClienteUnidad? IdUnidadClienteNavigation { get; set; }
        public virtual ICollection<Cotizacion> Cotizacions { get; set; }
    }
}

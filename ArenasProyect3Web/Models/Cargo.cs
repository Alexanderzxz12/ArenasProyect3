using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Cargo
    {
        public Cargo()
        {
            DatosAnexosClienteContactos = new HashSet<DatosAnexosClienteContacto>();
        }

        public int IdCargo { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<DatosAnexosClienteContacto> DatosAnexosClienteContactos { get; set; }
    }
}

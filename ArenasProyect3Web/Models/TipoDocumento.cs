using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class TipoDocumento
    {
        public TipoDocumento()
        {
            Clientes = new HashSet<Cliente>();
        }

        public int IdTipoDocumento { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}

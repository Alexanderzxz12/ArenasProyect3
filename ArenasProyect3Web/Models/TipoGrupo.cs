using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class TipoGrupo
    {
        public TipoGrupo()
        {
            Clientes = new HashSet<Cliente>();
        }

        public int IdTipoGrupo { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}

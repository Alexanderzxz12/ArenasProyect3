using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Diferencial
    {
        public Diferencial()
        {
            Productos = new HashSet<Producto>();
        }

        public int IdDiferencial { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Medidum
    {
        public Medidum()
        {
            Productos = new HashSet<Producto>();
        }

        public string IdMedida { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }
    }
}

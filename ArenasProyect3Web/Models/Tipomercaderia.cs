using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Tipomercaderia
    {
        public Tipomercaderia()
        {
            Lineas = new HashSet<Linea>();
            Productos = new HashSet<Producto>();
        }

        public int IdTipoMercaderias { get; set; }
        public string? Desciripcion { get; set; }
        public int? Estado { get; set; }
        public string? Abreviatura { get; set; }
        public string? CodSunet { get; set; }

        public virtual ICollection<Linea> Lineas { get; set; }
        public virtual ICollection<Producto> Productos { get; set; }
    }
}

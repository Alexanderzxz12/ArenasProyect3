using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DatosAnexosOrigen
    {
        public DatosAnexosOrigen()
        {
            DatosAnexosProductoImportacions = new HashSet<DatosAnexosProductoImportacion>();
        }

        public int IdOrigen { get; set; }
        public string? CodigoOrigen { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<DatosAnexosProductoImportacion> DatosAnexosProductoImportacions { get; set; }
    }
}

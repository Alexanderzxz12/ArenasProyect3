using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DatosAnexosTerminosCompra
    {
        public DatosAnexosTerminosCompra()
        {
            DatosAnexosProductoImportacions = new HashSet<DatosAnexosProductoImportacion>();
        }

        public int IdTerminosCompra { get; set; }
        public string? CodigoTerminosCompra { get; set; }
        public string? Abreviatura { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<DatosAnexosProductoImportacion> DatosAnexosProductoImportacions { get; set; }
    }
}

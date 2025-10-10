using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DatosAnexosBienesSujetoPercepcion
    {
        public DatosAnexosBienesSujetoPercepcion()
        {
            DatosAnexosProductoSunats = new HashSet<DatosAnexosProductoSunat>();
        }

        public int IdBienesSujetoPercepcion { get; set; }
        public string? CodigoBienesSujetoPercepcion { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<DatosAnexosProductoSunat> DatosAnexosProductoSunats { get; set; }
    }
}

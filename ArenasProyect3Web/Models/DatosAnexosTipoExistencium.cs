using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DatosAnexosTipoExistencium
    {
        public DatosAnexosTipoExistencium()
        {
            DatosAnexosProductoSunats = new HashSet<DatosAnexosProductoSunat>();
        }

        public int IdTipoExistencia { get; set; }
        public string? CodigoTipoExistencia { get; set; }
        public string? Descripcion { get; set; }
        public int? Estado { get; set; }

        public virtual ICollection<DatosAnexosProductoSunat> DatosAnexosProductoSunats { get; set; }
    }
}

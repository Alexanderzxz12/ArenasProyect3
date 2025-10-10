using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DatosAnexosProductoSunat
    {
        public int IdSunat { get; set; }
        public int? IdArt { get; set; }
        public int? IdTipoExistencia { get; set; }
        public int? IdBienesSujetoPercepcion { get; set; }
        public string? CodigoUnspcs { get; set; }
        public int? SujetoPercepcion { get; set; }
        public decimal? PorcentajePercepcion { get; set; }
        public int? SujetoDetraccion { get; set; }
        public decimal? PorcentajeDetraccion { get; set; }
        public int? SujetoIsc { get; set; }
        public decimal? PorcentajeIsc { get; set; }
        public int? Estado { get; set; }

        public virtual Producto? IdArtNavigation { get; set; }
        public virtual DatosAnexosBienesSujetoPercepcion? IdBienesSujetoPercepcionNavigation { get; set; }
        public virtual DatosAnexosTipoExistencium? IdTipoExistenciaNavigation { get; set; }
    }
}

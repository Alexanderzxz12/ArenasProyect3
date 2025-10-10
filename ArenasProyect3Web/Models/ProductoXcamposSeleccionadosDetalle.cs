using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class ProductoXcamposSeleccionadosDetalle
    {
        public int IdPoductoXcamposSeleccionadoDetalle { get; set; }
        public int? IdArt { get; set; }
        public int? IdTipoCaracteristicas1 { get; set; }
        public int? IdDescripcionCaracteristicas1 { get; set; }
        public int? IdTipoCaracteristicas2 { get; set; }
        public int? IdDescripcionCaracteristicas2 { get; set; }
        public int? IdTipoCaracteristicas3 { get; set; }
        public int? IdDescripcionCaracteristicas3 { get; set; }
        public int? IdTipoCaracteristicas4 { get; set; }
        public int? IdDescripcionCaracteristicas4 { get; set; }
        public int? IdTipoMedidas1 { get; set; }
        public int? IdDescripcionMedidas1 { get; set; }
        public int? IdTipoMedidas2 { get; set; }
        public int? IdDescripcionMedidas2 { get; set; }
        public int? IdTipoMedidas3 { get; set; }
        public int? IdDescripcionMedidas3 { get; set; }
        public int? IdTipoMedidas4 { get; set; }
        public int? IdDescripcionMedidas4 { get; set; }
        public int? IdTipoDiametros1 { get; set; }
        public int? IdDescripcionDiametros1 { get; set; }
        public int? IdTipoDiametros2 { get; set; }
        public int? IdDescripcionDiametros2 { get; set; }
        public int? IdTipoDiametros3 { get; set; }
        public int? IdDescripcionDiametros3 { get; set; }
        public int? IdTipoDiametros4 { get; set; }
        public int? IdDescripcionDiametros4 { get; set; }
        public int? IdTipoFormas1 { get; set; }
        public int? IdDescripcionFormas1 { get; set; }
        public int? IdTipoFormas2 { get; set; }
        public int? IdDescripcionFormas2 { get; set; }
        public int? IdTipoFormas3 { get; set; }
        public int? IdDescripcionFormas3 { get; set; }
        public int? IdTipoFormas4 { get; set; }
        public int? IdDescripcionFormas4 { get; set; }
        public int? IdTipoEspesores1 { get; set; }
        public int? IdDescripcionEspesores1 { get; set; }
        public int? IdTipoEspesores2 { get; set; }
        public int? IdDescripcionEspesores2 { get; set; }
        public int? IdTipoEspesores3 { get; set; }
        public int? IdDescripcionEspesores3 { get; set; }
        public int? IdTipoEspesores4 { get; set; }
        public int? IdDescripcionEspesores4 { get; set; }
        public int? IdTipoDiseñoAcabado1 { get; set; }
        public int? IdDescripcionDiseñoAcabado1 { get; set; }
        public int? IdTipoDiseñoAcabado2 { get; set; }
        public int? IdDescripcionDiseñoAcabado2 { get; set; }
        public int? IdTipoDiseñoAcabado3 { get; set; }
        public int? IdDescripcionDiseñoAcabado3 { get; set; }
        public int? IdTipoDiseñoAcabado4 { get; set; }
        public int? IdDescripcionDiseñoAcabado4 { get; set; }
        public int? IdTipoNtipos1 { get; set; }
        public int? IdDescripcionNtipos1 { get; set; }
        public int? IdTipoNtipos2 { get; set; }
        public int? IdDescripcionNtipos2 { get; set; }
        public int? IdTipoNtipos3 { get; set; }
        public int? IdDescripcionNtipos3 { get; set; }
        public int? IdTipoNtipos4 { get; set; }
        public int? IdDescripcionNtipos4 { get; set; }
        public int? IdTipoVarios01 { get; set; }
        public int? IdDescripcionVarios01 { get; set; }
        public int? IdTipoVarios02 { get; set; }
        public int? IdDescripcionVarios02 { get; set; }
        public int? Estado { get; set; }
        public string? CampoGeneral { get; set; }

        public virtual Producto? IdArtNavigation { get; set; }
    }
}

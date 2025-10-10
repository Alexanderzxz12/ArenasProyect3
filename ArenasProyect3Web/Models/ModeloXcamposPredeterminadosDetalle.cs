using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class ModeloXcamposPredeterminadosDetalle
    {
        public int IdModeloXcamposPredeterminadosDetalle { get; set; }
        public int? IdModelo { get; set; }
        public int? IdTipoCaracteristicas1 { get; set; }
        public int? IdTipoCaracteristicas2 { get; set; }
        public int? IdTipoCaracteristicas3 { get; set; }
        public int? IdTipoCaracteristicas4 { get; set; }
        public int? IdTipoMedidas1 { get; set; }
        public int? IdTipoMedidas2 { get; set; }
        public int? IdTipoMedidas3 { get; set; }
        public int? IdTipoMedidas4 { get; set; }
        public int? IdTipoDiametros1 { get; set; }
        public int? IdTipoDiametros2 { get; set; }
        public int? IdTipoDiametros3 { get; set; }
        public int? IdTipoDiametros4 { get; set; }
        public int? IdTipoFormas1 { get; set; }
        public int? IdTipoFormas2 { get; set; }
        public int? IdTipoFormas3 { get; set; }
        public int? IdTipoFormas4 { get; set; }
        public int? IdTipoEspesores1 { get; set; }
        public int? IdTipoEspesores2 { get; set; }
        public int? IdTipoEspesores3 { get; set; }
        public int? IdTipoEspesores4 { get; set; }
        public int? IdTipoDiseñoAcabado1 { get; set; }
        public int? IdTipoDiseñoAcabado2 { get; set; }
        public int? IdTipoDiseñoAcabado3 { get; set; }
        public int? IdTipoDiseñoAcabado4 { get; set; }
        public int? IdTipoNtipos1 { get; set; }
        public int? IdTipoNtipos2 { get; set; }
        public int? IdTipoNtipos3 { get; set; }
        public int? IdTipoNtipos4 { get; set; }
        public int? IdTipoVarios01 { get; set; }
        public int? IdTipoVarios02 { get; set; }
        public int? Estado { get; set; }
        public string? CampoGeneral { get; set; }

        public virtual Modelo? IdModeloNavigation { get; set; }
    }
}

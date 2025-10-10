using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class ModeloXcamposPredeterminado
    {
        public int IdModeloXcamposPredeterminado { get; set; }
        public int? IdModelo { get; set; }
        public int? CampCaracteristicas1 { get; set; }
        public int? CampCaracteristicas2 { get; set; }
        public int? CampMedidas1 { get; set; }
        public int? CampMedidas2 { get; set; }
        public int? CampDiametros1 { get; set; }
        public int? CampDiametros2 { get; set; }
        public int? CampFormas1 { get; set; }
        public int? CampFormas2 { get; set; }
        public int? CampEspesores1 { get; set; }
        public int? CampEspesores2 { get; set; }
        public int? CampDiseñoAcabado1 { get; set; }
        public int? CampDiseñoAcabado2 { get; set; }
        public int? CampNtipos1 { get; set; }
        public int? CampNtipos2 { get; set; }
        public int? CampVarios01 { get; set; }
        public int? CampVarios02 { get; set; }
        public int? CampGeneral { get; set; }
        public int? Estado { get; set; }

        public virtual Modelo? IdModeloNavigation { get; set; }
    }
}

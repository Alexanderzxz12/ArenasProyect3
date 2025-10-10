using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Modelo
    {
        public Modelo()
        {
            DescripcionCaracteristicas = new HashSet<DescripcionCaracteristica>();
            DescripcionDiametros = new HashSet<DescripcionDiametro>();
            DescripcionDiseñoAcabados = new HashSet<DescripcionDiseñoAcabado>();
            DescripcionEspesores = new HashSet<DescripcionEspesore>();
            DescripcionFormas = new HashSet<DescripcionForma>();
            DescripcionMedida = new HashSet<DescripcionMedida>();
            DescripcionNtipos = new HashSet<DescripcionNtipo>();
            DescripcionVarios0s = new HashSet<DescripcionVarios0>();
            ModeloXcamposPredeterminados = new HashSet<ModeloXcamposPredeterminado>();
            ModeloXcamposPredeterminadosDetalles = new HashSet<ModeloXcamposPredeterminadosDetalle>();
            Productos = new HashSet<Producto>();
        }

        public int IdModelo { get; set; }
        public int? IdLinea { get; set; }
        public string? Descripcion { get; set; }
        public int? HabilitarCreacion { get; set; }
        public int? Estado { get; set; }
        public string? Abreviatura { get; set; }
        public int? EstadoAtributos { get; set; }

        public virtual Linea? IdLineaNavigation { get; set; }
        public virtual ICollection<DescripcionCaracteristica> DescripcionCaracteristicas { get; set; }
        public virtual ICollection<DescripcionDiametro> DescripcionDiametros { get; set; }
        public virtual ICollection<DescripcionDiseñoAcabado> DescripcionDiseñoAcabados { get; set; }
        public virtual ICollection<DescripcionEspesore> DescripcionEspesores { get; set; }
        public virtual ICollection<DescripcionForma> DescripcionFormas { get; set; }
        public virtual ICollection<DescripcionMedida> DescripcionMedida { get; set; }
        public virtual ICollection<DescripcionNtipo> DescripcionNtipos { get; set; }
        public virtual ICollection<DescripcionVarios0> DescripcionVarios0s { get; set; }
        public virtual ICollection<ModeloXcamposPredeterminado> ModeloXcamposPredeterminados { get; set; }
        public virtual ICollection<ModeloXcamposPredeterminadosDetalle> ModeloXcamposPredeterminadosDetalles { get; set; }
        public virtual ICollection<Producto> Productos { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Linea
    {
        public Linea()
        {
            LineaXoperacionXmaquinaria = new HashSet<LineaXoperacionXmaquinarium>();
            LineaXoperacions = new HashSet<LineaXoperacion>();
            Modelos = new HashSet<Modelo>();
            Productos = new HashSet<Producto>();
        }

        public int IdLinea { get; set; }
        public int? IdTipMer { get; set; }
        public string? Descripcion { get; set; }
        public int? HabilitarOperacion { get; set; }
        public int? HabilitarBusqueda { get; set; }
        public int? HabilitarProductosTerminados { get; set; }
        public int? HabilitarProductosGenerales { get; set; }
        public int? Estado { get; set; }
        public string? Abreviatura { get; set; }
        public int? HabilitarLineaTrabajo { get; set; }

        public virtual Tipomercaderia? IdTipMerNavigation { get; set; }
        public virtual ICollection<LineaXoperacionXmaquinarium> LineaXoperacionXmaquinaria { get; set; }
        public virtual ICollection<LineaXoperacion> LineaXoperacions { get; set; }
        public virtual ICollection<Modelo> Modelos { get; set; }
        public virtual ICollection<Producto> Productos { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class DatosAnexosClienteCindicion
    {
        public int IdDatosAnexosClienteCondicion { get; set; }
        public int? IdCliente { get; set; }
        public int? IdCondicionPago { get; set; }
        public int? IdFormaPago { get; set; }
        public int? Estado { get; set; }

        public virtual Cliente? IdClienteNavigation { get; set; }
        public virtual CondicionPago? IdCondicionPagoNavigation { get; set; }
        public virtual FormaPago? IdFormaPagoNavigation { get; set; }
    }
}

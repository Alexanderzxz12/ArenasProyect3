using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class Transferencium
    {
        public int IdTransferencia { get; set; }
        public string? Descripcion { get; set; }
        public int? IdBonificacion { get; set; }
        public int? Estado { get; set; }
    }
}

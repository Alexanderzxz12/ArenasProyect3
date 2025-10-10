using System;
using System.Collections.Generic;

namespace ArenasProyect3Web.Models
{
    public partial class OrdenServicio
    {
        public int IdOrdenServicio { get; set; }
        public string? CodigoOrdenServicio { get; set; }
        public DateTime? FechaInicial { get; set; }
        public DateTime? FechaEmtrega { get; set; }
        public int? IdArt { get; set; }
        public string? CodigoProducto { get; set; }
        public string? DescripcionProducto { get; set; }
        public string? PlanoProducto { get; set; }
        public string? Color { get; set; }
        public string? CodigoBss { get; set; }
        public int? IdGeneraUsuario { get; set; }
        public string? UsuarioGenera { get; set; }
        public int? IdSede { get; set; }
        public int? IdPrioridad { get; set; }
        public int? IdLocal { get; set; }
        public int? IdOperacion { get; set; }
        public string? Obserbaciones { get; set; }
        public int? IdOp { get; set; }
        public int? Cantidad { get; set; }
        public int? EstadoOs { get; set; }
        public int? Estado { get; set; }
        public int? IdCliente { get; set; }
    }
}
